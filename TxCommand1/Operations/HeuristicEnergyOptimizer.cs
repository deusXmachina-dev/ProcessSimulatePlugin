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

        public HeuristicEnergyOptimizer(OperationUtilities utilities)
        {
            _utilities = utilities;
        }

        /// <summary>
        /// Optimizes the operation using heuristic approaches.
        /// </summary>
        public ITxOperation Optimize(ITxOperation operation, double limitDuration)
        {

            List<TxObjectList<TxRoboticViaLocationOperation>> rawMotions = _utilities.GetMotions(operation);

            List<OptimizableMotion> optimizableMotions = _utilities.CreateOptimizableMotions(rawMotions);
            List<OptimizableMotion> sortedMotions = optimizableMotions.OrderByDescending(
                m => m.HeuristicScore).ToList();

            // todo: 4. start heuristic optimization:
            // todo: 4.1. for movement in sorted list:
            // todo: 4.1.1. reduce speed to something around 60% if this does not exceed the limit duration
            // todo: 4.1.2. if it does exceed the limit duration, reduce speed as much as possible while staying within the limit duration and then break

            // todo: 4.2 for movement in sorted list:
            // todo: 4.2.1. reduce speed to something around 45% if this does not exceed the limit duration
            // todo: 4.2.2. if it does exceed the limit duration, reduce speed as much as possible while staying within the limit duration and then break
            return null;
        }
    }
}

