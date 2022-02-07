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
            this.Lbl_LimbColors = new System.Windows.Forms.Label();
            this.NumUpAlpha = new System.Windows.Forms.NumericUpDown();
            this.LblAlpha = new System.Windows.Forms.Label();
            this.Btn_SelectObject = new System.Windows.Forms.Button();
            this.Txb_ObjectID = new System.Windows.Forms.TextBox();
            this.Checkbox_EnvColor = new System.Windows.Forms.CheckBox();
            this.Button_EnvironmentColorPreview = new System.Windows.Forms.Button();
            this.ColorsDataGridView = new NPC_Maker.CustomDataGridView(this.components);
            this.StartLimbColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.TabPage_Segment_9 = new System.Windows.Forms.TabPage();
            this.TabPage_Segment_A = new System.Windows.Forms.TabPage();
            this.TabPage_Segment_B = new System.Windows.Forms.TabPage();
            this.TabPage_Segment_C = new System.Windows.Forms.TabPage();
            this.TabPage_Segment_D = new System.Windows.Forms.TabPage();
            this.TabPage_Segment_E = new System.Windows.Forms.TabPage();
            this.TabPage_Segment_F = new System.Windows.Forms.TabPage();
            this.NumUpDown_TalkSegment = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_BlinkSegment = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_TalkSpeed = new System.Windows.Forms.NumericUpDown();
            this.Textbox_BlinkPattern = new System.Windows.Forms.TextBox();
            this.Textbox_TalkingPattern = new System.Windows.Forms.TextBox();
            this.NumUpDown_BlinkSpeed = new System.Windows.Forms.NumericUpDown();
            this.DataGridView_ExtraDLists = new NPC_Maker.CustomDataGridView(this.components);
            this.ExtraDlists_Purpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Translation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Rotation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDLists_Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_Limb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_ObjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExtraDlists_ShowType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Tab3_BehaviorData = new System.Windows.Forms.TabPage();
            this.NumUpDown_ScriptsFVar = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_ScriptsVar = new System.Windows.Forms.NumericUpDown();
            this.Lbl_ScriptsFVars = new System.Windows.Forms.Label();
            this.Lbl_ScriptsVars = new System.Windows.Forms.Label();
            this.ChkInvisible = new System.Windows.Forms.CheckBox();
            this.ChkOnlyWhenLens = new System.Windows.Forms.CheckBox();
            this.ChkBox_DebugCol = new System.Windows.Forms.CheckBox();
            this.Panel_Colors = new System.Windows.Forms.Panel();
            this.Lbl_Radius = new System.Windows.Forms.Label();
            this.NumUp_LightRadius = new System.Windows.Forms.NumericUpDown();
            this.NumUp_LightZOffs = new System.Windows.Forms.NumericUpDown();
            this.NumUp_LightYOffs = new System.Windows.Forms.NumericUpDown();
            this.Lbl_LightColor = new System.Windows.Forms.Label();
            this.Btn_LightColor = new System.Windows.Forms.Button();
            this.NumUp_LightXOffs = new System.Windows.Forms.NumericUpDown();
            this.Label_LightLimbOffset = new System.Windows.Forms.Label();
            this.Label_LightLimb = new System.Windows.Forms.Label();
            this.ChkBox_Glow = new System.Windows.Forms.CheckBox();
            this.NumUp_LightLimb = new System.Windows.Forms.NumericUpDown();
            this.ChkBox_GenLight = new System.Windows.Forms.CheckBox();
            this.NumUp_RiddenBy = new System.Windows.Forms.NumericUpDown();
            this.Label_RiddenBy = new System.Windows.Forms.Label();
            this.ChkB_FadeOut = new System.Windows.Forms.CheckBox();
            this.Btn_ReactIfAtt = new System.Windows.Forms.Panel();
            this.Combo_EffIfAtt = new System.Windows.Forms.ComboBox();
            this.Label_EffIfAtt = new System.Windows.Forms.Label();
            this.Btn_ReactIfAttList = new System.Windows.Forms.Button();
            this.Txtbox_ReactIfAtt = new System.Windows.Forms.TextBox();
            this.Lbl_ReactIfAttSnd = new System.Windows.Forms.Label();
            this.Chkb_ReactIfAtt = new System.Windows.Forms.CheckBox();
            this.Chkb_Opendoors = new System.Windows.Forms.CheckBox();
            this.ChkRunJustScript = new System.Windows.Forms.CheckBox();
            this.Lbl_Misc = new System.Windows.Forms.Label();
            this.Label_CutsceneSlot = new System.Windows.Forms.Label();
            this.Panel_Collision = new System.Windows.Forms.Panel();
            this.NumUpDown_Mass = new System.Windows.Forms.NumericUpDown();
            this.Lbl_Mass = new System.Windows.Forms.Label();
            this.NumUpDown_YColOffs = new System.Windows.Forms.NumericUpDown();
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
            this.label2 = new System.Windows.Forms.Label();
            this.NumUp_MaxRoam = new System.Windows.Forms.NumericUpDown();
            this.Chkb_IgnoreY = new System.Windows.Forms.CheckBox();
            this.SmoothingCnts = new System.Windows.Forms.Label();
            this.NumUp_Smoothing = new System.Windows.Forms.NumericUpDown();
            this.Lb_PathEnd = new System.Windows.Forms.Label();
            this.tmpicker_timedPathStart = new System.Windows.Forms.DateTimePicker();
            this.Label_PathStTime = new System.Windows.Forms.Label();
            this.tmpicker_timedPathEnd = new System.Windows.Forms.DateTimePicker();
            this.Lbl_GravityForce = new System.Windows.Forms.Label();
            this.Label_LoopDelay = new System.Windows.Forms.Label();
            this.Label_LoopStartNode = new System.Windows.Forms.Label();
            this.Checkbox_Loop = new System.Windows.Forms.CheckBox();
            this.NumUpDown_LoopStartNode = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_GravityForce = new System.Windows.Forms.NumericUpDown();
            this.NumUpDown_LoopDelay = new System.Windows.Forms.NumericUpDown();
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
            this.Checkbox_CanPressSwitches = new System.Windows.Forms.CheckBox();
            this.MessagesTabPage = new System.Windows.Forms.TabPage();
            this.PanelMsgPreview = new System.Windows.Forms.Panel();
            this.MsgPreview = new System.Windows.Forms.PictureBox();
            this.MsgText = new FastColoredTextBoxNS.FastColoredTextBox();
            this.Btn_MsgRename = new System.Windows.Forms.Button();
            this.Lbl_Text = new System.Windows.Forms.Label();
            this.Combo_MsgPos = new System.Windows.Forms.ComboBox();
            this.Lbl_MsgPos = new System.Windows.Forms.Label();
            this.Combo_MsgType = new System.Windows.Forms.ComboBox();
            this.Lbl_MsgType = new System.Windows.Forms.Label();
            this.Btn_DeleteMsg = new System.Windows.Forms.Button();
            this.Btn_AddMsg = new System.Windows.Forms.Button();
            this.MessagesGrid = new NPC_Maker.CustomDataGridView(this.components);
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Panel_NPCList = new System.Windows.Forms.Panel();
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
            this.editGlobalHeaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxHighlightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkSyntaxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.improveMessagePreviewReadabilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.listsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actorsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sFXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musicToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.linkAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorPickerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.Panel_Editor.SuspendLayout();
            this.Panel_NPCData.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.Tab1_Data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Hierarchy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Animations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XModelOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Scale)).BeginInit();
            this.Tab2_ExtraData.SuspendLayout();
            this.TabControl_Segments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSegment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSegment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_ExtraDLists)).BeginInit();
            this.Tab3_BehaviorData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ScriptsFVar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ScriptsVar)).BeginInit();
            this.Panel_Colors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightZOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightYOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightXOffs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightLimb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_RiddenBy)).BeginInit();
            this.Btn_ReactIfAtt.SuspendLayout();
            this.Panel_Collision.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Mass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YColOffs)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_MaxRoam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_Smoothing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopStartNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_GravityForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopEndNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_PathFollowID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_CutsceneSlot)).BeginInit();
            this.MessagesTabPage.SuspendLayout();
            this.PanelMsgPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MsgPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MsgText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MessagesGrid)).BeginInit();
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
            this.Panel_Editor.Size = new System.Drawing.Size(1021, 659);
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
            this.Panel_NPCData.Size = new System.Drawing.Size(776, 653);
            this.Panel_NPCData.TabIndex = 6;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.Tab1_Data);
            this.TabControl.Controls.Add(this.Tab2_ExtraData);
            this.TabControl.Controls.Add(this.Tab3_BehaviorData);
            this.TabControl.Controls.Add(this.MessagesTabPage);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(776, 653);
            this.TabControl.TabIndex = 41;
            // 
            // Tab1_Data
            // 
            this.Tab1_Data.BackColor = System.Drawing.Color.White;
            this.Tab1_Data.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Tab1_Data.Controls.Add(this.Lbl_LimbColors);
            this.Tab1_Data.Controls.Add(this.NumUpAlpha);
            this.Tab1_Data.Controls.Add(this.LblAlpha);
            this.Tab1_Data.Controls.Add(this.Btn_SelectObject);
            this.Tab1_Data.Controls.Add(this.Txb_ObjectID);
            this.Tab1_Data.Controls.Add(this.Checkbox_EnvColor);
            this.Tab1_Data.Controls.Add(this.Button_EnvironmentColorPreview);
            this.Tab1_Data.Controls.Add(this.ColorsDataGridView);
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
            this.Tab1_Data.Size = new System.Drawing.Size(768, 627);
            this.Tab1_Data.TabIndex = 0;
            this.Tab1_Data.Text = "General data";
            // 
            // Lbl_LimbColors
            // 
            this.Lbl_LimbColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_LimbColors.AutoSize = true;
            this.Lbl_LimbColors.Location = new System.Drawing.Point(628, 151);
            this.Lbl_LimbColors.Name = "Lbl_LimbColors";
            this.Lbl_LimbColors.Size = new System.Drawing.Size(63, 13);
            this.Lbl_LimbColors.TabIndex = 76;
            this.Lbl_LimbColors.Text = "Limb colors:";
            // 
            // NumUpAlpha
            // 
            this.NumUpAlpha.Location = new System.Drawing.Point(556, 86);
            this.NumUpAlpha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpAlpha.Name = "NumUpAlpha";
            this.NumUpAlpha.Size = new System.Drawing.Size(72, 20);
            this.NumUpAlpha.TabIndex = 54;
            this.NumUpAlpha.Tag = "ALPHA";
            this.NumUpAlpha.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumUpAlpha.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // LblAlpha
            // 
            this.LblAlpha.AutoSize = true;
            this.LblAlpha.Location = new System.Drawing.Point(553, 62);
            this.LblAlpha.Name = "LblAlpha";
            this.LblAlpha.Size = new System.Drawing.Size(46, 13);
            this.LblAlpha.TabIndex = 53;
            this.LblAlpha.Text = "Opacity:";
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
            this.Txb_ObjectID.Tag = "";
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
            // ColorsDataGridView
            // 
            this.ColorsDataGridView.AllowUserToResizeColumns = false;
            this.ColorsDataGridView.AllowUserToResizeRows = false;
            this.ColorsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ColorsDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.ColorsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColorsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StartLimbColumn,
            this.ColorColumn});
            this.ColorsDataGridView.Location = new System.Drawing.Point(631, 167);
            this.ColorsDataGridView.MultiSelect = false;
            this.ColorsDataGridView.Name = "ColorsDataGridView";
            this.ColorsDataGridView.RowHeadersVisible = false;
            this.ColorsDataGridView.Size = new System.Drawing.Size(129, 454);
            this.ColorsDataGridView.TabIndex = 75;
            this.ColorsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ColorsDataGridView_CellDoubleClick);
            this.ColorsDataGridView.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.ColorsDataGridView_CellParsing);
            this.ColorsDataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ColorsDataGridView_KeyUp);
            // 
            // StartLimbColumn
            // 
            this.StartLimbColumn.FillWeight = 60F;
            this.StartLimbColumn.HeaderText = "Start limb";
            this.StartLimbColumn.Name = "StartLimbColumn";
            this.StartLimbColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColorColumn
            // 
            this.ColorColumn.FillWeight = 60F;
            this.ColorColumn.HeaderText = "Color";
            this.ColorColumn.Name = "ColorColumn";
            this.ColorColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.Textbox_NPCName.Tag = "NPCNAME";
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
            this.NumUpDown_ZModelOffs.Tag = "ZMODELOFFS";
            this.NumUpDown_ZModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_Hierarchy
            // 
            this.Label_Hierarchy.AutoSize = true;
            this.Label_Hierarchy.Location = new System.Drawing.Point(14, 62);
            this.Label_Hierarchy.Name = "Label_Hierarchy";
            this.Label_Hierarchy.Size = new System.Drawing.Size(108, 13);
            this.Label_Hierarchy.TabIndex = 7;
            this.Label_Hierarchy.Text = "Hierarchy / Skeleton:";
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
            this.NumUpDown_Hierarchy.Tag = "HIERARCHY";
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
            this.NumUpDown_YModelOffs.Tag = "YMODELOFFS";
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
            this.DataGrid_Animations.Size = new System.Drawing.Size(614, 454);
            this.DataGrid_Animations.TabIndex = 9;
            this.DataGrid_Animations.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGrid_Animations_CellMouseDoubleClick);
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
            this.Col_Anim.FillWeight = 50F;
            this.Col_Anim.HeaderText = "Offset";
            this.Col_Anim.Name = "Col_Anim";
            this.Col_Anim.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_StartFrame
            // 
            this.Col_StartFrame.FillWeight = 45F;
            this.Col_StartFrame.HeaderText = "Start frame";
            this.Col_StartFrame.Name = "Col_StartFrame";
            this.Col_StartFrame.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_EndFrame
            // 
            this.Col_EndFrame.FillWeight = 45F;
            this.Col_EndFrame.HeaderText = "End frame";
            this.Col_EndFrame.Name = "Col_EndFrame";
            this.Col_EndFrame.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_Speed
            // 
            this.Col_Speed.FillWeight = 30F;
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
            this.NumUpDown_XModelOffs.Tag = "XMODELOFFS";
            this.NumUpDown_XModelOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // ComboBox_HierarchyType
            // 
            this.ComboBox_HierarchyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_HierarchyType.FormattingEnabled = true;
            this.ComboBox_HierarchyType.Items.AddRange(new object[] {
            "Matrix, full opacity",
            "Matrix, transparency-enabled",
            "Non-matrix, full opacity",
            "Non-matrix, transparency-enabled",
            "Skin, full opacity (Horses)"});
            this.ComboBox_HierarchyType.Location = new System.Drawing.Point(134, 86);
            this.ComboBox_HierarchyType.Name = "ComboBox_HierarchyType";
            this.ComboBox_HierarchyType.Size = new System.Drawing.Size(245, 21);
            this.ComboBox_HierarchyType.TabIndex = 11;
            this.ComboBox_HierarchyType.Tag = "HIERARCHYTYPE";
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
            this.Label_HierarchyType.Size = new System.Drawing.Size(58, 13);
            this.Label_HierarchyType.TabIndex = 12;
            this.Label_HierarchyType.Text = "Draw type:";
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
            this.ComboBox_AnimType.Tag = "ANIMTYPE";
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
            this.NumUpDown_Scale.Tag = "SCALE";
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
            this.Tab2_ExtraData.Size = new System.Drawing.Size(768, 627);
            this.Tab2_ExtraData.TabIndex = 2;
            this.Tab2_ExtraData.Text = "Extra data";
            // 
            // Label_TalkingFramesBetween
            // 
            this.Label_TalkingFramesBetween.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_TalkingFramesBetween.AutoSize = true;
            this.Label_TalkingFramesBetween.Location = new System.Drawing.Point(263, 605);
            this.Label_TalkingFramesBetween.Name = "Label_TalkingFramesBetween";
            this.Label_TalkingFramesBetween.Size = new System.Drawing.Size(123, 13);
            this.Label_TalkingFramesBetween.TabIndex = 65;
            this.Label_TalkingFramesBetween.Text = "Talking frames between:";
            // 
            // Label_BlinkingFramesBetween
            // 
            this.Label_BlinkingFramesBetween.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_BlinkingFramesBetween.AutoSize = true;
            this.Label_BlinkingFramesBetween.Location = new System.Drawing.Point(263, 580);
            this.Label_BlinkingFramesBetween.Name = "Label_BlinkingFramesBetween";
            this.Label_BlinkingFramesBetween.Size = new System.Drawing.Size(125, 13);
            this.Label_BlinkingFramesBetween.TabIndex = 63;
            this.Label_BlinkingFramesBetween.Text = "Blinking frames between:";
            // 
            // Label_TalkingSegment
            // 
            this.Label_TalkingSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_TalkingSegment.AutoSize = true;
            this.Label_TalkingSegment.Location = new System.Drawing.Point(90, 603);
            this.Label_TalkingSegment.Name = "Label_TalkingSegment";
            this.Label_TalkingSegment.Size = new System.Drawing.Size(88, 13);
            this.Label_TalkingSegment.TabIndex = 58;
            this.Label_TalkingSegment.Text = "Talking segment:";
            // 
            // Label_TalkingPattern
            // 
            this.Label_TalkingPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_TalkingPattern.AutoSize = true;
            this.Label_TalkingPattern.Location = new System.Drawing.Point(472, 603);
            this.Label_TalkingPattern.Name = "Label_TalkingPattern";
            this.Label_TalkingPattern.Size = new System.Drawing.Size(81, 13);
            this.Label_TalkingPattern.TabIndex = 61;
            this.Label_TalkingPattern.Text = "Talking pattern:";
            // 
            // Label_BlinkingPattern
            // 
            this.Label_BlinkingPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_BlinkingPattern.AutoSize = true;
            this.Label_BlinkingPattern.Location = new System.Drawing.Point(470, 580);
            this.Label_BlinkingPattern.Name = "Label_BlinkingPattern";
            this.Label_BlinkingPattern.Size = new System.Drawing.Size(83, 13);
            this.Label_BlinkingPattern.TabIndex = 55;
            this.Label_BlinkingPattern.Text = "Blinking pattern:";
            // 
            // Label_BlinkingSegment
            // 
            this.Label_BlinkingSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_BlinkingSegment.AutoSize = true;
            this.Label_BlinkingSegment.Location = new System.Drawing.Point(90, 580);
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
            this.Label_ExtraTextures.Location = new System.Drawing.Point(6, 298);
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
            this.TabControl_Segments.Location = new System.Drawing.Point(9, 314);
            this.TabControl_Segments.Name = "TabControl_Segments";
            this.TabControl_Segments.SelectedIndex = 0;
            this.TabControl_Segments.Size = new System.Drawing.Size(756, 255);
            this.TabControl_Segments.TabIndex = 41;
            // 
            // TabPage_Segment_8
            // 
            this.TabPage_Segment_8.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_8.Name = "TabPage_Segment_8";
            this.TabPage_Segment_8.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_8.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_8.TabIndex = 0;
            this.TabPage_Segment_8.Text = "Segment 8";
            this.TabPage_Segment_8.UseVisualStyleBackColor = true;
            // 
            // TabPage_Segment_9
            // 
            this.TabPage_Segment_9.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_9.Name = "TabPage_Segment_9";
            this.TabPage_Segment_9.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_9.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_9.TabIndex = 1;
            this.TabPage_Segment_9.Text = "Segment 9";
            this.TabPage_Segment_9.UseVisualStyleBackColor = true;
            // 
            // TabPage_Segment_A
            // 
            this.TabPage_Segment_A.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_A.Name = "TabPage_Segment_A";
            this.TabPage_Segment_A.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_A.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_A.TabIndex = 2;
            this.TabPage_Segment_A.Text = "Segment A";
            this.TabPage_Segment_A.UseVisualStyleBackColor = true;
            // 
            // TabPage_Segment_B
            // 
            this.TabPage_Segment_B.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_B.Name = "TabPage_Segment_B";
            this.TabPage_Segment_B.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_B.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_B.TabIndex = 3;
            this.TabPage_Segment_B.Text = "Segment B";
            this.TabPage_Segment_B.UseVisualStyleBackColor = true;
            // 
            // TabPage_Segment_C
            // 
            this.TabPage_Segment_C.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_C.Name = "TabPage_Segment_C";
            this.TabPage_Segment_C.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_C.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_C.TabIndex = 4;
            this.TabPage_Segment_C.Text = "Segment C";
            this.TabPage_Segment_C.UseVisualStyleBackColor = true;
            // 
            // TabPage_Segment_D
            // 
            this.TabPage_Segment_D.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_D.Name = "TabPage_Segment_D";
            this.TabPage_Segment_D.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_D.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_D.TabIndex = 5;
            this.TabPage_Segment_D.Text = "Segment D";
            this.TabPage_Segment_D.UseVisualStyleBackColor = true;
            // 
            // TabPage_Segment_E
            // 
            this.TabPage_Segment_E.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_E.Name = "TabPage_Segment_E";
            this.TabPage_Segment_E.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_E.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_E.TabIndex = 6;
            this.TabPage_Segment_E.Text = "Segment E";
            this.TabPage_Segment_E.UseVisualStyleBackColor = true;
            // 
            // TabPage_Segment_F
            // 
            this.TabPage_Segment_F.Location = new System.Drawing.Point(4, 22);
            this.TabPage_Segment_F.Name = "TabPage_Segment_F";
            this.TabPage_Segment_F.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Segment_F.Size = new System.Drawing.Size(748, 229);
            this.TabPage_Segment_F.TabIndex = 7;
            this.TabPage_Segment_F.Text = "Segment F";
            this.TabPage_Segment_F.UseVisualStyleBackColor = true;
            // 
            // NumUpDown_TalkSegment
            // 
            this.NumUpDown_TalkSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUpDown_TalkSegment.Hexadecimal = true;
            this.NumUpDown_TalkSegment.Location = new System.Drawing.Point(186, 601);
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
            this.NumUpDown_BlinkSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUpDown_BlinkSegment.Hexadecimal = true;
            this.NumUpDown_BlinkSegment.Location = new System.Drawing.Point(186, 576);
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
            this.NumUpDown_BlinkSegment.Tag = "BLINKSEG";
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
            this.NumUpDown_TalkSpeed.Location = new System.Drawing.Point(394, 602);
            this.NumUpDown_TalkSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_TalkSpeed.Name = "NumUpDown_TalkSpeed";
            this.NumUpDown_TalkSpeed.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_TalkSpeed.TabIndex = 57;
            this.NumUpDown_TalkSpeed.Tag = "TALKSPE";
            this.NumUpDown_TalkSpeed.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Textbox_BlinkPattern
            // 
            this.Textbox_BlinkPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Textbox_BlinkPattern.Location = new System.Drawing.Point(559, 577);
            this.Textbox_BlinkPattern.Name = "Textbox_BlinkPattern";
            this.Textbox_BlinkPattern.Size = new System.Drawing.Size(206, 20);
            this.Textbox_BlinkPattern.TabIndex = 60;
            this.Textbox_BlinkPattern.Tag = "BLINKPAT";
            this.Textbox_BlinkPattern.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // Textbox_TalkingPattern
            // 
            this.Textbox_TalkingPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Textbox_TalkingPattern.Location = new System.Drawing.Point(559, 601);
            this.Textbox_TalkingPattern.Name = "Textbox_TalkingPattern";
            this.Textbox_TalkingPattern.Size = new System.Drawing.Size(206, 20);
            this.Textbox_TalkingPattern.TabIndex = 59;
            this.Textbox_TalkingPattern.Tag = "TALKPAT";
            this.Textbox_TalkingPattern.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // NumUpDown_BlinkSpeed
            // 
            this.NumUpDown_BlinkSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUpDown_BlinkSpeed.Location = new System.Drawing.Point(394, 578);
            this.NumUpDown_BlinkSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_BlinkSpeed.Name = "NumUpDown_BlinkSpeed";
            this.NumUpDown_BlinkSpeed.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_BlinkSpeed.TabIndex = 54;
            this.NumUpDown_BlinkSpeed.Tag = "BLINKSPE";
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
            this.ExtraDlists_Color,
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
            this.DataGridView_ExtraDLists.Size = new System.Drawing.Size(749, 276);
            this.DataGridView_ExtraDLists.TabIndex = 51;
            this.DataGridView_ExtraDLists.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView_ExtraDLists_CellMouseDoubleClick);
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
            // ExtraDlists_Color
            // 
            this.ExtraDlists_Color.FillWeight = 30F;
            this.ExtraDlists_Color.HeaderText = "Color";
            this.ExtraDlists_Color.Name = "ExtraDlists_Color";
            this.ExtraDlists_Color.ReadOnly = true;
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
            this.ExtraDlists_Translation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtraDlists_Rotation
            // 
            this.ExtraDlists_Rotation.FillWeight = 60F;
            this.ExtraDlists_Rotation.HeaderText = "X,Y,Z Rot.";
            this.ExtraDlists_Rotation.Name = "ExtraDlists_Rotation";
            this.ExtraDlists_Rotation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtraDLists_Scale
            // 
            this.ExtraDLists_Scale.FillWeight = 40F;
            this.ExtraDLists_Scale.HeaderText = "Scale";
            this.ExtraDLists_Scale.Name = "ExtraDLists_Scale";
            this.ExtraDLists_Scale.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtraDlists_Limb
            // 
            this.ExtraDlists_Limb.FillWeight = 35F;
            this.ExtraDlists_Limb.HeaderText = "Limb";
            this.ExtraDlists_Limb.Name = "ExtraDlists_Limb";
            this.ExtraDlists_Limb.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtraDlists_ObjectID
            // 
            this.ExtraDlists_ObjectID.FillWeight = 60F;
            this.ExtraDlists_ObjectID.HeaderText = "Object ID";
            this.ExtraDlists_ObjectID.Name = "ExtraDlists_ObjectID";
            this.ExtraDlists_ObjectID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ExtraDlists_ShowType
            // 
            this.ExtraDlists_ShowType.FillWeight = 80F;
            this.ExtraDlists_ShowType.HeaderText = "Show type";
            this.ExtraDlists_ShowType.Items.AddRange(new object[] {
            "Not visible",
            "With limb",
            "Replaces limb"});
            this.ExtraDlists_ShowType.Name = "ExtraDlists_ShowType";
            this.ExtraDlists_ShowType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Tab3_BehaviorData
            // 
            this.Tab3_BehaviorData.BackColor = System.Drawing.Color.White;
            this.Tab3_BehaviorData.Controls.Add(this.NumUpDown_ScriptsFVar);
            this.Tab3_BehaviorData.Controls.Add(this.NumUpDown_ScriptsVar);
            this.Tab3_BehaviorData.Controls.Add(this.Lbl_ScriptsFVars);
            this.Tab3_BehaviorData.Controls.Add(this.Lbl_ScriptsVars);
            this.Tab3_BehaviorData.Controls.Add(this.ChkInvisible);
            this.Tab3_BehaviorData.Controls.Add(this.ChkOnlyWhenLens);
            this.Tab3_BehaviorData.Controls.Add(this.ChkBox_DebugCol);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_Colors);
            this.Tab3_BehaviorData.Controls.Add(this.NumUp_RiddenBy);
            this.Tab3_BehaviorData.Controls.Add(this.Label_RiddenBy);
            this.Tab3_BehaviorData.Controls.Add(this.ChkB_FadeOut);
            this.Tab3_BehaviorData.Controls.Add(this.Btn_ReactIfAtt);
            this.Tab3_BehaviorData.Controls.Add(this.Chkb_Opendoors);
            this.Tab3_BehaviorData.Controls.Add(this.ChkRunJustScript);
            this.Tab3_BehaviorData.Controls.Add(this.Lbl_Misc);
            this.Tab3_BehaviorData.Controls.Add(this.Label_CutsceneSlot);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_Collision);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_Shadow);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_HeadRot);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_TargetPanel);
            this.Tab3_BehaviorData.Controls.Add(this.Panel_Movement);
            this.Tab3_BehaviorData.Controls.Add(this.Checkbox_AlwaysDraw);
            this.Tab3_BehaviorData.Controls.Add(this.NumUpDown_CutsceneSlot);
            this.Tab3_BehaviorData.Controls.Add(this.Checkbox_AlwaysActive);
            this.Tab3_BehaviorData.Controls.Add(this.Checkbox_CanPressSwitches);
            this.Tab3_BehaviorData.Location = new System.Drawing.Point(4, 22);
            this.Tab3_BehaviorData.Name = "Tab3_BehaviorData";
            this.Tab3_BehaviorData.Padding = new System.Windows.Forms.Padding(3);
            this.Tab3_BehaviorData.Size = new System.Drawing.Size(768, 627);
            this.Tab3_BehaviorData.TabIndex = 4;
            this.Tab3_BehaviorData.Text = "Behavior";
            // 
            // NumUpDown_ScriptsFVar
            // 
            this.NumUpDown_ScriptsFVar.Location = new System.Drawing.Point(526, 87);
            this.NumUpDown_ScriptsFVar.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_ScriptsFVar.Name = "NumUpDown_ScriptsFVar";
            this.NumUpDown_ScriptsFVar.Size = new System.Drawing.Size(71, 20);
            this.NumUpDown_ScriptsFVar.TabIndex = 86;
            this.NumUpDown_ScriptsFVar.Tag = "SCRIPTFVARS";
            this.NumUpDown_ScriptsFVar.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_ScriptsVar
            // 
            this.NumUpDown_ScriptsVar.Location = new System.Drawing.Point(422, 87);
            this.NumUpDown_ScriptsVar.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_ScriptsVar.Name = "NumUpDown_ScriptsVar";
            this.NumUpDown_ScriptsVar.Size = new System.Drawing.Size(71, 20);
            this.NumUpDown_ScriptsVar.TabIndex = 85;
            this.NumUpDown_ScriptsVar.Tag = "SCRIPTVARS";
            this.NumUpDown_ScriptsVar.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Lbl_ScriptsFVars
            // 
            this.Lbl_ScriptsFVars.AutoSize = true;
            this.Lbl_ScriptsFVars.Location = new System.Drawing.Point(523, 71);
            this.Lbl_ScriptsFVars.Name = "Lbl_ScriptsFVars";
            this.Lbl_ScriptsFVars.Size = new System.Drawing.Size(110, 13);
            this.Lbl_ScriptsFVars.TabIndex = 84;
            this.Lbl_ScriptsFVars.Text = "Scripts float variables:";
            // 
            // Lbl_ScriptsVars
            // 
            this.Lbl_ScriptsVars.AutoSize = true;
            this.Lbl_ScriptsVars.Location = new System.Drawing.Point(419, 71);
            this.Lbl_ScriptsVars.Name = "Lbl_ScriptsVars";
            this.Lbl_ScriptsVars.Size = new System.Drawing.Size(87, 13);
            this.Lbl_ScriptsVars.TabIndex = 83;
            this.Lbl_ScriptsVars.Text = "Scripts variables:";
            // 
            // ChkInvisible
            // 
            this.ChkInvisible.AutoSize = true;
            this.ChkInvisible.Location = new System.Drawing.Point(584, 212);
            this.ChkInvisible.Name = "ChkInvisible";
            this.ChkInvisible.Size = new System.Drawing.Size(64, 17);
            this.ChkInvisible.TabIndex = 82;
            this.ChkInvisible.Tag = "INVISIBLE";
            this.ChkInvisible.Text = "Invisible";
            this.ChkInvisible.UseVisualStyleBackColor = true;
            this.ChkInvisible.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // ChkOnlyWhenLens
            // 
            this.ChkOnlyWhenLens.AutoSize = true;
            this.ChkOnlyWhenLens.Location = new System.Drawing.Point(422, 212);
            this.ChkOnlyWhenLens.Name = "ChkOnlyWhenLens";
            this.ChkOnlyWhenLens.Size = new System.Drawing.Size(146, 17);
            this.ChkOnlyWhenLens.TabIndex = 81;
            this.ChkOnlyWhenLens.Tag = "VISIBLEONLYLENS";
            this.ChkOnlyWhenLens.Text = "Affected by Lens of Truth";
            this.ChkOnlyWhenLens.UseVisualStyleBackColor = true;
            this.ChkOnlyWhenLens.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // ChkBox_DebugCol
            // 
            this.ChkBox_DebugCol.AutoSize = true;
            this.ChkBox_DebugCol.Location = new System.Drawing.Point(422, 237);
            this.ChkBox_DebugCol.Name = "ChkBox_DebugCol";
            this.ChkBox_DebugCol.Size = new System.Drawing.Size(138, 17);
            this.ChkBox_DebugCol.TabIndex = 80;
            this.ChkBox_DebugCol.Tag = "DEBUGSHOWCOLS";
            this.ChkBox_DebugCol.Text = "Draw collision (DEBUG)";
            this.ChkBox_DebugCol.UseVisualStyleBackColor = true;
            this.ChkBox_DebugCol.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Panel_Colors
            // 
            this.Panel_Colors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Colors.Controls.Add(this.Lbl_Radius);
            this.Panel_Colors.Controls.Add(this.NumUp_LightRadius);
            this.Panel_Colors.Controls.Add(this.NumUp_LightZOffs);
            this.Panel_Colors.Controls.Add(this.NumUp_LightYOffs);
            this.Panel_Colors.Controls.Add(this.Lbl_LightColor);
            this.Panel_Colors.Controls.Add(this.Btn_LightColor);
            this.Panel_Colors.Controls.Add(this.NumUp_LightXOffs);
            this.Panel_Colors.Controls.Add(this.Label_LightLimbOffset);
            this.Panel_Colors.Controls.Add(this.Label_LightLimb);
            this.Panel_Colors.Controls.Add(this.ChkBox_Glow);
            this.Panel_Colors.Controls.Add(this.NumUp_LightLimb);
            this.Panel_Colors.Controls.Add(this.ChkBox_GenLight);
            this.Panel_Colors.Location = new System.Drawing.Point(419, 372);
            this.Panel_Colors.Name = "Panel_Colors";
            this.Panel_Colors.Size = new System.Drawing.Size(341, 134);
            this.Panel_Colors.TabIndex = 77;
            // 
            // Lbl_Radius
            // 
            this.Lbl_Radius.AutoSize = true;
            this.Lbl_Radius.Location = new System.Drawing.Point(185, 34);
            this.Lbl_Radius.Name = "Lbl_Radius";
            this.Lbl_Radius.Size = new System.Drawing.Size(43, 13);
            this.Lbl_Radius.TabIndex = 84;
            this.Lbl_Radius.Text = "Radius:";
            // 
            // NumUp_LightRadius
            // 
            this.NumUp_LightRadius.Location = new System.Drawing.Point(234, 32);
            this.NumUp_LightRadius.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUp_LightRadius.Name = "NumUp_LightRadius";
            this.NumUp_LightRadius.Size = new System.Drawing.Size(59, 20);
            this.NumUp_LightRadius.TabIndex = 83;
            this.NumUp_LightRadius.Tag = "LIGHTRADIUS";
            this.NumUp_LightRadius.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUp_LightZOffs
            // 
            this.NumUp_LightZOffs.Location = new System.Drawing.Point(123, 103);
            this.NumUp_LightZOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUp_LightZOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUp_LightZOffs.Name = "NumUp_LightZOffs";
            this.NumUp_LightZOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUp_LightZOffs.TabIndex = 80;
            this.NumUp_LightZOffs.Tag = "ZLIGHTOFFS";
            this.NumUp_LightZOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUp_LightYOffs
            // 
            this.NumUp_LightYOffs.Location = new System.Drawing.Point(63, 103);
            this.NumUp_LightYOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUp_LightYOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUp_LightYOffs.Name = "NumUp_LightYOffs";
            this.NumUp_LightYOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUp_LightYOffs.TabIndex = 79;
            this.NumUp_LightYOffs.Tag = "YLIGHTOFFS";
            this.NumUp_LightYOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Lbl_LightColor
            // 
            this.Lbl_LightColor.AutoSize = true;
            this.Lbl_LightColor.Location = new System.Drawing.Point(185, 7);
            this.Lbl_LightColor.Name = "Lbl_LightColor";
            this.Lbl_LightColor.Size = new System.Drawing.Size(34, 13);
            this.Lbl_LightColor.TabIndex = 82;
            this.Lbl_LightColor.Text = "Color:";
            // 
            // Btn_LightColor
            // 
            this.Btn_LightColor.BackColor = System.Drawing.Color.Black;
            this.Btn_LightColor.Location = new System.Drawing.Point(234, 3);
            this.Btn_LightColor.Name = "Btn_LightColor";
            this.Btn_LightColor.Size = new System.Drawing.Size(59, 23);
            this.Btn_LightColor.TabIndex = 81;
            this.Btn_LightColor.UseVisualStyleBackColor = false;
            this.Btn_LightColor.Click += new System.EventHandler(this.Btn_LightColor_Click);
            // 
            // NumUp_LightXOffs
            // 
            this.NumUp_LightXOffs.Location = new System.Drawing.Point(3, 103);
            this.NumUp_LightXOffs.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NumUp_LightXOffs.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
            this.NumUp_LightXOffs.Name = "NumUp_LightXOffs";
            this.NumUp_LightXOffs.Size = new System.Drawing.Size(54, 20);
            this.NumUp_LightXOffs.TabIndex = 78;
            this.NumUp_LightXOffs.Tag = "XLIGHTOFFS";
            this.NumUp_LightXOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_LightLimbOffset
            // 
            this.Label_LightLimbOffset.AutoSize = true;
            this.Label_LightLimbOffset.Location = new System.Drawing.Point(0, 85);
            this.Label_LightLimbOffset.Name = "Label_LightLimbOffset";
            this.Label_LightLimbOffset.Size = new System.Drawing.Size(38, 13);
            this.Label_LightLimbOffset.TabIndex = 77;
            this.Label_LightLimbOffset.Text = "Offset:";
            // 
            // Label_LightLimb
            // 
            this.Label_LightLimb.AutoSize = true;
            this.Label_LightLimb.Location = new System.Drawing.Point(0, 57);
            this.Label_LightLimb.Name = "Label_LightLimb";
            this.Label_LightLimb.Size = new System.Drawing.Size(32, 13);
            this.Label_LightLimb.TabIndex = 54;
            this.Label_LightLimb.Text = "Limb:";
            // 
            // ChkBox_Glow
            // 
            this.ChkBox_Glow.AutoSize = true;
            this.ChkBox_Glow.Location = new System.Drawing.Point(3, 30);
            this.ChkBox_Glow.Name = "ChkBox_Glow";
            this.ChkBox_Glow.Size = new System.Drawing.Size(50, 17);
            this.ChkBox_Glow.TabIndex = 76;
            this.ChkBox_Glow.Tag = "GLOW";
            this.ChkBox_Glow.Text = "Glow";
            this.ChkBox_Glow.UseVisualStyleBackColor = true;
            this.ChkBox_Glow.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUp_LightLimb
            // 
            this.NumUp_LightLimb.Location = new System.Drawing.Point(63, 55);
            this.NumUp_LightLimb.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.NumUp_LightLimb.Name = "NumUp_LightLimb";
            this.NumUp_LightLimb.Size = new System.Drawing.Size(54, 20);
            this.NumUp_LightLimb.TabIndex = 55;
            this.NumUp_LightLimb.Tag = "LIGHTLIMB";
            this.NumUp_LightLimb.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // ChkBox_GenLight
            // 
            this.ChkBox_GenLight.AutoSize = true;
            this.ChkBox_GenLight.Location = new System.Drawing.Point(3, 5);
            this.ChkBox_GenLight.Name = "ChkBox_GenLight";
            this.ChkBox_GenLight.Size = new System.Drawing.Size(92, 17);
            this.ChkBox_GenLight.TabIndex = 72;
            this.ChkBox_GenLight.Tag = "LIGHT";
            this.ChkBox_GenLight.Text = "Generate light";
            this.ChkBox_GenLight.UseVisualStyleBackColor = true;
            this.ChkBox_GenLight.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUp_RiddenBy
            // 
            this.NumUp_RiddenBy.Location = new System.Drawing.Point(526, 36);
            this.NumUp_RiddenBy.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUp_RiddenBy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUp_RiddenBy.Name = "NumUp_RiddenBy";
            this.NumUp_RiddenBy.Size = new System.Drawing.Size(71, 20);
            this.NumUp_RiddenBy.TabIndex = 79;
            this.NumUp_RiddenBy.Tag = "NPCTORIDE";
            this.NumUp_RiddenBy.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUp_RiddenBy.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_RiddenBy
            // 
            this.Label_RiddenBy.AutoSize = true;
            this.Label_RiddenBy.Location = new System.Drawing.Point(523, 15);
            this.Label_RiddenBy.Name = "Label_RiddenBy";
            this.Label_RiddenBy.Size = new System.Drawing.Size(78, 13);
            this.Label_RiddenBy.TabIndex = 78;
            this.Label_RiddenBy.Text = "NPC ID to ride:";
            // 
            // ChkB_FadeOut
            // 
            this.ChkB_FadeOut.AutoSize = true;
            this.ChkB_FadeOut.Location = new System.Drawing.Point(584, 164);
            this.ChkB_FadeOut.Name = "ChkB_FadeOut";
            this.ChkB_FadeOut.Size = new System.Drawing.Size(150, 17);
            this.ChkB_FadeOut.TabIndex = 77;
            this.ChkB_FadeOut.Tag = "FADEOUT";
            this.ChkB_FadeOut.Text = "Fade out if player far away";
            this.ChkB_FadeOut.UseVisualStyleBackColor = true;
            this.ChkB_FadeOut.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Btn_ReactIfAtt
            // 
            this.Btn_ReactIfAtt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Btn_ReactIfAtt.Controls.Add(this.Combo_EffIfAtt);
            this.Btn_ReactIfAtt.Controls.Add(this.Label_EffIfAtt);
            this.Btn_ReactIfAtt.Controls.Add(this.Btn_ReactIfAttList);
            this.Btn_ReactIfAtt.Controls.Add(this.Txtbox_ReactIfAtt);
            this.Btn_ReactIfAtt.Controls.Add(this.Lbl_ReactIfAttSnd);
            this.Btn_ReactIfAtt.Controls.Add(this.Chkb_ReactIfAtt);
            this.Btn_ReactIfAtt.Location = new System.Drawing.Point(419, 279);
            this.Btn_ReactIfAtt.Name = "Btn_ReactIfAtt";
            this.Btn_ReactIfAtt.Size = new System.Drawing.Size(341, 87);
            this.Btn_ReactIfAtt.TabIndex = 76;
            // 
            // Combo_EffIfAtt
            // 
            this.Combo_EffIfAtt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_EffIfAtt.FormattingEnabled = true;
            this.Combo_EffIfAtt.Items.AddRange(new object[] {
            "Blue blood, white effect",
            "No blood, dust",
            "Green blood, dust",
            "No blood, white effect",
            "Water burst, no effect",
            "No blood, red effect",
            "Green blood, white effect",
            "Red blood, white effect",
            "Blue blood, red effect",
            "Metal",
            "None",
            "Wood",
            "Hard surface",
            "Tree"});
            this.Combo_EffIfAtt.Location = new System.Drawing.Point(77, 55);
            this.Combo_EffIfAtt.Name = "Combo_EffIfAtt";
            this.Combo_EffIfAtt.Size = new System.Drawing.Size(244, 21);
            this.Combo_EffIfAtt.TabIndex = 69;
            this.Combo_EffIfAtt.Tag = "EFFIFATT";
            this.Combo_EffIfAtt.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ValueChanged);
            // 
            // Label_EffIfAtt
            // 
            this.Label_EffIfAtt.AutoSize = true;
            this.Label_EffIfAtt.Location = new System.Drawing.Point(3, 58);
            this.Label_EffIfAtt.Name = "Label_EffIfAtt";
            this.Label_EffIfAtt.Size = new System.Drawing.Size(53, 13);
            this.Label_EffIfAtt.TabIndex = 75;
            this.Label_EffIfAtt.Text = "Hit effect:";
            // 
            // Btn_ReactIfAttList
            // 
            this.Btn_ReactIfAttList.Location = new System.Drawing.Point(256, 28);
            this.Btn_ReactIfAttList.Name = "Btn_ReactIfAttList";
            this.Btn_ReactIfAttList.Size = new System.Drawing.Size(65, 20);
            this.Btn_ReactIfAttList.TabIndex = 74;
            this.Btn_ReactIfAttList.Text = "List";
            this.Btn_ReactIfAttList.UseVisualStyleBackColor = true;
            this.Btn_ReactIfAttList.Click += new System.EventHandler(this.Btn_ReactIfAttList_Click);
            // 
            // Txtbox_ReactIfAtt
            // 
            this.Txtbox_ReactIfAtt.Location = new System.Drawing.Point(77, 28);
            this.Txtbox_ReactIfAtt.MaxLength = 32;
            this.Txtbox_ReactIfAtt.Multiline = true;
            this.Txtbox_ReactIfAtt.Name = "Txtbox_ReactIfAtt";
            this.Txtbox_ReactIfAtt.Size = new System.Drawing.Size(173, 20);
            this.Txtbox_ReactIfAtt.TabIndex = 73;
            this.Txtbox_ReactIfAtt.Tag = "";
            this.Txtbox_ReactIfAtt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Txb_ReactIfAtt_KeyUp);
            this.Txtbox_ReactIfAtt.Leave += new System.EventHandler(this.Txtbox_ReactIfAtt_Leave);
            // 
            // Lbl_ReactIfAttSnd
            // 
            this.Lbl_ReactIfAttSnd.AutoSize = true;
            this.Lbl_ReactIfAttSnd.Location = new System.Drawing.Point(3, 31);
            this.Lbl_ReactIfAttSnd.Name = "Lbl_ReactIfAttSnd";
            this.Lbl_ReactIfAttSnd.Size = new System.Drawing.Size(71, 13);
            this.Lbl_ReactIfAttSnd.TabIndex = 69;
            this.Lbl_ReactIfAttSnd.Text = "Sound effect:";
            // 
            // Chkb_ReactIfAtt
            // 
            this.Chkb_ReactIfAtt.AutoSize = true;
            this.Chkb_ReactIfAtt.Location = new System.Drawing.Point(3, 5);
            this.Chkb_ReactIfAtt.Name = "Chkb_ReactIfAtt";
            this.Chkb_ReactIfAtt.Size = new System.Drawing.Size(108, 17);
            this.Chkb_ReactIfAtt.TabIndex = 72;
            this.Chkb_ReactIfAtt.Tag = "REACTATT";
            this.Chkb_ReactIfAtt.Text = "React if attacked";
            this.Chkb_ReactIfAtt.UseVisualStyleBackColor = true;
            this.Chkb_ReactIfAtt.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Chkb_Opendoors
            // 
            this.Chkb_Opendoors.AutoSize = true;
            this.Chkb_Opendoors.Location = new System.Drawing.Point(584, 142);
            this.Chkb_Opendoors.Name = "Chkb_Opendoors";
            this.Chkb_Opendoors.Size = new System.Drawing.Size(167, 17);
            this.Chkb_Opendoors.TabIndex = 74;
            this.Chkb_Opendoors.Tag = "OPENDOORS";
            this.Chkb_Opendoors.Text = "Opens doors if they\'re on path";
            this.Chkb_Opendoors.UseVisualStyleBackColor = true;
            this.Chkb_Opendoors.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // ChkRunJustScript
            // 
            this.ChkRunJustScript.AutoSize = true;
            this.ChkRunJustScript.Location = new System.Drawing.Point(584, 187);
            this.ChkRunJustScript.Name = "ChkRunJustScript";
            this.ChkRunJustScript.Size = new System.Drawing.Size(91, 17);
            this.ChkRunJustScript.TabIndex = 73;
            this.ChkRunJustScript.Tag = "JUSTSCRIPT";
            this.ChkRunJustScript.Text = "Just run script";
            this.ChkRunJustScript.UseVisualStyleBackColor = true;
            this.ChkRunJustScript.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Lbl_Misc
            // 
            this.Lbl_Misc.AutoSize = true;
            this.Lbl_Misc.Location = new System.Drawing.Point(419, 121);
            this.Lbl_Misc.Name = "Lbl_Misc";
            this.Lbl_Misc.Size = new System.Drawing.Size(32, 13);
            this.Lbl_Misc.TabIndex = 71;
            this.Lbl_Misc.Text = "Misc:";
            // 
            // Label_CutsceneSlot
            // 
            this.Label_CutsceneSlot.AutoSize = true;
            this.Label_CutsceneSlot.Location = new System.Drawing.Point(419, 15);
            this.Label_CutsceneSlot.Name = "Label_CutsceneSlot";
            this.Label_CutsceneSlot.Size = new System.Drawing.Size(74, 13);
            this.Label_CutsceneSlot.TabIndex = 64;
            this.Label_CutsceneSlot.Text = "Cutscene slot:";
            // 
            // Panel_Collision
            // 
            this.Panel_Collision.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Collision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Collision.Controls.Add(this.NumUpDown_Mass);
            this.Panel_Collision.Controls.Add(this.Lbl_Mass);
            this.Panel_Collision.Controls.Add(this.NumUpDown_YColOffs);
            this.Panel_Collision.Controls.Add(this.Label_ColOffs);
            this.Panel_Collision.Controls.Add(this.Label_ColHeight);
            this.Panel_Collision.Controls.Add(this.NumUpDown_ColHeight);
            this.Panel_Collision.Controls.Add(this.NumUpDown_ColRadius);
            this.Panel_Collision.Controls.Add(this.Label_ColRadius);
            this.Panel_Collision.Controls.Add(this.Checkbox_HaveCollision);
            this.Panel_Collision.Location = new System.Drawing.Point(7, 444);
            this.Panel_Collision.Name = "Panel_Collision";
            this.Panel_Collision.Size = new System.Drawing.Size(200, 176);
            this.Panel_Collision.TabIndex = 63;
            // 
            // NumUpDown_Mass
            // 
            this.NumUpDown_Mass.Location = new System.Drawing.Point(126, 106);
            this.NumUpDown_Mass.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_Mass.Name = "NumUpDown_Mass";
            this.NumUpDown_Mass.Size = new System.Drawing.Size(61, 20);
            this.NumUpDown_Mass.TabIndex = 77;
            this.NumUpDown_Mass.Tag = "MASS";
            this.NumUpDown_Mass.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Lbl_Mass
            // 
            this.Lbl_Mass.AutoSize = true;
            this.Lbl_Mass.Location = new System.Drawing.Point(5, 108);
            this.Lbl_Mass.Name = "Lbl_Mass";
            this.Lbl_Mass.Size = new System.Drawing.Size(35, 13);
            this.Lbl_Mass.TabIndex = 76;
            this.Lbl_Mass.Text = "Mass:";
            // 
            // NumUpDown_YColOffs
            // 
            this.NumUpDown_YColOffs.Location = new System.Drawing.Point(126, 82);
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
            this.NumUpDown_YColOffs.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_YColOffs.TabIndex = 33;
            this.NumUpDown_YColOffs.Tag = "YCOLOFFS";
            this.NumUpDown_YColOffs.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_ColOffs
            // 
            this.Label_ColOffs.AutoSize = true;
            this.Label_ColOffs.Location = new System.Drawing.Point(6, 84);
            this.Label_ColOffs.Name = "Label_ColOffs";
            this.Label_ColOffs.Size = new System.Drawing.Size(48, 13);
            this.Label_ColOffs.TabIndex = 31;
            this.Label_ColOffs.Text = "Y Offset:";
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
            this.NumUpDown_ColHeight.Tag = "COLHEIGHT";
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
            this.NumUpDown_ColRadius.Tag = "COLRADIUS";
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
            this.Checkbox_HaveCollision.Tag = "COLLISION";
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
            this.Panel_Shadow.Location = new System.Drawing.Point(7, 383);
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
            this.NumUpDown_ShRadius.Tag = "SHADOWRADIUS";
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
            this.Checkbox_DrawShadow.Tag = "SHADOW";
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
            this.Panel_HeadRot.Size = new System.Drawing.Size(200, 369);
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
            this.NumUpDown_LookAt_Z.Tag = "ZLOOKATOFFS";
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
            this.NumUpDown_LookAt_Y.Tag = "YLOOKATOFFS";
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
            this.Combo_Waist_Horiz.Tag = "WAISTHORIZAXIS";
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
            this.NumUpDown_LookAt_X.Tag = "XLOOKATOFFS";
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
            this.Combo_Waist_Vert.Tag = "WAISTVERTAXIS";
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
            this.NumUpDown_DegVert.Tag = "DEGVERT";
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
            128,
            0,
            0,
            0});
            this.NumUpDown_WaistLimb.Name = "NumUpDown_WaistLimb";
            this.NumUpDown_WaistLimb.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_WaistLimb.TabIndex = 38;
            this.NumUpDown_WaistLimb.Tag = "WAISTLIMB";
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
            this.ComboBox_LookAtType.Tag = "LOOKATTYPE";
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
            this.NumUpDown_DegHoz.Tag = "DEGHOZ";
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
            this.Combo_Head_Horiz.Tag = "HEADHORIZAXIS";
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
            this.Combo_Head_Vert.Tag = "HEADVERTAXIS";
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
            128,
            0,
            0,
            0});
            this.NumUpDown_HeadLimb.Name = "NumUpDown_HeadLimb";
            this.NumUpDown_HeadLimb.Size = new System.Drawing.Size(60, 20);
            this.NumUpDown_HeadLimb.TabIndex = 23;
            this.NumUpDown_HeadLimb.Tag = "HEADLIMB";
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
            this.Panel_TargetPanel.Location = new System.Drawing.Point(213, 383);
            this.Panel_TargetPanel.Name = "Panel_TargetPanel";
            this.Panel_TargetPanel.Size = new System.Drawing.Size(200, 237);
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
            this.NumUpDown_TalkRadi.Tag = "TALKRADIUS";
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
            this.NumUpDown_ZTargetOffs.Tag = "ZTARGETOFFS";
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
            this.ComboBox_TargetDist.Tag = "TARGETDIST";
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
            this.NumUpDown_YTargetOffs.Tag = "YTARGETOFFS";
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
            this.NumUpDown_XTargetOffs.Tag = "XTARGETOFFS";
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
            this.Checkbox_Targettable.Tag = "TARGETTABLE";
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
            this.NumUpDown_TargetLimb.Tag = "TARGETLIMB";
            this.NumUpDown_TargetLimb.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Panel_Movement
            // 
            this.Panel_Movement.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Movement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Movement.Controls.Add(this.label2);
            this.Panel_Movement.Controls.Add(this.NumUp_MaxRoam);
            this.Panel_Movement.Controls.Add(this.Chkb_IgnoreY);
            this.Panel_Movement.Controls.Add(this.SmoothingCnts);
            this.Panel_Movement.Controls.Add(this.NumUp_Smoothing);
            this.Panel_Movement.Controls.Add(this.Lb_PathEnd);
            this.Panel_Movement.Controls.Add(this.tmpicker_timedPathStart);
            this.Panel_Movement.Controls.Add(this.Label_PathStTime);
            this.Panel_Movement.Controls.Add(this.tmpicker_timedPathEnd);
            this.Panel_Movement.Controls.Add(this.Lbl_GravityForce);
            this.Panel_Movement.Controls.Add(this.Label_LoopDelay);
            this.Panel_Movement.Controls.Add(this.Label_LoopStartNode);
            this.Panel_Movement.Controls.Add(this.Checkbox_Loop);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopStartNode);
            this.Panel_Movement.Controls.Add(this.NumUpDown_GravityForce);
            this.Panel_Movement.Controls.Add(this.NumUpDown_LoopDelay);
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
            this.Panel_Movement.Location = new System.Drawing.Point(213, 8);
            this.Panel_Movement.Name = "Panel_Movement";
            this.Panel_Movement.Size = new System.Drawing.Size(200, 369);
            this.Panel_Movement.TabIndex = 65;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "(Roam) Max dist:";
            // 
            // NumUp_MaxRoam
            // 
            this.NumUp_MaxRoam.Location = new System.Drawing.Point(123, 84);
            this.NumUp_MaxRoam.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUp_MaxRoam.Name = "NumUp_MaxRoam";
            this.NumUp_MaxRoam.Size = new System.Drawing.Size(66, 20);
            this.NumUp_MaxRoam.TabIndex = 69;
            this.NumUp_MaxRoam.Tag = "ROAMMAX";
            this.NumUp_MaxRoam.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Chkb_IgnoreY
            // 
            this.Chkb_IgnoreY.AutoSize = true;
            this.Chkb_IgnoreY.Location = new System.Drawing.Point(74, 344);
            this.Chkb_IgnoreY.Name = "Chkb_IgnoreY";
            this.Chkb_IgnoreY.Size = new System.Drawing.Size(115, 17);
            this.Chkb_IgnoreY.TabIndex = 67;
            this.Chkb_IgnoreY.Tag = "IGNORENODEYAXIS";
            this.Chkb_IgnoreY.Text = "Ignore node Y Axis";
            this.Chkb_IgnoreY.UseVisualStyleBackColor = true;
            this.Chkb_IgnoreY.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // SmoothingCnts
            // 
            this.SmoothingCnts.AutoSize = true;
            this.SmoothingCnts.Location = new System.Drawing.Point(5, 164);
            this.SmoothingCnts.Name = "SmoothingCnts";
            this.SmoothingCnts.Size = new System.Drawing.Size(104, 13);
            this.SmoothingCnts.TabIndex = 68;
            this.SmoothingCnts.Text = "Smoothing constant:";
            // 
            // NumUp_Smoothing
            // 
            this.NumUp_Smoothing.DecimalPlaces = 2;
            this.NumUp_Smoothing.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUp_Smoothing.Location = new System.Drawing.Point(123, 162);
            this.NumUp_Smoothing.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUp_Smoothing.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.NumUp_Smoothing.Name = "NumUp_Smoothing";
            this.NumUp_Smoothing.Size = new System.Drawing.Size(66, 20);
            this.NumUp_Smoothing.TabIndex = 67;
            this.NumUp_Smoothing.Tag = "SMOOTH";
            this.NumUp_Smoothing.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Lb_PathEnd
            // 
            this.Lb_PathEnd.AutoSize = true;
            this.Lb_PathEnd.Location = new System.Drawing.Point(5, 320);
            this.Lb_PathEnd.Name = "Lb_PathEnd";
            this.Lb_PathEnd.Size = new System.Drawing.Size(75, 13);
            this.Lb_PathEnd.TabIndex = 66;
            this.Lb_PathEnd.Text = "Path end time:";
            // 
            // tmpicker_timedPathStart
            // 
            this.tmpicker_timedPathStart.CustomFormat = "HH:mm";
            this.tmpicker_timedPathStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpicker_timedPathStart.Location = new System.Drawing.Point(123, 292);
            this.tmpicker_timedPathStart.Name = "tmpicker_timedPathStart";
            this.tmpicker_timedPathStart.ShowUpDown = true;
            this.tmpicker_timedPathStart.Size = new System.Drawing.Size(66, 20);
            this.tmpicker_timedPathStart.TabIndex = 63;
            this.tmpicker_timedPathStart.Tag = "PATHSTARTTIME";
            this.tmpicker_timedPathStart.ValueChanged += new System.EventHandler(this.DatePicker_ValueChanged);
            // 
            // Label_PathStTime
            // 
            this.Label_PathStTime.AutoSize = true;
            this.Label_PathStTime.Location = new System.Drawing.Point(5, 295);
            this.Label_PathStTime.Name = "Label_PathStTime";
            this.Label_PathStTime.Size = new System.Drawing.Size(77, 13);
            this.Label_PathStTime.TabIndex = 64;
            this.Label_PathStTime.Text = "Path start time:";
            // 
            // tmpicker_timedPathEnd
            // 
            this.tmpicker_timedPathEnd.CustomFormat = "HH:mm";
            this.tmpicker_timedPathEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpicker_timedPathEnd.Location = new System.Drawing.Point(123, 318);
            this.tmpicker_timedPathEnd.Name = "tmpicker_timedPathEnd";
            this.tmpicker_timedPathEnd.ShowUpDown = true;
            this.tmpicker_timedPathEnd.Size = new System.Drawing.Size(65, 20);
            this.tmpicker_timedPathEnd.TabIndex = 65;
            this.tmpicker_timedPathEnd.Tag = "PATHENDTIME";
            this.tmpicker_timedPathEnd.ValueChanged += new System.EventHandler(this.DatePicker_ValueChanged);
            // 
            // Lbl_GravityForce
            // 
            this.Lbl_GravityForce.AutoSize = true;
            this.Lbl_GravityForce.Location = new System.Drawing.Point(5, 190);
            this.Lbl_GravityForce.Name = "Lbl_GravityForce";
            this.Lbl_GravityForce.Size = new System.Drawing.Size(70, 13);
            this.Lbl_GravityForce.TabIndex = 36;
            this.Lbl_GravityForce.Text = "Gravity force:";
            // 
            // Label_LoopDelay
            // 
            this.Label_LoopDelay.AutoSize = true;
            this.Label_LoopDelay.Location = new System.Drawing.Point(5, 138);
            this.Label_LoopDelay.Name = "Label_LoopDelay";
            this.Label_LoopDelay.Size = new System.Drawing.Size(88, 13);
            this.Label_LoopDelay.TabIndex = 47;
            this.Label_LoopDelay.Text = "Movement delay:";
            // 
            // Label_LoopStartNode
            // 
            this.Label_LoopStartNode.AutoSize = true;
            this.Label_LoopStartNode.Location = new System.Drawing.Point(4, 242);
            this.Label_LoopStartNode.Name = "Label_LoopStartNode";
            this.Label_LoopStartNode.Size = new System.Drawing.Size(84, 13);
            this.Label_LoopStartNode.TabIndex = 45;
            this.Label_LoopStartNode.Text = "Loop start node:";
            // 
            // Checkbox_Loop
            // 
            this.Checkbox_Loop.AutoSize = true;
            this.Checkbox_Loop.Location = new System.Drawing.Point(7, 344);
            this.Checkbox_Loop.Name = "Checkbox_Loop";
            this.Checkbox_Loop.Size = new System.Drawing.Size(50, 17);
            this.Checkbox_Loop.TabIndex = 41;
            this.Checkbox_Loop.Tag = "LOOP";
            this.Checkbox_Loop.Text = "Loop";
            this.Checkbox_Loop.UseVisualStyleBackColor = true;
            this.Checkbox_Loop.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUpDown_LoopStartNode
            // 
            this.NumUpDown_LoopStartNode.Location = new System.Drawing.Point(123, 240);
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
            this.NumUpDown_LoopStartNode.Tag = "PATHSTARTID";
            this.NumUpDown_LoopStartNode.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumUpDown_LoopStartNode.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // NumUpDown_GravityForce
            // 
            this.NumUpDown_GravityForce.DecimalPlaces = 2;
            this.NumUpDown_GravityForce.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUpDown_GravityForce.Location = new System.Drawing.Point(123, 188);
            this.NumUpDown_GravityForce.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_GravityForce.Name = "NumUpDown_GravityForce";
            this.NumUpDown_GravityForce.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_GravityForce.TabIndex = 35;
            this.NumUpDown_GravityForce.Tag = "GRAVITYFORCE";
            this.NumUpDown_GravityForce.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
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
            this.NumUpDown_LoopDelay.Tag = "MOVDEL";
            this.NumUpDown_LoopDelay.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Label_LoopEndNode
            // 
            this.Label_LoopEndNode.AutoSize = true;
            this.Label_LoopEndNode.Location = new System.Drawing.Point(5, 269);
            this.Label_LoopEndNode.Name = "Label_LoopEndNode";
            this.Label_LoopEndNode.Size = new System.Drawing.Size(82, 13);
            this.Label_LoopEndNode.TabIndex = 43;
            this.Label_LoopEndNode.Text = "Loop end node:";
            // 
            // NumUpDown_LoopEndNode
            // 
            this.NumUpDown_LoopEndNode.Location = new System.Drawing.Point(123, 266);
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
            this.NumUpDown_LoopEndNode.Tag = "PATHENDID";
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
            this.Label_PathFollowID.Location = new System.Drawing.Point(5, 216);
            this.Label_PathFollowID.Name = "Label_PathFollowID";
            this.Label_PathFollowID.Size = new System.Drawing.Size(46, 13);
            this.Label_PathFollowID.TabIndex = 39;
            this.Label_PathFollowID.Text = "Path ID:";
            // 
            // NumUpDown_PathFollowID
            // 
            this.NumUpDown_PathFollowID.Location = new System.Drawing.Point(123, 214);
            this.NumUpDown_PathFollowID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumUpDown_PathFollowID.Name = "NumUpDown_PathFollowID";
            this.NumUpDown_PathFollowID.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_PathFollowID.TabIndex = 38;
            this.NumUpDown_PathFollowID.Tag = "PATHID";
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
            this.NumUpDown_MovDistance.Tag = "MOVDISTANCE";
            this.NumUpDown_MovDistance.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Combo_MovementType
            // 
            this.Combo_MovementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_MovementType.FormattingEnabled = true;
            this.Combo_MovementType.Items.AddRange(new object[] {
            "None",
            "Roam",
            "Follow",
            "Run away",
            "Path",
            "Timed path"});
            this.Combo_MovementType.Location = new System.Drawing.Point(8, 27);
            this.Combo_MovementType.Name = "Combo_MovementType";
            this.Combo_MovementType.Size = new System.Drawing.Size(181, 21);
            this.Combo_MovementType.TabIndex = 61;
            this.Combo_MovementType.Tag = "MOVEMENT";
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
            this.NumUpDown_MovSpeed.Location = new System.Drawing.Point(123, 110);
            this.NumUpDown_MovSpeed.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumUpDown_MovSpeed.Name = "NumUpDown_MovSpeed";
            this.NumUpDown_MovSpeed.Size = new System.Drawing.Size(66, 20);
            this.NumUpDown_MovSpeed.TabIndex = 37;
            this.NumUpDown_MovSpeed.Tag = "MOVSPEED";
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
            this.Label_Speed.Location = new System.Drawing.Point(4, 112);
            this.Label_Speed.Name = "Label_Speed";
            this.Label_Speed.Size = new System.Drawing.Size(92, 13);
            this.Label_Speed.TabIndex = 36;
            this.Label_Speed.Text = "Movement speed:";
            // 
            // Checkbox_AlwaysDraw
            // 
            this.Checkbox_AlwaysDraw.AutoSize = true;
            this.Checkbox_AlwaysDraw.Location = new System.Drawing.Point(422, 166);
            this.Checkbox_AlwaysDraw.Name = "Checkbox_AlwaysDraw";
            this.Checkbox_AlwaysDraw.Size = new System.Drawing.Size(146, 17);
            this.Checkbox_AlwaysDraw.TabIndex = 70;
            this.Checkbox_AlwaysDraw.Tag = "DRAWOUTOFCAM";
            this.Checkbox_AlwaysDraw.Text = "Draw even out of camera";
            this.Checkbox_AlwaysDraw.UseVisualStyleBackColor = true;
            this.Checkbox_AlwaysDraw.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // NumUpDown_CutsceneSlot
            // 
            this.NumUpDown_CutsceneSlot.Location = new System.Drawing.Point(422, 36);
            this.NumUpDown_CutsceneSlot.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NumUpDown_CutsceneSlot.Name = "NumUpDown_CutsceneSlot";
            this.NumUpDown_CutsceneSlot.Size = new System.Drawing.Size(71, 20);
            this.NumUpDown_CutsceneSlot.TabIndex = 66;
            this.NumUpDown_CutsceneSlot.Tag = "CUTSCENEID";
            this.NumUpDown_CutsceneSlot.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged);
            // 
            // Checkbox_AlwaysActive
            // 
            this.Checkbox_AlwaysActive.AutoSize = true;
            this.Checkbox_AlwaysActive.Location = new System.Drawing.Point(422, 143);
            this.Checkbox_AlwaysActive.Name = "Checkbox_AlwaysActive";
            this.Checkbox_AlwaysActive.Size = new System.Drawing.Size(156, 17);
            this.Checkbox_AlwaysActive.TabIndex = 68;
            this.Checkbox_AlwaysActive.Tag = "ACTIVE";
            this.Checkbox_AlwaysActive.Text = "Update even out of camera";
            this.Checkbox_AlwaysActive.UseVisualStyleBackColor = true;
            this.Checkbox_AlwaysActive.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // Checkbox_CanPressSwitches
            // 
            this.Checkbox_CanPressSwitches.AutoSize = true;
            this.Checkbox_CanPressSwitches.Location = new System.Drawing.Point(422, 189);
            this.Checkbox_CanPressSwitches.Name = "Checkbox_CanPressSwitches";
            this.Checkbox_CanPressSwitches.Size = new System.Drawing.Size(107, 17);
            this.Checkbox_CanPressSwitches.TabIndex = 58;
            this.Checkbox_CanPressSwitches.Tag = "SWITCHES";
            this.Checkbox_CanPressSwitches.Text = "Presses switches";
            this.Checkbox_CanPressSwitches.UseVisualStyleBackColor = true;
            this.Checkbox_CanPressSwitches.CheckedChanged += new System.EventHandler(this.CheckBox_ValueChanged);
            // 
            // MessagesTabPage
            // 
            this.MessagesTabPage.Controls.Add(this.PanelMsgPreview);
            this.MessagesTabPage.Controls.Add(this.MsgText);
            this.MessagesTabPage.Controls.Add(this.Btn_MsgRename);
            this.MessagesTabPage.Controls.Add(this.Lbl_Text);
            this.MessagesTabPage.Controls.Add(this.Combo_MsgPos);
            this.MessagesTabPage.Controls.Add(this.Lbl_MsgPos);
            this.MessagesTabPage.Controls.Add(this.Combo_MsgType);
            this.MessagesTabPage.Controls.Add(this.Lbl_MsgType);
            this.MessagesTabPage.Controls.Add(this.Btn_DeleteMsg);
            this.MessagesTabPage.Controls.Add(this.Btn_AddMsg);
            this.MessagesTabPage.Controls.Add(this.MessagesGrid);
            this.MessagesTabPage.Location = new System.Drawing.Point(4, 22);
            this.MessagesTabPage.Name = "MessagesTabPage";
            this.MessagesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.MessagesTabPage.Size = new System.Drawing.Size(768, 627);
            this.MessagesTabPage.TabIndex = 5;
            this.MessagesTabPage.Text = "Messages";
            this.MessagesTabPage.UseVisualStyleBackColor = true;
            // 
            // PanelMsgPreview
            // 
            this.PanelMsgPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelMsgPreview.AutoScroll = true;
            this.PanelMsgPreview.BackColor = System.Drawing.Color.White;
            this.PanelMsgPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelMsgPreview.Controls.Add(this.MsgPreview);
            this.PanelMsgPreview.Location = new System.Drawing.Point(163, 269);
            this.PanelMsgPreview.Margin = new System.Windows.Forms.Padding(0);
            this.PanelMsgPreview.Name = "PanelMsgPreview";
            this.PanelMsgPreview.Size = new System.Drawing.Size(597, 352);
            this.PanelMsgPreview.TabIndex = 72;
            this.PanelMsgPreview.Resize += new System.EventHandler(this.PanelMsgPreview_Resize);
            // 
            // MsgPreview
            // 
            this.MsgPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MsgPreview.Location = new System.Drawing.Point(74, 39);
            this.MsgPreview.Margin = new System.Windows.Forms.Padding(0);
            this.MsgPreview.Name = "MsgPreview";
            this.MsgPreview.Size = new System.Drawing.Size(474, 275);
            this.MsgPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MsgPreview.TabIndex = 69;
            this.MsgPreview.TabStop = false;
            // 
            // MsgText
            // 
            this.MsgText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MsgText.AutoCompleteBracketsList = new char[] {
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
            this.MsgText.AutoIndent = false;
            this.MsgText.AutoIndentChars = false;
            this.MsgText.AutoScrollMinSize = new System.Drawing.Size(2, 12);
            this.MsgText.BackBrush = null;
            this.MsgText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MsgText.CharHeight = 12;
            this.MsgText.CharWidth = 6;
            this.MsgText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MsgText.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.MsgText.Font = new System.Drawing.Font("Cascadia Code", 8.25F);
            this.MsgText.IsReplaceMode = false;
            this.MsgText.Location = new System.Drawing.Point(163, 50);
            this.MsgText.Name = "MsgText";
            this.MsgText.Paddings = new System.Windows.Forms.Padding(0);
            this.MsgText.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.MsgText.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("MsgText.ServiceColors")));
            this.MsgText.ShowLineNumbers = false;
            this.MsgText.Size = new System.Drawing.Size(597, 216);
            this.MsgText.TabIndex = 68;
            this.MsgText.Tag = "0";
            this.MsgText.WordWrapAutoIndent = false;
            this.MsgText.Zoom = 100;
            this.MsgText.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.MsgText_TextChanged);
            // 
            // Btn_MsgRename
            // 
            this.Btn_MsgRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Btn_MsgRename.Location = new System.Drawing.Point(81, 553);
            this.Btn_MsgRename.Name = "Btn_MsgRename";
            this.Btn_MsgRename.Size = new System.Drawing.Size(73, 31);
            this.Btn_MsgRename.TabIndex = 67;
            this.Btn_MsgRename.Text = "Rename";
            this.Btn_MsgRename.UseVisualStyleBackColor = true;
            this.Btn_MsgRename.Click += new System.EventHandler(this.Btn_MsgRename_Click);
            // 
            // Lbl_Text
            // 
            this.Lbl_Text.AutoSize = true;
            this.Lbl_Text.Location = new System.Drawing.Point(161, 34);
            this.Lbl_Text.Name = "Lbl_Text";
            this.Lbl_Text.Size = new System.Drawing.Size(31, 13);
            this.Lbl_Text.TabIndex = 66;
            this.Lbl_Text.Text = "Text:";
            // 
            // Combo_MsgPos
            // 
            this.Combo_MsgPos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_MsgPos.FormattingEnabled = true;
            this.Combo_MsgPos.Items.AddRange(new object[] {
            "Dynamic",
            "Top",
            "Center",
            "Bottom"});
            this.Combo_MsgPos.Location = new System.Drawing.Point(457, 3);
            this.Combo_MsgPos.Name = "Combo_MsgPos";
            this.Combo_MsgPos.Size = new System.Drawing.Size(181, 21);
            this.Combo_MsgPos.TabIndex = 65;
            this.Combo_MsgPos.Tag = "MOVEMENT";
            this.Combo_MsgPos.SelectedIndexChanged += new System.EventHandler(this.Combo_MsgPos_SelectedIndexChanged);
            // 
            // Lbl_MsgPos
            // 
            this.Lbl_MsgPos.AutoSize = true;
            this.Lbl_MsgPos.Location = new System.Drawing.Point(404, 6);
            this.Lbl_MsgPos.Name = "Lbl_MsgPos";
            this.Lbl_MsgPos.Size = new System.Drawing.Size(47, 13);
            this.Lbl_MsgPos.TabIndex = 64;
            this.Lbl_MsgPos.Text = "Position:";
            // 
            // Combo_MsgType
            // 
            this.Combo_MsgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combo_MsgType.FormattingEnabled = true;
            this.Combo_MsgType.Items.AddRange(new object[] {
            "Black",
            "Wood",
            "Blue",
            "Ocarina",
            "None (White text)",
            "None (Black text)"});
            this.Combo_MsgType.Location = new System.Drawing.Point(217, 3);
            this.Combo_MsgType.Name = "Combo_MsgType";
            this.Combo_MsgType.Size = new System.Drawing.Size(181, 21);
            this.Combo_MsgType.TabIndex = 63;
            this.Combo_MsgType.Tag = "MOVEMENT";
            this.Combo_MsgType.SelectedIndexChanged += new System.EventHandler(this.Combo_MsgType_SelectedIndexChanged);
            // 
            // Lbl_MsgType
            // 
            this.Lbl_MsgType.AutoSize = true;
            this.Lbl_MsgType.Location = new System.Drawing.Point(160, 6);
            this.Lbl_MsgType.Name = "Lbl_MsgType";
            this.Lbl_MsgType.Size = new System.Drawing.Size(51, 13);
            this.Lbl_MsgType.TabIndex = 62;
            this.Lbl_MsgType.Text = "Box type:";
            // 
            // Btn_DeleteMsg
            // 
            this.Btn_DeleteMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Btn_DeleteMsg.Location = new System.Drawing.Point(3, 553);
            this.Btn_DeleteMsg.Name = "Btn_DeleteMsg";
            this.Btn_DeleteMsg.Size = new System.Drawing.Size(73, 31);
            this.Btn_DeleteMsg.TabIndex = 8;
            this.Btn_DeleteMsg.Text = "Delete";
            this.Btn_DeleteMsg.UseVisualStyleBackColor = true;
            this.Btn_DeleteMsg.Click += new System.EventHandler(this.Btn_DeleteMsg_Click);
            // 
            // Btn_AddMsg
            // 
            this.Btn_AddMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Btn_AddMsg.Location = new System.Drawing.Point(3, 590);
            this.Btn_AddMsg.Name = "Btn_AddMsg";
            this.Btn_AddMsg.Size = new System.Drawing.Size(151, 31);
            this.Btn_AddMsg.TabIndex = 8;
            this.Btn_AddMsg.Text = "Add";
            this.Btn_AddMsg.UseVisualStyleBackColor = true;
            this.Btn_AddMsg.Click += new System.EventHandler(this.Btn_AddMsg_Click);
            // 
            // MessagesGrid
            // 
            this.MessagesGrid.AllowUserToAddRows = false;
            this.MessagesGrid.AllowUserToDeleteRows = false;
            this.MessagesGrid.AllowUserToResizeColumns = false;
            this.MessagesGrid.AllowUserToResizeRows = false;
            this.MessagesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MessagesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MessagesGrid.BackgroundColor = System.Drawing.Color.White;
            this.MessagesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MessagesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.MessagesGrid.Location = new System.Drawing.Point(3, 3);
            this.MessagesGrid.MultiSelect = false;
            this.MessagesGrid.Name = "MessagesGrid";
            this.MessagesGrid.ReadOnly = true;
            this.MessagesGrid.RowHeadersVisible = false;
            this.MessagesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MessagesGrid.Size = new System.Drawing.Size(151, 544);
            this.MessagesGrid.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Message title";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Panel_NPCList
            // 
            this.Panel_NPCList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Panel_NPCList.Controls.Add(this.Button_Duplicate);
            this.Panel_NPCList.Controls.Add(this.Button_Delete);
            this.Panel_NPCList.Controls.Add(this.Button_Add);
            this.Panel_NPCList.Controls.Add(this.DataGrid_NPCs);
            this.Panel_NPCList.Location = new System.Drawing.Point(0, 3);
            this.Panel_NPCList.Name = "Panel_NPCList";
            this.Panel_NPCList.Size = new System.Drawing.Size(244, 653);
            this.Panel_NPCList.TabIndex = 5;
            // 
            // Button_Duplicate
            // 
            this.Button_Duplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Duplicate.Location = new System.Drawing.Point(87, 618);
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
            this.Button_Delete.Location = new System.Drawing.Point(166, 618);
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
            this.Button_Add.Location = new System.Drawing.Point(3, 618);
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
            this.DataGrid_NPCs.Size = new System.Drawing.Size(236, 612);
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
            this.renameCurrentScriptToolStripMenuItem,
            this.editGlobalHeaderToolStripMenuItem});
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
            // editGlobalHeaderToolStripMenuItem
            // 
            this.editGlobalHeaderToolStripMenuItem.Name = "editGlobalHeaderToolStripMenuItem";
            this.editGlobalHeaderToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.editGlobalHeaderToolStripMenuItem.Text = "Global headers";
            this.editGlobalHeaderToolStripMenuItem.Click += new System.EventHandler(this.EditGlobalHeaderToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syntaxHighlightingToolStripMenuItem,
            this.checkSyntaxToolStripMenuItem,
            this.improveMessagePreviewReadabilityToolStripMenuItem});
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
            this.syntaxHighlightingToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.syntaxHighlightingToolStripMenuItem.Text = "Syntax highlighting";
            this.syntaxHighlightingToolStripMenuItem.CheckedChanged += new System.EventHandler(this.SyntaxHighlightingToolStripMenuItem_CheckedChanged);
            // 
            // checkSyntaxToolStripMenuItem
            // 
            this.checkSyntaxToolStripMenuItem.Checked = true;
            this.checkSyntaxToolStripMenuItem.CheckOnClick = true;
            this.checkSyntaxToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSyntaxToolStripMenuItem.Name = "checkSyntaxToolStripMenuItem";
            this.checkSyntaxToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.checkSyntaxToolStripMenuItem.Text = "Check syntax";
            this.checkSyntaxToolStripMenuItem.CheckedChanged += new System.EventHandler(this.CheckSyntaxToolStripMenuItem_CheckedChanged);
            // 
            // improveMessagePreviewReadabilityToolStripMenuItem
            // 
            this.improveMessagePreviewReadabilityToolStripMenuItem.Checked = true;
            this.improveMessagePreviewReadabilityToolStripMenuItem.CheckOnClick = true;
            this.improveMessagePreviewReadabilityToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.improveMessagePreviewReadabilityToolStripMenuItem.Name = "improveMessagePreviewReadabilityToolStripMenuItem";
            this.improveMessagePreviewReadabilityToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.improveMessagePreviewReadabilityToolStripMenuItem.Text = "Improve message preview readability";
            this.improveMessagePreviewReadabilityToolStripMenuItem.Click += new System.EventHandler(this.ImproveMessagePreviewReadabilityToolStripMenuItem_Click);
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
            this.MenuStrip.Size = new System.Drawing.Size(1021, 24);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // listsToolStripMenuItem
            // 
            this.listsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectsToolStripMenuItem,
            this.actorsToolStripMenuItem1,
            this.sFXToolStripMenuItem,
            this.musicToolStripMenuItem1,
            this.linkAnimationsToolStripMenuItem,
            this.colorPickerToolStripMenuItem});
            this.listsToolStripMenuItem.Name = "listsToolStripMenuItem";
            this.listsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.listsToolStripMenuItem.Text = "Dictionaries";
            // 
            // objectsToolStripMenuItem
            // 
            this.objectsToolStripMenuItem.Name = "objectsToolStripMenuItem";
            this.objectsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.objectsToolStripMenuItem.Text = "Objects";
            this.objectsToolStripMenuItem.Click += new System.EventHandler(this.ObjectsToolStripMenuItem_Click);
            // 
            // actorsToolStripMenuItem1
            // 
            this.actorsToolStripMenuItem1.Name = "actorsToolStripMenuItem1";
            this.actorsToolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.actorsToolStripMenuItem1.Text = "Actors";
            this.actorsToolStripMenuItem1.Click += new System.EventHandler(this.ActorsToolStripMenuItem1_Click);
            // 
            // sFXToolStripMenuItem
            // 
            this.sFXToolStripMenuItem.Name = "sFXToolStripMenuItem";
            this.sFXToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.sFXToolStripMenuItem.Text = "SFX";
            this.sFXToolStripMenuItem.Click += new System.EventHandler(this.SFXToolStripMenuItem_Click);
            // 
            // musicToolStripMenuItem1
            // 
            this.musicToolStripMenuItem1.Name = "musicToolStripMenuItem1";
            this.musicToolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.musicToolStripMenuItem1.Text = "Music";
            this.musicToolStripMenuItem1.Click += new System.EventHandler(this.MusicToolStripMenuItem1_Click);
            // 
            // linkAnimationsToolStripMenuItem
            // 
            this.linkAnimationsToolStripMenuItem.Name = "linkAnimationsToolStripMenuItem";
            this.linkAnimationsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.linkAnimationsToolStripMenuItem.Text = "Link animations";
            this.linkAnimationsToolStripMenuItem.Click += new System.EventHandler(this.LinkAnimsToolStripMenuItem1_Click);
            // 
            // colorPickerToolStripMenuItem
            // 
            this.colorPickerToolStripMenuItem.Name = "colorPickerToolStripMenuItem";
            this.colorPickerToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.colorPickerToolStripMenuItem.Text = "Color Picker";
            this.colorPickerToolStripMenuItem.Click += new System.EventHandler(this.ColorPickerToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1021, 683);
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
            ((System.ComponentModel.ISupportInitialize)(this.NumUpAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ZModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Hierarchy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Animations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_XModelOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Scale)).EndInit();
            this.Tab2_ExtraData.ResumeLayout(false);
            this.Tab2_ExtraData.PerformLayout();
            this.TabControl_Segments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSegment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSegment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_TalkSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_BlinkSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_ExtraDLists)).EndInit();
            this.Tab3_BehaviorData.ResumeLayout(false);
            this.Tab3_BehaviorData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ScriptsFVar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_ScriptsVar)).EndInit();
            this.Panel_Colors.ResumeLayout(false);
            this.Panel_Colors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightZOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightYOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightXOffs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_LightLimb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_RiddenBy)).EndInit();
            this.Btn_ReactIfAtt.ResumeLayout(false);
            this.Btn_ReactIfAtt.PerformLayout();
            this.Panel_Collision.ResumeLayout(false);
            this.Panel_Collision.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_Mass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_YColOffs)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_MaxRoam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUp_Smoothing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopStartNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_GravityForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_LoopEndNode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_PathFollowID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_MovSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDown_CutsceneSlot)).EndInit();
            this.MessagesTabPage.ResumeLayout(false);
            this.MessagesTabPage.PerformLayout();
            this.PanelMsgPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MsgPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MsgText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MessagesGrid)).EndInit();
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
        private System.Windows.Forms.TabPage TabPage_Segment_9;
        private System.Windows.Forms.TabPage TabPage_Segment_A;
        private System.Windows.Forms.TabPage TabPage_Segment_B;
        private System.Windows.Forms.TabPage TabPage_Segment_C;
        private System.Windows.Forms.TabPage TabPage_Segment_D;
        private System.Windows.Forms.TabPage TabPage_Segment_E;
        private System.Windows.Forms.TabPage TabPage_Segment_F;
        private CustomDataGridView DataGridView_ExtraDLists;
        private System.Windows.Forms.TabPage Tab3_BehaviorData;
        private System.Windows.Forms.CheckBox Chkb_Opendoors;
        private System.Windows.Forms.CheckBox ChkRunJustScript;
        private System.Windows.Forms.CheckBox Chkb_ReactIfAtt;
        private System.Windows.Forms.Label Lbl_Misc;
        private System.Windows.Forms.CheckBox Checkbox_AlwaysDraw;
        private System.Windows.Forms.Label Label_CutsceneSlot;
        private System.Windows.Forms.NumericUpDown NumUpDown_CutsceneSlot;
        private System.Windows.Forms.Panel Panel_Collision;
        private System.Windows.Forms.NumericUpDown NumUpDown_YColOffs;
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
        private System.Windows.Forms.Label Lb_PathEnd;
        private System.Windows.Forms.DateTimePicker tmpicker_timedPathStart;
        private System.Windows.Forms.Label Label_PathStTime;
        private System.Windows.Forms.DateTimePicker tmpicker_timedPathEnd;
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
        private System.Windows.Forms.CheckBox Checkbox_CanPressSwitches;
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
        private System.Windows.Forms.ToolStripMenuItem checkSyntaxToolStripMenuItem;
        private System.Windows.Forms.CheckBox Chkb_IgnoreY;
        private System.Windows.Forms.Label SmoothingCnts;
        private System.Windows.Forms.NumericUpDown NumUp_Smoothing;
        private System.Windows.Forms.Panel Btn_ReactIfAtt;
        private System.Windows.Forms.Button Btn_ReactIfAttList;
        private System.Windows.Forms.TextBox Txtbox_ReactIfAtt;
        private System.Windows.Forms.Label Lbl_ReactIfAttSnd;
        private System.Windows.Forms.ComboBox Combo_EffIfAtt;
        private System.Windows.Forms.Label Label_EffIfAtt;
        private System.Windows.Forms.NumericUpDown NumUpDown_Mass;
        private System.Windows.Forms.Label Lbl_Mass;
        private System.Windows.Forms.NumericUpDown NumUpAlpha;
        private System.Windows.Forms.Label LblAlpha;
        private System.Windows.Forms.CheckBox ChkB_FadeOut;
        private System.Windows.Forms.NumericUpDown NumUp_RiddenBy;
        private System.Windows.Forms.Label Label_RiddenBy;
        private System.Windows.Forms.Panel Panel_Colors;
        private System.Windows.Forms.NumericUpDown NumUp_LightZOffs;
        private System.Windows.Forms.NumericUpDown NumUp_LightYOffs;
        private System.Windows.Forms.NumericUpDown NumUp_LightXOffs;
        private System.Windows.Forms.Label Label_LightLimbOffset;
        private System.Windows.Forms.Label Label_LightLimb;
        private System.Windows.Forms.CheckBox ChkBox_Glow;
        private System.Windows.Forms.NumericUpDown NumUp_LightLimb;
        private System.Windows.Forms.CheckBox ChkBox_GenLight;
        private System.Windows.Forms.CheckBox ChkBox_DebugCol;
        private System.Windows.Forms.Label Lbl_LimbColors;
        private CustomDataGridView ColorsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartLimbColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColorColumn;
        private System.Windows.Forms.Label Lbl_LightColor;
        private System.Windows.Forms.Button Btn_LightColor;
        private System.Windows.Forms.Label Lbl_Radius;
        private System.Windows.Forms.NumericUpDown NumUp_LightRadius;
        private System.Windows.Forms.CheckBox ChkOnlyWhenLens;
        private System.Windows.Forms.CheckBox ChkInvisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_AnimName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Anim;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_StartFrame;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_EndFrame;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OBJ;
        private System.Windows.Forms.TabPage MessagesTabPage;
        private System.Windows.Forms.ComboBox Combo_MsgType;
        private System.Windows.Forms.Label Lbl_MsgType;
        private System.Windows.Forms.Button Btn_DeleteMsg;
        private System.Windows.Forms.Button Btn_AddMsg;
        private CustomDataGridView MessagesGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label Lbl_Text;
        private System.Windows.Forms.ComboBox Combo_MsgPos;
        private System.Windows.Forms.Label Lbl_MsgPos;
        private System.Windows.Forms.Button Btn_MsgRename;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private FastColoredTextBoxNS.FastColoredTextBox MsgText;
        private System.Windows.Forms.NumericUpDown NumUpDown_ScriptsFVar;
        private System.Windows.Forms.NumericUpDown NumUpDown_ScriptsVar;
        private System.Windows.Forms.Label Lbl_ScriptsFVars;
        private System.Windows.Forms.Label Lbl_ScriptsVars;
        private System.Windows.Forms.ToolStripMenuItem linkAnimationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editGlobalHeaderToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Purpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Color;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Translation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Rotation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDLists_Scale;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_Limb;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExtraDlists_ObjectID;
        private System.Windows.Forms.DataGridViewComboBoxColumn ExtraDlists_ShowType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NumUp_MaxRoam;
        private System.Windows.Forms.ToolStripMenuItem colorPickerToolStripMenuItem;
        private System.Windows.Forms.PictureBox MsgPreview;
        private System.Windows.Forms.ToolStripMenuItem improveMessagePreviewReadabilityToolStripMenuItem;
        private System.Windows.Forms.Panel PanelMsgPreview;
    }
}

