using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace TxCommand1
{
    public partial class TxRobotTCPForm : TxForm
    {
        public TxRobotTCPForm()
        {
            InitializeComponent();
        }

        public override void OnInitTxForm()
        {
            base.OnInitTxForm();

            TxObjectList selectedObejcts = TxApplication.ActiveSelection.GetItems();
            if (selectedObejcts.Count > 0)
            {
                ITxObject selectedObject = selectedObejcts[0];
                if (selectedObject is TxRobot robot)
                {
                    _robotPicker.Object = robot;
                    _updateUI();
                }
            }

            _robotPicker.Focus();
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
            TxRobot robot = _robotPicker.Object as TxRobot;
            if (robot != null)
            {
                TxFrame tcpFrame = robot.TCPF;
                TxTransformation location = tcpFrame.AbsoluteLocation;
                TxVector translation = location.Translation;
                TxVector rotation = location.RotationRPY_XYZ;

                _txtTranslation.Text = translation.ToString();
                _txtRotation.Text = rotation.ToString();
            }
            else
            {
                _txtTranslation.Text = string.Empty;
                _txtRotation.Text = string.Empty;
            }
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
