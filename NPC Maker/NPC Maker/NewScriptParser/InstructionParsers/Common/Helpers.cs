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
    }
}
