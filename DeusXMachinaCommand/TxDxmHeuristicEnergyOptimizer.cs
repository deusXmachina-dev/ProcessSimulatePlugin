using Tecnomatix.Engineering;
using DeusXMachinaCommand.Forms;

namespace DeusXMachinaCommand
{
    public class TxDxmHeuristicEnergyOptimizer : TxButtonCommand
    {
        public override string Category => StringTable.CATEGORY;

        public override string Name => StringTable.NAME;

        public override void Execute(object cmdParams)
        {
            TxOperationForm robotForm = new TxOperationForm();
            robotForm.Show();
        }
    }
}   
