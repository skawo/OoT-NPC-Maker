using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionAwait : InstructionSubWValueType
    {
        public object Value;

        public InstructionAwait(byte _SubID, object _Value, byte _ValueType)
                                : base((int)Lists.Instructions.AWAIT, _SubID, _ValueType)
        {
            Value = _Value;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.AddObjectToByteList(ValueType, Data);
            ParserHelpers.AddObjectToByteList(Value, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

    public class InstructionAwaitValue : InstructionSubWValueType
    {
        public object Value;
        public byte Condition;

        public InstructionAwaitValue(byte _SubID, object _Value, Lists.ConditionTypes _Condition, byte _ValueType)
                                        : base((int)Lists.Instructions.AWAIT, _SubID, _ValueType)
        {
            Value = _Value;
            Condition = (byte)_Condition;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.AddObjectToByteList(ValueType, Data);
            ParserHelpers.AddObjectToByteList(Condition, Data);
            ParserHelpers.AddObjectToByteList(Value, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}
