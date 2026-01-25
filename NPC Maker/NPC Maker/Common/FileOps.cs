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
    public partial class FileOps
    {
        public static NPCMakerSettings ParseSettingsJSON(string FileName)
        {
            try
            {
                if (!File.Exists(FileName))
                    return new NPCMakerSettings();

                string Text = File.ReadAllText(FileName);
                NPCMakerSettings Deserialized = JsonConvert.DeserializeObject<NPCMakerSettings>(Text);

                return Deserialized;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Failed to read settings: {ex.Message}");
                return new NPCMakerSettings();
            }
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

        public static NPCFile ParseNPCJsonFile(string fileName)
        {
            try
            {
                string jsonText = File.ReadAllText(fileName);
                var jsonObject = JObject.Parse(jsonText);
                var version = jsonObject.SelectToken("Version");

                NPCFile npcFile = JsonConvert.DeserializeObject<NPCFile>(jsonText);
                int currentVersion = version?.Value<int>() ?? 1;

                // Apply version migrations in sequence
                currentVersion = MigrateToVersion2(npcFile, jsonObject, currentVersion);
                currentVersion = MigrateToVersion3(npcFile, currentVersion);

                if (currentVersion > 3)
                    ProcessTextLinesForVersion4Plus(npcFile);

                if (currentVersion > 4)
                    ProcessMessageLinesForVersion5Plus(npcFile);

                currentVersion = MigrateToVersion6(npcFile, currentVersion);
                currentVersion = MigrateToVersion7(npcFile, currentVersion);

                if (currentVersion >= 6)
                    ProcessCHeader(npcFile);

                NormalizeLineBreaks(npcFile);
                ResolveHeaderDefines(npcFile);

                npcFile.Version = 7;
                return npcFile;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read JSON: {ex.Message}");
                return null;
            }
        }

        private static void ProcessCHeader(NPCFile npcFile)
        {
            npcFile.CHeader = string.Join(Environment.NewLine, npcFile.CHeaderLines.Select(x => x.TrimEnd()));
        }

        private static void NormalizeLineBreaks(NPCFile npcFile)
        {
            var lineBreakRegex = new Regex(@"\r?\n");

            foreach (var entry in npcFile.Entries)
            {
                NormalizeMessageLineBreaks(entry.Messages, lineBreakRegex);

                foreach (var localization in entry.Localization)
                    NormalizeMessageLineBreaks(localization.Messages, lineBreakRegex);
            }
        }

        private static void NormalizeMessageLineBreaks(IEnumerable<MessageEntry> messages, Regex lineBreakRegex)
        {
            foreach (var message in messages)
                message.MessageText = lineBreakRegex.Replace(message.MessageText, Environment.NewLine);
        }

        private static void ResolveHeaderDefines(NPCFile npcFile)
        {
            foreach (var entry in npcFile.Entries.Where(e => !string.IsNullOrEmpty(e.HeaderPath)))
            {
                var headerDefines = Helpers.GetDefinesFromHeaders(entry.HeaderPath);

                entry.Hierarchy = ResolveHeaderDefineForField(entry.SkeletonHeaderDefinition, headerDefines, entry.Hierarchy);
                entry.FileStart = (int)ResolveHeaderDefineForField(entry.FileStartHeaderDefinition, headerDefines, (uint)entry.FileStart);

                foreach (var animation in entry.Animations)
                {
                    var anm = Helpers.SplitHeaderDefsString(animation.HeaderDefinition);

                    animation.Address = ResolveHeaderDefineForField(anm[1], headerDefines, animation.Address);
                    animation.FileStart = (int)ResolveHeaderDefineForField(anm[0], headerDefines, (uint)animation.FileStart);
                }

                foreach (var displayList in entry.ExtraDisplayLists)
                {
                    var dl = Helpers.SplitHeaderDefsString(displayList.HeaderDefinition);

                    displayList.Address = ResolveHeaderDefineForField(dl[1], headerDefines, displayList.Address);
                    displayList.FileStart = (int)ResolveHeaderDefineForField(dl[0], headerDefines, (uint)displayList.FileStart);
                }

                foreach (var segment in entry.Segments)
                {
                    foreach (var segmentEntry in segment)
                    {
                        var seg = Helpers.SplitHeaderDefsString(segmentEntry.HeaderDefinition);

                        segmentEntry.Address = ResolveHeaderDefineForField(seg[1], headerDefines, segmentEntry.Address);
                        segmentEntry.FileStart = (int)ResolveHeaderDefineForField(seg[0], headerDefines, (uint)segmentEntry.FileStart);
                    }
                }
            }
        }

        public static UInt32 ResolveHeaderDefineForField(string Name, Dictionary<string, string> defines, UInt32 field)
        {
            if (!String.IsNullOrEmpty(Name))
            {
                Common.HDefine h = Helpers.GetHDefineFromName(Name, defines);

                if (h != null && h.Value1 != null)
                    return (UInt32)h.Value1;
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
                    Common.HDefine h = Helpers.GetHDefineFromName(Name, defines);

                    if (h != null && h.Value1 != null)
                        return (UInt32)h.Value1;
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

        public static void SaveNPCJSON(string Path, NPCFile Data, IProgress<Common.ProgressReport> progress = null, string json = null)
        {
            try
            {
                if (json == null)
                    json = ProcessNPCJSON(Data, progress);

                if (json != null)
                    File.WriteAllText(Path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save JSON: {ex.Message}");
            }
        }

        public static string ProcessNPCJSON(NPCFile Data, IProgress<Common.ProgressReport> progress = null)
        {
            try
            {
                NPCFile outD = (NPCFile)Helpers.Clone<NPCFile>(Data);

                string[] newlineSeparators = new[] { "\r\n", "\n" };
                string envNewline = Environment.NewLine;

                float progressPer = 100f / outD.Entries.Count;
                int processedCount = 0;

                Parallel.ForEach(outD.Entries, entry =>
                {
                    // Process Scripts
                    foreach (var script in entry.Scripts)
                    {
                        script.TextLines = script.Text?.Split(newlineSeparators, StringSplitOptions.None)
                                                       .Select(x => x.TrimEnd())
                                                       .ToList();
                        script.Text = null;
                    }

                    // Process Messages
                    foreach (var message in entry.Messages)
                    {
                        message.MessageText = message.MessageText?.Replace(envNewline, "\n");
                        message.MessageTextLines = message.MessageText?.Split(newlineSeparators, StringSplitOptions.None).ToList();
                        message.MessageText = null;
                    }

                    // Process Localization
                    foreach (var loc in entry.Localization)
                    {
                        foreach (var message in loc.Messages)
                        {
                            message.MessageText = message.MessageText?.Replace(envNewline, "\n");
                            message.MessageTextLines = message.MessageText?.Split(newlineSeparators, StringSplitOptions.None).ToList();
                            message.MessageText = null;
                        }
                    }

                    // Process Embedded Overlay Code
                    if (entry.EmbeddedOverlayCode?.Code != null)
                    {
                        entry.EmbeddedOverlayCode.CodeLines = entry.EmbeddedOverlayCode.Code
                            .Split(newlineSeparators, StringSplitOptions.None)
                            .ToList();
                        entry.EmbeddedOverlayCode.Code = null;
                    }

                    // Report progress
                    if (progress != null)
                    {
                        int current = Interlocked.Increment(ref processedCount);
                        if (current % 10 == 0 || current == outD.Entries.Count) // reduce contention
                        {
                            float currentProgress = current * progressPer;
                            progress.Report(new Common.ProgressReport($"Saving {currentProgress:0}%", currentProgress));
                        }
                    }
                });

                // Process GlobalHeaders sequentially (small array)
                foreach (var script in outD.GlobalHeaders)
                {
                    script.TextLines = script.Text?.Split(newlineSeparators, StringSplitOptions.None)
                                                   .Select(x => x.TrimEnd())
                                                   .ToList();
                    script.Text = null;
                }

                // Process CHeader
                outD.CHeaderLines = outD.CHeader?.Split(newlineSeparators, StringSplitOptions.None)
                                                .Select(x => x.TrimEnd())
                                                .ToList();
                outD.CHeader = null;

                var jsonSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(outD, jsonSettings);
                json = json.Replace(envNewline, "\n");

                return json;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to process JSON: {ex.Message}");
                return null;
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
                    MessageBox.Show($"{Filename} is missing or incorrect. ({OffendingRow})");

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

            if (Program.IsRunningUnderMono || CLIMode)
                Console.WriteLine(Msg);

            // Occasionally crashes showing messagebox on another thread.
            if (Program.IsRunningUnderMono)
                Program.CompileMonoErrors = Msg;
            else if (!CLIMode)
                MessageBox.Show(Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            return;
        }

        public static bool[] GetCacheStatus(NPCFile data)
        {
            string jsonFileName = Program.JsonPath.FilenameFromPath();
            string extHeaderPath = "";

            try { extHeaderPath = data.GetExtHeader(); }
            catch { }

            // Build global + ext headers efficiently
            var ghBuilder = new StringBuilder();
            foreach (var h in data.GlobalHeaders)
                ghBuilder.AppendLine(h.Text);

            ghBuilder.Append(extHeaderPath);
            string gh = ghBuilder.ToString();

            string dicts = JsonConvert.SerializeObject(new
            {
                Dicts.Actors,
                Dicts.ObjectIDs,
                Dicts.SFXes,
                Dicts.LinkAnims,
                Dicts.Music
            });

            bool cacheInvalid = false;
            bool cCacheInvalid = false;

            string ver = FileVersionInfo
                .GetVersionInfo(Assembly.GetExecutingAssembly().Location)
                .ProductVersion;

            // Prepare C header + linkers
            data.CHeader = CCode.ReplaceGameVersionInclude(data.CHeader);

            var cHeaderBuilder = new StringBuilder(data.CHeader);
            foreach (var linkerPath in CCode.ResolveLinkerPaths(Program.Settings.LinkerPaths))
            {
                if (File.Exists(linkerPath))
                    cHeaderBuilder.Append(File.ReadAllText(linkerPath));
            }

            using (var sha1 = SHA1.Create())
            {

                string hashGlobalHeaders = Helpers.GetBase64Hash(gh);
                string hashDicts = Helpers.GetBase64Hash(dicts);
                string hashCHeader = Helpers.GetBase64Hash(cHeaderBuilder.ToString());

                string cachedHeaders = Path.Combine(
                    Program.ScriptCachePath, $"{jsonFileName}_gh_{ver}_{hashGlobalHeaders}");

                string cachedDicts = Path.Combine(
                    Program.ScriptCachePath, $"{jsonFileName}_dicts_{ver}_{hashDicts}");

                string cachedHeader = Path.Combine(
                    Program.CCachePath, $"{jsonFileName}_ch_{ver}_{hashCHeader}");


                if (!File.Exists(cachedHeaders) || !File.Exists(cachedDicts))
                {
                    cacheInvalid = true;
                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{jsonFileName}_");
                    File.WriteAllText(cachedHeaders, "");
                    File.WriteAllText(cachedDicts, "");
                }

                if (!File.Exists(cachedHeader))
                {
                    cCacheInvalid = true;
                    Helpers.DeleteFileStartingWith(Program.CCachePath, $"{jsonFileName}_");
                    File.WriteAllText(cachedHeader, "");
                }
            }

            return new[] { cacheInvalid, cCacheInvalid };
        }


        // Load all the includes text so that it can be appended to the string being hashed for cache
        private static string GetIncludesText(List<string> includeList)
        {
            var codeBuilder = new StringBuilder();

            List<string> alreadyIncluded = new List<string>();

            foreach (string hPath in includeList)
            {
                string cleanPath = Helpers.ReplaceTokenWithPath(Program.Settings.ProjectPath, hPath, Dicts.ProjectPathToken);
                string content = null;

                // Try original path first
                try
                {
                    if (!alreadyIncluded.Contains(cleanPath))
                    {
                        content = File.ReadAllText(cleanPath);
                        alreadyIncluded.Add(cleanPath);
                    }
                }
                catch
                {
                    // Try cleaned path as fallback
                    try
                    {
                        if (!alreadyIncluded.Contains(cleanPath))
                        {
                            cleanPath = cleanPath.TrimStart('/', '\\');
                            content = File.ReadAllText(cleanPath);
                            alreadyIncluded.Add(cleanPath);
                        }
                    }
                    catch
                    {
                        // Add random string. This way the cache always will be invalidated.
                        Program.ConsoleWriteLineS($"Warning: Couldn't resolve path {hPath} for cache use.");
                        content = Helpers.GenerateTemporaryFolderName();
                    }
                }

                codeBuilder.Append(content);
            }

            return codeBuilder.ToString();
        }

        public static async Task PreprocessCodeAndScripts(
            string outPath,
            NPCFile Data,
            IProgress<Common.ProgressReport> progress,
            bool CLIMode)
        {
            float progressPer = 100f / Data.Entries.Count;
            float curProgress = 0f;

            await TaskEx.Run(() =>
            {
                var cacheStatus = GetCacheStatus(Data);
                if (cacheStatus == null)
                {
                    Program.CompileInProgress = false;
                    return;
                }

                bool cacheInvalid = cacheStatus[0];
                bool cCacheInvalid = cacheStatus[1];

                Program.ConsoleWriteS("Compiling...");

                string baseDefines = Scripts.ScriptHelpers.GetBaseDefines(Data);
                string jsonFileName = Program.JsonPath.FilenameFromPath();

                var results = new ConcurrentBag<Common.PreprocessedEntry>();

                int lastReported = 0;

                Parallel.For(
                    0,
                    Data.Entries.Count,
                    new ParallelOptions
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    },
                    entryID =>
                    {
                        try
                        {
                            NPCEntry entry = Data.Entries[entryID];
                            string jsonEntryPrefix = jsonFileName + "_" + entryID + "_";

                            /* ---------------- C CODE ---------------- */

                            byte[] overlay = null;
                            string compErrors = "";

                            string codeString =
                                JsonConvert.SerializeObject(entry.EmbeddedOverlayCode) +
                                CCode.ReplaceGameVersionInclude("") +
                                GetIncludesText(entry.EmbeddedOverlayCode.HeaderPaths);

                            string codeHash = Helpers.GetBase64Hash(codeString);

                            string funcsAddrsName = jsonEntryPrefix + "funcsaddrs_" + codeHash;
                            string codeName = jsonEntryPrefix + "code_" + codeHash;

                            string cachedAddrsFile = Path.Combine(Program.CCachePath, funcsAddrsName);
                            string cachedCodeFile = Path.Combine(Program.CCachePath, codeName);

                            bool cCacheHit =
                                !cCacheInvalid &&
                                File.Exists(cachedAddrsFile) &&
                                File.Exists(cachedCodeFile);

                            if (!cCacheHit)
                            {
                                Helpers.DeleteFileStartingWith(Program.CCachePath, funcsAddrsName);
                                Helpers.DeleteFileStartingWith(Program.CCachePath, codeName);

                                if (!string.IsNullOrEmpty(entry.EmbeddedOverlayCode.Code))
                                {
                                    overlay = CCode.Compile(
                                        Data.CHeader,
                                        Program.Settings.LinkerPaths,
                                        entry.EmbeddedOverlayCode,
                                        ref compErrors,
                                        "NPCCOMPILE" + entryID);
                                }

                                if (overlay != null)
                                {
                                    Helpers.DeleteFileStartingWith(
                                        Program.ScriptCachePath,
                                        jsonEntryPrefix + "script");

                                    string addrJson = JsonConvert.SerializeObject(
                                        entry.EmbeddedOverlayCode,
                                        new JsonSerializerSettings
                                        {
                                            ContractResolver =
                                                new JsonIgnoreAttributeIgnorerContractResolver()
                                        });

                                    File.WriteAllText(cachedAddrsFile, addrJson);
                                    File.WriteAllBytes(cachedCodeFile, overlay);
                                }
                            }
                            else
                            {
                                entry.EmbeddedOverlayCode =
                                    JsonConvert.DeserializeObject<CCodeEntry>(
                                        File.ReadAllText(cachedAddrsFile),
                                        new JsonSerializerSettings
                                        {
                                            ContractResolver =
                                                new JsonIgnoreAttributeIgnorerContractResolver()
                                        });

                                overlay = File.ReadAllBytes(cachedCodeFile);
                            }

                            if (overlay != null)
                            {
                                results.Add(new Common.PreprocessedEntry(
                                    cachedAddrsFile, entry.EmbeddedOverlayCode));
                                results.Add(new Common.PreprocessedEntry(
                                    cachedCodeFile, overlay));
                            }

                            /* ---------------- EXT DATA ---------------- */

                            string extData =
                                JsonConvert.SerializeObject(new
                                {
                                    entry.Messages,
                                    entry.ExtraDisplayLists,
                                    entry.Segments,
                                    entry.Animations
                                }) +
                                Helpers.GetDefinesStringFromH(entry.HeaderPath);

                            string extDataHash = Helpers.GetBase64Hash(extData);
                            string extDataFile = Path.Combine(
                                Program.ScriptCachePath,
                                jsonEntryPrefix + "exdata_" + extDataHash);

                            bool extDataExists = File.Exists(extDataFile);

                            /* ---------------- SCRIPTS ---------------- */

                            int scriptNum = 0;

                            foreach (var scrEntry in entry.Scripts)
                            {
                                if (string.IsNullOrEmpty(scrEntry.Text))
                                {
                                    scriptNum++;
                                    continue;
                                }

                                string scriptHash = Helpers.GetBase64Hash(scrEntry.Text);
                                string cachedScriptFile = Path.Combine(
                                    Program.ScriptCachePath,
                                    jsonEntryPrefix + "script" + scriptNum + "_" + scriptHash);

                                bool scriptCacheHit =
                                    !cacheInvalid &&
                                    extDataExists &&
                                    File.Exists(cachedScriptFile);

                                if (!scriptCacheHit)
                                {
                                    Helpers.DeleteFileStartingWith(
                                        Program.ScriptCachePath,
                                        jsonEntryPrefix + "script" + scriptNum + "_");

                                    var parser = new Scripts.ScriptParser(
                                        Data,
                                        entry,
                                        scrEntry.Text,
                                        baseDefines);

                                    var script = parser.ParseScript(scrEntry.Name, true);

                                    if (script.ParseErrors.Count == 0)
                                    {
                                        File.WriteAllBytes(cachedScriptFile, script.Script);
                                        results.Add(new Common.PreprocessedEntry(
                                            cachedScriptFile, script.Script));
                                    }
                                }
                                else
                                {
                                    results.Add(new Common.PreprocessedEntry(
                                        cachedScriptFile,
                                        File.ReadAllBytes(cachedScriptFile)));
                                }

                                scriptNum++;
                            }

                            if (!extDataExists)
                            {
                                Helpers.DeleteFileStartingWith(
                                    Program.ScriptCachePath,
                                    jsonEntryPrefix + "exdata_");

                                File.Create(extDataFile).Dispose();
                            }

                            /* ---------------- PROGRESS ---------------- */

                            Helpers.AddInterlocked(ref curProgress, progressPer);
                            int percent = (int)curProgress;

                            if (percent != lastReported)
                            {
                                lastReported = percent;

                                if (progress != null)
                                {
                                    progress.Report(new Common.ProgressReport(
                                        $"Compiling {percent}%",
                                        curProgress));
                                }
                                else
                                {
                                    Program.ConsoleWriteS(
                                        $"\rCompiling {percent}%    ");
                                }
                            }
                        }
                        catch
                        {
                            // Intentionally swallowed (existing behavior)
                        }
                    });

                Program.ConsoleWriteLineS("\nPre-processing done!");

                SaveBinaryFile(
                    outPath,
                    Data,
                    progress,
                    baseDefines,
                    new[] { false, false },
                    results.ToList(),
                    CLIMode);

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

                Common.HDefine h = Helpers.GetHDefineFromName(name, defines);

                if (h == null)
                {
                    if (!String.IsNullOrWhiteSpace(name))
                        FileOps.ShowMsg(CLIMode, $"{NPCName}: Warning: Could not find define {name}!");

                    return defaultV;
                }
                else
                    return h.Value1 != null ? (UInt32)h.Value1 : defaultV;
            }
            catch (Exception)
            {
                FileOps.ShowMsg(CLIMode, $"{NPCName}: Error parsing define \"{name}\"!");
                return defaultV;
            }
        }

        public static void SaveBinaryFile(string outPath, NPCFile Data, IProgress<Common.ProgressReport> progress,
                                          string baseDefines, bool[] cacheStatus, List<Common.PreprocessedEntry> preProcessedFiles, bool CLIMode)
        {
            if (Data.Entries.Count() == 0)
            {
                ShowMsg(CLIMode, "Nothing to save.");
                Program.CompileThereWereErrors = false;
                return;
            }

            try
            {
                bool cacheInvalid = cacheStatus[0];
                bool CcacheInvalid = cacheStatus[1];

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

                List<Common.CompilationEntryData> CompilationData = new List<Common.CompilationEntryData>();


                foreach (NPCEntry Entry in Data.Entries)
                {
                    if (Entry.IsNull == false && !Entry.Omitted)
                    {
                        Program.ConsoleWriteS($"Processing entry {EntriesDone}: {Entry.NPCName}... ");
                        Dictionary<string, string> defines = Helpers.GetDefinesFromHeaders(Entry.HeaderPath);

                        int CurLen = 0;

                        var EntryBytes = new List<byte>(56)
                            {
                                Entry.CutsceneID,
                                Entry.HeadLimb,
                                Entry.WaistLimb,
                                Entry.TargetLimb,
                                Entry.PathID,
                                Entry.BlinkSpeed,
                                Entry.TalkSpeed,
                                Entry.HierarchyType,
                                Entry.TalkSegment,
                                Entry.BlinkSegment,
                                Entry.AnimationType,
                                Entry.MovementType,
                                Entry.WaistHorizAxis,
                                Entry.WaistVertAxis,
                                Entry.HeadHorizAxis,
                                Entry.HeadVertAxis,
                                Entry.LookAtType,
                                Entry.TargetDistance,
                                Entry.EffectIfAttacked,
                                Entry.Mass,
                                Entry.Alpha,
                                Entry.LightLimb,
                                Entry.EnvironmentColor.R,
                                Entry.EnvironmentColor.G,
                                Entry.EnvironmentColor.B,
                                Entry.LightColor.R,
                                Entry.LightColor.G,
                                Entry.LightColor.B,
                                Entry.AnimInterpFrames,
                                0,
                                0,
                                0,
                                Convert.ToByte(Entry.HasCollision),
                                Convert.ToByte(Entry.PushesSwitches),
                                Convert.ToByte(Entry.IgnoreYAxis),
                                Convert.ToByte(Entry.IsAlwaysActive),
                                Convert.ToByte(Entry.IsAlwaysDrawn),
                                Convert.ToByte(Entry.ExecuteJustScript),
                                Convert.ToByte(Entry.ReactsIfAttacked),
                                Convert.ToByte(Entry.OpensDoors),
                                Convert.ToByte(Entry.CastsShadow),
                                Convert.ToByte(Entry.IsTargettable),
                                Convert.ToByte(Entry.LoopPath),
                                Convert.ToByte(Entry.EnvironmentColor.A > 0),
                                Convert.ToByte(Entry.FadeOut),
                                Convert.ToByte(Entry.GenLight),
                                Convert.ToByte(Entry.Glow),
                                Convert.ToByte(Entry.DEBUGShowCols),
                                Convert.ToByte(Entry.VisibleUnderLensOfTruth),
                                Convert.ToByte(Entry.Invisible),
                                Convert.ToByte(Entry.ExistInAllRooms),
                                Convert.ToByte(Entry.NumVars),
                                Convert.ToByte(Entry.NumFVars),
                                Convert.ToByte(Entry.DEBUGExDlistEditor),
                                Convert.ToByte(Entry.DEBUGLookAtEditor),
                                Convert.ToByte(Entry.DEBUGPrintToScreen)
                            };

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
                        EntryBytes.AddRangeBigEndian(Entry.TargetPositionOffsets);
                        EntryBytes.AddRangeBigEndian(Entry.ModelPositionOffsets);
                        EntryBytes.AddRangeBigEndian(Entry.LightPositionOffsets);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 52;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        EntryBytes.AddRangeBigEndian(Entry.ModelScale);
                        EntryBytes.AddRangeBigEndian(Entry.TalkRadius);
                        EntryBytes.AddRangeBigEndian(Entry.MovementSpeed);
                        EntryBytes.AddRangeBigEndian(Entry.GravityForce);
                        EntryBytes.AddRangeBigEndian(Entry.SmoothingConstant);
                        EntryBytes.AddRangeBigEndian(TryGetFromH(CLIMode, Entry.NPCName, Entry.Hierarchy, defines, Entry.SkeletonHeaderDefinition));
                        EntryBytes.AddRangeBigEndian(TryGetFromH(CLIMode, Entry.NPCName, (uint)Entry.FileStart, defines, Entry.FileStartHeaderDefinition));
                        EntryBytes.AddRangeBigEndian(Entry.CullForward);
                        EntryBytes.AddRangeBigEndian(Entry.CullDown);
                        EntryBytes.AddRangeBigEndian(Entry.CullScale);
                        EntryBytes.AddRangeBigEndian(Entry.LookAtPositionOffsets);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 52;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #region Blink and talk patterns

                        EntryBytes.AddRange(Entry.GetBlinkPatternBytes());
                        EntryBytes.AddRange(Entry.GetTalkPatternBytes());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 8;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Messages

                        string msgDataS = JsonConvert.SerializeObject(new { Entry.Messages, Entry.Localization });
                        string msgDataHash = Helpers.GetBase64Hash(msgDataS);
                        string cachedMsgFile = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_msg_" + msgDataHash);

                        if (File.Exists(cachedMsgFile))
                        {
                            var cachedMsgBytes = File.ReadAllBytes(cachedMsgFile);
                            EntryBytes.AddRange(cachedMsgBytes);

                            CurLen += cachedMsgBytes.Length;
                            Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);
                        }
                        else
                        {
                            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_msg_");

                            var header = new List<byte>();
                            var defaultHeader = new List<byte>();
                            var msgData = new List<byte>();

                            var locales = new List<LocalizationEntry>
                                {
                                    new LocalizationEntry { Language = Dicts.DefaultLanguage, Messages = Entry.Messages }
                                };

                            locales.AddRange(Data.Languages.Select(lang =>
                            {
                                var loc = Entry.Localization.FirstOrDefault(x => x.Language == lang);
                                return new LocalizationEntry
                                {
                                    Language = lang,
                                    Messages = loc?.Messages
                                };
                            }));

                            int totalMessages = Entry.Messages.Count * locales.Count;
                            int msgOffset = 8 * totalMessages;

                            foreach (var loc in locales)
                            {
                                bool isDefault = loc.Language == Dicts.DefaultLanguage;

                                // If messages aren't there, add the default header again, so that the default messages will be displayed in game
                                if (loc.Messages == null)
                                {
                                    header.AddRange(defaultHeader);
                                    continue;
                                }

                                var errors = new ConcurrentBag<string>();

                                Parallel.ForEach(loc.Messages, msg =>
                                {
                                    try
                                    {
                                        msg.tempBytes = msg.ToBytes(loc.Language);
                                    }
                                    catch (Exception ex)
                                    {
                                        msg.tempBytes = null;
                                        errors.Add($"{Entry.NPCName}:\nError in message {msg.Name}:\n{ex.Message}");
                                    }
                                });

                                if (errors.Any())
                                {
                                    ShowMsg(CLIMode, errors.First());

                                    if (!ParseErrors.Contains(Entry.NPCName))
                                        ParseErrors.Add(Entry.NPCName);

                                    break;
                                }

                                foreach (var msg in loc.Messages)
                                {
                                    var messageBytes = msg.tempBytes ?? new List<byte>();
                                    msg.tempBytes = null;

                                    Helpers.Ensure4ByteAlign(messageBytes);
                                    msgData.AddRange(messageBytes);

                                    if (messageBytes.Count > 1280)
                                    {
                                        ShowMsg(CLIMode, $"{Entry.NPCName}: Message '{msg.Name}' exceeded 1280 bytes and could not be saved.");

                                        if (!ParseErrors.Contains(Entry.NPCName))
                                            ParseErrors.Add(Entry.NPCName);

                                        messageBytes.Clear();
                                    }

                                    void AddToHeader(List<byte> targetHeader)
                                    {
                                        targetHeader.AddRangeBigEndian(msgOffset);
                                        targetHeader.Add(msg.GetMessageTypePos());
                                        Helpers.Ensure2ByteAlign(targetHeader);
                                        targetHeader.AddRangeBigEndian((UInt16)messageBytes.Count);
                                    }

                                    // Create a default header in case any localizations are blank
                                    if (isDefault)
                                        AddToHeader(defaultHeader);

                                    AddToHeader(header);

                                    msgOffset += messageBytes.Count;
                                }
                            }

                            List<byte> MsgBytes = new List<byte>();
                            int len = 16 + header.Count + msgData.Count;

                            MsgBytes.AddRangeBigEndian(len);
                            MsgBytes.AddRangeBigEndian(0);
                            MsgBytes.AddRangeBigEndian(Data.Languages.Count + 1);
                            MsgBytes.AddRangeBigEndian(Entry.Messages.Count);
                            MsgBytes.AddRange(header);
                            MsgBytes.AddRange(msgData);

                            EntryBytes.AddRange(MsgBytes);

                            CurLen += len;
                            Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);
                            File.WriteAllBytes(cachedMsgFile, MsgBytes.ToArray());
                        }

                        #endregion

                        #region Animations

                        EntryBytes.AddRangeBigEndian((UInt32)Entry.Animations.Count());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        foreach (AnimationEntry Anim in Entry.Animations)
                        {
                            var anm = Helpers.SplitHeaderDefsString(Anim.HeaderDefinition);
                            Anim.Address = TryGetFromH(CLIMode, Entry.NPCName, (UInt32)Anim.Address, defines, anm[1]);
                            Anim.FileStart = (int)TryGetFromH(CLIMode, Entry.NPCName, (UInt32)Anim.FileStart, defines, anm[0]);
                            EntryBytes.AddRange(Anim.ToBytes());
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
                            var dl = Helpers.SplitHeaderDefsString(Dlist.HeaderDefinition);
                            Dlist.Address = TryGetFromH(CLIMode, Entry.NPCName, Dlist.Address, defines, dl[1]);
                            Dlist.FileStart = (int)TryGetFromH(CLIMode, Entry.NPCName, (uint)Dlist.FileStart, defines, dl[0]);
                            EntryBytes.AddRange(Dlist.ToBytes());
                        }

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 40 * Entry.ExtraDisplayLists.Count;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Colors

                        var parsedColors = Entry.ParseColorEntries().OrderBy(c => c.LimbID).ToList();
                        EntryBytes.AddRangeBigEndian((UInt32)parsedColors.Count);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        foreach (var color in parsedColors)
                            EntryBytes.AddRange(color.ToBytes());

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        CurLen += 4 * parsedColors.Count;
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region Extra segment data

                        var extraSegDataOffsets = new List<byte>();
                        var extraSegDataEntries = new List<byte>();
                        uint segOffset = 7 * 4;
                        CurLen += (int)segOffset + 4;

                        foreach (var segmentList in Entry.Segments)
                        {
                            uint segBytes = (uint)(12 * segmentList.Count);

                            extraSegDataOffsets.AddRangeBigEndian(segBytes != 0 ? segOffset : 0);
                            segOffset += segBytes;
                            CurLen += (int)segBytes;

                            foreach (var segEntry in segmentList)
                            {
                                var segDefs = Helpers.SplitHeaderDefsString(segEntry.HeaderDefinition);

                                segEntry.Address = TryGetFromH(CLIMode, Entry.NPCName, segEntry.Address, defines, segDefs[1]);
                                segEntry.FileStart = (int)TryGetFromH(CLIMode, Entry.NPCName, (uint)segEntry.FileStart, defines, segDefs[0]);

                                extraSegDataEntries.AddRange(segEntry.ToBytes());
                            }
                        }

                        EntryBytes.AddRangeBigEndian((uint)(extraSegDataOffsets.Count + extraSegDataEntries.Count));
                        CurLen += 4;

                        EntryBytes.AddRange(extraSegDataOffsets);
                        EntryBytes.AddRange(extraSegDataEntries);

                        Helpers.Ensure4ByteAlign(EntryBytes);
                        Helpers.ErrorIfExpectedLenWrong(EntryBytes, CurLen);

                        #endregion

                        #region CCode

                        if (Entry.EmbeddedOverlayCode.Code != "")
                        {
                            CompErrors = "";
                            byte[] Overlay;

                            //CCode.CreateCTempDirectory(Entry.EmbeddedOverlayCode.Code);


                            string CodeString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode);
                            CodeString = CCode.ReplaceGameVersionInclude(CodeString);
                            CodeString += GetIncludesText(Entry.EmbeddedOverlayCode.HeaderPaths);

                            string hash = Helpers.GetBase64Hash(CodeString);
                            string cachedAddrsFile = Path.Combine(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_funcsaddrs_" + hash);
                            string cachedcodeFile = Path.Combine(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_code_" + hash);

                            int addrsPreProc = -1;
                            int codePreProc = -1;

                            if (preProcessedFiles != null)
                            {
                                addrsPreProc = preProcessedFiles.FindIndex(x => x.identifier == cachedAddrsFile);
                                codePreProc = preProcessedFiles.FindIndex(x => x.identifier == cachedcodeFile);
                            }

                            if (addrsPreProc != -1 && codePreProc != -1)
                            {
                                Entry.EmbeddedOverlayCode = (CCodeEntry)preProcessedFiles[addrsPreProc].data;
                                Overlay = (byte[])preProcessedFiles[codePreProc].data;
                            }
                            else if (!CcacheInvalid && File.Exists(cachedcodeFile) && File.Exists(cachedAddrsFile))
                            {
                                Entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(File.ReadAllText(cachedAddrsFile), new JsonSerializerSettings() { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });
                                Overlay = File.ReadAllBytes(cachedcodeFile);
                            }
                            else
                            {
                                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_funcsaddrs_");
                                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{JsonFileName}_{EntriesDone}_code_");
                                Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_script");

                                Overlay = CCode.Compile(Data.CHeader, Program.Settings.LinkerPaths, Entry.EmbeddedOverlayCode, ref CompErrors);

                                if (Overlay != null)
                                {
                                    string CodeAddrsString = JsonConvert.SerializeObject(Entry.EmbeddedOverlayCode, new JsonSerializerSettings() { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });

                                    File.WriteAllText(cachedAddrsFile, CodeAddrsString);
                                    File.WriteAllBytes(cachedcodeFile, Overlay);
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

                                        UInt32 FuncAddr = UInt32.MaxValue;

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


                        string extData = JsonConvert.SerializeObject(new
                        {
                            Entry.Messages,
                            Entry.ExtraDisplayLists,
                            Entry.Segments,
                            Entry.Animations,
                        });

                        extData += Helpers.GetDefinesStringFromH(Entry.HeaderPath);

                        string extDataHash = Helpers.GetBase64Hash(extData);
                        string cachedExtDataFile = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_exdata_" + extDataHash);

                        foreach (ScriptEntry Scr in NonEmptyEntries)
                        {
                            Scripts.ScriptParser Par = new Scripts.ScriptParser(Data, Entry, Scr.Text, baseDefines);

                            string hash = Helpers.GetBase64Hash(Scr.Text);
                            string cachedFile = Path.Combine(Program.ScriptCachePath, $"{JsonFileName}_{EntriesDone}_script{scriptNum}_" + hash);

                            int cachedScriptId = -1;

                            if (preProcessedFiles != null)
                                cachedScriptId = preProcessedFiles.FindIndex(x => x.identifier == cachedFile);

                            if (cachedScriptId != -1)
                                ParsedScripts.Add(new Scripts.BScript() { Script = (byte[])preProcessedFiles[cachedScriptId].data, ParseErrors = new List<Scripts.ParseException>() });
                            else if (!cacheInvalid && File.Exists(cachedFile) && File.Exists(cachedExtDataFile))
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

                        CompilationData.Add(new Common.CompilationEntryData(EntryBytes));

                        if (ParseErrors.Count == 0)
                            Program.ConsoleWriteS($"OK{Environment.NewLine}");
                        else
                            break;
                    }

                    else
                    {
                        CompilationData.Add(new Common.CompilationEntryData(null));
                        Program.ConsoleWriteLineS($"Entry {EntriesDone} is blank or omitted.");
                    }

                    EntriesDone += 1;
                    CurProgress += ProgressPer;

                    if (progress != null)
                        progress.Report(new Common.ProgressReport($"Saving {EntriesDone}/{Data.Entries.Count}", CurProgress));
                }


                if (ParseErrors.Count != 0)
                {
                    ShowMsg(CLIMode,
                            $"File could not be saved." +
                            $"" + Environment.NewLine + Environment.NewLine +
                            $"There are errors in NPC: {String.Join(",", ParseErrors)}");

                    if (progress != null)
                        progress.Report(new Common.ProgressReport($"Done!", 100));
                }
                else
                {
                    List<byte> Output = new List<byte>();

                    Output.AddRangeBigEndian((UInt32)Data.Entries.Count());

                    if (Program.Settings.CompressIndividually)
                    {
                        Program.ConsoleWriteLineS($"Compressing...");

                        if (progress != null)
                            progress.Report(new Common.ProgressReport($"Compressing...", 100));
                    }

                    Parallel.ForEach(CompilationData, Entry =>
                    {
                        List<byte> outCompressed = null;

                        if (Entry.data == null)
                            return;

                        if (Program.Settings.CompressIndividually)
                        {
                            outCompressed = RLibrii.Szs.Yaz0.CompressYaz(Entry.data.ToArray(), 9).ToList();

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

                    foreach (var Entry in CompilationData)
                    {
                        EntryAddresses.AddRangeBigEndian(Offset);
                        EntryAddresses.AddRangeBigEndian(Entry.compressedSize);
                        EntryAddresses.AddRangeBigEndian(Entry.decompressedSize);

                        if (Entry.compressedSize != 0)
                            Offset += Entry.compressedSize;
                        else
                            Offset += Entry.decompressedSize;
                    }

                    Output.AddRange(EntryAddresses);

                    foreach (var Entry in CompilationData)
                    {
                        if (Entry.data != null)
                            Output.AddRange(Entry.data);
                    }

                    Program.ConsoleWriteLineS("\nDone!");

                    if (progress != null)
                        progress.Report(new Common.ProgressReport($"Done!", 100));

                    File.WriteAllBytes(outPath, Output.ToArray());
                }
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