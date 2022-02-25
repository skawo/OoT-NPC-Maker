using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionScript : Instruction
    {
         ScriptVarVal ScriptID { get; set; }
         byte SubID { get; set; }

        public InstructionScript(byte _SubID, ScriptVarVal _ScriptID) : base((byte)Lists.Instructions.SCRIPT)
        {
            SubID = _SubID;
            ScriptID = _ScriptID;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ScriptID.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(ScriptID.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);
            return Data.ToArray();
        }
    }
}

