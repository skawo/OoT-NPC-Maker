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
        public UInt32 AdultText;
        public UInt32 ChildText;
        public byte AdultTextT;
        public byte ChildTextT;

        public InstructionTrade(byte _ID, TradeSetting _Correct, List<TradeSetting> _Failure, UInt32 _AdultText, UInt32 _ChildText, byte _AdultTextT, byte _ChildTextT) : base(_ID)
        {
            AdultText = _AdultText;
            ChildText = _ChildText;
            Correct = _Correct;
            Failure = _Failure;
            AdultTextT = _AdultTextT;
            ChildTextT = _ChildTextT;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(AdultTextT, ChildTextT, 4), Data);
            DataHelpers.AddObjectToByteList(Convert.ToUInt16(Failure.Count), Data);

            DataHelpers.AddObjectToByteList(AdultText, Data);
            DataHelpers.AddObjectToByteList(ChildText, Data);

            DataHelpers.AddObjectToByteList(Correct.Item, Data);
            DataHelpers.AddObjectToByteList(Correct.AdultText, Data);
            DataHelpers.AddObjectToByteList(Correct.ChildText, Data);
            DataHelpers.AddObjectToByteList(Correct.ItemT, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(Correct.AdultTextT, Correct.ChildTextT, 4), Data);
            DataHelpers.Ensure2ByteAlign(Data); // 26

            foreach (TradeSetting Setting in Failure)
            {
                DataHelpers.AddObjectToByteList(Setting.Item, Data);
                DataHelpers.AddObjectToByteList(Setting.AdultText, Data);
                DataHelpers.AddObjectToByteList(Setting.ChildText, Data);
                DataHelpers.AddObjectToByteList(Setting.ItemT, Data);
                DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(Setting.AdultTextT, Setting.ChildTextT, 4), Data);
                DataHelpers.Ensure2ByteAlign(Data); // 14
            }

            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 26 + 14 * Failure.Count);

            return Data.ToArray();
        }
    }

    public class TradeSetting
    {
        public Int32 Item;
        public UInt32 AdultText;
        public UInt32 ChildText;
        public byte ItemT;
        public byte AdultTextT;
        public byte ChildTextT;

        public TradeSetting(Int32 _Item, UInt32 _AdultText, UInt32 _ChildText, byte _ItemT, byte _AdultTextT, byte _ChildTextT)
        {
            Item = _Item;
            AdultText = _AdultText;
            ChildText = _ChildText;
            ItemT = _ItemT;
            AdultText = _AdultTextT;
            ChildText = _ChildTextT;
        }
    }
}

