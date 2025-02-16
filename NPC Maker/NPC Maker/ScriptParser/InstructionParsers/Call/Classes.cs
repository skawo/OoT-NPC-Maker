using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionCall : Instruction
    {
        public string Goto = "";
        public InstructionLabel GotoInstr;

        List<ScriptVarVal> Args { get; set; }
        public byte numArgs { get; set; }

        public InstructionCall(byte _numArgs, string Label, List<ScriptVarVal> _Args) : base((byte)Lists.Instructions.CALL)
        {
            Goto = Label.ToUpper();
            GotoInstr = new InstructionLabel(Goto);
            numArgs = _numArgs;
            Args = _Args;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(numArgs, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoInstr, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            if (numArgs != 0)
            {
                for (int i = 0; i < 8; i += 2)
                {
                    if (numArgs > i + 1)
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Args[i].Vartype, Args[i + 1].Vartype, 4), Data);
                    else if (numArgs > i)
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Args[i].Vartype, (byte)0, 4), Data);
                    else
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether((byte)0, (byte)0, 4), Data);
                }

                Helpers.Ensure4ByteAlign(Data);
            }

            for (int i = 0; i < numArgs; i++)
                Helpers.AddObjectToByteList(Args[i].Value, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 4 + (numArgs != 0 ? 4 : 0) + (numArgs * 4));
            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + " " + Goto;
        }
    }
}

