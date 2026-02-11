using Newtonsoft.Json;
using NPC_Maker.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static NPC_Maker.Lists;

namespace NPC_Maker
{
    public class BiDictionary
    {
        public Dictionary<string, int> Forward { get; private set; }
        public Dictionary<int, string> Reverse { get; private set; }

        public BiDictionary(Dictionary<string, int> forward)
        {
            Forward = new Dictionary<string, int>(forward);
            Reverse = new Dictionary<int, string>();

            foreach (var pair in forward)
                Reverse[pair.Value] = pair.Key;
        }

        public void Add(string key, int value)
        {
            Forward[key] = value;
            Reverse[value] = key;
        }
    }

    public static class Dicts
    {
        public static Dictionary<string, int> LimbShowSubTypes = new Dictionary<string, int>()
        {
            { "Not visible", 0 },
            { "With limb", 1 },
            { "Replaces limb", 2 },
            { "In skeleton", 3 },
            { "Control existing", 4 },
        };

        public static Dictionary<string, int> LimbIndexSubTypes = new Dictionary<string, int>()
        {
            { "Relative Position", -1 },
            { "Absolute Position", -2 },
            { "At Camera", -3 },
            { "At Display", -4 },
            { "Orthographic", -5 },
            { "Orthographic Wide", -6 },
        };

        public static Dictionary<Lists.DictType, string> DictFilenames = new Dictionary<Lists.DictType, string>()
        {
            { Lists.DictType.SFX, $"Dicts/SFX.csv" },
            { Lists.DictType.Music, $"Dicts/Music.csv" },
            { Lists.DictType.Actors, $"Dicts/Actors.csv" },
            { Lists.DictType.Objects, $"Dicts/Objects.csv" },
            { Lists.DictType.LinkAnims, $"Dicts/LinkAnims.csv" },
        };

        public static string DefaultLanguage = "Default";
        public static string ProjectPathToken = "{PROJECTPATH}";

        public static Dictionary<string, MessageConfig> LanguageDefs;
        public static List<MessageDefinition> MsgDefinitions;

        public static BiDictionary ObjectIDs;
        public static BiDictionary SFXes;
        public static BiDictionary Music;
        public static BiDictionary Actors;
        public static BiDictionary LinkAnims;


