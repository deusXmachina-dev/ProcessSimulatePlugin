using Tecnomatix.Engineering;

namespace DeusXMachinaCommand.Operations
{
    /// <summary>
    /// Dummy implementation of energy optimization that tries joint-speed variants.
    /// </summary>
    public class DummyEnergyOptimizer : IOperationEnergyOptimization
    {
        private readonly OperationUtilities _utilities;

        public DummyEnergyOptimizer(OperationUtilities utilities)
        {
            _utilities = utilities;
        }

        /// <summary>
        /// Tries joint-speed variants from 5..100 by 5 and returns the first under the limit.
        /// The returned operation is a duplicated, renamed variant; caller owns its lifecycle.
        /// </summary>
        public ITxOperation Optimize(ITxOperation operation, double limitDuration)
        {
            if (operation == null)
                return null;

            for (int speed = 5; speed <= 100; speed += 5)
            {
                ITxOperation newOp = OperationDuplicator.DuplicateOperation(operation);
                newOp.Name = $"Temp copy of {operation.Name} at speed {speed}";
                _utilities.ModifyOperationSpeed(newOp, speed);

                var results = _utilities.RunSimulationAndGetDurations(newOp);
                if (newOp.Duration < limitDuration)
                {
                    newOp.Name = $"En. optimal ({limitDuration:F2} s) {operation.Name}";
                    return newOp;
                }
                newOp.Delete();
            }
            return null;
        }
    }
}


