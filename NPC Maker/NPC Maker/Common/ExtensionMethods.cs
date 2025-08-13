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

        public static Bitmap SetAlpha(this Bitmap bmpIn, int alpha)
        {
            Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
            float a = alpha / 255f;
            Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

            float[][] matrixItems =
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, a, 0},
                new float[] {0, 0, 0, 0, 1}
            };

            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmpOut))
                g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

            return bmpOut;
        }

        public static void DrawImageSourceCopySafe(this Bitmap targetBmp, Bitmap source, int x, int y)
        {
            int width = Math.Min(source.Width, targetBmp.Width - x);
            int height = Math.Min(source.Height, targetBmp.Height - y);

            if (width <= 0 || height <= 0) return;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Color sourcePixel = source.GetPixel(col, row);
                    targetBmp.SetPixel(x + col, y + row, sourcePixel);
                }
            }
        }

        public static Bitmap ResizeImageKeepAspectRatio(this Bitmap originalImage, int maxWidth, int maxHeight)
        {
            if (maxWidth == originalImage.Width && maxHeight == originalImage.Height)
                return originalImage;

            float aspectRatio = (float)originalImage.Width / originalImage.Height;
            int newWidth, newHeight;

            if (originalImage.Width > originalImage.Height)
            {
                newWidth = Math.Min(maxWidth, originalImage.Width);
                newHeight = (int)(newWidth / aspectRatio);

                if (newHeight > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = (int)(newHeight * aspectRatio);
                }
            }
            else
            {
                newHeight = Math.Min(maxHeight, originalImage.Height);
                newWidth = (int)(newHeight * aspectRatio);

                if (newWidth > maxWidth)
                {
                    newWidth = maxWidth;
                    newHeight = (int)(newWidth / aspectRatio);
                }
            }

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;

                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }


        public static string FilenameFromPath(this string path)
        {
            // Create hash for uniqueness
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(path));
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