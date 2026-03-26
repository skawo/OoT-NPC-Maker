using Microsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NPC_Maker
{
    static class Program
    {
        public static string ExecPath = "";
        public static bool IsRunningUnderMono = false;
        public static bool IsWSL = false;

        public static Process CodeEditorProcess;

        public static string SettingsFilePath;
        public static NPCMakerSettings Settings;

        public static string ScriptCachePath = "";
        public static string CCachePath = "";
        public static string JsonPath = "";
        public static string AutoSavePath = "";

        public static bool SaveInProgress = false;
        public static bool CompileInProgress = false;
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
        static void Main(string[] args)
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

            RunCLI(args);
        }

        private static void DetectRuntime()
        {
            IsRunningUnderMono = true;
            IsWSL = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
        }

        private static void SetupConsole(ref string[] args)
        {
            if (args.Any(a => a.Equals("--silent", StringComparison.OrdinalIgnoreCase)))
            {
                consoleSilent = true;
                args = args.Where(a => !a.Equals("--silent", StringComparison.OrdinalIgnoreCase)).ToArray();
            }
        }

        private static void PrintBanner()
        {
            ConsoleWriteLineS();
            ConsoleWriteLineS($"Zelda Ocarina of Time NPC Creation Tool v. TEMPORARY CLI NET");
        }

        private static void InitializePaths()
        {
            ExecPath = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location
            ) ?? AppContext.BaseDirectory;

            ScriptCachePath = Path.Combine(ExecPath, "cache", "s_cache");
            CCachePath = Path.Combine(ExecPath, "cache", "c_cache");
            AutoSavePath = Path.Combine(ExecPath, "autosave");
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

        private static void RunCLI(string[] args)
        {
            bool isCompileCommand = args.Length >= 4 && args.Length <= 5 && args[0].ToUpper() == "-C";
            bool isConvertCommand = args.Length >= 2;

            if (isCompileCommand)
                RunCompileCommand(args);
            else if (isConvertCommand)
                RunConvertCommand(args);
            else
                PrintUsage();
        }

        private static void RunCompileCommand(string[] args)
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
            CCode.Compile(inCFile,
                          linkerFiles,
                          outZovl, 
                          compileFlags, 
                          ref compileMsgs, 
                          out symbols);

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
        }

        private static void RunConvertCommand(string[] args)
        {
            NPCFile inFile = null;
            string jsonText = "";

            try
            {
                JsonPath = args[0];
                string outPath = args[1];
                string outDeps = args.Length > 2 ? args[2] : null;

                jsonText = File.ReadAllText(JsonPath);
                inFile = FileOps.ParseNPCJsonFile("", jsonText);

                Dicts.LoadDicts();
                Dicts.ReloadLanguages(inFile.Languages);
                Program.Settings.GameVersion = inFile.GameVersion;

                ConsoleWriteLineS($"Saving \"{Path.GetFileName(args[0])}\" to binary...");

                var cacheStatus = FileOps.GetCacheStatus(ref inFile);

                if (Program.Settings.CompileInParallel)
                    RunParallelCompile(outPath, outDeps, cacheStatus, inFile);
                else
                    RunSequentialCompile(outPath, outDeps, cacheStatus, ref inFile);
            }
            catch (Exception ex) when (inFile == null)
            {
                Console.WriteLine($"Error reading input JSON: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing output: {ex.Message}");
                return;
            }

            string newJson = FileOps.ProcessNPCJSON(ref inFile);

            if (!String.Equals(jsonText, newJson)) 
                FileOps.SaveNPCJSON(args[0], inFile, null, newJson);

            if (!Program.IsRunningUnderMono)
                Console.WriteLine("Press ENTER to exit...");
        }

        private static void RunParallelCompile(string outputPath, string outputDepsPath, Common.CacheStatus cacheStatus, NPCFile inFile)
        {
            FileOps.PreprocessCodeAndScripts(outputPath, outputDepsPath, inFile, cacheStatus, null, true);
        }

        private static void RunSequentialCompile(string outputPath, string outputDepsPath, Common.CacheStatus cacheStatus, ref NPCFile inFile)
        {
            var baseDefines = Scripts.ScriptHelpers.GetBaseDefines(inFile);

            FileOps.SaveBinaryFile(outputPath, outputDepsPath, ref inFile, null, baseDefines, cacheStatus, null, true);
            CCode.CleanupStandardCompilationArtifacts();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: \"NPC Maker.exe\" InputJson OutputZobj [OutputDeps] [--silent]");
            Console.WriteLine("Usage to compile C: \"NPC Maker.exe\" -c InputCFile OutputZovl [ExtraLinkerFiles|none] [\"COMPILEFLAGS\"] [--silent]");
            Console.WriteLine("Press ENTER to exit...");
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
