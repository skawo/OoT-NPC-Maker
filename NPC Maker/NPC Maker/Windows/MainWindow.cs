using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using NPC_Maker.Windows;

namespace NPC_Maker
{
    public partial class MainWindow : Form
    {
        string OpenedPath = "";
        NPCFile EditedFile = null;
        NPCEntry SelectedEntry = null;
        NPCEntry CopiedEntry = null;
        int SelectedIndex = -1;
        string OpenedFile = JsonConvert.SerializeObject(new NPCFile(), Formatting.Indented);
        Control LastRightClickedTextbox = null;

        bool SyntaxHighlighting = true;

        public MainWindow()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.ResizeBegin += Form1_ResizeBegin;
            this.ResizeEnd += Form1_ResizeEnd;

            foreach (string Item in Enum.GetNames(typeof(Lists.InstructionIDs)))
            {
                ToolStripMenuItem Tsmi = new ToolStripMenuItem
                {
                    Text = Item
                };

                if (Lists.FunctionSubtypes.ContainsKey(Item))
                {
                    Tsmi.DoubleClickEnabled = true;
                    Tsmi.DoubleClick += Tsmi_DoubleClick;
                    AddItemCollectionToToolStripMenuItem(Lists.FunctionSubtypes[Item], Tsmi);
                }
                else
                    Tsmi.Click += Tsmi_Click;

                functionsToolStripMenuItem.DropDownItems.Add(Tsmi);
            }

