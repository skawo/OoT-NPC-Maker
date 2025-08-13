
namespace NPC_Maker.Windows
{
    partial class GlobalHeader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalHeader));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.ToolMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddNew = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Rename = new System.Windows.Forms.ToolStripMenuItem();
            this.Tab = new System.Windows.Forms.TabControl();
            this.Btn_HeaderBrowse = new System.Windows.Forms.Button();
            this.Tx_HeaderPath = new System.Windows.Forms.TextBox();
            this.Label_Header = new System.Windows.Forms.Label();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.MaximumSize = new System.Drawing.Size(2000, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(834, 24);
            this.MenuStrip.TabIndex = 2;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // ToolMenu
            // 
            this.ToolMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddNew,
            this.Delete,
            this.Rename});
            this.ToolMenu.Name = "ToolMenu";
            this.ToolMenu.Size = new System.Drawing.Size(59, 20);
            this.ToolMenu.Text = "Actions";
            // 
            // AddNew
            // 
            this.AddNew.Name = "AddNew";
            this.AddNew.Size = new System.Drawing.Size(121, 22);
            this.AddNew.Text = "Add new";
            this.AddNew.Click += new System.EventHandler(this.AddNew_Click);
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(121, 22);
            this.Delete.Text = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Rename
            // 
            this.Rename.Name = "Rename";
            this.Rename.Size = new System.Drawing.Size(121, 22);
            this.Rename.Text = "Rename";
            this.Rename.Click += new System.EventHandler(this.Rename_Click);
            // 
            // Tab
            // 
            this.Tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab.Location = new System.Drawing.Point(0, 27);
            this.Tab.Name = "Tab";
            this.Tab.SelectedIndex = 0;
            this.Tab.Size = new System.Drawing.Size(834, 464);
            this.Tab.TabIndex = 3;
            this.Tab.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Tab_MouseUp);
            // 
            // Btn_HeaderBrowse
            // 
            this.Btn_HeaderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_HeaderBrowse.Location = new System.Drawing.Point(769, 497);
            this.Btn_HeaderBrowse.Name = "Btn_HeaderBrowse";
            this.Btn_HeaderBrowse.Size = new System.Drawing.Size(65, 20);
            this.Btn_HeaderBrowse.TabIndex = 84;
            this.Btn_HeaderBrowse.Text = "Browse";
            this.Btn_HeaderBrowse.UseVisualStyleBackColor = true;
            this.Btn_HeaderBrowse.Click += new System.EventHandler(this.Btn_HeaderBrowse_Click);
            // 
            // Tx_HeaderPath
            // 
            this.Tx_HeaderPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Tx_HeaderPath.Location = new System.Drawing.Point(330, 497);
            this.Tx_HeaderPath.MaxLength = 99999;
            this.Tx_HeaderPath.Name = "Tx_HeaderPath";
            this.Tx_HeaderPath.Size = new System.Drawing.Size(433, 20);
            this.Tx_HeaderPath.TabIndex = 83;
            this.Tx_HeaderPath.Tag = "HEADERPATH";
            this.Tx_HeaderPath.TextChanged += new System.EventHandler(this.Tx_HeaderPath_TextChanged);
            // 
            // Label_Header
            // 
            this.Label_Header.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_Header.AutoSize = true;
            this.Label_Header.Location = new System.Drawing.Point(240, 501);
            this.Label_Header.Name = "Label_Header";
            this.Label_Header.Size = new System.Drawing.Size(84, 13);
            this.Label_Header.TabIndex = 85;
            this.Label_Header.Text = "External header:";
            // 
            // GlobalHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 526);
            this.Controls.Add(this.Label_Header);
            this.Controls.Add(this.Btn_HeaderBrowse);
            this.Controls.Add(this.Tx_HeaderPath);
            this.Controls.Add(this.Tab);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GlobalHeader";
            this.Text = "Global headers";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu;
        private System.Windows.Forms.ToolStripMenuItem AddNew;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripMenuItem Rename;
        private System.Windows.Forms.TabControl Tab;
        private System.Windows.Forms.Button Btn_HeaderBrowse;
        private System.Windows.Forms.TextBox Tx_HeaderPath;
        private System.Windows.Forms.Label Label_Header;
    }
}