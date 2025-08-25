using System;
using System.Diagnostics;
using System.Text;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;
using Tecnomatix.Engineering.Ui;

namespace TxCommand1
{
    public partial class TxOperationForm : TxForm
    {
        private ITxPathPlanningRCSService _rcsService = new TxPathPlanningRCSService();
        
        public TxOperationForm()
        {
            InitializeComponent();
        }

        public override void OnInitTxForm()
        {
            base.OnInitTxForm();

            TxObjectList selectedObejcts = TxApplication.ActiveSelection.GetItems();
            // this will have to be filtered by path operations
            
            if (selectedObejcts.Count > 0)
            {
                ITxObject selectedObject = selectedObejcts[0];
                if (selectedObject is ITxOperation operation)
                {
                    _operationPicker.Object = operation;
                    _demo();
                }
            }

            _operationPicker.Focus();
        }

        private void _operationPicker_Picked(object sender, TxObjEditBoxCtrl_PickedEventArgs args)
        {
            _demo();
        }

        private void _operationPicker_TypeValid(object sender, EventArgs e)
        {
            _demo();
        }

        private void _operationPicker_TypeInvalid(object sender, EventArgs e)
        {
            _demo();
        }

        private void _demo()
        {
            
            TxObjectList<TxRoboticViaLocationOperation> leafOperations = _getLeafOperations(_operationPicker.Object);
            foreach (var op in leafOperations)
            {
                Debug.WriteLine($"Name: {op.Name}, Duration: {op.Duration}");
            }
            _runSimpleSimulation();
            foreach (var op in leafOperations)
            {
                Debug.WriteLine($"Name: {op.Name}, Duration: {op.Duration}");
            }
        }

        private void _runSimpleSimulation()
        {
            ITxOperation op = _operationPicker.Object as ITxOperation;
            TxSimulationPlayer simPlayer = TxApplication.ActiveDocument.SimulationPlayer;

            var oldOp = TxApplication.ActiveDocument.CurrentOperation;
            
            TxApplication.ActiveDocument.CurrentOperation = op;
            simPlayer.Rewind();
            simPlayer.PlaySilently();
            simPlayer.ResetToDefaultSetting();
            
            TxApplication.ActiveDocument.CurrentOperation = oldOp;
        }
        
        private TxObjectList<TxRoboticViaLocationOperation> _getLeafOperations(ITxObject operation)
        {
            var result = new TxObjectList<TxRoboticViaLocationOperation>();
            if (operation is ITxObjectCollection col)
            {
                TxObjectList descendants = col.GetAllDescendants(new TxTypeFilter(
                    includedTypes: new [] { typeof(TxRoboticViaLocationOperation) },
                    excludedTypes: new [] { typeof(ITxObjectCollection) }));
                
                foreach (TxRoboticViaLocationOperation leafOp in descendants) result.Add(leafOp);
            }
            else if (operation is TxRoboticViaLocationOperation leafOp)
            {
                result.Add(leafOp);
            }
            
            return result;
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
