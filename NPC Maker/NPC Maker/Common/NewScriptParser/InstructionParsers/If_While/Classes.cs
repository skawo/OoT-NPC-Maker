using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
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

        public override byte[] ToBytes()
        {

            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(Condition, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(GotoTrue.InstructionNumber, Data);
            DataHelpers.AddObjectToByteList(GotoFalse.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 16);

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

        public override byte[] ToBytes()
        {

            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ValueType, Condition, 4), Data);
            DataHelpers.AddObjectToByteList(ValueType2, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(Value2, Data);

            DataHelpers.AddObjectToByteList(GotoTrue.InstructionNumber, Data);
            DataHelpers.AddObjectToByteList(GotoFalse.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 20);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + SubID.ToString() + " " + GotoTrue.ToString() + " " + GotoFalse.ToString();
        }
    }



}

