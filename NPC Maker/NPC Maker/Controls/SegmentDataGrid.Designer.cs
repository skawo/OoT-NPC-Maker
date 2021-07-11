
namespace NPC_Maker.Controls
{
    partial class SegmentDataGrid
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
            this.Grid = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.Grid.AllowUserToResizeColumns = false;
            this.Grid.AllowUserToResizeRows = false;
            this.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Grid.BackgroundColor = System.Drawing.Color.White;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_Name,
            this.Seg_Offs,
            this.Seg_ObjId});
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 0);
            this.Grid.MultiSelect = false;
            this.Grid.Name = "Grid";
            this.Grid.RowHeadersVisible = false;
            this.Grid.Size = new System.Drawing.Size(960, 497);
            this.Grid.TabIndex = 12;
            // 
            // Seg_Name
            // 
            this.Seg_Name.FillWeight = 50F;
            this.Seg_Name.HeaderText = "Name";
            this.Seg_Name.Name = "Seg_Name";
            this.Seg_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_Offs
            // 
            this.Seg_Offs.HeaderText = "Offset";
            this.Seg_Offs.Name = "Seg_Offs";
            this.Seg_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_ObjId
            // 
            this.Seg_ObjId.FillWeight = 70F;
            this.Seg_ObjId.HeaderText = "Object ID";
            this.Seg_ObjId.Name = "Seg_ObjId";
            this.Seg_ObjId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SegmentDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Grid);
            this.Name = "SegmentDataGrid";
            this.Size = new System.Drawing.Size(960, 497);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public CustomDataGridView Grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_ObjId;
    }
}
