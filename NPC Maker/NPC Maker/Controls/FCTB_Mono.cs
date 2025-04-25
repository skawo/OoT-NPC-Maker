using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC_Maker
{
    public class FCTB_Mono : FastColoredTextBoxNS.FastColoredTextBox
    {
        static bool isWsl;

        public FCTB_Mono()
        {
            isWsl = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
            this.DoubleBuffered = true;
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
