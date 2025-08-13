using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Windows
{
    public partial class GlobalHeader : Form
    {
        NPCFile EditedFile;
        readonly bool SyntaxHeader;
        readonly bool CheckSyntax;

        NPCFile Dummy = new NPCFile();
        NPCEntry Dummy2 = new NPCEntry();

        public GlobalHeader(ref NPCFile File, bool SyntaxH, bool CheckSynt)
        {
            InitializeComponent();

            EditedFile = File;
            SyntaxHeader = SyntaxH;
            CheckSyntax = CheckSynt;

            foreach (ScriptEntry Entry in File.GlobalHeaders)
            {
                TabPage Page = new TabPage
                {
                    Text = Entry.Name
                };

                ScriptEditor Se = new ScriptEditor(ref Dummy2, ref Dummy, Entry, Program.Settings.ColorizeScriptSyntax, Program.Settings.CheckSyntax) { Dock = DockStyle.Fill };
                Page.Controls.Add(Se);

                Tab.TabPages.Add(Page);
            }

            Tx_HeaderPath.Text = File.ExtScriptHeaderPath;
        }

        private string GetName(string Current = "")
        {
            string ScriptName = Current;
            DialogResult Dr = InputBox.ShowInputDialog("Header name?", ref ScriptName);

            if (Dr != DialogResult.OK)
                return "";
            else
                return ScriptName;
        }

        private void AddNew_Click(object sender, EventArgs e)
        {
            string ScriptName = GetName();
            TabPage Page = new TabPage(ScriptName);

            ScriptEntry Sc = new ScriptEntry() { Name = ScriptName, ParseErrors = new List<string>(), Text = "" };
            EditedFile.GlobalHeaders.Add(Sc);

            ScriptEditor Se = new ScriptEditor(ref Dummy2, ref Dummy, Sc, Program.Settings.ColorizeScriptSyntax, Program.Settings.CheckSyntax)
            {
                Dock = DockStyle.Fill
            };

            

            Page.Controls.Add(Se);
            Tab.TabPages.Add(Page);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (Tab.TabPages.Count == 0)
                return;

            DialogResult Res = MessageBox.Show("Are you sure you want to delete this global header? You cannot reverse this action!", "Removing a header", MessageBoxButtons.YesNoCancel);

            if (Res == DialogResult.Yes)
            {
                EditedFile.GlobalHeaders.Remove((Tab.SelectedTab.Controls[0] as ScriptEditor).Script);
                Tab.TabPages.Remove(Tab.SelectedTab);
            }
        }

        private void Rename_Click(object sender, EventArgs e)
        {
            if (Tab.TabPages.Count == 0)
                return;

            string Name = GetName(Tab.SelectedTab.Text);

            if (Name != "")
            {

                (Tab.SelectedTab.Controls[0] as ScriptEditor).Script.Name = Name;
                Tab.SelectedTab.Text = Name;
            }
        }

        private void Tab_MouseUp(object sender, MouseEventArgs e)
        {
            int PageClicked = -1;

            for (int i = 0; i < Tab.TabCount; i++)
            {
                if (Tab.GetTabRect(i).Contains(e.Location))
                    PageClicked = i;
            }

            if (e.Button == MouseButtons.Right)
            {
                Tab.SelectedTab = Tab.TabPages[PageClicked];

                ContextMenuStrip mn = new ContextMenuStrip();

                ToolStripMenuItem renameHeader = new ToolStripMenuItem();
                ToolStripMenuItem deleteHeader = new ToolStripMenuItem();
                ToolStripMenuItem newHeader = new ToolStripMenuItem();

                mn.Items.AddRange(new ToolStripItem[] { newHeader, renameHeader, deleteHeader });

                renameHeader.Size = new System.Drawing.Size(156, 22);
                renameHeader.Text = "Rename";
                renameHeader.Click += Rename_Click;

                deleteHeader.Size = new System.Drawing.Size(156, 22);
                deleteHeader.Text = "Delete";
                deleteHeader.Click += Delete_Click;

                newHeader.Size = new System.Drawing.Size(156, 22);
                newHeader.Text = "Add new";
                newHeader.Click += AddNew_Click;

                mn.Show(Tab.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        private void Btn_HeaderBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Header files (*.h)|*.h|All files (*.*)|*.*";
            of.InitialDirectory = Program.Settings.ProjectPath;

            if (of.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(Program.Settings.ProjectPath))
                    Tx_HeaderPath.Text = of.FileName;
                else
                    Tx_HeaderPath.Text = Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, of.FileName, "{PROJECTPATH}");
            }
        }

        private void Tx_HeaderPath_TextChanged(object sender, EventArgs e)
        {
            EditedFile.ExtScriptHeaderPath = Tx_HeaderPath.Text;
        }
    }
}

