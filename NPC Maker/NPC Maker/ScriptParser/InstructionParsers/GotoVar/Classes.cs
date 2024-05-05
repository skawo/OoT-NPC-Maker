using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionGotoVar : Instruction
    {
        public ScriptVarVal Value;

        public InstructionGotoVar(ScriptVarVal _Val) : base((int)Lists.Instructions.GOTO_VAR)
        {
            Value = _Val;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString();
        }
    }
}

