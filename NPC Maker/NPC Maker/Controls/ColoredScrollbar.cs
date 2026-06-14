using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Controls
{
    public class ColoredMonoScrollbar : VScrollBar
    {
        public Color OverlayColor { get; set; } = Color.FromArgb(45, 0, 0, 0);

        public ColoredMonoScrollbar()
        {
            Scroll += (s, e) => Invalidate();
            ValueChanged += (s, e) => Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (Program.IsRunningUnderMono)
            {
                int trackTop = SystemInformation.VerticalScrollBarArrowHeight;
                int trackHeight = Height - trackTop * 2;
                double range = Maximum - Minimum - LargeChange;
                double thumbPos = range > 0 ? (Value - Minimum) / range : 0;

                int thumbHeight = Math.Max(20, trackHeight * LargeChange / Math.Max(1, Maximum - Minimum));
                int thumbTop = trackTop + (int)(thumbPos * (trackHeight - thumbHeight));

                using (var g = Graphics.FromHwnd(Handle))
                {
                    using (var brush = new SolidBrush(OverlayColor))
                    {
                        g.FillRectangle(brush, 0, thumbTop, Width, thumbHeight);
                    }
                }
            }

        }
    }
}