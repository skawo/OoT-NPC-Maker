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

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.Ensure4ByteAlign(Data);

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

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(Condition, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

    public class InstructionAwaitWithSecondValue : InstructionSubWValueType
    {
        public object Value;
        public object Value2;
        public byte Condition;

        public InstructionAwaitWithSecondValue(byte _SubID, object _Value, object _Value2, Lists.ConditionTypes _Condition, byte _Value1Type)
                                        : base((int)Lists.Instructions.AWAIT, _SubID, _Value1Type)
        {
            Value = _Value;
            Value2 = _Value2;
            Condition = (byte)_Condition;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(Condition, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(Value2, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}
