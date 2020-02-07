using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPC_Maker
{
    public partial class CustomDataGridView : System.Windows.Forms.DataGridView
    {
        public CustomDataGridView()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public CustomDataGridView(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
