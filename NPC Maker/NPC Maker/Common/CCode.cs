using NPC_Maker.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC_Maker
{
    public static class CCode
    {
        // ───────────────────────────────────────────────────────

        public static readonly string TempFolderName = "temp";
        public static readonly string CompileFolderName = "compile";
        public static readonly string TempFolderPath = Path.Combine(Program.ExecPath, TempFolderName);
        public static readonly string CompileFolderPath = Path.Combine(Program.ExecPath, CompileFolderName);
        public static readonly string CodeFileNameBase = "EmbeddedOverlay";
        public static readonly string HeaderFileName = "npc_maker_header.h";
        public static readonly string EditCodeFilePath = Path.Combine(TempFolderPath, $"{CodeFileNameBase}.c");
        public static readonly string EditHeaderFilePath = Path.Combine(TempFolderPath, HeaderFileName);

        public static uint BaseAddr = 0x80800000;

        private const string CompilationFailedString = "Compilation failed.";

        // ───────────────────────────────────────────────────────────

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

        public static readonly string[] CodeEditors =
        {
            CodeEditorEnum.VSCode.ToString(),
            CodeEditorEnum.Notepad.ToString(),
            CodeEditorEnum.NotepadPlusPlus.ToString(),
            CodeEditorEnum.Sublime.ToString(),
            CodeEditorEnum.WordPad.ToString(),
            CodeEditorEnum.Other.ToString()
        };

        // ──────────────────────────────────────────────────────────

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
            public string SourceFile { get; }
            public string ObjectFile { get; }
            public string ElfFile { get; }
            public string OvlFile { get; }
            public string DFile { get; }

            public CompilationPaths(CompilationConfig config, string sourceFile)
            {
                CompileFileName = Path.GetFileNameWithoutExtension(sourceFile);
                SourceFile = sourceFile;
                OvlFile = config.OutPath;
                WorkingDirectory = Path.Combine(Program.ExecPath, "gcc", config.BinDirectory);
                ObjectFile = Path.Combine(WorkingDirectory, $"{CompileFileName}.o");
                DFile = Path.Combine(WorkingDirectory, $"{CompileFileName}.d");
                ElfFile = Path.Combine(WorkingDirectory, $"{CompileFileName}.elf");
            }
        }

        // ──────────────────────────────────────────────────────────────

        public static void Compile(string cFilePath, string linkerFiles, string outFilePath, string compileFlags, ref string compileMsgs, out List<CSymbol> symbols)
        {
            string folder = Helpers.GenerateTemporaryFolderName();

            if (Program.IsRunningUnderMono)
                CompileUnderMono(folder, null, ref compileMsgs, out symbols, cFilePath, outFilePath, linkerFiles, compileFlags);
            else
                CompileUnderWindows(folder, null, ref compileMsgs, out symbols, cFilePath, outFilePath, linkerFiles, compileFlags);
        }

        public static byte[] Compile(string header, string linkerFiles, CCodeEntry codeEntry, ref string compileMsgs, out List<CSymbol> symbols, string folder = "")
        {
            symbols = null;

            try
            {
                if (string.IsNullOrWhiteSpace(codeEntry.Code))
                    return new byte[0];

                if (string.IsNullOrWhiteSpace(folder))
                    folder = Helpers.GenerateTemporaryFolderName();

                string code = ReplaceGameVersionInclude(codeEntry.Code);
                string processedHeader = ReplaceGameVersionInclude(header);

                string compileFolderPath = Path.Combine(Program.ExecPath, CompileFolderName, folder);
                string compileFilePath = Path.Combine(compileFolderPath, $"{CodeFileNameBase}{folder}.c");
                string compileHeaderPath = Path.Combine(compileFolderPath, HeaderFileName);
                string outFilePath = Path.Combine(compileFolderPath, $"{Path.GetFileName(compileFilePath)}.ovl");

                if (Directory.Exists(compileFolderPath))
                    Directory.Delete(compileFolderPath, true);

                Directory.CreateDirectory(compileFolderPath);
                File.WriteAllText(compileFilePath, code);
                File.WriteAllText(compileHeaderPath, processedHeader);

                byte[] result = Program.IsRunningUnderMono
                    ? CompileUnderMono(folder, codeEntry, ref compileMsgs, out symbols, compileFilePath, outFilePath, linkerFiles)
                    : CompileUnderWindows(folder, codeEntry, ref compileMsgs, out symbols, compileFilePath, outFilePath, linkerFiles);

                Directory.Delete(compileFolderPath, true);
                return result;
            }
            catch (Exception ex)
            {
                compileMsgs = "Compilation failed: " + ex.Message;
                return null;
            }
        }

        public static byte[] CompileUnderMono(string folder, CCodeEntry codeEntry, ref string compileMsgs, out List<CSymbol> outSymbols,
                                              string compileFile, string outFilePath, string linkerFiles = "", string compileFlags = "")
        {
            return CompileInternal(BuildMonoConfig(folder, outFilePath, linkerFiles, compileFlags),
                                   codeEntry, ref compileMsgs, out outSymbols, compileFile);
        }

        public static byte[] CompileUnderWindows(string folder, CCodeEntry codeEntry, ref string compileMsgs, out List<CSymbol> outSymbols,
                                                 string compileFile = "", string outFilePath = "", string linkerFiles = "", string compileFlags = "")
        {
            return CompileInternal(BuildWindowsConfig(folder, outFilePath, linkerFiles, compileFlags),
                                   codeEntry, ref compileMsgs, out outSymbols, compileFile);
        }

        public static string ReplaceGameVersionInclude(string code)
        {
            string replacement = $"#include <z64hdr/{Lists.GameVersionStrings[Lists.Library.z64hdr][(int)Program.Settings.GameVersion]}/z64hdr.h>";
            code = code.Replace("#include <z64hdr/oot_u10/z64hdr.h>", replacement);
            code = code.Replace("#include <z64hdr/oot_mq_debug/z64hdr.h>", replacement);
            return code;
        }

        public static void CleanupStandardCompilationArtifacts()
        {
            foreach (string binDir in new[] { "bin", "binmono" })
            {
                string path = Path.Combine(Program.ExecPath, "gcc", binDir);
                Helpers.DeleteFileStartingWith(path, "EmbeddedOverlayNPCCOMPILE");
                Helpers.DeleteFileStartingWith(path, "EmbeddedOverlaytemp");
            }
        }

        public static void ConsoleWriteCompileFail(string compilationMsgs)
        {
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{compilationMsgs}{Environment.NewLine}{Environment.NewLine}");
        }

        public static void Clean(string[] files)
        {
            try
            {
                foreach (string f in files)
                    if (File.Exists(f))
                        File.Delete(f);
            }
            catch (Exception) { }
        }

        // ── Compilation Configs ───────────────────────────────────────────────────

        private static CompilationConfig BuildMonoConfig(string folder, string outFilePath, string linkerFiles, string compileFlags)
        {
            return new CompilationConfig
            {
                IsMonoEnvironment = true,
                Folder = folder,
                LinkerFiles = Helpers.ResolveSemicolonPaths(linkerFiles),
                OutPath = outFilePath,
                CompileFlags = compileFlags,
                BinDirectory = "binmono",
                GccExecutable = "mips64-gcc",
                LdExecutable = Program.Settings.Linker == Lists.Linker.zlinker ? "zlinker" : "mips64-ld",
                NovlExecutable = "nOVL",
                NovlWorkingDirectory = "nOVL"
            };
        }

        private static CompilationConfig BuildWindowsConfig(string folder, string outFilePath, string linkerFiles, string compileFlags)
        {
            return new CompilationConfig
            {
                IsMonoEnvironment = false,
                Folder = folder,
                LinkerFiles = Helpers.ResolveSemicolonPaths(linkerFiles),
                OutPath = outFilePath,
                CompileFlags = compileFlags,
                BinDirectory = "bin",
                GccExecutable = "mips64-gcc.exe",
                LdExecutable = Program.Settings.Linker == Lists.Linker.zlinker ? "zlinker.exe" : "mips64-ld.exe",
                NovlExecutable = "novl.exe",
                NovlWorkingDirectory = Path.Combine("nOVL")
            };
        }

        // ── Compilation ──────────────────────────────────────────────────

        private static byte[] CompileInternal(CompilationConfig config, CCodeEntry codeEntry, ref string compileMsgs, out List<CSymbol> outSymbols, string compileFile = "")
        {
            var paths = new CompilationPaths(config, compileFile);
            outSymbols = null;

            try
            {
                if (codeEntry != null)
                    codeEntry.HeaderPaths.Clear();

                Clean(new[] { paths.ObjectFile, paths.ElfFile, paths.OvlFile });

                if (!RunGccCompilation(config, paths, ref compileMsgs))
                    return HandleCompilationFailure(CompilationFailedString, ref compileMsgs);

                if (!RunLinkerPhase(config, paths, codeEntry, ref compileMsgs, out outSymbols))
                    return HandleCompilationFailure(CompilationFailedString, ref compileMsgs);

                if (Program.Settings.Linker == Lists.Linker.MipsLD)
                {
                    if (!RunNovlPhase(config, paths, ref compileMsgs))
                        return HandleCompilationFailure(CompilationFailedString, ref compileMsgs, returnEmptyArray: true);
                }
                else
                {
                    if (new FileInfo(paths.OvlFile).Length == 0)
                    {
                        compileMsgs += "Code file was empty...";
                        return new byte[0];
                    }
                }

                if (codeEntry != null)
                    codeEntry.HeaderPaths = ExtractHeaderPaths(paths.DFile, config.Folder);

                compileMsgs += "Done!";

                if (Program.Settings.Linker == Lists.Linker.MipsLD)
                {
                    if (codeEntry != null)
                    {
                        codeEntry.Functions = GetNpcMakerFunctionsFromO(paths.ElfFile, paths.OvlFile, config.IsMonoEnvironment);
                        outSymbols = codeEntry.Functions;
                    }
                    else
                    {
                        outSymbols = GetAllSymbolsFromO(paths.ElfFile, paths.OvlFile, config.IsMonoEnvironment);
                    }
                }

                CleanupIntermediateFiles(paths.ObjectFile, paths.ElfFile, paths.DFile);
                return File.ReadAllBytes(paths.OvlFile);
            }
            catch (Exception ex)
            {
                CleanupIntermediateFiles(paths.ObjectFile, paths.ElfFile, paths.DFile);
                return HandleCompilationFailure($"{CompilationFailedString} {ex.Message}", ref compileMsgs);
            }
        }

        private static bool RunGccCompilation(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            compileMsgs += $"+==============+ Compiler +==============+{Environment.NewLine}";

            var info = CreateGccProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{info.FileName} {info.Arguments}{Environment.NewLine}";

            int exitCode;

            using (var process = Process.Start(info))
            {
                GetProcessOutput(process, ref compileMsgs, config.IsMonoEnvironment);
                exitCode = process.ExitCode;
            }

            return File.Exists(paths.ObjectFile) && exitCode == 0;
        }

        private static bool RunLinkerPhase(CompilationConfig config, CompilationPaths paths, CCodeEntry codeEntry, ref string compileMsgs, out List<CSymbol> symbols)
        {
            compileMsgs += $"+==============+ Linker +==============+{Environment.NewLine}";
            symbols = null;

            bool usingZLinker = Program.Settings.Linker == Lists.Linker.zlinker;
            var info = usingZLinker
                ? CreateZLinkerProcessInfo(config, paths)
                : CreateMipsLDLinkerProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{info.FileName} {info.Arguments}{Environment.NewLine}";

            int exitCode;

            using (var process = Process.Start(info))
            {
                if (usingZLinker)
                {
                    string symbolOutput = GetProcessOutput(process, ref compileMsgs, config.IsMonoEnvironment, errorsOnly: true);
                    exitCode = process.ExitCode;

                    if (exitCode == 0 && codeEntry != null)
                        codeEntry.Functions = GetNpcMFuncsFromZLinkerOutput(symbolOutput);
                    else
                        symbols = GetAllSymbolsFromZLinkerOutput(symbolOutput);
                }
                else
                {
                    GetProcessOutput(process, ref compileMsgs, config.IsMonoEnvironment);
                    exitCode = process.ExitCode;
                }
            }

            return File.Exists(paths.ObjectFile) && exitCode == 0;
        }

        private static bool RunNovlPhase(CompilationConfig config, CompilationPaths paths, ref string compileMsgs)
        {
            compileMsgs += $"+==============+ NOVL +==============+{Environment.NewLine}";

            var info = CreateNovlProcessInfo(config, paths);

            if (Program.Settings.Verbose)
                compileMsgs += $"{info.FileName} {info.Arguments}{Environment.NewLine}";

            if (!File.Exists(info.FileName))
                return false;

            int exitCode;
            using (var process = Process.Start(info))
            {
                GetProcessOutput(process, ref compileMsgs, config.IsMonoEnvironment);
                exitCode = process.ExitCode;
            }

            return File.Exists(paths.ObjectFile) && exitCode == 0;
        }

        // ── ProcessStartInfo ─────────────────────────────────────────────

        private static ProcessStartInfo CreateGccProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            string includeFlags = BuildIncludeFlags();
            string projectFlag = string.IsNullOrEmpty(Program.Settings.ProjectPath)
                ? string.Empty
                : $"-I {Program.Settings.ProjectPath.AppendQuotation()} ";

            string arguments = $"{includeFlags} {projectFlag}{Program.Settings.GCCFlags} {config.CompileFlags} -MMD -B {paths.WorkingDirectory.AppendQuotation()} {paths.SourceFile.AppendQuotation()}";

            return CreateProcessStartInfo(paths.WorkingDirectory,
                Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, config.GccExecutable),
                arguments);
        }

        private static ProcessStartInfo CreateZLinkerProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            string extraLinkerFiles = BuildLinkerFileArgs(config.LinkerFiles);

            string arguments = $"-s {extraLinkerFiles} " +
                               $"-o {paths.OvlFile.AppendQuotation()} " +
                               $"-e 0x{BaseAddr:X} " +
                               $"-i {paths.ObjectFile.AppendQuotation()}";

            return CreateProcessStartInfo(paths.WorkingDirectory,
                Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, config.LdExecutable),
                arguments);
        }

        private static ProcessStartInfo CreateMipsLDLinkerProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            string libraryFlags = BuildLibraryFlags(config);
            string extraLinkerFile = "";

            foreach (string lf in config.LinkerFiles)
                if (!string.IsNullOrWhiteSpace(lf))
                    extraLinkerFile = $"-T {lf.AppendQuotation()}";

            string arguments = $"{libraryFlags} -T syms.ld -T z64hdr_actor.ld {extraLinkerFile} --emit-relocs " +
                               $"-o {paths.ElfFile.AppendQuotation()} " +
                               $"{paths.ObjectFile.AppendQuotation()} " +
                               $"{Path.Combine(paths.WorkingDirectory, "libgcc.a").AppendQuotation()}";

            return CreateProcessStartInfo(paths.WorkingDirectory,
                Path.Combine(Program.ExecPath, "gcc", config.BinDirectory, config.LdExecutable),
                arguments);
        }

        private static ProcessStartInfo CreateNovlProcessInfo(CompilationConfig config, CompilationPaths paths)
        {
            string fileName = config.IsMonoEnvironment
                ? Path.Combine(paths.WorkingDirectory, "nOVL")
                : Path.Combine(paths.WorkingDirectory, "nOVL.exe");

            string verboseFlag = Program.Settings.Verbose ? "-vv" : "";
            string arguments = $"-c {verboseFlag} -A 0x{BaseAddr:X} -o {paths.OvlFile.AppendQuotation()} {paths.ElfFile.AppendQuotation()}";

            return CreateProcessStartInfo(paths.WorkingDirectory, fileName, arguments);
        }

        private static ProcessStartInfo CreateProcessStartInfo(string workingDirectory, string fileName, string arguments)
        {
            return new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory,
                FileName = fileName,
                Arguments = arguments
            };
        }

        // ── Process Output ────────────────────────────────────────────────────────

        private static string GetProcessOutput(Process process, ref string compileErrors, bool isMonoEnvironment, bool errorsOnly = false)
        {
            string outputResult = "";
            string errorResult = "";

            try
            {
                var outputTask = TaskEx.Run(() => process.StandardOutput.ReadToEnd());
                var errorTask = TaskEx.Run(() => process.StandardError.ReadToEnd());

                bool completed = process.WaitForExit((int)Program.Settings.CompileTimeout);

                if (!completed)
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

                string combined = FormatProcessOutput(errorsOnly ? "" : outputResult, errorResult);
                if (!string.IsNullOrWhiteSpace(combined))
                    compileErrors += combined;

                if (completed && process.ExitCode == 0)
                    compileErrors += $"{Environment.NewLine}OK!{Environment.NewLine}";
                else if (completed)
                    compileErrors += $"{Environment.NewLine}Process exited with code: {process.ExitCode}{Environment.NewLine}";

                return outputResult;
            }
            catch (Exception ex)
            {
                compileErrors += $"{Environment.NewLine}Exception while reading process output: {ex.Message}{Environment.NewLine}";
                return "";
            }
        }

        private static string FormatProcessOutput(string standardOutput, string standardError)
        {
            var sb = new StringBuilder();
            sb.AppendLine();

            if (!string.IsNullOrEmpty(standardOutput))
                sb.AppendLine(standardOutput.Replace("\n", Environment.NewLine));

            if (!string.IsNullOrEmpty(standardError))
                sb.AppendLine(standardError.Replace("\n", Environment.NewLine));

            return sb.ToString();
        }

        // ── Symbol Extraction ─────────────────────────────────────────────────────

        public static List<CSymbol> GetSymbolsFromO(string elfPath, string ovlPath, bool mono, bool addSection, Func<string[], bool> filter)
        {
            string raw = ExecuteObjDump(elfPath, mono);
            string normalized = Regex.Replace(raw.Replace("\t", " "), @"\s{2,}", " ", RegexOptions.Compiled);
            string[] lines = normalized.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] ovlData = File.ReadAllBytes(ovlPath);
            var sectionOffsets = CalculateSectionOffsets(ovlData);

            return lines
                .Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(filter)
                .Select(words => CreateSymbol(words, sectionOffsets, addSection))
                .Where(s => s != null)
                .OrderBy(s => s.Symbol)
                .ToList();
        }

        public static List<CSymbol> GetNpcMakerFunctionsFromO(string elfPath, string ovlPath, bool mono)
            => GetSymbolsFromO(elfPath, ovlPath, mono, addSection: true, filter: IsValidNpcMakerFunction);

        public static List<CSymbol> GetAllSymbolsFromO(string elfPath, string ovlPath, bool mono)
            => GetSymbolsFromO(elfPath, ovlPath, mono, addSection: false, filter: _ => true);

        public static List<CSymbol> GetSymbolsFromZLinkerOutput(string zLinkerOutput, Func<string[], bool> filter)
        {
            var result = new List<CSymbol>();

            foreach (string line in zLinkerOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (filter(parts))
                    result.Add(new CSymbol(parts[1].Trim(), Convert.ToUInt32(parts[0], 16) - BaseAddr));
            }

            return result;
        }

        public static List<CSymbol> GetNpcMFuncsFromZLinkerOutput(string zLinkerOutput)
            => GetSymbolsFromZLinkerOutput(zLinkerOutput,
                parts => parts[1].StartsWith("NpcM_", StringComparison.OrdinalIgnoreCase));

        public static List<CSymbol> GetAllSymbolsFromZLinkerOutput(string zLinkerOutput)
            => GetSymbolsFromZLinkerOutput(zLinkerOutput, _ => true);

        private static string ExecuteObjDump(string elfPath, bool mono)
        {
            string binDir = mono ? "binmono" : "bin";
            string executable = mono ? "mips64-objdump" : "mips64-objdump.exe";

            var info = CreateProcessStartInfo(
                Path.Combine(Program.ExecPath, "gcc", binDir),
                Path.Combine(Program.ExecPath, "gcc", binDir, executable),
                $"-t {elfPath.AppendQuotation()}");

            using (var process = Process.Start(info))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
        }

        private static Dictionary<string, uint> CalculateSectionOffsets(byte[] ovlData)
        {
            uint sectionOffs = Program.BEConverter.ToUInt32(ovlData, ovlData.Length - 4);
            int baseOffset = ovlData.Length - (int)sectionOffs;

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
            return words.Length == 6
                && words[2] == "F"
                && words[5].StartsWith("NpcM_", StringComparison.OrdinalIgnoreCase);
        }

        private static CSymbol CreateSymbol(string[] words, Dictionary<string, uint> sectionOffsets, bool addSection)
        {
            if (words.Length < 6)
                return null;

            uint address;
            if (!uint.TryParse(words[0], System.Globalization.NumberStyles.HexNumber, null, out address))
                return null;

            uint sectionOffset = 0;
            if (addSection && !sectionOffsets.TryGetValue(words[3], out sectionOffset))
                return null;

            return new CSymbol(words[5], address - BaseAddr + sectionOffset);
        }

        // ── Header Path Extraction ────────────────────────────────────────────────

        public static List<string> ExtractHeaderPaths(string dFilePath, string folderPath)
        {
            string content = File.ReadAllText(dFilePath);

            int colonIndex = content.IndexOf(':');
            if (colonIndex == -1) return new List<string>();

            string dependencies = content.Substring(colonIndex + 1);
            dependencies = Regex.Replace(dependencies, @"\\\s*\r?\n\s*", " ");

            var matches = Regex.Matches(dependencies, @"(?:[^\s\\]|\\.)+");
            var headerPaths = new List<string>();
            var excluded = new List<string>();

            foreach (Match match in matches)
            {
                string path = match.Value.Replace(@"\ ", " ");

                if (!IsFullyQualified(path) && !(Program.IsRunningUnderMono && path.StartsWith("..")))
                    path = Path.Combine(Program.ExecPath, path.TrimStart('/', '\\'));

                if (string.IsNullOrEmpty(path) || path.Contains(folderPath) || IsPathExcluded(path, excluded))
                    continue;

                path = Helpers.FixCygdrivePath(path);

                if (!File.Exists(path))
                    continue;

                string tokenized = Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, path, Lists.ProjectPathToken);

                if (!headerPaths.Contains(tokenized))
                    headerPaths.Add(tokenized);

                excluded.Add(path);
            }

            return headerPaths;
        }

        public static bool IsFullyQualified(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if (Program.IsRunningUnderMono && path.StartsWith("/", StringComparison.Ordinal))
                return true;

            return Path.IsPathRooted(path)
                && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }


        private static bool IsPathExcluded(string path, List<string> excludedPaths)
        {
            var parts = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (parts.Any(p => p.Equals("z64hdr", StringComparison.OrdinalIgnoreCase) || p.Equals("zocarina", StringComparison.OrdinalIgnoreCase)))
                return true;

            if (excludedPaths == null || excludedPaths.Count == 0)
                return false;

            string normalizedPath = null;
            try { normalizedPath = Path.GetFullPath(path); } catch { }

            foreach (string excluded in excludedPaths)
            {
                if (string.IsNullOrEmpty(excluded))
                    continue;

                if (path.StartsWith(excluded, StringComparison.OrdinalIgnoreCase))
                    return true;

                if (normalizedPath != null)
                {
                    string normalizedExcluded = null;
                    try { normalizedExcluded = Path.GetFullPath(excluded); } catch { }

                    if (normalizedExcluded != null
                        && normalizedPath.StartsWith(normalizedExcluded, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        // ── Editors ───────────────────────────────────────────────

        public static bool CreateCTempDirectory(string code, string header, bool errorMsg = true, bool headerOnly = false)
        {
            try
            {
                if (Directory.Exists(TempFolderPath))
                    Directory.Delete(TempFolderPath, true);

                Directory.CreateDirectory(TempFolderPath);

                TryCreateVsCodeFolder();

                if (!headerOnly)
                    File.WriteAllText(Path.Combine(TempFolderPath, $"{CodeFileNameBase}.c"), code);

                File.WriteAllText(Path.Combine(TempFolderPath, HeaderFileName), header);
                return true;
            }
            catch (Exception ex)
            {
                if (errorMsg)
                    BigMessageBox.Show("Error creating temporary directory: " + ex.Message);
                return false;
            }
        }

        private static void TryCreateVsCodeFolder()
        {
            try
            {
                string vsCodeFolder = Path.Combine(TempFolderPath, ".vscode");
                Directory.CreateDirectory(vsCodeFolder);

                string cprops = Properties.Resources.c_cpp_properties;

                if (Program.Settings.Library == Lists.Library.zocarina)
                    cprops = cprops.Replace("z64hdr", "zocarina");

                cprops = string.IsNullOrEmpty(Program.Settings.ProjectPath)
                    ? cprops.Replace(Lists.ProjectPathToken, "")
                    : cprops.Replace(Lists.ProjectPathToken, new Uri(Program.Settings.ProjectPath).AbsolutePath);

                File.WriteAllText(Path.Combine(vsCodeFolder, "c_cpp_properties.json"), cprops);
                File.WriteAllText(Path.Combine(vsCodeFolder, "settings.json"), Properties.Resources.settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create .vscode folder: {ex.Message}");
            }
        }

        public static Process OpenCodeEditor(CodeEditorEnum editor, string path, string args, bool justHeader)
        {
            try
            {
                var info = BuildEditorProcessInfo(editor, path, args, justHeader);
                return Process.Start(info);
            }
            catch (Exception ex)
            {
                BigMessageBox.Show("Error running editor: " + ex.Message);
                return null;
            }
        }

        private static ProcessStartInfo BuildEditorProcessInfo(CodeEditorEnum editor, string path, string args, bool justHeader)
        {
            var info = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = args
            };

            switch (editor)
            {
                case CodeEditorEnum.VSCode:
                    info.FileName = Program.IsRunningUnderMono
                        ? "code"
                        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                       @"Programs\Microsoft VS Code\code");
                    info.Arguments = $"-n {TempFolderPath.AppendQuotation()}";
                    break;

                case CodeEditorEnum.Notepad:
                    info.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "notepad.exe");
                    info.Arguments = EditCodeFilePath.AppendQuotation();
                    break;

                case CodeEditorEnum.NotepadPlusPlus:
                    info.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                                  @"Notepad++\notepad++.exe");
                    info.Arguments = justHeader
                        ? $"{EditHeaderFilePath.AppendQuotation()} -multiInst"
                        : $"{EditHeaderFilePath.AppendQuotation()} {EditCodeFilePath.AppendQuotation()} -multiInst";
                    break;

                case CodeEditorEnum.Sublime:
                    info.FileName = Program.IsRunningUnderMono
                        ? "subl"
                        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                       @"Sublime Text\subl.exe");
                    info.Arguments = Program.IsRunningUnderMono
                        ? EditCodeFilePath.AppendQuotation()
                        : $"-n {EditCodeFilePath.AppendQuotation()}";
                    break;

                case CodeEditorEnum.WordPad:
                    info.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "write.exe");
                    info.Arguments = $"-n {EditCodeFilePath.AppendQuotation()}";
                    break;

                case CodeEditorEnum.Kate:
                    info.FileName = "kate";
                    info.Arguments = justHeader
                        ? EditHeaderFilePath.AppendQuotation()
                        : $"{EditHeaderFilePath.AppendQuotation()} {EditCodeFilePath.AppendQuotation()}";
                    break;

                default:
                    info.FileName = path;
                    info.Arguments = args;
                    break;
            }

            return info;
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private static string BuildIncludeFlags()
        {
            return string.Join(" ",
                Helpers.ResolveSemicolonPaths(Program.Settings.IncludePaths)
                       .Select(p => $"-I {p.AppendQuotation()}"));
        }

        private static string BuildLibraryFlags(CompilationConfig config)
        {
            return string.Join(" ",
                Helpers.ResolveSemicolonPaths(Program.Settings.IncludePaths)
                       .Select(p => $"-L {p.AppendQuotation()}"));
        }

        private static string BuildLinkerFileArgs(string[] linkerFiles)
        {
            var sb = new StringBuilder();

            foreach (string lf in linkerFiles)
                if (!string.IsNullOrWhiteSpace(lf))
                    sb.Append($" {lf.AppendQuotation()}"); 

            return sb.ToString();
        }

        private static byte[] HandleCompilationFailure(string message, ref string compileMsgs, bool returnEmptyArray = false)
        {
            compileMsgs += message;
            ConsoleWriteCompileFail(compileMsgs);
            return returnEmptyArray ? new byte[0] : null;
        }

        private static void CleanupIntermediateFiles(params string[] files)
        {
            foreach (string file in files)
                if (File.Exists(file))
                    File.Delete(file);
        }
    }
}