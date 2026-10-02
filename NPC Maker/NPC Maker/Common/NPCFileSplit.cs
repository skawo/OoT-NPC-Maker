using Newtonsoft.Json;
using NPC_Maker.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NPC_Maker.Common
{
    internal class NPCFileSplit
    {
        public static List<string> GetSplitNPCFilePaths(string fileName, NPCFile npcFile)
        {
            var paths = new List<string>();

            string rootDirectory = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + "_content");

            string headersPath = Path.Combine(rootDirectory, "headers");
            string npcsPath = Path.Combine(rootDirectory, "npcs");
            string cHeaderPath = Path.Combine(rootDirectory, "npc_maker_header.h");

            paths.Add(cHeaderPath);

            // Global headers
            paths.AddRange(GetScriptPaths(headersPath, npcFile.GlobalHeaders));

            // Per-entry files
            int total = npcFile.Entries.Count;
            for (int i = 0; i < total; i++)
            {
                var entry = npcFile.Entries[i];
                string name = entry.IsNull ? "NULL_ENTRY" : entry.NPCName;
                string directory = Path.Combine(npcsPath, Prefixed(i, total, name));

                paths.Add(Path.Combine(directory, "npc.json"));

                if (entry.IsNull)
                    continue;

                var codeLines = entry.EmbeddedOverlayCode?.CodeLines;
                if (codeLines != null && codeLines.Count > 0)
                    paths.Add(Path.Combine(directory, "code.c"));

                paths.AddRange(GetScriptPaths(Path.Combine(directory, "scripts"), entry.Scripts));

                string messagesPath = Path.Combine(directory, "messages");
                paths.Add(Path.Combine(messagesPath, "Default.txt"));

                if (entry.Localization != null)
                {
                    foreach (var le in entry.Localization)
                        paths.Add(Path.Combine(messagesPath, $"{SanitizeName(le.Language)}.txt"));
                }
            }

            return paths;
        }

        private static IEnumerable<string> GetScriptPaths(string folder, List<ScriptEntry> scripts)
        {
            if (scripts == null)
                yield break;

            for (int i = 0; i < scripts.Count; i++)
            {
                string fileName = Prefixed(i, scripts.Count, scripts[i].Name) + ".npcm";
                yield return Path.Combine(folder, fileName);
            }
        }

        public static void ReconstructNPCFileFromFolder(string fileName, ref NPCFile inFile)
        {
            if (!inFile.IsFolder)
                return;

            string rootDirectory = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + "_content");
            string headersPath = Path.Combine(rootDirectory, "headers");

            try
            {
                inFile.GlobalHeaders = new List<ScriptEntry>();

                foreach (FolderEntry file in FolderLister.ListSortedWithPaths(headersPath, EntryKind.FilesOnly))
                {
                    if (Path.GetExtension(file.Name) == ".npcm")
                    {
                        ScriptEntry scr = new ScriptEntry();
                        scr.Name = Path.GetFileNameWithoutExtension(file.Name);
                        scr.TextLines = File.ReadAllLines(file.FullPath).ToList();
                        inFile.GlobalHeaders.Add(scr);
                    }
                }
                
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to open unpacked JSON: Couldn't load external headers: {ex.Message}");
                return;
            }

            string cHeaderPath = Path.Combine(rootDirectory, "npc_maker_header.h");

            try
            {
                inFile.CHeaderLines = File.ReadAllLines(cHeaderPath).ToList();
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to open unpacked JSON: Couldn't load C header: {ex.Message}");
                return;
            }

            string npcsPath = Path.Combine(rootDirectory, "npcs");

            if (!Directory.Exists(npcsPath))
            {
                BigMessageBox.Show($"Failed to open unpacked JSON: No NPCs folder?");
                return;
            }

            try
            {
                List<FolderEntry> npcDirs = FolderLister.ListSortedWithPaths(npcsPath, EntryKind.DirectoriesOnly).ToList();
                NPCEntry[] results = new NPCEntry[npcDirs.Count];
                List<string> languages = inFile.Languages.ToList();

                object failLock = new object();
                string failure = null;

                Parallel.For(0, npcDirs.Count, (i, state) =>
                {
                    try
                    {
                        results[i] = LoadNPCEntry(npcDirs[i].FullPath, languages);
                    }
                    catch (Exception ex)
                    {
                        lock (failLock)
                        {
                            if (failure == null)
                                failure = $"{npcDirs[i].Name}: {ex.Message}";
                        }
                        state.Stop();
                    }
                });

                if (failure != null)
                {
                    BigMessageBox.Show($"Failed to open unpacked JSON: {failure}");
                    return;
                }

                inFile.Entries.AddRange(results);
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to open unpacked JSON: {ex.Message}");
                return;
            }
        }

        private static NPCEntry LoadNPCEntry(string directory, List<string> languages)
        {
            string jsonText = File.ReadAllText(Path.Combine(directory, "npc.json"));
            var entry = JsonConvert.DeserializeObject<NPCEntry>(jsonText);

            if (entry.IsNull)
                return entry;

            string cCodePath = Path.Combine(directory, "code.c");
            if (File.Exists(cCodePath))
            {
                if (entry.EmbeddedOverlayCode == null)
                    entry.EmbeddedOverlayCode = new CCodeEntry();

                entry.EmbeddedOverlayCode.CodeLines = File.ReadAllLines(cCodePath).ToList();
            }

            entry.Scripts = new List<ScriptEntry>();
            string scriptsPath = Path.Combine(directory, "scripts");

            foreach (FolderEntry scriptFile in FolderLister.ListSortedWithPaths(scriptsPath, EntryKind.FilesOnly))
            {
                if (Path.GetExtension(scriptFile.Name) == ".npcm")
                {
                    ScriptEntry scr = new ScriptEntry();
                    scr.Name = Path.GetFileNameWithoutExtension(scriptFile.Name);
                    scr.TextLines = File.ReadAllLines(scriptFile.FullPath).ToList();
                    entry.Scripts.Add(scr);
                }
            }

            entry.Localization = new List<LocalizationEntry>();
            string messagesPath = Path.Combine(directory, "messages");

            if (!Directory.Exists(messagesPath))
                throw new Exception($"Failed to load NPC: No messages folder?");

            try
            {
                string defaultLPath = Path.Combine(messagesPath, "Default.txt");

                if (File.Exists(defaultLPath))
                    entry.Messages = NPCEntry.ConvertTxtToMessages(File.ReadAllLines(defaultLPath));
            }
            catch (Exception ex)
            {
                throw new Exception($"{Lists.DefaultLanguage}: {ex.Message}");
            }

            foreach (string language in languages)
            {
                string langPath = Path.Combine(messagesPath, $"{language}.txt");
                if (File.Exists(langPath))
                {
                    try
                    {
                        LocalizationEntry le = new LocalizationEntry();
                        le.Language = language;
                        le.Messages = NPCEntry.ConvertTxtToMessages(File.ReadAllLines(langPath));
                        entry.Localization.Add(le);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{language}:, {ex.Message}");
                    }
                }
            }

            return entry;
        }

        public static bool SplitNPCFileToFolder(string fileName, NPCFile inFile, IProgress<ProgressReport> progress = null)
        {
            string rootDirectory = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + "_content");
            string headersPath = Path.Combine(rootDirectory, "headers");
            string npcsPath = Path.Combine(rootDirectory, "npcs");
            string cHeaderPath = Path.Combine(rootDirectory, "npc_maker_header.h");

            string tempRoot = Path.Combine(rootDirectory, ".split_tmp_" + Guid.NewGuid().ToString("N"));
            string tempHeadersPath = Path.Combine(tempRoot, "headers");
            string tempNpcsPath = Path.Combine(tempRoot, "npcs");
            string tempCHeaderPath = Path.Combine(tempRoot, "npc_maker_header.h");
            string tempJsonPath = Path.Combine(tempRoot, Path.GetFileName(fileName));

            NPCFile npcFile = Helpers.Clone<NPCFile>(inFile);

            try
            {
                try
                {
                    Directory.CreateDirectory(tempNpcsPath);
                    Directory.CreateDirectory(tempHeadersPath);
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show($"Failed to unpack JSON: Couldn't create temporary directory: {ex.Message}");
                    return false;
                }

                try
                {
                    WriteScripts(tempHeadersPath, npcFile.GlobalHeaders);
                    npcFile.GlobalHeaders = null;
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show($"Failed to unpack JSON: Couldn't save external headers: {ex.Message}");
                    return false;
                }

                try
                {
                    File.WriteAllLines(tempCHeaderPath, npcFile.CHeaderLines);
                    npcFile.CHeader = null;
                    npcFile.CHeaderLines = null;
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show($"Failed to unpack JSON: Couldn't save C header: {ex.Message}");
                    return false;
                }

                int total = npcFile.Entries.Count;
                int processedCount = 0;
                object failLock = new object();
                string failure = null;

                Parallel.For(0, total, (i, state) =>
                {
                    var entry = npcFile.Entries[i];
                    string name = entry.IsNull ? "NULL_ENTRY" : entry.NPCName;
                    string directory = Path.Combine(tempNpcsPath, Prefixed(i, total, name));

                    try
                    {
                        var scripts = entry.Scripts;
                        var localization = entry.Localization;
                        var codeLines = entry.EmbeddedOverlayCode?.CodeLines;

                        Directory.CreateDirectory(directory);

                        if (!entry.IsNull)
                        {
                            if (codeLines != null && codeLines.Any(l => !string.IsNullOrWhiteSpace(l)))
                                File.WriteAllLines(Path.Combine(directory, "code.c"), codeLines);

                            WriteScripts(Path.Combine(directory, "scripts"), scripts);

                            string messagesPath = Path.Combine(directory, "messages");
                            Directory.CreateDirectory(messagesPath);

                            string mes = entry.ConvertMessagesToTxt(Lists.DefaultLanguage);

                            if (!String.IsNullOrWhiteSpace(mes))
                                File.WriteAllText(Path.Combine(messagesPath, "Default.txt"), mes);

                            if (localization != null)
                            {
                                foreach (var le in localization)
                                {
                                    mes = entry.ConvertMessagesToTxt(le.Language);

                                    if (!String.IsNullOrWhiteSpace(mes))
                                        File.WriteAllText(Path.Combine(messagesPath, $"{SanitizeName(le.Language)}.txt"), mes);
                                }
                            }

                            entry.Scripts = null;
                            entry.Messages = null;
                            entry.Localization = null;

                            if (entry.EmbeddedOverlayCode != null && String.IsNullOrWhiteSpace(entry.EmbeddedOverlayCode.Code))
                            {
                                entry.EmbeddedOverlayCode.CodeLines = null;
                                entry.EmbeddedOverlayCode.Code = null;
                            }
                        }

                        string json = JsonConvert.SerializeObject(entry, new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        File.WriteAllText(Path.Combine(directory, "npc.json"), json);

                        int done = Interlocked.Increment(ref processedCount);
                        float pct = done * 100f / total;
                        progress?.Report(new ProgressReport($"Saving {pct:0}%", pct));
                    }
                    catch (Exception ex)
                    {
                        lock (failLock)
                        {
                            if (failure == null)
                                failure = $"{directory}: {ex.Message}";
                        }
                        state.Stop();
                    }
                });

                if (failure != null)
                {
                    BigMessageBox.Show($"Failed to unpack JSON: {failure}");
                    return false;
                }

                npcFile.Entries = null;

                try
                {
                    string json = JsonConvert.SerializeObject(npcFile, new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    File.WriteAllText(tempJsonPath, json);
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show($"Failed to unpack JSON: Couldn't write main JSON to temp path: {ex.Message}");
                    return false;
                }

                // Everything was generated successfully. Now replace the real destination.
                try
                {
                    FileSwapper.SwapFiles(new FileSwapper.FileSwap[]
                    {
                        new FileSwapper.FileSwap(tempNpcsPath, npcsPath),
                        new FileSwapper.FileSwap(tempHeadersPath, headersPath),
                        new FileSwapper.FileSwap(tempCHeaderPath, cHeaderPath),
                        new FileSwapper.FileSwap(tempJsonPath, fileName),
                    });
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show($"Failed to unpack JSON: {ex.Message}");
                    return false;
                }

                progress?.Report(new ProgressReport($"Saved as unpacked.", 100.0f));

                return true;
            }
            finally
            {
                try
                {
                    if (Directory.Exists(tempRoot))
                        Directory.Delete(tempRoot, true);
                }
                catch
                {
                }
            }
        }

        private static void WriteScripts(string folder, List<ScriptEntry> scripts)
        {
            Directory.CreateDirectory(folder);
            if (scripts == null)
                return;

            for (int i = 0; i < scripts.Count; i++)
            {
                string fileName = Prefixed(i, scripts.Count, scripts[i].Name) + ".npcm";
                File.WriteAllLines(Path.Combine(folder, fileName), scripts[i].TextLines);
            }
        }

        // "007-Name"
        private static string Prefixed(int index, int count, string name)
        {
            int digits = Math.Max(3, (count - 1).ToString().Length);
            return $"{index.ToString().PadLeft(digits, '0')}-{SanitizeName(name)}";
        }

        private static string SanitizeName(string name)
        {
            name = name.Replace(" ", "_");

            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Trim();
        }

    }
}
