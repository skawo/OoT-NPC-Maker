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
        public List<Instruction> True;
        public List<Instruction> False;
        public int ElseLineNo;
        public int EndIfLineNo;
        public InstructionLabel GotoElse;
        public InstructionLabel GotoEndIf;

        public InstructionIfWhile(byte _ID, byte _SubID, byte _ValueType, object _Value, byte _Condition,
                             int _EndIfLineNo, int _ElseLineNo, int _CurrentLineNo, List<Instruction> _True, List<Instruction> _False) 
                             : base(_ID, _SubID, _ValueType)
        {
            Condition = _Condition;
            Value = _Value;
            True = _True;
            False = _False;
            ElseLineNo = _ElseLineNo;
            EndIfLineNo = _EndIfLineNo;
            ValueType = _ValueType;
            GotoElse = new InstructionLabel("__IFELSE__" + _CurrentLineNo.ToString());
            GotoEndIf = new InstructionLabel("__IFEND__" + _CurrentLineNo.ToString());
        }

        public override byte[] ToBytes()
        {

            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.AddObjectToByteList(ValueType, Data);
            ParserHelpers.AddObjectToByteList(Condition, Data);
            ParserHelpers.AddObjectToByteList(GotoElse.InstructionNumber, Data);
            ParserHelpers.AddObjectToByteList(GotoEndIf.InstructionNumber, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }



}

