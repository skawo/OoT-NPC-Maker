using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionGoto : Instruction
    {
        public string Goto = "";
        public InstructionLabel GotoInstr;

        public InstructionGoto(string Label) : base((int)Lists.Instructions.GOTO)
        {
            Goto = Label;
            GotoInstr = new InstructionLabel(Label);
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(GotoInstr.InstructionNumber, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + " " + Goto;
        }
    }
}

