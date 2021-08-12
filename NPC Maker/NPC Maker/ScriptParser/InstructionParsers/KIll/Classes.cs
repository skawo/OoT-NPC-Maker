using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionKill : Instruction
    {
        ScriptVarVal ActorID { get; set; }
        public byte SubID;

        public InstructionKill(byte _SubID, ScriptVarVal _ActorID) : base((byte)Lists.Instructions.KILL)
        {
            ActorID = _ActorID;
            SubID = _SubID;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ActorID.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(ActorID.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }
    }
}

