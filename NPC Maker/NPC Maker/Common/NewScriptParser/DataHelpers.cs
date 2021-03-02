using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public static class DataHelpers
    {        
        public static void AddObjectToByteList(object Value, List<byte> ByteList)
        {
            if (Value is byte @byte)
                ByteList.Add(@byte);
            else if (Value is sbyte)
                ByteList.Add((byte)Value);
            else
            {
                if (ByteList.Count() % 2 != 0)
                    ByteList.Add(0);

                if (Value is UInt16 @int)
                    ByteList.AddRange(Program.BEConverter.GetBytes(@int));
                else if (Value is Int16 int1)
                    ByteList.AddRange(Program.BEConverter.GetBytes(int1));
                else
                {
                    while (ByteList.Count() % 4 != 0)
                        ByteList.Add(0);

                    if (Value is UInt32 int2)
                        ByteList.AddRange(Program.BEConverter.GetBytes(int2));
                    if (Value is Int32 int3)
                        ByteList.AddRange(Program.BEConverter.GetBytes(int3));
                    if (Value is float single)
                        ByteList.AddRange(Program.BEConverter.GetBytes(single));
                    if (Value is decimal)
                        ByteList.AddRange(Program.BEConverter.GetBytes((float)Value));
                    else
                        throw new Exception();
                }
            }
        }

        public static void Ensure4ByteAlign(List<byte> ByteList)
        {
            while (ByteList.Count % 4 != 0)
                ByteList.Add(0);
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
    }
}
