using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionStop : Instruction
    {
         ScriptVarVal StopID { get; set; }
         byte SubID { get; set; }

        public InstructionStop(byte _SubID, ScriptVarVal _StopID) : base((byte)Lists.Instructions.STOP)
        {
            SubID = _SubID;
            StopID = _StopID;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(StopID.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(StopID.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);
            return Data.ToArray();
        }
    }
}

