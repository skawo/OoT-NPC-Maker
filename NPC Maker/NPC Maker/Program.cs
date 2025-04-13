using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        public static string CachePath = "";
        public static string CCachePath = "";
        public static string JsonPath = "";

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
            CachePath = Path.Combine(ExecPath, "cache");
            CCachePath = Path.Combine(ExecPath, "Ccache");

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

            CCode.CreateCTempDirectory("", "", false);

            if (!Directory.Exists(CachePath))
                Directory.CreateDirectory(CachePath);

            if (!Directory.Exists(CCachePath))
                Directory.CreateDirectory(CCachePath);

            Dicts.LoadDicts();


            if (args.Length == 0)
            {
                mw = new MainWindow();
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading input JSON:" + ex.Message);
                    }

                    try
                    {
                        Console.WriteLine("Writing output ZOBJ...");
                        FileOps.SaveBinaryFile(args[1], InFile, true);
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
