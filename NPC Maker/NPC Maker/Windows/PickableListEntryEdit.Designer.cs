
namespace NPC_Maker
{
    partial class PickableListEntryEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickableListEntryEdit));
            this.ID = new System.Windows.Forms.Label();
            this.NumUp_ID = new System.Windows.Forms.NumericUpDown();
            this.Lbl_Name = new System.Windows.Forms.Label();
            this.Txb_Name = new System.Windows.Forms.TextBox();
            this.Txb_Desc = new System.Windows.Forms.TextBox();
            this.Label_Desc = new System.Windows.Forms.Label();
            this.Lbl_HexID = new System.Windows.Forms.Label();
            this.NumUp_HexID = new System.Windows.Forms.NumericUpDown();
            this.Btn_SelectObject = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_HexID)).BeginInit();
            this.SuspendLayout();
            // 
            // ID
            // 
            this.ID.AutoSize = true;
            this.ID.BackColor = System.Drawing.Color.White;
            this.ID.Location = new System.Drawing.Point(12, 9);
            this.ID.Name = "ID";
            this.ID.Size = new System.Drawing.Size(21, 13);
            this.ID.TabIndex = 0;
            this.ID.Text = "ID:";
            // 
            // NumUp_ID
            // 
            this.NumUp_ID.Location = new System.Drawing.Point(126, 7);
            this.NumUp_ID.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUp_ID.Name = "NumUp_ID";
            this.NumUp_ID.Size = new System.Drawing.Size(189, 20);
            this.NumUp_ID.TabIndex = 1;
            this.NumUp_ID.ValueChanged += new System.EventHandler(this.NumUp_ID_ValueChanged);
            // 
            // Lbl_Name
            // 
            this.Lbl_Name.AutoSize = true;
            this.Lbl_Name.BackColor = System.Drawing.Color.White;
            this.Lbl_Name.Location = new System.Drawing.Point(12, 60);
            this.Lbl_Name.Name = "Lbl_Name";
            this.Lbl_Name.Size = new System.Drawing.Size(38, 13);
            this.Lbl_Name.TabIndex = 2;
            this.Lbl_Name.Text = "Name:";
            // 
            // Txb_Name
            // 
            this.Txb_Name.Location = new System.Drawing.Point(126, 57);
            this.Txb_Name.Name = "Txb_Name";
            this.Txb_Name.Size = new System.Drawing.Size(189, 20);
            this.Txb_Name.TabIndex = 3;
            this.Txb_Name.TextChanged += new System.EventHandler(this.Txb_Name_TextChanged);
            // 
            // Txb_Desc
            // 
            this.Txb_Desc.Location = new System.Drawing.Point(126, 83);
            this.Txb_Desc.Name = "Txb_Desc";
            this.Txb_Desc.Size = new System.Drawing.Size(189, 20);
            this.Txb_Desc.TabIndex = 4;
            this.Txb_Desc.TextChanged += new System.EventHandler(this.Txb_Desc_TextChanged);
            // 
            // Label_Desc
            // 
            this.Label_Desc.AutoSize = true;
            this.Label_Desc.BackColor = System.Drawing.Color.White;
            this.Label_Desc.Location = new System.Drawing.Point(12, 86);
            this.Label_Desc.Name = "Label_Desc";
            this.Label_Desc.Size = new System.Drawing.Size(63, 13);
            this.Label_Desc.TabIndex = 5;
            this.Label_Desc.Text = "Description:";
            // 
            // Lbl_HexID
            // 
            this.Lbl_HexID.AutoSize = true;
            this.Lbl_HexID.BackColor = System.Drawing.Color.White;
            this.Lbl_HexID.Location = new System.Drawing.Point(12, 33);
            this.Lbl_HexID.Name = "Lbl_HexID";
            this.Lbl_HexID.Size = new System.Drawing.Size(43, 13);
            this.Lbl_HexID.TabIndex = 6;
            this.Lbl_HexID.Text = "Hex ID:";
            // 
            // NumUp_HexID
            // 
            this.NumUp_HexID.Hexadecimal = true;
            this.NumUp_HexID.Location = new System.Drawing.Point(126, 31);
            this.NumUp_HexID.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUp_HexID.Name = "NumUp_HexID";
            this.NumUp_HexID.Size = new System.Drawing.Size(189, 20);
            this.NumUp_HexID.TabIndex = 7;
            this.NumUp_HexID.ValueChanged += new System.EventHandler(this.NumUp_HexID_ValueChanged);
            // 
            // Btn_SelectObject
            // 
            this.Btn_SelectObject.Location = new System.Drawing.Point(198, 109);
            this.Btn_SelectObject.Name = "Btn_SelectObject";
            this.Btn_SelectObject.Size = new System.Drawing.Size(117, 35);
            this.Btn_SelectObject.TabIndex = 53;
            this.Btn_SelectObject.Text = "Save";
            this.Btn_SelectObject.UseVisualStyleBackColor = true;
            this.Btn_SelectObject.Click += new System.EventHandler(this.Btn_SelectObject_Click);
            // 
            // PickableListEntryEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(324, 157);
            this.Controls.Add(this.Btn_SelectObject);
            this.Controls.Add(this.NumUp_HexID);
            this.Controls.Add(this.Lbl_HexID);
            this.Controls.Add(this.Label_Desc);
            this.Controls.Add(this.Txb_Desc);
            this.Controls.Add(this.Txb_Name);
            this.Controls.Add(this.Lbl_Name);
            this.Controls.Add(this.NumUp_ID);
            this.Controls.Add(this.ID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PickableListEntryEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add/edit entry";
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_HexID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ID;
        private System.Windows.Forms.NumericUpDown NumUp_ID;
        private System.Windows.Forms.Label Lbl_Name;
        private System.Windows.Forms.TextBox Txb_Name;
        private System.Windows.Forms.TextBox Txb_Desc;
        private System.Windows.Forms.Label Label_Desc;
        private System.Windows.Forms.Label Lbl_HexID;
        private System.Windows.Forms.NumericUpDown NumUp_HexID;
        private System.Windows.Forms.Button Btn_SelectObject;
    }
}