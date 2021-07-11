using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionPosition : InstructionSub
    {
        public object ActorCat;
        public object ActorID;
        public object X;
        public object Y;
        public object Z;
        public byte XType;
        public byte YType;
        public byte ZType;
        public byte ActorIDT;
        public byte ActorCatT;
        public byte Target;

        public object Speed;
        public byte SpeedT;
        public byte IgnoreY;

        public InstructionPosition(byte _SubID, object _ActorID, byte _ActorIDT, object _ActorCat, byte _ActorCatT, object _XPos, object _YPos, object _ZPos,
                                   byte _XPosValueType, byte _YPosValueType, byte _ZPosValueType, byte _Target, object _Speed, byte _SpeedT, byte _IgnoreY)
                                : base((int)Lists.Instructions.POSITION, _SubID)
        {
            ActorCat = _ActorCat;
            ActorID = _ActorID;
            ActorCatT = _ActorCatT;
            ActorIDT = _ActorIDT;

            X = _XPos;
            Y = _YPos;
            Z = _ZPos;
            XType = _XPosValueType;
            YType = _YPosValueType;
            ZType = _ZPosValueType;
            Target = _Target;

            Speed = _Speed;
            SpeedT = _SpeedT;
            IgnoreY = _IgnoreY;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SubID, SpeedT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(IgnoreY, Target, 4), Data);
            Helpers.AddObjectToByteList(ActorIDT, Data);

            Helpers.AddObjectToByteList(X, Data);
            Helpers.AddObjectToByteList(Y, Data);
            Helpers.AddObjectToByteList(Z, Data);
            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(ActorCat, Data);
            Helpers.AddObjectToByteList(Speed, Data);

            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ActorCatT, XType, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(YType, ZType, 4), Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 32);

            return Data.ToArray();
        }
    }
}

