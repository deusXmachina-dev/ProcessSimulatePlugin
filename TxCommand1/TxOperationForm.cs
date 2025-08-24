using System;
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

            TxObjectList selectedObejcts = TxApplication.ActiveSelection.GetItems();
            // this will have to be filtered by path operations
            
            if (selectedObejcts.Count > 0)
            {
                ITxObject selectedObject = selectedObejcts[0];
                if (selectedObject is TxRobot robot)
                {
                    _operationPicker.Object = robot;
                    _updateUI();
                }
            }

            _operationPicker.Focus();
        }

        private void _robotPicker_Picked(object sender, TxObjEditBoxCtrl_PickedEventArgs args)
        {
            _updateUI();
        }

        private void _robotPicker_TypeValid(object sender, EventArgs e)
        {
            _updateUI();
        }

        private void _robotPicker_TypeInvalid(object sender, EventArgs e)
        {
            _updateUI();
        }

        private void _updateUI()
        {
            // we'll need operation picker instead of robot picker
            if (_operationPicker.Object is ITxOperation operation)
            {
                // log operation name
                _txtOperationName.Text = operation.Name;
            }
            else
            {
                _txtOperationName.Text = string.Empty;
            }
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
