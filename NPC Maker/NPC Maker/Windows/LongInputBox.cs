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
            Helpers.MakeNotResizableMonoSafe(this);

            inputText = text;
            this.Text = title;
            textBox1.Text = text;
            label1.Text = command;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            inputText = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(inputText))
                inputText = String.Empty;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
