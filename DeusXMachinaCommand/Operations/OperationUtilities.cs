using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace DeusXMachinaCommand.Operations
{
    /// <summary>
    /// Utility helpers for inspecting and modifying Tecnomatix operations.
    /// </summary>
    public class OperationUtilities
    {
        private readonly ITxPathPlanningRCSService _rcsService = new TxPathPlanningRCSService();

        private TxObjectList<ITxRoboticLocationOperation> GetLocationOperations(ITxObject operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var result = new TxObjectList<ITxRoboticLocationOperation>();
            if (operation is ITxObjectCollection collection)
            {
                TxObjectList descendants = collection.GetAllDescendants(new TxTypeFilter(
                    includedTypes: new[] { typeof(ITxRoboticLocationOperation) },
                    excludedTypes: new[] { typeof(ITxObjectCollection) }));

                if (descendants != null)
                {
                    foreach (var o in descendants)
                    {
                        var leafOperation = (ITxRoboticLocationOperation)o;
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
        public void RunSimulationAndGetDurations(ITxOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            if (TxApplication.ActiveDocument == null)
                throw new InvalidOperationException("No active document available for simulation.");

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
        }

        /// <summary>
        /// Sets the joint speed parameter for all joint motions within the operation tree.
        /// </summary>
        public void ModifyOperationSpeed(ITxOperation operation, int speed)
        {
            TxObjectList<ITxRoboticLocationOperation> leafOperations = GetLocationOperations(operation);

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
        /// Gets all motions from the operation. Each motion is a sequence of joint operations.
        /// </summary>
        /// <param name="operation">The operation to extract motions from.</param>
        /// <returns>A list of motions, where each motion is a list of robotic via operations.</returns>
        public List<TxObjectList<ITxRoboticLocationOperation>> GetMotions(ITxOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var motions = new List<TxObjectList<ITxRoboticLocationOperation>>();
            TxObjectList<ITxRoboticLocationOperation> locations = GetLocationOperations(operation);
            
            var currentMotion = new TxObjectList<ITxRoboticLocationOperation>();
            
            for (int i = 1; i < locations.Count; i++)
            {
                var previousLoc = locations[i - 1];
                var currentLoc = locations[i];

                // If the current location is part of a different operation, then we have a new motion
                if (!Equals(previousLoc.ParentRoboticOperation, currentLoc.ParentRoboticOperation))
                {
                    motions.Add(currentMotion);
                    currentMotion = new TxObjectList<ITxRoboticLocationOperation>();
                }
                
                bool isJointMovement = _rcsService.GetLocationMotionType(currentLoc) == TxMotionType.Joint;

                if (isJointMovement && currentLoc is TxRoboticViaLocationOperation)
                {
                    // if the current motion already has some operations, just add the current one
                    if (currentMotion.Count > 0)
                    {
                        currentMotion.Add(currentLoc);
                    }
                    // otherwise add both the previous and the current location
                    else
                    {
                        currentMotion.Add(previousLoc);
                        currentMotion.Add(currentLoc);
                    }
                }
                else // we only want to include linear movements and non-via operations as the start of the next motion
                {
                    if (currentMotion.Count > 0)
                    {
                        motions.Add(currentMotion);
                        currentMotion = new TxObjectList<ITxRoboticLocationOperation>();
                    }
                }
            }
            
            // Add any remaining movement
            if (currentMotion.Count > 0)
            {
                motions.Add(currentMotion);
            }
            
            return motions;
        }

        /// <summary>
        /// Creates optimizable motion representations for all motions in a list.
        /// </summary>
        /// <param name="motions">A list of motions, where each motion is a sequence of robotic via operations.</param>
        /// <param name="motionsAtHalfSpeed">A list of motions at half-speed, where each motion is a sequence of robotic via operations.</param>
        /// <returns>A list of optimizable motions corresponding to each motion.</returns>
        public List<OptimizableMotion> CreateOptimizableMotions(List<TxObjectList<ITxRoboticLocationOperation>> motions,
            List<TxObjectList<ITxRoboticLocationOperation>> motionsAtHalfSpeed)
        {
            if (motions == null)
                throw new ArgumentNullException(nameof(motions));
            if (motionsAtHalfSpeed == null)
                throw new ArgumentNullException(nameof(motionsAtHalfSpeed));

            var optimizableMotions = motions
                .Zip(motionsAtHalfSpeed, (fullSpeed, halfSpeed) => new OptimizableMotion(fullSpeed, halfSpeed))
                .ToList();

            return optimizableMotions;
        }
    }
}


