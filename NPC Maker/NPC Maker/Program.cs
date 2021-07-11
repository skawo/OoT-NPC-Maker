using System;
using System.Windows.Forms;

namespace NPC_Maker
{
    static class Program
    {
        public static MiscUtil.Conversion.BigEndianBitConverter BEConverter = new MiscUtil.Conversion.BigEndianBitConverter();
        public static string ExecPath = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Program.ExecPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

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
