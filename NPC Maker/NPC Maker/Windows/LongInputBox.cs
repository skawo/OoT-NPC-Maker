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
    public partial class LongInputBox : Form
    {
        public string inputText;
        private string bFilter;

        public LongInputBox(string title, string command, string text, bool BrowseButton = false, string browseFilter = "")
        {
            InitializeComponent();
            Helpers.AdjustFormScale(this);

            inputText = text;
            this.Text = title;
            textBox1.Text = text;
            label1.Text = command;

            bFilter = browseFilter;

            if (!BrowseButton)
                button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(inputText))
                inputText = String.Empty;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            inputText = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = bFilter;

            if (of.ShowDialog() == DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(textBox1.Text))
                    textBox1.Text += ";";
               
                textBox1.Text += Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, of.FileName, Dicts.ProjectPathToken);
            }

        }
    }
}
