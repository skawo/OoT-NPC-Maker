using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionItem : InstructionSub
    {
        ScriptVarVal Value { get; set; }

        public InstructionItem(byte _ID, byte _SubID, ScriptVarVal _Value) : base(_ID, _SubID)
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

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.ItemSubTypes)SubID).ToString();
        }
    }
}

