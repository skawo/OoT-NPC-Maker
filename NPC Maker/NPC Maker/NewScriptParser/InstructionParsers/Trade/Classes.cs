using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionTrade : Instruction
    {
        public TradeSetting Correct;
        public List<TradeSetting> Failure;
        public UInt16 AdultText;
        public UInt16 ChildText;

        public InstructionTrade(byte _ID, TradeSetting _Correct, List<TradeSetting> _Failure, UInt16 _AdultText, UInt16 _ChildText) : base(_ID)
        {
            AdultText = _AdultText;
            ChildText = _ChildText;
            Correct = _Correct;
            Failure = _Failure;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(AdultText, Data);
            ParserHelpers.AddObjectToByteList(ChildText, Data);
            ParserHelpers.Ensure4ByteAlign(Data);
            ParserHelpers.AddObjectToByteList(Correct.Item, Data);
            ParserHelpers.AddObjectToByteList(Correct.AdultText, Data);
            ParserHelpers.AddObjectToByteList(Correct.ChildText, Data);
            ParserHelpers.AddObjectToByteList(Convert.ToUInt16(Failure.Count), Data);

            ParserHelpers.Ensure4ByteAlign(Data);

            foreach (TradeSetting Setting in Failure)
            {
                ParserHelpers.AddObjectToByteList(Setting.Item, Data);
                ParserHelpers.AddObjectToByteList(Setting.AdultText, Data);
                ParserHelpers.AddObjectToByteList(Setting.ChildText, Data);
                ParserHelpers.Ensure4ByteAlign(Data);
            }

            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

    public class TradeSetting
    {
        public Int16 Item;
        public UInt16 AdultText;
        public UInt16 ChildText;

        public TradeSetting(Int16 _Item, UInt16 _AdultText, UInt16 _ChildText)
        {
            Item = _Item;
            AdultText = _AdultText;
            ChildText = _ChildText;
        }
    }
}

