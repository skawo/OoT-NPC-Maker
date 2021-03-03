using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public static class DataHelpers
    {        
        public static byte SmooshTwoValues(byte a, byte b, int offset)
        {
            byte o = 0;

            o |= (byte)(a << offset);
            return o |= b;
        }

        public static void AddObjectToByteList(object Value, List<byte> ByteList)
        {
            if (Value.GetType() == typeof(byte))
                ByteList.Add((byte)Value);
            else if (Value.GetType() == typeof(sbyte))
                ByteList.Add((byte)Value);
            else
            {
                if (ByteList.Count() % 2 != 0)
                    ByteList.Add(0);

                if (Value.GetType() == typeof(UInt16))
                    ByteList.AddRange(Program.BEConverter.GetBytes((UInt16)Value));
                else if(Value.GetType() == typeof(Int16))
                    ByteList.AddRange(Program.BEConverter.GetBytes((Int16)Value));
                else
                {
                    while (ByteList.Count() % 4 != 0)
                        ByteList.Add(0);

                    if (Value.GetType() == typeof(UInt32))
                        ByteList.AddRange(Program.BEConverter.GetBytes((UInt32)Value));
                    else if(Value.GetType() == typeof(Int32))
                        ByteList.AddRange(Program.BEConverter.GetBytes((Int32)Value));
                    else if(Value.GetType() == typeof(float))
                        ByteList.AddRange(Program.BEConverter.GetBytes((float)Value));
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(Value.GetType().ToString());
                        throw new Exception();
                    }
                }
            }
        }

        public static void Ensure4ByteAlign(List<byte> ByteList)
        {
            while (ByteList.Count % 4 != 0)
                ByteList.Add(0);
        }

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
    }
}
