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

            // --- Form ---
            var f = new Form
            {
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowIcon = false,
                Font = font,
                TopMost = true,
                AutoSize = false,
            };

            // --- Label ---
            var lbl = new Label
            {
                Text = text,
                Font = font,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(16, 16, 16, 8),
            };

            // --- Button panel ---
            var pnlButtons = new Panel
            {
                AutoSize = false,
                Padding = new Padding(0, 8, 0, 12),
            };

            void AddButton(string btnText, DialogResult result)
            {
                var b = new Button
                {
                    Text = btnText,
                    Font = font,
                    DialogResult = result,
                    AutoSize = true,
                    MinimumSize = new Size(80, 28),
                    Margin = new Padding(5, 0, 5, 0)
                };

                pnlButtons.Controls.Add(b);

                if (f.AcceptButton == null && (result == DialogResult.OK || result == DialogResult.Yes))
                    f.AcceptButton = b;
                if (f.CancelButton == null && (result == DialogResult.Cancel || result == DialogResult.No))
                    f.CancelButton = b;
            }

            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:
                    AddButton("OK", DialogResult.OK);
                    AddButton("Cancel", DialogResult.Cancel);
                    break;
                case MessageBoxButtons.YesNo:
                    AddButton("Yes", DialogResult.Yes);
                    AddButton("No", DialogResult.No);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    AddButton("Yes", DialogResult.Yes);
                    AddButton("No", DialogResult.No);
                    AddButton("Cancel", DialogResult.Cancel);
                    break;
                default:
                    AddButton("OK", DialogResult.OK);
                    break;
            }

            f.Controls.Add(pnlButtons);
            f.Controls.Add(lbl);

            f.Shown += (s, e) =>
            {
                // Force buttons to measure themselves
                foreach (Control c in pnlButtons.Controls)
                    c.Size = c.GetPreferredSize(Size.Empty);

                // Measure content
                Size lblPreferred = lbl.GetPreferredSize(new Size(600, 0));
                int lblWidth = Math.Min(lblPreferred.Width + lbl.Padding.Horizontal, 600);
                int lblHeight = lblPreferred.Height + lbl.Padding.Vertical;

                // Measure button row
                int btnRowWidth = pnlButtons.Controls.Cast<Control>().Sum(c => c.Width + c.Margin.Horizontal);
                int btnRowHeight = pnlButtons.Controls.Cast<Control>().Max(c => c.Height);
                int btnPanelHeight = btnRowHeight + pnlButtons.Padding.Vertical + 8;

                // Final client size
                int clientWidth = Math.Max(lblWidth, btnRowWidth + 64);
                int clientHeight = lblHeight + btnPanelHeight;

                f.ClientSize = new Size(clientWidth, clientHeight);

                // Position label
                lbl.Location = new Point(0, 0);
                lbl.Size = new Size(clientWidth, lblHeight);

                // Position button panel
                pnlButtons.Location = new Point(0, lblHeight);
                pnlButtons.Size = new Size(clientWidth, btnPanelHeight);

                // Center buttons within panel
                int btnTop = (btnPanelHeight - btnRowHeight) / 2;
                int btnLeft = (clientWidth - btnRowWidth) / 2;

                foreach (Control c in pnlButtons.Controls)
                {
                    c.Location = new Point(btnLeft, btnTop);
                    btnLeft += c.Width + c.Margin.Horizontal;
                }

                // Center form on owner or screen
                if (owner != null)
                {
                    f.Location = new Point(
                        owner.Location.X + (owner.Width - f.Width) / 2,
                        owner.Location.Y + (owner.Height - f.Height) / 2);
                }
                else
                {
                    var screen = Screen.PrimaryScreen.WorkingArea;
                    f.Location = new Point(
                        (screen.Width - f.Width) / 2,
                        (screen.Height - f.Height) / 2);
                }

                f.Activate();
                f.BringToFront();
            };
            return f.ShowDialog(owner);
        }
    }
}