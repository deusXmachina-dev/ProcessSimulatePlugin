using System;
using System.Diagnostics;
using Tecnomatix.Engineering;

namespace TxCommand1.Operations
{
    /// <summary>
    /// Represents a motion sequence with calculated properties for energy optimization.
    /// </summary>
    public class OptimizableMotion
    {
        /// <summary>
        /// Gets the original sequence of robotic via location operations.
        /// </summary>
        private TxObjectList<ITxRoboticLocationOperation> ViaLocations { get; set; }

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
        /// Gets the heuristic score for energy optimization.
        /// Calculated as: TotalDistance + 2 * TotalVerticalDistance
        /// </summary>
        public double HeuristicScore => TotalDistance + 2 * TotalVerticalDistance;

        /// <summary>
        /// Initializes a new instance of the OptimizableMotion class and calculates distance metrics.
        /// </summary>
        /// <param name="viaLocations">The sequence of robotic via location operations.</param>
        public OptimizableMotion(TxObjectList<ITxRoboticLocationOperation> viaLocations)
        {
            ViaLocations = viaLocations ?? throw new ArgumentNullException(nameof(viaLocations));
            ModifyVelocity(100); // Default to 100% velocity
            CalculateDistances();
        }

        /// <summary>
        /// Calculates the total distance and vertical distance for this motion.
        /// </summary>
        private void CalculateDistances()
        {
            double totalDistance = 0.0;
            double totalVerticalDistance = 0.0;
            
            for (int i = 1; i < ViaLocations.Count; i++)
            {
                ITxLocatableObject previousLoc = ViaLocations[i - 1] as ITxLocatableObject;
                ITxLocatableObject currentLoc = ViaLocations[i] as ITxLocatableObject;

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

            for (int i = 1; i < ViaLocations.Count; i++)
            {
                var op = ViaLocations[i];
                Debug.WriteLine($"Setting speed for {op.Name} to {targetSpeed}");
                if (op.GetParameter("RRS_JOINT_SPEED") is TxRoboticDoubleParam speedParam)
                {
                    speedParam.Value = targetSpeed;
                    op.SetParameter(speedParam);
                }
            }
            
            Velocity = targetSpeed;
        }
    }
}
