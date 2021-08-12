using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionAwait : InstructionSub
    {
        ScriptVarVal Value { get; set; }
        public byte Condition;

        public InstructionAwait(byte _SubID, ScriptVarVal _Value, Lists.ConditionTypes _Condition)
                                : base((int)Lists.Instructions.AWAIT, _SubID)
        {
            Value = _Value;
            Condition = (byte)_Condition;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.AwaitSubTypes)SubID).ToString();
        }
    }

    public class InstructionAwaitWithSecondValue : InstructionSub
    {
        ScriptVarVal Value { get; set; }
        ScriptVarVal Value2 { get; set; }

        public byte Condition;

        public InstructionAwaitWithSecondValue(byte _SubID, ScriptVarVal _Value, ScriptVarVal _Value2, Lists.ConditionTypes _Condition)
                                                : base((int)Lists.Instructions.AWAIT, _SubID)
        {
            Value = _Value;
            Value2 = _Value2;
            Condition = (byte)_Condition;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Value.Vartype, Value2.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Value2.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.AwaitSubTypes)SubID).ToString();
        }
    }

    public class InstructionAwaitExtVar : InstructionSub
    {
        ScriptVarVal Value { get; set; }
        ScriptVarVal Value2 { get; set; }
        public byte Condition;
        public byte ExtVarNum;

        public InstructionAwaitExtVar(byte _SubID, byte _ExtVarNum, ScriptVarVal _Value, ScriptVarVal _NPCID, Lists.ConditionTypes _Condition)
                                                : base((int)Lists.Instructions.AWAIT, _SubID)
        {
            Value = _Value;
            Value2 = _NPCID;
            Condition = (byte)_Condition;
            ExtVarNum = _ExtVarNum;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Value.Vartype, Value2.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Condition, ExtVarNum, 4), Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Value2.Value, Data);
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
