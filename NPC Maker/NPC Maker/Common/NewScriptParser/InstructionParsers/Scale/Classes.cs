using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionScale : InstructionSub
    {
        public UInt32 ActorCat;
        public UInt32 ActorID;
        public float Scale;
        public byte ScaleType;
        public byte ActorIDT;
        public byte ActorCatT;

        public InstructionScale(byte _SubID, UInt32 _ActorID, byte _ActorIDT, UInt32 _ActorCat, byte _ActorCatT, float _Scale, byte _ScaleType)
                                : base((int)Lists.Instructions.SCALE, _SubID)
        {
            ActorCat = _ActorCat;
            ActorID = _ActorID;
            ActorCatT = _ActorCatT;
            ActorIDT = _ActorIDT;

            Scale = _Scale;
            ScaleType = _ScaleType;

        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ActorIDT, ActorCatT, 4), Data);
            DataHelpers.AddObjectToByteList(ScaleType, Data);

            DataHelpers.AddObjectToByteList(ActorID, Data);
            DataHelpers.AddObjectToByteList(ActorCat, Data);
            DataHelpers.AddObjectToByteList(Scale, Data);
            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }
    }
}