        public static Dictionary<Lists.ParticleTypes, List<ParticleSubOptions>> UsableParticleSubOptions = new Dictionary<ParticleTypes, List<ParticleSubOptions>>()
        {
            {ParticleTypes.DUST,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.EXPLOSION,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE} },
            {ParticleTypes.SPARK,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE} },
            {ParticleTypes.BUBBLE,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE,
                                                ParticleSubOptions.DURATION,
                                                ParticleSubOptions.RANDOMIZE_XZ} },
            {ParticleTypes.WATER_SPLASH,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION} },
            {ParticleTypes.SMOKE,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.ICE_CHUNK,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.ICE_BURST,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.RED_FLAME,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.BLUE_FLAME,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.ELECTRICITY,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.FOCUSED_STAR,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.DISPERSED_STAR,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.BURN_MARK,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE,
                                                ParticleSubOptions.OPACITY,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.RING,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE,
                                                ParticleSubOptions.SCALE_UPDATE_DOWN,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.FLAME,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.FIRE_TAIL,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.VARIABLE,
                                                ParticleSubOptions.DURATION} },

            {ParticleTypes.HIT_MARK_FLASH,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.HIT_MARK_DUST,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.HIT_MARK_BURST,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.HIT_MARK_SPARK,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.LIGHT_POINT,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.LIGHTPOINT_COLOR} },
            {ParticleTypes.SCORE,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCORE_AMOUNT} },
            {ParticleTypes.DODONGO_FIRE,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE,
                                                ParticleSubOptions.OPACITY,
                                                ParticleSubOptions.FADE_DELAY,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.FREEZARD_SMOKE,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.SCALE} },
            {ParticleTypes.LIGHTNING,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.COLOR1,
                                                ParticleSubOptions.COLOR2,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.YAW,
                                                ParticleSubOptions.COUNT,
                                                ParticleSubOptions.DURATION} },
            {ParticleTypes.DISPLAY_LIST,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.SCALE,
                                                ParticleSubOptions.SCALE_UPDATE,
                                                ParticleSubOptions.COUNT,
                                                ParticleSubOptions.DURATION,
                                                ParticleSubOptions.DLIST} },
            {ParticleTypes.SEARCH_EFFECT,
                new List<ParticleSubOptions>() {ParticleSubOptions.POSITION,
                                                ParticleSubOptions.VELOCITY,
                                                ParticleSubOptions.ACCELERATION,
                                                ParticleSubOptions.SPOTTED} },
        };

        public static Dictionary<string, string[]> FunctionSubtypes = new Dictionary<string, string[]>()
        {
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.IF), Enum.GetNames(typeof(Lists.IfSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.WHILE), Enum.GetNames(typeof(Lists.IfSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.GET),  Enum.GetNames(typeof(Lists.GetSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.SET), Enum.GetNames(typeof(Lists.SetSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.AWAIT), Enum.GetNames(typeof(Lists.AwaitSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.PLAY), Enum.GetNames(typeof(Lists.PlaySubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.KILL), Enum.GetNames(typeof(Lists.TargetActorSubtypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.ITEM), Enum.GetNames(typeof(Lists.ItemSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.FACE), Lists.FaceSubTypesForCtxMenu.ToArray() },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.SCRIPT),  Enum.GetNames(typeof(Lists.ScriptSubtypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.ROTATION),  Enum.GetNames(typeof(Lists.RotationSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.POSITION),  Enum.GetNames(typeof(Lists.PositionSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.SCALE),  Enum.GetNames(typeof(Lists.ScaleSubTypes)) },
        };

        public static void LoadDicts()
        {
            ReloadDict(DictType.Objects);
            ReloadDict(DictType.LinkAnims);
            ReloadDict(DictType.Actors);
            ReloadDict(DictType.Music);
            ReloadDict(DictType.SFX);
        }

        private static T TryLoadLanguageDict<T>(string Language)
        {
            try
            {
                string FileCheck = "";
                string FolderDef = Path.GetDirectoryName(Program.JsonPath == "" ? Program.ExecPath : Program.JsonPath);
                string Folder = FolderDef;
                string DictFile = $"Dicts/{Language}.json";

                FileCheck = Path.Combine(Folder, DictFile);

                if (!File.Exists(FileCheck))
                    Folder = Program.ExecPath;

                FileCheck = Path.Combine(Folder, DictFile);

                string langDefText = File.ReadAllText(FileCheck);

                var errors = new List<string>();

                var settings = new JsonSerializerSettings
                {
                    Error = (sender, args) =>
                    {
                        if (args.ErrorContext.Error.InnerException != null)
                            errors.Add($"{args.ErrorContext.Error.InnerException.Message}");
                        else
                            errors.Add($"{args.ErrorContext.Error.Message}");

                        args.ErrorContext.Handled = true; // Continue processing
                    }
                };

                T res = JsonConvert.DeserializeObject<T>(langDefText, settings);

                if (errors.Any())
                    throw new JsonException(errors[0]);

                return res;

            }
            catch (JsonException ex)
            {
                throw new Exception($"Error loading language definition for {Language}:\n{ex.Message}");
            }
            catch
            {
                return default;
            }

        }

        private static void ReplaceAndAddTags(MessageConfig first, MessageConfig second)
        {
            var secondTagsDict = second.Entries.ToDictionary(tag => tag.Token, tag => tag);
            var firstTokens = new HashSet<string>();

            first.ExtraFont = second.ExtraFont;
            first.EndMessage = second.EndMessage;
            first.NewLine = second.NewLine;
            first.EndMessageType = second.EndMessageType;
            first.NewLineType = second.NewLineType;
            
            // Replace matching tags and track first config tokens
            for (int i = 0; i < first.Entries.Count; i++)
            {
                string token = first.Entries[i].Token;
                firstTokens.Add(token);

                if (secondTagsDict.TryGetValue(token, out Tag matchingTag))
                {
                    first.Entries[i] = matchingTag;
                }
            }

            // Add unique tags from second config
            var uniqueTags = second.Entries.Where(tag => !firstTokens.Contains(tag.Token));
            first.Entries.AddRange(uniqueTags);
        }

        public static void ReloadLanguages(List<string> languages)
        {
            if (LanguageDefs == null)
                LanguageDefs = new Dictionary<string, MessageConfig>();
            else
                LanguageDefs.Clear();

            if (languages == null)
                languages = new List<string>();

            var allLanguages = new List<string>();
            allLanguages.Add(Dicts.DefaultLanguage);
            allLanguages.AddRange(languages);

            MessageConfig baseConfig =
                TryLoadLanguageDict<MessageConfig>("MessageBase") ?? throw new Exception("Could not load base language definitions.");

            foreach (string language in allLanguages)
            {
                MessageConfig langConfig =
                    TryLoadLanguageDict<MessageConfig>(language);

                if (langConfig == null)
                    continue;

                MessageConfig merged = Helpers.Clone<MessageConfig>(baseConfig);
                ReplaceAndAddTags(merged, langConfig);
                ApplyLanguageOverrides(merged, langConfig);

                // overwrite instead of throwing
                LanguageDefs[language] = merged;
            }

            MsgDefinitions =
                TryLoadLanguageDict<List<MessageDefinition>>("MsgDefs");

            if (LanguageDefs.Count == 0)
                throw new Exception("Could not load any language definitions...");
        }

        private static void ApplyLanguageOverrides(MessageConfig target, MessageConfig source)
        {
            if (source.EndMessage != -1)
                target.EndMessage = source.EndMessage;

            if (source.NewLine != -1)
                target.NewLine = source.NewLine;

            if (!string.IsNullOrWhiteSpace(source.EndMessageType))
                target.EndMessageType = source.EndMessageType;

            if (!string.IsNullOrWhiteSpace(source.NewLineType))
                target.NewLineType = source.NewLineType;
        }

        public static void ReloadSpellcheckDicts(List<string> languages)
        {
            Program.dictionary =
                new Dictionary<string, WeCantSpell.Hunspell.WordList>();

            LoadSpellcheckDictIfExists(Dicts.DefaultLanguage, "dict.dic");
            LoadSpellcheckDictIfExists(Dicts.DefaultLanguage, "Default.dic");

            if (languages == null)
                return;

            foreach (string lang in languages)
            {
                string file = lang + ".dic";
                LoadSpellcheckDictIfExists(lang, file);
            }
        }

        private static void LoadSpellcheckDictIfExists(string key, string file)
        {
            if (!File.Exists(file))
                return;

            Program.dictionary[key] =
                WeCantSpell.Hunspell.WordList.CreateFromFiles(file);
        }

        public static void ReloadDict(Lists.DictType type, bool allowFail = false)
        {
            try
            {
                string basePath = string.IsNullOrEmpty(Program.JsonPath)
                    ? Program.ExecPath
                    : Program.JsonPath;

                string folder = Path.GetDirectoryName(basePath);
                string filename = DictFilenames[type];

                string path = Path.Combine(folder, filename);
                if (!File.Exists(path))
                    path = Path.Combine(Program.ExecPath, filename);

                var dict = FileOps.GetDictionary(path, allowFail);
                BiDictionary dbi = new BiDictionary(dict);

                switch (type)
                {
                    case Lists.DictType.Actors: Actors = dbi; break;
                    case Lists.DictType.SFX: SFXes = dbi; break;
                    case Lists.DictType.Music: Music = dbi; break;
                    case Lists.DictType.Objects: ObjectIDs = dbi; break;
                    case Lists.DictType.LinkAnims: LinkAnims = dbi; break;
                }
            }
            catch (Exception ex)
            {
                if (!allowFail)
                    BigMessageBox.Show(
                        "Error loading the " + type + " dict: " + ex.Message);
            }
        }

        public static string GetStringFromBiDict(BiDictionary dict, int value, string defaultValue = null)
        {
            if (dict.Reverse.TryGetValue(value, out string result))
                return result;

            if (defaultValue != null)
                return defaultValue;

            return value.ToString();
        }
        public static int GetIntFromBiDict(BiDictionary dict, string value, int? defaultValue = null)
        {
            return GetIntFromStringIntDict(dict.Forward, value, defaultValue);
        }

        public static string GetStringFromStringIntDict(Dictionary<string, int> dict, int value, string defaultValue = null)
        {
            foreach (var pair in dict)
            {
                if (pair.Value == value)
                    return pair.Key;
            }

            if (defaultValue != null)
                return defaultValue;

            return value.ToString();
        }


        public static int GetIntFromStringIntDict(Dictionary<string, int> dict, string value, int? defaultValue = null)
        {
            if (dict.TryGetValue(value, out int result))
                return result;

            if (defaultValue.HasValue)
                return defaultValue.Value;

            if (Int32.TryParse(value, out int parsed))
                return parsed;

            return -1;
        }

    }
}
