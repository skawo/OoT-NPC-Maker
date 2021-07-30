using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionTextbox : Instruction
    {
        public object AdultText;
        public object ChildText;
        public byte AdultTextT;
        public byte ChildTextT;

        public InstructionTextbox(byte _ID, object Adult, object Child, byte AdultT, byte ChildT) : base(_ID)
        {
            AdultText = Adult;
            ChildText = Child;
            AdultTextT = AdultT;
            ChildTextT = ChildT;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(AdultTextT, Data);
            Helpers.AddObjectToByteList(ChildTextT, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(AdultText, Data);
            Helpers.AddObjectToByteList(ChildText, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

