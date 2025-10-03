using System;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;
using TxCommand1.Operations;

namespace TxCommand1.Forms
{
	public partial class TxOperationForm : TxForm
	{
		
		public TxOperationForm()
		{
			InitializeComponent();
		}
		
		private void _btnOptimize_Click(object sender, EventArgs e)
		{
			if (_operationPicker.Object is ITxOperation operation)
			{
				var optimizer = new DummyEnergyOptimizer(new OperationUtilities());
				var optimized = optimizer.Optimize(operation, _durationInput.Value);
				if (optimized == null)
				{
					MessageBox.Show($@"No optimization found within the duration limit of {_durationInput.Value}.", @"Optimization Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}
		
		private void _btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}

