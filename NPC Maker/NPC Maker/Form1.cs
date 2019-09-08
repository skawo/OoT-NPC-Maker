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

namespace NPC_Maker
{
    public partial class Form1 : Form
    {
        string OpenedPath = "";
        NPCFile EditedFile = null;
        NPCEntry SelectedEntry = null;
        int SelectedIndex = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (EditedFile != null)
            {
                if (MessageBox.Show("Save changes?", "Save changes before opening a new file?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FileMenu_SaveAs_Click(this, null);
                }
            }
        }

        private void InsertDataIntoDataGridView()
        {
            DataGrid_NPCs.Rows.Clear();

            int i = 0;

            foreach (NPCEntry Entry in EditedFile.Entries)
            {
                DataGrid_NPCs.Rows.Add(new object[] { i.ToString(), Entry.IsNull ? "EMPTY" : Entry.NPCName });
                i++;
            }
        }

        private void InsertDataToEditor()
        {
            Textbox_NPCName.Text = SelectedEntry.NPCName;

            NumUpDown_ObjectID.Value = SelectedEntry.ObjectID;
            NumUpDown_Hierarchy.Value = SelectedEntry.Hierarchy;
            ComboBox_HierarchyType.SelectedIndex = SelectedEntry.HierarchyType;
            NumUpDown_XModelOffs.Value = SelectedEntry.ModelOffs[0];
            NumUpDown_YModelOffs.Value = SelectedEntry.ModelOffs[1];
            NumUpDown_ZModelOffs.Value = SelectedEntry.ModelOffs[2];
            NumUpDown_Scale.Value = (decimal)SelectedEntry.Scale;

            ComboBox_LookAtType.SelectedIndex = SelectedEntry.LookAtType;
            ComboBox_HeadLookAxis.SelectedIndex = SelectedEntry.HeadAxis;
            NumUpDown_DegVert.Value = SelectedEntry.DegreesVert;
            NumUpDown_DegHoz.Value = SelectedEntry.DegreesHor;
            NumUpDown_Limb.Value = SelectedEntry.LimbIndex;

            Checkbox_HaveCollision.Checked = SelectedEntry.Collision;
            Checkbox_DrawShadow.Checked = SelectedEntry.Shadow;
            Checkbox_CanPressSwitches.Checked = SelectedEntry.Switches;
            Checkbox_Pushable.Checked = SelectedEntry.Pushable;
            NumUpDown_ColRadius.Value = SelectedEntry.Radius;
            NumUpDown_ColHeight.Value = SelectedEntry.Height;
            NumUpDown_XColOffs.Value = SelectedEntry.ColOffs[0];
            NumUpDown_YColOffs.Value = SelectedEntry.ColOffs[1];
            NumUpDown_ZColOffs.Value = SelectedEntry.ColOffs[2];

            Checkbox_Targettable.Checked = SelectedEntry.Targettable;
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

            DataGrid_Animations.Rows.Clear();

            foreach (AnimationEntry Animation in SelectedEntry.Animations)
                DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Animation.Address.ToString("X"), Animation.Speed, Animation.ObjID == UInt16.MaxValue ? "---" : Animation.ObjID.ToString()});

            Textbox_ParseErrors.Text = "";

            if (SelectedEntry.ParseErrors.Count == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
            {
                foreach (string Error in SelectedEntry.ParseErrors)
                {
                    Textbox_ParseErrors.Text += Error + Environment.NewLine;
                    string Line = Error.Substring(Error.IndexOf("Line: ") + 6);
                    Line = Line.Substring(1, Line.Length - 2);
                    FCTB.ApplyError(Textbox_Script, Line);
                }
            }
        }

        #region MenuStrip

