using Tecnomatix.Engineering;

namespace DeusXMachinaCommand.Operations
{
    /// <summary>
    /// Interface for energy optimization routines for operations.
    /// </summary>
    public interface IOperationEnergyOptimization
    {
        /// <summary>
        /// Optimizes the operation to meet the duration limit.
        /// The returned operation is a duplicated, renamed variant; caller owns its lifecycle.
        /// </summary>
        EnergyOptimizationResult Optimize(ITxOperation operation, double limitDuration);
    }
}


