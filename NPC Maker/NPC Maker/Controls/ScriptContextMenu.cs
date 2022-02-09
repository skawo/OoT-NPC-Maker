using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NPC_Maker
{
    public static class ScriptContextMenu
    {
        public static ContextMenuStrip ContextMenuStrip;

        private static FastColoredTextBox LastClickedTextbox;

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

        public static void MakeContextMenu()
        {
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

            ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                                                                    functionsToolStripMenuItem,
                                                                    keywordsToolStripMenuItem,
                                                                    itemsToolStripMenuItem,
                                                                    itemsgiveToolStripMenuItem,
                                                                    questItemsToolStripMenuItem,
                                                                    itemsdungeonToolStripMenuItem,
                                                                    itemstradeToolStripMenuItem,
                                                                    damageTypesStripMenuItem,
                                                                    playerMasksToolStripMenuItem,
                                                                    ocarinaSongstoolStripMenuItem,
                                                                    particlestoolStripMenuItem,
                                                                    soundEffectsToolStripMenuItem,
                                                                    musicToolStripMenuItem,
                                                                    actorstoolStripMenuItem,
                                                                    objectstoolStripMenuItem,
                                                                    linkAnimsStripMenuItem
                                                                });

            ContextMenuStrip.Size = new System.Drawing.Size(157, 268);
            ContextMenuStrip.Text = "Items";

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


            foreach (string Item in Enum.GetNames(typeof(Lists.Instructions)))
            {
                ToolStripMenuItem Tsmi = new ToolStripMenuItem(Item);

                if (Dicts.FunctionSubtypes.ContainsKey(Item))
                {
                    Tsmi.DoubleClickEnabled = true;
                    Tsmi.DoubleClick += Tsmi_DoubleClick;
                    AddItemCollectionToToolStripMenuItem(Dicts.FunctionSubtypes[Item], Tsmi);
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
        }

        private static void AddItemCollectionToToolStripMenuItem(string[] Collection, ToolStripMenuItem MenuItem)
        {
            MenuItem.DropDown.MaximumSize = new Size(300, 700);

            ToolStripMenuItem[] Items = new ToolStripMenuItem[Collection.Length];

            for (int i = 0; i < Items.Length; i++)
            {
                ToolStripMenuItem SubItem = new ToolStripMenuItem(Collection[i]);
                SubItem.Click += SubItem_Click;
                Items[i] = SubItem;
            }

            MenuItem.DropDownItems.AddRange(Items);
        }

        public static void SetTextBox(FastColoredTextBox Box)
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
            InsertTxtToScript((sender as ToolStripItem).Text + " ");
        }

        private static void Tsmi_Click(object sender, EventArgs e)
        {
            InsertTxtToScript((sender as ToolStripItem).Text + " ");
        }

        private static void SubItem_Click(object sender, EventArgs e)
        {
            InsertTxtToScript((sender as ToolStripItem).Text);
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
