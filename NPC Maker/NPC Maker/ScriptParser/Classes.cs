using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class BScript
    {
        public byte[] Script { get; set; }
        public List<string> ScriptDebug { get; set; }
        public List<ParseException> ParseErrors { get; set; }

        public string Name;

        public BScript()
        {
            Script = new byte[0];
            ScriptDebug = new List<string>();
            ParseErrors = new List<ParseException>();
            Name = "";
        }
    }

    public class ScriptVarVal
    {
        public byte Vartype { get; set; }
        public object Value { get; set; }

        public ScriptVarVal(float _Val = 0, byte _Var = (byte)Lists.VarTypes.NORMAL)
        {
            Vartype = _Var;
            Value = _Val;
        }
    }

}
