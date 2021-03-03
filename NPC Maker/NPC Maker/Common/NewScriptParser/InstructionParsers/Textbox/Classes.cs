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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(AdultTextT, ChildTextT, 4), Data);
            DataHelpers.AddObjectToByteList(AdultText, Data);
            DataHelpers.AddObjectToByteList(ChildText, Data);
            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 10);

            return Data.ToArray();
        }
    }
}

