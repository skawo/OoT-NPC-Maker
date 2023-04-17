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
        public enum CodeEditorEnum
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
            CodeEditorEnum.VSCode.ToString(),
            CodeEditorEnum.Notepad.ToString(),
            CodeEditorEnum.NotepadPlusPlus.ToString(),
            CodeEditorEnum.Sublime.ToString(),
            CodeEditorEnum.WordPad.ToString(),
            CodeEditorEnum.Other.ToString()
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
            try
            {
                if (File.Exists(oFile))
                    File.Delete(oFile);

                if (File.Exists(elfFile))
                    File.Delete(elfFile);

                if (File.Exists(ovlFile))
                    File.Delete(ovlFile);
            }
            catch (Exception)
            { }
        }


        public static List<KeyValuePair<string, UInt32>> GetNpcMakerFunctionsFromO(string elfPath, string ovlPath)
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

            byte[] ovl = File.ReadAllBytes(ovlPath);
            UInt32 SectionOffs = Program.BEConverter.ToUInt32(ovl, ovl.Length - 4);

            SectionOffsets.Add(".text", 0);
            SectionOffsets.Add(".data", SectionOffsets[".text"] + Program.BEConverter.ToUInt32(ovl, (int)(ovl.Length - SectionOffs)));
            SectionOffsets.Add(".rodata", SectionOffsets[".data"] + Program.BEConverter.ToUInt32(ovl, (int)(ovl.Length - SectionOffs + 4)));
            SectionOffsets.Add(".bss", SectionOffsets[".rodata"] + Program.BEConverter.ToUInt32(ovl, (int)(ovl.Length - SectionOffs + 8)));

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
                    Functions.Add(new KeyValuePair<string, UInt32>(Words[5], (UInt32)(Words[0].HexLeading2UInt32() - BaseAddr + SectionOffsets[Words[3]])));
            }

            return Functions.OrderBy(x => x.Key).ToList();

        }

        public static byte[] Compile(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            try
            {
                return Program.IsRunningUnderMono ? CompileUnderMono(OotVer, CodeEntry, ref CompileMsgs) : CompileUnderWindows(OotVer, CodeEntry, ref CompileMsgs);
            }
            catch (Exception ex)
            {
                CompileMsgs = "Compilation failed: " + ex.Message;
                return new byte[0];
            }
        }

        public static byte[] CompileUnderMono(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            Clean();

            string oFileMono = Path.Combine("..", "bin", "EmbeddedOverlay.o");
            string elfFileMono = Path.Combine("..", "bin", "EmbeddedOverlay.elf");

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
                $"-I {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", "include" }).AppendQuotation()} " +
                Program.Settings.GCCFlags + " " +
                $"{Path.Combine("..", "..", "temp", codeFileName).AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += gccInfo.FileName + " " + gccInfo.Arguments + Environment.NewLine;

            Process p = Process.Start(gccInfo);
            p.WaitForExit();

            GetOutput(p, "GCC", ref CompileMsgs);

            #endregion

            #region LD

            if (!File.Exists(oFile))
            {
                CompileMsgs += "Compilation failed.";
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
                    $"-L {Path.Combine(new string[] { "..", "mips64", "include", "npcmaker", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                    $"-L {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                    $"-L {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", "common" }).AppendQuotation()} " +
                    $"-T syms.ld -T z64hdr_actor.ld --emit-relocs " +
                    $"-o {elfFileMono.AppendQuotation()} {oFileMono.AppendQuotation()}"
            };

            if (Program.Settings.Verbose)
                CompileMsgs += ldInfo.FileName + " " + ldInfo.Arguments + Environment.NewLine;

            p = Process.Start(ldInfo);
            p.WaitForExit();
            p.WaitForExit();
            GetOutput(p, "LINKER", ref CompileMsgs);


            if (!File.Exists(elfFile))
            {
                CompileMsgs += "Compilation failed.";
                return null;
            }

            #endregion

            #region NOVL

            elfFileMono = Path.Combine("..", "gcc", "bin", "EmbeddedOverlay.elf");
            string ovlFileMono = Path.Combine("..", "temp", "EmbeddedOverlay.ovl");

            ProcessStartInfo nOVLInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "nOVL"),
                FileName = Path.Combine(Program.ExecPath, "nOVL", "nOVL.exe"),
                Arguments =
                $"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{BaseAddr:X} -o {ovlFileMono.AppendQuotation()} {elfFileMono.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += nOVLInfo.FileName + " " + nOVLInfo.Arguments + Environment.NewLine;

      
            p = Process.Start(nOVLInfo);
            p.WaitForExit();
            GetOutput(p, "NOVL", ref CompileMsgs);

            elfFileMono = Path.Combine("..", "bin", "EmbeddedOverlay.elf");
            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFileMono, ovlFile);

            if (!File.Exists(ovlFile))
            {
                CompileMsgs += "Compilation failed.";
                return new byte[0];
            }
            else
                CompileMsgs += "Compilation successful!";


            return File.ReadAllBytes(ovlFile);
            #endregion

        }

        public static byte[] CompileUnderWindows(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
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
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", "include" }).AppendQuotation()} " +
                Program.Settings.GCCFlags + " " +
                $"{Path.Combine(tempFolder, codeFileName).AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += gccInfo.FileName + " " + gccInfo.Arguments + Environment.NewLine;

            Process p = Process.Start(gccInfo);

            p.WaitForExit((int)Program.Settings.CompileTimeout);

            if (!p.HasExited)
                p.Kill();

            GetOutput(p, "GCC", ref CompileMsgs);

            #endregion

            #region LD

            if (!File.Exists(oFile))
            {
                CompileMsgs += "Compilation failed.";
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
                $"-L {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "npcmaker", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                $"-L {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                $"-L {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", "common" }).AppendQuotation()} " +
                $"-T syms.ld -T z64hdr_actor.ld --emit-relocs " +
                $"-o {elfFile.AppendQuotation()} {oFile.AppendQuotation()}"
            };

            if (Program.Settings.Verbose)
                CompileMsgs += ldInfo.FileName + " " + ldInfo.Arguments + Environment.NewLine;

            p = Process.Start(ldInfo);
            p.WaitForExit((int)Program.Settings.CompileTimeout);

            if (!p.HasExited)
                p.Kill();

            GetOutput(p, "LINKER", ref CompileMsgs);

            if (!File.Exists(elfFile))
            {
                CompileMsgs += "Compilation failed.";
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
                $"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{BaseAddr:X} -o {ovlFile.AppendQuotation()} {elfFile.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += nOVLInfo.FileName + " " + nOVLInfo.Arguments + Environment.NewLine;

            p = Process.Start(nOVLInfo);
            p.WaitForExit((int)Program.Settings.CompileTimeout);

            if (!p.HasExited)
                p.Kill();

            GetOutput(p, "NOVL", ref CompileMsgs);

            if (!File.Exists(ovlFile))
                CompileMsgs += "Compilation failed.";
            else
                CompileMsgs += "Compilation successful!";

            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFile, ovlFile);

            return File.ReadAllBytes(ovlFile);
            #endregion

        }

        public static bool CreateCTempDirectory(string Code)
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

        public static Process OpenCodeEditor(CodeEditorEnum SelectedCodeEditor, string Path, string Args)
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
                    case CCode.CodeEditorEnum.VSCode:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\code";
                            startInfo.Arguments = $"-a {CCode.tempFolder.AppendQuotation()}";
                            break;
                        }
                    case CCode.CodeEditorEnum.Notepad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\notepad.exe";
                            break;
                        }
                    case CCode.CodeEditorEnum.NotepadPlusPlus:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Notepad++\notepad++.exe";
                            break;
                        }
                    case CCode.CodeEditorEnum.Sublime:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Sublime Text\subl.exe";
                            break;
                        }
                    case CCode.CodeEditorEnum.WordPad:
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
