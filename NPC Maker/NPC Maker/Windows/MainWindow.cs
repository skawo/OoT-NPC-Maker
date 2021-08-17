using FastColoredTextBoxNS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class MainWindow : Form
    {
        string OpenedPath = "";
        NPCFile EditedFile = null;
        NPCEntry SelectedEntry = null;
        int SelectedIndex = -1;
        string OpenedFile = JsonConvert.SerializeObject(new NPCFile(), Formatting.Indented);

        public MainWindow()
        {
            InitializeComponent();

            foreach (TabPage Page in TabControl_Segments.TabPages)
            {
                Controls.SegmentDataGrid sg = new Controls.SegmentDataGrid();
                sg.Grid.CellParsing += DataGridViewSegments_CellParse;
                sg.Grid.CellMouseDoubleClick += Segments_CellMouseDoubleClick;
                sg.Grid.KeyUp += DataGridViewSegments_KeyUp;
                sg.Dock = DockStyle.Fill;

                Page.Controls.Add(sg);
            }

            MessagesContextMenu.MakeContextMenu();
            MsgText.ContextMenuStrip = MessagesContextMenu.MenuStrip;
            MessagesContextMenu.SetTextBox(MsgText);

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
                        FileMenu_Save_Click(this, null);
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
            TabControl.SuspendLayout();

            int SelectedTabIndex = TabControl.SelectedIndex;

            if (SelectedEntry == null)
                return;

            // All the easy stuff first
            #region Basic data

            Textbox_NPCName.Text = SelectedEntry.NPCName;

            Txb_ObjectID.Text = SelectedEntry.ObjectID.ToString();
            Txb_ObjectID_Leave(null, null);
            Txtbox_ReactIfAtt.Text = SelectedEntry.SfxIfAttacked.ToString();
            Txtbox_ReactIfAtt_Leave(null, null);

            Combo_EffIfAtt.SelectedIndex = SelectedEntry.EffectIfAttacked;
            NumUpAlpha.Value = SelectedEntry.Alpha;
            ChkB_FadeOut.Checked = SelectedEntry.FadeOut;
            NumUp_RiddenBy.Value = SelectedEntry.NPCToRide;
            ChkBox_Glow.Checked = SelectedEntry.Glow;
            ChkBox_GenLight.Checked = SelectedEntry.GenLight;
            NumUp_LightLimb.Value = SelectedEntry.LightLimb;
            NumUp_LightXOffs.Value = SelectedEntry.LightPositionOffsets[0];
            NumUp_LightYOffs.Value = SelectedEntry.LightPositionOffsets[1];
            NumUp_LightZOffs.Value = SelectedEntry.LightPositionOffsets[2];
            ChkBox_DebugCol.Checked = SelectedEntry.DEBUGShowCols;
            NumUp_LightRadius.Value = SelectedEntry.LightRadius;

            ChkOnlyWhenLens.Checked = SelectedEntry.VisibleUnderLensOfTruth;
            ChkInvisible.Checked = SelectedEntry.Invisible;

            NumUpDown_Hierarchy.Value = SelectedEntry.Hierarchy;
            ComboBox_HierarchyType.SelectedIndex = SelectedEntry.HierarchyType;
            NumUpDown_XModelOffs.Value = SelectedEntry.ModelPositionOffsets[0];
            NumUpDown_YModelOffs.Value = SelectedEntry.ModelPositionOffsets[1];
            NumUpDown_ZModelOffs.Value = SelectedEntry.ModelPositionOffsets[2];
            NumUpDown_Scale.Value = (decimal)SelectedEntry.ModelScale;
            NumUpDown_CutsceneSlot.Value = SelectedEntry.CutsceneID;

            ComboBox_LookAtType.SelectedIndex = SelectedEntry.LookAtType;

            Combo_Head_Horiz.SelectedIndex = SelectedEntry.HeadHorizAxis;
            Combo_Head_Vert.SelectedIndex = SelectedEntry.HeadVertAxis;
            Combo_Waist_Horiz.SelectedIndex = SelectedEntry.WaistHorizAxis;
            Combo_Waist_Vert.SelectedIndex = SelectedEntry.WaistVertAxis;

            NumUpDown_HeadLimb.Value = SelectedEntry.HeadLimb;
            NumUpDown_WaistLimb.Value = SelectedEntry.WaistLimb;
            NumUpDown_LookAt_X.Value = (decimal)SelectedEntry.LookAtPositionOffsets[0];
            NumUpDown_LookAt_Y.Value = (decimal)SelectedEntry.LookAtPositionOffsets[1];
            NumUpDown_LookAt_Z.Value = (decimal)SelectedEntry.LookAtPositionOffsets[2];
            NumUpDown_DegVert.Value = SelectedEntry.LookAtDegreesVertical;
            NumUpDown_DegHoz.Value = SelectedEntry.LookAtDegreesHorizontal;

            Checkbox_DrawShadow.Checked = SelectedEntry.CastsShadow;
            NumUpDown_ShRadius.Value = SelectedEntry.ShadowRadius;

            Checkbox_HaveCollision.Checked = SelectedEntry.HasCollision;
            Checkbox_CanPressSwitches.Checked = SelectedEntry.PushesSwitches;
            NumUpDown_Mass.Value = SelectedEntry.Mass;
            NumUpDown_ColRadius.Value = SelectedEntry.CollisionRadius;
            NumUpDown_ColHeight.Value = SelectedEntry.CollisionHeight;
            Checkbox_AlwaysActive.Checked = SelectedEntry.IsAlwaysActive;
            Checkbox_AlwaysDraw.Checked = SelectedEntry.IsAlwaysDrawn;
            Chkb_ReactIfAtt.Checked = SelectedEntry.ReactsIfAttacked;
            ChkRunJustScript.Checked = SelectedEntry.ExecuteJustScript;
            Chkb_Opendoors.Checked = SelectedEntry.OpensDoors;
            NumUp_Smoothing.Value = (decimal)SelectedEntry.SmoothingConstant;
            Chkb_IgnoreY.Checked = SelectedEntry.IgnoreYAxis;
            NumUpDown_YColOffs.Value = SelectedEntry.CollisionYShift;

            Checkbox_Targettable.Checked = SelectedEntry.IsTargettable;
            ComboBox_TargetDist.SelectedIndex = SelectedEntry.TargetDistance <= 10 ? SelectedEntry.TargetDistance : 1;
            NumUpDown_TargetLimb.Value = SelectedEntry.TargetLimb;
            NumUpDown_XTargetOffs.Value = SelectedEntry.TargetPositionOffsets[0];
            NumUpDown_YTargetOffs.Value = SelectedEntry.TargetPositionOffsets[1];
            NumUpDown_ZTargetOffs.Value = SelectedEntry.TargetPositionOffsets[2];
            NumUpDown_TalkRadi.Value = (decimal)SelectedEntry.TalkRadius;

            Combo_MovementType.SelectedIndex = SelectedEntry.MovementType;
            NumUpDown_MovDistance.Value = SelectedEntry.MovementDistance;
            NumUp_MaxRoam.Value = SelectedEntry.MaxDistRoam;
            NumUpDown_MovSpeed.Value = (decimal)SelectedEntry.MovementSpeed;
            NumUpDown_GravityForce.Value = (decimal)SelectedEntry.GravityForce;
            NumUpDown_LoopDelay.Value = SelectedEntry.MovementDelayTime;
            NumUpDown_LoopEndNode.Value = SelectedEntry.PathEndNodeID;
            NumUpDown_LoopStartNode.Value = SelectedEntry.PathStartNodeID;
            Checkbox_Loop.Checked = SelectedEntry.LoopPath;
            NumUpDown_PathFollowID.Value = SelectedEntry.PathID;
            tmpicker_timedPathStart.Value = Helpers.GetTimeFromOcarinaTime(SelectedEntry.TimedPathStart);
            tmpicker_timedPathEnd.Value = Helpers.GetTimeFromOcarinaTime(SelectedEntry.TimedPathEnd);

            ComboBox_AnimType.SelectedIndex = SelectedEntry.AnimationType;
            NumUpDown_Scale.Value = (decimal)SelectedEntry.ModelScale;

            NumUpDown_ScriptsVar.Value = (int)SelectedEntry.NumVars;
            NumUpDown_ScriptsFVar.Value = (int)SelectedEntry.NumFVars;

            Button_EnvironmentColorPreview.BackColor = Color.FromArgb(255, SelectedEntry.EnvironmentColor.R, SelectedEntry.EnvironmentColor.G, SelectedEntry.EnvironmentColor.B);
            Btn_LightColor.BackColor = Color.FromArgb(255, SelectedEntry.LightColor.R, SelectedEntry.LightColor.G, SelectedEntry.LightColor.B);

            if (SelectedEntry.EnvironmentColor.A == 0)
                Checkbox_EnvColor.Checked = false;
            else
                Checkbox_EnvColor.Checked = true;

            Textbox_BlinkPattern.Text = SelectedEntry.BlinkPattern;
            Textbox_TalkingPattern.Text = SelectedEntry.TalkPattern;
            NumUpDown_BlinkSegment.Value = SelectedEntry.BlinkSegment;
            NumUpDown_BlinkSpeed.Value = SelectedEntry.BlinkSpeed;
            NumUpDown_TalkSegment.Value = SelectedEntry.TalkSegment;
            NumUpDown_TalkSpeed.Value = SelectedEntry.TalkSpeed;

            #endregion
            // Create tab pages for the script, reusing ones that already existed prior to switching to this NPC.
            #region Script Tab Pages
            List<TabPage> ReusableTabPages = new List<TabPage>();

            foreach (TabPage Page in TabControl.TabPages)
            {
                if ((string)Page.Tag == "SCRIPT")
                    ReusableTabPages.Add(Page);
            }

            foreach (ScriptEntry ScriptT in SelectedEntry.Scripts)
            {
                TabPage Page;

                if (ReusableTabPages.Count != 0)
                {
                    Page = ReusableTabPages.First();
                    (Page.Controls[0] as ScriptEditor).Init(ref SelectedEntry, ref EditedFile, ScriptT, syntaxHighlightingToolStripMenuItem.Checked, checkSyntaxToolStripMenuItem.Checked);
                    Page.Text = ScriptT.Name;
                    ReusableTabPages.Remove(Page);
                }
                else
                {
                    Page = new TabPage(ScriptT.Name) { Tag = "SCRIPT" };
                    TabControl.TabPages.Add(Page);

                    ScriptEditor Se = new ScriptEditor(ref SelectedEntry, ref EditedFile, ScriptT, syntaxHighlightingToolStripMenuItem.Checked, checkSyntaxToolStripMenuItem.Checked) { Dock = DockStyle.Fill };
                    Page.Controls.Add(Se);
                }
            }

            foreach (TabPage Page in ReusableTabPages)
                TabControl.TabPages.Remove(Page);

            #endregion

            #region Colors grid
            ColorsDataGridView.Rows.Clear();

            foreach (ColorEntry ColorE in SelectedEntry.DisplayListColors)
            {
                int RowIndex = ColorsDataGridView.Rows.Add(new object[] { ColorE.Limbs, "" });

                ColorsDataGridView.Rows[RowIndex].Cells[1].Style = new DataGridViewCellStyle()
                {
                    SelectionBackColor = ColorE.Color,
                    SelectionForeColor = ColorE.Color,
                    BackColor = ColorE.Color
                };
            }

            #endregion

            #region Animations grid
            DataGrid_Animations.Rows.Clear();

            foreach (AnimationEntry Animation in SelectedEntry.Animations)
            {
                if (SelectedEntry.AnimationType == 1)
                    DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Dicts.GetStringFromStringIntDict(Dicts.LinkAnims, (int)Animation.Address), Animation.StartFrame, Animation.EndFrame, Animation.Speed, Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Animation.ObjID) });
                else
                    DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Animation.Address.ToString("X"), Animation.StartFrame, Animation.EndFrame, Animation.Speed, Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Animation.ObjID) });
            }

            #endregion

            #region Segments grid

            for (int j = 0; j < SelectedEntry.Segments.Count; j++)
            {
                DataGridView Grid = (TabControl_Segments.TabPages[j].Controls[0] as Controls.SegmentDataGrid).Grid;

                Grid.Rows.Clear();

                foreach (SegmentEntry Entry in SelectedEntry.Segments[j])
                    Grid.Rows.Add(Entry.Name, Entry.Address.ToString("X"), Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Entry.ObjectID));
            }

            #endregion

            #region Display lists grid

            DataGridView_ExtraDLists.Rows.Clear();

            foreach (DListEntry Dlist in SelectedEntry.ExtraDisplayLists)
            {
                string SelCombo = ExtraDlists_ShowType.Items[(int)Dlist.ShowType].ToString();

                int Row = DataGridView_ExtraDLists.Rows.Add(new object[] { Dlist.Name,
                                                                           "",
                                                                           Dlist.Address.ToString("X"),
                                                                           Dlist.TransX.ToString() + "," + Dlist.TransY.ToString() + "," + Dlist.TransZ.ToString(),
                                                                           Dlist.RotX.ToString() + "," + Dlist.RotY.ToString() + "," + Dlist.RotZ.ToString(),
                                                                           Dlist.Scale.ToString(),
                                                                           Dlist.Limb.ToString(),
                                                                           Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Dlist.ObjectID),
                                                                           SelCombo
                                                                          });

                DataGridView_ExtraDLists.Rows[Row].Cells[(int)EDlistsColumns.Color].Style.BackColor = Dlist.Color;

            }

            TabControl.SelectedIndex = Math.Min(TabControl.TabPages.Count - 1, SelectedTabIndex);

            #endregion

            #region Messages

            MessagesGrid.SelectionChanged -= MessagesGrid_SelectionChanged;
            MessagesGrid.Rows.Clear();

            foreach (MessageEntry Entry in SelectedEntry.Messages)
            {
                MessagesGrid.Rows.Add(new object[] { Entry.Name });
            }

            MessagesGrid.SelectionChanged += MessagesGrid_SelectionChanged;
            MessagesGrid_SelectionChanged(MessagesGrid, null);

            #endregion

            TabControl.ResumeLayout();
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
            EditedFile.GlobalHeaders.AddRange(new List<ScriptEntry>(){ Defaults.DefaultDefines, Defaults.DefaultMacros});

            Panel_Editor.Enabled = true;
            InsertDataIntoActorListGrid();
        }

        private void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

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
            if (EditedFile == null)
                return;

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
            if (EditedFile == null)
                return;


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

            //InsertDataToEditor();
        }

        private void FileMenu_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
                FileMenu_Save_Click(null, null);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ColorPickerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                Clipboard.SetText($"{ColorDialog.Color.R}, {ColorDialog.Color.G}, {ColorDialog.Color.B}");
            }
        }

        private void EditGlobalHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            Windows.GlobalHeader gh = new Windows.GlobalHeader(ref EditedFile, checkSyntaxToolStripMenuItem.Checked, checkSyntaxToolStripMenuItem.Checked);
            gh.ShowDialog();
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

        private void CheckSyntaxToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TabPage Page in TabControl.TabPages)
            {
                if ((string)Page.Tag == "SCRIPT")
                {
                    if (Page.Controls.Count != 0)
                        (Page.Controls[0] as ScriptEditor).SetAutoParsing((sender as ToolStripMenuItem).Checked);

                }
            }
        }

        private void FileMenu_Click(object sender, EventArgs e)
        {
            //hack
            Label_AnimDefs.Focus();
            Label_BlinkingFramesBetween.Focus();
            Label_ColHeight.Focus();
        }

        private string GetScriptName(string Current = "")
        {
            string ScriptName = Current;
            DialogResult Dr = InputBox.ShowInputDialog("Script name?", ref ScriptName);

            if (Dr != DialogResult.OK)
                return "";
            else
                return ScriptName;
        }

        private bool CheckScriptOpForValidity(bool OnTab = false)
        {
            if (SelectedEntry == null || SelectedEntry.IsNull)
            {
                MessageBox.Show("No actor chosen, or actor is null.");
                return false;
            }

            if (OnTab)
            {
                if ((string)TabControl.SelectedTab.Tag != "SCRIPT")
                {
                    MessageBox.Show("Select a script tab first.");
                    return false;
                }
            }

            return true;
        }

        private void AddNewScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckScriptOpForValidity())
                return;

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

            ScriptEditor Se = new ScriptEditor(ref SelectedEntry, ref EditedFile, Sc, syntaxHighlightingToolStripMenuItem.Checked, checkSyntaxToolStripMenuItem.Checked)
            {
                Dock = DockStyle.Fill
            };

            Page.Controls.Add(Se);
            TabControl.TabPages.Add(Page);
        }

        private void RenameCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckScriptOpForValidity(true))
                return;

            string Name = GetScriptName(TabControl.SelectedTab.Text);

            if (Name != "")
            {

                (TabControl.SelectedTab.Controls[0] as ScriptEditor).Script.Name = Name;
                TabControl.SelectedTab.Text = Name;
            }
        }

        private void DeleteCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckScriptOpForValidity(true))
                return;

            DialogResult Res = MessageBox.Show("Are you sure you want to delete this script? You cannot reverse this action!", "Removing a script", MessageBoxButtons.YesNoCancel);

            if (Res == DialogResult.Yes)
            {
                SelectedEntry.Scripts.Remove((TabControl.SelectedTab.Controls[0] as ScriptEditor).Script);
                TabControl.TabPages.Remove(TabControl.SelectedTab);
            }
        }

        private void ObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList Objects = new PickableList(Lists.DictType.Objects);
            Objects.ShowDialog();
        }

        private void ActorsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PickableList Actors = new PickableList(Lists.DictType.Actors);
            Actors.ShowDialog();
        }

        private void SFXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList(Lists.DictType.SFX);
            SFX.ShowDialog();
        }

        private void MusicToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PickableList Music = new PickableList(Lists.DictType.Music);
            Music.ShowDialog();
        }

        private void LinkAnimsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PickableList Music = new PickableList(Lists.DictType.LinkAnims);
            Music.ShowDialog();
        }

        #endregion

        #region NPCList

        private void Button_Add_Click(object sender, EventArgs e)
        {
            NPCEntry Entry = new NPCEntry();
            Entry.Animations.Add(new AnimationEntry("Idle", 0, 1.0f, -1, 0, 255));
            Entry.Animations.Add(new AnimationEntry("Walking", 0, 1.0f, -1, 0, 255));
            Entry.Animations.Add(new AnimationEntry("Attacked", 0, 1.0f, -1, 0, 255));

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


        private void Txb_ReactIfAtt_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Txtbox_ReactIfAtt_Leave(null, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private void Txtbox_ReactIfAtt_Leave(object sender, EventArgs e)
        {
            short ObjectId = (short)Dicts.GetIntFromStringIntDict(Dicts.SFXes, Txtbox_ReactIfAtt.Text);

            if (ObjectId < -1)
                ObjectId = -1;

            Txtbox_ReactIfAtt.Text = Dicts.GetStringFromStringIntDict(Dicts.SFXes, ObjectId);
            SelectedEntry.SfxIfAttacked = ObjectId;
        }

        private void Btn_ReactIfAttList_Click(object sender, EventArgs e)
        {
            PickableList Objects = new PickableList(Lists.DictType.SFX, true);
            DialogResult DR = Objects.ShowDialog();

            if (DR == DialogResult.OK)
            {
                Txtbox_ReactIfAtt.Text = Objects.Chosen.ID.ToString();
                Txtbox_ReactIfAtt_Leave(null, null);
            }
        }

        private void Txb_ObjectID_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Txb_ObjectID_Leave(null, null);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private void Txb_ObjectID_Leave(object sender, EventArgs e)
        {
            short ObjectId = (short)Dicts.GetIntFromStringIntDict(Dicts.ObjectIDs, Txb_ObjectID.Text);

            if (ObjectId < 0)
                ObjectId = (short)SelectedEntry.ObjectID;

            Txb_ObjectID.Text = Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, ObjectId);

            SelectedEntry.ObjectID = (ushort)ObjectId;
        }

        private void Btn_SelectObject_Click(object sender, EventArgs e)
        {
            PickableList Objects = new PickableList(Lists.DictType.Objects, true, new List<int>() { -1, -2, -3, -4, -5 });
            DialogResult DR = Objects.ShowDialog();

            if (DR == DialogResult.OK)
            {
                Txb_ObjectID.Text = Objects.Chosen.ID.ToString();
                Txb_ObjectID_Leave(null, null);
            }
        }

        private void Textbox_NPCName_TextChanged(object sender, EventArgs e)
        {
            NPCEntry.Members Member = NPCEntry.GetMemberFromTag((sender as TextBox).Tag, (sender as TextBox).Name);

            SelectedEntry.ChangeValueOfMember(Member, (sender as TextBox).Text);
            DataGrid_NPCs.Rows[SelectedIndex].Cells[1].Value = Textbox_NPCName.Text;
        }


        private void ComboBox_AnimType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox_ValueChanged(sender, e);
            Col_OBJ.Visible = (ComboBox_AnimType.SelectedIndex == 0);
            InsertDataToEditor();
        }

        private void Button_EnvironmentColorPreview_Click(object sender, EventArgs e)
        {
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                Button_EnvironmentColorPreview.BackColor = ColorDialog.Color;
                SelectedEntry.EnvironmentColor = ColorDialog.Color;

                if (Checkbox_EnvColor.Checked)
                    SelectedEntry.EnvironmentColor = Color.FromArgb(255, ColorDialog.Color.R, ColorDialog.Color.G, ColorDialog.Color.B);
                else
                    SelectedEntry.EnvironmentColor = Color.FromArgb(0, ColorDialog.Color.R, ColorDialog.Color.G, ColorDialog.Color.B);
            }

        }

        private void Checkbox_EnvColor_CheckedChanged(object sender, EventArgs e)
        {
            if (Checkbox_EnvColor.Checked)
                SelectedEntry.EnvironmentColor = Color.FromArgb(255, SelectedEntry.EnvironmentColor.R, SelectedEntry.EnvironmentColor.G, SelectedEntry.EnvironmentColor.B);
            else
                SelectedEntry.EnvironmentColor = Color.FromArgb(0, SelectedEntry.EnvironmentColor.R, SelectedEntry.EnvironmentColor.G, SelectedEntry.EnvironmentColor.B);
        }

        private void Btn_LightColor_Click(object sender, EventArgs e)
        {
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                Btn_LightColor.BackColor = ColorDialog.Color;
                SelectedEntry.LightColor = ColorDialog.Color;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            NPCEntry.Members Member = NPCEntry.GetMemberFromTag((sender as TextBox).Tag, (sender as TextBox).Name);
            SelectedEntry.ChangeValueOfMember(Member, (sender as TextBox).Text);
        }

        private void NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            NPCEntry.Members Member = NPCEntry.GetMemberFromTag((sender as NumericUpDown).Tag, (sender as NumericUpDown).Name);
            SelectedEntry.ChangeValueOfMember(Member, (sender as NumericUpDown).Value);
        }

        private void CheckBox_ValueChanged(object sender, EventArgs e)
        {
            NPCEntry.Members Member = NPCEntry.GetMemberFromTag((sender as CheckBox).Tag, (sender as CheckBox).Name);
            SelectedEntry.ChangeValueOfMember(Member, (sender as CheckBox).Checked);
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            NPCEntry.Members Member = NPCEntry.GetMemberFromTag((sender as DateTimePicker).Tag, (sender as DateTimePicker).Name);
            SelectedEntry.ChangeValueOfMember(Member, (sender as DateTimePicker).Value.ToString("HH:mm"));
        }

        private void ComboBox_ValueChanged(object sender, EventArgs e)
        {
            NPCEntry.Members Member = NPCEntry.GetMemberFromTag((sender as ComboBox).Tag, (sender as ComboBox).Name);
            SelectedEntry.ChangeValueOfMember(Member, (sender as ComboBox).SelectedIndex);
        }

        #endregion

        #region Animation Grid

        private enum AnimGridColumns
        {
            Name = 0,
            Address = 1,
            StartFrame = 2,
            EndFrame = 3,
            Speed = 4,
            Object = 5,
        }

        private void DataGrid_Animations_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == (int)AnimGridColumns.Object)
            {
                PickableList Objects = new PickableList(Lists.DictType.Objects, true, new List<int>() { -2, -3, -4, -5 });
                DialogResult DR = Objects.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Objects.Chosen.ID.ToString();
                    DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, Objects.Chosen.ID.ToString(), e.GetType(), null));
                    DataGrid_Animations.RefreshEdit();
                }
            }
            else if (e.ColumnIndex == (int)AnimGridColumns.Address && SelectedEntry.AnimationType == 1)
            {
                PickableList Anims = new PickableList(Lists.DictType.LinkAnims, true);
                DialogResult DR = Anims.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Anims.Chosen.ID.ToString("X");
                    DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, Anims.Chosen.ID.ToString(), e.GetType(), null));
                    DataGrid_Animations.RefreshEdit();
                }
            }
        }

        private void AddBlankAnim(int SkipIndex, int Index, string Name = null, uint? Address = null, float? Speed = null, short? ObjectID = null, byte StartFrame = 0, byte EndFrame = 0xFF)
        {
            Name = Name ?? "Animation_" + Index.ToString();
            Address = Address ?? 0;
            Speed = Speed ?? 1;
            ObjectID = ObjectID ?? -1;

            SelectedEntry.Animations.Add(new AnimationEntry(Name, (uint)Address, (float)Speed, (short)ObjectID, StartFrame, EndFrame));

            if (SkipIndex != (int)AnimGridColumns.Name)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Name].Value = Name;

            if (SkipIndex != (int)AnimGridColumns.Address)
                if (SelectedEntry.AnimationType == 1)
                    DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Address].Value = Address; 
                else
                    DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Address].Value = Dicts.GetStringFromStringIntDict(Dicts.LinkAnims, (int)Address);


            if (SkipIndex != (int)AnimGridColumns.StartFrame)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.StartFrame].Value = 0;

            if (SkipIndex != (int)AnimGridColumns.EndFrame)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.EndFrame].Value = 255;

            if (SkipIndex != (int)AnimGridColumns.Speed)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Speed].Value = 1.0;

            if (SkipIndex != (int)AnimGridColumns.Object)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Object].Value = Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, (int)ObjectID);
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
                case (int)AnimGridColumns.Name:
                    {
                        if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                            AddBlankAnim(e.ColumnIndex, e.RowIndex, e.Value.ToString());
                        else
                            SelectedEntry.Animations[e.RowIndex].Name = e.Value.ToString();

                        e.ParsingApplied = true;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        return;
                    }
                case (int)AnimGridColumns.Address:
                    {

                        if (SelectedEntry.AnimationType == 1)
                        {
                            try
                            {
                                int LinkAnim = Dicts.GetIntFromStringIntDict(Dicts.LinkAnims, e.Value.ToString());

                                e.Value = Dicts.GetStringFromStringIntDict(Dicts.LinkAnims, LinkAnim);
                                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;

                                if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                    AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, (short)LinkAnim);
                                else
                                    SelectedEntry.Animations[e.RowIndex].ObjID = (short)LinkAnim;

                                e.ParsingApplied = true;
                            }
                            catch (Exception)
                            {
                                if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                    AddBlankAnim(e.ColumnIndex, e.RowIndex);

                                e.Value = Dicts.LinkAnims.First().Key;
                                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                            }
                        }
                        else
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
                                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                                e.ParsingApplied = true;
                            }
                        }
                        return;
                    }
                case (int)AnimGridColumns.StartFrame:
                    {
                        try
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, null, Convert.ToByte(e.Value.ToString()));
                            else
                                SelectedEntry.Animations[e.RowIndex].StartFrame = Convert.ToByte(e.Value.ToString());

                            e.ParsingApplied = true;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex);

                            e.Value = "";
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                            e.ParsingApplied = true;
                        }
                        return;
                    }
                case (int)AnimGridColumns.EndFrame:
                    {
                        try
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, null, 0, Convert.ToByte(e.Value.ToString()));
                            else
                                SelectedEntry.Animations[e.RowIndex].EndFrame = Convert.ToByte(e.Value.ToString());

                            e.ParsingApplied = true;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex);

                            e.Value = "";
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                            e.ParsingApplied = true;
                        }
                        return;
                    }
                case (int)AnimGridColumns.Speed:
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
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                            e.ParsingApplied = true;
                        }
                        return;
                    }
                case (int)AnimGridColumns.Object:
                    {
                        try
                        {
                            int ObjectId = Dicts.GetIntFromStringIntDict(Dicts.ObjectIDs, e.Value.ToString());

                            if (ObjectId < -1)
                                ObjectId = 0;

                            e.Value = Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, ObjectId);
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;

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

                            e.Value = Dicts.ObjectIDs.First().Key;
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
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
                if ((sender as DataGridView).SelectedCells[0].RowIndex > 2)
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

        private enum EDlistsColumns
        {
            Purpose = 0,
            Color = 1,
            Offset = 2,
            Translation = 3,
            Rotation = 4,
            Scale = 5,
            Limb = 6,
            Object = 7,
            ShowType = 8,
        }

        private void DataGridView_ExtraDLists_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == (int)EDlistsColumns.Object)
            {
                PickableList Objects = new PickableList(Lists.DictType.Objects, true);
                DialogResult DR = Objects.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Objects.Chosen.ID.ToString();
                    DataGridView_ExtraDLists_CellParsing(DataGridView_ExtraDLists, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, Objects.Chosen.ID.ToString(), e.GetType(), null));
                    DataGridView_ExtraDLists.Update();
                }
            }
            else if (e.ColumnIndex == (int)EDlistsColumns.Color)
            {
                if (ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Style =
                        new DataGridViewCellStyle()
                        {
                            SelectionForeColor = ColorDialog.Color,
                            BackColor = ColorDialog.Color,
                            SelectionBackColor = ColorDialog.Color

                        };

                    DataGridView_ExtraDLists_CellParsing(DataGridView_ExtraDLists, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, "", e.GetType(), null));
                    DataGridView_ExtraDLists.Update();

                }
            }
        }

        private void AddBlankDList(int SkipIndex, int Index, string Name = null, uint? Address = null, float? TransX = null, float? TransY = null, float? TransZ = null,
                                   short? RotX = null, short? RotY = null, short? RotZ = null, float? Scale = null, ushort? Limb = null, int? ShowType = null, short? ObjectID = null, Color? EnvColor = null)
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
            EnvColor = EnvColor ?? Color.FromArgb(255, 255, 255, 255);


            SelectedEntry.ExtraDisplayLists.Add(new DListEntry(Name, (uint)Address, (float)TransX, (float)TransY, (float)TransZ, (Color)EnvColor,
                                                    (short)RotX, (short)RotY, (short)RotZ, (float)Scale, (ushort)Limb, (int)ShowType, (short)ObjectID));

            if (SkipIndex != (int)EDlistsColumns.Purpose)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Purpose].Value = Name;

            if (SkipIndex != (int)EDlistsColumns.Color)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Color].Style.BackColor = (Color)EnvColor;

            if (SkipIndex != (int)EDlistsColumns.Offset)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Offset].Value = Address;

            if (SkipIndex != (int)EDlistsColumns.Translation)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Translation].Value = $"{TransX},{TransY},{TransZ}";

            if (SkipIndex != (int)EDlistsColumns.Rotation)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Rotation].Value = $"{RotX},{RotY},{RotZ}";

            if (SkipIndex != (int)EDlistsColumns.Scale)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Scale].Value = Scale;

            if (SkipIndex != (int)EDlistsColumns.Limb)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Limb].Value = Limb;

            if (SkipIndex != (int)EDlistsColumns.Object)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Object].Value = ObjectID == -1 ? "---" : ObjectID.ToString();

            if (SkipIndex != (int)EDlistsColumns.ShowType)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.ShowType].Value = ExtraDlists_ShowType.Items[(int)ShowType];
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

        private Int16[] GetXYZRotation(string Value)
        {
            string[] Split = Value.Split(',');
            Int16[] Values = new short[3] { 0, 0, 0 };

            try
            {
                Values[0] = (Int16)Convert.ToInt16(Split[0]);
                Values[1] = (Int16)Convert.ToInt16(Split[1]);
                Values[2] = (Int16)Convert.ToInt16(Split[2]);
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
                case (int)EDlistsColumns.Purpose:
                    {
                        if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, e.Value.ToString());
                        else
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].Name = e.Value.ToString();

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Color:
                    {
                        if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, null, (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor);
                        else
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].Color = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Offset:
                    {
                        try
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, Convert.ToUInt32(e.Value.ToString(), 16));
                            else
                                SelectedEntry.ExtraDisplayLists[e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = 0;
                            DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Translation:
                    {
                        float[] Transl = GetXYZTranslation(e.Value.ToString());
                        e.Value = $"{Transl[0]},{Transl[1]},{Transl[2]}";
                        DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;

                        if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, Transl[0], Transl[1], Transl[2]);
                        else
                        {
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].TransX = Transl[0];
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].TransY = Transl[1];
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].TransZ = Transl[2];
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Rotation:
                    {
                        Int16[] Rot = GetXYZRotation(e.Value.ToString());
                        e.Value = $"{Rot[0]},{Rot[1]},{Rot[2]}";
                        DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;

                        if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, Rot[0], Rot[1], Rot[2]);
                        else
                        {
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].RotX = Rot[0];
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].RotY = Rot[1];
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].RotZ = Rot[2];
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Scale:
                    {
                        try
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, (float)Convert.ToDecimal(e.Value));
                            else
                                SelectedEntry.ExtraDisplayLists[e.RowIndex].Scale = (float)Convert.ToDecimal(e.Value);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = 0;
                            DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Limb:
                    {
                        try
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, Convert.ToUInt16(e.Value));
                            else
                                SelectedEntry.ExtraDisplayLists[e.RowIndex].Limb = Convert.ToUInt16(e.Value);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = 0;
                            DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Object:
                    {
                        try
                        {
                            short ObjectId = (short)Dicts.GetIntFromStringIntDict(Dicts.ObjectIDs, e.Value.ToString());

                            e.Value = Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, ObjectId);
                            DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;

                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, ObjectId);
                            else
                                SelectedEntry.ExtraDisplayLists[e.RowIndex].ObjectID = ObjectId;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = Dicts.ObjectIDs.First();
                            DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.ShowType:
                    {
                        int ShowType = Dicts.GetIntFromStringIntDict(Dicts.LimbShowSubTypes, e.Value.ToString(), 0);

                        if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, ShowType);
                        else
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].ShowType = ShowType;

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
                    SelectedEntry.ExtraDisplayLists.RemoveAt(Index);
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Segments Grid

        private enum SegmentsColumns
        {
            Name = 0,
            Address = 1,
            Object = 2,
        }

        private void Segments_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == (int)SegmentsColumns.Object)
            {
                PickableList Objects = new PickableList(Lists.DictType.Objects, true);
                DialogResult DR = Objects.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Objects.Chosen.ID.ToString();
                    DataGridViewSegments_CellParse(sender, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, Objects.Chosen.ID.ToString(), e.GetType(), null));
                    (sender as DataGridView).Update();
                }
            }
        }

        private void AddBlankSeg(int SkipIndex, int Index, int Segment, string Name = null, uint? Address = null, short? ObjectID = null)
        {
            Name = Name ?? "Texture_" + Index.ToString();
            Address = Address ?? 0;
            ObjectID = ObjectID ?? -1;

            SelectedEntry.Segments[Segment].Add(new SegmentEntry(Name, (uint)Address, (short)ObjectID));

            DataGridView dgv = (TabControl_Segments.TabPages[Segment].Controls[0] as Controls.SegmentDataGrid).Grid;

            if (SkipIndex != (int)SegmentsColumns.Name)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.Name].Value = Name;

            if (SkipIndex != (int)SegmentsColumns.Address)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.Address].Value = Address;

            if (SkipIndex != (int)SegmentsColumns.Object)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.Object].Value = Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, (int)ObjectID);
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
                case (int)SegmentsColumns.Name:
                    {
                        if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                            AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, e.Value.ToString());
                        else
                            SelectedEntry.Segments[DataGridIndex][e.RowIndex].Name = e.Value.ToString();

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)SegmentsColumns.Address:
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
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex);

                            e.Value = Convert.ToInt32("0", 16);
                            (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)SegmentsColumns.Object:
                    {
                        try
                        {
                            short ObjectId = (short)Dicts.GetIntFromStringIntDict(Dicts.ObjectIDs, e.Value.ToString());

                            e.Value = Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, ObjectId);
                            (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;

                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, null, ObjectId);
                            else
                                SelectedEntry.Segments[DataGridIndex][e.RowIndex].ObjectID = ObjectId;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex);

                            e.Value = Dicts.ObjectIDs.First();
                            (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
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

        #region Colors Grid
        private void ColorsDataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if ((sender as DataGridView).SelectedCells[0].RowIndex > -1)
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        int Index = (sender as DataGridView).SelectedCells[0].RowIndex;
                        (sender as DataGridView).Rows.RemoveAt(Index);
                        SelectedEntry.DisplayListColors.RemoveAt(Index);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void ColorsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 1)
            {
                if (ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    ColorsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style =
                        new DataGridViewCellStyle()
                        {
                            SelectionForeColor = ColorDialog.Color,
                            BackColor = ColorDialog.Color,
                            SelectionBackColor = ColorDialog.Color

                        };

                    if (SelectedEntry.DisplayListColors.Count() - 1 < e.RowIndex)
                    {
                        SelectedEntry.DisplayListColors.Add(new ColorEntry("", ColorDialog.Color));
                        ColorsDataGridView.Rows[e.RowIndex].Cells[0].Value = "";
                    }
                    else
                    {
                        SelectedEntry.DisplayListColors[e.RowIndex].Color = ColorDialog.Color;
                    }
                }
            }
        }

        private void ColorsDataGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (SelectedEntry.DisplayListColors.Count() - 1 < e.RowIndex)
                {
                    Color White = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                    SelectedEntry.DisplayListColors.Add(new ColorEntry(e.Value.ToString(), White));

                    try
                    {
                        SelectedEntry.ParseColorEntries();
                    }
                    catch
                    {
                        SelectedEntry.DisplayListColors[e.RowIndex].Limbs = "";
                        e.Value = "";
                    }

                    ColorsDataGridView.Rows[e.RowIndex].Cells[1].Style =
                        new DataGridViewCellStyle()
                        {
                            SelectionForeColor = White,
                            BackColor = White,
                            SelectionBackColor = White

                        };
                }
                else
                {
                    SelectedEntry.DisplayListColors[e.RowIndex].Limbs = e.Value.ToString();

                    try
                    {
                        SelectedEntry.ParseColorEntries();
                    }
                    catch
                    {
                        SelectedEntry.DisplayListColors[e.RowIndex].Limbs = "";
                        e.Value = "";
                    }
                }

                e.ParsingApplied = true;
                return;
            }
        }


        #endregion

        #region Messages

        private void MessagesGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
            {
                MsgText.Enabled = false;
                Combo_MsgType.Enabled = false;
                Combo_MsgPos.Enabled = false;
                return;
            }
            else
            {
                MsgText.Enabled = true;
                Combo_MsgType.Enabled = true;
                Combo_MsgPos.Enabled = true;
            }

            MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
            MsgText.Text = Entry.MessageText;
            Combo_MsgType.SelectedIndex = Entry.Type;
            Combo_MsgPos.SelectedIndex = Entry.Position;
        }

        private void NumUp_BoxNum_ValueChanged(object sender, EventArgs e)
        {
            MsgText_TextChanged(null, null);
        }

        private void MsgText_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
            Entry.MessageText = MsgText.Text;

            List<byte> Data = Entry.ConvertTextData(false);
            ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview((ZeldaMessage.Data.BoxType)Entry.Type, Data.ToArray());

            int NumBoxes = mp.MessageCount;

            if (NumBoxes == 0)
                numUp_BoxNum.Minimum = 0;
            else
                numUp_BoxNum.Minimum = 1;

            if (numUp_BoxNum.Value > NumBoxes)
                numUp_BoxNum.Value = NumBoxes;

            numUp_BoxNum.Maximum = NumBoxes;
            pictureBox1.BackgroundImage = mp.GetPreview((int)numUp_BoxNum.Value - 1);
        }

        private void Combo_MsgPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
            Entry.Position = Combo_MsgPos.SelectedIndex;
        }

        private void Combo_MsgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
            Entry.Type = Combo_MsgType.SelectedIndex;

            if (Entry.Type == 5)
                pictureBox1.BackColor = Color.Black;
            else
                pictureBox1.BackColor = Color.White;

            MsgText_TextChanged(null, null);
        }

        private void Btn_AddMsg_Click(object sender, EventArgs e)
        {
            string Title = "";
            InputBox.ShowInputDialog("Message title?", ref Title);

            SelectedEntry.Messages.Add(new MessageEntry() { Name = Title, MessageText = "", Position = 0, Type = 0 });
            int Index = MessagesGrid.Rows.Add(new object[] { Title });
            MessagesGrid.Rows[Index].Selected = true;
        }

        private void Btn_DeleteMsg_Click(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            SelectedEntry.Messages.RemoveAt(MessagesGrid.SelectedRows[0].Index);
            MessagesGrid.Rows.RemoveAt(MessagesGrid.SelectedRows[0].Index);
        }

        private void Btn_MsgRename_Click(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;


            string Title = (MessagesGrid.SelectedRows[0].Cells[0].Value as string);
            InputBox.ShowInputDialog("New message title?", ref Title);

            MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
            Entry.Name = Title;
            MessagesGrid.SelectedRows[0].Cells[0].Value = Title;
        }

        #endregion
    }
}
