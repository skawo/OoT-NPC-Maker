using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionAwait : InstructionSub
    {
        public ScriptVarVal Value { get; set; }
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

    public class InstructionAwaitCCall : InstructionAwait
    {
        readonly UInt32 Func;
        readonly byte IsBool;
        byte NumArgs { get; set; }
        List<ScriptVarVal> Params { get; set; }

        public InstructionAwaitCCall(byte _SubID, ScriptVarVal _Value, UInt32 _FuncAddr, Lists.ConditionTypes _Condition, List<ScriptVarVal> _Params, byte _IsBool)
                                : base(_SubID, _Value, _Condition)
        {
            Value = _Value;
            Condition = (byte)_Condition;
            Func = _FuncAddr;
            IsBool = _IsBool;
            Params = _Params;
            NumArgs = (byte)Params.Count;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Value.Vartype, IsBool, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(NumArgs, Condition, 4), Data);
            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Func, Data);
            Helpers.Ensure4ByteAlign(Data);

            if (NumArgs != 0)
            {
                for (int i = 0; i < 8; i += 2)
                {
                    if (NumArgs > i + 1)
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Params[i].Vartype, Params[i + 1].Vartype, 4), Data);
                    else if (NumArgs > i)
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Params[i].Vartype, (byte)0, 4), Data);
                    else
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether((byte)0, (byte)0, 4), Data);
                }

                Helpers.Ensure4ByteAlign(Data);
            }

            for (int i = 0; i < NumArgs; i++)
                Helpers.AddObjectToByteList(Params[i].Value, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12 + (NumArgs != 0 ? 4 : 0) + (NumArgs * 4));

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
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(ExtVarNum, Data);
            Helpers.AddObjectToByteList((byte)0, Data);
            Helpers.AddObjectToByteList((byte)0, Data);
            Helpers.AddObjectToByteList((byte)0, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Value2.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.AwaitSubTypes)SubID).ToString();
        }
    }
}
