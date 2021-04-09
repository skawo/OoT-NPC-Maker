using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace NPC_Maker
{
    public static class FileOps
    {
        public static NPCFile ParseJSONFile(string FileName)
        {
            try
            {
                string Text = File.ReadAllText(FileName);

                var Version = JObject.Parse(Text).SelectToken("Version");

                NPCFile Deserialized = JsonConvert.DeserializeObject<NPCFile>(Text);

                if (Version == null || (int)Version < 2)
                {
                    Deserialized.Version = 2;

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

                return Deserialized;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Failed to read JSON: {ex.Message}");
                return null;
            }
        }

        public static void SaveJSONFile(string Path, NPCFile Data)
        {
            try
            {
                File.WriteAllText(Path, JsonConvert.SerializeObject(Data, Formatting.Indented));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Failed to write JSON: {ex.Message}");
            }
        }

        public static Dictionary<string, int> GetDictionary(string Filename)
        {
            Dictionary<string, int> Dict = new Dictionary<string, int>();

            try
            {
                string[] RawData = File.ReadAllLines(Filename);

                foreach (string Row in RawData)
                {
                    string[] NameAndID = Row.Split(',');
                    Dict.Add(NameAndID[1], Convert.ToInt32(NameAndID[0]));
                }

                return Dict;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show($"{Filename} is missing or incorrect.");
                return Dict;
            }
        }

        public static void SaveBinaryFile(string Path, NPCFile Data)
        {
            if (Data.Entries.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing to save.");
                return;
            }

            try
            {
                int Offset = Data.Entries.Count() * 4 + 4;

                List<byte> EntryAddresses = new List<byte>();
                List<List<byte>> EntryData = new List<List<byte>>();
                List<string> ParseErrors = new List<string>();

                foreach (NPCEntry Entry in Data.Entries)
                {
                    if (Entry.IsNull == false)
                    {
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
                        EntryBytes.Add(Entry.EnvironmentColor.R);
                        EntryBytes.Add(Entry.EnvironmentColor.G);
                        EntryBytes.Add(Entry.EnvironmentColor.B);

                        EntryBytes.Add(Helpers.MakeByte(Entry.HasCollision,
                                                        Entry.PushesSwitches,
                                                        Entry.IsPushable,
                                                        Entry.IsAlwaysActive,
                                                        Entry.IsAlwaysDrawn,
                                                        Entry.ExecuteJustScript,
                                                        Entry.ReactsIfAttacked,
                                                        Entry.OpensDoors));

                        EntryBytes.Add(Helpers.MakeByte(Entry.CastsShadow,
                                                        Entry.IsTargettable,
                                                        Entry.LoopPath,
                                                        Entry.PathIsTimed,
                                                        Entry.EnvironmentColor.A > 0 ? true : false,
                                                        Entry.IgnoreYAxis,
                                                        Entry.PathSmoothen,
                                                        false));

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 24;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        EntryBytes.AddRangeBigEndian(Entry.ObjectID);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtDegreesVertical);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtDegreesHorizontal);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionRadius);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionHeight);
                        EntryBytes.AddRangeBigEndian(Entry.ShadowRadius);
                        EntryBytes.AddRangeBigEndian(Entry.MovementDistance);
                        EntryBytes.AddRangeBigEndian(Entry.PathLoopStartID);
                        EntryBytes.AddRangeBigEndian(Entry.PathLoopEndID);
                        EntryBytes.AddRangeBigEndian(Entry.PathLoopDelayTime);
                        EntryBytes.AddRangeBigEndian(Entry.TimedPathStart);
                        EntryBytes.AddRangeBigEndian(Entry.TimedPathEnd);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.CollisionPositionOffsets[2]);
                        EntryBytes.AddRangeBigEndian(Entry.TargetPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.TargetPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.TargetPositionOffsets[2]);
                        EntryBytes.AddRangeBigEndian(Entry.ModelPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.ModelPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.ModelPositionOffsets[2]);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 44;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        EntryBytes.AddRangeBigEndian(Entry.ModelScale);
                        EntryBytes.AddRangeBigEndian(Entry.TalkRadius);
                        EntryBytes.AddRangeBigEndian(Entry.MovementSpeed);
                        EntryBytes.AddRangeBigEndian(Entry.GravityForce);
                        EntryBytes.AddRangeBigEndian(Entry.Hierarchy);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtPositionOffsets[0]);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtPositionOffsets[1]);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtPositionOffsets[2]);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 32;
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
                            System.Windows.Forms.MessageBox.Show("Talking and blinking patterns may only be 4 entries long!");
                            return;
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            if (BlinkPat.Length > i)
                            {
                                int Index = Entry.Segments[Entry.BlinkSegment - 8].FindIndex(x => x.Name.ToLower() == BlinkPat[i].ToLower());

                                if (Index == -1)
                                {
                                    System.Windows.Forms.MessageBox.Show("Couldn't find one of the blink pattern textures: " + BlinkPat[i]);
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
                                    System.Windows.Forms.MessageBox.Show("Couldn't find one of the talking pattern textures: " + TalkPat[i]);
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

                        #region Animations

                        EntryBytes.AddRangeBigEndian((UInt32)Entry.Animations.Count());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        foreach (AnimationEntry Anim in Entry.Animations)
                        {
                            EntryBytes.AddRangeBigEndian((UInt32)Anim.Address);
                            EntryBytes.AddRangeBigEndian((float)Anim.Speed);
                            EntryBytes.AddRangeBigEndian((UInt16)Anim.ObjID);
                            EntryBytes.Add(Anim.StartFrame);
                            EntryBytes.Add(Anim.EndFrame);
                            Helpers.Ensure4ByteAlign(EntryBytes);
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += (12 * Entry.Animations.Count());
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
                            Helpers.Ensure4ByteAlign(EntryBytes);
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 32 * Entry.ExtraDisplayLists.Count;
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
                            UInt32 SegBytes = (UInt32)(8 * Segment.Count);

                            if (SegBytes != 0)
                                ExtraSegDataOffsets.AddRangeBigEndian(SegOffset);
                            else
                                ExtraSegDataOffsets.AddRangeBigEndian((UInt32)0);

                            SegOffset += SegBytes;
                            CurLen += (int)SegBytes;

                            foreach (SegmentEntry TexEntry in Segment)
                            {
                                ExtraSegDataEntries.AddRangeBigEndian(TexEntry.Address);
                                Helpers.Ensure4ByteAlign(ExtraSegDataEntries);
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

                        #region Scripts

                        List<ScriptEntry> NonEmptyEntries = Entry.Scripts.FindAll(x => !String.IsNullOrEmpty(x.Text));
                        EntryBytes.AddRangeBigEndian((UInt32)NonEmptyEntries.Count);

                        CurLen += 4;
                        Helpers.Ensure4ByteAlign(EntryBytes);
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        int ScrOffset = 0;

                        List<NewScriptParser.BScript> ParsedScripts = new List<NewScriptParser.BScript>();

                        foreach (ScriptEntry Scr in NonEmptyEntries)
                        {
                            NewScriptParser.ScriptParser Par = new NewScriptParser.ScriptParser(Entry, Scr.Text);
                            ParsedScripts.Add(Par.ParseScript());
                        }

                        foreach (NewScriptParser.BScript Scr in ParsedScripts)
                        {
                            EntryBytes.AddRangeBigEndian(ScrOffset);
                            ScrOffset += Scr.Script.Length;

                            CurLen += 4;

                        }

                        foreach (NewScriptParser.BScript Scr in ParsedScripts)
                        {
                            EntryBytes.AddRange(Scr.Script);

                            CurLen += Scr.Script.Length;

                            if (Scr.ParseErrors.Count != 0)
                                if (!ParseErrors.Contains(Entry.NPCName))
                                    ParseErrors.Add(Entry.NPCName);
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        EntryBytes.InsertRange(0, Program.BEConverter.GetBytes(EntryBytes.Count));
                        EntryData.Add(EntryBytes);
                        EntryAddresses.AddRangeBigEndian(Offset);
                        Offset += EntryBytes.Count();
                    }
                    else
                    {
                        EntryAddresses.AddRangeBigEndian((UInt32)0);
                    }
                }

                List<byte> Output = new List<byte>();

                Output.AddRangeBigEndian((UInt32)Data.Entries.Count());
                Output.AddRange(EntryAddresses);

                foreach (List<byte> Entry in EntryData)
                    Output.AddRange(Entry);

                if (ParseErrors.Count != 0)
                    System.Windows.Forms.MessageBox.Show($"There were errors parsing NPC(s): {String.Join(",", ParseErrors)}");
                else
                    File.WriteAllBytes(Path, Output.ToArray());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error writing file: {ex.Message}");
            }
        }

    }
}