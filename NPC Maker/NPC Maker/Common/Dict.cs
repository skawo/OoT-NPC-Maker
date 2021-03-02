using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NPC_Maker
{
    public static class Dicts
    {
        public static Dictionary<int, string> ObjectIDs = new Dictionary<int, string>()
        {
            {-1, "Current" },
            {-2, "RAM" },
        };

        public static Dictionary<int, string> LimbShowSubTypes = new Dictionary<int, string>()
        {
            { 0, "Don't show" },
            { 1, "Alongside limb" },
            { 2, "Instead of limb" },
            { 3, "Don't show" },
        };

        public static Dictionary<string, int> SFXes = FileOps.GetDictionary($"{Program.ExecPath}/SFX.csv");
        public static Dictionary<string, int> Music = FileOps.GetDictionary($"{Program.ExecPath}/Music.csv");
        public static Dictionary<string, int> Actors = FileOps.GetDictionary($"{Program.ExecPath}/Actors.csv");

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
        };

        public static int GetIntFromIntStringDict(Dictionary<int, string> Dict, string Value, int? Default = null)
        {
            if (Dict.ContainsValue(Value))
                return Dict.FirstOrDefault(x => x.Value == Value).Key;
            else if (Default == null)
                return Value.IsNumeric() ? Convert.ToInt32(Value) : -1;
            else
                return (int)Default;
        }

        public static string GetStringFromIntStringDict(Dictionary<int, string> Dict, int Value, string Default = null)
        {
            if (Dict.ContainsKey(Value))
                return Dict[Value];
            else if (Default == null)
                return Value.ToString();
            else
                return Default;
        }



    }
}
