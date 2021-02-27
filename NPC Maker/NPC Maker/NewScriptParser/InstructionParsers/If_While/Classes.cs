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

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.AddObjectToByteList(ValueType, Data);
            ParserHelpers.AddObjectToByteList(Condition, Data);
            ParserHelpers.AddObjectToByteList(Value, Data);
            ParserHelpers.AddObjectToByteList(GotoTrue.InstructionNumber, Data);
            ParserHelpers.AddObjectToByteList(GotoFalse.InstructionNumber, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + SubID.ToString() + " " + GotoTrue.ToString() + " " + GotoFalse.ToString();
        }
    }



}

