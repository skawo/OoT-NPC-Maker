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

            this.Text = title;
            this.label1.Text = command;
            this.textBox1.Text = text;
            this.DialogResult = DialogResult.Cancel;
            inputText = text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            inputText = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
