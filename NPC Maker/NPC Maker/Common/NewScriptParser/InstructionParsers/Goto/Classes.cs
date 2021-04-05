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

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.Ensure2ByteAlign(Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoInstr, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 4);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + " " + Goto;
        }
    }
}

