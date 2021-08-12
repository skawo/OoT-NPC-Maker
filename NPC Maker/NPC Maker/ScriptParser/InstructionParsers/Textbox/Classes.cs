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

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(AdultText.Vartype, Data);
            Helpers.AddObjectToByteList(ChildText.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(AdultText.Value, Data);
            Helpers.AddObjectToByteList(ChildText.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

