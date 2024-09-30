using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static NPC_Maker.Lists;

namespace NPC_Maker
{
    public static class Dicts
    {
        public static Dictionary<string, int> LimbShowSubTypes = new Dictionary<string, int>()
        {
            { "Not visible", 0 },
            { "With limb", 1 },
            { "Replaces limb", 2 },
            { "In Skeleton", 3 },
            { "Control existing", 4 },
        };

        public static Dictionary<Lists.DictType, string> DictFilenames = new Dictionary<Lists.DictType, string>()
        {
            { Lists.DictType.SFX, $"Dicts/SFX.csv" },
            { Lists.DictType.Music, $"Dicts/Music.csv" },
            { Lists.DictType.Actors, $"Dicts/Actors.csv" },
            { Lists.DictType.Objects, $"Dicts/Objects.csv" },
            { Lists.DictType.LinkAnims, $"Dicts/LinkAnims.csv" },
        };

        public static Dictionary<string, int> ObjectIDs;
        public static Dictionary<string, int> SFXes;
        public static Dictionary<string, int> Music;
        public static Dictionary<string, int> Actors;
        public static Dictionary<string, int> LinkAnims;
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

        public static Dictionary<Lists.MsgControlCode, string> MessageControlCodes = PopulateCodeDictionary();

        public static void LoadDicts()
        {
            ReloadDict(DictType.Objects);
            ReloadDict(DictType.LinkAnims);
            ReloadDict(DictType.Actors);
            ReloadDict(DictType.Music);
            ReloadDict(DictType.SFX);
        }


        public static void ReloadDict(Lists.DictType Type)
        {
            try
            {
                string FileCheck = "";
                string Folder = Path.GetDirectoryName(Program.JsonPath == "" ? Program.ExecPath : Program.JsonPath);

                switch (Type)
                {
                    case Lists.DictType.Actors: FileCheck = Path.Combine(Folder, DictFilenames[Lists.DictType.Actors]); break;
                    case Lists.DictType.SFX: FileCheck = Path.Combine(Folder, DictFilenames[Lists.DictType.SFX]); break;
                    case Lists.DictType.Music: FileCheck = Path.Combine(Folder, DictFilenames[Lists.DictType.Music]); break;
                    case Lists.DictType.Objects: FileCheck = Path.Combine(Folder, DictFilenames[Lists.DictType.Objects]); break;
                    case Lists.DictType.LinkAnims: FileCheck = Path.Combine(Folder, DictFilenames[Lists.DictType.LinkAnims]); break;
                    default: break;
                }

                if (!File.Exists(FileCheck))
                    Folder = Program.ExecPath;

                switch (Type)
                {
                    case Lists.DictType.Actors: Actors = FileOps.GetDictionary(Path.Combine(Folder, DictFilenames[Lists.DictType.Actors])); break;
                    case Lists.DictType.SFX: SFXes = FileOps.GetDictionary(Path.Combine(Folder, DictFilenames[Lists.DictType.SFX])); break;
                    case Lists.DictType.Music: Music = FileOps.GetDictionary(Path.Combine(Folder, DictFilenames[Lists.DictType.Music])); break;
                    case Lists.DictType.Objects: ObjectIDs = FileOps.GetDictionary(Path.Combine(Folder, DictFilenames[Lists.DictType.Objects])); break;
                    case Lists.DictType.LinkAnims: LinkAnims = FileOps.GetDictionary(Path.Combine(Folder, DictFilenames[Lists.DictType.LinkAnims])); break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error loading the {Type.ToString()} dict: {ex.Message}");
                return;
            }
        }

        public static string GetStringFromStringIntDict(Dictionary<string, int> Dict, int Value, string Default = null)
        {
            if (Dict.ContainsValue(Value))
                return Dict.FirstOrDefault(x => x.Value == Value).Key;
            else if (Default == null)
                return Value.ToString();
            else
                return Default;
        }

        public static int GetIntFromStringIntDict(Dictionary<string, int> Dict, string Value, int? Default = null)
        {
            if (Dict.ContainsKey(Value))
                return Dict[Value];
            else if (Default == null)
                return Value.IsNumeric() ? Convert.ToInt32(Value) : -1;
            else
                return (int)Default;
        }


        private static Dictionary<Lists.MsgControlCode, string> PopulateCodeDictionary()
        {
            Dictionary<Lists.MsgControlCode, string> output = new Dictionary<Lists.MsgControlCode, string>();

            foreach (Lists.MsgControlCode code in Enum.GetValues(typeof(Lists.MsgControlCode)))
                output.Add(code, code.ToString().Replace("_", " "));

            return output;
        }
    }
}
