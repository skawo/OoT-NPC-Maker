
namespace NPC_Maker.Windows
{
    partial class YesNoAllBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YesNoAllBox));
            this.LblExplanation = new System.Windows.Forms.Label();
            this.Btn_Y2A = new System.Windows.Forms.Button();
            this.Btn_Y = new System.Windows.Forms.Button();
            this.Btn_N = new System.Windows.Forms.Button();
            this.Btn_N2A = new System.Windows.Forms.Button();
            this.Btn_C = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblExplanation
            // 
            this.LblExplanation.Location = new System.Drawing.Point(13, 13);
            this.LblExplanation.Name = "LblExplanation";
            this.LblExplanation.Size = new System.Drawing.Size(393, 40);
            this.LblExplanation.TabIndex = 0;
            this.LblExplanation.Text = "label1";
            // 
            // Btn_Y2A
            // 
            this.Btn_Y2A.Location = new System.Drawing.Point(93, 56);
            this.Btn_Y2A.Name = "Btn_Y2A";
            this.Btn_Y2A.Size = new System.Drawing.Size(75, 23);
            this.Btn_Y2A.TabIndex = 2;
            this.Btn_Y2A.Text = "Yes to all";
            this.Btn_Y2A.UseVisualStyleBackColor = true;
            this.Btn_Y2A.Click += new System.EventHandler(this.Btn_Y2A_Click);
            // 
            // Btn_Y
            // 
            this.Btn_Y.Location = new System.Drawing.Point(12, 56);
            this.Btn_Y.Name = "Btn_Y";
            this.Btn_Y.Size = new System.Drawing.Size(75, 23);
            this.Btn_Y.TabIndex = 3;
            this.Btn_Y.Text = "Yes";
            this.Btn_Y.UseVisualStyleBackColor = true;
            this.Btn_Y.Click += new System.EventHandler(this.Btn_Y_Click);
            // 
            // Btn_N
            // 
            this.Btn_N.Location = new System.Drawing.Point(174, 56);
            this.Btn_N.Name = "Btn_N";
            this.Btn_N.Size = new System.Drawing.Size(75, 23);
            this.Btn_N.TabIndex = 4;
            this.Btn_N.Text = "No";
            this.Btn_N.UseVisualStyleBackColor = true;
            this.Btn_N.Click += new System.EventHandler(this.Btn_N_Click);
            // 
            // Btn_N2A
            // 
            this.Btn_N2A.Location = new System.Drawing.Point(255, 56);
            this.Btn_N2A.Name = "Btn_N2A";
            this.Btn_N2A.Size = new System.Drawing.Size(75, 23);
            this.Btn_N2A.TabIndex = 5;
            this.Btn_N2A.Text = "No to all";
            this.Btn_N2A.UseVisualStyleBackColor = true;
            this.Btn_N2A.Click += new System.EventHandler(this.Btn_N2A_Click);
            // 
            // Btn_C
            // 
            this.Btn_C.Location = new System.Drawing.Point(336, 56);
            this.Btn_C.Name = "Btn_C";
            this.Btn_C.Size = new System.Drawing.Size(75, 23);
            this.Btn_C.TabIndex = 6;
            this.Btn_C.Text = "Cancel";
            this.Btn_C.UseVisualStyleBackColor = true;
            this.Btn_C.Click += new System.EventHandler(this.Btn_C_Click);
            // 
            // YesNoAllBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 87);
            this.Controls.Add(this.Btn_C);
            this.Controls.Add(this.Btn_N2A);
            this.Controls.Add(this.Btn_N);
            this.Controls.Add(this.Btn_Y);
            this.Controls.Add(this.Btn_Y2A);
            this.Controls.Add(this.LblExplanation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "YesNoAllBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "YesNoAllBox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblExplanation;
        private System.Windows.Forms.Button Btn_Y2A;
        private System.Windows.Forms.Button Btn_Y;
        private System.Windows.Forms.Button Btn_N;
        private System.Windows.Forms.Button Btn_N2A;
        private System.Windows.Forms.Button Btn_C;
    }
}