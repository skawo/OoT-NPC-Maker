using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionPosition : InstructionSub
    {
        public float X;
        public float Y;
        public float Z;
        public byte XType;
        public byte YType;
        public byte ZType;

        public byte ValueType;

        public InstructionPosition(byte _SubID, float _XPos, float _YPos, float _ZPos, byte _XPosValueType, byte _YPosValueType, byte _ZPosValueType) 
                                : base((int)Lists.Instructions.ROTATION, _SubID)
        {
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
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(X, Data);
            DataHelpers.AddObjectToByteList(Y, Data);
            DataHelpers.AddObjectToByteList(Z, Data);
            DataHelpers.AddObjectToByteList(XType, Data);
            DataHelpers.AddObjectToByteList(YType, Data);
            DataHelpers.AddObjectToByteList(ZType, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

