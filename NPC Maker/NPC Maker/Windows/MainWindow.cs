﻿using System;
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

        public MainWindow()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.ResizeBegin += Form1_ResizeBegin;
            this.ResizeEnd += Form1_ResizeEnd;


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

        private void InsertDataIntoActorListGrid()
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
            Chkb_ReactIfAtt.Checked = SelectedEntry.ReactAttacked;
            ChkRunJustScript.Checked = SelectedEntry.JustScript;
            Chkb_Opendoors.Checked = SelectedEntry.OpenDoors;

            NumUpDown_XColOffs.Value = SelectedEntry.ColOffs[0];
            NumUpDown_YColOffs.Value = SelectedEntry.ColOffs[1];
            NumUpDown_ZColOffs.Value = SelectedEntry.ColOffs[2];

            Checkbox_Targettable.Checked = SelectedEntry.Targettable;
            ComboBox_TargetDist.SelectedIndex = SelectedEntry.TargetDist <= 10 ? SelectedEntry.TargetDist : 1;
            NumUpDown_TargetLimb.Value = SelectedEntry.TargetLimb;
            NumUpDown_XTargetOffs.Value = SelectedEntry.TargOffs[0];
            NumUpDown_YTargetOffs.Value = SelectedEntry.TargOffs[1];
            NumUpDown_ZTargetOffs.Value = SelectedEntry.TargOffs[2];
            NumUpDown_TalkRadi.Value = (decimal)SelectedEntry.TalkRadius;

            Combo_MovementType.SelectedIndex = SelectedEntry.MovementType;
            NumUpDown_MovDistance.Value = SelectedEntry.MovementDistance;
            NumUpDown_MovSpeed.Value = (decimal)SelectedEntry.MovementSpeed;
            NumUpDown_GravityForce.Value = (decimal)SelectedEntry.GravityForce;
            NumUpDown_LoopDelay.Value = SelectedEntry.LoopDel;
            NumUpDown_LoopEndNode.Value = SelectedEntry.LoopEnd;
            NumUpDown_LoopStartNode.Value = SelectedEntry.LoopStart;
            Checkbox_Loop.Checked = SelectedEntry.Loop;
            ChkBox_TimedPath.Checked = SelectedEntry.TimedPath;
            NumUpDown_PathFollowID.Value = SelectedEntry.PathID;
            tmpicker_timedPathStart.Value = Helpers.GetTimeFromOcarinaTime(SelectedEntry.TimedPathStart);
            tmpicker_timedPathEnd.Value = Helpers.GetTimeFromOcarinaTime(SelectedEntry.TimedPathEnd);

            ComboBox_AnimType.SelectedIndex = SelectedEntry.AnimationType;
            NumUpDown_Scale.Value = (decimal)SelectedEntry.Scale;

            foreach (TabPage Page in TabControl.TabPages)
            {
                if ((string)Page.Tag == "SCRIPT")
                    TabControl.TabPages.Remove(Page);
            }

            foreach (ScriptEntry ScriptT in SelectedEntry.Scripts)
            {
                TabPage Page = new TabPage(ScriptT.Name)
                {
                    Tag = "SCRIPT"
                };

                ScriptEditor Se = new ScriptEditor(ref SelectedEntry, ScriptT, syntaxHighlightingToolStripMenuItem.Checked)
                {
                    Dock = DockStyle.Fill
                };

                Page.Controls.Add(Se);
                TabControl.TabPages.Add(Page);
            }

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

                DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Animation.Address.ToString("X"), Frames, Animation.Speed, Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, Animation.ObjID, Dicts.ObjectIDs[-1]) });
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

            for (int j = 0; j < SelectedEntry.Segments.Count; j++)
            {
                DataGridView Grid = (TabControl_Segments.TabPages[j].Controls[0] as DataGridView);

                Grid.Rows.Clear();

                foreach (SegmentEntry Entry in SelectedEntry.Segments[j])
                    Grid.Rows.Add(Entry.Name, Entry.Address.ToString("X"), Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, Entry.ObjectID, Dicts.ObjectIDs[-1]) );
            }

            DataGridView_ExtraDLists.Rows.Clear();

            foreach (DListEntry Dlist in SelectedEntry.DLists)
            {
                string SelCombo = Dicts.GetStringFromIntStringDict(Dicts.LimbShowSubTypes, Dlist.ShowType, Dicts.LimbShowSubTypes[0]);

                DataGridView_ExtraDLists.Rows.Add(new object[] { Dlist.Name,
                                                                 Dlist.Address.ToString("X"),
                                                                 Dlist.TransX.ToString() + "," + Dlist.TransY.ToString() + "," + Dlist.TransZ.ToString(),
                                                                 Dlist.RotX.ToString() + "," + Dlist.RotY.ToString() + "," + Dlist.RotZ.ToString(),
                                                                 Dlist.Scale.ToString(),
                                                                 Dlist.Limb.ToString(),
                                                                 Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, Dlist.ObjectID, Dicts.ObjectIDs[-1]),
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
                    InsertDataIntoActorListGrid();
                }
            }

        }

        private void FileMenu_New_Click(object sender, EventArgs e)
        {
            if (SaveChangesAsPrompt() == false)
                return;

            EditedFile = new NPCFile();
            Panel_Editor.Enabled = true;
            InsertDataIntoActorListGrid();
        }

        private void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog
            {
                FileName = "ActorData.json",
                Filter = "Json Files | *.json"
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
                FileName = "zobj.zobj",
                Filter = "Zelda Object Files | *.zobj"
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
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About Window = new About();
            Window.ShowDialog();
        }

        private void SyntaxHighlightingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TabPage Page in TabControl.TabPages)
            {
                if ((string)Page.Tag == "SCRIPT")
                {
                    if (Page.Controls.Count != 0)
                        (Page.Controls[0] as ScriptEditor).SetSyntaxHighlighting((sender as ToolStripMenuItem).Checked);

                }
            }
        }

        private void FileMenu_Click(object sender, EventArgs e)
        {
            //hack
            NumUpDown_Hierarchy.Focus();
        }

        private string GetScriptName()
        {
            string ScriptName = "";
            DialogResult Dr = InputBox.ShowInputDialog("Script name?", ref ScriptName);

            if (Dr != DialogResult.OK)
                return "";
            else
                return ScriptName;
        }

        private void AddNewScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ScriptName = GetScriptName();

            if (SelectedEntry.Scripts.Count >= 255)
            {
                MessageBox.Show("Cannot define more than 255 scripts.");
                return;
            }

            TabPage Page = new TabPage(ScriptName)
            {
                Tag = "SCRIPT"
            };

            ScriptEntry Sc = new ScriptEntry() { Name = ScriptName, ParseErrors = new List<string>(), Text = "" };
            SelectedEntry.Scripts.Add(Sc);

            ScriptEditor Se = new ScriptEditor(ref SelectedEntry, Sc, syntaxHighlightingToolStripMenuItem.Checked)
            {
                Dock = DockStyle.Fill
            };

            Page.Controls.Add(Se);
            TabControl.TabPages.Add(Page);
        }

        private void RenameCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((string)TabControl.SelectedTab.Tag != "SCRIPT")
            {
                MessageBox.Show("Select a script tab first.");
                return;
            }

            string Name = GetScriptName();

            if (Name != "")
            {

                (TabControl.SelectedTab.Controls[0] as ScriptEditor).Script.Name = Name;
                TabControl.SelectedTab.Text = Name;
            }
        }

        private void DeleteCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((string)TabControl.SelectedTab.Tag != "SCRIPT")
            {
                MessageBox.Show("Select a script tab first.");
                return;
            }

            DialogResult Res = MessageBox.Show("Are you sure you want to delete this script? You cannot reverse this action!", "Removing a script", MessageBoxButtons.YesNoCancel);

            if (Res == DialogResult.Yes)
            {
                SelectedEntry.Scripts.Remove((TabControl.SelectedTab.Controls[0] as ScriptEditor).Script);
                TabControl.TabPages.Remove(TabControl.SelectedTab);
            }
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

                SelectedEntry.Segments.Clear();

                foreach (List<SegmentEntry> TexList in CopiedEntry.Segments)
                {
                    List<SegmentEntry> DestTexList = new List<SegmentEntry>();
                    SelectedEntry.Segments.Add(DestTexList);

                    foreach (SegmentEntry Tex in TexList)
                    {
                        DestTexList.Add(new SegmentEntry(Tex.Name, Tex.Address, Tex.ObjectID));
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
            Entry.Animations.Add(new AnimationEntry("Idle", 0, 1.0f, -1, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));
            Entry.Animations.Add(new AnimationEntry("Walking", 0, 1.0f, -1, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));
            Entry.Animations.Add(new AnimationEntry("Attacked", 0, 1.0f, -1, new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF }));

            for (int i = 0; i < 8; i++)
                Entry.Segments.Add(new List<SegmentEntry>());

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

        #region Field changes

        private void Textbox_NPCName_TextChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as TextBox).Tag, (sender as TextBox).Text);
            DataGrid_NPCs.Rows[SelectedIndex].Cells[1].Value = Textbox_NPCName.Text;
        }

        private void ComboBox_AnimType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox_ValueChanged(sender, e);
            Col_OBJ.Visible = (ComboBox_AnimType.SelectedIndex == 0);
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

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as DateTimePicker).Tag, (sender as DateTimePicker).Value.ToString("HH:mm"));
        }

        private void ComboBox_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((NPCEntry.Members)(sender as ComboBox).Tag, (sender as ComboBox).SelectedIndex);
        }

        #endregion

        #region Animation Grid

        private void AddBlankAnim(int SkipIndex, int Index, string Name = null, uint? Address = null, float? Speed = null, short? ObjectID = null, byte[] Frames = null)
        {
            Name = Name ?? "Animation_" + Index.ToString();
            Address = Address ?? 0;
            Speed = Speed ?? 0;
            ObjectID = ObjectID ?? -1;
            Frames = Frames ?? (new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });

            SelectedEntry.Animations.Add(new AnimationEntry(Name, (uint)Address, (float)Speed, (short)ObjectID, Frames));

            if (SkipIndex != 0)
                DataGrid_Animations.Rows[Index].Cells[0].Value = Name;

            if (SkipIndex != 1)
                DataGrid_Animations.Rows[Index].Cells[1].Value = Address;

            if (SkipIndex != 2)
                DataGrid_Animations.Rows[Index].Cells[2].Value = "";

            if (SkipIndex != 3)
                DataGrid_Animations.Rows[Index].Cells[3].Value = 1.0;

            if (SkipIndex != 4)
                DataGrid_Animations.Rows[Index].Cells[4].Value = Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, (int)ObjectID, Dicts.ObjectIDs[-1]);
        }

        private byte[] ConvertAnimationByteArrayString(string Value)
        {
            string[] Values = Value.Split(',');
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

            return Array;
        }

        private void DataGridViewAnimations_CellParse(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex > 255)
            {
                MessageBox.Show("Cannot define more than 255 animations.");
                (sender as DataGridView).Rows.RemoveAt(e.RowIndex);
                e.ParsingApplied = true;
                return;
            }

            switch (e.ColumnIndex)
            {
                case 0:     // Name
                    {
                        if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                            AddBlankAnim(e.ColumnIndex, e.RowIndex, e.Value.ToString());
                        else
                            SelectedEntry.Animations[e.RowIndex].Name = e.Value.ToString();

                        e.ParsingApplied = true;
                        return;
                    }
                case 1:     // Address
                    {
                        try
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, Convert.ToUInt32(e.Value.ToString(), 16));
                            else
                                SelectedEntry.Animations[e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);

                            e.ParsingApplied = true;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex);

                            e.Value = Convert.ToInt32("0", 16);
                            e.ParsingApplied = true;
                        }
                        return;
                    }
                case 2:     // Keyframes
                    {
                        try
                        {
                            byte[] Array = ConvertAnimationByteArrayString(e.Value.ToString());

                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, null, Array);
                            else
                                SelectedEntry.Animations[e.RowIndex].Frames = Array;

                            e.ParsingApplied = true;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex);

                            e.Value = "";
                            e.ParsingApplied = true;
                        }
                        return;
                    }
                case 3:     // Speed
                    {
                        try
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, (float)Convert.ToDecimal(e.Value));
                            else
                                SelectedEntry.Animations[e.RowIndex].Speed = (float)Convert.ToDecimal(e.Value);

                            e.ParsingApplied = true;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex);

                            e.Value = 1.0f;
                            e.ParsingApplied = true;
                        }
                        return;
                    }
                case 4:     // Object
                    {
                        try
                        {
                            int ObjectId = Dicts.GetIntFromIntStringDict(Dicts.ObjectIDs, e.Value.ToString()); 
                               
                            e.Value = Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, ObjectId, Dicts.ObjectIDs[-1]);

                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, (short)ObjectId);
                            else
                                SelectedEntry.Animations[e.RowIndex].ObjID = (short)ObjectId;

                            e.ParsingApplied = true;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex);

                            e.Value = Dicts.ObjectIDs[-1];
                        }

                        e.ParsingApplied = true;
                        return;
                    }
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

        #endregion

        #region Extra Display Lists Grid

        private void AddBlankDList(int SkipIndex, int Index, string Name = null, uint? Address = null, float? TransX = null, float? TransY = null, float? TransZ = null,
                                   short? RotX = null, short? RotY = null, short? RotZ = null, float? Scale = null, ushort? Limb = null, int? ShowType = null, short? ObjectID = null)
        {
            if (Name == null)
                Name = "DList_" + Index;

            Name = Name ?? "DList_" + Index;
            Address = Address ?? 0;
            TransX = TransX ?? 0;
            TransY = TransY ?? 0;
            TransZ = TransZ ?? 0;
            RotX = RotX ?? 0;
            RotY = RotY ?? 0;
            RotZ = RotZ ?? 0;
            Scale = Scale ?? 0.01f;
            Limb = Limb ?? 0;
            ShowType = ShowType ?? 0;
            ObjectID = ObjectID ?? -1;

            SelectedEntry.DLists.Add(new DListEntry(Name, (uint)Address, (float)TransX, (float)TransY, (float)TransZ,
                                                    (short)RotX, (short)RotY, (short)RotZ, (float)Scale, (ushort)Limb, (int)ShowType, (short)ObjectID));

            if (SkipIndex != 0)
                DataGridView_ExtraDLists.Rows[Index].Cells[0].Value = Name;

            if (SkipIndex != 1)
                DataGridView_ExtraDLists.Rows[Index].Cells[1].Value = Address;

            if (SkipIndex != 2)
                DataGridView_ExtraDLists.Rows[Index].Cells[2].Value = $"{TransX},{TransY},{TransZ}";

            if (SkipIndex != 3)
                DataGridView_ExtraDLists.Rows[Index].Cells[3].Value = $"{RotX},{RotY},{RotZ}";

            if (SkipIndex != 4)
                DataGridView_ExtraDLists.Rows[Index].Cells[4].Value = Scale;

            if (SkipIndex != 5)
                DataGridView_ExtraDLists.Rows[Index].Cells[5].Value = Limb;

            if (SkipIndex != 6)
                DataGridView_ExtraDLists.Rows[Index].Cells[6].Value = ObjectID == -1 ? "---" : ObjectID.ToString();

            if (SkipIndex != 7)
                DataGridView_ExtraDLists.Rows[Index].Cells[7].Value = ExtraDlists_ShowType.Items[(int)ShowType];
        }

        private float[] GetXYZTranslation(string Value)
        {
            string[] Split = Value.Split(',');
            float[] Values = new float[3] { 0, 0, 0 };

            try
            {
                Values[0] = (float)Convert.ToDecimal(Split[0]);
                Values[1] = (float)Convert.ToDecimal(Split[1]);
                Values[2] = (float)Convert.ToDecimal(Split[2]);
            }
            catch (Exception)
            {
                Values = new float[3] { 0, 0, 0 };
            }

            return Values;
        }

        private short[] GetXYZRotation(string Value)
        {
            string[] Split = Value.Split(',');
            short[] Values = new short[3] { 0, 0, 0 };

            try
            {
                Values[0] = (short)Convert.ToUInt16(Split[0]);
                Values[1] = (short)Convert.ToUInt16(Split[1]);
                Values[2] = (short)Convert.ToUInt16(Split[2]);
            }
            catch (Exception)
            {
                Values = new short[3] { 0, 0, 0 };
            }

            return Values;
        }

        private void DataGridView_ExtraDLists_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex > 255)
            {
                MessageBox.Show("Cannot define more than 255 extra display lists.");
                (sender as DataGridView).Rows.RemoveAt(e.RowIndex);
                e.ParsingApplied = true;
                return;
            }

            switch (e.ColumnIndex)
            {
                case 0:     // Purpose
                    {
                        if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, e.Value.ToString());
                        else
                            SelectedEntry.DLists[e.RowIndex].Name = e.Value.ToString();

                        e.ParsingApplied = true;
                        return;
                    }
                case 1:     // Offset
                    {
                        try
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, Convert.ToUInt32(e.Value.ToString(), 16));
                            else
                                SelectedEntry.DLists[e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = 0;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case 2:     // XYZ Translation
                    {
                        float[] Transl = GetXYZTranslation(e.Value.ToString());
                        e.Value = $"{Transl[0]},{Transl[1]},{Transl[2]}";

                        if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, Transl[0], Transl[1], Transl[2]);
                        else
                        {
                            SelectedEntry.DLists[e.RowIndex].TransX = Transl[0];
                            SelectedEntry.DLists[e.RowIndex].TransY = Transl[1];
                            SelectedEntry.DLists[e.RowIndex].TransZ = Transl[2];
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case 3:     // XYZ Rotation
                    {
                        short[] Rot = GetXYZRotation(e.Value.ToString());
                        e.Value = $"{Rot[0]},{Rot[1]},{Rot[2]}";

                        if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, Rot[0], Rot[1], Rot[2]);
                        else
                        {
                            SelectedEntry.DLists[e.RowIndex].RotX = Rot[0];
                            SelectedEntry.DLists[e.RowIndex].RotY = Rot[1];
                            SelectedEntry.DLists[e.RowIndex].RotZ = Rot[2];
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case 4:     // Scale
                    {
                        try
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, (float)Convert.ToDecimal(e.Value));
                            else
                                SelectedEntry.DLists[e.RowIndex].Scale = (float)Convert.ToDecimal(e.Value);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = 0;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case 5:     // Limb
                    {
                        try
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, Convert.ToUInt16(e.Value));
                            else
                                SelectedEntry.DLists[e.RowIndex].Limb = Convert.ToUInt16(e.Value);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = 0;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case 6:     // Object ID
                    {
                        try
                        {
                            int ObjectId = Dicts.GetIntFromIntStringDict(Dicts.ObjectIDs, e.Value.ToString());

                            e.Value = Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, ObjectId, Dicts.ObjectIDs[-1]);

                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, (short)ObjectId);
                            else
                                SelectedEntry.DLists[e.RowIndex].ObjectID = Convert.ToInt16(e.Value);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = Dicts.ObjectIDs[-1];
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case 7:     // ShowType
                    {
                        int ShowType = Dicts.GetIntFromIntStringDict(Dicts.LimbShowSubTypes, e.Value.ToString(), 0);

                        if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, ShowType);
                        else
                            SelectedEntry.DLists[e.RowIndex].ShowType = ShowType;

                        e.ParsingApplied = true;
                        return;
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

        #endregion

        #region Segments Grid

        private void AddBlankSeg(int SkipIndex, int Index, int Segment, string Name = null, uint? Address = null, short? ObjectID = null)
        {
            Name = Name ?? "Texture_" + Index.ToString();
            Address = Address ?? 0;
            ObjectID = ObjectID ?? -1;

            SelectedEntry.Segments[Segment].Add(new SegmentEntry(Name, (uint)Address, (short)ObjectID));

            DataGridView dgv = TabControl_Segments.TabPages[Segment].Controls[0] as DataGridView;

            if (SkipIndex != 0)
                dgv.Rows[Index].Cells[0].Value = Name;

            if (SkipIndex != 1)
                dgv.Rows[Index].Cells[1].Value = Address;

            if (SkipIndex != 2)
                dgv.Rows[Index].Cells[2].Value = Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, (int)ObjectID, Dicts.ObjectIDs[-1]);
        }

        private void DataGridViewSegments_CellParse(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex > 31)
            {
                MessageBox.Show("Cannot define more than 32 textures per segment.");
                (sender as DataGridView).Rows.RemoveAt(e.RowIndex);
                e.ParsingApplied = true;
                return;
            }

            int DataGridIndex = TabControl_Segments.SelectedIndex;

            switch (e.ColumnIndex)
            {
                case 0:     // Name
                    {
                        if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                            AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, e.Value.ToString());
                        else
                            SelectedEntry.Segments[DataGridIndex][e.RowIndex].Name = e.Value.ToString();

                        e.ParsingApplied = true;
                        return;
                    }
                case 1:     // Address
                    {
                        try
                        {
                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, Convert.ToUInt32(e.Value.ToString(), 16));
                            else
                                SelectedEntry.Segments[DataGridIndex][e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex);

                            e.Value = Convert.ToInt32("0", 16);
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case 2:     // Object
                    {
                        try
                        {
                            short ObjectId = (short)Dicts.GetIntFromIntStringDict(Dicts.ObjectIDs, e.Value.ToString());

                            e.Value = Dicts.GetStringFromIntStringDict(Dicts.ObjectIDs, ObjectId, Dicts.ObjectIDs[-1]);

                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, null, ObjectId);
                            else
                                SelectedEntry.Segments[DataGridIndex][e.RowIndex].ObjectID = ObjectId;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.DLists.Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex);

                            e.Value = Dicts.ObjectIDs[-1];
                        }

                        e.ParsingApplied = true;
                        return;
                    }
            }
        }

        private void DataGridViewSegments_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    int DataGridIndex = TabControl_Segments.SelectedIndex;
                    int Index = (sender as DataGridView).SelectedCells[0].RowIndex;
                    (sender as DataGridView).Rows.RemoveAt(Index);
                    SelectedEntry.Segments[DataGridIndex].RemoveAt(Index);
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Unused

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

        #endregion

    }
}