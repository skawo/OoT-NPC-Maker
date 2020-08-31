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
    
    public class Label
    {
        public string Name { get; set; }
        public int InstructionNumber { get; set; }

        public Label(string _Name, int _InstructionNumber)
        {
            Name = _Name;
            InstructionNumber = _InstructionNumber;
        }
    }
}
