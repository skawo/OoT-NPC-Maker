using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            LblVersion.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        }
    }
}
