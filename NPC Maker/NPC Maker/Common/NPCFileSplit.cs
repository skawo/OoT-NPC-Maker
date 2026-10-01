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
        public static void ReconstructNPCFileFromFolder(string fileName, ref NPCFile inFile)
        {
            if (!inFile.isFolder)
                return;

            string jsonPath = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));
            string headersPath = Path.Combine(jsonPath, "gScriptHeaders");

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
                BigMessageBox.Show($"Failed to reconstruct JSON: Couldn't load external headers: {ex.Message}");
                return;
            }

            string cHeaderPath = Path.Combine(jsonPath, "cHeader.h");

            try
            {
                inFile.CHeaderLines = File.ReadAllLines(cHeaderPath).ToList();
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to reconstruct JSON: Couldn't load C header: {ex.Message}");
                return;
            }

            string npcsPath = Path.Combine(jsonPath, "npcs");

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
                    BigMessageBox.Show($"Failed to reconstruct JSON: {failure}");
                    return;
                }

                inFile.Entries.AddRange(results);
            }
            catch (Exception ex)
            {
                BigMessageBox.Show($"Failed to reconstruct JSON: {ex.Message}");
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

            entry.Messages = NPCEntry.ConvertTxtToMessages(File.ReadAllLines(Path.Combine(messagesPath, "Default.txt")));

            foreach (string language in languages)
            {
                string langPath = Path.Combine(messagesPath, $"{language}.txt");
                if (File.Exists(langPath))
                {
                    LocalizationEntry le = new LocalizationEntry();
                    le.Language = language;
                    le.Messages = NPCEntry.ConvertTxtToMessages(File.ReadAllLines(langPath));
                    entry.Localization.Add(le);
                }
            }

            return entry;
        }

        public static bool SplitNPCFileToFolder(string fileName, NPCFile inFile, IProgress<ProgressReport> progress = null)
        {
            string rootDirectory = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));
            string headersPath = Path.Combine(rootDirectory, "gScriptHeaders");
            string npcsPath = Path.Combine(rootDirectory, "npcs");
            string cHeaderPath = Path.Combine(rootDirectory, "cHeader.h");

            string tempRoot = Path.Combine(rootDirectory, ".split_tmp_" + Guid.NewGuid().ToString("N"));
            string tempHeadersPath = Path.Combine(tempRoot, "gScriptHeaders");
            string tempNpcsPath = Path.Combine(tempRoot, "npcs");
            string tempCHeaderPath = Path.Combine(tempRoot, "cHeader.h");
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
                    BigMessageBox.Show($"Failed to split JSON: Couldn't create temporary directory: {ex.Message}");
                    return false;
                }

                try
                {
                    WriteScripts(tempHeadersPath, npcFile.GlobalHeaders);
                    npcFile.GlobalHeaders = null;
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show($"Failed to split JSON: Couldn't save external headers: {ex.Message}");
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
                    BigMessageBox.Show($"Failed to split JSON: Couldn't save C header: {ex.Message}");
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
                            if (codeLines != null && codeLines.Count > 0)
                                File.WriteAllLines(Path.Combine(directory, "code.c"), codeLines);

                            WriteScripts(Path.Combine(directory, "scripts"), scripts);

                            string messagesPath = Path.Combine(directory, "messages");
                            Directory.CreateDirectory(messagesPath);

                            File.WriteAllText(Path.Combine(messagesPath, "Default.txt"), entry.ConvertMessagesToTxt(Lists.DefaultLanguage));

                            if (localization != null)
                            {
                                foreach (var le in localization)
                                    File.WriteAllText(Path.Combine(messagesPath, $"{SanitizeName(le.Language)}.txt"), entry.ConvertMessagesToTxt(le.Language));
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
                    BigMessageBox.Show($"Failed to split JSON: {failure}");
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
                    BigMessageBox.Show($"Failed to split JSON: Couldn't write main JSON to temp path: {ex.Message}");
                    return false;
                }

                // Everything was generated successfully. Now replace the real destination.
                try
                {
                    SwapIntoPlace(new SwapItem[]
                    {
                        new SwapItem(tempNpcsPath, npcsPath),
                        new SwapItem(tempHeadersPath, headersPath),
                        new SwapItem(tempCHeaderPath, cHeaderPath),
                        new SwapItem(tempJsonPath, fileName),
                    });
                }
                catch (Exception ex)
                {
                    BigMessageBox.Show($"Failed to split JSON: Couldn't write to files: {ex.Message}");
                    return false;
                }

                progress?.Report(new ProgressReport($"Saved as folder.", 100.0f));

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

        private class SwapItem
        {
            public readonly string Source;
            public readonly string Destination;
            public string Backup;

            public SwapItem(string source, string destination)
            {
                Source = source;
                Destination = destination;
            }
        }

        // Replaces each destination (file or directory).
        // Existing destinations are first renamed to backups; if anything fails, all
        // swaps made so far are rolled back. Backups are deleted only after every swap succeeded.
        private static void SwapIntoPlace(SwapItem[] items)
        {
            string suffix = ".old_" + Guid.NewGuid().ToString("N");
            List<SwapItem> done = new List<SwapItem>();

            try
            {
                foreach (SwapItem item in items)
                {
                    try
                    {
                        if (Directory.Exists(item.Destination))
                        {
                            item.Backup = item.Destination + suffix;
                            Directory.Move(item.Destination, item.Backup);
                        }
                        else if (File.Exists(item.Destination))
                        {
                            item.Backup = item.Destination + suffix;
                            File.Move(item.Destination, item.Backup);
                        }

                        // Record before moving so a failed move still restores the backup.
                        done.Add(item);

                        if (Directory.Exists(item.Source))
                            Directory.Move(item.Source, item.Destination);
                        else
                            File.Move(item.Source, item.Destination);
                    }
                    catch (Exception ex)
                    {
                        throw new IOException($"'{item.Destination}': {ex.Message}", ex);
                    }
                }
            }
            catch
            {
                // Roll back in reverse order.
                for (int i = done.Count - 1; i >= 0; i--)
                {
                    SwapItem item = done[i];
                    try
                    {
                        if (item.Backup != null)
                        {
                            if (Directory.Exists(item.Destination))
                                Directory.Delete(item.Destination, true);
                            else if (File.Exists(item.Destination))
                                File.Delete(item.Destination);

                            if (Directory.Exists(item.Backup))
                                Directory.Move(item.Backup, item.Destination);
                            else if (File.Exists(item.Backup))
                                File.Move(item.Backup, item.Destination);
                        }
                    }
                    catch
                    {
                    }
                }

                throw;
            }

            // Success: discard the old versions.
            foreach (SwapItem item in done)
            {
                if (item.Backup == null) continue;

                try
                {
                    if (Directory.Exists(item.Backup))
                        Directory.Delete(item.Backup, true);
                    else if (File.Exists(item.Backup))
                        File.Delete(item.Backup);
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

        // "007 - Name"
        private static string Prefixed(int index, int count, string name)
        {
            int digits = Math.Max(2, (count - 1).ToString().Length);
            return $"{index.ToString().PadLeft(digits, '0')} - {SanitizeName(name)}";
        }

        private static string SanitizeName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Trim();
        }

    }
}
