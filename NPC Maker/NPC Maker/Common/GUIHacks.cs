using NPC_Maker.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Common
{
    public static class GUIHacks
    {
        private static void AdjustControlForNeighbour(Control ctrl)
        {
            int newHeight = ctrl.Height;

            if (ctrl.Parent != null)
            {
                if (ctrl.Parent is Panel par)
                {
                    int maxRight = par.ClientSize.Width - 5;

                    foreach (Control sibling in par.Controls)
                    {
                        if (sibling == ctrl) continue;

                        if (Math.Abs(sibling.Top - ctrl.Top) < 2) // 2 pixels tolerance
                        {
                            if (sibling.Left > ctrl.Left)
                            {
                                maxRight = Math.Min(maxRight, sibling.Left);
                                newHeight = sibling.Height;
                            }
                        }
                    }

                    ctrl.Width = maxRight - ctrl.Left - 5;
                }
            }

            ctrl.Height = newHeight;
        }

        public static void AdjustControlScale(Control ctr)
        {
            if (Program.Settings.GUIScale == 1.0f)
                return;

            float fontSize = GUIHacks.GetScaleFontSize();

            foreach (Control ctrl in ctr.Controls)
            {
                if (ctrl is ScriptEditor)
                {
                    (ctrl as ScriptEditor).SetupScale();
                }
                if (ctrl is DataGridView)
                {
                    ctrl.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as DataGridView).DefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as DataGridView).ColumnHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, Math.Min(11, fontSize));
                    (ctrl as DataGridView).RowHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                }
                else if (ctrl is SegmentDataGrid)
                {
                    ctrl.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as SegmentDataGrid).Grid.DefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as SegmentDataGrid).Grid.ColumnHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, Math.Min(11, fontSize));
                    (ctrl as SegmentDataGrid).Grid.RowHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                }
                else if (ctrl is FCTB_Mono)
                {
                    (ctrl as FCTB_Mono).Font = new Font(ctr.Font.FontFamily, fontSize);
                }
                else if (ctrl is DateTimePicker)
                {
                    if (Program.IsRunningUnderMono)
                    {
                        AdjustControlForNeighbour(ctrl);
                        ctrl.Font = new Font(ctr.Font.FontFamily, Math.Max(8.25f, fontSize - 3));
                    }
                }
                else if (ctrl is NumericUpDown)
                {
                    if (Program.IsRunningUnderMono)
                    {
                        AdjustControlForNeighbour(ctrl);
                        ctrl.Font = new Font(ctr.Font.FontFamily, Math.Max(8.25f, fontSize - 2));
                    }
                }

                if (ctrl.HasChildren)
                    AdjustControlScale(ctrl);
            }
        }

        public static void SetExplicitColors(Control root)
        {
            if (Program.Settings == null)
                return;

            var visitedMenus = new HashSet<ToolStrip>();
            SetExplicitColorsInternal(root, visitedMenus);
        }

        private static void SetExplicitColorsInternal(Control root, HashSet<ToolStrip> visitedMenus)
        {
            Color back = Program.Settings.BGColor;
            Color fore = Program.Settings.TextColor;
            Color input = Program.Settings.InputColor;
            Color disabled = Program.Settings.DisabledColor;

            switch (root)
            {
                case DataGridView grid:
                    grid.ColumnHeadersDefaultCellStyle.BackColor = back;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = fore;
                    grid.EnableHeadersVisualStyles = false;
                    grid.RowHeadersDefaultCellStyle.BackColor = back;
                    grid.RowHeadersDefaultCellStyle.ForeColor = fore;
                    grid.RowsDefaultCellStyle.BackColor = input;
                    grid.RowsDefaultCellStyle.ForeColor = fore;
                    grid.BackColor = input;
                    grid.BackgroundColor = back;
                    grid.ForeColor = fore;
                    grid.GridColor = back;
                    ApplyContextMenu(grid, visitedMenus);
                    return;

                case FCTB_Mono fctb:
                    fctb.BackColor = fctb.ReadOnly ? disabled : input;
                    fctb.ForeColor = fore;
                    fctb.LineNumberColor = fore;
                    fctb.IndentBackColor = fctb.ReadOnly ? disabled : input;
                    fctb.CaretColor = fore;
                    ApplyContextMenu(fctb, visitedMenus);
                    return;

                case FCTB_MonoCJK fctbc:
                    fctbc.BackColor = fctbc.ReadOnly ? disabled : input;
                    fctbc.ForeColor = fore;
                    fctbc.LineNumberColor = fore;
                    fctbc.IndentBackColor = fctbc.ReadOnly ? disabled : input;
                    fctbc.CaretColor = fore;
                    ApplyContextMenu(fctbc, visitedMenus);
                    return;

                case ComboBox cb:
                    cb.BackColor = input;
                    cb.ForeColor = fore;
                    return;

                case NumericUpDown numup:
                    numup.BackColor = input;
                    numup.ForeColor = fore;
                    return;

                case TextBox textb:
                    textb.BackColor = textb.ReadOnly ? disabled : input;
                    textb.ForeColor = fore;
                    ApplyContextMenu(textb, visitedMenus);
                    return;

                case DateTimePicker dtp:
                    dtp.BackColor = dtp.Enabled ? input : disabled;
                    dtp.ForeColor = fore;
                    return;

                case BigCheckBox bgcx:
                    bgcx.BackColor = back;
                    bgcx.ForeColor = fore;
                    break;

                case MenuStrip strip:
                    ApplyMenuColors(strip, visitedMenus);
                    break;

                case Panel pan:
                    pan.BackColor = back;
                    pan.ForeColor = fore;
                    break;

                case SplitContainer cont:

                    cont.Panel1.BackColor = back;
                    cont.Panel1.ForeColor = fore;
                    cont.Panel2.BackColor = back;
                    cont.Panel2.ForeColor = fore;
                    cont.BackColor = back;
                    cont.ForeColor = fore;

                    break;

                case Form form:
                    form.BackColor = back;
                    form.ForeColor = fore;
                    ApplyContextMenu(form, visitedMenus);
                    break;

                case PictureBox pic:
                    break;

                default:
                    root.BackColor = back;
                    root.ForeColor = fore;
                    ApplyContextMenu(root, visitedMenus);
                    break;
            }

            foreach (Control child in root.Controls)
                SetExplicitColorsInternal(child, visitedMenus);
        }

        private static void ApplyContextMenu(Control c, HashSet<ToolStrip> visited)
        {
            if (c.ContextMenuStrip != null)
                ApplyMenuColors(c.ContextMenuStrip, visited);
        }

        private static void ApplyMenuColors(ToolStrip strip, HashSet<ToolStrip> visited)
        {
            if (!visited.Add(strip))
                return;

            strip.RenderMode = ToolStripRenderMode.System;
            strip.BackColor = Program.Settings.BGColor;
            strip.ForeColor = Program.Settings.TextColor;

            foreach (ToolStripItem item in strip.Items)
                ApplyMenuItemColors(item, visited);
        }

        private static void ApplyMenuItemColors(ToolStripItem item, HashSet<ToolStrip> visited)
        {
            item.BackColor = Program.Settings.BGColor;
            item.ForeColor = Program.Settings.TextColor;

            if (item is ToolStripDropDownItem dropDown)
            {
                ApplyMenuColors(dropDown.DropDown, visited);
                foreach (ToolStripItem sub in dropDown.DropDownItems)
                    ApplyMenuItemColors(sub, visited);
            }
        }

        public static float GetScaleFontSize(float baseSize = 8.25f)
        {
            return (baseSize * Program.Settings.GUIScale);
        }

        public static void AdjustFormScaleAndColors(Form f)
        {
            if (Program.Settings.ChangeGUIColors)
                GUIHacks.SetExplicitColors(f);

            if (Program.Settings.GUIScale == 1.0f)
                return;

            float fontSize = GetScaleFontSize();

            f.Font = new Font(f.Font.FontFamily, fontSize);
            GUIHacks.AdjustControlScale(f);
        }

        public static void MakeNotResizableMonoSafe(Form f)
        {
            f.FormBorderStyle = FormBorderStyle.FixedSingle;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
            f.ShowInTaskbar = false;
        }

    }
}
