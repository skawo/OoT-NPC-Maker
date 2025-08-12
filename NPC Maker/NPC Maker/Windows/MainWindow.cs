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
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data;
using FastColoredTextBoxNS;
using System.Drawing.Drawing2D;

namespace NPC_Maker
{
    public partial class MainWindow : Form
    {
        string OpenedPath = "";
        NPCFile EditedFile = null;
        NPCEntry SelectedEntry = null;
        int SelectedIndex = -1;
        string NPCSave = JsonConvert.SerializeObject(new NPCFile(), Formatting.Indented);
        private readonly System.Windows.Forms.Timer MsgPreviewTimer = new Timer();
        public static List<KeyValuePair<ComboBox, ComboBox>> FunctionComboBoxes;
        readonly Timer compileTimer;
        int LastSearchDepth = 0;
        int LastMsgCount = 0;
        string LastSearch = "";
        int ScrollToMsg = 0;
        readonly Timer messageSearchTimer = new Timer();
        private Timer autoBackupTimer = new Timer();
        string LastBackup = "";

        private Common.SavedMsgPreviewData lastPreviewData;
        private Common.SavedMsgPreviewData lastPreviewDataOrig;

        private Dictionary<string, float[]> fontsWidths = new Dictionary<string, float[]>();
        private Dictionary<string, byte[]> fonts = new Dictionary<string, byte[]>();

        public MainWindow(string FilePath = "")
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

            compileTimer = new Timer();
            compileTimer.Interval = 100;
            compileTimer.Tick += CompileTimer_Tick;

            autoBackupTimer.Interval = 2000;
            autoBackupTimer.Tick += AutoBackupTimer_Tick;

            Combo_CodeEditor.SelectedIndexChanged -= Combo_CodeEditor_SelectedIndexChanged;
            Combo_CodeEditor.Items.Clear();
            Combo_CodeEditor.Items.AddRange(Enum.GetNames(typeof(CCode.CodeEditorEnum)));
            Combo_CodeEditor.Text = Program.Settings.CodeEditor.ToString();
            Textbox_CodeEditorArgs.Text = Program.Settings.CustomCodeEditorArgs;
            TextBox_CodeEditorPath.Text = Program.Settings.CustomCodeEditorPath;
            Combo_CodeEditor.SelectedIndexChanged += Combo_CodeEditor_SelectedIndexChanged;

            Combo_CodeEditor_SelectedIndexChanged(null, null);

            MsgPreviewTimer.Interval = 100;
            MsgPreviewTimer.Tick += MsgPreviewTimer_Tick;
            MsgPreviewTimer.Stop();

            MessagesContextMenu.MakeContextMenu();
            MsgText.ContextMenuStrip = MessagesContextMenu.MenuStrip;
            MessagesContextMenu.SetTextBox(MsgText);

            chkBox_ShowDefaultLanguagePreview.Checked = Program.Settings.OrigPreview;

