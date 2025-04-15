using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Common
{ 
    public class ProgressReport
    {
        public string Status;
        public float Value;

        public ProgressReport(string status, float val)
        {
            Status = status;
            Value = val;
        }
    }
}
