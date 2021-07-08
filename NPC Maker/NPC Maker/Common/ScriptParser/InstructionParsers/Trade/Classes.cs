using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Scripts
{
    public class InstructionTrade : Instruction
    {
        public TradeSetting Correct;
        public UInt32 AdultText;
        public UInt32 ChildText;
        public byte AdultTextT;
        public byte ChildTextT;

        public InstructionTrade(byte _ID, TradeSetting _Correct, UInt32 _AdultText, UInt32 _ChildText, byte _AdultTextT, byte _ChildTextT) : base(_ID)
        {
            AdultText = _AdultText;
            ChildText = _ChildText;
            Correct = _Correct;
            AdultTextT = _AdultTextT;
            ChildTextT = _ChildTextT;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(AdultTextT, Data);
            Helpers.AddObjectToByteList(ChildTextT, Data);
            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(AdultText, Data);
            Helpers.AddObjectToByteList(ChildText, Data);

            Helpers.AddObjectToByteList(Correct.Item, Data);
            Helpers.AddObjectToByteList(Correct.AdultText, Data);
            Helpers.AddObjectToByteList(Correct.ChildText, Data);
            Helpers.AddObjectToByteList(Correct.ItemT, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Correct.AdultTextT, Correct.ChildTextT, 4), Data);
            Helpers.Ensure4ByteAlign(Data); // 28

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 28);

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
            AdultTextT = _AdultTextT;
            ChildTextT = _ChildTextT;
        }
    }
}

