using System;
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
                    _updateUI();
                }
            }

            _operationPicker.Focus();
        }

        private void _operationPicker_Picked(object sender, TxObjEditBoxCtrl_PickedEventArgs args)
        {
            _updateUI();
        }

        private void _operationPicker_TypeValid(object sender, EventArgs e)
        {
            _updateUI();
        }

        private void _operationPicker_TypeInvalid(object sender, EventArgs e)
        {
            _updateUI();
        }

        private void _updateUI()
        {
            if (_operationPicker.Object is ITxRoboticOperation operation)
            {
                // todo: implement traversal of all the children of operation
                // after all the leaf operations are found, log tcpf and motion type for each

                StringBuilder sb = new StringBuilder();
                
                if (operation is ITxObjectCollection col)
                {
                    TxObjectList leafOperations = col.GetAllDescendants(new TxTypeFilter(
                        includedTypes: new [] { typeof(ITxRoboticOperation) },
                        excludedTypes: new [] { typeof(ITxObjectCollection) }));
                    foreach (ITxRoboticOperation leafOp in leafOperations)
                    {
                        TxMotionType motionType = _rcsService.GetLocationMotionType(leafOp);
                        
                        // , TCPF: {(tcpf != null ? tcpf.Name : "N/A")}
                        sb.AppendLine($"Leaf Operation: {leafOp.Name}, Motion Type: {motionType.ToString()}");
                    }
                }
                else
                {
                    TxMotionType motionType = _rcsService.GetLocationMotionType(operation);
                        
                    // , TCPF: {(tcpf != null ? tcpf.Name : "N/A")}
                    sb.AppendLine($"Leaf Operation: {operation.Name}, Motion Type: {motionType.ToString()}");
                }
                
                // log operation name
                _txtOperationName.Text = sb.ToString();
                // log operation type
                _txtOperationType.Text = operation.GetType().ToString();
            }
            else
            {
                _txtOperationName.Text = string.Empty;
                _txtOperationType.Text = string.Empty;
            }
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
