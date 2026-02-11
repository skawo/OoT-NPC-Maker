using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            SetupScale();

            Helpers.MakeNotResizableMonoSafe(this);

            LblVersion.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        }

        private void SetupScale()
        {
            float scale = Program.Settings.GUIScale;
            float fontSize = Helpers.GetScaleFontSize();

            this.Font = new Font(this.Font.FontFamily, fontSize);
            this.LblVersion.Font = new Font(this.LblVersion.Font.FontFamily, fontSize, FontStyle.Bold);
            this.LblVersionX.Font = new Font(this.LblVersionX.Font.FontFamily, fontSize, FontStyle.Bold);
            this.CreditsHeader.Font = new Font(this.CreditsHeader.Font.FontFamily, fontSize, FontStyle.Bold);

            Helpers.AdjustFormScale(this);
        }
    }
}
