using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NPC_Maker.NewScriptParser
{
    public static class ScriptHelpers
    {
        static public string ReplaceExpr(this string Orig, string Expr, string Replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            string Pattern = String.Format(@"\b{0}\b", Regex.Escape(Expr));
            return Regex.Replace(Orig, Pattern, Replacement, regexOptions);
        }

        public static void ErrorIfNumParamsNotEq(string[] Splitline, int Number)
        {
            if (Splitline.Count() != Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsSmaller(string[] Splitline, int Number)
        {
            if (Splitline.Count() < Number)
                throw ParseException.ParamCountSmall(Splitline);
        }

        public static void ErrorIfNumParamsBigger(string[] Splitline, int Number)
        {
            if (Splitline.Count() > Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static byte GetBoolConditionID(string Condition)
        {
            switch (Condition.ToUpper())
            {
                case Lists.Keyword_True: return 0;
                case Lists.Keyword_False: return 1;
                default: return byte.MaxValue;
            }
        }

        public static byte GetConditionID(string Condition)
        {
            switch (Condition)
            {
                case "=": return 0;
                case "==": return 0;
                case "<": return 1;
                case ">": return 2;
                case "<=": return 3;
                case ">=": return 4;
                case "!=": return 5;
                case "<>": return 5;

                default: return byte.MaxValue;
            }
        }

        public static byte GetVariable(string Variable)
        {
            switch (Variable)
            {
                case Lists.Keyword_RNG: return (int)Lists.VarTypes.RNG;
                case Lists.Keyword_ScriptVar1: return (int)Lists.VarTypes.Var1;
                case Lists.Keyword_ScriptVar2: return (int)Lists.VarTypes.Var2;
                case Lists.Keyword_ScriptVar3: return (int)Lists.VarTypes.Var3;
                case Lists.Keyword_ScriptVar4: return (int)Lists.VarTypes.Var4;
                case Lists.Keyword_ScriptVar5: return (int)Lists.VarTypes.Var5;
                default: return 0;
            }
        }

        public static UInt32? Helper_ConvertToUInt32(string Number)
        {
            try
            {
                if (Number.Length >= 3 && Number.Substring(0, 2) == "0x")
                    return Convert.ToUInt32(Number, 16);
                else
                    return Convert.ToUInt32(Number);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Int32? Helper_ConvertToInt32(string Number)
        {
            try
            {
                if (Number.Length >= 3 && Number.Substring(0, 2) == "0x")
                    return Convert.ToInt32(Number, 16);
                else
                    return Convert.ToInt32(Number);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UInt32? Helper_GetEnumByName(Type EnumType, string Name)
        {
            try
            {
                return Convert.ToUInt32(System.Enum.Parse(EnumType, Name.ToUpper()));
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static UInt32? Helper_GetAnimationID(string AnimName, List<AnimationEntry> Animations)
        {
            for (int i = 0; i < Animations.Count; i++)
            {
                if (AnimName.ToLower() == Animations[i].Name.Replace(" ", "").ToLower())
                    return (UInt32)i;
            }

            return null;
        }

        public static UInt32? Helper_GetSFXId(string SFXName)
        {
            try
            {
                return (UInt32?)Lists.SFXes[SFXName.ToUpper()];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UInt32? Helper_GetMusicId(string MusicName)
        {
            try
            {
                return (UInt32?)Lists.Music[MusicName.ToUpper()];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Int32? Helper_GetTextureID(string TextureName, int Segment, List<List<TextureEntry>> Textures)
        {
            for (int i = 0; i < Textures[Segment].Count; i++)
            {
                if (TextureName.ToLower() == Textures[Segment][i].Name.Replace(" ", "").ToLower())
                    return i;
            }

            return null;
        }

        public static Int32? Helper_GetDListID(string DlistName, List<DListEntry> DLists)
        {
            for (int i = 0; i < DLists.Count; i++)
            {
                if (DlistName.ToLower() == DLists[i].Name.Replace(" ", "").ToLower())
                    return i;
            }

            return null;
        }
    }
}
