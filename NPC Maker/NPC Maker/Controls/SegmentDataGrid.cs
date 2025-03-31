using System.Reflection;
using System.Windows.Forms;

namespace NPC_Maker.Controls
{
    public partial class SegmentDataGrid : UserControl
    {
        public SegmentDataGrid()
        {
            InitializeComponent();

            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, this, new object[] { DoubleBuffered });
        }
    }
}
