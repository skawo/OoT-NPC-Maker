using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class BScript
    {
        public List<byte> Script { get; set; }
        public List<string> ScriptDebug { get; set; }
        public List<ParseException> ParseErrors { get; set;}

        public BScript()
        {
            Script = new List<byte>();
            ScriptDebug = new List<string>();
            ParseErrors = new List<ParseException>();
        }
    }
   
}
