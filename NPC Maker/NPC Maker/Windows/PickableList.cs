using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class PickableList : Form
    {
        List<ListEntry> Data { get; set; }
        public ListEntry Chosen { get; set; }
        string FileName { get; set; }
        Lists.DictType DictType { get; set; }
        List<int> SkipEntries { get; set; }

        public PickableList(Lists.DictType Dict, bool PickMode = false, List<int> _SkipEntries = null)
        {
            InitializeComponent();

            SkipEntries = _SkipEntries;

            if (SkipEntries == null)
                SkipEntries = new List<int>();

            DictType = Dict;
            FileName = Dicts.DictFilenames[DictType];

            try
            {
                Data = new List<ListEntry>();
                string[] RawData = File.ReadAllLines(FileName);

                foreach (string Row in RawData)
                {
                    string[] NameAndDesc = Row.Split(',');

                    if (NameAndDesc.Length < 3)
                        Data.Add(new ListEntry(Convert.ToInt16(NameAndDesc[0]), NameAndDesc[1], ""));
                    else
                        Data.Add(new ListEntry(Convert.ToInt16(NameAndDesc[0]), NameAndDesc[1], NameAndDesc[2]));
                }
            }
            catch (Exception)
            {
                MessageBox.Show(FileName + " is missing or incorrect.");
                return;
            }

            SetListView(SkipEntries);

            this.ActiveControl = Btn_Search;

            if (!PickMode)
            {
                Btn_OK.Visible = false;
                listView1.MouseDoubleClick -= ListDoubleClick;
                listView1.KeyUp -= EnterPress;
            }
        }

        private void SetListView(List<int> SkipEntries)
        {
            listView1.BeginUpdate();

            listView1.Items.Clear();

            Data = Data.OrderBy(x => x.ID).ToList();

            foreach (ListEntry Entry in Data)
            {
                if (!SkipEntries.Contains(Entry.ID))
                    AddEntryToList(Entry);
            }

            listView1.EndUpdate();
        }

        private void AddEntryToList(ListEntry Entry)
        {
            listView1.Items.Add(new ListViewItem(new string[] { Entry.ID.ToString(),
                                                                "0x" + Entry.ID.ToString("X1"),
                                                                Entry.Name,
                                                                Entry.Description }));
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            if (!string.IsNullOrEmpty(Btn_Search.Text))
            {
                foreach (ListEntry Entry in Data)
                {
                    if (Entry.ID.ToString().ToUpper().Contains(Btn_Search.Text.ToUpper())
                        || Entry.Name.ToUpper().Contains(Btn_Search.Text.ToUpper())
                        || Entry.Description.ToUpper().Contains(Btn_Search.Text.ToUpper()))
                        AddEntryToList(Entry);
                }
            }
            else
                foreach (ListEntry Entry in Data)
                    AddEntryToList(Entry);

            listView1.EndUpdate();
        }

        private void PickCurrentAndClose()
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            string ID = (string)listView1.SelectedItems[0].Text;
            Chosen = Data.Find(x => x.ID.ToString() == ID);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ListDoubleClick(object sender, MouseEventArgs e)
        {
            PickCurrentAndClose();
        }

        private void OKClick(object sender, EventArgs e)
        {
            PickCurrentAndClose();
        }

        private void EnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PickCurrentAndClose();
            }
        }

        private void Btn_EditEntry_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            string ID = (string)listView1.SelectedItems[0].Text;
            ListEntry Sel = Data.Find(x => x.ID.ToString() == ID);

            if (Sel.ID < 0)
            {
                MessageBox.Show("Entries with negative IDs are treated as editor constants and cannot be edited.");
                return;
            }

            PickableListEntryEdit pE = new PickableListEntryEdit(Sel.ID, Sel.Name, Sel.Description);
            DialogResult dr = pE.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (Sel.ID == pE.Out_EntryID ||
                    MessageBox.Show("New entry ID already exists on the list. Replace?", "Replace ID?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int DataIndex = Data.FindIndex(x => x.ID == pE.Out_EntryID);

                    ListEntry NewEntry = new ListEntry(pE.Out_EntryID, pE.Out_Name, pE.Out_Desc);

                    Data[DataIndex] = NewEntry;
                    SetListView(SkipEntries);
                }

            }
        }

        private void Btn_AddEntry_Click(object sender, EventArgs e)
        {
            PickableListEntryEdit pE = new PickableListEntryEdit(0, "", "");
            DialogResult dr = pE.ShowDialog();

            if (dr == DialogResult.OK)
            {
                ListEntry NewEntry = new ListEntry(pE.Out_EntryID, pE.Out_Name, pE.Out_Desc);

                if (NewEntry.ID < 0)
                {
                    MessageBox.Show("Entries with negative IDs are treated as editor constants and cannot be edited.");
                    return;
                }

                ListEntry Sel = Data.Find(x => x.ID == pE.Out_EntryID);

                if (Sel != null)
                {
                    DialogResult Res = MessageBox.Show("New entry ID already exists on the list. Replace?", "Replace ID?", MessageBoxButtons.YesNo);

                    if (Res == DialogResult.Yes)
                    {
                        int DataIndex = Data.FindIndex(x => x.ID == pE.Out_EntryID);

                        Data[DataIndex] = NewEntry;
                        SetListView(SkipEntries);
                    }
                }
                else
                {
                    Data.Add(NewEntry);
                    SetListView(SkipEntries);
                }
            }
        }

        private void Btn_DelEntry_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            string ID = (string)listView1.SelectedItems[0].Text;
            ListEntry Sel = Data.Find(x => x.ID.ToString() == ID);

            if (Sel.ID < 0)
            {
                MessageBox.Show("Entries with negative IDs are treated as editor constants and cannot be deleted.");
                return;
            }

            DialogResult Res = MessageBox.Show("Definitely remove this entry?", "Remove ID?", MessageBoxButtons.YesNo);

            if (Res == DialogResult.Yes)
            {
                int DataIndex = Data.FindIndex(x => x.ID == Sel.ID);
                Data.RemoveAt(DataIndex);
                SetListView(SkipEntries);
            }
        }

        private void PickableList_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                string Out = "";



                foreach (ListEntry Entry in Data)
                    Out = String.Concat(Out, Entry.ID.ToString(), ",", Entry.Name, ",", Entry.Description, Environment.NewLine);

                File.WriteAllText(FileName, Out);
                Dicts.ReloadDict(DictType);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data." + ex.Message);
                return;
            }
        }
    }

    public class ListEntry
    {
        public short ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ListEntry(short _ID, string _Name, string _Description)
        {
            ID = _ID;
            Name = _Name;
            Description = _Description;
        }
    }
}




















































