using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NPC_Maker
{
    public static class Helpers
    {
        public static UInt16 GetOcarinaTime(string MilitaryTimeString)
        {
            string ExceptionMsg = "Time is in wrong format!";

            try
            {
                string[] HourMinute = MilitaryTimeString.Split(':');

                if (HourMinute.Length != 2)
                    throw new Exception(ExceptionMsg);

                byte Hour = Convert.ToByte(HourMinute[0]);
                byte Minute = Convert.ToByte(HourMinute[1]);

                if ((Hour > 23) || (Minute > 59))
                    throw new Exception(ExceptionMsg);

                double Time = Math.Ceiling((((Hour * 60) + Minute) * (float)((float)UInt16.MaxValue / (float)1440)));

                return Convert.ToUInt16(Time);
            }
            catch (Exception)
            {
                throw new Exception(ExceptionMsg);
            }
        }

        public static DateTime GetTimeFromOcarinaTime(UInt16 Value)
        {
            float Minutes = (float)Value / (float)((float)UInt16.MaxValue / (float)1440);

            int Hour = (int)(Minutes / 60);
            int Minute = (int)(Minutes - (Hour * 60));

            DateTime Out = new DateTime(2000, 01, 01, Hour, Minute, 0);

            return Out;
        }

        public static void Ensure2ByteAlign(List<byte> ByteList)
        {
            while (ByteList.Count % 2 != 0)
                ByteList.Add(0);
        }

        public static void Ensure4ByteAlign(List<byte> ByteList)
        {
            while (ByteList.Count % 4 != 0)
                ByteList.Add(0);
        }

        public static void ErrorIfExpectedLenWrong(List<byte> ByteList, int Len)
        {
            if (Len != ByteList.Count)
                System.Windows.Forms.MessageBox.Show($"Critical error: Got wrong amount of bytes.");
        }

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
                else if (Value.GetType() == typeof(Int16))
                    ByteList.AddRange(Program.BEConverter.GetBytes((Int16)Value));
                else
                {
                    while (ByteList.Count() % 2 != 0)
                        ByteList.Add(0);

                    if (Value.GetType() == typeof(UInt32))
                        ByteList.AddRange(Program.BEConverter.GetBytes((UInt32)Value));
                    else if (Value.GetType() == typeof(Int32))
                        ByteList.AddRange(Program.BEConverter.GetBytes((Int32)Value));
                    else if (Value.GetType() == typeof(float))
                        ByteList.AddRange(Program.BEConverter.GetBytes((float)Value));
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(Value.GetType().ToString());
                        throw new Exception();
                    }
                }
            }
        }

        public static byte MakeByte(bool a = false, bool b = false, bool c = false, bool d = false, bool e = false, bool f = false, bool g = false, bool h = false)
        {
            return MakeByte(Convert.ToByte(a), Convert.ToByte(b), Convert.ToByte(c), Convert.ToByte(d), Convert.ToByte(e), Convert.ToByte(f), Convert.ToByte(g), Convert.ToByte(h));
        }

        public static byte MakeByte(byte a = 0, byte b = 0, byte c = 0, byte d = 0, byte e = 0, byte f = 0, byte g = 0, byte h = 0)
        {
            byte res = 0;

            res |= (byte)((a != 0 ? 1 : 0) << 7);
            res |= (byte)((b != 0 ? 1 : 0) << 6);
            res |= (byte)((c != 0 ? 1 : 0) << 5);
            res |= (byte)((d != 0 ? 1 : 0) << 4);
            res |= (byte)((e != 0 ? 1 : 0) << 3);
            res |= (byte)((f != 0 ? 1 : 0) << 2);
            res |= (byte)((g != 0 ? 1 : 0) << 1);
            res |= (byte)((h != 0 ? 1 : 0) << 0);

            return res;
        }
    }
}
