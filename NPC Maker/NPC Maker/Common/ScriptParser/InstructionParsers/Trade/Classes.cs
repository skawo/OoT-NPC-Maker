using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionTrade : Instruction
    {
        public TradeSetting Correct;
        public List<TradeSetting> Failure;
        public object AdultText;
        public object ChildText;
        public byte AdultTextT;
        public byte ChildTextT;

        public InstructionTrade(byte _ID, TradeSetting _Correct, List<TradeSetting> _Failure, object _AdultText, object _ChildText, byte _AdultTextT, byte _ChildTextT) : base(_ID)
        {
            AdultText = _AdultText;
            ChildText = _ChildText;
            Correct = _Correct;
            Failure = _Failure;
            AdultTextT = _AdultTextT;
            ChildTextT = _ChildTextT;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(AdultTextT, ChildTextT, 4), Data);
            Helpers.AddObjectToByteList(Convert.ToUInt16(Failure.Count), Data);

            Helpers.AddObjectToByteList(AdultText, Data);
            Helpers.AddObjectToByteList(ChildText, Data);

            Helpers.AddObjectToByteList(Correct.Item, Data);
            Helpers.AddObjectToByteList(Correct.AdultText, Data);
            Helpers.AddObjectToByteList(Correct.ChildText, Data);
            Helpers.AddObjectToByteList(Correct.ItemT, Data);
            Helpers.AddObjectToByteList(Correct.AdultTextT, Data);
            Helpers.AddObjectToByteList(Correct.ChildTextT, Data);
            Helpers.Ensure4ByteAlign(Data); // 28

            foreach (TradeSetting Setting in Failure)
            {
                Helpers.AddObjectToByteList(Setting.Item, Data);
                Helpers.AddObjectToByteList(Setting.AdultText, Data);
                Helpers.AddObjectToByteList(Setting.ChildText, Data);
                Helpers.AddObjectToByteList(Setting.ItemT, Data);
                Helpers.AddObjectToByteList(Setting.AdultTextT, Data);
                Helpers.AddObjectToByteList(Setting.ChildTextT, Data);
                Helpers.Ensure4ByteAlign(Data); // 16
            }

            Helpers.Ensure2ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 28 + 16 * Failure.Count);

            return Data.ToArray();
        }

    }

    public class TradeSetting
    {
        public object Item;
        public object AdultText;
        public object ChildText;
        public byte ItemT;
        public byte AdultTextT;
        public byte ChildTextT;

        public TradeSetting(object _Item, object _AdultText, object _ChildText, byte _ItemT, byte _AdultTextT, byte _ChildTextT)
        {
            Item = _Item;
            AdultText = _AdultText;
            ChildText = _ChildText;
            ItemT = _ItemT;
            AdultTextT = _AdultTextT;
            ChildTextT = _ChildTextT;
        }
    }
}

