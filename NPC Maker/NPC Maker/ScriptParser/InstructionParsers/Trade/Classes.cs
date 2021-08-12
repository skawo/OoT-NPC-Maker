using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionTrade : Instruction
    {
        public TradeSetting Correct { get; set; }
        public List<TradeSetting> Failure { get; set; }

        public ScriptVarVal AdultText { get; set; }
        public ScriptVarVal ChildText { get; set; }

        public InstructionTrade(byte _ID, TradeSetting _Correct, List<TradeSetting> _Failure, ScriptVarVal _AdultText, ScriptVarVal _ChildText) : base(_ID)
        {
            AdultText = _AdultText;
            ChildText = _ChildText;
            Correct = _Correct;
            Failure = _Failure;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(AdultText.Vartype, ChildText.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Convert.ToUInt16(Failure.Count), Data);

            Helpers.AddObjectToByteList(AdultText.Value, Data);
            Helpers.AddObjectToByteList(ChildText.Value, Data);

            Helpers.AddObjectToByteList(Correct.Item.Value, Data);
            Helpers.AddObjectToByteList(Correct.AdultText.Value, Data);
            Helpers.AddObjectToByteList(Correct.ChildText.Value, Data);
            Helpers.AddObjectToByteList(Correct.Item.Vartype, Data);
            Helpers.AddObjectToByteList(Correct.AdultText.Vartype, Data);
            Helpers.AddObjectToByteList(Correct.ChildText.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data); // 28

            foreach (TradeSetting Setting in Failure)
            {
                Helpers.AddObjectToByteList(Setting.Item.Value, Data);
                Helpers.AddObjectToByteList(Setting.AdultText.Value, Data);
                Helpers.AddObjectToByteList(Setting.ChildText.Value, Data);
                Helpers.AddObjectToByteList(Setting.Item.Vartype, Data);
                Helpers.AddObjectToByteList(Setting.AdultText.Vartype, Data);
                Helpers.AddObjectToByteList(Setting.ChildText.Vartype, Data);
                Helpers.Ensure4ByteAlign(Data); // 16
            }

            Helpers.Ensure2ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 28 + 16 * Failure.Count);

            return Data.ToArray();
        }

    }

    public class TradeSetting
    {
        public ScriptVarVal Item { get; set; }
        public ScriptVarVal AdultText { get; set; }
        public ScriptVarVal ChildText { get; set; }

        public TradeSetting(ScriptVarVal _Item, ScriptVarVal _AdultText, ScriptVarVal _ChildText)
        {
            Item = _Item;
            AdultText = _AdultText;
            ChildText = _ChildText;
        }
    }
}

