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

        public InstructionItem(byte _ID, byte _SubID, object _Value) : base(_ID, _SubID)
        {
            Value = _Value;
        }

        public override byte[] ToBytes()
        {

            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(SubID, Data);
            ParserHelpers.AddObjectToByteList(Value, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((NewScriptParser.Lists.ItemSubTypes)SubID).ToString();
        }
    }
}

