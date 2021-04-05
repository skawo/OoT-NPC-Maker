using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionItem : InstructionSub
    {
        public object Value;
        public byte ValueType;

        public InstructionItem(byte _ID, byte _SubID, object _Value, byte _ValueType) : base(_ID, _SubID)
        {
            Value = _Value;
            ValueType = _ValueType;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {

            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(Value, Data);
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

