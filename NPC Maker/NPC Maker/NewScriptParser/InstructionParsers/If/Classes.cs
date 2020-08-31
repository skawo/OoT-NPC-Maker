using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionIf : InstructionSubWValueType
    {
        public byte Condition;
        public object Value;
        public List<Instruction> True;
        public List<Instruction> False;
        public int ElseLineNo;
        public int EndIfLineNo;

        public InstructionIf(byte _SubID, byte _ValueType, object _Value, byte _Condition, 
                             int _ElseLineNo, int _EndIfLineNo, List<Instruction> _True, List<Instruction> _False) 
                             : base((int)Lists.Instructions.IF, _SubID, _ValueType)
        {
            Condition = _Condition;
            Value = _Value;
            True = _True;
            False = _False;
            ElseLineNo = _ElseLineNo;
            EndIfLineNo = _EndIfLineNo;
            ValueType = _ValueType;
        }

        public override byte[] ToBytes()
        {

            List<byte> Data = new List<byte>();
            Data.Add(ID);
            Data.Add(SubID);
            return Data.ToArray();
        }
    }



}

