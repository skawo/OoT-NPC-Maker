using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionPosition : InstructionSub
    {
        public UInt32 ActorCat;
        public UInt32 ActorID;
        public float X;
        public float Y;
        public float Z;
        public byte XType;
        public byte YType;
        public byte ZType;
        public byte ActorIDT;
        public byte ActorCatT;

        public byte ValueType;

        public InstructionPosition(byte _SubID, UInt32 _ActorID, byte _ActorIDT, UInt32 _ActorCat, byte _ActorCatT, float _XPos, float _YPos, float _ZPos, 
                                   byte _XPosValueType, byte _YPosValueType, byte _ZPosValueType) 
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

