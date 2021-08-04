using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
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
            throw ParseException.GeneralError("Used wrong function to convert to bytes.");
        }

        public virtual byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.Ensure4ByteAlign(Data);

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

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.Ensure4ByteAlign(Data);

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
        public UInt16 InstructionNumber { get; set; }

        public InstructionLabel(string _Name) : base((int)Lists.Instructions.LABEL)
        {
            Name = _Name;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            return new byte[0];
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public string ToMarkingString()
        {
            return $"===================={Environment.NewLine + Name}:";
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

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}
