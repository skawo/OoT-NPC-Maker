﻿using System;
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
                ScriptParser Parser = new ScriptParser();

                foreach (NPCEntry Entry in Data.Entries)
                {
                    if (Entry.IsNull == false)
                    {
                        List<byte> EntryBytes = new List<byte>();

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ObjectID));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Hierarchy));
                        EntryBytes.AddRange(new byte[] { Entry.HierarchyType });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ModelOffs[0]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ModelOffs[1]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ModelOffs[2]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Scale));

                        EntryBytes.AddRange(new byte[] { Entry.LookAtType });
                        EntryBytes.AddRange(new byte[] { Entry.HeadVertAxis });
                        EntryBytes.AddRange(new byte[] { Entry.HeadHorizAxis });
                        EntryBytes.AddRange(new byte[] { Entry.HeadLimb });
                        EntryBytes.AddRange(new byte[] { Entry.WaistVertAxis });
                        EntryBytes.AddRange(new byte[] { Entry.WaistHorizAxis });
                        EntryBytes.AddRange(new byte[] { Entry.WaistLimb });

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.DegreesVert));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.DegreesHor));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LookAtOffs[0]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LookAtOffs[1]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LookAtOffs[2]));

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Shadow));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ShRadius));

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Collision));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Switches));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Pushable));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.AlwActive));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.AlwDraw));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.JustScript));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ReactAttacked));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.OpenDoors));

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ColRadius));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Height));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ColOffs[0]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ColOffs[1]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ColOffs[2]));

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Targettable));
                        EntryBytes.Add(Entry.TargetLimb);
                        EntryBytes.Add(Entry.TargetDist);
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TargOffs[0]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TargOffs[1]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TargOffs[2]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TalkRadius));

                        EntryBytes.AddRange(new byte[] { Entry.MovementType });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.MovementDistance));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.MovementSpeed));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.GravityForce));
                        EntryBytes.AddRange(new byte[] { Entry.PathID });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LoopStart));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LoopEnd));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LoopDel));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Loop));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TimedPath));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TimedPathStart));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TimedPathEnd));

                        EntryBytes.AddRange(new byte[] { Entry.AnimationType });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes((UInt16)Entry.Animations.Count()));

                        foreach (AnimationEntry Anim in Entry.Animations)
                        {
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Anim.Address));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Anim.Speed));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Anim.ObjID));
                            EntryBytes.Add((byte)Anim.Frames[0]);
                            EntryBytes.Add((byte)Anim.Frames[1]);
                            EntryBytes.Add((byte)Anim.Frames[2]);
                            EntryBytes.Add((byte)Anim.Frames[3]);
                        }

                        List<ScriptEntry> NonEmptyEntries = Entry.Scripts.FindAll(x => !String.IsNullOrEmpty(x.Text));

                        EntryBytes.Add((byte)NonEmptyEntries.Count);

                        while (EntryBytes.Count % 4 != 0)
                            EntryBytes.Add(0);

                        int ScrOffset = 0;

                        List<NewScriptParser.BScript> ParsedScripts = new List<NewScriptParser.BScript>();

                        foreach (ScriptEntry Scr in NonEmptyEntries)
                        {
                            NewScriptParser.ScriptParser Par = new NewScriptParser.ScriptParser(Entry, Scr.Text);
                            ParsedScripts.Add(Par.ParseScript());
                        }

                        foreach (NewScriptParser.BScript Scr in ParsedScripts)
                        {
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(ScrOffset));
                            ScrOffset += Scr.Script.Count;

                            while (ScrOffset % 4 != 0)
                                ScrOffset++;
                        }

                        foreach (NewScriptParser.BScript Scr in ParsedScripts)
                        {
                            while (EntryBytes.Count % 4 != 0)
                                EntryBytes.Add(0);

                            EntryBytes.AddRange(Scr.Script);

                            while (EntryBytes.Count % 4 != 0)
                                EntryBytes.Add(0);

                            if (Scr.ParseErrors.Count != 0)
                                if (!ParseErrors.Contains(Entry.NPCName))
                                    ParseErrors.Add(Entry.NPCName);
                        }

                        EntryBytes.Add(Entry.EnvColor.R);
                        EntryBytes.Add(Entry.EnvColor.G);
                        EntryBytes.Add(Entry.EnvColor.B);
                        EntryBytes.Add(Entry.EnvColor.A);

                        /*
                        List<OutputColorEntry> Cols = Entry.ParseColorEntries().OrderBy(x => x.LimbID).ToList();

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Cols.Count()));

                        foreach (OutputColorEntry Col in Cols)
                        {
                            EntryBytes.Add(Col.LimbID);
                            EntryBytes.Add(Col.R);
                            EntryBytes.Add(Col.G);
                            EntryBytes.Add(Col.B);
                        }
                        */

                        EntryBytes.Add(Entry.BlinkSegment);
                        EntryBytes.Add(Entry.TalkSegment);
                        EntryBytes.Add(Entry.BlinkSpeed);
                        EntryBytes.Add(Entry.TalkSpeed);

                        string[] BlinkPat = new string[0];
                        string[] TalkPat = new string[0];

                        if (Entry.BlinkPattern != "")
                            BlinkPat = Entry.BlinkPattern.Split(',');

                        if (Entry.TalkPattern != "")
                            TalkPat = Entry.TalkPattern.Split(',');

                        if (BlinkPat.Length > 6 || TalkPat.Length > 6)
                        {
                            System.Windows.Forms.MessageBox.Show("and blinking patterns may only be 6 entries long!");
                            return;
                        }

                        for (int i = 0; i < 8; i++)
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

                        for (int i = 0; i < 8; i++)
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

                        List<byte> TextureOffsets = new List<byte>();
                        List<byte> TextureEntries = new List<byte>();
                        UInt32 SegOffset = 7 * 4;

                        foreach (List<SegmentEntry> Segment in Entry.Segments)
                        {
                            UInt32 SegBytes = (UInt32)(8 * Segment.Count);

                            if (SegBytes != 0)
                                TextureOffsets.AddRange(Program.BEConverter.GetBytes(SegOffset));
                            else
                                TextureOffsets.AddRange(Program.BEConverter.GetBytes((UInt32)0));

                            SegOffset += SegBytes;

                            foreach (SegmentEntry TexEntry in Segment)
                            {
                                TextureEntries.AddRange(Program.BEConverter.GetBytes(TexEntry.Address));
                                TextureEntries.AddRange(Program.BEConverter.GetBytes(TexEntry.ObjectID));
                            }
                        }

                        UInt32 TexBytes = (7 * 4) + (UInt32)TextureEntries.Count;

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(TexBytes));

                        while ((EntryBytes.Count) % 4 != 0)
                            EntryBytes.Add(0);

                        EntryBytes.AddRange(TextureOffsets.ToArray());
                        EntryBytes.AddRange(TextureEntries.ToArray());

                        int DLists = Entry.DLists.Count;

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(DLists));

                        while ((EntryBytes.Count) % 4 != 0)
                            EntryBytes.Add(0);

                        foreach (DListEntry Dlist in Entry.DLists)
                        {
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.Address));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.RotX));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.RotY));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.RotZ));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.Limb));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.TransX));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.TransY));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.TransZ));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.Scale));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Dlist.ObjectID));
                            EntryBytes.Add((byte)Dlist.ShowType);
                            EntryBytes.AddRange(new byte[1]);
                        }

                        while ((EntryBytes.Count) % 4 != 0)
                            EntryBytes.Add(0);

                        EntryData.Add(EntryBytes);
                        EntryAddresses.AddRange(Program.BEConverter.GetBytes(Offset));
                        Offset += EntryBytes.Count();
                    }
                    else
                    {
                        EntryAddresses.AddRange(Program.BEConverter.GetBytes((UInt32)0));
                    }
                }

                List<byte> Output = new List<byte>();

                Output.AddRange(Program.BEConverter.GetBytes((UInt32)Data.Entries.Count()));
                Output.AddRange(EntryAddresses);

                foreach (List<byte> Entry in EntryData)
                    Output.AddRange(Entry);

                File.WriteAllBytes(Path, Output.ToArray());

                if (ParseErrors.Count != 0)
                {
                    System.Windows.Forms.MessageBox.Show($"There were errors parsing NPC(s): {String.Join(",", ParseErrors)}");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error writing file: {ex.Message}");
            }
        }

    }
}