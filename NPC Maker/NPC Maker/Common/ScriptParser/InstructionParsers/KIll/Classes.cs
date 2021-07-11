using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionKill : Instruction
    {
        public UInt32 ActorID;
        public Int32 ActorCat;
        public byte AIDVarT;
        public byte ACatVarT;
        public byte SubID;

        public InstructionKill(byte _SubID, UInt32 _ActorID, byte _AIDVarT, Int32 _ActorCat, byte _ACatVarT) : base((byte)Lists.Instructions.KILL)
        {
            ActorID = _ActorID;
            ActorCat = _ActorCat;
            AIDVarT = _AIDVarT;
            ACatVarT = _ACatVarT;
            SubID = _SubID;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(AIDVarT, Data);
            Helpers.AddObjectToByteList(ACatVarT, Data);
            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(ActorCat, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

