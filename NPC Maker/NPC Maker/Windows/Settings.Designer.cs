
namespace NPC_Maker.Windows
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Cb_ImproveTextReadability = new System.Windows.Forms.CheckBox();
            this.Cb_ColorizeScripts = new System.Windows.Forms.CheckBox();
            this.Cb_CheckSyntax = new System.Windows.Forms.CheckBox();
            this.Cb_Verbose = new System.Windows.Forms.CheckBox();
            this.Combo_CompileFor = new System.Windows.Forms.ComboBox();
            this.Lbl_CompileFor = new System.Windows.Forms.Label();
            this.Txt_GCCArgs = new System.Windows.Forms.TextBox();
            this.Lbl_GCCArgs = new System.Windows.Forms.Label();
            this.BtnSave = new System.Windows.Forms.Button();
            this.Label_CompileTimeout = new System.Windows.Forms.Label();
            this.NumUpCompileTimeout = new System.Windows.Forms.NumericUpDown();
            this.NumUpParseTime = new System.Windows.Forms.NumericUpDown();
            this.AutoSaveC = new System.Windows.Forms.CheckBox();
            this.NumUpDown_AutoSaveCTime = new System.Windows.Forms.NumericUpDown();
            this.Cb_AutoCompile = new System.Windows.Forms.CheckBox();
            this.Btn_ResetCache = new System.Windows.Forms.Button();
            this.checkBox_CompileInParallel = new System.Windows.Forms.CheckBox();
            this.chkBox_Spellcheck = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpCompileTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpParseTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_AutoSaveCTime)).BeginInit();
            this.SuspendLayout();
            // 
            // Cb_ImproveTextReadability
            // 
            this.Cb_ImproveTextReadability.AutoSize = true;
            this.Cb_ImproveTextReadability.Location = new System.Drawing.Point(13, 13);
            this.Cb_ImproveTextReadability.Name = "Cb_ImproveTextReadability";
            this.Cb_ImproveTextReadability.Size = new System.Drawing.Size(199, 17);
            this.Cb_ImproveTextReadability.TabIndex = 0;
            this.Cb_ImproveTextReadability.Tag = "IMPROVEMSGPRV";
            this.Cb_ImproveTextReadability.Text = "Improve message preview readability";
            this.Cb_ImproveTextReadability.UseVisualStyleBackColor = true;
            this.Cb_ImproveTextReadability.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // Cb_ColorizeScripts
            // 
            this.Cb_ColorizeScripts.AutoSize = true;
            this.Cb_ColorizeScripts.Location = new System.Drawing.Point(13, 36);
            this.Cb_ColorizeScripts.Name = "Cb_ColorizeScripts";
            this.Cb_ColorizeScripts.Size = new System.Drawing.Size(124, 17);
            this.Cb_ColorizeScripts.TabIndex = 1;
            this.Cb_ColorizeScripts.Tag = "COLORIZESYNTAX";
            this.Cb_ColorizeScripts.Text = "Colorize script syntax";
            this.Cb_ColorizeScripts.UseVisualStyleBackColor = true;
            this.Cb_ColorizeScripts.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // Cb_CheckSyntax
            // 
            this.Cb_CheckSyntax.AutoSize = true;
            this.Cb_CheckSyntax.Location = new System.Drawing.Point(13, 59);
            this.Cb_CheckSyntax.Name = "Cb_CheckSyntax";
            this.Cb_CheckSyntax.Size = new System.Drawing.Size(169, 17);
            this.Cb_CheckSyntax.TabIndex = 2;
            this.Cb_CheckSyntax.Tag = "CHECKSYNTAX";
            this.Cb_CheckSyntax.Text = "Check script syntax every (ms)";
            this.Cb_CheckSyntax.UseVisualStyleBackColor = true;
            this.Cb_CheckSyntax.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // Cb_Verbose
            // 
            this.Cb_Verbose.AutoSize = true;
            this.Cb_Verbose.Location = new System.Drawing.Point(13, 82);
            this.Cb_Verbose.Name = "Cb_Verbose";
            this.Cb_Verbose.Size = new System.Drawing.Size(181, 17);
            this.Cb_Verbose.TabIndex = 3;
            this.Cb_Verbose.Tag = "VERBOSECODE";
            this.Cb_Verbose.Text = "Verbose code compilation output";
            this.Cb_Verbose.UseVisualStyleBackColor = true;
            this.Cb_Verbose.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // Combo_CompileFor
            // 
            this.Combo_CompileFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_CompileFor.FormattingEnabled = true;
            this.Combo_CompileFor.Location = new System.Drawing.Point(122, 204);
            this.Combo_CompileFor.Name = "Combo_CompileFor";
            this.Combo_CompileFor.Size = new System.Drawing.Size(149, 21);
            this.Combo_CompileFor.TabIndex = 4;
            this.Combo_CompileFor.Tag = "GAMEVERSION";
            this.Combo_CompileFor.SelectedIndexChanged += new System.EventHandler(this.ComboSettingChanged);
            // 
            // Lbl_CompileFor
            // 
            this.Lbl_CompileFor.AutoSize = true;
            this.Lbl_CompileFor.Location = new System.Drawing.Point(11, 207);
            this.Lbl_CompileFor.Name = "Lbl_CompileFor";
            this.Lbl_CompileFor.Size = new System.Drawing.Size(75, 13);
            this.Lbl_CompileFor.TabIndex = 5;
            this.Lbl_CompileFor.Text = "Game version:";
            // 
            // Txt_GCCArgs
            // 
            this.Txt_GCCArgs.Location = new System.Drawing.Point(13, 278);
            this.Txt_GCCArgs.Name = "Txt_GCCArgs";
            this.Txt_GCCArgs.Size = new System.Drawing.Size(450, 20);
            this.Txt_GCCArgs.TabIndex = 6;
            this.Txt_GCCArgs.Tag = "GCCARGS";
            this.Txt_GCCArgs.TextChanged += new System.EventHandler(this.TextBoxChanged);
            // 
            // Lbl_GCCArgs
            // 
            this.Lbl_GCCArgs.AutoSize = true;
            this.Lbl_GCCArgs.Location = new System.Drawing.Point(12, 262);
            this.Lbl_GCCArgs.Name = "Lbl_GCCArgs";
            this.Lbl_GCCArgs.Size = new System.Drawing.Size(85, 13);
            this.Lbl_GCCArgs.TabIndex = 7;
            this.Lbl_GCCArgs.Text = "GCC Arguments:";
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(343, 304);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(120, 29);
            this.BtnSave.TabIndex = 8;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // Label_CompileTimeout
            // 
            this.Label_CompileTimeout.AutoSize = true;
            this.Label_CompileTimeout.Location = new System.Drawing.Point(11, 233);
            this.Label_CompileTimeout.Name = "Label_CompileTimeout";
            this.Label_CompileTimeout.Size = new System.Drawing.Size(106, 13);
            this.Label_CompileTimeout.TabIndex = 10;
            this.Label_CompileTimeout.Text = "Compile timeout (ms):";
            // 
            // NumUpCompileTimeout
            // 
            this.NumUpCompileTimeout.Location = new System.Drawing.Point(122, 231);
            this.NumUpCompileTimeout.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NumUpCompileTimeout.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUpCompileTimeout.Name = "NumUpCompileTimeout";
            this.NumUpCompileTimeout.Size = new System.Drawing.Size(120, 20);
            this.NumUpCompileTimeout.TabIndex = 11;
            this.NumUpCompileTimeout.Tag = "COMPILETIMEOUT";
            this.NumUpCompileTimeout.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.NumUpCompileTimeout.ValueChanged += new System.EventHandler(this.NumUpSettingChanged);
            // 
            // NumUpParseTime
            // 
            this.NumUpParseTime.Location = new System.Drawing.Point(188, 58);
            this.NumUpParseTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NumUpParseTime.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUpParseTime.Name = "NumUpParseTime";
            this.NumUpParseTime.Size = new System.Drawing.Size(120, 20);
            this.NumUpParseTime.TabIndex = 12;
            this.NumUpParseTime.Tag = "PARSETIME";
            this.NumUpParseTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumUpParseTime.ValueChanged += new System.EventHandler(this.NumUpSettingChanged);
            // 
            // AutoSaveC
            // 
            this.AutoSaveC.AutoSize = true;
            this.AutoSaveC.Location = new System.Drawing.Point(13, 128);
            this.AutoSaveC.Name = "AutoSaveC";
            this.AutoSaveC.Size = new System.Drawing.Size(162, 17);
            this.AutoSaveC.TabIndex = 14;
            this.AutoSaveC.Tag = "AUTOSAVE";
            this.AutoSaveC.Text = "C Code Update Interval (ms):";
            this.AutoSaveC.UseVisualStyleBackColor = true;
            this.AutoSaveC.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // NumUpDown_AutoSaveCTime
            // 
            this.NumUpDown_AutoSaveCTime.Location = new System.Drawing.Point(188, 127);
            this.NumUpDown_AutoSaveCTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NumUpDown_AutoSaveCTime.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUpDown_AutoSaveCTime.Name = "NumUpDown_AutoSaveCTime";
            this.NumUpDown_AutoSaveCTime.Size = new System.Drawing.Size(120, 20);
            this.NumUpDown_AutoSaveCTime.TabIndex = 15;
            this.NumUpDown_AutoSaveCTime.Tag = "AUTOSAVETIME";
            this.NumUpDown_AutoSaveCTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumUpDown_AutoSaveCTime.ValueChanged += new System.EventHandler(this.NumUpSettingChanged);
            // 
            // Cb_AutoCompile
            // 
            this.Cb_AutoCompile.AutoSize = true;
            this.Cb_AutoCompile.Location = new System.Drawing.Point(13, 105);
            this.Cb_AutoCompile.Name = "Cb_AutoCompile";
            this.Cb_AutoCompile.Size = new System.Drawing.Size(194, 17);
            this.Cb_AutoCompile.TabIndex = 16;
            this.Cb_AutoCompile.Tag = "AUTOSAVESWITCH";
            this.Cb_AutoCompile.Text = "Auto Compile Code on Actor Switch";
            this.Cb_AutoCompile.UseVisualStyleBackColor = true;
            this.Cb_AutoCompile.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // Btn_ResetCache
            // 
            this.Btn_ResetCache.Location = new System.Drawing.Point(13, 304);
            this.Btn_ResetCache.Name = "Btn_ResetCache";
            this.Btn_ResetCache.Size = new System.Drawing.Size(120, 29);
            this.Btn_ResetCache.TabIndex = 17;
            this.Btn_ResetCache.Text = "Reset Cache";
            this.Btn_ResetCache.UseVisualStyleBackColor = true;
            this.Btn_ResetCache.Click += new System.EventHandler(this.ResetCache_Click);
            // 
            // checkBox_CompileInParallel
            // 
            this.checkBox_CompileInParallel.AutoSize = true;
            this.checkBox_CompileInParallel.Location = new System.Drawing.Point(13, 151);
            this.checkBox_CompileInParallel.Name = "checkBox_CompileInParallel";
            this.checkBox_CompileInParallel.Size = new System.Drawing.Size(141, 17);
            this.checkBox_CompileInParallel.TabIndex = 18;
            this.checkBox_CompileInParallel.Tag = "PARALLEL";
            this.checkBox_CompileInParallel.Text = "Compile binary in parallel";
            this.checkBox_CompileInParallel.UseVisualStyleBackColor = true;
            this.checkBox_CompileInParallel.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // chkBox_Spellcheck
            // 
            this.chkBox_Spellcheck.AutoSize = true;
            this.chkBox_Spellcheck.Location = new System.Drawing.Point(13, 174);
            this.chkBox_Spellcheck.Name = "chkBox_Spellcheck";
            this.chkBox_Spellcheck.Size = new System.Drawing.Size(79, 17);
            this.chkBox_Spellcheck.TabIndex = 19;
            this.chkBox_Spellcheck.Tag = "SPELLCHECK";
            this.chkBox_Spellcheck.Text = "Spellcheck";
            this.chkBox_Spellcheck.UseVisualStyleBackColor = true;
            this.chkBox_Spellcheck.CheckedChanged += new System.EventHandler(this.Cb_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 341);
            this.Controls.Add(this.chkBox_Spellcheck);
            this.Controls.Add(this.checkBox_CompileInParallel);
            this.Controls.Add(this.Btn_ResetCache);
            this.Controls.Add(this.Cb_AutoCompile);
            this.Controls.Add(this.NumUpDown_AutoSaveCTime);
            this.Controls.Add(this.AutoSaveC);
            this.Controls.Add(this.NumUpParseTime);
            this.Controls.Add(this.NumUpCompileTimeout);
            this.Controls.Add(this.Label_CompileTimeout);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.Lbl_GCCArgs);
            this.Controls.Add(this.Txt_GCCArgs);
            this.Controls.Add(this.Lbl_CompileFor);
            this.Controls.Add(this.Combo_CompileFor);
            this.Controls.Add(this.Cb_Verbose);
            this.Controls.Add(this.Cb_CheckSyntax);
            this.Controls.Add(this.Cb_ColorizeScripts);
            this.Controls.Add(this.Cb_ImproveTextReadability);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.NumUpCompileTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpParseTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_AutoSaveCTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox Cb_ImproveTextReadability;
        private System.Windows.Forms.CheckBox Cb_ColorizeScripts;
        private System.Windows.Forms.CheckBox Cb_CheckSyntax;
        private System.Windows.Forms.CheckBox Cb_Verbose;
        private System.Windows.Forms.ComboBox Combo_CompileFor;
        private System.Windows.Forms.Label Lbl_CompileFor;
        private System.Windows.Forms.TextBox Txt_GCCArgs;
        private System.Windows.Forms.Label Lbl_GCCArgs;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label Label_CompileTimeout;
        private System.Windows.Forms.NumericUpDown NumUpCompileTimeout;
        private System.Windows.Forms.NumericUpDown NumUpParseTime;
        private System.Windows.Forms.CheckBox AutoSaveC;
        private System.Windows.Forms.NumericUpDown NumUpDown_AutoSaveCTime;
        private System.Windows.Forms.CheckBox Cb_AutoCompile;
        private System.Windows.Forms.Button Btn_ResetCache;
        private System.Windows.Forms.CheckBox checkBox_CompileInParallel;
        private System.Windows.Forms.CheckBox chkBox_Spellcheck;
    }
}