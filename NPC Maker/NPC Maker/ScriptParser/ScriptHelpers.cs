﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NPC_Maker.Scripts
{
    public static class ScriptHelpers
    {
        static public int GetCorresponding(List<string> Lines, int LineNo, string Statement, string EndStatement)
        {
            for (int i = LineNo + 1; i < Lines.Count; i++)
            {
                int spaceIndex = Lines[i].IndexOf(' ');
                string firstWord = spaceIndex >= 0 ? Lines[i].Substring(0, spaceIndex) : Lines[i];

                if (String.Equals(firstWord, Statement, StringComparison.OrdinalIgnoreCase))
                {
                    i = GetCorresponding(Lines, i, Statement, EndStatement);
                    if (i < 0)
                        throw ParseException.StatementNotClosed(Lines[LineNo]);
                }
                else if (String.Equals(firstWord, EndStatement, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        static public string ReplaceExpr(this string Orig, string Expr, string Replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            string pattern = $@"\b{Regex.Escape(Expr)}\b";
            Regex regex = new Regex(pattern, regexOptions);
            return regex.Replace(Orig, Replacement);
        }

        static public string ReplaceExprAndEscaped(this string Orig, string Expr, string Replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            Orig = Orig.Replace($"${Expr}$", Replacement);
            string pattern = $@"\b{Regex.Escape(Expr)}\b";
            Regex regex = new Regex(pattern, regexOptions);
            return regex.Replace(Orig, Replacement);
        }

        static public string ReplaceFirstExpr(this string Orig, string Expr, string Replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            string pattern = $@"\b{Regex.Escape(Expr)}\b";
            Regex regex = new Regex(pattern, regexOptions);
            return regex.Replace(Orig, Replacement, 1);
        }

        public static void ErrorIfNumParamsNotEq(string[] Splitline, int Number)
        {
            if (Splitline.Count() != Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsSmaller(string[] Splitline, int Number)
        {
            if (Splitline.Count() < Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsBigger(string[] Splitline, int Number)
        {
            if (Splitline.Count() > Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsNotBetween(string[] Splitline, int Min, int Max)
        {
            int count = Splitline.Count();

            if (count < Min || count > Max)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static int GetSubIDValue(string[] SplitLine, Type SubTypeEnum, int Index = 1)
        {
            string SubIdText = SplitLine[Index].ToUpper();

            if (Enum.IsDefined(SubTypeEnum, SubIdText))
                return (int)Enum.Parse(SubTypeEnum, SubIdText);
            else
                return -1;
        }

        public static UInt16 GetOcarinaTime(string[] SplitLine, int Index)
        {
            try
            {
                return Helpers.GetOcarinaTime(SplitLine[Index]);
            }
            catch (Exception)
            {
                throw ParseException.BadTime(SplitLine);
            }
        }

        public static object GetValueAndCheckRangeInt(string[] Splitstring, int Index, UInt32 Min, UInt32 Max)
        {
            UInt32? Value;

            try
            {
                if (IsHex(Splitstring[Index]))
                {
                    string str = Splitstring[Index];

                    if (str.StartsWith("-"))
                    {
                        str = str.Substring(1);
                        throw ParseException.ParamOutOfRange(Splitstring);
                    }

                    Value = (UInt32?)Convert.ToInt32(str, 16);
                }
                else
                    Value = (UInt32?)Convert.ToInt32(Splitstring[Index]);

                if (Value == null)
                    throw ParseException.ParamConversionError(Splitstring);

                if (Value < Min || Value > Max)
                    throw ParseException.ParamOutOfRange(Splitstring);

                return Value;
            }
            catch (Exception)
            {
                throw ParseException.ParamConversionError(Splitstring);
            }
        }

        public static object GetValueAndCheckRange(string[] Splitstring, int Index, float Min, float Max)
        {
            float? Value = null;

            try
            {
                if (IsHex(Splitstring[Index]))
                {
                    string str = Splitstring[Index];
                    bool neg = false;

                    if (str.StartsWith("-"))
                    {
                        str = str.Substring(1);
                        neg = true;
                    }

                    Value = (float?)Convert.ToDecimal(Convert.ToInt32(str, 16));

                    if (neg)
                        Value = -Value;

                }
                else
                    Value = (float?)Convert.ToDecimal(Splitstring[Index]);
            }
            catch (Exception)
            {
            }

            if (Value == null)
                throw ParseException.ParamConversionError(Splitstring);

            if (Value < Min || Value > Max)
                throw ParseException.ParamOutOfRange(Splitstring);

            return Value;
        }

        public static byte GetOperator(string[] SplitLine, int Index)
        {
            string operatorString = SplitLine[Index].ToUpper();

            switch (operatorString)
            {
                case "=": return 0;
                case "-=": return 1;
                case "+=": return 2;
                case "/=": return 3;
                case "*=": return 4;
                default: throw ParseException.UnrecognizedOperator(SplitLine);
            }
        }

        public static Int32 Rot2Deg(Int32 Degrees)
        {
            return (Int32)(32768.0f / 180.0f) * Degrees;
        }

        public static void GetXYZ(string[] SplitLine, int XIndex, int YIndex, int ZIndex, ref ScriptVarVal X, ref ScriptVarVal Y, ref ScriptVarVal Z, int Min, int Max)
        {
            X = GetScriptVarVal(SplitLine, XIndex, Min, Max);
            Y = GetScriptVarVal(SplitLine, YIndex, Min, Max);
            Z = GetScriptVarVal(SplitLine, ZIndex, Min, Max);
        }

        public static void GetXYZPos(string[] SplitLine, int XIndex, int YIndex, int ZIndex, ref ScriptVarVal XPos, ref ScriptVarVal YPos, ref ScriptVarVal ZPos)
        {
            XPos = GetScriptVarVal(SplitLine, XIndex, float.MinValue, float.MaxValue);
            YPos = GetScriptVarVal(SplitLine, YIndex, float.MinValue, float.MaxValue);
            ZPos = GetScriptVarVal(SplitLine, ZIndex, float.MinValue, float.MaxValue);
        }

        public static void GetRGBorRGBA(string[] SplitLine, int StartIndex, ref ScriptVarVal R, ref ScriptVarVal G, ref ScriptVarVal B, ref ScriptVarVal A)
        {
            R = ScriptHelpers.GetScriptVarVal(SplitLine, StartIndex + 0, 0, 255);
            G = ScriptHelpers.GetScriptVarVal(SplitLine, StartIndex + 1, 0, 255);
            B = ScriptHelpers.GetScriptVarVal(SplitLine, StartIndex + 2, 0, 255);

            if (A != null)
                A = ScriptHelpers.GetScriptVarVal(SplitLine, StartIndex + 3, 0, 255);
        }

        public static ScriptVarVal GetScriptVarVal(string[] SplitLine, int Index, float Min, float Max)
        {
            var outv = new ScriptVarVal();

            outv.Vartype = ScriptHelpers.GetVarType(SplitLine, Index);
            outv.Value = ScriptHelpers.GetValueByType(SplitLine, Index, outv.Vartype, Min, Max);

            return outv;
        }

        public static ScriptVarVal GetScriptExtVarVal(string[] SplitLine, int Index, float Min, float Max)
        {
            var outv = new ScriptVarVal();

            byte Vartype = ScriptHelpers.GetVarType(SplitLine, Index);

            if (Vartype > (byte)Lists.VarTypes.NORMAL && Vartype < (byte)Lists.VarTypes.VAR)
                throw ParseException.ParamOutOfRange(SplitLine);

            outv.Value = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, Index, Vartype, Min, Max));

            if ((byte)outv.Value < 1 || (byte)outv.Value > Lists.Num_User_Vars)
                throw ParseException.ParamOutOfRange(SplitLine);

            outv.Vartype = Vartype;

            return outv;
        }

        public static void GetScriptVarVal(string[] SplitLine, int Index, float Min, float Max, ref object Value, ref byte Type)
        {
            Type = ScriptHelpers.GetVarType(SplitLine, Index);
            Value = ScriptHelpers.GetValueByType(SplitLine, Index, Type, Min, Max);
        }

        public static void GetScriptVarVal(string[] SplitLine, int Index, float Min, float Max, ref ScriptVarVal scv)
        {
            scv.Vartype = ScriptHelpers.GetVarType(SplitLine, Index);
            scv.Value = ScriptHelpers.GetValueByType(SplitLine, Index, scv.Vartype, Min, Max);
        }

        public static ScriptVarVal GetScriptVarVal(string[] SplitLine, int Index, Type EnumType, ParseException Throw)
        {
            var outv = new ScriptVarVal();

            outv.Vartype = ScriptHelpers.GetVarType(SplitLine, Index);
            outv.Value = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, Index, outv.Vartype, EnumType, Throw);

            return outv;
        }

        public static void GetScriptVarVal(string[] SplitLine, int Index, Type EnumType, ParseException Throw, ref object Value, ref byte Type)
        {
            Type = ScriptHelpers.GetVarType(SplitLine, Index);
            Value = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, Index, Type, EnumType, Throw);
        }

        public static void GetScriptVarVal(string[] SplitLine, int Index, Type EnumType, ParseException Throw, ref ScriptVarVal scv)
        {
            scv.Vartype = ScriptHelpers.GetVarType(SplitLine, Index);
            scv.Value = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, Index, scv.Vartype, EnumType, Throw);
        }

        public static float GetNormalVar(string[] SplitLine, int Index, float Min, float Max)
        {
            string varString = SplitLine[Index].ToUpper();

            try
            {
                if (varString.StartsWith(Lists.Keyword_Degree))
                {
                    string[] s = SplitLine[Index].Split('_');
                    float value = (float)ScriptHelpers.GetValueAndCheckRange(s, 1, Min, Max);
                    float degrees = (float)Rot2Deg(Convert.ToInt32(value));

                    if (degrees < Min || degrees > Max)
                        throw ParseException.ParamOutOfRange(SplitLine);

                    return degrees;

                }
                else if (varString.StartsWith(Lists.Keyword_Music))
                {
                    return (float)Dicts.Music[varString.Replace(Lists.Keyword_Music, "")];
                }
                else if (varString.StartsWith(Lists.Keyword_Actor))
                {
                    return (float)Dicts.Actors[varString.Replace(Lists.Keyword_Actor, "")];
                }
                else if (varString.StartsWith(Lists.Keyword_Sfx))
                {
                    return (float)Dicts.SFXes[varString.Replace(Lists.Keyword_Sfx, "")];
                }
                else
                {
                    try
                    {
                        DataTable table = new DataTable();
                        var res = table.Compute(SplitLine[Index], null).ToString();

                        if (res != null)
                            SplitLine[Index] = res;
                    }
                    catch (Exception)
                    {

                    }

                    
                    return (float)ScriptHelpers.GetValueAndCheckRange(SplitLine, Index, Min, Max);

                }
            }
            catch (ParseException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw ParseException.UnrecognizedParameter(SplitLine);
            }
        }

        public static object GetValueByType(string[] SplitLine, int Index, int VarType, float Min, float Max)
        {
            switch (VarType)
            {
                case (int)Lists.VarTypes.RANDOM:
                    {
                        if (SplitLine[Index].Split(' ').Length != 1)
                            throw ParseException.ParamConversionError(SplitLine);

                        string[] Values = SplitLine[Index].Split('.').Last().Split('>');

                        if (!Values[0].EndsWith("-") || Values.Length != 2)
                            throw ParseException.UnrecognizedParameter(SplitLine);

                        Values[0] = Values[0].Substring(0, Values[0].Length - 1);

                        float minValue = (float)Min < Int16.MinValue ? Int16.MinValue : Min;
                        float maxValue = (float)Max > Int16.MaxValue ? Int16.MaxValue : Max;

                        Int16 MinV = Convert.ToInt16(GetNormalVar(Values, 0, minValue, maxValue));
                        Int16 MaxV = Convert.ToInt16(GetNormalVar(Values, 1, minValue, maxValue));

                        return Helpers.TwoInt16ToWord(MinV, MaxV);
                    }

                case (int)Lists.VarTypes.ACTOR8:
                case (int)Lists.VarTypes.ACTOR16:
                case (int)Lists.VarTypes.ACTOR32:
                case (int)Lists.VarTypes.ACTORF:
                case (int)Lists.VarTypes.GLOBAL8:
                case (int)Lists.VarTypes.GLOBAL16:
                case (int)Lists.VarTypes.GLOBAL32:
                case (int)Lists.VarTypes.GLOBALF:
                case (int)Lists.VarTypes.SAVE8:
                case (int)Lists.VarTypes.SAVE16:
                case (int)Lists.VarTypes.SAVE32:
                case (int)Lists.VarTypes.SAVEF:
                    {
                        string[] Values = SplitLine[Index].Split('.');
                        return Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRangeInt(Values, 1, 0, 0x88000000));
                    }
                case (int)Lists.VarTypes.VAR:
                case (int)Lists.VarTypes.VARF:
                    {
                        string[] Values = SplitLine[Index].Split('.');
                        return Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRangeInt(Values, 1, 1, Lists.Num_User_Vars));
                    }
                default:
                    return (float)GetNormalVar(SplitLine, Index, Min, Max);
            }
        }

        public static byte? GetSubIDForRamType(string RamType)
        {
            int dotIndex = RamType.IndexOf('.');
            RamType = dotIndex >= 0 ? RamType.Substring(0, dotIndex).ToUpper() : RamType.ToUpper();

            if (Enum.IsDefined(typeof(Lists.IfWhileAwaitSetRamSubTypes), RamType))
                return (byte)(int)Enum.Parse(typeof(Lists.IfWhileAwaitSetRamSubTypes), RamType);
            else
                return null;
        }

        public static Lists.ConditionTypes GetBoolConditionID(string[] SplitLine, int IndexOfCondition)
        {
            switch (SplitLine[IndexOfCondition].ToUpper())
            {
                case Lists.Keyword_True: return Lists.ConditionTypes.TRUE;
                case Lists.Keyword_False: return Lists.ConditionTypes.FALSE;
                default: throw ParseException.UnrecognizedCondition(SplitLine);
            }
        }

        public static bool IsCondition(string[] SplitLine, int Index)
        {
            switch (SplitLine[Index])
            {
                case "=":
                case "==":
                case "<":
                case ">":
                case ">=":
                case "<=":
                case "!=":
                case "<>":
                    return true;

                default:
                    return false;
            }
        }

        public static Lists.ConditionTypes GetConditionID(string[] SplitLine, int Index)
        {
            switch (SplitLine[Index])
            {
                case "=": return Lists.ConditionTypes.EQUALTO;
                case "==": return Lists.ConditionTypes.EQUALTO;
                case "<": return Lists.ConditionTypes.LESSTHAN;
                case ">": return Lists.ConditionTypes.MORETHAN;
                case "<=": return Lists.ConditionTypes.LESSOREQ;
                case ">=": return Lists.ConditionTypes.MOREOREQ;
                case "!=": return Lists.ConditionTypes.NOTEQUAL;
                case "<>": return Lists.ConditionTypes.NOTEQUAL;

                default: throw ParseException.UnrecognizedCondition(SplitLine);
            }
        }

        public static byte GetVarType(string[] SplitLine, int Index)
        {
            int dotIndex = SplitLine[Index].IndexOf('.');

            string Type = dotIndex >= 0 ? SplitLine[Index].Substring(0, dotIndex).ToUpper() : SplitLine[Index].ToUpper();

            if (dotIndex >= 0 && Enum.IsDefined(typeof(Lists.VarTypes), Type))
                return (byte)(int)Enum.Parse(typeof(Lists.VarTypes), Type);
            else
                return (byte)Lists.VarTypes.NORMAL;
        }

        public static bool IsHex(string Number)
        {
            string nU = Number.ToUpper();
            return (Number.Length >= 3 && nU.StartsWith("0X") || Number.Length >= 4 && nU.StartsWith("-0X"));
        }

        public static bool OnlyHexInString(string test)
        {
            return Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z", RegexOptions.Compiled);
        }

        public static object Helper_GetEnumByName(string[] SplitLine, int Index, Type EnumType, ParseException ErrorToThrow = null)
        {

            if (!Enum.IsDefined(EnumType, SplitLine[Index].ToUpper()))
            {
                if (ErrorToThrow == null)
                    throw ParseException.GeneralError(SplitLine);
                else
                    throw ErrorToThrow;
            }
            else
            {
                try
                {
                    return (float)(int)System.Enum.Parse(EnumType, SplitLine[Index].ToUpper());
                }
                catch (Exception)
                {
                    throw ParseException.ParamConversionError(SplitLine);
                }
            }

        }

        public static object Helper_GetEnumByNameOrVarType(string[] SplitLine, int Index, byte VarType, Type EnumType, ParseException Throw)
        {
            try
            {
                if (!Enum.IsDefined(EnumType, SplitLine[Index].ToUpper()))
                {
                    try
                    {
                        return GetValueByType(SplitLine, Index, VarType, Enum.GetValues(EnumType).Cast<int>().Min(), Enum.GetValues(EnumType).Cast<int>().Max());
                    }
                    catch (Exception)
                    {
                        throw Throw;
                    }
                }
                else
                {
                    return (float)(int)Enum.Parse(EnumType, SplitLine[Index].ToUpper());
                }
            }
            catch (Exception)
            {
                throw Throw;
            }
        }

        private static object GetNPCMakerEmbeddedTextID(string Name, List<MessageEntry> Messages)
        {
            MessageEntry Mes = Messages.Find(x => x.Name == Name);

            if (Mes == null)
                return (float)-1;
            else
                return (float)(1 + Int16.MaxValue + Messages.IndexOf(Mes));
        }

        public static void Helper_GetAdultChildTextIds(string[] SplitLine, ref ScriptVarVal TextID_Adult, ref ScriptVarVal TextID_Child, List<MessageEntry> Messages, int Index = 1)
        {
            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);

            TextID_Adult = new ScriptVarVal
            {
                Value = GetNPCMakerEmbeddedTextID(SplitLine[Index], Messages),
                Vartype = 0
            };

            if ((float)TextID_Adult.Value < 0)
            {
                ScriptHelpers.GetScriptVarVal(SplitLine, 1, 0, UInt16.MaxValue, ref TextID_Adult);
            }

            if (SplitLine.Length == 3)
            {
                TextID_Child = new ScriptVarVal
                {
                    Value = GetNPCMakerEmbeddedTextID(SplitLine[2], Messages),
                    Vartype = 0
                };

                if ((float)TextID_Child.Value < 0)
                {
                    ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt16.MaxValue, ref TextID_Child);
                }
            }
            else
            {
                TextID_Child = new ScriptVarVal
                {
                    Value = TextID_Adult.Value,
                    Vartype = TextID_Adult.Vartype
                };
            }
        }

        private static ScriptVarVal Helper_GetValFromDict(string[] SplitLine, int Index, int RangeMin, int RangeMax,
                                                   Dictionary<string, int> Dict, ParseException ToThrow)
        {
            var outV = new ScriptVarVal();
            outV.Vartype = GetVarType(SplitLine, Index);

            try
            {
                outV.Value = (float)Convert.ToDecimal(Dict[SplitLine[Index].ToUpper()]);
                return outV;
            }
            catch (Exception)
            {
                try
                {
                    outV.Value = GetValueByType(SplitLine, Index, outV.Vartype, RangeMin, RangeMax);
                    return outV;
                }
                catch (Exception)
                {
                    throw ToThrow;
                }
            }
        }

        public static ScriptVarVal Helper_GetActorId(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, 0, UInt16.MaxValue, Dicts.Actors, ParseException.UnrecognizedActor(SplitLine));
        }

        public static ScriptVarVal Helper_GetSFXId(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, -1, Int16.MaxValue, Dicts.SFXes, ParseException.UnrecognizedSFX(SplitLine));
        }

        public static ScriptVarVal Helper_GetMusicId(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, -1, UInt16.MaxValue, Dicts.Music, ParseException.UnrecognizedBGM(SplitLine));
        }

        public static ScriptVarVal Helper_GetLinkAnimation(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, 0, Int16.MaxValue, Dicts.LinkAnims, ParseException.UnrecognizedLinkAnim(SplitLine));
        }

        private static ScriptVarVal Helper_GetFromStringList(string[] SplitLine, int Index, List<string> SList, int RangeMin, int RangeMax, ParseException ToThrow, int? VarTypeOverride = null)
        {
            var outV = new ScriptVarVal
            {
                Vartype = (byte)(VarTypeOverride ?? GetVarType(SplitLine, Index))
            };

            string target = SplitLine[Index].ToLower().Replace(" ", "");

            for (int i = 0; i < SList.Count; i++)
            {
                if (target == SList[i].Replace(" ", "").ToLower())
                {
                    outV.Value = (float)i;
                    return outV;
                }
            }

            try
            {
                outV.Value = GetValueByType(SplitLine, Index, outV.Vartype, RangeMin, RangeMax);
                return outV;
            }
            catch (Exception)
            {
                throw ToThrow;
            }
        }

        public static ScriptVarVal Helper_GetAnimationID(string[] SplitLine, int Index, List<AnimationEntry> Animations)
        {
            return Helper_GetFromStringList(SplitLine, Index, Animations.Select(x => x.Name).ToList(), 0, Animations.Count, ParseException.UnrecognizedAnimation(SplitLine));
        }

        public static ScriptVarVal Helper_GetSegmentDataEntryID(string[] SplitLine, int Index, int Segment, List<List<SegmentEntry>> Textures, bool forceNormal = false)
        {
            if (forceNormal)
                return Helper_GetFromStringList(SplitLine, Index, Textures[Segment].Select(x => x.Name).ToList(), 0, Textures.Count, ParseException.UnrecognizedSegmentDataEntry(SplitLine), (int)Lists.VarTypes.NORMAL);
            else
                return Helper_GetFromStringList(SplitLine, Index, Textures[Segment].Select(x => x.Name).ToList(), 0, Textures.Count, ParseException.UnrecognizedSegmentDataEntry(SplitLine));
        }

        public static ScriptVarVal Helper_GetDListID(string[] SplitLine, int Index, List<DListEntry> DLists)
        {
            return Helper_GetFromStringList(SplitLine, Index, DLists.Select(x => x.Name).ToList(), 0, DLists.Count, ParseException.UnrecognizedDList(SplitLine));
        }

        public static string AppendDictToBaseDefines(Dictionary<string, int> dict, string prefix)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var entry in dict)
                sb.Append($"#{Lists.Keyword_Define} {prefix}{(entry.Key).Replace(" ", "_")} {entry.Value}{Environment.NewLine}");

            return sb.ToString();
        }

        public static string GetBaseDefines(NPCFile npc)
        {
            StringBuilder sb = new StringBuilder();

            /*
            sb.Append(AppendDictToBaseDefines(Dicts.Music, "MUSID_"));
            sb.Append(AppendDictToBaseDefines(Dicts.SFXes, "SFXID_"));
            sb.Append(AppendDictToBaseDefines(Dicts.Actors, "ACTORID_"));
            sb.Append(AppendDictToBaseDefines(Dicts.LinkAnims, "LINKANIMID_"));
            sb.Append(AppendDictToBaseDefines(Dicts.ObjectIDs, "OBJECTID_"));
            */

            int id = 0;

            foreach (var entry in npc.Entries)
            {
                if (!entry.IsNull)
                    sb.Append($"#{Lists.Keyword_Define} NPCID_{entry.NPCName.Replace(" ", "_")} {id}{Environment.NewLine}");
                
                id++;
            }

            string s = sb.ToString();

            s = s.Replace("(", "");
            s = s.Replace(")", "");
            s = s.Replace("{", "");
            s = s.Replace("}", "");
            s = s.Replace(";", "");
            s = s.Replace("\\", "");

            return s;
        }
    }
}
