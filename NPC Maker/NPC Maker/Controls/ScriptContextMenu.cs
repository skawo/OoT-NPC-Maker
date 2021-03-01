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
        private static ToolStripMenuItem keyValuesToolStripMenuItem;
        private static ToolStripMenuItem itemsToolStripMenuItem;
        private static ToolStripMenuItem itemsgiveToolStripMenuItem;
        private static ToolStripMenuItem questItemsToolStripMenuItem;
        private static ToolStripMenuItem itemsdungeonToolStripMenuItem;
        private static ToolStripMenuItem itemstradeToolStripMenuItem;
        private static ToolStripMenuItem playerMasksToolStripMenuItem;
        private static ToolStripMenuItem soundEffectsToolStripMenuItem;
        private static ToolStripMenuItem musicToolStripMenuItem;
        private static ToolStripMenuItem actorstoolStripMenuItem;

        public static void MakeContextMenu()
        {
            ContextMenuStrip = new ContextMenuStrip();

            functionsToolStripMenuItem = new ToolStripMenuItem();
            keywordsToolStripMenuItem = new ToolStripMenuItem();
            keyValuesToolStripMenuItem = new ToolStripMenuItem();
            itemsToolStripMenuItem = new ToolStripMenuItem();
            itemsgiveToolStripMenuItem = new ToolStripMenuItem();
            questItemsToolStripMenuItem = new ToolStripMenuItem();
            itemsdungeonToolStripMenuItem = new ToolStripMenuItem();
            itemstradeToolStripMenuItem = new ToolStripMenuItem();
            playerMasksToolStripMenuItem = new ToolStripMenuItem();
            soundEffectsToolStripMenuItem = new ToolStripMenuItem();
            musicToolStripMenuItem = new ToolStripMenuItem();
            actorstoolStripMenuItem = new ToolStripMenuItem();


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
                                                                    actorstoolStripMenuItem
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
            itemsToolStripMenuItem.Text = "Inventory Items";
            // 
            // itemsgiveToolStripMenuItem
            // 
            itemsgiveToolStripMenuItem.Name = "itemsgiveToolStripMenuItem";
            itemsgiveToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            itemsgiveToolStripMenuItem.Text = "Give Items";
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
            itemsdungeonToolStripMenuItem.Text = "Dungeon Items";
            itemsdungeonToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // itemstradeToolStripMenuItem
            // 
            itemstradeToolStripMenuItem.Name = "itemstradeToolStripMenuItem";
            itemstradeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            itemstradeToolStripMenuItem.Text = "Trade Items";
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
            // 
            // musicToolStripMenuItem
            // 
            musicToolStripMenuItem.Name = "musicToolStripMenuItem";
            musicToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            musicToolStripMenuItem.Text = "Music";
            // 
            // actorstoolStripMenuItem
            // 
            actorstoolStripMenuItem.Name = "actorstoolStripMenuItem";
            actorstoolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            actorstoolStripMenuItem.Text = "Actors";

            foreach (string Item in Enum.GetNames(typeof(NewScriptParser.Lists.Instructions)))
            {
                ToolStripMenuItem Tsmi = new ToolStripMenuItem
                {
                    Text = Item
                };

                if (NewScriptParser.Lists.FunctionSubtypes.ContainsKey(Item))
                {
                    Tsmi.DoubleClickEnabled = true;
                    Tsmi.DoubleClick += Tsmi_DoubleClick;
                    AddItemCollectionToToolStripMenuItem(NewScriptParser.Lists.FunctionSubtypes[Item], Tsmi);
                }
                else
                    Tsmi.Click += Tsmi_Click;

                functionsToolStripMenuItem.DropDownItems.Add(Tsmi);
            }

            AddItemCollectionToToolStripMenuItem(NewScriptParser.Lists.KeyValues.ToArray(), keywordsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(NewScriptParser.Lists.AllKeywords.ToArray(), keywordsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(NewScriptParser.Lists.GiveItems)), itemsgiveToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(NewScriptParser.Lists.TradeItems)), itemstradeToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(NewScriptParser.Lists.DungeonItems)), itemsdungeonToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(NewScriptParser.Lists.Items)), itemsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(NewScriptParser.Lists.QuestItems)), questItemsToolStripMenuItem);
            AddItemCollectionToToolStripMenuItem(Enum.GetNames(typeof(NewScriptParser.Lists.PlayerMasks)), playerMasksToolStripMenuItem);

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
            PickableList SFX = new PickableList("SFX.csv");
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(SFX.Chosen);
            }
        }

        private static void MusicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList("Music.csv");
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(SFX.Chosen);
            }
        }

        private static void ActorstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            PickableList Actors = new PickableList("Actors.csv");
            DialogResult DR = Actors.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript(Actors.Chosen);
            }
        }
    }
}
