using System;
using System.Diagnostics;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace TxCommand1
{
    public partial class TxOperationForm : TxForm
    {
        public TxOperationForm()
        {
            InitializeComponent();
        }

        public override void OnInitTxForm()
        {
            base.OnInitTxForm();

            TxObjectList selectedObjects = TxApplication.ActiveSelection.GetItems();
            // this will have to be filtered by path operations
            
            if (selectedObjects.Count > 0)
            {
                ITxObject selectedObject = selectedObjects[0];
                if (selectedObject is ITxOperation operation)
                {
                    _operationPicker.Object = operation;
                    Demo();
                }
            }

            _operationPicker.Focus();
        }

        private void _operationPicker_Picked(object sender, TxObjEditBoxCtrl_PickedEventArgs args)
        {
            Demo();
        }

        private void _operationPicker_TypeValid(object sender, EventArgs e)
        {
            Demo();
        }

        private void _operationPicker_TypeInvalid(object sender, EventArgs e)
        {
            Demo();
        }

        private void Demo()
        {
            try
            {
                // Use the new method to run simulation and get durations
                var operationResults = RunSimulationAndGetDurations();
                
                // Output the results using the collection's ToString method
                Debug.WriteLine("=== Simulation Results ===");
                Debug.WriteLine(operationResults.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during simulation: {ex.Message}");
            }
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
                    
                    // Set the operation as current and run a simulation
                    TxApplication.ActiveDocument.CurrentOperation = operation;

                    var simPlayer = TxApplication.ActiveDocument.SimulationPlayer;
                    if (simPlayer == null)
                        throw new InvalidOperationException("Simulation player is not available.");

                    // Run the simulation
                    simPlayer.Rewind();
                    simPlayer.PlaySilently();
                    simPlayer.ResetToDefaultSetting();

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

        /// <summary>
        /// Runs a simulation for the currently selected operation in the operation picker and returns
        /// a collection of operation results containing the durations of all leaf operations.
        /// </summary>
        /// <returns>An OperationResultCollection containing the name and duration of each leaf operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no operation is selected or simulation fails.</exception>
        private OperationResultCollection RunSimulationAndGetDurations()
        {
            var operation = _operationPicker.Object as ITxOperation;
            if (operation == null)
                throw new InvalidOperationException("No valid operation selected in the operation picker.");

            return RunSimulationAndGetDurations(operation);
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void _programPicker_Picked(object sender, TxObjEditBoxCtrl_PickedEventArgs args)
        {
            if (_programPicker.Object is ITxOperation operation) OperationDuplicator.DuplicateOperation(operation);
        }
    }
}