        private void SaveChangesAsPrompt()
        {
            if (EditedFile != null)
            {
                if (MessageBox.Show("Save changes before opening a new file?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FileMenu_SaveAs_Click(this, null);
                }
            }
        }

        private void FileMenu_Open_Click(object sender, EventArgs e)
        {
            SaveChangesAsPrompt();

            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();

            if (OFD.FileName != "")
            {
                EditedFile = FileOps.ParseJSONFile(OFD.FileName);

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
            SaveChangesAsPrompt();

            EditedFile = new NPCFile();
            Panel_Editor.Enabled = true;
            InsertDataIntoDataGridView();
        }

        private void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.FileName = "ActorData.json";
            SFD.ShowDialog();

            if (SFD.FileName != "")
            {
                FileOps.SaveJSONFile(SFD.FileName, EditedFile);
            }
        }

        private void FileMenu_Save_Click(object sender, EventArgs e)
        {
            if (OpenedPath == "")
                FileMenu_SaveAs_Click(this, null);
            else
                FileOps.SaveJSONFile(OpenedPath, EditedFile);
        }

        private void FileMenu_SaveBinary_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.FileName = "zobj.zobj";
            SFD.ShowDialog();

            if (SFD.FileName != "")
            {
                FileOps.SaveBinaryFile(SFD.FileName, EditedFile);
            }

            InsertDataToEditor();
        }

        private void FileMenu_Exit_Click(object sender, EventArgs e)
        {
            SaveChangesAsPrompt();
            Application.Exit();
        }

        #endregion

        #region NPCList

        private void Button_Add_Click(object sender, EventArgs e)
        {
            NPCEntry Entry = new NPCEntry();
            Entry.Animations.Add(new AnimationEntry("Idle", 0, 1.0f, 0xFFFF));
            Entry.Animations.Add(new AnimationEntry("Walking", 0, 1.0f, 0xFFFF));
            Entry.Animations.Add(new AnimationEntry("Start talking", 0, 1.0f, 0xFFFF));
            Entry.Animations.Add(new AnimationEntry("Talking", 0, 1.0f, 0xFFFF));

            EditedFile.Entries.Add(Entry);
            DataGrid_NPCs.Rows.Add(new object[] { EditedFile.Entries.Count - 1, Entry.NPCName });
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            if (DataGrid_NPCs.SelectedRows[0].Index == EditedFile.Entries.Count - 1)
            {
                EditedFile.Entries.RemoveAt(SelectedIndex);
                DataGrid_NPCs.Rows.RemoveAt(SelectedIndex);
            }
            else
            {
                SelectedEntry = new NPCEntry();
                SelectedEntry.IsNull = true;
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
                SelectedEntry = new NPCEntry();
                SelectedEntry.IsNull = false;
                DataGrid_NPCs.Rows[SelectedIndex].Cells[1].Value = "";
                DataGrid_NPCs_SelectionChanged(this, null);
            }
        }

        #endregion

