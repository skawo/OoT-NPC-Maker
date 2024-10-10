using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public static class ScriptDataHelpers
    {
        public static void ErrorIfExpectedLenWrong(List<byte> ByteList, int Len)
        {
            if (Len != ByteList.Count)
                System.Windows.Forms.MessageBox.Show($"Critical error: Got wrong amount of bytes: {(Lists.Instructions)ByteList[0]}, data: {BitConverter.ToString(ByteList.ToArray())}");
        }

        public static string GetRandomLabelString(ScriptParser Prs, int length = 5)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string Out = "";

            do
            {
                Out = "lbl_" + new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (Prs.RandomLabels.Contains(Out));

            Prs.RandomLabels.Add(Out);
            return Out;
        }

        public static void FindLabelAndAddToByteList(List<InstructionLabel> Labels, InstructionLabel ToFind, ref List<byte> Data)
        {
            Helpers.AddObjectToByteList(FindLabel(Labels, ToFind), Data);
        }

        public static UInt16 FindLabel(List<InstructionLabel> Labels, InstructionLabel ToFind)
        {
            if (ToFind.Name == Lists.Keyword_Label_Return || ToFind.Name == Lists.Keyword_Label_Null)
                return UInt16.MaxValue; 

            bool SkipCheck = false;
#if DEBUG
            if (Labels.Find(x => x.Name == Lists.Keyword_Debug_Skip_Label_Check) != null)
                SkipCheck = true;
#endif

            InstructionLabel Found = Labels.Find(x => x.Name == ToFind.Name);

            if (Found == null && !SkipCheck)
                throw ParseException.LabelNotFound(ToFind.Name);

            return Found == null ? UInt16.MaxValue : Found.InstructionNumber;
        }
    }
}
