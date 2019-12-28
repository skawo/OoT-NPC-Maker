using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class SFXList : Form
    {
        string[] Data = Enum.GetNames(typeof(OotSFX.SFXes));
        public string ChosenSFX;

        public SFXList()
        {
            InitializeComponent();
            listBox1.Items.AddRange(Data);
            this.ActiveControl = textBox1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                foreach (string String in Data)
                {
                    if (String.ToUpper().Contains(textBox1.Text.ToUpper()))
                    {
                        listBox1.Items.Add(String);
                    }
                }
            }
            else
                listBox1.Items.AddRange(Data);

            listBox1.EndUpdate();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChosenSFX = (string)listBox1.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChosenSFX = (string)listBox1.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChosenSFX = (string)listBox1.SelectedItem;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}




















































