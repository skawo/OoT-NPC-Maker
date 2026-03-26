using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPC_Maker.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NPC_Maker
{
    public partial class FileOps
    {
        private const int MaxMessageBytes = 1280;

        private static readonly JsonSerializerSettings IgnoreAttributeSettings =
            new JsonSerializerSettings { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() };

        // ── Settings ─────────────────────────────────────────────────────────────

        public static NPCMakerSettings ParseSettingsJSON(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                    return new NPCMakerSettings();

                var settings = JsonConvert.DeserializeObject<NPCMakerSettings>(File.ReadAllText(fileName));
                settings?.EnsureSettingsSanity();
                return settings ?? new NPCMakerSettings();
            }
            catch (Exception ex)
            {
                Program.ConsoleWriteLineS($"Failed to read settings: {ex.Message}");
                return new NPCMakerSettings();
            }
        }

        public static void SaveSettingsJSON(string path, NPCMakerSettings data)
        {
            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Program.ConsoleWriteLineS($"Failed to write settings: {ex.Message}");
            }
        }

        // ── NPC JSON I/O ──────────────────────────────────────────────────────────

        public static NPCFile ParseNPCJsonFile(string fileName, string jsonText = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonText))
                    jsonText = File.ReadAllText(fileName);

                var npcFile = JsonConvert.DeserializeObject<NPCFile>(jsonText);
                int version = npcFile.Version > 0 ? npcFile.Version : 1;

                version = MigrateToVersion2(ref npcFile, version > 1 ? null : JObject.Parse(jsonText), version);
                version = MigrateToVersion3(ref npcFile, version);

                if (version > 3) ProcessTextLinesForVersion4Plus(ref npcFile);
                if (version > 4) ProcessMessageLinesForVersion5Plus(ref npcFile);

                version = MigrateToVersion6(ref npcFile, version);
                version = MigrateToVersion7(ref npcFile, version);

                if (version >= 6) ProcessCHeader(ref npcFile);

                NormalizeLineBreaks(ref npcFile);
                ResolveHeaderDefines(ref npcFile);

                npcFile.Version = 7;
                return npcFile;
            }
            catch (Exception ex)
            {
                Program.ConsoleWriteLineS($"Failed to read JSON: {ex.Message}");
                return null;
            }
        }

        public static void SaveNPCJSON(string path, NPCFile data, IProgress<ProgressReport> progress = null, string json = null)
        {
            try
            {
                if (json == null)
                    json = ProcessNPCJSON(ref data, progress);

                if (json != null)
                    File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Program.ConsoleWriteLineS($"Failed to save JSON: {ex.Message}");
            }
        }

        public static string ProcessNPCJSON(ref NPCFile data, IProgress<ProgressReport> progress = null)
        {
            try
            {
                var output = Helpers.Clone<NPCFile>(data);
                string envNewline = Environment.NewLine;

                float progressPer = 100f / output.Entries.Count;
                int processedCount = 0;

                Parallel.ForEach(output.Entries, entry =>
                {
                    foreach (var script in entry.Scripts)
                    {
                        script.TextLines = Helpers.SplitToTrimmedLines(script.Text);
                        script.Text = null;
                    }

                    foreach (var message in entry.Messages)
                        FlattenMessage(message, envNewline);

                    foreach (var loc in entry.Localization)
                        foreach (var message in loc.Messages)
                            FlattenMessage(message, envNewline);

                    if (entry.EmbeddedOverlayCode?.Code != null)
                    {
                        entry.EmbeddedOverlayCode.CodeLines = Helpers.SplitToTrimmedLines(entry.EmbeddedOverlayCode.Code);
                        entry.EmbeddedOverlayCode.Code = null;
                    }

                    if (progress != null)
                    {
                        int current = Interlocked.Increment(ref processedCount);
                        if (current % 10 == 0 || current == output.Entries.Count)
                        {
                            float pct = Math.Min(current * progressPer, 100f);
                            progress.Report(new ProgressReport($"Saving {pct:0}%", pct));
                        }
                    }

                    entry.ClearHeaderValues();
                });

                foreach (var script in output.GlobalHeaders)
                {
                    script.TextLines = Helpers.SplitToTrimmedLines(script.Text);
                    script.Text = null;
                }

                output.CHeaderLines = Helpers.SplitToTrimmedLines(output.CHeader);
                output.CHeader = null;

                string json = JsonConvert.SerializeObject(output, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });

                return json.Replace(envNewline, "\n");
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
                return null;
            }
            catch (Exception ex)
            {
                Program.ConsoleWriteLineS($"Failed to process JSON: {ex.Message}");
                return null;
            }
        }

        // ── Dictionaries ──────────────────────────────────────────────────────────

        public static Dictionary<string, int> GetDictionary(string filename, bool allowFail = false)
        {
            var dict = new Dictionary<string, int>();
            string offendingRow = "";

            try
            {
                foreach (string row in File.ReadAllLines(filename))
                {
                    offendingRow = row;
                    string[] parts = row.Split(',');
                    dict.Add(parts[1], Convert.ToInt32(parts[0]));
                }
            }
            catch (Exception)
            {
                if (!allowFail)
                    Program.ConsoleWriteLineS($"{filename} is missing or incorrect. ({offendingRow})");
            }

            return dict;
        }

        public static Dictionary<string, string> GetDictionaryStringString(string filename, bool allowFail = false)
        {
            var dict = new Dictionary<string, string>();
            string offendingRow = "";

            try
            {
                foreach (string row in File.ReadAllLines(filename))
                {
                    offendingRow = row;
                    string[] parts = row.Split(',');
                    dict.Add(parts[0], parts[1]);
                }
            }
            catch (Exception)
            {
                if (!allowFail)
                    Program.ConsoleWriteLineS($"{filename} is missing or incorrect. ({offendingRow})");
            }

            return dict;
        }

        // ── Cache ─────────────────────────────────────────────────────────────────

        public static CacheStatus GetCacheStatus(ref NPCFile data)
        {
            string jsonFileName = Program.JsonPath.FilenameFromPath();
            string extHeaderPath = "";
            try { extHeaderPath = data.GetExtHeader(); } catch { }

            string globalHeadersHash = GetGlobalHeadersHash(data, extHeaderPath);
            string dictsHash = GetDictsHash();
            string cHeaderHash = GetCHeaderHash(data);

            string ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            string cachedHeaders = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_gh_{ver}_{globalHeadersHash}");
            string cachedDicts = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_dicts_{ver}_{dictsHash}");
            string cachedCHeader = Path.Combine(Program.CCachePath, $"{jsonFileName}_ch_{ver}_{cHeaderHash}");

            var status = new CacheStatus { CacheInvalid = false, CCacheInvalid = false };

            if (!File.Exists(cachedHeaders) || !File.Exists(cachedDicts))
            {
                status.CacheInvalid = true;
                Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{jsonFileName}_");
                File.WriteAllText(cachedHeaders, "");
                File.WriteAllText(cachedDicts, "");
            }

            if (!File.Exists(cachedCHeader))
            {
                status.CCacheInvalid = true;
                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{jsonFileName}_");
                File.WriteAllText(cachedCHeader, "");
            }

            return status;
        }

        private static string GetGlobalHeadersHash(NPCFile data, string extHeaderPath)
        {
            var sb = new StringBuilder();
            foreach (var h in data.GlobalHeaders)
                sb.AppendLine(h.Text);
            sb.Append(extHeaderPath);
            return Helpers.GetBase64Hash(sb.ToString());
        }

        private static string GetDictsHash()
        {
            return Helpers.GetBase64Hash(JsonConvert.SerializeObject(new
            {
                Dicts.Actors,
                Dicts.ObjectIDs,
                Dicts.SFXes,
                Dicts.LinkAnims,
                Dicts.Music
            }));
        }

        private static string GetCHeaderHash(NPCFile data)
        {
            data.CHeader = CCode.ReplaceGameVersionInclude(data.CHeader);

            var sb = new StringBuilder(data.CHeader);

            foreach (var linkerPath in Helpers.ResolveSemicolonPaths(Program.Settings.LinkerPaths))
                if (File.Exists(linkerPath))
                    sb.Append(File.ReadAllText(linkerPath));

            sb.Append(JsonConvert.SerializeObject(new
            {
                Program.Settings.Linker,
                Program.Settings.LinkerPaths,
                Program.Settings.Library,
                Program.Settings.GCCFlags,
                Program.Settings.IncludePaths
            }));

            return Helpers.GetBase64Hash(sb.ToString());
        }

        private static string GetIncludesText(List<string> includeList)
        {
            var seen = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
            var results = new string[includeList.Count];

            Parallel.For(0, includeList.Count, i =>
            {
                string hPath = includeList[i];
                string cleanPath = Helpers.DenormalizeExtPath(hPath);
                var fi = new FileInfo(cleanPath);

                if (!fi.Exists)
                {
                    string trimmed = cleanPath.TrimStart('/', '\\');
                    fi = new FileInfo(trimmed);
                    cleanPath = trimmed;
                }

                if (!seen.TryAdd(cleanPath, 0))
                    return;

                results[i] = fi.Exists
                    ? fi.LastWriteTime.Ticks.ToString()
                    : Helpers.GenerateTemporaryFolderName();

                if (!fi.Exists)
                    Program.ConsoleWriteLineS($"Warning: Couldn't resolve path {hPath} for cache use.");
            });

            return string.Concat(results);
        }

        // ── Header Defines ────────────────────────────────────────────────────────

        public static uint ResolveHeaderDefineForField(string name, Dictionary<string, string> defines, uint field)
        {
            if (string.IsNullOrEmpty(name))
                return field;

            var h = Helpers.GetHDefineFromName(name, defines);
            return h?.Value1 != null ? (uint)h.Value1 : field;
        }

        public static uint ResolveHeaderDefineOrFail(string npcName, string name, Dictionary<string, string> defines, uint field, StringBuilder errors)
        {
            if (string.IsNullOrEmpty(name))
                return field;

            try
            {
                var h = Helpers.GetHDefineFromName(name, defines);
                if (h?.Value1 != null)
                    return (uint)h.Value1;

                errors.AppendLine($"Entry {npcName}, Definition \"{name}\": could not be resolved.");
                return field;
            }
            catch (Exception ex)
            {
                errors.AppendLine($"Entry {npcName}, Definition \"{name}\": {ex.Message}");
                return field;
            }
        }

        // ── Compilation ──────────────────────────────────────────────────

        public static void PreprocessCodeAndScripts(string outPath, string outputDepsPath, NPCFile data,
                                                          CacheStatus cacheStatus, IProgress<ProgressReport> progress, bool cliMode)
        {
            float progressPer = 100f / data.Entries.Count;
            float curProgress = 0f;
            int lastReported = 0;

            Program.ConsoleWriteS("Compiling...");

            string baseDefines = Scripts.ScriptHelpers.GetBaseDefines(data);
            string jsonFileName = Program.JsonPath.FilenameFromPath();

            var scriptCacheFiles = new HashSet<string>(Directory.GetFiles(Program.ScriptCachePath));
            var cCacheFiles = new HashSet<string>(Directory.GetFiles(Program.CCachePath));
            var results = new ConcurrentDictionary<string, object>();

            Parallel.For(0, data.Entries.Count,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 3 },
                entryID =>
                {
                    try
                    {
                        var entry = data.Entries[entryID];

                        if (entry.IsNull || entry.Omitted)
                            return;

                        entry.ClearHeaderValues();

                        string prefix = jsonFileName + "_" + entryID + "_";
                        var cs = new RecompilationStatus();
                        string compErrors;

                        ProcessCCode(data, entry, entryID, prefix, cacheStatus,
                                     cCacheFiles, scriptCacheFiles, results, ref cs, out compErrors);

                        bool extDataExists = CheckExtDataCache(entry, prefix, scriptCacheFiles, out string extDataFile);

                        ProcessScripts(data, entry, prefix, baseDefines, cacheStatus,
                                       extDataExists, scriptCacheFiles, results, ref cs);

                        results[entryID.ToString()] = cs;

                        if (!extDataExists)
                        {
                            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, prefix + "exdata_");
                            File.Create(extDataFile).Dispose();
                        }
                    }
                    catch { }
                    finally
                    {
                        Helpers.AddInterlocked(ref curProgress, progressPer);
                        int percent = (int)curProgress;

                        if (Interlocked.CompareExchange(ref lastReported, percent, percent - 1) == percent - 1)
                        {
                            if (progress != null)
                                progress.Report(new ProgressReport($"Compiling {percent}%", curProgress));
                            else
                                Program.ConsoleWriteS($"\rCompiling {percent}%    ");
                        }
                    }
                });

            Program.ConsoleWriteLineS("\nPre-processing done!");

            SaveBinaryFile(outPath, outputDepsPath, ref data, progress, baseDefines,
                           new CacheStatus { CCacheInvalid = false, CacheInvalid = false }, results, cliMode);

            CCode.CleanupStandardCompilationArtifacts();
        }

        public static void SaveBinaryFile(string outPath, string outputDepsPath, ref NPCFile data, IProgress<ProgressReport> progress, string baseDefines,
                                         CacheStatus cacheStatus, ConcurrentDictionary<string, object> preProcessedFiles, bool cliMode)
        {
            if (!data.Entries.Any())
            {
                ShowMsg(cliMode, "Nothing to save.");
                Program.CompileThereWereErrors = false;
                return;
            }

            try
            {
                int offset = data.Entries.Count * 12 + 4;
                string jsonFileName = Program.JsonPath.FilenameFromPath();

                var parseErrors = new ConcurrentBag<string>();
                var compilationData = new CompilationEntryData[data.Entries.Count];
                var definesCache = new ConcurrentDictionary<string, Dictionary<string, string>>();

                float progressPer = 100f / data.Entries.Count;
                float curProgress = 0f;
                int entriesDone = 0;

                progress?.Report(new ProgressReport("Saving...", 0));

                var cts = new CancellationTokenSource();
                var localData = data;

                Action<int> processEntry = i =>
                {
                    if (cts.IsCancellationRequested)
                        return;

                    var entry = localData.Entries[i];

                    if (entry.IsNull || entry.Omitted)
                    {
                        compilationData[i] = new CompilationEntryData(null);
                        Program.ConsoleWriteLineS($"Entry {i} is blank or omitted.");
                    }
                    else
                    {
                        Program.ConsoleWriteS($"Processing entry {i}: {entry.NPCName}... ");

                        string compErrors = "";
                        var cs = new RecompilationStatus();

                        object csi;

                        if (preProcessedFiles?.TryGetValue(i.ToString(), out csi) == true)
                            cs = (RecompilationStatus)csi;

                        entry.ClearHeaderValues();
                        var entryBytes = BuildEntryBytes(entry, localData, cliMode, baseDefines,
                                                         cacheStatus, jsonFileName, i, preProcessedFiles,
                                                         parseErrors, definesCache, ref cs, ref compErrors);

                        if (parseErrors.IsEmpty)
                        {
                            compilationData[i] = new CompilationEntryData(entryBytes);
                            Program.ConsoleWriteS($"OK {cs}{Environment.NewLine}");
                        }
                        else
                        {
                            cts.Cancel();
                            return;
                        }
                    }

                    Helpers.AddInterlocked(ref curProgress, progressPer);
                    Interlocked.Increment(ref entriesDone);
                    progress?.Report(new ProgressReport($"Saving {entriesDone}/{localData.Entries.Count}", curProgress));
                };

                if (!Program.Settings.CompileInParallel || (cliMode && !Program.consoleSilent))
                {
                    for (int i = 0; i < localData.Entries.Count; i++)
                        processEntry(i);
                }
                else
                {
                    try
                    {
                        Parallel.For(0, localData.Entries.Count,
                            new ParallelOptions { CancellationToken = cts.Token },
                            i => processEntry(i));
                    }
                    catch (OperationCanceledException) { }
                }

                data = localData;

                if (!parseErrors.IsEmpty)
                {
                    ShowMsg(cliMode,
                        $"File could not be saved.{Environment.NewLine}{Environment.NewLine}" +
                        $"There are errors in NPC: {string.Join(",", parseErrors.Distinct())}");

                    progress?.Report(new ProgressReport("Done!", 100));
                    return;
                }

                WriteOutput(outPath, outputDepsPath, data, compilationData.ToList(), progress, cliMode, ref offset);
            }
            catch (Exception ex)
            {
                ShowMsg(cliMode, $"Error writing file: {ex.Message}");
            }
            finally
            {
                Program.CompileInProgress = false;
            }
        }

        // ── Entry Building ────────────────────────────────────────────────────────

        private const int InitialFieldsSize = 56;

        private static List<byte> BuildEntryBytes(NPCEntry entry, NPCFile data, bool cliMode, string baseDefines,
                                                  CacheStatus cacheStatus, string jsonFileName, int entriesDone,
                                                  ConcurrentDictionary<string, object> preProcessedFiles, ConcurrentBag<string> parseErrors,
                                                  ConcurrentDictionary<string, Dictionary<string, string>> definesCache,
                                                  ref RecompilationStatus cs, ref string compErrors)
        {
            var defines = definesCache.GetOrAdd(entry.HeaderPath, Helpers.GetDefinesFromHeaders);
            var entryBytes = new List<byte>(InitialFieldsSize);
            int curLen = 0;

            BuildFixedFields(entryBytes, entry, data, defines, cliMode, parseErrors, ref curLen);
            BuildMessages(entryBytes, entry, data, jsonFileName, entriesDone, parseErrors, cliMode, ref cs, ref curLen);
            BuildAnimations(entryBytes, entry, defines, cliMode, parseErrors, ref curLen);
            BuildExtraDisplayLists(entryBytes, entry, defines, cliMode, parseErrors, ref curLen);
            BuildColors(entryBytes, entry, ref curLen);
            BuildSegments(entryBytes, entry, defines, cliMode, parseErrors, ref curLen);
            BuildCCode(entryBytes, entry, data, jsonFileName, entriesDone, cacheStatus,
                       preProcessedFiles, parseErrors, cliMode, ref cs, ref compErrors, ref curLen);
            BuildScripts(entryBytes, entry, data, jsonFileName, entriesDone, baseDefines,
                         cacheStatus, preProcessedFiles, parseErrors, cliMode, ref cs, ref curLen);

            return entryBytes;
        }

        private static void BuildFixedFields(List<byte> entryBytes, NPCEntry entry, NPCFile data, Dictionary<string, string> defines, bool cliMode, ConcurrentBag<string> parseErrors, ref int curLen)
        {
            entryBytes.AddRange(new byte[]
            {
                entry.CutsceneID,
                entry.HeadLimb,
                entry.WaistLimb,
                entry.TargetLimb,
                entry.PathID,
                entry.BlinkSpeed,
                entry.TalkSpeed,
                entry.HierarchyType,
                entry.TalkSegment,
                entry.BlinkSegment,
                entry.AnimationType,
                entry.MovementType,
                entry.WaistHorizAxis,
                entry.WaistVertAxis,
                entry.HeadHorizAxis,
                entry.HeadVertAxis,
                entry.LookAtType,
                entry.TargetDistance,
                entry.EffectIfAttacked,
                entry.Mass,
                entry.Alpha,
                entry.LightLimb,
                entry.EnvironmentColor.R,
                entry.EnvironmentColor.G,
                entry.EnvironmentColor.B,
                entry.LightColor.R,
                entry.LightColor.G,
                entry.LightColor.B,
                entry.AnimInterpFrames, 0, 0, 0,
                Convert.ToByte(entry.HasCollision),
                Convert.ToByte(entry.PushesSwitches),
                Convert.ToByte(entry.IgnoreYAxis),
                Convert.ToByte(entry.IsAlwaysActive),
                Convert.ToByte(entry.IsAlwaysDrawn),
                Convert.ToByte(entry.ExecuteJustScript),
                Convert.ToByte(entry.ReactsIfAttacked),
                Convert.ToByte(entry.OpensDoors),
                Convert.ToByte(entry.CastsShadow),
                Convert.ToByte(entry.IsTargettable),
                Convert.ToByte(entry.LoopPath),
                Convert.ToByte(entry.EnvironmentColor.A > 0),
                Convert.ToByte(entry.FadeOut),
                Convert.ToByte(entry.GenLight),
                Convert.ToByte(entry.Glow),
                Convert.ToByte(entry.DEBUGShowCols),
                Convert.ToByte(entry.VisibleUnderLensOfTruth),
                Convert.ToByte(entry.Invisible),
                Convert.ToByte(entry.ExistInAllRooms),
                Convert.ToByte(entry.NumVars),
                Convert.ToByte(entry.NumFVars),
                Convert.ToByte(entry.DEBUGExDlistEditor),
                Convert.ToByte(entry.DEBUGLookAtEditor),
                Convert.ToByte(entry.DEBUGPrintToScreen)
            });

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += InitialFieldsSize;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            entryBytes.AddRangeBigEndian(entry.ObjectID);
            entryBytes.AddRangeBigEndian(entry.LookAtDegreesVertical);
            entryBytes.AddRangeBigEndian(entry.LookAtDegreesHorizontal);
            entryBytes.AddRangeBigEndian(entry.CollisionRadius);
            entryBytes.AddRangeBigEndian(entry.CollisionHeight);
            entryBytes.AddRangeBigEndian(entry.CollisionYShift);
            entryBytes.AddRangeBigEndian(entry.ShadowRadius);
            entryBytes.AddRangeBigEndian(entry.MovementDistance);
            entryBytes.AddRangeBigEndian(entry.MaxDistRoam);
            entryBytes.AddRangeBigEndian(entry.PathStartNodeID);
            entryBytes.AddRangeBigEndian(entry.PathEndNodeID);
            entryBytes.AddRangeBigEndian(entry.MovementDelayTime);
            entryBytes.AddRangeBigEndian(entry.TimedPathStart);
            entryBytes.AddRangeBigEndian(entry.TimedPathEnd);
            entryBytes.AddRangeBigEndian(entry.SfxIfAttacked);
            entryBytes.AddRangeBigEndian(entry.NPCToRide);
            entryBytes.AddRangeBigEndian(entry.LightRadius);
            entryBytes.AddRangeBigEndian(entry.TargetPositionOffsets);
            entryBytes.AddRangeBigEndian(entry.ModelPositionOffsets);
            entryBytes.AddRangeBigEndian(entry.LightPositionOffsets);

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 52;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            entryBytes.AddRangeBigEndian(entry.ModelScale);
            entryBytes.AddRangeBigEndian(entry.TalkRadius);
            entryBytes.AddRangeBigEndian(entry.MovementSpeed);
            entryBytes.AddRangeBigEndian(entry.GravityForce);
            entryBytes.AddRangeBigEndian(entry.SmoothingConstant);
            entryBytes.AddRangeBigEndian(TryGetFromH(cliMode, entry.NPCName, entry.Hierarchy, defines, entry.SkeletonHeaderDefinition, parseErrors));
            entryBytes.AddRangeBigEndian(TryGetFromH(cliMode, entry.NPCName, (uint)entry.FileStart, defines, entry.FileStartHeaderDefinition, parseErrors));
            entryBytes.AddRangeBigEndian(entry.CullForward);
            entryBytes.AddRangeBigEndian(entry.CullDown);
            entryBytes.AddRangeBigEndian(entry.CullScale);
            entryBytes.AddRangeBigEndian(entry.LookAtPositionOffsets);

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 52;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            entryBytes.AddRange(entry.GetBlinkPatternBytes());
            entryBytes.AddRange(entry.GetTalkPatternBytes());

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 8;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildMessages(List<byte> entryBytes, NPCEntry entry, NPCFile data, string jsonFileName, int entriesDone,
                                          ConcurrentBag<string> parseErrors, bool cliMode, ref RecompilationStatus cs, ref int curLen)
        {
            string msgDataHash = Helpers.GetBase64Hash(JsonConvert.SerializeObject(new { entry.Messages, entry.Localization }));
            string cachedMsgFile = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_{entriesDone}_msg_{msgDataHash}");

            if (File.Exists(cachedMsgFile))
            {
                var cached = File.ReadAllBytes(cachedMsgFile);
                entryBytes.AddRange(cached);
                curLen += cached.Length;
                Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
                return;
            }

            cs.Messages = true;
            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{jsonFileName}_{entriesDone}_msg_");

            var locales = BuildLocaleList(entry, data);
            int msgOffset = 8 * entry.Messages.Count * locales.Count;
            var header = new List<byte>();
            var defaultHeader = new List<byte>();
            var msgData = new List<byte>();

            foreach (var loc in locales)
            {
                bool isDefault = loc.Language == Lists.DefaultLanguage;

                if (loc.Messages == null)
                {
                    header.AddRange(defaultHeader);
                    continue;
                }

                var errors = new ConcurrentBag<string>();
                Parallel.ForEach(loc.Messages, msg =>
                {
                    try { msg.TempBytes = msg.ToBytes(loc.Language); }
                    catch (Exception ex)
                    {
                        msg.TempBytes = null;
                        errors.Add($"{entry.NPCName}:\nError in message {msg.Name}:\n{ex.Message}");
                    }
                });

                if (errors.Any())
                {
                    ShowMsg(cliMode, errors.First());
                    if (!parseErrors.Contains(entry.NPCName)) parseErrors.Add(entry.NPCName);
                    break;
                }

                foreach (var msg in loc.Messages)
                {
                    var messageBytes = msg.TempBytes ?? new List<byte>();
                    msg.TempBytes = null;

                    Helpers.Ensure4ByteAlign(messageBytes);
                    msgData.AddRange(messageBytes);

                    if (messageBytes.Count > MaxMessageBytes)
                    {
                        ShowMsg(cliMode, $"{entry.NPCName}: Message '{msg.Name}' exceeded {MaxMessageBytes} bytes and could not be saved.");
                        if (!parseErrors.Contains(entry.NPCName)) parseErrors.Add(entry.NPCName);
                        messageBytes.Clear();
                    }

                    if (isDefault) AppendMessageHeader(defaultHeader, msgOffset, msg, messageBytes.Count);
                    AppendMessageHeader(header, msgOffset, msg, messageBytes.Count);

                    msgOffset += messageBytes.Count;
                }
            }

            var msgBytes = new List<byte>();
            int len = 16 + header.Count + msgData.Count;

            msgBytes.AddRangeBigEndian(len);
            msgBytes.AddRangeBigEndian(0);
            msgBytes.AddRangeBigEndian(data.Languages.Count + 1);
            msgBytes.AddRangeBigEndian(entry.Messages.Count);
            msgBytes.AddRange(header);
            msgBytes.AddRange(msgData);

            entryBytes.AddRange(msgBytes);
            curLen += len;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            File.WriteAllBytes(cachedMsgFile, msgBytes.ToArray());
        }

        private static void AppendMessageHeader(List<byte> target, int msgOffset, MessageEntry msg, int byteCount)
        {
            target.AddRangeBigEndian(msgOffset);
            target.Add(msg.GetMessageTypePos());
            Helpers.Ensure2ByteAlign(target);
            target.AddRangeBigEndian((ushort)byteCount);
        }

        private static void BuildAnimations(List<byte> entryBytes, NPCEntry entry, Dictionary<string, string> defines, bool cliMode, ConcurrentBag<string> parseErrors, ref int curLen)
        {
            entryBytes.AddRangeBigEndian((uint)entry.Animations.Count);
            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            foreach (var anim in entry.Animations)
            {
                var parts = Helpers.SplitHeaderDefsString(anim.HeaderDefinition);

                uint curAddr = anim.Address;
                int curFileStart = anim.FileStart;

                anim.Address = TryGetFromH(cliMode, entry.NPCName, (uint)anim.Address, defines, parts[1], parseErrors);
                anim.FileStart = (int)TryGetFromH(cliMode, entry.NPCName, (uint)anim.FileStart, defines, parts[0], parseErrors);
                entryBytes.AddRange(anim.ToBytes());

                anim.Address = curAddr;
                anim.FileStart = curFileStart;
            }

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 16 * entry.Animations.Count;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildExtraDisplayLists(List<byte> entryBytes, NPCEntry entry, Dictionary<string, string> defines, bool cliMode, ConcurrentBag<string> parseErrors, ref int curLen)
        {
            entryBytes.AddRangeBigEndian((uint)entry.ExtraDisplayLists.Count);
            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            foreach (var dlist in entry.ExtraDisplayLists)
            {
                uint curAddr = dlist.Address;
                int curFileStart = dlist.FileStart;

                var parts = Helpers.SplitHeaderDefsString(dlist.HeaderDefinition);
                dlist.Address = TryGetFromH(cliMode, entry.NPCName, dlist.Address, defines, parts[1], parseErrors);
                dlist.FileStart = (int)TryGetFromH(cliMode, entry.NPCName, (uint)dlist.FileStart, defines, parts[0], parseErrors);
                entryBytes.AddRange(dlist.ToBytes());

                dlist.Address = curAddr;
                dlist.FileStart = curFileStart;
            }

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 40 * entry.ExtraDisplayLists.Count;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildColors(List<byte> entryBytes, NPCEntry entry, ref int curLen)
        {
            var colors = entry.ParseColorEntries().OrderBy(c => c.LimbID).ToList();

            entryBytes.AddRangeBigEndian((uint)colors.Count);
            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            foreach (var color in colors)
                entryBytes.AddRange(color.ToBytes());

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4 * colors.Count;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildSegments(List<byte> entryBytes, NPCEntry entry, Dictionary<string, string> defines, bool cliMode, ConcurrentBag<string> parseErrors, ref int curLen)
        {
            var segOffsets = new List<byte>();
            var segEntries = new List<byte>();
            uint segOffset = 7 * 4;
            curLen += (int)segOffset + 4;

            foreach (var segmentList in entry.Segments)
            {
                uint segBytes = (uint)(12 * segmentList.Count);
                segOffsets.AddRangeBigEndian(segBytes != 0 ? segOffset : 0);
                segOffset += segBytes;
                curLen += (int)segBytes;

                foreach (var segEntry in segmentList)
                {
                    var parts = Helpers.SplitHeaderDefsString(segEntry.HeaderDefinition);

                    uint curAddr = segEntry.Address;
                    int curFileStart = segEntry.FileStart;

                    segEntry.Address = TryGetFromH(cliMode, entry.NPCName, segEntry.Address, defines, parts[1], parseErrors);
                    segEntry.FileStart = (int)TryGetFromH(cliMode, entry.NPCName, (uint)segEntry.FileStart, defines, parts[0], parseErrors);
                    segEntries.AddRange(segEntry.ToBytes());

                    segEntry.Address = curAddr;
                    segEntry.FileStart = curFileStart;
                }
            }

            entryBytes.AddRangeBigEndian((uint)(segOffsets.Count + segEntries.Count));
            curLen += 4;
            entryBytes.AddRange(segOffsets);
            entryBytes.AddRange(segEntries);

            Helpers.Ensure4ByteAlign(entryBytes);
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildCCode(List<byte> entryBytes, NPCEntry entry, NPCFile data, string jsonFileName, int entriesDone,
                                       CacheStatus cacheStatus, ConcurrentDictionary<string, object> preProcessedFiles,
                                       ConcurrentBag<string> parseErrors, bool cliMode,
                                       ref RecompilationStatus cs, ref string compErrors, ref int curLen)
        {
            if (string.IsNullOrEmpty(entry.EmbeddedOverlayCode.Code))
            {
                entryBytes.AddRangeBigEndian(-1);
                curLen += 4;
                return;
            }

            string prefix = jsonFileName + "_" + entriesDone + "_";
            byte[] overlay = ResolveOverlay(data, entry, prefix, cacheStatus,
                                            preProcessedFiles, null, null,
                                            entriesDone, ref cs, ref compErrors);

            if (overlay == null)
            {
                if (!parseErrors.Contains(entry.NPCName))
                    parseErrors.Add(entry.NPCName);
                return;
            }

            curLen += 4;

            if (entry.EmbeddedOverlayCode.Functions == null || entry.EmbeddedOverlayCode.Functions.Count == 0)
            {
                entryBytes.AddRangeBigEndian(-1);
                return;
            }

            entryBytes.AddRangeBigEndian(overlay.Length);

            var funcsList = new List<byte>();
            var funcsWhenList = new List<byte>();
            int funcCount = entry.EmbeddedOverlayCode.FuncsRunWhen.GetLength(0);

            for (int i = 0; i < funcCount; i++)
            {
                string fname = entry.EmbeddedOverlayCode.SetFuncNames[i];
                int funcIdx = entry.EmbeddedOverlayCode.Functions.FindIndex(x => x.Symbol == fname);

                if (funcIdx == -1 && !string.IsNullOrEmpty(fname))
                {
                    ShowMsg(cliMode, $"{entry.NPCName}: Function {fname} not found in the C Code!");
                    return;
                }

                uint funcAddr = funcIdx >= 0
                    ? entry.EmbeddedOverlayCode.Functions[funcIdx].Addr
                    : uint.MaxValue;

                funcsList.AddRangeBigEndian(funcAddr);
                funcsWhenList.Add((byte)entry.EmbeddedOverlayCode.FuncsRunWhen[i, 1]);
            }

            entryBytes.AddRange(funcsList);
            entryBytes.AddRange(funcsWhenList);
            Helpers.Ensure4ByteAlign(entryBytes);

            curLen += 24 + 8;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            entryBytes.AddRange(overlay);
            curLen += overlay.Length;
            Helpers.Ensure4ByteAlign(entryBytes);

            if (overlay.Length % 4 != 0)
                curLen += overlay.Length % 4;

            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildScripts(List<byte> entryBytes, NPCEntry entry, NPCFile data, string jsonFileName, int entriesDone,
                                         string baseDefines, CacheStatus cacheStatus, ConcurrentDictionary<string, object> preProcessedFiles,
                                         ConcurrentBag<string> parseErrors, bool cliMode, ref RecompilationStatus cs, ref int curLen)
        {
            var nonEmptyScripts = entry.Scripts.FindAll(x => !string.IsNullOrEmpty(x.Text));
            entryBytes.AddRangeBigEndian((uint)nonEmptyScripts.Count);
            curLen += 4;
            Helpers.Ensure4ByteAlign(entryBytes);
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            string extData = BuildExtDataString(entry);
            string cachedExtData = Path.Combine(Program.ScriptCachePath, jsonFileName + "_" + entriesDone + "_exdata_" + Helpers.GetBase64Hash(extData));
            bool extDataExists = File.Exists(cachedExtData);

            var parsedScripts = new List<Scripts.BScript>();
            int scriptNum = 0;

            foreach (var scr in nonEmptyScripts)
            {
                string scriptPrefix = jsonFileName + "_" + entriesDone + "_script" + scriptNum + "_";
                string cachedScriptFile = Path.Combine(Program.ScriptCachePath,
                    scriptPrefix + Helpers.GetBase64Hash(scr.Text));

                var result = ResolveScript(data, entry, scr, scriptPrefix, cachedScriptFile,
                                           baseDefines, cacheStatus, extDataExists,
                                           preProcessedFiles, null, ref cs);

                parsedScripts.Add(result);
                scriptNum++;
            }

            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, jsonFileName + "_" + entriesDone + "_exdata_");
            File.Create(cachedExtData).Dispose();

            int scrOffset = 0;
            foreach (var scr in parsedScripts)
            {
                entryBytes.AddRangeBigEndian(scrOffset);
                scrOffset += scr.Script.Length;
                curLen += 4;
            }

            foreach (var scr in parsedScripts)
            {
                entryBytes.AddRange(scr.Script);
                curLen += scr.Script.Length;

                if (scr.ParseErrors.Count != 0)
                {
                    if (!parseErrors.Contains(entry.NPCName))
                        parseErrors.Add(entry.NPCName);

                    Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}" +
                                      $"Script \"{scr.Name}\" had errors:{Environment.NewLine}");
                    Console.WriteLine(string.Join(Environment.NewLine, scr.ParseErrors));
                }
            }

            Helpers.Ensure4ByteAlign(entryBytes);
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        // ── Overlay / Script Resolution ───────────────────────────────────────────

        private static CCacheKeys GetCCacheKeys(string jsonEntryPrefix, CCodeEntry overlayCode)
        {
            string codeString = JsonConvert.SerializeObject(overlayCode)
                              + CCode.ReplaceGameVersionInclude("")
                              + GetIncludesText(overlayCode.HeaderPaths);

            string hash = Helpers.GetBase64Hash(codeString);

            return new CCacheKeys
            {
                CachedAddrs = Path.Combine(Program.CCachePath, jsonEntryPrefix + "funcsaddrs_" + hash),
                CachedCode = Path.Combine(Program.CCachePath, jsonEntryPrefix + "code_" + hash)
            };
        }

        private static byte[] ResolveOverlay(NPCFile data, NPCEntry entry, string jsonEntryPrefix, CacheStatus cacheStatus, ConcurrentDictionary<string, object> preProcessedFiles,
                                             HashSet<string> cCacheFiles, HashSet<string> scriptCacheFiles, int entryID, ref RecompilationStatus cs, ref string compErrors)
        {
            var keys = GetCCacheKeys(jsonEntryPrefix, entry.EmbeddedOverlayCode);

            if (preProcessedFiles != null)
            {
                object addrsData, codeData;
                preProcessedFiles.TryGetValue(keys.CachedAddrs, out addrsData);
                preProcessedFiles.TryGetValue(keys.CachedCode, out codeData);

                if (addrsData != null && codeData != null)
                {
                    entry.EmbeddedOverlayCode = (CCodeEntry)addrsData;
                    return (byte[])codeData;
                }
            }

            bool diskCacheHit = !cacheStatus.CCacheInvalid
                && (cCacheFiles != null ? cCacheFiles.Contains(keys.CachedAddrs) : File.Exists(keys.CachedAddrs))
                && (cCacheFiles != null ? cCacheFiles.Contains(keys.CachedCode) : File.Exists(keys.CachedCode));

            if (diskCacheHit)
            {
                entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(
                    File.ReadAllText(keys.CachedAddrs), IgnoreAttributeSettings);

                return File.ReadAllBytes(keys.CachedCode);
            }

            compErrors = "";

            Helpers.DeleteFileStartingWith(Program.CCachePath, jsonEntryPrefix + "funcsaddrs_", cCacheFiles);
            Helpers.DeleteFileStartingWith(Program.CCachePath, jsonEntryPrefix + "code_", cCacheFiles);

            if (string.IsNullOrEmpty(entry.EmbeddedOverlayCode.Code))
                return null;

            cs.CCode = true;
            byte[] overlay = CCode.Compile(data.CHeader, Program.Settings.LinkerPaths, entry.EmbeddedOverlayCode, ref compErrors, out _, "NPCCOMPILE" + entryID);

            if (overlay != null)
            {
                Helpers.DeleteFileStartingWith(Program.ScriptCachePath, jsonEntryPrefix + "script", scriptCacheFiles);

                File.WriteAllText(keys.CachedAddrs, JsonConvert.SerializeObject(entry.EmbeddedOverlayCode, IgnoreAttributeSettings));
                File.WriteAllBytes(keys.CachedCode, overlay);

                preProcessedFiles?.TryAdd(keys.CachedAddrs, entry.EmbeddedOverlayCode);
                preProcessedFiles?.TryAdd(keys.CachedCode, overlay);
            }

            return overlay;
        }

        private static void ProcessCCode(NPCFile data, NPCEntry entry, int entryID, string jsonEntryPrefix, CacheStatus cacheStatus,
                                         HashSet<string> cCacheFiles, HashSet<string> scriptCacheFiles, ConcurrentDictionary<string, object> results,
                                         ref RecompilationStatus cs, out string compErrors)
        {
            compErrors = "";

            byte[] overlay = ResolveOverlay(data, entry, jsonEntryPrefix, cacheStatus,
                                            null, cCacheFiles, scriptCacheFiles,
                                            entryID, ref cs, ref compErrors);

            if (overlay != null)
            {
                var keys = GetCCacheKeys(jsonEntryPrefix, entry.EmbeddedOverlayCode);
                results[keys.CachedAddrs] = entry.EmbeddedOverlayCode;
                results[keys.CachedCode] = overlay;
            }
        }

        private static Scripts.BScript ResolveScript(NPCFile data, NPCEntry entry, ScriptEntry scrEntry, string scriptPrefix, string cachedScriptFile, string baseDefines,
                                                    CacheStatus cacheStatus, bool extDataExists, ConcurrentDictionary<string, object> preProcessedFiles, HashSet<string> scriptCacheFiles,
                                                    ref RecompilationStatus cs)
        {
            if (preProcessedFiles != null)
            {
                object cached;
                preProcessedFiles.TryGetValue(cachedScriptFile, out cached);
                if (cached != null)
                    return new Scripts.BScript
                    {
                        Script = (byte[])cached,
                        ParseErrors = new List<Scripts.ParseException>()
                    };
            }

            bool diskCacheHit = !cacheStatus.CacheInvalid
                && extDataExists
                && (scriptCacheFiles != null
                    ? scriptCacheFiles.Contains(cachedScriptFile)
                    : File.Exists(cachedScriptFile));

            if (diskCacheHit)
                return new Scripts.BScript
                {
                    Script = File.ReadAllBytes(cachedScriptFile),
                    ParseErrors = new List<Scripts.ParseException>()
                };

            cs.Scripts = true;
            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, scriptPrefix, scriptCacheFiles);

            var result = new Scripts.ScriptParser(ref data, entry, scrEntry.Text, baseDefines)
                .ParseScript(scrEntry.Name, true);

            if (result.ParseErrors.Count == 0)
            {
                File.WriteAllBytes(cachedScriptFile, result.Script);
                preProcessedFiles?.TryAdd(cachedScriptFile, result.Script);
            }

            return result;
        }

        private static void ProcessScripts(NPCFile data, NPCEntry entry, string jsonEntryPrefix, string baseDefines,
                                           CacheStatus cacheStatus, bool extDataExists, HashSet<string> scriptCacheFiles, ConcurrentDictionary<string, object> results,
                                           ref RecompilationStatus cs)
        {
            int scriptNum = 0;

            foreach (var scrEntry in entry.Scripts)
            {
                if (string.IsNullOrEmpty(scrEntry.Text))
                {
                    scriptNum++;
                    continue;
                }

                string scriptPrefix = jsonEntryPrefix + "script" + scriptNum + "_";
                string cachedScriptFile = Path.Combine(Program.ScriptCachePath,
                    scriptPrefix + Helpers.GetBase64Hash(scrEntry.Text));

                var result = ResolveScript(data, entry, scrEntry, scriptPrefix, cachedScriptFile,
                                           baseDefines, cacheStatus, extDataExists, null, scriptCacheFiles, ref cs);

                if (result.ParseErrors.Count == 0)
                    results[cachedScriptFile] = result.Script;

                scriptNum++;
            }
        }

        private static bool CheckExtDataCache(NPCEntry entry, string jsonEntryPrefix, HashSet<string> scriptCacheFiles, out string extDataFile)
        {
            string hash = Helpers.GetBase64Hash(BuildExtDataString(entry));
            extDataFile = Path.Combine(Program.ScriptCachePath, jsonEntryPrefix + "exdata_" + hash);
            return scriptCacheFiles.Contains(extDataFile);
        }

        // ── Output ────────────────────────────────────────────────────────────────

        private static void WriteOutput(
            string outPath, string outputDepsPath, NPCFile data,
            List<CompilationEntryData> compilationData,
            IProgress<ProgressReport> progress, bool cliMode, ref int offset)
        {
            var output = new List<byte>();
            output.AddRangeBigEndian((uint)data.Entries.Count);

            if (Program.Settings.CompressIndividually)
            {
                string msgCompressing = "Compressing...";
                Program.ConsoleWriteLineS(msgCompressing);
                progress?.Report(new ProgressReport(msgCompressing, 100));
            }

            Parallel.ForEach(compilationData, entry =>
            {
                if (entry.data == null) return;

                if (!Program.Settings.CompressIndividually)
                {
                    entry.compressedSize = 0;
                    entry.decompressedSize = entry.data.Count;
                    return;
                }

                var compressed = RLibrii.Szs.Yaz0.CompressYaz(entry.data.ToArray(), 9).ToList();
                Helpers.Ensure4ByteAlign(compressed);

                if (compressed.Count >= entry.data.Count)
                {
                    entry.compressedSize = 0;
                    entry.decompressedSize = entry.data.Count;
                }
                else
                {
                    entry.compressedSize = compressed.Count;
                    entry.decompressedSize = entry.data.Count;
                    entry.data = compressed;
                }
            });

            var entryAddresses = new List<byte>();
            foreach (var entry in compilationData)
            {
                entryAddresses.AddRangeBigEndian(offset);
                entryAddresses.AddRangeBigEndian(entry.compressedSize);
                entryAddresses.AddRangeBigEndian(entry.decompressedSize);
                offset += entry.compressedSize != 0 ? entry.compressedSize : entry.decompressedSize;
            }

            output.AddRange(entryAddresses);

            foreach (var entry in compilationData)
                if (entry.data != null)
                    output.AddRange(entry.data);

            if (!string.IsNullOrEmpty(outputDepsPath))
                File.WriteAllText(outputDepsPath, CreateDepsFile(data, outPath));

            Program.ConsoleWriteLineS("\nDone!");
            progress?.Report(new ProgressReport("Done!", 100));
            File.WriteAllBytes(outPath, output.ToArray());
        }

        public static string CreateDepsFile(NPCFile data, string zobjFilename)
        {
            var deps = new List<string>();

            Action<string> addDep = p =>
            {
                string escaped = p.Replace(" ", "\\ ");
                if (!deps.Contains(escaped))
                    deps.Add(escaped);
            };

            addDep(Helpers.MakePathRelativeToProjectPath(Program.JsonPath));

            foreach (var entry in data.Entries)
            {
                foreach (var header in entry.EmbeddedOverlayCode.HeaderPaths)
                    addDep(Helpers.DenormalizeExtPath(header, true));

                foreach (var p in Helpers.ResolveSemicolonPaths(entry.HeaderPath, true))
                    addDep(p);

                foreach (var p in Helpers.ResolveSemicolonPaths(data.ExtScriptHeaderPath, true))
                    addDep(p);
            }

            string escapedTarget = Helpers.MakePathRelativeToProjectPath(zobjFilename).Replace(" ", "\\ ");

            var sb = new StringBuilder();
            sb.Append(escapedTarget);
            sb.Append(":");

            foreach (var dep in deps)
            {
                sb.Append(" \\\n  ");
                sb.Append(dep);
            }

            sb.AppendLine();
            return sb.ToString();
        }

        // ── Normalization & Header Processing ─────────────────────────────────────

        private static void ProcessCHeader(ref NPCFile npcFile)
        {
            npcFile.CHeader = string.Join(Environment.NewLine, npcFile.CHeaderLines.Select(x => x.TrimEnd()));
        }

        private static void NormalizeLineBreaks(ref NPCFile npcFile)
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
            {
                message.MessageText = lineBreakRegex.Replace(message.MessageText, Environment.NewLine);

                if (message.Comment != null)
                    message.Comment = lineBreakRegex.Replace(message.Comment, Environment.NewLine);
            }
        }

        private static void ResolveHeaderDefines(ref NPCFile npcFile)
        {
            foreach (var entry in npcFile.Entries.Where(e => !string.IsNullOrEmpty(e.HeaderPath)))
            {
                var defines = Helpers.GetDefinesFromHeaders(entry.HeaderPath);

                entry.Hierarchy = ResolveHeaderDefineForField(entry.SkeletonHeaderDefinition, defines, entry.Hierarchy);
                entry.FileStart = (int)ResolveHeaderDefineForField(entry.FileStartHeaderDefinition, defines, (uint)entry.FileStart);

                foreach (var anim in entry.Animations)
                {
                    var parts = Helpers.SplitHeaderDefsString(anim.HeaderDefinition);
                    anim.Address = ResolveHeaderDefineForField(parts[1], defines, anim.Address);
                    anim.FileStart = (int)ResolveHeaderDefineForField(parts[0], defines, (uint)anim.FileStart);
                }

                foreach (var dlist in entry.ExtraDisplayLists)
                {
                    var parts = Helpers.SplitHeaderDefsString(dlist.HeaderDefinition);
                    dlist.Address = ResolveHeaderDefineForField(parts[1], defines, dlist.Address);
                    dlist.FileStart = (int)ResolveHeaderDefineForField(parts[0], defines, (uint)dlist.FileStart);
                }

                foreach (var segment in entry.Segments)
                {
                    foreach (var segEntry in segment)
                    {
                        var parts = Helpers.SplitHeaderDefsString(segEntry.HeaderDefinition);
                        segEntry.Address = ResolveHeaderDefineForField(parts[1], defines, segEntry.Address);
                        segEntry.FileStart = (int)ResolveHeaderDefineForField(parts[0], defines, (uint)segEntry.FileStart);
                    }
                }
            }
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private static void ShowMsg(bool cliMode, string msg)
        {
            Program.CompileThereWereErrors = true;

            if (Program.IsRunningUnderMono || cliMode)
                Console.WriteLine(msg);

            // Occasionally crashed showing messagebox on another thread.
            if (Program.IsRunningUnderMono)
                Program.CompileMonoErrors = msg;
        }

        private static uint TryGetFromH(bool cliMode, string npcName, uint defaultValue, Dictionary<string, string> defines, string name, ConcurrentBag<string> parseErrors)
        {
            if (string.IsNullOrWhiteSpace(name))
                return defaultValue;

            string errorMsg = $"{npcName}: Warning: Could not find define {name}!";

            if (defines.Count == 0)
            {
                parseErrors.Add(errorMsg);
                return defaultValue;
            }

            try
            {
                var h = Helpers.GetHDefineFromName(name, defines);

                if (h == null)
                {
                    parseErrors.Add(errorMsg);
                    return defaultValue;
                }

                return h.Value1 != null ? (uint)h.Value1 : defaultValue;
            }
            catch (Exception ex)
            {
                parseErrors.Add($"{npcName}: Error parsing define \"{name}\": {ex.Message}");
                return defaultValue;
            }
        }

        private static List<LocalizationEntry> BuildLocaleList(NPCEntry entry, NPCFile data)
        {
            var locales = new List<LocalizationEntry>
            {
                new LocalizationEntry { Language = Lists.DefaultLanguage, Messages = entry.Messages }
            };

            locales.AddRange(data.Languages.Select(lang =>
            {
                var loc = entry.Localization.FirstOrDefault(x => x.Language == lang);
                return new LocalizationEntry { Language = lang, Messages = loc?.Messages };
            }));

            return locales;
        }

        private static string BuildExtDataString(NPCEntry entry)
        {
            return JsonConvert.SerializeObject(new
            {
                entry.Messages,
                entry.ExtraDisplayLists,
                entry.Segments,
                entry.Animations
            }) + Helpers.GetDefinesStringFromH(entry.HeaderPath);
        }

        private static void FlattenMessage(MessageEntry message, string envNewline)
        {
            message.MessageText = message.MessageText?.Replace(envNewline, "\n");
            message.MessageTextLines = message.MessageText?.Split(Lists.NewlineSeparators, StringSplitOptions.None).ToList();
            message.MessageText = null;
            message.Comment = message.Comment?.Replace(envNewline, "\n");
        }
    }
}