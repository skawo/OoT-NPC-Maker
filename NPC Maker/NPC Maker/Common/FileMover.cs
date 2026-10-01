using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NPC_Maker.Common
{
    public class FileSwapper
    {
        public class FileSwap
        {
            public readonly string SourcePath;
            public readonly string DestinationPath;
            public string BackupPath;

            public FileSwap(string source, string destination)
            {
                SourcePath = source;
                DestinationPath = destination;
            }
        }

        // Replaces each destination.
        // Existing destinations are first moved to backups; if anything fails,
        // all swaps made so far are rolled back. Backups are deleted only after every swap succeeded.
        public static void SwapFiles(FileSwap[] items)
        {
            string suffix = ".old_" + Guid.NewGuid().ToString("N");
            List<FileSwap> done = new List<FileSwap>();
            HashSet<FileSwap> contentSwapped = new HashSet<FileSwap>();
            HashSet<FileSwap> newContentIn = new HashSet<FileSwap>();

            try
            {
                foreach (FileSwap item in items)
                {
                    try
                    {
                        if (Directory.Exists(item.SourcePath))
                        {
                            if (Directory.Exists(item.DestinationPath))
                            {
                                item.BackupPath = item.DestinationPath + suffix;

                                if (TryRename(item.DestinationPath, item.BackupPath))
                                {
                                    done.Add(item);
                                }
                                else
                                {
                                    // Folder is locked. Move its contents out instead.
                                    contentSwapped.Add(item);
                                    done.Add(item);

                                    DrainContents(item.DestinationPath, item.BackupPath);
                                }
                            }
                            else
                            {
                                done.Add(item);
                            }

                            if (contentSwapped.Contains(item))
                            {
                                HashSet<string> expected = ListRelative(item.SourcePath);

                                newContentIn.Add(item); // destination may now hold new files
                                MergeInto(item.SourcePath, item.DestinationPath);
                                Directory.Delete(item.SourcePath, false);

                                PruneAndVerify(item.DestinationPath, "", expected);
                            }
                            else
                            {
                                MoveDir(item.SourcePath, item.DestinationPath);
                            }
                        }
                        else
                        {
                            if (File.Exists(item.DestinationPath))
                            {
                                item.BackupPath = item.DestinationPath + suffix;
                                MoveFile(item.DestinationPath, item.BackupPath);
                            }

                            // Record before moving so a failed move still restores the backup.
                            done.Add(item);
                            MoveFile(item.SourcePath, item.DestinationPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new IOException($"Writing file failed: {ex.Message}", ex);
                    }
                }
            }
            catch (Exception original)
            {
                List<string> rollbackErrors = new List<string>();

                // Roll back in reverse order.
                for (int i = done.Count - 1; i >= 0; i--)
                {
                    FileSwap item = done[i];
                    try
                    {
                        if (item.BackupPath == null) continue;

                        if (contentSwapped.Contains(item))
                        {
                            if (!Directory.Exists(item.DestinationPath))
                                Directory.CreateDirectory(item.DestinationPath);
                            else if (newContentIn.Contains(item))
                                ClearContents(item.DestinationPath);

                            MergeInto(item.BackupPath, item.DestinationPath);
                            Directory.Delete(item.BackupPath, false);
                        }
                        else
                        {
                            if (Directory.Exists(item.DestinationPath))
                                Directory.Delete(item.DestinationPath, true);
                            else if (File.Exists(item.DestinationPath))
                                File.Delete(item.DestinationPath);

                            if (Directory.Exists(item.BackupPath))
                                Directory.Move(item.BackupPath, item.DestinationPath);
                            else if (File.Exists(item.BackupPath))
                                File.Move(item.BackupPath, item.DestinationPath);
                        }
                    }
                    catch (Exception rex)
                    {
                        rollbackErrors.Add($"'{item.DestinationPath}': {rex.GetType().Name}: {rex.Message}");
                    }
                }

                if (rollbackErrors.Count > 0)
                {
                    throw new IOException(
                        original.Message + Environment.NewLine +
                        "Rollback also failed (backups are kept next to the destinations as '*" + suffix + "'): " +
                        Environment.NewLine + string.Join(Environment.NewLine, rollbackErrors),
                        original);
                }

                throw;
            }

            // Success: discard the old versions.
            foreach (FileSwap item in done)
            {
                if (item.BackupPath == null) continue;

                try
                {
                    if (Directory.Exists(item.BackupPath))
                        Directory.Delete(item.BackupPath, true);
                    else if (File.Exists(item.BackupPath))
                        File.Delete(item.BackupPath);
                }
                catch
                {
                    // If this fails, whatever.
                }
            }
        }

        private static void MoveFile(string from, string to)
        {
            try
            {
                File.Move(from, to);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Could not move file {from}': {ex.Message}", ex);
            }
        }

        private static void MoveDir(string from, string to)
        {
            try
            {
                Directory.Move(from, to);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Could not move folder {from}': {ex.Message}", ex);
            }
        }

        // Returns false if the folder can't be renamed (e.g. open in Explorer).
        private static bool TryRename(string from, string to)
        {
            try
            {
                Directory.Move(from, to);
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                return false;
            }
        }

        // Moves everything out of 'from' into 'to'. Locked subfolders are emptied, not moved,
        // so they may remain as empty folders. Verifies no files are left anywhere under 'from'.
        // Repeats only to catch files that appear while working; any file move failure throws.
        private static void DrainContents(string from, string to)
        {
            for (int pass = 0; pass < 5; pass++)
            {
                DrainPass(from, to);

                if (!Directory.EnumerateFiles(from, "*", SearchOption.AllDirectories).Any())
                    return;
            }

            List<string> left = Directory.EnumerateFiles(from, "*", SearchOption.AllDirectories).Take(6).ToList();
            string list = string.Join(", ", left.Take(5).Select(f => $"'{f}'")) + (left.Count > 5 ? ", ..." : "");

            throw new IOException($"Could not empty '{from}'");
        }

        private static void DrainPass(string from, string to)
        {
            Directory.CreateDirectory(to);

            foreach (string entry in Directory.GetFileSystemEntries(from))
            {
                string target = Path.Combine(to, Path.GetFileName(entry));

                if (Directory.Exists(entry))
                {
                    if (!Directory.Exists(target) && TryRename(entry, target))
                        continue;

                    // Locked (or already partly drained): empty it and leave the folder itself.
                    DrainPass(entry, target);
                }
                else
                {
                    MoveFile(entry, target);
                }
            }
        }

        // Moves 'from' into 'to', merging into folders that already exist in 'to'.
        // Used for the new content, and for restoring the backup on rollback.
        private static void MergeInto(string from, string to)
        {
            Directory.CreateDirectory(to);

            foreach (string entry in Directory.GetFileSystemEntries(from))
            {
                string target = Path.Combine(to, Path.GetFileName(entry));

                if (Directory.Exists(entry))
                {
                    if (Directory.Exists(target))
                    {
                        MergeInto(entry, target);
                        Directory.Delete(entry, false);
                    }
                    else
                    {
                        MoveDir(entry, target);
                    }
                }
                else
                {
                    MoveFile(entry, target);
                }
            }
        }

        // Deletes everything under 'dir', keeping folders that can't be deleted.
        private static void ClearContents(string dir)
        {
            foreach (string entry in Directory.GetFileSystemEntries(dir))
            {
                if (Directory.Exists(entry))
                {
                    ClearContents(entry);

                    try { Directory.Delete(entry, false); } catch { }
                }
                else
                {
                    File.Delete(entry);
                }
            }
        }

        // Relative paths of every file and folder under 'root'.
        private static HashSet<string> ListRelative(string root)
        {
            int len = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length + 1;

            return new HashSet<string>(
                Directory.EnumerateFileSystemEntries(root, "*", SearchOption.AllDirectories)
                    .Select(p => p.Substring(len)),
                StringComparer.OrdinalIgnoreCase);
        }

        // Throws on any file that isn't part of the new content; removes leftover empty folders.
        private static void PruneAndVerify(string dir, string rel, HashSet<string> expected)
        {
            foreach (string entry in Directory.GetFileSystemEntries(dir))
            {
                string name = Path.GetFileName(entry);
                string r = rel.Length == 0 ? name : rel + Path.DirectorySeparatorChar + name;

                if (Directory.Exists(entry))
                {
                    PruneAndVerify(entry, r, expected);

                    if (!expected.Contains(r))
                    {
                        // Locked: stays as an empty folder.
                        try 
                        { 
                            Directory.Delete(entry, false); 
                        } 
                        catch 
                        { }
                    }
                }
                else if (!expected.Contains(r))
                {
                    throw new IOException($"Unexpected file '{entry}' appeared in destination.");
                }
            }
        }
    }
}