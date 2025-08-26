using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace NPC_Maker
{
    public static class Helpers
    {

        public static void MakeNotResizableMonoSafe(System.Windows.Forms.Form f)
        {
            f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
            f.ShowInTaskbar = false;
        }

        public static string GetBase64Hash(SHA1 hasher, string s)
        {
            return Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace("+", "_").Replace("/", "-").Replace("=", "");
        }

        public static string ReplacePathWithToken(string basePath, string fullPath, string token)
        {
            try
            {
                string normalizedBasePath = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string normalizedFullPath = Path.GetFullPath(fullPath);

                StringComparison comparison = Environment.OSVersion.Platform == PlatformID.Unix ?
                    StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                if (string.Equals(normalizedBasePath, normalizedFullPath, comparison))
                    return token;

                string basePathWithSeparator = normalizedBasePath + Path.DirectorySeparatorChar;

                if (normalizedFullPath.StartsWith(basePathWithSeparator, comparison))
                {
                    string relativePortion = normalizedFullPath.Substring(basePathWithSeparator.Length);
                    return token + "/" + relativePortion.Replace(Path.DirectorySeparatorChar, '/');
                }

                return fullPath;
            }
            catch (Exception)
            {
                return fullPath;
            }
        }

        public static string ReplaceTokenWithPath(string basePath, string tokenizedPath, string token)
        {
            try
            {
                if (String.IsNullOrEmpty(basePath))
                    basePath = Program.ExecPath;

                string normalizedBasePath = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                if (string.Equals(tokenizedPath, token, StringComparison.Ordinal))
                    return normalizedBasePath;

                string tokenWithSeparator = token + "/";
                if (tokenizedPath.StartsWith(tokenWithSeparator, StringComparison.Ordinal))
                {
                    string relativePortion = tokenizedPath.Substring(tokenWithSeparator.Length);
                    string normalizedRelativePortion = relativePortion.Replace('/', Path.DirectorySeparatorChar);
                    return Path.Combine(normalizedBasePath, normalizedRelativePortion);
                }

                return tokenizedPath;
            }
            catch (Exception)
            {
                return tokenizedPath;
            }
        }

        public static Dictionary<string, string> GetDefinesFromH(string Path)
        {
            Dictionary<string, string> defines = new Dictionary<string, string>();

            try
            {
                if (!String.IsNullOrEmpty(Path))
                    defines = CCode.GetDefinesFromH(Helpers.ReplaceTokenWithPath(Program.Settings.ProjectPath, Path, "{PROJECTPATH}"));
            }
            catch (Exception)
            {
            }

            return defines;
        }

        public static Common.HDefine GetDefineFromName(string name, Dictionary<string, string> defines)
        {
            if (name != null && defines.ContainsKey(name))
            {
                Common.HDefine h = new Common.HDefine(name, defines[name]);
                return h;
            }
            else
                return null;
        }

        public static Color TryGetColorWithName(Color color)
        {
            var colorLookup = typeof(Color)
                   .GetProperties(BindingFlags.Public | BindingFlags.Static)
                   .Select(f => (Color)f.GetValue(null, null))
                   .Where(c => c.IsNamedColor)
                   .ToLookup(c => c.ToArgb());

            if (colorLookup[color.ToArgb()].Count() != 0)
                return colorLookup[color.ToArgb()].First();
            else
                return color;
        }
        public static bool DgCheckAddSanity(object entry, object[] current, int entrycount, int rowindex)
        {
            if (entry != null && rowindex < entrycount && entry != current[rowindex])
                return false;
            else if (entry != null && rowindex == entrycount)
                return true;
            else
                return false;
        }


        public static void DeleteFileStartingWith(string Path, string Prefix)
        {
            string[] f = System.IO.Directory.GetFiles(Path);

            List<string> s = f.Where(x => System.IO.Path.GetFileName(x).StartsWith(Prefix)).ToList();

            foreach (string p in s)
                System.IO.File.Delete(p);
        }

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

        public static byte PutTwoValuesTogether(byte a, byte b, int offset)
        {
            byte o = 0;

            o |= (byte)(a << offset);
            return o |= b;
        }

        public static UInt32 TwoInt16ToWord(Int16 a, Int16 b)
        {
            List<byte> Bytes = new List<byte>();

            Bytes.AddRange(BitConverter.GetBytes(a));
            Bytes.AddRange(BitConverter.GetBytes(b));

            return BitConverter.ToUInt32(Bytes.ToArray(), 0);
        }

        public static void AddObjectToByteList(object value, List<byte> byteList)
        {
            if (value.GetType() == typeof(byte) || value.GetType() == typeof(sbyte))
            {
                byteList.Add((byte)value);
            }
            else
            {
                if (value.GetType() == typeof(UInt16) || value.GetType() == typeof(Int16))
                {
                    Ensure2ByteAlign(byteList);
                    byteList.AddRange(Program.BEConverter.GetBytes((dynamic)value));
                }
                else if (value.GetType() == typeof(UInt32) || value.GetType() == typeof(Int32) || value.GetType() == typeof(float))
                {
                    Ensure4ByteAlign(byteList);
                    byteList.AddRange(Program.BEConverter.GetBytes((dynamic)value));
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(value.GetType().ToString());
                    throw new Exception();
                }
            }
        }

        public static byte MakeByte(bool a = false, bool b = false, bool c = false, bool d = false, bool e = false, bool f = false, bool g = false, bool h = false)
        {
            return MakeByte(Convert.ToByte(a),
                            Convert.ToByte(b),
                            Convert.ToByte(c),
                            Convert.ToByte(d),
                            Convert.ToByte(e),
                            Convert.ToByte(f),
                            Convert.ToByte(g),
                            Convert.ToByte(h));
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

        public static double AddInterlocked(ref float location1, float value)
        {
            float newCurrentValue = location1; // non-volatile read, so may be stale
            while (true)
            {
                float currentValue = newCurrentValue;
                float newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue.Equals(currentValue)) // see "Update" below
                    return newValue;
            }
        }

        public static T Clone<T>(object Data)
        {
            string t = JsonConvert.SerializeObject(Data);
            return JsonConvert.DeserializeObject<T>(t);
        }
    }
}
