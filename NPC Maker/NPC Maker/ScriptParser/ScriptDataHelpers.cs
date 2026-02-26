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

        private static readonly char[] _labelChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        public static string GetRandomLabelString(ScriptParser Prs, int length = 5)
        {
            char[] buffer = new char[4 + length];
            buffer[0] = 'l';
            buffer[1] = 'b';
            buffer[2] = 'l';
            buffer[3] = '_';

            while (true)
            {
                for (int i = 0; i < length; i++)
                    buffer[4 + i] = _labelChars[Program._random.Next(_labelChars.Length)];

                string randomLabel = new string(buffer);
                if (Prs.RandomLabels.Add(randomLabel))
                    return randomLabel;
            }
        }

        public static void FindLabelAndAddToByteList(Dictionary<string, InstructionLabel> Labels, InstructionLabel ToFind, ref List<byte> Data)
        {
            Helpers.AddObjectToByteList(FindLabel(Labels, ToFind), Data);
        }

        public static ushort FindLabel(Dictionary<string, InstructionLabel> labels, InstructionLabel toFind)
        {
            if (toFind.Name == Lists.Keyword_Label_Return ||
                toFind.Name == Lists.Keyword_Label_Null)
                return ushort.MaxValue;

            #if DEBUG
                    bool skipCheck = labels.ContainsKey(Lists.Keyword_Debug_Skip_Label_Check);
            #else
                    bool skipCheck = false;
            #endif

            InstructionLabel found;

            if (!labels.TryGetValue(toFind.Name, out found))
            {
                if (!skipCheck)
                    throw ParseException.LabelNotFound(toFind.Name);
                return ushort.MaxValue;
            }

            return found.InstructionNumber;
        }

    }
}
