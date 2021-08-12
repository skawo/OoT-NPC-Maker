using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionPlay : InstructionSub
    {
        ScriptVarVal Value { get; set; }

        public InstructionPlay(byte _SubID, ScriptVarVal _Value) : base((int)Lists.Instructions.PLAY, _SubID)
        {
            Value = _Value;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }
    }
}

