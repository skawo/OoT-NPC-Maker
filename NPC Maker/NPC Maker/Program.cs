using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace NPC_Maker
{
    static class Program
    {
        public static MiscUtil.Conversion.BigEndianBitConverter BEConverter = new MiscUtil.Conversion.BigEndianBitConverter();
        public static string ExecPath = "";
        public static bool IsRunningUnderMono = false;

        public static FileSystemWatcher Watcher;
        public static Process CodeEditorProcess;

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

            if (args.Length == 0)
                Application.Run(new MainWindow());
            else
            {
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
                        InFile = FileOps.ParseJSONFile(args[0]);
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
