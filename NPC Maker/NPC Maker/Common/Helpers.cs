using Newtonsoft.Json;
using NPC_Maker.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace NPC_Maker
{
    public static class Helpers
    {
        public static string StripTerminalControlCodes(string s)
        {
            return Regex.Replace(s, @"\x1B\[[^@-~]*[@-~]", "");
        }

        public static float GetScaleFontSize(float baseSize = 8.25f)
        {
            return (baseSize * Program.Settings.GUIScale);
        }

        public static void AdjustFormScale(Form f)
        {
            if (Program.Settings.GUIScale == 1.0f)
                return;

            float fontSize = Helpers.GetScaleFontSize();

            f.Font = new Font(f.Font.FontFamily, fontSize);
            Helpers.AdjustControlScale(f);
        }

        public static string NormalizeExtPath(string path)
        {
            // Always replace the longer (more specific) path first
            if (Program.Settings.ProjectPath.Length >= Program.ExecPath.Length)
            {
                path = Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, path, Lists.ProjectPathToken);
                path = Helpers.ReplacePathWithToken(Program.ExecPath, path, Lists.ProgramPathToken);
            }
            else
            {
                path = Helpers.ReplacePathWithToken(Program.ExecPath, path, Lists.ProgramPathToken);
                path = Helpers.ReplacePathWithToken(Program.Settings.ProjectPath, path, Lists.ProjectPathToken);
            }
            return path;
        }

        public static string MakePathRelativeToProjectPath(string path)
        {
            string projectPath = Path.GetFullPath(Program.Settings.ProjectPath);
            string fullPath = Path.GetFullPath(path);

            Uri projectUri = new Uri(projectPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
            Uri fileUri = new Uri(fullPath);
            path = Uri.UnescapeDataString(projectUri.MakeRelativeUri(fileUri).ToString())
                       .Replace('/', Path.DirectorySeparatorChar);
            return path;
        }

        public static string DenormalizeExtPath(string path, bool relativeToProjectPath = false)
        {
            if (Program.Settings.ProjectPath.Length >= Program.ExecPath.Length)
            {
                path = Helpers.ReplaceTokenWithPath(Program.Settings.ProjectPath, path, Lists.ProjectPathToken);
                path = Helpers.ReplaceTokenWithPath(Program.ExecPath, path, Lists.ProgramPathToken);
            }
            else
            {
                path = Helpers.ReplaceTokenWithPath(Program.ExecPath, path, Lists.ProgramPathToken);
                path = Helpers.ReplaceTokenWithPath(Program.Settings.ProjectPath, path, Lists.ProjectPathToken);
            }
            path = path.Replace(Lists.GameVersionPathToken, Lists.GameVersionStrings[Program.Settings.Library][(int)Program.Settings.GameVersion]);

            if (relativeToProjectPath)
                path = MakePathRelativeToProjectPath(path);

            return path;
        }

        private static void AdjustControlForNeighbour(Control ctrl)
        {
            int newHeight = ctrl.Height;

            if (ctrl.Parent != null)
            {
                if (ctrl.Parent is Panel par)
                {
                    int maxRight = par.ClientSize.Width - 5;

                    foreach (Control sibling in par.Controls)
                    {
                        if (sibling == ctrl) continue;

                        if (Math.Abs(sibling.Top - ctrl.Top) < 2) // 2 pixels tolerance
                        {
                            if (sibling.Left > ctrl.Left)
                            {
                                maxRight = Math.Min(maxRight, sibling.Left);
                                newHeight = sibling.Height;
                            }
                        }
                    }

                    ctrl.Width = maxRight - ctrl.Left - 5;
                }
            }

            ctrl.Height = newHeight;
        }

        public static void AdjustControlScale(Control ctr)
        {
            if (Program.Settings.GUIScale == 1.0f)
                return;

            float fontSize = GetScaleFontSize();

            foreach (Control ctrl in ctr.Controls)
            {
                if (ctrl is ScriptEditor)
                {
                    (ctrl as ScriptEditor).SetupScale();
                }
                if (ctrl is DataGridView)
                {
                    ctrl.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as DataGridView).DefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as DataGridView).ColumnHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, Math.Min(11, fontSize));
                    (ctrl as DataGridView).RowHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                }
                else if (ctrl is SegmentDataGrid)
                {
                    ctrl.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as SegmentDataGrid).Grid.DefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                    (ctrl as SegmentDataGrid).Grid.ColumnHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, Math.Min(11, fontSize));
                    (ctrl as SegmentDataGrid).Grid.RowHeadersDefaultCellStyle.Font = new Font(ctr.Font.FontFamily, fontSize);
                }
                else if (ctrl is FCTB_Mono)
                {
                    (ctrl as FCTB_Mono).Font = new Font(ctr.Font.FontFamily, fontSize);
                }
                else if (ctrl is DateTimePicker)
                {
                    if (Program.IsRunningUnderMono)
                    {
                        AdjustControlForNeighbour(ctrl);
                        ctrl.Font = new Font(ctr.Font.FontFamily, Math.Max(8.25f, fontSize - 3));
                    }
                }
                else if (ctrl is NumericUpDown)
                {
                    if (Program.IsRunningUnderMono)
                    {
                        AdjustControlForNeighbour(ctrl);
                        ctrl.Font = new Font(ctr.Font.FontFamily, Math.Max(8.25f, fontSize - 2));
                    }
                }

                if (ctrl.HasChildren)
                    AdjustControlScale(ctrl);
            }
        }

        public static string TruncatePath(string path, int maxLength = 60)
        {
            if (path.Length <= maxLength) return path;

            string fileName = Path.GetFileName(path);
            string ellipsis = "...";
            int keepLength = maxLength - fileName.Length - ellipsis.Length;

            if (keepLength <= 0) return ellipsis + fileName;

            return path.Substring(0, keepLength) + ellipsis + fileName;
        }

        public static UInt32 HexConvertToUInt32(string value)
        {
            if (value.IsHex())
                return Convert.ToUInt32(value, 16);
            else
                return Convert.ToUInt32(value);
        }

        public static UInt16 HexConvertToUInt16(string value)
        {
            if (value.IsHex())
                return Convert.ToUInt16(value, 16);
            else
                return Convert.ToUInt16(value);
        }

        public static byte HexConvertToByte(string value)
        {
            if (value.IsHex())
                return Convert.ToByte(value, 16);
            else
                return Convert.ToByte(value);
        }

        public static Int32 HexConvertToInt32(string value)
        {
            if (value.IsHex())
                return Convert.ToInt32(value, 16);
            else
                return Convert.ToInt32(value);
        }

        public static Int16 HexConvertToInt16(string value)
        {
            if (value.IsHex())
                return Convert.ToInt16(value, 16);
            else
                return Convert.ToInt16(value);
        }

        public static sbyte HexConvertToSByte(string value)
        {
            if (value.IsHex())
                return Convert.ToSByte(value, 16);
            else
                return Convert.ToSByte(value);
        }

        public static string GetDefinesStringFromH(string HeaderPath)
        {
            Dictionary<string, string> hDefines = Helpers.GetDefinesFromHeaders(HeaderPath);

            return string.Join(
                Environment.NewLine,
                hDefines
                    .Where(kvp => HasReplacement(kvp.Value))
                    .Select(kvp => $"#define H_{kvp.Key} {kvp.Value}")
            );
        }

        private static bool HasReplacement(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }


        public static List<string> SplitHeaderDefsString(string headerDefinitionString)
        {
            if (string.IsNullOrEmpty(headerDefinitionString))
                return new List<string> { "", "" };

            var parts = headerDefinitionString.Split(new[] { ';' }, 2);

            if (parts.Length == 1)
                return new List<string> { "", parts[0] };

            return new List<string> { parts[0], parts[1] };
        }

        public static Common.HDefine SelectOffsetFileStartFromH(NPCEntry entry, string CurOffs, string CurFileSt)
        {
            if (entry == null || String.IsNullOrEmpty(entry.HeaderPath))
                return null;

            Dictionary<string, string> hDict = Helpers.GetDefinesFromHeaders(entry.HeaderPath);

            if (hDict.Count == 0)
                return null;

            Windows.OffsetFileStartPicker com = new Windows.OffsetFileStartPicker(hDict.Keys.ToList(), CurOffs, CurFileSt);

            if (com.ShowDialog() == DialogResult.OK)
            {
                string Offset = com.SelectedIndexOffset == 0 ? "" : com.SelectedOptionOffset;
                string FileStart = com.SelectedIndexFileStart == 0 ? "" : com.SelectedOptionFileStart;

                string OffsetVal = com.SelectedIndexOffset == 0 ? "" : hDict[com.SelectedOptionOffset];
                string FileStartVal = com.SelectedIndexFileStart == 0 ? "" : hDict[com.SelectedOptionFileStart];

                return new Common.HDefine(Offset, OffsetVal, FileStart, FileStartVal);
            }
            else
                return null;
        }

        public static Common.HDefine SelectSingleFromH(NPCEntry entry)
        {
            if (entry == null || String.IsNullOrEmpty(entry.HeaderPath))
                return null;

            Dictionary<string, string> hDict = Helpers.GetDefinesFromHeaders(entry.HeaderPath);

            if (hDict.Count == 0)
                return null;

            Windows.ComboPicker com = new Windows.ComboPicker(hDict.Keys.ToList(), "Select symbol from header...", "Selection", true);

            if (com.ShowDialog() == DialogResult.OK)
                return new Common.HDefine(com.SelectedOption, hDict[com.SelectedOption], "", "");
            else
                return null;
        }

        public static string RunBash(string commandLine)
        {
            StringBuilder errorBuilder = new StringBuilder();
            StringBuilder outputBuilder = new StringBuilder();
            var arguments = $"-c \"{commandLine}\"";

            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "bash",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };

            process.Start();
            process.OutputDataReceived += (_, args) => { outputBuilder.AppendLine(args.Data); };
            process.BeginOutputReadLine();
            process.ErrorDataReceived += (_, args) => { errorBuilder.AppendLine(args.Data); };
            process.BeginErrorReadLine();
            if (!process.DoubleWaitForExit())
            {
                var timeoutError = $@"Process timed out. Command line: bash {arguments}.Output: {outputBuilder}Error: {errorBuilder}";
                throw new Exception(timeoutError);
            }
            if (process.ExitCode == 0)
            {
                return outputBuilder.ToString();
            }

            var error = $@"Could not execute process. Command line: bash {arguments}.Output: {outputBuilder} Error: {errorBuilder}";
            throw new Exception(error);
        }

        public static void ResetVerticalScrollbar(Control c)
        {
            foreach (Control control in c.Controls)
            {
                if (control is VScrollBar vScrollBar)
                {
                    vScrollBar.Value = 0;
                    break;
                }
            }
        }

        public static void MakeNotResizableMonoSafe(System.Windows.Forms.Form f)
        {
            f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
            f.ShowInTaskbar = false;
        }
        public static string GenerateTemporaryFolderName()
        {
            return $"temp_{DateTime.Now.Ticks}_{System.Diagnostics.Process.GetCurrentProcess().Id}";
        }

        public static string GetBase64Hash(string s)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] bytes = Program.Utf8.GetBytes(s);
                byte[] hash = sha1.ComputeHash(bytes);

                // URL-safe Base64 without extra Replace allocations
                return Convert.ToBase64String(hash)
                    .TrimEnd('=')
                    .Replace('+', '_')
                    .Replace('/', '-');
            }
        }


        public static string GetBase64Hash(byte[] b)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(b);

                // URL-safe Base64 without extra Replace allocations
                return Convert.ToBase64String(hash)
                    .TrimEnd('=')
                    .Replace('+', '_')
                    .Replace('/', '-');
            }
        }


        public static string ReplacePathWithToken(string basePath, string fullPath, string token)
        {
            try
            {
                if (String.IsNullOrEmpty(basePath))
                    return fullPath;

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

        public static string[] ResolveSemicolonPaths(string PathsString, bool relativeToProjectPath = false)
        {
            string[] Paths = PathsString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < Paths.Length; i++)
            {
                if (!String.IsNullOrWhiteSpace(Paths[i]))
                {
                    Paths[i] = Helpers.DenormalizeExtPath(Paths[i], relativeToProjectPath);
                }
            }

            return Paths;
        }

        public static Dictionary<string, string> GetDefinesFromHeaders(string PathString)
        {
            string[] Paths = Helpers.ResolveSemicolonPaths(PathString);
            Dictionary<string, string> defines = new Dictionary<string, string>();

            try
            {
                foreach (string p in Paths)
                {
                    if (!String.IsNullOrEmpty(p))
                    {
                        Dictionary<string, string> dict;

                        if (Path.GetExtension(p) == ".xml")
                            dict = ParseDefinesXML(p);
                        else
                            dict = ParseDefinesH(p);

                        foreach (var f in dict)
                        {
                            string key = f.Key;
                            string val = f.Value;

                            while (defines.ContainsKey(key))
                                key += "_";

                            defines[key] = val;
                        }
                    }

                }
            }
            catch (Exception)
            {
            }

            return defines;
        }

        public static string GetOnlyDefinesFromH(string hPath)
        {
            var lines = File.ReadLines(hPath)
                .Select(l => l.Trim())
                .Where(l => l.StartsWith("#define "))
                .Select(l =>
                {
                    int commentIndex = l.IndexOf("//");
                    return commentIndex >= 0
                        ? l.Substring(0, commentIndex).TrimEnd()
                        : l;
                })
                // skip defines without a value
                .Where(l =>
                {
                    var parts = l.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    return parts.Length >= 3; // #define NAME VALUE
                })
                .ToList();

            return string.Join(Environment.NewLine, lines);
        }


        private static Dictionary<string, string> ParseDefinesH(string hPath)
        {
            var result = new Dictionary<string, string>();

            foreach (var rawLine in File.ReadLines(hPath))
            {
                var line = rawLine.Trim();

                if (!line.StartsWith("#define "))
                    continue;

                var remainder = line.Substring(8).Trim();

                var parts = remainder.Split(
                    new[] { ' ', '\t' },
                    2,
                    StringSplitOptions.RemoveEmptyEntries
                );

                string name = parts[0];
                string value = parts.Length > 1 ? parts[1].Trim() : string.Empty;

                if (!Regex.IsMatch(value, @"^(0x[0-9A-Fa-f]+|\d+)$"))
                    continue;

                // Handle duplicates
                string key = name;
                int counter = 1;
                while (result.ContainsKey(key))
                    key = $"{name}_{counter++}";

                result[key] = value;
            }

            return result;
        }


        private static Dictionary<string, string> ParseDefinesXML(string xmlPath)
        {
            string xmlContent = File.ReadAllText(xmlPath);
            var result = new Dictionary<string, string>();

            try
            {
                var doc = XDocument.Parse(xmlContent);

                var elementsWithNameAndOffset = doc.Descendants()
                    .Where(e => e.Attribute("Name") != null && e.Attribute("Offset") != null);

                foreach (var element in elementsWithNameAndOffset)
                {
                    string name = element.Attribute("Name").Value;
                    string offset = element.Attribute("Offset").Value;

                    // Handle duplicate names
                    string key = name;
                    int counter = 1;
                    while (result.ContainsKey(key))
                    {
                        key = $"{name}_{counter}";
                        counter++;
                    }

                    result[key] = offset;
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"Error parsing XML: {ex.Message}");
            }

            return result;
        }

        public static Common.HDefine GetHDefineFromName(string name, Dictionary<string, string> defines)
        {
            if (name != null && defines.ContainsKey(name))
            {
                Common.HDefine h = new Common.HDefine(name, defines[name]);
                return h;
            }
            else
                return null;
        }

        public static string FixCygdrivePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // Split the path into segments
            string[] segments = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            // Find where cygdrive starts
            int cygdriveIndex = -1;
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i].Equals("cygdrive", StringComparison.OrdinalIgnoreCase))
                {
                    cygdriveIndex = i;
                    break;
                }
            }

            // If cygdrive is found, take everything from cygdrive onwards
            if (cygdriveIndex >= 0)
            {
                var relevantSegments = segments.Skip(cygdriveIndex).ToArray();

                // Convert cygdrive path to Windows path
                if (relevantSegments.Length >= 3 &&
                    relevantSegments[0].Equals("cygdrive", StringComparison.OrdinalIgnoreCase))
                {
                    // Replace cygdrive/d with D:
                    string driveLetter = relevantSegments[1].ToUpper() + ":";
                    var pathSegments = new string[] { driveLetter }
                        .Concat(relevantSegments.Skip(2))
                        .ToArray();

                    return string.Join("\\", pathSegments);
                }
            }

            // If no cygdrive found, return original path
            return path;
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

        public static void DeleteFileStartingWith(string path, string prefix, HashSet<string> fileList = null)
        {
            var files = fileList ?? Directory.EnumerateFiles(path);

            foreach (var file in files.ToList())
            {
                if (Path.GetFileName(file).StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    File.Delete(file);
                    fileList?.Remove(file);
                }
            }
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

        public static void Ensure2ByteAlign(List<byte> bytes)
        {
            if ((bytes.Count & 1) != 0)
                bytes.Add(0);
        }

        public static void Ensure4ByteAlign(List<byte> bytes)
        {
            int pad = (-bytes.Count) & 3;
            for (int i = 0; i < pad; i++)
                bytes.Add(0);
        }

        public static void Ensure16ByteAlign(List<byte> bytes)
        {
            int pad = (-bytes.Count) & 15;
            for (int i = 0; i < pad; i++)
                bytes.Add(0);
        }

        public static void ErrorIfExpectedLenWrong(List<byte> ByteList, int Len)
        {
            if (Len != ByteList.Count)
                BigMessageBox.Show($"Critical error: Got wrong amount of bytes.");
        }

        public static byte PutTwoValuesTogether(byte a, byte b, int offset)
        {
            byte o = 0;

            o |= (byte)(a << offset);
            return o |= b;
        }

        public static uint TwoInt16ToWord(short a, short b)
        {
            return (uint)((ushort)a | ((uint)(ushort)b << 16));
        }

        public static void AddObjectToByteList(object value, List<byte> byteList)
        {
            switch (value)
            {
                case byte b:
                    byteList.Add(b);
                    break;
                case sbyte sb:
                    byteList.Add((byte)sb);
                    break;
                case ushort us:
                    Ensure2ByteAlign(byteList);
                    AddUShort(byteList, us);
                    break;
                case short s:
                    Ensure2ByteAlign(byteList);
                    AddUShort(byteList, (ushort)s);
                    break;
                case uint ui:
                    Ensure4ByteAlign(byteList);
                    AddUInt(byteList, ui);
                    break;
                case int i:
                    Ensure4ByteAlign(byteList);
                    AddUInt(byteList, (uint)i);
                    break;
                case float f:
                    Ensure4ByteAlign(byteList);
                    AddFloat(byteList, f);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported type: {0}", value != null ? value.GetType().ToString() : "null"));
            }
        }

        private static unsafe void AddFloat(List<byte> list, float value)
        {
            uint bits = *(uint*)&value;
            list.Add((byte)(bits >> 24));
            list.Add((byte)(bits >> 16));
            list.Add((byte)(bits >> 8));
            list.Add((byte)bits);
        }

        private static void AddUShort(List<byte> list, ushort value)
        {
            // Big-endian
            list.Add((byte)(value >> 8));
            list.Add((byte)value);
        }

        private static void AddUInt(List<byte> list, uint value)
        {
            // Big-endian
            list.Add((byte)(value >> 24));
            list.Add((byte)(value >> 16));
            list.Add((byte)(value >> 8));
            list.Add((byte)value);
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
            float newCurrentValue = location1;

            while (true)
            {
                float currentValue = newCurrentValue;
                float newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue.Equals(currentValue))
                    return newValue;
            }
        }

        public static T Clone<T>(object Data)
        {
            string t = JsonConvert.SerializeObject(Data);
            return JsonConvert.DeserializeObject<T>(t);
        }

        public static List<string> SplitToTrimmedLines(string text)
        {
            if (text == null) return null;
            return text
                .Split(Lists.NewlineSeparators, StringSplitOptions.None)
                .Select(x => x.TrimEnd())
                .ToList();
        }

    }
}
