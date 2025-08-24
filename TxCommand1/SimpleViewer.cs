using System.Windows.Forms;
using Tecnomatix.Engineering;

namespace TxCommand1
{
    public partial class SimpleViewer : UserControl, ITxViewerControl, ITxViewerUpdate
    {
        public SimpleViewer()
        {
            InitializeComponent();
        }

        public string ViewerName => "SimpleViewer";
        public string Description => "A simple viewer for Tecnomatix";

        public ITxViewerMenusCollection Menus
        {
            get
            {
                TxViewerMenusCollection menus = new TxViewerMenusCollection();
                menus.AddItem(new TxViewerMenu("My menu", "My only menu"));
                return menus;
            }
        }
        public void Initialize()
        {
        }

        public void Uninitialize()
        {
        }

        public event TxViewerControl_MenuDisplayRequestedEventHandler MenuDisplayRequested;
        public void RefreshDisplay()
        {
        }
    }
}