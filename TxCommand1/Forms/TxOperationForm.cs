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
		
		

		private void _btnOptimize_Click(object sender, EventArgs e)
		{
			if (_operationPicker.Object is ITxOperation operation)
			{
				var optimizer = new OperationOptimization(new OperationUtilities());
				var optimized = optimizer.OptimizePathEnergy(operation, _durationInput.Value);
				if (optimized == null)
				{
					// No variant found under limit
				}
			}
		}
		
		private void _btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}

