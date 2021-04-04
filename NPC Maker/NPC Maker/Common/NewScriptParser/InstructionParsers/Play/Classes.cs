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

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.AddObjectToByteList(ValueType2, Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.AddObjectToByteList(Value2, Data);
            Helpers.Ensure2ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