            AddItemCollectionToToolStripMenuItem(Lists.Keywords.ToArray(), keywordsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Lists.KeywordsDarkGray.ToArray(), keywordsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Lists.KeyValues.ToArray(), keyValuesToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.GiveItems)), itemsgiveToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.TradeItems)), itemstradeToolStripMenuItem);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.ResumeLayout();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (EditedFile != null)
            {
                string CurrentFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

                if (!String.Equals(CurrentFile, OpenedFile))
                {
                    DialogResult Res = MessageBox.Show("Save changes before exiting?", "Save changes?", MessageBoxButtons.YesNoCancel);

                    if (Res == DialogResult.Yes)
                    {
                        FileMenu_SaveAs_Click(this, null);
                    }
                    else if (Res == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void InsertTxtToScript(string Text)
        {
            FastColoredTextBox T = (LastRightClickedTextbox as FastColoredTextBox);

            int start = T.SelectionStart;
            string newTxt = T.Text;
            newTxt = newTxt.Remove(T.SelectionStart, T.SelectionLength);
            newTxt = newTxt.Insert(T.SelectionStart, Text);
            T.Text = newTxt;
            T.SelectionStart = start + Text.Length;
        }

        private void AddItemCollectionToToolStripMenuItem(string[] Collection, ToolStripMenuItem MenuItem)
        {
            MenuItem.DropDown.MaximumSize = new Size(300, 700);

            foreach (string Item in Collection)
            {
                ToolStripItem SubItem = MenuItem.DropDownItems.Add(Item);
                SubItem.Click += SubItem_Click;
            }
        }

        private void InsertDataIntoDataGridView()
        {
            this.SuspendLayout();

            DataGrid_NPCs.SelectionChanged -= DataGrid_NPCs_SelectionChanged;

            DataGrid_NPCs.Rows.Clear();

            int i = 0;

            foreach (NPCEntry Entry in EditedFile.Entries)
            {
                DataGrid_NPCs.Rows.Add(new object[] { i.ToString(), Entry.IsNull ? "EMPTY" : Entry.NPCName });
                i++;
            }

            DataGrid_NPCs.SelectionChanged += DataGrid_NPCs_SelectionChanged;

            DataGrid_NPCs_SelectionChanged(DataGrid_NPCs, null);

            this.ResumeLayout();

        }

        private void InsertDataToEditor()
        {
            this.SuspendLayout();

            if (SelectedEntry == null)
                return;

            Textbox_NPCName.Text = SelectedEntry.NPCName;

            NumUpDown_ObjectID.Value = SelectedEntry.ObjectID;
            NumUpDown_Hierarchy.Value = SelectedEntry.Hierarchy;
            ComboBox_HierarchyType.SelectedIndex = SelectedEntry.HierarchyType;
            NumUpDown_XModelOffs.Value = SelectedEntry.ModelOffs[0];
            NumUpDown_YModelOffs.Value = SelectedEntry.ModelOffs[1];
            NumUpDown_ZModelOffs.Value = SelectedEntry.ModelOffs[2];
            NumUpDown_Scale.Value = (decimal)SelectedEntry.Scale;
            NumUpDown_CutsceneSlot.Value = SelectedEntry.CutsceneID;

            ComboBox_LookAtType.SelectedIndex = SelectedEntry.LookAtType;

            Combo_Head_Horiz.SelectedIndex = SelectedEntry.HeadHorizAxis;
            Combo_Head_Vert.SelectedIndex = SelectedEntry.HeadVertAxis;
            Combo_Waist_Horiz.SelectedIndex = SelectedEntry.WaistHorizAxis;
            Combo_Waist_Vert.SelectedIndex = SelectedEntry.WaistVertAxis;

            NumUpDown_HeadLimb.Value = SelectedEntry.HeadLimb;
            NumUpDown_WaistLimb.Value = SelectedEntry.WaistLimb;
            NumUpDown_LookAt_X.Value = SelectedEntry.LookAtOffs[0];
            NumUpDown_LookAt_Y.Value = SelectedEntry.LookAtOffs[1];
            NumUpDown_LookAt_Z.Value = SelectedEntry.LookAtOffs[2];
            NumUpDown_DegVert.Value = SelectedEntry.DegreesVert;
            NumUpDown_DegHoz.Value = SelectedEntry.DegreesHor;

            Checkbox_DrawShadow.Checked = SelectedEntry.Shadow;
            NumUpDown_ShRadius.Value = SelectedEntry.ShRadius;

            Checkbox_HaveCollision.Checked = SelectedEntry.Collision;
            Checkbox_CanPressSwitches.Checked = SelectedEntry.Switches;
            Checkbox_Pushable.Checked = SelectedEntry.Pushable;
            NumUpDown_ColRadius.Value = SelectedEntry.ColRadius;
            NumUpDown_ColHeight.Value = SelectedEntry.Height;
            Checkbox_AlwaysActive.Checked = SelectedEntry.AlwActive;
            Checkbox_AlwaysDraw.Checked = SelectedEntry.AlwDraw;

            NumUpDown_XColOffs.Value = SelectedEntry.ColOffs[0];
            NumUpDown_YColOffs.Value = SelectedEntry.ColOffs[1];
            NumUpDown_ZColOffs.Value = SelectedEntry.ColOffs[2];

            Checkbox_Targettable.Checked = SelectedEntry.Targettable;
            ComboBox_TargetDist.SelectedIndex = SelectedEntry.TargetDist <= 10 ? SelectedEntry.TargetDist : 1;
            NumUpDown_TargetLimb.Value = SelectedEntry.TargetLimb;
            NumUpDown_XTargetOffs.Value = SelectedEntry.TargOffs[0];
            NumUpDown_YTargetOffs.Value = SelectedEntry.TargOffs[1];
            NumUpDown_ZTargetOffs.Value = SelectedEntry.TargOffs[2];

            Combo_MovementType.SelectedIndex = SelectedEntry.MovementType;
            NumUpDown_MovDistance.Value = SelectedEntry.MovementDistance;
            NumUpDown_MovSpeed.Value = (decimal)SelectedEntry.MovementSpeed;
            NumUpDown_LoopDelay.Value = SelectedEntry.LoopDel;
            NumUpDown_LoopEndNode.Value = SelectedEntry.LoopEnd;
            NumUpDown_LoopStartNode.Value = SelectedEntry.LoopStart;
            Checkbox_Loop.Checked = SelectedEntry.Loop;
            NumUpDown_PathFollowID.Value = SelectedEntry.PathID;

            ComboBox_AnimType.SelectedIndex = SelectedEntry.AnimationType;
            NumUpDown_Scale.Value = (decimal)SelectedEntry.Scale;

            Textbox_Script.Text = SelectedEntry.Script;
            Textbox_Script2.Text = SelectedEntry.Script2;

            /*
            dataGridView1.Rows.Clear();

            foreach (ColorEntry ColorE in SelectedEntry.Colors)
            {
                int RowIndex = dataGridView1.Rows.Add(new object[] { ColorE.Limbs, "" });

                dataGridView1.Rows[RowIndex].Cells[1].Style = new DataGridViewCellStyle()
                {
                    SelectionBackColor = ColorE.Color,
                    SelectionForeColor = ColorE.Color,
                    BackColor = ColorE.Color
                };
            }
            */

            DataGrid_Animations.Rows.Clear();

            foreach (AnimationEntry Animation in SelectedEntry.Animations)
            {
                string Frames = "";

                foreach (byte B in Animation.Frames)
                {
                    if (B != 0xFF)
                    {
                        if (Frames == "")
                            Frames = Convert.ToInt32(B).ToString();
                        else
                            Frames = String.Join(",", new string[] { Frames, Convert.ToInt32(B).ToString() });
                    }
                }

                DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Animation.Address.ToString("X"), Frames, Animation.Speed, Animation.ObjID == UInt16.MaxValue ? "---" : Animation.ObjID.ToString() });
            }

            Textbox_ParseErrors.Text = "";

            if (SelectedEntry.ParseErrors.Count == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
            {
                foreach (string Error in SelectedEntry.ParseErrors)
                    Textbox_ParseErrors.Text += Error + Environment.NewLine;
            }

            Textbox_ParseErrors2.Text = "";

            if (SelectedEntry.ParseErrors2.Count == 0)
                Textbox_ParseErrors2.Text = "Parsed successfully!";
            else
            {
                foreach (string Error in SelectedEntry.ParseErrors2)
                {
                    Textbox_ParseErrors2.Text += Error + Environment.NewLine;
                }
            }

            Button_EnvironmentColorPreview.BackColor = Color.FromArgb(255, SelectedEntry.EnvColor.R, SelectedEntry.EnvColor.G, SelectedEntry.EnvColor.B);

            if (SelectedEntry.EnvColor.A == 0)
                Checkbox_EnvColor.Checked = false;
            else
                Checkbox_EnvColor.Checked = true;

            Textbox_BlinkPattern.Text = SelectedEntry.BlinkPattern;
            Textbox_TalkingPattern.Text = SelectedEntry.TalkPattern;
            NumUpDown_BlinkSegment.Value = SelectedEntry.BlinkSegment;
            NumUpDown_BlinkSpeed.Value = SelectedEntry.BlinkSpeed;
            NumUpDown_TalkSegment.Value = SelectedEntry.TalkSegment;
            NumUpDown_TalkSpeed.Value = SelectedEntry.TalkSpeed;

            for (int i = 0; i < SelectedEntry.Textures.Count; i++)
            {
                DataGridView Grid = (TabControl_Textures.TabPages[i].Controls[0] as DataGridView);

                Grid.Rows.Clear();

                foreach (TextureEntry Entry in SelectedEntry.Textures[i])
                    Grid.Rows.Add(Entry.Name, Entry.Address.ToString("X"), Entry.ObjectID == -1 ? SelectedEntry.ObjectID.ToString() : Entry.ObjectID.ToString());
            }

            DataGridView_ExtraDLists.Rows.Clear();

            foreach (DListEntry Dlist in SelectedEntry.DLists)
            {
                string SelCombo = "";

                switch (Dlist.ShowType)
                {
                    case 0: SelCombo = "Don't show"; break;
                    case 1: SelCombo = "Alongside limb"; break;
                    case 2: SelCombo = "Instead of limb"; break;
                    default: SelCombo = "Don't show"; break;
                }


                DataGridView_ExtraDLists.Rows.Add(new object[] { Dlist.Name,
                                                                 Dlist.Address.ToString("X"),
                                                                 Dlist.TransX.ToString() + "," + Dlist.TransY.ToString() + "," + Dlist.TransZ.ToString(),
                                                                 Dlist.RotX.ToString() + "," + Dlist.RotY.ToString() + "," + Dlist.RotZ.ToString(),
                                                                 Dlist.Scale.ToString(),
                                                                 Dlist.Limb.ToString(),
                                                                 Dlist.ObjectID == -1 ? SelectedEntry.ObjectID.ToString() : Dlist.ObjectID.ToString(),
                                                                 SelCombo
                                                                });
            }

            this.ResumeLayout();
        }

        #region MenuStrip

        private bool SaveChangesAsPrompt()
        {
            if (EditedFile != null)
            {
                string CurrentFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

                if (!String.Equals(CurrentFile, OpenedFile))
                {
                    DialogResult DR = MessageBox.Show("Save changes before opening a new file?", "Save changes?", MessageBoxButtons.YesNoCancel);

                    if (DR == DialogResult.Yes)
                    {
                        FileMenu_SaveAs_Click(this, null);
                        return true;
                    }
                    else if (DR == DialogResult.No)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return true;
            }
            else
                return true;
        }

        private void FileMenu_Open_Click(object sender, EventArgs e)
        {
            if (SaveChangesAsPrompt() == false)
                return;

            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();

            if (OFD.FileName != "")
            {
                EditedFile = FileOps.ParseJSONFile(OFD.FileName);
                OpenedFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

                if (EditedFile != null)
                {
                    OpenedPath = OFD.FileName;
                    Panel_Editor.Enabled = true;
                    InsertDataIntoDataGridView();
                }
            }

        }

        private void FileMenu_New_Click(object sender, EventArgs e)
        {
            if (SaveChangesAsPrompt() == false)
                return;

            EditedFile = new NPCFile();
            Panel_Editor.Enabled = true;
            InsertDataIntoDataGridView();
        }

        private void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog
            {
                FileName = "ActorData.json"
            };
            SFD.ShowDialog();

            if (SFD.FileName != "")
            {
                OpenedFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);
                FileOps.SaveJSONFile(SFD.FileName, EditedFile);
            }
        }

        private void FileMenu_Save_Click(object sender, EventArgs e)
        {
            if (OpenedPath == "")
                FileMenu_SaveAs_Click(this, null);
            else
            {
                OpenedFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);
                FileOps.SaveJSONFile(OpenedPath, EditedFile);
            }
        }

        private void FileMenu_SaveBinary_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog
            {
                FileName = "zobj.zobj"
            };
            SFD.ShowDialog();

            if (SFD.FileName != "")
            {
                FileOps.SaveBinaryFile(SFD.FileName, EditedFile);
            }

            InsertDataToEditor();
        }

        private void FileMenu_Exit_Click(object sender, EventArgs e)
        {
            if (SaveChangesAsPrompt() == false)
                return;

            Application.Exit();
        }

        #endregion

        #region NPCList

        private void Button_CopyBase_Click(object sender, EventArgs e)
        {
            if (DataGrid_NPCs.SelectedRows.Count == 0)
                return;

            string Copy = JsonConvert.SerializeObject(SelectedEntry, Formatting.Indented);
            CopiedEntry = JsonConvert.DeserializeObject<NPCEntry>(Copy);
        }

        private void Button_PasteBase_Click(object sender, EventArgs e)
        {
            if (DataGrid_NPCs.SelectedRows.Count == 0)
                return;

            if (CopiedEntry != null && !SelectedEntry.IsNull)
            {
                SelectedEntry.Animations.Clear();

                foreach (AnimationEntry Anim in CopiedEntry.Animations)
                {
                    SelectedEntry.Animations.Add(new AnimationEntry(Anim.Name, Anim.Address, Anim.Speed, Anim.ObjID, Anim.Frames));
                }

                SelectedEntry.Textures.Clear();

                foreach (List<TextureEntry> TexList in CopiedEntry.Textures)
                {
                    List<TextureEntry> DestTexList = new List<TextureEntry>();
                    SelectedEntry.Textures.Add(DestTexList);

                    foreach (TextureEntry Tex in TexList)
                    {
                        DestTexList.Add(new TextureEntry(Tex.Name, Tex.Address, Tex.ObjectID));
                    }

                }

                SelectedEntry.DLists.Clear();

                foreach (DListEntry D in CopiedEntry.DLists)
                {
                    SelectedEntry.DLists.Add(new DListEntry(D.Name, D.Address, D.TransX, D.TransY, D.TransZ, D.RotX, D.RotY, D.RotZ, D.Scale, D.Limb, D.ShowType, D.ObjectID));
                }

                SelectedEntry.ObjectID = CopiedEntry.ObjectID;
                SelectedEntry.Hierarchy = CopiedEntry.Hierarchy;
                SelectedEntry.HierarchyType = CopiedEntry.HierarchyType;
                SelectedEntry.AnimationType = CopiedEntry.AnimationType;
                SelectedEntry.BlinkPattern = CopiedEntry.BlinkPattern;
                SelectedEntry.BlinkSegment = CopiedEntry.BlinkSegment;
                SelectedEntry.BlinkSpeed = CopiedEntry.BlinkSpeed;
                SelectedEntry.TalkPattern = CopiedEntry.TalkPattern;
                SelectedEntry.TalkSegment = CopiedEntry.TalkSegment;
                SelectedEntry.TalkSpeed = CopiedEntry.TalkSpeed;

                InsertDataToEditor();
            }
        }

        private void Button_Add_Click(object sender, EventArgs e)
        {
            NPCEntry Entry = new NPCEntry();
            Entry.Animations.Add(new AnimationEntry("Idle", 0, 1.0f, 0xFFFF, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));
            Entry.Animations.Add(new AnimationEntry("Walking", 0, 1.0f, 0xFFFF, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));

            for (int i = 0; i < 8; i++)
                Entry.Textures.Add(new List<TextureEntry>());

            EditedFile.Entries.Add(Entry);
            DataGrid_NPCs.Rows.Add(new object[] { EditedFile.Entries.Count - 1, Entry.NPCName });
        }

        private void Button_Duplicate_Click(object sender, EventArgs e)
        {
            if (DataGrid_NPCs.SelectedRows.Count == 0)
                return;

            string Obj = JsonConvert.SerializeObject(SelectedEntry, Formatting.Indented);
            NPCEntry Entry = JsonConvert.DeserializeObject<NPCEntry>(Obj);
            EditedFile.Entries.Add(Entry);
            DataGrid_NPCs.Rows.Add(new object[] { EditedFile.Entries.Count - 1, Entry.NPCName });
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            if (DataGrid_NPCs.SelectedRows.Count == 0)
                return;

            if (DataGrid_NPCs.SelectedRows[0].Index == EditedFile.Entries.Count - 1)
            {
                EditedFile.Entries.RemoveAt(SelectedIndex);
                DataGrid_NPCs.Rows.RemoveAt(SelectedIndex);
            }
            else
            {
                SelectedEntry = new NPCEntry
                {
                    IsNull = true
                };
                EditedFile.Entries[SelectedIndex] = SelectedEntry;
                DataGrid_NPCs.Rows[SelectedIndex].Cells[1].Value = "Null";
                DataGrid_NPCs_SelectionChanged(this, null);
            }
        }

        private void DataGrid_NPCs_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGrid_NPCs.SelectedRows.Count != 0)
            {
                SelectedIndex = DataGrid_NPCs.SelectedRows[0].Index;
                SelectedEntry = EditedFile.Entries[SelectedIndex];
            }
            else
            {
                SelectedIndex = -1;
                SelectedEntry = null;
            }

            if (SelectedEntry == null || SelectedEntry.IsNull)
                Panel_NPCData.Enabled = false;
            else
            {
                Panel_NPCData.Enabled = true;
                InsertDataToEditor();
            }
        }

        private void DataGrid_NPCs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGrid_NPCs.SelectedRows.Count != 0 &&
                EditedFile.Entries[DataGrid_NPCs.SelectedRows[0].Index].IsNull)
            {
                SelectedEntry = new NPCEntry
                {
                    IsNull = false
                };
                EditedFile.Entries[SelectedIndex] = SelectedEntry;
                DataGrid_NPCs.Rows[SelectedIndex].Cells[1].Value = "";
                DataGrid_NPCs_SelectionChanged(this, null);
            }
        }

        #endregion

        private void Textbox_NPCName_TextChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as TextBox).Tag, (sender as TextBox).Text);
            DataGrid_NPCs.Rows[SelectedIndex].Cells[1].Value = Textbox_NPCName.Text;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as TextBox).Tag, (sender as TextBox).Text);
        }

        private void NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as NumericUpDown).Tag, (sender as NumericUpDown).Value);
        }

        private void CheckBox_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as CheckBox).Tag, (sender as CheckBox).Checked);
        }

        private void ComboBox_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as ComboBox).Tag, (sender as ComboBox).SelectedIndex);
        }

        private void Textbox_Script_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedEntry == null)
                return;
            SelectedEntry.Script = (sender as FastColoredTextBox).Text;

            FCTB.ApplySyntaxHighlight(sender, e, SyntaxHighlighting);
        }

        private void AddBlankDList(int Index)
        {
            SelectedEntry.DLists.Add(new DListEntry("DList_" + Index, 0, 0, 0, 0, 0, 0, 0, 0.01f, 0, 0, 0));
            DataGridView_ExtraDLists.Rows[Index].Cells[0].Value = "DList_" + Index;
            DataGridView_ExtraDLists.Rows[Index].Cells[1].Value = 0;
            DataGridView_ExtraDLists.Rows[Index].Cells[2].Value = "0, 0, 0";
            DataGridView_ExtraDLists.Rows[Index].Cells[3].Value = "0, 0, 0";
            DataGridView_ExtraDLists.Rows[Index].Cells[4].Value = "1";
            DataGridView_ExtraDLists.Rows[Index].Cells[5].Value = 0;
            DataGridView_ExtraDLists.Rows[Index].Cells[6].Value = -1;
            DataGridView_ExtraDLists.Rows[Index].Cells[7].Value = ExtraDlists_ShowType.Items[0];
        }

        private void SetDlist(DataGridView Grid, int RowIndex, int CellIndex, object CellValue)
        {
            Grid.Rows[RowIndex].Cells[0].Value = "DList_" + RowIndex.ToString();
            Grid.Rows[RowIndex].Cells[1].Value = 0;
            Grid.Rows[RowIndex].Cells[2].Value = "0, 0, 0";
            Grid.Rows[RowIndex].Cells[3].Value = "0, 0, 0";
            Grid.Rows[RowIndex].Cells[4].Value = 1.0f;
            Grid.Rows[RowIndex].Cells[5].Value = 0;
            Grid.Rows[RowIndex].Cells[6].Value = -1;
            Grid.Rows[RowIndex].Cells[7].Value = ExtraDlists_ShowType.Items[0];

            Grid.Rows[RowIndex].Cells[CellIndex].Value = CellValue;
        }

        private void DataGridView_ExtraDLists_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == 0)     // Purpose
            {
                if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                {
                    SelectedEntry.DLists.Add(new DListEntry(e.Value.ToString(), 0, 0, 0, 0, 0, 0, 0, 1.0f, 0, 0, -1));
                    SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, e.Value.ToString());
                }
                else
                    SelectedEntry.DLists[e.RowIndex].Name = e.Value.ToString();

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 1)    // Offset
            {
                try
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.DLists.Add(new DListEntry("DList_" + e.RowIndex.ToString(), Convert.ToUInt32(e.Value.ToString(), 16), 0, 0, 0, 0, 0, 0, 1.0f, 0, 0, -1));
                        SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, Convert.ToUInt32(e.Value.ToString(), 16));
                    }
                    else
                        SelectedEntry.DLists[e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);

                    e.ParsingApplied = true;
                    return;
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankDList(e.RowIndex);

                    e.Value = 0;
                    e.ParsingApplied = true;
                    return;
                }
            }
            else if (e.ColumnIndex == 2)    // XYZ Transl Offs
            {
                string[] Split = e.Value.ToString().Split(',');

                float X, Y, Z;

                try
                {
                    X = (float)Convert.ToDecimal(Split[0]);
                    Y = (float)Convert.ToDecimal(Split[1]);
                    Z = (float)Convert.ToDecimal(Split[2]);
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankDList(e.RowIndex);

                    e.Value = "0,0,0";
                    e.ParsingApplied = true;
                    return;
                }


                if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                {
                    SelectedEntry.DLists.Add(new DListEntry("DList_" + e.RowIndex.ToString(), 0, X, Y, Z, 0, 0, 0, 1.0f, 0, 0, -1));
                    SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, e.Value.ToString());
                }
                else
                {
                    SelectedEntry.DLists[e.RowIndex].TransX = X;
                    SelectedEntry.DLists[e.RowIndex].TransY = Y;
                    SelectedEntry.DLists[e.RowIndex].TransZ = Z;
                }

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 3) // XYZ Rot 
            {
                string[] Split = e.Value.ToString().Split(',');

                Int16 X, Y, Z;

                try
                {
                    X = Convert.ToInt16(Split[0]);
                    Y = Convert.ToInt16(Split[1]);
                    Z = Convert.ToInt16(Split[2]);
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankDList(e.RowIndex);

                    e.Value = "0,0,0";
                    e.ParsingApplied = true;
                    return;
                }


                if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                {
                    SelectedEntry.DLists.Add(new DListEntry("DList_" + e.RowIndex.ToString(), 0, 0, 0, 0, X, Y, Z, 1.0f, 0, 0, -1));
                    SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, e.Value.ToString());
                }
                else
                {
                    SelectedEntry.DLists[e.RowIndex].RotX = X;
                    SelectedEntry.DLists[e.RowIndex].RotY = Y;
                    SelectedEntry.DLists[e.RowIndex].RotZ = Z;
                }

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 4) // Scale
            {
                try
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.DLists.Add(new DListEntry("DList_" + e.RowIndex.ToString(), 0, 0, 0, 0, 0, 0, 0, (float)Convert.ToDecimal(e.Value), 0, 0, -1));
                        SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, (float)Convert.ToDecimal(e.Value));
                    }
                    else
                    {
                        SelectedEntry.DLists[e.RowIndex].Scale = (float)Convert.ToDecimal(e.Value);
                    }
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankDList(e.RowIndex);

                    e.Value = 0;
                    e.ParsingApplied = true;
                    return;
                }

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 5)  // Limb
            {
                try
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.DLists.Add(new DListEntry("DList_" + e.RowIndex.ToString(), 0, 0, 0, 0, 0, 0, 0, 1.0f, Convert.ToUInt16(e.Value), 0, -1));
                        SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, Convert.ToUInt16(e.Value));
                    }
                    else
                    {
                        SelectedEntry.DLists[e.RowIndex].Limb = Convert.ToUInt16(e.Value);
                    }
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankDList(e.RowIndex);

                    e.Value = 0;
                    e.ParsingApplied = true;
                    return;
                }

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 6)  // ObjectID
            {
                try
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.DLists.Add(new DListEntry("DList_" + e.RowIndex.ToString(), 0, 0, 0, 0, 0, 0, 0, 1.0f, 0, 0, Convert.ToInt16(e.Value)));
                        SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, Convert.ToInt32(e.Value));
                    }
                    else
                    {
                        SelectedEntry.DLists[e.RowIndex].ObjectID = Convert.ToInt16(e.Value);
                    }
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankDList(e.RowIndex);

                    e.Value = 0;
                    e.ParsingApplied = true;
                    return;
                }

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 7)  // Showtype
            {
                int ShowType;

                switch (e.Value.ToString())
                {
                    case "Don't show": ShowType = 0; break;
                    case "Alongside limb": ShowType = 1; break;
                    case "Instead of limb": ShowType = 2; break;
                    default: ShowType = 0; break;
                }

                if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                {
                    SelectedEntry.DLists.Add(new DListEntry("DList_" + e.RowIndex.ToString(), 0, 0, 0, 0, 0, 0, 0, 1.0f, 0, ShowType, -1));
                    SetDlist((sender as DataGridView), e.RowIndex, e.ColumnIndex, ShowType);
                }
                else
                {
                    SelectedEntry.DLists[e.RowIndex].ShowType = ShowType;
                }

                e.ParsingApplied = true;
                return;
            }
        }

        private void DataGridViewTextures_CellParse(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex > 31)
            {
                MessageBox.Show("Cannot define more than 32 textures per segment.");
                (sender as DataGridView).Rows.RemoveAt(e.RowIndex);
                e.ParsingApplied = true;
                return;
            }

            int DataGridIndex = TabControl_Textures.SelectedIndex;

            if (e.ColumnIndex == 0)
            {
                if (SelectedEntry.Textures[DataGridIndex].Count() - 1 < e.RowIndex)
                {
                    SelectedEntry.Textures[DataGridIndex].Add(new TextureEntry(e.Value.ToString(), 0, -1));
                    (sender as DataGridView).Rows[e.RowIndex].Cells[1].Value = 0;
                }
                else
                    SelectedEntry.Textures[DataGridIndex][e.RowIndex].Name = e.Value.ToString();

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 1)
            {
                try
                {
                    if (SelectedEntry.Textures[DataGridIndex].Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Textures[DataGridIndex].Add(new TextureEntry("Texture_" + e.RowIndex.ToString(), Convert.ToUInt32(e.Value.ToString(), 16), -1));
                        (sender as DataGridView).Rows[e.RowIndex].Cells[0].Value = "Texture_" + e.RowIndex.ToString();
                    }
                    else
                        SelectedEntry.Textures[DataGridIndex][e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);

                    e.ParsingApplied = true;
                    return;
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Textures[DataGridIndex].Add(new TextureEntry("Texture_" + e.RowIndex.ToString(), 0, -1));
                        (sender as DataGridView).Rows[e.RowIndex].Cells[0].Value = "Texture_" + e.RowIndex.ToString();
                    }

                    e.Value = Convert.ToInt32("0", 16);
                    e.ParsingApplied = true;
                }
            }
            else if (e.ColumnIndex == 2)
            {
                try
                {
                    if (SelectedEntry.Textures[DataGridIndex].Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Textures[DataGridIndex].Add(new TextureEntry("Texture_" + e.RowIndex.ToString(), 0, Convert.ToInt32(e.Value)));
                        (sender as DataGridView).Rows[e.RowIndex].Cells[0].Value = "Texture_" + e.RowIndex.ToString();
                    }
                    else
                        SelectedEntry.Textures[DataGridIndex][e.RowIndex].ObjectID = Convert.ToInt32(e.Value);

                    e.ParsingApplied = true;
                    return;
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Textures[DataGridIndex].Add(new TextureEntry("Texture_" + e.RowIndex.ToString(), 0, -1));
                        (sender as DataGridView).Rows[e.RowIndex].Cells[0].Value = "Texture_" + e.RowIndex.ToString();
                    }

                    e.Value = Convert.ToInt32("-1");
                    e.ParsingApplied = true;
                }
            }
        }

        private void AddBlankTex(int Index)
        {
            SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + Index.ToString(), 0, 1.0f, (UInt16)NumUpDown_ObjectID.Value, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }));
            DataGrid_Animations.Rows[Index].Cells[1].Value = 0;
            DataGrid_Animations.Rows[Index].Cells[2].Value = 1.0;
            DataGrid_Animations.Rows[Index].Cells[3].Value = "---";
        }

        private void DataGridViewAnimations_CellParse(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                {
                    SelectedEntry.Animations.Add(new AnimationEntry(e.Value.ToString(), 0, 1.0f, (UInt16)NumUpDown_ObjectID.Value, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));
                    DataGrid_Animations.Rows[e.RowIndex].Cells[1].Value = 0;
                    DataGrid_Animations.Rows[e.RowIndex].Cells[2].Value = "";
                    DataGrid_Animations.Rows[e.RowIndex].Cells[3].Value = 1.0;
                    DataGrid_Animations.Rows[e.RowIndex].Cells[4].Value = "---";
                }
                else
                    SelectedEntry.Animations[e.RowIndex].Name = e.Value.ToString();

                e.ParsingApplied = true;
                return;
            }
            else if (e.ColumnIndex == 1)
            {
                try
                {
                    if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), Convert.ToUInt32(e.Value.ToString(), 16), 1.0f, (UInt16)NumUpDown_ObjectID.Value, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));
                        DataGrid_Animations.Rows[e.RowIndex].Cells[0].Value = "Animation_" + e.RowIndex.ToString();
                        DataGrid_Animations.Rows[e.RowIndex].Cells[2].Value = "";
                        DataGrid_Animations.Rows[e.RowIndex].Cells[3].Value = 1.0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[4].Value = "---";
                    }
                    else
                    {
                        SelectedEntry.Animations[e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);
                    }

                    e.ParsingApplied = true;
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankTex(e.RowIndex);

                    e.Value = Convert.ToInt32("0", 16);
                    e.ParsingApplied = true;
                }
            }
            else if (e.ColumnIndex == 2)
            {
                try
                {
                    string[] Values = e.Value.ToString().Split(',');
                    byte[] Array = new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF };

                    if (Array.Count() > 4)
                    {
                        MessageBox.Show("Interpolation mode supports only up to 4 animations.");
                        throw new Exception();
                    }

                    int i = 0;

                    foreach (string Val in Values)
                    {
                        if (Val == "")
                            continue;

                        if (Convert.ToSByte(Val) >= 0)
                        {
                            Array[i] = Convert.ToByte(Val);
                            i++;
                        }
                    }

                    if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), 0, 1.0f, (UInt16)NumUpDown_ObjectID.Value, Array));
                        DataGrid_Animations.Rows[e.RowIndex].Cells[0].Value = "Animation_" + e.RowIndex.ToString();
                        DataGrid_Animations.Rows[e.RowIndex].Cells[1].Value = 0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[3].Value = 1.0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[4].Value = "---";
                    }
                    else
                        SelectedEntry.Animations[e.RowIndex].Frames = Array;

                    e.ParsingApplied = true;
                    return;
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankTex(e.RowIndex);

                    e.Value = "";
                    e.ParsingApplied = true;
                }
            }
            else if (e.ColumnIndex == 3)
            {
                try
                {
                    if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), 0, (float)Convert.ToDecimal(e.Value), (UInt16)NumUpDown_ObjectID.Value, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));
                        DataGrid_Animations.Rows[e.RowIndex].Cells[0].Value = "Animation_" + e.RowIndex.ToString();
                        DataGrid_Animations.Rows[e.RowIndex].Cells[1].Value = 0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[2].Value = "";
                        DataGrid_Animations.Rows[e.RowIndex].Cells[4].Value = "---";
                    }
                    else
                    {
                        SelectedEntry.Animations[e.RowIndex].Speed = (float)Convert.ToDecimal(e.Value.ToString());
                    }

                    e.ParsingApplied = true;
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankTex(e.RowIndex);

                    e.Value = 1.0f;
                    e.ParsingApplied = true;
                }
            }
            else if (e.ColumnIndex == 4)
            {
                try
                {
                    if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                    {
                        if (e.Value.ToString() == "---" || e.Value.ToString() == "")
                            SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), 0, 1.0f, UInt16.MaxValue, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));
                        else
                            SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), 0, 1.0f, Convert.ToUInt16(e.Value), new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));

                        DataGrid_Animations.Rows[e.RowIndex].Cells[0].Value = "Animation_" + e.RowIndex.ToString();
                        DataGrid_Animations.Rows[e.RowIndex].Cells[1].Value = 0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[2].Value = "";
                        DataGrid_Animations.Rows[e.RowIndex].Cells[3].Value = 1.0;
                    }
                    else
                    {
                        if (e.Value.ToString() == "---" || e.Value.ToString() == "")
                            SelectedEntry.Animations[e.RowIndex].ObjID = UInt16.MaxValue;
                        else
                            SelectedEntry.Animations[e.RowIndex].ObjID = Convert.ToUInt16(e.Value.ToString());
                    }

                    e.ParsingApplied = true;
                }
                catch (Exception)
                {
                    if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                        AddBlankTex(e.RowIndex);

                    e.Value = "---";
                    e.ParsingApplied = true;
                }
            }
        }

        private void Button_TryParse_Click(object sender, EventArgs e)
        {
            string[] Lines = Textbox_Script.Text.Replace(";", Environment.NewLine).Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range(Textbox_Script, 0, 0, Textbox_Script.Text.Length, Lines.Length);
            r.ClearStyle(FCTB.ErrorStyle);


            NewScriptParser.ScriptParser Parser = new NPC_Maker.NewScriptParser.ScriptParser(SelectedEntry, SelectedEntry.Script);
            Textbox_ParseErrors.Clear();

            NewScriptParser.BScript Output = Parser.ParseScript();

#if DEBUG

            Debug Dbg = new Debug(String.Join(Environment.NewLine, Output.ScriptDebug.ToArray()));
            Dbg.Show();

#endif



            if (Output.ParseErrors.Count() == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
            {
                foreach (NewScriptParser.ParseException Error in Output.ParseErrors)
                {
                    Textbox_ParseErrors.Text += Error.ToString() + Environment.NewLine;
                }
            }
        }

        private void DataGridView_ExtraDLists_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    int Index = (sender as DataGridView).SelectedCells[0].RowIndex;
                    (sender as DataGridView).Rows.RemoveAt(Index);
                    SelectedEntry.DLists.RemoveAt(Index);


                }
            }
            catch (Exception)
            {

            }
        }

        private void DataGridViewTextures_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    int DataGridIndex = TabControl_Textures.SelectedIndex;
                    int Index = (sender as DataGridView).SelectedCells[0].RowIndex;
                    (sender as DataGridView).Rows.RemoveAt(Index);
                    SelectedEntry.Textures[DataGridIndex].RemoveAt(Index);
                }
            }
            catch (Exception)
            {

            }
        }

        private void DataGrid_Animations_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if ((sender as DataGridView).SelectedCells[0].RowIndex > 1)
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        int Index = (sender as DataGridView).SelectedCells[0].RowIndex;
                        (sender as DataGridView).Rows.RemoveAt(Index);
                        SelectedEntry.Animations.RemoveAt(Index);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void Button_EnvironmentColorPreview_Click(object sender, EventArgs e)
        {
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                Button_EnvironmentColorPreview.BackColor = ColorDialog.Color;
                SelectedEntry.EnvColor = ColorDialog.Color;

                if (Checkbox_EnvColor.Checked)
                    SelectedEntry.EnvColor = Color.FromArgb(255, ColorDialog.Color.R, ColorDialog.Color.G, ColorDialog.Color.B);
                else
                    SelectedEntry.EnvColor = Color.FromArgb(0, ColorDialog.Color.R, ColorDialog.Color.G, ColorDialog.Color.B);
            }

        }

        private void Checkbox_EnvColor_CheckedChanged(object sender, EventArgs e)
        {
            if (Checkbox_EnvColor.Checked)
                SelectedEntry.EnvColor = Color.FromArgb(255, SelectedEntry.EnvColor.R, SelectedEntry.EnvColor.G, SelectedEntry.EnvColor.B);
            else
                SelectedEntry.EnvColor = Color.FromArgb(0, SelectedEntry.EnvColor.R, SelectedEntry.EnvColor.G, SelectedEntry.EnvColor.B);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About Window = new About();
            Window.ShowDialog();

        }

        private void Textbox_Script_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                LastRightClickedTextbox = (sender as Control);
                ContextMenuStrip.Show(sender as Control, e.Location);
            }
        }

        private void Tsmi_DoubleClick(object sender, EventArgs e)
        {
            InsertTxtToScript((sender as ToolStripItem).Text + " ");
        }

        private void Tsmi_Click(object sender, EventArgs e)
        {
            InsertTxtToScript((sender as ToolStripItem).Text + " ");
        }

        private void SubItem_Click(object sender, EventArgs e)
        {
            InsertTxtToScript((sender as ToolStripItem).Text);
        }

        private void SoundEffectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList("SFX.csv");
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(SFX.Chosen);
            }
        }

        private void MusicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList("Music.csv");
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(SFX.Chosen);
            }
        }

        private void FileMenu_Click(object sender, EventArgs e)
        {
            //hack
            NumUpDown_Hierarchy.Focus();
        }

        private void Button_TryParse2_Click(object sender, EventArgs e)
        {
            string[] Lines = Textbox_Script2.Text.Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range(Textbox_Script2, 0, 0, Textbox_Script2.Text.Length, Lines.Length);
            r.ClearStyle(FCTB.ErrorStyle);

            ScriptParser Parser = new ScriptParser();

            Parser.Parse(SelectedEntry, SelectedEntry.Script2);
            SelectedEntry.ParseErrors2 = Parser.ParseErrors;

            Textbox_ParseErrors2.Text = "";

            if (Parser.ParseErrors.Count == 0)
                Textbox_ParseErrors2.Text = "Parsed successfully!";
            else
            {
                foreach (string Error in Parser.ParseErrors)
                    Textbox_ParseErrors2.Text += Error + Environment.NewLine;
            }

            Textbox_Script2.Focus();

        }

        private void Textbox_Script2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedEntry == null)
                return;
            SelectedEntry.Script2 = (sender as FastColoredTextBox).Text;

            FCTB.ApplySyntaxHighlight(sender, e, SyntaxHighlighting);
        }

        private void SyntaxHighlightingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            SyntaxHighlighting = (sender as ToolStripMenuItem).Checked;

            Textbox_Script.Text += " ";
            Textbox_Script2.Text += " ";
        }

        private void ComboBox_AnimType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox_ValueChanged(sender, e);
            Col_OBJ.Visible = (ComboBox_AnimType.SelectedIndex == 0);
        }

        private void actorstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList Actors = new PickableList("Actors.csv");
            DialogResult DR = Actors.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(Actors.Chosen);
            }
        }

        /*      
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if ((sender as DataGridView).SelectedCells[0].RowIndex > -1)
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        int Index = (sender as DataGridView).SelectedCells[0].RowIndex;
                        (sender as DataGridView).Rows.RemoveAt(Index);
                        SelectedEntry.Colors.RemoveAt(Index);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style =
                        new DataGridViewCellStyle()
                        {
                            SelectionForeColor = ColorDialog.Color,
                            BackColor = ColorDialog.Color,
                            SelectionBackColor = ColorDialog.Color

                        };

                    if (SelectedEntry.Colors.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Colors.Add(new ColorEntry("", ColorDialog.Color));
                        dataGridView1.Rows[e.RowIndex].Cells[0].Value = "";
                    }
                    else
                    {
                        SelectedEntry.Colors[e.RowIndex].Color = ColorDialog.Color;
                    }
                }
            }
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (SelectedEntry.Colors.Count() - 1 < e.RowIndex)
                {
                    Color White = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                    SelectedEntry.Colors.Add(new ColorEntry(e.Value.ToString(), White));

                    try
                    {
                        SelectedEntry.ParseColorEntries();
                    }
                    catch
                    {
                        SelectedEntry.Colors[e.RowIndex].Limbs = "";
                        e.Value = "";
                    }

                    dataGridView1.Rows[e.RowIndex].Cells[1].Style =
                        new DataGridViewCellStyle()
                        {
                            SelectionForeColor = White,
                            BackColor = White,
                            SelectionBackColor = White

                        };
                }
                else
                    SelectedEntry.Colors[e.RowIndex].Limbs = e.Value.ToString();

                e.ParsingApplied = true;
                return;
            }
        }
        */

    }
}
