using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace NPC_Maker
{
    public static class ScriptContextMenu
    {
        public static ContextMenuStrip ContextMenuStrip;

        private static FCTB_Mono LastClickedTextbox;

        private static ToolStripMenuItem functionsToolStripMenuItem;
        private static ToolStripMenuItem keywordsToolStripMenuItem;
        private static ToolStripMenuItem itemsToolStripMenuItem;
        private static ToolStripMenuItem itemsgiveToolStripMenuItem;
        private static ToolStripMenuItem questItemsToolStripMenuItem;
        private static ToolStripMenuItem itemsdungeonToolStripMenuItem;
        private static ToolStripMenuItem itemstradeToolStripMenuItem;
        private static ToolStripMenuItem playerMasksToolStripMenuItem;
        private static ToolStripMenuItem soundEffectsToolStripMenuItem;
        private static ToolStripMenuItem musicToolStripMenuItem;
        private static ToolStripMenuItem actorstoolStripMenuItem;
        private static ToolStripMenuItem ocarinaSongstoolStripMenuItem;
        private static ToolStripMenuItem objectstoolStripMenuItem;
        private static ToolStripMenuItem particlestoolStripMenuItem;
        private static ToolStripMenuItem linkAnimsStripMenuItem;
        private static ToolStripMenuItem damageTypesStripMenuItem;
        private static ToolStripMenuItem stateTypesStripMenuItem;
        private static ToolStripMenuItem cFunctionsStripMenuItem;
        private static ToolStripMenuItem quakeTypesStripMenuItem;
        private static ToolStripMenuItem messagesStripMenuItem;
        private static ToolStripMenuItem transitionTypesStripMenuItem;
        private static ToolStripMenuItem definesStripMenuItem;
        private static NPCEntry curEntry;

        public static void MakeContextMenu(NPCEntry Entry)
        {
            if (ContextMenuStrip != null)
                ContextMenuStrip.Dispose();

            curEntry = Entry;

            ContextMenuStrip = new ContextMenuStrip();

            functionsToolStripMenuItem = new ToolStripMenuItem();
            keywordsToolStripMenuItem = new ToolStripMenuItem();
            itemsToolStripMenuItem = new ToolStripMenuItem();
            itemsgiveToolStripMenuItem = new ToolStripMenuItem();
            questItemsToolStripMenuItem = new ToolStripMenuItem();
            itemsdungeonToolStripMenuItem = new ToolStripMenuItem();
            itemstradeToolStripMenuItem = new ToolStripMenuItem();
            playerMasksToolStripMenuItem = new ToolStripMenuItem();
            soundEffectsToolStripMenuItem = new ToolStripMenuItem();
            musicToolStripMenuItem = new ToolStripMenuItem();
            actorstoolStripMenuItem = new ToolStripMenuItem();
            objectstoolStripMenuItem = new ToolStripMenuItem();
            ocarinaSongstoolStripMenuItem = new ToolStripMenuItem();
            particlestoolStripMenuItem = new ToolStripMenuItem();
            linkAnimsStripMenuItem = new ToolStripMenuItem();
            damageTypesStripMenuItem = new ToolStripMenuItem();
            stateTypesStripMenuItem = new ToolStripMenuItem();
            cFunctionsStripMenuItem = new ToolStripMenuItem();
            quakeTypesStripMenuItem = new ToolStripMenuItem();
            transitionTypesStripMenuItem = new ToolStripMenuItem();
            messagesStripMenuItem = new ToolStripMenuItem();
            definesStripMenuItem = new ToolStripMenuItem();

            ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                                                                    functionsToolStripMenuItem,
                                                                    keywordsToolStripMenuItem,
                                                                    itemsToolStripMenuItem,
                                                                    itemsgiveToolStripMenuItem,
                                                                    questItemsToolStripMenuItem,
                                                                    itemsdungeonToolStripMenuItem,
                                                                    itemstradeToolStripMenuItem,
                                                                    damageTypesStripMenuItem,
                                                                    stateTypesStripMenuItem,
                                                                    quakeTypesStripMenuItem,
                                                                    transitionTypesStripMenuItem,
                                                                    playerMasksToolStripMenuItem,
                                                                    ocarinaSongstoolStripMenuItem,
                                                                    particlestoolStripMenuItem,
                                                                    soundEffectsToolStripMenuItem,
                                                                    musicToolStripMenuItem,
                                                                    actorstoolStripMenuItem,
                                                                    objectstoolStripMenuItem,
                                                                    linkAnimsStripMenuItem,
                                                                    cFunctionsStripMenuItem,
                                                                    messagesStripMenuItem,
                                                                    definesStripMenuItem
                                                                });

            ContextMenuStrip.Size = new System.Drawing.Size(157, 268);
            ContextMenuStrip.Text = "Items";

            stateTypesStripMenuItem.Text = "States";
            functionsToolStripMenuItem.Text = "Functions";
            keywordsToolStripMenuItem.Text = "Keywords";
            itemsToolStripMenuItem.Text = "Inventory items";
            itemsgiveToolStripMenuItem.Text = "Award items";
            questItemsToolStripMenuItem.Text = "Quest items";
            itemsdungeonToolStripMenuItem.Text = "Dungeon items";
            itemstradeToolStripMenuItem.Text = "Trade items";
            damageTypesStripMenuItem.Text = "Damage types";
            playerMasksToolStripMenuItem.Text = "Player Masks";
            ocarinaSongstoolStripMenuItem.Text = "Ocarina songs";
            particlestoolStripMenuItem.Text = "Particles";
            quakeTypesStripMenuItem.Text = "Quake types";
            quakeTypesStripMenuItem.Text = "Quake types";
            transitionTypesStripMenuItem.Text = "Transition types";
            cFunctionsStripMenuItem.Text = "C Functions";
            messagesStripMenuItem.Text = "Messages";

            definesStripMenuItem.Text = "Header defines";
            definesStripMenuItem.Click += DefinesStripMenuItem_Click;

            soundEffectsToolStripMenuItem.Text = "Sound effects";
            soundEffectsToolStripMenuItem.Click += SoundEffectsToolStripMenuItem_Click;

            musicToolStripMenuItem.Text = "Music";
            musicToolStripMenuItem.Click += MusicToolStripMenuItem_Click;
 
            actorstoolStripMenuItem.Text = "Actors";
            actorstoolStripMenuItem.Click += ActorstoolStripMenuItem_Click;

            objectstoolStripMenuItem.Text = "Objects";
            objectstoolStripMenuItem.Click += ObjectstoolStripMenuItem_Click;

            linkAnimsStripMenuItem.Text = "Player animations";
            linkAnimsStripMenuItem.Click += LinkAnimsStripMenuItem_Click;


            List<string> TalkInstructions = new List<string>() { Lists.Instructions.TALK.ToString(), 
                                                                 Lists.Instructions.FORCE_TALK.ToString(), 
                                                                 Lists.Instructions.SHOW_TEXTBOX.ToString(), 
                                                                 Lists.Instructions.SHOW_TEXTBOX_SP.ToString() };

            foreach (string Item in Enum.GetNames(typeof(Lists.Instructions)))
            {
                ToolStripMenuItem Tsmi = new ToolStripMenuItem(Item);

                if (Dicts.FunctionSubtypes.ContainsKey(Item))
                {
                    Tsmi.DoubleClickEnabled = true;
                    Tsmi.DoubleClick += Tsmi_DoubleClick;
                    AddItemCollectionToToolStripMenuItem(Dicts.FunctionSubtypes[Item], Tsmi);
                }
                else if (TalkInstructions.Contains(Item))
                {
                    if (Entry.Messages.Count != 0)
                    { 
                        Tsmi.DoubleClickEnabled = true;
                        Tsmi.DoubleClick += Tsmi_DoubleClick;
                        AddItemCollectionToToolStripMenuItem(Entry.Messages.Select(x => x.Name).ToArray(), Tsmi);
                    }
                    else
                        Tsmi.Click += Tsmi_Click;

                }
                else if (Item == Lists.Instructions.PARTICLE.ToString())
                {
                    Tsmi.DoubleClickEnabled = true;
                    Tsmi.DoubleClick += Tsmi_DoubleClick;
                    AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.ParticleTypes)), Tsmi);
                }
                else
                    Tsmi.Click += Tsmi_Click;

                functionsToolStripMenuItem.DropDownItems.Add(Tsmi);
            }

            AddItemCollectionToToolStripMenuItem(Lists.KeyValues.ToArray(), keywordsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Lists.AllKeywords.ToArray(), keywordsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.DamageTypes)), damageTypesStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.AwardItems)), itemsgiveToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.TradeItems)), itemstradeToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.DungeonItems)), itemsdungeonToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.Items)), itemsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.QuestItems)), questItemsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.PlayerMasks)), playerMasksToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.OcarinaSongs)), ocarinaSongstoolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.ParticleTypes)), particlestoolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.StateTypes)), stateTypesStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.QuakeTypes)), quakeTypesStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.TransitionTypes)), transitionTypesStripMenuItem);

            List<string> FunctionNames = new List<string>();

            foreach (var kvp in Entry.EmbeddedOverlayCode.Functions)
                FunctionNames.Add(kvp.FuncName);

            AddItemCollectionToToolStripMenuItem(FunctionNames.ToArray(), cFunctionsStripMenuItem);

            List<string> MessageNames = new List<string>();
            List<string> MessageToolTips = new List<string>();

            foreach (var msg in Entry.Messages)
            {
                MessageNames.Add(msg.Name);
                MessageToolTips.Add(msg.MessageText.Substring(0, Math.Min(80, msg.MessageText.Length)) + (msg.MessageText.Length > 80 ? "..." : ""));

            }

            AddItemCollectionToToolStripMenuItem(MessageNames.ToArray(), messagesStripMenuItem, MessageToolTips.ToArray());
        }

        private static void DefinesStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.HDefine h = Helpers.SelectNameFromH(curEntry);

            if (h != null)
                InsertTxtToScript($"H_{h.Name}");
        }

        private static void AddItemCollectionToToolStripMenuItem(string[] Collection, ToolStripMenuItem MenuItem, string[] ToolTips = null)
        {
            MenuItem.DropDown.MaximumSize = new Size(300, 700);

            ToolStripMenuItem[] Items = new ToolStripMenuItem[Collection.Length];

            for (int i = 0; i < Items.Length; i++)
            {
                ToolStripMenuItem SubItem = new ToolStripMenuItem(Collection[i]);

                if (ToolTips != null)
                {
                    SubItem.ToolTipText = ToolTips[i];
                    SubItem.AutoToolTip = true;
                }

                SubItem.Click += SubItem_Click;
                Items[i] = SubItem;
            }

            MenuItem.DropDownItems.AddRange(Items);
        }

        public static void SetTextBox(FCTB_Mono Box)
        {
            LastClickedTextbox = Box;
        }

        private static void InsertTxtToScript(string Text)
        {
            if (LastClickedTextbox == null)
                return;

            int Scroll = LastClickedTextbox.VerticalScroll.Value;
            int start = LastClickedTextbox.SelectionStart;
            string newTxt = LastClickedTextbox.Text;
            newTxt = newTxt.Remove(LastClickedTextbox.SelectionStart, LastClickedTextbox.SelectionLength);
            newTxt = newTxt.Insert(LastClickedTextbox.SelectionStart, Text);

            LastClickedTextbox.Text = newTxt;
            LastClickedTextbox.SelectionStart = start + Text.Length;
            LastClickedTextbox.VerticalScroll.Value = Scroll;
            LastClickedTextbox.UpdateScrollbars();
        }

        private static void Tsmi_DoubleClick(object sender, EventArgs e)
        {
            string Usage = ScriptsUsages.GetUsage((sender as ToolStripItem).Text, "");
            Usage = Usage.Trim(Environment.NewLine.ToCharArray());

            InsertTxtToScript(Usage == "" ? (sender as ToolStripItem).Text : Usage);
        }

        private static void Tsmi_Click(object sender, EventArgs e)
        {
            Tsmi_DoubleClick(sender, e);
        }

        private static void SubItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem s = (sender as ToolStripMenuItem);

            bool res = Enum.TryParse(s.OwnerItem.Text, out Lists.Instructions _);

            if (res)
            {
                string Usage = ScriptsUsages.GetUsage(s.OwnerItem.Text, s.Text);
                Usage = Usage.Trim(Environment.NewLine.ToCharArray());
                InsertTxtToScript(Usage == "" ? s.OwnerItem.Text + " " + s.Text : Usage);
            }
            else
                InsertTxtToScript(s.Text);
        }

        private static void SoundEffectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList(Lists.DictType.SFX, true);
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(SFX.Chosen.Name);
            }
        }

        private static void MusicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList(Lists.DictType.Music, true);
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(SFX.Chosen.Name);
            }
        }

        private static void ActorstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList Actors = new PickableList(Lists.DictType.Actors, true);
            DialogResult DR = Actors.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(Actors.Chosen.Name);
            }
        }

        private static void ObjectstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList Objects = new PickableList(Lists.DictType.Objects, true);
            DialogResult DR = Objects.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(Objects.Chosen.Name);
            }
        }

        private static void LinkAnimsStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList(Lists.DictType.LinkAnims, true);
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(SFX.Chosen.Name);
            }
        }
    }
}
