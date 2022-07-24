using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionFade : Instruction
    {
        ScriptVarVal R;
        ScriptVarVal G;
        ScriptVarVal B;
        ScriptVarVal Rate;


        public InstructionFade(byte ID, ScriptVarVal _R, ScriptVarVal _G, ScriptVarVal _B, ScriptVarVal _Rate) : base(ID)
        {
            R = _R;
            G = _G;
            B = _B;
            Rate = _Rate;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(R.Vartype, G.Vartype, 4), Data);
            Helpers.AddObjectToByteList(B.Vartype, Data);
            Helpers.AddObjectToByteList(Rate.Vartype, Data);
            Helpers.AddObjectToByteList(R.Value, Data);
            Helpers.AddObjectToByteList(G.Value, Data);
            Helpers.AddObjectToByteList(B.Value, Data);
            Helpers.AddObjectToByteList(Rate.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 20);

            return Data.ToArray();
        }
    }
}
