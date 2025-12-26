using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionAsync : InstructionSub
    {
        public InstructionLabel EndLabel;

        public InstructionAsync(byte _ID, byte _SubID, string LabelR): base(_ID, _SubID)
        {
            EndLabel = new InstructionLabel("__ASYNC_END__" + LabelR);
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, EndLabel, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 4);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return Lists.Instructions.ASYNC.ToString() + ", " + SubID.ToString();
        }
    }

    public class InstructionAsyncExit : InstructionSub
    {
        public InstructionAsyncExit(byte _ID, byte _SubID) : base(_ID, _SubID)
        {
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 4);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return Lists.Instructions.ASYNC.ToString() + ", " + SubID.ToString();
        }
    }

}

