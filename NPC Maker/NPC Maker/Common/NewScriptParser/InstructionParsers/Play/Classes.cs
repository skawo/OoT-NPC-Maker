using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionPlay : InstructionSubWValueType
    {
        public UInt32 Value;
        public UInt32 Value2;
        public byte ValueType2;

        public InstructionPlay(byte _SubID, UInt32 _Value, byte _ValueType, UInt32 _Value2, byte _ValueType2) : base((int)Lists.Instructions.PLAY, _SubID, _ValueType)
        {
            Value = _Value;
            Value2 = _Value2;
            ValueType = _ValueType;
            ValueType2 = _ValueType2;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(ValueType2, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(Value2, Data);
            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

