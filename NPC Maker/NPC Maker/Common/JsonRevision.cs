using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public partial class FileOps
    {
        private static int MigrateToVersion2(ref NPCFile npcFile, JObject jsonObject, int currentVersion)
        {
            if (currentVersion >= 2) 
                return currentVersion;

            npcFile.Version = 2;

            for (int i = 0; i < npcFile.Entries.Count; i++)
            {
                var scriptToken = jsonObject.SelectToken($"Entries[{i}].Script");
                var script2Token = jsonObject.SelectToken($"Entries[{i}].Script2");

                npcFile.Entries[i].Scripts.AddRange(new[]
                {
                    new ScriptEntry { Text = scriptToken?.ToString() ?? string.Empty, Name = "Script 1" },
                    new ScriptEntry { Text = script2Token?.ToString() ?? string.Empty, Name = "Script 2" }
                });
            }

            return 2;
        }

        private static int MigrateToVersion3(ref NPCFile npcFile, int currentVersion)
        {
            if (currentVersion >= 3) 
                return currentVersion;

            npcFile.Version = 3;

            foreach (var entry in npcFile.Entries)
            {
                entry.FileStart = 0;

                entry.Animations.ForEach(anim => anim.FileStart = -1);
                entry.ExtraDisplayLists.ForEach(dlist => dlist.FileStart = -1);

                foreach (var segment in entry.Segments)
                    segment.ForEach(segEntry => segEntry.FileStart = -1);
            }

            return 3;
        }

        private static void ProcessTextLinesForVersion4Plus(ref NPCFile npcFile)
        {
            foreach (var entry in npcFile.Entries)
            {
                foreach (var script in entry.Scripts)
                    script.Text = string.Join(Environment.NewLine, script.TextLines.Select(x => x.TrimEnd()));

                entry.EmbeddedOverlayCode.Code = string.Join(Environment.NewLine, entry.EmbeddedOverlayCode.CodeLines);
            }

            foreach (var globalHeader in npcFile.GlobalHeaders)
                globalHeader.Text = string.Join(Environment.NewLine, globalHeader.TextLines.Select(x => x.TrimEnd()));
        }

        private static void ProcessMessageLinesForVersion5Plus(ref NPCFile npcFile)
        {
            foreach (var entry in npcFile.Entries)
            {
                ProcessMessageEntries(entry.Messages);

                foreach (var localization in entry.Localization)
                    ProcessMessageEntries(localization.Messages);
            }
        }

        private static void ProcessMessageEntries(IEnumerable<MessageEntry> messages)
        {
            foreach (var message in messages)
                message.MessageText = string.Join(Environment.NewLine, message.MessageTextLines);
        }

        private static int MigrateToVersion6(ref NPCFile npcFile, int currentVersion)
        {
            if (currentVersion >= 6) 
                return currentVersion;

            foreach (var entry in npcFile.Entries)
            {
                var embeddedCode = entry.EmbeddedOverlayCode;

                for (int i = 0; i < 5; i++)
                {
                    int functionIndex = embeddedCode.FuncsRunWhen[i, 0];

                    embeddedCode.SetFuncNames[i] = functionIndex >= 0 && functionIndex < embeddedCode.Functions.Count
                        ? embeddedCode.Functions[functionIndex].FuncName
                        : "Not found?";
                }
            }

            return 6;
        }

        private static int MigrateToVersion7(ref NPCFile npcFile, int currentVersion)
        {
            if (currentVersion >= 7) 
                return currentVersion;

            foreach (var entry in npcFile.Entries)
            {
                var embeddedCode = entry.EmbeddedOverlayCode;

                // Expand SetFuncNames array
                var expandedFuncNames = embeddedCode.SetFuncNames.ToList();
                expandedFuncNames.Add(string.Empty);
                embeddedCode.SetFuncNames = expandedFuncNames.ToArray();

                // Expand FuncsRunWhen array
                var newFuncsRunWhen = new int[6, 2];
                for (int i = 0; i < 6; i++)
                {
                    newFuncsRunWhen[i, 0] = i < 5 ? embeddedCode.FuncsRunWhen[i, 0] : -1;
                    newFuncsRunWhen[i, 1] = i < 5 ? embeddedCode.FuncsRunWhen[i, 1] : -1;
                }
                embeddedCode.FuncsRunWhen = newFuncsRunWhen;
            }

            return 7;
        }

    }
}
