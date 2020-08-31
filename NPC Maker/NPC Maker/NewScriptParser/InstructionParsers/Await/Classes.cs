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

    public class InstructionAwaitScriptVar : InstructionSubWValueType
    {
        public sbyte Value;
        public byte Condition;

        public InstructionAwaitScriptVar(byte _SubID, sbyte _Value, byte _Condition, byte _ValueType)
                                        : base((int)Lists.Instructions.AWAIT, _SubID, _ValueType)
        {
            Value = _Value;
            Condition = _Condition;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.AddObjectToByteList(ValueType, Data);
            ParserHelpers.AddObjectToByteList(Value, Data);
            ParserHelpers.AddObjectToByteList(Condition, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}
