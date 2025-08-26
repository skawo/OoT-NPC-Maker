using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class Debug : Form
    {
        public Debug(string Text)
        {
            InitializeComponent();
            Helpers.MakeNotResizableMonoSafe(this);

            fastColoredTextBox1.Text = Text;
        }
    }
}
