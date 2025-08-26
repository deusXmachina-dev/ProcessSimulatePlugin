using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;
using Tecnomatix.Engineering.Ui;

namespace TxCommand1
{
    public partial class TxOperationForm : TxForm
    {
        private readonly ITxPathPlanningRCSService _rcsService = new TxPathPlanningRCSService();
        
        public TxOperationForm()
        {
            InitializeComponent();
        }

        private void _operationPicker_Picked(object sender, TxObjEditBoxCtrl_PickedEventArgs args)
        {
            SimulatePickedOperationAtDifferentSpeeds();
        }

        private void _operationPicker_TypeValid(object sender, EventArgs e)
        {
            SimulatePickedOperationAtDifferentSpeeds();
        }

        private void _operationPicker_TypeInvalid(object sender, EventArgs e)
        {
            SimulatePickedOperationAtDifferentSpeeds();
        }
        
        /// <summary>
        /// Gets all leaf robotic via location operations from the specified operation.
        /// If the operation is a collection, recursively searches for all descendant leaf operations.
        /// If the operation is already a leaf operation, returns it directly.
        /// </summary>
        /// <param name="operation">The operation to extract leaf operations from. Can be a single operation or a collection.</param>
        /// <returns>A list of all TxRoboticViaLocationOperation instances found as leaf operations.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the operation parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when there's an error accessing operation properties or descendants.</exception>
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
        /// Runs a simulation for the specified operation and returns a collection of operation results
        /// containing the durations of all leaf operations after simulation.
        /// </summary>
        /// <param name="operation">The operation to simulate. Can be a single operation or a collection of operations.</param>
        /// <returns>An OperationResultCollection containing the name and duration of each leaf operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the operation is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when simulation fails or no active document is available.</exception>
        private OperationResultCollection RunSimulationAndGetDurations(ITxOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            if (TxApplication.ActiveDocument == null)
                throw new InvalidOperationException("No active document available for simulation.");

            var results = new OperationResultCollection();
            
            try
            {
                // Store the original current operation to restore later
                var originalCurrentOperation = TxApplication.ActiveDocument.CurrentOperation;
                
                try
                {
                    var simPlayer = TxApplication.ActiveDocument.SimulationPlayer;
                    if (simPlayer == null)
                        throw new InvalidOperationException("Simulation player is not available.");


                    // Run the simulation
                    simPlayer.Rewind();
                    TxApplication.ActiveDocument.CurrentOperation = operation;
                    simPlayer.PlaySilently();
                    simPlayer.Rewind();

                    // Collect durations after simulation
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
                    // Always restore the original current operation
                    TxApplication.ActiveDocument.CurrentOperation = originalCurrentOperation;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to run simulation for operation '{operation.Name}': {ex.Message}", ex);
            }

            return results;
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void SimulatePickedOperationAtDifferentSpeeds()
        {
            if (_operationPicker.Object is ITxOperation operation)
            {
                var simulationResults = new System.Text.StringBuilder();
                simulationResults.AppendLine($"Simulation Results for: {operation.Name}");
                simulationResults.AppendLine(new string('=', 50));
                
                // for each speed from 5 to 100 at 5 steps - copy the op, modify the speed and run the sim
                for (int speed = 5; speed <= 100; speed += 5)
                {
                    ITxOperation newOp = OperationDuplicator.DuplicateOperation(operation);
                    newOp.Name = $"Temp copy of {operation.Name} at speed {speed}";
                    ModifyOperationSpeed(newOp, speed);

                    var results = RunSimulationAndGetDurations(newOp);
                    simulationResults.AppendLine($"Speed: {speed}% - Duration: {results.GetTotalDuration():F2} seconds");
                    newOp.Delete();
                }
                
                // Show results in a message box
                MessageBox.Show(simulationResults.ToString(), @"Simulation Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ModifyOperationSpeed(ITxOperation operation, int speed)
        {
            // 1. get all leaf operations
            TxObjectList<TxRoboticViaLocationOperation> leafOperations = GetLeafOperations(operation);

            // 2. filter only joint motion type operations
            var jointMotionOperations = leafOperations
                .Where(o => _rcsService.GetLocationMotionType(o) == TxMotionType.Joint);

            // 3. set the speed for each joint motion operation
            foreach (var op in jointMotionOperations)
            {
                // op.SetParameter();
                Debug.WriteLine($"Setting speed for {op.Name} to {speed}");
                if (op.GetParameter("RRS_JOINT_SPEED") is TxRoboticDoubleParam speedParam)
                {
                    speedParam.Value = speed;
                    op.SetParameter(speedParam);
                }
            }
        }
    }
}
