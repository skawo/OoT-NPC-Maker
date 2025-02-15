using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;


namespace NPC_Maker
{
    public partial class MainWindow : Form
    {
        string OpenedPath = "";
        NPCFile EditedFile = null;
        NPCEntry SelectedEntry = null;
        int SelectedIndex = -1;
        string OpenedFile = JsonConvert.SerializeObject(new NPCFile(), Formatting.Indented);
        private readonly System.Windows.Forms.Timer MsgPreviewTimer = new Timer();
        public static List<KeyValuePair<ComboBox, ComboBox>> FunctionComboBoxes;

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


            Combo_CodeEditor.SelectedIndexChanged -= Combo_CodeEditor_SelectedIndexChanged;
            Combo_CodeEditor.Items.Clear();
            Combo_CodeEditor.Items.AddRange(Enum.GetNames(typeof(CCode.CodeEditorEnum)));
            Combo_CodeEditor.Text = Program.Settings.CodeEditor.ToString();
            Textbox_CodeEditorArgs.Text = Program.Settings.CustomCodeEditorArgs;
            TextBox_CodeEditorPath.Text = Program.Settings.CustomCodeEditorPath;
            Combo_CodeEditor.SelectedIndexChanged += Combo_CodeEditor_SelectedIndexChanged;

            MsgPreviewTimer.Interval = 100;
            MsgPreviewTimer.Tick += MsgPreviewTimer_Tick;
            MsgPreviewTimer.Stop();

            MessagesContextMenu.MakeContextMenu();
            MsgText.ContextMenuStrip = MessagesContextMenu.MenuStrip;
            MessagesContextMenu.SetTextBox(MsgText);

