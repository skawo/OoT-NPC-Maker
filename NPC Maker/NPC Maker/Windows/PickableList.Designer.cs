namespace NPC_Maker
{
    partial class PickableList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickableList));
            this.Btn_Search = new System.Windows.Forms.TextBox();
            this.Btn_OK = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IDHEX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Btn_EditEntry = new System.Windows.Forms.Button();
            this.Btn_DelEntry = new System.Windows.Forms.Button();
            this.Btn_AddEntry = new System.Windows.Forms.Button();
            this.Label_Desc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Btn_Search
            // 
            this.Btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Search.Location = new System.Drawing.Point(62, 542);
            this.Btn_Search.Name = "Btn_Search";
            this.Btn_Search.Size = new System.Drawing.Size(609, 20);
            this.Btn_Search.TabIndex = 1;
            this.Btn_Search.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            this.Btn_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EnterPress);
            // 
            // Btn_OK
            // 
            this.Btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_OK.Location = new System.Drawing.Point(677, 540);
            this.Btn_OK.Name = "Btn_OK";
            this.Btn_OK.Size = new System.Drawing.Size(75, 23);
            this.Btn_OK.TabIndex = 2;
            this.Btn_OK.Text = "Select";
            this.Btn_OK.UseVisualStyleBackColor = true;
            this.Btn_OK.Click += new System.EventHandler(this.OKClick);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.IDHEX,
            this.NameCol,
            this.Description});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(740, 481);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EnterPress);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListDoubleClick);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            // 
            // IDHEX
            // 
            this.IDHEX.Text = "ID (Hex)";
            // 
            // NameCol
            // 
            this.NameCol.Text = "Internal name";
            this.NameCol.Width = 300;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 300;
            // 
            // Btn_EditEntry
            // 
            this.Btn_EditEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_EditEntry.Location = new System.Drawing.Point(617, 499);
            this.Btn_EditEntry.Name = "Btn_EditEntry";
            this.Btn_EditEntry.Size = new System.Drawing.Size(135, 23);
            this.Btn_EditEntry.TabIndex = 4;
            this.Btn_EditEntry.Text = "Edit entry";
            this.Btn_EditEntry.UseVisualStyleBackColor = true;
            this.Btn_EditEntry.Click += new System.EventHandler(this.Btn_EditEntry_Click);
            // 
            // Btn_DelEntry
            // 
            this.Btn_DelEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Btn_DelEntry.Location = new System.Drawing.Point(12, 499);
            this.Btn_DelEntry.Name = "Btn_DelEntry";
            this.Btn_DelEntry.Size = new System.Drawing.Size(135, 23);
            this.Btn_DelEntry.TabIndex = 5;
            this.Btn_DelEntry.Text = "Delete entry";
            this.Btn_DelEntry.UseVisualStyleBackColor = true;
            this.Btn_DelEntry.Click += new System.EventHandler(this.Btn_DelEntry_Click);
            // 
            // Btn_AddEntry
            // 
            this.Btn_AddEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_AddEntry.Location = new System.Drawing.Point(476, 499);
            this.Btn_AddEntry.Name = "Btn_AddEntry";
            this.Btn_AddEntry.Size = new System.Drawing.Size(135, 23);
            this.Btn_AddEntry.TabIndex = 6;
            this.Btn_AddEntry.Text = "Add entry";
            this.Btn_AddEntry.UseVisualStyleBackColor = true;
            this.Btn_AddEntry.Click += new System.EventHandler(this.Btn_AddEntry_Click);
            // 
            // Label_Desc
            // 
            this.Label_Desc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Label_Desc.AutoSize = true;
            this.Label_Desc.Location = new System.Drawing.Point(12, 545);
            this.Label_Desc.Name = "Label_Desc";
            this.Label_Desc.Size = new System.Drawing.Size(44, 13);
            this.Label_Desc.TabIndex = 7;
            this.Label_Desc.Text = "Search:";
            // 
            // PickableList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(764, 575);
            this.Controls.Add(this.Label_Desc);
            this.Controls.Add(this.Btn_AddEntry);
            this.Controls.Add(this.Btn_DelEntry);
            this.Controls.Add(this.Btn_EditEntry);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.Btn_OK);
            this.Controls.Add(this.Btn_Search);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PickableList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PickableList_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Btn_Search;
        private System.Windows.Forms.Button Btn_OK;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader NameCol;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader IDHEX;
        private System.Windows.Forms.Button Btn_EditEntry;
        private System.Windows.Forms.Button Btn_DelEntry;
        private System.Windows.Forms.Button Btn_AddEntry;
        private System.Windows.Forms.Label Label_Desc;
    }
}