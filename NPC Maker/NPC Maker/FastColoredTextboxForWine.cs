using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace NPC_Maker
{
    public partial class FastColoredTextboxForWine : FastColoredTextBox
    {
        public FastColoredTextboxForWine()
        {
            InitializeComponent();
        }

        public FastColoredTextboxForWine(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                base.OnMouseWheel(e);
            }
            catch (Exception)
            {
                try
                {
                    if (e.Delta < 0)
                        for (int i = 0; i < Math.Abs(e.Delta / 30); i++)
                            SendMessage(this.Handle, 0x115, (IntPtr)1, IntPtr.Zero);
                    else
                        for (int i = 0; i < Math.Abs(e.Delta / 30); i++)
                            SendMessage(this.Handle, 0x115, (IntPtr)0, IntPtr.Zero);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
