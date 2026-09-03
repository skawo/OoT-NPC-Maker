using NPC_Maker.Common;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class Debug : Form
    {
        public Debug(string Text)
        {
            InitializeComponent();
            GUIHacks.AdjustFormScaleAndColors(this);

            GUIHacks.MakeNotResizableMonoSafe(this);

            fastColoredTextBox1.Text = Text;
        }
    }
}
