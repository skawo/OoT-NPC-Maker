using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace NPC_Maker
{
    static class Program
    {
        public static string ExecPath = "";
        public static bool IsRunningUnderMono = false;
        public static bool IsWSL = false;

        public static Process CodeEditorProcess;

        public static string SettingsFilePath;

        public static string ExtSettingsPathOverride = null;

        public static NPCMakerSettings Settings;

        public static string ScriptCachePath = "";
        public static string CCachePath = "";
        public static string JsonPath = "";
        public static string AutoSavePath = "";

        public static bool SaveInProgress = false;
        public static volatile bool CompileInProgress = false;
        public static bool CompileThereWereErrors = false;
        public static string CompileMonoErrors = "";

        public static bool consoleSilent = false;
        public static readonly Encoding Utf8 = Encoding.UTF8;
        public static readonly DataTable _sharedTable = new DataTable();
        public static Stopwatch _stopWatch;
        public static readonly Random _random = new Random();

        public static List<string> Monofonts = null;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [STAThread]
        static int Main(string[] args)
        {
            DetectRuntime();

            bool hasArgs = args.Length > 0;

            if (hasArgs)
            {
                SetupConsole(ref args);
                PrintBanner();
            }

            InitializePaths();
            EnsureDirectoriesExist();
            LoadSettings();

            return RunCLI(args);
        }

        private static void DetectRuntime()
        {
            IsRunningUnderMono = OperatingSystem.IsLinux();
            IsWSL = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
        }

        private static void SetupConsole(ref string[] args)
        {
            if (args.Any(a => a.Equals("--silent", StringComparison.OrdinalIgnoreCase)))
            {
                consoleSilent = true;
                args = args.Where(a => !a.Equals("--silent", StringComparison.OrdinalIgnoreCase)).ToArray();
            }

            string extSettingsArg = args.FirstOrDefault(a =>
                a.StartsWith("--extsettings=", StringComparison.OrdinalIgnoreCase));

            if (extSettingsArg != null)
            {
                ExtSettingsPathOverride = extSettingsArg
                    .Substring("--extsettings=".Length)
                    .Trim('"');

                args = args.Where(a => !a.Equals(extSettingsArg, StringComparison.OrdinalIgnoreCase)).ToArray();
            }
        }

        private static void PrintBanner()
        {
            ConsoleWriteLineS();
            ConsoleWriteLineS($"Zelda Ocarina of Time NPC Creation Tool v.3.785 tempCLI dotNET");
        }

        private static void InitializePaths()
        {
            ExecPath = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location
            ) ?? AppContext.BaseDirectory;

            ScriptCachePath = Path.Combine(ExecPath, "cache", "s_cache");
            CCachePath = Path.Combine(ExecPath, "cache", "c_cache");
            AutoSavePath = Path.Combine(ExecPath, "autosave");

            if (!string.IsNullOrEmpty(ExtSettingsPathOverride))
                SettingsFilePath = ExtSettingsPathOverride;
            else
                SettingsFilePath = Path.Combine(ExecPath, "Settings.json");
        }


        private static void EnsureDirectoriesExist()
        {
            if (!Directory.Exists(ScriptCachePath)) 
                Directory.CreateDirectory(ScriptCachePath);
            if (!Directory.Exists(CCachePath)) 
                Directory.CreateDirectory(CCachePath);
        }

        private static void LoadSettings()
        {
            Settings = FileOps.ParseSettingsJSON(SettingsFilePath);
        }

        private static int RunCLI(string[] args)
        {
            bool isCompileCommand = args.Length >= 4 && args.Length <= 5 && args[0].ToUpper() == "-C";
            bool isTableCommand = args.Length >= 5 && args.Length <= 6 && args[0].ToUpper() == "-M";
            bool isConvertCommand = args.Length >= 2;

            if (isCompileCommand)
                return RunCompileCommand(args);
            else if (isTableCommand)
                return RunMessageTableCommand(args);
            else if (isConvertCommand)
                return RunConvertCommand(args);
            else
                return PrintUsage();
        }

        private static int RunMessageTableCommand(string[] args)
        {
            try
            {
                NPCFile inFile = null;
                JsonPath = args[1];
                string outPathTable = args[3];
                string outPathStrings = args[4];

                inFile = FileOps.ParseNPCJsonFile(JsonPath);

                Dicts.LoadDicts();
                Dicts.ReloadLanguages(inFile.Languages);
                Program.Settings.GameVersion = inFile.GameVersion;

                if (!Int32.TryParse(args[2], out int actorID))
                    actorID = inFile.Entries.FindIndex(x => x.NPCName == args[2]);

                if (inFile.Entries.Count <= actorID || actorID < 0)
                    throw new Exception($"Actor ID {args[2]} not present in JSON");

                ConsoleWriteLineS($"Converting \"{Path.GetFileName(args[1])}\", actor ID {actorID} to {outPathTable} and {outPathStrings}...");

                List<byte> msgTable = new List<byte>();
                List<byte> msgData = new List<byte>();

                inFile.Entries[actorID].ConvertMessagesToGameTables(inFile.Languages, out msgTable, out msgData);
                File.WriteAllBytes(outPathTable, msgTable.ToArray());
                File.WriteAllBytes(outPathStrings, msgData.ToArray());

                if (!Program.IsRunningUnderMono)
                    Console.WriteLine("Press ENTER to exit...");

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting to message table: " + ex.Message);
                return 1;
            }
        }

        private static int RunCompileCommand(string[] args)
        {
            ConsoleWriteLineS($"Compiling \"{Path.GetFileName(args[1])}\" to {args[3]}...");

            string compileMsgs = "";

            string inCFile = args[1];
            string outZovl = args[2];

            string linkerFiles;

            if (args.Length > 3 && !args[3].Equals("none", StringComparison.OrdinalIgnoreCase))
                linkerFiles = $"{Program.Settings.LinkerPaths};{args[3]}";
            else
                linkerFiles = Program.Settings.LinkerPaths;

            string compileFlags = args.Length > 4 ? args[4].Trim('"') : string.Empty;

            List<CSymbol> symbols = null;
            byte[] res = CCode.Compile(inCFile,
                          linkerFiles,
                          outZovl, 
                          compileFlags, 
                          ref compileMsgs, 
                          out symbols);

            if (res == null)
                return 1;

            if (symbols != null)
            {
                CSymbol c = symbols.FirstOrDefault(x => x.Symbol.Equals("sNpcMakerInit", StringComparison.InvariantCultureIgnoreCase))
                         ?? symbols.FirstOrDefault(x => x.Symbol.Equals("sActorVars", StringComparison.InvariantCultureIgnoreCase));

                if (c != null)
                {
                    string config = $"alloc_type = 0\nvram_addr = 0x{CCode.BaseAddr.ToString("X")}\ninit_vars = 0x{(CCode.BaseAddr + c.Addr).ToString("X")}";
                    System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(args[3]), "config.toml"), config);
                }
            }

            if (!Program.IsRunningUnderMono)
                Console.WriteLine("Press ENTER to exit...");

            return 0;
        }

        private static int RunConvertCommand(string[] args)
        {
            const string Skip = "skip";

            // Returns the argument at the given index, or null if missing or "skip".
            string Arg(int i) => i < args.Length && args[i] != Skip ? args[i] : null;

            NPCFile inFile = null;
            string headerPathsHash = "";
            bool success;

            try
            {
                JsonPath = args[0];
                string outPath = Arg(1);
                string outDeps = Arg(2);
                string outH = Arg(3);

                if (outPath == null && outH == null)
                {
                    Console.WriteLine("Nothing to do.");
                    return 0;
                }

                if (outPath == null && outDeps != null)
                {
                    Console.WriteLine("Cannot generate deps without outPath.");
                    return 1;
                }

                inFile = FileOps.ParseNPCJsonFile(JsonPath);
                StringBuilder sb = new StringBuilder();

                foreach (var entry in inFile.Entries)
                {
                    foreach (var path in entry.EmbeddedOverlayCode.HeaderPaths)
                        sb.Append(path);
                }

                headerPathsHash = Helpers.GetBase64Hash(sb.ToString());

                Dicts.LoadDicts();
                Dicts.ReloadLanguages(inFile.Languages);
                Program.Settings.GameVersion = inFile.GameVersion;

                // Header-only output
                if (outPath == null)
                {
                    ConsoleWriteLineS($"Saving \"{Path.GetFileName(JsonPath)}\" to header...");
                    File.WriteAllText(outH, FileOps.CreateNPCHFile(inFile, outPath));
                    return 0;
                }

                ConsoleWriteLineS($"Saving \"{Path.GetFileName(JsonPath)}\" to binary...");

                var cacheStatus = FileOps.GetCacheStatus(ref inFile);

                success = Program.Settings.CompileInParallel
                    ? RunParallelCompile(outPath, outDeps, outH, cacheStatus, inFile)
                    : RunSequentialCompile(outPath, outDeps, outH, cacheStatus, ref inFile);
            }
            catch (Exception ex) when (inFile == null)
            {
                Console.WriteLine($"Error reading input JSON: {ex.Message}");
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing output: {ex.Message}");
                return 1;
            }

            if (success)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var entry in inFile.Entries)
                {
                    foreach (var path in entry.EmbeddedOverlayCode.HeaderPaths)
                        sb.Append(path);
                }

                string headerPathsHashNew = Helpers.GetBase64Hash(sb.ToString());

                if (headerPathsHash != headerPathsHashNew)
                    success = FileOps.SaveNPCJSON(JsonPath, inFile);
            }

            if (!Program.IsRunningUnderMono)
                Console.WriteLine("Press ENTER to exit...");

            return success ? 0 : 1;
        }

        private static bool RunParallelCompile(string outputPath, string outputDepsPath, string outputHPath, Common.CacheStatus cacheStatus, NPCFile inFile)
        {
            Program.CompileInProgress = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            var res = FileOps.PreprocessCodeAndScripts(outputPath, outputDepsPath, outputHPath, inFile, cacheStatus, null, true);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            while (Program.CompileInProgress) {}
            return res.Result;
        }

        private static bool RunSequentialCompile(string outputPath, string outputDepsPath, string outputHPath, Common.CacheStatus cacheStatus, ref NPCFile inFile)
        {
            bool res = false;
            var baseDefines = Scripts.ScriptHelpers.GetBaseDefines(inFile);

            res = FileOps.SaveBinaryFile(outputPath, outputDepsPath, outputHPath, ref inFile, null, baseDefines, cacheStatus, null, true);
            CCode.CleanupStandardCompilationArtifacts();
            return res;
        }

        private static int PrintUsage()
        {
            Console.WriteLine("Usage: \"NPC Maker.exe\" InputJson [OutputZobj|skip] [OutputDeps|skip] [OutputH|skip] [--silent] [--extsettings=Path]");
            Console.WriteLine("Usage to compile C: \"NPC Maker.exe\" -c InputCFile OutputZovl [ExtraLinkerFiles|none] [\"COMPILEFLAGS\"] [--silent] [--extsettings=Path]");
            Console.WriteLine("Usage to make msgtable: \"NPC Maker.exe\" -m InputJson InputActorId OutputTable OutputStrings [--silent] [--extsettings=Path]");
            Console.WriteLine("Press ENTER to exit...");
            return 1;
        }

        public static void ConsoleWriteLineS(string s = "")
        {
            if (!consoleSilent) Console.WriteLine(s);
        }

        public static void ConsoleWriteS(string s = "")
        {
            if (!consoleSilent) Console.Write(s);
        }
    }
}
