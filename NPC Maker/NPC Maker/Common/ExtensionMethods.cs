using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);

            if (pos < 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string AppendQuotation(this string text)
        {
            return "\"" + text + "\"";
        }
        
        public static UInt32 HexLeading2UInt32(this string text)
        {
            return text.TrimStart('0') == "" ? (UInt32)0: UInt32.Parse(text.TrimStart('0'), System.Globalization.NumberStyles.HexNumber);
        }

        public static string StripPunctuation(this string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
public class JsonTextWriterEx : JsonTextWriter
{
    public string NewLine { get; set; }

    public JsonTextWriterEx(TextWriter textWriter) : base(textWriter)
    {
        NewLine = Environment.NewLine;
    }

    protected override void WriteIndent()
    {
        if (Formatting == Formatting.Indented)
        {
            WriteWhitespace(NewLine);
            int currentIndentCount = Top * Indentation;
            for (int i = 0; i < currentIndentCount; i++)
                WriteIndentSpace();
        }
    }
}