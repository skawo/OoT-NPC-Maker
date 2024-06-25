using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;

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
        public static string codeFilenameForCompile = "EmbeddedOverlay_comp.c";
        public static string EmbeddedCodeFile = $"{Path.Combine(tempFolder, codeFileName)}";
        public static string compileFile = Path.Combine(tempFolder, codeFilenameForCompile);
        public static string oFile = Path.Combine(Program.ExecPath, "gcc", "bin", "EmbeddedOverlay_comp.o");
        public static string elfFile = Path.Combine(Program.ExecPath, "gcc", "bin", "EmbeddedOverlay_comp.elf");
        public static string ovlFile = Path.Combine(tempFolder, "EmbeddedOverlay_comp.ovl");
        public static UInt32 BaseAddr = 0x80800000;

        public static void GetOutput(Process p, string Section, ref string CompileErrors)
        {
            CompileErrors += $"+==============+ {Section} +==============+ {Environment.NewLine}";

            string Out = $"{Environment.NewLine}{p.StandardOutput.ReadToEnd().Replace("\n", Environment.NewLine)}{Environment.NewLine}{p.StandardError.ReadToEnd().Replace("\n", Environment.NewLine)}";

            Out = Regex.Replace(Out, @"\x1B\[[^@-~]*[@-~]", "");

            if (!String.IsNullOrWhiteSpace(Out))
                CompileErrors += Out;

            if (p.ExitCode == 0)
                CompileErrors += $"{Environment.NewLine}OK!{Environment.NewLine}";
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

                if (File.Exists(compileFile))
                    File.Delete(compileFile);
            }
            catch (Exception)
            { }
        }


        public static List<KeyValuePair<string, UInt32>> GetNpcMakerFunctionsFromO(string elfPath, string ovlPath, bool mono)
        {
            string Out;

            if (!mono)
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
                Out = p.StandardOutput.ReadToEnd();
            }
            else
            {
                ProcessStartInfo objDump = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", "binmono"),
                    FileName = Path.Combine(Program.ExecPath, "gcc", "binmono", "mips64-elf-objdump"),
                    Arguments =
                    $"-t {elfPath.AppendQuotation()}"
                };

                Process p = Process.Start(objDump);
                Out = p.StandardOutput.ReadToEnd();
            }

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
                Clean();
                string Code = CCode.ReplaceGameVersionInclude(CodeEntry.Code);

                File.WriteAllText(Path.Combine(tempFolder, codeFilenameForCompile), Code);

                byte[] outf = (Program.IsRunningUnderMono ? Program.Settings.UseWine ? CompileUnderWine(OotVer, CodeEntry, ref CompileMsgs) : CompileUnderMono(OotVer, CodeEntry, ref CompileMsgs) : CompileUnderWindows(OotVer, CodeEntry, ref CompileMsgs));
               
                Clean();
                return outf;
            }
            catch (Exception ex)
            {
                CompileMsgs = "Compilation failed: " + ex.Message;
                return new byte[0];
            }
        }

        public static void ConsoleWriteCompileFail(string CompilationMsgs)
        {
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{CompilationMsgs}{Environment.NewLine}{Environment.NewLine}");
        }

        public static byte[] CompileUnderWine(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            string oFileMono = Path.Combine("..", "bin", "EmbeddedOverlay_comp.o");
            string elfFileMono = Path.Combine("..", "bin", "EmbeddedOverlay_comp.elf");

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
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include" }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", "include" }).AppendQuotation()} " +
                Program.Settings.GCCFlags + " " +
                $"{Path.Combine("..", "..", "temp", codeFilenameForCompile).AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += gccInfo.FileName + " " + gccInfo.Arguments + Environment.NewLine;

            Process p = Process.Start(gccInfo);
            p.WaitForExit();

            GetOutput(p, "WINE GCC", ref CompileMsgs);

            #endregion

            #region LD

            if (!File.Exists(oFile))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
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
            GetOutput(p, "WINE LINKER", ref CompileMsgs);


            if (!File.Exists(elfFile))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return null;
            }

            #endregion

            #region NOVL

            elfFileMono = Path.Combine("..", "gcc", "bin", "EmbeddedOverlay_comp.elf");
            string ovlFileMono = Path.Combine("..", "temp", "EmbeddedOverlay_comp.ovl");

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
            GetOutput(p, "WINE NOVL", ref CompileMsgs);

            elfFileMono = Path.Combine("..", "bin", "EmbeddedOverlay_comp.elf");
            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFileMono, ovlFile, false);

            if (!File.Exists(ovlFile))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return new byte[0];
            }
            else
                CompileMsgs += "Compilation successful!";


            return File.ReadAllBytes(ovlFile);
            #endregion

        }

        public static byte[] CompileUnderMono(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            string oFileMono = Path.Combine(Program.ExecPath, "gcc", "binmono", "EmbeddedOverlay_comp.o");
            string elfFileMono = Path.Combine(Program.ExecPath, "gcc", "binmono", "EmbeddedOverlay_comp.elf");

            #region GCC

            ProcessStartInfo gccInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", "binmono"),
                FileName = Path.Combine(Program.ExecPath, "gcc", "binmono", "mips64-elf-gcc"),
                Arguments =
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include" }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { "..", "mips64", "include", "z64hdr", "include" }).AppendQuotation()} " +
                Program.Settings.GCCFlags + " " + $"-B {Path.Combine(new string[] { "..", "mips64", "binmono" })} " +
                $"{Path.Combine("..", "..", "temp", codeFilenameForCompile).AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += gccInfo.FileName + " " + gccInfo.Arguments + Environment.NewLine;

            Process p = Process.Start(gccInfo);
            p.WaitForExit();

            GetOutput(p, "Mono GCC", ref CompileMsgs);

            #endregion

            #region LD

            if (!File.Exists(oFileMono))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return null;
            }

            ProcessStartInfo ldInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", "binmono"),
                FileName = Path.Combine(Program.ExecPath, "gcc", "binmono", "mips64-elf-ld"),
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
            GetOutput(p, "Mono LINKER", ref CompileMsgs);


            if (!File.Exists(elfFileMono))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return null;
            }

            #endregion

            #region NOVL

            // elfFileMono = Path.Combine("..", "gcc", "bin", "EmbeddedOverlay_comp.elf");
            string ovlFileMono = Path.Combine(Program.ExecPath, "temp", "EmbeddedOverlay_comp.ovl");

            ProcessStartInfo nOVLInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "nOVL"),
                FileName = Path.Combine(Program.ExecPath, "nOVL", "nOVL"),
                Arguments =
                $"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{BaseAddr:X} -o {ovlFileMono.AppendQuotation()} {elfFileMono.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += nOVLInfo.FileName + " " + nOVLInfo.Arguments + Environment.NewLine;

      
            p = Process.Start(nOVLInfo);
            p.WaitForExit();
            GetOutput(p, "Mono NOVL", ref CompileMsgs);

            if (!File.Exists(ovlFileMono))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return new byte[0];
            }
            else
                CompileMsgs += "Compilation successful!";

            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFileMono, ovlFileMono, true);

            return File.ReadAllBytes(ovlFile);
            #endregion

        }

        public static byte[] CompileUnderWindows(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
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
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include" }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString() }).AppendQuotation()} " +
                $"-I {Path.Combine(new string[] { Program.ExecPath, "gcc", "mips64", "include", "z64hdr", "include" }).AppendQuotation()} " +
                Program.Settings.GCCFlags + " " +
                $"{Path.Combine(tempFolder, codeFilenameForCompile).AppendQuotation()}",
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
                ConsoleWriteCompileFail(CompileMsgs);
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
                ConsoleWriteCompileFail(CompileMsgs);
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
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
            }
            else
                CompileMsgs += "Compilation successful!";

            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFile, ovlFile, false);

            return File.ReadAllBytes(ovlFile);
            #endregion

        }

        public static string ReplaceGameVersionInclude(string Code)
        {
            Code = Code.Replace("#include <z64hdr/oot_u10/z64hdr.h>", $"#include <z64hdr/{Program.Settings.GameVersion}/z64hdr.h>");
            Code = Code.Replace("#include <z64hdr/oot_mq_debug/z64hdr.h>", $"#include <z64hdr/{Program.Settings.GameVersion}/z64hdr.h>");
            return Code;
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
