using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Windows
{
    public partial class ProgressWithLabel : UserControl
    {
        public Common.ProgressReport NewProgress
        {
            set { SetProgress((int)Math.Ceiling(value.Value), value.Status); }
        }

        public ProgressWithLabel()
        {
            InitializeComponent();
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 100;
        }

        public void SetProgress(int Progress, string Msg = "")
        {
            if (Progress > 100)
                Progress = 100;

            if (Progress + 1 <= progressBar.Maximum)
                progressBar.Value = Progress + 1;

            progressBar.Value = Progress;

            if (Msg != "")
                labelProgress.Text = Msg;

            progressBar.Update();
            labelProgress.Update();
        }

        public int GetProgress()
        {
            return progressBar.Value;
        }

    }
}
