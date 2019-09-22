namespace NPC_Maker
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_New = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_SaveBinary = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DataGrid_NPCs = new System.Windows.Forms.DataGridView();
            this.Col_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label_NPCName = new System.Windows.Forms.Label();
            this.Textbox_NPCName = new System.Windows.Forms.TextBox();
            this.Panel_Editor = new System.Windows.Forms.Panel();
            this.Panel_NPCData = new System.Windows.Forms.Panel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.Tab1_Data = new System.Windows.Forms.TabPage();
            this.Label_ExtraTextures = new System.Windows.Forms.Label();
            this.Label_ExtraDisplayLists = new System.Windows.Forms.Label();
            this.DataGridView_ExtraDLists = new System.Windows.Forms.DataGridView();
            this.Checkbox_EnvColor = new System.Windows.Forms.CheckBox();
            this.Button_EnvironmentColorPreview = new System.Windows.Forms.Button();
            this.Label_TalkingFramesBetween = new System.Windows.Forms.Label();
            this.NumUpDown_TalkSegment = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_BlinkSegment = new System.Windows.Forms.NumericUpDown();
            this.Label_BlinkingFramesBetween = new System.Windows.Forms.Label();
            this.Label_TalkingSegment = new System.Windows.Forms.Label();
            this.Label_TalkingPattern = new System.Windows.Forms.Label();
            this.NumUpDown_TalkSpeed = new System.Windows.Forms.NumericUpDown();
            this.Textbox_BlinkPattern = new System.Windows.Forms.TextBox();
            this.NumUpDown_BlinkSpeed = new System.Windows.Forms.NumericUpDown();
            this.Label_BlinkingPattern = new System.Windows.Forms.Label();
            this.Label_BlinkingSegment = new System.Windows.Forms.Label();
            this.Textbox_TalkingPattern = new System.Windows.Forms.TextBox();
            this.TabControl_Textures = new System.Windows.Forms.TabControl();
            this.TabPage_Segment_8 = new System.Windows.Forms.TabPage();
            this.Seg_8 = new System.Windows.Forms.DataGridView();
            this.Seg_8_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_8_TextOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_9 = new System.Windows.Forms.TabPage();
            this.Seg_9 = new System.Windows.Forms.DataGridView();
            this.Seg_9_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_9_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_A = new System.Windows.Forms.TabPage();
            this.Seg_A = new System.Windows.Forms.DataGridView();
            this.Seg_A_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_A_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_B = new System.Windows.Forms.TabPage();
            this.Seg_B = new System.Windows.Forms.DataGridView();
            this.Seg_B_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_B_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_C = new System.Windows.Forms.TabPage();
            this.Seg_C = new System.Windows.Forms.DataGridView();
            this.Seg_C_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_C_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_D = new System.Windows.Forms.TabPage();
            this.Seg_D = new System.Windows.Forms.DataGridView();
            this.Seg_D_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_D_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_E = new System.Windows.Forms.TabPage();
            this.Seg_E = new System.Windows.Forms.DataGridView();
            this.Seg_E_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_E_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_F = new System.Windows.Forms.TabPage();
            this.Seg_F = new System.Windows.Forms.DataGridView();
            this.Seg_F_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_F_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Panel_HeadRot = new System.Windows.Forms.Panel();
            this.ComboBox_HeadLookAxis = new System.Windows.Forms.ComboBox();
            this.NumUpDown_DegVert = new System.Windows.Forms.NumericUpDown();
            this.Label_HeadLookAxis = new System.Windows.Forms.Label();
            this.Label_DegVert = new System.Windows.Forms.Label();
            this.Label_Limb = new System.Windows.Forms.Label();
            this.NumUpDown_DegHoz = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_Limb = new System.Windows.Forms.NumericUpDown();
            this.Label_DegHoz = new System.Windows.Forms.Label();
            this.Panel_TargetPanel = new System.Windows.Forms.Panel();
            this.NumUpDown_ZTargetOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_TargetLimb = new System.Windows.Forms.Label();
            this.NumUpDown_YTargetOffs = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_TargetLimb = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_XTargetOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_TargetOffset = new System.Windows.Forms.Label();
            this.Checkbox_Targettable = new System.Windows.Forms.CheckBox();
            this.NumUpDown_ObjectID = new System.Windows.Forms.NumericUpDown();
            this.Panel_Movement = new System.Windows.Forms.Panel();
            this.Label_LoopDelay = new System.Windows.Forms.Label();
            this.Label_LoopStartNode = new System.Windows.Forms.Label();
            this.NumUpDown_LoopStartNode = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_LoopDelay = new System.Windows.Forms.NumericUpDown();
            this.Label_LoopEndNode = new System.Windows.Forms.Label();
            this.NumUpDown_LoopEndNode = new System.Windows.Forms.NumericUpDown();
            this.Checkbox_Loop = new System.Windows.Forms.CheckBox();
            this.Label_PathFollowID = new System.Windows.Forms.Label();
            this.NumUpDown_PathFollowID = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_MovDistance = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_MovSpeed = new System.Windows.Forms.NumericUpDown();
            this.Label_Distance = new System.Windows.Forms.Label();
            this.Label_Speed = new System.Windows.Forms.Label();
            this.Label_ObjectID = new System.Windows.Forms.Label();
            this.Combo_MovementType = new System.Windows.Forms.ComboBox();
            this.NumUpDown_ZModelOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_MovementType = new System.Windows.Forms.Label();
            this.Label_Hierarchy = new System.Windows.Forms.Label();
            this.Checkbox_Pushable = new System.Windows.Forms.CheckBox();
            this.NumUpDown_Hierarchy = new System.Windows.Forms.NumericUpDown();
            this.Checkbox_CanPressSwitches = new System.Windows.Forms.CheckBox();
            this.NumUpDown_YModelOffs = new System.Windows.Forms.NumericUpDown();
            this.Checkbox_DrawShadow = new System.Windows.Forms.CheckBox();
            this.DataGrid_Animations = new System.Windows.Forms.DataGridView();
            this.Col_AnimName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Anim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_OBJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Checkbox_HaveCollision = new System.Windows.Forms.CheckBox();
            this.Label_AnimDefs = new System.Windows.Forms.Label();
            this.Panel_Collision = new System.Windows.Forms.Panel();
            this.NumUpDown_ZColOffs = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_YColOffs = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_XColOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_Offset = new System.Windows.Forms.Label();
            this.Label_Height = new System.Windows.Forms.Label();
            this.NumUpDown_ColHeight = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_ColRadius = new System.Windows.Forms.NumericUpDown();
            this.Label_Radius = new System.Windows.Forms.Label();
            this.Label_Collision = new System.Windows.Forms.Label();
            this.NumUpDown_XModelOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_LookAtType = new System.Windows.Forms.Label();
            this.ComboBox_LookAtType = new System.Windows.Forms.ComboBox();
            this.ComboBox_HierarchyType = new System.Windows.Forms.ComboBox();
            this.Label_ModelDrawOffs = new System.Windows.Forms.Label();
            this.Label_HierarchyType = new System.Windows.Forms.Label();
            this.ComboBox_AnimType = new System.Windows.Forms.ComboBox();
            this.Label_AnimType = new System.Windows.Forms.Label();
            this.Label_Scale = new System.Windows.Forms.Label();
            this.NumUpDown_Scale = new System.Windows.Forms.NumericUpDown();
            this.Tab2_Script = new System.Windows.Forms.TabPage();
            this.Textbox_Script = new FastColoredTextBoxNS.FastColoredTextBox();
            this.Button_TryParse = new System.Windows.Forms.Button();
            this.Textbox_ParseErrors = new System.Windows.Forms.TextBox();
            this.Panel_NPCList = new System.Windows.Forms.Panel();
            this.Button_PasteBase = new System.Windows.Forms.Button();
            this.Button_CopyBase = new System.Windows.Forms.Button();
            this.Button_Duplicate = new System.Windows.Forms.Button();
            this.Button_Delete = new System.Windows.Forms.Button();
            this.Button_Add = new System.Windows.Forms.Button();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.ExtraDlists_Purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Translation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Rotation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Limb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_ShowType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_NPCs)).BeginInit();
            this.Panel_Editor.SuspendLayout();
            this.Panel_NPCData.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.Tab1_Data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_ExtraDLists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSegment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSegment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSpeed)).BeginInit();
            this.TabControl_Textures.SuspendLayout();
            this.TabPage_Segment_8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_8)).BeginInit();
            this.TabPage_Segment_9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_9)).BeginInit();
            this.TabPage_Segment_A.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_A)).BeginInit();
            this.TabPage_Segment_B.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_B)).BeginInit();
            this.TabPage_Segment_C.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_C)).BeginInit();
            this.TabPage_Segment_D.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_D)).BeginInit();
            this.TabPage_Segment_E.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_E)).BeginInit();
            this.TabPage_Segment_F.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Seg_F)).BeginInit();
            this.Panel_HeadRot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegVert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegHoz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Limb)).BeginInit();
            this.Panel_TargetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZTargetOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YTargetOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TargetLimb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XTargetOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ObjectID)).BeginInit();
            this.Panel_Movement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopStartNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopEndNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_PathFollowID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Hierarchy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Animations)).BeginInit();
            this.Panel_Collision.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZColOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YColOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XColOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Scale)).BeginInit();
            this.Tab2_Script.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Textbox_Script)).BeginInit();
            this.Panel_NPCList.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.aboutToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.MaximumSize = new System.Drawing.Size(2000, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1551, 24);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu_New,
            this.FileMenu_Open,
            this.FileMenu_Save,
            this.FileMenu_SaveAs,
            this.FileMenu_SaveBinary,
            this.FileMenu_Exit});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // FileMenu_New
            // 
            this.FileMenu_New.Name = "FileMenu_New";
            this.FileMenu_New.Size = new System.Drawing.Size(143, 22);
            this.FileMenu_New.Text = "New";
            this.FileMenu_New.Click += new System.EventHandler(this.FileMenu_New_Click);
            // 
            // FileMenu_Open
            // 
            this.FileMenu_Open.Name = "FileMenu_Open";
            this.FileMenu_Open.Size = new System.Drawing.Size(143, 22);
            this.FileMenu_Open.Text = "Open...";
            this.FileMenu_Open.Click += new System.EventHandler(this.FileMenu_Open_Click);
            // 
            // FileMenu_Save
            // 
            this.FileMenu_Save.Name = "FileMenu_Save";
            this.FileMenu_Save.Size = new System.Drawing.Size(143, 22);
            this.FileMenu_Save.Text = "Save...";
            this.FileMenu_Save.Click += new System.EventHandler(this.FileMenu_Save_Click);
            // 
            // FileMenu_SaveAs
            // 
            this.FileMenu_SaveAs.Name = "FileMenu_SaveAs";
            this.FileMenu_SaveAs.Size = new System.Drawing.Size(143, 22);
            this.FileMenu_SaveAs.Text = "Save as...";
            this.FileMenu_SaveAs.Click += new System.EventHandler(this.FileMenu_SaveAs_Click);
            // 
            // FileMenu_SaveBinary
            // 
            this.FileMenu_SaveBinary.Name = "FileMenu_SaveBinary";
            this.FileMenu_SaveBinary.Size = new System.Drawing.Size(143, 22);
            this.FileMenu_SaveBinary.Text = "Save binary...";
            this.FileMenu_SaveBinary.Click += new System.EventHandler(this.FileMenu_SaveBinary_Click);
            // 
            // FileMenu_Exit
            // 
            this.FileMenu_Exit.Name = "FileMenu_Exit";
            this.FileMenu_Exit.Size = new System.Drawing.Size(143, 22);
            this.FileMenu_Exit.Text = "Exit";
            this.FileMenu_Exit.Click += new System.EventHandler(this.FileMenu_Exit_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // DataGrid_NPCs
            // 
            this.DataGrid_NPCs.AllowUserToAddRows = false;
            this.DataGrid_NPCs.AllowUserToDeleteRows = false;
            this.DataGrid_NPCs.AllowUserToResizeColumns = false;
            this.DataGrid_NPCs.AllowUserToResizeRows = false;
            this.DataGrid_NPCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DataGrid_NPCs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGrid_NPCs.BackgroundColor = System.Drawing.Color.White;
            this.DataGrid_NPCs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_NPCs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_ID,
            this.Col_Name});
            this.DataGrid_NPCs.Location = new System.Drawing.Point(3, 0);
            this.DataGrid_NPCs.MultiSelect = false;
            this.DataGrid_NPCs.Name = "DataGrid_NPCs";
            this.DataGrid_NPCs.ReadOnly = true;
            this.DataGrid_NPCs.RowHeadersVisible = false;
            this.DataGrid_NPCs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGrid_NPCs.Size = new System.Drawing.Size(236, 708);
            this.DataGrid_NPCs.TabIndex = 2;
            this.DataGrid_NPCs.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_NPCs_CellDoubleClick);
            this.DataGrid_NPCs.SelectionChanged += new System.EventHandler(this.DataGrid_NPCs_SelectionChanged);
            // 
            // Col_ID
            // 
            this.Col_ID.FillWeight = 40F;
            this.Col_ID.HeaderText = "NPC ID";
            this.Col_ID.Name = "Col_ID";
            this.Col_ID.ReadOnly = true;
            this.Col_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_Name
            // 
            this.Col_Name.HeaderText = "NPC Name";
            this.Col_Name.Name = "Col_Name";
            this.Col_Name.ReadOnly = true;
            this.Col_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Label_NPCName
            // 
            this.Label_NPCName.AutoSize = true;
            this.Label_NPCName.Location = new System.Drawing.Point(11, 11);
            this.Label_NPCName.Name = "Label_NPCName";
            this.Label_NPCName.Size = new System.Drawing.Size(63, 13);
            this.Label_NPCName.TabIndex = 3;
            this.Label_NPCName.Text = "NPC Name:";
            // 
            // Textbox_NPCName
            // 
            this.Textbox_NPCName.Location = new System.Drawing.Point(113, 8);
            this.Textbox_NPCName.MaxLength = 32;
            this.Textbox_NPCName.Name = "Textbox_NPCName";
            this.Textbox_NPCName.Size = new System.Drawing.Size(218, 20);
            this.Textbox_NPCName.TabIndex = 4;
            this.Textbox_NPCName.Tag = "NPCNAME";
            this.Textbox_NPCName.TextChanged += new System.EventHandler(this.Textbox_NPCName_TextChanged);
            // 
            // Panel_Editor
            // 
            this.Panel_Editor.Controls.Add(this.Panel_NPCData);
            this.Panel_Editor.Controls.Add(this.Panel_NPCList);
            this.Panel_Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Editor.Enabled = false;
            this.Panel_Editor.Location = new System.Drawing.Point(0, 24);
            this.Panel_Editor.Name = "Panel_Editor";
            this.Panel_Editor.Size = new System.Drawing.Size(1551, 794);
            this.Panel_Editor.TabIndex = 5;
            // 
            // Panel_NPCData
            // 
            this.Panel_NPCData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_NPCData.Controls.Add(this.TabControl);
            this.Panel_NPCData.Enabled = false;
            this.Panel_NPCData.Location = new System.Drawing.Point(245, 3);
            this.Panel_NPCData.Name = "Panel_NPCData";
            this.Panel_NPCData.Size = new System.Drawing.Size(1303, 788);
            this.Panel_NPCData.TabIndex = 6;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.Tab1_Data);
            this.TabControl.Controls.Add(this.Tab2_Script);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1303, 788);
            this.TabControl.TabIndex = 41;
            // 
            // Tab1_Data
            // 
            this.Tab1_Data.Controls.Add(this.Label_ExtraTextures);
            this.Tab1_Data.Controls.Add(this.Label_ExtraDisplayLists);
            this.Tab1_Data.Controls.Add(this.DataGridView_ExtraDLists);
            this.Tab1_Data.Controls.Add(this.Checkbox_EnvColor);
            this.Tab1_Data.Controls.Add(this.Button_EnvironmentColorPreview);
            this.Tab1_Data.Controls.Add(this.Label_TalkingFramesBetween);
            this.Tab1_Data.Controls.Add(this.NumUpDown_TalkSegment);
            this.Tab1_Data.Controls.Add(this.NumUpDown_BlinkSegment);
            this.Tab1_Data.Controls.Add(this.Label_BlinkingFramesBetween);
            this.Tab1_Data.Controls.Add(this.Label_TalkingSegment);
            this.Tab1_Data.Controls.Add(this.Label_TalkingPattern);
            this.Tab1_Data.Controls.Add(this.NumUpDown_TalkSpeed);
            this.Tab1_Data.Controls.Add(this.Textbox_BlinkPattern);
            this.Tab1_Data.Controls.Add(this.NumUpDown_BlinkSpeed);
            this.Tab1_Data.Controls.Add(this.Label_BlinkingPattern);
            this.Tab1_Data.Controls.Add(this.Label_BlinkingSegment);
            this.Tab1_Data.Controls.Add(this.Textbox_TalkingPattern);
            this.Tab1_Data.Controls.Add(this.TabControl_Textures);
            this.Tab1_Data.Controls.Add(this.Panel_HeadRot);
            this.Tab1_Data.Controls.Add(this.Label_NPCName);
            this.Tab1_Data.Controls.Add(this.Panel_TargetPanel);
            this.Tab1_Data.Controls.Add(this.Textbox_NPCName);
            this.Tab1_Data.Controls.Add(this.Checkbox_Targettable);
            this.Tab1_Data.Controls.Add(this.NumUpDown_ObjectID);
            this.Tab1_Data.Controls.Add(this.Panel_Movement);
            this.Tab1_Data.Controls.Add(this.Label_ObjectID);
            this.Tab1_Data.Controls.Add(this.Combo_MovementType);
            this.Tab1_Data.Controls.Add(this.NumUpDown_ZModelOffs);
            this.Tab1_Data.Controls.Add(this.Label_MovementType);
            this.Tab1_Data.Controls.Add(this.Label_Hierarchy);
            this.Tab1_Data.Controls.Add(this.Checkbox_Pushable);
            this.Tab1_Data.Controls.Add(this.NumUpDown_Hierarchy);
            this.Tab1_Data.Controls.Add(this.Checkbox_CanPressSwitches);
            this.Tab1_Data.Controls.Add(this.NumUpDown_YModelOffs);
            this.Tab1_Data.Controls.Add(this.Checkbox_DrawShadow);
            this.Tab1_Data.Controls.Add(this.DataGrid_Animations);
            this.Tab1_Data.Controls.Add(this.Checkbox_HaveCollision);
            this.Tab1_Data.Controls.Add(this.Label_AnimDefs);
            this.Tab1_Data.Controls.Add(this.Panel_Collision);
            this.Tab1_Data.Controls.Add(this.NumUpDown_XModelOffs);
            this.Tab1_Data.Controls.Add(this.Label_LookAtType);
            this.Tab1_Data.Controls.Add(this.ComboBox_LookAtType);
            this.Tab1_Data.Controls.Add(this.ComboBox_HierarchyType);
            this.Tab1_Data.Controls.Add(this.Label_ModelDrawOffs);
            this.Tab1_Data.Controls.Add(this.Label_HierarchyType);
            this.Tab1_Data.Controls.Add(this.ComboBox_AnimType);
            this.Tab1_Data.Controls.Add(this.Label_AnimType);
            this.Tab1_Data.Controls.Add(this.Label_Scale);
            this.Tab1_Data.Controls.Add(this.NumUpDown_Scale);
            this.Tab1_Data.Location = new System.Drawing.Point(4, 22);
            this.Tab1_Data.Name = "Tab1_Data";
            this.Tab1_Data.Padding = new System.Windows.Forms.Padding(3);
            this.Tab1_Data.Size = new System.Drawing.Size(1295, 762);
            this.Tab1_Data.TabIndex = 0;
            this.Tab1_Data.Text = "General data";
            this.Tab1_Data.UseVisualStyleBackColor = true;
            // 
            // Label_ExtraTextures
            // 
            this.Label_ExtraTextures.AutoSize = true;
            this.Label_ExtraTextures.Location = new System.Drawing.Point(351, 393);
            this.Label_ExtraTextures.Name = "Label_ExtraTextures";
            this.Label_ExtraTextures.Size = new System.Drawing.Size(74, 13);
            this.Label_ExtraTextures.TabIndex = 53;
            this.Label_ExtraTextures.Text = "Extra textures:";
            // 
            // Label_ExtraDisplayLists
            // 
            this.Label_ExtraDisplayLists.AutoSize = true;
            this.Label_ExtraDisplayLists.Location = new System.Drawing.Point(351, 11);
            this.Label_ExtraDisplayLists.Name = "Label_ExtraDisplayLists";
            this.Label_ExtraDisplayLists.Size = new System.Drawing.Size(89, 13);
            this.Label_ExtraDisplayLists.TabIndex = 52;
            this.Label_ExtraDisplayLists.Text = "Extra display lists:";
            // 
            // DataGridView_ExtraDLists
            // 
            this.DataGridView_ExtraDLists.AllowUserToResizeColumns = false;
            this.DataGridView_ExtraDLists.AllowUserToResizeRows = false;
            this.DataGridView_ExtraDLists.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DataGridView_ExtraDLists.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView_ExtraDLists.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_ExtraDLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_ExtraDLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExtraDlists_Purpose,
            this.ExtraDlists_Offset,
            this.ExtraDlists_Translation,
            this.ExtraDlists_Rotation,
            this.ExtraDlists_Limb,
            this.ExtraDlists_ShowType});
            this.DataGridView_ExtraDLists.Location = new System.Drawing.Point(350, 34);
            this.DataGridView_ExtraDLists.MultiSelect = false;
            this.DataGridView_ExtraDLists.Name = "DataGridView_ExtraDLists";
            this.DataGridView_ExtraDLists.RowHeadersVisible = false;
            this.DataGridView_ExtraDLists.Size = new System.Drawing.Size(514, 356);
            this.DataGridView_ExtraDLists.TabIndex = 51;
            this.DataGridView_ExtraDLists.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridView_ExtraDLists_CellParsing);
            this.DataGridView_ExtraDLists.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridView_ExtraDLists_KeyUp);
            // 
            // Checkbox_EnvColor
            // 
            this.Checkbox_EnvColor.AutoSize = true;
            this.Checkbox_EnvColor.Location = new System.Drawing.Point(1126, 375);
            this.Checkbox_EnvColor.Name = "Checkbox_EnvColor";
            this.Checkbox_EnvColor.Size = new System.Drawing.Size(114, 17);
            this.Checkbox_EnvColor.TabIndex = 50;
            this.Checkbox_EnvColor.Tag = "";
            this.Checkbox_EnvColor.Text = "Environment color:";
            this.Checkbox_EnvColor.UseVisualStyleBackColor = true;
            this.Checkbox_EnvColor.CheckedChanged += new System.EventHandler(this.Checkbox_EnvColor_CheckedChanged);
            // 
            // Button_EnvironmentColorPreview
            // 
            this.Button_EnvironmentColorPreview.BackColor = System.Drawing.Color.Black;
            this.Button_EnvironmentColorPreview.Location = new System.Drawing.Point(1246, 371);
            this.Button_EnvironmentColorPreview.Name = "Button_EnvironmentColorPreview";
            this.Button_EnvironmentColorPreview.Size = new System.Drawing.Size(42, 23);
            this.Button_EnvironmentColorPreview.TabIndex = 49;
            this.Button_EnvironmentColorPreview.UseVisualStyleBackColor = false;
            this.Button_EnvironmentColorPreview.Click += new System.EventHandler(this.Button_EnvironmentColorPreview_Click);
            // 
            // Label_TalkingFramesBetween
            // 
            this.Label_TalkingFramesBetween.AutoSize = true;
            this.Label_TalkingFramesBetween.Location = new System.Drawing.Point(662, 691);
            this.Label_TalkingFramesBetween.Name = "Label_TalkingFramesBetween";
            this.Label_TalkingFramesBetween.Size = new System.Drawing.Size(123, 13);
            this.Label_TalkingFramesBetween.TabIndex = 48;
            this.Label_TalkingFramesBetween.Text = "Talking frames between:";
            // 
            // NumUpDown_TalkSegment
            // 
            this.NumUpDown_TalkSegment.Hexadecimal = true;
            this.NumUpDown_TalkSegment.Location = new System.Drawing.Point(804, 666);
            this.NumUpDown_TalkSegment.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_TalkSegment.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NumUpDown_TalkSegment.Name = "NumUpDown_TalkSegment";
            this.NumUpDown_TalkSegment.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_TalkSegment.TabIndex = 47;
            this.NumUpDown_TalkSegment.Tag = "TALKSEG";
            this.NumUpDown_TalkSegment.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NumUpDown_TalkSegment.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_BlinkSegment
            // 
            this.NumUpDown_BlinkSegment.Hexadecimal = true;
            this.NumUpDown_BlinkSegment.Location = new System.Drawing.Point(596, 666);
            this.NumUpDown_BlinkSegment.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_BlinkSegment.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NumUpDown_BlinkSegment.Name = "NumUpDown_BlinkSegment";
            this.NumUpDown_BlinkSegment.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_BlinkSegment.TabIndex = 45;
            this.NumUpDown_BlinkSegment.Tag = "BLINKSEG";
            this.NumUpDown_BlinkSegment.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NumUpDown_BlinkSegment.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_BlinkingFramesBetween
            // 
            this.Label_BlinkingFramesBetween.AutoSize = true;
            this.Label_BlinkingFramesBetween.Location = new System.Drawing.Point(461, 691);
            this.Label_BlinkingFramesBetween.Name = "Label_BlinkingFramesBetween";
            this.Label_BlinkingFramesBetween.Size = new System.Drawing.Size(125, 13);
            this.Label_BlinkingFramesBetween.TabIndex = 46;
            this.Label_BlinkingFramesBetween.Text = "Blinking frames between:";
            // 
            // Label_TalkingSegment
            // 
            this.Label_TalkingSegment.AutoSize = true;
            this.Label_TalkingSegment.Location = new System.Drawing.Point(662, 670);
            this.Label_TalkingSegment.Name = "Label_TalkingSegment";
            this.Label_TalkingSegment.Size = new System.Drawing.Size(88, 13);
            this.Label_TalkingSegment.TabIndex = 38;
            this.Label_TalkingSegment.Text = "Talking segment:";
            // 
            // Label_TalkingPattern
            // 
            this.Label_TalkingPattern.AutoSize = true;
            this.Label_TalkingPattern.Location = new System.Drawing.Point(550, 741);
            this.Label_TalkingPattern.Name = "Label_TalkingPattern";
            this.Label_TalkingPattern.Size = new System.Drawing.Size(81, 13);
            this.Label_TalkingPattern.TabIndex = 44;
            this.Label_TalkingPattern.Text = "Talking pattern:";
            // 
            // NumUpDown_TalkSpeed
            // 
            this.NumUpDown_TalkSpeed.Location = new System.Drawing.Point(804, 689);
            this.NumUpDown_TalkSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_TalkSpeed.Name = "NumUpDown_TalkSpeed";
            this.NumUpDown_TalkSpeed.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_TalkSpeed.TabIndex = 37;
            this.NumUpDown_TalkSpeed.Tag = "TALKSPE";
            this.NumUpDown_TalkSpeed.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Textbox_BlinkPattern
            // 
            this.Textbox_BlinkPattern.Location = new System.Drawing.Point(648, 715);
            this.Textbox_BlinkPattern.Name = "Textbox_BlinkPattern";
            this.Textbox_BlinkPattern.Size = new System.Drawing.Size(216, 20);
            this.Textbox_BlinkPattern.TabIndex = 43;
            this.Textbox_BlinkPattern.Tag = "BLINKPAT";
            this.Textbox_BlinkPattern.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // NumUpDown_BlinkSpeed
            // 
            this.NumUpDown_BlinkSpeed.Location = new System.Drawing.Point(596, 689);
            this.NumUpDown_BlinkSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_BlinkSpeed.Name = "NumUpDown_BlinkSpeed";
            this.NumUpDown_BlinkSpeed.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_BlinkSpeed.TabIndex = 35;
            this.NumUpDown_BlinkSpeed.Tag = "BLINKSPE";
            this.NumUpDown_BlinkSpeed.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_BlinkingPattern
            // 
            this.Label_BlinkingPattern.AutoSize = true;
            this.Label_BlinkingPattern.Location = new System.Drawing.Point(550, 718);
            this.Label_BlinkingPattern.Name = "Label_BlinkingPattern";
            this.Label_BlinkingPattern.Size = new System.Drawing.Size(83, 13);
            this.Label_BlinkingPattern.TabIndex = 35;
            this.Label_BlinkingPattern.Text = "Blinking pattern:";
            // 
            // Label_BlinkingSegment
            // 
            this.Label_BlinkingSegment.AutoSize = true;
            this.Label_BlinkingSegment.Location = new System.Drawing.Point(461, 668);
            this.Label_BlinkingSegment.Name = "Label_BlinkingSegment";
            this.Label_BlinkingSegment.Size = new System.Drawing.Size(90, 13);
            this.Label_BlinkingSegment.TabIndex = 36;
            this.Label_BlinkingSegment.Text = "Blinking segment:";
            // 
            // Textbox_TalkingPattern
            // 
            this.Textbox_TalkingPattern.Location = new System.Drawing.Point(648, 738);
            this.Textbox_TalkingPattern.Name = "Textbox_TalkingPattern";
            this.Textbox_TalkingPattern.Size = new System.Drawing.Size(216, 20);
            this.Textbox_TalkingPattern.TabIndex = 42;
            this.Textbox_TalkingPattern.Tag = "TALKPAT";
            this.Textbox_TalkingPattern.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // TabControl_Textures
            // 
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_8);
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_9);
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_A);
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_B);
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_C);
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_D);
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_E);
            this.TabControl_Textures.Controls.Add(this.TabPage_Segment_F);
            this.TabControl_Textures.Location = new System.Drawing.Point(350, 409);
            this.TabControl_Textures.Name = "TabControl_Textures";
            this.TabControl_Textures.SelectedIndex = 0;
            this.TabControl_Textures.Size = new System.Drawing.Size(521, 255);
            this.TabControl_Textures.TabIndex = 41;
            // 
            // TabPage_Segment_8
            // 
            this.TabPage_Segment_8.Controls.Add(this.Seg_8);
            this.TabPage_Segment_8.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_8.Name = "TabPage_Segment_8";
            this.TabPage_Segment_8.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_8.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_8.TabIndex = 0;
            this.TabPage_Segment_8.Text = "Segment 8";
            this.TabPage_Segment_8.UseVisualStyleBackColor = true;
            // 
            // Seg_8
            // 
            this.Seg_8.AllowUserToResizeColumns = false;
            this.Seg_8.AllowUserToResizeRows = false;
            this.Seg_8.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_8.BackgroundColor = System.Drawing.Color.White;
            this.Seg_8.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_8.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_8_Name,
            this.Seg_8_TextOffs});
            this.Seg_8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_8.Location = new System.Drawing.Point(3, 3);
            this.Seg_8.MultiSelect = false;
            this.Seg_8.Name = "Seg_8";
            this.Seg_8.RowHeadersVisible = false;
            this.Seg_8.Size = new System.Drawing.Size(507, 223);
            this.Seg_8.TabIndex = 10;
            this.Seg_8.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_8.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_8_Name
            // 
            this.Seg_8_Name.FillWeight = 50F;
            this.Seg_8_Name.HeaderText = "Name";
            this.Seg_8_Name.Name = "Seg_8_Name";
            this.Seg_8_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_8_TextOffs
            // 
            this.Seg_8_TextOffs.HeaderText = "Texture Offset";
            this.Seg_8_TextOffs.Name = "Seg_8_TextOffs";
            this.Seg_8_TextOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TabPage_Segment_9
            // 
            this.TabPage_Segment_9.Controls.Add(this.Seg_9);
            this.TabPage_Segment_9.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_9.Name = "TabPage_Segment_9";
            this.TabPage_Segment_9.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_9.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_9.TabIndex = 1;
            this.TabPage_Segment_9.Text = "Segment 9";
            this.TabPage_Segment_9.UseVisualStyleBackColor = true;
            // 
            // Seg_9
            // 
            this.Seg_9.AllowUserToResizeColumns = false;
            this.Seg_9.AllowUserToResizeRows = false;
            this.Seg_9.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_9.BackgroundColor = System.Drawing.Color.White;
            this.Seg_9.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_9.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_9_Name,
            this.Seg_9_TexOffs});
            this.Seg_9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_9.Location = new System.Drawing.Point(3, 3);
            this.Seg_9.MultiSelect = false;
            this.Seg_9.Name = "Seg_9";
            this.Seg_9.RowHeadersVisible = false;
            this.Seg_9.Size = new System.Drawing.Size(507, 223);
            this.Seg_9.TabIndex = 11;
            this.Seg_9.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_9.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_9_Name
            // 
            this.Seg_9_Name.FillWeight = 50F;
            this.Seg_9_Name.HeaderText = "Name";
            this.Seg_9_Name.Name = "Seg_9_Name";
            this.Seg_9_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_9_TexOffs
            // 
            this.Seg_9_TexOffs.HeaderText = "Texture Offset";
            this.Seg_9_TexOffs.Name = "Seg_9_TexOffs";
            this.Seg_9_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TabPage_Segment_A
            // 
            this.TabPage_Segment_A.Controls.Add(this.Seg_A);
            this.TabPage_Segment_A.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_A.Name = "TabPage_Segment_A";
            this.TabPage_Segment_A.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_A.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_A.TabIndex = 2;
            this.TabPage_Segment_A.Text = "Segment A";
            this.TabPage_Segment_A.UseVisualStyleBackColor = true;
            // 
            // Seg_A
            // 
            this.Seg_A.AllowUserToResizeColumns = false;
            this.Seg_A.AllowUserToResizeRows = false;
            this.Seg_A.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_A.BackgroundColor = System.Drawing.Color.White;
            this.Seg_A.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_A.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_A_Name,
            this.Seg_A_TexOffs});
            this.Seg_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_A.Location = new System.Drawing.Point(3, 3);
            this.Seg_A.MultiSelect = false;
            this.Seg_A.Name = "Seg_A";
            this.Seg_A.RowHeadersVisible = false;
            this.Seg_A.Size = new System.Drawing.Size(507, 223);
            this.Seg_A.TabIndex = 11;
            this.Seg_A.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_A.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_A_Name
            // 
            this.Seg_A_Name.FillWeight = 50F;
            this.Seg_A_Name.HeaderText = "Name";
            this.Seg_A_Name.Name = "Seg_A_Name";
            this.Seg_A_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_A_TexOffs
            // 
            this.Seg_A_TexOffs.HeaderText = "Texture Offset";
            this.Seg_A_TexOffs.Name = "Seg_A_TexOffs";
            this.Seg_A_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TabPage_Segment_B
            // 
            this.TabPage_Segment_B.Controls.Add(this.Seg_B);
            this.TabPage_Segment_B.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_B.Name = "TabPage_Segment_B";
            this.TabPage_Segment_B.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_B.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_B.TabIndex = 3;
            this.TabPage_Segment_B.Text = "Segment B";
            this.TabPage_Segment_B.UseVisualStyleBackColor = true;
            // 
            // Seg_B
            // 
            this.Seg_B.AllowUserToResizeColumns = false;
            this.Seg_B.AllowUserToResizeRows = false;
            this.Seg_B.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_B.BackgroundColor = System.Drawing.Color.White;
            this.Seg_B.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_B.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_B_Name,
            this.Seg_B_TexOffs});
            this.Seg_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_B.Location = new System.Drawing.Point(3, 3);
            this.Seg_B.MultiSelect = false;
            this.Seg_B.Name = "Seg_B";
            this.Seg_B.RowHeadersVisible = false;
            this.Seg_B.Size = new System.Drawing.Size(507, 223);
            this.Seg_B.TabIndex = 11;
            this.Seg_B.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_B.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_B_Name
            // 
            this.Seg_B_Name.FillWeight = 50F;
            this.Seg_B_Name.HeaderText = "Name";
            this.Seg_B_Name.Name = "Seg_B_Name";
            this.Seg_B_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_B_TexOffs
            // 
            this.Seg_B_TexOffs.HeaderText = "Texture Offset";
            this.Seg_B_TexOffs.Name = "Seg_B_TexOffs";
            this.Seg_B_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TabPage_Segment_C
            // 
            this.TabPage_Segment_C.Controls.Add(this.Seg_C);
            this.TabPage_Segment_C.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_C.Name = "TabPage_Segment_C";
            this.TabPage_Segment_C.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_C.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_C.TabIndex = 4;
            this.TabPage_Segment_C.Text = "Segment C";
            this.TabPage_Segment_C.UseVisualStyleBackColor = true;
            // 
            // Seg_C
            // 
            this.Seg_C.AllowUserToResizeColumns = false;
            this.Seg_C.AllowUserToResizeRows = false;
            this.Seg_C.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_C.BackgroundColor = System.Drawing.Color.White;
            this.Seg_C.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_C.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_C_Name,
            this.Seg_C_TexOffs});
            this.Seg_C.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_C.Location = new System.Drawing.Point(3, 3);
            this.Seg_C.MultiSelect = false;
            this.Seg_C.Name = "Seg_C";
            this.Seg_C.RowHeadersVisible = false;
            this.Seg_C.Size = new System.Drawing.Size(507, 223);
            this.Seg_C.TabIndex = 11;
            this.Seg_C.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_C.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_C_Name
            // 
            this.Seg_C_Name.FillWeight = 50F;
            this.Seg_C_Name.HeaderText = "Name";
            this.Seg_C_Name.Name = "Seg_C_Name";
            this.Seg_C_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_C_TexOffs
            // 
            this.Seg_C_TexOffs.HeaderText = "Texture Offset";
            this.Seg_C_TexOffs.Name = "Seg_C_TexOffs";
            this.Seg_C_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TabPage_Segment_D
            // 
            this.TabPage_Segment_D.Controls.Add(this.Seg_D);
            this.TabPage_Segment_D.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_D.Name = "TabPage_Segment_D";
            this.TabPage_Segment_D.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_D.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_D.TabIndex = 5;
            this.TabPage_Segment_D.Text = "Segment D";
            this.TabPage_Segment_D.UseVisualStyleBackColor = true;
            // 
            // Seg_D
            // 
            this.Seg_D.AllowUserToResizeColumns = false;
            this.Seg_D.AllowUserToResizeRows = false;
            this.Seg_D.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_D.BackgroundColor = System.Drawing.Color.White;
            this.Seg_D.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_D.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_D_Name,
            this.Seg_D_TexOffs});
            this.Seg_D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_D.Location = new System.Drawing.Point(3, 3);
            this.Seg_D.MultiSelect = false;
            this.Seg_D.Name = "Seg_D";
            this.Seg_D.RowHeadersVisible = false;
            this.Seg_D.Size = new System.Drawing.Size(507, 223);
            this.Seg_D.TabIndex = 11;
            this.Seg_D.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_D.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_D_Name
            // 
            this.Seg_D_Name.FillWeight = 50F;
            this.Seg_D_Name.HeaderText = "Name";
            this.Seg_D_Name.Name = "Seg_D_Name";
            this.Seg_D_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_D_TexOffs
            // 
            this.Seg_D_TexOffs.HeaderText = "Texture Offset";
            this.Seg_D_TexOffs.Name = "Seg_D_TexOffs";
            this.Seg_D_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TabPage_Segment_E
            // 
            this.TabPage_Segment_E.Controls.Add(this.Seg_E);
            this.TabPage_Segment_E.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_E.Name = "TabPage_Segment_E";
            this.TabPage_Segment_E.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_E.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_E.TabIndex = 6;
            this.TabPage_Segment_E.Text = "Segment E";
            this.TabPage_Segment_E.UseVisualStyleBackColor = true;
            // 
            // Seg_E
            // 
            this.Seg_E.AllowUserToResizeColumns = false;
            this.Seg_E.AllowUserToResizeRows = false;
            this.Seg_E.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_E.BackgroundColor = System.Drawing.Color.White;
            this.Seg_E.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_E.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_E_Name,
            this.Seg_E_TexOffs});
            this.Seg_E.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_E.Location = new System.Drawing.Point(3, 3);
            this.Seg_E.MultiSelect = false;
            this.Seg_E.Name = "Seg_E";
            this.Seg_E.RowHeadersVisible = false;
            this.Seg_E.Size = new System.Drawing.Size(507, 223);
            this.Seg_E.TabIndex = 11;
            this.Seg_E.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_E.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_E_Name
            // 
            this.Seg_E_Name.FillWeight = 50F;
            this.Seg_E_Name.HeaderText = "Name";
            this.Seg_E_Name.Name = "Seg_E_Name";
            this.Seg_E_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_E_TexOffs
            // 
            this.Seg_E_TexOffs.HeaderText = "Texture Offset";
            this.Seg_E_TexOffs.Name = "Seg_E_TexOffs";
            this.Seg_E_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TabPage_Segment_F
            // 
            this.TabPage_Segment_F.Controls.Add(this.Seg_F);
            this.TabPage_Segment_F.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_F.Name = "TabPage_Segment_F";
            this.TabPage_Segment_F.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_F.Size = new System.Drawing.Size(513, 229);
            this.TabPage_Segment_F.TabIndex = 7;
            this.TabPage_Segment_F.Text = "Segment F";
            this.TabPage_Segment_F.UseVisualStyleBackColor = true;
            // 
            // Seg_F
            // 
            this.Seg_F.AllowUserToResizeColumns = false;
            this.Seg_F.AllowUserToResizeRows = false;
            this.Seg_F.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Seg_F.BackgroundColor = System.Drawing.Color.White;
            this.Seg_F.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Seg_F.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Seg_F_Name,
            this.Seg_F_TexOffs});
            this.Seg_F.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_F.Location = new System.Drawing.Point(3, 3);
            this.Seg_F.MultiSelect = false;
            this.Seg_F.Name = "Seg_F";
            this.Seg_F.RowHeadersVisible = false;
            this.Seg_F.Size = new System.Drawing.Size(507, 223);
            this.Seg_F.TabIndex = 11;
            this.Seg_F.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewTextures_CellParse);
            this.Seg_F.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewTextures_KeyUp);
            // 
            // Seg_F_Name
            // 
            this.Seg_F_Name.FillWeight = 50F;
            this.Seg_F_Name.HeaderText = "Name";
            this.Seg_F_Name.Name = "Seg_F_Name";
            this.Seg_F_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_F_TexOffs
            // 
            this.Seg_F_TexOffs.HeaderText = "Texture Offset";
            this.Seg_F_TexOffs.Name = "Seg_F_TexOffs";
            this.Seg_F_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Panel_HeadRot
            // 
            this.Panel_HeadRot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_HeadRot.Controls.Add(this.ComboBox_HeadLookAxis);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_DegVert);
            this.Panel_HeadRot.Controls.Add(this.Label_HeadLookAxis);
            this.Panel_HeadRot.Controls.Add(this.Label_DegVert);
            this.Panel_HeadRot.Controls.Add(this.Label_Limb);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_DegHoz);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_Limb);
            this.Panel_HeadRot.Controls.Add(this.Label_DegHoz);
            this.Panel_HeadRot.Location = new System.Drawing.Point(877, 66);
            this.Panel_HeadRot.Name = "Panel_HeadRot";
            this.Panel_HeadRot.Size = new System.Drawing.Size(200, 133);
            this.Panel_HeadRot.TabIndex = 28;
            // 
            // ComboBox_HeadLookAxis
            // 
            this.ComboBox_HeadLookAxis.FormattingEnabled = true;
            this.ComboBox_HeadLookAxis.Items.AddRange(new object[] {
            "-X+Z",
            "-X-Y",
            "+Y+Z",
            "-Y+X"});
            this.ComboBox_HeadLookAxis.Location = new System.Drawing.Point(16, 23);
            this.ComboBox_HeadLookAxis.Name = "ComboBox_HeadLookAxis";
            this.ComboBox_HeadLookAxis.Size = new System.Drawing.Size(168, 21);
            this.ComboBox_HeadLookAxis.TabIndex = 21;
            this.ComboBox_HeadLookAxis.Tag = "AXIS";
            this.ComboBox_HeadLookAxis.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // NumUpDown_DegVert
            // 
            this.NumUpDown_DegVert.Location = new System.Drawing.Point(125, 103);
            this.NumUpDown_DegVert.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.NumUpDown_DegVert.Name = "NumUpDown_DegVert";
            this.NumUpDown_DegVert.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_DegVert.TabIndex = 27;
            this.NumUpDown_DegVert.Tag = "DEGVERT";
            this.NumUpDown_DegVert.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_HeadLookAxis
            // 
            this.Label_HeadLookAxis.AutoSize = true;
            this.Label_HeadLookAxis.Location = new System.Drawing.Point(11, 7);
            this.Label_HeadLookAxis.Name = "Label_HeadLookAxis";
            this.Label_HeadLookAxis.Size = new System.Drawing.Size(80, 13);
            this.Label_HeadLookAxis.TabIndex = 20;
            this.Label_HeadLookAxis.Text = "Head look axis:";
            // 
            // Label_DegVert
            // 
            this.Label_DegVert.AutoSize = true;
            this.Label_DegVert.Location = new System.Drawing.Point(14, 105);
            this.Label_DegVert.Name = "Label_DegVert";
            this.Label_DegVert.Size = new System.Drawing.Size(94, 13);
            this.Label_DegVert.TabIndex = 26;
            this.Label_DegVert.Text = "Degrees vertically:";
            // 
            // Label_Limb
            // 
            this.Label_Limb.AutoSize = true;
            this.Label_Limb.Location = new System.Drawing.Point(14, 53);
            this.Label_Limb.Name = "Label_Limb";
            this.Label_Limb.Size = new System.Drawing.Size(32, 13);
            this.Label_Limb.TabIndex = 22;
            this.Label_Limb.Text = "Limb:";
            // 
            // NumUpDown_DegHoz
            // 
            this.NumUpDown_DegHoz.Location = new System.Drawing.Point(125, 77);
            this.NumUpDown_DegHoz.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.NumUpDown_DegHoz.Name = "NumUpDown_DegHoz";
            this.NumUpDown_DegHoz.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_DegHoz.TabIndex = 25;
            this.NumUpDown_DegHoz.Tag = "DEGHOZ";
            this.NumUpDown_DegHoz.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_Limb
            // 
            this.NumUpDown_Limb.Location = new System.Drawing.Point(125, 51);
            this.NumUpDown_Limb.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_Limb.Name = "NumUpDown_Limb";
            this.NumUpDown_Limb.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_Limb.TabIndex = 23;
            this.NumUpDown_Limb.Tag = "LIMB";
            this.NumUpDown_Limb.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_DegHoz
            // 
            this.Label_DegHoz.AutoSize = true;
            this.Label_DegHoz.Location = new System.Drawing.Point(14, 79);
            this.Label_DegHoz.Name = "Label_DegHoz";
            this.Label_DegHoz.Size = new System.Drawing.Size(105, 13);
            this.Label_DegHoz.TabIndex = 24;
            this.Label_DegHoz.Text = "Degrees horizontally:";
            // 
            // Panel_TargetPanel
            // 
            this.Panel_TargetPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_ZTargetOffs);
            this.Panel_TargetPanel.Controls.Add(this.Label_TargetLimb);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_YTargetOffs);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_TargetLimb);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_XTargetOffs);
            this.Panel_TargetPanel.Controls.Add(this.Label_TargetOffset);
            this.Panel_TargetPanel.Location = new System.Drawing.Point(1088, 285);
            this.Panel_TargetPanel.Name = "Panel_TargetPanel";
            this.Panel_TargetPanel.Size = new System.Drawing.Size(200, 80);
            this.Panel_TargetPanel.TabIndex = 40;
            // 
            // NumUpDown_ZTargetOffs
            // 
            this.NumUpDown_ZTargetOffs.Location = new System.Drawing.Point(132, 50);
            this.NumUpDown_ZTargetOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_ZTargetOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_ZTargetOffs.Name = "NumUpDown_ZTargetOffs";
            this.NumUpDown_ZTargetOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_ZTargetOffs.TabIndex = 38;
            this.NumUpDown_ZTargetOffs.Tag = "ZTARGETOFFS";
            this.NumUpDown_ZTargetOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_TargetLimb
            // 
            this.Label_TargetLimb.AutoSize = true;
            this.Label_TargetLimb.Location = new System.Drawing.Point(11, 11);
            this.Label_TargetLimb.Name = "Label_TargetLimb";
            this.Label_TargetLimb.Size = new System.Drawing.Size(62, 13);
            this.Label_TargetLimb.TabIndex = 28;
            this.Label_TargetLimb.Text = "Target limb:";
            // 
            // NumUpDown_YTargetOffs
            // 
            this.NumUpDown_YTargetOffs.Location = new System.Drawing.Point(72, 50);
            this.NumUpDown_YTargetOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_YTargetOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_YTargetOffs.Name = "NumUpDown_YTargetOffs";
            this.NumUpDown_YTargetOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_YTargetOffs.TabIndex = 37;
            this.NumUpDown_YTargetOffs.Tag = "YTARGETOFFS";
            this.NumUpDown_YTargetOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_TargetLimb
            // 
            this.NumUpDown_TargetLimb.Location = new System.Drawing.Point(122, 9);
            this.NumUpDown_TargetLimb.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_TargetLimb.Name = "NumUpDown_TargetLimb";
            this.NumUpDown_TargetLimb.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_TargetLimb.TabIndex = 28;
            this.NumUpDown_TargetLimb.Tag = "TARGETLIMB";
            this.NumUpDown_TargetLimb.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_XTargetOffs
            // 
            this.NumUpDown_XTargetOffs.Location = new System.Drawing.Point(12, 50);
            this.NumUpDown_XTargetOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_XTargetOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_XTargetOffs.Name = "NumUpDown_XTargetOffs";
            this.NumUpDown_XTargetOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_XTargetOffs.TabIndex = 36;
            this.NumUpDown_XTargetOffs.Tag = "XTARGETOFFS";
            this.NumUpDown_XTargetOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_TargetOffset
            // 
            this.Label_TargetOffset.AutoSize = true;
            this.Label_TargetOffset.Location = new System.Drawing.Point(11, 35);
            this.Label_TargetOffset.Name = "Label_TargetOffset";
            this.Label_TargetOffset.Size = new System.Drawing.Size(38, 13);
            this.Label_TargetOffset.TabIndex = 35;
            this.Label_TargetOffset.Text = "Offset:";
            // 
            // Checkbox_Targettable
            // 
            this.Checkbox_Targettable.AutoSize = true;
            this.Checkbox_Targettable.Location = new System.Drawing.Point(1088, 263);
            this.Checkbox_Targettable.Name = "Checkbox_Targettable";
            this.Checkbox_Targettable.Size = new System.Drawing.Size(80, 17);
            this.Checkbox_Targettable.TabIndex = 39;
            this.Checkbox_Targettable.Tag = "TARGETTABLE";
            this.Checkbox_Targettable.Text = "Targettable";
            this.Checkbox_Targettable.UseVisualStyleBackColor = true;
            this.Checkbox_Targettable.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUpDown_ObjectID
            // 
            this.NumUpDown_ObjectID.Location = new System.Drawing.Point(113, 34);
            this.NumUpDown_ObjectID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_ObjectID.Name = "NumUpDown_ObjectID";
            this.NumUpDown_ObjectID.Size = new System.Drawing.Size(218, 20);
            this.NumUpDown_ObjectID.TabIndex = 5;
            this.NumUpDown_ObjectID.Tag = "OBJID";
            this.NumUpDown_ObjectID.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Panel_Movement
            // 
            this.Panel_Movement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Movement.Controls.Add(this.Label_LoopDelay);
            this.Panel_Movement.Controls.Add(this.Label_LoopStartNode);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopStartNode);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopDelay);
            this.Panel_Movement.Controls.Add(this.Label_LoopEndNode);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopEndNode);
            this.Panel_Movement.Controls.Add(this.Checkbox_Loop);
            this.Panel_Movement.Controls.Add(this.Label_PathFollowID);
            this.Panel_Movement.Controls.Add(this.NumUpDown_PathFollowID);
            this.Panel_Movement.Controls.Add(this.NumUpDown_MovDistance);
            this.Panel_Movement.Controls.Add(this.NumUpDown_MovSpeed);
            this.Panel_Movement.Controls.Add(this.Label_Distance);
            this.Panel_Movement.Controls.Add(this.Label_Speed);
            this.Panel_Movement.Location = new System.Drawing.Point(1088, 66);
            this.Panel_Movement.Name = "Panel_Movement";
            this.Panel_Movement.Size = new System.Drawing.Size(200, 186);
            this.Panel_Movement.TabIndex = 38;
            // 
            // Label_LoopDelay
            // 
            this.Label_LoopDelay.AutoSize = true;
            this.Label_LoopDelay.Location = new System.Drawing.Point(14, 57);
            this.Label_LoopDelay.Name = "Label_LoopDelay";
            this.Label_LoopDelay.Size = new System.Drawing.Size(81, 13);
            this.Label_LoopDelay.TabIndex = 47;
            this.Label_LoopDelay.Text = "Delay between:";
            // 
            // Label_LoopStartNode
            // 
            this.Label_LoopStartNode.AutoSize = true;
            this.Label_LoopStartNode.Location = new System.Drawing.Point(14, 109);
            this.Label_LoopStartNode.Name = "Label_LoopStartNode";
            this.Label_LoopStartNode.Size = new System.Drawing.Size(84, 13);
            this.Label_LoopStartNode.TabIndex = 45;
            this.Label_LoopStartNode.Text = "Loop start node:";
            // 
            // NumUpDown_LoopStartNode
            // 
            this.NumUpDown_LoopStartNode.Location = new System.Drawing.Point(120, 107);
            this.NumUpDown_LoopStartNode.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_LoopStartNode.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_LoopStartNode.Name = "NumUpDown_LoopStartNode";
            this.NumUpDown_LoopStartNode.Size = new System.Drawing.Size(65, 20);
            this.NumUpDown_LoopStartNode.TabIndex = 44;
            this.NumUpDown_LoopStartNode.Tag = "LOOPSTART";
            this.NumUpDown_LoopStartNode.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_LoopStartNode.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_LoopDelay
            // 
            this.NumUpDown_LoopDelay.Location = new System.Drawing.Point(120, 55);
            this.NumUpDown_LoopDelay.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_LoopDelay.Name = "NumUpDown_LoopDelay";
            this.NumUpDown_LoopDelay.Size = new System.Drawing.Size(65, 20);
            this.NumUpDown_LoopDelay.TabIndex = 46;
            this.NumUpDown_LoopDelay.Tag = "LOOPDEL";
            this.NumUpDown_LoopDelay.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_LoopEndNode
            // 
            this.Label_LoopEndNode.AutoSize = true;
            this.Label_LoopEndNode.Location = new System.Drawing.Point(14, 135);
            this.Label_LoopEndNode.Name = "Label_LoopEndNode";
            this.Label_LoopEndNode.Size = new System.Drawing.Size(82, 13);
            this.Label_LoopEndNode.TabIndex = 43;
            this.Label_LoopEndNode.Text = "Loop end node:";
            // 
            // NumUpDown_LoopEndNode
            // 
            this.NumUpDown_LoopEndNode.Location = new System.Drawing.Point(120, 133);
            this.NumUpDown_LoopEndNode.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_LoopEndNode.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_LoopEndNode.Name = "NumUpDown_LoopEndNode";
            this.NumUpDown_LoopEndNode.Size = new System.Drawing.Size(64, 20);
            this.NumUpDown_LoopEndNode.TabIndex = 42;
            this.NumUpDown_LoopEndNode.Tag = "LOOPEND";
            this.NumUpDown_LoopEndNode.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_LoopEndNode.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Checkbox_Loop
            // 
            this.Checkbox_Loop.AutoSize = true;
            this.Checkbox_Loop.Location = new System.Drawing.Point(134, 159);
            this.Checkbox_Loop.Name = "Checkbox_Loop";
            this.Checkbox_Loop.Size = new System.Drawing.Size(50, 17);
            this.Checkbox_Loop.TabIndex = 41;
            this.Checkbox_Loop.Tag = "LOOP";
            this.Checkbox_Loop.Text = "Loop";
            this.Checkbox_Loop.UseVisualStyleBackColor = true;
            this.Checkbox_Loop.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Label_PathFollowID
            // 
            this.Label_PathFollowID.AutoSize = true;
            this.Label_PathFollowID.Location = new System.Drawing.Point(13, 83);
            this.Label_PathFollowID.Name = "Label_PathFollowID";
            this.Label_PathFollowID.Size = new System.Drawing.Size(76, 13);
            this.Label_PathFollowID.TabIndex = 39;
            this.Label_PathFollowID.Text = "Path follow ID:";
            // 
            // NumUpDown_PathFollowID
            // 
            this.NumUpDown_PathFollowID.Location = new System.Drawing.Point(120, 81);
            this.NumUpDown_PathFollowID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_PathFollowID.Name = "NumUpDown_PathFollowID";
            this.NumUpDown_PathFollowID.Size = new System.Drawing.Size(65, 20);
            this.NumUpDown_PathFollowID.TabIndex = 38;
            this.NumUpDown_PathFollowID.Tag = "PATHID";
            this.NumUpDown_PathFollowID.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_MovDistance
            // 
            this.NumUpDown_MovDistance.Location = new System.Drawing.Point(120, 3);
            this.NumUpDown_MovDistance.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_MovDistance.Name = "NumUpDown_MovDistance";
            this.NumUpDown_MovDistance.Size = new System.Drawing.Size(65, 20);
            this.NumUpDown_MovDistance.TabIndex = 35;
            this.NumUpDown_MovDistance.Tag = "MOVDISTANCE";
            this.NumUpDown_MovDistance.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_MovSpeed
            // 
            this.NumUpDown_MovSpeed.DecimalPlaces = 2;
            this.NumUpDown_MovSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumUpDown_MovSpeed.Location = new System.Drawing.Point(120, 29);
            this.NumUpDown_MovSpeed.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_MovSpeed.Name = "NumUpDown_MovSpeed";
            this.NumUpDown_MovSpeed.Size = new System.Drawing.Size(65, 20);
            this.NumUpDown_MovSpeed.TabIndex = 37;
            this.NumUpDown_MovSpeed.Tag = "MOVSPEED";
            this.NumUpDown_MovSpeed.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_Distance
            // 
            this.Label_Distance.AutoSize = true;
            this.Label_Distance.Location = new System.Drawing.Point(14, 5);
            this.Label_Distance.Name = "Label_Distance";
            this.Label_Distance.Size = new System.Drawing.Size(103, 13);
            this.Label_Distance.TabIndex = 35;
            this.Label_Distance.Text = "Movement distance:";
            // 
            // Label_Speed
            // 
            this.Label_Speed.AutoSize = true;
            this.Label_Speed.Location = new System.Drawing.Point(14, 31);
            this.Label_Speed.Name = "Label_Speed";
            this.Label_Speed.Size = new System.Drawing.Size(92, 13);
            this.Label_Speed.TabIndex = 36;
            this.Label_Speed.Text = "Movement speed:";
            // 
            // Label_ObjectID
            // 
            this.Label_ObjectID.AutoSize = true;
            this.Label_ObjectID.Location = new System.Drawing.Point(11, 36);
            this.Label_ObjectID.Name = "Label_ObjectID";
            this.Label_ObjectID.Size = new System.Drawing.Size(55, 13);
            this.Label_ObjectID.TabIndex = 6;
            this.Label_ObjectID.Text = "Object ID:";
            // 
            // Combo_MovementType
            // 
            this.Combo_MovementType.FormattingEnabled = true;
            this.Combo_MovementType.Items.AddRange(new object[] {
            "None",
            "Walks randomly",
            "Follows Link",
            "Follow a path, collisionwise",
            "Follow a path, direct"});
            this.Combo_MovementType.Location = new System.Drawing.Point(1105, 36);
            this.Combo_MovementType.Name = "Combo_MovementType";
            this.Combo_MovementType.Size = new System.Drawing.Size(168, 21);
            this.Combo_MovementType.TabIndex = 26;
            this.Combo_MovementType.Tag = "MOVEMENT";
            this.Combo_MovementType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // NumUpDown_ZModelOffs
            // 
            this.NumUpDown_ZModelOffs.Location = new System.Drawing.Point(277, 115);
            this.NumUpDown_ZModelOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_ZModelOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_ZModelOffs.Name = "NumUpDown_ZModelOffs";
            this.NumUpDown_ZModelOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_ZModelOffs.TabIndex = 38;
            this.NumUpDown_ZModelOffs.Tag = "ZMODELOFFS";
            this.NumUpDown_ZModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_MovementType
            // 
            this.Label_MovementType.AutoSize = true;
            this.Label_MovementType.Location = new System.Drawing.Point(1102, 15);
            this.Label_MovementType.Name = "Label_MovementType";
            this.Label_MovementType.Size = new System.Drawing.Size(83, 13);
            this.Label_MovementType.TabIndex = 25;
            this.Label_MovementType.Text = "Movement type:";
            // 
            // Label_Hierarchy
            // 
            this.Label_Hierarchy.AutoSize = true;
            this.Label_Hierarchy.Location = new System.Drawing.Point(11, 64);
            this.Label_Hierarchy.Name = "Label_Hierarchy";
            this.Label_Hierarchy.Size = new System.Drawing.Size(55, 13);
            this.Label_Hierarchy.TabIndex = 7;
            this.Label_Hierarchy.Text = "Hierarchy:";
            // 
            // Checkbox_Pushable
            // 
            this.Checkbox_Pushable.AutoSize = true;
            this.Checkbox_Pushable.Location = new System.Drawing.Point(983, 230);
            this.Checkbox_Pushable.Name = "Checkbox_Pushable";
            this.Checkbox_Pushable.Size = new System.Drawing.Size(70, 17);
            this.Checkbox_Pushable.TabIndex = 24;
            this.Checkbox_Pushable.Tag = "PICKUPPABLE";
            this.Checkbox_Pushable.Text = "Pushable";
            this.Checkbox_Pushable.UseVisualStyleBackColor = true;
            this.Checkbox_Pushable.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUpDown_Hierarchy
            // 
            this.NumUpDown_Hierarchy.Hexadecimal = true;
            this.NumUpDown_Hierarchy.Location = new System.Drawing.Point(113, 62);
            this.NumUpDown_Hierarchy.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.NumUpDown_Hierarchy.Name = "NumUpDown_Hierarchy";
            this.NumUpDown_Hierarchy.Size = new System.Drawing.Size(218, 20);
            this.NumUpDown_Hierarchy.TabIndex = 8;
            this.NumUpDown_Hierarchy.Tag = "HIERARCHY";
            this.NumUpDown_Hierarchy.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Checkbox_CanPressSwitches
            // 
            this.Checkbox_CanPressSwitches.AutoSize = true;
            this.Checkbox_CanPressSwitches.Location = new System.Drawing.Point(877, 231);
            this.Checkbox_CanPressSwitches.Name = "Checkbox_CanPressSwitches";
            this.Checkbox_CanPressSwitches.Size = new System.Drawing.Size(107, 17);
            this.Checkbox_CanPressSwitches.TabIndex = 23;
            this.Checkbox_CanPressSwitches.Tag = "SWITCHES";
            this.Checkbox_CanPressSwitches.Text = "Presses switches";
            this.Checkbox_CanPressSwitches.UseVisualStyleBackColor = true;
            this.Checkbox_CanPressSwitches.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUpDown_YModelOffs
            // 
            this.NumUpDown_YModelOffs.Location = new System.Drawing.Point(217, 115);
            this.NumUpDown_YModelOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_YModelOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_YModelOffs.Name = "NumUpDown_YModelOffs";
            this.NumUpDown_YModelOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_YModelOffs.TabIndex = 37;
            this.NumUpDown_YModelOffs.Tag = "YMODELOFFS";
            this.NumUpDown_YModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Checkbox_DrawShadow
            // 
            this.Checkbox_DrawShadow.AutoSize = true;
            this.Checkbox_DrawShadow.Location = new System.Drawing.Point(983, 205);
            this.Checkbox_DrawShadow.Name = "Checkbox_DrawShadow";
            this.Checkbox_DrawShadow.Size = new System.Drawing.Size(96, 17);
            this.Checkbox_DrawShadow.TabIndex = 22;
            this.Checkbox_DrawShadow.Tag = "SHADOW";
            this.Checkbox_DrawShadow.Text = "Draws shadow";
            this.Checkbox_DrawShadow.UseVisualStyleBackColor = true;
            this.Checkbox_DrawShadow.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // DataGrid_Animations
            // 
            this.DataGrid_Animations.AllowUserToResizeColumns = false;
            this.DataGrid_Animations.AllowUserToResizeRows = false;
            this.DataGrid_Animations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DataGrid_Animations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGrid_Animations.BackgroundColor = System.Drawing.Color.White;
            this.DataGrid_Animations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_Animations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_AnimName,
            this.Col_Anim,
            this.Col_Speed,
            this.Col_OBJ});
            this.DataGrid_Animations.Location = new System.Drawing.Point(14, 165);
            this.DataGrid_Animations.MultiSelect = false;
            this.DataGrid_Animations.Name = "DataGrid_Animations";
            this.DataGrid_Animations.RowHeadersVisible = false;
            this.DataGrid_Animations.Size = new System.Drawing.Size(317, 552);
            this.DataGrid_Animations.TabIndex = 9;
            this.DataGrid_Animations.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewAnimations_CellParse);
            this.DataGrid_Animations.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGrid_Animations_KeyUp);
            // 
            // Col_AnimName
            // 
            this.Col_AnimName.FillWeight = 50F;
            this.Col_AnimName.HeaderText = "Purpose";
            this.Col_AnimName.Name = "Col_AnimName";
            this.Col_AnimName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_Anim
            // 
            this.Col_Anim.HeaderText = "Animation offset";
            this.Col_Anim.Name = "Col_Anim";
            this.Col_Anim.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_Speed
            // 
            this.Col_Speed.FillWeight = 40F;
            this.Col_Speed.HeaderText = "Speed";
            this.Col_Speed.Name = "Col_Speed";
            this.Col_Speed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_OBJ
            // 
            this.Col_OBJ.FillWeight = 40F;
            this.Col_OBJ.HeaderText = "Object";
            this.Col_OBJ.Name = "Col_OBJ";
            this.Col_OBJ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Checkbox_HaveCollision
            // 
            this.Checkbox_HaveCollision.AutoSize = true;
            this.Checkbox_HaveCollision.Location = new System.Drawing.Point(877, 205);
            this.Checkbox_HaveCollision.Name = "Checkbox_HaveCollision";
            this.Checkbox_HaveCollision.Size = new System.Drawing.Size(85, 17);
            this.Checkbox_HaveCollision.TabIndex = 21;
            this.Checkbox_HaveCollision.Tag = "COLLISION";
            this.Checkbox_HaveCollision.Text = "Has collision";
            this.Checkbox_HaveCollision.UseVisualStyleBackColor = true;
            this.Checkbox_HaveCollision.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Label_AnimDefs
            // 
            this.Label_AnimDefs.AutoSize = true;
            this.Label_AnimDefs.Location = new System.Drawing.Point(14, 151);
            this.Label_AnimDefs.Name = "Label_AnimDefs";
            this.Label_AnimDefs.Size = new System.Drawing.Size(106, 13);
            this.Label_AnimDefs.TabIndex = 10;
            this.Label_AnimDefs.Text = "Animation definitions:";
            // 
            // Panel_Collision
            // 
            this.Panel_Collision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Collision.Controls.Add(this.NumUpDown_ZColOffs);
            this.Panel_Collision.Controls.Add(this.NumUpDown_YColOffs);
            this.Panel_Collision.Controls.Add(this.NumUpDown_XColOffs);
            this.Panel_Collision.Controls.Add(this.Label_Offset);
            this.Panel_Collision.Controls.Add(this.Label_Height);
            this.Panel_Collision.Controls.Add(this.NumUpDown_ColHeight);
            this.Panel_Collision.Controls.Add(this.NumUpDown_ColRadius);
            this.Panel_Collision.Controls.Add(this.Label_Radius);
            this.Panel_Collision.Controls.Add(this.Label_Collision);
            this.Panel_Collision.Location = new System.Drawing.Point(877, 257);
            this.Panel_Collision.Name = "Panel_Collision";
            this.Panel_Collision.Size = new System.Drawing.Size(200, 133);
            this.Panel_Collision.TabIndex = 20;
            // 
            // NumUpDown_ZColOffs
            // 
            this.NumUpDown_ZColOffs.Location = new System.Drawing.Point(132, 103);
            this.NumUpDown_ZColOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_ZColOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_ZColOffs.Name = "NumUpDown_ZColOffs";
            this.NumUpDown_ZColOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_ZColOffs.TabIndex = 34;
            this.NumUpDown_ZColOffs.Tag = "ZCOLOFFS";
            this.NumUpDown_ZColOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_YColOffs
            // 
            this.NumUpDown_YColOffs.Location = new System.Drawing.Point(72, 103);
            this.NumUpDown_YColOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_YColOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_YColOffs.Name = "NumUpDown_YColOffs";
            this.NumUpDown_YColOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_YColOffs.TabIndex = 33;
            this.NumUpDown_YColOffs.Tag = "YCOLOFFS";
            this.NumUpDown_YColOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_XColOffs
            // 
            this.NumUpDown_XColOffs.Location = new System.Drawing.Point(12, 103);
            this.NumUpDown_XColOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_XColOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_XColOffs.Name = "NumUpDown_XColOffs";
            this.NumUpDown_XColOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_XColOffs.TabIndex = 32;
            this.NumUpDown_XColOffs.Tag = "XCOLOFFS";
            this.NumUpDown_XColOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_Offset
            // 
            this.Label_Offset.AutoSize = true;
            this.Label_Offset.Location = new System.Drawing.Point(11, 88);
            this.Label_Offset.Name = "Label_Offset";
            this.Label_Offset.Size = new System.Drawing.Size(38, 13);
            this.Label_Offset.TabIndex = 31;
            this.Label_Offset.Text = "Offset:";
            // 
            // Label_Height
            // 
            this.Label_Height.AutoSize = true;
            this.Label_Height.Location = new System.Drawing.Point(11, 60);
            this.Label_Height.Name = "Label_Height";
            this.Label_Height.Size = new System.Drawing.Size(41, 13);
            this.Label_Height.TabIndex = 30;
            this.Label_Height.Text = "Height:";
            // 
            // NumUpDown_ColHeight
            // 
            this.NumUpDown_ColHeight.Location = new System.Drawing.Point(122, 58);
            this.NumUpDown_ColHeight.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_ColHeight.Name = "NumUpDown_ColHeight";
            this.NumUpDown_ColHeight.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_ColHeight.TabIndex = 29;
            this.NumUpDown_ColHeight.Tag = "COLHEIGHT";
            this.NumUpDown_ColHeight.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_ColRadius
            // 
            this.NumUpDown_ColRadius.Location = new System.Drawing.Point(122, 32);
            this.NumUpDown_ColRadius.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_ColRadius.Name = "NumUpDown_ColRadius";
            this.NumUpDown_ColRadius.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_ColRadius.TabIndex = 28;
            this.NumUpDown_ColRadius.Tag = "COLRADIUS";
            this.NumUpDown_ColRadius.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_Radius
            // 
            this.Label_Radius.AutoSize = true;
            this.Label_Radius.Location = new System.Drawing.Point(11, 34);
            this.Label_Radius.Name = "Label_Radius";
            this.Label_Radius.Size = new System.Drawing.Size(43, 13);
            this.Label_Radius.TabIndex = 28;
            this.Label_Radius.Text = "Radius:";
            // 
            // Label_Collision
            // 
            this.Label_Collision.AutoSize = true;
            this.Label_Collision.Location = new System.Drawing.Point(11, 9);
            this.Label_Collision.Name = "Label_Collision";
            this.Label_Collision.Size = new System.Drawing.Size(48, 13);
            this.Label_Collision.TabIndex = 28;
            this.Label_Collision.Text = "Collision:";
            // 
            // NumUpDown_XModelOffs
            // 
            this.NumUpDown_XModelOffs.Location = new System.Drawing.Point(157, 115);
            this.NumUpDown_XModelOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_XModelOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_XModelOffs.Name = "NumUpDown_XModelOffs";
            this.NumUpDown_XModelOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_XModelOffs.TabIndex = 36;
            this.NumUpDown_XModelOffs.Tag = "XMODELOFFFS";
            this.NumUpDown_XModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_LookAtType
            // 
            this.Label_LookAtType.AutoSize = true;
            this.Label_LookAtType.Location = new System.Drawing.Point(891, 15);
            this.Label_LookAtType.Name = "Label_LookAtType";
            this.Label_LookAtType.Size = new System.Drawing.Size(92, 13);
            this.Label_LookAtType.TabIndex = 19;
            this.Label_LookAtType.Text = "Look at Link type:";
            // 
            // ComboBox_LookAtType
            // 
            this.ComboBox_LookAtType.FormattingEnabled = true;
            this.ComboBox_LookAtType.Items.AddRange(new object[] {
            "None",
            "Body",
            "Head"});
            this.ComboBox_LookAtType.Location = new System.Drawing.Point(894, 37);
            this.ComboBox_LookAtType.Name = "ComboBox_LookAtType";
            this.ComboBox_LookAtType.Size = new System.Drawing.Size(168, 21);
            this.ComboBox_LookAtType.TabIndex = 18;
            this.ComboBox_LookAtType.Tag = "LOOKATTYPE";
            this.ComboBox_LookAtType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // ComboBox_HierarchyType
            // 
            this.ComboBox_HierarchyType.FormattingEnabled = true;
            this.ComboBox_HierarchyType.Items.AddRange(new object[] {
            "Matrix (Link, etc.)",
            "Non-matrix (Hylian guards, etc.)",
            "Weighted (Horses)"});
            this.ComboBox_HierarchyType.Location = new System.Drawing.Point(113, 88);
            this.ComboBox_HierarchyType.Name = "ComboBox_HierarchyType";
            this.ComboBox_HierarchyType.Size = new System.Drawing.Size(218, 21);
            this.ComboBox_HierarchyType.TabIndex = 11;
            this.ComboBox_HierarchyType.Tag = "HIERARCHYTYPE";
            this.ComboBox_HierarchyType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_ModelDrawOffs
            // 
            this.Label_ModelDrawOffs.AutoSize = true;
            this.Label_ModelDrawOffs.Location = new System.Drawing.Point(12, 117);
            this.Label_ModelDrawOffs.Name = "Label_ModelDrawOffs";
            this.Label_ModelDrawOffs.Size = new System.Drawing.Size(94, 13);
            this.Label_ModelDrawOffs.TabIndex = 35;
            this.Label_ModelDrawOffs.Text = "Model draw offset:";
            // 
            // Label_HierarchyType
            // 
            this.Label_HierarchyType.AutoSize = true;
            this.Label_HierarchyType.Location = new System.Drawing.Point(12, 91);
            this.Label_HierarchyType.Name = "Label_HierarchyType";
            this.Label_HierarchyType.Size = new System.Drawing.Size(78, 13);
            this.Label_HierarchyType.TabIndex = 12;
            this.Label_HierarchyType.Text = "Hierarchy type:";
            // 
            // ComboBox_AnimType
            // 
            this.ComboBox_AnimType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ComboBox_AnimType.FormattingEnabled = true;
            this.ComboBox_AnimType.Items.AddRange(new object[] {
            "Standard",
            "Link"});
            this.ComboBox_AnimType.Location = new System.Drawing.Point(242, 723);
            this.ComboBox_AnimType.Name = "ComboBox_AnimType";
            this.ComboBox_AnimType.Size = new System.Drawing.Size(89, 21);
            this.ComboBox_AnimType.TabIndex = 13;
            this.ComboBox_AnimType.Tag = "ANIMTYPE";
            this.ComboBox_AnimType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_AnimType
            // 
            this.Label_AnimType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Label_AnimType.AutoSize = true;
            this.Label_AnimType.Location = new System.Drawing.Point(154, 726);
            this.Label_AnimType.Name = "Label_AnimType";
            this.Label_AnimType.Size = new System.Drawing.Size(79, 13);
            this.Label_AnimType.TabIndex = 14;
            this.Label_AnimType.Text = "Animation type:";
            // 
            // Label_Scale
            // 
            this.Label_Scale.AutoSize = true;
            this.Label_Scale.Location = new System.Drawing.Point(207, 143);
            this.Label_Scale.Name = "Label_Scale";
            this.Label_Scale.Size = new System.Drawing.Size(37, 13);
            this.Label_Scale.TabIndex = 16;
            this.Label_Scale.Text = "Scale:";
            // 
            // NumUpDown_Scale
            // 
            this.NumUpDown_Scale.DecimalPlaces = 4;
            this.NumUpDown_Scale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumUpDown_Scale.Location = new System.Drawing.Point(254, 140);
            this.NumUpDown_Scale.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_Scale.Name = "NumUpDown_Scale";
            this.NumUpDown_Scale.Size = new System.Drawing.Size(77, 20);
            this.NumUpDown_Scale.TabIndex = 17;
            this.NumUpDown_Scale.Tag = "SCALE";
            this.NumUpDown_Scale.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumUpDown_Scale.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Tab2_Script
            // 
            this.Tab2_Script.Controls.Add(this.Textbox_Script);
            this.Tab2_Script.Controls.Add(this.Button_TryParse);
            this.Tab2_Script.Controls.Add(this.Textbox_ParseErrors);
            this.Tab2_Script.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tab2_Script.Location = new System.Drawing.Point(4, 22);
            this.Tab2_Script.Name = "Tab2_Script";
            this.Tab2_Script.Padding = new System.Windows.Forms.Padding(3);
            this.Tab2_Script.Size = new System.Drawing.Size(1295, 762);
            this.Tab2_Script.TabIndex = 1;
            this.Tab2_Script.Text = "Interaction script";
            this.Tab2_Script.UseVisualStyleBackColor = true;
            // 
            // Textbox_Script
            // 
            this.Textbox_Script.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.Textbox_Script.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.Textbox_Script.BackBrush = null;
            this.Textbox_Script.CharHeight = 14;
            this.Textbox_Script.CharWidth = 8;
            this.Textbox_Script.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Textbox_Script.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Textbox_Script.IsReplaceMode = false;
            this.Textbox_Script.Location = new System.Drawing.Point(3, 3);
            this.Textbox_Script.Name = "Textbox_Script";
            this.Textbox_Script.Paddings = new System.Windows.Forms.Padding(0);
            this.Textbox_Script.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Textbox_Script.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("Textbox_Script.ServiceColors")));
            this.Textbox_Script.Size = new System.Drawing.Size(757, 615);
            this.Textbox_Script.TabIndex = 3;
            this.Textbox_Script.WordWrapAutoIndent = false;
            this.Textbox_Script.Zoom = 100;
            this.Textbox_Script.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.Textbox_Script_TextChanged);
            // 
            // Button_TryParse
            // 
            this.Button_TryParse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_TryParse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Button_TryParse.Location = new System.Drawing.Point(9, 624);
            this.Button_TryParse.Name = "Button_TryParse";
            this.Button_TryParse.Size = new System.Drawing.Size(751, 30);
            this.Button_TryParse.TabIndex = 2;
            this.Button_TryParse.Text = "Try parsing";
            this.Button_TryParse.UseVisualStyleBackColor = true;
            this.Button_TryParse.Click += new System.EventHandler(this.Button_TryParse_Click);
            // 
            // Textbox_ParseErrors
            // 
            this.Textbox_ParseErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Textbox_ParseErrors.Location = new System.Drawing.Point(4, 660);
            this.Textbox_ParseErrors.Multiline = true;
            this.Textbox_ParseErrors.Name = "Textbox_ParseErrors";
            this.Textbox_ParseErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Textbox_ParseErrors.Size = new System.Drawing.Size(762, 96);
            this.Textbox_ParseErrors.TabIndex = 1;
            // 
            // Panel_NPCList
            // 
            this.Panel_NPCList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Panel_NPCList.Controls.Add(this.Button_PasteBase);
            this.Panel_NPCList.Controls.Add(this.Button_CopyBase);
            this.Panel_NPCList.Controls.Add(this.Button_Duplicate);
            this.Panel_NPCList.Controls.Add(this.Button_Delete);
            this.Panel_NPCList.Controls.Add(this.Button_Add);
            this.Panel_NPCList.Controls.Add(this.DataGrid_NPCs);
            this.Panel_NPCList.Location = new System.Drawing.Point(0, 3);
            this.Panel_NPCList.Name = "Panel_NPCList";
            this.Panel_NPCList.Size = new System.Drawing.Size(244, 788);
            this.Panel_NPCList.TabIndex = 5;
            // 
            // Button_PasteBase
            // 
            this.Button_PasteBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_PasteBase.Location = new System.Drawing.Point(125, 751);
            this.Button_PasteBase.Name = "Button_PasteBase";
            this.Button_PasteBase.Size = new System.Drawing.Size(109, 31);
            this.Button_PasteBase.TabIndex = 7;
            this.Button_PasteBase.Text = "Paste model info";
            this.Button_PasteBase.UseVisualStyleBackColor = true;
            this.Button_PasteBase.Click += new System.EventHandler(this.Button_PasteBase_Click);
            // 
            // Button_CopyBase
            // 
            this.Button_CopyBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_CopyBase.Location = new System.Drawing.Point(10, 751);
            this.Button_CopyBase.Name = "Button_CopyBase";
            this.Button_CopyBase.Size = new System.Drawing.Size(109, 31);
            this.Button_CopyBase.TabIndex = 6;
            this.Button_CopyBase.Text = "Copy model info";
            this.Button_CopyBase.UseVisualStyleBackColor = true;
            this.Button_CopyBase.Click += new System.EventHandler(this.Button_CopyBase_Click);
            // 
            // Button_Duplicate
            // 
            this.Button_Duplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Duplicate.Location = new System.Drawing.Point(87, 714);
            this.Button_Duplicate.Name = "Button_Duplicate";
            this.Button_Duplicate.Size = new System.Drawing.Size(73, 31);
            this.Button_Duplicate.TabIndex = 5;
            this.Button_Duplicate.Text = "Duplicate";
            this.Button_Duplicate.UseVisualStyleBackColor = true;
            this.Button_Duplicate.Click += new System.EventHandler(this.Button_Duplicate_Click);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Delete.Location = new System.Drawing.Point(166, 714);
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.Size = new System.Drawing.Size(73, 31);
            this.Button_Delete.TabIndex = 4;
            this.Button_Delete.Text = "Delete";
            this.Button_Delete.UseVisualStyleBackColor = true;
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_Click);
            // 
            // Button_Add
            // 
            this.Button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Add.Location = new System.Drawing.Point(3, 714);
            this.Button_Add.Name = "Button_Add";
            this.Button_Add.Size = new System.Drawing.Size(79, 31);
            this.Button_Add.TabIndex = 3;
            this.Button_Add.Text = "Add";
            this.Button_Add.UseVisualStyleBackColor = true;
            this.Button_Add.Click += new System.EventHandler(this.Button_Add_Click);
            // 
            // ExtraDlists_Purpose
            // 
            this.ExtraDlists_Purpose.FillWeight = 70F;
            this.ExtraDlists_Purpose.HeaderText = "Purpose";
            this.ExtraDlists_Purpose.Name = "ExtraDlists_Purpose";
            this.ExtraDlists_Purpose.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtraDlists_Offset
            // 
            this.ExtraDlists_Offset.FillWeight = 50F;
            this.ExtraDlists_Offset.HeaderText = "Offset";
            this.ExtraDlists_Offset.Name = "ExtraDlists_Offset";
            this.ExtraDlists_Offset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtraDlists_Translation
            // 
            this.ExtraDlists_Translation.FillWeight = 70F;
            this.ExtraDlists_Translation.HeaderText = "X,Y,Z Transl.";
            this.ExtraDlists_Translation.Name = "ExtraDlists_Translation";
            // 
            // ExtraDlists_Rotation
            // 
            this.ExtraDlists_Rotation.FillWeight = 70F;
            this.ExtraDlists_Rotation.HeaderText = "X,Y,Z Rot.";
            this.ExtraDlists_Rotation.Name = "ExtraDlists_Rotation";
            // 
            // ExtraDlists_Limb
            // 
            this.ExtraDlists_Limb.FillWeight = 35F;
            this.ExtraDlists_Limb.HeaderText = "Limb";
            this.ExtraDlists_Limb.Name = "ExtraDlists_Limb";
            // 
            // ExtraDlists_ShowType
            // 
            this.ExtraDlists_ShowType.FillWeight = 80F;
            this.ExtraDlists_ShowType.HeaderText = "Show type";
            this.ExtraDlists_ShowType.Items.AddRange(new object[] {
            "Don\'t show",
            "Alongside limb",
            "Instead of limb"});
            this.ExtraDlists_ShowType.Name = "ExtraDlists_ShowType";
            this.ExtraDlists_ShowType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ExtraDlists_ShowType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1551, 818);
            this.Controls.Add(this.Panel_Editor);
            this.Controls.Add(this.MenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "Form1";
            this.Text = "OoT NPC Maker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_NPCs)).EndInit();
            this.Panel_Editor.ResumeLayout(false);
            this.Panel_NPCData.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.Tab1_Data.ResumeLayout(false);
            this.Tab1_Data.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_ExtraDLists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSegment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSegment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSpeed)).EndInit();
            this.TabControl_Textures.ResumeLayout(false);
            this.TabPage_Segment_8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_8)).EndInit();
            this.TabPage_Segment_9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_9)).EndInit();
            this.TabPage_Segment_A.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_A)).EndInit();
            this.TabPage_Segment_B.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_B)).EndInit();
            this.TabPage_Segment_C.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_C)).EndInit();
            this.TabPage_Segment_D.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_D)).EndInit();
            this.TabPage_Segment_E.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_E)).EndInit();
            this.TabPage_Segment_F.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Seg_F)).EndInit();
            this.Panel_HeadRot.ResumeLayout(false);
            this.Panel_HeadRot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegVert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegHoz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Limb)).EndInit();
            this.Panel_TargetPanel.ResumeLayout(false);
            this.Panel_TargetPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZTargetOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YTargetOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TargetLimb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XTargetOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ObjectID)).EndInit();
            this.Panel_Movement.ResumeLayout(false);
            this.Panel_Movement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopStartNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopEndNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_PathFollowID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Hierarchy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Animations)).EndInit();
            this.Panel_Collision.ResumeLayout(false);
            this.Panel_Collision.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZColOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YColOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XColOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Scale)).EndInit();
            this.Tab2_Script.ResumeLayout(false);
            this.Tab2_Script.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Textbox_Script)).EndInit();
            this.Panel_NPCList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_Open;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_Save;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_Exit;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_New;
        private System.Windows.Forms.DataGridView DataGrid_NPCs;
        private System.Windows.Forms.Label Label_NPCName;
        private System.Windows.Forms.TextBox Textbox_NPCName;
        private System.Windows.Forms.Panel Panel_Editor;
        private System.Windows.Forms.Panel Panel_NPCData;
        private System.Windows.Forms.Panel Panel_NPCList;
        private System.Windows.Forms.Button Button_Delete;
        private System.Windows.Forms.Button Button_Add;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Name;
        private System.Windows.Forms.NumericUpDown NumUpDown_Hierarchy;
        private System.Windows.Forms.Label Label_Hierarchy;
        private System.Windows.Forms.Label Label_ObjectID;
        private System.Windows.Forms.NumericUpDown NumUpDown_ObjectID;
        private System.Windows.Forms.DataGridView DataGrid_Animations;
        private System.Windows.Forms.Label Label_AnimDefs;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_SaveBinary;
        private System.Windows.Forms.Label Label_HierarchyType;
        private System.Windows.Forms.ComboBox ComboBox_HierarchyType;
        private System.Windows.Forms.Label Label_AnimType;
        private System.Windows.Forms.ComboBox ComboBox_AnimType;
        private System.Windows.Forms.Label Label_Scale;
        private System.Windows.Forms.NumericUpDown NumUpDown_Scale;
        private System.Windows.Forms.NumericUpDown NumUpDown_ZModelOffs;
        private System.Windows.Forms.NumericUpDown NumUpDown_YModelOffs;
        private System.Windows.Forms.NumericUpDown NumUpDown_XModelOffs;
        private System.Windows.Forms.Label Label_ModelDrawOffs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_AnimName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Anim;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OBJ;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage Tab1_Data;
        private System.Windows.Forms.Panel Panel_HeadRot;
        private System.Windows.Forms.ComboBox ComboBox_HeadLookAxis;
        private System.Windows.Forms.NumericUpDown NumUpDown_DegVert;
        private System.Windows.Forms.Label Label_HeadLookAxis;
        private System.Windows.Forms.Label Label_DegVert;
        private System.Windows.Forms.Label Label_Limb;
        private System.Windows.Forms.NumericUpDown NumUpDown_DegHoz;
        private System.Windows.Forms.NumericUpDown NumUpDown_Limb;
        private System.Windows.Forms.Label Label_DegHoz;
        private System.Windows.Forms.Panel Panel_TargetPanel;
        private System.Windows.Forms.NumericUpDown NumUpDown_ZTargetOffs;
        private System.Windows.Forms.Label Label_TargetLimb;
        private System.Windows.Forms.NumericUpDown NumUpDown_YTargetOffs;
        private System.Windows.Forms.NumericUpDown NumUpDown_TargetLimb;
        private System.Windows.Forms.NumericUpDown NumUpDown_XTargetOffs;
        private System.Windows.Forms.Label Label_TargetOffset;
        private System.Windows.Forms.CheckBox Checkbox_Targettable;
        private System.Windows.Forms.Panel Panel_Movement;
        private System.Windows.Forms.NumericUpDown NumUpDown_MovDistance;
        private System.Windows.Forms.NumericUpDown NumUpDown_MovSpeed;
        private System.Windows.Forms.Label Label_Distance;
        private System.Windows.Forms.Label Label_Speed;
        private System.Windows.Forms.ComboBox Combo_MovementType;
        private System.Windows.Forms.Label Label_MovementType;
        private System.Windows.Forms.CheckBox Checkbox_Pushable;
        private System.Windows.Forms.CheckBox Checkbox_CanPressSwitches;
        private System.Windows.Forms.CheckBox Checkbox_DrawShadow;
        private System.Windows.Forms.CheckBox Checkbox_HaveCollision;
        private System.Windows.Forms.Panel Panel_Collision;
        private System.Windows.Forms.NumericUpDown NumUpDown_ZColOffs;
        private System.Windows.Forms.NumericUpDown NumUpDown_YColOffs;
        private System.Windows.Forms.NumericUpDown NumUpDown_XColOffs;
        private System.Windows.Forms.Label Label_Offset;
        private System.Windows.Forms.Label Label_Height;
        private System.Windows.Forms.NumericUpDown NumUpDown_ColHeight;
        private System.Windows.Forms.NumericUpDown NumUpDown_ColRadius;
        private System.Windows.Forms.Label Label_Radius;
        private System.Windows.Forms.Label Label_Collision;
        private System.Windows.Forms.Label Label_LookAtType;
        private System.Windows.Forms.ComboBox ComboBox_LookAtType;
        private System.Windows.Forms.TabPage Tab2_Script;
        private System.Windows.Forms.TextBox Textbox_ParseErrors;
        private System.Windows.Forms.Button Button_TryParse;
        private FastColoredTextBoxNS.FastColoredTextBox Textbox_Script;
        private System.Windows.Forms.Label Label_LoopDelay;
        private System.Windows.Forms.NumericUpDown NumUpDown_LoopDelay;
        private System.Windows.Forms.Label Label_LoopStartNode;
        private System.Windows.Forms.NumericUpDown NumUpDown_LoopStartNode;
        private System.Windows.Forms.Label Label_LoopEndNode;
        private System.Windows.Forms.NumericUpDown NumUpDown_LoopEndNode;
        private System.Windows.Forms.CheckBox Checkbox_Loop;
        private System.Windows.Forms.Label Label_PathFollowID;
        private System.Windows.Forms.NumericUpDown NumUpDown_PathFollowID;
        private System.Windows.Forms.TabControl TabControl_Textures;
        private System.Windows.Forms.TabPage TabPage_Segment_8;
        private System.Windows.Forms.TabPage TabPage_Segment_9;
        private System.Windows.Forms.TabPage TabPage_Segment_A;
        private System.Windows.Forms.TabPage TabPage_Segment_B;
        private System.Windows.Forms.TabPage TabPage_Segment_C;
        private System.Windows.Forms.TabPage TabPage_Segment_D;
        private System.Windows.Forms.DataGridView Seg_8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_8_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_8_TextOffs;
        private System.Windows.Forms.DataGridView Seg_9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_9_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_9_TexOffs;
        private System.Windows.Forms.DataGridView Seg_A;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_A_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_A_TexOffs;
        private System.Windows.Forms.DataGridView Seg_B;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_B_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_B_TexOffs;
        private System.Windows.Forms.DataGridView Seg_C;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_C_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_C_TexOffs;
        private System.Windows.Forms.DataGridView Seg_D;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_D_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_D_TexOffs;
        private System.Windows.Forms.TabPage TabPage_Segment_E;
        private System.Windows.Forms.DataGridView Seg_E;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_E_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_E_TexOffs;
        private System.Windows.Forms.TabPage TabPage_Segment_F;
        private System.Windows.Forms.DataGridView Seg_F;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_F_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_F_TexOffs;
        private System.Windows.Forms.Label Label_TalkingSegment;
        private System.Windows.Forms.Label Label_TalkingPattern;
        private System.Windows.Forms.NumericUpDown NumUpDown_TalkSpeed;
        private System.Windows.Forms.TextBox Textbox_BlinkPattern;
        private System.Windows.Forms.NumericUpDown NumUpDown_BlinkSpeed;
        private System.Windows.Forms.Label Label_BlinkingPattern;
        private System.Windows.Forms.Label Label_BlinkingSegment;
        private System.Windows.Forms.TextBox Textbox_TalkingPattern;
        private System.Windows.Forms.Label Label_TalkingFramesBetween;
        private System.Windows.Forms.NumericUpDown NumUpDown_TalkSegment;
        private System.Windows.Forms.NumericUpDown NumUpDown_BlinkSegment;
        private System.Windows.Forms.Label Label_BlinkingFramesBetween;
        private System.Windows.Forms.Button Button_EnvironmentColorPreview;
        private System.Windows.Forms.ColorDialog ColorDialog;
        private System.Windows.Forms.CheckBox Checkbox_EnvColor;
        private System.Windows.Forms.Button Button_Duplicate;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button Button_PasteBase;
        private System.Windows.Forms.Button Button_CopyBase;
        private System.Windows.Forms.Label Label_ExtraTextures;
        private System.Windows.Forms.Label Label_ExtraDisplayLists;
        private System.Windows.Forms.DataGridView DataGridView_ExtraDLists;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Purpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Translation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Rotation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Limb;
        private System.Windows.Forms.DataGridViewComboBoxColumn ExtraDlists_ShowType;
    }
}

