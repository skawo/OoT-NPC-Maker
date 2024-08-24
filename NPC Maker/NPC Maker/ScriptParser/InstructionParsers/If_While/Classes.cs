using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionIfWhile : InstructionSub
    {
        public byte Condition;

        public ScriptVarVal Value { get; set; }
        public int ElseLineNo;
        public int EndIfLineNo;
        public InstructionLabel GotoTrue;
        public InstructionLabel GotoFalse;

        public InstructionIfWhile(byte _ID, byte _SubID, ScriptVarVal _Value, Lists.ConditionTypes _Condition, int _EndIfLineNo, int _ElseLineNo, string LabelStr)
                             : base(_ID, _SubID)
        {
            Condition = (byte)_Condition;
            Value = _Value;
            ElseLineNo = _ElseLineNo;
            EndIfLineNo = _EndIfLineNo;
            GotoTrue = new InstructionLabel("__IFTRUE__" + LabelStr);
            GotoFalse = new InstructionLabel("__IFFALSE__" + LabelStr);
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {

            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoTrue, ref Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoFalse, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + SubID.ToString() + " " + GotoTrue.ToString() + " " + GotoFalse.ToString();
        }
    }

    public class InstructionIfWhileCCall : InstructionIfWhile
    {
        public UInt32 Func;
        public byte IsBool;
        byte NumArgs { get; set; }
        List<ScriptVarVal> Params { get; set; }


        public InstructionIfWhileCCall(byte _ID, byte _SubID, ScriptVarVal _Value, UInt32 _FuncAddr, List<ScriptVarVal> _Params, byte _IsBool, Lists.ConditionTypes _Condition, int _EndIfLineNo, int _ElseLineNo, string LabelStr)
                             : base(_ID, _SubID, _Value, _Condition, _EndIfLineNo, _ElseLineNo, LabelStr )
        {
            Condition = (byte)_Condition;
            Value = _Value;
            Func = _FuncAddr;
            ElseLineNo = _ElseLineNo;
            EndIfLineNo = _EndIfLineNo;
            IsBool = _IsBool;
            GotoTrue = new InstructionLabel("__IFTRUE__" + LabelStr);
            GotoFalse = new InstructionLabel("__IFFALSE__" + LabelStr);
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

            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Func, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoTrue, ref Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoFalse, ref Data);

            for (int i = 0; i < NumArgs; i++)
                Helpers.AddObjectToByteList(Params[i].Value, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 20 + (NumArgs * 4));

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + SubID.ToString() + " " + GotoTrue.ToString() + " " + GotoFalse.ToString();
        }
    }

    public class InstructionIfWhileWithSecondValue : InstructionIfWhile
    {
        public ScriptVarVal Value2 { get; set; }

        public InstructionIfWhileWithSecondValue(byte _ID, byte _SubID, ScriptVarVal _Value, ScriptVarVal _Value2,
                                                 Lists.ConditionTypes _Condition, int _EndIfLineNo, int _ElseLineNo, string LabelStr)
                                                 : base(_ID, _SubID, _Value, _Condition, _EndIfLineNo, _ElseLineNo, LabelStr)
        {
            Value2 = _Value2;
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
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoTrue, ref Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoFalse, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + SubID.ToString() + " " + GotoTrue.ToString() + " " + GotoFalse.ToString();
        }
    }

    public class InstructionIfWhileExtVar : InstructionIfWhileWithSecondValue
    {
        public byte ExtVarNum;

        public InstructionIfWhileExtVar(byte _ID, byte _SubID, byte _ExtVarNum, ScriptVarVal _Value, ScriptVarVal _Value2,
                                                 Lists.ConditionTypes _Condition, int _EndIfLineNo, int _ElseLineNo, string LabelStr)
                                                 : base(_ID, _SubID,  _Value, _Value2, _Condition, _EndIfLineNo, _ElseLineNo, LabelStr)
        {
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
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoTrue, ref Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoFalse, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 20);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + SubID.ToString() + " " + GotoTrue.ToString() + " " + GotoFalse.ToString();
        }
    }



}

