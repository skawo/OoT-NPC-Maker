using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionScale : InstructionSub
    {
        public float Scale;
        public byte VarType;

        public InstructionScale(byte _SubID, float _Scale, byte _ValueType) 
                                : base((int)Lists.Instructions.ROTATION, _SubID)
        {
            Scale = _Scale;
            VarType = _ValueType;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(VarType, Data);
            DataHelpers.Ensure4ByteAlign(Data);
            DataHelpers.AddObjectToByteList(Scale, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

