using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionScale : InstructionSub
    {
        public object ActorID;
        public object Scale;
        public byte ScaleType;
        public byte ActorIDT;
        public byte Target;

        public object Speed;
        public byte SpeedT;

        public InstructionScale(byte _SubID, object _ActorID, byte _ActorIDT, object _Scale, byte _ScaleType, byte _Target, object _Speed, byte _SpeedT)
                                : base((int)Lists.Instructions.SCALE, _SubID)
        {
            ActorID = _ActorID;
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
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SubID, Target, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ActorIDT, SpeedT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ScaleType, 0, 4), Data);

            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(Scale, Data);
            Helpers.AddObjectToByteList(Speed, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }
    }
}

