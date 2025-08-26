using Tecnomatix.Engineering;
using TxCommand1.Forms;

namespace TxCommand1
{
    public class TxHelloWorldCmd : TxButtonCommand
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
