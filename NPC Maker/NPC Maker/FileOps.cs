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

        public static void SaveBinaryFile(string Path, NPCFile Data)
        {
            int Offset = Data.Entries.Count() * 4 + 2;

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

                    byte[] Script = Parser.Parse(Entry.Script, Entry.Animations);
                    EntryBytes.AddRange(Program.BEConverter.GetBytes(Script.Length));

                    while ((EntryBytes.Count + 2) % 4 != 0)
                        EntryBytes.Add(0);


                    EntryBytes.AddRange(Script);
                    Entry.ParseErrors = Parser.ParseErrors.ToList();

                    if (Parser.ParseErrors.Count != 0)
                        ParseErrors.Add(Entry.NPCName);

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

            Output.AddRange(Program.BEConverter.GetBytes((UInt16)Data.Entries.Count()));
            Output.AddRange(EntryAddresses);

            foreach (List<byte> Entry in EntryData)
                Output.AddRange(Entry);

            File.WriteAllBytes(Path, Output.ToArray());

            if (ParseErrors.Count != 0)
            {
                System.Windows.Forms.MessageBox.Show("There were errors parsing NPC(s): " + String.Join(",", ParseErrors));
            }
        }
    }
}