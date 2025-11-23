using FastColoredTextBoxNS;
using FastColoredTextBoxNS.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC_Maker
{
    public class FCTB_Mono : FastColoredTextBoxNS.FastColoredTextBox
    {
        [DllImport("user32.dll")]
        private static extern int EnableScrollBar(IntPtr hWnd, int wSBflags, int wArrows);

        private const int SB_BOTH = 3;
        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;
        private const int ESB_DISABLE_BOTH = 0x3;
        private const int ESB_ENABLE_BOTH = 0x0;
        private bool lastVerticalState = true;
        private bool lastHorizontalState = true;
        private bool setScrollbarsVisible = false;

        private bool wordSelectMode = false;
        private Place wordSelectModeStart;

        public FCTB_Mono(bool scrollbarsVisible = true)
        {
            this.DoubleBuffered = true;
            setScrollbarsVisible = scrollbarsVisible;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (setScrollbarsVisible)
                {
                    cp.Style |= 0x200000;  // WS_VSCROLL
                    cp.Style |= 0x100000;  // WS_HSCROLL
                }

                return cp;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                wordSelectMode = true;
                wordSelectModeStart = GetWordStart(PointToPlace(e.Location));
            }
            else
                wordSelectMode = false;

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (wordSelectMode && e.Button == MouseButtons.Left)
            {
                Place mousePos = PointToPlace(e.Location);
                Place wordStart = GetWordStart(mousePos);
                Place wordEnd = GetWordEnd(mousePos);

                if (mousePos < wordSelectModeStart)
                    Selection = new TextSelectionRange(this, wordStart, Selection.End);
                else
                    Selection = new TextSelectionRange(this, wordSelectModeStart, wordEnd);
            }
            else
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            wordSelectMode = false;
            base.OnMouseUp(e);
        }

        private Place GetWordStart(Place pos)
        {
            string line = this[pos.iLine].Text;
            int i = Math.Min(pos.iChar, line.Length - 1);

            while (i > 0 && char.IsLetterOrDigit(line[i - 1]))
                i--;

            return new Place(i, pos.iLine);
        }

        private Place GetWordEnd(Place pos)
        {
            string line = this[pos.iLine].Text;
            int i = pos.iChar;

            while (i < line.Length && char.IsLetterOrDigit(line[i]))
                i++;

            return new Place(i, pos.iLine);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (!Program.IsRunningUnderMono)
                UpdateScrollBarState();
        }

        private void UpdateScrollBarState()
        {
            if (!IsHandleCreated || IsDisposed) 
                return;

            try
            {
                // Check if vertical scrolling is needed
                bool needVerticalScroll = Lines.Count > 1 &&
                    TextRenderer.MeasureText(Text, Font).Height > ClientSize.Height;

                // Check if horizontal scrolling is needed
                bool needHorizontalScroll = false;
                if (Lines.Count > 0)
                {
                    foreach (string line in Lines)
                    {
                        if (TextRenderer.MeasureText(line, Font).Width > ClientSize.Width)
                        {
                            needHorizontalScroll = true;
                            break;
                        }
                    }
                }

                // Only update if state changed to avoid potential paint loops
                if (needVerticalScroll != lastVerticalState)
                {
                    EnableScrollBar(Handle, SB_VERT, needVerticalScroll ? ESB_ENABLE_BOTH : ESB_DISABLE_BOTH);
                    lastVerticalState = needVerticalScroll;
                }

                if (needHorizontalScroll != lastHorizontalState)
                {
                    EnableScrollBar(Handle, SB_HORZ, needHorizontalScroll ? ESB_ENABLE_BOTH : ESB_DISABLE_BOTH);
                    lastHorizontalState = needHorizontalScroll;
                }
            }
            catch
            {
                // Ignore exceptions during paint
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            if (Program.IsRunningUnderMono)
            {
                if (se.Type != ScrollEventType.ThumbTrack && se.Type != ScrollEventType.ThumbPosition && se.Type != ScrollEventType.EndScroll)
                    base.OnScroll(se, true);
            }
            else
                base.OnScroll(se, true);
        }
    
        public override void Copy()
        {
            if (Program.IsRunningUnderMono)
            {
                try
                {
                    if (Selection.End != Selection.Start)
                    {
                        var tempFileName = Path.GetTempFileName();
                        File.WriteAllText(tempFileName, SelectedText);
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
                }
                catch (Exception)
                {

                }
            }
            else
                base.Copy();
        }

        public FCTB_Mono(IContainer container)
        {
            container.Add(this);
        }
    }
}
