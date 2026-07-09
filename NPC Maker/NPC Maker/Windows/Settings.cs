using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPC_Maker.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static ZeldaMessage.Data;

namespace NPC_Maker.Windows
{
    public partial class Settings : Form
    {
        public NPCMakerSettings EditedSettings;
        public NPCFile EditedFile;
        private NativeColorDialog ColorDialog = new NativeColorDialog();

        public Settings(ref NPCFile _EditedFile)
        {
            InitializeComponent();
            Helpers.AdjustFormScaleAndColors(this);

            Helpers.MakeNotResizableMonoSafe(this);

            string JSON = JsonConvert.SerializeObject(Program.Settings);
            EditedSettings = JsonConvert.DeserializeObject<NPCMakerSettings>(JSON);

            Combo_CompileFor.Items.AddRange(Enum.GetNames(typeof(Lists.GameVersions)));
            Combo_Library.Items.AddRange(Enum.GetNames(typeof(Lists.Library)));
            Combo_Linker.Items.AddRange(Enum.GetNames(typeof(Lists.Linker)));

            Cb_CheckSyntax.Checked = EditedSettings.CheckSyntax;
            Cb_ColorizeScripts.Checked = EditedSettings.ColorizeScriptSyntax;
            Cb_ImproveTextReadability.Checked = EditedSettings.ImproveTextMsgReadability;
            Cb_Verbose.Checked = EditedSettings.Verbose;
            Txt_GCCArgs.Text = EditedSettings.GCCFlags;
            Cb_AutoCompile.Checked = EditedSettings.AutoComp_ActorSwitch;
            NumUpCompileTimeout.Value = EditedSettings.CompileTimeout;
            NumUpParseTime.Value = EditedSettings.ParseTime;
            AutoSaveC.Checked = EditedSettings.AutoSave;
            NumUpDown_AutoSaveCTime.Value = EditedSettings.AutoSaveTime;
            checkBox_CompileInParallel.Checked = EditedSettings.CompileInParallel;
            chkBox_Spellcheck.Checked = EditedSettings.Spellcheck;
            chkBox_Compress.Checked = EditedSettings.CompressIndividually;
            Txt_ProjectPath.Text = EditedSettings.ProjectPath;
            Chk_AllowCommentsOnLoc.Checked = EditedSettings.AllowCommentsOnLoc;
            outputDFile.Checked = EditedSettings.OutputDeps;
            guiScale.Value = (decimal)EditedSettings.GUIScale;

            Combo_Linker.SelectedIndex = (int)EditedSettings.Linker;
            Combo_Library.SelectedIndex = (int)EditedSettings.Library;
            Combo_CompileFor.SelectedIndex = (int)EditedSettings.GameVersion;

            Btn_BgColor.BackColor = EditedSettings.BGColor;
            Btn_TextColor.BackColor = EditedSettings.TextColor;
            Btn_InputColor.BackColor = EditedSettings.InputColor;
            Chk_ChangeGUI.Checked = EditedSettings.ChangeGUIColors;
            Btn_DisabledColor.BackColor = EditedSettings.DisabledColor;

            EditedFile = _EditedFile;
        }

        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            NPCMakerSettings.Members Member = NPCMakerSettings.GetMemberFromTag((sender as CheckBox).Tag, (sender as CheckBox).Name);
            EditedSettings.ChangeValueOfMember(Member, (sender as CheckBox).Checked);
        }

        private void ComboSettingChanged(object sender, EventArgs e)
        {
            NPCMakerSettings.Members Member = NPCMakerSettings.GetMemberFromTag((sender as ComboBox).Tag, (sender as ComboBox).Name);
            EditedSettings.ChangeValueOfMember(Member, (sender as ComboBox).Text);
        }

        private void NumUpSettingChanged(object sender, EventArgs e)
        {
            NPCMakerSettings.Members Member = NPCMakerSettings.GetMemberFromTag((sender as NumericUpDown).Tag, (sender as NumericUpDown).Name);
            EditedSettings.ChangeValueOfMember(Member, (sender as NumericUpDown).Value);
        }

        private void TextBoxChanged(object sender, EventArgs e)
        {
            NPCMakerSettings.Members Member = NPCMakerSettings.GetMemberFromTag((sender as TextBox).Tag, (sender as TextBox).Name);
            EditedSettings.ChangeValueOfMember(Member, (sender as TextBox).Text);
        }

