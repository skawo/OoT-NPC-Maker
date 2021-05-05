using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionRotation : InstructionSub
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

        public InstructionRotation(byte _SubID, object _ActorID, byte _ActorIDT, object _ActorCat, byte _ActorCatT, object _XRot, object _YRot, object _ZRot,
                                   byte _XRotValueType, byte _YRotValueType, byte _ZRotValueType)
                                : base((int)Lists.Instructions.ROTATION, _SubID)
        {
            ActorCat = _ActorCat;
            ActorID = _ActorID;
            ActorCatT = _ActorCatT;
            ActorIDT = _ActorIDT;

            X = _XRot;
            Y = _YRot;
            Z = _ZRot;
            XType = _XRotValueType;
            YType = _YRotValueType;
            ZType = _ZRotValueType;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(SubID, ActorIDT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(ActorCatT, XType, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(YType, ZType, 4), Data);
            Helpers.AddObjectToByteList(X, Data);
            Helpers.AddObjectToByteList(Y, Data);
            Helpers.AddObjectToByteList(Z, Data);
            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(ActorCat, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 24);

            return Data.ToArray();
        }
    }
}

