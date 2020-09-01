using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionGoto : Instruction
    {
        public string Goto = "";
        public UInt16 InstrNumber = 0;

        public InstructionGoto(string Label) : base((int)Lists.Instructions.GOTO)
        {
            Goto = Label;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>
            {
                ID
            };

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(InstrNumber, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

