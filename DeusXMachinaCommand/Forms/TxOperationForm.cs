using System;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;
using DeusXMachinaCommand.Operations;

namespace DeusXMachinaCommand.Forms
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
				var optimizer = new HeuristicEnergyOptimizer(new OperationUtilities());
				var result = optimizer.Optimize(operation, _durationInput.Value);
				if (result == null)
				{
					MessageBox.Show($@"No optimization found within the duration limit of {_durationInput.Value}.", @"Optimization Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					var optimized = result.Operation;
					MessageBox.Show($@"Optimized operation: '{optimized?.Name}'\nDuration: {optimized?.Duration:F3}s\nEstimated energy savings: {result.EnergySavingsPercent:F1}%",
						@"Optimization Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}
		
		private void _btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}

