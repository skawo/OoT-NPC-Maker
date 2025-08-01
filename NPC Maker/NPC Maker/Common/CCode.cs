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


        public static string gtempFolderName = "temp";
        public static string gcompileFolderName = "compile";
        public static string gtempFolderPath = Path.Combine(Program.ExecPath, gtempFolderName);
        public static string gcompileFolderPath = Path.Combine(Program.ExecPath, gcompileFolderName);
        public static string gcodeFileNameBase = "EmbeddedOverlay";
        public static string gheaderFileName = "npc_maker_header.h";
        public static string geditCodeFilePath = $"{Path.Combine(gtempFolderPath, $"{gcodeFileNameBase}.c")}";
        public static string geditHeaderFilePath = $"{Path.Combine(gtempFolderPath, gheaderFileName)}";

        public static UInt32 gBaseAddr = 0x80800000;


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

        public static void CleanupCompileArtifacts()
        {
            Helpers.DeleteFileStartingWith(System.IO.Path.Combine(Program.ExecPath, "gcc", "bin"), "EmbeddedOverlayNPCCOMPILE");
            Helpers.DeleteFileStartingWith(System.IO.Path.Combine(Program.ExecPath, "gcc", "binmono"), "EmbeddedOverlayNPCCOMPILE");
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
            Out = Regex.Replace(Out, "[ ]{2,}", " ", RegexOptions.Compiled);
            List<string> Lines = Out.Split(new[] { '\n' }).ToList();

            byte[] ovl = File.ReadAllBytes(ovlPath);
            UInt32 sectionOffs = Program.BEConverter.ToUInt32(ovl, ovl.Length - 4);

            // Calculate section offsets once
            Dictionary<string, UInt32> sectionOffsets = new Dictionary<string, UInt32>
            {
                { ".text", 0 },
                { ".data", Program.BEConverter.ToUInt32(ovl, (int)(ovl.Length - sectionOffs)) },
                { ".rodata", Program.BEConverter.ToUInt32(ovl, (int)(ovl.Length - sectionOffs + 4)) },
                { ".bss", Program.BEConverter.ToUInt32(ovl, (int)(ovl.Length - sectionOffs + 8)) }
            };

            sectionOffsets[".data"] += sectionOffsets[".text"];
            sectionOffsets[".rodata"] += sectionOffsets[".data"];
            sectionOffsets[".bss"] += sectionOffsets[".rodata"];

            List<FunctionEntry> functions = new List<FunctionEntry>();
            foreach (string Line in Lines)
            {
                string[] words = Line.Split(' ');

                if (words.Length != 6 || words[2] != "F" || !words[5].StartsWith("NpcM_", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (UInt32.TryParse(words[0], System.Globalization.NumberStyles.HexNumber, null, out UInt32 address) && sectionOffsets.TryGetValue(words[3], out UInt32 sectionOffset))
                    functions.Add(new FunctionEntry(words[5], address - gBaseAddr + sectionOffset));
            }

            return functions.OrderBy(x => x.FuncName).ToList();
        }

        public static byte[] Compile(string Header, CCodeEntry CodeEntry, ref string CompileMsgs, string folder = "default")
        {
            try
            {
                string Code = CCode.ReplaceGameVersionInclude(CodeEntry.Code);
                string _Header = CCode.ReplaceGameVersionInclude(Header);

                string compileFileName = $"EmbeddedOverlay{folder}";
                string compileFolderPath = Path.Combine(Program.ExecPath, gcompileFolderName, folder);
                string compileFilePath = Path.Combine(compileFolderPath, $"{gcodeFileNameBase}{folder}.c");
                string compileHeaderPath = Path.Combine(compileFolderPath, gheaderFileName);

                if (Directory.Exists(compileFolderPath))
                    Directory.Delete(compileFolderPath, true);

                Directory.CreateDirectory(compileFolderPath);

                File.WriteAllText(compileFilePath, Code);
                File.WriteAllText(compileHeaderPath, _Header);

                byte[] outf = (Program.IsRunningUnderMono ? CompileUnderMono(folder, CodeEntry, ref CompileMsgs) : CompileUnderWindows(folder, CodeEntry, ref CompileMsgs));

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

        public static byte[] CompileUnderMono(string folder, CCodeEntry CodeEntry, ref string CompileMsgs)
        {

            string compileFolderPath = Path.Combine(Program.ExecPath, gcompileFolderName, folder);
            string compileFileName = $"{gcodeFileNameBase}{folder}";
            string oFileMono = Path.Combine(Program.ExecPath, "gcc", "binmono", $"{compileFileName}.o");
            string elfFileMono = Path.Combine(Program.ExecPath, "gcc", "binmono", $"{compileFileName}.elf");
            string ovlFileMono = Path.Combine(Program.ExecPath, compileFolderPath, $"{compileFileName}.ovl");

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
                $"{(String.IsNullOrEmpty(Program.Settings.ProjectPath) ? String.Empty : "-I " + Program.Settings.ProjectPath + " ")}" +
                Program.Settings.GCCFlags + " " + $"-B {Path.Combine(new string[] { "..", "mips64", "binmono" })} " +
                $"{Path.Combine("..", "..", gcompileFolderName, folder, $"{compileFileName}.c").AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += $"{gccInfo.FileName} {gccInfo.Arguments}{Environment.NewLine}";

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
                CompileMsgs += $"{ldInfo.FileName} {ldInfo.Arguments}{Environment.NewLine}";

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
                $"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{gBaseAddr:X} -o {ovlFileMono.AppendQuotation()} {elfFileMono.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += $"{nOVLInfo.FileName} {nOVLInfo.Arguments}{Environment.NewLine}";


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

            return File.ReadAllBytes(ovlFileMono);
            #endregion

        }

        public static byte[] CompileUnderWindows(string folder,  CCodeEntry CodeEntry, ref string CompileMsgs)
        {
            string compileFolderPath = Path.Combine(Program.ExecPath, gcompileFolderName, folder);
            string compileFileName = $"{gcodeFileNameBase}{folder}";
            string oFilePath = Path.Combine(Program.ExecPath, "gcc", "bin", $"{compileFileName}.o");
            string elfFilePath = Path.Combine(Program.ExecPath, "gcc", "bin", $"{compileFileName}.elf");
            string ovlFilePath = Path.Combine(compileFolderPath, $"{compileFileName}.ovl");

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
                $@"-I {Path.Combine(Program.ExecPath, "gcc", "mips64", "include").AppendQuotation()} " +
                $@"-I {Path.Combine(Program.ExecPath, "gcc", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString()).AppendQuotation()} " +
                $@"-I {Path.Combine(Program.ExecPath, "gcc", "mips64", "include", "z64hdr", "include").AppendQuotation()} " +
                $"{(String.IsNullOrEmpty(Program.Settings.ProjectPath) ? String.Empty : "-I " + Program.Settings.ProjectPath + " ")}" +
                Program.Settings.GCCFlags + " " +
                $@"{Path.Combine(compileFolderPath, $"{compileFileName}.c").AppendQuotation()}",
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
                $@"-L {Path.Combine(Program.ExecPath, "gcc", "mips64", "include", "npcmaker", Program.Settings.GameVersion.ToString()).AppendQuotation()} " +
                $@"-L {Path.Combine(Program.ExecPath, "gcc", "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString()).AppendQuotation()} " +
                $@"-L {Path.Combine(Program.ExecPath, "gcc", "mips64", "include", "z64hdr", "common").AppendQuotation()} " +
                $@"-T syms.ld -T z64hdr_actor.ld --emit-relocs " +
                $@"-o {elfFilePath.AppendQuotation()} {oFilePath.AppendQuotation()}"
            };

            if (Program.Settings.Verbose)
                CompileMsgs += $"{ldInfo.FileName} {ldInfo.Arguments}{Environment.NewLine}";

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
                $@"-c {(Program.Settings.Verbose ? "-vv" : "")} -A 0x{gBaseAddr:X} -o {ovlFilePath.AppendQuotation()} {elfFilePath.AppendQuotation()}",
            };

            if (Program.Settings.Verbose)
                CompileMsgs += $"{nOVLInfo.FileName} {nOVLInfo.Arguments}{Environment.NewLine}";

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
                if (Directory.Exists(gtempFolderPath))
                    Directory.Delete(gtempFolderPath, true);

                Directory.CreateDirectory(gtempFolderPath);

                string vscodeFolder = Path.Combine(gtempFolderPath, ".vscode");

                Directory.CreateDirectory(vscodeFolder);

                string cprops = Properties.Resources.c_cpp_properties;

                if (!String.IsNullOrEmpty(Program.Settings.ProjectPath))
                {
                    string uriPath = new Uri(Program.Settings.ProjectPath).AbsolutePath;
                    cprops = cprops.Replace("{PROJECTPATH}", uriPath);
                }
                else
                    cprops = cprops.Replace("{PROJECTPATH}", "");

                File.WriteAllText(Path.Combine(vscodeFolder, "c_cpp_properties.json"), cprops);
                File.WriteAllText(Path.Combine(vscodeFolder, "settings.json"), Properties.Resources.settings);

                if (!HeaderOnly)
                    File.WriteAllText(Path.Combine(gtempFolderPath, $"{CCode.gcodeFileNameBase}.c"), Code);

                File.WriteAllText(Path.Combine(gtempFolderPath, gheaderFileName), Header);
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
                            if (Program.IsRunningUnderMono)
                            {
                                startInfo.FileName = "code";
                                startInfo.Arguments = $"-n {CCode.gtempFolderPath.AppendQuotation()}";
                            }
                            else
                            {
                                startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\code";
                                startInfo.Arguments = $"-n {CCode.gtempFolderPath.AppendQuotation()}";
                            }
                            break;
                        }
                    case CCode.CodeEditorEnum.Notepad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\notepad.exe";
                            startInfo.Arguments = $"{CCode.geditCodeFilePath.AppendQuotation()}";
                            break;
                        }
                    case CCode.CodeEditorEnum.NotepadPlusPlus:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Notepad++\notepad++.exe";
                            startInfo.Arguments = $"{CCode.geditHeaderFilePath.AppendQuotation()} {CCode.geditCodeFilePath.AppendQuotation()} -multiInst";

                            break;
                        }
                    case CCode.CodeEditorEnum.Sublime:
                        {
                            if (Program.IsRunningUnderMono)
                            {
                                startInfo.FileName = "subl";
                                startInfo.Arguments = $"{CCode.geditCodeFilePath.AppendQuotation()}";
                            }
                            else
                            {
                                startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Sublime Text\subl.exe";
                                startInfo.Arguments = $"-n {CCode.geditCodeFilePath.AppendQuotation()}";
                            }
                            break;
                        }
                    case CCode.CodeEditorEnum.WordPad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\write.exe";
                            startInfo.Arguments = $"-n {CCode.geditCodeFilePath.AppendQuotation()}";
                            break;
                        }
                    case CCode.CodeEditorEnum.Kate:
                        {
                            startInfo.FileName = "kate";
                            startInfo.Arguments = $"{CCode.geditHeaderFilePath.AppendQuotation()} {CCode.geditCodeFilePath.AppendQuotation()}";

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
