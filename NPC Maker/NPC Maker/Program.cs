﻿using System;
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

        //public static FileSystemWatcher Watcher;
        public static Process CodeEditorProcess;

        public static string SettingsFilePath;
        public static NPCMakerSettings Settings;
        public static MainWindow mw;

        public static string ScriptCachePath = "";
        public static string CCachePath = "";
        public static string JsonPath = "";

        public static bool CompileInProgress = false;
        public static bool CompileThereWereErrors = false;
        public static string CompileMonoErrors = "";
        public static DateTime CompileStartTime;

        public static WeCantSpell.Hunspell.WordList dictionary;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CultureInfo ci = CultureInfo.GetCultureInfo("en-US");
            Application.CurrentCulture = ci;

            Program.ExecPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            SettingsFilePath = Path.Combine(ExecPath, "Settings.json");
            ScriptCachePath = Path.Combine(ExecPath, "cache", "s_cache");
            CCachePath = Path.Combine(ExecPath, "cache", "c_cache");

            // To create this in memory quicker
            TaskEx.Run(() => { ZeldaMessage.MessagePreview p = new ZeldaMessage.MessagePreview(ZeldaMessage.Data.BoxType.Black, new byte[0]); });

            Type t = Type.GetType("Mono.Runtime");

            if (t != null)
                IsRunningUnderMono = true;

            try
            {
                DropDownMenuScrollWheelHandler.Enable(true);
            }
            catch (Exception)
            {

            }

            Settings = FileOps.ParseSettingsJSON(SettingsFilePath);

            if (File.Exists("dict.dic"))
                dictionary = WeCantSpell.Hunspell.WordList.CreateFromFiles("dict.dic");
            else
                dictionary = WeCantSpell.Hunspell.WordList.CreateFromWords(new List<string>() { });


            CCode.CreateCTempDirectory("", "", false);

            if (!Directory.Exists(ScriptCachePath))
                Directory.CreateDirectory(ScriptCachePath);

            if (!Directory.Exists(CCachePath))
                Directory.CreateDirectory(CCachePath);

            Dicts.LoadDicts();


            if (args.Length == 0)
            {
                string fileToOpen = "";

                if (File.Exists("backup"))
                {
                    if (MessageBox.Show("NPCMaker was not closed properly the last time it was run. Load auto-saved backup?", "Autosaved backup exists", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                        fileToOpen = "backup";
                }

                mw = new MainWindow(fileToOpen);
                Application.Run(mw);

                FileOps.SaveSettingsJSON(SettingsFilePath, Program.Settings);
            }
            else
            {
                if (!IsRunningUnderMono)
                    AttachConsole(-1);

                Console.WriteLine();

                if (args[0].ToUpper() == "/?" || args[0].ToUpper() == "-HELP" || args.Length != 2)
                {
                    Console.WriteLine($"Zelda Ocarina of Time NPC Creation Tool v.{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}");
                    Console.WriteLine("Usage: \"NPC Maker.exe\" [InputJson] [OutputZobj]");
                    Console.WriteLine("Press ENTER to exit...");
                }
                else if (args.Length == 2)
                {
                    NPCFile InFile = null;

                    try
                    {
                        InFile = FileOps.ParseNPCJsonFile(args[0]);
                        Program.JsonPath = args[0];
                        Dicts.LoadDicts();
                        Dicts.ReloadMsgTagOverrides(InFile.Languages);
                        Program.Settings.GameVersion = InFile.GameVersion;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading input JSON:" + ex.Message);
                    }

                    try
                    {
                        Console.WriteLine($"NPC Maker: Saving \"{Path.GetFileName(args[0])}\" to binary...");

                        if (Program.Settings.CompileInParallel)
                        {
                            Program.CompileInProgress = true;

                            FileOps.PreprocessCodeAndScripts(args[1], InFile, null);

                            while (Program.CompileInProgress)
                            {
                                ;
                            }
                        }
                        else
                        {
                            bool[] cacheStatus = FileOps.GetCacheStatus(InFile, true);

                            if (cacheStatus != null)
                            {
                                string baseDefines = Scripts.ScriptHelpers.GetBaseDefines(InFile);
                                FileOps.SaveBinaryFile(args[1], InFile, null, baseDefines, cacheStatus[0], cacheStatus[1], true);
                                CCode.CleanupCompileArtifacts();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error writing output:" + ex.Message);
                    }

                    Console.WriteLine("Press ENTER to exit...");
                }
            }

        }
    }
}
