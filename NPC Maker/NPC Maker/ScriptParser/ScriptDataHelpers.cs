using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            while (true)
            {
                sb.Clear();
                sb.Append("lbl_");
                sb.Append(new string(Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray()));
                string randomLabel = sb.ToString();

                if (!Prs.RandomLabels.Contains(randomLabel))
                {
                    Prs.RandomLabels.Add(randomLabel);
                    return randomLabel;
                }
            }
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
