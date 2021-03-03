using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionRotation : InstructionSub
    {
        public Int16 X;
        public Int16 Y;
        public Int16 Z;
        public byte XType;
        public byte YType;
        public byte ZType;

        public byte ValueType;

        public InstructionRotation(byte _SubID, Int16 _XRot, Int16 _YRot, Int16 _ZRot, byte _XRotValueType, byte _YRotValueType, byte _ZRotValueType) 
                                : base((int)Lists.Instructions.ROTATION, _SubID)
        {
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

