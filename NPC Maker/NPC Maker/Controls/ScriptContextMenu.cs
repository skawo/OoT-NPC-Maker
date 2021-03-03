using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private static ToolStripMenuItem acatstoolStripMenuItem;
        private static ToolStripMenuItem objectstoolStripMenuItem;

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
            acatstoolStripMenuItem = new ToolStripMenuItem();
            objectstoolStripMenuItem = new ToolStripMenuItem();


            ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                                                                    functionsToolStripMenuItem,
                                                                    keywordsToolStripMenuItem,
                                                                    itemsToolStripMenuItem,
                                                                    itemsgiveToolStripMenuItem,
                                                                    questItemsToolStripMenuItem,
                                                                    itemsdungeonToolStripMenuItem,
                                                                    itemstradeToolStripMenuItem,
                                                                    playerMasksToolStripMenuItem,
                                                                    soundEffectsToolStripMenuItem,
                                                                    musicToolStripMenuItem,
                                                                    actorstoolStripMenuItem,
                                                                    acatstoolStripMenuItem,
                                                                    objectstoolStripMenuItem,
                                                                });
            ContextMenuStrip.Name = "ContextMenuStrip";
            ContextMenuStrip.Size = new System.Drawing.Size(157, 268);
            ContextMenuStrip.Text = "Items";

            // 
            // functionsToolStripMenuItem
            // 
            functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            functionsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            functionsToolStripMenuItem.Text = "Functions";
            // 
            // keywordsToolStripMenuItem
            // 
            keywordsToolStripMenuItem.Name = "keywordsToolStripMenuItem";
            keywordsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            keywordsToolStripMenuItem.Text = "Keywords";
            // 
            // itemsToolStripMenuItem
            // 
            itemsToolStripMenuItem.Name = "itemsToolStripMenuItem";
            itemsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            itemsToolStripMenuItem.Text = "Inventory items";
            // 
            // itemsgiveToolStripMenuItem
            // 
            itemsgiveToolStripMenuItem.Name = "itemsgiveToolStripMenuItem";
            itemsgiveToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            itemsgiveToolStripMenuItem.Text = "Award items";
            // 
            // questItemsToolStripMenuItem
            // 
            questItemsToolStripMenuItem.Name = "questItemsToolStripMenuItem";
            questItemsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            questItemsToolStripMenuItem.Text = "Quest items";
            // 
            // itemsdungeonToolStripMenuItem
            // 
            itemsdungeonToolStripMenuItem.Name = "itemsdungeonToolStripMenuItem";
            itemsdungeonToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            itemsdungeonToolStripMenuItem.Text = "Dungeon items";
            itemsdungeonToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // itemstradeToolStripMenuItem
            // 
            itemstradeToolStripMenuItem.Name = "itemstradeToolStripMenuItem";
            itemstradeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            itemstradeToolStripMenuItem.Text = "Trade items";
            // 
            // playerMasksToolStripMenuItem
            // 
            playerMasksToolStripMenuItem.Name = "playerMasksToolStripMenuItem";
            playerMasksToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            playerMasksToolStripMenuItem.Text = "Player Masks";
            // 
            // soundEffectsToolStripMenuItem
            // 
            soundEffectsToolStripMenuItem.Name = "soundEffectsToolStripMenuItem";
            soundEffectsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            soundEffectsToolStripMenuItem.Text = "Sound effects";
            soundEffectsToolStripMenuItem.Click += SoundEffectsToolStripMenuItem_Click;
            // 
            // musicToolStripMenuItem
            // 
            musicToolStripMenuItem.Name = "musicToolStripMenuItem";
            musicToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            musicToolStripMenuItem.Text = "Music";
            musicToolStripMenuItem.Click += MusicToolStripMenuItem_Click;
            // 
            // actorstoolStripMenuItem
            // 
            actorstoolStripMenuItem.Name = "actorstoolStripMenuItem";
            actorstoolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            actorstoolStripMenuItem.Text = "Actors";
            actorstoolStripMenuItem.Click += ActorstoolStripMenuItem_Click;
            // 
            // acatstoolStripMenuItem
            // 
            acatstoolStripMenuItem.Name = "acatstoolStripMenuItem";
            acatstoolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            acatstoolStripMenuItem.Text = "Actor categories";
            acatstoolStripMenuItem.Click += AcatstoolStripMenuItem_Click;
            // 
            // objectstoolStripMenuItem
            // 
            objectstoolStripMenuItem.Name = "objectstoolStripMenuItem";
            objectstoolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            objectstoolStripMenuItem.Text = "Objects";
            objectstoolStripMenuItem.Click += ObjectstoolStripMenuItem_Click;

            foreach (string Item in Enum.GetNames(typeof(Lists.Instructions)))
            {
                ToolStripMenuItem Tsmi = new ToolStripMenuItem
                {
                    Text = Item
                };

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
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.AwardItems)), itemsgiveToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.TradeItems)), itemstradeToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.DungeonItems)), itemsdungeonToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.Items)), itemsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.QuestItems)), questItemsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(Lists.PlayerMasks)), playerMasksToolStripMenuItem);

        }

        private static void AddItemCollectionToToolStripMenuItem(string[] Collection, ToolStripMenuItem MenuItem)
        {
            MenuItem.DropDown.MaximumSize = new Size(300, 700);

            foreach (string Item in Collection)
            {
                ToolStripItem SubItem = MenuItem.DropDownItems.Add(Item);
                SubItem.Click += SubItem_Click;
            }
        }

        public static void SetTextBox(FastColoredTextBox Box)
        {
            LastClickedTextbox = Box;
        }

        private static void InsertTxtToScript(string Text)
        {
            if (LastClickedTextbox == null)
                return;

            int start = LastClickedTextbox.SelectionStart;
            string newTxt = LastClickedTextbox.Text;
            newTxt = newTxt.Remove(LastClickedTextbox.SelectionStart, LastClickedTextbox.SelectionLength);
            newTxt = newTxt.Insert(LastClickedTextbox.SelectionStart, Text);
            LastClickedTextbox.Text = newTxt;
            LastClickedTextbox.SelectionStart = start + Text.Length;
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

        private static void AcatstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList Acats = new PickableList(Lists.DictType.ActorCategories, true);
            DialogResult DR = Acats.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(Acats.Chosen.Name);
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
    }
}
