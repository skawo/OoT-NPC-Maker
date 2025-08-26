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
    public partial class ComboPicker : Form
    {
        public string SelectedOption = "";
        public int SelectedIndex = -1;

        public ComboPicker(List<string> Options, string Explanation, string Title, bool hasCancel = true)
        {
            InitializeComponent();

            Helpers.MakeNotResizableMonoSafe(this);

            foreach (string o in Options)
                Combo.Items.Add(o);

            if (Combo.Items.Count != 0)
                Combo.SelectedIndex = 0;

            LblExplanation.Text = Explanation;
            this.Text = Title;

            if (!hasCancel)
                Btn_Cancel.Visible = false;

        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            SelectedOption = Combo.Text;
            SelectedIndex = Combo.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            SelectedOption = "";
            SelectedIndex = -1;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
