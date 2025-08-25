using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace NPC_Maker
{
    public static class CCode
    {
        public static string gtempFolderName = "temp";
        public static string gcompileFolderName = "compile";
        public static string gtempFolderPath = Path.Combine(Program.ExecPath, gtempFolderName);
        public static string gcompileFolderPath = Path.Combine(Program.ExecPath, gcompileFolderName);
        public static string gcodeFileNameBase = "EmbeddedOverlay";
        public static string gheaderFileName = "npc_maker_header.h";
        public static string geditCodeFilePath = $"{Path.Combine(gtempFolderPath, $"{gcodeFileNameBase}.c")}";
        public static string geditHeaderFilePath = $"{Path.Combine(gtempFolderPath, gheaderFileName)}";
        public static UInt32 gBaseAddr = 0x80800000;

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

        private class CompilationConfig
        {
            public bool IsMonoEnvironment { get; set; }
            public string Folder { get; set; }
            public string BinDirectory { get; set; }
            public string GccExecutable { get; set; }
            public string LdExecutable { get; set; }
            public string NovlExecutable { get; set; }
            public string NovlWorkingDirectory { get; set; }
        }

        private class CompilationPaths
        {
            public string CompileFileName { get; }
            public string WorkingDirectory { get; }
            public string ObjectFile { get; }
            public string ElfFile { get; }
            public string OvlFile { get; }
            public string SourceFile { get; }

            public CompilationPaths(CompilationConfig config)
            {
                CompileFileName = $"{gcodeFileNameBase}{config.Folder}";
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory);
                ObjectFile = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, $"{CompileFileName}.o");
                ElfFile = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, $"{CompileFileName}.elf");

                var compileFolderPath = Path.Combine(Program.ExecPath, gcompileFolderName, config.Folder);
                OvlFile = config.IsMonoEnvironment
                    ? Path.Combine(compileFolderPath, $"{CompileFileName}.ovl")
                    : Path.Combine(compileFolderPath, $"{CompileFileName}.ovl");

                SourceFile = config.IsMonoEnvironment
                    ? Path.Combine("..", "..", gcompileFolderName, config.Folder, $"{CompileFileName}.c")
                    : Path.Combine(compileFolderPath, $"{CompileFileName}.c");
            }
        }

        private static void GetProcessOutput(Process process, string section, ref string compileErrors, bool isMonoEnvironment)
        {
            compileErrors += $"+==============+ {section} +==============+{Environment.NewLine}";

            string outputResult = "";
            string errorResult = "";
            bool processCompleted = true;

            try
            {
                if (false)
                {
                    // Old Mono code
                    //process.DoubleWaitForExit();
                    //outputResult = process.StandardOutput.ReadToEnd();
                    //errorResult = process.StandardError.ReadToEnd();
                }
                else
                {
                    var outputTask = TaskEx.Run(() => process.StandardOutput.ReadToEnd());
                    var errorTask = TaskEx.Run(() => process.StandardError.ReadToEnd());

                    processCompleted = process.WaitForExit((int)Program.Settings.CompileTimeout);

                    if (!processCompleted)
                    {
                        process.Kill();
                        compileErrors += $"{Environment.NewLine}Process timed out and was terminated.{Environment.NewLine}";
                    }

                    // Wait for output reading to complete
                    try
                    {
                        outputResult = outputTask.Result;
                        errorResult = errorTask.Result;
                    }
                    catch (AggregateException ex)
                    {
                        compileErrors += $"{Environment.NewLine}Error reading process output: {ex.InnerException?.Message}{Environment.NewLine}";
                    }
                }

                string combinedOutput = FormatProcessOutput(outputResult, errorResult);

                if (!string.IsNullOrWhiteSpace(combinedOutput))
                    compileErrors += combinedOutput;

                if (processCompleted && process.ExitCode == 0)
                    compileErrors += $"{Environment.NewLine}OK!{Environment.NewLine}";
                else if (processCompleted)
                    compileErrors += $"{Environment.NewLine}Process exited with code: {process.ExitCode}{Environment.NewLine}";
            }
            catch (Exception ex)
            {
                compileErrors += $"{Environment.NewLine}Exception while reading process output: {ex.Message}{Environment.NewLine}";
            }
        }

        private static string FormatProcessOutput(string standardOutput, string standardError)
        {
            var output = new StringBuilder();

            output.AppendLine();

            if (!string.IsNullOrEmpty(standardOutput))
                output.AppendLine(standardOutput.Replace("\n", Environment.NewLine));

            if (!string.IsNullOrEmpty(standardError))
                output.AppendLine(standardError.Replace("\n", Environment.NewLine));

            string result = output.ToString();

            // Remove ANSI escape sequences
            return Regex.Replace(result, @"\x1B\[[^@-~]*[@-~]", "");
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

        public static Dictionary<string, string> GetDefinesFromH(string hPath)
        {
            string headerContent = File.ReadAllText(hPath);

            string pattern = @"^\s*#define\s+(\w+)(?:\s+(.+?))?(?:\s*//.*)?$";
            Dictionary<string, string> outD = new Dictionary<string, string>();


            foreach (Match match in Regex.Matches(headerContent, pattern, RegexOptions.Multiline))
            {
                string name = match.Groups[1].Value;
                string value = match.Groups[2].Value.Trim();

                if (!outD.ContainsKey(name))
                    outD.Add(name, value);
            }

            return outD;
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

        public static byte[] CompileUnderMono(string folder, CCodeEntry codeEntry, ref string compileMsgs)
        {
            var config = new CompilationConfig
            {
                IsMonoEnvironment = true,
                Folder = folder,
                BinDirectory = "binmono",
                GccExecutable = "mips64-elf-gcc",
                LdExecutable = "mips64-elf-ld",
                NovlExecutable = "nOVL",
                NovlWorkingDirectory = "nOVL"
            };

            return CompileInternal(config, codeEntry, ref compileMsgs);
        }

        public static byte[] CompileUnderWindows(string folder, CCodeEntry codeEntry, ref string compileMsgs)
        {
            var config = new CompilationConfig
            {
                IsMonoEnvironment = false,
                Folder = folder,
                BinDirectory = "bin",
                GccExecutable = "mips64-gcc.exe",
                LdExecutable = "mips64-ld.exe",
                NovlExecutable = "novl.exe",
                NovlWorkingDirectory = Path.Combine("nOVL")
            };

            return CompileInternal(config, codeEntry, ref compileMsgs);
        }

        private static byte[] CompileInternal(CompilationConfig config, CCodeEntry codeEntry, ref string compileMsgs)
        {
            var paths = new CompilationPaths(config);
            string compilationFailedString = "Compilation failed.";

            Clean(new[] { paths.ObjectFile, paths.ElfFile, paths.OvlFile });

            if (!RunGccCompilation(config, paths, ref compileMsgs))
                return HandleCompilationFailure(compilationFailedString, ref compileMsgs);

            if (!RunLinkerPhase(config, paths, ref compileMsgs))
                return HandleCompilationFailure(compilationFailedString, ref compileMsgs);

            if (!RunNovlPhase(config, paths, ref compileMsgs))
                return HandleCompilationFailure(compilationFailedString, ref compileMsgs, true);

            compileMsgs += "Compilation successful!";
            codeEntry.Functions = GetNpcMakerFunctionsFromO(paths.ElfFile, paths.OvlFile, config.IsMonoEnvironment);

            CleanupIntermediateFiles(paths.ObjectFile, paths.ElfFile);
            return File.ReadAllBytes(paths.OvlFile);
        }

        private static bool RunGccCompilation(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            var gccInfo = CreateGccProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{gccInfo.FileName} {gccInfo.Arguments}{Environment.NewLine}";

            using (var process = Process.Start(gccInfo))
            {
                var sectionName = config.IsMonoEnvironment ? "Mono GCC" : "GCC";
                GetProcessOutput(process, sectionName, ref compileMsgs, config.IsMonoEnvironment);
            }

            return File.Exists(paths.ObjectFile);
        }

        private static bool RunLinkerPhase(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            var ldInfo = CreateLinkerProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{ldInfo.FileName} {ldInfo.Arguments}{Environment.NewLine}";

            using (var process = Process.Start(ldInfo))
            {
                var sectionName = config.IsMonoEnvironment ? "Mono LINKER" : "LINKER";
                GetProcessOutput(process, sectionName, ref compileMsgs, config.IsMonoEnvironment);
            }

            return File.Exists(paths.ElfFile);
        }

        private static bool RunNovlPhase(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            var novlInfo = CreateNovlProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{novlInfo.FileName} {novlInfo.Arguments}{Environment.NewLine}";

            using (var process = Process.Start(novlInfo))
            {
                var sectionName = config.IsMonoEnvironment ? "Mono NOVL" : "NOVL";
                GetProcessOutput(process, sectionName, ref compileMsgs, config.IsMonoEnvironment);
            }

            return File.Exists(paths.OvlFile);
        }


        private static ProcessStartInfo CreateGccProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            var includeFlags = BuildIncludeFlags(config);
            var projectFlag = string.IsNullOrEmpty(Program.Settings.ProjectPath)
                ? string.Empty
                : $"-I {Program.Settings.ProjectPath} ";

            var arguments = config.IsMonoEnvironment
                ? $"{includeFlags} {projectFlag}{Program.Settings.GCCFlags} -B {Path.Combine("..", "mips64", "binmono")} {paths.SourceFile.AppendQuotation()}"
                : $"{includeFlags} {projectFlag}{Program.Settings.GCCFlags} {paths.SourceFile.AppendQuotation()}";

            return new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = paths.WorkingDirectory,
                FileName = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, config.GccExecutable),
                Arguments = arguments
            };
        }

        private static ProcessStartInfo CreateLinkerProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            var libraryFlags = BuildLibraryFlags(config);
            var arguments = $"{libraryFlags} -T syms.ld -T z64hdr_actor.ld --emit-relocs -o {paths.ElfFile.AppendQuotation()} {paths.ObjectFile.AppendQuotation()}";

            return new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = paths.WorkingDirectory,
                FileName = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, config.LdExecutable),
                Arguments = arguments
            };
        }

        private static ProcessStartInfo CreateNovlProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            var workingDir = config.IsMonoEnvironment
                ? Path.Combine(Program.ExecPath, "nOVL")
                : paths.WorkingDirectory;

            var fileName = config.IsMonoEnvironment
                ? Path.Combine(Program.ExecPath, "nOVL", "nOVL")
                : Path.Combine(Program.ExecPath, "nOVL", "novl.exe");

            var verboseFlag = Program.Settings.Verbose ? "-vv" : "";
            var arguments = $"-c {verboseFlag} -A 0x{gBaseAddr:X} -o {paths.OvlFile.AppendQuotation()} {paths.ElfFile.AppendQuotation()}";

            return new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDir,
                FileName = fileName,
                Arguments = arguments
            };
        }

        private static string BuildIncludeFlags(CompilationConfig config)
        {
            var basePath = config.IsMonoEnvironment ? ".." : Path.Combine(Program.ExecPath, "gcc");
            var includes = new[]
            {
                Path.Combine(basePath, "mips64", "include"),
                Path.Combine(basePath, "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString()),
                Path.Combine(basePath, "mips64", "include", "z64hdr", "include")
            };

            return string.Join(" ", includes.Select(path => $"-I {path.AppendQuotation()}"));
        }

        private static string BuildLibraryFlags(CompilationConfig config)
        {
            var basePath = config.IsMonoEnvironment ? ".." : Path.Combine(Program.ExecPath, "gcc");
            var libraries = new[]
            {
                Path.Combine(basePath, "mips64", "include", "npcmaker", Program.Settings.GameVersion.ToString()),
                Path.Combine(basePath, "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString()),
                Path.Combine(basePath, "mips64", "include", "z64hdr", "common")
            };

            return string.Join(" ", libraries.Select(path => $"-L {path.AppendQuotation()}"));
        }

        private static byte[] HandleCompilationFailure(string message, ref string compileMsgs, bool returnEmptyArray = false)
        {
            compileMsgs += message;
            ConsoleWriteCompileFail(compileMsgs);
            return returnEmptyArray ? new byte[0] : null;
        }

        private static void CleanupIntermediateFiles(params string[] files)
        {
            foreach (var file in files.Where(File.Exists))
            {
                File.Delete(file);
            }
        }

        public static string ReplaceGameVersionInclude(string Code)
        {
            Code = Code.Replace("#include <z64hdr/oot_u10/z64hdr.h>", $"#include <z64hdr/{Program.Settings.GameVersion}/z64hdr.h>");
            Code = Code.Replace("#include <z64hdr/oot_mq_debug/z64hdr.h>", $"#include <z64hdr/{Program.Settings.GameVersion}/z64hdr.h>");
            return Code;
        }

        public static List<FunctionEntry> GetNpcMakerFunctionsFromO(string elfPath, string ovlPath, bool mono)
        {
            // Execute objdump process
            var objDumpOutput = ExecuteObjDump(elfPath, mono);

            // Parse objdump output
            var normalizedOutput = Regex.Replace(objDumpOutput.Replace("\t", " "), @"\s{2,}", " ", RegexOptions.Compiled);
            var lines = normalizedOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Load overlay file and calculate section offsets
            var ovlData = File.ReadAllBytes(ovlPath);
            var sectionOffsets = CalculateSectionOffsets(ovlData);

            // Extract NPC Maker functions
            var functions = lines
                .Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(IsValidNpcMakerFunction)
                .Select(words => CreateFunctionEntry(words, sectionOffsets))
                .Where(entry => entry != null)
                .OrderBy(entry => entry.FuncName)
                .ToList();

            return functions;
        }

        private static string ExecuteObjDump(string elfPath, bool mono)
        {
            var binDirectory = mono ? "binmono" : "bin";
            var executable = mono ? "mips64-elf-objdump" : "mips64-objdump.exe";

            var processInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", binDirectory),
                FileName = Path.Combine(Program.ExecPath, "gcc", binDirectory, executable),
                Arguments = string.Format("-t {0}", elfPath.AppendQuotation())
            };

            var process = Process.Start(processInfo);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();

            return output;
        }

        private static Dictionary<string, uint> CalculateSectionOffsets(byte[] ovlData)
        {
            var sectionOffs = Program.BEConverter.ToUInt32(ovlData, ovlData.Length - 4);
            var baseOffset = ovlData.Length - (int)sectionOffs;

            var offsets = new Dictionary<string, uint>
            {
                [".text"] = 0,
                [".data"] = Program.BEConverter.ToUInt32(ovlData, baseOffset),
                [".rodata"] = Program.BEConverter.ToUInt32(ovlData, baseOffset + 4),
                [".bss"] = Program.BEConverter.ToUInt32(ovlData, baseOffset + 8)
            };

            offsets[".data"] += offsets[".text"];
            offsets[".rodata"] += offsets[".data"];
            offsets[".bss"] += offsets[".rodata"];

            return offsets;
        }

        private static bool IsValidNpcMakerFunction(string[] words)
        {
            return words.Length == 6 &&
                   words[2] == "F" &&
                   words[5].StartsWith("NpcM_", StringComparison.OrdinalIgnoreCase);
        }

        private static FunctionEntry CreateFunctionEntry(string[] words, Dictionary<string, uint> sectionOffsets)
        {
            if (!uint.TryParse(words[0], System.Globalization.NumberStyles.HexNumber, null, out uint address) ||
                !sectionOffsets.TryGetValue(words[3], out uint sectionOffset))
                return null;

            return new FunctionEntry(words[5], address - gBaseAddr + sectionOffset);
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
