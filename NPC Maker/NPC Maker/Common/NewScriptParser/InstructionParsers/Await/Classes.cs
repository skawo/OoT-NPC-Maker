using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Scripts
{
    public class InstructionAwait : InstructionSubWValueType
    {
        public object Value;
        public byte Condition;

        public InstructionAwait(byte _SubID, object _Value, Lists.ConditionTypes _Condition, byte _ValueType)
                                : base((int)Lists.Instructions.AWAIT, _SubID, _ValueType)
        {
            Value = _Value;
            Condition = (byte)_Condition;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.AwaitSubTypes)SubID).ToString();
        }
    }

    public class InstructionAwaitWithSecondValue : InstructionSubWValueType
    {
        public object Value;
        public object Value2;
        public byte Value2T;
        public byte Condition;

        public InstructionAwaitWithSecondValue(byte _SubID, object _Value, object _Value2, Lists.ConditionTypes _Condition, byte _Value1Type, byte _Value2Type)
                                                : base((int)Lists.Instructions.AWAIT, _SubID, _Value1Type)
        {
            Value = _Value;
            Value2 = _Value2;
            Value2T = _Value2Type;
            Condition = (byte)_Condition;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(ValueType, Value2T, 4), Data);
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.AddObjectToByteList(Value2, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.AwaitSubTypes)SubID).ToString();
        }
    }

    public class InstructionAwaitExtVar: InstructionSubWValueType
    {
        public object Value;
        public object Value2;
        public byte Value2T;
        public byte Condition;
        public byte ExtVarNum;

        public InstructionAwaitExtVar(byte _SubID, byte _ExtVarNum, object _Value, object _NPCID, Lists.ConditionTypes _Condition, byte _Value1Type, byte _NPCIDType)
                                                : base((int)Lists.Instructions.AWAIT, _SubID, _Value1Type)
        {
            Value = _Value;
            Value2 = _NPCID;
            Value2T = _NPCIDType;
            Condition = (byte)_Condition;
            ExtVarNum = _ExtVarNum;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(ValueType, Value2T, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(Condition, ExtVarNum, 4), Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.AddObjectToByteList(Value2, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.AwaitSubTypes)SubID).ToString();
        }
    }
}
