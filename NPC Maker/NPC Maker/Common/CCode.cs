using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
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
        public static string geditCodeFilePath = Path.Combine(gtempFolderPath, $"{gcodeFileNameBase}.c");
        public static string geditHeaderFilePath = Path.Combine(gtempFolderPath, gheaderFileName);
        public static uint gBaseAddr = 0x80800000;

        public enum CodeEditorEnum
        {
            VSCode,
            Notepad,
            NotepadPlusPlus,
            Sublime,
            WordPad,
            Kate,
            Other
        }

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
            public string OutPath { get; set; }
            public string[] LinkerFiles { get; set; }
            public string CompileFlags { get; set; }
        }

        private class CompilationPaths
        {
            public string CompileFileName { get; }
            public string WorkingDirectory { get; }
            public string ObjectFile { get; }
            public string ElfFile { get; }
            public string OvlFile { get; }
            public string dFile { get; }
            public string SourceFile { get; }

            public CompilationPaths(CompilationConfig config, string sourceFile)
            {
                CompileFileName = Path.GetFileNameWithoutExtension(sourceFile);

                SourceFile = sourceFile;
                OvlFile = config.OutPath;
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory);
                ObjectFile = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, $"{CompileFileName}.o");
                dFile = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, $"{CompileFileName}.d");
                ElfFile = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, $"{CompileFileName}.elf");
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
                var outputTask = TaskEx.Run(() => process.StandardOutput.ReadToEnd());
                var errorTask = TaskEx.Run(() => process.StandardError.ReadToEnd());

                processCompleted = process.WaitForExit((int)Program.Settings.CompileTimeout);

                if (!processCompleted)
                {
                    process.Kill();
                    compileErrors += $"{Environment.NewLine}Process timed out and was terminated.{Environment.NewLine}";
                }

                try
                {
                    outputResult = outputTask.Result;
                    errorResult = errorTask.Result;
                }
                catch (AggregateException ex)
                {
                    compileErrors += $"{Environment.NewLine}Error reading process output: {ex.InnerException?.Message}{Environment.NewLine}";
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
            {
            }
        }

        public static void CleanupStandardCompilationArtifacts()
        {
            Helpers.DeleteFileStartingWith(Path.Combine(Program.ExecPath, "gcc", "bin"), "EmbeddedOverlayNPCCOMPILE");
            Helpers.DeleteFileStartingWith(Path.Combine(Program.ExecPath, "gcc", "binmono"), "EmbeddedOverlayNPCCOMPILE");
            Helpers.DeleteFileStartingWith(Path.Combine(Program.ExecPath, "gcc", "bin"), "EmbeddedOverlaytemp");
            Helpers.DeleteFileStartingWith(Path.Combine(Program.ExecPath, "gcc", "binmono"), "EmbeddedOverlaytemp");
        }

        public static void Compile(string cFilePath, string linkerFiles, string outFilePath, string compileFlags, ref string CompileMsgs)
        {
            string folder = Helpers.GenerateTemporaryFolderName();

            if (Program.IsRunningUnderMono)
                CompileUnderMono(folder, null, ref CompileMsgs, cFilePath, outFilePath, linkerFiles, compileFlags);
            else
                CompileUnderWindows(folder, null, ref CompileMsgs, cFilePath, outFilePath, linkerFiles, compileFlags);
        }

        public static byte[] Compile(string Header, string linkerFiles, CCodeEntry CodeEntry, ref string CompileMsgs, string folder = "")
        {
            try
            {
                if (String.IsNullOrWhiteSpace(CodeEntry.Code))
                    return new byte[0];

                if (String.IsNullOrWhiteSpace(folder))
                    folder = Helpers.GenerateTemporaryFolderName();

                string Code = ReplaceGameVersionInclude(CodeEntry.Code);
                string _Header = ReplaceGameVersionInclude(Header);

                string compileFolderPath = Path.Combine(Program.ExecPath, gcompileFolderName, folder);
                string compileFilePath = Path.Combine(compileFolderPath, $"{gcodeFileNameBase}{folder}.c");
                string compileHeaderPath = Path.Combine(compileFolderPath, gheaderFileName);
                string compileFileName = Path.GetFileName(compileFilePath);
                string outFilePath = Path.Combine(compileFolderPath, $"{compileFileName}.ovl");


                if (Directory.Exists(compileFolderPath))
                    Directory.Delete(compileFolderPath, true);

                Directory.CreateDirectory(compileFolderPath);

                File.WriteAllText(compileFilePath, Code);
                File.WriteAllText(compileHeaderPath, _Header);

                byte[] outf = Program.IsRunningUnderMono
                    ? CompileUnderMono(folder, CodeEntry, ref CompileMsgs, compileFilePath, outFilePath, linkerFiles)
                    : CompileUnderWindows(folder, CodeEntry, ref CompileMsgs, compileFilePath, outFilePath, linkerFiles);

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

        public static string[] ResolveLinkerPaths(string LinkerPaths)
        {
            string[] Paths = LinkerPaths.Split(';');

            for (int i = 0; i < Paths.Length; i++)
            {
                if (!String.IsNullOrEmpty(Paths[i]))
                {
                    Paths[i] = Helpers.ReplaceTokenWithPath(Program.Settings.ProjectPath, Paths[i], Dicts.ProjectPathToken);
                }
            }

            return Paths;
        }

        public static byte[] CompileUnderMono(string folder, CCodeEntry codeEntry, ref string compileMsgs,
                                              string compileFile, string outFilePath, string linkerFiles = "", string compileFlags = "")
        {
            var config = new CompilationConfig
            {
                IsMonoEnvironment = true,
                Folder = folder,
                LinkerFiles = ResolveLinkerPaths(linkerFiles),
                OutPath = outFilePath,
                CompileFlags = compileFlags,
                BinDirectory = "binmono",
                GccExecutable = "mips64-gcc",
                LdExecutable = "mips64-ld",
                NovlExecutable = "nOVL",
                NovlWorkingDirectory = "nOVL"
            };

            return CompileInternal(config, codeEntry, ref compileMsgs, compileFile);
        }

        public static byte[] CompileUnderWindows(string folder, CCodeEntry codeEntry, ref string compileMsgs,
                                                 string compileFile = "", string outFilePath = "", string linkerFiles = "", string compileFlags = "")
        {
            var config = new CompilationConfig
            {
                IsMonoEnvironment = false,
                Folder = folder,
                LinkerFiles = ResolveLinkerPaths(linkerFiles),
                OutPath = outFilePath,
                CompileFlags = compileFlags,
                BinDirectory = "bin",
                GccExecutable = "mips64-gcc.exe",
                LdExecutable = "mips64-ld.exe",
                NovlExecutable = "novl.exe",
                NovlWorkingDirectory = Path.Combine("nOVL")
            };

            return CompileInternal(config, codeEntry, ref compileMsgs, compileFile);
        }

        public static bool IsFullyQualified(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // Check if it's a Unix absolute path
            if (Program.IsRunningUnderMono && path.StartsWith("/", StringComparison.Ordinal))
                return true;

            return Path.IsPathRooted(path) &&
                   !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(),
                                                 StringComparison.Ordinal);
        }

        public static List<string> ExtractHeaderPaths(string dFilePath, string folderPath, string[] excludedPaths)
        {
            string content = File.ReadAllText(dFilePath);
            List<string> excluded = excludedPaths.ToList();

            // Remove everything before and including the first colon
            int colonIndex = content.IndexOf(':');
            if (colonIndex == -1) return new List<string>();

            string dependencies = content.Substring(colonIndex + 1);

            // Handle line continuations (backslash + newline)
            dependencies = Regex.Replace(dependencies, @"\\\s*\r?\n\s*", " ");

            // Match paths, handling escaped spaces
            // This regex matches sequences of non-whitespace characters and escaped spaces
            var matches = Regex.Matches(dependencies, @"(?:[^\s\\]|\\.)+");

            var headerPaths = new List<string>();

            foreach (Match match in matches)
            {
                string path = match.Value;

                // Unescape spaces
                path = path.Replace(@"\ ", " ");

                if (!IsFullyQualified(path) && !(Program.IsRunningUnderMono && path.StartsWith("..")))
                    path = Path.Combine(Program.ExecPath, path.TrimStart('/', '\\'));

                if (!string.IsNullOrEmpty(path) &&
                    !path.Contains(folderPath) &&
                    !IsPathExcluded(path, excluded))
                {
                    path = Helpers.FixCygdrivePath(path);

                    if (File.Exists(path))
                    {
                        string pathProjPathReplaced = Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, path, Dicts.ProjectPathToken);

                        if (!headerPaths.Contains(pathProjPathReplaced))
                            headerPaths.Add(Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, path, Dicts.ProjectPathToken));

                        excluded.Add(path);
                    }
                }
            }

            return headerPaths;
        }

        private static bool IsPathExcluded(string path, List<string> excludedPaths)
        {
            if (excludedPaths == null || excludedPaths.Count == 0)
                return false;

            // Normalize the path for comparison
            string normalizedPath = Path.GetFullPath(path);

            foreach (string excludedPath in excludedPaths)
            {
                if (string.IsNullOrEmpty(excludedPath))
                    continue;

                try
                {
                    if (path.StartsWith(excludedPath, StringComparison.OrdinalIgnoreCase))
                        return true;

                    string normalizedExcludedPath = Path.GetFullPath(excludedPath);

                    // Check if the path starts with the excluded path (is contained within)
                    if (normalizedPath.StartsWith(normalizedExcludedPath, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                catch (Exception)
                {
                    // If path normalization fails, fall back to simple string comparison
                    if (path.StartsWith(excludedPath, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private static byte[] CompileInternal(CompilationConfig config, CCodeEntry codeEntry, ref string compileMsgs, string compileFile = "")
        {
            var paths = new CompilationPaths(config, compileFile);
            string compilationFailedString = "Compilation failed.";

            Clean(new[] { paths.ObjectFile, paths.ElfFile, paths.OvlFile });

            if (!RunGccCompilation(config, paths, ref compileMsgs))
                return HandleCompilationFailure(compilationFailedString, ref compileMsgs);

            if (!RunLinkerPhase(config, paths, ref compileMsgs))
                return HandleCompilationFailure(compilationFailedString, ref compileMsgs);

            if (!RunNovlPhase(config, paths, ref compileMsgs))
                return HandleCompilationFailure(compilationFailedString, ref compileMsgs, true);

            if (codeEntry != null)
            {
                string[] excludedHeaderPaths = GetIncludePaths(config);
                codeEntry.HeaderPaths = ExtractHeaderPaths(paths.dFile, config.Folder, excludedHeaderPaths);
            }

            compileMsgs += "Done!";

            if (codeEntry != null)
                codeEntry.Functions = GetNpcMakerFunctionsFromO(paths.ElfFile, paths.OvlFile, config.IsMonoEnvironment);

            CleanupIntermediateFiles(paths.ObjectFile, paths.ElfFile, paths.dFile);
            return File.ReadAllBytes(paths.OvlFile);
        }

        private static bool RunGccCompilation(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            var gccInfo = CreateGccProcessInfo(config, paths);


            if (Program.Settings.Verbose)
                compileMsgs += $"{gccInfo.FileName} {gccInfo.Arguments}{Environment.NewLine}";

            int result = -1;

            using (var process = Process.Start(gccInfo))
            {
                var sectionName = config.IsMonoEnvironment ? "Mono GCC" : "GCC";
                GetProcessOutput(process, sectionName, ref compileMsgs, config.IsMonoEnvironment);
                result = process.ExitCode;
            }

            return File.Exists(paths.ObjectFile) && (result == 0);
        }

        private static bool RunLinkerPhase(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            var ldInfo = CreateLinkerProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{ldInfo.FileName} {ldInfo.Arguments}{Environment.NewLine}";

            int result = -1;

            using (var process = Process.Start(ldInfo))
            {
                var sectionName = config.IsMonoEnvironment ? "Mono LINKER" : "LINKER";
                GetProcessOutput(process, sectionName, ref compileMsgs, config.IsMonoEnvironment);
                result = process.ExitCode;
            }

            return File.Exists(paths.ObjectFile) && (result == 0);
        }

        private static bool RunNovlPhase(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            var novlInfo = CreateNovlProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{novlInfo.FileName} {novlInfo.Arguments}{Environment.NewLine}";

            int result = -1;

            if (File.Exists(novlInfo.FileName))
            {
                using (var process = Process.Start(novlInfo))
                {
                    var sectionName = config.IsMonoEnvironment ? "Mono NOVL" : "NOVL";
                    GetProcessOutput(process, sectionName, ref compileMsgs, config.IsMonoEnvironment);
                    result = process.ExitCode;
                }
            }

            return File.Exists(paths.ObjectFile) && (result == 0);
        }

        private static ProcessStartInfo CreateGccProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            var includeFlags = BuildIncludeFlags(config);
            var projectFlag = string.IsNullOrEmpty(Program.Settings.ProjectPath)
                ? string.Empty
                : $"-I {Program.Settings.ProjectPath.AppendQuotation()} ";

            var arguments = $"{includeFlags} {projectFlag}{Program.Settings.GCCFlags} {config.CompileFlags} -MMD -B {paths.WorkingDirectory.AppendQuotation()} {paths.SourceFile.AppendQuotation()}";


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
            var extraLinkerFile = "";

            foreach (string lf in config.LinkerFiles)
            {
                if (!string.IsNullOrWhiteSpace(lf))
                    extraLinkerFile = $"-T {lf.AppendQuotation()}";
            }

            var arguments = $"{libraryFlags} -T syms.ld -T z64hdr_actor.ld {extraLinkerFile} --emit-relocs -o {paths.ElfFile.AppendQuotation()} {paths.ObjectFile.AppendQuotation()} {Path.Combine(paths.WorkingDirectory, "libgcc.a")}";

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
            var workingDir = paths.WorkingDirectory;

            var fileName = config.IsMonoEnvironment
                ? Path.Combine(paths.WorkingDirectory, "nOVL")
                : Path.Combine(paths.WorkingDirectory, "nOVL.exe");

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

        private static string[] GetIncludePaths(CompilationConfig config)
        {
            var basePath = config.IsMonoEnvironment ? ".." : Path.Combine(Program.ExecPath, "gcc");
            return new[]
            {
                Path.Combine(basePath, "mips64", "include"),
                Path.Combine(basePath, "mips64", "include", "z64hdr", Program.Settings.GameVersion.ToString()),
                Path.Combine(basePath, "mips64", "include", "z64hdr", "include")
            };
        }

        private static string BuildIncludeFlags(CompilationConfig config)
        {
            var includes = GetIncludePaths(config);
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
            var objDumpOutput = ExecuteObjDump(elfPath, mono);
            var normalizedOutput = Regex.Replace(objDumpOutput.Replace("\t", " "), @"\s{2,}", " ", RegexOptions.Compiled);
            var lines = normalizedOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var ovlData = File.ReadAllBytes(ovlPath);
            var sectionOffsets = CalculateSectionOffsets(ovlData);

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
            var executable = mono ? "mips64-objdump" : "mips64-objdump.exe";

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

                try
                {
                    string vscodeFolder = Path.Combine(gtempFolderPath, ".vscode");
                    Directory.CreateDirectory(vscodeFolder);

                    string cprops = Properties.Resources.c_cpp_properties;

                    if (!string.IsNullOrEmpty(Program.Settings.ProjectPath))
                    {
                        string uriPath = new Uri(Program.Settings.ProjectPath).AbsolutePath;
                        cprops = cprops.Replace(Dicts.ProjectPathToken, uriPath);
                    }
                    else
                        cprops = cprops.Replace(Dicts.ProjectPathToken, "");

                    File.WriteAllText(Path.Combine(vscodeFolder, "c_cpp_properties.json"), cprops);
                    File.WriteAllText(Path.Combine(vscodeFolder, "settings.json"), Properties.Resources.settings);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create .vscode folder {ex.Message}");
                }

                if (!HeaderOnly)
                    File.WriteAllText(Path.Combine(gtempFolderPath, $"{gcodeFileNameBase}.c"), Code);

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

        public static Process OpenCodeEditor(CodeEditorEnum SelectedCodeEditor, string Path, string Args, bool justHeader)
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
                    case CodeEditorEnum.VSCode:
                        {
                            if (Program.IsRunningUnderMono)
                            {
                                startInfo.FileName = "code";
                                startInfo.Arguments = $"-n {gtempFolderPath.AppendQuotation()}";
                            }
                            else
                            {
                                startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\code";
                                startInfo.Arguments = $"-n {gtempFolderPath.AppendQuotation()}";
                            }
                            break;
                        }
                    case CodeEditorEnum.Notepad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\notepad.exe";
                            startInfo.Arguments = $"{geditCodeFilePath.AppendQuotation()}";
                            break;
                        }
                    case CodeEditorEnum.NotepadPlusPlus:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Notepad++\notepad++.exe";

                            if (justHeader)
                                startInfo.Arguments = $"{geditHeaderFilePath.AppendQuotation()} -multiInst";
                            else
                                startInfo.Arguments = $"{geditHeaderFilePath.AppendQuotation()} {geditCodeFilePath.AppendQuotation()} -multiInst";

                            break;
                        }
                    case CodeEditorEnum.Sublime:
                        {
                            if (Program.IsRunningUnderMono)
                            {
                                startInfo.FileName = "subl";
                                startInfo.Arguments = $"{geditCodeFilePath.AppendQuotation()}";
                            }
                            else
                            {
                                startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Sublime Text\subl.exe";
                                startInfo.Arguments = $"-n {geditCodeFilePath.AppendQuotation()}";
                            }
                            break;
                        }
                    case CodeEditorEnum.WordPad:
                        {
                            startInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\write.exe";
                            startInfo.Arguments = $"-n {geditCodeFilePath.AppendQuotation()}";
                            break;
                        }
                    case CodeEditorEnum.Kate:
                        {
                            startInfo.FileName = "kate";

                            if (justHeader)
                                startInfo.Arguments = $"{geditHeaderFilePath.AppendQuotation()}";
                            else
                                startInfo.Arguments = $"{geditHeaderFilePath.AppendQuotation()} {geditCodeFilePath.AppendQuotation()}";

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
