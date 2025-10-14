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

        public override string Bitmap
        {
            get
            {
                return "logo_16.bmp";
            }   
        }

        public override string MediumBitmap
        {
            get
            {
                return "logo_24.bmp";
            }
        }

        public override string LargeBitmap
        {
            get
            {
                return "logo_32.png";
            }
        }
    }
}   
