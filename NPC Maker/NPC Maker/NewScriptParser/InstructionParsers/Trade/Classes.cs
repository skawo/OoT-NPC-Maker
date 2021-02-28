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

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(AdultText, Data);
            DataHelpers.AddObjectToByteList(ChildText, Data);
            DataHelpers.Ensure4ByteAlign(Data);
            DataHelpers.AddObjectToByteList(Correct.Item, Data);
            DataHelpers.AddObjectToByteList(Correct.AdultText, Data);
            DataHelpers.AddObjectToByteList(Correct.ChildText, Data);
            DataHelpers.AddObjectToByteList(Convert.ToUInt16(Failure.Count), Data);

            DataHelpers.Ensure4ByteAlign(Data);

            foreach (TradeSetting Setting in Failure)
            {
                DataHelpers.AddObjectToByteList(Setting.Item, Data);
                DataHelpers.AddObjectToByteList(Setting.AdultText, Data);
                DataHelpers.AddObjectToByteList(Setting.ChildText, Data);
                DataHelpers.Ensure4ByteAlign(Data);
            }

            DataHelpers.Ensure4ByteAlign(Data);

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

