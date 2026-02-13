using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class BigMessageBox
    {
        public static DialogResult Show(string text, string caption = "",
                                        MessageBoxButtons buttons = MessageBoxButtons.OK,
                                        Font font = null, Form owner = null)
        {
            if (font == null)
                font = new Font("Microsoft Sans Serif", Helpers.GetScaleFontSize());

            Form f = new Form();
            f.Text = caption;
            f.StartPosition = owner != null ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            f.FormBorderStyle = FormBorderStyle.Sizable;
            f.MinimizeBox = false;
            f.MaximizeBox = false;
            f.ShowIcon = false;
            f.Font = font;
            f.AutoSize = true;
            f.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            f.TopMost = true;

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.RowCount = 2;
            layout.ColumnCount = 1;
            layout.Dock = DockStyle.Fill;
            layout.AutoSize = true;
            layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // label
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // buttons
            f.Controls.Add(layout);

            // Label
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = font;
            lbl.AutoSize = true;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Padding = new Padding(10);
            lbl.MaximumSize = new Size(600, 0); // wrap long text
            layout.Controls.Add(lbl, 0, 0);

            // Buttons panel
            FlowLayoutPanel pnlButtons = new FlowLayoutPanel();
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Dock = DockStyle.Fill;
            pnlButtons.Padding = new Padding(10);
            pnlButtons.AutoSize = true;
            pnlButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            layout.Controls.Add(pnlButtons, 0, 1);

            // Helper to add buttons
            Action<string, DialogResult> AddButton = delegate (string textBtn, DialogResult result)
            {
                Button b = new Button();
                b.Text = textBtn;
                b.AutoSize = true;
                b.Font = font;
                b.DialogResult = result;
                b.Margin = new Padding(5);
                pnlButtons.Controls.Add(b);

                if (f.AcceptButton == null && (result == DialogResult.OK || result == DialogResult.Yes))
                    f.AcceptButton = b;

                if (f.CancelButton == null && (result == DialogResult.Cancel || result == DialogResult.No))
                    f.CancelButton = b;
            };

            // Add buttons based on MessageBoxButtons
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    AddButton("OK", DialogResult.OK);
                    break;
                case MessageBoxButtons.OKCancel:
                    AddButton("Cancel", DialogResult.Cancel);
                    AddButton("OK", DialogResult.OK);
                    break;
                case MessageBoxButtons.YesNo:
                    AddButton("No", DialogResult.No);
                    AddButton("Yes", DialogResult.Yes);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    AddButton("Cancel", DialogResult.Cancel);
                    AddButton("No", DialogResult.No);
                    AddButton("Yes", DialogResult.Yes);
                    break;
                default:
                    AddButton("OK", DialogResult.OK);
                    break;
            }

            return f.ShowDialog(owner);
        }
    }
}