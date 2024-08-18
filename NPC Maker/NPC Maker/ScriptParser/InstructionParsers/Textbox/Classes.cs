using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionTextbox : Instruction
    {
        ScriptVarVal AdultText { get; set; }
        ScriptVarVal ChildText { get; set; }

        public InstructionTextbox(byte _ID, ScriptVarVal Adult, ScriptVarVal Child) : base(_ID)
        {
            AdultText = Adult;
            ChildText = Child;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            byte SkipChildText = (AdultText.Value == ChildText.Value && AdultText.Vartype == ChildText.Vartype) ? (byte)1 : (byte)0;

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SkipChildText, Data);
            Helpers.AddObjectToByteList(AdultText.Vartype, Data);
            Helpers.AddObjectToByteList(ChildText.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(AdultText.Value, Data);

            if (SkipChildText == 0)
                Helpers.AddObjectToByteList(ChildText.Value, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, SkipChildText == 1 ? 8 : 12);

            return Data.ToArray();
        }
    }
}

