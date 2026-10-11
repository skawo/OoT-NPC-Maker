using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPC_Maker.Common;
using NPC_Maker.Scripts;
using System;
using System.Collections;
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

                NPCFileSplit.ReconstructNPCFileFromFolder(fileName, ref npcFile);

                version = MigrateToVersion2(ref npcFile, version > 1 ? null : JObject.Parse(jsonText), version);
                version = MigrateToVersion3(ref npcFile, version);

                if (version > 3) ProcessTextLinesForVersion4Plus(ref npcFile);
                if (version > 4) ProcessMessageLinesForVersion5Plus(ref npcFile);

                version = MigrateToVersion6(ref npcFile, version);
                version = MigrateToVersion7(ref npcFile, version);

                if (version >= 6) ProcessCHeader(ref npcFile);

                version = MigrateToVersion9(ref npcFile, version);

                NormalizeLineBreaks(ref npcFile);
                ResolveHeaderDefines(ref npcFile);

                npcFile.Version = 9;
                return npcFile;
            }
            catch (Exception ex)
            {
                Program.ConsoleWriteLineS($"Failed to read JSON: {ex.Message}");
                return null;
            }
        }

        public static string ConvertNPCFileToJSON(NPCFile npcFile)
        {
            string json = JsonConvert.SerializeObject(npcFile, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json.Replace(Environment.NewLine, "\n");
        }

        public static bool SaveNPCJSON(string path, NPCFile data, IProgress<ProgressReport> progress = null, string json = null, bool isBackup = false)
        {
            try
            {
                NPCFile processedData = data;

                if (json == null)
                    processedData = ProcessNPCJSON(data, progress, isBackup);

                if (json == null && data.IsFolder && !isBackup)
                {
                    NPCFileSplit.SplitNPCFileToFolder(path, processedData, progress);
                    return true;
                }
                else
                {
                    if (json == null)
                        json = ConvertNPCFileToJSON(processedData);

                    if (json != null)
                        File.WriteAllText(path, json);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not save backup: {ex.Message}");
                return false;
            }
        }

        public static NPCFile ProcessNPCJSON(NPCFile data, IProgress<ProgressReport> progress = null, bool isBackup = false)
        {
            try
            {
                var output = Helpers.Clone<NPCFile>(data);

                float progressPer = 100f / output.Entries.Count;
                int processedCount = 0;

                Parallel.ForEach(output.Entries, entry =>
                {
                    if (!data.IsFolder)
                    {
                        if (entry.Scripts != null)
                        {
                            foreach (var script in entry.Scripts)
                            {
                                script.TextLines = Helpers.SplitToTrimmedLines(script.Text);
                                script.Text = null;
                            }
                        }

                        if (entry.Messages != null)
                        {
                            foreach (var message in entry.Messages)
                                message.FlattenMessage();
                        }

                        if (entry.Localization != null)
                        {
                            foreach (var loc in entry.Localization)
                                foreach (var message in loc.Messages)
                                    message.FlattenMessage();
                        }

                        if (entry.EmbeddedOverlayCode?.Code != null)
                        {
                            if (entry.EmbeddedOverlayCode.Code != null)
                            {
                                entry.EmbeddedOverlayCode.CodeLines = Helpers.SplitToTrimmedLines(entry.EmbeddedOverlayCode.Code);
                                entry.EmbeddedOverlayCode.Code = null;
                            }
                        }

                        entry.ClearHeaderValues();

                        if (progress != null)
                        {
                            int current = Interlocked.Increment(ref processedCount);
                            if (current % 10 == 0 || current == output.Entries.Count)
                            {
                                float pct = Math.Min(current * progressPer, 100f);
                                progress.Report(new ProgressReport($"Saving {pct:0}%", pct));
                            }
                        }
                    }
                });

                if (!data.IsFolder)
                {
                    if (output.GlobalHeaders != null)
                    {
                        foreach (var script in output.GlobalHeaders)
                        {
                            script.TextLines = Helpers.SplitToTrimmedLines(script.Text);
                            script.Text = null;
                        }
                    }

                    if (output.CHeader != null)
                    {
                        output.CHeaderLines = Helpers.SplitToTrimmedLines(output.CHeader);
                        output.CHeader = null;
                    }
                }

                return output;
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
    }
}