        private bool CheckCommentDisable()
        {
            if (EditedSettings.AllowCommentsOnLoc == false && Program.Settings.AllowCommentsOnLoc && EditedFile != null)
            {
                StringBuilder sb = new StringBuilder();
                int numLines = 0;

                foreach (var Entry in EditedFile.Entries)
                {
                    foreach (var Loc in Entry.Localization)
                    {
                        foreach (var Msg in Loc.Messages)
                        {
                            if (Msg.Comment != null && numLines <= 20)
                            {
                                if (numLines == 20)
                                    sb.Append("...and more.");
                                else
                                    sb.Append($"{Entry.NPCName}: {Msg.Name} ({Loc.Language}){Environment.NewLine}");

                                numLines++;
                            }
                        }
                    }
                }

                string HiddenMsgs = sb.ToString();

                if (numLines != 0)
                {
                    DialogResult Res = BigMessageBox.Show("Are you sure? The following comments will be hidden:" + Environment.NewLine + HiddenMsgs, "Confirmation", MessageBoxButtons.YesNo);

                    if (Res != DialogResult.Yes)
                        return false;
                }
            }

            return true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!CheckCommentDisable())
                return;

            Program.Settings = EditedSettings;

            if (Program.Settings.UseWine)
                Program.IsRunningUnderMono = false;

            this.Close();
        }

        private void ResetCache_Click(object sender, EventArgs e)
        {
            if (Program.CompileInProgress)
                return;

            try
            {
                if (Directory.Exists(Program.ScriptCachePath))
                    Directory.Delete(Program.ScriptCachePath, true);

                if (Directory.Exists(Program.CCachePath))
                    Directory.Delete(Program.CCachePath, true);

                if (!Directory.Exists(Program.ScriptCachePath))
                    Directory.CreateDirectory(Program.ScriptCachePath);

                if (!Directory.Exists(Program.CCachePath))
                    Directory.CreateDirectory(Program.CCachePath);
            }
            catch (Exception ex)
            {
                BigMessageBox.Show("Error resetting cache: " + ex.Message);
            }
        }

        private void Btn_Browse_Click(object sender, EventArgs e)
        {
            NativeFolderBrowserDialog fB = new NativeFolderBrowserDialog();

            if (fB.ShowDialog() == DialogResult.OK)
                Txt_ProjectPath.Text = fB.SelectedPath;
        }

        private void Btn_LinkerFiles_Click(object sender, EventArgs e)
        {
            string filter =
                "Linker files (*.ld;*.zsym)|*.ld;*.zsym|" +
                "LD Files (*.ld)|*.ld|" +
                "ZLINKER Symbols (*.zsym)|*.zsym|" +
                "All files (*.*)|*.*";

            LongInputBox liB = new LongInputBox("Additional linker files", 
                                                "Add linker paths:", 
                                                Program.Settings.LinkerPaths, 
                                                true,
                                                filter);

            if (liB.ShowDialog() == DialogResult.OK)
                EditedSettings.LinkerPaths = liB.inputText;
        }

        private void Btn_IncludePaths_Click(object sender, EventArgs e)
        {
            LongInputBox liB = new LongInputBox("C include directories", "Add include directories:", Program.Settings.IncludePaths, true, "", true, Lists.DefaultIncludePaths[EditedSettings.Library]);

            if (liB.ShowDialog() == DialogResult.OK)
                EditedSettings.IncludePaths = liB.inputText;
        }

        private void PickColor(Button button, Color currentColor, Action<Color> setColor)
        {
            ColorDialog.Color = currentColor;

            if (ColorDialog.ShowDialog() != DialogResult.OK)
                return;

            Color color = Color.FromArgb(255, ColorDialog.Color.R, ColorDialog.Color.G, ColorDialog.Color.B);
            button.BackColor = color;
            setColor(color);
        }

        private void Btn_BgColor_Click(object sender, EventArgs e)
        {
            PickColor(Btn_BgColor, EditedSettings.BGColor, c => EditedSettings.BGColor = c);
        }

        private void Btn_TextColor_Click(object sender, EventArgs e)
        {
            PickColor(Btn_TextColor, EditedSettings.TextColor, c => EditedSettings.TextColor = c);
        }

        private void Btn_InputColor_Click(object sender, EventArgs e)
        {
            PickColor(Btn_InputColor, EditedSettings.InputColor, c => EditedSettings.InputColor = c);
        }

        private void Btn_DisabledColor_Click(object sender, EventArgs e)
        {
            PickColor(Btn_DisabledColor, EditedSettings.DisabledColor, c => EditedSettings.DisabledColor = c);
        }

        private void Btn_SetDefault_Click(object sender, EventArgs e)
        {
            EditedSettings.BGColor = System.Drawing.Color.FromArgb(255, 240, 240, 240);
            EditedSettings.TextColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            EditedSettings.InputColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            EditedSettings.DisabledColor = System.Drawing.Color.FromArgb(255, 210, 210, 210);
            Btn_BgColor.BackColor = EditedSettings.BGColor;
            Btn_TextColor.BackColor = EditedSettings.TextColor;
            Btn_InputColor.BackColor = EditedSettings.InputColor;
            Btn_DisabledColor.BackColor = EditedSettings.DisabledColor;
        }
    }
}
