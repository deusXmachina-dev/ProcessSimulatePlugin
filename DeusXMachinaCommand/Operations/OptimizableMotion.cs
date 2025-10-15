using System;
using System.Diagnostics;
using Tecnomatix.Engineering;

namespace DeusXMachinaCommand.Operations
{
    /// <summary>
    /// Represents a motion sequence with calculated properties for energy optimization.
    /// </summary>
    public class OptimizableMotion
    {
        /// <summary>
        /// Sequence of robotic waypoint operations
        /// </summary>
        private TxObjectList<ITxRoboticLocationOperation> WaypointList { get; set; }
        
        /// <summary>
        /// Gets the total duration of the motion.
        /// </summary>
        private double DurationAtFullSpeed { get; set; }
        
        /// <summary>
        /// Expression of how energy demanding this motion is.
        /// Is computed using a ratio of durations at full and at 50% speed.
        /// Note that this is a rough estimate, which now ignores important factors like
        /// vertical motion.
        /// </summary>
        public double EnergyDemandScore { get; private set; }
        
        /// <summary>
        /// Gets the total distance traveled during the motion.
        /// </summary>
        private double TotalDistance { get; set; }

        /// <summary>
        /// Gets the total vertical distance traveled during the motion.
        /// </summary>
        private double TotalVerticalDistance { get; set; }

        /// <summary>
        /// Gets the current velocity (speed) setting of this motion.
        /// </summary>
        public double Velocity { get; private set; }

        /// <summary>
        /// Initializes a new instance of the OptimizableMotion class and calculates distance metrics.
        /// </summary>
        /// <param name="waypointListSpeed100">The sequence of robotic via location operations at full speed</param>
        /// <param name="waypointListSpeed50">The sequence of robotic via location operations at 50% speed</param>
        public OptimizableMotion(TxObjectList<ITxRoboticLocationOperation> waypointListSpeed100,
            TxObjectList<ITxRoboticLocationOperation> waypointListSpeed50)
        {
            WaypointList = waypointListSpeed100 ?? throw new ArgumentNullException(nameof(waypointListSpeed100));
            CalculateDistances();
            ComputeEnergyDemandScore(waypointListSpeed100, waypointListSpeed50);
        }

        /// <summary>
        /// Gets the total duration of the motion.
        /// </summary>
        /// <returns></returns>
        public double Duration()
        {
            return Duration(WaypointList);
        }

        /// <summary>
        /// Computes energy demand score for this motion.
        /// Uses a ratio of durations at full and at 50% speed.
        /// This is then projected beteween 0 and 1 using empirical formula which is based on
        /// observed minimal and maximal ratios.
        /// </summary>
        private void ComputeEnergyDemandScore(TxObjectList<ITxRoboticLocationOperation> waypointListSpeed100,
            TxObjectList<ITxRoboticLocationOperation> waypointListSpeed50)
        {
            // get total duration at full speed and at 50% speed
            double durationFullSpeed = Duration(waypointListSpeed100);
            double durationHalfSpeed = Duration(waypointListSpeed50);
            
            double speedRatio = durationFullSpeed / durationHalfSpeed;
            EnergyDemandScore = (EnergyOptimizationConstants.MaxSpeedRatio - speedRatio) / 
                                (EnergyOptimizationConstants.MaxSpeedRatio - EnergyOptimizationConstants.MinSpeedRatio);
            DurationAtFullSpeed = durationFullSpeed;
        }

        private double Duration(TxObjectList<ITxRoboticLocationOperation> waypointList)
        {
            double duration = 0;
            for (int i = 1; i < waypointList.Count; i++)
            {
                duration += waypointList[i].Duration;
            }
            return duration;
        }

