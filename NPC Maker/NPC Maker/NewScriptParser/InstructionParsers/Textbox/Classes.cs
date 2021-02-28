using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionTextbox : Instruction
    {
        public UInt16 AdultText;
        public UInt16 ChildText;

        public InstructionTextbox(byte _ID, UInt16 Adult, UInt16 Child) : base(_ID)
        {
            AdultText = Adult;
            ChildText = Child;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(AdultText, Data);
            DataHelpers.AddObjectToByteList(ChildText, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

