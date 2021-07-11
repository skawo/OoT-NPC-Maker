using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionPlay : InstructionSubWValueType
    {
        public UInt32 Value;

        public InstructionPlay(byte _SubID, UInt32 _Value, byte _ValueType) : base((int)Lists.Instructions.PLAY, _SubID, _ValueType)
        {
            Value = _Value;
            ValueType = _ValueType;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }
    }
}

