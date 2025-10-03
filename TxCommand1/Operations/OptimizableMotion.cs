using System;
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
        public TxObjectList<TxRoboticViaLocationOperation> ViaLocations { get; private set; }

        /// <summary>
        /// Gets the total distance traveled during the motion.
        /// </summary>
        public double TotalDistance { get; private set; }

        /// <summary>
        /// Gets the total vertical distance traveled during the motion.
        /// </summary>
        public double TotalVerticalDistance { get; private set; }

        /// <summary>
        /// Gets the heuristic score for energy optimization.
        /// Calculated as: TotalDistance + 2 * TotalVerticalDistance
        /// </summary>
        public double HeuristicScore => TotalDistance + 2 * TotalVerticalDistance;

        /// <summary>
        /// Initializes a new instance of the OptimizableMotion class and calculates distance metrics.
        /// </summary>
        /// <param name="viaLocations">The sequence of robotic via location operations.</param>
        public OptimizableMotion(TxObjectList<TxRoboticViaLocationOperation> viaLocations)
        {
            ViaLocations = viaLocations ?? throw new ArgumentNullException(nameof(viaLocations));
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
                var previousLoc = ViaLocations[i - 1];
                var currentLoc = ViaLocations[i];
                
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
    }
}
