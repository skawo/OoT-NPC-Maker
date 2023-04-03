
namespace NPC_Maker
{
    partial class ScriptEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptEditor));
            this.Textbox_Script = new FastColoredTextBoxNS.FastColoredTextBox();
            this.Textbox_ParseErrors = new System.Windows.Forms.TextBox();
            this.Button_TryParse = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.Textbox_Script)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Textbox_Script
            // 
            this.Textbox_Script.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.Textbox_Script.AutoIndent = false;
            this.Textbox_Script.AutoIndentChars = false;
            this.Textbox_Script.AutoScrollMinSize = new System.Drawing.Size(27, 15);
            this.Textbox_Script.BackBrush = null;
            this.Textbox_Script.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Textbox_Script.CharHeight = 15;
            this.Textbox_Script.CharWidth = 8;
            this.Textbox_Script.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Textbox_Script.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Textbox_Script.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Textbox_Script.Font = new System.Drawing.Font("Cascadia Code", 9.75F);
            this.Textbox_Script.IsReplaceMode = false;
            this.Textbox_Script.Location = new System.Drawing.Point(0, 0);
            this.Textbox_Script.Name = "Textbox_Script";
            this.Textbox_Script.Paddings = new System.Windows.Forms.Padding(0);
            this.Textbox_Script.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Textbox_Script.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("Textbox_Script.ServiceColors")));
            this.Textbox_Script.Size = new System.Drawing.Size(802, 492);
            this.Textbox_Script.TabIndex = 4;
            this.Textbox_Script.Tag = "";
            this.Textbox_Script.WordWrapAutoIndent = false;
            this.Textbox_Script.Zoom = 100;
            this.Textbox_Script.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.Textbox_Script_TextChanged);
            this.Textbox_Script.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Textbox_Script_MouseClick);
            // 
            // Textbox_ParseErrors
            // 
            this.Textbox_ParseErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Textbox_ParseErrors.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Textbox_ParseErrors.Location = new System.Drawing.Point(0, 0);
            this.Textbox_ParseErrors.Multiline = true;
            this.Textbox_ParseErrors.Name = "Textbox_ParseErrors";
            this.Textbox_ParseErrors.ReadOnly = true;
            this.Textbox_ParseErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Textbox_ParseErrors.Size = new System.Drawing.Size(716, 85);
            this.Textbox_ParseErrors.TabIndex = 5;
            // 
            // Button_TryParse
            // 
            this.Button_TryParse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_TryParse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Button_TryParse.Location = new System.Drawing.Point(722, 0);
            this.Button_TryParse.Name = "Button_TryParse";
            this.Button_TryParse.Size = new System.Drawing.Size(80, 85);
            this.Button_TryParse.TabIndex = 6;
            this.Button_TryParse.Text = "Try parsing";
            this.Button_TryParse.UseVisualStyleBackColor = true;
            this.Button_TryParse.Click += new System.EventHandler(this.Button_TryParse_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Textbox_Script);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Textbox_ParseErrors);
            this.splitContainer1.Panel2.Controls.Add(this.Button_TryParse);
            this.splitContainer1.Size = new System.Drawing.Size(802, 584);
            this.splitContainer1.SplitterDistance = 492;
            this.splitContainer1.TabIndex = 7;
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScriptEditor";
            this.Size = new System.Drawing.Size(802, 584);
            ((System.ComponentModel.ISupportInitialize)(this.Textbox_Script)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox Textbox_Script;
        private System.Windows.Forms.TextBox Textbox_ParseErrors;
        private System.Windows.Forms.Button Button_TryParse;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
