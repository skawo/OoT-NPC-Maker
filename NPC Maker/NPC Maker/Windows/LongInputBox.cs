using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Windows
{
    public partial class LongInputBox : Form
    {
        public string inputText;
        private string bFilter;
        private bool folderExpl;
        private string resetStr;
        public LongInputBox(string title, string command, string text, bool BrowseButton = false, string browseFilter = "", bool folderExplorer = false, string resetString = "")
        {
            InitializeComponent();
            Helpers.AdjustFormScale(this);

            inputText = text;
            this.Text = title;
            textBox1.Text = text;
            label1.Text = command;
            resetStr = resetString;

            bFilter = browseFilter;
            folderExpl = folderExplorer;

            if (!BrowseButton)
                button2.Visible = false;

            if (String.IsNullOrEmpty(resetStr))
                button3.Visible = false;
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
            if (folderExpl)
            {
                NativeFolderBrowserDialog fd = new NativeFolderBrowserDialog();

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(textBox1.Text))
                        textBox1.Text += ";";

                    textBox1.Text += Helpers.NormalizeExtPath(fd.SelectedPath);
                }
            }
            else
            {
                NativeOpenFileDialog of = new NativeOpenFileDialog();
                of.Filter = bFilter;

                if (of.ShowDialog() == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(textBox1.Text))
                        textBox1.Text += ";";

                    textBox1.Text += Helpers.NormalizeExtPath(of.FileName);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = resetStr;
        }
    }
}
