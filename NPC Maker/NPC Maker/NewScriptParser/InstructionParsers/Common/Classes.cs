using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class Instruction
    {
        public byte ID { get; set; }

        public Instruction(byte _ID)
        {
            ID = _ID;
        }

        public virtual byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString();
        }
    }

    public class InstructionSub : Instruction
    {
        public byte SubID { get; set; }

        public InstructionSub(byte _ID, byte _SubID) : base(_ID)
        {
            SubID = _SubID;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + SubID.ToString();
        }
    }

    public class InstructionLabel : Instruction
    {
        public string Name { get; set; }
        public int InstructionNumber { get; set; }

        public InstructionLabel(string _Name) : base((int)Lists.Instructions.LABEL)
        {
            Name = _Name;
        }

        public override string ToString()
        {
            return Name + ":";
        }
    }

    public class InstructionSubWValueType : InstructionSub
    {
        public byte ValueType { get; set; }

        public InstructionSubWValueType(byte _ID, byte _SubID, byte _ValueType) : base(_ID, _SubID)
        {
            SubID = _SubID;
            ValueType = _ValueType;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.AddObjectToByteList(ValueType, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}
