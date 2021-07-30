using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionKill : Instruction
    {
        public object ActorID;
        public byte AIDVarT;
        public byte SubID;

        public InstructionKill(byte _SubID, object _ActorID, byte _AIDVarT) : base((byte)Lists.Instructions.KILL)
        {
            ActorID = _ActorID;
            AIDVarT = _AIDVarT;
            SubID = _SubID;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(AIDVarT, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }
    }
}

