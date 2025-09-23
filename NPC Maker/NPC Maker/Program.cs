using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace NPC_Maker
{
    static class Program
    {
        public static MiscUtil.Conversion.BigEndianBitConverter BEConverter = new MiscUtil.Conversion.BigEndianBitConverter();
        public static string ExecPath = "";
        public static bool IsRunningUnderMono = false;
        public static bool IsWSL = false;

        //public static FileSystemWatcher Watcher;
        public static Process CodeEditorProcess;

        public static string SettingsFilePath;
        public static NPCMakerSettings Settings;
        public static MainWindow mw;

        public static string ScriptCachePath = "";
        public static string CCachePath = "";
        public static string JsonPath = "";

        public static bool SaveInProgress = false;
        public static bool CompileInProgress = false;
        public static bool CompileThereWereErrors = false;
        public static string CompileMonoErrors = "";
        public static DateTime CompileStartTime;

        public static Dictionary<string, WeCantSpell.Hunspell.WordList> dictionary;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Type t = Type.GetType("Mono.Runtime");

            if (t != null)
            {
                IsRunningUnderMono = true;
                IsWSL = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
            }

            if (args.Length != 0)
            {
                if (!IsRunningUnderMono)
                    AttachConsole(-1);

                var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
                Console.WriteLine();
                Console.WriteLine($"Zelda Ocarina of Time NPC Creation Tool v.{version}");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CultureInfo ci = CultureInfo.GetCultureInfo("en-US");
            Application.CurrentCulture = ci;

            Program.ExecPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            string settingsWindows = Path.Combine(ExecPath, "Settings.json");
            string settingsMono = Path.Combine(ExecPath, "SettingsMono.json");

            if (Program.IsRunningUnderMono)
            {
                if (!File.Exists(settingsMono) && File.Exists(settingsWindows))
                    File.Copy(settingsWindows, settingsMono);

                SettingsFilePath = settingsMono;
            }
            else
                SettingsFilePath = settingsWindows;

            ScriptCachePath = Path.Combine(ExecPath, "cache", "s_cache");
            CCachePath = Path.Combine(ExecPath, "cache", "c_cache");

            // To create this in memory quicker
            TaskEx.Run(() => { ZeldaMessage.MessagePreview p = new ZeldaMessage.MessagePreview(ZeldaMessage.Data.BoxType.Black, new byte[0]); });

            try
            {
                DropDownMenuScrollWheelHandler.Enable(true);
            }
            catch (Exception)
            {

            }

            Settings = FileOps.ParseSettingsJSON(SettingsFilePath);

            CCode.CreateCTempDirectory("", "", false);

            if (!Directory.Exists(ScriptCachePath))
                Directory.CreateDirectory(ScriptCachePath);

            if (!Directory.Exists(CCachePath))
                Directory.CreateDirectory(CCachePath);

            Dicts.LoadDicts();
            Dicts.ReoadSpellcheckDicts(new List<string>());


            if (args.Length == 0)
            {
                var fileToOpen = File.Exists("backup") &&
                                MessageBox.Show("NPCMaker was not closed properly the last time it was run. Load auto-saved backup?",
                                              "Autosaved backup exists", MessageBoxButtons.YesNo) == DialogResult.Yes
                                ? "backup" : "";

                mw = new MainWindow(fileToOpen);
                Application.Run(mw);
                FileOps.SaveSettingsJSON(SettingsFilePath, Program.Settings);
            }
            else
            {
                var isValidCompileCommand = args.Length >= 4 && args[0].ToUpper() == "-C" && args.Length <= 5;
                var isValidConvertCommand = args.Length == 2;

                if (isValidCompileCommand)
                {
                    Console.WriteLine($"Compiling \"{Path.GetFileName(args[1])}\" to {args[3]}...");
                    var compileFlags = args.Length == 5 ? args[4].Trim('"') : "";
                    string compileMsgs = "";
                    CCode.Compile(args[1], args[2], args[3], compileFlags, ref compileMsgs);
                    Console.WriteLine("Press ENTER to exit...");
                    return;
                }
                else if (isValidConvertCommand)
                {
                    NPCFile inFile = null;

                    try
                    {
                        inFile = FileOps.ParseNPCJsonFile(args[0]);
                        Program.JsonPath = args[0];
                        Dicts.LoadDicts();
                        Dicts.ReloadMsgTagOverrides(inFile.Languages);
                        Program.Settings.GameVersion = inFile.GameVersion;

                        Console.WriteLine($"Saving \"{Path.GetFileName(args[0])}\" to binary...");

                        if (Program.Settings.CompileInParallel)
                        {
                            Program.CompileInProgress = true;
                            FileOps.PreprocessCodeAndScripts(args[1], inFile, null);

                            while (Program.CompileInProgress) { /* Wait for completion */ }
                        }
                        else
                        {
                            var cacheStatus = FileOps.GetCacheStatus(inFile, true);
                            if (cacheStatus != null)
                            {
                                var baseDefines = Scripts.ScriptHelpers.GetBaseDefines(inFile);
                                FileOps.SaveBinaryFile(args[1], inFile, null, baseDefines, cacheStatus[0], cacheStatus[1], null, true);
                                CCode.CleanupStandardCompilationArtifacts();
                            }
                        }
                    }
                    catch (Exception ex) when (inFile == null)
                    {
                        Console.WriteLine($"Error reading input JSON: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error writing output: {ex.Message}");
                    }

                    Console.WriteLine("Press ENTER to exit...");
                }
                else
                {
                    Console.WriteLine("Usage: \"NPC Maker.exe\" [InputJson] [OutputZobj]");
                    Console.WriteLine("Usage to compile C: \"NPC Maker.exe\" -c [InputCFile] [InputLinkerFile] [OutputZovl] [\"COMPILEFLAGS\"]");
                    Console.WriteLine("Press ENTER to exit...");
                }
            }


        }
    }
}
