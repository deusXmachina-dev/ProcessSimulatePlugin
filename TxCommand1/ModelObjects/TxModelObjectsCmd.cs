using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tecnomatix.Engineering;

namespace TxCommand1
{
    public class TxModelObjectsCmd : TxButtonCommand
    {
        private const string MessageFormat = "Selected object type: {0}";

        public override string Category => StringTable.CATEGORY;

        public override string Name => StringTable.MODEL_OBJECTS_COMMAND_NAME;

        public override void Execute(object cmdParams)
        {
            TxObjectList selectedObjects = TxApplication.ActiveSelection.GetItems();
            if (selectedObjects.Count <= 0) return;
            ITxObject selectedObject = selectedObjects[0];
            string objectType = selectedObject.GetType().ToString();
            MessageBox.Show(
                string.Format(MessageFormat, objectType),
                Name    ,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}
