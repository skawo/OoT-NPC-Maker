using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class Dicts
    {
        public static Dictionary<string, int> LimbShowSubTypes = new Dictionary<string, int>()
        {
            { "Don't show", 0 },
            { "Alongside limb", 1 },
            { "Instead of limb", 2 },
        };

        public static Dictionary<string, int> LimbDrawTypes = new Dictionary<string, int>()
        {
            { "OPA", 0 },
            { "XLU", 1 },
        };

        public static Dictionary<Lists.DictType, string> DictFilenames = new Dictionary<Lists.DictType, string>()
        {
            { Lists.DictType.SFX, $"{Program.ExecPath}/SFX.csv" },
            { Lists.DictType.Music, $"{Program.ExecPath}/Music.csv" },
            { Lists.DictType.Actors, $"{Program.ExecPath}/Actors.csv" },
            { Lists.DictType.Objects, $"{Program.ExecPath}/Objects.csv" },
            { Lists.DictType.ActorCategories, $"{Program.ExecPath}/ActorCategories.csv" },
        };

        public static Dictionary<string, int> ObjectIDs = FileOps.GetDictionary(DictFilenames[Lists.DictType.Objects]);
        public static Dictionary<string, int> SFXes = FileOps.GetDictionary(DictFilenames[Lists.DictType.SFX]);
        public static Dictionary<string, int> Music = FileOps.GetDictionary(DictFilenames[Lists.DictType.Music]);
        public static Dictionary<string, int> Actors = FileOps.GetDictionary(DictFilenames[Lists.DictType.Actors]);
        public static Dictionary<string, int> ActorCategories = FileOps.GetDictionary(DictFilenames[Lists.DictType.ActorCategories]);

        public static Dictionary<string, string[]> FunctionSubtypes = new Dictionary<string, string[]>()
        {
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.IF), Enum.GetNames(typeof(Lists.IfSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.WHILE), Enum.GetNames(typeof(Lists.IfSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.SET), Enum.GetNames(typeof(Lists.SetSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.AWAIT), Enum.GetNames(typeof(Lists.AwaitSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.PLAY), Enum.GetNames(typeof(Lists.PlaySubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.KILL), Enum.GetNames(typeof(Lists.TargetActorSubtypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.CHANGE_SCRIPT), Enum.GetNames(typeof(Lists.ScriptChangeSubtypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.ITEM), Enum.GetNames(typeof(Lists.ItemSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.ROTATION), Enum.GetNames(typeof(Lists.RotationSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.POSITION), Enum.GetNames(typeof(Lists.PositionSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.FACE), Lists.FaceSubTypesForCtxMenu.ToArray() },
        };

        public static void ReloadDict(Lists.DictType Type)
        {
            switch (Type)
            {
                case Lists.DictType.Actors: ObjectIDs = FileOps.GetDictionary(DictFilenames[Lists.DictType.Actors]); break;
                case Lists.DictType.SFX: SFXes = FileOps.GetDictionary(DictFilenames[Lists.DictType.SFX]); break;
                case Lists.DictType.Music: Music = FileOps.GetDictionary(DictFilenames[Lists.DictType.Music]); break;
                case Lists.DictType.Objects: ObjectIDs = FileOps.GetDictionary(DictFilenames[Lists.DictType.Objects]); break;
                case Lists.DictType.ActorCategories: ActorCategories = FileOps.GetDictionary(DictFilenames[Lists.DictType.ActorCategories]); break;
                default: break;
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
    }
}
