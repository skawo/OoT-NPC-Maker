using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NPC_Maker.Common
{
    public enum EntryKind
    {
        Both,
        FilesOnly,
        DirectoriesOnly
    }

    public class FolderEntry
    {
        public string FullPath { get; private set; }
        public string Name { get; private set; }   // prefix removed

        public FolderEntry(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
        }
    }

    public static class FolderLister
    {
        private static readonly Regex Prefix =
            new Regex(@"^\s*(\d+)\s*-\s*(.*)$", RegexOptions.Compiled);

        private static IEnumerable<FileSystemInfo> GetEntries(string folder, EntryKind kind)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);

            switch (kind)
            {
                case EntryKind.FilesOnly:
                    return dir.EnumerateFiles().Cast<FileSystemInfo>();
                case EntryKind.DirectoriesOnly:
                    return dir.EnumerateDirectories().Cast<FileSystemInfo>();
                default:
                    return dir.EnumerateFileSystemInfos();
            }
        }

        // Get files/folders sorted by numeric prefix ("12-Name"), with full path and prefix-stripped name.
        public static List<FolderEntry> ListSortedWithPaths(string folder, EntryKind kind = EntryKind.Both)
        {
            return GetEntries(folder, kind)
                .Select(info => new { Info = info, Match = Prefix.Match(info.Name) })
                .OrderBy(x => x.Match.Success ? 0 : 1)                                    // prefixed first
                .ThenBy(x => x.Match.Success ? long.Parse(x.Match.Groups[1].Value) : 0L)  // numeric order
                .ThenBy(x => x.Info.Name, StringComparer.OrdinalIgnoreCase)               // tie-breaker / unprefixed
                .Select(x => new FolderEntry(
                    x.Info.FullName,
                    x.Match.Success ? x.Match.Groups[2].Value : x.Info.Name))
                .ToList();
        }

        public static List<string> ListSortedStripped(string folder, EntryKind kind = EntryKind.Both)
        {
            return ListSortedWithPaths(folder, kind).Select(e => e.Name).ToList();
        }

        public static List<string> ListFilesSortedStripped(string folder)
        {
            return ListSortedStripped(folder, EntryKind.FilesOnly);
        }

        public static List<string> ListDirectoriesSortedStripped(string folder)
        {
            return ListSortedStripped(folder, EntryKind.DirectoriesOnly);
        }
    }
}
