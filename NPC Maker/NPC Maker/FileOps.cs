using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MiscUtil;
using MiscUtil.IO;
using MiscUtil.Conversion;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace NPC_Maker
{
    public static class FileOps
    {
        public static NPCFile ParseJSONFile(string FileName)
        {
            try
            {
                return JsonConvert.DeserializeObject<NPCFile>(File.ReadAllText(FileName));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed to read JSON: " + ex.Message);
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
                System.Windows.Forms.MessageBox.Show("Failed to write JSON: " + ex.Message);
            }
        }

        public static Dictionary<string, int> GetSoundDictionary(string Filename)
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
                System.Windows.Forms.MessageBox.Show(Filename + " is missing or incorrect.");
                return Dict;
            }

        }

        public static void SaveBinaryFile(string Path, NPCFile Data)
        {
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
                        EntryBytes.AddRange(new byte[] { Entry.HeadAxis });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.DegreesVert));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.DegreesHor));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LimbIndex));

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Collision));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Shadow));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Switches));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Pushable));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Radius));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Height));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ColOffs[0]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ColOffs[1]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.ColOffs[2]));

                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Targettable));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TargetLimb));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TargOffs[0]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TargOffs[1]));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.TargOffs[2]));

                        EntryBytes.AddRange(new byte[] { Entry.MovementType });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.MovementDistance));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.MovementSpeed));
                        EntryBytes.AddRange(new byte[] { Entry.PathID });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LoopStart));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LoopEnd));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.LoopDel));
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Entry.Loop));

                        EntryBytes.AddRange(new byte[] { Entry.AnimationType });
                        EntryBytes.AddRange(Program.BEConverter.GetBytes((UInt16)Entry.Animations.Count()));

                        foreach (AnimationEntry Anim in Entry.Animations)
                        {
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Anim.Address));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Anim.Speed));
                            EntryBytes.AddRange(Program.BEConverter.GetBytes(Anim.ObjID));
                        }

                        byte[] Script = Parser.Parse(Entry);
                        EntryBytes.AddRange(Program.BEConverter.GetBytes(Script.Length));

                        while (EntryBytes.Count % 4 != 0)
                            EntryBytes.Add(0);

                        EntryBytes.AddRange(Script);
                        Entry.ParseErrors = Parser.ParseErrors.ToList();

                        if (Parser.ParseErrors.Count != 0)
                            ParseErrors.Add(Entry.NPCName);

                        EntryBytes.Add(Entry.EnvColor.R);
                        EntryBytes.Add(Entry.EnvColor.G);
                        EntryBytes.Add(Entry.EnvColor.B);
                        EntryBytes.Add(Entry.EnvColor.A);

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
                                int Index = Entry.Textures[Entry.BlinkSegment - 8].FindIndex(x => x.Name.ToLower() == BlinkPat[i].ToLower());

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
                                int Index = Entry.Textures[Entry.TalkSegment - 8].FindIndex(x => x.Name.ToLower() == TalkPat[i].ToLower());

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

                        foreach (List<TextureEntry> Segment in Entry.Textures)
                        {
                            UInt32 SegBytes = (UInt32)(8 * Segment.Count);

                            if (SegBytes != 0)
                                TextureOffsets.AddRange(Program.BEConverter.GetBytes(SegOffset));
                            else
                                TextureOffsets.AddRange(Program.BEConverter.GetBytes((UInt32)0));

                            SegOffset += SegBytes;

                            foreach (TextureEntry TexEntry in Segment)
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
                    System.Windows.Forms.MessageBox.Show("There were errors parsing NPC(s): " + String.Join(",", ParseErrors));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error writing file: "  + ex.Message);
            }
        }

    }
}