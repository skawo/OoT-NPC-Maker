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

        public LongInputBox(string title, string command, string text)
        {
            InitializeComponent();

            inputText = text;
            this.Text = title;
            textBox1.Text = text;
            label1.Text = command;
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
    }
}
