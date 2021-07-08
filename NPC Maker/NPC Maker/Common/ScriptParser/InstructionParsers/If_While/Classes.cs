using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Scripts
{
    public class InstructionIfWhile : InstructionSubWValueType
    {
        public byte Condition;
        public object Value;
        public int ElseLineNo;
        public int EndIfLineNo;
        public InstructionLabel GotoTrue;
        public InstructionLabel GotoFalse;

        public InstructionIfWhile(byte _ID, byte _SubID, byte _ValueType, object _Value, Lists.ConditionTypes _Condition, int _EndIfLineNo, int _ElseLineNo, string LabelStr) 
                             : base(_ID, _SubID, _ValueType)
        {
            Condition = (byte)_Condition;
            Value = _Value;
            ElseLineNo = _ElseLineNo;
            EndIfLineNo = _EndIfLineNo;
            ValueType = _ValueType;
            GotoTrue = new InstructionLabel("__IFTRUE__" + LabelStr);
            GotoFalse = new InstructionLabel("__IFFALSE__" + LabelStr);
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {

            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(Value, Data);
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

    public class InstructionIfWhileWithSecondValue : InstructionIfWhile
    {
        public object Value2;
        public byte ValueType2;

        public InstructionIfWhileWithSecondValue(byte _ID, byte _SubID, byte _ValueType, object _Value, byte _ValueType2, object _Value2, 
                                                 Lists.ConditionTypes _Condition, int _EndIfLineNo, int _ElseLineNo, string LabelStr)
                                                 : base(_ID, _SubID, _ValueType, _Value, _Condition, _EndIfLineNo, _ElseLineNo, LabelStr)
        {
            Value2 = _Value2;
            ValueType2 = _ValueType2;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {

            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ValueType, ValueType2, 4), Data);
            Helpers.AddObjectToByteList(Condition, Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.AddObjectToByteList(Value2, Data);
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

    public class InstructionIfWhileExtVar : InstructionIfWhile
    {
        public object Value2;
        public byte ValueType2;
        public byte ExtVarNum;

        public InstructionIfWhileExtVar(byte _ID, byte _SubID, byte _ExtVarNum, byte _ValueType, object _Value, byte _ValueType2, object _Value2,
                                                 Lists.ConditionTypes _Condition, int _EndIfLineNo, int _ElseLineNo, string LabelStr)
                                                 : base(_ID, _SubID, _ValueType, _Value, _Condition, _EndIfLineNo, _ElseLineNo, LabelStr)
        {
            Value2 = _Value2;
            ValueType2 = _ValueType2;
            ExtVarNum = _ExtVarNum;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {

            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ValueType, ValueType2, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Condition, ExtVarNum, 4), Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.AddObjectToByteList(Value2, Data);
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



}

