using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionRotation : InstructionSub
    {
        public object ActorID;
        public object X;
        public object Y;
        public object Z;
        public byte XType;
        public byte YType;
        public byte ZType;
        public byte ActorIDT;
        public byte Target;
        public object Speed;
        public byte SpeedT;

        public InstructionRotation(byte _SubID, object _ActorID, byte _ActorIDT, object _XRot, object _YRot, object _ZRot,
                                   byte _XRotValueType, byte _YRotValueType, byte _ZRotValueType, byte _Target, object _Speed, byte _SpeedT)
                                : base((int)Lists.Instructions.ROTATION, _SubID)
        {
            ActorID = _ActorID;
            ActorIDT = _ActorIDT;

            X = _XRot;
            Y = _YRot;
            Z = _ZRot;
            XType = _XRotValueType;
            YType = _YRotValueType;
            ZType = _ZRotValueType;
            Target = _Target;

            Speed = _Speed;
            SpeedT = _SpeedT;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Target, Data);
            Helpers.AddObjectToByteList(SpeedT, Data);

            Helpers.AddObjectToByteList(X, Data);
            Helpers.AddObjectToByteList(Y, Data);
            Helpers.AddObjectToByteList(Z, Data);
            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(Speed, Data);

            Helpers.AddObjectToByteList(XType, Data);
            Helpers.AddObjectToByteList(YType, Data);
            Helpers.AddObjectToByteList(ZType, Data);
            Helpers.AddObjectToByteList(ActorIDT, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 28);

            return Data.ToArray();
        }
    }
}

