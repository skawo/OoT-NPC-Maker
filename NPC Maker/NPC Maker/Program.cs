using Newtonsoft.Json;
using NPC_Maker.Controls;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC_Maker
{
    static class Program
    {
        public static MiscUtil.Conversion.BigEndianBitConverter BEConverter = new MiscUtil.Conversion.BigEndianBitConverter();
        public static string ExecPath = "";
        public static bool IsRunningUnderMono = false;
        public static bool IsWSL = false;

        public static Process CodeEditorProcess;

        public static string SettingsFilePath;
        public static NPCMakerSettings Settings;
        public static MainWindow mw;

        public static string ScriptCachePath = "";
        public static string CCachePath = "";
        public static string JsonPath = "";
        public static string AutoSavePath = "";

        public static bool SaveInProgress = false;
        public static bool CompileInProgress = false;
        public static bool CompileThereWereErrors = false;
        public static string CompileMonoErrors = "";

        public static Dictionary<string, WeCantSpell.Hunspell.WordList> dictionary;

        public static bool consoleSilent = false;
        public static readonly Encoding Utf8 = Encoding.UTF8;
        public static readonly DataTable _sharedTable = new DataTable();
        public static Stopwatch _stopWatch;
        public static readonly Random _random = new Random();

        public static List<string> Monofonts = null;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        [STAThread]
        static void Main(string[] args)
        {
            DetectRuntime();
            Application.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            bool hasArgs = args.Length > 0;

            if (hasArgs)
            {
                SetupConsole(args);
                PrintBanner();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!hasArgs)
                TaskEx.Run(() => GetMonospacedFonts());

            InitializePaths();
            EnsureDirectoriesExist();
            LoadSettings();

            if (!hasArgs)
            {
                // Create this in memory, so it gets cached.
                TaskEx.Run(() => new ZeldaMessage.MessagePreview(ZeldaMessage.Data.BoxType.Black, new byte[0]));

                try
                {
                    DropDownMenuScrollWheelHandler.Enable(true);
                }
                catch { }

                RunGUI();
            }
            else
                RunCLI(args);
        }

        private static void DetectRuntime()
        {
            Type monoType = Type.GetType("Mono.Runtime");
            if (monoType == null) return;

            IsRunningUnderMono = true;
            IsWSL = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
        }

        private static void SetupConsole(string[] args)
        {
            if (args.Contains("--silent"))
                consoleSilent = true;

            if (!IsRunningUnderMono)
                AttachConsole(-1);
        }

        private static void PrintBanner()
        {
            var version = FileVersionInfo
                .GetVersionInfo(Assembly.GetExecutingAssembly().Location)
                .ProductVersion;

            ConsoleWriteLineS();
            ConsoleWriteLineS($"Zelda Ocarina of Time NPC Creation Tool v{version}");
        }

        private static void GetMonospacedFonts()
        {
            InstalledFontCollection fontsCollection = new InstalledFontCollection();
            var fonts = fontsCollection.Families;
            List<string> fnts = new List<string>();

            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    foreach (FontFamily fontFamily in fonts)
                    {
                        if (fontFamily.IsMonospaced(g))
                            fnts.Add(fontFamily.Name);
                    }
                }
            }

            Monofonts = fnts;
        }

        private static void InitializePaths()
        {
            ExecPath = Path.GetDirectoryName(Application.ExecutablePath);
            ScriptCachePath = Path.Combine(ExecPath, "cache", "s_cache");
            CCachePath = Path.Combine(ExecPath, "cache", "c_cache");
            AutoSavePath = Path.Combine(ExecPath, "autosave");

            string settingsWindows = Path.Combine(ExecPath, "Settings.json");
            string settingsMono = Path.Combine(ExecPath, "SettingsMono.json");

            if (IsRunningUnderMono)
            {
                if (!File.Exists(settingsMono) && File.Exists(settingsWindows))
                    File.Copy(settingsWindows, settingsMono);

                SettingsFilePath = settingsMono;
            }
            else
            {
                SettingsFilePath = settingsWindows;
            }
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

        private static void RunGUI()
        {
            string fileToOpen = GetInitialFileToOpen();

            while (true)
            {
                mw = new MainWindow(fileToOpen);
                mw.Shown += (s, e) => 
                { 
                    mw.Activate(); 
                    mw.BringToFront(); 
                };

                Application.Run(mw);

                FileOps.SaveSettingsJSON(SettingsFilePath, Program.Settings);

                if (mw.DialogResult != DialogResult.Retry)
                    break;

                fileToOpen = mw.OpenedPath;
            }
        }

        private static string GetInitialFileToOpen()
        {
            bool hasBackup = File.Exists("backup");
            if (!hasBackup) return "";

            bool loadBackup = BigMessageBox.Show(
                "NPCMaker was not closed properly the last time it was run. Load auto-saved backup?",
                "Autosaved backup exists",
                MessageBoxButtons.YesNo) == DialogResult.Yes;

            return loadBackup ? "backup" : "";
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

            string compileFlags = args.Length == 5 ? args[4].Trim('"') : "";
            string compileMsgs = "";

            CCode.Compile(args[1], args[2], args[3], compileFlags, ref compileMsgs);
            ConsoleWriteLineS("Press ENTER to exit...");
        }

        private static void RunConvertCommand(string[] args)
        {
            NPCFile inFile = null;

            try
            {
                inFile = FileOps.ParseNPCJsonFile(args[0]);
                Program.JsonPath = args[0];

                Dicts.LoadDicts();
                Dicts.ReloadLanguages(inFile.Languages);
                Program.Settings.GameVersion = inFile.GameVersion;

                ConsoleWriteLineS($"Saving \"{Path.GetFileName(args[0])}\" to binary...");

                var cacheStatus = FileOps.GetCacheStatus(ref inFile);

                if (Program.Settings.CompileInParallel && (cacheStatus.CacheInvalid || cacheStatus.CCacheInvalid))
                    RunParallelCompile(args[1], cacheStatus, inFile);
                else
                    RunSequentialCompile(args[1], cacheStatus, ref inFile);
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

            FileOps.SaveNPCJSON(args[0], inFile);
            ConsoleWriteLineS("Press ENTER to exit...");
        }

        private static void RunParallelCompile(string outputPath, Common.CacheStatus cacheStatus, NPCFile inFile)
        {
            Program.CompileInProgress = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            FileOps.PreprocessCodeAndScripts(outputPath, inFile, cacheStatus, null, true);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            while (Program.CompileInProgress) {}
        }

        private static void RunSequentialCompile(string outputPath, Common.CacheStatus cacheStatus, ref NPCFile inFile)
        {
            var baseDefines = Scripts.ScriptHelpers.GetBaseDefines(inFile);

            FileOps.SaveBinaryFile(outputPath, ref inFile, null, baseDefines, cacheStatus, null, true);
            CCode.CleanupStandardCompilationArtifacts();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: \"NPC Maker.exe\" [InputJson] [OutputZobj] [-silent]");
            Console.WriteLine("Usage to compile C: \"NPC Maker.exe\" -c [InputCFile] [InputLinkerFile] [OutputZovl] [\"COMPILEFLAGS\"]");
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
