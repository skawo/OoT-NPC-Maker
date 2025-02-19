using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionSwitch : Instruction
    {
        ScriptVarVal SwitchedVar { get; set; }
        List<SwitchEntry> Entries { get; set; }

        public InstructionSwitch(ScriptVarVal swVar, List<SwitchEntry> entryList) : base((byte)Lists.Instructions.SWITCH)
        {
            SwitchedVar = swVar;
            Entries = entryList;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SwitchedVar.Vartype, Data);
            Helpers.AddObjectToByteList((UInt16)Entries.Count, Data);
            Helpers.Ensure4ByteAlign(Data);

            foreach(SwitchEntry e in Entries)
            {
                Helpers.AddObjectToByteList(e.var.Value, Data);
                ScriptDataHelpers.FindLabelAndAddToByteList(Labels, e.l, ref Data);
                Helpers.AddObjectToByteList(e.var.Vartype, Data);
                Helpers.AddObjectToByteList((byte)0, Data);
            }

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 4 + 8 * Entries.Count);

            return Data.ToArray();
        }
    }

    public class SwitchEntry
    {
        public InstructionLabel l;
        public ScriptVarVal var;
        public string n;

        public SwitchEntry(string Label, ScriptVarVal _var)
        {
            l = new InstructionLabel(Label);
            n = Label;
            var = _var;

        }
    }
}

