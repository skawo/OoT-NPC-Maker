using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class Progress : Form
    {
        public Progress()
        {
            InitializeComponent();
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = 100;
            
        }

        public void SetProgress(int Progress, string Msg = "")
        {
            if (Progress > 100)
                Progress = 100;

            if (Progress + 1 <= progressBar1.Maximum)
                progressBar1.Value = Progress + 1;

            progressBar1.Value = Progress;

            if (Msg != "")
                label1.Text = Msg;

            progressBar1.Update();
            label1.Update();
        }

        public int GetProgress()
        {
            return progressBar1.Value;
        }
    }
}
