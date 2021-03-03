using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionRotation : InstructionSub
    {
        public UInt32 ActorCat;
        public UInt32 ActorID;
        public Int32 X;
        public Int32 Y;
        public Int32 Z;
        public byte XType;
        public byte YType;
        public byte ZType;
        public byte ActorIDT;
        public byte ActorCatT;

        public InstructionRotation(byte _SubID, UInt32 _ActorID, byte _ActorIDT, UInt32 _ActorCat, byte _ActorCatT, Int32 _XRot, Int32 _YRot, Int32 _ZRot,
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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);

            byte SubIDAcIDT = 0;
            byte AcCatTXT = 0;
            byte YTZT = 0;

            SubIDAcIDT |= (byte)(SubID << 4);
            SubIDAcIDT |= ActorIDT;

            AcCatTXT |= (byte)(YType << 4);
            AcCatTXT |= XType;

            YTZT |= (byte)(YType << 4);
            YTZT |= ZType;

            DataHelpers.AddObjectToByteList(SubIDAcIDT, Data);
            DataHelpers.AddObjectToByteList(AcCatTXT, Data);
            DataHelpers.AddObjectToByteList(YTZT, Data);

            DataHelpers.AddObjectToByteList(X, Data);
            DataHelpers.AddObjectToByteList(Y, Data);
            DataHelpers.AddObjectToByteList(Z, Data);
            DataHelpers.AddObjectToByteList(ActorID, Data);
            DataHelpers.AddObjectToByteList(ActorCat, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 24);

            return Data.ToArray();
        }
    }
}

