using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class ExtensionMethods
    {
        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

        public static void AddRangeBigEndian(this List<byte> list, UInt16 item) => list.AddRange(Program.BEConverter.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, UInt32 item) => list.AddRange(Program.BEConverter.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, Int16 item) => list.AddRange(Program.BEConverter.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, Int32 item) => list.AddRange(Program.BEConverter.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, float item) => list.AddRange(Program.BEConverter.GetBytes(item));
    }
}
