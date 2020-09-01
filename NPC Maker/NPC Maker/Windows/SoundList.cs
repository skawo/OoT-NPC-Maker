using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace NPC_Maker
{
    public partial class SoundList : Form
    {
        List<string[]> Data { get; set; }
        public string ChosenSFX;

        public SoundList(string CSV)
        {
            InitializeComponent();

            try
            {
                Data = new List<string[]>();
                string[] RawData = File.ReadAllLines(CSV);

                foreach (string Row in RawData)
                {
                    string[] NameAndDesc = Row.Split(',');

                    if (NameAndDesc.Length < 3)
                        Data.Add(new string[] { "0x" + Convert.ToInt32(NameAndDesc[0]).ToString("X"), NameAndDesc[1], "" });
                    else
                        Data.Add(new string[] { "0x" + Convert.ToInt32(NameAndDesc[0]).ToString("X"), NameAndDesc[1], NameAndDesc[2] });
                }
            }
            catch (Exception)
            {
                MessageBox.Show(CSV + " is missing or incorrect.");
                return;
            }

            foreach (string[] Row in Data)
                listView1.Items.Add(new ListViewItem(new string[] { Row[0], Row[1], Row[2] }));

            this.ActiveControl = textBox1;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                foreach (string[] s in Data)
                {
                    if (s[0].ToUpper().Contains(textBox1.Text.ToUpper()) 
                        || s[1].ToUpper().Contains(textBox1.Text.ToUpper())
                        || s[2].ToUpper().Contains(textBox1.Text.ToUpper()))
                    {
                        listView1.Items.Add(new ListViewItem(new string[] { s[0], s[1], s[2] }));
                    }
                }
            }
            else
                foreach (string[] Row in Data)
                    listView1.Items.Add(new ListViewItem(new string[] { Row[0], Row[1], Row[2] }));

            listView1.EndUpdate();
        }

        private void ListDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            string ID = (string)listView1.SelectedItems[0].Text;
            ChosenSFX = Data.Find(x => x[0] == ID)[1];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OKClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            string ID = (string)listView1.SelectedItems[0].Text;
            ChosenSFX = Data.Find(x => x[0] == ID)[1];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EnterPress(object sender, KeyEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (e.KeyCode == Keys.Enter)
            {
                string ID = (string)listView1.SelectedItems[0].Text;
                ChosenSFX = Data.Find(x => x[0] == ID)[1];
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}




















