        private void Textbox_NPCName_TextChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((sender as TextBox).Tag.ToString(), (sender as TextBox).Text);
            DataGrid_NPCs.Rows[SelectedIndex].Cells[1].Value = Textbox_NPCName.Text;
        }

        private void NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((sender as NumericUpDown).Tag.ToString(), (sender as NumericUpDown).Value);
        }

        private void CheckBox_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((sender as CheckBox).Tag.ToString(), (sender as CheckBox).Checked);
        }

        private void ComboBox_ValueChanged(object sender, EventArgs e)
        {
            SelectedEntry.ChangeValueOfMember((sender as ComboBox).Tag.ToString(), (sender as ComboBox).SelectedIndex);
        }

        private void Textbox_Script_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedEntry == null)
                return;
            SelectedEntry.Script = Textbox_Script.Text;
            FCTB.ApplySyntaxHighlight(sender, e);
        }

        private void DataGridViewAnimations_CellParse(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                {
                    SelectedEntry.Animations.Add(new AnimationEntry(e.Value.ToString(), 0, 1.0f, (UInt16)NumUpDown_ObjectID.Value));
                    DataGrid_Animations.Rows[e.RowIndex].Cells[1].Value = 0;
                    DataGrid_Animations.Rows[e.RowIndex].Cells[2].Value = 1.0;
                    DataGrid_Animations.Rows[e.RowIndex].Cells[3].Value = "---";
                }
                else
                    SelectedEntry.Animations[e.RowIndex].Name = e.Value.ToString();

                e.ParsingApplied = true;
                return;
            }
            if (e.ColumnIndex == 1)
            {
                try
                {
                    if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), Convert.ToUInt32(e.Value.ToString(), 16), 1.0f, (UInt16)NumUpDown_ObjectID.Value));
                        DataGrid_Animations.Rows[e.RowIndex].Cells[0].Value = "Animation_" + e.RowIndex.ToString();
                        DataGrid_Animations.Rows[e.RowIndex].Cells[2].Value = 1.0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[3].Value = "---";
                    }
                    else
                    {
                        SelectedEntry.Animations[e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);
                    }

                    e.ParsingApplied = true;
                }
                catch (Exception)
                {
                    e.Value = Convert.ToInt32("0", 16);
                    e.ParsingApplied = true;
                }
            }
            if (e.ColumnIndex == 2)
            {
                try
                {
                    if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), 0, (float)Convert.ToDecimal(e.Value), (UInt16)NumUpDown_ObjectID.Value));
                        DataGrid_Animations.Rows[e.RowIndex].Cells[0].Value = "Animation_" + e.RowIndex.ToString();
                        DataGrid_Animations.Rows[e.RowIndex].Cells[1].Value = 0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[3].Value = "---";
                    }
                    else
                    {
                        SelectedEntry.Animations[e.RowIndex].Speed = (float)Convert.ToDecimal(e.Value.ToString());
                    }

                    e.ParsingApplied = true;
                }
                catch (Exception)
                {
                    e.Value = 1.0f;
                    e.ParsingApplied = true;
                }
            }
            if (e.ColumnIndex == 3)
            {
                try
                {
                    if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                    {
                        if (e.Value.ToString() == "---")
                            SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), 0, 1.0f, UInt16.MaxValue));
                        else
                            SelectedEntry.Animations.Add(new AnimationEntry("Animation_" + e.RowIndex.ToString(), 0, 1.0f, Convert.ToUInt16(e.Value)));

                        DataGrid_Animations.Rows[e.RowIndex].Cells[0].Value = "Animation_" + e.RowIndex.ToString();
                        DataGrid_Animations.Rows[e.RowIndex].Cells[1].Value = 0;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[2].Value = 1.0;
                    }
                    else
                    {
                        SelectedEntry.Animations[e.RowIndex].ObjID = Convert.ToUInt16(e.Value.ToString());
                    }

                    e.ParsingApplied = true;
                }
                catch (Exception)
                {
                    e.Value = "---";
                    e.ParsingApplied = true;
                }
            }
        }

        private void Button_TryParse_Click(object sender, EventArgs e)
        {
            string[] Lines = Textbox_Script.Text.Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range(Textbox_Script, 0, 0, Textbox_Script.Text.Length, Lines.Length);
            r.ClearStyle(FCTB.ErrorStyle);

            ScriptParser Parser = new ScriptParser();

            Parser.Parse(Textbox_Script.Text, SelectedEntry.Animations);
            SelectedEntry.ParseErrors = Parser.ParseErrors;

            Textbox_ParseErrors.Text = "";

            if (Parser.ParseErrors.Count == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
            {
                foreach (string Error in Parser.ParseErrors)
                {
                    Textbox_ParseErrors.Text += Error + Environment.NewLine;
                    string Line = Error.Substring(Error.IndexOf("Line: ") + 6);
                    Line = Line.Substring(1, Line.Length - 2);
                    FCTB.ApplyError(Textbox_Script, Line);
                }
            }

            Textbox_Script.Focus();
        }

        private void DataGrid_NPCs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
