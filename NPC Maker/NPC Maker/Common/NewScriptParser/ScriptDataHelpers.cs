﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public static class ScriptDataHelpers
    {
        public static void ErrorIfExpectedLenWrong(List<byte> ByteList, int Len)
        {
            if (Len != ByteList.Count)
                System.Windows.Forms.MessageBox.Show($"Critical error: Got wrong amount of bytes: {(Lists.Instructions)ByteList[0]}, data: {BitConverter.ToString(ByteList.ToArray())}");
        }

        public static string RandomString(ScriptParser Prs, int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string Out = "";

            do
            {
                Out = new string(Enumerable.Repeat(chars, length)
                            .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (Prs.RandomLabels.Contains(Out));

            Prs.RandomLabels.Add(Out);
            return Out;
        }

        public static void FindLabelAndAddToByteList(List<InstructionLabel> Labels, InstructionLabel ToFind, ref List<byte> Data)
        {
            if (ToFind.Name == Lists.Keyword_Label_Return || ToFind.Name == Lists.Keyword_Label_Null)
            {
                Helpers.AddObjectToByteList(UInt16.MaxValue, Data);
                return;
            }

            bool SkipCheck = false;
#if DEBUG
            if (Labels.Find(x => x.Name == Lists.Keyword_Debug_Skip_Label_Check) != null)
                SkipCheck = true;
#endif

            InstructionLabel Found = Labels.Find(x => x.Name == ToFind.Name);

            if (Found == null && !SkipCheck)
                throw ParseException.LabelNotFound(ToFind.Name);

            Helpers.AddObjectToByteList(Found == null ? UInt16.MaxValue : Found.InstructionNumber, Data);
        }
    }
}