        /// <summary>
        /// Calculates the total distance and vertical distance for this motion.
        /// </summary>
        private void CalculateDistances()
        {
            double totalDistance = 0.0;
            double totalVerticalDistance = 0.0;
            
            for (int i = 1; i < WaypointList.Count; i++)
            {
                ITxLocatableObject previousLoc = WaypointList[i - 1] as ITxLocatableObject;
                ITxLocatableObject currentLoc = WaypointList[i] as ITxLocatableObject;

                if (previousLoc == null || currentLoc == null)
                {
                    continue;
                }
                
                // Get the absolute positions for each via location
                TxVector previousPosition = previousLoc.AbsoluteLocation.Translation;
                TxVector currentPosition = currentLoc.AbsoluteLocation.Translation;
                
                // Calculate 3D distance between consecutive positions
                totalDistance += TxUtilities.Distance(previousPosition, currentPosition);
                
                // Calculate vertical (Z-axis) distance
                var verticalDistance = TxUtilities.VerticalDistance(previousPosition, currentPosition);
                if (verticalDistance > 0.0)
                {
                    // If this part of the motion is going up, add to vertical distance
                    totalVerticalDistance += verticalDistance;
                }
            }
            
            TotalDistance = totalDistance;
            TotalVerticalDistance = totalVerticalDistance;
        }

        /// <summary>
        /// Modifies the velocity (speed) of all via locations in this motion.
        /// </summary>
        /// <param name="targetSpeed">The target speed value to set.</param>
        public void ModifyVelocity(double targetSpeed)
        {

            for (int i = 1; i < WaypointList.Count; i++)
            {
                var op = WaypointList[i];
                Debug.WriteLine($"Setting speed for {op.Name} to {targetSpeed}");
                if (op.GetParameter("RRS_JOINT_SPEED") is TxRoboticDoubleParam speedParam)
                {
                    speedParam.Value = targetSpeed;
                    op.SetParameter(speedParam);
                }
            }
            
            Velocity = targetSpeed;
        }

        /// <summary>
        /// Estimates energy expenditure of the motion if it was performed at full speed.
        /// </summary>
        /// <returns>Energy expenditure in relative units (i.e.: it's unitless quantity) </returns>
        public double EstimateEnergyExpenditureAtFullSpeed()
        {
            // Clamp inputs to safe ranges
            double score    = Clamp01(EnergyDemandScore);
            double duration = Math.Max(0.0, DurationAtFullSpeed);

            // Interpolate power draw based on demand
            double powerDraw = Lerp(EnergyOptimizationConstants.MinEnergyConsumption, 
                                   EnergyOptimizationConstants.MaxEnergyConsumption, score);

            // Energy (power × time)
            return powerDraw * duration;
        }
        
        /// <summary>
        /// Estimates energy expenditure of the motion
        /// </summary>
        /// <returns>Energy expenditure in relative units (i.e.: it's unitless quantity) </returns>
        public double EstimateEnergyExpenditure()
        {
            double energyAtFull = EstimateEnergyExpenditureAtFullSpeed();

            // Clamp inputs to safe ranges
            double score    = Clamp01(EnergyDemandScore);         // 0..1
            double v        = Math.Max(0.0, Math.Min(100.0, Velocity)); // 0..100
            
            // Interpolate savings between min/max (fixes prior omission of MinSavings)
            double baseSavings = Lerp(EnergyOptimizationConstants.MinSavings, 
                                     EnergyOptimizationConstants.MaxSavings, score); // 0..1 expected

            // Velocity factor for savings:
            // <45  -> 1.0
            // 45-60 -> linearly 1.0 .. 0.8
            // 60-100 -> linearly 0.8 .. 0.0
            double velocityFactor =
                (v < 45.0)  ? 1.0 :
                (v < 60.0)  ? Lerp(1.0, 0.8, InverseLerp(45.0, 60.0, v)) :
                Lerp(0.8, 0.0, InverseLerp(60.0, 100.0, v));

            double savings = baseSavings * velocityFactor;

            // Final energy with savings applied
            return energyAtFull * (1.0 - savings);
        }
        
        // Local helpers
        private double Lerp(double a, double b, double t) => a + (b - a) * t;
        private double InverseLerp(double a, double b, double x) => (x - a) / (b - a);
        private double Clamp01(double t) => (t < 0.0) ? 0.0 : (t > 1.0) ? 1.0 : t;
    }
}
