using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(AIDVarT, Data);
            DataHelpers.AddObjectToByteList(ACatVarT, Data);
            DataHelpers.AddObjectToByteList(ActorID, Data);
            DataHelpers.AddObjectToByteList(ActorCat, Data);
            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

