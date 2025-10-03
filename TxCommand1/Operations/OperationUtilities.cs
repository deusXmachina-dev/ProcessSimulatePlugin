using System;
using System.Diagnostics;
using System.Linq;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace TxCommand1.Operations
{
    /// <summary>
    /// Utility helpers for inspecting and modifying Tecnomatix operations.
    /// </summary>
    public class OperationUtilities
    {
        private readonly ITxPathPlanningRCSService _rcsService = new TxPathPlanningRCSService();

        private TxObjectList<TxRoboticViaLocationOperation> GetLeafOperations(ITxObject operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var result = new TxObjectList<TxRoboticViaLocationOperation>();
            if (operation is ITxObjectCollection collection)
            {
                TxObjectList descendants = collection.GetAllDescendants(new TxTypeFilter(
                    includedTypes: new[] { typeof(TxRoboticViaLocationOperation) },
                    excludedTypes: new[] { typeof(ITxObjectCollection) }));

                if (descendants != null)
                {
                    foreach (var o in descendants)
                    {
                        var leafOperation = (TxRoboticViaLocationOperation)o;
                        if (leafOperation != null)
                        {
                            result.Add(leafOperation);
                        }
                    }
                }
            }
            else if (operation is TxRoboticViaLocationOperation leafOperation)
            {
                result.Add(leafOperation);
            }
            return result;
        }

        /// <summary>
        /// Runs the simulation for the given operation and returns per-leaf durations.
        /// </summary>
        public OperationResultCollection RunSimulationAndGetDurations(ITxOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            if (TxApplication.ActiveDocument == null)
                throw new InvalidOperationException("No active document available for simulation.");

            var results = new OperationResultCollection();

            try
            {
                var originalCurrentOperation = TxApplication.ActiveDocument.CurrentOperation;

                try
                {
                    var simPlayer = TxApplication.ActiveDocument.SimulationPlayer;
                    if (simPlayer == null)
                        throw new InvalidOperationException("Simulation player is not available.");

                    simPlayer.Rewind();
                    TxApplication.ActiveDocument.CurrentOperation = operation;
                    simPlayer.PlaySilently();
                    simPlayer.Rewind();

                    var leafOperations = GetLeafOperations(operation);
                    foreach (var leafOp in leafOperations)
                    {
                        if (leafOp != null && !string.IsNullOrEmpty(leafOp.Name))
                        {
                            results.Add(leafOp.Name, leafOp.Duration);
                        }
                    }
                }
                finally
                {
                    TxApplication.ActiveDocument.CurrentOperation = originalCurrentOperation;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to run simulation for operation '{operation.Name}': {ex.Message}", ex);
            }

            return results;
        }

        /// <summary>
        /// Sets the joint speed parameter for all joint motions within the operation tree.
        /// </summary>
        public void ModifyOperationSpeed(ITxOperation operation, int speed)
        {
            TxObjectList<TxRoboticViaLocationOperation> leafOperations = GetLeafOperations(operation);

            var jointMotionOperations = leafOperations
                .Where(o => _rcsService.GetLocationMotionType(o) == TxMotionType.Joint);

            foreach (var op in jointMotionOperations)
            {
                Debug.WriteLine($"Setting speed for {op.Name} to {speed}");
                if (op.GetParameter("RRS_JOINT_SPEED") is TxRoboticDoubleParam speedParam)
                {
                    speedParam.Value = speed;
                    op.SetParameter(speedParam);
                }
            }
        }

        /// <summary>
        /// Gets all movements from the operation. Each movement is a sequence of joint operations.
        /// </summary>
        /// <param name="operation">The operation to extract movements from.</param>
        /// <returns>A list of movements, where each movement is a list of robotic via operations.</returns>
        public TxObjectList GetMovements(ITxOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var movements = new TxObjectList();
            
            // TODO: Implement logic to group joint operations into movements
            // This should analyze the operation structure and group consecutive joint operations
            // that form logical movements together
            // Each item in the movements list should be a TxObjectList<TxRoboticViaLocationOperation>
            
            return movements;
        }
    }
}


