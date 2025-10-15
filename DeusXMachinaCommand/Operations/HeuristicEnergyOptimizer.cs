using System;
using System.Collections.Generic;
using System.Linq;
using Tecnomatix.Engineering;

namespace DeusXMachinaCommand.Operations
{
    /// <summary>
    /// Heuristic-based implementation of energy optimization.
    /// Tries to optimize operations starting from the "most promising" motions first,
    /// using a two-pass approach with different target velocities and a bit of randomness.
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
        public EnergyOptimizationResult Optimize(ITxOperation operation, double limitDuration)
        {
            ITxOperation optimizedOperation = PrepareOperationForOptimization(operation);
            _utilities.ModifyOperationSpeed(optimizedOperation, 100);
            
            _utilities.RunSimulationAndGetDurations(optimizedOperation);
            if (IsOverDurationLimit(optimizedOperation, limitDuration))
            {
                optimizedOperation.Delete();
                return null;
            }
            
            ITxOperation operationHalfSpeed = PrepareOperationForOptimization(operation);
            _utilities.ModifyOperationSpeed(operationHalfSpeed, 50);
            _utilities.RunSimulationAndGetDurations(operationHalfSpeed);

            List<OptimizableMotion> sortedMotions = GetSortedOptimizableMotions(optimizedOperation, operationHalfSpeed);
            operationHalfSpeed.Delete();

            // Two-pass optimization: first conservative (60%), then aggressive (45%)
            // to find the optimal balance between energy savings and time constraints
            OptimizeMotionsWithVelocityTarget(sortedMotions, optimizedOperation, 
                FirstPassTargetVelocity, limitDuration);
            OptimizeMotionsWithVelocityTarget(sortedMotions, optimizedOperation, 
                SecondPassTargetVelocity, limitDuration);


            double savingsPercent = EstimateEnergySavingsPercent(sortedMotions, optimizedOperation);

            optimizedOperation.Name = $"En. optimal ({limitDuration:F2} s) {operation.Name}";
            return new EnergyOptimizationResult(optimizedOperation, savingsPercent);
        }

        private double EstimateEnergySavingsPercent(
            List<OptimizableMotion> sortedMotions, 
            ITxOperation operation)
        {
            double technicalMotionsDuration = operation.Duration - sortedMotions.Sum(m => m.Duration());
            double technicalMotionsEnergy = technicalMotionsDuration *
                                            EnergyOptimizationConstants.MinEnergyConsumption;
            
            double optimizedEnergy = sortedMotions.Sum(m => m.EstimateEnergyExpenditure()) + technicalMotionsEnergy;
            double fullSpeedEnergy = sortedMotions.Sum(m => m.EstimateEnergyExpenditureAtFullSpeed()) + technicalMotionsEnergy;
            
            return 100 * (1 - optimizedEnergy / fullSpeedEnergy);
        }

        private ITxOperation PrepareOperationForOptimization(ITxOperation operation)
        {
            ITxOperation duplicatedOperation = OperationDuplicator.DuplicateOperation(operation);
            duplicatedOperation.Name = $"Temp copy of {operation.Name} for heuristic optimization";
            return duplicatedOperation;
        }

        private List<OptimizableMotion> GetSortedOptimizableMotions(ITxOperation operation, ITxOperation operationHalfSpeed)
        {
            List<TxObjectList<ITxRoboticLocationOperation>> rawMotions = 
                _utilities.GetMotions(operation);
            
            List<TxObjectList<ITxRoboticLocationOperation>> rawMotionsHalfSpeed = 
                _utilities.GetMotions(operationHalfSpeed);
            if (rawMotions.Count != rawMotionsHalfSpeed.Count)
                throw new InvalidOperationException("Invalid optimization state: Motions at 50% speed not found.");
            
            List<OptimizableMotion> optimizableMotions = 
                _utilities.CreateOptimizableMotions(rawMotions, rawMotionsHalfSpeed);
            
            return optimizableMotions
                .OrderByDescending(m => m.EnergyDemandScore) 
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
                
                _utilities.RunSimulationAndGetDurations(operation);
                while (IsOverDurationLimit(operation, limitDuration))
                {
                    {
                        targetVelocity = Math.Min(targetVelocity + VelocityIncrementStep, previousVelocity);
                        motion.ModifyVelocity(targetVelocity);
                        _utilities.RunSimulationAndGetDurations(operation);
                    }
                    if (IsOverDurationLimit(operation, limitDuration) && targetVelocity >= previousVelocity)
                    {
                        // This is not expected, raise some exception for invalid state
                        throw new InvalidOperationException("Invalid optimization state: Rollback of velocity modification failed.");
                    }
                }
            }
        }

        private bool IsOverDurationLimit(ITxOperation operation, double limitDuration)
        {
            return operation.Duration >= limitDuration;
        }
    }
}

