
namespace NPC_Maker.Windows
{
    partial class ComboPicker
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
            this.LblExplanation = new System.Windows.Forms.Label();
            this.Combo = new System.Windows.Forms.ComboBox();
            this.Btn_OK = new System.Windows.Forms.Button();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblExplanation
            // 
            this.LblExplanation.AutoSize = true;
            this.LblExplanation.Location = new System.Drawing.Point(13, 13);
            this.LblExplanation.Name = "LblExplanation";
            this.LblExplanation.Size = new System.Drawing.Size(35, 13);
            this.LblExplanation.TabIndex = 0;
            this.LblExplanation.Text = "label1";
            // 
            // Combo
            // 
            this.Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo.FormattingEnabled = true;
            this.Combo.Location = new System.Drawing.Point(12, 29);
            this.Combo.Name = "Combo";
            this.Combo.Size = new System.Drawing.Size(323, 21);
            this.Combo.TabIndex = 1;
            // 
            // Btn_OK
            // 
            this.Btn_OK.Location = new System.Drawing.Point(260, 56);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(75, 23);
            this.Btn_OK.TabIndex = 2;
            this.Btn_OK.Text = "OK";
            this.Btn_OK.UseVisualStyleBackColor = true;
            this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(179, 56);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Btn_Cancel.TabIndex = 3;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // ComboPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 86);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_OK);
            this.Controls.Add(this.Combo);
            this.Controls.Add(this.LblExplanation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ComboPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ComboPicker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblExplanation;
        private System.Windows.Forms.ComboBox Combo;
        private System.Windows.Forms.Button Btn_OK;
        private System.Windows.Forms.Button Btn_Cancel;
    }
}