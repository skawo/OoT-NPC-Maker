using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NPC_Maker
{
    public static class CCode
    {
        public enum eCodeEditors
        {
            VSCode,
            Notepad,
            NotepadPlusPlus,
            Sublime,
            WordPad,
            Other
        };

        public static string[] CodeEditors = new string[]
        {
            eCodeEditors.VSCode.ToString(),
            eCodeEditors.Notepad.ToString(),
            eCodeEditors.NotepadPlusPlus.ToString(),
            eCodeEditors.Sublime.ToString(),
            eCodeEditors.WordPad.ToString(),
            eCodeEditors.Other.ToString()
        };

        public static string tempFolder = Path.Combine(Program.ExecPath, "temp");
        public static string codeFileName = "EmbeddedOverlay.c";
        public static string EmbeddedCodeFile = $"{Path.Combine(tempFolder, codeFileName)}";
        public static string oFile = Path.Combine(Program.ExecPath, "gcc", "bin", "EmbeddedOverlay.o");
        public static string elfFile = Path.Combine(Program.ExecPath, "gcc", "bin", "EmbeddedOverlay.elf");
        public static string ovlFile = Path.Combine(tempFolder, "EmbeddedOverlay.ovl");
        public static UInt32 BaseAddr = 0x80800000;

        public static void GetOutput(Process p, string Section, ref string CompileErrors)
        {
            CompileErrors += $"+==============+ {Section} +==============+";

            string Out = Environment.NewLine + p.StandardError.ReadToEnd().Replace("\n", Environment.NewLine) + Environment.NewLine + p.StandardOutput.ReadToEnd().Replace("\n", Environment.NewLine);

            if (!String.IsNullOrWhiteSpace(Out))
                CompileErrors += Out;

            if (p.ExitCode == 0)
                CompileErrors += Environment.NewLine + "OK!" + Environment.NewLine;
        }

        public static void Clean()
        {
            if (File.Exists(oFile))
                File.Delete(oFile);

            if (File.Exists(elfFile))
                File.Delete(elfFile);

            if (File.Exists(ovlFile))
                File.Delete(ovlFile);
        }

        
        public static List<KeyValuePair<string, UInt32>> GetNpcMakerFunctionsFromO(string elfPath)
        {
            ProcessStartInfo objDump = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", "bin"),
                FileName = Path.Combine(Program.ExecPath, "gcc", "bin", "mips64-objdump.exe"),
                Arguments =
                $"-t {elfPath.AppendQuotation()}"
            };

            Process p = Process.Start(objDump);
            string Out = p.StandardOutput.ReadToEnd();
            Out = Out.Replace("\t", " ");
            Out = Regex.Replace(Out, "[ ]{2,}", " ");

            List<string> Lines = Out.Split(new[] { '\n' }).ToList();


            List<KeyValuePair<string, UInt32>> Functions = new List<KeyValuePair<string, UInt32>>();
            Dictionary<string, UInt32> SectionOffsets = new Dictionary<string, UInt32>();

            foreach (string Line in Lines)
            {
                List<string> Words = Line.Split(' ').ToList();

                if (Words.Count != 6)
                    continue;

                if (Words[2] != "d")
                    continue;

                if (Words[5] == ".text" || Words[5] == ".data" || Words[5] == ".bss")
                    SectionOffsets.Add(Words[5], Words[0].Int2HexLeading() - BaseAddr);
            }

            foreach (string Line in Lines)
            {
                List<string> Words = Line.Split(' ').ToList();

                if (Words.Count != 6)
                    continue;

                if (Words[2] != "F")
                    continue;

                if (!Words[5].StartsWith("NpcM_", StringComparison.OrdinalIgnoreCase))
                    continue;
                else
                    Functions.Add(new KeyValuePair<string, UInt32>(Words[5], (UInt32)(Words[0].Int2HexLeading() - BaseAddr + SectionOffsets[Words[3]])));
            }

            return Functions.OrderBy(x => x.Key).ToList();

        }

        public static byte[] Compile(bool OotVer, CCodeEntry CodeEntry, ref string CompileErrors)
        {
            Clean();

            #region GCC

            ProcessStartInfo gccInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", "bin"),
                FileName = Path.Combine(Program.ExecPath, "gcc", "bin", "mips64-gcc.exe"),
                Arguments =
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", OotVer ? "oot_mq_debug" : "oot_u10" }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", "include" }).AppendQuotation()} " +
                $"-G 0 -Os -fno-reorder-blocks -std=gnu99 -mtune=vr4300 -march=vr4300 -mabi=32 -c -mips3 -mno-explicit-relocs -mno-memcpy -mno-check-zero-division " +
                $"{Path.Combine(tempFolder, codeFileName).AppendQuotation()}",
            };

            Process p = Process.Start(gccInfo);
            p.WaitForExit();

            GetOutput(p, "GCC", ref CompileErrors);

            #endregion

            #region LD

            if (!File.Exists(oFile))
            {
                CompileErrors += "Compilation failed.";
                return null;
            }

            ProcessStartInfo ldInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", "bin"),
                FileName = Path.Combine(Program.ExecPath, "gcc", "bin", "mips64-ld.exe"),
                Arguments =
                $"-L {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", OotVer ? "oot_mq_debug" : "oot_u10" }).AppendQuotation()} " +
                $"-L {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", "common" }).AppendQuotation()} " +
                $"-T syms.ld -T z64hdr_actor.ld --emit-relocs " +
                $"-o {elfFile.AppendQuotation()} {oFile.AppendQuotation()}"
            };

            p = Process.Start(ldInfo);
            p.WaitForExit();
            GetOutput(p, "LINKER", ref CompileErrors);


            if (!File.Exists(elfFile))
            {
                CompileErrors += "Compilation failed.";
                return null;
            }

            #endregion

            #region NOVL

            ProcessStartInfo nOVLInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = Path.Combine(Program.ExecPath, "nOVL", "novl.exe"),
                Arguments =
                $"-c -A 0x{BaseAddr.ToString("X")} -o {ovlFile.AppendQuotation()} {elfFile.AppendQuotation()}",
            };

            p = Process.Start(nOVLInfo);
            p.WaitForExit();
            GetOutput(p, "NOVL", ref CompileErrors);

            if (!File.Exists(ovlFile))
                CompileErrors += "Compilation failed.";
            else
                CompileErrors += "Compilation successful!";

            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFile);

            return File.ReadAllBytes(ovlFile);
            #endregion

        }

        public static bool CreateCTempDirectory(eCodeEditors Editor, string Code)
        {
            try
            {
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);

                Directory.CreateDirectory(tempFolder);

                string vscodeFolder = Path.Combine(tempFolder, ".vscode");

                Directory.CreateDirectory(vscodeFolder);
                File.WriteAllText(Path.Combine(vscodeFolder, "c_cpp_properties.json"), Properties.Resources.c_cpp_properties);
                File.WriteAllText(Path.Combine(vscodeFolder, "settings.json"), Properties.Resources.settings);

                File.WriteAllText(Path.Combine(tempFolder, codeFileName), Code);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating temporary directory: " + ex.Message);
                return false;
            }
        }

        public static Process OpenCodeEditor(eCodeEditors SelectedCodeEditor, string Path, string Args)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = CCode.EmbeddedCodeFile.AppendQuotation()
                };

                switch (SelectedCodeEditor)
                {
                    case CCode.eCodeEditors.VSCode:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\code";
                            startInfo.Arguments = $"-a {CCode.tempFolder.AppendQuotation()}";
                            break;
                        }
                    case CCode.eCodeEditors.Notepad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\notepad.exe";
                            break;
                        }
                    case CCode.eCodeEditors.NotepadPlusPlus:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Notepad++\notepad++.exe";
                            break;
                        }
                    case CCode.eCodeEditors.Sublime:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Sublime Text\subl.exe";
                            break;
                        }
                    case CCode.eCodeEditors.WordPad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\write.exe";
                            break;
                        }
                    default:
                        {
                            startInfo.FileName = Path;
                            startInfo.Arguments = Args;
                            break;
                        }
                }

                return Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running editor: " + ex.Message);
                return null;
            }
        }

    }
}
