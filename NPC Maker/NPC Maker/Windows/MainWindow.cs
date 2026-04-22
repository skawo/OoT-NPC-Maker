using FastColoredTextBoxNS;
using MiscUtil.Collections.Extensions;
using Newtonsoft.Json;
using NPC_Maker.Controls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeldaMessage;

namespace NPC_Maker
{
    public partial class MainWindow : Form
    {
        public string OpenedPath = "";

        private NPCFile EditedFile = null;
        private NPCEntry SelectedEntry = null;
        private int SelectedIndex = -1;
        private string NPCSave = "";

        private List<KeyValuePair<ComboBox, ComboBox>> FunctionComboBoxes;

        private int LastSearchDepth = 0;
        private int LastMsgCount = 0;
        private string LastSearch = "";
        private int ScrollToMsg = 0;

        private readonly System.Windows.Forms.Timer compileTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer messageSearchTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer autoBackupTimer = new System.Windows.Forms.Timer();

        private string LastBackup = "";

        private Common.SavedMsgPreviewData lastPreviewData;
        private Common.SavedMsgPreviewData lastPreviewDataOrig;

        private Dictionary<string, float[]> fontsWidths = new Dictionary<string, float[]>();
        private Dictionary<string, byte[]> fonts = new Dictionary<string, byte[]>();

        private Dictionary<string, float[]> exfontsWidths = new Dictionary<string, float[]>();
        private Dictionary<string, byte[]> exfonts = new Dictionary<string, byte[]>();

        private readonly object _previewLock = new object();
        private Common.PreviewSnapshot _pendingSnapshot;
        private Task _previewWorker;

        private float lastScale = 0.0f;

        private NativeColorDialog ColorDialog = new NativeColorDialog();

        public MainWindow(string FilePath = "")
        {
            InitializeComponent();

            this.SuspendLayout();
            TabControl_Segments.SuspendLayout();

            foreach (TabPage Page in TabControl_Segments.TabPages)
            {
                SegmentDataGrid sg = new SegmentDataGrid();
                sg.Grid.CellParsing += DataGridViewSegments_CellParse;
                sg.Grid.CellMouseDoubleClick += Segments_CellMouseDoubleClick;
                sg.Grid.KeyUp += DataGridViewSegments_KeyUp;
                sg.Dock = DockStyle.Fill;

                Page.Controls.Add(sg);
            }

            TabControl_Segments.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            compileTimer.Interval = 50;
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

            StartPreviewWorker();

            chkBox_ShowDefaultLanguagePreview.Checked = Program.Settings.OrigPreview;
            ChkBox_UseCJK.Checked = Program.Settings.UseCJK;

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

            SetupFonts();
            UpdateLastPathsList();
            UpdateSaveBinaryLastPathsList();

            // ============================================================

            if (FilePath != "")
                OpenFile(FilePath);

            SplitContainer1_Panel1_SizeChanged(null, null);
            MsgTabSplitContainer_SizeChanged(null, null);

            SetupScale();
        }

        private void SetupScale()
        {
            float scale = Program.Settings.GUIScale;

            if (scale != lastScale)
            {
                lastScale = scale;

                float fontSize = Helpers.GetScaleFontSize();

                this.Font = new Font(this.Font.FontFamily, fontSize);
                this.MenuStrip.Font = new Font(this.MenuStrip.Font.FontFamily, fontSize);
                this.Panel_Editor.AutoScrollMinSize = new Size((int)(936 * scale), (int)(647 * scale));
                this.TextBox_CompileMsg.Font = new Font(this.TextBox_CompileMsg.Font.FontFamily, fontSize);
                this.TextBox_CompileMsg.Height = LblFuncToRun.Location.Y - LblFuncToRun.Height - this.TextBox_CompileMsg.Location.Y - 12;

                Helpers.AdjustControlScale(this);

                this.MsgText.Font = new Font(this.MsgText.Font.FontFamily, Helpers.GetScaleFontSize(Program.Settings.MessageEditorFontSize));
                this.MsgTextCJK.Font = new Font(this.MsgTextCJK.Font.FontFamily, Helpers.GetScaleFontSize(Program.Settings.MessageEditorFontSize));

                msgCommentTooltip.Font = new Font(this.MsgText.Font.FontFamily, Helpers.GetScaleFontSize(1 + Program.Settings.MessageEditorFontSize));
                msgCommentTooltipLoc.Font = new Font(this.MsgText.Font.FontFamily, Helpers.GetScaleFontSize(1 + Program.Settings.MessageEditorFontSize));
                CodeParamsTooltip.Font = new Font(this.MsgText.Font.FontFamily, Helpers.GetScaleFontSize(1 + Program.Settings.MessageEditorFontSize));
            }
        }

        private void SetupFonts()
        {
            while (Program.Monofonts == null) ;

            foreach (string font in Program.Monofonts)
                comboFont.Items.Add(font);

            comboFont.SelectedIndexChanged -= ComboFont_SelectedChanged;
            numUpDownFont.ValueChanged -= NumUpDownFont_ValueChanged;

            comboFont.Text = MsgText.Font.Name;
            numUpDownFont.Value = (int)MsgText.Font.Size;

            try
            {
                if (!string.IsNullOrWhiteSpace(Program.Settings.MessageEditorFont))
                {
                    string savedFont = Program.Settings.MessageEditorFont;

                    if (comboFont.Items.Contains(savedFont))
                    {
                        comboFont.Text = savedFont;
                        numUpDownFont.Value = (int)Program.Settings.MessageEditorFontSize;
                    }
                    else
                    {
                        // Saved font no longer available — fall back to default and update settings
                        Program.Settings.MessageEditorFont = MsgText.Font.Name;
                        Program.Settings.MessageEditorFontSize = MsgText.Font.Size;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                comboFont.SelectedIndexChanged += ComboFont_SelectedChanged;
                numUpDownFont.ValueChanged += NumUpDownFont_ValueChanged;
            }
        }

        private void StartPreviewWorker()
        {
            _previewWorker = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Common.PreviewSnapshot snap = WaitForSnapshot();

                    if (snap == null)
                        continue;

                    Bitmap bmp = GetMessagePreviewImage(snap.Entry, snap.Language, ref lastPreviewData);

                    Bitmap bmpOrig = null;

                    if (snap.ShowOrig && snap.OrigEntry != null)
                        bmpOrig = GetMessagePreviewImage(snap.OrigEntry, Lists.DefaultLanguage, ref lastPreviewDataOrig);

                    this.BeginInvoke((Action)(() =>
                    {
                        UpdatePreviewUI(bmp, bmpOrig, snap.ShowOrig);
                    }));
                }
            }, TaskCreationOptions.LongRunning);
        }

        private Common.PreviewSnapshot WaitForSnapshot()
        {
            lock (_previewLock)
            {
                while (_pendingSnapshot == null)
                    Monitor.Wait(_previewLock);

                var snap = _pendingSnapshot;
                _pendingSnapshot = null;
                Monitor.Pulse(_previewLock);
                return snap;
            }
        }

        private void UpdatePreviewUI(Bitmap bmp, Bitmap bmpOrig, bool showOrig)
        {
            if (showOrig)
            {
                PreviewSplitContainer.SplitterDistance = PreviewSplitContainer.Width / 2;
                PreviewSplitContainer.Panel1Collapsed = false;
                PreviewSplitContainer.Panel1.AutoScroll = true;

                SetPreviewImage(MsgPreviewOrig, bmpOrig, lastPreviewDataOrig, PreviewSplitContainer.Panel1);
            }
            else
            {
                PreviewSplitContainer.Panel1Collapsed = true;
            }

            SetPreviewImage(MsgPreview, bmp, lastPreviewData, PreviewSplitContainer.Panel2);
        }

        private void SetPreviewImage(PictureBox box, Bitmap bmp, dynamic lastData, Panel container)
        {
            if (bmp == null && lastData?.previewImage != null)
            {
                bmp = new Bitmap(lastData.previewImage);
                bmp = bmp.SetAlpha(170);
            }

            if (bmp != null)
            {
                bmp = bmp.ResizeImageKeepAspectRatio(Math.Min(container.Width - 35, bmp.Width), bmp.Height);
            }

            if (bmp != null)
            {
                box.Size = new Size(bmp.Width, bmp.Height);
                box.Location = new Point((container.Width - box.Width) / 2, 0 - container.VerticalScroll.Value);
                box.Image = bmp;
            }
        }

        private void RequestPreviewUpdate()
        {
            if (MessagesGrid.SelectedRows.Count == 0 || SelectedEntry == null)
                return;

            int rowIndex = MessagesGrid.SelectedRows[0].Index;
            string language = Combo_Language.Text;
            bool showOrig = Program.Settings.OrigPreview && Combo_Language.SelectedIndex != 0;

            List<MessageEntry> list = GetLanguageMessageList(SelectedEntry, language);
            list[rowIndex].MessageText = MsgText.Text;

            if (showOrig)
                SelectedEntry.Messages[rowIndex].MessageText = MsgTextDefault.Text;

            var snap = new Common.PreviewSnapshot
            {
                Entry = list[rowIndex],
                Language = language,
                NpcName = SelectedEntry.NPCName,
                ShowOrig = showOrig,
                OrigEntry = showOrig ? SelectedEntry.Messages[rowIndex] : null
            };

            lock (_previewLock)
            {
                while (_pendingSnapshot != null)
                    Monitor.Wait(_previewLock);

                _pendingSnapshot = snap;
                Monitor.Pulse(_previewLock);
            }
        }

        private void ComboFont_SelectedChanged(object sender, EventArgs e)
        {
            try
            {
                float size = Helpers.GetScaleFontSize(Program.Settings.MessageEditorFontSize);

                MsgTextDefault.Font = new Font(comboFont.Text, size);
                MsgText.Font = new Font(comboFont.Text, size);
                MsgTextCJK.Font = new Font(comboFont.Text, size);

                Program.Settings.MessageEditorFont = comboFont.Text;
            }
            catch
            { }
        }

        private void NumUpDownFont_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Program.Settings.MessageEditorFontSize = (int)numUpDownFont.Value;
                float size = Helpers.GetScaleFontSize(Program.Settings.MessageEditorFontSize);

