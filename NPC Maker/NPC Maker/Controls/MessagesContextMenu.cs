using FastColoredTextBoxNS;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace NPC_Maker
{
    public static class MessagesContextMenu
    {
        public static ContextMenuStrip MenuStrip;

        private static FastColoredTextBox Owner;

        public static ToolStripMenuItem CreateDictionaryItem(Tag t)
        {
            ToolStripMenuItem groupItem = new ToolStripMenuItem();
            groupItem.Text = t.ContextDict;

            if (t.ContextDict == Lists.SoundsDictType)
            {
                groupItem.Name = t.ContextDict;
                groupItem.Tag = t.Token + ";" + t.ContextDict;
                groupItem.Click += Sounds_Click;
                return groupItem;
            }
            else
            {
                int defIdx = Dicts.MsgDefinitions.FindIndex(x => x.Identifier == t.ContextDict);

                if (defIdx != -1)
                {
                    foreach (var tt in Dicts.MsgDefinitions[defIdx].Entries)
                        groupItem.DropDownItems.Add(new ToolStripMenuItem() { Text = tt.Key, Tag = t.Token + ";" + t.ContextDict });

                    return groupItem;
                }
            }

            return null;
        }

        public static void MakeContextMenu(string Language)
        {
            MenuStrip = new ContextMenuStrip();
            MenuStrip.Name = "ContextMenuStrip";
            MenuStrip.Size = new System.Drawing.Size(157, 268);
            MenuStrip.Text = "Items";

            var tagDict = Dicts.LanguageDefs[Dicts.DefaultLanguage];

            if (Dicts.LanguageDefs.ContainsKey(Language))
                tagDict = Dicts.LanguageDefs[Language];


            var uniqueContextGroups = tagDict.Entries.Select(t => t.ContextGroup).Distinct().ToList();

            foreach (var group in uniqueContextGroups)
            {
                if (String.IsNullOrWhiteSpace(group))
                    continue;

                ToolStripMenuItem groupItem = new ToolStripMenuItem();
                groupItem.Text = group;

                var groupedTags = tagDict.Entries.FindAll(x => x.ContextGroup == group);

                foreach (var tag in groupedTags)
                {
                    if (!String.IsNullOrWhiteSpace(tag.ContextName))
                    {
                        if (!String.IsNullOrWhiteSpace(tag.ContextDict))
                        {
                            var item = CreateDictionaryItem(tag);

                            if (item != null)
                                groupItem.DropDownItems.Add(item);
                        }
                        else
                            groupItem.DropDownItems.Add(new ToolStripMenuItem() { Text = tag.ContextName, Tag = tag.Token, ToolTipText = tag.Description });
                    }
                }

                MenuStrip.Items.Add(groupItem);
            }

            foreach (var tag in tagDict.Entries)
            {
                if (String.IsNullOrWhiteSpace(tag.ContextGroup) && !String.IsNullOrWhiteSpace(tag.ContextName))
                {
                    if (!String.IsNullOrWhiteSpace(tag.ContextName))
                    {
                        if (!String.IsNullOrWhiteSpace(tag.ContextDict))
                        {
                            var item = CreateDictionaryItem(tag);

                            if (item != null)
                                MenuStrip.Items.Add(item);
                        }
                        else
                            MenuStrip.Items.Add(new ToolStripMenuItem() { Text = tag.ContextName, Tag = tag.Token, ToolTipText = tag.Description });
                    }
                }
            }

            //ToolStripMenuItem toc = new ToolStripMenuItem();

            //toc.Size = new System.Drawing.Size(156, 22);
            //toc.Text = "Copy as C String";
            //toc.Click += Toc_Click;

            //MenuStrip.Items.Add(toc);

            foreach (ToolStripMenuItem Item in MenuStrip.Items)
            {
                if (Item.Name == Lists.SoundsDictType)
                    continue;

                if (Item.Tag != null)
                {
                    Item.DoubleClick += Tsmi_DoubleClick;
                    Item.Click += Tsmi_Click;
                }

                if (Item.HasDropDownItems)
                    Item.DropDown.MaximumSize = new Size(300, 700);

                foreach (ToolStripItem SubItem in Item.DropDownItems)
                    SubItem.Click += SubItem_Click;
            }

        }

        private static void Toc_Click(object sender, EventArgs e)
        {
            if (Owner == null)
                return;

            FCTB_Mono fb = (FCTB_Mono)Owner;
            string text = fb.SelectedText;

            if (String.IsNullOrWhiteSpace(text))
                text = fb.Text;

            MessageEntry me = new MessageEntry();
            me.MessageText = text;

            string outS = me.ToCString((string)fb.Tag);

            if (Program.IsRunningUnderMono)
            {
                try
                {
                    var tempFileName = Path.GetTempFileName();
                    File.WriteAllText(tempFileName, outS);
                    try
                    {
                        if (Program.IsWSL)
                            Helpers.RunBash($"cat {tempFileName} | clip.exe ");

                        Helpers.RunBash($"cat {tempFileName} | xsel -i --clipboard ");
                    }
                    finally
                    {
                        File.Delete(tempFileName);
                    }
                }
                catch (Exception)
                {

                }
            }
            else
                Clipboard.SetText(outS);
        }

        private static void Sounds_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList(Lists.DictType.SFX, true);
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                string tag = (sender as ToolStripItem).Tag as string;
                string[] subTag = tag.Split(';');

                InsertTxtToScript($"{subTag[0].Replace(subTag[1], SFX.Chosen.Name)}");
            }
        }

        public static void SetTextBox(FastColoredTextBox Box)
        {
            Owner = Box;
        }

        private static void InsertTxtToScript(string Text)
        {
            if (Owner == null)
                return;

            Point Scroll = Owner.AutoScrollOffset;
            int start = Owner.SelectionStart;
            string newTxt = Owner.Text;

            newTxt = newTxt.Remove(Owner.SelectionStart, Owner.SelectionLength);
            newTxt = newTxt.Insert(Owner.SelectionStart, Text);

            Owner.Text = newTxt;
            Owner.SelectionStart = start + Text.Length;
            Owner.AutoScrollOffset = Scroll;
            Owner.UpdateScrollbars();
        }

        private static void Tsmi_DoubleClick(object sender, EventArgs e)
        {
            InsertTxtToScript((sender as ToolStripItem).Tag as string);
        }

        private static void Tsmi_Click(object sender, EventArgs e)
        {
            InsertTxtToScript((sender as ToolStripItem).Tag as string);
        }

        private static void SubItem_Click(object sender, EventArgs e)
        {
            string tag = (sender as ToolStripItem).Tag as string;
            string[] subTag = tag.Split(';');

            if (subTag.Count() == 2)
                InsertTxtToScript($"{subTag[0].Replace(subTag[1], (sender as ToolStripItem).Text)}");
            else
                InsertTxtToScript((sender as ToolStripItem).Tag as string);
        }
    }
}
