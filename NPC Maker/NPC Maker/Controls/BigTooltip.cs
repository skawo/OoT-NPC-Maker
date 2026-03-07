using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Controls
{
    public class BigToolTip : ToolTip
    {
        public Font Font { get; set; } = new Font("Segoe UI", 10, FontStyle.Italic);
        public bool ShowRight = false;

        public BigToolTip()
        {
            OwnerDraw = true;
            Draw += OnDraw;
            Popup += OnPopup;
        }

        public new void SetToolTip(Control control, string text)
        {
            base.SetToolTip(control, text);

            control.MouseEnter += (s, e) => ShowLeft(control);
            control.MouseLeave += (s, e) => Hide(control);
        }

        private void ShowLeft(Control control)
        {
            if (!ShowRight)
            {
                string text = GetToolTip(control);
                if (string.IsNullOrEmpty(text)) return;

                Size tipSize;
                using (Graphics g = control.CreateGraphics())
                {
                    SizeF size = g.MeasureString(text, Font);
                    tipSize = new Size((int)size.Width + 10, (int)size.Height + 6);
                }

                Point screenPos = control.PointToScreen(Point.Empty);
                int x = screenPos.X - tipSize.Width - 5;
                int y = screenPos.Y;

                Show(text, control, control.PointToClient(new Point(x, y)), this.AutoPopDelay);
            }
        }

        private void OnPopup(object sender, PopupEventArgs e)
        {
            using (Graphics g = e.AssociatedControl.CreateGraphics())
            {
                SizeF size = g.MeasureString(GetToolTip(e.AssociatedControl), Font);
                e.ToolTipSize = new Size((int)size.Width + 10, (int)size.Height + 6);
            }
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                e.Graphics.DrawString(e.ToolTipText, Font,
                    Brushes.Black, e.Bounds, sf);
            }
        }
    }
}
