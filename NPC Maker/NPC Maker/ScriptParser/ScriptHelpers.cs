using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NPC_Maker.Scripts
{
    public static class ScriptHelpers
    {
        static public int GetCorresponding(List<string> Lines, int LineNo, string Statement, string EndStatement)
        {
            int depth = 0;
            for (int i = LineNo + 1; i < Lines.Count; i++)
            {
                string line = Lines[i];
                int spaceIndex = line.IndexOf(' ');
                string firstWord = spaceIndex >= 0 ? line.Substring(0, spaceIndex) : line;

                if (String.Equals(firstWord, Statement, StringComparison.OrdinalIgnoreCase))
                    depth++;
                else if (String.Equals(firstWord, EndStatement, StringComparison.OrdinalIgnoreCase))
                {
                    if (depth == 0)
                        return i;
                    depth--;
                }
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
            if (Splitline.Length != Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsSmaller(string[] Splitline, int Number)
        {
            if (Splitline.Length < Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsBigger(string[] Splitline, int Number)
        {
            if (Splitline.Length > Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsNotBetween(string[] splitLine, int min, int max)
        {
            if (splitLine.Length < min || splitLine.Length > max)
                throw ParseException.ParamCountWrong(splitLine);
        }

        public static int GetSubIDValue(string[] splitLine, Type subTypeEnum, int index = 1)
        {
            var map = Dicts.SubTypeCache.GetOrAdd(subTypeEnum, t =>
                Enum.GetValues(t)
                    .Cast<object>()
                    .ToDictionary(v => v.ToString(), v => (int)v, StringComparer.OrdinalIgnoreCase));

            return map.TryGetValue(splitLine[index], out int value) ? value : -1;
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

        public static uint GetValueAndCheckRangeInt(string[] splitString, int index, uint min, uint max)
        {
            if (index < 0 || index >= splitString.Length)
                throw ParseException.ParamOutOfRange(splitString);

            string token = splitString[index];

            if (token.StartsWith("-"))
                throw ParseException.ParamOutOfRange(splitString);

            uint value;
            if (token.IsHex())
            {
                string str = token;
                if (str.StartsWith("0x") || str.StartsWith("0X"))
                    str = str.Substring(2);
                if (!uint.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                    throw ParseException.ParamConversionError(splitString);
            }
            else
            {
                if (!uint.TryParse(token, NumberStyles.None, CultureInfo.InvariantCulture, out value))
                    throw ParseException.ParamConversionError(splitString);
            }

            if (value < min || value > max)
                throw ParseException.ParamOutOfRange(splitString);

            return value;
        }

        public static object GetValueAndCheckRange(string[] Splitstring, int Index, float Min, float Max)
        {
            float value;
            string token = Splitstring[Index];

            bool neg = token.StartsWith("-");
            string str = neg ? token.Substring(1) : token;

            if (str.IsHex())
            {
                if (str.StartsWith("0x") || str.StartsWith("0X"))
                    str = str.Substring(2);
                int hexVal;
                if (!int.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexVal))
                    throw ParseException.ParamConversionError(Splitstring);
                value = neg ? -hexVal : hexVal;
            }
            else
            {
                if (!float.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    throw ParseException.ParamConversionError(Splitstring);
            }

            if (value < Min || value > Max)
                throw ParseException.ParamOutOfRange(Splitstring);

            return value;
        }

        public static byte GetOperator(string[] SplitLine, int Index)
        {
            string operatorString = SplitLine[Index];

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
            string value = SplitLine[Index].Trim();

            if (value.Equals(Lists.Keyword_False, StringComparison.OrdinalIgnoreCase))
                return new ScriptVarVal { Vartype = (byte)Lists.VarTypes.NORMAL, Value = 0f };

            if (value.Equals(Lists.Keyword_True, StringComparison.OrdinalIgnoreCase))
                return new ScriptVarVal { Vartype = (byte)Lists.VarTypes.NORMAL, Value = 1f };

            var varType = ScriptHelpers.GetVarType(SplitLine, Index);
            return new ScriptVarVal
            {
                Vartype = varType,
                Value = ScriptHelpers.GetValueByType(SplitLine, Index, varType, Min, Max)
            };
        }

        public static ScriptVarVal GetScriptExtVarVal(string[] SplitLine, int Index, float Min, float Max)
        {
            byte vartype = GetVarType(SplitLine, Index);
            if (vartype > (byte)Lists.VarTypes.NORMAL && vartype < (byte)Lists.VarTypes.VAR)
                throw ParseException.ParamOutOfRange(SplitLine);

            byte value = Convert.ToByte(GetValueByType(SplitLine, Index, vartype, Min, Max));
            if (value < 1 || value > Lists.Num_User_Vars)
                throw ParseException.ParamOutOfRange(SplitLine);

            return new ScriptVarVal { Vartype = vartype, Value = value };
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
                    return (float)Dicts.Music.Forward[varString.Substring(Lists.Keyword_Music.Length)];
                }
                else if (varString.StartsWith(Lists.Keyword_Actor))
                {
                    return (float)Dicts.Actors.Forward[varString.Substring(Lists.Keyword_Actor.Length)];
                }
                else if (varString.StartsWith(Lists.Keyword_Sfx))
                {
                    return (float)Dicts.SFXes.Forward[varString.Substring(Lists.Keyword_Sfx.Length)];
                }
                else
                {
                    try
                    {
                        var res = Program._sharedTable.Compute(SplitLine[Index], null);
                        if (res != null)
                            SplitLine[Index] = res.ToString();
                    }
                    catch (Exception)
                    {
                    }
                    return (float)ScriptHelpers.GetValueAndCheckRange(SplitLine, Index, Min, Max);
                }
            }
            catch (ParseException)
            {
                throw;
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

        public static Lists.ConditionTypes GetBoolConditionID(string[] splitLine, int indexOfCondition)
        {
            string cond = splitLine[indexOfCondition];

            if (cond.Equals(Lists.Keyword_True, StringComparison.OrdinalIgnoreCase)) return Lists.ConditionTypes.TRUE;
            if (cond.Equals(Lists.Keyword_False, StringComparison.OrdinalIgnoreCase)) return Lists.ConditionTypes.FALSE;

            try
            {
                float v = GetNormalVar(splitLine, indexOfCondition, float.MinValue, float.MaxValue);
                return v == 0 ? Lists.ConditionTypes.FALSE : Lists.ConditionTypes.TRUE;
            }
            catch
            {
                throw ParseException.UnrecognizedCondition(splitLine);
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

        public static Lists.ConditionTypes GetConditionID(string[] splitLine, int index)
        {
            switch (splitLine[index])
            {
                case "=":
                case "==": return Lists.ConditionTypes.EQUALTO;
                case "!=":
                case "<>": return Lists.ConditionTypes.NOTEQUAL;
                case "<": return Lists.ConditionTypes.LESSTHAN;
                case ">": return Lists.ConditionTypes.MORETHAN;
                case "<=": return Lists.ConditionTypes.LESSOREQ;
                case ">=": return Lists.ConditionTypes.MOREOREQ;
                default: throw ParseException.UnrecognizedCondition(splitLine);
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
            int index = Messages.FindIndex(x => x.Name == Name);
            return index < 0 ? (float)-1 : (float)(1 + short.MaxValue + index);
        }

        public static void Helper_GetAdultChildTextIds(string[] SplitLine, ref ScriptVarVal TextID_Adult, ref ScriptVarVal TextID_Child, List<MessageEntry> Messages, int Index = 1)
        {
            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);

            TextID_Adult = new ScriptVarVal { Value = GetNPCMakerEmbeddedTextID(SplitLine[Index], Messages), Vartype = 0 };
            if ((float)TextID_Adult.Value < 0)
                ScriptHelpers.GetScriptVarVal(SplitLine, Index, 0, ushort.MaxValue, ref TextID_Adult);

            if (SplitLine.Length == 3)
            {
                TextID_Child = new ScriptVarVal { Value = GetNPCMakerEmbeddedTextID(SplitLine[2], Messages), Vartype = 0 };
                if ((float)TextID_Child.Value < 0)
                    ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, ushort.MaxValue, ref TextID_Child);
            }
            else
            {
                TextID_Child = new ScriptVarVal { Value = TextID_Adult.Value, Vartype = TextID_Adult.Vartype };
            }
        }

        private static ScriptVarVal Helper_GetValFromDict(string[] SplitLine, int Index, int RangeMin, int RangeMax,
                                                           Dictionary<string, int> Dict, ParseException ToThrow)
        {
            var outV = new ScriptVarVal();
            outV.Vartype = GetVarType(SplitLine, Index);

            int dictVal;
            if (Dict.TryGetValue(SplitLine[Index].ToUpper(), out dictVal))
            {
                outV.Value = (float)dictVal;
                return outV;
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

        public static ScriptVarVal Helper_GetActorId(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, 0, UInt16.MaxValue, Dicts.Actors.Forward, ParseException.UnrecognizedActor(SplitLine));
        }

        public static ScriptVarVal Helper_GetSFXId(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, -1, Int16.MaxValue, Dicts.SFXes.Forward, ParseException.UnrecognizedSFX(SplitLine));
        }

        public static ScriptVarVal Helper_GetMusicId(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, -1, UInt16.MaxValue, Dicts.Music.Forward, ParseException.UnrecognizedBGM(SplitLine));
        }

        public static ScriptVarVal Helper_GetLinkAnimation(string[] SplitLine, int Index)
        {
            return Helper_GetValFromDict(SplitLine, Index, 0, Int16.MaxValue, Dicts.LinkAnims.Forward, ParseException.UnrecognizedLinkAnim(SplitLine));
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

        private static readonly HashSet<char> _invalidChars = new HashSet<char> { '(', ')', '{', '}', ';', '\\' };

        public static string GetBaseDefines(NPCFile npc)
        {
            StringBuilder sb = new StringBuilder();
            int id = 0;
            foreach (var entry in npc.Entries)
            {
                if (!entry.IsNull)
                {
                    string name = StripInvalidChars(entry.NPCName.Replace(" ", "_"));
                    sb.Append('#').Append(Lists.Keyword_Define)
                      .Append(' ').Append("NPCID_").Append(name)
                      .Append(' ').Append(id)
                      .Append(Environment.NewLine);
                }
                id++;
            }
            return sb.ToString();
        }

        private static string StripInvalidChars(string s)
        {
            if (s.IndexOfAny(new[] { '(', ')', '{', '}', ';', '\\' }) < 0)
                return s;
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
                if (!_invalidChars.Contains(c))
                    sb.Append(c);
            return sb.ToString();
        }
    }
}
