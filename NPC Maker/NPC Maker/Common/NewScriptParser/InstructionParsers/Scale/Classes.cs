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
        public object Scale;
        public byte ScaleType;
        public byte ActorIDT;
        public byte ActorCatT;
        public byte Target;

        public object Speed;
        public byte SpeedT;

        public InstructionScale(byte _SubID, UInt32 _ActorID, byte _ActorIDT, Int32 _ActorCat, byte _ActorCatT, object _Scale, byte _ScaleType, byte _Target, object _Speed, byte _SpeedT)
                                : base((int)Lists.Instructions.SCALE, _SubID)
        {
            ActorCat = _ActorCat;
            ActorID = _ActorID;
            ActorCatT = _ActorCatT;
            ActorIDT = _ActorIDT;

            Scale = _Scale;
            ScaleType = _ScaleType;
            Target = _Target;

            Speed = _Speed;
            SpeedT = _SpeedT;

        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(SubID, Target, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(ActorIDT, ActorCatT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(SpeedT, ScaleType, 4), Data);

            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(ActorCat, Data);
            Helpers.AddObjectToByteList(Scale, Data);
            Helpers.AddObjectToByteList(Speed, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 20);

            return Data.ToArray();
        }
    }
}