            FunctionComboBoxes = new List<KeyValuePair<ComboBox, ComboBox>>()
                        {
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnInit, null),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnUpdate, Combo_WhenOnUpdate ),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnDraw, Combo_WhenOnDraw ),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnLimb, null ),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnDelete, null ),
                        };

            this.DoubleBuffered = true;

            this.ResizeBegin += Form1_ResizeBegin;
            this.ResizeEnd += Form1_ResizeEnd;

            CodeParamsTooltip.SetToolTip(Textbox_CodeEditorArgs, "Available constants: $CODEFILE, $CODEFOLDER");
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

            try
            {
                if (Program.CodeEditorProcess != null)
                    Program.CodeEditorProcess.Kill();
            }
            catch (Exception)
            {

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

            numUpFileStart.Value = SelectedEntry.FileStart;

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
            ChkBox_DBGCol.Checked = SelectedEntry.DEBUGShowCols;
            ChkBox_DBGLookAt.Checked = SelectedEntry.DEBUGLookAtEditor;
            ChkBox_DBGDlist.Checked = SelectedEntry.DEBUGExDlistEditor;
            ChkBox_DBGPrint.Checked = SelectedEntry.DEBUGPrintToScreen;
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

            ChkBox_ExistInAll.Checked = SelectedEntry.ExistInAllRooms;

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

            UncullFwd.Value = (decimal)SelectedEntry.CullForward;
            UncullDown.Value = (decimal)SelectedEntry.CullDown;
            UncullScale.Value = (decimal)SelectedEntry.CullScale;

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

            foreach (TabPage Page in TabControl_Scripts.TabPages)
                ReusableTabPages.Add(Page);

            foreach (ScriptEntry ScriptT in SelectedEntry.Scripts)
            {
                TabPage Page;

                string PageName = ScriptT.Name == "" ? "Script" : ScriptT.Name;

                if (ReusableTabPages.Count != 0)
                {
                    Page = ReusableTabPages.First();
                    (Page.Controls[0] as ScriptEditor).Init(ref SelectedEntry, ref EditedFile, ScriptT, Program.Settings.ColorizeScriptSyntax, Program.Settings.CheckSyntax);
                    Page.Text = PageName;
                    ReusableTabPages.Remove(Page);
                }
                else
                {
                    Page = new TabPage(PageName);
                    TabControl_Scripts.TabPages.Add(Page);

                    ScriptEditor Se = new ScriptEditor(ref SelectedEntry, ref EditedFile, ScriptT, Program.Settings.ColorizeScriptSyntax, Program.Settings.CheckSyntax) { Dock = DockStyle.Fill };
                    Page.Controls.Add(Se);
                }
            }

            foreach (TabPage Page in ReusableTabPages)
                TabControl_Scripts.TabPages.Remove(Page);

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
                    DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Animation.FileStart < 0 ? "Same as main" : Animation.FileStart.ToString("X"), Dicts.GetStringFromStringIntDict(Dicts.LinkAnims, (int)Animation.Address), Animation.StartFrame, Animation.EndFrame, Animation.Speed, Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Animation.ObjID) });
                else
                    DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Animation.FileStart < 0 ? "Same as main" : Animation.FileStart.ToString("X"), Animation.Address.ToString("X"), Animation.StartFrame, Animation.EndFrame, Animation.Speed, Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Animation.ObjID) });
            }

            #endregion

            #region Segments grid

            for (int j = 0; j < TabControl_Segments.TabPages.Count; j++)
            {
                DataGridView Grid = (TabControl_Segments.TabPages[j].Controls[0] as Controls.SegmentDataGrid).Grid;

                Grid.Rows.Clear();

                foreach (SegmentEntry Entry in SelectedEntry.Segments[j])
                    Grid.Rows.Add(Entry.Name, Entry.FileStart < 0 ? "Same as main" : Entry.FileStart.ToString("X"), Entry.Address.ToString("X"), Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Entry.ObjectID));
            }

            #endregion

            #region Display lists grid

            DataGridView_ExtraDLists.Rows.Clear();

            foreach (DListEntry Dlist in SelectedEntry.ExtraDisplayLists)
            {
                string SelCombo = ExtraDlists_ShowType.Items[(int)Dlist.ShowType].ToString();

                int Row = DataGridView_ExtraDLists.Rows.Add(new object[] { Dlist.Name,
                                                                           "",
                                                                           Dlist.FileStart < 0 ? "Same as main" : Dlist.FileStart.ToString("X"),
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
                MessagesGrid.Rows.Add(new object[] { Entry.Name });

            MessagesGrid_SelectionChanged(MessagesGrid, null);
            MessagesGrid.SelectionChanged += MessagesGrid_SelectionChanged;

            #endregion

            #region CCode


            string CompileErrors = "";

            if (SelectedEntry.EmbeddedOverlayCode.Code != "")
            {
                //CCode.CreateCTempDirectory(SelectedEntry.EmbeddedOverlayCode.Code);
                CCode.Compile(true, EditedFile.CHeader, SelectedEntry.EmbeddedOverlayCode, ref CompileErrors);
            }

            TextBox_CompileMsg.Text = CompileErrors;

            int Index = 0;

            foreach (KeyValuePair<ComboBox, ComboBox> kvp in FunctionComboBoxes)
            {
                ComboBox c = kvp.Key;
                ComboBox w = kvp.Value;

                c.SelectedIndexChanged -= Combo_Func_SelectedIndexChanged;

                if (w != null)
                    w.SelectedIndexChanged -= Combo_Func_SelectedIndexChanged;


                if (SelectedEntry.EmbeddedOverlayCode.Functions == null || SelectedEntry.EmbeddedOverlayCode.Functions.Count == 0)
                    c.DataSource = null;
                else
                {
                    c.DisplayMember = "FuncName";
                    c.ValueMember = "Addr";
                    c.DataSource = SelectedEntry.EmbeddedOverlayCode.Functions;
                    c.SelectedIndex = -1;
                    c.BindingContext = new BindingContext();

                    c.SelectedIndex = SelectedEntry.EmbeddedOverlayCode.Functions.FindIndex(x => x.FuncName == SelectedEntry.EmbeddedOverlayCode.SetFuncNames[Index]);

                    if (w != null)
                        w.SelectedIndex = SelectedEntry.EmbeddedOverlayCode.FuncsRunWhen[Index, 1];
                }

                Index++;

                c.SelectedIndexChanged += Combo_Func_SelectedIndexChanged;

                if (w != null)
                    w.SelectedIndexChanged += Combo_Func_SelectedIndexChanged;
            }


            #endregion

            TabControl.ResumeLayout();
        }

        private void TabControlScripts_MouseUp(object sender, MouseEventArgs e)
        {
            int PageClicked = -1;

            for (int i = 0; i < TabControl_Scripts.TabCount; i++)
            {
                if (TabControl_Scripts.GetTabRect(i).Contains(e.Location))
                    PageClicked = i;
            }

            if (PageClicked < 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                // Gotta select the menu so we can reuse the main menu strip code.
                TabControl_Scripts.SelectedTab = TabControl_Scripts.TabPages[PageClicked];

                ContextMenuStrip mn = new ContextMenuStrip();

                ToolStripMenuItem renameScript = new ToolStripMenuItem();
                ToolStripMenuItem deleteScript = new ToolStripMenuItem();
                ToolStripMenuItem newScript = new ToolStripMenuItem();

                mn.Items.AddRange(new ToolStripItem[] { newScript, renameScript, deleteScript });

                renameScript.Size = new System.Drawing.Size(156, 22);
                renameScript.Text = "Rename script";
                renameScript.Click += RenameScript_Click;

                deleteScript.Size = new System.Drawing.Size(156, 22);
                deleteScript.Text = "Delete script";
                deleteScript.Click += DeleteScript_Click;

                newScript.Size = new System.Drawing.Size(156, 22);
                newScript.Text = "New script";
                newScript.Click += NewScript_Click;

                mn.Show(TabControl_Scripts.PointToScreen(new Point(e.X, e.Y)));
            }

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

            OpenFileDialog OFD = new OpenFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(Program.Settings.LastOpenPath),
                RestoreDirectory = true,
            };

            DialogResult DR = OFD.ShowDialog();

            if (DR == DialogResult.OK)
            {
                EditedFile = FileOps.ParseNPCJsonFile(OFD.FileName);
                OpenedFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

                if (EditedFile != null)
                {
                    OpenedPath = OFD.FileName;
                    Program.JsonPath = OpenedPath;
                    Panel_Editor.Enabled = true;
                    InsertDataIntoActorListGrid();
                    ChkBox_UseSpaceFont.Checked = EditedFile.SpaceFromFont;
                    Program.Settings.LastOpenPath = OFD.FileName;
                    Dicts.LoadDicts();
                }
            }

        }

        private void FileMenu_New_Click(object sender, EventArgs e)
        {
            if (SaveChangesAsPrompt() == false)
                return;

            EditedFile = new NPCFile();
            EditedFile.GlobalHeaders.AddRange(new List<ScriptEntry>() { Defaults.DefaultDefines, Defaults.DefaultMacros });

            Panel_Editor.Enabled = true;
            InsertDataIntoActorListGrid();
        }

        private void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            SaveFileDialog SFD = new SaveFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(Program.Settings.LastOpenPath),
                RestoreDirectory = true,
                FileName = "ActorData.json",
                Filter = "Json Files | *.json"
            };

            DialogResult DR = SFD.ShowDialog();

            if (DR == DialogResult.OK)
            {
                OpenedFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);
                FileOps.SaveNPCJSON(SFD.FileName, EditedFile);

                Program.Settings.LastOpenPath = SFD.FileName;
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
                FileOps.SaveNPCJSON(OpenedPath, EditedFile);
            }
        }

        private void FileMenu_SaveBinary_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;


            SaveFileDialog SFD = new SaveFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(Program.Settings.LastSaveBinaryPath),
                RestoreDirectory = true,
                FileName = "zobj.zobj",
                Filter = "Zelda Object Files | *.zobj"
            };
            DialogResult DR = SFD.ShowDialog();

            if (DR == DialogResult.OK)
            {
                FileOps.SaveBinaryFile(SFD.FileName, EditedFile);
                Program.Settings.LastSaveBinaryPath = SFD.FileName;
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

        private void ImproveMessagePreviewReadabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MsgText_TextChanged(null, null);
        }

        private void EditGlobalHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            Windows.GlobalHeader gh = new Windows.GlobalHeader(ref EditedFile, Program.Settings.ColorizeScriptSyntax, Program.Settings.CheckSyntax);
            gh.ShowDialog();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About Window = new About();
            Window.ShowDialog();
        }

        private void NewScript_Click(object sender, EventArgs e)
        {
            AddNewScriptToolStripMenuItem_Click(null, null);
        }

        private void DeleteScript_Click(object sender, EventArgs e)
        {
            DeleteCurrentScriptToolStripMenuItem_Click(null, null);
        }

        private void RenameScript_Click(object sender, EventArgs e)
        {
            RenameCurrentScriptToolStripMenuItem_Click(null, null);
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
                if (TabControl.SelectedTab != Tab5_Scripts)
                {
                    MessageBox.Show("Select the script tab first.");
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

            if (Name != "")
            {
                if (SelectedEntry.Scripts.Count >= 255)
                {
                    MessageBox.Show("Cannot define more than 255 scripts.");
                    return;
                }

                TabPage Page = new TabPage(ScriptName);
                ScriptEntry Sc = new ScriptEntry() { Name = ScriptName, ParseErrors = new List<string>(), Text = "" };
                SelectedEntry.Scripts.Add(Sc);

                ScriptEditor Se = new ScriptEditor(ref SelectedEntry, ref EditedFile, Sc, Program.Settings.ColorizeScriptSyntax, Program.Settings.CheckSyntax)
                {
                    Dock = DockStyle.Fill
                };

                Page.Controls.Add(Se);
                TabControl_Scripts.TabPages.Add(Page);
            }
        }

        private void RenameCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckScriptOpForValidity(true))
                return;

            string Name = GetScriptName(TabControl_Scripts.SelectedTab.Text);

            if (Name != "")
            {
                (TabControl_Scripts.SelectedTab.Controls[0] as ScriptEditor).Script.Name = Name;
                TabControl_Scripts.SelectedTab.Text = Name;
            }
        }

        private void DeleteCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckScriptOpForValidity(true))
                return;

            DialogResult Res = MessageBox.Show("Are you sure you want to delete this script? You cannot reverse this action!", "Removing a script", MessageBoxButtons.YesNoCancel);

            if (Res == DialogResult.Yes)
            {
                SelectedEntry.Scripts.Remove((TabControl_Scripts.SelectedTab.Controls[0] as ScriptEditor).Script);
                TabControl_Scripts.TabPages.Remove(TabControl_Scripts.SelectedTab);
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

        private NPCEntry GetNewNPCEntry()
        {
            NPCEntry Entry = new NPCEntry();
            Entry.Animations.Add(new AnimationEntry("Idle", 0, 1.0f, -1, 0, 255, -1));
            Entry.Animations.Add(new AnimationEntry("Walking", 0, 1.0f, -1, 0, 255, -1));
            Entry.Animations.Add(new AnimationEntry("Attacked", 0, 1.0f, -1, 0, 255, -1));

            for (int i = 0; i < 8; i++)
                Entry.Segments.Add(new List<SegmentEntry>());

            Entry.Scripts.Add(new ScriptEntry() { Name = "Script" });
            return Entry;
        }

        private void Button_Add_Click(object sender, EventArgs e)
        {
            NPCEntry Entry = GetNewNPCEntry();

            Entry.NPCName = $"NPC_{EditedFile.Entries.Count}";
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
            Entry.NPCName = $"{Entry.NPCName}_{EditedFile.Entries.Count - 1}";
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
                SelectedEntry = GetNewNPCEntry();
                SelectedEntry.NPCName = $"NPC_{SelectedIndex}";
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
            FileStart.Visible = (ComboBox_AnimType.SelectedIndex == 0);
            InsertDataToEditor();
        }

        private void Button_EnvironmentColorPreview_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = SelectedEntry.EnvironmentColor;

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
                SelectedEntry.EnvironmentColor = Button_EnvironmentColorPreview.BackColor;
            else
                SelectedEntry.EnvironmentColor = Color.FromArgb(0, SelectedEntry.EnvironmentColor.R, SelectedEntry.EnvironmentColor.G, SelectedEntry.EnvironmentColor.B);

            SelectedEntry.EnvironmentColor = Helpers.TryGetColorWithName(SelectedEntry.EnvironmentColor);
        }

        private void Btn_LightColor_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = SelectedEntry.LightColor;

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
            SelectedEntry.ChangeValueOfMember(Member, (sender as DateTimePicker).Value.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture));
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
            FileStart = 1,
            Address = 2,
            StartFrame = 3,
            EndFrame = 4,
            Speed = 5,
            Object = 6,
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

        private void AddBlankAnim(int SkipIndex, int Index, string Name = null, uint? Address = null, float? Speed = null, short? ObjectID = null, byte StartFrame = 0, byte EndFrame = 0xFF, Int32? FileStart = null)
        {
            Name = Name ?? "Animation_" + Index.ToString();
            Address = Address ?? 0;
            Speed = Speed ?? 1;
            ObjectID = ObjectID ?? -1;
            FileStart = FileStart ?? -1;

            SelectedEntry.Animations.Add(new AnimationEntry(Name, (uint)Address, (float)Speed, (short)ObjectID, StartFrame, EndFrame, (int)FileStart));

            if (SkipIndex != (int)AnimGridColumns.Name)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Name].Value = Name;

            if (SkipIndex != (int)AnimGridColumns.Address)
                if (SelectedEntry.AnimationType == 1)
                    DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Address].Value = Address;
                else
                    DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Address].Value = Dicts.GetStringFromStringIntDict(Dicts.LinkAnims, (int)Address);


            if (SkipIndex != (int)AnimGridColumns.StartFrame)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.StartFrame].Value = 0;

            if (SkipIndex != (int)AnimGridColumns.FileStart)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.FileStart].Value = FileStart == -1 ? "Same as main" : ((int)FileStart).ToString("X");

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
                        string Name = e.Value.ToString();

                        if (!SanitizeName(ref Name))
                            e.Value = "Animation_" + e.RowIndex;
                        else
                            e.Value = Name;

                        while (true)
                        {
                            var entry = SelectedEntry.Animations.Find(x => x.Name.ToUpper() == e.Value.ToString().ToUpper());

                            if (Helpers.DgCheckAddSanity(entry, SelectedEntry.Animations.ToArray(), SelectedEntry.Animations.Count, e.RowIndex))
                                e.Value += "_";
                            else
                                break;
                        }

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
                                    AddBlankAnim(e.ColumnIndex, e.RowIndex, null, (UInt32)LinkAnim);
                                else
                                    SelectedEntry.Animations[e.RowIndex].Address = (UInt32)LinkAnim;

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
                case (int)AnimGridColumns.FileStart:
                    {
                        try
                        {
                            Int32 Value = Convert.ToInt32(e.Value.ToString(), 16);

                            if (Value < 0)
                            {
                                e.Value = -1;
                                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Same as main";
                            }

                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, null, 0, 255, Value);
                            else
                                SelectedEntry.Animations[e.RowIndex].FileStart = Value;

                            e.ParsingApplied = true;
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                AddBlankAnim(e.ColumnIndex, e.RowIndex);

                            e.Value = "Same as main";
                            SelectedEntry.Animations[e.RowIndex].FileStart = -1;
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
            FileStart = 2,
            Offset = 3,
            Translation = 4,
            Rotation = 5,
            Scale = 6,
            Limb = 7,
            Object = 8,
            ShowType = 9,
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
                ColorDialog.Color = DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

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
                                   short? RotX = null, short? RotY = null, short? RotZ = null, float? Scale = null, short? Limb = null, int? ShowType = null, short? ObjectID = null, Color? EnvColor = null, int? FileStart = null)
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
            Scale = Scale ?? 1f;
            Limb = Limb ?? 0;
            ShowType = ShowType ?? 0;
            ObjectID = ObjectID ?? -1;
            EnvColor = EnvColor ?? Color.FromArgb(255, 255, 255, 255);
            FileStart = FileStart ?? -1;

            SelectedEntry.ExtraDisplayLists.Add(new DListEntry(Name, (uint)Address, (float)TransX, (float)TransY, (float)TransZ, (Color)EnvColor,
                                                    (short)RotX, (short)RotY, (short)RotZ, (float)Scale, (short)Limb, (int)ShowType, (short)ObjectID, (int)FileStart));

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

            if (SkipIndex != (int)EDlistsColumns.FileStart)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.FileStart].Value = (FileStart == -1 ? "Same as main" : FileStart.ToString());
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
                        string Name = e.Value.ToString();

                        if (!SanitizeName(ref Name))
                            e.Value = "Dlist_" + e.RowIndex;
                        else
                            e.Value = Name;

                        while (true)
                        {
                            var entry = SelectedEntry.ExtraDisplayLists.Find(x => x.Name.ToUpper() == e.Value.ToString().ToUpper());

                            if (Helpers.DgCheckAddSanity(entry, SelectedEntry.ExtraDisplayLists.ToArray(), SelectedEntry.ExtraDisplayLists.Count, e.RowIndex))
                                e.Value += "_";
                            else
                                break;
                        }

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
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, Convert.ToInt16(e.Value));
                            else
                                SelectedEntry.ExtraDisplayLists[e.RowIndex].Limb = Convert.ToInt16(e.Value);
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
                case (int)EDlistsColumns.FileStart:
                    {
                        try
                        {
                            Int32 Value = Convert.ToInt32(e.Value.ToString(), 16);

                            if (Value < 0)
                            {
                                e.Value = -1;
                                DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Same as main";
                            }

                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, null, null, Value);
                            else
                                SelectedEntry.ExtraDisplayLists[e.RowIndex].FileStart = Convert.ToInt32(e.Value.ToString(), 16);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                                AddBlankDList(e.ColumnIndex, e.RowIndex);

                            e.Value = "Same as main";
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].FileStart = -1;
                        }

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
            FileStart = 1,
            Address = 2,
            Object = 3,
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

        private void AddBlankSeg(int SkipIndex, int Index, int Segment, string Name = null, uint? Address = null, short? ObjectID = null, int? FileStart = null)
        {
            Name = Name ?? "Texture_" + Index.ToString();
            Address = Address ?? 0;
            ObjectID = ObjectID ?? -1;
            FileStart = FileStart ?? -1;

            SelectedEntry.Segments[Segment].Add(new SegmentEntry(Name, (uint)Address, (short)ObjectID, (int)FileStart));

            DataGridView dgv = (TabControl_Segments.TabPages[Segment].Controls[0] as Controls.SegmentDataGrid).Grid;

            if (SkipIndex != (int)SegmentsColumns.Name)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.Name].Value = Name;

            if (SkipIndex != (int)SegmentsColumns.Address)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.Address].Value = Address;

            if (SkipIndex != (int)SegmentsColumns.Object)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.Object].Value = Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, (int)ObjectID);

            if (SkipIndex != (int)SegmentsColumns.FileStart)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.FileStart].Value = (FileStart == -1 ? "Same as main" : ((int)FileStart).ToString("X"));
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
                        string Name = e.Value.ToString();

                        if (!SanitizeName(ref Name))
                            e.Value = "Data_" + e.RowIndex;
                        else
                            e.Value = Name;

                        while (true)
                        {
                            var entry = SelectedEntry.Segments[DataGridIndex].Find(x => x.Name.ToUpper() == e.Value.ToString().ToUpper());

                            if (Helpers.DgCheckAddSanity(entry, SelectedEntry.Segments[DataGridIndex].ToArray(), SelectedEntry.Segments[DataGridIndex].Count, e.RowIndex))
                                e.Value += "_";
                            else
                                break;
                        }

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
                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
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
                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex);

                            e.Value = Dicts.ObjectIDs.First();
                            (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)SegmentsColumns.FileStart:
                    {
                        try
                        {
                            Int32 Value = Convert.ToInt32(e.Value.ToString(), 16);

                            if (Value < 0)
                            {
                                e.Value = -1;
                                (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Same as main";
                            }

                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, null, null, Value);
                            else
                                SelectedEntry.Segments[DataGridIndex][e.RowIndex].FileStart = Convert.ToInt32(e.Value.ToString(), 16);
                        }
                        catch (Exception)
                        {
                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex);

                            e.Value = "Same as main";
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

        private void Btn_MsgMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessagesGrid.SelectedRows.Count == 0)
                    return;

                string Title = (MessagesGrid.SelectedRows[0].Cells[0].Value as string);

                int i = SelectedEntry.Messages.FindIndex(x => x.Name == Title);

                if (i <= 0)
                    return;
                else
                {
                    string titleToSwap = SelectedEntry.Messages[i - 1].Name;
                    MessageEntry msg = SelectedEntry.Messages[i];
                    SelectedEntry.Messages.RemoveAt(i);
                    SelectedEntry.Messages.Insert(i - 1, msg);

                    MessagesGrid.Rows[i].Cells[0].Value = titleToSwap;
                    MessagesGrid.Rows[i - 1].Cells[0].Value = Title;
                    MessagesGrid.Rows[i - 1].Selected = true;
                    MessagesGrid.CurrentCell = MessagesGrid.Rows[i - 1].Cells[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_MsgMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessagesGrid.SelectedRows.Count == 0)
                    return;

                string Title = (MessagesGrid.SelectedRows[0].Cells[0].Value as string);

                int i = SelectedEntry.Messages.FindIndex(x => x.Name == Title);

                if (i >= SelectedEntry.Messages.Count - 1)
                    return;
                else
                {
                    string titleToSwap = SelectedEntry.Messages[i + 1].Name;
                    MessageEntry msg = SelectedEntry.Messages[i];
                    SelectedEntry.Messages.RemoveAt(i);
                    SelectedEntry.Messages.Insert(i + 1, msg);

                    MessagesGrid.Rows[i].Cells[0].Value = titleToSwap;
                    MessagesGrid.Rows[i + 1].Cells[0].Value = Title;
                    MessagesGrid.Rows[i + 1].Selected = true;
                    MessagesGrid.CurrentCell = MessagesGrid.Rows[i + 1].Cells[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetMsgBackground(int Type)
        {
            if (Type == (int)ZeldaMessage.Data.BoxType.None_White)
                PanelMsgPreview.BackColor = Color.Black;
            else
                PanelMsgPreview.BackColor = Color.White;
        }

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

            MsgText.TextChanged -= MsgText_TextChanged;
            Combo_MsgType.SelectedIndexChanged -= Combo_MsgType_SelectedIndexChanged;

            if (SelectedEntry.Messages.Count != 0)
            {

                MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
                MsgText.Text = Entry.MessageText;
                MsgText.ClearUndo();
                Combo_MsgType.SelectedIndex = Entry.Type;
                Combo_MsgPos.SelectedIndex = Entry.Position;

                SetMsgBackground(Entry.Type);
                MsgPreviewTimer_Tick(null, null);
            }

            MsgText.TextChanged += MsgText_TextChanged;
            Combo_MsgType.SelectedIndexChanged += Combo_MsgType_SelectedIndexChanged;
        }

        private void MsgText_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            MsgPreviewTimer.Stop();
            MsgPreviewTimer.Start();
        }

        private void MsgPreviewTimer_Tick(object sender, EventArgs e)
        {
            MsgPreviewTimer.Stop();

            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
            Entry.MessageText = MsgText.Text;

            List<byte> Data = Entry.ConvertTextData(SelectedEntry.NPCName, false);

            if (Data == null || (Data.Count == 0 && !String.IsNullOrEmpty(Entry.MessageText)))
                return;

            bool CreditsTxBox = (ZeldaMessage.Data.BoxType)Entry.Type > ZeldaMessage.Data.BoxType.None_Black;

            ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview((ZeldaMessage.Data.BoxType)Entry.Type, Data.ToArray(), null, null, EditedFile.SpaceFromFont);
            Bitmap bmp = new Bitmap((CreditsTxBox ? 480 : 384), mp.MessageCount * (CreditsTxBox ? 360 : 108));
            bmp.MakeTransparent();

            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < mp.MessageCount; i++)
                {
                    Bitmap bmpTemp = mp.GetPreview(i, Program.Settings.ImproveTextMsgReadability, 1.5f);
                    grfx.DrawImage(bmpTemp, 0, bmpTemp.Height * i);
                }
            }

            MsgPreview.Size = new Size((CreditsTxBox ? 480 : 384), bmp.Height);
            MsgPreview.Location = new Point((this.PanelMsgPreview.Width - MsgPreview.Width) / 2, 0 - PanelMsgPreview.VerticalScroll.Value);
            MsgPreview.Image = bmp;
        }

        private void PanelMsgPreview_Resize(object sender, EventArgs e)
        {
            MsgPreview.Left = (this.PanelMsgPreview.Width - MsgPreview.Width) / 2;
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

            MsgText.TextChanged -= MsgText_TextChanged;
            Combo_MsgType.SelectedIndexChanged -= Combo_MsgType_SelectedIndexChanged;

            SetMsgBackground(Entry.Type);
            MsgPreviewTimer_Tick(null, null);

            MsgText.TextChanged += MsgText_TextChanged;
            Combo_MsgType.SelectedIndexChanged += Combo_MsgType_SelectedIndexChanged;
        }

        private void Btn_AddMsg_Click(object sender, EventArgs e)
        {
            string Title = "";
            DialogResult DR = InputBox.ShowInputDialog("Message title?", ref Title);

            if (DR != DialogResult.OK)
                return;

            if (!SanitizeName(ref Title))
                return;

            if (SelectedEntry.Messages.Find(x => x.Name.ToUpper() == Title.ToUpper()) != null)
            {
                MessageBox.Show("Message with that name already exists.");
                return;
            }

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

            if (!SanitizeName(ref Title))
                return;

            MessageEntry Entry = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];

            MessageEntry same = SelectedEntry.Messages.Find(x => x.Name.ToUpper() == Title.ToUpper());

            if (same != null && same != Entry)
            {
                MessageBox.Show("Message with that name already exists.");
                return;
            }

            if (Title.IsNumeric())
            {
                MessageBox.Show("Message name cannot be just a number.");
                return;
            }

            Entry.Name = Title;
            MessagesGrid.SelectedRows[0].Cells[0].Value = Title;
        }

        private bool SanitizeName(ref string Title)
        {
            if (String.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Name cannot be empty.");
                return false;
            }

            Title = Title.Replace(" ", "_");

            if (Title.IsNumeric())
            {
                MessageBox.Show("Name cannot be just a number.");
                return false;
            }

            foreach (string s in Lists.AllKeywords)
            {
                if (s.ToUpper() == Title.ToUpper() || (Title.ToUpper().StartsWith(s.ToUpper()) && Title.Contains(".")))
                {
                    MessageBox.Show("Name cannot be a script keyword.");
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region CCompile

        private void Button_CCompile_Click(object sender, EventArgs e)
        {
            if (SelectedEntry.EmbeddedOverlayCode.Code != "")
            {
                //CCode.CreateCTempDirectory(SelectedEntry.EmbeddedOverlayCode.Code);
                CompileCode();
            }
            else
                TextBox_CompileMsg.Text = "No code to compile!";
        }
        private void Combo_CodeEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.CodeEditor = (CCode.CodeEditorEnum)Enum.Parse(typeof(CCode.CodeEditorEnum), Combo_CodeEditor.SelectedItem.ToString());

            if (Combo_CodeEditor.SelectedIndex == Combo_CodeEditor.Items.Count - 1)
            {
                Button_FindCodeEditor.Enabled = true;
                Textbox_CodeEditorArgs.Enabled = true;
                TextBox_CodeEditorPath.Enabled = true;
            }
            else
            {
                Button_FindCodeEditor.Enabled = false;
                Textbox_CodeEditorArgs.Enabled = false;
                TextBox_CodeEditorPath.Enabled = false;
            }

        }

        private void TextBox_CodeEditorPath_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.CustomCodeEditorPath = TextBox_CodeEditorPath.Text;
        }

        private void Textbox_CodeEditorArgs_TextChanged(object sender, EventArgs e)
        {
            Program.Settings.CustomCodeEditorArgs = Textbox_CodeEditorArgs.Text;
        }

        Timer autoSaveTimer;
        DateTime LastWriteTime;

        private void WatchFile(NPCEntry EditedEntry)
        {
            if (Program.Settings.AutoSave)
            {
                WatchedEntry = EditedEntry;

                string fPath = Path.Combine(CCode.tempFolderPath, CCode.codeFileName);
                LastWriteTime = GetLastWriteTimeForFile(fPath);

                autoSaveTimer = new Timer();
                autoSaveTimer.Interval = (int)Program.Settings.AutoSaveTime;
                autoSaveTimer.Tick += AutoSaveTimer_Tick;
                autoSaveTimer.Start();
            }
            /*
            else if (Program.Settings.AutoComp_Save)
            {

                if (Program.IsRunningUnderMono)
                    Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "1");

                if (Program.Watcher != null)
                    Program.Watcher.Dispose();

                WatchedEntry = EditedEntry;

                Program.Watcher = new FileSystemWatcher(CCode.tempFolderPath);
                Program.Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                Program.Watcher.Changed += Watcher_Changed;
                Program.Watcher.IncludeSubdirectories = true;
                Program.Watcher.EnableRaisingEvents = true;
                Program.Watcher.Filter = "*.*";
            }
            */
            else
            {
                WatchedEntry = EditedEntry;
            }

            //Process.HasExited doesn't work under mono...

            var t = Task.Factory.StartNew(() =>
            {
                Program.CodeEditorProcess.WaitForExit();

                try
                {
                    Watcher_Changed(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""));

                    //if (Program.Watcher != null)
                    //    Program.Watcher.Dispose();
                }
                catch (Exception)
                {

                }

                Button_CCompile.Invoke((MethodInvoker)delegate
                {
                    globalCHeaderToolStripMenuItem.Enabled = true;
                    Button_CCompile.Enabled = true;
                    Button_OpenCCode.Enabled = true;
                });

                return;
            });

        }

        private DateTime GetLastWriteTimeForFile(string Filename)
        {
            try
            {
                if (File.Exists(Filename))
                {
                    return File.GetLastWriteTime(Filename);
                }
                else
                    return new DateTime();
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            autoSaveTimer.Stop();

            string fPath = Path.Combine(CCode.tempFolderPath, CCode.codeFileName);

            var Dt = GetLastWriteTimeForFile(fPath);

            if (Dt != LastWriteTime)
                Watcher_Changed(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""));

            LastWriteTime = Dt;
            autoSaveTimer.Start();
        }

        private void CompileCode()
        {

            string CompileMsgs = "";
            CCode.Compile(true, EditedFile.CHeader, SelectedEntry.EmbeddedOverlayCode, ref CompileMsgs);

            this.TextBox_CompileMsg.Invoke((MethodInvoker)delegate
            {
                TextBox_CompileMsg.Text = CompileMsgs;
            });

            foreach (KeyValuePair<ComboBox, ComboBox> kvp in FunctionComboBoxes)
            {
                ComboBox c = kvp.Key;

                c.Invoke((MethodInvoker)delegate
                {
                    string CurrentSelection = c.Text;

                    if (SelectedEntry.EmbeddedOverlayCode.Functions == null || SelectedEntry.EmbeddedOverlayCode.Functions.Count == 0)
                        c.DataSource = null;
                    else
                    {
                        c.DisplayMember = "FuncName";
                        c.ValueMember = "Addr";
                        c.DataSource = SelectedEntry.EmbeddedOverlayCode.Functions;
                        c.SelectedIndex = -1;
                        c.BindingContext = new BindingContext();

                        FunctionEntry Function = SelectedEntry.EmbeddedOverlayCode.Functions.FirstOrDefault(x => x.FuncName == CurrentSelection);

                        if (Function != null)
                            c.SelectedIndex = c.Items.IndexOf(Function);
                        else
                            c.SelectedIndex = -1;
                    }
                });
            }
        }

        private void ManualWatcher(object sender, EventArgs e)
        {
            Watcher_Changed(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""));
        }


        NPCEntry WatchedEntry = null;

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (WatchedEntry == null)
                    return;

                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    FileStream fs = null;
                    FileStream fs2 = null;

                    // Hacky workaround
                    try
                    {
                        if (File.Exists(CCode.editCodeFilePath))
                            fs = File.Open(CCode.editCodeFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        if (File.Exists(CCode.editHeaderFilePath))
                            fs2 = File.Open(CCode.editHeaderFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    }
                    catch (Exception)
                    {
                        if (File.Exists(CCode.editCodeFilePath))
                            fs = File.Open(Path.Combine(CCode.tempFolderPath, CCode.codeFileName), FileMode.Open, FileAccess.Read, FileShare.Read);

                        if (File.Exists(CCode.editHeaderFilePath))
                            fs2 = File.Open(Path.Combine(CCode.tempFolderPath, CCode.headerFileName), FileMode.Open, FileAccess.Read, FileShare.Read);
                    }

                    if (fs2 != null)
                    {
                        using (var sr = new StreamReader(fs2, Encoding.Default))
                        {
                            string Header = sr.ReadToEnd();

                            if (Header == "" && EditedFile.CHeader != "")
                                return;
                            else
                                EditedFile.CHeader = Header;
                        }

                        fs2.Close();
                        fs2.Dispose();
                    }

                    if (fs != null)
                    {
                        using (var sr = new StreamReader(fs, Encoding.Default))
                        {
                            string Code = sr.ReadToEnd();

                            if (Code == "" && WatchedEntry.EmbeddedOverlayCode.Code != "")
                                return;
                            else
                                WatchedEntry.EmbeddedOverlayCode.Code = Code;
                        }

                        CompileCode();
                        fs.Close();
                        fs.Dispose();
                    }
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Share error");
                //MessageBox.Show("An error has occurred while attempting to update the embedded overlay: " + ex.Message);
            }
        }

        private void Button_OpenCCode_Click(object sender, EventArgs e)
        {
            if (autoSaveTimer != null)
            {
                autoSaveTimer.Stop();
                autoSaveTimer.Dispose();
            }

            string Code = SelectedEntry.EmbeddedOverlayCode.Code == "" ? Properties.Resources.EmbeddedOverlay : SelectedEntry.EmbeddedOverlayCode.Code;
            Code = CCode.ReplaceGameVersionInclude(Code);

            string Header = EditedFile.CHeader == "" ? Properties.Resources.CHeader : EditedFile.CHeader;
            Header = CCode.ReplaceGameVersionInclude(Header);

            if (!CCode.CreateCTempDirectory(Code, Header))
                return;

            if (Program.CodeEditorProcess != null && !Program.CodeEditorProcess.HasExited)
                Program.CodeEditorProcess.Kill();

            Program.CodeEditorProcess = CCode.OpenCodeEditor(
                                                                (CCode.CodeEditorEnum)Enum.Parse(typeof(CCode.CodeEditorEnum), Combo_CodeEditor.SelectedItem.ToString()),
                                                                TextBox_CodeEditorPath.Text,
                                                                Textbox_CodeEditorArgs.Text.Replace("$CODEFILE", CCode.editCodeFilePath.AppendQuotation()).Replace("$CODEFOLDER", CCode.tempFolderPath.AppendQuotation())
                                                            );

            if (Program.CodeEditorProcess == null)
                return;
            else
            {
                globalCHeaderToolStripMenuItem.Enabled = false;
                Button_CCompile.Enabled = false;
                Button_OpenCCode.Enabled = false;
                Button_UpdateCompile.Enabled = true;
                WatchFile(SelectedEntry);
            }
        }

        private void Button_FindCodeEditor_Click(object sender, EventArgs e)
        {
            OpenFileDialog oF = new OpenFileDialog();
            var Res = oF.ShowDialog();

            if (Res == DialogResult.OK)
            {
                TextBox_CodeEditorPath.Text = oF.FileName;
                Textbox_CodeEditorArgs.Text = "$CODEFILE";
                Combo_CodeEditor.SelectedItem = CCode.CodeEditorEnum.Other.ToString();
                Combo_CodeEditor.Refresh();
            }

        }

        private void Combo_Func_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (sender as ComboBox);

            int ComboId = Convert.ToInt32(c.Tag);

            if (ComboId < 5)
            {
                SelectedEntry.EmbeddedOverlayCode.FuncsRunWhen[ComboId, 0] = c.SelectedIndex;
                SelectedEntry.EmbeddedOverlayCode.SetFuncNames[ComboId] = c.Text;

            }
            else
                SelectedEntry.EmbeddedOverlayCode.FuncsRunWhen[ComboId - 5, 1] = c.SelectedIndex;
        }

        private void Combo_Func_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                (sender as ComboBox).SelectedIndex = -1;
        }

        private void Button_CDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? This operation wipes the code completely and cannot be reversed.", "Code Removal", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                SelectedEntry.EmbeddedOverlayCode.Code = "";
                SelectedEntry.EmbeddedOverlayCode.Functions = new List<FunctionEntry>();
                SelectedEntry.EmbeddedOverlayCode.FuncsRunWhen = new int[5, 2]
                {
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                };

                SelectedEntry.EmbeddedOverlayCode.SetFuncNames = new string[5];

                foreach (KeyValuePair<ComboBox, ComboBox> kvp in FunctionComboBoxes)
                {
                    ComboBox c = kvp.Key;
                    ComboBox w = kvp.Value;

                    c.DataSource = null;

                    if (w != null)
                        w.DataSource = null;
                }

                Button_CCompile_Click(null, null);
            }
        }

        #endregion

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NPC_Maker.Windows.Settings s = new Windows.Settings();
            s.StartPosition = FormStartPosition.CenterParent;
            s.ShowDialog();

            foreach (TabPage Page in TabControl_Scripts.TabPages)
            {
                if (Page.Controls.Count != 0)
                {
                    (Page.Controls[0] as ScriptEditor).SetAutoParsing(Program.Settings.CheckSyntax);
                    (Page.Controls[0] as ScriptEditor).SetSyntaxHighlighting(Program.Settings.ColorizeScriptSyntax);
                }
            }

            MsgText_TextChanged(null, null);

        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/skawo/OoT-NPC-Maker/wiki");
        }

        private void ChkBox_UseSpaceFont_CheckedChanged(object sender, EventArgs e)
        {
            EditedFile.SpaceFromFont = (sender as CheckBox).Checked;
            MsgText_TextChanged(null, null);
        }

        private void globalCHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoSaveTimer != null)
            {
                autoSaveTimer.Stop();
                autoSaveTimer.Dispose();
            }

            string Code = SelectedEntry.EmbeddedOverlayCode.Code == "" ? Properties.Resources.EmbeddedOverlay : SelectedEntry.EmbeddedOverlayCode.Code;
            Code = CCode.ReplaceGameVersionInclude(Code);

            string Header = EditedFile.CHeader == "" ? Properties.Resources.CHeader : EditedFile.CHeader;
            Header = CCode.ReplaceGameVersionInclude(Header);

            if (!CCode.CreateCTempDirectory(Code, Header, true, true))
                return;

            if (Program.CodeEditorProcess != null && !Program.CodeEditorProcess.HasExited)
                Program.CodeEditorProcess.Kill();

            Program.CodeEditorProcess = CCode.OpenCodeEditor(
                                                                (CCode.CodeEditorEnum)Enum.Parse(typeof(CCode.CodeEditorEnum), Combo_CodeEditor.SelectedItem.ToString()),
                                                                TextBox_CodeEditorPath.Text,
                                                                Textbox_CodeEditorArgs.Text.Replace("$CODEFILE", CCode.editHeaderFilePath.AppendQuotation()).Replace("$CODEFOLDER", CCode.tempFolderPath.AppendQuotation())
                                                            );

            if (Program.CodeEditorProcess == null)
                return;
            else
            {
                globalCHeaderToolStripMenuItem.Enabled = false;
                Button_CCompile.Enabled = false;
                Button_OpenCCode.Enabled = false;
                Button_UpdateCompile.Enabled = true;
                WatchFile(SelectedEntry);
            }
        }
    }
}
