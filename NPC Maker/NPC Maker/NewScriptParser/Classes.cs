using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class bScript
    {
        public List<byte> Script { get; set; }
        public List<ParseException> ParseErrors { get; set;}

        public bScript()
        {
            Script = new List<byte>();
            ParseErrors = new List<ParseException>();
        }
    }
   
}
