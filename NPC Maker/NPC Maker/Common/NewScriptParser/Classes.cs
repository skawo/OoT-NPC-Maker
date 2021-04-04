using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class BScript
    {
        public byte[] Script { get; set; }
        public List<string> ScriptDebug { get; set; }
        public List<ParseException> ParseErrors { get; set;}

        public BScript()
        {
            Script = new byte[0];
            ScriptDebug = new List<string>();
            ParseErrors = new List<ParseException>();
        }
    }

    public class Procedure
    {
        public string Name { get; set; }
        public List<string> Instructions { get; set; }

        public Procedure(string _Name, List<string> _Instructions)
        {
            Name = _Name;
            Instructions = _Instructions;
        }
    }
   
}
