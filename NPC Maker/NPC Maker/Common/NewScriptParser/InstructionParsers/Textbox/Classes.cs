using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionTextbox : Instruction
    {
        public UInt32 AdultText;
        public UInt32 ChildText;
        public byte AdultTextT;
        public byte ChildTextT;

        public InstructionTextbox(byte _ID, UInt32 Adult, UInt32 Child, byte AdultT, byte ChildT) : base(_ID)
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
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(AdultTextT, ChildTextT, 4), Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(AdultText, Data);
            Helpers.AddObjectToByteList(ChildText, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

