using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionOcarina : Instruction
    {
        public object SongID;
        public byte SongIDT;
        public InstructionLabel GotoTrue;
        public InstructionLabel GotoFalse;

        public InstructionOcarina(byte _ID, object Song, byte SongT, string LabelTrue, string LabelFalse) : base(_ID)
        {
            SongID = Song;
            SongIDT = SongT;
            GotoTrue = new InstructionLabel(LabelTrue);
            GotoFalse = new InstructionLabel(LabelFalse);
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SongIDT, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(SongID, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoTrue, ref Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, GotoFalse, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

