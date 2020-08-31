using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public static class ParserHelpers
    {
        public static object GetValueAndCheckRange(string[] Splitstring, int Index, int Min, int Max)
        {
            Int32? Value = ScriptHelpers.Helper_ConvertToInt32(Splitstring[Index]);

            if (Value == null)
                throw ParseException.ParamConversionError(Splitstring);

            if (Value < Min || Value > Max)
                throw ParseException.ParamOutOfRange(Splitstring);

            return Value;
        }
        
        public static void AddObjectToByteList(object Value, List<byte> ByteList)
        {
            if (Value is byte)
                ByteList.Add((byte)Value);
            else if (Value is sbyte)
                ByteList.Add((byte)Value);
            else
            {
                if (ByteList.Count() % 2 != 0)
                    ByteList.Add(0);

                if (Value is UInt16)
                    ByteList.AddRange(Program.BEConverter.GetBytes((UInt16)Value));
                else if (Value is Int16)
                    ByteList.AddRange(Program.BEConverter.GetBytes((Int16)Value));
                else
                {
                    while (ByteList.Count() % 4 != 0)
                        ByteList.Add(0);

                    if (Value is UInt32)
                        ByteList.AddRange(Program.BEConverter.GetBytes((UInt32)Value));
                    if (Value is Int32)
                        ByteList.AddRange(Program.BEConverter.GetBytes((Int32)Value));
                    if (Value is float)
                        ByteList.AddRange(Program.BEConverter.GetBytes((float)Value));
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
    }
}
