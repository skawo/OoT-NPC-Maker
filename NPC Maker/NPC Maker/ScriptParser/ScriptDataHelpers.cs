using NPC_Maker.Controls;
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
                BigMessageBox.Show($"Critical error: Got wrong amount of bytes: {(Lists.Instructions)ByteList[0]}, data: {BitConverter.ToString(ByteList.ToArray())}");
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

        public static ushort FindLabel(List<InstructionLabel> labels, InstructionLabel toFind)
        {
            if (toFind.Name == Lists.Keyword_Label_Return ||
                toFind.Name == Lists.Keyword_Label_Null)
                return ushort.MaxValue;

            #if DEBUG
                bool skipCheck = labels.Any(l => l.Name == Lists.Keyword_Debug_Skip_Label_Check);
            #else
                bool skipCheck = false;
            #endif

            var found = labels.FirstOrDefault(l => l.Name == toFind.Name);

            if (found == null)
            {
                if (!skipCheck)
                    throw ParseException.LabelNotFound(toFind.Name);

                return ushort.MaxValue;
            }

            return found.InstructionNumber;
        }

    }
}
