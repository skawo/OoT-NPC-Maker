using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Controls
{
    public class TabControl_MonoColorFix : TabControl
    {
        public TabControl_MonoColorFix()
        {
            if (!Program.IsRunningUnderMono || !Program.Settings.ChangeGUIColors)
                return;

            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw, true);

        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (!Program.IsRunningUnderMono || !Program.Settings.ChangeGUIColors)
                return;

            Color Back = Program.Settings.ChangeGUIColors ? Program.Settings.BGColor : BackColor;

            using (var brush = new SolidBrush(Back))
                pevent.Graphics.FillRectangle(brush, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Program.IsRunningUnderMono || !Program.Settings.ChangeGUIColors)
            {
                base.OnPaint(e);
                return;
            }

            Color Back = Program.Settings.ChangeGUIColors ? Program.Settings.BGColor : BackColor;
            Color Inactive = Program.Settings.ChangeGUIColors ? Program.Settings.DisabledColor : ControlPaint.Dark(Back, 0.05f);
            Color Input = Program.Settings.ChangeGUIColors ? Program.Settings.InputColor : ForeColor;

            for (int i = 0; i < TabCount; i++)
            {
                Rectangle tabRect = GetTabRect(i);
                bool isSelected = (i == SelectedIndex);

                using (var brush = new SolidBrush(isSelected ? Back : Inactive))
                    e.Graphics.FillRectangle(brush, tabRect);

                using (var pen = new Pen(ForeColor))
                    e.Graphics.DrawRectangle(pen, tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);

                TextRenderer.DrawText(e.Graphics, TabPages[i].Text, Font, tabRect, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            Rectangle pageRect = DisplayRectangle;
            using (var pen = new Pen(Input))
                e.Graphics.DrawRectangle(pen, pageRect.X - 1, pageRect.Y - 1, pageRect.Width + 1, pageRect.Height + 1);
        }
    }
}