                MsgTextDefault.Font = new Font(comboFont.Text, size);
                MsgText.Font = new Font(comboFont.Text, size);
                MsgTextCJK.Font = new Font(comboFont.Text, size);

            }
            catch
            { }
        }

        private void MsgTabSplitContainer_SizeChanged(object sender, EventArgs e)
        {
            if (Program.IsRunningUnderMono)
            {
                int width = Tab4_Messages.Width - MessagesGrid.Width - 20;
                int height = Btn_MsgMoveDown.Location.Y + Btn_MsgMoveDown.Height - Lbl_Localization.Location.Y - Lbl_Localization.Height;

                MsgTabSplitContainer.Size = new Size(width, height);
                MsgEntrySplitContainer.Size = new Size(width, MsgTabSplitContainer.SplitterDistance);
                PanelMsgPreview.Size = new Size(width, MsgTabSplitContainer.SplitterDistance);
            }
        }

        private void SplitContainer1_Panel1_SizeChanged(object sender, EventArgs e)
        {
            int width = MainSplitPanel.Panel1.Width;

            if (MainSplitPanel.Panel1.Width == 0)
                width = MainSplitPanel.SplitterDistance;

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

        private void LoadAddFontByName(string fontName, ref Dictionary<string, byte[]> fontDict, ref Dictionary<string, float[]> widthsDict)
        {
            string basePath = Path.GetDirectoryName(string.IsNullOrEmpty(Program.JsonPath) ? Program.ExecPath : Program.JsonPath);
            string fontDir = Path.Combine(basePath, "font");

            string fontPath = Path.Combine(fontDir, $"{fontName}.font_static");
            string widthPath = Path.Combine(fontDir, $"{fontName}.width_table");
            string fontPathDef = Path.Combine(fontDir, "font.font_static");
            string widthPathDef = Path.Combine(fontDir, "font.width_table");

            bool namedExists = File.Exists(fontPath) && File.Exists(widthPath);
            bool defaultExists = File.Exists(fontPathDef) && File.Exists(widthPathDef)
                                 && !fontDict.ContainsKey(Lists.DefaultLanguage);

            if (!namedExists && !defaultExists)
                return;

            string resolvedFontPath = namedExists ? fontPath : fontPathDef;
            string resolvedWidthPath = namedExists ? widthPath : widthPathDef;
            string resolvedKey = namedExists ? fontName : Lists.DefaultLanguage;

            fontDict[resolvedKey] = File.ReadAllBytes(resolvedFontPath);
            widthsDict[resolvedKey] = ParseWidthTable(resolvedWidthPath);
        }

        private static float[] ParseWidthTable(string path)
        {
            byte[] raw = File.ReadAllBytes(path);
            float[] widths = new float[raw.Length / 4];

            for (int i = 0; i < widths.Length; i++)
            {
                // Reverse 4 bytes for endianness before converting
                byte[] chunk = new byte[4] { raw[i * 4 + 3], raw[i * 4 + 2], raw[i * 4 + 1], raw[i * 4] };
                widths[i] = BitConverter.ToSingle(chunk, 0);
            }

            return widths;
        }

        private void ReloadAllFonts()
        {
            fonts.Clear();
            fontsWidths.Clear();
            exfonts.Clear();
            exfontsWidths.Clear();

            LoadAddFontByName(Lists.DefaultLanguage, ref fonts, ref fontsWidths);

            foreach (string lan in EditedFile.Languages)
            {
                LoadAddFontByName(lan, ref fonts, ref fontsWidths);

                if (Dicts.LanguageDefs.ContainsKey(lan) && !String.IsNullOrWhiteSpace(Dicts.LanguageDefs[lan].ExtraFont))
                    LoadAddFontByName(Dicts.LanguageDefs[lan].ExtraFont, ref exfonts, ref exfontsWidths);

            }
        }

        private void SetupLanguageCombo()
        {
            Combo_Language.SelectedIndexChanged -= Combo_Language_SelectedIndexChanged;

            Combo_Language.Items.Clear();
            Combo_Language.Items.Add(Lists.DefaultLanguage);

            foreach (string lan in EditedFile.Languages)
                Combo_Language.Items.Add(lan);

            try
            {
                Dicts.ReloadLanguages(EditedFile.Languages);
                ReloadAllFonts();
                Dicts.ReloadSpellcheckDicts(EditedFile.Languages);
            }
            catch (Exception ex)
            {
                BigMessageBox.Show("Error setting up language combobox: " + ex.Message);
            }

            Combo_Language.SelectedIndex = 0;
            Combo_Language.SelectedIndexChanged += Combo_Language_SelectedIndexChanged;
            Combo_Language_SelectedIndexChanged(null, null);

        }

        private void OpenFile(string FilePath)
        {
            autoBackupTimer.Stop();

            if (string.IsNullOrWhiteSpace(FilePath))
                return;

            EditedFile = FileOps.ParseNPCJsonFile(FilePath);
            NPCSave = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

            if (Program.Settings.LastPaths.Contains(FilePath))
                Program.Settings.LastPaths.Remove(FilePath);

            Program.Settings.LastPaths.Insert(0, FilePath);

            if (Program.Settings.LastPaths.Count > 10)
                Program.Settings.LastPaths.RemoveAt(10);

            UpdateLastPathsList();

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

        private void OpenLastFile(string FilePath)
        {
            if (string.IsNullOrWhiteSpace(FilePath) || !File.Exists(FilePath))
            {
                DialogResult Res = BigMessageBox.Show("File no longer exists. Remove from list?", "File not found.", MessageBoxButtons.YesNo);

                if (Res == DialogResult.Yes)
                {
                    Program.Settings.LastPaths.Remove(FilePath);
                    UpdateLastPathsList();
                }
            }
            else
                OpenFile(FilePath);
        }

        private void UpdateLastPathsList()
        {
            while (openRecentToolStripMenuItem.DropDownItems.Count > 2)
                openRecentToolStripMenuItem.DropDownItems.RemoveAt(0);

            var recentItems = Program.Settings.LastPaths
                .Select(path => new ToolStripMenuItem(Helpers.TruncatePath(path)) { Tag = path })
                .ToList();

            recentItems.ForEach(item => item.Click += (s, e) => OpenLastFile((string)item.Tag));

            for (int i = 0; i < recentItems.Count; i++)
                openRecentToolStripMenuItem.DropDownItems.Insert(i, recentItems[i]);
        }

        private void UpdateSaveBinaryLastPathsList()
        {
            while (saveBinaryToRecentToolStripMenuItem.DropDownItems.Count > 2)
                saveBinaryToRecentToolStripMenuItem.DropDownItems.RemoveAt(0);

            var recentItems = Program.Settings.LastSavePaths
                .Select(path => new ToolStripMenuItem(Helpers.TruncatePath(path)) { Tag = path })
                .ToList();

            recentItems.ForEach(item => item.Click += (s, e) => RunSaveBinary((string)item.Tag));

            for (int i = 0; i < recentItems.Count; i++)
                saveBinaryToRecentToolStripMenuItem.DropDownItems.Insert(i, recentItems[i]);
        }

        private void clearThisListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.LastPaths = new List<string>();
            UpdateLastPathsList();
        }

        private void AutoBackupTimer_Tick(object sender, EventArgs e)
        {
            autoBackupTimer.Stop();

            if (!Program.CompileInProgress)
            {
                TaskEx.Run(() =>
                {
                    try
                    {
                        string currentBackup = Helpers.GetBase64Hash(JsonConvert.SerializeObject(EditedFile, Formatting.Indented));

                        if (currentBackup != LastBackup)
                        {
                            string json = FileOps.ProcessNPCJSON(ref EditedFile, null);

                            if (json != null)
                            {
                                FileOps.SaveNPCJSON("backup", null, null, json);

                                if (!Directory.Exists(Program.AutoSavePath))
                                    Directory.CreateDirectory(Program.AutoSavePath);

                                FileOps.SaveNPCJSON(Path.Combine(Program.AutoSavePath, $"autosave_{DateTime.Now.Ticks}.json"), null, null, json);
                                PruneOldAutosaves(Program.AutoSavePath, 30);

                                LastBackup = currentBackup;
                            }
                        }
                    }
                    catch { }
                });
            }

            autoBackupTimer.Start();
        }


        private void PruneOldAutosaves(string folderPath, int maxAutosaves)
        {
            if (!Directory.Exists(folderPath))
                return;

            var autosaveFiles = new DirectoryInfo(folderPath)
                .GetFiles("autosave_*")
                .OrderByDescending(f => f.CreationTimeUtc)
                .Skip(maxAutosaves);

            foreach (var file in autosaveFiles)
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                }
            }
        }

        private void CompileTimer_Tick(object sender, EventArgs e)
        {
            if (Program.CompileInProgress)
                return;

            compileTimer.Stop();
            MenuStrip.Enabled = true;
            Panel_Editor.Enabled = true;
            btn_FindMsg.Enabled = true;
            Program._stopWatch.Stop();

            if (Program.CompileThereWereErrors)
            {
                progressL.SetProgress(0, "Compilation failed.");
                if (Program.IsRunningUnderMono)
                    BigMessageBox.Show(Program.CompileMonoErrors, "Error", MessageBoxButtons.OK);
            }
            else
            {
                string time = Program._stopWatch.ElapsedMilliseconds < 1000
                    ? string.Format("{0} ms", Program._stopWatch.ElapsedMilliseconds)
                    : string.Format("{0} s {1} ms", Program._stopWatch.Elapsed.Seconds, Program._stopWatch.Elapsed.Milliseconds);
                progressL.SetProgress(100, string.Format("Completed in {0}.", time));
            }
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
            if (Program.SaveInProgress)
            {
                e.Cancel = true;
                return;
            }

            if (EditedFile != null)
            {
                string CurrentFile = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);

                if (!String.Equals(CurrentFile, NPCSave))
                {
                    DialogResult Res = BigMessageBox.Show("Save changes before exiting?", "Save changes?", MessageBoxButtons.YesNoCancel);

                    if (Res == DialogResult.Yes)
                    {
                        Save_Sync(this, null);
                    }
                    else if (Res == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }

            try
            {
                Program.CodeEditorProcess?.Kill();
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
            SuspendLayout();
            DataGrid_NPCs.SelectionChanged -= DataGrid_NPCs_SelectionChanged;

            try
            {
                DataGrid_NPCs.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                DataGrid_NPCs.Rows.Clear();

                var rows = EditedFile.Entries
                    .Select((entry, index) => new object[] { index.ToString(), entry.IsNull ? "EMPTY" : entry.NPCName })
                    .ToArray();

                foreach (var row in rows)
                    DataGrid_NPCs.Rows.Add(row);

                DataGrid_NPCs.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            }
            finally
            {
                DataGrid_NPCs.SelectionChanged += DataGrid_NPCs_SelectionChanged;
                DataGrid_NPCs_SelectionChanged(DataGrid_NPCs, EventArgs.Empty);
                ResumeLayout();
            }
        }

        private void InsertDataToEditor()
        {
            if (SelectedEntry == null)
                return;

            TabControl.SuspendLayout();

            var e = SelectedEntry;
            int savedTabIndex = TabControl.SelectedIndex;

            #region Basic data

            Textbox_NPCName.Text = e.NPCName;
            Tx_SkeletonName.Text = e.SkeletonHeaderDefinition;
            Tx_FileStartName.Text = e.FileStartHeaderDefinition;

            Txb_ObjectID.Text = e.ObjectID.ToString();
            Txb_ObjectID_Leave(null, null);
            Txtbox_ReactIfAtt.Text = e.SfxIfAttacked.ToString();
            Txtbox_ReactIfAtt_Leave(null, null);

            numUpFileStart.Value = e.FileStart;
            Combo_EffIfAtt.SelectedIndex = e.EffectIfAttacked;
            NumUpAlpha.Value = e.Alpha;
            ChkB_FadeOut.Checked = e.FadeOut;
            NumUp_RiddenBy.Value = e.NPCToRide;
            ChkBox_Glow.Checked = e.Glow;
            ChkBox_GenLight.Checked = e.GenLight;
            NumUp_LightLimb.Value = e.LightLimb;
            NumUp_LightXOffs.Value = e.LightPositionOffsets[0];
            NumUp_LightYOffs.Value = e.LightPositionOffsets[1];
            NumUp_LightZOffs.Value = e.LightPositionOffsets[2];
            NumUp_LightRadius.Value = e.LightRadius;
            ChkBox_DBGCol.Checked = e.DEBUGShowCols;
            ChkBox_DBGLookAt.Checked = e.DEBUGLookAtEditor;
            ChkBox_DBGDlist.Checked = e.DEBUGExDlistEditor;
            ChkBox_DBGPrint.Checked = e.DEBUGPrintToScreen;

            ChkOnlyWhenLens.Checked = e.VisibleUnderLensOfTruth;
            ChkInvisible.Checked = e.Invisible;

            NumUpDown_Hierarchy.Value = e.Hierarchy;
            ComboBox_HierarchyType.SelectedIndex = e.HierarchyType;
            NumUpDown_XModelOffs.Value = e.ModelPositionOffsets[0];
            NumUpDown_YModelOffs.Value = e.ModelPositionOffsets[1];
            NumUpDown_ZModelOffs.Value = e.ModelPositionOffsets[2];
            NumUpDown_Scale.Value = (decimal)e.ModelScale;
            NumUpDown_CutsceneSlot.Value = e.CutsceneID;

            ComboBox_LookAtType.SelectedIndex = e.LookAtType;
            Combo_Head_Horiz.SelectedIndex = e.HeadHorizAxis;
            Combo_Head_Vert.SelectedIndex = e.HeadVertAxis;
            Combo_Waist_Horiz.SelectedIndex = e.WaistHorizAxis;
            Combo_Waist_Vert.SelectedIndex = e.WaistVertAxis;

            ChkBox_ExistInAll.Checked = e.ExistInAllRooms;
            NumUpDown_HeadLimb.Value = e.HeadLimb;
            NumUpDown_WaistLimb.Value = e.WaistLimb;
            NumUpDown_LookAt_X.Value = (decimal)e.LookAtPositionOffsets[0];
            NumUpDown_LookAt_Y.Value = (decimal)e.LookAtPositionOffsets[1];
            NumUpDown_LookAt_Z.Value = (decimal)e.LookAtPositionOffsets[2];
            NumUpDown_DegVert.Value = e.LookAtDegreesVertical;
            NumUpDown_DegHoz.Value = e.LookAtDegreesHorizontal;

            Checkbox_DrawShadow.Checked = e.CastsShadow;
            NumUpDown_ShRadius.Value = e.ShadowRadius;

            Checkbox_HaveCollision.Checked = e.HasCollision;
            Checkbox_CanPressSwitches.Checked = e.PushesSwitches;
            NumUpDown_Mass.Value = e.Mass;
            NumUpDown_ColRadius.Value = e.CollisionRadius;
            NumUpDown_ColHeight.Value = e.CollisionHeight;
            Checkbox_AlwaysActive.Checked = e.IsAlwaysActive;
            Checkbox_AlwaysDraw.Checked = e.IsAlwaysDrawn;
            Chkb_ReactIfAtt.Checked = e.ReactsIfAttacked;
            ChkRunJustScript.Checked = e.ExecuteJustScript;
            Chkb_Opendoors.Checked = e.OpensDoors;
            NumUp_Smoothing.Value = (decimal)e.SmoothingConstant;
            Chkb_IgnoreY.Checked = e.IgnoreYAxis;
            NumUpDown_YColOffs.Value = e.CollisionYShift;

            Checkbox_Targettable.Checked = e.IsTargettable;
            ComboBox_TargetDist.SelectedIndex = e.TargetDistance <= 10 ? e.TargetDistance : 1;
            NumUpDown_TargetLimb.Value = e.TargetLimb;
            NumUpDown_XTargetOffs.Value = e.TargetPositionOffsets[0];
            NumUpDown_YTargetOffs.Value = e.TargetPositionOffsets[1];
            NumUpDown_ZTargetOffs.Value = e.TargetPositionOffsets[2];
            NumUpDown_TalkRadi.Value = (decimal)e.TalkRadius;

            UncullFwd.Value = (decimal)e.CullForward;
            UncullDown.Value = (decimal)e.CullDown;
            UncullScale.Value = (decimal)e.CullScale;

            Combo_MovementType.SelectedIndex = e.MovementType;
            NumUpDown_MovDistance.Value = e.MovementDistance;
            NumUp_MaxRoam.Value = e.MaxDistRoam;
            NumUpDown_MovSpeed.Value = (decimal)e.MovementSpeed;
            NumUpDown_GravityForce.Value = (decimal)e.GravityForce;
            NumUpDown_LoopDelay.Value = e.MovementDelayTime;
            NumUpDown_LoopEndNode.Value = e.PathEndNodeID;
            NumUpDown_LoopStartNode.Value = e.PathStartNodeID;
            Checkbox_Loop.Checked = e.LoopPath;
            NumUpDown_PathFollowID.Value = e.PathID;
            tmpicker_timedPathStart.Value = Helpers.GetTimeFromOcarinaTime(e.TimedPathStart);
            tmpicker_timedPathEnd.Value = Helpers.GetTimeFromOcarinaTime(e.TimedPathEnd);

            ComboBox_AnimType.SelectedIndex = e.AnimationType;

            NumUpDown_ScriptsVar.Value = (int)e.NumVars;
            NumUpDown_ScriptsFVar.Value = (int)e.NumFVars;

            Button_EnvironmentColorPreview.BackColor = Color.FromArgb(255, e.EnvironmentColor.R, e.EnvironmentColor.G, e.EnvironmentColor.B);
            Btn_LightColor.BackColor = Color.FromArgb(255, e.LightColor.R, e.LightColor.G, e.LightColor.B);
            Checkbox_EnvColor.Checked = e.EnvironmentColor.A != 0;

            Textbox_BlinkPattern.Text = e.BlinkPattern;
            Textbox_TalkingPattern.Text = e.TalkPattern;
            NumUpDown_BlinkSegment.Value = e.BlinkSegment;
            NumUpDown_BlinkSpeed.Value = e.BlinkSpeed;
            NumUpDown_TalkSegment.Value = e.TalkSegment;
            NumUpDown_TalkSpeed.Value = e.TalkSpeed;

            NumUpDown_AnimInterpFrames.Value = e.AnimInterpFrames;
            Checkbox_Omitted.Checked = e.Omitted;

            #endregion

            #region Script Tab Pages

            List<TabPage> reusablePages = new List<TabPage>();

            foreach (TabPage page in TabControl_Scripts.TabPages)
                reusablePages.Add(page);

            foreach (ScriptEntry script in e.Scripts)
            {
                string pageName = script.Name == "" ? "Script" : script.Name;

                if (reusablePages.Count != 0)
                {
                    TabPage page = reusablePages.First();
                    (page.Controls[0] as ScriptEditor).Init(ref SelectedEntry,
                                                            ref EditedFile,
                                                            script,
                                                            Program.
                                                            Settings.ColorizeScriptSyntax,
                                                            Program.Settings.CheckSyntax);
                    page.Text = pageName;
                    reusablePages.Remove(page);
                }
                else
                {
                    TabPage page = new TabPage(pageName);
                    TabControl_Scripts.TabPages.Add(page);
                    ScriptEditor se = new ScriptEditor(ref SelectedEntry,
                                                       ref EditedFile,
                                                       script,
                                                       Program.Settings.ColorizeScriptSyntax,
                                                       Program.Settings.CheckSyntax)
                    { Dock = DockStyle.Fill };
                    page.Controls.Add(se);
                }
            }

            foreach (TabPage page in reusablePages)
                TabControl_Scripts.TabPages.Remove(page);

            #endregion

            #region Colors grid

            ColorsDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            ColorsDataGridView.Rows.Clear();

            foreach (ColorEntry colorEntry in e.DisplayListColors)
            {
                int rowIndex = ColorsDataGridView.Rows.Add(new object[] { colorEntry.Limbs, "" });

                ColorsDataGridView.Rows[rowIndex].Cells[1].Style = new DataGridViewCellStyle()
                {
                    BackColor = colorEntry.Color,
                    SelectionBackColor = colorEntry.Color,
                    SelectionForeColor = colorEntry.Color
                };
            }

            ColorsDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;


            #endregion

            #region Animations grid

            DataGrid_Animations.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DataGrid_Animations.Rows.Clear();

            bool isLinkAnim = e.AnimationType == 1;

            foreach (AnimationEntry anim in e.Animations)
            {
                string cValue = GetAnimationFilestartString(anim.FileStart);
                string addrStr = isLinkAnim
                    ? Dicts.GetStringFromBiDict(Dicts.LinkAnims, (int)anim.Address)
                    : anim.Address.ToString("X");

                DataGrid_Animations.Rows.Add(new object[] {
                                                            anim.Name,
                                                            anim.HeaderDefinition,
                                                            cValue,
                                                            addrStr,
                                                            anim.StartFrame,
                                                            anim.EndFrame,
                                                            anim.Speed,
                                                            Dicts.GetStringFromBiDict(Dicts.ObjectIDs, anim.ObjID)
                                                         });
            }

            DataGrid_Animations.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            #endregion

            #region Segments grid

            for (int j = 0; j < TabControl_Segments.TabPages.Count; j++)
            {
                DataGridView grid = (TabControl_Segments.TabPages[j].Controls[0] as Controls.SegmentDataGrid).Grid;
                grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                grid.Rows.Clear();

                foreach (SegmentEntry seg in SelectedEntry.Segments[j])
                    grid.Rows.Add(seg.Name,
                                  seg.HeaderDefinition,
                                  seg.FileStart < 0 ? "Same as main" : seg.FileStart.ToString("X"),
                                  seg.Address.ToString("X"),
                                  Dicts.GetStringFromBiDict(Dicts.ObjectIDs, seg.ObjectID));

                grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            }

            #endregion

            #region Display lists grid

            DataGridView_ExtraDLists.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DataGridView_ExtraDLists.Rows.Clear();

            foreach (DListEntry dlist in e.ExtraDisplayLists)
            {
                string selCombo = ExtraDlists_ShowType.Items[(int)dlist.ShowType].ToString();
                string posType = Dicts.GetStringFromStringIntDict(Dicts.LimbIndexSubTypes, dlist.Limb, null);

                int row = DataGridView_ExtraDLists.Rows.Add(new object[] {
                                                                            dlist.Name,
                                                                            dlist.HeaderDefinition,
                                                                            "",
                                                                            dlist.FileStart < 0 ? "Same as main" : dlist.FileStart.ToString("X"),
                                                                            dlist.Address.ToString("X"),
                                                                            dlist.TransX + "," + dlist.TransY + "," + dlist.TransZ,
                                                                            dlist.RotX   + "," + dlist.RotY   + "," + dlist.RotZ,
                                                                            dlist.Scale.ToString(),
                                                                            posType ?? dlist.Limb.ToString(),
                                                                            Dicts.GetStringFromBiDict(Dicts.ObjectIDs, dlist.ObjectID),
                                                                            selCombo
                                                                        });

                DataGridView_ExtraDLists.Rows[row].Cells[(int)EDlistsColumns.Color].Style.BackColor = dlist.Color;
            }

            DataGridView_ExtraDLists.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            TabControl.SelectedIndex = Math.Min(TabControl.TabPages.Count - 1, savedTabIndex);

            #endregion

            HeaderPathChanged();

            #region Messages

            Combo_Language.SelectedIndexChanged -= Combo_Language_SelectedIndexChanged;
            MessagesGrid.SelectionChanged -= MessagesGrid_SelectionChanged;
            MessagesGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            MessagesGrid.Rows.Clear();

            List<MessageEntry> messageList = e.Messages;
            int locIndex = e.Localization.FindIndex(x => x.Language == Combo_Language.Text);

            if (Combo_Language.SelectedIndex != 0 && locIndex != -1)
                messageList = e.Localization[locIndex].Messages;
            else
                Combo_Language.SelectedIndex = 0;

            foreach (MessageEntry msg in messageList)
                MessagesGrid.Rows.Add(new object[] { msg.Name });

            MessagesGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            MessagesGrid_SelectionChanged(MessagesGrid, null);
            MessagesGrid.SelectionChanged += MessagesGrid_SelectionChanged;
            Combo_Language.SelectedIndexChanged += Combo_Language_SelectedIndexChanged;

            if (Program.IsRunningUnderMono)
                Helpers.ResetVerticalScrollbar(MessagesGrid);

            #endregion

            #region CCode

            var code = e.EmbeddedOverlayCode;

            if (code.Code != "")
            {
                if (Program.Settings.AutoComp_ActorSwitch)
                {
                    string compileErrors = "";
                    CCode.Compile(EditedFile.CHeader, Program.Settings.LinkerPaths, code, ref compileErrors, out _);
                    TextBox_CompileMsg.Text = compileErrors;
                }
                else
                {
                    TextBox_CompileMsg.Text = "Click \"Compile\"!";
                }
            }
            else
            {
                TextBox_CompileMsg.Text = "No code to compile.";
            }

            int index = 0;
            bool hasFunctions = code.Functions != null && code.Functions.Count > 0;

            foreach (KeyValuePair<ComboBox, ComboBox> kvp in FunctionComboBoxes)
            {
                ComboBox c = kvp.Key;
                ComboBox w = kvp.Value;

                c.SelectedIndexChanged -= Combo_Func_SelectedIndexChanged;
                if (w != null) w.SelectedIndexChanged -= Combo_Func_SelectedIndexChanged;

                if (!hasFunctions)
                {
                    c.DataSource = null;
                }
                else
                {
                    c.DisplayMember = "Symbol";
                    c.ValueMember = "Addr";
                    c.DataSource = code.Functions;
                    c.SelectedIndex = -1;
                    c.BindingContext = new BindingContext();
                    c.SelectedIndex = code.Functions.FindIndex(x => x.Symbol == code.SetFuncNames[index]);

                    if (w != null)
                        w.SelectedIndex = code.FuncsRunWhen[index, 1];
                }

                index++;
                c.SelectedIndexChanged += Combo_Func_SelectedIndexChanged;
                if (w != null) w.SelectedIndexChanged += Combo_Func_SelectedIndexChanged;
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
                    DialogResult DR = BigMessageBox.Show("Save changes before opening a new file?", "Save changes?", MessageBoxButtons.YesNoCancel);

                    if (DR == DialogResult.Yes)
                    {
                        SaveAs_Sync(this, null);
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

            NativeOpenFileDialog OFD = new NativeOpenFileDialog()
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
            Program.JsonPath = Path.Combine(Program.ExecPath, Helpers.GenerateNewJsonName());

            SetupLanguageCombo();
            InsertDataIntoActorListGrid();
        }

        private async void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            NativeSaveFileDialog SFD = new NativeSaveFileDialog
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
                await RunSave(SFD.FileName);

                Program.Settings.LastOpenPath = SFD.FileName;
            }
        }

        private async void FileMenu_Save_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            if (OpenedPath == "")
                FileMenu_SaveAs_Click(this, null);
            else
            {
                NPCSave = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);
                await RunSave(OpenedPath);
            }
        }

        private void SaveAs_Sync(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            NativeSaveFileDialog SFD = new NativeSaveFileDialog
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
                RunSaveSync(SFD.FileName);
                Program.Settings.LastOpenPath = SFD.FileName;
            }
        }

        private void Save_Sync(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            if (OpenedPath == "")
                SaveAs_Sync(this, null);
            else
            {
                NPCSave = JsonConvert.SerializeObject(EditedFile, Formatting.Indented);
                RunSaveSync(OpenedPath);
            }
        }

        private async Task RunSave(string path)
        {
            IProgress<Common.ProgressReport> progress = new Microsoft.Progress<Common.ProgressReport>(n => progressL.NewProgress = n);

            progressL.Visible = true;
            Program.SaveInProgress = true;

            try
            {
                progress.Report(new Common.ProgressReport("Saving...", 0));

                await TaskEx.Run(() =>
                {
                    FileOps.SaveNPCJSON(path, EditedFile, progress);
                });

                progress.Report(new Common.ProgressReport("Saved!", 100));
            }
            finally
            {
                Program.SaveInProgress = false;
            }
        }


        private void RunSaveSync(string Path)
        {
            FileOps.SaveNPCJSON(Path, EditedFile, null);
        }

        private async void RunSaveBinary(string Path)
        {
            IProgress<Common.ProgressReport> progress = new Microsoft.Progress<Common.ProgressReport>(n => progressL.NewProgress = n);
            progress.Report(new Common.ProgressReport("Starting...", 0));

            Program.CompileInProgress = true;
            this.MenuStrip.Enabled = false;
            this.Panel_Editor.Enabled = false;
            this.btn_FindMsg.Enabled = false;
            this.progressL.Visible = true;

            Program._stopWatch = Stopwatch.StartNew();
            Program.CompileThereWereErrors = false;

            compileTimer.Start();

            var cacheStatus = FileOps.GetCacheStatus(ref EditedFile);

            if (Program.Settings.CompileInParallel)
            {
                await FileOps.PreprocessCodeAndScripts(Path, Program.Settings.OutputDeps ? Path + ".d" : null, EditedFile, cacheStatus, progress, false);
            }
            else
            {
                await TaskEx.Run(() =>
                {
                    string baseDefines = Scripts.ScriptHelpers.GetBaseDefines(EditedFile);
                    FileOps.SaveBinaryFile(Path, Program.Settings.OutputDeps ? Path + ".d" : null, ref EditedFile, progress, baseDefines, cacheStatus, null, false);
                    CCode.CleanupStandardCompilationArtifacts();
                });
            }

            Program.Settings.LastSaveBinaryPath = Path;

            if (Program.Settings.LastSavePaths.Contains(Path))
                Program.Settings.LastSavePaths.Remove(Path);

            Program.Settings.LastSavePaths.Insert(0, Path);

            if (Program.Settings.LastSavePaths.Count > 10)
                Program.Settings.LastSavePaths.RemoveAt(10);

            UpdateSaveBinaryLastPathsList();

        }

        private async void FileMenu_SaveBinary_Click(object sender, EventArgs e)
        {
            if (EditedFile == null || Program.CompileInProgress)
                return;

            NativeSaveFileDialog SFD = new NativeSaveFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(Program.Settings.LastSaveBinaryPath),
                RestoreDirectory = true,
                FileName = "zobj.zobj",
                Filter = "Zelda Object Files | *.zobj"
            };
            DialogResult DR = SFD.ShowDialog();

            if (DR == DialogResult.OK)
            {

                RunSaveBinary(SFD.FileName);
            }
        }

        private void AddNewLocalizationToolClick(object sender, EventArgs e)
        {
            if (EditedFile != null)
            {
                string Language = "";
                DialogResult DR = InputBox.ShowInputDialog("Language name?", ref Language);

                if (DR != DialogResult.OK)
                    return;

                if (EditedFile.Languages.Contains(Language) || Language == Lists.DefaultLanguage)
                {
                    BigMessageBox.Show("Language already exists.");
                    return;
                }

                EditedFile.Languages.Add(Language);
                Combo_Language.Items.Add(Language);

                DialogResult Res = BigMessageBox.Show("Fill all actors with copies of the messages?", "Filling", MessageBoxButtons.YesNo);

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

                try
                {
                    Dicts.ReloadLanguages(EditedFile.Languages);
                    ReloadAllFonts();
                    Dicts.ReloadSpellcheckDicts(EditedFile.Languages);
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show("Add localization error:" + ex.Message);
                }

            }
        }

        private void RemoveLocalizationToolClick(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            if (EditedFile.Languages.Count == 0)
            {
                BigMessageBox.Show("There are no localizations.");
                return;
            }

            Windows.ComboPicker pick = new Windows.ComboPicker(EditedFile.Languages, "Which language?", "Language selection");

            try
            {
                if (pick.ShowDialog() == DialogResult.OK)
                {

                    if (EditedFile != null)
                    {
                        DialogResult Res = BigMessageBox.Show("Are you sure? All messages of that language will be removed.", "Confirmation", MessageBoxButtons.YesNoCancel);

                        if (Res == DialogResult.Yes)
                        {
                            EditedFile.Languages.Remove(pick.SelectedOption);

                            foreach (NPCEntry entry in EditedFile.Entries)
                                entry.Localization.RemoveAll(x => x.Language == pick.SelectedOption);

                            Combo_Language.Items.Remove(pick.SelectedOption);

                            try
                            {
                                Dicts.ReloadLanguages(EditedFile.Languages);
                                ReloadAllFonts();
                                Dicts.ReloadSpellcheckDicts(EditedFile.Languages);
                            }
                            catch (Exception ex)
                            {
                                BigMessageBox.Show("Remove localization error:" + ex.Message);
                            }

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
            MsgTextCJK_TextChanged(null, null);
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
                BigMessageBox.Show("No actor chosen, or actor is null.");
                return false;
            }

            if (OnTab)
            {
                if (TabControl.SelectedTab != Tab5_Scripts)
                {
                    BigMessageBox.Show("Select the script tab first.");
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

            if (!String.IsNullOrWhiteSpace(ScriptName))
            {
                if (SelectedEntry.Scripts.Count >= 255)
                {
                    BigMessageBox.Show("Cannot define more than 255 scripts.");
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

            string ScriptName = GetScriptName(TabControl_Scripts.SelectedTab.Text);

            if (!String.IsNullOrWhiteSpace(ScriptName))
            {
                (TabControl_Scripts.SelectedTab.Controls[0] as ScriptEditor).Script.Name = Name;
                TabControl_Scripts.SelectedTab.Text = Name;
            }
        }

        private void DeleteCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckScriptOpForValidity(true))
                return;

            DialogResult Res = BigMessageBox.Show("Are you sure you want to delete this script? You cannot reverse this action!", "Removing a script", MessageBoxButtons.YesNoCancel);

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
            float curScale = Program.Settings.GUIScale;
            Windows.Settings s = new Windows.Settings(ref EditedFile);
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
            MsgTextCJK_TextChanged(null, null);
            SplitMsgContainer_Paint(null, null);

            if (curScale != Program.Settings.GUIScale)
            {
                BigMessageBox.Show("The GUI Scale has changed, so the program will now restart.");
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
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
                                                                Textbox_CodeEditorArgs.Text.Replace("$CODEFILE", CCode.EditHeaderFilePath.AppendQuotation()).Replace("$CODEFOLDER", CCode.TempFolderPath.AppendQuotation()),
                                                                true
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

        private void CompileActorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NativeOpenFileDialog of = new NativeOpenFileDialog();
            of.Title = "Select the source file...";
            of.Filter = "C Files (*.c)|*.c|All files (*.*)|*.*";

            if (of.ShowDialog() == DialogResult.OK)
            {
                string cFile = of.FileName;

                of.Title = "Select the linker file...";
                of.Filter = "Linker files (*.ld;*.zsym)|*.ld;*.zsym|" +
                            "LD Files (*.ld)|*.ld|" +
                            "ZLINKER Symbols (*.zsym)|*.zsym|" +
                            "All files (*.*)|*.*";

                DialogResult ofD = of.ShowDialog();

                Windows.LongInputBox flg = new Windows.LongInputBox("Compile flags", "Add compile flags:", "");
                flg.ShowDialog();

                NativeSaveFileDialog sf = new NativeSaveFileDialog
                {
                    Title = "Select output ZOVL...",
                    Filter = "ZOVL files (*.zovl)|*.zovl|All files (*.*)|*.*"
                };

                if (sf.ShowDialog() == DialogResult.OK)
                {
                    string CompileMsgs = "";

                    try
                    {
                        List<CSymbol> symbols = null;
                        CCode.Compile(cFile, 
                                      ofD == DialogResult.OK ? $"{Program.Settings.LinkerPaths};{of.FileName}" : Program.Settings.LinkerPaths, 
                                      sf.FileName, 
                                      flg.inputText, 
                                      ref CompileMsgs, 
                                      out symbols);

                        if (symbols != null)
                        {
                            CSymbol c = symbols.FirstOrDefault(x => x.Symbol.Equals("sNpcMakerInit", StringComparison.InvariantCultureIgnoreCase))
                                     ?? symbols.FirstOrDefault(x => x.Symbol.Equals("sActorVars", StringComparison.InvariantCultureIgnoreCase));

                            if (c != null)
                            {
                                string config = $"alloc_type = 0\nvram_addr = 0x{CCode.BaseAddr.ToString("X")}\ninit_vars = 0x{(CCode.BaseAddr + c.Addr).ToString("X")}";
                                File.WriteAllText(Path.Combine(Path.GetDirectoryName(sf.FileName), "config.toml"), config);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CompileMsgs += $"{Environment.NewLine}{ex.Message}";
                    }

                    Windows.LongInputBox li = new Windows.LongInputBox("Results", "Compilation result:", Helpers.StripTerminalControlCodes(CompileMsgs));
                    li.ShowDialog();
                    return;
                }

            }
        }

        private void CheckDefinitionValidityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile == null)
                return;

            var errors = new StringBuilder();

            foreach (NPCEntry entry in EditedFile.Entries)
            {
                if (!String.IsNullOrEmpty(entry.HeaderPath))
                {
                    Dictionary<string, string> hDict = Helpers.GetDefinesFromHeaders(entry.HeaderPath);
                    entry.Hierarchy = (uint)FileOps.ResolveHeaderDefineOrFail(entry.NPCName, entry.SkeletonHeaderDefinition, hDict, entry.Hierarchy, errors);
                    entry.FileStart = (int)FileOps.ResolveHeaderDefineOrFail(entry.NPCName, entry.FileStartHeaderDefinition, hDict, (uint)entry.FileStart, errors);

                    foreach (var animation in entry.Animations)
                    {
                        var anm = Helpers.SplitHeaderDefsString(animation.HeaderDefinition);
                        animation.Address = FileOps.ResolveHeaderDefineOrFail(entry.NPCName, anm[1], hDict, animation.Address, errors);
                        animation.FileStart = (int)FileOps.ResolveHeaderDefineOrFail(entry.NPCName, anm[0], hDict, (uint)animation.FileStart, errors);
                    }

                    foreach (var displayList in entry.ExtraDisplayLists)
                    {
                        var dl = Helpers.SplitHeaderDefsString(displayList.HeaderDefinition);
                        displayList.Address = FileOps.ResolveHeaderDefineOrFail(entry.NPCName, dl[1], hDict, displayList.Address, errors);
                        displayList.FileStart = (int)FileOps.ResolveHeaderDefineOrFail(entry.NPCName, dl[0], hDict, (uint)displayList.FileStart, errors);
                    }

                    foreach (var segment in entry.Segments)
                    {
                        foreach (var segmentEntry in segment)
                        {
                            var seg = Helpers.SplitHeaderDefsString(segmentEntry.HeaderDefinition);
                            segmentEntry.Address = FileOps.ResolveHeaderDefineOrFail(entry.NPCName, seg[1], hDict, segmentEntry.Address, errors);
                            segmentEntry.FileStart = (int)FileOps.ResolveHeaderDefineOrFail(entry.NPCName, seg[0], hDict, (uint)segmentEntry.FileStart, errors);
                        }
                    }
                }
            }

            if (errors.Length > 0)
            {
                BigMessageBox.Show($"Check complete with errors. Saving report.", "Result");

                NativeSaveFileDialog sf = new NativeSaveFileDialog { FileName = "report.txt" };

                if (sf.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(sf.FileName, errors.ToString());
            }
            else
            {
                BigMessageBox.Show("Check complete. All valid fields have been updated.", "Result");
            }

            InsertDataToEditor();
        }

        private void CheckLocalizationConsistencyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (EditedFile.Languages.Count == 0)
            {
                BigMessageBox.Show("There are no localizations.");
                return;
            }

            Windows.ComboPicker pick = new Windows.ComboPicker(EditedFile.Languages, "Which language?", "Language selection");

            try
            {
                if (pick.ShowDialog() == DialogResult.OK)
                {
                    string selectedLanguage = pick.SelectedOption;
                    int selectedLangIndex = pick.SelectedIndex;

                    ConcurrentBag<string> reportItems = new ConcurrentBag<string>();

                    Parallel.ForEach(EditedFile.Entries, ent =>
                    {
                        if (ent.Localization.FindIndex(x => x.Language == selectedLanguage) != -1)
                        {
                            for (int i = 0; i < ent.Messages.Count; i++)
                            {
                                int numBoxesOg = 0;

                                try
                                {
                                    byte[] msgData = ent.Messages[i].ToBytes(Lists.DefaultLanguage).ToArray();
                                    ZeldaMessage.MessagePreview zm = new ZeldaMessage.MessagePreview(ZeldaMessage.Data.BoxType.Black, msgData);
                                    numBoxesOg = zm.MessageCount;
                                }
                                catch
                                {
                                    numBoxesOg = -1;
                                }

                                int locIndex = ent.Localization[selectedLangIndex].Messages.FindIndex(x => x.Name == ent.Messages[i].Name);

                                if (locIndex == -1)
                                {
                                    reportItems.Add($"{ent.NPCName}, {ent.Messages[i].Name} Fatal error, message missing?");
                                    continue;
                                }

                                int numBoxesLoc = 0;

                                try
                                {
                                    byte[] msgData = ent.Localization[selectedLangIndex].Messages[locIndex].ToBytes(selectedLanguage).ToArray();
                                    ZeldaMessage.MessagePreview zm = new ZeldaMessage.MessagePreview(ZeldaMessage.Data.BoxType.Black, msgData);
                                    numBoxesLoc = zm.MessageCount;
                                }
                                catch
                                {
                                    numBoxesLoc = -2;
                                }

                                if (numBoxesOg != numBoxesLoc)
                                    reportItems.Add($"{ent.NPCName}, {ent.Messages[i].Name} {numBoxesOg} vs {numBoxesLoc}");
                            }
                        }
                        else
                        {
                            reportItems.Add($"{ent.NPCName} does not contain this language.");
                        }
                    });

                    StringBuilder report = new StringBuilder();
                    foreach (string item in reportItems)
                    {
                        report.AppendLine(item);
                    }

                    NativeSaveFileDialog sf = new NativeSaveFileDialog { FileName = "report.txt" };

                    if (sf.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(sf.FileName, report.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Error: {ex.Message}");
            }

        }


        private void ImportLocalizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditedFile != null)
            {

                NativeOpenFileDialog OFD = new NativeOpenFileDialog()
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

                        List<string> LanguagesWithDefault = new List<string>() { Lists.DefaultLanguage };
                        LanguagesWithDefault.AddRange(LocalizationFile.Languages);

                        Windows.ComboPicker pick = new Windows.ComboPicker(LanguagesWithDefault, "Import which language?", "Language selection");

                        if (pick.ShowDialog() == DialogResult.OK)
                        {
                            string SelectedLanguage = pick.SelectedOption;
                            int SelectedLangIndex = pick.SelectedIndex;

                            int IndexInCur = EditedFile.Languages.FindIndex(x => x == SelectedLanguage);

                            if (IndexInCur != -1 || SelectedLanguage == Lists.DefaultLanguage)
                            {
                                DialogResult Res = BigMessageBox.Show("This language already exists. Replace it?", "Confirmation", MessageBoxButtons.YesNoCancel);

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

                                // Make a copy of all the default language textboxes if the language doesn't exist in an actor
                                if (ImportedEntry.Localization.FindIndex(x => x.Language == SelectedLanguage) != -1)
                                {
                                    if (SelectedLanguage != Lists.DefaultLanguage)
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
                                }

                                LocalizationEntry newLocalization = new LocalizationEntry();
                                newLocalization.Language = SelectedLanguage;

                                List<MessageEntry> messageList = GetLanguageMessageList(ImportedEntry, SelectedLanguage);

                                foreach (MessageEntry msg in entry.Messages)
                                {
                                    int textDefaultIndex = entry.Messages.FindIndex(x => x.Name == msg.Name);
                                    int importMsgIndex = messageList.FindIndex(x => x.Name == msg.Name);
                                    int curlocMsgIndex = -1;

                                    if (SelectedLangIndex != 0)
                                        curlocMsgIndex = entry.Localization[IndexInCur].Messages.FindIndex(x => x.Name == msg.Name);

                                    if (importMsgIndex != -1)
                                    {
                                        if (curlocMsgIndex != -1)
                                        {
                                            string textDefault = entry.Messages[textDefaultIndex].MessageText;
                                            string text = entry.Localization[IndexInCur].Messages[curlocMsgIndex].MessageText;
                                            string textNew = messageList[importMsgIndex].MessageText;

                                            if (textDefault != textNew && textDefault != text && text != textNew)
                                            {
                                                if (y2aRes != DialogResult.OK && y2aRes != DialogResult.Ignore)
                                                {
                                                    var w = new Windows.YesNoAllBox($"Localization of textbox {msg.Name} is already different. Update it with the one from the file?", "Message conflict");
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

                                y2aRes = DialogResult.None;

                                // Add messages which exist in the new language, but don't in the default 
                                foreach (MessageEntry msg in diff)
                                {
                                    if (y2aRes != DialogResult.OK && y2aRes != DialogResult.Ignore)
                                    {
                                        var w = new Windows.YesNoAllBox($"Message {msg.Name} doesn't exist in the default language. Add it?", "New messages");
                                        y2aRes = w.ShowDialog();
                                    }

                                    if (y2aRes == DialogResult.Yes || y2aRes == DialogResult.OK)
                                    {
                                        int msgIndex = messageList.IndexOf(msg);

                                        MessageEntry msgN = Helpers.Clone<MessageEntry>(msg);

                                        // If importing a non-default language, blank the text for the new entry in the default language.
                                        if (SelectedLangIndex != 0)
                                        {
                                            msgN.MessageText = "";
                                            msgN.MessageTextLines.Clear();
                                            msgN.Comment = "";
                                        }

                                        entry.Messages.Insert(msgIndex, msgN);

                                        // Add to each localized language too
                                        foreach (var loc in entry.Localization)
                                        {
                                            MessageEntry msgNLoc = Helpers.Clone<MessageEntry>(msg);

                                            // Blank if adding to a language which we're not importing to
                                            if (loc.Language != SelectedLanguage)
                                            {
                                                msgNLoc.MessageText = "";
                                                msgNLoc.MessageTextLines.Clear();
                                                msgNLoc.Comment = "";
                                            }

                                            loc.Messages.Insert(msgIndex, msgNLoc);
                                        }
                                    }
                                }
                            }

                            InsertDataToEditor();
                            SetupLanguageCombo();
                        }
                    }
                    catch (Exception ex)
                    {
                        BigMessageBox.Show($"Failed to read JSON: {ex.Message}");
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
                NativeSaveFileDialog SFD = new NativeSaveFileDialog
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
                NativeOpenFileDialog OFD = new NativeOpenFileDialog()
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
                                    BigMessageBox.Show($"NPC is malformed: Localization does not match default messages.");
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
                        BigMessageBox.Show($"Failed to read NPC Entry JSON: {ex.Message}");
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
                BigMessageBox.Show("NPC with that name already exists.");
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
            if (SelectedEntry == null)
                return;

            Windows.LongInputBox liB = new Windows.LongInputBox("Header definition", "Add header paths:", SelectedEntry.HeaderPath, true, "Header files (*.h;*.xml)|*.h;*.xml|All files (*.*)|*.*");

            if (liB.ShowDialog() == DialogResult.OK)
            {
                SelectedEntry.HeaderPath = liB.inputText;
                HeaderPathChanged();
            }
        }

        private bool ShowHDefineError(Common.HDefine hD)
        {
            if (hD.Value1 == null && hD.Value2 == null)
            {
                BigMessageBox.Show("Error parsing defined value");
                return true;
            }

            return false;
        }

        private void Tx_SkeletonName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Common.HDefine hD = Helpers.SelectSingleFromH(SelectedEntry);

            if (hD != null)
            {
                if (!ShowHDefineError(hD))
                {
                    Tx_SkeletonName.Text = hD.ToString();
                    NumUpDown_Hierarchy.Value = (decimal)hD.Value1;
                }
            }
        }

        private void Tx_FileStart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Common.HDefine hD = Helpers.SelectSingleFromH(SelectedEntry);

            if (hD != null)
            {
                if (!ShowHDefineError(hD))
                {
                    Tx_FileStartName.Text = hD.ToString();
                    numUpFileStart.Value = (decimal)hD.Value1;
                }
            }
        }

        private void HeaderPathChanged()
        {
            bool vis = !string.IsNullOrEmpty(SelectedEntry.HeaderPath);
            int fullWidth = ComboBox_HierarchyType.Width;

            NumUpDown_Hierarchy.Width = vis ? fullWidth / 2 : fullWidth;
            numUpFileStart.Width = vis ? fullWidth / 2 : fullWidth;
            Tx_SkeletonName.Visible = vis;
            Tx_FileStartName.Visible = vis;
            Col_HDefine.Visible = vis;
            ExtraDlists_HeaderDefinition.Visible = vis;

            for (int j = 0; j < TabControl_Segments.TabPages.Count; j++)
            {
                DataGridView grid = (TabControl_Segments.TabPages[j].Controls[0] as Controls.SegmentDataGrid).Grid;
                grid.Columns[(int)SegmentsColumns.HeaderDefinition].Visible = vis;
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
            short ObjectId = (short)Dicts.GetIntFromBiDict(Dicts.SFXes, Txtbox_ReactIfAtt.Text);

            if (ObjectId < -1)
                ObjectId = -1;

            Txtbox_ReactIfAtt.Text = Dicts.GetStringFromBiDict(Dicts.SFXes, ObjectId);
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
            short ObjectId = (short)Dicts.GetIntFromBiDict(Dicts.ObjectIDs, Txb_ObjectID.Text);

            if (ObjectId < 0)
                ObjectId = (short)SelectedEntry.ObjectID;

            Txb_ObjectID.Text = Dicts.GetStringFromBiDict(Dicts.ObjectIDs, ObjectId);

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
                BigMessageBox.Show("NPC with that name already exists.");
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
            bool wasNewRow = e.RowIndex >= SelectedEntry.Animations.Count;

            if (wasNewRow)
            {
                DataGrid_Animations.NotifyCurrentCellDirty(true);
                DataGrid_Animations.EndEdit();
            }

            if (e.ColumnIndex == (int)AnimGridColumns.Object)
            {
                PickableList objects = new PickableList(Lists.DictType.Objects, true, new List<int>() { -2, -3, -4, -5 });
                if (objects.ShowDialog() != DialogResult.OK) return;

                string id = objects.Chosen.ID.ToString();
                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = id;
                DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, id, e.GetType(), null));
                DataGrid_Animations.RefreshEdit();
            }
            else if (e.ColumnIndex == (int)AnimGridColumns.Address && SelectedEntry.AnimationType == 1)
            {
                PickableList anims = new PickableList(Lists.DictType.LinkAnims, true);
                if (anims.ShowDialog() != DialogResult.OK) return;

                string id = anims.Chosen.ID.ToString();
                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = anims.Chosen.ID.ToString("X");
                DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, id, e.GetType(), null));
                DataGrid_Animations.RefreshEdit();
            }
            else if (e.ColumnIndex == (int)AnimGridColumns.HeaderDefinition && SelectedEntry.AnimationType == 0)
            {
                var curr = Helpers.SplitHeaderDefsString((string)DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                Common.HDefine hD = Helpers.SelectOffsetFileStartFromH(SelectedEntry, curr[1], curr[0]);
                if (hD == null || ShowHDefineError(hD)) return;

                DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hD.ToString();
                DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, hD.ToString(), e.GetType(), null));

                if (hD.Value1 != null)
                {
                    DataGrid_Animations.Rows[e.RowIndex].Cells[(int)AnimGridColumns.Address].Value = hD.Value1String;
                    DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)AnimGridColumns.Address, hD.Value1String, e.GetType(), null));
                }

                if (hD.Value2 != null)
                {
                    DataGrid_Animations.Rows[e.RowIndex].Cells[(int)AnimGridColumns.FileStart].Value = hD.Value2String;
                    DataGridViewAnimations_CellParse(DataGrid_Animations, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)AnimGridColumns.FileStart, hD.Value2String, e.GetType(), null));
                }

                DataGrid_Animations.RefreshEdit();
            }
        }

        private static string GetAnimationFilestartString(Int32? FileStart)
        {
            FileStart = FileStart ?? -1;

            string cValue;

            if (FileStart == -2)
                cValue = "Loaded by user method";
            else if (FileStart < 0)
                cValue = "Same as main";
            else
                cValue = ((int)FileStart).ToString("X");

            return cValue;
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
                    DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Address].Value = Dicts.GetStringFromBiDict(Dicts.LinkAnims, (int)Address);


            if (SkipIndex != (int)AnimGridColumns.StartFrame)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.StartFrame].Value = 0;

            if (SkipIndex != (int)AnimGridColumns.FileStart)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.FileStart].Value = GetAnimationFilestartString(FileStart);

            if (SkipIndex != (int)AnimGridColumns.EndFrame)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.EndFrame].Value = 255;

            if (SkipIndex != (int)AnimGridColumns.Speed)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Speed].Value = 1.0;

            if (SkipIndex != (int)AnimGridColumns.Object)
                DataGrid_Animations.Rows[Index].Cells[(int)AnimGridColumns.Object].Value = Dicts.GetStringFromBiDict(Dicts.ObjectIDs, (int)ObjectID);
        }

        private void DataGridViewAnimations_CellParse(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex > 255)
            {
                BigMessageBox.Show("Cannot define more than 255 animations.");
                (sender as DataGridView).Rows.RemoveAt(e.RowIndex);
                e.ParsingApplied = true;
                return;
            }

            // Ensure the backing entry exists before any case runs
            if (SelectedEntry.Animations.Count - 1 < e.RowIndex)
                AddBlankAnim(e.ColumnIndex, e.RowIndex);

            AnimationEntry anim = SelectedEntry.Animations[e.RowIndex];
            e.ParsingApplied = true;

            switch (e.ColumnIndex)
            {
                case (int)AnimGridColumns.Name:
                    {
                        string name = e.Value.ToString();

                        if (!SanitizeName(ref name))
                            name = "Animation_" + e.RowIndex;

                        while (SelectedEntry.Animations.Any(x => x != anim && x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                            name += "_";

                        anim.Name = name;
                        e.Value = name;
                        DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = name;
                        return;
                    }
                case (int)AnimGridColumns.HeaderDefinition:
                    {
                        string name = e.Value.ToString();
                        anim.HeaderDefinition = name;
                        e.Value = name;

                        if (name != "")
                        {
                            Dictionary<string, string> hDict = Helpers.GetDefinesFromHeaders(SelectedEntry.HeaderPath);
                            Common.HDefine hD = Helpers.GetHDefineFromName(name, hDict);

                            if (hD?.Value1 != null)
                            {
                                anim.Address = (uint)hD.Value1;
                                DataGrid_Animations.Rows[e.RowIndex].Cells[(int)AnimGridColumns.Address].Value = hD.Value1String;
                            }
                        }

                        DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = name;
                        return;
                    }
                case (int)AnimGridColumns.Address:
                    {
                        if (SelectedEntry.AnimationType == 1)
                        {
                            try
                            {
                                int linkAnim = Dicts.GetIntFromBiDict(Dicts.LinkAnims, e.Value.ToString());
                                anim.Address = (uint)linkAnim;
                                e.Value = Dicts.GetStringFromBiDict(Dicts.LinkAnims, linkAnim);
                            }
                            catch
                            {
                                e.Value = Dicts.LinkAnims.Forward.First().Key;
                            }
                        }
                        else
                        {
                            try
                            {
                                anim.Address = Convert.ToUInt32(e.Value.ToString(), 16);
                            }
                            catch
                            {
                                anim.Address = 0;
                                e.Value = "0";
                            }
                        }

                        DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        return;
                    }
                case (int)AnimGridColumns.StartFrame:
                    {
                        try
                        {
                            anim.StartFrame = Convert.ToByte(e.Value.ToString());
                        }
                        catch
                        {
                            anim.StartFrame = 0;
                            e.Value = "";
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)AnimGridColumns.EndFrame:
                    {
                        try
                        {
                            anim.EndFrame = Convert.ToByte(e.Value.ToString());
                        }
                        catch
                        {
                            anim.EndFrame = 0xFF;
                            e.Value = "";
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)AnimGridColumns.Speed:
                    {
                        try
                        {
                            anim.Speed = (float)Convert.ToDecimal(e.Value);
                        }
                        catch
                        {
                            anim.Speed = 1.0f;
                            e.Value = 1.0f;
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)AnimGridColumns.Object:
                    {
                        try
                        {
                            int objectId = Dicts.GetIntFromBiDict(Dicts.ObjectIDs, e.Value.ToString());
                            if (objectId < -1) objectId = 0;

                            anim.ObjID = (short)objectId;
                            e.Value = Dicts.GetStringFromBiDict(Dicts.ObjectIDs, objectId);
                        }
                        catch
                        {
                            e.Value = Dicts.ObjectIDs.Forward.First().Key;
                        }

                        DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        return;
                    }
                case (int)AnimGridColumns.FileStart:
                    {
                        try
                        {
                            if (e.Value.ToString() == "-2")
                            {
                                anim.FileStart = -2;
                                e.Value = "Loaded by user method";
                            }
                            else
                            {
                                anim.FileStart = Convert.ToInt32(e.Value.ToString(), 16);
                            }
                        }
                        catch
                        {
                            anim.FileStart = -1;
                            e.Value = "Same as main";
                            DataGrid_Animations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
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

            var grid = sender as DataGridView;
            bool wasNewRow = e.RowIndex >= SelectedEntry.ExtraDisplayLists.Count;

            if (wasNewRow)
            {
                grid.NotifyCurrentCellDirty(true);
                grid.EndEdit();
            }

            switch (e.ColumnIndex)
            {
                case (int)EDlistsColumns.Object:
                    {
                        PickableList objects = new PickableList(Lists.DictType.Objects, true);
                        if (objects.ShowDialog() != DialogResult.OK) break;

                        string id = objects.Chosen.ID.ToString();
                        grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = id;
                        DataGridView_ExtraDLists_CellParsing(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, id, e.GetType(), null));
                        grid.Update();
                        break;
                    }
                case (int)EDlistsColumns.Limb:
                    {
                        PickableList subTypes = new PickableList(Dicts.LimbIndexSubTypes);
                        if (subTypes.ShowDialog() != DialogResult.OK) break;

                        string id = subTypes.Chosen.ID.ToString();
                        grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = id;
                        DataGridView_ExtraDLists_CellParsing(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, id, e.GetType(), null));
                        grid.Update();
                        break;
                    }
                case (int)EDlistsColumns.Color:
                    {
                        ColorDialog.Color = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                        if (ColorDialog.ShowDialog() != DialogResult.OK) break;

                        grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle()
                        {
                            BackColor = ColorDialog.Color,
                            SelectionBackColor = ColorDialog.Color,
                            SelectionForeColor = ColorDialog.Color
                        };

                        DataGridView_ExtraDLists_CellParsing(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, "", e.GetType(), null));
                        grid.Update();
                        break;
                    }
                case (int)EDlistsColumns.HeaderDefinition:
                    {
                        var curr = Helpers.SplitHeaderDefsString((string)grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        Common.HDefine hD = Helpers.SelectOffsetFileStartFromH(SelectedEntry, curr[1], curr[0]);
                        if (hD == null || ShowHDefineError(hD)) break;

                        grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hD.ToString();
                        DataGridView_ExtraDLists_CellParsing(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, hD.ToString(), e.GetType(), null));

                        if (hD.Value1 != null)
                        {
                            grid.Rows[e.RowIndex].Cells[(int)EDlistsColumns.Offset].Value = hD.Value1String;
                            DataGridView_ExtraDLists_CellParsing(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)EDlistsColumns.Offset, hD.Value1String, e.GetType(), null));
                        }

                        if (hD.Value2 != null)
                        {
                            grid.Rows[e.RowIndex].Cells[(int)EDlistsColumns.FileStart].Value = hD.Value2String;
                            DataGridView_ExtraDLists_CellParsing(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)EDlistsColumns.FileStart, hD.Value2String, e.GetType(), null));
                        }

                        break;
                    }
            }
        }

        private void AddBlankDList(int skipIndex, int index, string name = null, string headerDefinition = null, uint? address = null,
                                   float? transX = null, float? transY = null, float? transZ = null,
                                   short? rotX = null, short? rotY = null, short? rotZ = null,
                                   float? scale = null, short? limb = null, int? showType = null,
                                   short? objectID = null, Color? envColor = null, int? fileStart = null)
        {
            name = name ?? "DList_" + index;
            headerDefinition = headerDefinition ?? "";
            address = address ?? 0;
            transX = transX ?? 0;
            transY = transY ?? 0;
            transZ = transZ ?? 0;
            rotX = rotX ?? 0;
            rotY = rotY ?? 0;
            rotZ = rotZ ?? 0;
            scale = scale ?? 1f;
            limb = limb ?? 0;
            showType = showType ?? 0;
            objectID = objectID ?? -1;
            envColor = envColor ?? Color.FromArgb(255, 255, 255, 255);
            fileStart = fileStart ?? -1;

            SelectedEntry.ExtraDisplayLists.Add(new DListEntry(name, headerDefinition, (uint)address, (float)transX, (float)transY, (float)transZ, (Color)envColor,
                                                    (short)rotX, (short)rotY, (short)rotZ, (float)scale, (short)limb, (int)showType, (short)objectID, (int)fileStart));

            var row = DataGridView_ExtraDLists.Rows[index].Cells;

            if (skipIndex != (int)EDlistsColumns.Purpose)
                row[(int)EDlistsColumns.Purpose].Value = name;

            if (skipIndex != (int)EDlistsColumns.HeaderDefinition)
                row[(int)EDlistsColumns.HeaderDefinition].Value = headerDefinition;

            if (skipIndex != (int)EDlistsColumns.Color)
                row[(int)EDlistsColumns.Color].Style.BackColor = (Color)envColor;

            if (skipIndex != (int)EDlistsColumns.Offset)
                row[(int)EDlistsColumns.Offset].Value = address;

            if (skipIndex != (int)EDlistsColumns.Translation)
                row[(int)EDlistsColumns.Translation].Value = $"{transX},{transY},{transZ}";

            if (skipIndex != (int)EDlistsColumns.Rotation)
                row[(int)EDlistsColumns.Rotation].Value = $"{rotX},{rotY},{rotZ}";

            if (skipIndex != (int)EDlistsColumns.Scale)
                row[(int)EDlistsColumns.Scale].Value = scale;

            if (skipIndex != (int)EDlistsColumns.Limb)
                row[(int)EDlistsColumns.Limb].Value = limb;

            if (skipIndex != (int)EDlistsColumns.Object)
                row[(int)EDlistsColumns.Object].Value = objectID == -1 ? "---" : objectID.ToString();

            if (skipIndex != (int)EDlistsColumns.ShowType)
                row[(int)EDlistsColumns.ShowType].Value = ExtraDlists_ShowType.Items[(int)showType];

            if (skipIndex != (int)EDlistsColumns.FileStart)
                row[(int)EDlistsColumns.FileStart].Value = fileStart == -1 ? "Same as main" : fileStart.ToString();
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
                BigMessageBox.Show("Cannot define more than 255 extra display lists.");
                (sender as DataGridView).Rows.RemoveAt(e.RowIndex);
                e.ParsingApplied = true;
                return;
            }

            if (SelectedEntry.ExtraDisplayLists.Count - 1 < e.RowIndex)
                AddBlankDList(e.ColumnIndex, e.RowIndex);

            DListEntry dlist = SelectedEntry.ExtraDisplayLists[e.RowIndex];
            var grid = sender as DataGridView;
            e.ParsingApplied = true;

            switch (e.ColumnIndex)
            {
                case (int)EDlistsColumns.Purpose:
                    {
                        string name = e.Value.ToString();

                        if (!SanitizeName(ref name))
                            name = "Dlist_" + e.RowIndex;

                        if (SelectedEntry.ExtraDisplayLists.Count == e.RowIndex)
                            while (SelectedEntry.ExtraDisplayLists.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                                name += "_";

                        dlist.Name = name;
                        e.Value = name;
                        return;
                    }
                case (int)EDlistsColumns.HeaderDefinition:
                    {
                        dlist.HeaderDefinition = e.Value.ToString();
                        return;
                    }
                case (int)EDlistsColumns.Color:
                    {
                        dlist.Color = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                        return;
                    }
                case (int)EDlistsColumns.Offset:
                    {
                        try
                        {
                            dlist.Address = Convert.ToUInt32(e.Value.ToString(), 16);
                        }
                        catch
                        {
                            dlist.Address = 0;
                            e.Value = 0;
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)EDlistsColumns.Translation:
                    {
                        float[] transl = GetXYZTranslation(e.Value.ToString());
                        dlist.TransX = transl[0];
                        dlist.TransY = transl[1];
                        dlist.TransZ = transl[2];
                        e.Value = $"{transl[0]},{transl[1]},{transl[2]}";
                        grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        return;
                    }
                case (int)EDlistsColumns.Rotation:
                    {
                        short[] rot = GetXYZRotation(e.Value.ToString());
                        dlist.RotX = rot[0];
                        dlist.RotY = rot[1];
                        dlist.RotZ = rot[2];
                        e.Value = $"{rot[0]},{rot[1]},{rot[2]}";
                        grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        return;
                    }
                case (int)EDlistsColumns.Scale:
                    {
                        try
                        {
                            dlist.Scale = (float)Convert.ToDecimal(e.Value);
                        }
                        catch
                        {
                            dlist.Scale = 1f;
                            e.Value = 0;
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)EDlistsColumns.Limb:
                    {
                        try
                        {
                            short value = Convert.ToInt16(e.Value);
                            string positionType = Dicts.GetStringFromStringIntDict(Dicts.LimbIndexSubTypes, value, null);
                            dlist.Limb = value;
                            e.Value = positionType ?? e.Value;
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        catch
                        {
                            dlist.Limb = 0;
                            e.Value = 0;
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)EDlistsColumns.Object:
                    {
                        try
                        {
                            short objectId = (short)Dicts.GetIntFromBiDict(Dicts.ObjectIDs, e.Value.ToString());
                            dlist.ObjectID = objectId;
                            e.Value = Dicts.GetStringFromBiDict(Dicts.ObjectIDs, objectId);
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        catch
                        {
                            dlist.ObjectID = -1;
                            e.Value = Dicts.ObjectIDs.Forward.First();
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)EDlistsColumns.ShowType:
                    {
                        dlist.ShowType = Dicts.GetIntFromStringIntDict(Dicts.LimbShowSubTypes, e.Value.ToString(), 0);
                        return;
                    }
                case (int)EDlistsColumns.FileStart:
                    {
                        try
                        {
                            int value = Convert.ToInt32(e.Value.ToString(), 16);

                            if (value < 0)
                            {
                                value = -1;
                                e.Value = -1;
                                grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Same as main";
                            }

                            dlist.FileStart = value;
                        }
                        catch
                        {
                            dlist.FileStart = -1;
                            e.Value = "Same as main";
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
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

            var grid = sender as DataGridView;
            int segIndex = TabControl_Segments.SelectedIndex;
            bool wasNewRow = e.RowIndex >= SelectedEntry.Segments[segIndex].Count;

            if (wasNewRow)
            {
                grid.NotifyCurrentCellDirty(true);
                grid.EndEdit();
            }

            if (e.ColumnIndex == (int)SegmentsColumns.Object)
            {
                PickableList objects = new PickableList(Lists.DictType.Objects, true);
                if (objects.ShowDialog() != DialogResult.OK) return;

                string id = objects.Chosen.ID.ToString();
                grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = id;
                DataGridViewSegments_CellParse(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, id, e.GetType(), null));
                grid.Update();
            }
            else if (e.ColumnIndex == (int)SegmentsColumns.HeaderDefinition)
            {
                var curr = Helpers.SplitHeaderDefsString((string)grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                Common.HDefine hD = Helpers.SelectOffsetFileStartFromH(SelectedEntry, curr[1], curr[0]);
                if (hD == null || ShowHDefineError(hD)) return;

                grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = hD.ToString();
                DataGridViewSegments_CellParse(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, e.ColumnIndex, hD.ToString(), e.GetType(), null));

                if (hD.Value1 != null)
                {
                    grid.Rows[e.RowIndex].Cells[(int)SegmentsColumns.Address].Value = hD.Value1String;
                    DataGridViewSegments_CellParse(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)SegmentsColumns.Address, hD.Value1String, e.GetType(), null));
                }

                if (hD.Value2 != null)
                {
                    grid.Rows[e.RowIndex].Cells[(int)SegmentsColumns.FileStart].Value = hD.Value2String;
                    DataGridViewSegments_CellParse(grid, new DataGridViewCellParsingEventArgs(e.RowIndex, (int)SegmentsColumns.FileStart, hD.Value2String, e.GetType(), null));
                }

                grid.Update();
            }
        }

        private void AddBlankSeg(int skipIndex, int index, int segment, string name = null, string headerDefinition = null, uint? address = null, short? objectID = null, int? fileStart = null)
        {
            name = name ?? "Texture_" + index;
            headerDefinition = headerDefinition ?? "";
            address = address ?? 0;
            objectID = objectID ?? -1;
            fileStart = fileStart ?? -1;

            SelectedEntry.Segments[segment].Add(new SegmentEntry(name, headerDefinition, (uint)address, (short)objectID, (int)fileStart));

            DataGridView dgv = (TabControl_Segments.TabPages[segment].Controls[0] as Controls.SegmentDataGrid).Grid;
            var row = dgv.Rows[index].Cells;

            if (skipIndex != (int)SegmentsColumns.Name)
                row[(int)SegmentsColumns.Name].Value = name;

            if (skipIndex != (int)SegmentsColumns.HeaderDefinition)
                row[(int)SegmentsColumns.HeaderDefinition].Value = headerDefinition; // was: Name (bug)

            if (skipIndex != (int)SegmentsColumns.Address)
                row[(int)SegmentsColumns.Address].Value = address;

            if (skipIndex != (int)SegmentsColumns.Object)
                row[(int)SegmentsColumns.Object].Value = Dicts.GetStringFromBiDict(Dicts.ObjectIDs, (int)objectID);

            if (skipIndex != (int)SegmentsColumns.FileStart)
                row[(int)SegmentsColumns.FileStart].Value = fileStart == -1 ? "Same as main" : ((int)fileStart).ToString("X");
        }

        private void DataGridViewSegments_CellParse(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.RowIndex > 31)
            {
                BigMessageBox.Show("Cannot define more than 32 textures per segment.");
                (sender as DataGridView).Rows.RemoveAt(e.RowIndex);
                e.ParsingApplied = true;
                return;
            }

            var grid = sender as DataGridView;
            int segIndex = TabControl_Segments.SelectedIndex;
            var segList = SelectedEntry.Segments[segIndex];

            if (segList.Count - 1 < e.RowIndex)
                AddBlankSeg(e.ColumnIndex, e.RowIndex, segIndex);

            SegmentEntry seg = segList[e.RowIndex];
            e.ParsingApplied = true;

            switch (e.ColumnIndex)
            {
                case (int)SegmentsColumns.Name:
                    {
                        string name = e.Value.ToString();

                        if (!SanitizeName(ref name))
                            name = "Data_" + e.RowIndex;

                        if (segList.Count == e.RowIndex)
                            while (segList.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                                name += "_";

                        seg.Name = name;
                        e.Value = name;
                        return;
                    }
                case (int)SegmentsColumns.HeaderDefinition:
                    {
                        seg.HeaderDefinition = e.Value.ToString();
                        return;
                    }
                case (int)SegmentsColumns.Address:
                    {
                        try
                        {
                            seg.Address = Convert.ToUInt32(e.Value.ToString(), 16);
                        }
                        catch
                        {
                            seg.Address = 0;
                            e.Value = 0;
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)SegmentsColumns.Object:
                    {
                        try
                        {
                            short objectId = (short)Dicts.GetIntFromBiDict(Dicts.ObjectIDs, e.Value.ToString());
                            seg.ObjectID = objectId;
                            e.Value = Dicts.GetStringFromBiDict(Dicts.ObjectIDs, objectId);
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        catch
                        {
                            seg.ObjectID = -1;
                            e.Value = Dicts.ObjectIDs.Forward.First();
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
                        return;
                    }
                case (int)SegmentsColumns.FileStart:
                    {
                        try
                        {
                            int value = Convert.ToInt32(e.Value.ToString(), 16);

                            if (value < 0)
                            {
                                value = -1;
                                e.Value = -1;
                                grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Same as main";
                            }

                            seg.FileStart = value;
                        }
                        catch
                        {
                            seg.FileStart = -1;
                            e.Value = "Same as main";
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.Value;
                        }
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
            if (e.RowIndex < 0 || e.ColumnIndex != 1)
                return;

            if (ColorDialog.ShowDialog() != DialogResult.OK)
                return;

            ColorsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = new DataGridViewCellStyle()
            {
                BackColor = ColorDialog.Color,
                SelectionBackColor = ColorDialog.Color,
                SelectionForeColor = ColorDialog.Color
            };

            if (SelectedEntry.DisplayListColors.Count - 1 < e.RowIndex)
            {
                SelectedEntry.DisplayListColors.Add(new ColorEntry("", ColorDialog.Color));
                ColorsDataGridView.Rows[e.RowIndex].Cells[0].Value = "";
            }
            else
            {
                SelectedEntry.DisplayListColors[e.RowIndex].Color = ColorDialog.Color;
            }
        }

        private void ColorsDataGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex != 0)
                return;

            var colors = SelectedEntry.DisplayListColors;

            if (colors.Count - 1 < e.RowIndex)
            {
                Color white = Color.FromArgb(0, 0, 0, 0);
                colors.Add(new ColorEntry(e.Value.ToString(), white));
                ColorsDataGridView.Rows[e.RowIndex].Cells[1].Style = new DataGridViewCellStyle()
                {
                    BackColor = white,
                    SelectionBackColor = white,
                    SelectionForeColor = white
                };
            }
            else
            {
                colors[e.RowIndex].Limbs = e.Value.ToString();
            }

            try
            {
                SelectedEntry.ParseColorEntries();
            }
            catch
            {
                colors[e.RowIndex].Limbs = "";
                e.Value = "";
            }

            e.ParsingApplied = true;
        }


        #endregion

        #region Messages

        private void MsgText_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (MessagesContextMenu.MenuStrip == null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessagesContextMenu.MakeContextMenu(Combo_Language.Text);
                    Cursor.Current = Cursors.Default;
                }

                if (ChkBox_UseCJK.Checked)
                    MessagesContextMenu.SetTextBox(sender as FCTB_MonoCJK);
                else
                    MessagesContextMenu.SetTextBox(sender as FCTB_Mono);

                MessagesContextMenu.MenuStrip.Show(sender as Control, e.Location);
            }
            else if (e.Button == MouseButtons.Left && Program.IsRunningUnderMono)
            {
                if (MessagesContextMenu.MenuStrip != null)
                {
                    MessagesContextMenu.MenuStrip.Hide();
                    MessagesContextMenu.MenuStrip.Dispose();
                    MessagesContextMenu.MenuStrip = null;
                }
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
                DialogResult Res = BigMessageBox.Show("Are you sure? All messages of that language within this actor will be removed.", "Confirmation", MessageBoxButtons.YesNo);

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

                if (SelLang != Lists.DefaultLanguage)
                {
                    if (SelectedEntry.Localization.FindIndex(x => x.Language == SelLang) == -1)
                    {
                        DialogResult Res = BigMessageBox.Show("This actor doesn't contain this language. Create it?", "Confirmation", MessageBoxButtons.YesNo);

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

            MsgTextDefault.Tag = Lists.DefaultLanguage;
            MsgText.Tag = Combo_Language.Text;

            int curSelMsg = 0;

            lastPreviewData = null;
            lastPreviewDataOrig = null;

            if (MessagesGrid.SelectedRows.Count != 0)
                curSelMsg = MessagesGrid.SelectedRows[0].Index;

            if (Combo_Language.SelectedIndex != 0)
            {
                MsgEntrySplitContainer.Panel1Collapsed = false;
                MsgEntrySplitContainer.Panel1MinSize = 25;
                MsgEntrySplitContainer.Panel2MinSize = 25;
                MsgEntrySplitContainer.SplitterDistance = MsgEntrySplitContainer.Width / 2;
                MsgEntrySplitContainer.IsSplitterFixed = false;
            }
            else
            {
                MsgEntrySplitContainer.Panel1Collapsed = true;
                MsgEntrySplitContainer.Panel1MinSize = 0;
                MsgEntrySplitContainer.Panel2MinSize = 0;
                MsgEntrySplitContainer.SplitterDistance = 0;
                MsgEntrySplitContainer.IsSplitterFixed = true;
            }

            MessagesGrid_SelectionChanged(MessagesGrid, null);

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

            RequestPreviewUpdate();
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
                BigMessageBox.Show("Error moving message up: " + ex.Message);
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
                BigMessageBox.Show("Error moving message down: " + ex.Message);
            }
        }

        private void SetMsgBackground(MessageEntry Loc, MessageEntry Default)
        {
            if (Loc.Type == (int)ZeldaMessage.Data.BoxType.None_White)
                PreviewSplitContainer.Panel2.BackColor = Color.Black;
            else
                PreviewSplitContainer.Panel2.BackColor = Color.White;

            if (Default.Type == (int)ZeldaMessage.Data.BoxType.None_White)
                PreviewSplitContainer.Panel1.BackColor = Color.Black;
            else
                PreviewSplitContainer.Panel1.BackColor = Color.White;
        }

        private List<MessageEntry> GetLanguageMessageList(NPCEntry entry, string Language)
        {
            List<MessageEntry> MessageList = entry.Messages;

            if (Language != Lists.DefaultLanguage)
            {
                int LocalizationIndex = entry.Localization.FindIndex(x => x.Language == Language);

                if (LocalizationIndex != -1)
                    MessageList = entry.Localization[LocalizationIndex].Messages;
            }

            return MessageList;
        }

        private void SplitMsgContainer_Paint(object sender, PaintEventArgs e)
        {
            int msgCommentSize = (int)(18 * Program.Settings.GUIScale);
            int xoffs = 0;
            int yoffs = 0;

            pictureBox_Comment.Location = new Point(MsgTextDefault.Location.X + MsgTextDefault.Width - msgCommentSize - xoffs,
                                                    MsgTextDefault.Location.Y + MsgTextDefault.Height - msgCommentSize - yoffs);
            pictureBox_Comment.Size = new Size(msgCommentSize, msgCommentSize);

            MsgText.VerticalScroll.Visible = true;
            MsgText.HorizontalScroll.Visible = true;
            MsgTextCJK.VerticalScroll.Visible = true;
            MsgTextCJK.HorizontalScroll.Visible = true;

            pictureBox_Comment_Loc.Visible = IsDefaultLanguageSelected() || Program.Settings.AllowCommentsOnLoc;

            if (ChkBox_UseCJK.Checked)
            {
                pictureBox_Comment_Loc.Location = new Point(MsgTextCJK.Location.X + MsgTextCJK.Width - msgCommentSize - xoffs,
                                                        MsgTextCJK.Location.Y + MsgTextCJK.Height - msgCommentSize - yoffs);
                pictureBox_Comment_Loc.Size = new Size(msgCommentSize, msgCommentSize);
            }
            else
            {
                pictureBox_Comment_Loc.Location = new Point(MsgText.Location.X + MsgText.Width - msgCommentSize - xoffs,
                                                        MsgText.Location.Y + MsgText.Height - msgCommentSize - yoffs);
                pictureBox_Comment_Loc.Size = new Size(msgCommentSize, msgCommentSize);
            }
        }

        private bool IsDefaultLanguageSelected()
        {
            string SelLang = Combo_Language.Text;
            return (SelLang == Lists.DefaultLanguage);
        }

        private string CommentInput(string startComment)
        {
            Windows.LongInputBox lib = new Windows.LongInputBox("Comment input", "Message comment:", startComment);

            DialogResult dr = lib.ShowDialog();

            if (dr == DialogResult.OK)
                return String.IsNullOrWhiteSpace(lib.inputText) ? null : lib.inputText;
            else
                return startComment;
        }

        private MessageEntry GetCurMsgEntry(string Language)
        {
            if (SelectedEntry == null)
                return null;

            if (MessagesGrid.SelectedRows.Count == 0)
                return null;

            int index = MessagesGrid.SelectedRows[0].Index;

            if (index >= MessagesGrid.RowCount)
                index = MessagesGrid.RowCount - 1;

            List<MessageEntry> messageList = GetLanguageMessageList(SelectedEntry, Language);
            MessageEntry entry = messageList[index];
            return entry;
        }

        private void PictureBox_Comment_DoubleClick(object sender, EventArgs e)
        {
            MessageEntry entry = GetCurMsgEntry(Lists.DefaultLanguage);

            if (entry != null)
            {
                entry.Comment = CommentInput(entry.Comment);
                SetPictureBoxCommentImageToolTip(pictureBox_Comment, msgCommentTooltip, entry.Comment);
            }
        }

        private void PictureBox_Comment_Loc_DoubleClick(object sender, EventArgs e)
        {
            MessageEntry entry = GetCurMsgEntry(Combo_Language.Text);

            if (entry != null)
            {
                entry.Comment = CommentInput(entry.Comment);
                SetPictureBoxCommentImageToolTip(pictureBox_Comment_Loc, msgCommentTooltipLoc, entry.Comment);
            }
        }

        private void SetPictureBoxCommentImageToolTip(PictureBox box, Controls.BigToolTip tip, string comment)
        {
            if (box != null && !box.IsDisposed)
            {
                if (String.IsNullOrWhiteSpace(comment))
                    box.Image = Properties.Resources.commentNo;
                else
                    box.Image = Properties.Resources.comment;
            }

            if (box == null || box.IsDisposed || !box.IsHandleCreated)
                return;

            // On Mono, delay tooltip operations to avoid X11 timing issues
            if (Program.IsRunningUnderMono)
                DelayedSetToolTip(box, tip, comment);
            else
                SetToolTipDirect(box, tip, comment);
        }

        private void DelayedSetToolTip(PictureBox box, Controls.BigToolTip tip, string comment)
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                timer.Dispose();

                if (box != null && !box.IsDisposed && box.IsHandleCreated)
                    SetToolTipDirect(box, tip, comment);
            };

            timer.Start();
        }

        private void SetToolTipDirect(PictureBox box, Controls.BigToolTip tip, string comment)
        {
            if (comment != null)
                tip.SetToolTip(box, comment.LimitToCharNum(1000).WrapToLength(80));
            else
                tip.SetToolTip(box, null);
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
            MsgTextCJK.TextChanged -= MsgTextCJK_TextChanged;
            Combo_MsgType.SelectedIndexChanged -= Combo_MsgType_SelectedIndexChanged;

            if (SelectedEntry.Messages.Count != 0)
            {
                int index = MessagesGrid.SelectedRows[0].Index;

                if (index >= MessagesGrid.RowCount)
                    index = MessagesGrid.RowCount - 1;

                List<MessageEntry> MessageList = GetLanguageMessageList(SelectedEntry, Combo_Language.Text);

                MessageEntry Entry = MessageList[index];
                MsgText.Text = Entry.MessageText;
                MsgTextCJK.Text = Entry.MessageText;

                MsgTextCJK.ClearUndo();
                MsgText.ClearUndo();

                SetPictureBoxCommentImageToolTip(pictureBox_Comment_Loc, msgCommentTooltipLoc, Entry.Comment);

                MessageEntry DefaultEntry = SelectedEntry.Messages[index];
                MsgTextDefault.Text = DefaultEntry.MessageText;
                MsgTextDefault.ClearUndo();

                SetPictureBoxCommentImageToolTip(pictureBox_Comment, msgCommentTooltip, DefaultEntry.Comment);

                Combo_MsgType.SelectedIndex = Entry.Type;
                Combo_MsgPos.SelectedIndex = Entry.Position;

                SetMsgBackground(Entry, DefaultEntry);
                RequestPreviewUpdate();
            }

            MsgText.TextChanged += MsgText_TextChanged;
            MsgTextCJK.TextChanged += MsgTextCJK_TextChanged;
            Combo_MsgType.SelectedIndexChanged += Combo_MsgType_SelectedIndexChanged;

            MsgText.ToolTip.RemoveAll();
            PerformSpellCheck();
        }

        private void MsgText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FastColoredTextBoxNS.FastColoredTextBox box = (FastColoredTextBoxNS.FastColoredTextBox)sender;
            box.ToolTip.RemoveAll();

            string hoverWord = box.SelectedText;
            string Language = Combo_Language.Text;

            if (Program.dictionary.ContainsKey(Language))
            {
                if (!Program.dictionary[Language].Check(hoverWord))
                {
                    List<string> sugg = Program.dictionary[Language].Suggest(hoverWord).ToList();

                    box.ToolTip.SetToolTip(box, String.Join(Environment.NewLine, sugg));

                }
            }
        }

        private void MsgTextCJK_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FastColoredTextBoxCJK.FastColoredTextBox box = (FastColoredTextBoxCJK.FastColoredTextBox)sender;
            box.ToolTip.RemoveAll();

            string hoverWord = box.SelectedText;
            string Language = Combo_Language.Text;

            if (Program.dictionary.ContainsKey(Language))
            {
                if (!Program.dictionary[Language].Check(hoverWord))
                {
                    List<string> sugg = Program.dictionary[Language].Suggest(hoverWord).ToList();

                    box.ToolTip.SetToolTip(box, String.Join(Environment.NewLine, sugg));

                }
            }
        }

        private void PerformSpellCheck()
        {
            MsgText.ClearStyle(StyleIndex.All);

            if (MsgText.Text.Length == 0 || !Program.Settings.Spellcheck)
                return;

            string tagLess = Regex.Replace(MsgText.Text.ToUpper().Replace(Environment.NewLine, " "), @"<([\s\S]*?)>", string.Empty, RegexOptions.Compiled);
            string[] strings = tagLess.StripPunctuation().Split(' ');

            Range r = new Range(MsgText, 0, 0, MsgText.Text.Length - 1, MsgText.LinesCount - 1);

            foreach (string s in strings)
            {
                try
                {
                    string Language = Combo_Language.Text;

                    if (Program.dictionary.ContainsKey(Language))
                    {
                        if (!Program.dictionary[Language].Check(s))
                            r.SetStyle(SyntaxHighlighter.UnderlineStyle, @"\b" + s + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void MsgText_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (!MsgText.Visible)
                return;

            MsgTextCJK.Text = MsgText.Text;

            RequestPreviewUpdate();
            PerformSpellCheck();

        }

        private void MsgTextCJK_TextChanged(object sender, FastColoredTextBoxCJK.TextChangedEventArgs e)
        {
            if (!MsgTextCJK.Visible)
                return;

            MsgText.Text = MsgTextCJK.Text;

            RequestPreviewUpdate();
            PerformSpellCheck();
        }

        private MessageEntry CreateErrorEntry()
        {
            MessageEntry Entry = new MessageEntry();
            Entry.MessageText = "Error: Over maximum size.";
            return Entry;
        }

        private void SaveMsgPreviewToImage(MessageEntry entry, string Language)
        {
            Bitmap original = GetMessagePreviewImage(entry, Language, ref lastPreviewData);

            NativeSaveFileDialog sfd = new NativeSaveFileDialog();
            sfd.Filter =
                "All Supported Images (*.png;*.bmp;*.jpg;*.jpeg)|*.png;*.bmp;*.jpg;*.jpeg|" +
                "PNG Files (*.png)|*.png|" +
                "BMP Files (*.bmp)|*.bmp|" +
                "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "All files (*.*)|*.*";

            sfd.FileName = "preview.png";

            if (original != null && sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string ext = Path.GetExtension(sfd.FileName).ToLower();

                    if (ext == ".bmp" || ext == ".jpg" || ext == ".jpeg")
                    {
                        using (Bitmap flattened = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb))
                        using (Graphics g = Graphics.FromImage(flattened))
                        {
                            g.Clear(Color.White);
                            g.DrawImage(original, 0, 0);

                            if (ext == ".bmp")
                                flattened.Save(sfd.FileName, ImageFormat.Bmp);
                            else
                                flattened.Save(sfd.FileName, ImageFormat.Jpeg);
                        }
                    }
                    else
                    {
                        original.Save(sfd.FileName, ImageFormat.Png);
                    }
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show("Error saving preview: " + ex.Message);
                }
            }
        }

        private void MsgPreviewDefault_Click(object sender, EventArgs e)
        {
            SaveMsgPreviewToImage(GetCurLocMsgEntry(), Combo_Language.Text);
        }

        private void MsgPreview_DoubleClick(object sender, EventArgs e)
        {
            SaveMsgPreviewToImage(GetDefaultMsgEntry(), Lists.DefaultLanguage);
        }

        private Bitmap GetMessagePreviewImage(MessageEntry Entry, string Language, ref Common.SavedMsgPreviewData savedPreviewData)
        {
            List<byte> Data = null;

            try
            {
                Data = Entry.ToBytes(Language);
            }
            catch { }

            if (Data == null || (Data.Count == 0 && !String.IsNullOrEmpty(Entry.MessageText)))
                return null;

            if (Data.Count > 1280)
                Entry = CreateErrorEntry();

            try
            {
                Data = Entry.ToBytes(Language);
            }
            catch { }

            float[] fontWidths = null;
            byte[] font = null;
            float[] fontWidthsEx = null;
            byte[] fontEx = null;

            if (fontsWidths.ContainsKey(Language) && fonts.ContainsKey(Language))
            {
                fontWidths = fontsWidths[Language];
                font = fonts[Language];
            }
            else if (fontsWidths.ContainsKey(Lists.DefaultLanguage) && fonts.ContainsKey(Lists.DefaultLanguage))
            {
                fontWidths = fontsWidths[Lists.DefaultLanguage];
                font = fonts[Lists.DefaultLanguage];
            }

            if (Dicts.LanguageDefs.ContainsKey(Language) && !String.IsNullOrWhiteSpace(Dicts.LanguageDefs[Language].ExtraFont))
            {
                string exFontName = Dicts.LanguageDefs[Language].ExtraFont;

                if (exfontsWidths.ContainsKey(exFontName))
                    fontWidthsEx = exfontsWidths[exFontName];

                if (exfonts.ContainsKey(exFontName))
                    fontEx = exfonts[exFontName];
            }

            ZeldaMessage.MessagePreview mp = new ZeldaMessage.MessagePreview(
                                                                                (Data.BoxType)Entry.Type,
                                                                                Data.ToArray(),
                                                                                fontWidths,
                                                                                font,
                                                                                EditedFile.SpaceFromFont,
                                                                                fontWidthsEx,
                                                                                fontEx,
                                                                                Language
                                                                            );

            Bitmap bmp = null;

            Common.SavedMsgPreviewData localSaved = savedPreviewData;

            bool canReuse =
                localSaved != null &&
                localSaved.MessageArrays != null &&
                mp.Message.Count == localSaved.MessageArrays.Count &&
                Entry.Type == localSaved.Type;

            if (canReuse)
                bmp = (Bitmap)localSaved.previewImage;

            Bitmap[] previews = new Bitmap[mp.MessageCount];

            Parallel.For(0, mp.MessageCount, i =>
            {
                if (!canReuse || !mp.Message[i].SequenceEqual(localSaved.MessageArrays[i]))
                    previews[i] = mp.GetPreview(i, Program.Settings.ImproveTextMsgReadability, 1.5f * Program.Settings.GUIScale);
            });

            Graphics grfx = null;

            for (int i = 0; i < previews.Length; i++)
            {
                Bitmap box = previews[i];
                if (box == null)
                    continue;

                if (bmp == null)
                {
                    bmp = new Bitmap(box.Width, mp.MessageCount * box.Height);
                    bmp.MakeTransparent();
                }

                if (grfx == null)
                    grfx = Graphics.FromImage(bmp);

                if (Program.IsRunningUnderMono)
                {
                    bmp.DrawImageSourceCopySafe(box, 0, box.Height * i);
                }
                else
                {
                    grfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    grfx.DrawImage(box, 0, box.Height * i);
                }

                box.Dispose();
            }

            grfx?.Dispose();

            savedPreviewData = new Common.SavedMsgPreviewData
            {
                MessageArrays = mp.Message,
                previewImage = bmp,
                Type = Entry.Type,
                Position = Entry.Position
            };

            return bmp;
        }

        private MessageEntry GetCurLocMsgEntry()
        {
            return Combo_Language.SelectedIndex == 0 ?
                SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index] :
                SelectedEntry.Localization[Combo_Language.SelectedIndex - 1].Messages[MessagesGrid.SelectedRows[0].Index];
        }

        private MessageEntry GetDefaultMsgEntry()
        {
            return SelectedEntry.Messages[MessagesGrid.SelectedRows[0].Index];
        }

        private void PanelMsgPreview_Resize(object sender, EventArgs e)
        {
            MsgPreview.Left = (this.PanelMsgPreview.Width - MsgPreview.Width) / 2;
            RequestPreviewUpdate();
        }

        private void Combo_MsgPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = GetCurLocMsgEntry();
            Entry.Position = Combo_MsgPos.SelectedIndex;
        }

        private void Combo_MsgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count == 0)
                return;

            MessageEntry Entry = GetCurLocMsgEntry();
            MessageEntry Default = GetDefaultMsgEntry();
            Entry.Type = Combo_MsgType.SelectedIndex;

            MsgText.TextChanged -= MsgText_TextChanged;
            MsgTextCJK.TextChanged -= MsgTextCJK_TextChanged;
            Combo_MsgType.SelectedIndexChanged -= Combo_MsgType_SelectedIndexChanged;

            SetMsgBackground(Entry, Default);
            RequestPreviewUpdate();

            MsgText.TextChanged += MsgText_TextChanged;
            MsgTextCJK.TextChanged += MsgTextCJK_TextChanged;
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
                BigMessageBox.Show("Message with that name already exists.");
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
                BigMessageBox.Show("Message with that name already exists.");
                return;
            }

            if (Title.IsNumeric())
            {
                BigMessageBox.Show("Message name cannot be just a number.");
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
                BigMessageBox.Show("Name cannot be empty.");
                return false;
            }

            Title = Title.Replace(" ", "_");

            if (Title.IsNumeric())
            {
                BigMessageBox.Show("Name cannot be just a number.");
                return false;
            }

            foreach (string s in Lists.AllKeywords)
            {
                if (s.ToUpper() == Title.ToUpper() || (Title.ToUpper().StartsWith(s.ToUpper()) && Title.Contains(".")))
                {
                    BigMessageBox.Show("Name cannot be a script keyword.");
                    return false;
                }
            }

            return true;
        }

        private void ChkBox_UseSpaceFont_CheckedChanged(object sender, EventArgs e)
        {
            EditedFile.SpaceFromFont = (sender as CheckBox).Checked;
            RequestPreviewUpdate();
        }
        private void ChkBox_ShowDefaultLanguagePreview_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.OrigPreview = chkBox_ShowDefaultLanguagePreview.Checked;
            RequestPreviewUpdate();
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
            try
            {
                messageSearchTimer.Stop();
                MessagesGrid.CurrentCell = MessagesGrid.Rows[ScrollToMsg].Cells[0];
                MessagesGrid.FirstDisplayedScrollingRowIndex = ScrollToMsg;
                btn_FindMsg.Enabled = true;
            }
            catch
            { }
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

        System.Windows.Forms.Timer autoSaveTimer;
        DateTime LastWriteTime;
        DateTime LastWriteTime2;

        private void WatchFile(NPCEntry EditedEntry)
        {
            if (Program.Settings.AutoSave)
            {
                WatchedEntry = EditedEntry;

                string fPath = Path.Combine(CCode.TempFolderPath, $"{CCode.CodeFileNameBase}.c");
                LastWriteTime = GetLastWriteTimeForFile(fPath);

                autoSaveTimer = new System.Windows.Forms.Timer();
                autoSaveTimer.Interval = (int)Program.Settings.AutoSaveTime;
                autoSaveTimer.Tick += AutoSaveTimer_Tick;
                autoSaveTimer.Start();
            }
            else
            {
                WatchedEntry = EditedEntry;
            }

            TaskEx.Run(() =>
            {
                //Process.HasExited doesn't work under mono...
                Program.CodeEditorProcess.WaitForExit();

                try
                {
                    Watcher_Changed(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""));
                }
                catch
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

            var Dt = GetLastWriteTimeForFile(CCode.EditCodeFilePath);
            var Dt2 = GetLastWriteTimeForFile(CCode.EditHeaderFilePath);

            if (Dt != LastWriteTime || Dt2 != LastWriteTime2)
                Watcher_Changed(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, "", ""));

            LastWriteTime = Dt;
            LastWriteTime2 = Dt2;
            autoSaveTimer.Start();
        }

        private void CompileCode()
        {

            string CompileMsgs = "";
            CCode.Compile(EditedFile.CHeader, Program.Settings.LinkerPaths, SelectedEntry.EmbeddedOverlayCode, ref CompileMsgs, out _);

            this.TextBox_CompileMsg.Invoke((MethodInvoker)delegate
            {
                TextBox_CompileMsg.Text = Helpers.StripTerminalControlCodes(CompileMsgs);
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
                        c.DisplayMember = "Symbol";
                        c.ValueMember = "Addr";
                        c.DataSource = SelectedEntry.EmbeddedOverlayCode.Functions;
                        c.SelectedIndex = -1;
                        c.BindingContext = new BindingContext();

                        CSymbol Function = SelectedEntry.EmbeddedOverlayCode.Functions.FirstOrDefault(x => x.Symbol == CurrentSelection);

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
                        if (File.Exists(CCode.EditCodeFilePath))
                            fs = File.Open(CCode.EditCodeFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        if (File.Exists(CCode.EditHeaderFilePath))
                            fs2 = File.Open(CCode.EditHeaderFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    }
                    catch (Exception)
                    {
                        if (File.Exists(CCode.EditCodeFilePath))
                            fs = File.Open(Path.Combine(CCode.TempFolderPath, $"{CCode.CodeFileNameBase}.c"), FileMode.Open, FileAccess.Read, FileShare.Read);


                        if (File.Exists(CCode.EditCodeFilePath))
                            fs2 = File.Open(Path.Combine(CCode.TempFolderPath, CCode.HeaderFileName), FileMode.Open, FileAccess.Read, FileShare.Read);
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
                //BigMessageBox.Show("An error has occurred while attempting to update the embedded overlay: " + ex.Message);
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
                                                                Textbox_CodeEditorArgs.Text.Replace("$CODEFILE", CCode.EditCodeFilePath.AppendQuotation()).Replace("$CODEFOLDER", CCode.TempFolderPath.AppendQuotation()).Replace("$CODEHEADER", CCode.EditHeaderFilePath.AppendQuotation()),
                                                                false
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
            NativeOpenFileDialog oF = new NativeOpenFileDialog();
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
            if (BigMessageBox.Show("Are you sure? This operation wipes the code completely and cannot be reversed.", "Code Removal", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                SelectedEntry.EmbeddedOverlayCode.Code = "";
                SelectedEntry.EmbeddedOverlayCode.Functions = new List<CSymbol>();
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

        private void ChkBox_UseCJK_CheckedChanged(object sender, EventArgs e)
        {
            MsgText.TextChanged -= MsgText_TextChanged;
            MsgTextCJK.TextChanged -= MsgTextCJK_TextChanged;


            if (ChkBox_UseCJK.Checked)
            {
                MsgText.Visible = false;
                MsgText.Enabled = false;
                MsgTextCJK.Visible = true;
                MsgTextCJK.Enabled = true;

                MsgTextCJK.Text = MsgText.Text;
            }
            else
            {
                MsgText.Visible = true;
                MsgText.Enabled = true;
                MsgTextCJK.Visible = false;
                MsgTextCJK.Enabled = false;

                MsgText.Text = MsgTextCJK.Text;
            }

            MsgText.TextChanged += MsgText_TextChanged;
            MsgTextCJK.TextChanged += MsgTextCJK_TextChanged;

            Program.Settings.UseCJK = ChkBox_UseCJK.Checked;
            SplitMsgContainer_Paint(null, null);


        }

        private void ExportCurrentActorMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedEntry == null)
                return;

            List<byte> msgTable = new List<byte>();
            List<byte> msgData = new List<byte>();

            UInt16 id = 0;
            int locOffset = 65532 / (EditedFile.Languages.Count + 1);

            try
            {
                foreach (MessageEntry msg in SelectedEntry.Messages)
                {
                    if (id >= locOffset)
                        throw new Exception("Too many messages.");

                    var bytes = msg.ToBytes(Lists.DefaultLanguage);
                    Helpers.Ensure4ByteAlign(bytes);
                    msgData.AddRange(bytes);

                    msgTable.AddRange(msg.MakeHeaderEntry(id, msgData.Count - bytes.Count));

                    int locId = 1;

                    foreach (string Localization in EditedFile.Languages)
                    {
                        LocalizationEntry loc = SelectedEntry.Localization.Find(x => x.Language == Localization);

                        MessageEntry msgLoc = null;

                        if (loc != null)
                            msgLoc = loc.Messages.Find(x => x.Name == msg.Name);

                        if (msgLoc == null)
                        {
                            msgLoc = new MessageEntry();
                            msgLoc.MessageText = $"NO MESSAGE ({loc.Language})";
                        }

                        var bytesLoc = msgLoc.ToBytes(loc.Language);
                        Helpers.Ensure4ByteAlign(bytesLoc);
                        msgData.AddRange(bytesLoc);

                        UInt16 localizedId = (UInt16)((locId * locOffset) + id);
                        msgTable.AddRange(msgLoc.MakeHeaderEntry(localizedId, msgData.Count - bytesLoc.Count));

                        locId++;
                    }

                    id++;

                    // Skip message ID 11A, since it's used by NPC Maker...
                    if (id == 0x11A)
                        id++;
                }

                // Add dummy NPC Maker message entry if it doesn't already exist
                MessageEntry msgDummy = new MessageEntry();
                msgDummy.MessageText = "011a NPC MAKER DUMMY MSG";
                var bytesDummy = msgDummy.ToBytes(Lists.DefaultLanguage);
                Helpers.Ensure4ByteAlign(bytesDummy);

                msgData.AddRange(bytesDummy);
                msgTable.AddRange(msgDummy.MakeHeaderEntry(0x11A, msgData.Count - bytesDummy.Count));

                msgDummy.MessageText = "End!";
                bytesDummy = msgDummy.ToBytes(Lists.DefaultLanguage);
                Helpers.Ensure4ByteAlign(bytesDummy);

                msgData.AddRange(bytesDummy);
                msgTable.AddRange(msgDummy.MakeHeaderEntry(UInt16.MaxValue - 2, msgData.Count - bytesDummy.Count));
                msgTable.AddRange(msgDummy.MakeHeaderEntry(UInt16.MaxValue, 0));
            }
            catch (Exception ex)
            {
                BigMessageBox.Show("Error converting messages: " + ex.Message);
            }

            FolderBrowserDialog FBD = new FolderBrowserDialog();
            DialogResult DR = FBD.ShowDialog();

            Helpers.Ensure16ByteAlign(msgTable);
            Helpers.Ensure16ByteAlign(msgData);

            if (DR == DialogResult.OK)
            {
                try
                {
                    string Foldername = FBD.SelectedPath;
                    string TableDataFn = Path.Combine(Foldername, "MessageTable.tbl");
                    string StringDataFn = Path.Combine(Foldername, "StringData.bin");

                    File.WriteAllBytes(TableDataFn, msgTable.ToArray());
                    File.WriteAllBytes(StringDataFn, msgData.ToArray());
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show("Error saving data: " + ex.Message);
                }
            }

        }

        private void clearThisListToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Program.Settings.LastSavePaths = new List<string>();
            UpdateSaveBinaryLastPathsList();
        }
    }
}