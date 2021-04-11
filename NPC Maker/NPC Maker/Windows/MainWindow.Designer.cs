namespace NPC_Maker
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.Panel_Editor = new System.Windows.Forms.Panel();
            this.Panel_NPCData = new System.Windows.Forms.Panel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.Tab1_Data = new System.Windows.Forms.TabPage();
            this.Btn_SelectObject = new System.Windows.Forms.Button();
            this.Txb_ObjectID = new System.Windows.Forms.TextBox();
            this.Checkbox_EnvColor = new System.Windows.Forms.CheckBox();
            this.Button_EnvironmentColorPreview = new System.Windows.Forms.Button();
            this.Label_NPCName = new System.Windows.Forms.Label();
            this.Textbox_NPCName = new System.Windows.Forms.TextBox();
            this.Label_ObjectID = new System.Windows.Forms.Label();
            this.NumUpDown_ZModelOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_Hierarchy = new System.Windows.Forms.Label();
            this.NumUpDown_Hierarchy = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_YModelOffs = new System.Windows.Forms.NumericUpDown();
            this.DataGrid_Animations = new NPC_Maker.CustomDataGridView(this.components);
            this.Col_AnimName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Anim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_StartFrame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_EndFrame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_OBJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label_AnimDefs = new System.Windows.Forms.Label();
            this.NumUpDown_XModelOffs = new System.Windows.Forms.NumericUpDown();
            this.ComboBox_HierarchyType = new System.Windows.Forms.ComboBox();
            this.Label_ModelDrawOffs = new System.Windows.Forms.Label();
            this.Label_HierarchyType = new System.Windows.Forms.Label();
            this.ComboBox_AnimType = new System.Windows.Forms.ComboBox();
            this.Label_AnimType = new System.Windows.Forms.Label();
            this.Label_Scale = new System.Windows.Forms.Label();
            this.NumUpDown_Scale = new System.Windows.Forms.NumericUpDown();
            this.Tab2_ExtraData = new System.Windows.Forms.TabPage();
            this.Label_TalkingFramesBetween = new System.Windows.Forms.Label();
            this.Label_BlinkingFramesBetween = new System.Windows.Forms.Label();
            this.Label_TalkingSegment = new System.Windows.Forms.Label();
            this.Label_TalkingPattern = new System.Windows.Forms.Label();
            this.Label_BlinkingPattern = new System.Windows.Forms.Label();
            this.Label_BlinkingSegment = new System.Windows.Forms.Label();
            this.Label_ExtraTextures = new System.Windows.Forms.Label();
            this.Label_ExtraDisplayLists = new System.Windows.Forms.Label();
            this.TabControl_Segments = new System.Windows.Forms.TabControl();
            this.TabPage_Segment_8 = new System.Windows.Forms.TabPage();
            this.Seg_8 = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_8_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_8_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg8_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_9 = new System.Windows.Forms.TabPage();
            this.Seg_9 = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_9_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_9_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_9_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_A = new System.Windows.Forms.TabPage();
            this.Seg_A = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_A_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_A_TexOffs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_A_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_B = new System.Windows.Forms.TabPage();
            this.Seg_B = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_B_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_B_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_B_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_C = new System.Windows.Forms.TabPage();
            this.Seg_C = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_C_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_C_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_C_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_D = new System.Windows.Forms.TabPage();
            this.Seg_D = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_D_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_D_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_D_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_E = new System.Windows.Forms.TabPage();
            this.Seg_E = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_E_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_E_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_E_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage_Segment_F = new System.Windows.Forms.TabPage();
            this.Seg_F = new NPC_Maker.CustomDataGridView(this.components);
            this.Seg_F_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_F_Offs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Seg_F_ObjId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumUpDown_TalkSegment = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_BlinkSegment = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_TalkSpeed = new System.Windows.Forms.NumericUpDown();
            this.Textbox_BlinkPattern = new System.Windows.Forms.TextBox();
            this.Textbox_TalkingPattern = new System.Windows.Forms.TextBox();
            this.NumUpDown_BlinkSpeed = new System.Windows.Forms.NumericUpDown();
            this.DataGridView_ExtraDLists = new NPC_Maker.CustomDataGridView(this.components);
            this.ExtraDlists_Purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Translation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Rotation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDLists_Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Limb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_ObjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_ShowType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Tab3_BehaviorData = new System.Windows.Forms.TabPage();
            this.ColorsDataGridView = new NPC_Maker.CustomDataGridView(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Chkb_Opendoors = new System.Windows.Forms.CheckBox();
            this.ChkRunJustScript = new System.Windows.Forms.CheckBox();
            this.Chkb_ReactIfAtt = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Label_CutsceneSlot = new System.Windows.Forms.Label();
            this.Panel_Collision = new System.Windows.Forms.Panel();
            this.NumUpDown_ZColOffs = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_YColOffs = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_XColOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_ColOffs = new System.Windows.Forms.Label();
            this.Label_ColHeight = new System.Windows.Forms.Label();
            this.NumUpDown_ColHeight = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_ColRadius = new System.Windows.Forms.NumericUpDown();
            this.Label_ColRadius = new System.Windows.Forms.Label();
            this.Checkbox_HaveCollision = new System.Windows.Forms.CheckBox();
            this.Panel_Shadow = new System.Windows.Forms.Panel();
            this.NumUpDown_ShRadius = new System.Windows.Forms.NumericUpDown();
            this.Label_ShRadius = new System.Windows.Forms.Label();
            this.Checkbox_DrawShadow = new System.Windows.Forms.CheckBox();
            this.Panel_HeadRot = new System.Windows.Forms.Panel();
            this.NumUpDown_LookAt_Z = new System.Windows.Forms.NumericUpDown();
            this.Label_WaistSep = new System.Windows.Forms.Label();
            this.NumUpDown_LookAt_Y = new System.Windows.Forms.NumericUpDown();
            this.Combo_Waist_Horiz = new System.Windows.Forms.ComboBox();
            this.Label_Waist_Horiz = new System.Windows.Forms.Label();
            this.NumUpDown_LookAt_X = new System.Windows.Forms.NumericUpDown();
            this.Label_LookAt_Offs = new System.Windows.Forms.Label();
            this.Combo_Waist_Vert = new System.Windows.Forms.ComboBox();
            this.Label_Waist_Vert = new System.Windows.Forms.Label();
            this.Label_WastSepr = new System.Windows.Forms.Label();
            this.NumUpDown_DegVert = new System.Windows.Forms.NumericUpDown();
            this.Label_LookAtWaistHeader = new System.Windows.Forms.Label();
            this.Label_DegVert = new System.Windows.Forms.Label();
            this.Label_WaistLimb = new System.Windows.Forms.Label();
            this.Label_LookAtType = new System.Windows.Forms.Label();
            this.NumUpDown_WaistLimb = new System.Windows.Forms.NumericUpDown();
            this.ComboBox_LookAtType = new System.Windows.Forms.ComboBox();
            this.NumUpDown_DegHoz = new System.Windows.Forms.NumericUpDown();
            this.Combo_Head_Horiz = new System.Windows.Forms.ComboBox();
            this.Label_DegHoz = new System.Windows.Forms.Label();
            this.Label_HeadHoriz = new System.Windows.Forms.Label();
            this.Combo_Head_Vert = new System.Windows.Forms.ComboBox();
            this.Label_Head_Vert = new System.Windows.Forms.Label();
            this.Label_HeadSepr = new System.Windows.Forms.Label();
            this.Label_LookAtHeadHeader = new System.Windows.Forms.Label();
            this.Label_Head_Limb = new System.Windows.Forms.Label();
            this.NumUpDown_HeadLimb = new System.Windows.Forms.NumericUpDown();
            this.Panel_TargetPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.NumUpDown_TalkRadi = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_ZTargetOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_TargetLimb = new System.Windows.Forms.Label();
            this.ComboBox_TargetDist = new System.Windows.Forms.ComboBox();
            this.NumUpDown_YTargetOffs = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.NumUpDown_XTargetOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_TargetOffset = new System.Windows.Forms.Label();
            this.Checkbox_Targettable = new System.Windows.Forms.CheckBox();
            this.NumUpDown_TargetLimb = new System.Windows.Forms.NumericUpDown();
            this.Panel_Movement = new System.Windows.Forms.Panel();
            this.Chkb_Smoothen = new System.Windows.Forms.CheckBox();
            this.Chkb_IgnoreY = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tmpicker_timedPathStart = new System.Windows.Forms.DateTimePicker();
            this.Label_PathStTime = new System.Windows.Forms.Label();
            this.tmpicker_timedPathEnd = new System.Windows.Forms.DateTimePicker();
            this.ChkBox_TimedPath = new System.Windows.Forms.CheckBox();
            this.Lbl_GravityForce = new System.Windows.Forms.Label();
            this.Label_LoopDelay = new System.Windows.Forms.Label();
            this.NumUpDown_GravityForce = new System.Windows.Forms.NumericUpDown();
            this.Label_LoopStartNode = new System.Windows.Forms.Label();
            this.NumUpDown_LoopStartNode = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_LoopDelay = new System.Windows.Forms.NumericUpDown();
            this.Checkbox_Loop = new System.Windows.Forms.CheckBox();
            this.Label_LoopEndNode = new System.Windows.Forms.Label();
            this.NumUpDown_LoopEndNode = new System.Windows.Forms.NumericUpDown();
            this.Label_PathFollowID = new System.Windows.Forms.Label();
            this.NumUpDown_PathFollowID = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_MovDistance = new System.Windows.Forms.NumericUpDown();
            this.Combo_MovementType = new System.Windows.Forms.ComboBox();
            this.Label_MovementType = new System.Windows.Forms.Label();
            this.NumUpDown_MovSpeed = new System.Windows.Forms.NumericUpDown();
            this.Label_Distance = new System.Windows.Forms.Label();
            this.Label_Speed = new System.Windows.Forms.Label();
            this.Checkbox_AlwaysDraw = new System.Windows.Forms.CheckBox();
            this.NumUpDown_CutsceneSlot = new System.Windows.Forms.NumericUpDown();
            this.Checkbox_AlwaysActive = new System.Windows.Forms.CheckBox();
            this.Checkbox_Pushable = new System.Windows.Forms.CheckBox();
            this.Checkbox_CanPressSwitches = new System.Windows.Forms.CheckBox();
            this.Panel_NPCList = new System.Windows.Forms.Panel();
            this.Button_PasteBase = new System.Windows.Forms.Button();
            this.Button_CopyBase = new System.Windows.Forms.Button();
            this.Button_Duplicate = new System.Windows.Forms.Button();
            this.Button_Delete = new System.Windows.Forms.Button();
            this.Button_Add = new System.Windows.Forms.Button();
            this.DataGrid_NPCs = new NPC_Maker.CustomDataGridView(this.components);
            this.Col_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.functionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemsgiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.questItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemsdungeonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemstradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerMasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundEffectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actorstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_New = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_SaveBinary = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCurrentScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameCurrentScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxHighlightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkSyntaxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.listsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actorsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.actorCategoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sFXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musicToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel_Editor.SuspendLayout();
            this.Panel_NPCData.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.Tab1_Data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Hierarchy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Animations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Scale)).BeginInit();
            this.Tab2_ExtraData.SuspendLayout();
            this.TabControl_Segments.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSegment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSegment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_ExtraDLists)).BeginInit();
            this.Tab3_BehaviorData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorsDataGridView)).BeginInit();
            this.Panel_Collision.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZColOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YColOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XColOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColRadius)).BeginInit();
            this.Panel_Shadow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ShRadius)).BeginInit();
            this.Panel_HeadRot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LookAt_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LookAt_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LookAt_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegVert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_WaistLimb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegHoz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_HeadLimb)).BeginInit();
            this.Panel_TargetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkRadi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZTargetOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YTargetOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XTargetOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TargetLimb)).BeginInit();
            this.Panel_Movement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_GravityForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopStartNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopEndNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_PathFollowID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_CutsceneSlot)).BeginInit();
            this.Panel_NPCList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_NPCs)).BeginInit();
            this.ContextMenuStrip.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Editor
            // 
            this.Panel_Editor.AutoScroll = true;
            this.Panel_Editor.AutoScrollMinSize = new System.Drawing.Size(936, 647);
            this.Panel_Editor.Controls.Add(this.Panel_NPCData);
            this.Panel_Editor.Controls.Add(this.Panel_NPCList);
            this.Panel_Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Editor.Enabled = false;
            this.Panel_Editor.Location = new System.Drawing.Point(0, 24);
            this.Panel_Editor.Name = "Panel_Editor";
            this.Panel_Editor.Size = new System.Drawing.Size(1063, 647);
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
            this.Panel_NPCData.Size = new System.Drawing.Size(818, 641);
            this.Panel_NPCData.TabIndex = 6;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.Tab1_Data);
            this.TabControl.Controls.Add(this.Tab2_ExtraData);
            this.TabControl.Controls.Add(this.Tab3_BehaviorData);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(818, 641);
            this.TabControl.TabIndex = 41;
            // 
            // Tab1_Data
            // 
            this.Tab1_Data.BackColor = System.Drawing.Color.White;
            this.Tab1_Data.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Tab1_Data.Controls.Add(this.Btn_SelectObject);
            this.Tab1_Data.Controls.Add(this.Txb_ObjectID);
            this.Tab1_Data.Controls.Add(this.Checkbox_EnvColor);
            this.Tab1_Data.Controls.Add(this.Button_EnvironmentColorPreview);
            this.Tab1_Data.Controls.Add(this.Label_NPCName);
            this.Tab1_Data.Controls.Add(this.Textbox_NPCName);
            this.Tab1_Data.Controls.Add(this.Label_ObjectID);
            this.Tab1_Data.Controls.Add(this.NumUpDown_ZModelOffs);
            this.Tab1_Data.Controls.Add(this.Label_Hierarchy);
            this.Tab1_Data.Controls.Add(this.NumUpDown_Hierarchy);
            this.Tab1_Data.Controls.Add(this.NumUpDown_YModelOffs);
            this.Tab1_Data.Controls.Add(this.DataGrid_Animations);
            this.Tab1_Data.Controls.Add(this.Label_AnimDefs);
            this.Tab1_Data.Controls.Add(this.NumUpDown_XModelOffs);
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
            this.Tab1_Data.Size = new System.Drawing.Size(810, 615);
            this.Tab1_Data.TabIndex = 0;
            this.Tab1_Data.Text = "General data";
            // 
            // Btn_SelectObject
            // 
            this.Btn_SelectObject.Location = new System.Drawing.Point(314, 35);
            this.Btn_SelectObject.Name = "Btn_SelectObject";
            this.Btn_SelectObject.Size = new System.Drawing.Size(65, 20);
            this.Btn_SelectObject.TabIndex = 52;
            this.Btn_SelectObject.Text = "List";
            this.Btn_SelectObject.UseVisualStyleBackColor = true;
            this.Btn_SelectObject.Click += new System.EventHandler(this.Btn_SelectObject_Click);
            // 
            // Txb_ObjectID
            // 
            this.Txb_ObjectID.Location = new System.Drawing.Point(134, 35);
            this.Txb_ObjectID.MaxLength = 32;
            this.Txb_ObjectID.Multiline = true;
            this.Txb_ObjectID.Name = "Txb_ObjectID";
            this.Txb_ObjectID.Size = new System.Drawing.Size(173, 20);
            this.Txb_ObjectID.TabIndex = 51;
            this.Txb_ObjectID.Tag = NPC_Maker.NPCEntry.Members.NPCNAME;
            this.Txb_ObjectID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Txb_ObjectID_KeyUp);
            this.Txb_ObjectID.Leave += new System.EventHandler(this.Txb_ObjectID_Leave);
            // 
            // Checkbox_EnvColor
            // 
            this.Checkbox_EnvColor.AutoSize = true;
            this.Checkbox_EnvColor.Location = new System.Drawing.Point(404, 117);
            this.Checkbox_EnvColor.Name = "Checkbox_EnvColor";
            this.Checkbox_EnvColor.Size = new System.Drawing.Size(146, 17);
            this.Checkbox_EnvColor.TabIndex = 50;
            this.Checkbox_EnvColor.Text = "Global environment color:";
            this.Checkbox_EnvColor.UseVisualStyleBackColor = true;
            this.Checkbox_EnvColor.CheckedChanged += new System.EventHandler(this.Checkbox_EnvColor_CheckedChanged);
            // 
            // Button_EnvironmentColorPreview
            // 
            this.Button_EnvironmentColorPreview.BackColor = System.Drawing.Color.Black;
            this.Button_EnvironmentColorPreview.Location = new System.Drawing.Point(556, 113);
            this.Button_EnvironmentColorPreview.Name = "Button_EnvironmentColorPreview";
            this.Button_EnvironmentColorPreview.Size = new System.Drawing.Size(42, 23);
            this.Button_EnvironmentColorPreview.TabIndex = 49;
            this.Button_EnvironmentColorPreview.UseVisualStyleBackColor = false;
            this.Button_EnvironmentColorPreview.Click += new System.EventHandler(this.Button_EnvironmentColorPreview_Click);
            // 
            // Label_NPCName
            // 
            this.Label_NPCName.AutoSize = true;
            this.Label_NPCName.Location = new System.Drawing.Point(14, 11);
            this.Label_NPCName.Name = "Label_NPCName";
            this.Label_NPCName.Size = new System.Drawing.Size(63, 13);
            this.Label_NPCName.TabIndex = 3;
            this.Label_NPCName.Text = "NPC Name:";
            // 
            // Textbox_NPCName
            // 
            this.Textbox_NPCName.Location = new System.Drawing.Point(134, 8);
            this.Textbox_NPCName.MaxLength = 32;
            this.Textbox_NPCName.Name = "Textbox_NPCName";
            this.Textbox_NPCName.Size = new System.Drawing.Size(245, 20);
            this.Textbox_NPCName.TabIndex = 4;
            this.Textbox_NPCName.Tag = NPC_Maker.NPCEntry.Members.NPCNAME;
            this.Textbox_NPCName.TextChanged += new System.EventHandler(this.Textbox_NPCName_TextChanged);
            // 
            // Label_ObjectID
            // 
            this.Label_ObjectID.AutoSize = true;
            this.Label_ObjectID.Location = new System.Drawing.Point(14, 38);
            this.Label_ObjectID.Name = "Label_ObjectID";
            this.Label_ObjectID.Size = new System.Drawing.Size(55, 13);
            this.Label_ObjectID.TabIndex = 6;
            this.Label_ObjectID.Text = "Object ID:";
            // 
            // NumUpDown_ZModelOffs
            // 
            this.NumUpDown_ZModelOffs.Location = new System.Drawing.Point(524, 36);
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
            this.NumUpDown_ZModelOffs.Tag = NPC_Maker.NPCEntry.Members.ZMODELOFFS;
            this.NumUpDown_ZModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_Hierarchy
            // 
            this.Label_Hierarchy.AutoSize = true;
            this.Label_Hierarchy.Location = new System.Drawing.Point(14, 62);
            this.Label_Hierarchy.Name = "Label_Hierarchy";
            this.Label_Hierarchy.Size = new System.Drawing.Size(84, 13);
            this.Label_Hierarchy.TabIndex = 7;
            this.Label_Hierarchy.Text = "Hierarchy offset:";
            // 
            // NumUpDown_Hierarchy
            // 
            this.NumUpDown_Hierarchy.Hexadecimal = true;
            this.NumUpDown_Hierarchy.Location = new System.Drawing.Point(134, 60);
            this.NumUpDown_Hierarchy.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.NumUpDown_Hierarchy.Name = "NumUpDown_Hierarchy";
            this.NumUpDown_Hierarchy.Size = new System.Drawing.Size(245, 20);
            this.NumUpDown_Hierarchy.TabIndex = 8;
            this.NumUpDown_Hierarchy.Tag = NPC_Maker.NPCEntry.Members.HIERARCHY;
            this.NumUpDown_Hierarchy.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_YModelOffs
            // 
            this.NumUpDown_YModelOffs.Location = new System.Drawing.Point(464, 36);
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
            this.NumUpDown_YModelOffs.Tag = NPC_Maker.NPCEntry.Members.YMODELOFFS;
            this.NumUpDown_YModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // DataGrid_Animations
            // 
            this.DataGrid_Animations.AllowUserToResizeColumns = false;
            this.DataGrid_Animations.AllowUserToResizeRows = false;
            this.DataGrid_Animations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGrid_Animations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGrid_Animations.BackgroundColor = System.Drawing.Color.White;
            this.DataGrid_Animations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_Animations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_AnimName,
            this.Col_Anim,
            this.Col_StartFrame,
            this.Col_EndFrame,
            this.Col_Speed,
            this.Col_OBJ});
            this.DataGrid_Animations.Location = new System.Drawing.Point(14, 167);
            this.DataGrid_Animations.MultiSelect = false;
            this.DataGrid_Animations.Name = "DataGrid_Animations";
            this.DataGrid_Animations.RowHeadersVisible = false;
            this.DataGrid_Animations.Size = new System.Drawing.Size(790, 442);
            this.DataGrid_Animations.TabIndex = 9;
            this.DataGrid_Animations.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewAnimations_CellParse);
            this.DataGrid_Animations.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGrid_Animations_KeyUp);
            // 
            // Col_AnimName
            // 
            this.Col_AnimName.FillWeight = 90F;
            this.Col_AnimName.HeaderText = "Purpose";
            this.Col_AnimName.Name = "Col_AnimName";
            this.Col_AnimName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_Anim
            // 
            this.Col_Anim.FillWeight = 60F;
            this.Col_Anim.HeaderText = "Offset";
            this.Col_Anim.Name = "Col_Anim";
            this.Col_Anim.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_StartFrame
            // 
            this.Col_StartFrame.FillWeight = 40F;
            this.Col_StartFrame.HeaderText = "Start Frame";
            this.Col_StartFrame.Name = "Col_StartFrame";
            this.Col_StartFrame.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_EndFrame
            // 
            this.Col_EndFrame.FillWeight = 40F;
            this.Col_EndFrame.HeaderText = "End Frame";
            this.Col_EndFrame.Name = "Col_EndFrame";
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
            this.Col_OBJ.FillWeight = 60F;
            this.Col_OBJ.HeaderText = "Object";
            this.Col_OBJ.Name = "Col_OBJ";
            this.Col_OBJ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // NumUpDown_XModelOffs
            // 
            this.NumUpDown_XModelOffs.Location = new System.Drawing.Point(404, 36);
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
            this.NumUpDown_XModelOffs.Tag = NPC_Maker.NPCEntry.Members.XMODELOFFS;
            this.NumUpDown_XModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // ComboBox_HierarchyType
            // 
            this.ComboBox_HierarchyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_HierarchyType.FormattingEnabled = true;
            this.ComboBox_HierarchyType.Items.AddRange(new object[] {
            "Matrix (Link, etc.)",
            "Non-matrix (Hylian guards, etc.)",
            "Weighted (Horses)"});
            this.ComboBox_HierarchyType.Location = new System.Drawing.Point(134, 86);
            this.ComboBox_HierarchyType.Name = "ComboBox_HierarchyType";
            this.ComboBox_HierarchyType.Size = new System.Drawing.Size(245, 21);
            this.ComboBox_HierarchyType.TabIndex = 11;
            this.ComboBox_HierarchyType.Tag = NPC_Maker.NPCEntry.Members.HIERARCHYTYPE;
            this.ComboBox_HierarchyType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_ModelDrawOffs
            // 
            this.Label_ModelDrawOffs.AutoSize = true;
            this.Label_ModelDrawOffs.Location = new System.Drawing.Point(401, 11);
            this.Label_ModelDrawOffs.Name = "Label_ModelDrawOffs";
            this.Label_ModelDrawOffs.Size = new System.Drawing.Size(94, 13);
            this.Label_ModelDrawOffs.TabIndex = 35;
            this.Label_ModelDrawOffs.Text = "Model draw offset:";
            // 
            // Label_HierarchyType
            // 
            this.Label_HierarchyType.AutoSize = true;
            this.Label_HierarchyType.Location = new System.Drawing.Point(14, 89);
            this.Label_HierarchyType.Name = "Label_HierarchyType";
            this.Label_HierarchyType.Size = new System.Drawing.Size(78, 13);
            this.Label_HierarchyType.TabIndex = 12;
            this.Label_HierarchyType.Text = "Hierarchy type:";
            // 
            // ComboBox_AnimType
            // 
            this.ComboBox_AnimType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_AnimType.FormattingEnabled = true;
            this.ComboBox_AnimType.Items.AddRange(new object[] {
            "Standard",
            "Link"});
            this.ComboBox_AnimType.Location = new System.Drawing.Point(134, 113);
            this.ComboBox_AnimType.Name = "ComboBox_AnimType";
            this.ComboBox_AnimType.Size = new System.Drawing.Size(245, 21);
            this.ComboBox_AnimType.TabIndex = 13;
            this.ComboBox_AnimType.Tag = NPC_Maker.NPCEntry.Members.ANIMTYPE;
            this.ComboBox_AnimType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_AnimType_SelectedIndexChanged);
            // 
            // Label_AnimType
            // 
            this.Label_AnimType.AutoSize = true;
            this.Label_AnimType.Location = new System.Drawing.Point(14, 116);
            this.Label_AnimType.Name = "Label_AnimType";
            this.Label_AnimType.Size = new System.Drawing.Size(79, 13);
            this.Label_AnimType.TabIndex = 14;
            this.Label_AnimType.Text = "Animation type:";
            // 
            // Label_Scale
            // 
            this.Label_Scale.AutoSize = true;
            this.Label_Scale.Location = new System.Drawing.Point(401, 62);
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
            this.NumUpDown_Scale.Location = new System.Drawing.Point(404, 86);
            this.NumUpDown_Scale.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_Scale.Name = "NumUpDown_Scale";
            this.NumUpDown_Scale.Size = new System.Drawing.Size(114, 20);
            this.NumUpDown_Scale.TabIndex = 17;
            this.NumUpDown_Scale.Tag = NPC_Maker.NPCEntry.Members.SCALE;
            this.NumUpDown_Scale.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumUpDown_Scale.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Tab2_ExtraData
            // 
            this.Tab2_ExtraData.BackColor = System.Drawing.Color.White;
            this.Tab2_ExtraData.Controls.Add(this.Label_TalkingFramesBetween);
            this.Tab2_ExtraData.Controls.Add(this.Label_BlinkingFramesBetween);
            this.Tab2_ExtraData.Controls.Add(this.Label_TalkingSegment);
            this.Tab2_ExtraData.Controls.Add(this.Label_TalkingPattern);
            this.Tab2_ExtraData.Controls.Add(this.Label_BlinkingPattern);
            this.Tab2_ExtraData.Controls.Add(this.Label_BlinkingSegment);
            this.Tab2_ExtraData.Controls.Add(this.Label_ExtraTextures);
            this.Tab2_ExtraData.Controls.Add(this.Label_ExtraDisplayLists);
            this.Tab2_ExtraData.Controls.Add(this.TabControl_Segments);
            this.Tab2_ExtraData.Controls.Add(this.NumUpDown_TalkSegment);
            this.Tab2_ExtraData.Controls.Add(this.NumUpDown_BlinkSegment);
            this.Tab2_ExtraData.Controls.Add(this.NumUpDown_TalkSpeed);
            this.Tab2_ExtraData.Controls.Add(this.Textbox_BlinkPattern);
            this.Tab2_ExtraData.Controls.Add(this.Textbox_TalkingPattern);
            this.Tab2_ExtraData.Controls.Add(this.NumUpDown_BlinkSpeed);
            this.Tab2_ExtraData.Controls.Add(this.DataGridView_ExtraDLists);
            this.Tab2_ExtraData.Location = new System.Drawing.Point(4, 22);
            this.Tab2_ExtraData.Name = "Tab2_ExtraData";
            this.Tab2_ExtraData.Padding = new System.Windows.Forms.Padding(3);
            this.Tab2_ExtraData.Size = new System.Drawing.Size(810, 615);
            this.Tab2_ExtraData.TabIndex = 2;
            this.Tab2_ExtraData.Text = "Extra data";
            // 
            // Label_TalkingFramesBetween
            // 
            this.Label_TalkingFramesBetween.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_TalkingFramesBetween.AutoSize = true;
            this.Label_TalkingFramesBetween.Location = new System.Drawing.Point(305, 593);
            this.Label_TalkingFramesBetween.Name = "Label_TalkingFramesBetween";
            this.Label_TalkingFramesBetween.Size = new System.Drawing.Size(123, 13);
            this.Label_TalkingFramesBetween.TabIndex = 65;
            this.Label_TalkingFramesBetween.Text = "Talking frames between:";
            // 
            // Label_BlinkingFramesBetween
            // 
            this.Label_BlinkingFramesBetween.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_BlinkingFramesBetween.AutoSize = true;
            this.Label_BlinkingFramesBetween.Location = new System.Drawing.Point(305, 568);
            this.Label_BlinkingFramesBetween.Name = "Label_BlinkingFramesBetween";
            this.Label_BlinkingFramesBetween.Size = new System.Drawing.Size(125, 13);
            this.Label_BlinkingFramesBetween.TabIndex = 63;
            this.Label_BlinkingFramesBetween.Text = "Blinking frames between:";
            // 
            // Label_TalkingSegment
            // 
            this.Label_TalkingSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_TalkingSegment.AutoSize = true;
            this.Label_TalkingSegment.Location = new System.Drawing.Point(132, 591);
            this.Label_TalkingSegment.Name = "Label_TalkingSegment";
            this.Label_TalkingSegment.Size = new System.Drawing.Size(88, 13);
            this.Label_TalkingSegment.TabIndex = 58;
            this.Label_TalkingSegment.Text = "Talking segment:";
            // 
            // Label_TalkingPattern
            // 
            this.Label_TalkingPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_TalkingPattern.AutoSize = true;
            this.Label_TalkingPattern.Location = new System.Drawing.Point(514, 591);
            this.Label_TalkingPattern.Name = "Label_TalkingPattern";
            this.Label_TalkingPattern.Size = new System.Drawing.Size(81, 13);
            this.Label_TalkingPattern.TabIndex = 61;
            this.Label_TalkingPattern.Text = "Talking pattern:";
            // 
            // Label_BlinkingPattern
            // 
            this.Label_BlinkingPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_BlinkingPattern.AutoSize = true;
            this.Label_BlinkingPattern.Location = new System.Drawing.Point(512, 568);
            this.Label_BlinkingPattern.Name = "Label_BlinkingPattern";
            this.Label_BlinkingPattern.Size = new System.Drawing.Size(83, 13);
            this.Label_BlinkingPattern.TabIndex = 55;
            this.Label_BlinkingPattern.Text = "Blinking pattern:";
            // 
            // Label_BlinkingSegment
            // 
            this.Label_BlinkingSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_BlinkingSegment.AutoSize = true;
            this.Label_BlinkingSegment.Location = new System.Drawing.Point(132, 568);
            this.Label_BlinkingSegment.Name = "Label_BlinkingSegment";
            this.Label_BlinkingSegment.Size = new System.Drawing.Size(90, 13);
            this.Label_BlinkingSegment.TabIndex = 56;
            this.Label_BlinkingSegment.Text = "Blinking segment:";
            // 
            // Label_ExtraTextures
            // 
            this.Label_ExtraTextures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Label_ExtraTextures.AutoSize = true;
            this.Label_ExtraTextures.BackColor = System.Drawing.Color.Transparent;
            this.Label_ExtraTextures.Location = new System.Drawing.Point(6, 286);
            this.Label_ExtraTextures.Name = "Label_ExtraTextures";
            this.Label_ExtraTextures.Size = new System.Drawing.Size(81, 13);
            this.Label_ExtraTextures.TabIndex = 53;
            this.Label_ExtraTextures.Text = "Segments data:";
            // 
            // Label_ExtraDisplayLists
            // 
            this.Label_ExtraDisplayLists.AutoSize = true;
            this.Label_ExtraDisplayLists.BackColor = System.Drawing.Color.Transparent;
            this.Label_ExtraDisplayLists.Location = new System.Drawing.Point(6, 3);
            this.Label_ExtraDisplayLists.Name = "Label_ExtraDisplayLists";
            this.Label_ExtraDisplayLists.Size = new System.Drawing.Size(89, 13);
            this.Label_ExtraDisplayLists.TabIndex = 52;
            this.Label_ExtraDisplayLists.Text = "Extra display lists:";
            // 
            // TabControl_Segments
            // 
            this.TabControl_Segments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_8);
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_9);
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_A);
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_B);
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_C);
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_D);
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_E);
            this.TabControl_Segments.Controls.Add(this.TabPage_Segment_F);
            this.TabControl_Segments.Location = new System.Drawing.Point(9, 302);
            this.TabControl_Segments.Name = "TabControl_Segments";
            this.TabControl_Segments.SelectedIndex = 0;
            this.TabControl_Segments.Size = new System.Drawing.Size(798, 255);
            this.TabControl_Segments.TabIndex = 41;
            // 
            // TabPage_Segment_8
            // 
            this.TabPage_Segment_8.Controls.Add(this.Seg_8);
            this.TabPage_Segment_8.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_8.Name = "TabPage_Segment_8";
            this.TabPage_Segment_8.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_8.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_8_Offs,
            this.Seg8_ObjId});
            this.Seg_8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_8.Location = new System.Drawing.Point(3, 3);
            this.Seg_8.MultiSelect = false;
            this.Seg_8.Name = "Seg_8";
            this.Seg_8.RowHeadersVisible = false;
            this.Seg_8.Size = new System.Drawing.Size(784, 223);
            this.Seg_8.TabIndex = 10;
            this.Seg_8.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_8.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
            // 
            // Seg_8_Name
            // 
            this.Seg_8_Name.FillWeight = 50F;
            this.Seg_8_Name.HeaderText = "Name";
            this.Seg_8_Name.Name = "Seg_8_Name";
            this.Seg_8_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_8_Offs
            // 
            this.Seg_8_Offs.HeaderText = "Offset";
            this.Seg_8_Offs.Name = "Seg_8_Offs";
            this.Seg_8_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg8_ObjId
            // 
            this.Seg8_ObjId.FillWeight = 70F;
            this.Seg8_ObjId.HeaderText = "Object ID";
            this.Seg8_ObjId.Name = "Seg8_ObjId";
            // 
            // TabPage_Segment_9
            // 
            this.TabPage_Segment_9.Controls.Add(this.Seg_9);
            this.TabPage_Segment_9.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_9.Name = "TabPage_Segment_9";
            this.TabPage_Segment_9.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_9.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_9_Offs,
            this.Seg_9_ObjId});
            this.Seg_9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_9.Location = new System.Drawing.Point(3, 3);
            this.Seg_9.MultiSelect = false;
            this.Seg_9.Name = "Seg_9";
            this.Seg_9.RowHeadersVisible = false;
            this.Seg_9.Size = new System.Drawing.Size(784, 223);
            this.Seg_9.TabIndex = 11;
            this.Seg_9.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_9.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
            // 
            // Seg_9_Name
            // 
            this.Seg_9_Name.FillWeight = 50F;
            this.Seg_9_Name.HeaderText = "Name";
            this.Seg_9_Name.Name = "Seg_9_Name";
            this.Seg_9_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_9_Offs
            // 
            this.Seg_9_Offs.HeaderText = "Offset";
            this.Seg_9_Offs.Name = "Seg_9_Offs";
            this.Seg_9_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_9_ObjId
            // 
            this.Seg_9_ObjId.FillWeight = 70F;
            this.Seg_9_ObjId.HeaderText = "Object ID";
            this.Seg_9_ObjId.Name = "Seg_9_ObjId";
            // 
            // TabPage_Segment_A
            // 
            this.TabPage_Segment_A.Controls.Add(this.Seg_A);
            this.TabPage_Segment_A.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_A.Name = "TabPage_Segment_A";
            this.TabPage_Segment_A.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_A.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_A_TexOffs,
            this.Seg_A_ObjId});
            this.Seg_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_A.Location = new System.Drawing.Point(3, 3);
            this.Seg_A.MultiSelect = false;
            this.Seg_A.Name = "Seg_A";
            this.Seg_A.RowHeadersVisible = false;
            this.Seg_A.Size = new System.Drawing.Size(784, 223);
            this.Seg_A.TabIndex = 11;
            this.Seg_A.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_A.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
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
            this.Seg_A_TexOffs.HeaderText = "Offset";
            this.Seg_A_TexOffs.Name = "Seg_A_TexOffs";
            this.Seg_A_TexOffs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_A_ObjId
            // 
            this.Seg_A_ObjId.FillWeight = 70F;
            this.Seg_A_ObjId.HeaderText = "Object ID";
            this.Seg_A_ObjId.Name = "Seg_A_ObjId";
            // 
            // TabPage_Segment_B
            // 
            this.TabPage_Segment_B.Controls.Add(this.Seg_B);
            this.TabPage_Segment_B.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_B.Name = "TabPage_Segment_B";
            this.TabPage_Segment_B.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_B.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_B_Offs,
            this.Seg_B_ObjId});
            this.Seg_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_B.Location = new System.Drawing.Point(3, 3);
            this.Seg_B.MultiSelect = false;
            this.Seg_B.Name = "Seg_B";
            this.Seg_B.RowHeadersVisible = false;
            this.Seg_B.Size = new System.Drawing.Size(784, 223);
            this.Seg_B.TabIndex = 11;
            this.Seg_B.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_B.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
            // 
            // Seg_B_Name
            // 
            this.Seg_B_Name.FillWeight = 50F;
            this.Seg_B_Name.HeaderText = "Name";
            this.Seg_B_Name.Name = "Seg_B_Name";
            this.Seg_B_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_B_Offs
            // 
            this.Seg_B_Offs.HeaderText = "Offset";
            this.Seg_B_Offs.Name = "Seg_B_Offs";
            this.Seg_B_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_B_ObjId
            // 
            this.Seg_B_ObjId.FillWeight = 70F;
            this.Seg_B_ObjId.HeaderText = "Object ID";
            this.Seg_B_ObjId.Name = "Seg_B_ObjId";
            // 
            // TabPage_Segment_C
            // 
            this.TabPage_Segment_C.Controls.Add(this.Seg_C);
            this.TabPage_Segment_C.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_C.Name = "TabPage_Segment_C";
            this.TabPage_Segment_C.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_C.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_C_Offs,
            this.Seg_C_ObjId});
            this.Seg_C.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_C.Location = new System.Drawing.Point(3, 3);
            this.Seg_C.MultiSelect = false;
            this.Seg_C.Name = "Seg_C";
            this.Seg_C.RowHeadersVisible = false;
            this.Seg_C.Size = new System.Drawing.Size(784, 223);
            this.Seg_C.TabIndex = 11;
            this.Seg_C.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_C.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
            // 
            // Seg_C_Name
            // 
            this.Seg_C_Name.FillWeight = 50F;
            this.Seg_C_Name.HeaderText = "Name";
            this.Seg_C_Name.Name = "Seg_C_Name";
            this.Seg_C_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_C_Offs
            // 
            this.Seg_C_Offs.HeaderText = "Offset";
            this.Seg_C_Offs.Name = "Seg_C_Offs";
            this.Seg_C_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_C_ObjId
            // 
            this.Seg_C_ObjId.FillWeight = 70F;
            this.Seg_C_ObjId.HeaderText = "Object ID";
            this.Seg_C_ObjId.Name = "Seg_C_ObjId";
            // 
            // TabPage_Segment_D
            // 
            this.TabPage_Segment_D.Controls.Add(this.Seg_D);
            this.TabPage_Segment_D.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_D.Name = "TabPage_Segment_D";
            this.TabPage_Segment_D.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_D.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_D_Offs,
            this.Seg_D_ObjId});
            this.Seg_D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_D.Location = new System.Drawing.Point(3, 3);
            this.Seg_D.MultiSelect = false;
            this.Seg_D.Name = "Seg_D";
            this.Seg_D.RowHeadersVisible = false;
            this.Seg_D.Size = new System.Drawing.Size(784, 223);
            this.Seg_D.TabIndex = 11;
            this.Seg_D.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_D.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
            // 
            // Seg_D_Name
            // 
            this.Seg_D_Name.FillWeight = 50F;
            this.Seg_D_Name.HeaderText = "Name";
            this.Seg_D_Name.Name = "Seg_D_Name";
            this.Seg_D_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_D_Offs
            // 
            this.Seg_D_Offs.HeaderText = "Offset";
            this.Seg_D_Offs.Name = "Seg_D_Offs";
            this.Seg_D_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_D_ObjId
            // 
            this.Seg_D_ObjId.FillWeight = 70F;
            this.Seg_D_ObjId.HeaderText = "Object ID";
            this.Seg_D_ObjId.Name = "Seg_D_ObjId";
            // 
            // TabPage_Segment_E
            // 
            this.TabPage_Segment_E.Controls.Add(this.Seg_E);
            this.TabPage_Segment_E.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_E.Name = "TabPage_Segment_E";
            this.TabPage_Segment_E.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_E.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_E_Offs,
            this.Seg_E_ObjId});
            this.Seg_E.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_E.Location = new System.Drawing.Point(3, 3);
            this.Seg_E.MultiSelect = false;
            this.Seg_E.Name = "Seg_E";
            this.Seg_E.RowHeadersVisible = false;
            this.Seg_E.Size = new System.Drawing.Size(784, 223);
            this.Seg_E.TabIndex = 11;
            this.Seg_E.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_E.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
            // 
            // Seg_E_Name
            // 
            this.Seg_E_Name.FillWeight = 50F;
            this.Seg_E_Name.HeaderText = "Name";
            this.Seg_E_Name.Name = "Seg_E_Name";
            this.Seg_E_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_E_Offs
            // 
            this.Seg_E_Offs.HeaderText = "Offset";
            this.Seg_E_Offs.Name = "Seg_E_Offs";
            this.Seg_E_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_E_ObjId
            // 
            this.Seg_E_ObjId.FillWeight = 70F;
            this.Seg_E_ObjId.HeaderText = "Object ID";
            this.Seg_E_ObjId.Name = "Seg_E_ObjId";
            // 
            // TabPage_Segment_F
            // 
            this.TabPage_Segment_F.Controls.Add(this.Seg_F);
            this.TabPage_Segment_F.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_F.Name = "TabPage_Segment_F";
            this.TabPage_Segment_F.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_F.Size = new System.Drawing.Size(790, 229);
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
            this.Seg_F_Offs,
            this.Seg_F_ObjId});
            this.Seg_F.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Seg_F.Location = new System.Drawing.Point(3, 3);
            this.Seg_F.MultiSelect = false;
            this.Seg_F.Name = "Seg_F";
            this.Seg_F.RowHeadersVisible = false;
            this.Seg_F.Size = new System.Drawing.Size(784, 223);
            this.Seg_F.TabIndex = 11;
            this.Seg_F.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridViewSegments_CellParse);
            this.Seg_F.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridViewSegments_KeyUp);
            // 
            // Seg_F_Name
            // 
            this.Seg_F_Name.FillWeight = 50F;
            this.Seg_F_Name.HeaderText = "Name";
            this.Seg_F_Name.Name = "Seg_F_Name";
            this.Seg_F_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_F_Offs
            // 
            this.Seg_F_Offs.HeaderText = "Offset";
            this.Seg_F_Offs.Name = "Seg_F_Offs";
            this.Seg_F_Offs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Seg_F_ObjId
            // 
            this.Seg_F_ObjId.FillWeight = 70F;
            this.Seg_F_ObjId.HeaderText = "Object ID";
            this.Seg_F_ObjId.Name = "Seg_F_ObjId";
            // 
            // NumUpDown_TalkSegment
            // 
            this.NumUpDown_TalkSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUpDown_TalkSegment.Hexadecimal = true;
            this.NumUpDown_TalkSegment.Location = new System.Drawing.Point(228, 589);
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
            this.NumUpDown_TalkSegment.TabIndex = 64;
            this.NumUpDown_TalkSegment.Tag = NPC_Maker.NPCEntry.Members.TALKSEG;
            this.NumUpDown_TalkSegment.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NumUpDown_TalkSegment.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_BlinkSegment
            // 
            this.NumUpDown_BlinkSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUpDown_BlinkSegment.Hexadecimal = true;
            this.NumUpDown_BlinkSegment.Location = new System.Drawing.Point(228, 564);
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
            this.NumUpDown_BlinkSegment.TabIndex = 62;
            this.NumUpDown_BlinkSegment.Tag = NPC_Maker.NPCEntry.Members.BLINKSEG;
            this.NumUpDown_BlinkSegment.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NumUpDown_BlinkSegment.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_TalkSpeed
            // 
            this.NumUpDown_TalkSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUpDown_TalkSpeed.Location = new System.Drawing.Point(436, 590);
            this.NumUpDown_TalkSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_TalkSpeed.Name = "NumUpDown_TalkSpeed";
            this.NumUpDown_TalkSpeed.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_TalkSpeed.TabIndex = 57;
            this.NumUpDown_TalkSpeed.Tag = NPC_Maker.NPCEntry.Members.TALKSPE;
            this.NumUpDown_TalkSpeed.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Textbox_BlinkPattern
            // 
            this.Textbox_BlinkPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Textbox_BlinkPattern.Location = new System.Drawing.Point(601, 565);
            this.Textbox_BlinkPattern.Name = "Textbox_BlinkPattern";
            this.Textbox_BlinkPattern.Size = new System.Drawing.Size(206, 20);
            this.Textbox_BlinkPattern.TabIndex = 60;
            this.Textbox_BlinkPattern.Tag = NPC_Maker.NPCEntry.Members.BLINKPAT;
            this.Textbox_BlinkPattern.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // Textbox_TalkingPattern
            // 
            this.Textbox_TalkingPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Textbox_TalkingPattern.Location = new System.Drawing.Point(601, 589);
            this.Textbox_TalkingPattern.Name = "Textbox_TalkingPattern";
            this.Textbox_TalkingPattern.Size = new System.Drawing.Size(206, 20);
            this.Textbox_TalkingPattern.TabIndex = 59;
            this.Textbox_TalkingPattern.Tag = NPC_Maker.NPCEntry.Members.TALKPAT;
            this.Textbox_TalkingPattern.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // NumUpDown_BlinkSpeed
            // 
            this.NumUpDown_BlinkSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUpDown_BlinkSpeed.Location = new System.Drawing.Point(436, 566);
            this.NumUpDown_BlinkSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_BlinkSpeed.Name = "NumUpDown_BlinkSpeed";
            this.NumUpDown_BlinkSpeed.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_BlinkSpeed.TabIndex = 54;
            this.NumUpDown_BlinkSpeed.Tag = NPC_Maker.NPCEntry.Members.BLINKSPE;
            this.NumUpDown_BlinkSpeed.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // DataGridView_ExtraDLists
            // 
            this.DataGridView_ExtraDLists.AllowUserToResizeColumns = false;
            this.DataGridView_ExtraDLists.AllowUserToResizeRows = false;
            this.DataGridView_ExtraDLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridView_ExtraDLists.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView_ExtraDLists.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_ExtraDLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_ExtraDLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExtraDlists_Purpose,
            this.ExtraDlists_Offset,
            this.ExtraDlists_Translation,
            this.ExtraDlists_Rotation,
            this.ExtraDLists_Scale,
            this.ExtraDlists_Limb,
            this.ExtraDlists_ObjectID,
            this.ExtraDlists_ShowType});
            this.DataGridView_ExtraDLists.Location = new System.Drawing.Point(9, 19);
            this.DataGridView_ExtraDLists.MultiSelect = false;
            this.DataGridView_ExtraDLists.Name = "DataGridView_ExtraDLists";
            this.DataGridView_ExtraDLists.RowHeadersVisible = false;
            this.DataGridView_ExtraDLists.Size = new System.Drawing.Size(798, 264);
            this.DataGridView_ExtraDLists.TabIndex = 51;
            this.DataGridView_ExtraDLists.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DataGridView_ExtraDLists_CellParsing);
            this.DataGridView_ExtraDLists.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DataGridView_ExtraDLists_KeyUp);
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
            this.ExtraDlists_Translation.FillWeight = 60F;
            this.ExtraDlists_Translation.HeaderText = "X,Y,Z Transl.";
            this.ExtraDlists_Translation.Name = "ExtraDlists_Translation";
            // 
            // ExtraDlists_Rotation
            // 
            this.ExtraDlists_Rotation.FillWeight = 60F;
            this.ExtraDlists_Rotation.HeaderText = "X,Y,Z Rot.";
            this.ExtraDlists_Rotation.Name = "ExtraDlists_Rotation";
            // 
            // ExtraDLists_Scale
            // 
            this.ExtraDLists_Scale.FillWeight = 40F;
            this.ExtraDLists_Scale.HeaderText = "Scale";
            this.ExtraDLists_Scale.Name = "ExtraDLists_Scale";
            // 
            // ExtraDlists_Limb
            // 
            this.ExtraDlists_Limb.FillWeight = 35F;
            this.ExtraDlists_Limb.HeaderText = "Limb";
            this.ExtraDlists_Limb.Name = "ExtraDlists_Limb";
            // 
            // ExtraDlists_ObjectID
            // 
            this.ExtraDlists_ObjectID.FillWeight = 60F;
            this.ExtraDlists_ObjectID.HeaderText = "Object ID";
            this.ExtraDlists_ObjectID.Name = "ExtraDlists_ObjectID";
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
            // Tab3_BehaviorData
            // 
            this.Tab3_BehaviorData.BackColor = System.Drawing.Color.White;
            this.Tab3_BehaviorData.Controls.Add(this.ColorsDataGridView);
            this.Tab3_BehaviorData.Controls.Add(this.Chkb_Opendoors);
            this.Tab3_BehaviorData.Controls.Add(this.ChkRunJustScript);
            this.Tab3_BehaviorData.Controls.Add(this.Chkb_ReactIfAtt);
            this.Tab3_BehaviorData.Controls.Add(this.label2);
            this.Tab3_BehaviorData.Controls.Add(this.Label_CutsceneSlot);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_Collision);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_Shadow);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_HeadRot);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_TargetPanel);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_Movement);
            this.Tab3_BehaviorData.Controls.Add(this.Checkbox_AlwaysDraw);
            this.Tab3_BehaviorData.Controls.Add(this.NumUpDown_CutsceneSlot);
            this.Tab3_BehaviorData.Controls.Add(this.Checkbox_AlwaysActive);
            this.Tab3_BehaviorData.Controls.Add(this.Checkbox_Pushable);
            this.Tab3_BehaviorData.Controls.Add(this.Checkbox_CanPressSwitches);
            this.Tab3_BehaviorData.Location = new System.Drawing.Point(4, 22);
            this.Tab3_BehaviorData.Name = "Tab3_BehaviorData";
            this.Tab3_BehaviorData.Padding = new System.Windows.Forms.Padding(3);
            this.Tab3_BehaviorData.Size = new System.Drawing.Size(810, 615);
            this.Tab3_BehaviorData.TabIndex = 4;
            this.Tab3_BehaviorData.Text = "Behavior";
            // 
            // ColorsDataGridView
            // 
            this.ColorsDataGridView.AllowUserToResizeColumns = false;
            this.ColorsDataGridView.AllowUserToResizeRows = false;
            this.ColorsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ColorsDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.ColorsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColorsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.ColorsDataGridView.Location = new System.Drawing.Point(440, 252);
            this.ColorsDataGridView.MultiSelect = false;
            this.ColorsDataGridView.Name = "ColorsDataGridView";
            this.ColorsDataGridView.RowHeadersVisible = false;
            this.ColorsDataGridView.Size = new System.Drawing.Size(362, 324);
            this.ColorsDataGridView.TabIndex = 75;
            this.ColorsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ColorsDataGridView_CellDoubleClick);
            this.ColorsDataGridView.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.ColorsDataGridView_CellParsing);
            this.ColorsDataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ColorsDataGridView_KeyUp);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 90F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Start limb";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 60F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Color";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Chkb_Opendoors
            // 
            this.Chkb_Opendoors.AutoSize = true;
            this.Chkb_Opendoors.Location = new System.Drawing.Point(440, 183);
            this.Chkb_Opendoors.Name = "Chkb_Opendoors";
            this.Chkb_Opendoors.Size = new System.Drawing.Size(167, 17);
            this.Chkb_Opendoors.TabIndex = 74;
            this.Chkb_Opendoors.Tag = NPC_Maker.NPCEntry.Members.OPENDOORS;
            this.Chkb_Opendoors.Text = "Opens doors if they\'re on path";
            this.Chkb_Opendoors.UseVisualStyleBackColor = true;
            this.Chkb_Opendoors.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // ChkRunJustScript
            // 
            this.ChkRunJustScript.AutoSize = true;
            this.ChkRunJustScript.Location = new System.Drawing.Point(440, 229);
            this.ChkRunJustScript.Name = "ChkRunJustScript";
            this.ChkRunJustScript.Size = new System.Drawing.Size(91, 17);
            this.ChkRunJustScript.TabIndex = 73;
            this.ChkRunJustScript.Tag = NPC_Maker.NPCEntry.Members.JUSTSCRIPT;
            this.ChkRunJustScript.Text = "Just run script";
            this.ChkRunJustScript.UseVisualStyleBackColor = true;
            this.ChkRunJustScript.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Chkb_ReactIfAtt
            // 
            this.Chkb_ReactIfAtt.AutoSize = true;
            this.Chkb_ReactIfAtt.Location = new System.Drawing.Point(440, 206);
            this.Chkb_ReactIfAtt.Name = "Chkb_ReactIfAtt";
            this.Chkb_ReactIfAtt.Size = new System.Drawing.Size(108, 17);
            this.Chkb_ReactIfAtt.TabIndex = 72;
            this.Chkb_ReactIfAtt.Tag = NPC_Maker.NPCEntry.Members.REACTATT;
            this.Chkb_ReactIfAtt.Text = "React if attacked";
            this.Chkb_ReactIfAtt.UseVisualStyleBackColor = true;
            this.Chkb_ReactIfAtt.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 71;
            this.label2.Text = "Misc:";
            // 
            // Label_CutsceneSlot
            // 
            this.Label_CutsceneSlot.AutoSize = true;
            this.Label_CutsceneSlot.Location = new System.Drawing.Point(437, 8);
            this.Label_CutsceneSlot.Name = "Label_CutsceneSlot";
            this.Label_CutsceneSlot.Size = new System.Drawing.Size(74, 13);
            this.Label_CutsceneSlot.TabIndex = 64;
            this.Label_CutsceneSlot.Text = "Cutscene slot:";
            // 
            // Panel_Collision
            // 
            this.Panel_Collision.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Collision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Collision.Controls.Add(this.NumUpDown_ZColOffs);
            this.Panel_Collision.Controls.Add(this.NumUpDown_YColOffs);
            this.Panel_Collision.Controls.Add(this.NumUpDown_XColOffs);
            this.Panel_Collision.Controls.Add(this.Label_ColOffs);
            this.Panel_Collision.Controls.Add(this.Label_ColHeight);
            this.Panel_Collision.Controls.Add(this.NumUpDown_ColHeight);
            this.Panel_Collision.Controls.Add(this.NumUpDown_ColRadius);
            this.Panel_Collision.Controls.Add(this.Label_ColRadius);
            this.Panel_Collision.Controls.Add(this.Checkbox_HaveCollision);
            this.Panel_Collision.Location = new System.Drawing.Point(7, 433);
            this.Panel_Collision.Name = "Panel_Collision";
            this.Panel_Collision.Size = new System.Drawing.Size(200, 143);
            this.Panel_Collision.TabIndex = 63;
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
            this.NumUpDown_ZColOffs.Tag = NPC_Maker.NPCEntry.Members.ZCOLOFFS;
            this.NumUpDown_ZColOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_YColOffs
            // 
            this.NumUpDown_YColOffs.Location = new System.Drawing.Point(69, 103);
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
            this.NumUpDown_YColOffs.Tag = NPC_Maker.NPCEntry.Members.YCOLOFFS;
            this.NumUpDown_YColOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_XColOffs
            // 
            this.NumUpDown_XColOffs.Location = new System.Drawing.Point(8, 103);
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
            this.NumUpDown_XColOffs.Tag = NPC_Maker.NPCEntry.Members.XCOLOFFS;
            this.NumUpDown_XColOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_ColOffs
            // 
            this.Label_ColOffs.AutoSize = true;
            this.Label_ColOffs.Location = new System.Drawing.Point(5, 87);
            this.Label_ColOffs.Name = "Label_ColOffs";
            this.Label_ColOffs.Size = new System.Drawing.Size(38, 13);
            this.Label_ColOffs.TabIndex = 31;
            this.Label_ColOffs.Text = "Offset:";
            // 
            // Label_ColHeight
            // 
            this.Label_ColHeight.AutoSize = true;
            this.Label_ColHeight.Location = new System.Drawing.Point(5, 60);
            this.Label_ColHeight.Name = "Label_ColHeight";
            this.Label_ColHeight.Size = new System.Drawing.Size(41, 13);
            this.Label_ColHeight.TabIndex = 30;
            this.Label_ColHeight.Text = "Height:";
            // 
            // NumUpDown_ColHeight
            // 
            this.NumUpDown_ColHeight.Location = new System.Drawing.Point(126, 58);
            this.NumUpDown_ColHeight.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_ColHeight.Name = "NumUpDown_ColHeight";
            this.NumUpDown_ColHeight.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_ColHeight.TabIndex = 29;
            this.NumUpDown_ColHeight.Tag = NPC_Maker.NPCEntry.Members.COLHEIGHT;
            this.NumUpDown_ColHeight.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_ColRadius
            // 
            this.NumUpDown_ColRadius.Location = new System.Drawing.Point(126, 32);
            this.NumUpDown_ColRadius.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_ColRadius.Name = "NumUpDown_ColRadius";
            this.NumUpDown_ColRadius.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_ColRadius.TabIndex = 28;
            this.NumUpDown_ColRadius.Tag = NPC_Maker.NPCEntry.Members.COLRADIUS;
            this.NumUpDown_ColRadius.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_ColRadius
            // 
            this.Label_ColRadius.AutoSize = true;
            this.Label_ColRadius.Location = new System.Drawing.Point(5, 34);
            this.Label_ColRadius.Name = "Label_ColRadius";
            this.Label_ColRadius.Size = new System.Drawing.Size(43, 13);
            this.Label_ColRadius.TabIndex = 28;
            this.Label_ColRadius.Text = "Radius:";
            // 
            // Checkbox_HaveCollision
            // 
            this.Checkbox_HaveCollision.AutoSize = true;
            this.Checkbox_HaveCollision.Location = new System.Drawing.Point(8, 5);
            this.Checkbox_HaveCollision.Name = "Checkbox_HaveCollision";
            this.Checkbox_HaveCollision.Size = new System.Drawing.Size(85, 17);
            this.Checkbox_HaveCollision.TabIndex = 21;
            this.Checkbox_HaveCollision.Tag = NPC_Maker.NPCEntry.Members.COLLISION;
            this.Checkbox_HaveCollision.Text = "Has collision";
            this.Checkbox_HaveCollision.UseVisualStyleBackColor = true;
            this.Checkbox_HaveCollision.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Panel_Shadow
            // 
            this.Panel_Shadow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Shadow.Controls.Add(this.NumUpDown_ShRadius);
            this.Panel_Shadow.Controls.Add(this.Label_ShRadius);
            this.Panel_Shadow.Controls.Add(this.Checkbox_DrawShadow);
            this.Panel_Shadow.Location = new System.Drawing.Point(7, 372);
            this.Panel_Shadow.Name = "Panel_Shadow";
            this.Panel_Shadow.Size = new System.Drawing.Size(200, 55);
            this.Panel_Shadow.TabIndex = 69;
            // 
            // NumUpDown_ShRadius
            // 
            this.NumUpDown_ShRadius.Location = new System.Drawing.Point(125, 25);
            this.NumUpDown_ShRadius.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_ShRadius.Name = "NumUpDown_ShRadius";
            this.NumUpDown_ShRadius.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_ShRadius.TabIndex = 35;
            this.NumUpDown_ShRadius.Tag = NPC_Maker.NPCEntry.Members.SHADOWRADIUS;
            this.NumUpDown_ShRadius.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_ShRadius
            // 
            this.Label_ShRadius.AutoSize = true;
            this.Label_ShRadius.Location = new System.Drawing.Point(6, 27);
            this.Label_ShRadius.Name = "Label_ShRadius";
            this.Label_ShRadius.Size = new System.Drawing.Size(43, 13);
            this.Label_ShRadius.TabIndex = 35;
            this.Label_ShRadius.Text = "Radius:";
            // 
            // Checkbox_DrawShadow
            // 
            this.Checkbox_DrawShadow.AutoSize = true;
            this.Checkbox_DrawShadow.Location = new System.Drawing.Point(9, 3);
            this.Checkbox_DrawShadow.Name = "Checkbox_DrawShadow";
            this.Checkbox_DrawShadow.Size = new System.Drawing.Size(96, 17);
            this.Checkbox_DrawShadow.TabIndex = 22;
            this.Checkbox_DrawShadow.Tag = NPC_Maker.NPCEntry.Members.SHADOW;
            this.Checkbox_DrawShadow.Text = "Draws shadow";
            this.Checkbox_DrawShadow.UseVisualStyleBackColor = true;
            this.Checkbox_DrawShadow.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Panel_HeadRot
            // 
            this.Panel_HeadRot.BackColor = System.Drawing.Color.Transparent;
            this.Panel_HeadRot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_LookAt_Z);
            this.Panel_HeadRot.Controls.Add(this.Label_WaistSep);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_LookAt_Y);
            this.Panel_HeadRot.Controls.Add(this.Combo_Waist_Horiz);
            this.Panel_HeadRot.Controls.Add(this.Label_Waist_Horiz);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_LookAt_X);
            this.Panel_HeadRot.Controls.Add(this.Label_LookAt_Offs);
            this.Panel_HeadRot.Controls.Add(this.Combo_Waist_Vert);
            this.Panel_HeadRot.Controls.Add(this.Label_Waist_Vert);
            this.Panel_HeadRot.Controls.Add(this.Label_WastSepr);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_DegVert);
            this.Panel_HeadRot.Controls.Add(this.Label_LookAtWaistHeader);
            this.Panel_HeadRot.Controls.Add(this.Label_DegVert);
            this.Panel_HeadRot.Controls.Add(this.Label_WaistLimb);
            this.Panel_HeadRot.Controls.Add(this.Label_LookAtType);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_WaistLimb);
            this.Panel_HeadRot.Controls.Add(this.ComboBox_LookAtType);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_DegHoz);
            this.Panel_HeadRot.Controls.Add(this.Combo_Head_Horiz);
            this.Panel_HeadRot.Controls.Add(this.Label_DegHoz);
            this.Panel_HeadRot.Controls.Add(this.Label_HeadHoriz);
            this.Panel_HeadRot.Controls.Add(this.Combo_Head_Vert);
            this.Panel_HeadRot.Controls.Add(this.Label_Head_Vert);
            this.Panel_HeadRot.Controls.Add(this.Label_HeadSepr);
            this.Panel_HeadRot.Controls.Add(this.Label_LookAtHeadHeader);
            this.Panel_HeadRot.Controls.Add(this.Label_Head_Limb);
            this.Panel_HeadRot.Controls.Add(this.NumUpDown_HeadLimb);
            this.Panel_HeadRot.Location = new System.Drawing.Point(7, 8);
            this.Panel_HeadRot.Name = "Panel_HeadRot";
            this.Panel_HeadRot.Size = new System.Drawing.Size(200, 358);
            this.Panel_HeadRot.TabIndex = 62;
            // 
            // NumUpDown_LookAt_Z
            // 
            this.NumUpDown_LookAt_Z.Location = new System.Drawing.Point(132, 323);
            this.NumUpDown_LookAt_Z.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_LookAt_Z.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_LookAt_Z.Name = "NumUpDown_LookAt_Z";
            this.NumUpDown_LookAt_Z.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_LookAt_Z.TabIndex = 43;
            this.NumUpDown_LookAt_Z.Tag = NPC_Maker.NPCEntry.Members.ZLOOKATOFFS;
            this.NumUpDown_LookAt_Z.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_WaistSep
            // 
            this.Label_WaistSep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Label_WaistSep.Location = new System.Drawing.Point(15, 240);
            this.Label_WaistSep.Name = "Label_WaistSep";
            this.Label_WaistSep.Size = new System.Drawing.Size(171, 1);
            this.Label_WaistSep.TabIndex = 46;
            // 
            // NumUpDown_LookAt_Y
            // 
            this.NumUpDown_LookAt_Y.Location = new System.Drawing.Point(71, 323);
            this.NumUpDown_LookAt_Y.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_LookAt_Y.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_LookAt_Y.Name = "NumUpDown_LookAt_Y";
            this.NumUpDown_LookAt_Y.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_LookAt_Y.TabIndex = 42;
            this.NumUpDown_LookAt_Y.Tag = NPC_Maker.NPCEntry.Members.YLOOKATOFFS;
            this.NumUpDown_LookAt_Y.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Combo_Waist_Horiz
            // 
            this.Combo_Waist_Horiz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_Waist_Horiz.FormattingEnabled = true;
            this.Combo_Waist_Horiz.Items.AddRange(new object[] {
            "+X",
            "-X",
            "+Y",
            "-Y",
            "+Z",
            "-Z"});
            this.Combo_Waist_Horiz.Location = new System.Drawing.Point(125, 216);
            this.Combo_Waist_Horiz.Name = "Combo_Waist_Horiz";
            this.Combo_Waist_Horiz.Size = new System.Drawing.Size(60, 21);
            this.Combo_Waist_Horiz.TabIndex = 44;
            this.Combo_Waist_Horiz.Tag = NPC_Maker.NPCEntry.Members.WAISTHORIZAXIS;
            this.Combo_Waist_Horiz.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_Waist_Horiz
            // 
            this.Label_Waist_Horiz.AutoSize = true;
            this.Label_Waist_Horiz.Location = new System.Drawing.Point(5, 219);
            this.Label_Waist_Horiz.Name = "Label_Waist_Horiz";
            this.Label_Waist_Horiz.Size = new System.Drawing.Size(58, 13);
            this.Label_Waist_Horiz.TabIndex = 43;
            this.Label_Waist_Horiz.Text = "Horiz. axis:";
            // 
            // NumUpDown_LookAt_X
            // 
            this.NumUpDown_LookAt_X.Location = new System.Drawing.Point(8, 323);
            this.NumUpDown_LookAt_X.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUpDown_LookAt_X.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUpDown_LookAt_X.Name = "NumUpDown_LookAt_X";
            this.NumUpDown_LookAt_X.Size = new System.Drawing.Size(54, 20);
            this.NumUpDown_LookAt_X.TabIndex = 41;
            this.NumUpDown_LookAt_X.Tag = NPC_Maker.NPCEntry.Members.XLOOKATOFFS;
            this.NumUpDown_LookAt_X.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_LookAt_Offs
            // 
            this.Label_LookAt_Offs.AutoSize = true;
            this.Label_LookAt_Offs.Location = new System.Drawing.Point(5, 307);
            this.Label_LookAt_Offs.Name = "Label_LookAt_Offs";
            this.Label_LookAt_Offs.Size = new System.Drawing.Size(38, 13);
            this.Label_LookAt_Offs.TabIndex = 40;
            this.Label_LookAt_Offs.Text = "Offset:";
            // 
            // Combo_Waist_Vert
            // 
            this.Combo_Waist_Vert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_Waist_Vert.FormattingEnabled = true;
            this.Combo_Waist_Vert.Items.AddRange(new object[] {
            "+X",
            "-X",
            "+Y",
            "-Y",
            "+Z",
            "-Z"});
            this.Combo_Waist_Vert.Location = new System.Drawing.Point(125, 193);
            this.Combo_Waist_Vert.Name = "Combo_Waist_Vert";
            this.Combo_Waist_Vert.Size = new System.Drawing.Size(60, 21);
            this.Combo_Waist_Vert.TabIndex = 42;
            this.Combo_Waist_Vert.Tag = NPC_Maker.NPCEntry.Members.WAISTVERTAXIS;
            this.Combo_Waist_Vert.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_Waist_Vert
            // 
            this.Label_Waist_Vert.AutoSize = true;
            this.Label_Waist_Vert.Location = new System.Drawing.Point(5, 196);
            this.Label_Waist_Vert.Name = "Label_Waist_Vert";
            this.Label_Waist_Vert.Size = new System.Drawing.Size(53, 13);
            this.Label_Waist_Vert.TabIndex = 41;
            this.Label_Waist_Vert.Text = "Vert. axis:";
            // 
            // Label_WastSepr
            // 
            this.Label_WastSepr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Label_WastSepr.Location = new System.Drawing.Point(14, 167);
            this.Label_WastSepr.Name = "Label_WastSepr";
            this.Label_WastSepr.Size = new System.Drawing.Size(171, 1);
            this.Label_WastSepr.TabIndex = 40;
            // 
            // NumUpDown_DegVert
            // 
            this.NumUpDown_DegVert.Location = new System.Drawing.Point(125, 276);
            this.NumUpDown_DegVert.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.NumUpDown_DegVert.Name = "NumUpDown_DegVert";
            this.NumUpDown_DegVert.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_DegVert.TabIndex = 27;
            this.NumUpDown_DegVert.Tag = NPC_Maker.NPCEntry.Members.DEGVERT;
            this.NumUpDown_DegVert.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_LookAtWaistHeader
            // 
            this.Label_LookAtWaistHeader.AutoSize = true;
            this.Label_LookAtWaistHeader.Location = new System.Drawing.Point(5, 151);
            this.Label_LookAtWaistHeader.Name = "Label_LookAtWaistHeader";
            this.Label_LookAtWaistHeader.Size = new System.Drawing.Size(37, 13);
            this.Label_LookAtWaistHeader.TabIndex = 39;
            this.Label_LookAtWaistHeader.Text = "Waist:";
            // 
            // Label_DegVert
            // 
            this.Label_DegVert.AutoSize = true;
            this.Label_DegVert.Location = new System.Drawing.Point(5, 278);
            this.Label_DegVert.Name = "Label_DegVert";
            this.Label_DegVert.Size = new System.Drawing.Size(94, 13);
            this.Label_DegVert.TabIndex = 26;
            this.Label_DegVert.Text = "Degrees vertically:";
            // 
            // Label_WaistLimb
            // 
            this.Label_WaistLimb.AutoSize = true;
            this.Label_WaistLimb.Location = new System.Drawing.Point(5, 173);
            this.Label_WaistLimb.Name = "Label_WaistLimb";
            this.Label_WaistLimb.Size = new System.Drawing.Size(32, 13);
            this.Label_WaistLimb.TabIndex = 37;
            this.Label_WaistLimb.Text = "Limb:";
            // 
            // Label_LookAtType
            // 
            this.Label_LookAtType.AutoSize = true;
            this.Label_LookAtType.Location = new System.Drawing.Point(5, 6);
            this.Label_LookAtType.Name = "Label_LookAtType";
            this.Label_LookAtType.Size = new System.Drawing.Size(92, 13);
            this.Label_LookAtType.TabIndex = 57;
            this.Label_LookAtType.Text = "Look at Link type:";
            // 
            // NumUpDown_WaistLimb
            // 
            this.NumUpDown_WaistLimb.Location = new System.Drawing.Point(125, 171);
            this.NumUpDown_WaistLimb.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_WaistLimb.Name = "NumUpDown_WaistLimb";
            this.NumUpDown_WaistLimb.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_WaistLimb.TabIndex = 38;
            this.NumUpDown_WaistLimb.Tag = NPC_Maker.NPCEntry.Members.WAISTLIMB;
            // 
            // ComboBox_LookAtType
            // 
            this.ComboBox_LookAtType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_LookAtType.FormattingEnabled = true;
            this.ComboBox_LookAtType.Items.AddRange(new object[] {
            "None",
            "Body",
            "Head",
            "Waist",
            "Head & Waist"});
            this.ComboBox_LookAtType.Location = new System.Drawing.Point(8, 28);
            this.ComboBox_LookAtType.Name = "ComboBox_LookAtType";
            this.ComboBox_LookAtType.Size = new System.Drawing.Size(178, 21);
            this.ComboBox_LookAtType.TabIndex = 56;
            this.ComboBox_LookAtType.Tag = NPC_Maker.NPCEntry.Members.LOOKATTYPE;
            this.ComboBox_LookAtType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // NumUpDown_DegHoz
            // 
            this.NumUpDown_DegHoz.Location = new System.Drawing.Point(125, 250);
            this.NumUpDown_DegHoz.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.NumUpDown_DegHoz.Name = "NumUpDown_DegHoz";
            this.NumUpDown_DegHoz.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_DegHoz.TabIndex = 25;
            this.NumUpDown_DegHoz.Tag = NPC_Maker.NPCEntry.Members.DEGHOZ;
            this.NumUpDown_DegHoz.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Combo_Head_Horiz
            // 
            this.Combo_Head_Horiz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_Head_Horiz.FormattingEnabled = true;
            this.Combo_Head_Horiz.Items.AddRange(new object[] {
            "+X",
            "-X",
            "+Y",
            "-Y",
            "+Z",
            "-Z"});
            this.Combo_Head_Horiz.Location = new System.Drawing.Point(125, 123);
            this.Combo_Head_Horiz.Name = "Combo_Head_Horiz";
            this.Combo_Head_Horiz.Size = new System.Drawing.Size(60, 21);
            this.Combo_Head_Horiz.TabIndex = 36;
            this.Combo_Head_Horiz.Tag = NPC_Maker.NPCEntry.Members.HEADHORIZAXIS;
            this.Combo_Head_Horiz.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_DegHoz
            // 
            this.Label_DegHoz.AutoSize = true;
            this.Label_DegHoz.Location = new System.Drawing.Point(5, 252);
            this.Label_DegHoz.Name = "Label_DegHoz";
            this.Label_DegHoz.Size = new System.Drawing.Size(105, 13);
            this.Label_DegHoz.TabIndex = 24;
            this.Label_DegHoz.Text = "Degrees horizontally:";
            // 
            // Label_HeadHoriz
            // 
            this.Label_HeadHoriz.AutoSize = true;
            this.Label_HeadHoriz.Location = new System.Drawing.Point(5, 126);
            this.Label_HeadHoriz.Name = "Label_HeadHoriz";
            this.Label_HeadHoriz.Size = new System.Drawing.Size(58, 13);
            this.Label_HeadHoriz.TabIndex = 35;
            this.Label_HeadHoriz.Text = "Horiz. axis:";
            // 
            // Combo_Head_Vert
            // 
            this.Combo_Head_Vert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_Head_Vert.FormattingEnabled = true;
            this.Combo_Head_Vert.Items.AddRange(new object[] {
            "+X",
            "-X",
            "+Y",
            "-Y",
            "+Z",
            "-Z"});
            this.Combo_Head_Vert.Location = new System.Drawing.Point(125, 100);
            this.Combo_Head_Vert.Name = "Combo_Head_Vert";
            this.Combo_Head_Vert.Size = new System.Drawing.Size(60, 21);
            this.Combo_Head_Vert.TabIndex = 34;
            this.Combo_Head_Vert.Tag = NPC_Maker.NPCEntry.Members.HEADVERTAXIS;
            this.Combo_Head_Vert.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_Head_Vert
            // 
            this.Label_Head_Vert.AutoSize = true;
            this.Label_Head_Vert.Location = new System.Drawing.Point(5, 103);
            this.Label_Head_Vert.Name = "Label_Head_Vert";
            this.Label_Head_Vert.Size = new System.Drawing.Size(53, 13);
            this.Label_Head_Vert.TabIndex = 33;
            this.Label_Head_Vert.Text = "Vert. axis:";
            // 
            // Label_HeadSepr
            // 
            this.Label_HeadSepr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Label_HeadSepr.Location = new System.Drawing.Point(14, 74);
            this.Label_HeadSepr.Name = "Label_HeadSepr";
            this.Label_HeadSepr.Size = new System.Drawing.Size(171, 1);
            this.Label_HeadSepr.TabIndex = 32;
            // 
            // Label_LookAtHeadHeader
            // 
            this.Label_LookAtHeadHeader.AutoSize = true;
            this.Label_LookAtHeadHeader.Location = new System.Drawing.Point(5, 58);
            this.Label_LookAtHeadHeader.Name = "Label_LookAtHeadHeader";
            this.Label_LookAtHeadHeader.Size = new System.Drawing.Size(36, 13);
            this.Label_LookAtHeadHeader.TabIndex = 31;
            this.Label_LookAtHeadHeader.Text = "Head:";
            // 
            // Label_Head_Limb
            // 
            this.Label_Head_Limb.AutoSize = true;
            this.Label_Head_Limb.Location = new System.Drawing.Point(5, 80);
            this.Label_Head_Limb.Name = "Label_Head_Limb";
            this.Label_Head_Limb.Size = new System.Drawing.Size(32, 13);
            this.Label_Head_Limb.TabIndex = 22;
            this.Label_Head_Limb.Text = "Limb:";
            // 
            // NumUpDown_HeadLimb
            // 
            this.NumUpDown_HeadLimb.Location = new System.Drawing.Point(125, 78);
            this.NumUpDown_HeadLimb.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_HeadLimb.Name = "NumUpDown_HeadLimb";
            this.NumUpDown_HeadLimb.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_HeadLimb.TabIndex = 23;
            this.NumUpDown_HeadLimb.Tag = NPC_Maker.NPCEntry.Members.HEADLIMB;
            this.NumUpDown_HeadLimb.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Panel_TargetPanel
            // 
            this.Panel_TargetPanel.BackColor = System.Drawing.Color.Transparent;
            this.Panel_TargetPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_TargetPanel.Controls.Add(this.label3);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_TalkRadi);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_ZTargetOffs);
            this.Panel_TargetPanel.Controls.Add(this.Label_TargetLimb);
            this.Panel_TargetPanel.Controls.Add(this.ComboBox_TargetDist);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_YTargetOffs);
            this.Panel_TargetPanel.Controls.Add(this.label1);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_XTargetOffs);
            this.Panel_TargetPanel.Controls.Add(this.Label_TargetOffset);
            this.Panel_TargetPanel.Controls.Add(this.Checkbox_Targettable);
            this.Panel_TargetPanel.Controls.Add(this.NumUpDown_TargetLimb);
            this.Panel_TargetPanel.Location = new System.Drawing.Point(222, 372);
            this.Panel_TargetPanel.Name = "Panel_TargetPanel";
            this.Panel_TargetPanel.Size = new System.Drawing.Size(200, 204);
            this.Panel_TargetPanel.TabIndex = 67;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Talk/trade radius:";
            // 
            // NumUpDown_TalkRadi
            // 
            this.NumUpDown_TalkRadi.DecimalPlaces = 2;
            this.NumUpDown_TalkRadi.Location = new System.Drawing.Point(124, 60);
            this.NumUpDown_TalkRadi.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.NumUpDown_TalkRadi.Name = "NumUpDown_TalkRadi";
            this.NumUpDown_TalkRadi.Size = new System.Drawing.Size(65, 20);
            this.NumUpDown_TalkRadi.TabIndex = 35;
            this.NumUpDown_TalkRadi.Tag = NPC_Maker.NPCEntry.Members.TALKRADIUS;
            this.NumUpDown_TalkRadi.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_ZTargetOffs
            // 
            this.NumUpDown_ZTargetOffs.Location = new System.Drawing.Point(134, 158);
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
            this.NumUpDown_ZTargetOffs.Tag = NPC_Maker.NPCEntry.Members.ZTARGETOFFS;
            this.NumUpDown_ZTargetOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_TargetLimb
            // 
            this.Label_TargetLimb.AutoSize = true;
            this.Label_TargetLimb.Location = new System.Drawing.Point(4, 36);
            this.Label_TargetLimb.Name = "Label_TargetLimb";
            this.Label_TargetLimb.Size = new System.Drawing.Size(62, 13);
            this.Label_TargetLimb.TabIndex = 28;
            this.Label_TargetLimb.Text = "Target limb:";
            // 
            // ComboBox_TargetDist
            // 
            this.ComboBox_TargetDist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_TargetDist.FormattingEnabled = true;
            this.ComboBox_TargetDist.Items.AddRange(new object[] {
            "0: Very Short",
            "1: Short",
            "2: Very long",
            "3: Medium",
            "4: Long",
            "5: Long",
            "6: Very Short",
            "7: Short",
            "8: Medium",
            "9: Infinite",
            "10: No targetting"});
            this.ComboBox_TargetDist.Location = new System.Drawing.Point(8, 109);
            this.ComboBox_TargetDist.Name = "ComboBox_TargetDist";
            this.ComboBox_TargetDist.Size = new System.Drawing.Size(181, 21);
            this.ComboBox_TargetDist.TabIndex = 52;
            this.ComboBox_TargetDist.Tag = NPC_Maker.NPCEntry.Members.TARGETDIST;
            this.ComboBox_TargetDist.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // NumUpDown_YTargetOffs
            // 
            this.NumUpDown_YTargetOffs.Location = new System.Drawing.Point(70, 158);
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
            this.NumUpDown_YTargetOffs.Tag = NPC_Maker.NPCEntry.Members.YTARGETOFFS;
            this.NumUpDown_YTargetOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "Target distance:";
            // 
            // NumUpDown_XTargetOffs
            // 
            this.NumUpDown_XTargetOffs.Location = new System.Drawing.Point(6, 158);
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
            this.NumUpDown_XTargetOffs.Tag = NPC_Maker.NPCEntry.Members.XTARGETOFFS;
            this.NumUpDown_XTargetOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_TargetOffset
            // 
            this.Label_TargetOffset.AutoSize = true;
            this.Label_TargetOffset.Location = new System.Drawing.Point(5, 143);
            this.Label_TargetOffset.Name = "Label_TargetOffset";
            this.Label_TargetOffset.Size = new System.Drawing.Size(38, 13);
            this.Label_TargetOffset.TabIndex = 35;
            this.Label_TargetOffset.Text = "Offset:";
            // 
            // Checkbox_Targettable
            // 
            this.Checkbox_Targettable.AutoSize = true;
            this.Checkbox_Targettable.Location = new System.Drawing.Point(7, 10);
            this.Checkbox_Targettable.Name = "Checkbox_Targettable";
            this.Checkbox_Targettable.Size = new System.Drawing.Size(80, 17);
            this.Checkbox_Targettable.TabIndex = 39;
            this.Checkbox_Targettable.Tag = NPC_Maker.NPCEntry.Members.TARGETTABLE;
            this.Checkbox_Targettable.Text = "Targettable";
            this.Checkbox_Targettable.UseVisualStyleBackColor = true;
            this.Checkbox_Targettable.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUpDown_TargetLimb
            // 
            this.NumUpDown_TargetLimb.Location = new System.Drawing.Point(124, 34);
            this.NumUpDown_TargetLimb.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.NumUpDown_TargetLimb.Name = "NumUpDown_TargetLimb";
            this.NumUpDown_TargetLimb.Size = new System.Drawing.Size(65, 20);
            this.NumUpDown_TargetLimb.TabIndex = 28;
            this.NumUpDown_TargetLimb.Tag = NPC_Maker.NPCEntry.Members.TARGETLIMB;
            this.NumUpDown_TargetLimb.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Panel_Movement
            // 
            this.Panel_Movement.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Movement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Movement.Controls.Add(this.Chkb_Smoothen);
            this.Panel_Movement.Controls.Add(this.Chkb_IgnoreY);
            this.Panel_Movement.Controls.Add(this.label5);
            this.Panel_Movement.Controls.Add(this.tmpicker_timedPathStart);
            this.Panel_Movement.Controls.Add(this.Label_PathStTime);
            this.Panel_Movement.Controls.Add(this.tmpicker_timedPathEnd);
            this.Panel_Movement.Controls.Add(this.ChkBox_TimedPath);
            this.Panel_Movement.Controls.Add(this.Lbl_GravityForce);
            this.Panel_Movement.Controls.Add(this.Label_LoopDelay);
            this.Panel_Movement.Controls.Add(this.NumUpDown_GravityForce);
            this.Panel_Movement.Controls.Add(this.Label_LoopStartNode);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopStartNode);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopDelay);
            this.Panel_Movement.Controls.Add(this.Checkbox_Loop);
            this.Panel_Movement.Controls.Add(this.Label_LoopEndNode);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopEndNode);
            this.Panel_Movement.Controls.Add(this.Label_PathFollowID);
            this.Panel_Movement.Controls.Add(this.NumUpDown_PathFollowID);
            this.Panel_Movement.Controls.Add(this.NumUpDown_MovDistance);
            this.Panel_Movement.Controls.Add(this.Combo_MovementType);
            this.Panel_Movement.Controls.Add(this.Label_MovementType);
            this.Panel_Movement.Controls.Add(this.NumUpDown_MovSpeed);
            this.Panel_Movement.Controls.Add(this.Label_Distance);
            this.Panel_Movement.Controls.Add(this.Label_Speed);
            this.Panel_Movement.Location = new System.Drawing.Point(222, 8);
            this.Panel_Movement.Name = "Panel_Movement";
            this.Panel_Movement.Size = new System.Drawing.Size(200, 358);
            this.Panel_Movement.TabIndex = 65;
            // 
            // Chkb_Smoothen
            // 
            this.Chkb_Smoothen.AutoSize = true;
            this.Chkb_Smoothen.Location = new System.Drawing.Point(6, 265);
            this.Chkb_Smoothen.Name = "Chkb_Smoothen";
            this.Chkb_Smoothen.Size = new System.Drawing.Size(98, 17);
            this.Chkb_Smoothen.TabIndex = 68;
            this.Chkb_Smoothen.Tag = NPC_Maker.NPCEntry.Members.SMOOTH;
            this.Chkb_Smoothen.Text = "Smoothen path";
            this.Chkb_Smoothen.UseVisualStyleBackColor = true;
            this.Chkb_Smoothen.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Chkb_IgnoreY
            // 
            this.Chkb_IgnoreY.AutoSize = true;
            this.Chkb_IgnoreY.Location = new System.Drawing.Point(6, 240);
            this.Chkb_IgnoreY.Name = "Chkb_IgnoreY";
            this.Chkb_IgnoreY.Size = new System.Drawing.Size(115, 17);
            this.Chkb_IgnoreY.TabIndex = 67;
            this.Chkb_IgnoreY.Tag = NPC_Maker.NPCEntry.Members.IGNORENODEYAXIS;
            this.Chkb_IgnoreY.Text = "Ignore node Y Axis";
            this.Chkb_IgnoreY.UseVisualStyleBackColor = true;
            this.Chkb_IgnoreY.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 323);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 66;
            this.label5.Text = "Path end time:";
            // 
            // tmpicker_timedPathStart
            // 
            this.tmpicker_timedPathStart.CustomFormat = "HH:mm";
            this.tmpicker_timedPathStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpicker_timedPathStart.Location = new System.Drawing.Point(125, 293);
            this.tmpicker_timedPathStart.Name = "tmpicker_timedPathStart";
            this.tmpicker_timedPathStart.ShowUpDown = true;
            this.tmpicker_timedPathStart.Size = new System.Drawing.Size(65, 20);
            this.tmpicker_timedPathStart.TabIndex = 63;
            this.tmpicker_timedPathStart.Tag = NPC_Maker.NPCEntry.Members.PATHSTARTTIME;
            this.tmpicker_timedPathStart.ValueChanged += new System.EventHandler(this.DatePicker_ValueChanged);
            // 
            // Label_PathStTime
            // 
            this.Label_PathStTime.AutoSize = true;
            this.Label_PathStTime.Location = new System.Drawing.Point(4, 297);
            this.Label_PathStTime.Name = "Label_PathStTime";
            this.Label_PathStTime.Size = new System.Drawing.Size(77, 13);
            this.Label_PathStTime.TabIndex = 64;
            this.Label_PathStTime.Text = "Path start time:";
            // 
            // tmpicker_timedPathEnd
            // 
            this.tmpicker_timedPathEnd.CustomFormat = "HH:mm";
            this.tmpicker_timedPathEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpicker_timedPathEnd.Location = new System.Drawing.Point(125, 319);
            this.tmpicker_timedPathEnd.Name = "tmpicker_timedPathEnd";
            this.tmpicker_timedPathEnd.ShowUpDown = true;
            this.tmpicker_timedPathEnd.Size = new System.Drawing.Size(65, 20);
            this.tmpicker_timedPathEnd.TabIndex = 65;
            this.tmpicker_timedPathEnd.Tag = NPC_Maker.NPCEntry.Members.PATHENDTIME;
            this.tmpicker_timedPathEnd.ValueChanged += new System.EventHandler(this.DatePicker_ValueChanged);
            // 
            // ChkBox_TimedPath
            // 
            this.ChkBox_TimedPath.AutoSize = true;
            this.ChkBox_TimedPath.Location = new System.Drawing.Point(110, 265);
            this.ChkBox_TimedPath.Name = "ChkBox_TimedPath";
            this.ChkBox_TimedPath.Size = new System.Drawing.Size(79, 17);
            this.ChkBox_TimedPath.TabIndex = 62;
            this.ChkBox_TimedPath.Tag = NPC_Maker.NPCEntry.Members.TIMEDPATH;
            this.ChkBox_TimedPath.Text = "Timed path";
            this.ChkBox_TimedPath.UseVisualStyleBackColor = true;
            this.ChkBox_TimedPath.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Lbl_GravityForce
            // 
            this.Lbl_GravityForce.AutoSize = true;
            this.Lbl_GravityForce.Location = new System.Drawing.Point(4, 112);
            this.Lbl_GravityForce.Name = "Lbl_GravityForce";
            this.Lbl_GravityForce.Size = new System.Drawing.Size(70, 13);
            this.Lbl_GravityForce.TabIndex = 36;
            this.Lbl_GravityForce.Text = "Gravity force:";
            // 
            // Label_LoopDelay
            // 
            this.Label_LoopDelay.AutoSize = true;
            this.Label_LoopDelay.Location = new System.Drawing.Point(3, 139);
            this.Label_LoopDelay.Name = "Label_LoopDelay";
            this.Label_LoopDelay.Size = new System.Drawing.Size(81, 13);
            this.Label_LoopDelay.TabIndex = 47;
            this.Label_LoopDelay.Text = "Delay between:";
            // 
            // NumUpDown_GravityForce
            // 
            this.NumUpDown_GravityForce.DecimalPlaces = 2;
            this.NumUpDown_GravityForce.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUpDown_GravityForce.Location = new System.Drawing.Point(123, 110);
            this.NumUpDown_GravityForce.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_GravityForce.Name = "NumUpDown_GravityForce";
            this.NumUpDown_GravityForce.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_GravityForce.TabIndex = 35;
            this.NumUpDown_GravityForce.Tag = NPC_Maker.NPCEntry.Members.GRAVITYFORCE;
            this.NumUpDown_GravityForce.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_LoopStartNode
            // 
            this.Label_LoopStartNode.AutoSize = true;
            this.Label_LoopStartNode.Location = new System.Drawing.Point(3, 191);
            this.Label_LoopStartNode.Name = "Label_LoopStartNode";
            this.Label_LoopStartNode.Size = new System.Drawing.Size(84, 13);
            this.Label_LoopStartNode.TabIndex = 45;
            this.Label_LoopStartNode.Text = "Loop start node:";
            // 
            // NumUpDown_LoopStartNode
            // 
            this.NumUpDown_LoopStartNode.Location = new System.Drawing.Point(123, 188);
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
            this.NumUpDown_LoopStartNode.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_LoopStartNode.TabIndex = 44;
            this.NumUpDown_LoopStartNode.Tag = NPC_Maker.NPCEntry.Members.LOOPSTART;
            this.NumUpDown_LoopStartNode.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_LoopStartNode.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_LoopDelay
            // 
            this.NumUpDown_LoopDelay.Location = new System.Drawing.Point(123, 136);
            this.NumUpDown_LoopDelay.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_LoopDelay.Name = "NumUpDown_LoopDelay";
            this.NumUpDown_LoopDelay.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_LoopDelay.TabIndex = 46;
            this.NumUpDown_LoopDelay.Tag = NPC_Maker.NPCEntry.Members.LOOPDEL;
            this.NumUpDown_LoopDelay.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Checkbox_Loop
            // 
            this.Checkbox_Loop.AutoSize = true;
            this.Checkbox_Loop.Location = new System.Drawing.Point(138, 240);
            this.Checkbox_Loop.Name = "Checkbox_Loop";
            this.Checkbox_Loop.Size = new System.Drawing.Size(50, 17);
            this.Checkbox_Loop.TabIndex = 41;
            this.Checkbox_Loop.Tag = NPC_Maker.NPCEntry.Members.LOOP;
            this.Checkbox_Loop.Text = "Loop";
            this.Checkbox_Loop.UseVisualStyleBackColor = true;
            this.Checkbox_Loop.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Label_LoopEndNode
            // 
            this.Label_LoopEndNode.AutoSize = true;
            this.Label_LoopEndNode.Location = new System.Drawing.Point(3, 216);
            this.Label_LoopEndNode.Name = "Label_LoopEndNode";
            this.Label_LoopEndNode.Size = new System.Drawing.Size(82, 13);
            this.Label_LoopEndNode.TabIndex = 43;
            this.Label_LoopEndNode.Text = "Loop end node:";
            // 
            // NumUpDown_LoopEndNode
            // 
            this.NumUpDown_LoopEndNode.Location = new System.Drawing.Point(123, 214);
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
            this.NumUpDown_LoopEndNode.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_LoopEndNode.TabIndex = 42;
            this.NumUpDown_LoopEndNode.Tag = NPC_Maker.NPCEntry.Members.LOOPEND;
            this.NumUpDown_LoopEndNode.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_LoopEndNode.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_PathFollowID
            // 
            this.Label_PathFollowID.AutoSize = true;
            this.Label_PathFollowID.Location = new System.Drawing.Point(3, 164);
            this.Label_PathFollowID.Name = "Label_PathFollowID";
            this.Label_PathFollowID.Size = new System.Drawing.Size(46, 13);
            this.Label_PathFollowID.TabIndex = 39;
            this.Label_PathFollowID.Text = "Path ID:";
            // 
            // NumUpDown_PathFollowID
            // 
            this.NumUpDown_PathFollowID.Location = new System.Drawing.Point(123, 162);
            this.NumUpDown_PathFollowID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_PathFollowID.Name = "NumUpDown_PathFollowID";
            this.NumUpDown_PathFollowID.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_PathFollowID.TabIndex = 38;
            this.NumUpDown_PathFollowID.Tag = NPC_Maker.NPCEntry.Members.PATHID;
            this.NumUpDown_PathFollowID.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_MovDistance
            // 
            this.NumUpDown_MovDistance.Location = new System.Drawing.Point(123, 58);
            this.NumUpDown_MovDistance.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_MovDistance.Name = "NumUpDown_MovDistance";
            this.NumUpDown_MovDistance.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_MovDistance.TabIndex = 35;
            this.NumUpDown_MovDistance.Tag = NPC_Maker.NPCEntry.Members.MOVDISTANCE;
            this.NumUpDown_MovDistance.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Combo_MovementType
            // 
            this.Combo_MovementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_MovementType.FormattingEnabled = true;
            this.Combo_MovementType.Items.AddRange(new object[] {
            "None",
            "Walk randomly",
            "Follow link",
            "Follow path"});
            this.Combo_MovementType.Location = new System.Drawing.Point(8, 27);
            this.Combo_MovementType.Name = "Combo_MovementType";
            this.Combo_MovementType.Size = new System.Drawing.Size(181, 21);
            this.Combo_MovementType.TabIndex = 61;
            this.Combo_MovementType.Tag = NPC_Maker.NPCEntry.Members.MOVEMENT;
            this.Combo_MovementType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_MovementType
            // 
            this.Label_MovementType.AutoSize = true;
            this.Label_MovementType.Location = new System.Drawing.Point(5, 6);
            this.Label_MovementType.Name = "Label_MovementType";
            this.Label_MovementType.Size = new System.Drawing.Size(83, 13);
            this.Label_MovementType.TabIndex = 60;
            this.Label_MovementType.Text = "Movement type:";
            // 
            // NumUpDown_MovSpeed
            // 
            this.NumUpDown_MovSpeed.DecimalPlaces = 2;
            this.NumUpDown_MovSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumUpDown_MovSpeed.Location = new System.Drawing.Point(123, 84);
            this.NumUpDown_MovSpeed.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_MovSpeed.Name = "NumUpDown_MovSpeed";
            this.NumUpDown_MovSpeed.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_MovSpeed.TabIndex = 37;
            this.NumUpDown_MovSpeed.Tag = NPC_Maker.NPCEntry.Members.MOVSPEED;
            this.NumUpDown_MovSpeed.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_Distance
            // 
            this.Label_Distance.AutoSize = true;
            this.Label_Distance.Location = new System.Drawing.Point(4, 60);
            this.Label_Distance.Name = "Label_Distance";
            this.Label_Distance.Size = new System.Drawing.Size(103, 13);
            this.Label_Distance.TabIndex = 35;
            this.Label_Distance.Text = "Movement distance:";
            // 
            // Label_Speed
            // 
            this.Label_Speed.AutoSize = true;
            this.Label_Speed.Location = new System.Drawing.Point(4, 86);
            this.Label_Speed.Name = "Label_Speed";
            this.Label_Speed.Size = new System.Drawing.Size(92, 13);
            this.Label_Speed.TabIndex = 36;
            this.Label_Speed.Text = "Movement speed:";
            // 
            // Checkbox_AlwaysDraw
            // 
            this.Checkbox_AlwaysDraw.AutoSize = true;
            this.Checkbox_AlwaysDraw.Location = new System.Drawing.Point(440, 113);
            this.Checkbox_AlwaysDraw.Name = "Checkbox_AlwaysDraw";
            this.Checkbox_AlwaysDraw.Size = new System.Drawing.Size(146, 17);
            this.Checkbox_AlwaysDraw.TabIndex = 70;
            this.Checkbox_AlwaysDraw.Tag = NPC_Maker.NPCEntry.Members.DRAWOUTOFCAM;
            this.Checkbox_AlwaysDraw.Text = "Draw even out of camera";
            this.Checkbox_AlwaysDraw.UseVisualStyleBackColor = true;
            this.Checkbox_AlwaysDraw.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUpDown_CutsceneSlot
            // 
            this.NumUpDown_CutsceneSlot.Location = new System.Drawing.Point(440, 29);
            this.NumUpDown_CutsceneSlot.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_CutsceneSlot.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_CutsceneSlot.Name = "NumUpDown_CutsceneSlot";
            this.NumUpDown_CutsceneSlot.Size = new System.Drawing.Size(71, 20);
            this.NumUpDown_CutsceneSlot.TabIndex = 66;
            this.NumUpDown_CutsceneSlot.Tag = NPC_Maker.NPCEntry.Members.CUTSCENEID;
            this.NumUpDown_CutsceneSlot.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_CutsceneSlot.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Checkbox_AlwaysActive
            // 
            this.Checkbox_AlwaysActive.AutoSize = true;
            this.Checkbox_AlwaysActive.Location = new System.Drawing.Point(440, 90);
            this.Checkbox_AlwaysActive.Name = "Checkbox_AlwaysActive";
            this.Checkbox_AlwaysActive.Size = new System.Drawing.Size(156, 17);
            this.Checkbox_AlwaysActive.TabIndex = 68;
            this.Checkbox_AlwaysActive.Tag = NPC_Maker.NPCEntry.Members.ACTIVE;
            this.Checkbox_AlwaysActive.Text = "Update even out of camera";
            this.Checkbox_AlwaysActive.UseVisualStyleBackColor = true;
            this.Checkbox_AlwaysActive.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Checkbox_Pushable
            // 
            this.Checkbox_Pushable.AutoSize = true;
            this.Checkbox_Pushable.Location = new System.Drawing.Point(440, 159);
            this.Checkbox_Pushable.Name = "Checkbox_Pushable";
            this.Checkbox_Pushable.Size = new System.Drawing.Size(70, 17);
            this.Checkbox_Pushable.TabIndex = 59;
            this.Checkbox_Pushable.Tag = NPC_Maker.NPCEntry.Members.PUSHABLE;
            this.Checkbox_Pushable.Text = "Pushable";
            this.Checkbox_Pushable.UseVisualStyleBackColor = true;
            this.Checkbox_Pushable.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Checkbox_CanPressSwitches
            // 
            this.Checkbox_CanPressSwitches.AutoSize = true;
            this.Checkbox_CanPressSwitches.Location = new System.Drawing.Point(440, 136);
            this.Checkbox_CanPressSwitches.Name = "Checkbox_CanPressSwitches";
            this.Checkbox_CanPressSwitches.Size = new System.Drawing.Size(107, 17);
            this.Checkbox_CanPressSwitches.TabIndex = 58;
            this.Checkbox_CanPressSwitches.Tag = NPC_Maker.NPCEntry.Members.SWITCHES;
            this.Checkbox_CanPressSwitches.Text = "Presses switches";
            this.Checkbox_CanPressSwitches.UseVisualStyleBackColor = true;
            this.Checkbox_CanPressSwitches.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
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
            this.Panel_NPCList.Size = new System.Drawing.Size(244, 641);
            this.Panel_NPCList.TabIndex = 5;
            // 
            // Button_PasteBase
            // 
            this.Button_PasteBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_PasteBase.Location = new System.Drawing.Point(125, 604);
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
            this.Button_CopyBase.Location = new System.Drawing.Point(10, 604);
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
            this.Button_Duplicate.Location = new System.Drawing.Point(87, 567);
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
            this.Button_Delete.Location = new System.Drawing.Point(166, 567);
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
            this.Button_Add.Location = new System.Drawing.Point(3, 567);
            this.Button_Add.Name = "Button_Add";
            this.Button_Add.Size = new System.Drawing.Size(79, 31);
            this.Button_Add.TabIndex = 3;
            this.Button_Add.Text = "Add";
            this.Button_Add.UseVisualStyleBackColor = true;
            this.Button_Add.Click += new System.EventHandler(this.Button_Add_Click);
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
            this.DataGrid_NPCs.Size = new System.Drawing.Size(236, 561);
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
            // ContextMenuStrip
            // 
            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.functionsToolStripMenuItem,
            this.keywordsToolStripMenuItem,
            this.keyValuesToolStripMenuItem,
            this.itemsToolStripMenuItem,
            this.itemsgiveToolStripMenuItem,
            this.questItemsToolStripMenuItem,
            this.itemsdungeonToolStripMenuItem,
            this.itemstradeToolStripMenuItem,
            this.playerMasksToolStripMenuItem,
            this.soundEffectsToolStripMenuItem,
            this.musicToolStripMenuItem,
            this.actorstoolStripMenuItem});
            this.ContextMenuStrip.Name = "ContextMenuStrip";
            this.ContextMenuStrip.Size = new System.Drawing.Size(157, 268);
            this.ContextMenuStrip.Text = "Items";
            // 
            // functionsToolStripMenuItem
            // 
            this.functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            this.functionsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.functionsToolStripMenuItem.Text = "Functions";
            // 
            // keywordsToolStripMenuItem
            // 
            this.keywordsToolStripMenuItem.Name = "keywordsToolStripMenuItem";
            this.keywordsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.keywordsToolStripMenuItem.Text = "Keywords";
            // 
            // keyValuesToolStripMenuItem
            // 
            this.keyValuesToolStripMenuItem.Name = "keyValuesToolStripMenuItem";
            this.keyValuesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.keyValuesToolStripMenuItem.Text = "Key values";
            // 
            // itemsToolStripMenuItem
            // 
            this.itemsToolStripMenuItem.Name = "itemsToolStripMenuItem";
            this.itemsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.itemsToolStripMenuItem.Text = "Inventory items";
            // 
            // itemsgiveToolStripMenuItem
            // 
            this.itemsgiveToolStripMenuItem.Name = "itemsgiveToolStripMenuItem";
            this.itemsgiveToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.itemsgiveToolStripMenuItem.Text = "Award items";
            // 
            // questItemsToolStripMenuItem
            // 
            this.questItemsToolStripMenuItem.Name = "questItemsToolStripMenuItem";
            this.questItemsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.questItemsToolStripMenuItem.Text = "Quest items";
            // 
            // itemsdungeonToolStripMenuItem
            // 
            this.itemsdungeonToolStripMenuItem.Name = "itemsdungeonToolStripMenuItem";
            this.itemsdungeonToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.itemsdungeonToolStripMenuItem.Text = "Dungeon items";
            this.itemsdungeonToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // itemstradeToolStripMenuItem
            // 
            this.itemstradeToolStripMenuItem.Name = "itemstradeToolStripMenuItem";
            this.itemstradeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.itemstradeToolStripMenuItem.Text = "Trade items";
            // 
            // playerMasksToolStripMenuItem
            // 
            this.playerMasksToolStripMenuItem.Name = "playerMasksToolStripMenuItem";
            this.playerMasksToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.playerMasksToolStripMenuItem.Text = "Player Masks";
            // 
            // soundEffectsToolStripMenuItem
            // 
            this.soundEffectsToolStripMenuItem.Name = "soundEffectsToolStripMenuItem";
            this.soundEffectsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.soundEffectsToolStripMenuItem.Text = "Sound effects";
            // 
            // musicToolStripMenuItem
            // 
            this.musicToolStripMenuItem.Name = "musicToolStripMenuItem";
            this.musicToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.musicToolStripMenuItem.Text = "Music";
            // 
            // actorstoolStripMenuItem
            // 
            this.actorstoolStripMenuItem.Name = "actorstoolStripMenuItem";
            this.actorstoolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.actorstoolStripMenuItem.Text = "Actors";
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
            this.FileMenu.Click += new System.EventHandler(this.FileMenu_Click);
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
            // scriptsToolStripMenuItem
            // 
            this.scriptsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewScriptToolStripMenuItem,
            this.deleteCurrentScriptToolStripMenuItem,
            this.renameCurrentScriptToolStripMenuItem});
            this.scriptsToolStripMenuItem.Name = "scriptsToolStripMenuItem";
            this.scriptsToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.scriptsToolStripMenuItem.Text = "Scripts";
            // 
            // addNewScriptToolStripMenuItem
            // 
            this.addNewScriptToolStripMenuItem.Name = "addNewScriptToolStripMenuItem";
            this.addNewScriptToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.addNewScriptToolStripMenuItem.Text = "Add new script";
            this.addNewScriptToolStripMenuItem.Click += new System.EventHandler(this.AddNewScriptToolStripMenuItem_Click);
            // 
            // deleteCurrentScriptToolStripMenuItem
            // 
            this.deleteCurrentScriptToolStripMenuItem.Name = "deleteCurrentScriptToolStripMenuItem";
            this.deleteCurrentScriptToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.deleteCurrentScriptToolStripMenuItem.Text = "Delete current script";
            this.deleteCurrentScriptToolStripMenuItem.Click += new System.EventHandler(this.DeleteCurrentScriptToolStripMenuItem_Click);
            // 
            // renameCurrentScriptToolStripMenuItem
            // 
            this.renameCurrentScriptToolStripMenuItem.Name = "renameCurrentScriptToolStripMenuItem";
            this.renameCurrentScriptToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.renameCurrentScriptToolStripMenuItem.Text = "Rename current script";
            this.renameCurrentScriptToolStripMenuItem.Click += new System.EventHandler(this.RenameCurrentScriptToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syntaxHighlightingToolStripMenuItem,
            this.checkSyntaxToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // syntaxHighlightingToolStripMenuItem
            // 
            this.syntaxHighlightingToolStripMenuItem.Checked = true;
            this.syntaxHighlightingToolStripMenuItem.CheckOnClick = true;
            this.syntaxHighlightingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.syntaxHighlightingToolStripMenuItem.Name = "syntaxHighlightingToolStripMenuItem";
            this.syntaxHighlightingToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.syntaxHighlightingToolStripMenuItem.Text = "Syntax highlighting";
            this.syntaxHighlightingToolStripMenuItem.CheckedChanged += new System.EventHandler(this.SyntaxHighlightingToolStripMenuItem_CheckedChanged);
            // 
            // checkSyntaxToolStripMenuItem
            // 
            this.checkSyntaxToolStripMenuItem.Checked = true;
            this.checkSyntaxToolStripMenuItem.CheckOnClick = true;
            this.checkSyntaxToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSyntaxToolStripMenuItem.Name = "checkSyntaxToolStripMenuItem";
            this.checkSyntaxToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.checkSyntaxToolStripMenuItem.Text = "Check syntax";
            this.checkSyntaxToolStripMenuItem.CheckedChanged += new System.EventHandler(this.CheckSyntaxToolStripMenuItem_CheckedChanged);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.scriptsToolStripMenuItem,
            this.listsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.MaximumSize = new System.Drawing.Size(2000, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1063, 24);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // listsToolStripMenuItem
            // 
            this.listsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectsToolStripMenuItem,
            this.actorsToolStripMenuItem1,
            this.actorCategoriesToolStripMenuItem,
            this.sFXToolStripMenuItem,
            this.musicToolStripMenuItem1});
            this.listsToolStripMenuItem.Name = "listsToolStripMenuItem";
            this.listsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.listsToolStripMenuItem.Text = "Dictionaries";
            // 
            // objectsToolStripMenuItem
            // 
            this.objectsToolStripMenuItem.Name = "objectsToolStripMenuItem";
            this.objectsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.objectsToolStripMenuItem.Text = "Objects";
            this.objectsToolStripMenuItem.Click += new System.EventHandler(this.ObjectsToolStripMenuItem_Click);
            // 
            // actorsToolStripMenuItem1
            // 
            this.actorsToolStripMenuItem1.Name = "actorsToolStripMenuItem1";
            this.actorsToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.actorsToolStripMenuItem1.Text = "Actors";
            this.actorsToolStripMenuItem1.Click += new System.EventHandler(this.ActorsToolStripMenuItem1_Click);
            // 
            // actorCategoriesToolStripMenuItem
            // 
            this.actorCategoriesToolStripMenuItem.Name = "actorCategoriesToolStripMenuItem";
            this.actorCategoriesToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.actorCategoriesToolStripMenuItem.Text = "Actor categories";
            this.actorCategoriesToolStripMenuItem.Click += new System.EventHandler(this.ActorCategoriesToolStripMenuItem_Click);
            // 
            // sFXToolStripMenuItem
            // 
            this.sFXToolStripMenuItem.Name = "sFXToolStripMenuItem";
            this.sFXToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.sFXToolStripMenuItem.Text = "SFX";
            this.sFXToolStripMenuItem.Click += new System.EventHandler(this.SFXToolStripMenuItem_Click);
            // 
            // musicToolStripMenuItem1
            // 
            this.musicToolStripMenuItem1.Name = "musicToolStripMenuItem1";
            this.musicToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.musicToolStripMenuItem1.Text = "Music";
            this.musicToolStripMenuItem1.Click += new System.EventHandler(this.MusicToolStripMenuItem1_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1063, 671);
            this.Controls.Add(this.Panel_Editor);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OoT NPC Maker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Panel_Editor.ResumeLayout(false);
            this.Panel_NPCData.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.Tab1_Data.ResumeLayout(false);
            this.Tab1_Data.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Hierarchy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Animations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Scale)).EndInit();
            this.Tab2_ExtraData.ResumeLayout(false);
            this.Tab2_ExtraData.PerformLayout();
            this.TabControl_Segments.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSegment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSegment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_ExtraDLists)).EndInit();
            this.Tab3_BehaviorData.ResumeLayout(false);
            this.Tab3_BehaviorData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorsDataGridView)).EndInit();
            this.Panel_Collision.ResumeLayout(false);
            this.Panel_Collision.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZColOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YColOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XColOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ColRadius)).EndInit();
            this.Panel_Shadow.ResumeLayout(false);
            this.Panel_Shadow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ShRadius)).EndInit();
            this.Panel_HeadRot.ResumeLayout(false);
            this.Panel_HeadRot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LookAt_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LookAt_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LookAt_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegVert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_WaistLimb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_DegHoz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_HeadLimb)).EndInit();
            this.Panel_TargetPanel.ResumeLayout(false);
            this.Panel_TargetPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkRadi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZTargetOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YTargetOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XTargetOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TargetLimb)).EndInit();
            this.Panel_Movement.ResumeLayout(false);
            this.Panel_Movement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_GravityForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopStartNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopEndNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_PathFollowID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_CutsceneSlot)).EndInit();
            this.Panel_NPCList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_NPCs)).EndInit();
            this.ContextMenuStrip.ResumeLayout(false);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CustomDataGridView DataGrid_NPCs;
        private System.Windows.Forms.Panel Panel_Editor;
        private System.Windows.Forms.Panel Panel_NPCData;
        private System.Windows.Forms.Panel Panel_NPCList;
        private System.Windows.Forms.Button Button_Delete;
        private System.Windows.Forms.Button Button_Add;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Name;
        private System.Windows.Forms.ColorDialog ColorDialog;
        private System.Windows.Forms.Button Button_Duplicate;
        private System.Windows.Forms.Button Button_PasteBase;
        private System.Windows.Forms.Button Button_CopyBase;
        private new System.Windows.Forms.ContextMenuStrip ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem functionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keywordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemsgiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemstradeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundEffectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem musicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actorstoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemsdungeonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem questItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerMasksToolStripMenuItem;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage Tab1_Data;
        private System.Windows.Forms.CheckBox Checkbox_EnvColor;
        private System.Windows.Forms.Button Button_EnvironmentColorPreview;
        private System.Windows.Forms.Label Label_NPCName;
        private System.Windows.Forms.TextBox Textbox_NPCName;
        private System.Windows.Forms.Label Label_ObjectID;
        private System.Windows.Forms.NumericUpDown NumUpDown_ZModelOffs;
        private System.Windows.Forms.Label Label_Hierarchy;
        private System.Windows.Forms.NumericUpDown NumUpDown_Hierarchy;
        private System.Windows.Forms.NumericUpDown NumUpDown_YModelOffs;
        private CustomDataGridView DataGrid_Animations;
        private System.Windows.Forms.Label Label_AnimDefs;
        private System.Windows.Forms.NumericUpDown NumUpDown_XModelOffs;
        private System.Windows.Forms.ComboBox ComboBox_HierarchyType;
        private System.Windows.Forms.Label Label_ModelDrawOffs;
        private System.Windows.Forms.Label Label_HierarchyType;
        private System.Windows.Forms.ComboBox ComboBox_AnimType;
        private System.Windows.Forms.Label Label_AnimType;
        private System.Windows.Forms.Label Label_Scale;
        private System.Windows.Forms.NumericUpDown NumUpDown_Scale;
        private System.Windows.Forms.TabPage Tab2_ExtraData;
        private System.Windows.Forms.Label Label_TalkingFramesBetween;
        private System.Windows.Forms.NumericUpDown NumUpDown_TalkSegment;
        private System.Windows.Forms.NumericUpDown NumUpDown_BlinkSegment;
        private System.Windows.Forms.Label Label_BlinkingFramesBetween;
        private System.Windows.Forms.Label Label_TalkingSegment;
        private System.Windows.Forms.Label Label_TalkingPattern;
        private System.Windows.Forms.NumericUpDown NumUpDown_TalkSpeed;
        private System.Windows.Forms.TextBox Textbox_BlinkPattern;
        private System.Windows.Forms.TextBox Textbox_TalkingPattern;
        private System.Windows.Forms.NumericUpDown NumUpDown_BlinkSpeed;
        private System.Windows.Forms.Label Label_BlinkingPattern;
        private System.Windows.Forms.Label Label_BlinkingSegment;
        private System.Windows.Forms.Label Label_ExtraTextures;
        private System.Windows.Forms.Label Label_ExtraDisplayLists;
        private System.Windows.Forms.TabControl TabControl_Segments;
        private System.Windows.Forms.TabPage TabPage_Segment_8;
        private CustomDataGridView Seg_8;
        private System.Windows.Forms.TabPage TabPage_Segment_9;
        private CustomDataGridView Seg_9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_9_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_9_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_9_ObjId;
        private System.Windows.Forms.TabPage TabPage_Segment_A;
        private CustomDataGridView Seg_A;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_A_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_A_TexOffs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_A_ObjId;
        private System.Windows.Forms.TabPage TabPage_Segment_B;
        private CustomDataGridView Seg_B;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_B_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_B_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_B_ObjId;
        private System.Windows.Forms.TabPage TabPage_Segment_C;
        private CustomDataGridView Seg_C;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_C_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_C_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_C_ObjId;
        private System.Windows.Forms.TabPage TabPage_Segment_D;
        private CustomDataGridView Seg_D;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_D_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_D_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_D_ObjId;
        private System.Windows.Forms.TabPage TabPage_Segment_E;
        private CustomDataGridView Seg_E;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_E_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_E_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_E_ObjId;
        private System.Windows.Forms.TabPage TabPage_Segment_F;
        private CustomDataGridView Seg_F;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_F_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_F_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_F_ObjId;
        private CustomDataGridView DataGridView_ExtraDLists;
        private System.Windows.Forms.TabPage Tab3_BehaviorData;
        private System.Windows.Forms.CheckBox Chkb_Opendoors;
        private System.Windows.Forms.CheckBox ChkRunJustScript;
        private System.Windows.Forms.CheckBox Chkb_ReactIfAtt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox Checkbox_AlwaysDraw;
        private System.Windows.Forms.Label Label_CutsceneSlot;
        private System.Windows.Forms.NumericUpDown NumUpDown_CutsceneSlot;
        private System.Windows.Forms.Panel Panel_Collision;
        private System.Windows.Forms.NumericUpDown NumUpDown_ZColOffs;
        private System.Windows.Forms.NumericUpDown NumUpDown_YColOffs;
        private System.Windows.Forms.NumericUpDown NumUpDown_XColOffs;
        private System.Windows.Forms.Label Label_ColOffs;
        private System.Windows.Forms.Label Label_ColHeight;
        private System.Windows.Forms.NumericUpDown NumUpDown_ColHeight;
        private System.Windows.Forms.NumericUpDown NumUpDown_ColRadius;
        private System.Windows.Forms.Label Label_ColRadius;
        private System.Windows.Forms.CheckBox Checkbox_HaveCollision;
        private System.Windows.Forms.Panel Panel_Shadow;
        private System.Windows.Forms.NumericUpDown NumUpDown_ShRadius;
        private System.Windows.Forms.Label Label_ShRadius;
        private System.Windows.Forms.CheckBox Checkbox_DrawShadow;
        private System.Windows.Forms.CheckBox Checkbox_AlwaysActive;
        private System.Windows.Forms.Panel Panel_HeadRot;
        private System.Windows.Forms.NumericUpDown NumUpDown_LookAt_Z;
        private System.Windows.Forms.Label Label_WaistSep;
        private System.Windows.Forms.NumericUpDown NumUpDown_LookAt_Y;
        private System.Windows.Forms.ComboBox Combo_Waist_Horiz;
        private System.Windows.Forms.Label Label_Waist_Horiz;
        private System.Windows.Forms.NumericUpDown NumUpDown_LookAt_X;
        private System.Windows.Forms.Label Label_LookAt_Offs;
        private System.Windows.Forms.ComboBox Combo_Waist_Vert;
        private System.Windows.Forms.Label Label_Waist_Vert;
        private System.Windows.Forms.Label Label_WastSepr;
        private System.Windows.Forms.NumericUpDown NumUpDown_DegVert;
        private System.Windows.Forms.Label Label_LookAtWaistHeader;
        private System.Windows.Forms.Label Label_DegVert;
        private System.Windows.Forms.Label Label_WaistLimb;
        private System.Windows.Forms.Label Label_LookAtType;
        private System.Windows.Forms.NumericUpDown NumUpDown_WaistLimb;
        private System.Windows.Forms.ComboBox ComboBox_LookAtType;
        private System.Windows.Forms.NumericUpDown NumUpDown_DegHoz;
        private System.Windows.Forms.ComboBox Combo_Head_Horiz;
        private System.Windows.Forms.Label Label_DegHoz;
        private System.Windows.Forms.Label Label_HeadHoriz;
        private System.Windows.Forms.ComboBox Combo_Head_Vert;
        private System.Windows.Forms.Label Label_Head_Vert;
        private System.Windows.Forms.Label Label_HeadSepr;
        private System.Windows.Forms.Label Label_LookAtHeadHeader;
        private System.Windows.Forms.Label Label_Head_Limb;
        private System.Windows.Forms.NumericUpDown NumUpDown_HeadLimb;
        private System.Windows.Forms.Panel Panel_TargetPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumUpDown_TalkRadi;
        private System.Windows.Forms.NumericUpDown NumUpDown_ZTargetOffs;
        private System.Windows.Forms.Label Label_TargetLimb;
        private System.Windows.Forms.ComboBox ComboBox_TargetDist;
        private System.Windows.Forms.NumericUpDown NumUpDown_YTargetOffs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NumUpDown_XTargetOffs;
        private System.Windows.Forms.Label Label_TargetOffset;
        private System.Windows.Forms.CheckBox Checkbox_Targettable;
        private System.Windows.Forms.NumericUpDown NumUpDown_TargetLimb;
        private System.Windows.Forms.Panel Panel_Movement;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker tmpicker_timedPathStart;
        private System.Windows.Forms.Label Label_PathStTime;
        private System.Windows.Forms.DateTimePicker tmpicker_timedPathEnd;
        private System.Windows.Forms.CheckBox ChkBox_TimedPath;
        private System.Windows.Forms.Label Lbl_GravityForce;
        private System.Windows.Forms.Label Label_LoopDelay;
        private System.Windows.Forms.NumericUpDown NumUpDown_GravityForce;
        private System.Windows.Forms.Label Label_LoopStartNode;
        private System.Windows.Forms.NumericUpDown NumUpDown_LoopStartNode;
        private System.Windows.Forms.NumericUpDown NumUpDown_LoopDelay;
        private System.Windows.Forms.Label Label_LoopEndNode;
        private System.Windows.Forms.NumericUpDown NumUpDown_LoopEndNode;
        private System.Windows.Forms.CheckBox Checkbox_Loop;
        private System.Windows.Forms.Label Label_PathFollowID;
        private System.Windows.Forms.NumericUpDown NumUpDown_PathFollowID;
        private System.Windows.Forms.NumericUpDown NumUpDown_MovDistance;
        private System.Windows.Forms.ComboBox Combo_MovementType;
        private System.Windows.Forms.Label Label_MovementType;
        private System.Windows.Forms.NumericUpDown NumUpDown_MovSpeed;
        private System.Windows.Forms.Label Label_Distance;
        private System.Windows.Forms.Label Label_Speed;
        private System.Windows.Forms.CheckBox Checkbox_Pushable;
        private System.Windows.Forms.CheckBox Checkbox_CanPressSwitches;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_8_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg_8_Offs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Seg8_ObjId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Purpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Translation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Rotation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDLists_Scale;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Limb;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_ObjectID;
        private System.Windows.Forms.DataGridViewComboBoxColumn ExtraDlists_ShowType;
        private System.Windows.Forms.TextBox Txb_ObjectID;
        private System.Windows.Forms.Button Btn_SelectObject;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_New;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_Open;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_Save;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_SaveBinary;
        private System.Windows.Forms.ToolStripMenuItem FileMenu_Exit;
        private System.Windows.Forms.ToolStripMenuItem scriptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCurrentScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameCurrentScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syntaxHighlightingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem listsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actorsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sFXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem musicToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem actorCategoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkSyntaxToolStripMenuItem;
        private CustomDataGridView ColorsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_AnimName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Anim;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_StartFrame;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_EndFrame;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OBJ;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.CheckBox Chkb_IgnoreY;
        private System.Windows.Forms.CheckBox Chkb_Smoothen;
    }
}

