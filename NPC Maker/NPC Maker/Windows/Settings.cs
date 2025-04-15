using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NPC_Maker.Windows
{
    public partial class Settings : Form
    {
        public NPCMakerSettings EditedSettings;

        public Settings()
        {
            InitializeComponent();

            string JSON = JsonConvert.SerializeObject(Program.Settings);
            EditedSettings = JsonConvert.DeserializeObject<NPCMakerSettings>(JSON);

            foreach (var v in Enum.GetNames(typeof(Lists.GameVersions)))
                Combo_CompileFor.Items.Add(v.ToString());

            Cb_CheckSyntax.Checked = EditedSettings.CheckSyntax;
            Cb_ColorizeScripts.Checked = EditedSettings.ColorizeScriptSyntax;
            Cb_ImproveTextReadability.Checked = EditedSettings.ImproveTextMsgReadability;
            Cb_Verbose.Checked = EditedSettings.Verbose;
            Txt_GCCArgs.Text = EditedSettings.GCCFlags;
            Combo_CompileFor.Text = EditedSettings.GameVersion.ToString();
            Cb_AutoCompile.Checked = EditedSettings.AutoComp_ActorSwitch;
            NumUpCompileTimeout.Value = EditedSettings.CompileTimeout;
            NumUpParseTime.Value = EditedSettings.ParseTime;
            AutoSaveC.Checked = EditedSettings.AutoSave;
            NumUpDown_AutoSaveCTime.Value = EditedSettings.AutoSaveTime;
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Program.Settings = EditedSettings;

            if (Program.Settings.UseWine)
                Program.IsRunningUnderMono = false;

            this.Close();
        }

        private void ResetCache_Click(object sender, EventArgs e)
        {
            if (Program.CompileInProgress)
                return;

            if (Directory.Exists(Program.CachePath))
                Directory.Delete(Program.CachePath, true);

            if (Directory.Exists(Program.CCachePath))
                Directory.Delete(Program.CCachePath, true);

            if (!Directory.Exists(Program.CachePath))
                Directory.CreateDirectory(Program.CachePath);

            if (!Directory.Exists(Program.CCachePath))
                Directory.CreateDirectory(Program.CCachePath);
        }
    }
}
