using Newtonsoft.Json;
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
        private const int InitialFieldsSize = 56;
        private sealed class BuildContext
        {
            public NPCFile Data;
            public CacheStatus CacheStatus;
            public string BaseDefines;
            public string JsonFileName;

            public ConcurrentDictionary<string, object> PreProcessed;
            public ConcurrentDictionary<string, object> Results;
            public HashSet<string> ScriptCacheFiles;
            public HashSet<string> CCacheFiles;

            public readonly ConcurrentBag<string> ParseErrors = new ConcurrentBag<string>();
            public readonly ConcurrentDictionary<string, Dictionary<string, string>> DefinesCache =
                new ConcurrentDictionary<string, Dictionary<string, string>>();
        }

        private sealed class EntryContext
        {
            public readonly BuildContext Build;
            public readonly NPCEntry Entry;
            public readonly int Index;
            public readonly string Prefix;
            public RecompilationStatus Status = new RecompilationStatus();
            public string CompErrors = "";
            public bool ExtDataExists;
            public readonly List<byte> Bytes = new List<byte>(InitialFieldsSize);
            public int CurLen;

            private Dictionary<string, string> _defines;

            public EntryContext(BuildContext build, int index)
            {
                Build = build;
                Index = index;
                Entry = build.Data.Entries[index];
                Prefix = build.JsonFileName + "_" + index + "_";
            }

            public NPCFile Data => Build.Data;

            public Dictionary<string, string> Defines =>
                _defines ?? (_defines = Build.DefinesCache.GetOrAdd(Entry.HeaderPath, Helpers.GetDefinesFromHeaders));

            public void AddParseError()
            {
                if (!Build.ParseErrors.Contains(Entry.NPCName))
                    Build.ParseErrors.Add(Entry.NPCName);
            }

            public uint Resolve(uint defaultValue, string defineName) =>
                TryGetFromH(Entry.NPCName, defaultValue, Defines, defineName, Build.ParseErrors);

            public void CheckLen() => Helpers.ErrorIfExpectedLenWrong(Bytes, CurLen);

            public void AlignAndCheck(int addedLen)
            {
                Helpers.Ensure4ByteAlign(Bytes);
                CurLen += addedLen;
                CheckLen();
            }
        }

        private sealed class OutputTargets
        {
            public string Bin;
            public string Deps;
            public string H;
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
            string linkerHash = GetLinkerHash(data);

            string ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            string cachedHeaders = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_gh_{ver}_{globalHeadersHash}");
            string cachedDicts = Path.Combine(Program.ScriptCachePath, $"{jsonFileName}_dicts_{ver}_{dictsHash}");
            string cachedCHeader = Path.Combine(Program.CCachePath, $"{jsonFileName}_ch_{ver}_{cHeaderHash}");
            string cachedLinker = Path.Combine(Program.CCachePath, $"{jsonFileName}_linker_{ver}_{linkerHash}");

            var status = new CacheStatus { CacheInvalid = false, CCacheInvalid = false, SymbolsInvalid = false };

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

            if (!File.Exists(cachedLinker))
            {
                status.SymbolsInvalid = true;
                Helpers.DeleteFileStartingWith(Program.CCachePath, $"{jsonFileName}_linker_");
                File.WriteAllText(cachedLinker, "");
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

        private static string GetLinkerHash(NPCFile data)
        {
            var sb = new StringBuilder();

            foreach (var linkerPath in Helpers.ResolveSemicolonPaths(Program.Settings.LinkerPaths))
                if (File.Exists(linkerPath))
                    sb.Append(File.ReadAllText(linkerPath));

            sb.Append(Program.Settings.LinkerPaths);

            return Helpers.GetBase64Hash(sb.ToString());
        }

        private static string GetCHeaderHash(NPCFile data)
        {
            data.CHeader = CCode.ReplaceGameVersionInclude(data.CHeader);

            var sb = new StringBuilder(data.CHeader);

            sb.Append(JsonConvert.SerializeObject(new
            {
                Program.Settings.Linker,
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

        // ── Compilation ───────────────────────────────────────────────────────────

        public static async Task<bool> PreprocessCodeAndScripts(string outPath, string outputDepsPath, string outputHPath, NPCFile data,
                                                          CacheStatus cacheStatus, IProgress<ProgressReport> progress)
        {
            float progressPer = 100f / data.Entries.Count;
            float curProgress = 0f;
            int lastReported = 0;
            bool ret = false;

            await Task.Run(() =>
            {

                Program.ConsoleWriteS("Compiling...");

                var results = new ConcurrentDictionary<string, object>();

                var ctx = new BuildContext
                {
                    Data = data,
                    CacheStatus = cacheStatus,
                    BaseDefines = Scripts.ScriptHelpers.GetBaseDefines(data),
                    JsonFileName = Program.JsonPath.FilenameFromPath(),
                    Results = results,
                    ScriptCacheFiles = new HashSet<string>(Directory.GetFiles(Program.ScriptCachePath)),
                    CCacheFiles = new HashSet<string>(Directory.GetFiles(Program.CCachePath))
                };

                Parallel.For(0, data.Entries.Count,
                    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 3 },
                    entryID =>
                    {
                        try
                        {
                            var ec = new EntryContext(ctx, entryID);

                            if (ec.Entry.IsNull || ec.Entry.Omitted)
                                return;

                            ec.Entry.ClearHeaderValues();

                            ProcessCCode(ec);

                            ec.ExtDataExists = CheckExtDataCache(ec, out string extDataFile);
                            ProcessScripts(ec);

                            results[entryID.ToString()] = ec.Status;

                            if (!ec.ExtDataExists)
                            {
                                Helpers.DeleteFileStartingWith(Program.ScriptCachePath, ec.Prefix + "exdata_");
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

                ret = SaveBinaryFile(outPath, outputDepsPath, outputHPath, ref data, progress, ctx.BaseDefines,
                                          new CacheStatus { CCacheInvalid = false, CacheInvalid = false, SymbolsInvalid = false }, results);

                CCode.CleanupStandardCompilationArtifacts();
                Program.CompileInProgress = false;
            });

            return ret;
        }

        public static bool SaveBinaryFile(string outPath, string outputDepsPath, string outputHPath, ref NPCFile data, IProgress<ProgressReport> progress, string baseDefines,
                                         CacheStatus cacheStatus, ConcurrentDictionary<string, object> preProcessedFiles)
        {
            if (!data.Entries.Any())
            {
                ShowMsg("Nothing to save.");
                Program.CompileThereWereErrors = false;
                return false;
            }

            try
            {
                var ctx = new BuildContext
                {
                    Data = data,
                    CacheStatus = cacheStatus,
                    BaseDefines = baseDefines,
                    JsonFileName = Program.JsonPath.FilenameFromPath(),
                    PreProcessed = preProcessedFiles
                };

                var compilationData = new CompilationEntryData[data.Entries.Count];

                float progressPer = 100f / data.Entries.Count;
                float curProgress = 0f;
                int entriesDone = 0;

                progress?.Report(new ProgressReport("Saving...", 0));

                var cts = new CancellationTokenSource();

                Action<int> processEntry = i =>
                {
                    if (cts.IsCancellationRequested)
                        return;

                    var ec = new EntryContext(ctx, i);

                    if (ec.Entry.IsNull || ec.Entry.Omitted)
                    {
                        compilationData[i] = new CompilationEntryData(null);
                        Program.ConsoleWriteLineS($"Entry {i} is blank or omitted.");
                    }
                    else
                    {
                        Program.ConsoleWriteS($"Processing entry {i}: {ec.Entry.NPCName}... ");

                        object csi;
                        if (preProcessedFiles?.TryGetValue(i.ToString(), out csi) == true)
                            ec.Status = (RecompilationStatus)csi;

                        ec.Entry.ClearHeaderValues();
                        BuildEntryBytes(ec);

                        if (ctx.ParseErrors.IsEmpty)
                        {
                            compilationData[i] = new CompilationEntryData(ec.Bytes);
                            Program.ConsoleWriteS($"OK {ec.Status}{Environment.NewLine}");
                        }
                        else
                        {
                            cts.Cancel();
                            return;
                        }
                    }

                    Helpers.AddInterlocked(ref curProgress, progressPer);
                    Interlocked.Increment(ref entriesDone);
                    progress?.Report(new ProgressReport($"Saving {entriesDone}/{ctx.Data.Entries.Count}", curProgress));
                };

                if (!Program.Settings.CompileInParallel || (Program.CliMode && !Program.consoleSilent))
                {
                    for (int i = 0; i < ctx.Data.Entries.Count; i++)
                        processEntry(i);
                }
                else
                {
                    try
                    {
                        Parallel.For(0, ctx.Data.Entries.Count,
                            new ParallelOptions { CancellationToken = cts.Token },
                            i => processEntry(i));
                    }
                    catch (OperationCanceledException) { }
                }

                data = ctx.Data;

                if (!ctx.ParseErrors.IsEmpty)
                {
                    ShowMsg($"File could not be saved.{Environment.NewLine}{Environment.NewLine}" +
                            $"There are errors in NPC: {string.Join(",", ctx.ParseErrors.Distinct())}");

                    progress?.Report(new ProgressReport("Done!", 100));
                    return false;
                }

                var targets = new OutputTargets { Bin = outPath, Deps = outputDepsPath, H = outputHPath };
                WriteOutput(targets, data, compilationData.ToList(), progress);
            }
            catch (Exception ex)
            {
                ShowMsg($"Error writing file: {ex.Message} {ex.StackTrace}");
                return false;
            }
            finally
            {
                Program.CompileInProgress = false;
            }

            return true;
        }

        // ── Entry Building ────────────────────────────────────────────────────────

        private static void BuildEntryBytes(EntryContext e)
        {
            BuildFixedFields(e);
            BuildMessages(e);
            BuildAnimations(e);
            BuildExtraDisplayLists(e);
            BuildColors(e);
            BuildSegments(e);
            BuildCCode(e);
            BuildScripts(e);
        }

        private static void BuildFixedFields(EntryContext e)
        {
            var entry = e.Entry;
            var bytes = e.Bytes;

            bytes.AddRange(new byte[]
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
            e.AlignAndCheck(InitialFieldsSize);

            bytes.AddRangeBigEndian(entry.ObjectID);
            bytes.AddRangeBigEndian(entry.LookAtDegreesVertical);
            bytes.AddRangeBigEndian(entry.LookAtDegreesHorizontal);
            bytes.AddRangeBigEndian(entry.CollisionRadius);
            bytes.AddRangeBigEndian(entry.CollisionHeight);
            bytes.AddRangeBigEndian(entry.CollisionYShift);
            bytes.AddRangeBigEndian(entry.ShadowRadius);
            bytes.AddRangeBigEndian(entry.MovementDistance);
            bytes.AddRangeBigEndian(entry.MaxDistRoam);
            bytes.AddRangeBigEndian(entry.PathStartNodeID);
            bytes.AddRangeBigEndian(entry.PathEndNodeID);
            bytes.AddRangeBigEndian(entry.MovementDelayTime);
            bytes.AddRangeBigEndian(entry.TimedPathStart);
            bytes.AddRangeBigEndian(entry.TimedPathEnd);
            bytes.AddRangeBigEndian(entry.SfxIfAttacked);
            bytes.AddRangeBigEndian(entry.NPCToRide);
            bytes.AddRangeBigEndian(entry.LightRadius);
            bytes.AddRangeBigEndian(entry.TargetPositionOffsets);
            bytes.AddRangeBigEndian(entry.ModelPositionOffsets);
            bytes.AddRangeBigEndian(entry.LightPositionOffsets);
            e.AlignAndCheck(52);

            bytes.AddRangeBigEndian(entry.ModelScale);
            bytes.AddRangeBigEndian(entry.TalkRadius);
            bytes.AddRangeBigEndian(entry.MovementSpeed);
            bytes.AddRangeBigEndian(entry.GravityForce);
            bytes.AddRangeBigEndian(entry.SmoothingConstant);
            bytes.AddRangeBigEndian(e.Resolve(entry.Hierarchy, entry.SkeletonHeaderDefinition));
            bytes.AddRangeBigEndian(e.Resolve((uint)entry.FileStart, entry.FileStartHeaderDefinition));
            bytes.AddRangeBigEndian(entry.CullForward);
            bytes.AddRangeBigEndian(entry.CullDown);
            bytes.AddRangeBigEndian(entry.CullScale);
            bytes.AddRangeBigEndian(entry.LookAtPositionOffsets);
            e.AlignAndCheck(52);

            bytes.AddRange(entry.GetBlinkPatternBytes());
            bytes.AddRange(entry.GetTalkPatternBytes());
            e.AlignAndCheck(8);
        }

        private static void BuildMessages(EntryContext e)
        {
            var entry = e.Entry;

            string msgDataHash = Helpers.GetBase64Hash(JsonConvert.SerializeObject(new { entry.Messages, entry.Localization }));
            string msgPrefix = e.Prefix + "msg_";
            string cachedMsgFile = Path.Combine(Program.ScriptCachePath, msgPrefix + msgDataHash);

            if (File.Exists(cachedMsgFile))
            {
                var cached = File.ReadAllBytes(cachedMsgFile);
                e.Bytes.AddRange(cached);
                e.CurLen += cached.Length;
                e.CheckLen();
                return;
            }

            e.Status.Messages = RecompStatus.Recompiled;
            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, msgPrefix);

            var locales = BuildLocaleList(entry, e.Data);
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
                    ShowMsg(errors.First());
                    e.AddParseError();
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
                        ShowMsg($"{entry.NPCName}: Message '{msg.Name}' exceeded {MaxMessageBytes} bytes and could not be saved.");
                        e.AddParseError();
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
            msgBytes.AddRangeBigEndian(e.Data.Languages.Count + 1);
            msgBytes.AddRangeBigEndian(entry.Messages.Count);
            msgBytes.AddRange(header);
            msgBytes.AddRange(msgData);

            e.Bytes.AddRange(msgBytes);
            e.CurLen += len;
            e.CheckLen();

            File.WriteAllBytes(cachedMsgFile, msgBytes.ToArray());
        }

        private static void AppendMessageHeader(List<byte> target, int msgOffset, MessageEntry msg, int byteCount)
        {
            target.AddRangeBigEndian(msgOffset);
            target.Add(msg.GetMessageTypePos());
            Helpers.Ensure2ByteAlign(target);
            target.AddRangeBigEndian((ushort)byteCount);
        }

        private static void BuildAnimations(EntryContext e)
        {
            var anims = e.Entry.Animations;

            e.Bytes.AddRangeBigEndian((uint)anims.Count);
            e.AlignAndCheck(4);

            foreach (var anim in anims)
            {
                var parts = Helpers.SplitHeaderDefsString(anim.HeaderDefinition);

                uint curAddr = anim.Address;
                int curFileStart = anim.FileStart;

                anim.Address = e.Resolve(anim.Address, parts[1]);
                anim.FileStart = (int)e.Resolve((uint)anim.FileStart, parts[0]);
                e.Bytes.AddRange(anim.ToBytes());

                anim.Address = curAddr;
                anim.FileStart = curFileStart;
            }

            e.AlignAndCheck(16 * anims.Count);
        }

        private static void BuildExtraDisplayLists(EntryContext e)
        {
            var dlists = e.Entry.ExtraDisplayLists;

            e.Bytes.AddRangeBigEndian((uint)dlists.Count);
            e.AlignAndCheck(4);

            foreach (var dlist in dlists)
            {
                uint curAddr = dlist.Address;
                int curFileStart = dlist.FileStart;

                var parts = Helpers.SplitHeaderDefsString(dlist.HeaderDefinition);
                dlist.Address = e.Resolve(dlist.Address, parts[1]);
                dlist.FileStart = (int)e.Resolve((uint)dlist.FileStart, parts[0]);
                e.Bytes.AddRange(dlist.ToBytes());

                dlist.Address = curAddr;
                dlist.FileStart = curFileStart;
            }

            e.AlignAndCheck(40 * dlists.Count);
        }

        private static void BuildColors(EntryContext e)
        {
            var colors = e.Entry.ParseColorEntries().OrderBy(c => c.LimbID).ToList();

            e.Bytes.AddRangeBigEndian((uint)colors.Count);
            e.AlignAndCheck(4);

            foreach (var color in colors)
                e.Bytes.AddRange(color.ToBytes());

            e.AlignAndCheck(4 * colors.Count);
        }

        private static void BuildSegments(EntryContext e)
        {
            var segOffsets = new List<byte>();
            var segEntries = new List<byte>();
            uint segOffset = 7 * 4;
            e.CurLen += (int)segOffset + 4;

            foreach (var segmentList in e.Entry.Segments)
            {
                uint segBytes = (uint)(12 * segmentList.Count);
                segOffsets.AddRangeBigEndian(segBytes != 0 ? segOffset : 0);
                segOffset += segBytes;
                e.CurLen += (int)segBytes;

                foreach (var segEntry in segmentList)
                {
                    var parts = Helpers.SplitHeaderDefsString(segEntry.HeaderDefinition);

                    uint curAddr = segEntry.Address;
                    int curFileStart = segEntry.FileStart;

                    segEntry.Address = e.Resolve(segEntry.Address, parts[1]);
                    segEntry.FileStart = (int)e.Resolve((uint)segEntry.FileStart, parts[0]);
                    segEntries.AddRange(segEntry.ToBytes());

                    segEntry.Address = curAddr;
                    segEntry.FileStart = curFileStart;
                }
            }

            e.Bytes.AddRangeBigEndian((uint)(segOffsets.Count + segEntries.Count));
            e.CurLen += 4;
            e.Bytes.AddRange(segOffsets);
            e.Bytes.AddRange(segEntries);

            Helpers.Ensure4ByteAlign(e.Bytes);
            e.CheckLen();
        }

        private static void BuildCCode(EntryContext e)
        {
            var code = e.Entry.EmbeddedOverlayCode;

            if (string.IsNullOrEmpty(code.Code))
            {
                e.Bytes.AddRangeBigEndian(-1);
                e.CurLen += 4;
                return;
            }

            byte[] overlay = ResolveOverlay(e);

            if (overlay == null)
            {
                e.AddParseError();
                e.Bytes.AddRangeBigEndian(-1);
                e.CurLen += 4;
                return;
            }

            // ResolveOverlay may have replaced the entry's code object with the cached one.
            code = e.Entry.EmbeddedOverlayCode;

            e.CurLen += 4;

            if (code.Functions == null || code.Functions.Count == 0)
            {
                e.Bytes.AddRangeBigEndian(-1);
                return;
            }

            e.Bytes.AddRangeBigEndian(overlay.Length);

            var funcsList = new List<byte>();
            var funcsWhenList = new List<byte>();

            for (int i = 0; i < code.FunctionHooks.Length; i++)
            {
                string fname = code.FunctionHooks[i].Symbol;
                int funcIdx = code.Functions.FindIndex(x => x.Symbol == fname);

                if (funcIdx == -1 && !string.IsNullOrEmpty(fname))
                {
                    ShowMsg($"{e.Entry.NPCName}: Function {fname} not found in the C Code!");
                    e.AddParseError();
                    return;
                }

                uint funcAddr = funcIdx >= 0 ? code.Functions[funcIdx].Addr : uint.MaxValue;

                funcsList.AddRangeBigEndian(funcAddr);
                funcsWhenList.Add((byte)code.FunctionHooks[i].RunWhen);
            }

            e.Bytes.AddRange(funcsList);
            e.Bytes.AddRange(funcsWhenList);
            e.AlignAndCheck(24 + 8);

            e.Bytes.AddRange(overlay);
            e.CurLen += overlay.Length;
            Helpers.Ensure4ByteAlign(e.Bytes);

            if (overlay.Length % 4 != 0)
                e.CurLen += 4 - (overlay.Length % 4);

            e.CheckLen();
        }

        private static void BuildScripts(EntryContext e)
        {
            var entry = e.Entry;
            var nonEmptyScripts = entry.Scripts.FindAll(x => !string.IsNullOrEmpty(x.Text));

            e.Bytes.AddRangeBigEndian((uint)nonEmptyScripts.Count);
            e.CurLen += 4;
            Helpers.Ensure4ByteAlign(e.Bytes);
            e.CheckLen();

            string extDataPrefix = e.Prefix + "exdata_";
            string cachedExtData = Path.Combine(Program.ScriptCachePath, extDataPrefix + Helpers.GetBase64Hash(BuildExtDataString(entry)));
            e.ExtDataExists = File.Exists(cachedExtData);

            var parsedScripts = new List<Scripts.BScript>();
            int scriptNum = 0;

            foreach (var scr in nonEmptyScripts)
            {
                parsedScripts.Add(ResolveScript(e, scr, scriptNum));
                scriptNum++;
            }

            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, extDataPrefix);
            File.Create(cachedExtData).Dispose();

            int scrOffset = 0;
            foreach (var scr in parsedScripts)
            {
                e.Bytes.AddRangeBigEndian(scrOffset);
                scrOffset += scr.Script.Length;
                e.CurLen += 4;
            }

            foreach (var scr in parsedScripts)
            {
                e.Bytes.AddRange(scr.Script);
                e.CurLen += scr.Script.Length;

                if (scr.ParseErrors.Count != 0)
                {
                    e.AddParseError();

                    Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}" +
                                      $"Script \"{scr.Name}\" had errors:{Environment.NewLine}");
                    Console.WriteLine(string.Join(Environment.NewLine, scr.ParseErrors));
                }
            }

            Helpers.Ensure4ByteAlign(e.Bytes);
            e.CheckLen();
        }

        // ── Overlay / Script Resolution ───────────────────────────────────────────

        private static CCacheKeys GetCCacheKeys(string jsonEntryPrefix, CCodeEntry overlayCode)
        {
            string codeString = CCode.ReplaceGameVersionInclude(overlayCode.Code)
                                + GetIncludesText(overlayCode.HeaderPaths);

            string hash = Helpers.GetBase64Hash(codeString);

            return new CCacheKeys
            {
                CachedAddrs = Path.Combine(Program.CCachePath, jsonEntryPrefix + "funcsaddrs_" + hash),
                CachedCode = Path.Combine(Program.CCachePath, jsonEntryPrefix + "code_" + hash),
                CachedObject = Path.Combine(Program.CCachePath, jsonEntryPrefix + "object_" + hash),
            };
        }

        private static byte[] ResolveOverlay(EntryContext e)
        {
            var ctx = e.Build;
            var entry = e.Entry;
            var keys = GetCCacheKeys(e.Prefix, entry.EmbeddedOverlayCode);

            if (ctx.PreProcessed != null)
            {
                object addrsData, codeData;
                ctx.PreProcessed.TryGetValue(keys.CachedAddrs, out addrsData);
                ctx.PreProcessed.TryGetValue(keys.CachedCode, out codeData);

                if (addrsData != null && codeData != null)
                {
                    entry.EmbeddedOverlayCode = (CCodeEntry)addrsData;
                    return (byte[])codeData;
                }
            }

            var cCacheFiles = ctx.CCacheFiles;

            bool diskCacheHit = !ctx.CacheStatus.CCacheInvalid
                && (cCacheFiles != null ? cCacheFiles.Contains(keys.CachedAddrs) : File.Exists(keys.CachedAddrs))
                && (cCacheFiles != null ? cCacheFiles.Contains(keys.CachedCode) : File.Exists(keys.CachedCode));

            if (diskCacheHit && !ctx.CacheStatus.SymbolsInvalid)
            {
                entry.EmbeddedOverlayCode = JsonConvert.DeserializeObject<CCodeEntry>(
                    File.ReadAllText(keys.CachedAddrs), IgnoreAttributeSettings);

                return File.ReadAllBytes(keys.CachedCode);
            }

            e.CompErrors = "";

            Helpers.DeleteFileStartingWith(Program.CCachePath, e.Prefix + "funcsaddrs_", cCacheFiles);
            Helpers.DeleteFileStartingWith(Program.CCachePath, e.Prefix + "code_", cCacheFiles);

            if (string.IsNullOrEmpty(entry.EmbeddedOverlayCode.Code))
                return null;

            string cachedObjectPath = "";
            e.Status.CCode = RecompStatus.Recompiled;

            if (diskCacheHit)
            {
                if (File.Exists(keys.CachedObject))
                {
                    cachedObjectPath = keys.CachedObject;
                    e.Status.CCode = RecompStatus.Relinked;
                }
                else
                    Helpers.DeleteFileStartingWith(Program.CCachePath, e.Prefix + "object_", cCacheFiles);
            }

            string compErrors = e.CompErrors;
            byte[] overlay = CCode.Compile(e.Data.CHeader, Program.Settings.LinkerPaths, entry.EmbeddedOverlayCode,
                                           ref compErrors, out _, "NPCCOMPILE" + e.Index, keys.CachedObject, cachedObjectPath);
            e.CompErrors = compErrors;

            if (overlay != null)
            {
                if (!diskCacheHit)
                    Helpers.DeleteFileStartingWith(Program.ScriptCachePath, e.Prefix + "script", ctx.ScriptCacheFiles);

                File.WriteAllText(keys.CachedAddrs, JsonConvert.SerializeObject(entry.EmbeddedOverlayCode, IgnoreAttributeSettings));
                File.WriteAllBytes(keys.CachedCode, overlay);

                ctx.PreProcessed?.TryAdd(keys.CachedAddrs, entry.EmbeddedOverlayCode);
                ctx.PreProcessed?.TryAdd(keys.CachedCode, overlay);
            }

            return overlay;
        }

        private static void ProcessCCode(EntryContext e)
        {
            byte[] overlay = ResolveOverlay(e);

            if (overlay != null)
            {
                var keys = GetCCacheKeys(e.Prefix, e.Entry.EmbeddedOverlayCode);
                e.Build.Results[keys.CachedAddrs] = e.Entry.EmbeddedOverlayCode;
                e.Build.Results[keys.CachedCode] = overlay;
            }
        }

        private static Scripts.BScript ResolveScript(EntryContext e, ScriptEntry scrEntry, int scriptNum)
        {
            var ctx = e.Build;

            string scriptPrefix = e.Prefix + "script" + scriptNum + "_";
            string cachedScriptFile = Path.Combine(Program.ScriptCachePath, scriptPrefix + Helpers.GetBase64Hash(scrEntry.Text));

            if (ctx.PreProcessed != null)
            {
                object cached;
                ctx.PreProcessed.TryGetValue(cachedScriptFile, out cached);
                if (cached != null)
                    return new Scripts.BScript
                    {
                        Script = (byte[])cached,
                        ParseErrors = new List<Scripts.ParseException>()
                    };
            }

            bool diskCacheHit = !ctx.CacheStatus.CacheInvalid
                && e.ExtDataExists
                && (ctx.ScriptCacheFiles != null
                    ? ctx.ScriptCacheFiles.Contains(cachedScriptFile)
                    : File.Exists(cachedScriptFile));

            if (diskCacheHit)
                return new Scripts.BScript
                {
                    Script = File.ReadAllBytes(cachedScriptFile),
                    ParseErrors = new List<Scripts.ParseException>()
                };

            e.Status.Scripts = RecompStatus.Recompiled;
            Helpers.DeleteFileStartingWith(Program.ScriptCachePath, scriptPrefix, ctx.ScriptCacheFiles);

            var result = new Scripts.ScriptParser(ref ctx.Data, e.Entry, scrEntry.Text, ctx.BaseDefines)
                            .ParseScript(scrEntry.Name, true);

            if (result.ParseErrors.Count == 0)
            {
                File.WriteAllBytes(cachedScriptFile, result.Script);
                ctx.PreProcessed?.TryAdd(cachedScriptFile, result.Script);
            }

            return result;
        }

        private static void ProcessScripts(EntryContext e)
        {
            var scripts = e.Entry.Scripts;

            for (int scriptNum = 0; scriptNum < scripts.Count; scriptNum++)
            {
                var scrEntry = scripts[scriptNum];

                if (string.IsNullOrEmpty(scrEntry.Text))
                    continue;

                var result = ResolveScript(e, scrEntry, scriptNum);

                if (result.ParseErrors.Count == 0)
                {
                    string cachedScriptFile = Path.Combine(Program.ScriptCachePath,
                        e.Prefix + "script" + scriptNum + "_" + Helpers.GetBase64Hash(scrEntry.Text));

                    e.Build.Results[cachedScriptFile] = result.Script;
                }
            }
        }

        private static bool CheckExtDataCache(EntryContext e, out string extDataFile)
        {
            string hash = Helpers.GetBase64Hash(BuildExtDataString(e.Entry));
            extDataFile = Path.Combine(Program.ScriptCachePath, e.Prefix + "exdata_" + hash);
            return e.Build.ScriptCacheFiles.Contains(extDataFile);
        }

        // ── Output ────────────────────────────────────────────────────────────────

        private static void WriteOutput(OutputTargets targets, NPCFile data,
                                        List<CompilationEntryData> compilationData, IProgress<ProgressReport> progress)
        {
            int offset = data.Entries.Count * 12 + 4;

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

            if (!string.IsNullOrEmpty(targets.Deps))
                File.WriteAllText(targets.Deps, CreateDepsFile(data, targets.Bin));

            if (!string.IsNullOrEmpty(targets.H))
                File.WriteAllText(targets.H, CreateNPCHFile(data, targets.Bin));

            Program.ConsoleWriteLineS("\nDone!");
            progress?.Report(new ProgressReport("Done!", 100));
            File.WriteAllBytes(targets.Bin, output.ToArray());
        }

        public static string CreateNPCHFile(NPCFile data, string zobjFilename)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < data.Entries.Count(); i++)
            {
                if (!data.Entries[i].IsNull)
                    sb.AppendLine($"#define NPCID_{data.Entries[i].NPCName} {i}");
            }

            return sb.ToString();
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

            addDep(Helpers.MakePathRelativeToCwd(Program.JsonPath));

            foreach (var entry in data.Entries)
            {
                foreach (var header in entry.EmbeddedOverlayCode.HeaderPaths)
                    addDep(Helpers.DenormalizeExtPath(header, false, true));

                foreach (var p in Helpers.ResolveSemicolonPaths(entry.HeaderPath, false, true))
                    addDep(p);

                foreach (var p in Helpers.ResolveSemicolonPaths(data.ExtScriptHeaderPath, false, true))
                    addDep(p);
            }

            if (data.IsFolder)
            {
                foreach (var p in NPCFileSplit.GetSplitNPCFilePaths(Program.JsonPath, data))
                    addDep(Helpers.DenormalizeExtPath(p, false, true));
            }

            var dictDirs = new[]
            {
                Path.Combine(Program.ExecPath, "Dicts"),
                Path.Combine(Path.GetDirectoryName(Program.JsonPath)!, "Dicts")
            };

            foreach (var dir in dictDirs)
            {
                foreach (var dict in Directory.GetFiles(dir))
                {
                    var extension = Path.GetExtension(dict);

                    if (extension.Equals(".json", StringComparison.OrdinalIgnoreCase) || extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        addDep(Helpers.MakePathRelativeToCwd(dict));
                }
            }

            string escapedTarget = Helpers.MakePathRelativeToCwd(zobjFilename).Replace(" ", "\\ ");

            var sb = new StringBuilder();
            sb.Append(escapedTarget);
            sb.Append(':');

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

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static void ShowMsg(string msg)
        {
            Program.CompileThereWereErrors = true;

            if (Program.IsRunningUnderMono || Program.CliMode)
                Console.WriteLine(msg);

            // Occasionally crashed showing messagebox on another thread.
            if (Program.IsRunningUnderMono)
                Program.CompileMonoErrors = msg;
        }

        private static uint TryGetFromH(string npcName, uint defaultValue, Dictionary<string, string> defines, string name, ConcurrentBag<string> parseErrors)
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
    }
}