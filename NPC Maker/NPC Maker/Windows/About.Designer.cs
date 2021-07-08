namespace NPC_Maker
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.CreditsHeader = new System.Windows.Forms.Label();
            this.Credits = new System.Windows.Forms.Label();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.LblVersionX = new System.Windows.Forms.Label();
            this.LblVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // CreditsHeader
            // 
            this.CreditsHeader.AutoSize = true;
            this.CreditsHeader.BackColor = System.Drawing.Color.White;
            this.CreditsHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditsHeader.Location = new System.Drawing.Point(12, 186);
            this.CreditsHeader.Name = "CreditsHeader";
            this.CreditsHeader.Size = new System.Drawing.Size(50, 13);
            this.CreditsHeader.TabIndex = 4;
            this.CreditsHeader.Text = "Credits:";
            // 
            // Credits
            // 
            this.Credits.AutoSize = true;
            this.Credits.BackColor = System.Drawing.Color.White;
            this.Credits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Credits.Location = new System.Drawing.Point(12, 199);
            this.Credits.Name = "Credits";
            this.Credits.Size = new System.Drawing.Size(377, 52);
            this.Credits.TabIndex = 5;
            this.Credits.Text = "Programming: Skawo.\r\nProgramming help: Fig, rankaisija, z64me, logyk, Nokaubure, " +
    "RoadrunnerWMC\r\nSpecial thanks to: OoT decompilation team, z64ovl team\r\nManual: S" +
    "kawo, RoadrunnerWMC";
            // 
            // Logo
            // 
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(33, 12);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(421, 162);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Logo.TabIndex = 7;
            this.Logo.TabStop = false;
            // 
            // LblVersionX
            // 
            this.LblVersionX.AutoSize = true;
            this.LblVersionX.BackColor = System.Drawing.Color.White;
            this.LblVersionX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVersionX.Location = new System.Drawing.Point(342, 161);
            this.LblVersionX.Name = "LblVersionX";
            this.LblVersionX.Size = new System.Drawing.Size(57, 13);
            this.LblVersionX.TabIndex = 8;
            this.LblVersionX.Text = "Version: ";
            // 
            // LblVersion
            // 
            this.LblVersion.AutoSize = true;
            this.LblVersion.BackColor = System.Drawing.Color.White;
            this.LblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVersion.Location = new System.Drawing.Point(415, 161);
            this.LblVersion.Name = "LblVersion";
            this.LblVersion.Size = new System.Drawing.Size(39, 13);
            this.LblVersion.TabIndex = 9;
            this.LblVersion.Text = "X.X.X";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(487, 262);
            this.Controls.Add(this.LblVersion);
            this.Controls.Add(this.LblVersionX);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.Credits);
            this.Controls.Add(this.CreditsHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label CreditsHeader;
        private System.Windows.Forms.Label Credits;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.Label LblVersionX;
        private System.Windows.Forms.Label LblVersion;
    }
}