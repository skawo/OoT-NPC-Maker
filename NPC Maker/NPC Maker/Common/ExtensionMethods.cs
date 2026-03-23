using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NPC_Maker
{
    public static class ExtensionMethods
    {
        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

        public static bool IsHex(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            return (text.Length >= 3 && text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) ||
                   (text.Length >= 4 && text.StartsWith("-0x", StringComparison.OrdinalIgnoreCase));
        }

        public static void AddRangeBigEndian(this List<byte> list, UInt16 item) => list.AddRange(BigEndian.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, UInt32 item) => list.AddRange(BigEndian.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, Int16 item) => list.AddRange(BigEndian.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, Int32 item) => list.AddRange(BigEndian.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, float item) => list.AddRange(BigEndian.GetBytes(item));
        public static void AddRangeBigEndian(this List<byte> list, float[] array)
        {
            foreach (var item in array)
                list.AddRangeBigEndian(item);
        }

        public static void AddRangeBigEndian(this List<byte> list, uint[] array)
        {
            foreach (var item in array)
                list.AddRangeBigEndian(item);
        }
        public static void AddRangeBigEndian(this List<byte> list, ushort[] array)
        {
            foreach (var item in array)
                list.AddRangeBigEndian(item);
        }
        public static void AddRangeBigEndian(this List<byte> list, short[] array)
        {
            foreach (var item in array)
                list.AddRangeBigEndian(item);
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);

            if (pos < 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string LimitToCharNum(this string text, int maxLength)
        {
            if (text.Length > maxLength)
                return $"{text.Substring(0, maxLength)}...";
            else
                return text;
        }

        public static string WrapToLength(this string text, int maxLineLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var resultLines = new List<string>();
            var originalLines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (var line in originalLines)
            {
                if (line.Length <= maxLineLength)
                {
                    resultLines.Add(line);
                    continue;
                }

                var words = line.Split(' ');
                var currentLine = "";

                foreach (var word in words)
                {
                    var next = string.IsNullOrEmpty(currentLine)
                        ? word
                        : currentLine + " " + word;

                    if (next.Length > maxLineLength)
                    {
                        resultLines.Add(currentLine);
                        currentLine = word;
                    }
                    else
                        currentLine = next;
                }

                if (!string.IsNullOrEmpty(currentLine))
                    resultLines.Add(currentLine);
            }

            return string.Join(Environment.NewLine, resultLines);
        }

        public static string AppendQuotation(this string text)
        {
            return "\"" + text + "\"";
        }

        public static UInt32 HexLeading2UInt32(this string text)
        {
            return text.TrimStart('0') == "" ? (UInt32)0 : UInt32.Parse(text.TrimStart('0'), System.Globalization.NumberStyles.HexNumber);
        }

        public static string StripPunctuation(this string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!char.IsPunctuation(c) || c == '\'')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static bool DoubleWaitForExit(this System.Diagnostics.Process process)
        {
            var result = process.WaitForExit(500);
            if (result)
            {
                process.WaitForExit();
            }
            return result;
        }

        public static string FilenameFromPath(this string path)
        {
            // Create hash for uniqueness
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Program.Utf8.GetBytes(Path.GetFileName(path)));
                path = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 8);
            }

            return path;
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