using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

                        foreach (LocalizationEntry loc in e.Localization)
                        {
                            foreach (MessageEntry entry in loc.Messages)
                            {
                                entry.MessageText = String.Join(Environment.NewLine, entry.MessageTextLines);
                            }
                        }

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

                    foreach (LocalizationEntry loc in e.Localization)
                    {
                        foreach (MessageEntry entry in loc.Messages)
                        {
                            entry.MessageText = Regex.Replace(entry.MessageText, @"\r?\n", Environment.NewLine);
                        }
                    }
                }

                foreach (NPCEntry e in Deserialized.Entries)
                {
                    if (!String.IsNullOrEmpty(e.HeaderPath))
                    {
                        Dictionary<string, string> hDict = Helpers.GetDefinesFromH(e.HeaderPath);

                        e.Hierarchy = ResolveHeaderDefineForField(e.SkeletonHeaderDefinition, hDict, e.Hierarchy);

                        foreach (var a in e.Animations)
                            a.Address = ResolveHeaderDefineForField(a.HeaderDefinition, hDict, a.Address);

                        foreach (var d in e.ExtraDisplayLists)
                            d.Address = ResolveHeaderDefineForField(d.HeaderDefinition, hDict, d.Address);

                        foreach (var s in e.Segments)
                        {
                            foreach (var se in s)
                                se.Address = ResolveHeaderDefineForField(se.HeaderDefinition, hDict, se.Address);
                        }
                    }
                }

                Deserialized.Version = 7;
                Version = 7;

                return Deserialized;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read JSON: {ex.Message}");
                return null;
            }
        }

        public static UInt32 ResolveHeaderDefineForField(string Name, Dictionary<string, string> defines, UInt32 field)
        {
            if (!String.IsNullOrEmpty(Name))
            {
                Common.HDefine h = Helpers.GetDefineFromName(Name, defines);

                if (h != null && h.Value != null)
                    return (UInt32)h.Value;
                else
                    return field;
            }
            else
                return field;
        }

        public static UInt32 ResolveHeaderDefineForFieldOrFail(string NpcName, string Name, Dictionary<string, string> defines, UInt32 field)
        {
            try
            {
                if (!String.IsNullOrEmpty(Name))
                {
                    Common.HDefine h = Helpers.GetDefineFromName(Name, defines);

                    if (h != null && h.Value != null)
                        return (UInt32)h.Value;
                    else
                        throw new Exception();
                }
                else
                    return field;
            }
            catch (Exception)
            {
                MessageBox.Show($"Entry {NpcName}, Definition {Name} is invalid or cannot be resolved.");
                return field;
            }
        }

        public static void SaveNPCJSON(string Path, NPCFile Data, IProgress<Common.ProgressReport> progress = null)
        {
            try
            {
                NPCFile outD = (NPCFile)Helpers.Clone<NPCFile>(Data);

                var newlineRegex = new Regex("\r?\n", RegexOptions.Compiled);
                var environmentNewlineRegex = new Regex(Regex.Escape(Environment.NewLine), RegexOptions.Compiled);

                float ProgressPer = 100f / outD.Entries.Count;
                int processedCount = 0;

                Parallel.ForEach(outD.Entries, entry =>
                {
                    Parallel.ForEach(entry.Scripts, script =>
                    {
                        var lines = newlineRegex.Split(script.Text);
                        script.TextLines = lines.Select(x => x.TrimEnd()).ToList();
                        script.Text = null;
                    });

                    Parallel.ForEach(entry.Messages, message =>
                    {
                        message.MessageText = environmentNewlineRegex.Replace(message.MessageText, "\n");
                        message.MessageTextLines = newlineRegex.Split(message.MessageText).ToList();
                        message.MessageText = null;
                    });

                    Parallel.ForEach(entry.Localization, loc =>
                    {
                        Parallel.ForEach(loc.Messages, message =>
                        {
                            message.MessageText = environmentNewlineRegex.Replace(message.MessageText, "\n");
                            message.MessageTextLines = newlineRegex.Split(message.MessageText).ToList();
                            message.MessageText = null;
                        });
                    });

                    entry.EmbeddedOverlayCode.CodeLines = newlineRegex.Split(entry.EmbeddedOverlayCode.Code).ToList();
                    entry.EmbeddedOverlayCode.Code = null;

                    if (progress != null)
                    {
                        int currentProcessed = Interlocked.Increment(ref processedCount);
                        float currentProgress = currentProcessed * ProgressPer;
                        progress.Report(new Common.ProgressReport($"Saving {currentProgress:0}%", currentProgress));
                    }
                });

                Parallel.ForEach(outD.GlobalHeaders, script =>
                {
                    var lines = newlineRegex.Split(script.Text);
                    script.TextLines = lines.Select(x => x.TrimEnd()).ToList();
                    script.Text = null;
                });

                var headerLines = newlineRegex.Split(outD.CHeader);
                outD.CHeaderLines = headerLines.Select(x => x.TrimEnd()).ToList();
                outD.CHeader = null;

                var jsonSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(outD, jsonSettings);
                json = environmentNewlineRegex.Replace(json, "\n");

                File.WriteAllText(Path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to write JSON: {ex.Message}");
            }
        }


        public static Dictionary<string, int> GetDictionary(string Filename, bool allowFail = false)
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
                if (!allowFail)
                {
                    System.Windows.Forms.MessageBox.Show($"{Filename} is missing or incorrect. ({OffendingRow})");
                }

                return Dict;
            }
        }

        public static Dictionary<string, string> GetDictionaryStringString(string Filename, bool allowFail = false)
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();

            string OffendingRow = "";

            try
            {
                string[] RawData = File.ReadAllLines(Filename);

                foreach (string Row in RawData)
                {
                    OffendingRow = Row;
                    string[] data = Row.Split(',');
                    Dict.Add(data[0], data[1]);
                }

                return Dict;
            }
            catch (Exception)
            {
                if (!allowFail)
                {
                    System.Windows.Forms.MessageBox.Show($"{Filename} is missing or incorrect. ({OffendingRow})");
                }

                return Dict;
            }
        }

        private static void ShowMsg(bool CLIMode, string Msg)
        {
            Program.CompileThereWereErrors = true;

            if (CLIMode)
                Console.WriteLine(Msg);
            // Occasionally crashes showing messagebox on another thread.
            else if (Program.IsRunningUnderMono)
                Program.CompileMonoErrors = Msg;
            else
                MessageBox.Show(Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            return;
        }

        public static bool[] GetCacheStatus(NPCFile Data, bool CLIMode = false)
        {
            string JsonFileName = Program.JsonPath.FilenameFromPath();

            string extHeaderPath = "";

            try
            {
                extHeaderPath = Data.GetExtHeader();
            }
            catch (Exception)
            {

            }

            string gh = String.Join(Environment.NewLine, Data.GlobalHeaders.Select(x => x.Text)) + Environment.NewLine + extHeaderPath;
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
                string hashGlobalHeaders = Helpers.GetBase64Hash(s, gh);
                string hashDicts = Helpers.GetBase64Hash(s, dicts);
                Data.CHeader = CCode.ReplaceGameVersionInclude(Data.CHeader);
                string hashCHeader = Helpers.GetBase64Hash(s, Data.CHeader);

                string cachedHeaders = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_gh_{Ver}" + hashGlobalHeaders);
                string cachedDicts = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_dicts_{Ver}" + hashDicts);
                string cachedHeader = Path.Combine(Program.CCachePath, $"{JsonFileName}_ch_{Ver}" + hashCHeader);

                if ((!File.Exists(cachedHeaders)) || (!File.Exists(cachedDicts)))
                {
                    cacheInvalid = true;
                    Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_");
                    File.Create(cachedHeaders).Dispose();
                    File.Create(cachedDicts).Dispose();

                }

                if (!File.Exists(cachedHeader))
                {
                    CcacheInvalid = true;
                    Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_");
                    File.Create(cachedHeader).Dispose();
                }
            }

            return new bool[2] { cacheInvalid, CcacheInvalid };
        }

        public async static void PreprocessCodeAndScripts(string outPath, NPCFile Data, IProgress<Common.ProgressReport> progress, bool CLIMode = false)
        {
            float ProgressPer = (float)((float)100 / (float)Data.Entries.Count);
            float CurProgress = 0;

            await TaskEx.Run(() =>
            {
                bool[] cacheStatus = GetCacheStatus(Data, CLIMode);

                if (cacheStatus == null)
                {
                    Program.CompileInProgress = false;
                    return;
                }

                bool cacheInvalid = cacheStatus[0];
                bool CcacheInvalid = cacheStatus[1];

                Dictionary<int, NPCEntry> dict = new Dictionary<int, NPCEntry>();

                int id = 0;

                foreach (NPCEntry Entry in Data.Entries)
                {
                    dict.Add(id, Entry);
                    id++;
                }

                Console.Write($"Compiling...");

                string BaseDefines = Scripts.ScriptHelpers.GetBaseDefines(Data);
                string JsonFileName = Program.JsonPath.FilenameFromPath();

                Parallel.ForEach(dict, dictEntry =>
                {
                    try
                    {
                        NPCEntry Entry = dictEntry.Value;
                        int EntryID = dictEntry.Key;

                        string CompErrors = "";
                        byte[] Overlay = null;

                        using (SHA1 s = SHA1.Create())
                        {
                            string CodeString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode);
                            CodeString = CCode.ReplaceGameVersionInclude(CodeString);
                            string hash = Helpers.GetBase64Hash(s, CodeString);

                            string cachedAddrsFile = Path.Combine(Program.CCachePath, $"{JsonFileName}_{EntryID}_funcsaddrs_" + hash);
                            string cachedcodeFile = Path.Combine(Program.CCachePath, $"{JsonFileName}_{EntryID}_code_" + hash);

                            if (CcacheInvalid || !File.Exists(cachedcodeFile) || !File.Exists(cachedAddrsFile))
                            {
                                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_{EntryID}_funcsaddrs_");
                                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_{EntryID}_code_");

                                if (Entry.EmbeddedOverlayCode.Code != "")
                                    Overlay = CCode.Compile(Data.CHeader, Entry.EmbeddedOverlayCode, ref CompErrors, $"NPCCOMPILE{EntryID}");

                                if (Overlay != null)
                                {
                                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntryID}_script");
                                    CodeString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode);
                                    string CodeAddrsString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode, new JsonSerializerSettings() { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });

                                    File.WriteAllText(cachedAddrsFile, CodeAddrsString);
                                    File.WriteAllBytes(cachedcodeFile, Overlay);
                                }
                            }
                            else
                            {
                                // Need to load the overlay in so that the function addresses for the scripts are present.
                                Entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(File.ReadAllText(cachedAddrsFile), new JsonSerializerSettings() { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });
                                Overlay = File.ReadAllBytes(cachedcodeFile);
                            }

                            int scriptNum = 0;
                            List<ScriptEntry> NonEmptyEntries = Entry.Scripts.FindAll(x => !String.IsNullOrEmpty(x.Text));

                            string extData = JsonConvert.SerializeObject(Entry.Messages) + JsonConvert.SerializeObject(Entry.ExtraDisplayLists) + JsonConvert.SerializeObject(Entry.Segments) + JsonConvert.SerializeObject(Entry.Animations);
                            string extDataHash = Helpers.GetBase64Hash(s, extData);
                            string extDataFile = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_{EntryID}_exdata_" + extDataHash);

                            foreach (ScriptEntry Scr in NonEmptyEntries)
                            {
                                Scripts.ScriptParser Par = new Scripts.ScriptParser(Data, Entry, Scr.Text, BaseDefines);

                                hash = Helpers.GetBase64Hash(s, Scr.Text);
                                string cachedFile = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_{EntryID}_script{scriptNum}_" + hash);

                                if (cacheInvalid || !File.Exists(cachedFile) || !File.Exists(extDataFile))
                                {
                                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntryID}_script{scriptNum}_");

                                    Scripts.BScript scr = Par.ParseScript(Scr.Name, true);

                                    if (scr.ParseErrors.Count == 0)
                                        File.WriteAllBytes(cachedFile, scr.Script);
                                }

                                scriptNum++;
                            }

                            if (!File.Exists(extDataFile))
                            {
                                Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntryID}_exdata_");
                                File.Create(extDataFile).Dispose();
                            }
                        }

                        if (progress != null)
                        {
                            Helpers.AddInterlocked(ref CurProgress, ProgressPer);
                            progress.Report(new Common.ProgressReport($"Compiling {String.Format("{0:0.##}", CurProgress)}%", CurProgress));
                        }
                        else
                        {
                            Helpers.AddInterlocked(ref CurProgress, ProgressPer);
                            Console.Write($"\rCompiling {String.Format("{0:0.##}", CurProgress)}%    ");
                        }
                    }
                    catch (Exception)
                    {

                    }
                });

                Console.WriteLine("\nPre-processing done!");

                SaveBinaryFile(outPath, Data, progress, BaseDefines, false, false, CLIMode);
                CCode.CleanupStandardCompilationArtifacts();
                Program.CompileInProgress = false;
            });
        }

        private static UInt32 TryGetFromH(bool CLIMode, string NPCName, uint defaultV, Dictionary<string, string> defines, string name)
        {
            try
            {
                if (defines.Count == 0)
                    return defaultV;

                Common.HDefine h = Helpers.GetDefineFromName(name, defines);

                if (h == null)
                    return defaultV;
                else
                {
                    if (h.Value != null)
                        return (UInt32)h.Value;
                    else
                        return defaultV;
                }
            }
            catch (Exception)
            {
                FileOps.ShowMsg(CLIMode, $"{NPCName}: Error parsing define \"{name}\"!");
                return defaultV;
            }
        }

        public static void SaveBinaryFile(string outPath, NPCFile Data, IProgress<Common.ProgressReport> progress, string baseDefines, bool cacheInvalid, bool CcacheInvalid, bool CLIMode = false)
        {
            if (Data.Entries.Count() == 0)
            {
                ShowMsg(CLIMode, "Nothing to save.");
                Program.CompileThereWereErrors = false;
                return;
            }

            try
            {
                int Offset = Data.Entries.Count() * 12 + 4;
                string JsonFileName = Program.JsonPath.FilenameFromPath();

                List<byte> EntryAddresses = new List<byte>();
                List<List<byte>> EntryData = new List<List<byte>>();
                List<string> ParseErrors = new List<string>();

                string CompErrors = "";

                float ProgressPer = (float)((float)100 / (float)Data.Entries.Count);
                float CurProgress = 0;
                int EntriesDone = 0;

                if (progress != null)
                    progress.Report(new Common.ProgressReport($"Saving...", 0));

                foreach (NPCEntry Entry in Data.Entries)
                {

                    if (Entry.IsNull == false && !Entry.Omitted)
                    {
                        Console.Write($"Processing entry {EntriesDone}: {Entry.NPCName}... ");

                        Dictionary<string, string> defines = new Dictionary<string, string>();

                        if (!String.IsNullOrEmpty(Entry.HeaderPath))
                            defines = CCode.GetDefinesFromH(Helpers.ReplaceTokenWithPath(Program.Settings.ProjectPath, Entry.HeaderPath, "{PROJECTPATH}"));

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
                        EntryBytes.AddRangeBigEndian(TryGetFromH(CLIMode, Entry.NPCName, Entry.Hierarchy, defines, Entry.SkeletonHeaderDefinition));
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
                            if (TalkPat.Length > i && (Entry.TalkSegment - 8) >= 0)
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
                        List<byte> DefaultHeader = new List<byte>();
                        List<byte> MsgData = new List<byte>();

                        LocalizationEntry def = new LocalizationEntry() { Language = Dicts.DefaultLanguage, Messages = Entry.Messages };
                        List<LocalizationEntry> locales = new List<LocalizationEntry>() { def };

                        foreach (string language in Data.Languages)
                        {
                            int LanguageIndex = Entry.Localization.FindIndex(x => x.Language == language);

                            if (LanguageIndex != -1)
                                locales.Add(new LocalizationEntry() { Language = language, Messages = Entry.Localization[LanguageIndex].Messages });
                            else
                                locales.Add(new LocalizationEntry() { Language = language, Messages = null });
                        }

                        int Count = Entry.Messages.Count * locales.Count;
                        int MsgOffset = 8 * Count;

                        foreach (LocalizationEntry loc in locales)
                        {
                            bool isDefault = (loc.Language == Dicts.DefaultLanguage);
                            
                            if (loc.Messages == null)
                            {
                                Header = Header.Concat(DefaultHeader).ToList();
                                continue;
                            }

                            foreach (MessageEntry Msg in loc.Messages)
                            {
                                int numBoxes = 0;
                                List<byte> Message = Msg.ConvertTextData(Entry.NPCName, loc.Language, out numBoxes, !CLIMode);

                                if (Message == null)
                                {
                                    Message = new List<byte>();

                                    if (!ParseErrors.Contains(Entry.NPCName))
                                        ParseErrors.Add(Entry.NPCName);
                                }

                                Helpers.Ensure4ByteAlign(Message);
                                MsgData.AddRange(Message);

                                if (Message.Count > 1280)
                                {
                                    ShowMsg(CLIMode, $"{Entry.NPCName}: One of the messages ({Msg.Name}) has exceeded 1280 bytes (the maximum allowed), and could not be saved.");
                                    Message = new List<byte>();

                                    if (!ParseErrors.Contains(Entry.NPCName))
                                        ParseErrors.Add(Entry.NPCName);
                                }

                                if (isDefault)
                                {
                                    DefaultHeader.AddRangeBigEndian(MsgOffset);
                                    DefaultHeader.Add(Msg.GetMessageTypePos());
                                    Helpers.Ensure2ByteAlign(DefaultHeader);
                                    DefaultHeader.AddRangeBigEndian((UInt16)Message.Count);
                                }    

                                Header.AddRangeBigEndian(MsgOffset);
                                Header.Add(Msg.GetMessageTypePos());
                                Helpers.Ensure2ByteAlign(Header);
                                Header.AddRangeBigEndian((UInt16)Message.Count);

                                MsgOffset += Message.Count();
                            }
                        }

                        EntryBytes.AddRangeBigEndian(16 + Header.Count + MsgData.Count);
                        EntryBytes.AddRangeBigEndian(0);
                        EntryBytes.AddRangeBigEndian(Data.Languages.Count + 1);
                        EntryBytes.AddRangeBigEndian(Entry.Messages.Count);
                        EntryBytes.AddRange(Header);
                        EntryBytes.AddRange(MsgData);

                        CurLen += 16 + Header.Count + MsgData.Count;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Animations

                        EntryBytes.AddRangeBigEndian((UInt32)Entry.Animations.Count());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        foreach (AnimationEntry Anim in Entry.Animations)
                        {
                            EntryBytes.AddRangeBigEndian(TryGetFromH(CLIMode, Entry.NPCName, (UInt32)Anim.Address, defines, Anim.HeaderDefinition));
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
                            EntryBytes.AddRangeBigEndian(TryGetFromH(CLIMode, Entry.NPCName, Dlist.Address, defines, Dlist.HeaderDefinition));
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
                                ExtraSegDataEntries.AddRangeBigEndian(TryGetFromH(CLIMode, Entry.NPCName, TexEntry.Address, defines, TexEntry.HeaderDefinition));
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
                            CompErrors = "";
                            byte[] Overlay;

                            //CCode.CreateCTempDirectory(Entry.EmbeddedOverlayCode.Code);

                            using (SHA1 s = SHA1.Create())
                            {
                                string CodeString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode);
                                CodeString = CCode.ReplaceGameVersionInclude(CodeString);
                                string hash = Helpers.GetBase64Hash(s, CodeString);
                                string cachedAddrsFile = Path.Combine(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_funcsaddrs_" + hash);
                                string cachedcodeFile = Path.Combine(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_code_" + hash);

                                if (!CcacheInvalid && File.Exists(cachedcodeFile) && File.Exists(cachedAddrsFile))
                                {
                                    Entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(File.ReadAllText(cachedAddrsFile), new JsonSerializerSettings() { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });
                                    Overlay = File.ReadAllBytes(cachedcodeFile);
                                }
                                else
                                {
                                    Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_funcsaddrs_");
                                    Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_code_");
                                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_script");

                                    Overlay = CCode.Compile(Data.CHeader, Entry.EmbeddedOverlayCode, ref CompErrors);

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
                            string extData = JsonConvert.SerializeObject(Entry.Messages) + JsonConvert.SerializeObject(Entry.ExtraDisplayLists) + JsonConvert.SerializeObject(Entry.Segments) + JsonConvert.SerializeObject(Entry.Animations);
                            string extDataHash = Helpers.GetBase64Hash(s, extData);
                            string cachedExtDataFile = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_exdata_" + extDataHash);

                            foreach (ScriptEntry Scr in NonEmptyEntries)
                            {
                                Scripts.ScriptParser Par = new Scripts.ScriptParser(Data, Entry, Scr.Text, baseDefines);

                                string hash = Helpers.GetBase64Hash(s, Scr.Text);
                                string cachedFile = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_script{scriptNum}_" + hash);

                                if (!cacheInvalid && File.Exists(cachedFile) && File.Exists(cachedExtDataFile))
                                    ParsedScripts.Add(new Scripts.BScript() { Script = File.ReadAllBytes(cachedFile), ParseErrors = new List<Scripts.ParseException>() });
                                else
                                {
                                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_script{scriptNum}_");
                                    Scripts.BScript scr = Par.ParseScript(Scr.Name, true);
                                    ParsedScripts.Add(scr);

                                    if (scr.ParseErrors.Count == 0)
                                        File.WriteAllBytes(cachedFile, scr.Script);
                                }

                                scriptNum++;
                            }

                            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_exdata_");
                            File.Create(cachedExtDataFile).Dispose();

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

                        EntryData.Add(EntryBytes);

                        if (ParseErrors.Count == 0)
                            Console.Write($"OK{Environment.NewLine}");
                        else
                            break;
                    }
                    else
                    {
                        EntryData.Add(null);
                        Console.WriteLine($"Entry {EntriesDone} is blank or omitted.");

                    }

                    EntriesDone += 1;
                    CurProgress += ProgressPer;

                    if (progress != null)
                        progress.Report(new Common.ProgressReport($"Saving {EntriesDone}/{Data.Entries.Count}", CurProgress));
                }

                List<byte> Output = new List<byte>();

                Output.AddRangeBigEndian((UInt32)Data.Entries.Count());

                List<Common.CompilationEntryData> CompilationData = new List<Common.CompilationEntryData>();

                for (int i = 0; i < EntryData.Count; i++)
                    CompilationData.Add(new Common.CompilationEntryData(EntryData[i]));

                if (progress != null && Program.Settings.CompressIndividually)
                    progress.Report(new Common.ProgressReport($"Compressing...", 100));

                Parallel.ForEach(CompilationData, Entry =>
                {
                    List<byte> outCompressed = null;

                    if (Entry.data == null)
                        return;

                    if (Program.Settings.CompressIndividually)
                    {
                        outCompressed = PeepsCompress.YAZ0.Compress(Entry.data.ToArray(), 0).ToList();
                        Helpers.Ensure4ByteAlign(outCompressed);
                    }

                    if (!Program.Settings.CompressIndividually || outCompressed.Count >= Entry.data.Count)
                    {
                        Entry.compressedSize = 0;
                        Entry.decompressedSize = Entry.data.Count;
                    }
                    else
                    {
                        Entry.compressedSize = outCompressed.Count;
                        Entry.decompressedSize = Entry.data.Count;
                        Entry.data = outCompressed;
                    }
                });

                foreach (Common.CompilationEntryData entry in CompilationData)
                {
                    EntryAddresses.AddRangeBigEndian(Offset);
                    EntryAddresses.AddRangeBigEndian(entry.compressedSize);
                    EntryAddresses.AddRangeBigEndian(entry.decompressedSize);

                    if (entry.compressedSize != 0)
                        Offset += entry.compressedSize;
                    else
                        Offset += entry.decompressedSize;
                }

                Output.AddRange(EntryAddresses);

                foreach (Common.CompilationEntryData entry in CompilationData)
                {
                    if (entry.data != null)
                        Output.AddRange(entry.data);
                }

                if (progress != null)
                    progress.Report(new Common.ProgressReport($"Done!", 100));

                if (ParseErrors.Count != 0)
                {
                    ShowMsg(CLIMode,
                            $"File could not be saved." +
                            $"" + Environment.NewLine + Environment.NewLine +
                            $"There are errors in NPC: {String.Join(",", ParseErrors)}");

                }
                else
                    File.WriteAllBytes(outPath, Output.ToArray());
            }
            catch (Exception ex)
            {
                ShowMsg(CLIMode, $"Error writing file: {ex.Message}");
            }
            finally
            {
                Program.CompileInProgress = false;
            }
        }
    }
}