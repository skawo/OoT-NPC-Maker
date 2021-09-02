using System;
using System.Windows.Forms;

namespace NPC_Maker
{
    static class Program
    {
        public static MiscUtil.Conversion.BigEndianBitConverter BEConverter = new MiscUtil.Conversion.BigEndianBitConverter();
        public static string ExecPath = "";
        public static bool IsRunningUnderMono = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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

            Application.Run(new MainWindow());
        }
    }
}
