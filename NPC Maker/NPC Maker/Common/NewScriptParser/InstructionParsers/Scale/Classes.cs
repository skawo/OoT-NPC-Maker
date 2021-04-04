using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionScale : InstructionSub
    {
        public Int32 ActorCat;
        public UInt32 ActorID;
        public float Scale;
        public byte ScaleType;
        public byte ActorIDT;
        public byte ActorCatT;

        public InstructionScale(byte _SubID, UInt32 _ActorID, byte _ActorIDT, Int32 _ActorCat, byte _ActorCatT, float _Scale, byte _ScaleType)
                                : base((int)Lists.Instructions.SCALE, _SubID)
        {
            ActorCat = _ActorCat;
            ActorID = _ActorID;
            ActorCatT = _ActorCatT;
            ActorIDT = _ActorIDT;

            Scale = _Scale;
            ScaleType = _ScaleType;

        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(ActorIDT, ActorCatT, 4), Data);
            Helpers.AddObjectToByteList(ScaleType, Data);

            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(ActorCat, Data);
            Helpers.AddObjectToByteList(Scale, Data);
            Helpers.Ensure2ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }
    }
}

