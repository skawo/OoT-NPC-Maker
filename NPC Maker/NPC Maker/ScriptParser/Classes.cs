using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class BScript
    {
        public byte[] Script { get; set; }
        public List<string> ScriptDebug { get; set; }
        public List<ParseException> ParseErrors { get; set; }

        public BScript()
        {
            Script = new byte[0];
            ScriptDebug = new List<string>();
            ParseErrors = new List<ParseException>();
        }
    }

    public class ScriptVarVal
    {
        public byte Vartype { get; set; }
        public object Value { get; set; }

        public ScriptVarVal(int _Val, byte _Var)
        {
            Vartype = _Var;
            Value = _Val;
        }
    }

}
