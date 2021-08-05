using FastColoredTextBoxNS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NPC_Maker
{
    public static class MessagesContextMenu
    {
        public static ContextMenuStrip MenuStrip;

        private static FastColoredTextBox Owner;

        private static ToolStripMenuItem colors;
        private static ToolStripMenuItem sounds;
        private static ToolStripMenuItem highscore;
        private static ToolStripMenuItem newtextbox;
        private static ToolStripMenuItem playername;
        private static ToolStripMenuItem noskip;
        private static ToolStripMenuItem icon;
        private static ToolStripMenuItem speed;
        private static ToolStripMenuItem fade;
        private static ToolStripMenuItem shopdescription;
        private static ToolStripMenuItem drawinstant;
        private static ToolStripMenuItem drawchar;
        private static ToolStripMenuItem twochoices;
        private static ToolStripMenuItem threechoices;
        private static ToolStripMenuItem delay;

        public static void MakeContextMenu()
        {
            MenuStrip = new ContextMenuStrip();

            colors = new ToolStripMenuItem();
            sounds = new ToolStripMenuItem();
            highscore = new ToolStripMenuItem();
            newtextbox = new ToolStripMenuItem();
            playername = new ToolStripMenuItem();
            noskip = new ToolStripMenuItem();
            icon = new ToolStripMenuItem();
            speed = new ToolStripMenuItem();
            fade = new ToolStripMenuItem();
            shopdescription = new ToolStripMenuItem();
            drawinstant = new ToolStripMenuItem();
            drawchar = new ToolStripMenuItem();
            twochoices = new ToolStripMenuItem();
            threechoices = new ToolStripMenuItem();
            delay = new ToolStripMenuItem();

            MenuStrip.Items.AddRange(new ToolStripItem[] {
                                                                    colors,
                                                                    sounds,
                                                                    highscore,
                                                                    newtextbox,
                                                                    playername,
                                                                    noskip,
                                                                    icon,
                                                                    speed,
                                                                    fade,
                                                                    delay,
                                                                    shopdescription,
                                                                    drawinstant,
                                                                    drawchar,
                                                                    twochoices,
                                                                    threechoices,
                                                                });
            MenuStrip.Name = "ContextMenuStrip";
            MenuStrip.Size = new System.Drawing.Size(157, 268);
            MenuStrip.Text = "Items";

            // 
            // colors
            // 
            colors.Size = new System.Drawing.Size(156, 22);
            colors.Text = "Color";


            colors.DropDownItems.AddRange(new ToolStripMenuItem[]
                                            { 
                                                new ToolStripMenuItem() { Text = "White", Tag = "<W>" },
                                                new ToolStripMenuItem() { Text = "Red", Tag = "<R>" },
                                                new ToolStripMenuItem() { Text = "Green", Tag = "<G>" },
                                                new ToolStripMenuItem() { Text = "Blue", Tag = "<B>" },
                                                new ToolStripMenuItem() { Text = "Cyan", Tag = "<C>" },
                                                new ToolStripMenuItem() { Text = "Magenta", Tag = "<M>" },
                                                new ToolStripMenuItem() { Text = "Yellow", Tag = "<Y>" },
                                                new ToolStripMenuItem() { Text = "Black", Tag = "<Blk>" }
                                            }
                                         );    
            // 
            // sounds
            // 
            sounds.Size = new System.Drawing.Size(156, 22);
            sounds.Text = "Sound";

            sounds.DropDownItems.AddRange(new ToolStripMenuItem[]
                                            { 
                                                new ToolStripMenuItem() { Text = "Item fanfare", Tag = "<Sound:Item Fanfare>" },
                                                new ToolStripMenuItem() { Text = "Cow mooing", Tag = "<Sound:Moo>" },
                                                new ToolStripMenuItem() { Text = "Frog ribbit 1", Tag = "<Sound:Frog Ribbit 1>" },
                                                new ToolStripMenuItem() { Text = "Frog ribbit 2", Tag = "<Sound:Frog Ribbit 2>" },
                                                new ToolStripMenuItem() { Text = "Deku squeak", Tag = "<Sound:Deku Squeak>" },
                                                new ToolStripMenuItem() { Text = "Generic event", Tag = "<Sound:Generic Event>" },
                                                new ToolStripMenuItem() { Text = "Poe vanishing", Tag = "<Sound:Poe Vanishing>" },
                                                new ToolStripMenuItem() { Text = "Twinrova 1", Tag = "<Sound:Twinrova 1>" },
                                                new ToolStripMenuItem() { Text = "Twinrova 2", Tag = "<Sound:Twinrova 2>" },
                                                new ToolStripMenuItem() { Text = "Navi hello", Tag = "<Sound:Navi Hello>" },
                                                new ToolStripMenuItem() { Text = "Talon Ehh", Tag = "<Sound:Talon Ehh>" },
                                                new ToolStripMenuItem() { Text = "Carpenter Waaaa", Tag = "<Sound:Carpenter Waaaa>" },
                                                new ToolStripMenuItem() { Text = "Navi HEY!", Tag = "<Sound:Navi Hey>" },
                                                new ToolStripMenuItem() { Text = "Saria giggle", Tag = "<Sound:Saria Giggle>" },
                                                new ToolStripMenuItem() { Text = "Yaaaa", Tag = "<Sound:Yaaaa>" },
                                                new ToolStripMenuItem() { Text = "Zelda heh", Tag = "<Sound:Zelda Heh>" },
                                                new ToolStripMenuItem() { Text = "Zelda awww", Tag = "<Sound:Zelda Awww>" },
                                                new ToolStripMenuItem() { Text = "Zelda huh", Tag = "<Sound:Zelda Huh>" },
                                                new ToolStripMenuItem() { Text = "Generic giggle", Tag = "<Sound:Generic Giggle>" },
                                                new ToolStripMenuItem() { Text = "Unused", Tag = "<Sound:Unused 1>" }
                                            }
                                         );


            // 
            // highscore
            // 
            highscore.Size = new System.Drawing.Size(156, 22);
            highscore.Text = "High score";

            highscore.DropDownItems.AddRange(new ToolStripMenuItem[]
                                            {
                                                new ToolStripMenuItem() { Text = "Archery", Tag = "<High Score:Archery>" },
                                                new ToolStripMenuItem() { Text = "Poe Salesman Points", Tag = "<High Score:Poe Points>" },
                                                new ToolStripMenuItem() { Text = "Fishing", Tag = "<High Score:Fishing>" },
                                                new ToolStripMenuItem() { Text = "Horse race", Tag = "<High Score:Horse Race>" },
                                                new ToolStripMenuItem() { Text = "Running Man's Marathon", Tag = "<High Score:Marathon>" },
                                                new ToolStripMenuItem() { Text = "Dampe Race", Tag = "<High Score:Dampe Race>" },
                                            }
                                         );

            // 
            // newtextbox
            // 
            newtextbox.Size = new System.Drawing.Size(156, 22);
            newtextbox.Text = "New textbox";
            newtextbox.Tag = "<New Box>";
            // 
            // playername
            // 
            playername.Size = new System.Drawing.Size(156, 22);
            playername.Text = "Player name";
            playername.Tag = "<Player>";
            // 
            // noskip
            // 
            noskip.Size = new System.Drawing.Size(156, 22);
            noskip.Text = "No skip";
            noskip.Tag = "<NS>";
            // 
            // fade
            // 
            fade.Size = new System.Drawing.Size(156, 22);
            fade.Text = "Fade";
            fade.Tag = "<Fade:0>";
            // 
            // icon
            // 
            icon.Size = new System.Drawing.Size(156, 22);
            icon.Text = "Icon";
            icon.Tag = "<Icon:0>";
            // 
            // speed
            // 
            speed.Size = new System.Drawing.Size(156, 22);
            speed.Text = "Speed";
            speed.Tag = "<Speed:0>";
            // 
            // shopdescription
            // 
            shopdescription.Size = new System.Drawing.Size(156, 22);
            shopdescription.Text = "Shop description";
            shopdescription.Tag = "<Shop Description>";
            // 
            // drawinstant
            // 
            drawinstant.Size = new System.Drawing.Size(156, 22);
            drawinstant.Text = "Draw instantly";
            drawinstant.Tag = "<DI>";
            // 
            // drawchar
            // 
            drawchar.Size = new System.Drawing.Size(156, 22);
            drawchar.Text = "Draw per-character";
            drawchar.Tag = "<DC>";
            // 
            // twochoices
            // 
            twochoices.Size = new System.Drawing.Size(156, 22);
            twochoices.Text = "Two choices";
            twochoices.Tag = "<Two Choices>";
            // 
            // threechoices
            // 
            threechoices.Size = new System.Drawing.Size(156, 22);
            threechoices.Text = "Three choices";
            threechoices.Tag = "<Three Choices>";
            // 
            // Delay
            // 
            delay.Size = new System.Drawing.Size(156, 22);
            delay.Text = "Delay";
            delay.Tag = "<Delay:0>";

            foreach (ToolStripMenuItem Item in MenuStrip.Items)
            {
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
            InsertTxtToScript((sender as ToolStripItem).Tag as string);
        }
    }
}
