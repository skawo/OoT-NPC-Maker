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
    public partial class SFXList : Form
    {
        List<string[]> Data = new List<string[]>();
        public string ChosenSFX;

        public SFXList()
        {
            InitializeComponent();

            try
            {
                string[] RawData = File.ReadAllLines("SFX.csv");

                foreach (string Row in RawData)
                {
                    string[] NameAndDesc = Row.Split(',');

                    if (NameAndDesc.Length == 1)
                        Data.Add(new string[] { NameAndDesc[0], "" });
                    else
                        Data.Add(NameAndDesc);
                }
            }
            catch (Exception)
            {
                string[] DataWoDescriptions = Enum.GetNames(typeof(OotSFX.SFXes));

                foreach (string Row in DataWoDescriptions)
                    Data.Add(new string[] { Row, "" });
            }

            foreach (string[] Row in Data)
                listView1.Items.Add(new ListViewItem(new string[] { Row[0], Row[1] }));

            this.ActiveControl = textBox1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                foreach (string[] s in Data)
                {
                    if (s[0].ToUpper().Contains(textBox1.Text.ToUpper()) 
                        || s[1].ToUpper().Contains(textBox1.Text.ToUpper()))
                    {
                        listView1.Items.Add(new ListViewItem(new string[] { s[0], s[1] }));
                    }
                }
            }
            else
                foreach (string[] Row in Data)
                    listView1.Items.Add(new ListViewItem(new string[] { Row[0], Row[1] }));

            listView1.EndUpdate();
        }

        private void ListDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ChosenSFX = (string)listView1.SelectedItems[0].Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OKClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ChosenSFX = (string)listView1.SelectedItems[0].Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EnterPress(object sender, KeyEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (e.KeyCode == Keys.Enter)
            {
                ChosenSFX = (string)listView1.SelectedItems[0].Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}




















































