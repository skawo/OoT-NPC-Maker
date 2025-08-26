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
        static bool isWsl;
        [DllImport("user32.dll")]
        private static extern int EnableScrollBar(IntPtr hWnd, int wSBflags, int wArrows);

        private const int SB_BOTH = 3;
        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;
        private const int ESB_DISABLE_BOTH = 0x3;
        private const int ESB_ENABLE_BOTH = 0x0;
        private bool lastVerticalState = true;
        private bool lastHorizontalState = true;

        public FCTB_Mono()
        {
            isWsl = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
            this.DoubleBuffered = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x200000;  // WS_VSCROLL
                cp.Style |= 0x100000;  // WS_HSCROLL
                return cp;
            }
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
                            if (isWsl)
                                RunBash($"cat {tempFileName} | clip.exe ");

                            RunBash($"cat {tempFileName} | xsel -i --clipboard ");
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

        private static string RunBash(string commandLine)
        {
            StringBuilder errorBuilder = new StringBuilder();
            StringBuilder outputBuilder = new StringBuilder();
            var arguments = $"-c \"{commandLine}\"";

            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "bash",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };

            process.Start();
            process.OutputDataReceived += (_, args) => { outputBuilder.AppendLine(args.Data); };
            process.BeginOutputReadLine();
            process.ErrorDataReceived += (_, args) => { errorBuilder.AppendLine(args.Data); };
            process.BeginErrorReadLine();
            if (!process.DoubleWaitForExit())
            {
                var timeoutError = $@"Process timed out. Command line: bash {arguments}.Output: {outputBuilder}Error: {errorBuilder}";
                throw new Exception(timeoutError);
            }
            if (process.ExitCode == 0)
            {
                return outputBuilder.ToString();
            }

            var error = $@"Could not execute process. Command line: bash {arguments}.Output: {outputBuilder} Error: {errorBuilder}";
            throw new Exception(error);
        }

        public FCTB_Mono(IContainer container)
        {
            container.Add(this);
        }
    }
}
