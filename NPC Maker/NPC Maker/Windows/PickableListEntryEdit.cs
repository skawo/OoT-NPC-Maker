using System;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class PickableListEntryEdit : Form
    {
        public short Out_EntryID;
        public string Out_Name;
        public string Out_Desc;

        public PickableListEntryEdit(int ID, string Name, string Desc)
        {
            InitializeComponent();

            Txb_Desc.Text = Desc;
            Txb_Name.Text = Name;
            NumUp_ID.Value = ID;
        }

        private void NumUp_ID_ValueChanged(object sender, EventArgs e)
        {
            NumUp_HexID.ValueChanged -= NumUp_HexID_ValueChanged;

            NumUp_HexID.Value = NumUp_ID.Value;

            NumUp_HexID.ValueChanged += NumUp_HexID_ValueChanged;

            Out_EntryID = (short)NumUp_ID.Value;
        }

        private void NumUp_HexID_ValueChanged(object sender, EventArgs e)
        {
            NumUp_ID.ValueChanged -= NumUp_ID_ValueChanged;

            NumUp_ID.Value = NumUp_HexID.Value;

            NumUp_ID.ValueChanged += NumUp_ID_ValueChanged;

            Out_EntryID = (short)NumUp_HexID.Value;
        }

        private void Btn_SelectObject_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Txb_Name_TextChanged(object sender, EventArgs e)
        {
            Out_Name = Txb_Name.Text;
        }

        private void Txb_Desc_TextChanged(object sender, EventArgs e)
        {
            Out_Desc = Txb_Desc.Text;
        }
    }
}
