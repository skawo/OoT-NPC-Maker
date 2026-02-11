using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class BigCheckBox : CheckBox
    {
        public int BoxSize { get; set; } = 22; // default checkbox size

        public BigCheckBox()
        {
            this.Width = 200;

            if (Program.Settings != null)
                this.BoxSize = (int)(this.BoxSize * Program.Settings.GUIScale);

            UpdateSize();
        }

        private void UpdateSize()
        {
            this.AutoSize = false;

            using (Graphics g = this.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(this.Text, this.Font);
                this.Height = Math.Max(BoxSize + 4, (int)textSize.Height + 4);
                this.Width = BoxSize + 4 + (int)textSize.Width;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            UpdateSize();

            Graphics g = pevent.Graphics;
            g.Clear(this.BackColor == Color.Transparent ? Color.White : this.BackColor);

            // Draw the checkbox
            Rectangle boxRect = new Rectangle(0, (this.Height - BoxSize) / 2, BoxSize, BoxSize);
            ControlPaint.DrawCheckBox(g, boxRect,
                this.Checked ? ButtonState.Checked : ButtonState.Normal);

            // Draw the text
            using (Brush brush = new SolidBrush(this.ForeColor))
            {
                g.DrawString(this.Text, this.Font, brush, BoxSize + 4, (this.Height - BoxSize) / 2);
            }
        }
    }

}
