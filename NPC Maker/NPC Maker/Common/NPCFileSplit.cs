using Newtonsoft.Json;
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
        // Folder / file names
        private const string ContentFolderSuffix = "_content";
        private const string HeadersFolder = "headers";
        private const string NpcsFolder = "npcs";
        private const string ScriptsFolder = "scripts";
        private const string MessagesFolder = "messages";
        private const string CHeaderFileName = "npc_maker_header.h";
        private const string NpcJsonFileName = "npc.json";
        private const string CodeFileName = "code.c";
        private const string DefaultMessagesFileName = "Default.txt";
        private const string TempFolderPrefix = ".split_tmp_";

        // Extensions
        private const string ScriptExtension = ".npcm";
        private const string MessagesExtension = ".txt";

        // Misc
        private const string NullEntryName = "NULL_ENTRY";

        // Error message prefixes
        private const string OpenFailed = "Failed to open unpacked JSON";
        private const string UnpackFailed = "Failed to unpack JSON";

        private static string GetRootDirectory(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ContentFolderSuffix);
        }

        public static List<string> GetSplitNPCFilePaths(string fileName, NPCFile npcFile)
        {
            var paths = new List<string>();

            string rootDirectory = GetRootDirectory(fileName);

            string headersPath = Path.Combine(rootDirectory, HeadersFolder);
            string npcsPath = Path.Combine(rootDirectory, NpcsFolder);
            string cHeaderPath = Path.Combine(rootDirectory, CHeaderFileName);

            paths.Add(cHeaderPath);

            // Global headers
            paths.AddRange(GetScriptPaths(headersPath, npcFile.GlobalHeaders));

            // Per-entry files
            int total = npcFile.Entries.Count;
            for (int i = 0; i < total; i++)
            {
                var entry = npcFile.Entries[i];
                string name = entry.IsNull ? NullEntryName : entry.NPCName;
                string directory = Path.Combine(npcsPath, Prefixed(i, total, name));

                paths.Add(Path.Combine(directory, NpcJsonFileName));

                if (entry.IsNull)
                    continue;

                var codeLines = entry.EmbeddedOverlayCode?.CodeLines;
                if (codeLines != null && codeLines.Count > 0)
                    paths.Add(Path.Combine(directory, CodeFileName));

                paths.AddRange(GetScriptPaths(Path.Combine(directory, ScriptsFolder), entry.Scripts));

                string messagesPath = Path.Combine(directory, MessagesFolder);
                paths.Add(Path.Combine(messagesPath, DefaultMessagesFileName));

                if (entry.Localization != null)
                {
                    foreach (var le in entry.Localization)
                        paths.Add(Path.Combine(messagesPath, $"{SanitizeName(le.Language)}{MessagesExtension}"));
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
                string fileName = Prefixed(i, scripts.Count, scripts[i].Name) + ScriptExtension;
                yield return Path.Combine(folder, fileName);
            }
        }

        public static void ReconstructNPCFileFromFolder(string fileName, ref NPCFile inFile)
        {
            if (!inFile.IsFolder)
                return;

            string rootDirectory = GetRootDirectory(fileName);
            string headersPath = Path.Combine(rootDirectory, HeadersFolder);

            try
            {
                inFile.GlobalHeaders = new List<ScriptEntry>();

                foreach (FolderEntry file in FolderLister.ListSortedWithPaths(headersPath, EntryKind.FilesOnly))
                {
                    if (Path.GetExtension(file.Name) == ScriptExtension)
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
                Console.WriteLine($"{OpenFailed}: Couldn't load external headers: {ex.Message}");
                return;
            }

            string cHeaderPath = Path.Combine(rootDirectory, CHeaderFileName);

            try
            {
                inFile.CHeaderLines = File.ReadAllLines(cHeaderPath).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{OpenFailed}: Couldn't load C header: {ex.Message}");
                return;
            }

            string npcsPath = Path.Combine(rootDirectory, NpcsFolder);

            if (!Directory.Exists(npcsPath))
            {
                Console.WriteLine($"{OpenFailed}: No NPCs folder?");
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
                    Console.WriteLine($"{OpenFailed}: {failure}");
                    return;
                }

                inFile.Entries.AddRange(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{OpenFailed}: {ex.Message}");
                return;
            }
        }

        private static NPCEntry LoadNPCEntry(string directory, List<string> languages)
        {
            string jsonText = File.ReadAllText(Path.Combine(directory, NpcJsonFileName));
            var entry = JsonConvert.DeserializeObject<NPCEntry>(jsonText);

            if (entry.IsNull)
                return entry;

            string cCodePath = Path.Combine(directory, CodeFileName);
            if (File.Exists(cCodePath))
            {
                if (entry.EmbeddedOverlayCode == null)
                    entry.EmbeddedOverlayCode = new CCodeEntry();

                entry.EmbeddedOverlayCode.CodeLines = File.ReadAllLines(cCodePath).ToList();
            }

            entry.Scripts = new List<ScriptEntry>();
            string scriptsPath = Path.Combine(directory, ScriptsFolder);

            foreach (FolderEntry scriptFile in FolderLister.ListSortedWithPaths(scriptsPath, EntryKind.FilesOnly))
            {
                if (Path.GetExtension(scriptFile.Name) == ScriptExtension)
                {
                    ScriptEntry scr = new ScriptEntry();
                    scr.Name = Path.GetFileNameWithoutExtension(scriptFile.Name);
                    scr.TextLines = File.ReadAllLines(scriptFile.FullPath).ToList();
                    entry.Scripts.Add(scr);
                }
            }

            entry.Messages = new List<MessageEntry>();
            entry.Localization = new List<LocalizationEntry>();
            string messagesPath = Path.Combine(directory, MessagesFolder);

            if (!Directory.Exists(messagesPath))
                throw new Exception($"Failed to load NPC: No messages folder?");

            try
            {
                string defaultLPath = Path.Combine(messagesPath, DefaultMessagesFileName);

                if (File.Exists(defaultLPath))
                    entry.Messages = NPCEntry.ConvertTxtToMessages(File.ReadAllLines(defaultLPath));
            }
            catch (Exception ex)
            {
                throw new Exception($"{Lists.DefaultLanguage}: {ex.Message}");
            }

            foreach (string language in languages)
            {
                string langPath = Path.Combine(messagesPath, $"{language}{MessagesExtension}");

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
            string rootDirectory = GetRootDirectory(fileName);
            string headersPath = Path.Combine(rootDirectory, HeadersFolder);
            string npcsPath = Path.Combine(rootDirectory, NpcsFolder);
            string cHeaderPath = Path.Combine(rootDirectory, CHeaderFileName);

            string tempRoot = Path.Combine(rootDirectory, TempFolderPrefix + Guid.NewGuid().ToString("N"));
            string tempHeadersPath = Path.Combine(tempRoot, HeadersFolder);
            string tempNpcsPath = Path.Combine(tempRoot, NpcsFolder);
            string tempCHeaderPath = Path.Combine(tempRoot, CHeaderFileName);
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
                    Console.WriteLine($"{UnpackFailed}: Couldn't create temporary directory: {ex.Message}");
                    return false;
                }

                try
                {
                    WriteScripts(tempHeadersPath, npcFile.GlobalHeaders);
                    npcFile.GlobalHeaders = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{UnpackFailed}: Couldn't save external headers: {ex.Message}");
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
                    Console.WriteLine($"{UnpackFailed}: Couldn't save C header: {ex.Message}");
                    return false;
                }

                int total = npcFile.Entries.Count;
                int processedCount = 0;
                object failLock = new object();
                string failure = null;

                Parallel.For(0, total, (i, state) =>
                {
                    var entry = npcFile.Entries[i];
                    string name = entry.IsNull ? NullEntryName : entry.NPCName;
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
                                File.WriteAllLines(Path.Combine(directory, CodeFileName), codeLines);

                            WriteScripts(Path.Combine(directory, ScriptsFolder), scripts);

                            string messagesPath = Path.Combine(directory, MessagesFolder);
                            Directory.CreateDirectory(messagesPath);

                            string mes = entry.ConvertMessagesToTxt(Lists.DefaultLanguage);

                            if (!String.IsNullOrWhiteSpace(mes))
                                File.WriteAllText(Path.Combine(messagesPath, DefaultMessagesFileName), mes);

                            if (localization != null)
                            {
                                foreach (var le in localization)
                                {
                                    mes = entry.ConvertMessagesToTxt(le.Language);

                                    if (!String.IsNullOrWhiteSpace(mes))
                                        File.WriteAllText(Path.Combine(messagesPath, $"{SanitizeName(le.Language)}{MessagesExtension}"), mes);
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

                        File.WriteAllText(Path.Combine(directory, NpcJsonFileName), json);

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
                    Console.WriteLine($"{UnpackFailed}: {failure}");
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
                    Console.WriteLine($"{UnpackFailed}: Couldn't write main JSON to temp path: {ex.Message}");
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
                    Console.WriteLine($"{UnpackFailed}: {ex.Message}");
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

        private static void WriteScripts(string folder, List<ScriptEntry> scripts)
        {
            Directory.CreateDirectory(folder);
            if (scripts == null)
                return;

            for (int i = 0; i < scripts.Count; i++)
            {
                string fileName = Prefixed(i, scripts.Count, scripts[i].Name) + ScriptExtension;
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