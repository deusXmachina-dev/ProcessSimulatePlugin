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
            
            ITxOperation newOp = OperationDuplicator.DuplicateOperation(operation);
            newOp.Name = $"Temp copy of {operation.Name} for heuristic optimization";

            List<TxObjectList<TxRoboticViaLocationOperation>> rawMotions = _utilities.GetMotions(newOp);

            List<OptimizableMotion> optimizableMotions = _utilities.CreateOptimizableMotions(rawMotions);
            List<OptimizableMotion> sortedMotions = optimizableMotions.OrderByDescending(
                m => m.HeuristicScore).ToList();
            
            foreach (var motion in sortedMotions) motion.ModifyVelocity(100);
            
            // todo: maybe run simulation to get the current duration of the motion
            var currentSimResults = _utilities.RunSimulationAndGetDurations(newOp);
            if (currentSimResults.GetTotalDuration() >= limitDuration)
            {
                newOp.Delete();
                return null;
            }
            

            // todo: 4. start heuristic optimization:
            // todo: 4.1. for movement in sorted list:
            foreach (var motion in sortedMotions)
            {   
                // todo: 4.1.1. reduce speed to something around 60% if this does not exceed the limit duration
                int targetSpeed = (int)NextGaussian(mean: 60.0, stdDev: 2.5, clampRange: 5);
                motion.ModifyVelocity(targetSpeed);
                // todo: 4.1.2. if it does exceed the limit duration, reduce speed as much as possible (perhaps in 10% increments) while staying within the limit duration and then break
                currentSimResults = _utilities.RunSimulationAndGetDurations(newOp);
                if (currentSimResults.GetTotalDuration() >= limitDuration)
                {
                    
                }

            }
            // todo: 4.2 for movement in sorted list:
            // todo: 4.2.1. reduce speed to something around 45% if this does not exceed the limit duration
            // todo: 4.2.2. if it does exceed the limit duration, reduce speed as much as possible while staying within the limit duration and then break
            
            
            if (true)  // todo: review what this condition is supposed to be
            {
                newOp.Name = $"En. optimal ({limitDuration:F2} s) {operation.Name}";
                return newOp;
            }
            newOp.Delete();
            return null;
        }
    }
}