            FunctionComboBoxes = new List<KeyValuePair<ComboBox, ComboBox>>()
                        {
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnInit, null),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnUpdate, Combo_WhenOnUpdate ),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnDraw, Combo_WhenOnDraw ),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnLimb, null ),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_FuncOnDelete, null ),
                            new KeyValuePair<ComboBox, ComboBox>(Combo_postLimb, null ),
                        };

            this.DoubleBuffered = true;
            this.ResizeBegin += Form1_ResizeBegin;
            this.ResizeEnd += Form1_ResizeEnd;

            CodeParamsTooltip.SetToolTip(Textbox_CodeEditorArgs, "Available constants: $CODEFILE, $CODEHEADER, $CODEFOLDER");

            if (FilePath != "")
                OpenFile(FilePath);

            if (Program.IsRunningUnderMono)
            {
                Btn_MsgMoveUp.Text = "Up";
                Btn_MsgMoveDown.Text = "Dn";
            }

            splitContainer1_Panel1_SizeChanged(null, null);

        }

        private void splitContainer1_Panel1_SizeChanged(object sender, EventArgs e)
        {
            int width = SplitPanel.Panel1.Width;

            if (SplitPanel.Panel1.Width == 0)
                width = SplitPanel.SplitterDistance;

            int btnX = (width - 12) / 3;
            int btnXRow2 = (width - 12) / 2;

            Button_Add.Width = btnX;
            Button_Duplicate.Width = btnX;
            Button_Delete.Width = btnX;

            Button_Export.Width = btnXRow2;
            Button_Import.Width = btnXRow2;

            Button_Add.Location = new Point(2, Button_Add.Location.Y);
            Button_Duplicate.Location = new Point(width / 2 - btnX / 2, Button_Duplicate.Location.Y);
            Button_Delete.Location = new Point(width - btnX, Button_Delete.Location.Y);

            Button_Export.Location = new Point(2, Button_Export.Location.Y);
            Button_Import.Location = new Point(width - btnXRow2, Button_Import.Location.Y);

        }

        private void LoadAddFontByName(string FontName)
        {
            string fontf = $"{FontName}.font_static";
            string fontfW = $"{FontName}.width_table";

            string basePath = Path.GetDirectoryName(Program.JsonPath == "" ? Program.ExecPath : Program.JsonPath);

            string fontfP = Path.Combine(basePath, "font", fontf);
            string fontfWP = Path.Combine(basePath, "font", fontfW);

            string fontfDef = $"font.font_static";
            string fontfWDef = $"font.width_table";

            string fontfPDef = Path.Combine(basePath, "font", fontfDef);
            string fontfWPDef = Path.Combine(basePath, "font", fontfWDef);


            if (File.Exists(fontfP) && File.Exists(fontfWP))
            {
                fonts.Add(FontName, File.ReadAllBytes(fontfP));
                List<float> fontWidths = new List<float>();

                byte[] widths = System.IO.File.ReadAllBytes(fontfWP);

                for (int i = 0; i < widths.Length; i += 4)
                {
                    byte[] width = widths.Skip(i).Take(4).Reverse().ToArray();
                    fontWidths.Add(BitConverter.ToSingle(width, 0));
                }

                fontsWidths.Add(FontName, fontWidths.ToArray());
            }
            else if (File.Exists(fontfPDef) && File.Exists(fontfWPDef) && !fonts.ContainsKey(Dicts.DefaultLanguage))
            {
                fonts.Add(Dicts.DefaultLanguage, File.ReadAllBytes(fontfPDef));
                List<float> fontWidths = new List<float>();

                byte[] widths = System.IO.File.ReadAllBytes(fontfWPDef);

                for (int i = 0; i < widths.Length; i += 4)
                {
                    byte[] width = widths.Skip(i).Take(4).Reverse().ToArray();
                    fontWidths.Add(BitConverter.ToSingle(width, 0));
                }

                fontsWidths.Add(Dicts.DefaultLanguage, fontWidths.ToArray());
            }
        }

        private void ReloadAllFonts()
        {
            fonts.Clear();
            fontsWidths.Clear();
            LoadAddFontByName(Dicts.DefaultLanguage);

            foreach (string lan in EditedFile.Languages)
                LoadAddFontByName(lan);
        }

        private void SetupLanguageCombo()
        {
            Combo_Language.SelectedIndexChanged -= Combo_Language_SelectedIndexChanged;

            Combo_Language.Items.Clear();
            Combo_Language.Items.Add(Dicts.DefaultLanguage);

            foreach (string lan in EditedFile.Languages)
                Combo_Language.Items.Add(lan);

            ReloadAllFonts();
            Dicts.ReloadMsgTagOverrides(EditedFile.Languages);

            Combo_Language.SelectedIndex = 0;
            Combo_Language.SelectedIndexChanged += Combo_Language_SelectedIndexChanged;
            Combo_Language_SelectedIndexChanged(null, null);

        }

        private void OpenFile(string FilePath)
        {
            autoBackupTimer.Stop();
            EditedFile = FileOps.ParseNPCJsonFile(FilePath);
            NPCSave = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

            if (EditedFile != null)
            {
                OpenedPath = FilePath;
                Program.JsonPath = OpenedPath;
                Panel_Editor.Enabled = true;

                SetupLanguageCombo();

                InsertDataIntoActorListGrid();
                ChkBox_UseSpaceFont.Checked = EditedFile.SpaceFromFont;
                Program.Settings.LastOpenPath = FilePath;
                Dicts.LoadDicts();

                Program.Settings.GameVersion = EditedFile.GameVersion;
                autoBackupTimer.Start();
            }
        }

        private void AutoBackupTimer_Tick(object sender, EventArgs e)
        {
            autoBackupTimer.Stop();

            string CurrentBackup = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

            if (CurrentBackup != LastBackup)
            {
                FileOps.SaveNPCJSON("backup", EditedFile);
                LastBackup = CurrentBackup;
            }

            autoBackupTimer.Start();
        }

        private void CompileTimer_Tick(object sender, EventArgs e)
        {
            compileTimer.Stop();

            if (!Program.CompileInProgress)
            {
                MenuStrip.Enabled = true;
                Panel_Editor.Enabled = true;
                btn_FindMsg.Enabled = true;

                if (Program.CompileThereWereErrors)
                {
                    progressL.SetProgress(0, $"Compilation failed.");

                    if (Program.IsRunningUnderMono)
                        MessageBox.Show(Program.CompileMonoErrors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    TimeSpan s = DateTime.Now - Program.CompileStartTime;

                    if (s.Seconds == 0)
                        progressL.SetProgress(100, $"Completed in {s.Milliseconds} ms.");
                    else
                        progressL.SetProgress(100, $"Completed in {s.Seconds} s {s.Milliseconds} ms.");
                }
            }
            else
                compileTimer.Start();
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

                if (!String.Equals(CurrentFile, NPCSave))
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
            finally
            {
                if (File.Exists("backup"))
                    File.Delete("backup");
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
            Tx_HeaderPath.Text = SelectedEntry.HeaderPath;
            Tx_SkeletonName.Text = SelectedEntry.SkeletonHeaderDefinition;

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

            NumUpDown_AnimInterpFrames.Value = SelectedEntry.AnimInterpFrames;

            Checkbox_Omitted.Checked = SelectedEntry.Omitted;

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
                    DataGrid_Animations.Rows.Add(new object[] { Animation.Name, Animation.HeaderDefinition, Animation.FileStart < 0 ? "Same as main" : Animation.FileStart.ToString("X"), Animation.Address.ToString("X"), Animation.StartFrame, Animation.EndFrame, Animation.Speed, Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Animation.ObjID) });
            }

            #endregion

            #region Segments grid

            for (int j = 0; j < TabControl_Segments.TabPages.Count; j++)
            {
                DataGridView Grid = (TabControl_Segments.TabPages[j].Controls[0] as Controls.SegmentDataGrid).Grid;

                Grid.Rows.Clear();

                foreach (SegmentEntry Entry in SelectedEntry.Segments[j])
                    Grid.Rows.Add(Entry.Name, Entry.HeaderDefinition, Entry.FileStart < 0 ? "Same as main" : Entry.FileStart.ToString("X"), Entry.Address.ToString("X"), Dicts.GetStringFromStringIntDict(Dicts.ObjectIDs, Entry.ObjectID));
            }

            #endregion

            #region Display lists grid

            DataGridView_ExtraDLists.Rows.Clear();

            foreach (DListEntry Dlist in SelectedEntry.ExtraDisplayLists)
            {
                string SelCombo = ExtraDlists_ShowType.Items[(int)Dlist.ShowType].ToString();

                int Row = DataGridView_ExtraDLists.Rows.Add(new object[] { Dlist.Name,
                                                                           Dlist.HeaderDefinition,
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

            Tx_HeaderPath_TextChanged(null, null);

            #region Messages

            Combo_Language.SelectedIndexChanged -= Combo_Language_SelectedIndexChanged;
            MessagesGrid.SelectionChanged -= MessagesGrid_SelectionChanged;
            MessagesGrid.Rows.Clear();

            List<MessageEntry> MessageList = SelectedEntry.Messages;
            int LocalizationIndex = SelectedEntry.Localization.FindIndex(x => x.Language == Combo_Language.Text);

            if (Combo_Language.SelectedIndex != 0 && LocalizationIndex != -1)
                MessageList = SelectedEntry.Localization[LocalizationIndex].Messages;
            else
                Combo_Language.SelectedIndex = 0;

            foreach (MessageEntry Entry in MessageList)
                MessagesGrid.Rows.Add(new object[] { Entry.Name });

            MessagesGrid_SelectionChanged(MessagesGrid, null);
            MessagesGrid.SelectionChanged += MessagesGrid_SelectionChanged;
            Combo_Language.SelectedIndexChanged += Combo_Language_SelectedIndexChanged;

            #endregion

            #region CCode

            if (SelectedEntry.EmbeddedOverlayCode.Code != "" && Program.Settings.AutoComp_ActorSwitch)
            {
                string CompileErrors = "";
                CCode.Compile(EditedFile.CHeader, SelectedEntry.EmbeddedOverlayCode, ref CompileErrors);
                TextBox_CompileMsg.Text = CompileErrors;
            }
            else if (SelectedEntry.EmbeddedOverlayCode.Code != "")
                TextBox_CompileMsg.Text = "Click \"Compile\"!";
            else
                TextBox_CompileMsg.Text = "No code to compile.";

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

                if (!String.Equals(CurrentFile, NPCSave))
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
                OpenFile(OFD.FileName);
        }

        private void FileMenu_New_Click(object sender, EventArgs e)
        {
            if (SaveChangesAsPrompt() == false)
                return;

            EditedFile = new NPCFile();
            EditedFile.GlobalHeaders.AddRange(new List<ScriptEntry>() { Defaults.DefaultDefines, Defaults.DefaultMacros });
            SelectedEntry = null;

            Panel_Editor.Enabled = true;

            SetupLanguageCombo();
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
                NPCSave = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);
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
                NPCSave = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);
                FileOps.SaveNPCJSON(OpenedPath, EditedFile);
            }
        }

        private async void FileMenu_SaveBinary_Click(object sender, EventArgs e)
        {
            if (EditedFile == null || Program.CompileInProgress)
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

                IProgress<Common.ProgressReport> progress = new Microsoft.Progress<Common.ProgressReport>(n => progressL.NewProgress = n);
                progress.Report(new Common.ProgressReport("Starting...", 0));

                Program.CompileInProgress = true;
                this.MenuStrip.Enabled = false;
                this.Panel_Editor.Enabled = false;
                this.btn_FindMsg.Enabled = false;
                this.progressL.Visible = true;

                Program.CompileStartTime = DateTime.Now;
                Program.CompileThereWereErrors = false;

                compileTimer.Start();

                if (Program.Settings.CompileInParallel)
                {
                    await TaskEx.Run(() => { FileOps.PreprocessCodeAndScripts(SFD.FileName, EditedFile, progress); });
                }
                else
                {
                    await TaskEx.Run(() =>
                    {
                        bool[] caches = FileOps.GetCacheStatus(EditedFile);
                        string baseDefines = Scripts.ScriptHelpers.GetBaseDefines(EditedFile);
                        FileOps.SaveBinaryFile(SFD.FileName, EditedFile, progress, baseDefines, caches[0], caches[1]);
                        CCode.CleanupCompileArtifacts();
                    });
                }

                Program.Settings.LastSaveBinaryPath = SFD.FileName;
            }

            //InsertDataToEditor();
        }

        private void addNewLocalizationToolClick(object sender, EventArgs e)
        {
            if (EditedFile != null)
            {
                string Language = "";
                DialogResult DR = InputBox.ShowInputDialog("Language name?", ref Language);

                if (DR != DialogResult.OK)
                    return;

                if (EditedFile.Languages.Contains(Language) || Language == Dicts.DefaultLanguage)
                {
                    MessageBox.Show("Language already exists.");
                    return;
                }

                EditedFile.Languages.Add(Language);
                Combo_Language.Items.Add(Language);

                DialogResult Res = MessageBox.Show("Fill all actors with copies of the messages?", "Filling", MessageBoxButtons.YesNo);

                if (Res == DialogResult.Yes)
                {
                    foreach (NPCEntry entry in EditedFile.Entries)
                    {
                        LocalizationEntry newEntry = new LocalizationEntry();
                        newEntry.Language = Language;

                        foreach (MessageEntry me in entry.Messages)
                            newEntry.Messages.Add(Helpers.Clone<MessageEntry>(me));

                        entry.Localization.Add(newEntry);
                    }
                }

                ReloadAllFonts();
                Dicts.ReloadMsgTagOverrides(EditedFile.Languages);

            }
        }

        private void removeLocalizationToolClick(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            if (EditedFile.Languages.Count == 0)
            {
                MessageBox.Show("There are no localizations.");
                return;
            }

            Windows.ComboPicker pick = new Windows.ComboPicker(EditedFile.Languages, "Which language?", "Language selection");

            try
            {
                if (pick.ShowDialog() == DialogResult.OK)
                {

                    if (EditedFile != null)
                    {
                        DialogResult Res = MessageBox.Show("Are you sure? All messages of that language will be removed.", "Confirmation", MessageBoxButtons.YesNoCancel);

                        if (Res == DialogResult.Yes)
                        {
                            EditedFile.Languages.Remove(pick.SelectedOption);

                            foreach (NPCEntry entry in EditedFile.Entries)
                                entry.Localization.RemoveAll(x => x.Language == pick.SelectedOption);

                            Combo_Language.Items.Remove(pick.SelectedOption);

                            ReloadAllFonts();
                            Dicts.ReloadMsgTagOverrides(EditedFile.Languages);
                            Combo_Language.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
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

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
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

            if (EditedFile != null)
                EditedFile.GameVersion = Program.Settings.GameVersion;

            MsgText_TextChanged(null, null);
        }

        private void DocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/skawo/OoT-NPC-Maker/wiki");
        }

        private void GlobalCHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

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
                                                                Textbox_CodeEditorArgs.Text.Replace("$CODEFILE", CCode.geditHeaderFilePath.AppendQuotation()).Replace("$CODEFOLDER", CCode.gtempFolderPath.AppendQuotation())
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


        #endregion

        #region Tools

        private void checkDefinitionValidityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            foreach (NPCEntry entry in EditedFile.Entries)
            {
                if (!String.IsNullOrEmpty(entry.HeaderPath))
                {
                    Dictionary<string, string> hDict = Helpers.GetDefinesFromH(entry.HeaderPath);

                    entry.Hierarchy = FileOps.ResolveHeaderDefineForFieldOrFail(entry.NPCName, entry.SkeletonHeaderDefinition, hDict, entry.Hierarchy);

                    foreach (var a in entry.Animations)
                        a.Address = FileOps.ResolveHeaderDefineForFieldOrFail(entry.NPCName, a.HeaderDefinition, hDict, a.Address);

                    foreach (var d in entry.ExtraDisplayLists)
                        d.Address = FileOps.ResolveHeaderDefineForFieldOrFail(entry.NPCName, d.HeaderDefinition, hDict, d.Address);

                    foreach (var s in entry.Segments)
                    {
                        foreach (var se in s)
                            se.Address = FileOps.ResolveHeaderDefineForFieldOrFail(entry.NPCName, se.HeaderDefinition, hDict, se.Address);
                    }
                }
            }

            MessageBox.Show("Check complete. All valid fields have been updated.");
            InsertDataToEditor();
        }

        private void checkLocalizationConsistencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile.Languages.Count == 0)
            {
                MessageBox.Show("There are no localizations.");
                return;
            }

            Windows.ComboPicker pick = new Windows.ComboPicker(EditedFile.Languages, "Which language?", "Language selection");

            string report = "";

            try
            {

                if (pick.ShowDialog() == DialogResult.OK)
                {
                    string SelectedLanguage = pick.SelectedOption;
                    int SelectedLangIndex = pick.SelectedIndex;

                    foreach (NPCEntry ent in EditedFile.Entries)
                    {
                        if (ent.Localization.FindIndex(x => x.Language == SelectedLanguage) != -1)
                        {
                            for (int i = 0; i < ent.Messages.Count; i++)
                            {
                                byte[] msgData = ent.Messages[i].ConvertTextData(ent.NPCName, Dicts.DefaultLanguage, false).ToArray();
                                ZeldaMessage.MessagePreview pp = new ZeldaMessage.MessagePreview(ZeldaMessage.Data.BoxType.Black, msgData);

                                byte[] msgDataLoc = ent.Localization[SelectedLangIndex].Messages[i].ConvertTextData(ent.NPCName, SelectedLanguage, false).ToArray();
                                ZeldaMessage.MessagePreview pp2 = new ZeldaMessage.MessagePreview(ZeldaMessage.Data.BoxType.Black, msgDataLoc);

                                if (pp.MessageCount != pp2.MessageCount)
                                    report += $"{ent.NPCName}, {ent.Messages[i].Name} {pp.MessageCount} vs {pp2.MessageCount}{Environment.NewLine}";
                            }
                        }
                        else
                            report += $"{ent.NPCName} does not contain this language.{Environment.NewLine}";
                    }
                }

                SaveFileDialog sf = new SaveFileDialog();
                sf.FileName = "report.txt";

                if (sf.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sf.FileName, report);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void importLocalizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile != null)
            {

                OpenFileDialog OFD = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(Program.Settings.LastOpenPath),
                    RestoreDirectory = true,
                };

                DialogResult DR = OFD.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    try
                    {
                        NPCFile LocalizationFile = FileOps.ParseNPCJsonFile(OFD.FileName);

                        List<string> LanguagesWithDefault = new List<string>() { Dicts.DefaultLanguage };
                        LanguagesWithDefault.AddRange(LocalizationFile.Languages);

                        Windows.ComboPicker pick = new Windows.ComboPicker(LanguagesWithDefault, "Import which language?", "Language selection");

                        if (pick.ShowDialog() == DialogResult.OK)
                        {
                            string SelectedLanguage = pick.SelectedOption;
                            int SelectedLangIndex = pick.SelectedIndex;

                            int IndexInCur = EditedFile.Languages.FindIndex(x => x == SelectedLanguage);

                            if (IndexInCur != -1 || SelectedLanguage == Dicts.DefaultLanguage)
                            {
                                DialogResult Res = MessageBox.Show("This language already exists. Replace it?", "Confirmation", MessageBoxButtons.YesNoCancel);

                                if (Res != DialogResult.Yes)
                                    return;
                            }
                            else
                            {
                                EditedFile.Languages.Add(SelectedLanguage);
                            }

                            DialogResult y2aRes = DialogResult.None;

                            foreach (NPCEntry entry in EditedFile.Entries)
                            {
                                int importIndex = LocalizationFile.Entries.FindIndex(x => x.NPCName == entry.NPCName);

                                if (importIndex == -1)
                                    continue;

                                NPCEntry ImportedEntry = LocalizationFile.Entries[importIndex];

                                if (ImportedEntry.Localization.FindIndex(x => x.Language == SelectedLanguage) == -1)
                                    continue;
                                else if (SelectedLanguage != Dicts.DefaultLanguage)
                                {
                                    IndexInCur = entry.Localization.FindIndex(x => x.Language == SelectedLanguage);

                                    if (IndexInCur == -1)
                                    {
                                        LocalizationEntry newlocEntry = new LocalizationEntry();
                                        newlocEntry.Language = SelectedLanguage;

                                        foreach (var msg in entry.Messages)
                                            newlocEntry.Messages.Add(Helpers.Clone<MessageEntry>(msg));

                                        entry.Localization.Add(newlocEntry);
                                        IndexInCur = entry.Localization.Count - 1;
                                    }
                                }

                                LocalizationEntry newLocalization = new LocalizationEntry();
                                newLocalization.Language = SelectedLanguage;

                                List<MessageEntry> messageList = ImportedEntry.Messages;

                                if (SelectedLangIndex != 0)
                                    messageList = ImportedEntry.Localization[SelectedLangIndex - 1].Messages;

                                foreach (MessageEntry msg in entry.Messages)
                                {
                                    int importMsgIndex = messageList.FindIndex(x => x.Name == msg.Name);

                                    int curlocMsgIndex = -1;

                                    if (SelectedLangIndex != 0)
                                        curlocMsgIndex = entry.Localization[IndexInCur].Messages.FindIndex(x => x.Name == msg.Name);

                                    if (importMsgIndex != -1)
                                    {
                                        if (curlocMsgIndex != -1)
                                        {
                                            string textDefault = entry.Messages[curlocMsgIndex].MessageText;
                                            string text = entry.Localization[IndexInCur].Messages[curlocMsgIndex].MessageText;
                                            string textNew = messageList[importMsgIndex].MessageText;

                                            if (textDefault != textNew && textDefault != text && text != textNew)
                                            {
                                                if (y2aRes != DialogResult.OK && y2aRes != DialogResult.Ignore)
                                                {
                                                    var w = new Windows.YesNoAllBox($"Localization of textbox {msg.Name} is already different. Update it with the one from the file?", "Textbox already translated");
                                                    y2aRes = w.ShowDialog();
                                                }

                                                if (y2aRes == DialogResult.Yes || y2aRes == DialogResult.OK)
                                                {
                                                    MessageEntry import = messageList[importMsgIndex];
                                                    newLocalization.Messages.Add(import);
                                                }
                                                else
                                                {
                                                    newLocalization.Messages.Add(entry.Localization[IndexInCur].Messages[curlocMsgIndex]);
                                                }
                                            }
                                            else if (textDefault != text && textDefault == textNew)
                                            {
                                                newLocalization.Messages.Add(entry.Localization[IndexInCur].Messages[curlocMsgIndex]);
                                            }
                                            else
                                            {
                                                MessageEntry import = messageList[importMsgIndex];
                                                newLocalization.Messages.Add(import);
                                            }
                                        }
                                        else
                                        {
                                            MessageEntry import = messageList[importMsgIndex];
                                            newLocalization.Messages.Add(import);
                                        }
                                    }
                                    else
                                    {
                                        newLocalization.Messages.Add(Helpers.Clone<MessageEntry>(msg));
                                    }
                                }

                                if (SelectedLangIndex == 0)
                                    entry.Messages = newLocalization.Messages;
                                else
                                    entry.Localization[IndexInCur] = newLocalization;

                                List<MessageEntry> diff = messageList.Where(item2 => !entry.Messages.Any(item1 => item1.Name == item2.Name)).ToList();

                                foreach (MessageEntry msg in diff)
                                {
                                    int msgIndex = messageList.IndexOf(msg);

                                    MessageEntry msgN = Helpers.Clone<MessageEntry>(msg);

                                    if (SelectedLangIndex != 0)
                                    {
                                        msgN.MessageText = "";
                                        msgN.MessageTextLines.Clear();
                                    }

                                    entry.Messages.Insert(msgIndex, msgN);

                                    foreach (var loc in entry.Localization)
                                    {
                                        msgN = Helpers.Clone<MessageEntry>(msg);

                                        if (loc.Language != SelectedLanguage)
                                        {
                                            msgN.MessageText = "";
                                            msgN.MessageTextLines.Clear();
                                        }

                                        loc.Messages.Insert(msgIndex, Helpers.Clone<MessageEntry>(msgN));
                                    }
                                }
                            }

                            InsertDataToEditor();
                            SetupLanguageCombo();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show($"Failed to read JSON: {ex.Message}");
                    }
                }
            }
        }

        #endregion

        #region NPCList

        private void NPCGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            if (e.ColumnIndex == -1 && e.RowIndex >= 0)
            {
                var headerStyle = dgv.RowHeadersDefaultCellStyle;
                using (SolidBrush backBrush = new SolidBrush(headerStyle.BackColor))
                {
                    e.Graphics.FillRectangle(backBrush, e.CellBounds);
                }

                ControlPaint.DrawBorder3D(e.Graphics, e.CellBounds, Border3DStyle.RaisedInner);

                string rowNumber = e.RowIndex.ToString();

                using (SolidBrush textBrush = new SolidBrush(headerStyle.ForeColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    e.Graphics.DrawString(rowNumber, headerStyle.Font, textBrush, e.CellBounds, sf);
                }

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (EditedFile != null)
                {
                    NPCEntry entry = EditedFile.Entries[e.RowIndex];

                    if (entry.IsNull)
                    {
                        var headerStyle = dgv.RowHeadersDefaultCellStyle;

                        Color c = headerStyle.BackColor;

                        if (dgv.SelectedRows.Count != 0 && dgv.SelectedRows[0].Index == e.RowIndex)
                            c = headerStyle.SelectionBackColor;

                        using (SolidBrush backBrush = new SolidBrush(c))
                        {
                            e.Graphics.FillRectangle(backBrush, e.CellBounds);
                        }

                        ControlPaint.DrawBorder3D(e.Graphics, e.CellBounds, Border3DStyle.RaisedInner);

                        e.Handled = true;
                    }

                }

            }
        }
        private void Button_Export_Click(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
            {
                SaveFileDialog SFD = new SaveFileDialog
                {
                    InitialDirectory = Path.GetDirectoryName(Program.Settings.LastOpenPath),
                    RestoreDirectory = true,
                    FileName = $"{SelectedEntry.NPCName}.json",
                    Filter = "Json Files | *.json"
                };

                DialogResult DR = SFD.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    NPCSave = JsonConvert.SerializeObject(SelectedEntry, Formatting.Indented);
                    File.WriteAllText(SFD.FileName, NPCSave);

                    Program.Settings.LastOpenPath = SFD.FileName;
                }
            }

        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
            {
                OpenFileDialog OFD = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(Program.Settings.LastOpenPath),
                    RestoreDirectory = true,
                };

                DialogResult DR = OFD.ShowDialog();

                if (DR == DialogResult.OK)
                {
                    try
                    {
                        string Text = File.ReadAllText(OFD.FileName);
                        NPCEntry Deserialized = JsonConvert.DeserializeObject<NPCEntry>(Text);

                        int indexSame = EditedFile.Entries.FindIndex(x => x.NPCName.ToUpper() == Deserialized.NPCName.ToUpper());

                        while (indexSame != -1 && indexSame != SelectedIndex)
                        {
                            Deserialized.NPCName += "_";
                            indexSame = EditedFile.Entries.FindIndex(x => x.NPCName.ToUpper() == Deserialized.NPCName.ToUpper());
                        }

                        for (int i = 0; i < Deserialized.Messages.Count(); i++)
                        {
                            MessageEntry msg = Deserialized.Messages[i];

                            foreach (LocalizationEntry loc in Deserialized.Localization)
                            {
                                int index = loc.Messages.FindIndex(x => x.Name == msg.Name);

                                if (index != i)
                                {
                                    MessageBox.Show($"NPC is malformed: Localization does not match default messages.");
                                    break;
                                }
                            }
                        }

                        List<LocalizationEntry> newLoc = new List<LocalizationEntry>();

                        foreach (string Language in EditedFile.Languages)
                        {
                            int LocalizeIndex = Deserialized.Localization.FindIndex(x => x.Language == Language);

                            if (LocalizeIndex == -1)
                            {
                                LocalizationEntry newLocEntry = new LocalizationEntry();
                                newLocEntry.Language = Language;

                                foreach (MessageEntry msg in Deserialized.Messages)
                                    newLocEntry.Messages.Add(msg);

                                newLoc.Add(newLocEntry);
                            }
                            else
                            {
                                newLoc.Add(Deserialized.Localization[LocalizeIndex]);
                            }
                        }

                        Deserialized.Localization = newLoc;
                        EditedFile.Entries[SelectedIndex] = Deserialized;
                        SelectedEntry = Deserialized;

                        InsertDataToEditor();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show($"Failed to read NPC Entry JSON: {ex.Message}");
                    }

                }
            }
        }

        private void NpcsFilter_TextChanged(object sender, EventArgs e)
        {
            int index = 0;

            foreach (NPCEntry entry in EditedFile.Entries)
            {
                if (String.IsNullOrWhiteSpace(NpcsFilter.Text))
                    DataGrid_NPCs.Rows[index].Visible = true;
                else if (entry.NPCName.ToUpper().Contains(NpcsFilter.Text.ToUpper()))
                    DataGrid_NPCs.Rows[index].Visible = true;
                else
                    DataGrid_NPCs.Rows[index].Visible = false;

                index++;
            }
        }

        private NPCEntry GetNewNPCEntry()
        {
            NPCEntry Entry = new NPCEntry();
            Entry.Animations.Add(new AnimationEntry("Idle", "", 0, 1.0f, -1, 0, 255, -1));
            Entry.Animations.Add(new AnimationEntry("Walking", "", 0, 1.0f, -1, 0, 255, -1));
            Entry.Animations.Add(new AnimationEntry("Attacked", "", 0, 1.0f, -1, 0, 255, -1));

            for (int i = 0; i < 8; i++)
                Entry.Segments.Add(new List<SegmentEntry>());

            Entry.Scripts.Add(new ScriptEntry() { Name = "Script" });
            return Entry;
        }

        private void Button_Add_Click(object sender, EventArgs e)
        {
            NPCEntry Entry = GetNewNPCEntry();
            Entry.NPCName = $"NPC_{EditedFile.Entries.Count}";

            string Title = Entry.NPCName;
            DialogResult DR = InputBox.ShowInputDialog("NPC Name?", ref Title);

            if (DR != DialogResult.OK)
                return;

            if (!SanitizeName(ref Title))
                return;

            int indexSame = EditedFile.Entries.FindIndex(x => x.NPCName.ToUpper() == Title.ToUpper());

            if (indexSame != -1)
            {
                MessageBox.Show("NPC with that name already exists.");
                return;
            }

            Entry.NPCName = Title;
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

        private void Btn_HeaderBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Header files (*.h)|*.h|All files (*.*)|*.*";
            of.InitialDirectory = Program.Settings.ProjectPath;

            if (of.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(Program.Settings.ProjectPath))
                    Tx_HeaderPath.Text = of.FileName;
                else
                    Tx_HeaderPath.Text = Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, of.FileName, "{PROJECTPATH}");
            }
        }

        private Common.HDefine SelectNameFromH()
        {
            if (SelectedEntry == null || String.IsNullOrEmpty(SelectedEntry.HeaderPath))
                return null;

            Dictionary<string, string> hDict = Helpers.GetDefinesFromH(SelectedEntry.HeaderPath);

            if (hDict.Count == 0)
                return null;

            Windows.ComboPicker com = new Windows.ComboPicker(hDict.Keys.ToList(), "Select symbol from header...", "Selection", true);

            if (com.ShowDialog() == DialogResult.OK)
                return new Common.HDefine(com.SelectedOption, hDict[com.SelectedOption]);
            else
                return null;
        }

        private bool ShowHDefineError(Common.HDefine hD)
        {
            if (hD.Value == null)
            {
                MessageBox.Show("Error parsing defined value");
                return true;
            }

            return false;
        }

        private void Tx_SkeletonName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Common.HDefine hD = SelectNameFromH();

            if (hD != null)
            {
                if (!ShowHDefineError(hD))
                {
                    Tx_SkeletonName.Text = hD.Name;
                    NumUpDown_Hierarchy.Value = (decimal)hD.Value;
                }
            }
        }

        private void Tx_HeaderPath_TextChanged(object sender, EventArgs e)
        {
            if (sender != null)
                TextBox_TextChanged(sender, e);

            bool vis = true;

            if (String.IsNullOrEmpty(Tx_HeaderPath.Text))
            {
                vis = false;
            }

            if (vis)
                NumUpDown_Hierarchy.Width = numUpFileStart.Width / 2;
            else
                NumUpDown_Hierarchy.Width = numUpFileStart.Width;

            Tx_SkeletonName.Visible = vis;
            Col_HDefine.Visible = vis;
            ExtraDlists_HeaderDefinition.Visible = vis;

            for (int j = 0; j < TabControl_Segments.TabPages.Count; j++)
            {
                DataGridView Grid = (TabControl_Segments.TabPages[j].Controls[0] as Controls.SegmentDataGrid).Grid;
                Grid.Columns[(int)SegmentsColumns.HeaderDefinition].Visible = vis;
            }
        }


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

        private void Button_NPCRename_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null)
                return;

            string Title = SelectedEntry.NPCName;

            if (InputBox.ShowInputDialog("New NPC Name?", ref Title) != DialogResult.OK)
                return;

            if (!SanitizeName(ref Title))
                return;

            int indexSame = EditedFile.Entries.FindIndex(x => x.NPCName.ToUpper() == Title.ToUpper());

            if (indexSame != -1 && indexSame != SelectedIndex)
            {
                MessageBox.Show("NPC with that name already exists.");
                return;
            }

            Textbox_NPCName.Text = Title;
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
            Col_Filestart.Visible = (ComboBox_AnimType.SelectedIndex == 0);
            Col_HDefine.Visible = (ComboBox_AnimType.SelectedIndex == 0);
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
            HeaderDefinition = 1,
            FileStart = 2,
            Address = 3,
            StartFrame = 4,
            EndFrame = 5,
            Speed = 6,
            Object = 7,
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
            else if (e.ColumnIndex == (int)AnimGridColumns.HeaderDefinition && SelectedEntry.AnimationType == 0)
            {
                Common.HDefine hD = SelectNameFromH();

                if (hD != null)
                {
                    if (!ShowHDefineError(hD))
                    {
                        DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hD.Name;
                        DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, hD.Name, e.GetType(), null));
                        DataGrid_Animations.Rows[e.RowIndex].Cells[(int)AnimGridColumns.Address].Value = hD.ValueString;
                        DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)AnimGridColumns.Address, hD.ValueString, e.GetType(), null));
                        DataGrid_Animations.RefreshEdit();
                    }
                }
            }
        }

        private void AddBlankAnim(int SkipIndex, int Index, string Name = null, string HeaderName = null, uint? Address = null, float? Speed = null, short? ObjectID = null, byte StartFrame = 0, byte EndFrame = 0xFF, Int32? FileStart = null)
        {
            Name = Name ?? "Animation_" + Index.ToString();
            HeaderName = HeaderName ?? "";
            Address = Address ?? 0;
            Speed = Speed ?? 1;
            ObjectID = ObjectID ?? -1;
            FileStart = FileStart ?? -1;

            SelectedEntry.Animations.Add(new AnimationEntry(Name, HeaderName, (uint)Address, (float)Speed, (short)ObjectID, StartFrame, EndFrame, (int)FileStart));

            if (SkipIndex != (int)AnimGridColumns.Name)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Name].Value = Name;

            if (SkipIndex != (int)AnimGridColumns.HeaderDefinition)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.HeaderDefinition].Value = HeaderName;

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
                case (int)AnimGridColumns.HeaderDefinition:
                    {
                        string Name = e.Value.ToString();
                        e.Value = Name;

                        if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                            AddBlankAnim(e.ColumnIndex, e.RowIndex, null, e.Value.ToString());
                        else
                            SelectedEntry.Animations[e.RowIndex].HeaderDefinition = e.Value.ToString();

                        if (Name != "")
                        {
                            Dictionary<string, string> hDict = Helpers.GetDefinesFromH(SelectedEntry.HeaderPath);
                            Common.HDefine hD = Helpers.GetDefineFromName(Name, hDict);

                            if (hD != null && hD.Value != null)
                            {
                                SelectedEntry.Animations[e.RowIndex].Address = (UInt32)hD.Value;
                                DataGrid_Animations.Rows[e.RowIndex].Cells[(int)AnimGridColumns.Address].Value = hD.ValueString;
                            }
                        }

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
                                    AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, (UInt32)LinkAnim);
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
                                    AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, Convert.ToUInt32(e.Value.ToString(), 16));
                                else
                                    SelectedEntry.Animations[e.RowIndex].Address = Convert.ToUInt32(e.Value.ToString(), 16);
                            }
                            catch (Exception)
                            {
                                if (SelectedEntry.Animations.Count() - 1 < e.RowIndex)
                                    AddBlankAnim(e.ColumnIndex, e.RowIndex);

                                e.Value = Convert.ToInt32("0", 16);
                                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                            }
                            finally
                            {
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
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, (float)Convert.ToDecimal(e.Value));
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
                                AddBlankAnim(e.ColumnIndex, e.RowIndex, null, null, null, null, null, 0, 255, Value);
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
            HeaderDefinition = 1,
            Color = 2,
            FileStart = 3,
            Offset = 4,
            Translation = 5,
            Rotation = 6,
            Scale = 7,
            Limb = 8,
            Object = 9,
            ShowType = 10,
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
            else if (e.ColumnIndex == (int)EDlistsColumns.HeaderDefinition)
            {
                Common.HDefine hD = SelectNameFromH();

                if (hD != null)
                {
                    if (!ShowHDefineError(hD))
                    {
                        DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hD.Name;
                        DataGridView_ExtraDLists_CellParsing(DataGridView_ExtraDLists, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, hD.Name, e.GetType(), null));
                        DataGridView_ExtraDLists.Rows[e.RowIndex].Cells[(int)EDlistsColumns.Offset].Value = hD.ValueString;
                        DataGridView_ExtraDLists_CellParsing(DataGridView_ExtraDLists, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)EDlistsColumns.Offset, hD.ValueString, e.GetType(), null));
                        DataGridView_ExtraDLists.RefreshEdit();
                    }
                }
            }
        }

        private void AddBlankDList(int SkipIndex, int Index, string Name = null, string HeaderDefinition = null, uint? Address = null, float? TransX = null, float? TransY = null, float? TransZ = null,
                                   short? RotX = null, short? RotY = null, short? RotZ = null, float? Scale = null, short? Limb = null, int? ShowType = null, short? ObjectID = null, Color? EnvColor = null, int? FileStart = null)
        {
            if (Name == null)
                Name = "DList_" + Index;

            Name = Name ?? "DList_" + Index;
            HeaderDefinition = HeaderDefinition ?? "";
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

            SelectedEntry.ExtraDisplayLists.Add(new DListEntry(Name, HeaderDefinition, (uint)Address, (float)TransX, (float)TransY, (float)TransZ, (Color)EnvColor,
                                                    (short)RotX, (short)RotY, (short)RotZ, (float)Scale, (short)Limb, (int)ShowType, (short)ObjectID, (int)FileStart));

            if (SkipIndex != (int)EDlistsColumns.Purpose)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.Purpose].Value = Name;

            if (SkipIndex != (int)EDlistsColumns.HeaderDefinition)
                DataGridView_ExtraDLists.Rows[Index].Cells[(int)EDlistsColumns.HeaderDefinition].Value = Name;

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
                case (int)EDlistsColumns.HeaderDefinition:
                    {
                        string Name = e.Value.ToString();
                        e.Value = Name;

                        if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, e.Value.ToString());
                        else
                            SelectedEntry.ExtraDisplayLists[e.RowIndex].HeaderDefinition = e.Value.ToString();

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)EDlistsColumns.Color:
                    {
                        if (SelectedEntry.ExtraDisplayLists.Count() - 1 < e.RowIndex)
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, null, null, (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor);
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
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, Convert.ToUInt32(e.Value.ToString(), 16));
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
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, Transl[0], Transl[1], Transl[2]);
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
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, Rot[0], Rot[1], Rot[2]);
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
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, (float)Convert.ToDecimal(e.Value));
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
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, Convert.ToInt16(e.Value));
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
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, null, ObjectId);
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
                            AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, ShowType);
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
                                AddBlankDList(e.ColumnIndex, e.RowIndex, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Value);
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
            HeaderDefinition = 1,
            FileStart = 2,
            Address = 3,
            Object = 4,
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
            else if (e.ColumnIndex == (int)SegmentsColumns.HeaderDefinition)
            {
                Common.HDefine hD = SelectNameFromH();

                if (hD != null)
                {
                    if (!ShowHDefineError(hD))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hD.Name;
                        DataGridViewSegments_CellParse(sender, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, hD.Name, e.GetType(), null));
                        (sender as DataGridView).Rows[e.RowIndex].Cells[(int)SegmentsColumns.Address].Value = hD.ValueString;
                        DataGridViewSegments_CellParse(sender, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)SegmentsColumns.Address, hD.ValueString, e.GetType(), null));
                        (sender as DataGridView).Update();
                    }
                }
            }
        }

        private void AddBlankSeg(int SkipIndex, int Index, int Segment, string Name = null, string HeaderDefinition = null, uint? Address = null, short? ObjectID = null, int? FileStart = null)
        {
            Name = Name ?? "Texture_" + Index.ToString();
            HeaderDefinition = HeaderDefinition ?? "";
            Address = Address ?? 0;
            ObjectID = ObjectID ?? -1;
            FileStart = FileStart ?? -1;

            SelectedEntry.Segments[Segment].Add(new SegmentEntry(Name, HeaderDefinition, (uint)Address, (short)ObjectID, (int)FileStart));

            DataGridView dgv = (TabControl_Segments.TabPages[Segment].Controls[0] as Controls.SegmentDataGrid).Grid;

            if (SkipIndex != (int)SegmentsColumns.Name)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.Name].Value = Name;

            if (SkipIndex != (int)SegmentsColumns.HeaderDefinition)
                dgv.Rows[Index].Cells[(int)SegmentsColumns.HeaderDefinition].Value = Name;

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
                case (int)SegmentsColumns.HeaderDefinition:
                    {
                        string Name = e.Value.ToString();
                        e.Value = Name;

                        if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                            AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, e.Value.ToString());
                        else
                            SelectedEntry.Segments[DataGridIndex][e.RowIndex].HeaderDefinition = e.Value.ToString();

                        e.ParsingApplied = true;
                        return;
                    }
                case (int)SegmentsColumns.Address:
                    {
                        try
                        {
                            if (SelectedEntry.Segments[DataGridIndex].Count() - 1 < e.RowIndex)
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, null, Convert.ToUInt32(e.Value.ToString(), 16));
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
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, null, null, ObjectId);
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
                                AddBlankSeg(e.ColumnIndex, e.RowIndex, DataGridIndex, null, null, null, null, Value);
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

        private void SplitMsgContainer_SizeChanged(object sender, EventArgs e)
        {
            if (Program.IsRunningUnderMono)
            {
                SplitMsgContainer.Width = PanelMsgPreview.Width;
                SplitMsgContainer.Height = PanelMsgPreview.Location.Y - SplitMsgContainer.Location.Y - 10;
            }
        }

        private void MessagesGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex == -1 && e.RowIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;

                var headerStyle = dgv.RowHeadersDefaultCellStyle;
                using (SolidBrush backBrush = new SolidBrush(headerStyle.BackColor))
                {
                    e.Graphics.FillRectangle(backBrush, e.CellBounds);
                }

                ControlPaint.DrawBorder3D(e.Graphics, e.CellBounds, Border3DStyle.RaisedInner);

                string rowNumber = e.RowIndex.ToString();

                using (SolidBrush textBrush = new SolidBrush(headerStyle.ForeColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    e.Graphics.DrawString(rowNumber, headerStyle.Font, textBrush, e.CellBounds, sf);
                }

                e.Handled = true;
            }
        }

        private void Btn_RemoveLanguage_Click(object sender, EventArgs e)
        {
            if (SelectedEntry != null && Combo_Language.SelectedIndex != 0)
            {
                DialogResult Res = MessageBox.Show("Are you sure? All messages of that language within this actor will be removed.", "Confirmation", MessageBoxButtons.YesNo);

                if (Res == DialogResult.Yes)
                {
                    SelectedEntry.Localization.RemoveAll(x => x.Language == Combo_Language.Text);
                    Combo_Language.SelectedIndex = 0;
                }
            }
        }

        private void Combo_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
            {
                string SelLang = Combo_Language.Text;

                if (SelLang != Dicts.DefaultLanguage)
                {
                    if (SelectedEntry.Localization.FindIndex(x => x.Language == SelLang) == -1)
                    {
                        DialogResult Res = MessageBox.Show("This actor doesn't contain this language. Create it?", "Confirmation", MessageBoxButtons.YesNo);

                        if (Res == DialogResult.Yes)
                        {
                            LocalizationEntry newEntry = new LocalizationEntry();
                            newEntry.Language = SelLang;

                            foreach (MessageEntry me in SelectedEntry.Messages)
                                newEntry.Messages.Add(Helpers.Clone<MessageEntry>(me));

                            SelectedEntry.Localization.Add(newEntry);
                        }
                        else
                        {
                            Combo_Language.SelectedIndex = 0;
                            return;
                        }
                    }
                }
            }
            else
                return;

            int curSelMsg = 0;

            lastPreviewData = null;
            lastPreviewDataOrig = null;

            if (MessagesGrid.SelectedRows.Count != 0)
                curSelMsg = MessagesGrid.SelectedRows[0].Index;

            if (Combo_Language.SelectedIndex != 0)
            {
                SplitMsgContainer.Panel1Collapsed = false;
                SplitMsgContainer.Panel1MinSize = 25;
                SplitMsgContainer.Panel2MinSize = 25;
                SplitMsgContainer.SplitterDistance = SplitMsgContainer.Width / 2;
                SplitMsgContainer.IsSplitterFixed = false;
            }
            else
            {
                SplitMsgContainer.Panel1Collapsed = true;
                SplitMsgContainer.Panel1MinSize = 0;
                SplitMsgContainer.Panel2MinSize = 0;
                SplitMsgContainer.SplitterDistance = 0;
                SplitMsgContainer.IsSplitterFixed = true;
            }

            InsertDataToEditor();

            if (MessagesGrid.Rows.Count > curSelMsg)
            {
                MessagesGrid.Rows[curSelMsg].Selected = true;

                if (Program.IsRunningUnderMono)
                {
                    ScrollToMsg = curSelMsg;
                    messageSearchTimer.Interval = 10;
                    messageSearchTimer.Tick += MessageSearchTimer_Tick;
                    messageSearchTimer.Stop();
                    messageSearchTimer.Start();
                }
                else
                {
                    MessagesGrid.FirstDisplayedScrollingRowIndex = curSelMsg;
                }
            }

            MsgPreviewTimer_Tick(null, null);
        }

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

                    foreach (LocalizationEntry l in SelectedEntry.Localization)
                    {
                        MessageEntry localMsg = l.Messages[i];
                        l.Messages.RemoveAt(i);
                        l.Messages.Insert(i - 1, localMsg);
                    }

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

                    foreach (LocalizationEntry l in SelectedEntry.Localization)
                    {
                        MessageEntry localMsg = l.Messages[i];
                        l.Messages.RemoveAt(i);
                        l.Messages.Insert(i + 1, localMsg);
                    }


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
            Panel p = Combo_Language.SelectedIndex == 0 ? PreviewSplitContainer.Panel2 : PreviewSplitContainer.Panel1;

            if (Type == (int)ZeldaMessage.Data.BoxType.None_White)
                p.BackColor = Color.Black;
            else
                p.BackColor = Color.White;
        }

        private List<MessageEntry> GetLanguageMessageList(NPCEntry entry, string Language)
        {
            List<MessageEntry> MessageList = entry.Messages;

            if (Language != Dicts.DefaultLanguage)
            {
                int LocalizationIndex = entry.Localization.FindIndex(x => x.Language == Language);

                if (LocalizationIndex != -1)
                    MessageList = entry.Localization[LocalizationIndex].Messages;
            }

            return MessageList;
        }

        private void MessagesGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (SelectedEntry == null)
                return;

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
                int index = MessagesGrid.SelectedRows[0].Index;

                if (index >= MessagesGrid.RowCount)
                    index = MessagesGrid.RowCount - 1;

                List<MessageEntry> MessageList = GetLanguageMessageList(SelectedEntry, Combo_Language.Text);

                MessageEntry Entry = MessageList[index];

                MsgText.Text = Entry.MessageText;
                MsgText.ClearUndo();

                MessageEntry DefaultEntry = SelectedEntry.Messages[index];
                MsgTextDefault.Text = DefaultEntry.MessageText;
                MsgTextDefault.ClearUndo();


                Combo_MsgType.SelectedIndex = Entry.Type;
                Combo_MsgPos.SelectedIndex = Entry.Position;

                SetMsgBackground(Entry.Type);
                MsgPreviewTimer_Tick(null, null);
            }

            MsgText.TextChanged += MsgText_TextChanged;
            Combo_MsgType.SelectedIndexChanged += Combo_MsgType_SelectedIndexChanged;

            MsgText.ToolTip.RemoveAll();
            PerformSpellCheck();
        }

        private void MsgText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FastColoredTextBox box = (FastColoredTextBox)sender;
            box.ToolTip.RemoveAll();

            string hoverWord = box.SelectedText;

            if (!Program.dictionary.Check(hoverWord))
            {
                List<string> sugg = Program.dictionary.Suggest(hoverWord).ToList();

                box.ToolTip.SetToolTip(box, String.Join(Environment.NewLine, sugg));

            }
        }

        private void PerformSpellCheck()
        {
            MsgText.ClearStyle(FastColoredTextBoxNS.StyleIndex.All);

            if (MsgText.Text.Length == 0 || !Program.Settings.Spellcheck)
                return;

            string tagLess = Regex.Replace(MsgText.Text.ToUpper().Replace(Environment.NewLine, " "), @"<([\s\S]*?)>", string.Empty, RegexOptions.Compiled);
            string[] strings = tagLess.StripPunctuation().Split(' ');

            Range r = new Range(MsgText, 0, 0, MsgText.Text.Length - 1, MsgText.LinesCount - 1);

            foreach (string s in strings)
            {
                try
                {
                    if (!Program.dictionary.Check(s))
                        r.SetStyle(SyntaxHighlighter.UnderlineStyle, @"\b" + s + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                }
                catch (Exception)
                {
                }
            }
        }

        private void MsgText_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            MsgPreviewTimer.Stop();
            MsgPreviewTimer.Start();

            PerformSpellCheck();
        }

        private Bitmap GetMessagePreviewImage(MessageEntry Entry, string Language, string NPCName, ref Common.SavedMsgPreviewData savedPreviewData)
        {
            List<byte> Data = Entry.ConvertTextData(NPCName, Language, false);

            if (Data == null || (Data.Count == 0 && !String.IsNullOrEmpty(Entry.MessageText)))
                return null;

            if (Data.Count > 1280)
            {
                Entry = new MessageEntry();
                Entry.MessageText = "Error: Over 1280 bytes.";
                Data = Entry.ConvertTextData(NPCName, Language, false);
            }

            bool CreditsTxBox = (ZeldaMessage.Data.BoxType)Entry.Type > ZeldaMessage.Data.BoxType.None_Black;

            float[] fontWidths = null;
            byte[] font = null;

            if (fontsWidths.ContainsKey(Language) && fonts.ContainsKey(Language))
            {
                fontWidths = fontsWidths[Language];
                font = fonts[Language];
            }
            else if (fontsWidths.ContainsKey(Dicts.DefaultLanguage) && fonts.ContainsKey(Dicts.DefaultLanguage))
            {
                fontWidths = fontsWidths[Dicts.DefaultLanguage];
                font = fonts[Dicts.DefaultLanguage];
            }

            ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview((ZeldaMessage.Data.BoxType)Entry.Type,
                                                                              Data.ToArray(),
                                                                              fontWidths,
                                                                              font,
                                                                              EditedFile.SpaceFromFont);

            Bitmap bmp;

            if (savedPreviewData != null &&
                savedPreviewData.MessageArrays != null &&
                mp.Message.Count == savedPreviewData.MessageArrays.Count &&
                Entry.Type == savedPreviewData.Type)
            {
                bmp = (Bitmap)savedPreviewData.previewImage;
            }
            else
            {
                bmp = new Bitmap((CreditsTxBox ? 480 : 384), mp.MessageCount * (CreditsTxBox ? 360 : 108));
                bmp.MakeTransparent();
            }

            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < mp.MessageCount; i++)
                {
                    if (savedPreviewData == null ||
                        savedPreviewData.MessageArrays == null ||
                        mp.Message.Count != savedPreviewData.MessageArrays.Count ||
                        Entry.Type != savedPreviewData.Type ||
                        !mp.Message[i].SequenceEqual(savedPreviewData.MessageArrays[i]))
                    {
                        grfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        Bitmap box = mp.GetPreview(i, Program.Settings.ImproveTextMsgReadability, 1.5f);
                        grfx.DrawImage(box, 0, box.Height * i);
                    }
                }
            }

            savedPreviewData = new Common.SavedMsgPreviewData();
            savedPreviewData.MessageArrays = mp.Message;
            savedPreviewData.previewImage = bmp;
            savedPreviewData.Type = Entry.Type;
            savedPreviewData.Position = Entry.Position;

            return bmp;
        }

        private void MsgPreviewTimer_Tick(object sender, EventArgs e)
        {
            MsgPreviewTimer.Stop();

            if (MessagesGrid.SelectedRows.Count == 0 || SelectedEntry == null)
                return;

            if (Program.Settings.OrigPreview && Combo_Language.SelectedIndex != 0)
            {
                PreviewSplitContainer.SplitterDistance = PreviewSplitContainer.Width / 2;
                PreviewSplitContainer.Panel1Collapsed = false;
                PreviewSplitContainer.Panel1.AutoScroll = true;

                MessageEntry Entry2 = SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
                Entry2.MessageText = MsgTextDefault.Text;

                Bitmap bmp2 = GetMessagePreviewImage(Entry2, Dicts.DefaultLanguage, SelectedEntry.NPCName, ref lastPreviewDataOrig);

                if (bmp2 != null)
                {
                    bmp2 = bmp2.ResizeImageKeepAspectRatio(Math.Min(this.PreviewSplitContainer.Panel1.Width - 35, bmp2.Width), bmp2.Height);

                    MsgPreviewOrig.Size = new Size(bmp2.Width, bmp2.Height);
                    MsgPreviewOrig.Location = new Point((this.PreviewSplitContainer.Panel1.Width - MsgPreviewOrig.Width) / 2, 0 - PreviewSplitContainer.Panel1.VerticalScroll.Value);
                    MsgPreviewOrig.Image = bmp2;
                }
                else
                {
                    if (lastPreviewDataOrig != null && lastPreviewDataOrig.previewImage != null)
                    {
                        Bitmap b = new Bitmap(lastPreviewDataOrig.previewImage);
                        MsgPreviewOrig.Image = b.SetAlpha(170);
                    }
                }
            }
            else
            {
                PreviewSplitContainer.Panel1Collapsed = true;
            }

            List<MessageEntry> MessageList = GetLanguageMessageList(SelectedEntry, Combo_Language.Text);

            MessageEntry Entry = MessageList[MessagesGrid.SelectedRows[0].Index];
            Entry.MessageText = MsgText.Text;

            Bitmap bmp = GetMessagePreviewImage(Entry, Combo_Language.Text, SelectedEntry.NPCName, ref lastPreviewData);

            if (bmp != null)
            {
                if (Program.Settings.OrigPreview && Combo_Language.SelectedIndex != 0)
                    bmp = bmp.ResizeImageKeepAspectRatio(Math.Min(this.PreviewSplitContainer.Panel2.Width - 35, bmp.Width), bmp.Height);

                MsgPreview.Size = new Size(bmp.Width, bmp.Height);
                MsgPreview.Location = new Point((this.PreviewSplitContainer.Panel2.Width - MsgPreview.Width) / 2, 0 - PreviewSplitContainer.Panel2.VerticalScroll.Value);
                MsgPreview.Image = bmp;
            }
            else
            {
                if (lastPreviewData != null && lastPreviewData.previewImage != null)
                {
                    bmp = new Bitmap(lastPreviewData.previewImage);

                    if (Program.Settings.OrigPreview && Combo_Language.SelectedIndex != 0)
                        bmp = bmp.ResizeImageKeepAspectRatio(Math.Min(this.PreviewSplitContainer.Panel2.Width - 35, bmp.Width), bmp.Height);

                    MsgPreview.Image = bmp.SetAlpha(170);
                }
            }
        }

        private void PanelMsgPreview_Resize(object sender, EventArgs e)
        {
            MsgPreview.Left = (this.PanelMsgPreview.Width - MsgPreview.Width) / 2;
            MsgPreviewTimer.Stop();
            MsgPreviewTimer.Start();
        }

        private void Combo_MsgPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = Combo_Language.SelectedIndex == 0 ? SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index] : SelectedEntry.Localization[Combo_Language.SelectedIndex - 1].Messages[MessagesGrid.SelectedRows[0].Index];
            Entry.Position = Combo_MsgPos.SelectedIndex;
        }

        private void Combo_MsgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = Combo_Language.SelectedIndex == 0 ? SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index] : SelectedEntry.Localization[Combo_Language.SelectedIndex - 1].Messages[MessagesGrid.SelectedRows[0].Index];
            Entry.Type = Combo_MsgType.SelectedIndex;

            MsgText.TextChanged -= MsgText_TextChanged;
            Combo_MsgType.SelectedIndexChanged -= Combo_MsgType_SelectedIndexChanged;

            SetMsgBackground(Entry.Type);
            MsgPreviewTimer.Stop();
            MsgPreviewTimer.Start();

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

            int RowToInsert = MessagesGrid.Rows.Count;

            if (MessagesGrid.SelectedRows.Count != 0)
                RowToInsert = MessagesGrid.SelectedRows[0].Index + 1;

            SelectedEntry.Messages.Insert(RowToInsert, new MessageEntry() { Name = Title, MessageText = "", Position = 0, Type = 0 });

            foreach (string language in EditedFile.Languages)
            {
                int locIndex = SelectedEntry.Localization.FindIndex(x => x.Language == language);

                if (locIndex != -1)
                    SelectedEntry.Localization[locIndex].Messages.Insert(RowToInsert, new MessageEntry() { Name = Title, MessageText = "", Position = 0, Type = 0 });
            }

            MessagesGrid.Rows.Insert(RowToInsert, new object[] { Title });

            if (RowToInsert > 0)
                MessagesGrid.Rows[RowToInsert - 1].Selected = false;

            MessagesGrid.Rows[RowToInsert].Selected = true;
        }

        private void Btn_DeleteMsg_Click(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            int index = MessagesGrid.SelectedRows[0].Index;
            string Name = SelectedEntry.Messages[index].Name;

            MessagesGrid.Rows.RemoveAt(index);
            SelectedEntry.Messages.RemoveAll(x => x.Name == Name);

            foreach (var loc in SelectedEntry.Localization)
                loc.Messages.RemoveAll(x => x.Name == Name);

        }

        private void Btn_MsgRename_Click(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            string Title = (MessagesGrid.SelectedRows[0].Cells[0].Value as string);

            if (InputBox.ShowInputDialog("New message title?", ref Title) != DialogResult.OK)
                return;

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

            foreach (LocalizationEntry l in SelectedEntry.Localization)
            {
                l.Messages[MessagesGrid.SelectedRows[0].Index].Name = Title;
            }

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

        private void ChkBox_UseSpaceFont_CheckedChanged(object sender, EventArgs e)
        {
            EditedFile.SpaceFromFont = (sender as CheckBox).Checked;
            MsgPreviewTimer.Stop();
            MsgPreviewTimer.Start();
        }
        private void chkBox_ShowDefaultLanguagePreview_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.OrigPreview = chkBox_ShowDefaultLanguagePreview.Checked;
            MsgPreviewTimer.Stop();
            MsgPreviewTimer.Start();
        }

        private void FindMsgBtn_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            int MsgCount = 0;
            btn_FindMsg.Enabled = false;

            foreach (NPCEntry n in EditedFile.Entries)
                MsgCount += n.Messages.Count;

            if (LastSearch != txBox_Search.Text || LastMsgCount != MsgCount)
                LastSearchDepth = 0;

            LastMsgCount = MsgCount;

            int CurSearchDepth = 0;
            int RowIndex = 0;
            bool thereWereMessages = false;

            foreach (NPCEntry n in EditedFile.Entries)
            {
                int MsgRowIndex = 0;

                List<MessageEntry> messageList = GetLanguageMessageList(n, Combo_Language.Text);

                foreach (MessageEntry msg in messageList)
                {
                    string r = Regex.Replace(msg.MessageText.ToUpper().Replace(Environment.NewLine, " "), @"<([\s\S]*?)>", string.Empty, RegexOptions.Compiled);

                    if (r.Contains(txBox_Search.Text.ToUpper()))
                    {
                        thereWereMessages = true;

                        if (CurSearchDepth >= LastSearchDepth)
                        {
                            try
                            {
                                NpcsFilter.Text = "";
                                MessagesFilter.Text = "";
                                DataGrid_NPCs.SuspendLayout();
                                MessagesGrid.SuspendLayout();

                                DataGrid_NPCs.ClearSelection();
                                MessagesGrid.ClearSelection();

                                DataGrid_NPCs.Rows[RowIndex].Selected = true;

                                DataGrid_NPCs.CurrentCell = DataGrid_NPCs.Rows[RowIndex].Cells[1];
                                DataGrid_NPCs.FirstDisplayedScrollingRowIndex = RowIndex;
                                DataGrid_NPCs.FirstDisplayedCell = DataGrid_NPCs.Rows[RowIndex].Cells[1];
                                TabControl.SelectedTab = Tab4_Messages;

                                MessagesGrid.Rows[MsgRowIndex].Selected = true;

                                if (Program.IsRunningUnderMono)
                                {
                                    ScrollToMsg = MsgRowIndex;
                                    messageSearchTimer.Interval = 10;
                                    messageSearchTimer.Tick += MessageSearchTimer_Tick;
                                    messageSearchTimer.Start();
                                }
                                else
                                {
                                    MessagesGrid.FirstDisplayedScrollingRowIndex = MsgRowIndex;
                                }

                                LastSearchDepth = CurSearchDepth + 1;
                                LastSearch = txBox_Search.Text;

                            }
                            catch (Exception)
                            {
                            }
                            finally
                            {
                                DataGrid_NPCs.ResumeLayout();
                                MessagesGrid.ResumeLayout();
                            }

                            if (!Program.IsRunningUnderMono)
                                btn_FindMsg.Enabled = true;

                            return;
                        }
                        else
                            CurSearchDepth++;
                    }

                    MsgRowIndex++;
                }

                RowIndex++;

            }

            SystemSounds.Exclamation.Play();

            if (thereWereMessages)
            {
                LastSearchDepth = 0;
                FindMsgBtn_Click(null, null);
            }
            else
                btn_FindMsg.Enabled = true;
        }

        private void MessageSearchTimer_Tick(object sender, EventArgs e)
        {
            messageSearchTimer.Stop();
            MessagesGrid.CurrentCell = MessagesGrid.Rows[ScrollToMsg].Cells[0];
            MessagesGrid.FirstDisplayedScrollingRowIndex = ScrollToMsg;
            btn_FindMsg.Enabled = true;
        }

        private void TxBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (!btn_FindMsg.Enabled)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                FindMsgBtn_Click(null, null);
            }
        }

        private void MessagesFilter_TextChanged(object sender, EventArgs e)
        {
            int index = 0;

            foreach (MessageEntry entry in SelectedEntry.Messages)
            {
                if (String.IsNullOrWhiteSpace(MessagesFilter.Text))
                    MessagesGrid.Rows[index].Visible = true;
                else if (entry.Name.ToUpper().Contains(MessagesFilter.Text.ToUpper()))
                    MessagesGrid.Rows[index].Visible = true;
                else
                    MessagesGrid.Rows[index].Visible = false;

                index++;
            }
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
        DateTime LastWriteTime2;

        private void WatchFile(NPCEntry EditedEntry)
        {
            if (Program.Settings.AutoSave)
            {
                WatchedEntry = EditedEntry;

                string fPath = Path.Combine(CCode.gtempFolderPath, $"{CCode.gcodeFileNameBase}.c");
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

            var Dt = GetLastWriteTimeForFile(CCode.geditCodeFilePath);
            var Dt2 = GetLastWriteTimeForFile(CCode.geditHeaderFilePath);

            if (Dt != LastWriteTime || Dt2 != LastWriteTime2)
                Watcher_Changed(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""));

            LastWriteTime = Dt;
            LastWriteTime2 = Dt2;
            autoSaveTimer.Start();
        }

        private void CompileCode()
        {

            string CompileMsgs = "";
            CCode.Compile(EditedFile.CHeader, SelectedEntry.EmbeddedOverlayCode, ref CompileMsgs);

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
                        if (File.Exists(CCode.geditCodeFilePath))
                            fs = File.Open(CCode.geditCodeFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        if (File.Exists(CCode.geditHeaderFilePath))
                            fs2 = File.Open(CCode.geditHeaderFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    }
                    catch (Exception)
                    {
                        if (File.Exists(CCode.geditCodeFilePath))
                            fs = File.Open(Path.Combine(CCode.gtempFolderPath, $"{CCode.gcodeFileNameBase}.c"), FileMode.Open, FileAccess.Read, FileShare.Read);


                        if (File.Exists(CCode.geditHeaderFilePath))
                            fs2 = File.Open(Path.Combine(CCode.gtempFolderPath, CCode.gheaderFileName), FileMode.Open, FileAccess.Read, FileShare.Read);
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
                                                                Textbox_CodeEditorArgs.Text.Replace("$CODEFILE", CCode.geditCodeFilePath.AppendQuotation()).Replace("$CODEFOLDER", CCode.gtempFolderPath.AppendQuotation()).Replace("$CODEHEADER", CCode.geditHeaderFilePath.AppendQuotation())
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

            if (ComboId < 6)
            {
                SelectedEntry.EmbeddedOverlayCode.FuncsRunWhen[ComboId, 0] = c.SelectedIndex;
                SelectedEntry.EmbeddedOverlayCode.SetFuncNames[ComboId] = c.Text;

            }
            else
                SelectedEntry.EmbeddedOverlayCode.FuncsRunWhen[ComboId - 6, 1] = c.SelectedIndex;
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
                SelectedEntry.EmbeddedOverlayCode.FuncsRunWhen = new int[6, 2]
                {
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                    {-1, -1},
                };

                SelectedEntry.EmbeddedOverlayCode.SetFuncNames = new string[6];

                foreach (KeyValuePair<ComboBox, ComboBox> kvp in FunctionComboBoxes)
                {
                    ComboBox c = kvp.Key;
                    ComboBox w = kvp.Value;

                    c.SelectedIndex = -1;
                    c.DataSource = null;

                    if (w != null)
                    {
                        w.SelectedIndex = -1;
                        w.DataSource = null;
                    }
                }

                Button_CCompile_Click(null, null);
            }
        }

        #endregion
    }
}