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
        readonly NPCFile EditedFile;
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

                ScriptEditor Se = new ScriptEditor(ref Dummy2, ref Dummy, Entry, SyntaxH, CheckSynt) { Dock = DockStyle.Fill };
                Page.Controls.Add(Se);

                Tab.TabPages.Add(Page);
            }
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

            ScriptEditor Se = new ScriptEditor(ref Dummy2, ref Dummy, Sc, SyntaxHeader, CheckSyntax)
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
    }
}
