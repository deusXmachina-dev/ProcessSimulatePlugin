using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tecnomatix.Engineering;

namespace TxCommand1
{
    public class TxHelloWorldCmd : TxButtonCommand
    {
        public override string Category => StringTable.CATEGORY;

        public override string Name => StringTable.NAME;

        public override void Execute(object cmdParams)
        {
            TxRobotTCPForm robotForm = new TxRobotTCPForm();
            robotForm.Show();
        }
    }
}   
