using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

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
            Kate,
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

        public static string tempFolderName = "temp";
        public static string compileFolderName = "compile";
        public static string tempFolderPath = Path.Combine(Program.ExecPath, tempFolderName);
        public static string compileFolderPath = Path.Combine(Program.ExecPath, compileFolderName);
        public static string codeFileName = "EmbeddedOverlay.c";
        public static string headerFileName = "npc_maker_header.h";
        public static string editCodeFilePath = $"{Path.Combine(tempFolderPath, codeFileName)}";
        public static string editHeaderFilePath = $"{Path.Combine(tempFolderPath, headerFileName)}";
        public static string compileCodeFileName = "EmbeddedOverlay_comp.c";
        public static string compileFilePath = Path.Combine(compileFolderPath, compileCodeFileName);
        public static string compileHeaderPath = Path.Combine(compileFolderPath, headerFileName);
        public static string oFilePath = Path.Combine(Program.ExecPath, "gcc", "bin", "EmbeddedOverlay_comp.o");
        public static string elfFilePath = Path.Combine(Program.ExecPath, "gcc", "bin", "EmbeddedOverlay_comp.elf");
        public static string ovlFilePath = Path.Combine(compileFolderPath, "EmbeddedOverlay_comp.ovl");
        public static UInt32 BaseAddr = 0x80800000;

        public static void GetOutput(Process p, string Section, ref string CompileErrors)
        {
            CompileErrors += $"+==============+ {Section} +==============+ {Environment.NewLine}";

            string outputResult = "";
            string errorResult = "";

            Thread r = new Thread(() => { outputResult = p.StandardOutput.ReadToEnd(); });
            r.Start();
            Thread r2 = new Thread(() => { errorResult = p.StandardError.ReadToEnd(); });
            r2.Start();

            p.WaitForExit((int)Program.Settings.CompileTimeout);

            if (!p.HasExited)
                p.Kill();

            r.Join();
            r2.Join();

            string Out = $"{Environment.NewLine}{outputResult.Replace("\n", Environment.NewLine)}{Environment.NewLine}{errorResult.Replace("\n", Environment.NewLine)}";

            Out = Regex.Replace(Out, @"\x1B\[[^@-~]*[@-~]", "");

            if (!String.IsNullOrWhiteSpace(Out))
                CompileErrors += Out;

            if (p.ExitCode == 0)
                CompileErrors += $"{Environment.NewLine}OK!{Environment.NewLine}";
        }

        public static void GetOutputMono(Process p, string Section, ref string CompileErrors)
        {
            CompileErrors += $"+==============+ {Section} +==============+ {Environment.NewLine}";

            string Out = $"{Environment.NewLine}{p.StandardOutput.ReadToEnd().Replace("\n", Environment.NewLine)}{Environment.NewLine}{p.StandardError.ReadToEnd().Replace("\n", Environment.NewLine)}";
            
            Out = Regex.Replace(Out, @"\x1B\[[^@-~]*[@-~]", "");

            if (!String.IsNullOrWhiteSpace(Out))
                CompileErrors += Out;

            if (p.ExitCode == 0)
                CompileErrors += $"{Environment.NewLine}OK!{Environment.NewLine}";
        }

        public static void Clean(string[] Files)
        {
            try
            {
                foreach (string f in Files)
                {
                    if (File.Exists(f))
                        File.Delete(f);
                }
            }
            catch (Exception)
            { }
        }


        public static List<FunctionEntry> GetNpcMakerFunctionsFromO(string elfPath, string ovlPath, bool mono)
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


            List<FunctionEntry> Functions = new List<FunctionEntry>();
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
                    Functions.Add(new FunctionEntry(Words[5], (UInt32)(Words[0].HexLeading2UInt32() - BaseAddr + SectionOffsets[Words[3]])));
            }

            return Functions.OrderBy(x => x.FuncName).ToList();

        }

        public static byte[] Compile(bool OotVer, string Header, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            try
            {
                string Code = CCode.ReplaceGameVersionInclude(CodeEntry.Code);
                string _Header = CCode.ReplaceGameVersionInclude(Header);

                if (Directory.Exists(compileFolderPath))
                    Directory.Delete(compileFolderPath, true);

                Directory.CreateDirectory(compileFolderPath);

                File.WriteAllText(compileFilePath, Code);
                File.WriteAllText(compileHeaderPath, _Header);

                byte[] outf = (Program.IsRunningUnderMono ? Program.Settings.UseWine ? CompileUnderWine(OotVer, CodeEntry, ref CompileMsgs) : CompileUnderMono(OotVer, CodeEntry, ref CompileMsgs) : CompileUnderWindows(OotVer, CodeEntry, ref CompileMsgs));

                Directory.Delete(compileFolderPath, true);

                return outf;
            }
            catch (Exception ex)
            {
                CompileMsgs = "Compilation failed: " + ex.Message;
                return null;
            }
        }

        public static void ConsoleWriteCompileFail(string CompilationMsgs)
        {
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{CompilationMsgs}{Environment.NewLine}{Environment.NewLine}");
        }

        public static byte[] CompileUnderWine(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            string oFileWine = Path.Combine("..", "bin", "EmbeddedOverlay_comp.o");
            string elfFileWine = Path.Combine("..", "bin", "EmbeddedOverlay_comp.elf");

            Clean(new string[] { oFileWine, elfFileWine });

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
                $"{Path.Combine("..", "..", compileFolderName, compileCodeFileName).AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += $"{gccInfo.FileName} {gccInfo.Arguments}{Environment.NewLine}";

            Process p = Process.Start(gccInfo);
     
            GetOutput(p, "WINE GCC", ref CompileMsgs);

            #endregion

            #region LD

            if (!File.Exists(oFilePath))
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
                    $"-o {elfFileWine.AppendQuotation()} {oFileWine.AppendQuotation()}"
            };

            if (Program.Settings.Verbose)
                CompileMsgs += ldInfo.FileName + " " + ldInfo.Arguments + Environment.NewLine;

            p = Process.Start(ldInfo);
  
            GetOutput(p, "WINE LINKER", ref CompileMsgs);


            if (!File.Exists(elfFilePath))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return null;
            }

            #endregion

            #region NOVL

            elfFileWine = Path.Combine("..", "gcc", "bin", "EmbeddedOverlay_comp.elf");
            string ovlFileWine = Path.Combine("..", compileFolderName, "EmbeddedOverlay_comp.ovl");

            Clean(new string[] { elfFileWine, ovlFileWine });

            ProcessStartInfo nOVLInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "nOVL"),
                FileName = Path.Combine(Program.ExecPath, "nOVL", "nOVL.exe"),
                Arguments =
                $"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{BaseAddr:X} -o {ovlFileWine.AppendQuotation()} {elfFileWine.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += nOVLInfo.FileName + " " + nOVLInfo.Arguments + Environment.NewLine;


            p = Process.Start(nOVLInfo);
            GetOutput(p, "WINE NOVL", ref CompileMsgs);

            elfFileWine = Path.Combine("..", "bin", "EmbeddedOverlay_comp.elf");
            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFileWine, ovlFilePath, false);

            if (!File.Exists(ovlFilePath))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return new byte[0];
            }
            else
                CompileMsgs += "Compilation successful!";


            return File.ReadAllBytes(ovlFilePath);
            #endregion

        }

        public static byte[] CompileUnderMono(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            string oFileMono = Path.Combine(Program.ExecPath, "gcc", "binmono", "EmbeddedOverlay_comp.o");
            string elfFileMono = Path.Combine(Program.ExecPath, "gcc", "binmono", "EmbeddedOverlay_comp.elf");
            string ovlFileMono = Path.Combine(Program.ExecPath, compileFolderName, "EmbeddedOverlay_comp.ovl");

            Clean(new string[] { oFileMono, elfFileMono, ovlFileMono });

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
                $"{Path.Combine("..", "..", compileFolderName, compileCodeFileName).AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += gccInfo.FileName + " " + gccInfo.Arguments + Environment.NewLine;

            Process p = Process.Start(gccInfo);
            p.WaitForExit();

            GetOutputMono(p, "Mono GCC", ref CompileMsgs);

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
            GetOutputMono(p, "Mono LINKER", ref CompileMsgs);


            if (!File.Exists(elfFileMono))
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
                WorkingDirectory = Path.Combine(Program.ExecPath, "nOVL"),
                FileName = Path.Combine(Program.ExecPath, "nOVL", "nOVL"),
                Arguments =
                $"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{BaseAddr:X} -o {ovlFileMono.AppendQuotation()} {elfFileMono.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += nOVLInfo.FileName + " " + nOVLInfo.Arguments + Environment.NewLine;

      
            p = Process.Start(nOVLInfo);
            p.WaitForExit();
            GetOutputMono(p, "Mono NOVL", ref CompileMsgs);

            if (!File.Exists(ovlFileMono))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
                return new byte[0];
            }
            else
                CompileMsgs += "Compilation successful!";

            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFileMono, ovlFileMono, true);

            return File.ReadAllBytes(ovlFilePath);
            #endregion

        }

        public static byte[] CompileUnderWindows(bool OotVer, CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            #region GCC

            Clean(new string[] { oFilePath, elfFilePath, ovlFilePath });

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
                $"{Path.Combine(compileFolderPath, compileCodeFileName).AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += $"{gccInfo.FileName} {gccInfo.Arguments}{Environment.NewLine}";

            Process p = Process.Start(gccInfo);

            GetOutput(p, "GCC", ref CompileMsgs);

            #endregion

            #region LD

            if (!File.Exists(oFilePath))
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
                $"-o {elfFilePath.AppendQuotation()} {oFilePath.AppendQuotation()}"
            };

            if (Program.Settings.Verbose)
                CompileMsgs += ldInfo.FileName + " " + ldInfo.Arguments + Environment.NewLine;

            p = Process.Start(ldInfo);

            GetOutput(p, "LINKER", ref CompileMsgs);

            if (!File.Exists(elfFilePath))
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
                $"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{BaseAddr:X} -o {ovlFilePath.AppendQuotation()} {elfFilePath.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += nOVLInfo.FileName + " " + nOVLInfo.Arguments + Environment.NewLine;

            p = Process.Start(nOVLInfo);
            GetOutput(p, "NOVL", ref CompileMsgs);

            if (!File.Exists(ovlFilePath))
            {
                CompileMsgs += "Compilation failed.";
                ConsoleWriteCompileFail(CompileMsgs);
            }
            else
                CompileMsgs += "Compilation successful!";

            CodeEntry.Functions = GetNpcMakerFunctionsFromO(elfFilePath, ovlFilePath, false);

            return File.ReadAllBytes(ovlFilePath);
            #endregion

        }

        public static string ReplaceGameVersionInclude(string Code)
        {
            Code = Code.Replace("#include <z64hdr/oot_u10/z64hdr.h>", $"#include <z64hdr/{Program.Settings.GameVersion}/z64hdr.h>");
            Code = Code.Replace("#include <z64hdr/oot_mq_debug/z64hdr.h>", $"#include <z64hdr/{Program.Settings.GameVersion}/z64hdr.h>");
            return Code;
        }

        public static bool CreateCTempDirectory(string Code, string Header, bool ErrorMsg = true, bool HeaderOnly = false)
        {
            try
            {
                if (Directory.Exists(tempFolderPath))
                    Directory.Delete(tempFolderPath, true);

                Directory.CreateDirectory(tempFolderPath);

                string vscodeFolder = Path.Combine(tempFolderPath, ".vscode");

                Directory.CreateDirectory(vscodeFolder);
                File.WriteAllText(Path.Combine(vscodeFolder, "c_cpp_properties.json"), Properties.Resources.c_cpp_properties);
                File.WriteAllText(Path.Combine(vscodeFolder, "settings.json"), Properties.Resources.settings);

                if (!HeaderOnly)
                    File.WriteAllText(Path.Combine(tempFolderPath, codeFileName), Code);
                
                File.WriteAllText(Path.Combine(tempFolderPath, headerFileName), Header);
                return true;
            }
            catch (Exception ex)
            {
                if (ErrorMsg)
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
                    Arguments = Args
                };

                switch (SelectedCodeEditor)
                {
                    case CCode.CodeEditorEnum.VSCode:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\code";
                            startInfo.Arguments = $"-n {CCode.tempFolderPath.AppendQuotation()}";
                            break;
                        }
                    case CCode.CodeEditorEnum.Notepad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\notepad.exe";
                            startInfo.Arguments = $"-n {CCode.editCodeFilePath.AppendQuotation()}";
                            break;
                        }
                    case CCode.CodeEditorEnum.NotepadPlusPlus:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Notepad++\notepad++.exe";
                            startInfo.Arguments = $"{CCode.editHeaderFilePath.AppendQuotation()} {CCode.editCodeFilePath.AppendQuotation()} -multiInst";

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
                            startInfo.Arguments = $"-n {CCode.editCodeFilePath.AppendQuotation()}";
                            break;
                        }
                    case CCode.CodeEditorEnum.Kate:
                        {
                            startInfo.FileName = "kate";
                            startInfo.Arguments = $"{CCode.editHeaderFilePath.AppendQuotation()} {CCode.editCodeFilePath.AppendQuotation()}";

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
