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

        public override byte[] ToBytes()
        {

            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.Ensure2ByteAlign(Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.ItemSubTypes)SubID).ToString();
        }
    }
}

