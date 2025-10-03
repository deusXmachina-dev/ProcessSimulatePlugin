using System;
using System.Collections.Generic;
using System.Linq;
using Tecnomatix.Engineering;

namespace TxCommand1.Operations
{
    /// <summary>
    /// Heuristic-based implementation of energy optimization.
    /// </summary>
    public class HeuristicEnergyOptimizer : IOperationEnergyOptimization
    {
        private readonly OperationUtilities _utilities;
        private readonly Random _random;

        // Constants for optimization parameters
        private const double DefaultStdDev = 2.5;
        private const double DefaultClampRange = 5.0;
        private const double VelocityIncrementStep = 5.0;
        private const double FirstPassTargetVelocity = 60.0;
        private const double SecondPassTargetVelocity = 45.0;

        public HeuristicEnergyOptimizer(OperationUtilities utilities)
        {
            _utilities = utilities;
            _random = new Random();
        }

        /// <summary>
        /// Generates a random number with a normal distribution around a mean value.
        /// </summary>
        /// <param name="mean">The mean (center) of the distribution.</param>
        /// <param name="stdDev">The standard deviation (spread) of the distribution.</param>
        /// <param name="clampRange">The range to clamp the result to.</param>
        /// <returns>A random number following a normal distribution.</returns>
        private double NextGaussian(double mean, double stdDev, double clampRange)
        {
            // Box-Muller transform to generate normally distributed random numbers
            double u1 = 1.0 - _random.NextDouble();
            double u2 = 1.0 - _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double result = mean + stdDev * randStdNormal;
            
            // Clamp to range [mean - clampRange, mean + clampRange]
            double min = mean - clampRange;
            double max = mean + clampRange;
            return Math.Max(min, Math.Min(max, result));
        }

        /// <summary>
        /// Optimizes the operation using heuristic approaches.
        /// </summary>
        public ITxOperation Optimize(ITxOperation operation, double limitDuration)
        {
            ITxOperation optimizedOperation = PrepareOperationForOptimization(operation);
            
            var currentSimResults = _utilities.RunSimulationAndGetDurations(optimizedOperation);
            if (IsOverDurationLimit(currentSimResults, limitDuration))
            {
                optimizedOperation.Delete();
                return null;
            }

            List<OptimizableMotion> sortedMotions = GetSortedOptimizableMotions(optimizedOperation);
            
            // Two-pass optimization: first conservative (60%), then aggressive (45%)
            // to find the optimal balance between energy savings and time constraints
            OptimizeMotionsWithVelocityTarget(sortedMotions, optimizedOperation, 
                FirstPassTargetVelocity, limitDuration);
            OptimizeMotionsWithVelocityTarget(sortedMotions, optimizedOperation, 
                SecondPassTargetVelocity, limitDuration);
            
            optimizedOperation.Name = $"En. optimal ({limitDuration:F2} s) {operation.Name}";
            return optimizedOperation;
        }

        private ITxOperation PrepareOperationForOptimization(ITxOperation operation)
        {
            ITxOperation duplicatedOperation = OperationDuplicator.DuplicateOperation(operation);
            duplicatedOperation.Name = $"Temp copy of {operation.Name} for heuristic optimization";
            return duplicatedOperation;
        }

        private List<OptimizableMotion> GetSortedOptimizableMotions(ITxOperation operation)
        {
            List<TxObjectList<TxRoboticViaLocationOperation>> rawMotions = 
                _utilities.GetMotions(operation);
            List<OptimizableMotion> optimizableMotions = 
                _utilities.CreateOptimizableMotions(rawMotions);
            
            return optimizableMotions
                .OrderByDescending(m => m.HeuristicScore)
                .ToList();
        }

        private void OptimizeMotionsWithVelocityTarget(
            List<OptimizableMotion> sortedMotions, 
            ITxOperation operation,
            double targetMeanVelocity, 
            double limitDuration)
        {
            foreach (var motion in sortedMotions)
            {
                double previousVelocity = motion.Velocity;
                double targetVelocity = NextGaussian(
                    mean: targetMeanVelocity, 
                    stdDev: DefaultStdDev, 
                    clampRange: DefaultClampRange);
                
                motion.ModifyVelocity(targetVelocity);
                
                var currentSimResults = _utilities.RunSimulationAndGetDurations(operation);
                if (IsOverDurationLimit(currentSimResults, limitDuration))
                {
                    RollbackVelocityChange(motion, previousVelocity, targetVelocity);
                }
            }
        }

        private void RollbackVelocityChange(OptimizableMotion motion, 
            double previousVelocity, double targetVelocity)
        {
            while (targetVelocity < previousVelocity)
            {
                targetVelocity = Math.Min(targetVelocity + VelocityIncrementStep, previousVelocity);
                motion.ModifyVelocity(targetVelocity);
            }
        }

        private bool IsOverDurationLimit(OperationResultCollection results, double limitDuration)
        {
            return results.GetTotalDuration() >= limitDuration;
        }
    }
}

