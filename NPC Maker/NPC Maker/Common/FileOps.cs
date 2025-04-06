using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NPC_Maker
{
    public static class FileOps
    {
        public static NPCMakerSettings ParseSettingsJSON(string FileName)
        {
            if (!File.Exists(FileName))
                return new NPCMakerSettings();

            string Text = File.ReadAllText(FileName);
            NPCMakerSettings Deserialized = JsonConvert.DeserializeObject<NPCMakerSettings>(Text);

            return Deserialized;
        }

        public static void SaveSettingsJSON(string Path, NPCMakerSettings Data)
        {
            try
            {
                File.WriteAllText(Path, JsonConvert.SerializeObject(Data, Formatting.Indented));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Failed to write settings: {ex.Message}");
            }
        }

        public static NPCFile ParseNPCJsonFile(string FileName)
        {
            try
            {
                string Text = File.ReadAllText(FileName);

                var Version = JObject.Parse(Text).SelectToken("Version");

                NPCFile Deserialized = JsonConvert.DeserializeObject<NPCFile>(Text);


                if (Version == null || (int)Version < 2)
                {
                    Deserialized.Version = 2;
                    Version = 2;

                    for (int i = 0; i < Deserialized.Entries.Count; i++)
                    {
                        ScriptEntry Sc = new ScriptEntry()
                        {
                            Text = (string)JObject.Parse(Text).SelectToken($"Entries[{i}].Script"),
                            Name = "Script 1"
                        };

                        ScriptEntry Sc2 = new ScriptEntry()
                        {
                            Text = (string)JObject.Parse(Text).SelectToken($"Entries[{i}].Script2"),
                            Name = "Script 2"
                        };

                        Deserialized.Entries[i].Scripts.Add(Sc);
                        Deserialized.Entries[i].Scripts.Add(Sc2);
                    }
                }

                if ((int)Version < 3)
                {
                    Deserialized.Version = 3;
                    Version = 3;

                    for (int i = 0; i < Deserialized.Entries.Count; i++)
                    {
                        Deserialized.Entries[i].FileStart = 0;

                        foreach (var anim in Deserialized.Entries[i].Animations)
                            anim.FileStart = -1;

                        foreach (var dlist in Deserialized.Entries[i].ExtraDisplayLists)
                            dlist.FileStart = -1;

                        foreach (var seg in Deserialized.Entries[i].Segments)
                            foreach (var entry in seg)
                                entry.FileStart = -1;


                    }
                }

                if ((int)Version > 3)
                {
                    foreach (NPCEntry e in Deserialized.Entries)
                    {
                        foreach (ScriptEntry s in e.Scripts)
                            s.Text = String.Join(Environment.NewLine, s.TextLines.Select(x => x.TrimEnd()).ToList());

                        e.EmbeddedOverlayCode.Code = String.Join(Environment.NewLine, e.EmbeddedOverlayCode.CodeLines);
                    }

                    foreach (ScriptEntry s in Deserialized.GlobalHeaders)
                        s.Text = String.Join(Environment.NewLine, s.TextLines.Select(x => x.TrimEnd()).ToList());
                }

                if ((int)Version > 4)
                {
                    foreach (NPCEntry e in Deserialized.Entries)
                    {
                        foreach (MessageEntry s in e.Messages)
                            s.MessageText = String.Join(Environment.NewLine, s.MessageTextLines);
                    }
                }

                if ((int)Version < 6)
                {
                    foreach (NPCEntry e in Deserialized.Entries)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int idx = e.EmbeddedOverlayCode.FuncsRunWhen[i, 0];

                            if (idx >= 0)
                            {
                                if (e.EmbeddedOverlayCode.Functions.Count > idx)
                                    e.EmbeddedOverlayCode.SetFuncNames[i] = e.EmbeddedOverlayCode.Functions[idx].FuncName;
                                else
                                    e.EmbeddedOverlayCode.SetFuncNames[i] = "Not found?";
                            }
                        }
                    }

                    Version = 6;
                }

                if ((int)Version < 7)
                {
                    foreach (NPCEntry e in Deserialized.Entries)
                    {
                        List<string> s = e.EmbeddedOverlayCode.SetFuncNames.ToList();
                        s.Add("");
                        e.EmbeddedOverlayCode.SetFuncNames = s.ToArray();

                        int[,] FuncsRunWhen = new int[6, 2]
                        {
                            {-1, -1},
                            {-1, -1},
                            {-1, -1},
                            {-1, -1},
                            {-1, -1},
                            {-1, -1},
                        };

                        for (int i = 0; i < 5; i++)
                        {
                            FuncsRunWhen[i, 0] = e.EmbeddedOverlayCode.FuncsRunWhen[i, 0];
                            FuncsRunWhen[i, 1] = e.EmbeddedOverlayCode.FuncsRunWhen[i, 1];
                        }

                        e.EmbeddedOverlayCode.FuncsRunWhen = FuncsRunWhen;
                    }

                    Version = 7;
                }

                if ((int)Version >= 6)
                    Deserialized.CHeader = String.Join(Environment.NewLine, Deserialized.CHeaderLines.Select(x => x.TrimEnd()).ToList());

                // For cross-compatibility with Linux, update all messages converting linebreaks into native system linebreaks.
                foreach (NPCEntry e in Deserialized.Entries)
                {
                    foreach (MessageEntry entry in e.Messages)
                    {
                        entry.MessageText = Regex.Replace(entry.MessageText, @"\r?\n", Environment.NewLine);
                    }
                }

                Deserialized.Version = 7;
                Version = 7;

                return Deserialized;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Failed to read JSON: {ex.Message}");
                return null;
            }
        }

        public static void SaveNPCJSON(string Path, NPCFile Data)
        {
            try
            {
                NPCFile outD = Clone(Data);

                foreach (NPCEntry e in outD.Entries)
                {
                    foreach (ScriptEntry s in e.Scripts)
                    {
                        s.TextLines = Regex.Split(s.Text, "\r?\n").ToList();
                        s.TextLines.ForEach(x => x.TrimEnd());
                        s.Text = "";
                    }

                    foreach (MessageEntry entry in e.Messages)
                    {
                        entry.MessageText = Regex.Replace(entry.MessageText, Environment.NewLine, "\n");
                        entry.MessageTextLines = Regex.Split(entry.MessageText, "\r?\n").ToList();
                        entry.MessageText = "";
                    }

                    e.EmbeddedOverlayCode.CodeLines = Regex.Split(e.EmbeddedOverlayCode.Code, "\r?\n").ToList();
                    e.EmbeddedOverlayCode.Code = "";
                }

                foreach (ScriptEntry s in outD.GlobalHeaders)
                {
                    s.TextLines = Regex.Split(s.Text, "\r?\n").ToList();
                    s.TextLines.ForEach(x => x.TrimEnd());
                    s.Text = "";
                }
                
                outD.CHeaderLines = Regex.Split(outD.CHeader, "\r?\n").ToList();
                outD.CHeaderLines.ForEach(x => x.TrimEnd());
                outD.CHeader = "";

                File.WriteAllText(Path, JsonConvert.SerializeObject(outD, Formatting.Indented).Replace(Environment.NewLine, "\n"));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Failed to write JSON: {ex.Message}");
            }
        }

        public static NPCFile Clone(NPCFile Data)
        {
            string t = JsonConvert.SerializeObject(Data, Formatting.Indented);
            return JsonConvert.DeserializeObject<NPCFile>(t);
        }

        public static Dictionary<string, int> GetDictionary(string Filename)
        {
            Dictionary<string, int> Dict = new Dictionary<string, int>();

            string OffendingRow = "";

            try
            {
                string[] RawData = File.ReadAllLines(Filename);

                foreach (string Row in RawData)
                {
                    OffendingRow = Row;
                    string[] NameAndID = Row.Split(',');
                    Dict.Add(NameAndID[1], Convert.ToInt32(NameAndID[0]));
                }

                return Dict;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show($"{Filename} is missing or incorrect. ({OffendingRow})");
                return Dict;
            }
        }

        private static void ShowMsg(bool CLIMode, string Msg)
        {
            if (CLIMode)
                Console.WriteLine(Msg);
            else
                System.Windows.Forms.MessageBox.Show(Msg);

            return;
        }

        public static void SaveBinaryFile(string Path, NPCFile Data, bool CLIMode = false)
        {
            if (Data.Entries.Count() == 0)
            {
                ShowMsg(CLIMode, "Nothing to save.");
                return;
            }

            Progress pr = new Progress();

            try
            {
                int Offset = Data.Entries.Count() * 4 + 4;

                List<byte> EntryAddresses = new List<byte>();
                List<List<byte>> EntryData = new List<List<byte>>();
                List<string> ParseErrors = new List<string>();

                string CompErrors = "";

                if (Program.mw != null)
                {
                    pr.SetProgress(0);
                    pr.Show();
                    pr.Location = new System.Drawing.Point(Program.mw.Location.X + Program.mw.Width / 2 - pr.Width / 2, Program.mw.Location.Y + Program.mw.Height / 2 - pr.Height / 2);
                    pr.Refresh();
                }

                float ProgressPer = (float)((float)100 / (float)Data.Entries.Count);
                float CurProgress = 0;
                int EntriesDone = 0;

                string gh = String.Join(Environment.NewLine, Data.GlobalHeaders.Select(x => x.Text));
                string dicts = String.Join(
                                             JsonConvert.SerializeObject(Dicts.Actors),
                                             JsonConvert.SerializeObject(Dicts.ObjectIDs),
                                             JsonConvert.SerializeObject(Dicts.SFXes),
                                             JsonConvert.SerializeObject(Dicts.LinkAnims),
                                             JsonConvert.SerializeObject(Dicts.Music)
                                          );


                bool cacheInvalid = false;
                bool CcacheInvalid = false;
                string Ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

                // Check if the Global Headers changed - if they have, we need to redo everything.
                using (SHA1 s = SHA1.Create())
                {
                    string hash = Convert.ToBase64String(s.ComputeHash(Encoding.UTF8.GetBytes(gh))).Replace("+", "_").Replace("/", "-").Replace("=", "");

                    string cachedHeaders = System.IO.Path.Combine(Program.CachePath, $"gh_{Ver}" + hash);

                    if (!File.Exists(cachedHeaders))
                    {
                        Helpers.DeleteFileStartingWith(Program.CachePath, "gh_");
                        File.WriteAllText(cachedHeaders, "");
                        cacheInvalid = true;
                    }

                    hash = Convert.ToBase64String(s.ComputeHash(Encoding.UTF8.GetBytes(dicts))).Replace("+", "_").Replace("/", "-").Replace("=", "");
                    string cachedDicts = System.IO.Path.Combine(Program.CachePath, $"dicts_{Ver}" + hash);

                    if (!File.Exists(cachedDicts))
                    {
                        Helpers.DeleteFileStartingWith(Program.CachePath, "dicts_");
                        File.WriteAllText(cachedDicts, "");
                        cacheInvalid = true;
                    }

                    Data.CHeader = CCode.ReplaceGameVersionInclude(Data.CHeader);
                    hash = Convert.ToBase64String(s.ComputeHash(Encoding.UTF8.GetBytes(Data.CHeader))).Replace("+", "_").Replace("/", "-").Replace("=", "");
                    string cachedHeader = System.IO.Path.Combine(Program.CachePath, $"ch_{Ver}" + hash);

                    if (!File.Exists(cachedHeader))
                    {
                        Helpers.DeleteFileStartingWith(Program.CachePath, "ch_");
                        File.WriteAllText(cachedHeader, "");
                        CcacheInvalid = true;
                    }
                }

                foreach (NPCEntry Entry in Data.Entries)
                { 

                    if (Entry.IsNull == false)
                    {
                        Console.Write($"Processing entry {EntriesDone}: {Entry.NPCName}... ");

                        List<byte> EntryBytes = new List<byte>();

                        int CurLen = 0;

                        EntryBytes.Add(Entry.CutsceneID);
                        EntryBytes.Add(Entry.HeadLimb);
                        EntryBytes.Add(Entry.WaistLimb);
                        EntryBytes.Add(Entry.TargetLimb);
                        EntryBytes.Add(Entry.PathID);
                        EntryBytes.Add(Entry.BlinkSpeed);
                        EntryBytes.Add(Entry.TalkSpeed);
                        EntryBytes.Add(Entry.HierarchyType);
                        EntryBytes.Add(Entry.TalkSegment);
                        EntryBytes.Add(Entry.BlinkSegment);
                        EntryBytes.Add(Entry.AnimationType);
                        EntryBytes.Add(Entry.MovementType);
                        EntryBytes.Add(Entry.WaistHorizAxis);
                        EntryBytes.Add(Entry.WaistVertAxis);
                        EntryBytes.Add(Entry.HeadHorizAxis);
                        EntryBytes.Add(Entry.HeadVertAxis);
                        EntryBytes.Add(Entry.LookAtType);
                        EntryBytes.Add(Entry.TargetDistance);
                        EntryBytes.Add(Entry.EffectIfAttacked);
                        EntryBytes.Add(Entry.Mass);
                        EntryBytes.Add(Entry.Alpha);
                        EntryBytes.Add(Entry.LightLimb);
                        EntryBytes.Add(Entry.EnvironmentColor.R);
                        EntryBytes.Add(Entry.EnvironmentColor.G);
                        EntryBytes.Add(Entry.EnvironmentColor.B);
                        EntryBytes.Add(Entry.LightColor.R);
                        EntryBytes.Add(Entry.LightColor.G);
                        EntryBytes.Add(Entry.LightColor.B);
                        EntryBytes.Add(Entry.AnimInterpFrames);
                        EntryBytes.Add(0);
                        EntryBytes.Add(0);
                        EntryBytes.Add(0);

                        EntryBytes.Add(Convert.ToByte(Entry.HasCollision));
                        EntryBytes.Add(Convert.ToByte(Entry.PushesSwitches));
                        EntryBytes.Add(Convert.ToByte(Entry.IgnoreYAxis));
                        EntryBytes.Add(Convert.ToByte(Entry.IsAlwaysActive));
                        EntryBytes.Add(Convert.ToByte(Entry.IsAlwaysDrawn));
                        EntryBytes.Add(Convert.ToByte(Entry.ExecuteJustScript));
                        EntryBytes.Add(Convert.ToByte(Entry.ReactsIfAttacked));
                        EntryBytes.Add(Convert.ToByte(Entry.OpensDoors));
                        EntryBytes.Add(Convert.ToByte(Entry.CastsShadow));
                        EntryBytes.Add(Convert.ToByte(Entry.IsTargettable));
                        EntryBytes.Add(Convert.ToByte(Entry.LoopPath));
                        EntryBytes.Add(Convert.ToByte(Entry.EnvironmentColor.A > 0));
                        EntryBytes.Add(Convert.ToByte(Entry.FadeOut));
                        EntryBytes.Add(Convert.ToByte(Entry.GenLight));
                        EntryBytes.Add(Convert.ToByte(Entry.Glow));
                        EntryBytes.Add(Convert.ToByte(Entry.DEBUGShowCols));
                        EntryBytes.Add(Convert.ToByte(Entry.VisibleUnderLensOfTruth));
                        EntryBytes.Add(Convert.ToByte(Entry.Invisible));
                        EntryBytes.Add(Convert.ToByte(Entry.ExistInAllRooms));
                        EntryBytes.Add(Convert.ToByte(Entry.NumVars));
                        EntryBytes.Add(Convert.ToByte(Entry.NumFVars));
                        EntryBytes.Add(Convert.ToByte(Entry.DEBUGExDlistEditor));
                        EntryBytes.Add(Convert.ToByte(Entry.DEBUGLookAtEditor));
                        EntryBytes.Add(Convert.ToByte(Entry.DEBUGPrintToScreen));
                        
                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 56;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        EntryBytes.AddRangeBigEndian(Entry.ObjectID);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtDegreesVertical);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtDegreesHorizontal);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionRadius);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionHeight);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionYShift);
                        EntryBytes.AddRangeBigEndian(Entry.ShadowRadius);
                        EntryBytes.AddRangeBigEndian(Entry.MovementDistance);
                        EntryBytes.AddRangeBigEndian(Entry.MaxDistRoam);
                        EntryBytes.AddRangeBigEndian(Entry.PathStartNodeID);
                        EntryBytes.AddRangeBigEndian(Entry.PathEndNodeID);
                        EntryBytes.AddRangeBigEndian(Entry.MovementDelayTime);
                        EntryBytes.AddRangeBigEndian(Entry.TimedPathStart);
                        EntryBytes.AddRangeBigEndian(Entry.TimedPathEnd);
                        EntryBytes.AddRangeBigEndian(Entry.SfxIfAttacked);
                        EntryBytes.AddRangeBigEndian(Entry.NPCToRide);
                        EntryBytes.AddRangeBigEndian(Entry.LightRadius);
                        EntryBytes.AddRangeBigEndian(Entry.TargetPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.TargetPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.TargetPositionOffsets[2]);
                        EntryBytes.AddRangeBigEndian(Entry.ModelPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.ModelPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.ModelPositionOffsets[2]);
                        EntryBytes.AddRangeBigEndian(Entry.LightPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.LightPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.LightPositionOffsets[2]);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 52;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        EntryBytes.AddRangeBigEndian(Entry.ModelScale);
                        EntryBytes.AddRangeBigEndian(Entry.TalkRadius);
                        EntryBytes.AddRangeBigEndian(Entry.MovementSpeed);
                        EntryBytes.AddRangeBigEndian(Entry.GravityForce);
                        EntryBytes.AddRangeBigEndian(Entry.SmoothingConstant);
                        EntryBytes.AddRangeBigEndian(Entry.Hierarchy);
                        EntryBytes.AddRangeBigEndian(Entry.FileStart);
                        EntryBytes.AddRangeBigEndian(Entry.CullForward);
                        EntryBytes.AddRangeBigEndian(Entry.CullDown);
                        EntryBytes.AddRangeBigEndian(Entry.CullScale);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtPositionOffsets[2]);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 52;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #region Blink and talk patterns

                        string[] BlinkPat = new string[0];
                        string[] TalkPat = new string[0];

                        if (Entry.BlinkPattern != "")
                            BlinkPat = Entry.BlinkPattern.Split(',');

                        if (Entry.TalkPattern != "")
                            TalkPat = Entry.TalkPattern.Split(',');

                        if (BlinkPat.Length > 4 || TalkPat.Length > 4)
                        {
                            ShowMsg(CLIMode, $"{Entry.NPCName}: Talking and blinking patterns may only be 4 entries long!");
                            return;
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            if (BlinkPat.Length > i && (Entry.BlinkSegment - 8) >= 0)
                            {
                                int Index = Entry.Segments[Entry.BlinkSegment - 8].FindIndex(x => x.Name.ToLower() == BlinkPat[i].ToLower());

                                if (Index == -1)
                                {
                                    ShowMsg(CLIMode, $"{Entry.NPCName}: Couldn't find one of the blink pattern textures: " + BlinkPat[i]);
                                    return;
                                }
                                else
                                    EntryBytes.Add((byte)Index);
                            }
                            else
                                EntryBytes.Add((byte)0xFF);
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            if (TalkPat.Length > i)
                            {
                                int Index = Entry.Segments[Entry.TalkSegment - 8].FindIndex(x => x.Name.ToLower() == TalkPat[i].ToLower());

                                if (Index == -1)
                                {
                                    ShowMsg(CLIMode, $"{Entry.NPCName}: Couldn't find one of the talk pattern textures: " + TalkPat[i]);
                                    return;
                                }
                                else
                                    EntryBytes.Add((byte)Index);
                            }
                            else
                                EntryBytes.Add((byte)0xFF);
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 8;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Messages

                        List<byte> Header = new List<byte>();
                        List<byte> MsgData = new List<byte>();

                        int MsgOffset = 8 * Entry.Messages.Count();

                        foreach (MessageEntry Msg in Entry.Messages)
                        {
                            List<byte> Message = Msg.ConvertTextData(Entry.NPCName, !CLIMode);

                            if (Message == null)
                            {
                                Message = new List<byte>();

                                if (!ParseErrors.Contains(Entry.NPCName))
                                    ParseErrors.Add(Entry.NPCName);
                            }

                            Helpers.Ensure4ByteAlign(Message);
                            MsgData.AddRange(Message);

                            if (Message.Count > 640)
                            {
                                ShowMsg(CLIMode, $"{Entry.NPCName}: One of the messages ({Msg.Name}) has exceeded 640 bytes (the maximum allowed), and could not be saved.");
                                Message = new List<byte>();

                                if (!ParseErrors.Contains(Entry.NPCName))
                                    ParseErrors.Add(Entry.NPCName);
                            }

                            Header.AddRangeBigEndian(MsgOffset);
                            Header.Add(Msg.GetMessageTypePos());
                            Helpers.Ensure2ByteAlign(Header);
                            Header.AddRangeBigEndian((UInt16)Message.Count);

                            MsgOffset += Message.Count();
                        }

                        EntryBytes.AddRangeBigEndian(8 + Header.Count + MsgData.Count);
                        EntryBytes.AddRangeBigEndian(Offset + EntryBytes.Count + 8);
                        EntryBytes.AddRange(Header);
                        EntryBytes.AddRange(MsgData);

                        CurLen += 8 + Header.Count + MsgData.Count;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Animations

                        EntryBytes.AddRangeBigEndian((UInt32)Entry.Animations.Count());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        foreach (AnimationEntry Anim in Entry.Animations)
                        {
                            EntryBytes.AddRangeBigEndian((UInt32)Anim.Address);
                            EntryBytes.AddRangeBigEndian((UInt32)Anim.FileStart);
                            EntryBytes.AddRangeBigEndian((float)Anim.Speed);
                            EntryBytes.AddRangeBigEndian((UInt16)Anim.ObjID);
                            EntryBytes.Add(Anim.StartFrame);
                            EntryBytes.Add(Anim.EndFrame);
                            Helpers.Ensure4ByteAlign(EntryBytes);
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += (16 * Entry.Animations.Count());
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Extra display lists

                        EntryBytes.AddRangeBigEndian((UInt32)Entry.ExtraDisplayLists.Count);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        foreach (DListEntry Dlist in Entry.ExtraDisplayLists)
                        {
                            EntryBytes.AddRangeBigEndian(Dlist.Address);
                            EntryBytes.AddRangeBigEndian(Dlist.FileStart);
                            EntryBytes.AddRangeBigEndian(Dlist.TransX);
                            EntryBytes.AddRangeBigEndian(Dlist.TransY);
                            EntryBytes.AddRangeBigEndian(Dlist.TransZ);
                            EntryBytes.AddRangeBigEndian(Dlist.Scale);
                            EntryBytes.AddRangeBigEndian(Dlist.ObjectID);
                            EntryBytes.AddRangeBigEndian(Dlist.RotX);
                            EntryBytes.AddRangeBigEndian(Dlist.RotY);
                            EntryBytes.AddRangeBigEndian(Dlist.RotZ);
                            EntryBytes.AddRangeBigEndian(Dlist.Limb);
                            EntryBytes.Add((byte)Dlist.ShowType);
                            EntryBytes.Add(Dlist.Color.R);
                            EntryBytes.Add(Dlist.Color.G);
                            EntryBytes.Add(Dlist.Color.B);
                            Helpers.Ensure4ByteAlign(EntryBytes);
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 40 * Entry.ExtraDisplayLists.Count;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Colors

                        List<OutputColorEntry> ParsedColors = Entry.ParseColorEntries().OrderBy(x => x.LimbID).ToList();
                        EntryBytes.AddRangeBigEndian((UInt32)ParsedColors.Count());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        foreach (OutputColorEntry Col in ParsedColors)
                        {
                            EntryBytes.Add(Col.LimbID);
                            EntryBytes.Add(Col.R);
                            EntryBytes.Add(Col.G);
                            EntryBytes.Add(Col.B);
                            Helpers.Ensure4ByteAlign(EntryBytes);
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4 * ParsedColors.Count;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Extra segment data

                        List<byte> ExtraSegDataOffsets = new List<byte>();
                        List<byte> ExtraSegDataEntries = new List<byte>();
                        UInt32 SegOffset = 7 * 4;
                        CurLen += (int)SegOffset + 4;

                        foreach (List<SegmentEntry> Segment in Entry.Segments)
                        {
                            UInt32 SegBytes = (UInt32)(12 * Segment.Count);

                            if (SegBytes != 0)
                                ExtraSegDataOffsets.AddRangeBigEndian(SegOffset);
                            else
                                ExtraSegDataOffsets.AddRangeBigEndian((UInt32)0);

                            SegOffset += SegBytes;
                            CurLen += (int)SegBytes;

                            foreach (SegmentEntry TexEntry in Segment)
                            {
                                ExtraSegDataEntries.AddRangeBigEndian(TexEntry.Address);
                                ExtraSegDataEntries.AddRangeBigEndian(TexEntry.FileStart);
                                ExtraSegDataEntries.AddRangeBigEndian(TexEntry.ObjectID);
                                Helpers.Ensure4ByteAlign(ExtraSegDataEntries);
                            }
                        }

                        EntryBytes.AddRangeBigEndian((UInt32)(ExtraSegDataOffsets.Count + ExtraSegDataEntries.Count));
                        CurLen += 4;
                        EntryBytes.AddRange(ExtraSegDataOffsets.ToArray());
                        EntryBytes.AddRange(ExtraSegDataEntries.ToArray());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region CCode

                        if (Entry.EmbeddedOverlayCode.Code != "")
                        {
                            pr.SetProgress((int)Math.Floor(CurProgress), $"Compiling C {EntriesDone}/{Data.Entries.Count()}");

                            CompErrors = "";
                            byte[] Overlay;

                            //CCode.CreateCTempDirectory(Entry.EmbeddedOverlayCode.Code);

                            using (SHA1 s = SHA1.Create())
                            {
                                string CodeString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode);
                                CodeString = CCode.ReplaceGameVersionInclude(CodeString);
                                string hash = Convert.ToBase64String(s.ComputeHash(Encoding.UTF8.GetBytes(CodeString))).Replace("+", "_").Replace("/", "-").Replace("=", "");
                                string cachedAddrsFile = System.IO.Path.Combine(Program.CachePath, $"{EntriesDone}_funcsaddrs_" + hash);
                                string cachedcodeFile = System.IO.Path.Combine(Program.CachePath, $"{EntriesDone}_code_" + hash);

                                if (!CcacheInvalid && File.Exists(cachedcodeFile) && File.Exists(cachedAddrsFile))
                                {
                                    Entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(File.ReadAllText(cachedAddrsFile), new JsonSerializerSettings() { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });
                                    Overlay = File.ReadAllBytes(cachedcodeFile);
                                }
                                else
                                {
                                    Helpers.DeleteFileStartingWith(Program.CachePath, $"{EntriesDone}_funcsaddrs_");
                                    Helpers.DeleteFileStartingWith(Program.CachePath, $"{EntriesDone}_code_");
                                    Helpers.DeleteFileStartingWith(Program.CachePath, $"{EntriesDone}_script");

                                    Overlay = CCode.Compile(true, Data.CHeader, Entry.EmbeddedOverlayCode, ref CompErrors);

                                    if (Overlay != null)
                                    {
                                        CodeString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode);
                                        string CodeAddrsString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode, new JsonSerializerSettings() { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });

                                        File.WriteAllText(cachedAddrsFile, CodeAddrsString);
                                        File.WriteAllBytes(cachedcodeFile, Overlay);
                                    }
                                }
                            }

                            if (Overlay == null)
                            {
                                if (!ParseErrors.Contains(Entry.NPCName))
                                    ParseErrors.Add(Entry.NPCName);

                                break;
                            }
                            else
                            {
                                CurLen += 4;

                                if (Entry.EmbeddedOverlayCode.Functions.Count != 0)
                                {
                                    EntryBytes.AddRangeBigEndian(Overlay.Length);

                                    List<byte> FuncsList = new List<byte>();
                                    List<byte> FuncsWhenList = new List<byte>();

                                    for (int i = 0; i < Entry.EmbeddedOverlayCode.FuncsRunWhen.GetLength(0); i++)
                                    {
                                        string FName = Entry.EmbeddedOverlayCode.SetFuncNames[i];
                                        int FuncIdx = Entry.EmbeddedOverlayCode.Functions.FindIndex(x => x.FuncName == FName);

                                        if (FuncIdx == -1 && FName != null && FName != "")
                                        {
                                            ShowMsg(CLIMode, $"{Entry.NPCName}: Function {FName} not found in the C Code!");
                                            return;
                                        }

                                        UInt32 FuncAddr = 0xFFFFFFFF;

                                        if (FuncIdx >= 0)
                                            FuncAddr = Entry.EmbeddedOverlayCode.Functions[FuncIdx].Addr;

                                        FuncsList.AddRangeBigEndian((UInt32)FuncAddr);
                                        FuncsWhenList.Add((byte)Entry.EmbeddedOverlayCode.FuncsRunWhen[i, 1]);
                                    }

                                    EntryBytes.AddRange(FuncsList.ToArray());
                                    EntryBytes.AddRange(FuncsWhenList.ToArray());
                                    Helpers.Ensure4ByteAlign(EntryBytes);

                                    CurLen += 24 + 8;
                                    Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                                    EntryBytes.AddRange(Overlay);
                                    CurLen += Overlay.Length;
                                    Helpers.Ensure4ByteAlign(EntryBytes);

                                    if (Overlay.Length % 4 != 0)
                                        CurLen += Overlay.Length % 4;

                                    Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);
                                }
                                else
                                    EntryBytes.AddRangeBigEndian(-1);
                            }
                        }
                        else
                        {
                            EntryBytes.AddRangeBigEndian(-1);
                            CurLen += 4;
                        }

                        #endregion

                        #region Scripts

                        List<ScriptEntry> NonEmptyEntries = Entry.Scripts.FindAll(x => !String.IsNullOrEmpty(x.Text));
                        EntryBytes.AddRangeBigEndian((UInt32)NonEmptyEntries.Count);

                        CurLen += 4;
                        Helpers.Ensure4ByteAlign(EntryBytes);
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        int ScrOffset = 0;

                        List<Scripts.BScript> ParsedScripts = new List<Scripts.BScript>();

                        int scriptNum = 0;

                        using (SHA1 s = SHA1.Create())
                        {
                            string m = JsonConvert.SerializeObject(Entry.Messages) + JsonConvert.SerializeObject(Entry.ExtraDisplayLists) + JsonConvert.SerializeObject(Entry.Segments) + JsonConvert.SerializeObject(Entry.Animations);
                            string hashM = Convert.ToBase64String(s.ComputeHash(Encoding.UTF8.GetBytes(m))).Replace("+", "_").Replace("/", "-").Replace("=", "");
                            string cachedmsgFile = System.IO.Path.Combine(Program.CachePath, $"{EntriesDone}_exdata_" + hashM);

                            foreach (ScriptEntry Scr in NonEmptyEntries)
                            {
                                Scripts.ScriptParser Par = new Scripts.ScriptParser(Data, Entry, Scr.Text, Data.GlobalHeaders);

                                string hash = Convert.ToBase64String(s.ComputeHash(Encoding.UTF8.GetBytes(Scr.Text))).Replace("+", "_").Replace("/", "-").Replace("=", "");
                                string cachedFile = System.IO.Path.Combine(Program.CachePath, $"{EntriesDone}_script{scriptNum}_" + hash);

                                if (!cacheInvalid && File.Exists(cachedFile) && File.Exists(cachedmsgFile))
                                    ParsedScripts.Add(new Scripts.BScript() { Script = File.ReadAllBytes(cachedFile), ParseErrors = new List<Scripts.ParseException>() });
                                else
                                {
                                    Helpers.DeleteFileStartingWith(Program.CachePath, $"{EntriesDone}_script{scriptNum}_");
                                    Scripts.BScript scr = Par.ParseScript(Scr.Name, true);
                                    ParsedScripts.Add(scr);
                                    File.WriteAllBytes(cachedFile, scr.Script);
                                }

                                scriptNum++;
                            }

                            Helpers.DeleteFileStartingWith(Program.CachePath, $"{EntriesDone}_exdata_");
                            File.WriteAllText(cachedmsgFile, "");

                        }

                        foreach (Scripts.BScript Scr in ParsedScripts)
                        {
                            EntryBytes.AddRangeBigEndian(ScrOffset);
                            ScrOffset += Scr.Script.Length;

                            CurLen += 4;
                        }

                        foreach (Scripts.BScript Scr in ParsedScripts)
                        {
                            EntryBytes.AddRange(Scr.Script);

                            CurLen += Scr.Script.Length;

                            if (Scr.ParseErrors.Count != 0)
                            {
                                if (!ParseErrors.Contains(Entry.NPCName))
                                    ParseErrors.Add(Entry.NPCName);

                                Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Script \"{Scr.Name}\" had errors:{Environment.NewLine}");
                                Console.WriteLine(String.Join(Environment.NewLine, Scr.ParseErrors));
                            }
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        EntryBytes.InsertRange(0, Program.BEConverter.GetBytes(EntryBytes.Count));
                        EntryData.Add(EntryBytes);
                        EntryAddresses.AddRangeBigEndian(Offset);
                        Offset += EntryBytes.Count();

                        if (ParseErrors.Count == 0)
                            Console.Write($"OK{Environment.NewLine}");
                        else
                            break;
                    }
                    else
                    {
                        EntryAddresses.AddRangeBigEndian((UInt32)0);
                        Console.WriteLine($"Entry {EntriesDone} is blank.");

                    }

                    EntriesDone += 1;
                    CurProgress += ProgressPer;

                    pr.SetProgress((int)Math.Ceiling(CurProgress), $"Saved {EntriesDone}/{Data.Entries.Count()}");
                    pr.Refresh();
                }

                pr.SetProgress(100, "Done!");
                pr.Refresh();

                List<byte> Output = new List<byte>();

                Output.AddRangeBigEndian((UInt32)Data.Entries.Count());
                Output.AddRange(EntryAddresses);

                foreach (List<byte> Entry in EntryData)
                    Output.AddRange(Entry);

                if (ParseErrors.Count != 0)
                {
                    if (cacheInvalid)
                    {
                        Helpers.DeleteFileStartingWith(Program.CachePath, "gh_");
                        Helpers.DeleteFileStartingWith(Program.CachePath, "ch_");
                    }

                    ShowMsg(CLIMode,
                            $"File could not be saved." +
                            $"" + Environment.NewLine + Environment.NewLine +
                            $"There are errors in NPC: {String.Join(",", ParseErrors)}");

                }
                else
                    File.WriteAllBytes(Path, Output.ToArray());
            }
            catch (Exception ex)
            {
                ShowMsg(CLIMode, $"Error writing file: {ex.Message}");
            }
            finally
            {
                pr.Dispose();
            }
        }

    }
}