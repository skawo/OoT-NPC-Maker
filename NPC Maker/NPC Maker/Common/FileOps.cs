using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPC_Maker.Common;
using NPC_Maker.Controls;
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
        private static void EnsureSettingsSanity(NPCMakerSettings set)
        {
            if (set == null)
                return;

            if (set.GUIScale <= 0 || set.GUIScale > 3)
                set.GUIScale = 1;

            if (set.MessageEditorFontSize <= 0)
                set.MessageEditorFontSize = 8;

            if (set.AutoSaveTime < 100)
                set.AutoSaveTime = 100;

            if (set.ParseTime < 100)
                set.ParseTime = 100;

            if (set.CompileTimeout < 500)
                set.CompileTimeout = 500;
        }

        public static NPCMakerSettings ParseSettingsJSON(string FileName)
        {
            try
            {
                if (!File.Exists(FileName))
                    return new NPCMakerSettings();

                string Text = File.ReadAllText(FileName);
                NPCMakerSettings Deserialized = JsonConvert.DeserializeObject<NPCMakerSettings>(Text);

                EnsureSettingsSanity(Deserialized);

                return Deserialized;
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to read settings: {ex.Message}");
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
                BigMessageBox.Show($"Failed to write settings: {ex.Message}");
            }
        }

        public static NPCFile ParseNPCJsonFile(string fileName, string jsonText = "")
        {
            try
            {
                if (String.IsNullOrWhiteSpace(jsonText))
                    jsonText = File.ReadAllText(fileName);

                NPCFile npcFile = JsonConvert.DeserializeObject<NPCFile>(jsonText);
                int currentVersion = npcFile.Version > 0 ? npcFile.Version : 1;

                currentVersion = MigrateToVersion2(ref npcFile, currentVersion > 1 ? null : JObject.Parse(jsonText), currentVersion);
                currentVersion = MigrateToVersion3(ref npcFile, currentVersion);

                if (currentVersion > 3)
                    ProcessTextLinesForVersion4Plus(ref npcFile);
                if (currentVersion > 4)
                    ProcessMessageLinesForVersion5Plus(ref npcFile);

                currentVersion = MigrateToVersion6(ref npcFile, currentVersion);
                currentVersion = MigrateToVersion7(ref npcFile, currentVersion);

                if (currentVersion >= 6)
                    ProcessCHeader(ref npcFile);

                NormalizeLineBreaks(ref npcFile);
                ResolveHeaderDefines(ref npcFile);
                npcFile.Version = 7;
                return npcFile;
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to read JSON: {ex.Message}");
                return null;
            }
        }

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

        public static uint ResolveHeaderDefineOrFail(string npcName, string name, Dictionary<string, string> defines, uint field, StringBuilder errors)
        {
            if (String.IsNullOrEmpty(name))
                return field;

            try
            {
                Common.HDefine h = Helpers.GetHDefineFromName(name, defines);
                if (h != null && h.Value1 != null)
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

        public static void SaveNPCJSON(string Path, NPCFile Data, IProgress<Common.ProgressReport> progress = null, string json = null)
        {
            try
            {
                if (json == null)
                    json = ProcessNPCJSON(ref Data, progress);

                if (json != null)
                    File.WriteAllText(Path, json);
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to save JSON: {ex.Message}");
            }
        }

        public static string ProcessNPCJSON(ref NPCFile Data, IProgress<Common.ProgressReport> progress = null)
        {
            try
            {
                NPCFile outD = Helpers.Clone<NPCFile>(Data);

                string[] newlineSeparators = { "\r\n", "\n" };
                string envNewline = Environment.NewLine;

                float progressPer = 100f / outD.Entries.Count;
                int processedCount = 0;

                Parallel.ForEach(outD.Entries, entry =>
                {
                    foreach (var script in entry.Scripts)
                    {
                        script.TextLines = SplitLines(script.Text);
                        script.Text = null;
                    }

                    foreach (var message in entry.Messages)
                        ProcessMessage(message, envNewline, newlineSeparators);

                    foreach (var loc in entry.Localization)
                        foreach (var message in loc.Messages)
                            ProcessMessage(message, envNewline, newlineSeparators);

                    if (entry.EmbeddedOverlayCode?.Code != null)
                    {
                        entry.EmbeddedOverlayCode.CodeLines = SplitLines(entry.EmbeddedOverlayCode.Code);
                        entry.EmbeddedOverlayCode.Code = null;
                    }

                    if (progress != null)
                    {
                        int current = Interlocked.Increment(ref processedCount);
                        if (current % 10 == 0 || current == outD.Entries.Count)
                        {
                            float pct = Math.Min(current * progressPer, 100f);
                            progress.Report(new Common.ProgressReport($"Saving {pct:0}%", pct));
                        }
                    }
                });

                foreach (var script in outD.GlobalHeaders)
                {
                    script.TextLines = SplitLines(script.Text);
                    script.Text = null;
                }

                outD.CHeaderLines = SplitLines(outD.CHeader);
                outD.CHeader = null;

                string json = JsonConvert.SerializeObject(outD, new JsonSerializerSettings
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
                BigMessageBox.Show($"Failed to process JSON: {ex.Message}");
                return null;
            }

            List<string> SplitLines(string text) =>
                text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                     .Select(x => x.TrimEnd())
                     .ToList();

            void ProcessMessage(MessageEntry message, string newline, string[] separators)
            {
                message.MessageText = message.MessageText?.Replace(newline, "\n");
                message.MessageTextLines = message.MessageText?.Split(separators, StringSplitOptions.None).ToList();
                message.MessageText = null;
                message.Comment = message.Comment?.Replace(newline, "\n");
            }
        }

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
                    BigMessageBox.Show($"{filename} is missing or incorrect. ({offendingRow})");
            }

            return dict;
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
                    BigMessageBox.Show($"{Filename} is missing or incorrect. ({OffendingRow})");
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
                BigMessageBox.Show(Msg, "Error", MessageBoxButtons.OK);

            return;
        }

        public static Common.CacheStatus GetCacheStatus(ref NPCFile data)
        {
            string jsonFileName = Program.JsonPath.FilenameFromPath();
            string extHeaderPath = "";

            try { extHeaderPath = data.GetExtHeader(); } catch { }

            // Global headers + ext header
            var ghBuilder = new StringBuilder();
            foreach (var h in data.GlobalHeaders)
                ghBuilder.AppendLine(h.Text);
            ghBuilder.Append(extHeaderPath);

            string dicts = JsonConvert.SerializeObject(new
            {
                Dicts.Actors,
                Dicts.ObjectIDs,
                Dicts.SFXes,
                Dicts.LinkAnims,
                Dicts.Music
            });

            // C header + linker contents
            data.CHeader = CCode.ReplaceGameVersionInclude(data.CHeader);
            var cHeaderBuilder = new StringBuilder(data.CHeader);

            foreach (var linkerPath in Helpers.ResolveSemicolonPaths(Program.Settings.LinkerPaths))
            {
                if (File.Exists(linkerPath))
                    cHeaderBuilder.Append(File.ReadAllText(linkerPath));
            }

            string cSettings = JsonConvert.SerializeObject(new
            {
                Program.Settings.Linker,
                Program.Settings.LinkerPaths,
                Program.Settings.Library,
                Program.Settings.GCCFlags,
                Program.Settings.IncludePaths
            });

            cHeaderBuilder.Append(cSettings);

            string ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            string hashGH = Helpers.GetBase64Hash(ghBuilder.ToString());
            string hashDicts = Helpers.GetBase64Hash(dicts);
            string hashCHeader = Helpers.GetBase64Hash(cHeaderBuilder.ToString());

            string cachedHeaders = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_gh_{ver}_{hashGH}");
            string cachedDicts = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_dicts_{ver}_{hashDicts}");
            string cachedCHeader = Path.Combine(Program.CCachePath, $"{jsonFileName}_ch_{ver}_{hashCHeader}");

            var ret = new Common.CacheStatus() { CacheInvalid = false, CCacheInvalid = false };

            if (!File.Exists(cachedHeaders) || !File.Exists(cachedDicts))
            {
                ret.CacheInvalid = true;
                Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{jsonFileName}_");
                File.WriteAllText(cachedHeaders, "");
                File.WriteAllText(cachedDicts, "");
            }

            if (!File.Exists(cachedCHeader))
            {
                ret.CCacheInvalid = true;
                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{jsonFileName}_");
                File.WriteAllText(cachedCHeader, "");
            }

            return ret;
        }


        // Load all the includes text so that it can be appended to the string being hashed for cache
        private static string GetIncludesText(List<string> includeList)
        {
            var codeBuilder = new StringBuilder();
            var alreadyIncluded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string hPath in includeList)
            {
                string cleanPath = Helpers.DenormalizeExtPath(hPath);

                // Try trimmed path as fallback
                if (!File.Exists(cleanPath))
                    cleanPath = cleanPath.TrimStart('/', '\\');

                if (!alreadyIncluded.Add(cleanPath))
                    continue;

                if (File.Exists(cleanPath))
                {
                    codeBuilder.Append(File.GetLastWriteTime(cleanPath).Ticks);
                }
                else
                {
                    Program.ConsoleWriteLineS($"Warning: Couldn't resolve path {hPath} for cache use.");
                    codeBuilder.Append(Helpers.GenerateTemporaryFolderName());
                }
            }

            return codeBuilder.ToString();
        }

        public static async Task PreprocessCodeAndScripts(string outPath, string outputDepsPath, NPCFile Data, Common.CacheStatus cacheStatus, IProgress<Common.ProgressReport> progress, bool CLIMode)
        {
            float progressPer = 100f / Data.Entries.Count;
            float curProgress = 0f;
            int lastReported = 0;

            await TaskEx.Run(() =>
            {
                Program.ConsoleWriteS("Compiling...");

                string baseDefines = Scripts.ScriptHelpers.GetBaseDefines(Data);
                string jsonFileName = Program.JsonPath.FilenameFromPath();

                var scriptCacheFiles = new HashSet<string>(Directory.GetFiles(Program.ScriptCachePath));
                var cCacheFiles = new HashSet<string>(Directory.GetFiles(Program.CCachePath));

                var results = new ConcurrentDictionary<string, object>();

                Parallel.For(0, Data.Entries.Count,
                    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 3 },
                    entryID =>
                    {
                        try
                        {
                            NPCEntry entry = Data.Entries[entryID];

                            if (!entry.IsNull && !entry.Omitted)
                            {
                                string jsonEntryPrefix = jsonFileName + "_" + entryID + "_";

                                CompilationStatus cs = new CompilationStatus();

                                ProcessCCode(Data, entry, entryID, jsonEntryPrefix, cacheStatus.CCacheInvalid, cCacheFiles, scriptCacheFiles, results, ref cs, out string compErrors);
                                bool extDataExists = CheckExtDataCache(entry, jsonEntryPrefix, scriptCacheFiles, out string extDataFile);
                                ProcessScripts(Data, entry, jsonEntryPrefix, baseDefines, cacheStatus.CacheInvalid, extDataExists, scriptCacheFiles, results, ref cs);

                                results[entryID.ToString()] = cs;

                                if (!extDataExists)
                                {
                                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, jsonEntryPrefix + "exdata_");
                                    File.Create(extDataFile).Dispose();
                                }
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
                                    progress.Report(new Common.ProgressReport($"Compiling {percent}%", curProgress));
                                else
                                    Program.ConsoleWriteS($"\rCompiling {percent}%    ");
                            }
                        }
                    });

                Program.ConsoleWriteLineS("\nPre-processing done!");
                SaveBinaryFile(outPath, outputDepsPath, ref Data, progress, baseDefines, new CacheStatus() { CCacheInvalid = false, CacheInvalid = false }, results, CLIMode);

                CCode.CleanupStandardCompilationArtifacts();
                Program.CompileInProgress = false;
            });
        }

        private static void ProcessCCode(NPCFile data, NPCEntry entry, int entryID, string jsonEntryPrefix,
                                         bool cCacheInvalid, HashSet<string> cCacheFiles, HashSet<string> scriptCacheFiles,
                                         ConcurrentDictionary<string, object> results, ref CompilationStatus cs, out string compErrors)
        {
            compErrors = "";

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
                cCacheFiles.Contains(cachedAddrsFile) &&
                cCacheFiles.Contains(cachedCodeFile);

            byte[] overlay;

            if (!cCacheHit)
            {
                cs.CCode = true;

                Helpers.DeleteFileStartingWith(Program.CCachePath, funcsAddrsName, cCacheFiles);
                Helpers.DeleteFileStartingWith(Program.CCachePath, codeName, cCacheFiles);

                overlay = null;

                if (!string.IsNullOrEmpty(entry.EmbeddedOverlayCode.Code))
                    overlay = CCode.Compile(data.CHeader, Program.Settings.LinkerPaths, entry.EmbeddedOverlayCode, ref compErrors, out _, "NPCCOMPILE" + entryID);

                if (overlay != null)
                {
                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, jsonEntryPrefix + "script", scriptCacheFiles);

                    string addrJson = JsonConvert.SerializeObject(
                        entry.EmbeddedOverlayCode,
                        new JsonSerializerSettings { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() }
                    );

                    File.WriteAllText(cachedAddrsFile, addrJson);
                    File.WriteAllBytes(cachedCodeFile, overlay);
                }
            }
            else
            {
                entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(
                    File.ReadAllText(cachedAddrsFile),
                    new JsonSerializerSettings { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() }
                );

                overlay = File.ReadAllBytes(cachedCodeFile);
            }

            if (overlay != null)
            {
                results[cachedAddrsFile] = entry.EmbeddedOverlayCode;
                results[cachedCodeFile] = overlay;
            }
        }

        private static bool CheckExtDataCache(NPCEntry entry, string jsonEntryPrefix, HashSet<string> scriptCacheFiles, out string extDataFile)
        {
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
            extDataFile = Path.Combine(Program.ScriptCachePath, jsonEntryPrefix + "exdata_" + extDataHash);

            return scriptCacheFiles.Contains(extDataFile);
        }

        private static void ProcessScripts(NPCFile data, NPCEntry entry, string jsonEntryPrefix, string baseDefines,
                                           bool cacheInvalid, bool extDataExists, HashSet<string> scriptCacheFiles,
                                           ConcurrentDictionary<string, object> results, ref CompilationStatus cs)
        {
            int scriptNum = 0;

            foreach (var scrEntry in entry.Scripts)
            {
                if (string.IsNullOrEmpty(scrEntry.Text))
                {
                    scriptNum++;
                    continue;
                }

                string scriptHash = Helpers.GetBase64Hash(scrEntry.Text);
                string cachedScriptFile = Path.Combine(Program.ScriptCachePath, jsonEntryPrefix + "script" + scriptNum + "_" + scriptHash);

                bool scriptCacheHit = !cacheInvalid && extDataExists && scriptCacheFiles.Contains(cachedScriptFile);

                if (!scriptCacheHit)
                {
                    cs.Scripts = true;

                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, jsonEntryPrefix + "script" + scriptNum + "_", scriptCacheFiles);

                    var parser = new Scripts.ScriptParser(ref data, entry, scrEntry.Text, baseDefines);
                    var script = parser.ParseScript(scrEntry.Name, true);

                    if (script.ParseErrors.Count == 0)
                    {
                        File.WriteAllBytes(cachedScriptFile, script.Script);
                        results[cachedScriptFile] = script.Script;
                    }
                }
                else
                {
                    results[cachedScriptFile] = File.ReadAllBytes(cachedScriptFile);
                }

                scriptNum++;
            }
        }


        private static uint TryGetFromH(bool CLIMode, string NPCName, uint defaultV, Dictionary<string, string> defines, string name)
        {
            string Error = $"{NPCName}: Warning: Could not find define {name}!";

            if (defines.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(name))
                    FileOps.ShowMsg(CLIMode, Error);
    
                return defaultV;
            }

            try
            {
                var h = Helpers.GetHDefineFromName(name, defines);

                if (h == null)
                {
                    if (!string.IsNullOrWhiteSpace(name))
                        FileOps.ShowMsg(CLIMode, Error);

                    return defaultV;
                }

                return h.Value1 != null ? (uint)h.Value1 : defaultV;
            }
            catch (Exception ex)
            {
                FileOps.ShowMsg(CLIMode, $"{NPCName}: Error parsing define \"{name}\": {ex.Message}");
                return defaultV;
            }
        }

        public static void SaveBinaryFile(string outPath, string outputDepsPath, ref NPCFile Data, IProgress<Common.ProgressReport> progress,
                                          string baseDefines, Common.CacheStatus cacheStatus, ConcurrentDictionary<string, object> preProcessedFiles, bool CLIMode)
        {
            if (!Data.Entries.Any())
            {
                ShowMsg(CLIMode, "Nothing to save.");
                Program.CompileThereWereErrors = false;
                return;
            }

            try
            {
                int offset = Data.Entries.Count * 12 + 4;
                string jsonFileName = Program.JsonPath.FilenameFromPath();

                var parseErrors = new ConcurrentBag<string>();
                var compilationData = new Common.CompilationEntryData[Data.Entries.Count];
                var definesCache = new ConcurrentDictionary<string, Dictionary<string, string>>();

                float progressPer = 100f / Data.Entries.Count;
                float curProgress = 0f;
                int entriesDone = 0;

                progress?.Report(new Common.ProgressReport("Saving...", 0));

                var cts = new CancellationTokenSource();
                var localData = Data;

                if (!Program.Settings.CompileInParallel || (CLIMode && !Program.consoleSilent))
                {
                    for (int i = 0; i < localData.Entries.Count; i++)
                    {
                        ProcessEntry(i);
                    }
                }
                else
                {
                    try
                    {
                        Parallel.For(0, localData.Entries.Count,
                            new ParallelOptions { CancellationToken = cts.Token },
                            i => ProcessEntry(i));
                    }
                    catch (OperationCanceledException) 
                    { 
                    }
                }

                void ProcessEntry(int i)
                {
                    if (cts.IsCancellationRequested) return;

                    NPCEntry entry = localData.Entries[i];

                    if (entry.IsNull || entry.Omitted)
                    {
                        compilationData[i] = new Common.CompilationEntryData(null);
                        Program.ConsoleWriteLineS($"Entry {i} is blank or omitted.");
                    }
                    else
                    {
                        Program.ConsoleWriteS($"Processing entry {i}: {entry.NPCName}... ");

                        string compErrors = "";

                        CompilationStatus cs = new CompilationStatus();
                        object csi = cs;

                        if (preProcessedFiles?.TryGetValue(i.ToString(), out csi) == true)
                            cs = (CompilationStatus)csi;

                        var entryBytes = BuildEntryBytes(entry, localData, CLIMode, baseDefines,
                                                         cacheStatus, jsonFileName, i, preProcessedFiles, 
                                                         parseErrors, definesCache, ref cs, ref compErrors);

                        if (parseErrors.IsEmpty)
                        {
                            compilationData[i] = new Common.CompilationEntryData(entryBytes);
                            Program.ConsoleWriteS($"OK {cs.ToString()}{Environment.NewLine}");
                        }
                        else
                        {
                            cts.Cancel();
                            return;
                        }
                    }

                    Helpers.AddInterlocked(ref curProgress, progressPer);
                    Interlocked.Increment(ref entriesDone);
                    progress?.Report(new Common.ProgressReport($"Saving {entriesDone}/{localData.Entries.Count}", curProgress));
                }

                Data = localData;

                if (!parseErrors.IsEmpty)
                {
                    ShowMsg(CLIMode,
                        $"File could not be saved.{Environment.NewLine}{Environment.NewLine}" +
                        $"There are errors in NPC: {string.Join(",", parseErrors.Distinct())}");

                    progress?.Report(new Common.ProgressReport("Done!", 100));
                    return;
                }

                WriteOutput(outPath, outputDepsPath, Data, compilationData.ToList(), progress, CLIMode, ref offset);
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

        private static List<byte> BuildEntryBytes(NPCEntry entry, NPCFile data, bool CLIMode, string baseDefines,
                                                  CacheStatus cacheStatus, string jsonFileName, int entriesDone,
                                                  ConcurrentDictionary<string, object> preProcessedFiles, ConcurrentBag<string> parseErrors,
                                                  ConcurrentDictionary<string, Dictionary<string, string>> definesCache, ref CompilationStatus cs, ref string compErrors)
        {
            var defines = definesCache.GetOrAdd(entry.HeaderPath, Helpers.GetDefinesFromHeaders);

            var entryBytes = new List<byte>(56);
            int curLen = 0;

            BuildFixedFields(entryBytes, entry, data, defines, CLIMode, ref curLen);
            BuildMessages(entryBytes, entry, data, jsonFileName, entriesDone, parseErrors, CLIMode, ref cs, ref curLen);
            BuildAnimations(entryBytes, entry, defines, CLIMode, ref curLen);
            BuildExtraDisplayLists(entryBytes, entry, defines, CLIMode, ref curLen);
            BuildColors(entryBytes, entry, ref curLen);
            BuildSegments(entryBytes, entry, defines, CLIMode, ref curLen);
            BuildCCode(entryBytes, entry, data, jsonFileName, entriesDone, cacheStatus.CCacheInvalid,
                       preProcessedFiles, parseErrors, CLIMode, ref cs, ref compErrors, ref curLen);
            BuildScripts(entryBytes, entry, data, jsonFileName, entriesDone, baseDefines,
                         cacheStatus.CacheInvalid, preProcessedFiles, parseErrors, CLIMode, ref cs, ref curLen);

            return entryBytes;
        }

        private static void BuildFixedFields(List<byte> entryBytes, NPCEntry entry, NPCFile data, Dictionary<string, string> defines, bool CLIMode, ref int curLen)
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
            curLen += 56;
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
            entryBytes.AddRangeBigEndian(TryGetFromH(CLIMode, entry.NPCName, entry.Hierarchy, defines, entry.SkeletonHeaderDefinition));
            entryBytes.AddRangeBigEndian(TryGetFromH(CLIMode, entry.NPCName, (uint)entry.FileStart, defines, entry.FileStartHeaderDefinition));
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
                                          ConcurrentBag<string> parseErrors, bool CLIMode, ref CompilationStatus cs, ref int curLen)
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

            var header = new List<byte>();
            var defaultHeader = new List<byte>();
            var msgData = new List<byte>();

            var locales = new List<LocalizationEntry>
            { 
                new LocalizationEntry 
                { 
                    Language = Lists.DefaultLanguage, 
                    Messages = entry.Messages 
                } 
            };

            locales.AddRange(data.Languages.Select(lang =>
            {
                var loc = entry.Localization.FirstOrDefault(x => x.Language == lang);
                return new LocalizationEntry { Language = lang, Messages = loc?.Messages };
            }));

            int totalMessages = entry.Messages.Count * locales.Count;
            int msgOffset = 8 * totalMessages;

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
                    ShowMsg(CLIMode, errors.First());
                    if (!parseErrors.Contains(entry.NPCName)) parseErrors.Add(entry.NPCName);
                    break;
                }

                foreach (var msg in loc.Messages)
                {
                    var messageBytes = msg.TempBytes ?? new List<byte>();
                    msg.TempBytes = null;

                    Helpers.Ensure4ByteAlign(messageBytes);
                    msgData.AddRange(messageBytes);

                    if (messageBytes.Count > 1280)
                    {
                        ShowMsg(CLIMode, $"{entry.NPCName}: Message '{msg.Name}' exceeded 1280 bytes and could not be saved.");
                        if (!parseErrors.Contains(entry.NPCName)) parseErrors.Add(entry.NPCName);
                        messageBytes.Clear();
                    }

                    void AddToHeader(List<byte> target)
                    {
                        target.AddRangeBigEndian(msgOffset);
                        target.Add(msg.GetMessageTypePos());
                        Helpers.Ensure2ByteAlign(target);
                        target.AddRangeBigEndian((ushort)messageBytes.Count);
                    }

                    if (isDefault) AddToHeader(defaultHeader);
                    AddToHeader(header);

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

        private static void BuildAnimations(List<byte> entryBytes, NPCEntry entry, Dictionary<string, string> defines, bool CLIMode, ref int curLen)
        {
            entryBytes.AddRangeBigEndian((uint)entry.Animations.Count);
            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            foreach (AnimationEntry anim in entry.Animations)
            {
                var anm = Helpers.SplitHeaderDefsString(anim.HeaderDefinition);
                anim.Address = TryGetFromH(CLIMode, entry.NPCName, (uint)anim.Address, defines, anm[1]);
                anim.FileStart = (int)TryGetFromH(CLIMode, entry.NPCName, (uint)anim.FileStart, defines, anm[0]);
                entryBytes.AddRange(anim.ToBytes());
            }

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 16 * entry.Animations.Count;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildExtraDisplayLists(List<byte> entryBytes, NPCEntry entry, Dictionary<string, string> defines, bool CLIMode, ref int curLen)
        {
            entryBytes.AddRangeBigEndian((uint)entry.ExtraDisplayLists.Count);
            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            foreach (DListEntry dlist in entry.ExtraDisplayLists)
            {
                var dl = Helpers.SplitHeaderDefsString(dlist.HeaderDefinition);
                dlist.Address = TryGetFromH(CLIMode, entry.NPCName, dlist.Address, defines, dl[1]);
                dlist.FileStart = (int)TryGetFromH(CLIMode, entry.NPCName, (uint)dlist.FileStart, defines, dl[0]);
                entryBytes.AddRange(dlist.ToBytes());
            }

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 40 * entry.ExtraDisplayLists.Count;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildColors(List<byte> entryBytes, NPCEntry entry, ref int curLen)
        {
            var parsedColors = entry.ParseColorEntries().OrderBy(c => c.LimbID).ToList();

            entryBytes.AddRangeBigEndian((uint)parsedColors.Count);
            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            foreach (var color in parsedColors)
                entryBytes.AddRange(color.ToBytes());

            Helpers.Ensure4ByteAlign(entryBytes);
            curLen += 4 * parsedColors.Count;
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildSegments(List<byte> entryBytes, NPCEntry entry, Dictionary<string, string> defines, bool CLIMode, ref int curLen)
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
                    var segDefs = Helpers.SplitHeaderDefsString(segEntry.HeaderDefinition);
                    segEntry.Address = TryGetFromH(CLIMode, entry.NPCName, segEntry.Address, defines, segDefs[1]);
                    segEntry.FileStart = (int)TryGetFromH(CLIMode, entry.NPCName, (uint)segEntry.FileStart, defines, segDefs[0]);
                    segEntries.AddRange(segEntry.ToBytes());
                }
            }

            entryBytes.AddRangeBigEndian((uint)(segOffsets.Count + segEntries.Count));
            curLen += 4;
            entryBytes.AddRange(segOffsets);
            entryBytes.AddRange(segEntries);

            Helpers.Ensure4ByteAlign(entryBytes);
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void BuildCCode(List<byte> entryBytes, NPCEntry entry, NPCFile data, string jsonFileName, int entriesDone, bool cCacheInvalid,
                                       ConcurrentDictionary<string, object> preProcessedFiles, ConcurrentBag<string> parseErrors,
                                       bool CLIMode, ref CompilationStatus cs, ref string compErrors, ref int curLen)
        {
            if (string.IsNullOrEmpty(entry.EmbeddedOverlayCode.Code))
            {
                entryBytes.AddRangeBigEndian(-1);
                curLen += 4;
                return;
            }

            string codeString = JsonConvert.SerializeObject(entry.EmbeddedOverlayCode)
                              + CCode.ReplaceGameVersionInclude("")
                              + GetIncludesText(entry.EmbeddedOverlayCode.HeaderPaths);

            string hash = Helpers.GetBase64Hash(codeString);
            string cachedAddrs = Path.Combine(Program.CCachePath, $"{jsonFileName}_{entriesDone}_funcsaddrs_{hash}");
            string cachedCode = Path.Combine(Program.CCachePath, $"{jsonFileName}_{entriesDone}_code_{hash}");

            object addrsData = null;
            object codeData = null;

            preProcessedFiles?.TryGetValue(cachedAddrs, out addrsData);
            preProcessedFiles?.TryGetValue(cachedCode, out codeData);

            byte[] overlay;

            if (addrsData != null && codeData != null)
            {
                entry.EmbeddedOverlayCode = (CCodeEntry)addrsData;
                overlay = (byte[])codeData;
            }
            else if (!cCacheInvalid && File.Exists(cachedCode) && File.Exists(cachedAddrs))
            {
                entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(
                    File.ReadAllText(cachedAddrs),
                    new JsonSerializerSettings { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });
                overlay = File.ReadAllBytes(cachedCode);
            }
            else
            {
                cs.CCode = true;

                compErrors = "";
                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{jsonFileName}_{entriesDone}_funcsaddrs_");
                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{jsonFileName}_{entriesDone}_code_");
                Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{jsonFileName}_{entriesDone}_script");

                overlay = CCode.Compile(data.CHeader, Program.Settings.LinkerPaths, entry.EmbeddedOverlayCode, ref compErrors, out _, "NPCCOMPILE" + entriesDone);

                if (overlay != null)
                {
                    File.WriteAllText(cachedAddrs, JsonConvert.SerializeObject(entry.EmbeddedOverlayCode,
                        new JsonSerializerSettings { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() }));
                    File.WriteAllBytes(cachedCode, overlay);
                }
            }

            if (overlay == null)
            {
                if (!parseErrors.Contains(entry.NPCName)) parseErrors.Add(entry.NPCName);
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

            for (int i = 0; i < entry.EmbeddedOverlayCode.FuncsRunWhen.GetLength(0); i++)
            {
                string fname = entry.EmbeddedOverlayCode.SetFuncNames[i];
                int funcIdx = entry.EmbeddedOverlayCode.Functions.FindIndex(x => x.Symbol == fname);

                if (funcIdx == -1 && !string.IsNullOrEmpty(fname))
                {
                    ShowMsg(CLIMode, $"{entry.NPCName}: Function {fname} not found in the C Code!");
                    return;
                }

                uint funcAddr = funcIdx >= 0 ? entry.EmbeddedOverlayCode.Functions[funcIdx].Addr : uint.MaxValue;
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

        private static void BuildScripts(List<byte> entryBytes, NPCEntry entry, NPCFile data,
                                         string jsonFileName, int entriesDone, string baseDefines,
                                         bool cacheInvalid, ConcurrentDictionary<string, object> preProcessedFiles,
                                         ConcurrentBag<string> parseErrors, bool CLIMode, ref CompilationStatus cs, ref int curLen)
        {
            var nonEmptyScripts = entry.Scripts.FindAll(x => !string.IsNullOrEmpty(x.Text));
            entryBytes.AddRangeBigEndian((uint)nonEmptyScripts.Count);
            curLen += 4;
            Helpers.Ensure4ByteAlign(entryBytes);
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);

            string extData = JsonConvert.SerializeObject(new
            { entry.Messages, entry.ExtraDisplayLists, entry.Segments, entry.Animations })
                + Helpers.GetDefinesStringFromH(entry.HeaderPath);

            string extDataHash = Helpers.GetBase64Hash(extData);
            string cachedExtData = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_{entriesDone}_exdata_{extDataHash}");
            bool extDataExists = File.Exists(cachedExtData);

            var parsedScripts = new List<Scripts.BScript>();
            int scriptNum = 0;

            foreach (ScriptEntry scr in nonEmptyScripts)
            {
                string hash = Helpers.GetBase64Hash(scr.Text);
                string cachedFile = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_{entriesDone}_script{scriptNum}_{hash}");

                object cached = null;
                preProcessedFiles?.TryGetValue(cachedFile, out cached);

                if (cached != null)
                {
                    parsedScripts.Add(new Scripts.BScript { Script = (byte[])cached, ParseErrors = new List<Scripts.ParseException>() });
                }
                else if (!cacheInvalid && extDataExists && File.Exists(cachedFile))
                {
                    parsedScripts.Add(new Scripts.BScript { Script = File.ReadAllBytes(cachedFile), ParseErrors = new List<Scripts.ParseException>() });
                }
                else
                {
                    cs.Scripts = true;

                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{jsonFileName}_{entriesDone}_script{scriptNum}_");
                    var parsed = new Scripts.ScriptParser(ref data, entry, scr.Text, baseDefines).ParseScript(scr.Name, true);
                    parsedScripts.Add(parsed);

                    if (parsed.ParseErrors.Count == 0)
                        File.WriteAllBytes(cachedFile, parsed.Script);
                }

                scriptNum++;
            }

            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, $"{jsonFileName}_{entriesDone}_exdata_");
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
                    if (!parseErrors.Contains(entry.NPCName)) parseErrors.Add(entry.NPCName);
                    Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Script \"{scr.Name}\" had errors:{Environment.NewLine}");
                    Console.WriteLine(string.Join(Environment.NewLine, scr.ParseErrors));
                }
            }

            Helpers.Ensure4ByteAlign(entryBytes);
            Helpers.ErrorIfExpectedLenWrong(entryBytes, curLen);
        }

        private static void WriteOutput(string outPath, string outputDepsPath, NPCFile data, List<Common.CompilationEntryData> compilationData, 
                                        IProgress<Common.ProgressReport> progress, bool CLIMode, ref int offset)
        {
            var output = new List<byte>();
            output.AddRangeBigEndian((uint)data.Entries.Count);

            if (Program.Settings.CompressIndividually)
            {
                Program.ConsoleWriteLineS("Compressing...");
                progress?.Report(new Common.ProgressReport("Compressing...", 100));
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
            
            if (!String.IsNullOrEmpty(outputDepsPath))
                File.WriteAllText(outputDepsPath, CreateDepsFile(data, outPath));

            Program.ConsoleWriteLineS("\nDone!");
            progress?.Report(new Common.ProgressReport("Done!", 100));

            File.WriteAllBytes(outPath, output.ToArray());
        }
        public static string CreateDepsFile(NPCFile data, string zobjFilename)
        {
            List<string> deps = new List<string>();
            Action<string> addDep = (p) => {
                string escaped = p.Replace(" ", "\\ ");
                if (!deps.Contains(escaped))
                    deps.Add(escaped);
            };

            string jsonFile = Helpers.MakePathRelativeToProjectPath(Program.JsonPath);
            addDep(jsonFile);

            foreach (var entry in data.Entries)
            {
                foreach (var header in entry.EmbeddedOverlayCode.HeaderPaths)
                {
                    addDep(Helpers.DenormalizeExtPath(header, true));
                }
                foreach (var p in Helpers.ResolveSemicolonPaths(entry.HeaderPath, true))
                {
                    addDep(p);
                }
                foreach (var p in Helpers.ResolveSemicolonPaths(data.ExtScriptHeaderPath, true))
                {
                    addDep(p);
                }
            }

            zobjFilename = Helpers.MakePathRelativeToProjectPath(zobjFilename);
            string escapedTarget = zobjFilename.Replace(" ", "\\ ");

            StringBuilder sb = new StringBuilder();
            sb.Append(escapedTarget);
            sb.Append(":");
            for (int i = 0; i < deps.Count; i++)
            {
                sb.Append(" \\\n  ");
                sb.Append(deps[i]);
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}