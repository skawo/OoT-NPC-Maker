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
        private static ToolStripMenuItem buttonPrompt;

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
            buttonPrompt = new ToolStripMenuItem();

            MenuStrip.Items.AddRange(new ToolStripItem[] {
                                                                    colors,
                                                                    highscore,
                                                                    sounds,
                                                                    newtextbox,
                                                                    buttonPrompt,
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
                                                new ToolStripMenuItem() { Text = "White", Tag = $"<{Lists.MsgColor.W}>" },
                                                new ToolStripMenuItem() { Text = "Red", Tag = $"<{Lists.MsgColor.R}>" },
                                                new ToolStripMenuItem() { Text = "Green", Tag = $"<{Lists.MsgColor.G}>" },
                                                new ToolStripMenuItem() { Text = "Blue", Tag = $"<{Lists.MsgColor.B}>" },
                                                new ToolStripMenuItem() { Text = "Cyan", Tag = $"<{Lists.MsgColor.C}>" },
                                                new ToolStripMenuItem() { Text = "Magenta", Tag = $"<{Lists.MsgColor.M}>" },
                                                new ToolStripMenuItem() { Text = "Yellow", Tag = $"<{Lists.MsgColor.Y}>" },
                                                new ToolStripMenuItem() { Text = "Black", Tag = $"<{Lists.MsgColor.BLK}>" }
                                            }
                                         );    
            // 
            // sounds
            // 
            sounds.Size = new System.Drawing.Size(156, 22);
            sounds.Text = "Sound";
            sounds.Tag = Lists.MsgControlCode.SOUND.ToString();
            sounds.Click += Sounds_Click;

            // 
            // highscore
            // 
            highscore.Size = new System.Drawing.Size(156, 22);
            highscore.Text = "High score";

            highscore.DropDownItems.AddRange(new ToolStripMenuItem[]
                                            {
                                                new ToolStripMenuItem() { Text = "Archery", Tag = $"<{Lists.MsgControlCode.HIGH_SCORE}:{Lists.MsgHighScore.ARCHERY}>" },
                                                new ToolStripMenuItem() { Text = "Poe Salesman Points", Tag = $"<{Lists.MsgControlCode.HIGH_SCORE}:{Lists.MsgHighScore.POE_POINTS}>" },
                                                new ToolStripMenuItem() { Text = "Fishing", Tag = $"<{Lists.MsgControlCode.HIGH_SCORE}:{Lists.MsgHighScore.FISHING}>" },
                                                new ToolStripMenuItem() { Text = "Horse race", Tag = $"<{Lists.MsgControlCode.HIGH_SCORE}:{Lists.MsgHighScore.HORSE_RACE}>" },
                                                new ToolStripMenuItem() { Text = "Running Man's Marathon", Tag = $"<{Lists.MsgControlCode.HIGH_SCORE}:{Lists.MsgHighScore.MARATHON}>" },
                                                new ToolStripMenuItem() { Text = "Dampe Race", Tag = $"<{Lists.MsgControlCode.HIGH_SCORE}:{Lists.MsgHighScore.DAMPE_RACE}>" },
                                            }
                                         );

            // 
            // newtextbox
            // 
            newtextbox.Size = new System.Drawing.Size(156, 22);
            newtextbox.Text = "New textbox";
            newtextbox.Tag = $"<{Lists.MsgControlCode.NEW_BOX}>";
            // 
            // newtextbox
            // 
            buttonPrompt.Size = new System.Drawing.Size(156, 22);
            buttonPrompt.Text = "Button prompt";
            buttonPrompt.Tag = $"<{Lists.MsgControlCode.BUTTON_PROMPT}>";
            // 
            // playername
            // 
            playername.Size = new System.Drawing.Size(156, 22);
            playername.Text = "Player name";
            playername.Tag = $"<{Lists.MsgControlCode.PLAYER}>";
            // 
            // noskip
            // 
            noskip.Size = new System.Drawing.Size(156, 22);
            noskip.Text = "No skip";
            noskip.Tag = $"<{Lists.MsgControlCode.NS}>";
            // 
            // fade
            // 
            fade.Size = new System.Drawing.Size(156, 22);
            fade.Text = "Fade";
            fade.Tag = $"<{Lists.MsgControlCode.FADE}:0>";
            // 
            // icon
            // 
            icon.Size = new System.Drawing.Size(156, 22);
            icon.Text = "Icon";
            icon.Tag = $"<{Lists.MsgControlCode.ICON}:0>";
            // 
            // speed
            // 
            speed.Size = new System.Drawing.Size(156, 22);
            speed.Text = "Speed";
            speed.Tag = $"<{Lists.MsgControlCode.SPEED}:0>";
            // 
            // shopdescription
            // 
            shopdescription.Size = new System.Drawing.Size(156, 22);
            shopdescription.Text = "Shop description";
            shopdescription.Tag = $"<{Lists.MsgControlCode.SHOP_DESCRIPTION}>";
            // 
            // drawinstant
            // 
            drawinstant.Size = new System.Drawing.Size(156, 22);
            drawinstant.Text = "Draw instantly";
            drawinstant.Tag = $"<{Lists.MsgControlCode.DI}>";
            // 
            // drawchar
            // 
            drawchar.Size = new System.Drawing.Size(156, 22);
            drawchar.Text = "Draw per-character";
            drawchar.Tag = $"<{Lists.MsgControlCode.DC}>";
            // 
            // twochoices
            // 
            twochoices.Size = new System.Drawing.Size(156, 22);
            twochoices.Text = "Two choices";
            twochoices.Tag = $"<{Lists.MsgControlCode.TWO_CHOICES}>";
            // 
            // threechoices
            // 
            threechoices.Size = new System.Drawing.Size(156, 22);
            threechoices.Text = "Three choices";
            threechoices.Tag = $"<{Lists.MsgControlCode.THREE_CHOICES}>";
            // 
            // Delay
            // 
            delay.Size = new System.Drawing.Size(156, 22);
            delay.Text = "Delay";
            delay.Tag = $"<{Lists.MsgControlCode.DELAY}:0>";

            foreach (ToolStripMenuItem Item in MenuStrip.Items)
            {
                if (Item == sounds)
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

        private static void Sounds_Click(object sender, EventArgs e)
        {
            PickableList SFX = new PickableList(Lists.DictType.SFX, true);
            DialogResult DR = SFX.ShowDialog();

            if (DR == DialogResult.OK)
            {
                InsertTxtToScript($"<{(sender as ToolStripItem).Tag as string}:{SFX.Chosen.Name}>");
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
