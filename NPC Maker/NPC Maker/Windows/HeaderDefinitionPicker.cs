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
    public partial class OffsetFileStartPicker : Form
    {
        public string SelectedOptionOffset = "";
        public int SelectedIndexOffset = -1;
        public string SelectedOptionFileStart = "";
        public int SelectedIndexFileStart = -1;

        public OffsetFileStartPicker(List<string> Options, string CurrentOffset, string CurrentFileStart, string Explanation = "Offset:", string Title = "Select symbols from header...", string Explanation2 = "File start:", bool hasCancel = true)
        {
            InitializeComponent();

            Helpers.MakeNotResizableMonoSafe(this);

            Combo.Items.Add("None");

            foreach (string o in Options)
                Combo.Items.Add(o);

            int CurrentIndex = Combo.Items.IndexOf(CurrentOffset);

            if (CurrentIndex >= 0)
                Combo.SelectedIndex = CurrentIndex;
            else if (Combo.Items.Count != 0)
                Combo.SelectedIndex = 0;

            Combo2.Items.Add("None");

            foreach (string o in Options)
                Combo2.Items.Add(o);

            int CurrentIndex2 = Combo.Items.IndexOf(CurrentFileStart);

            if (CurrentIndex2 >= 0)
                Combo2.SelectedIndex = CurrentIndex2;
            else if (Combo.Items.Count != 0)
                Combo2.SelectedIndex = 0;

            LblExplanation.Text = Explanation;
            LblExplanation2.Text = Explanation2;
            this.Text = Title;

            if (!hasCancel)
                Btn_Cancel.Visible = false;

        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            SelectedOptionOffset = Combo.Text;
            SelectedIndexOffset = Combo.SelectedIndex;
            SelectedOptionFileStart = Combo2.Text;
            SelectedIndexFileStart = Combo2.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            SelectedOptionOffset = "";
            SelectedIndexOffset = -1;
            SelectedOptionFileStart = "";
            SelectedIndexFileStart = -1;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
