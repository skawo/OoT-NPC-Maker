using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC_Maker
{
    public class CustomDataGridView : System.Windows.Forms.DataGridView
    {
        public CustomDataGridView()
        {
            this.DoubleBuffered = true;
        }

        public CustomDataGridView(IContainer container)
        {
            container.Add(this);
        }
    }
}
