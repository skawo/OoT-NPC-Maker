﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NPC_Maker.Scripts
{
    public static class ScriptHelpers
    {
        static public string ReplaceExpr(this string Orig, string Expr, string Replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            string Pattern = String.Format(@"\b{0}\b", Regex.Escape(Expr));
            return Regex.Replace(Orig, Pattern, Replacement, regexOptions);
        }

        static public string ReplaceFirstExpr(this string Orig, string Expr, string Replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            var regex = new Regex(String.Format(@"\b{0}\b", Regex.Escape(Expr)), regexOptions);
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
                throw ParseException.ParamCountSmall(Splitline);
        }

        public static void ErrorIfNumParamsBigger(string[] Splitline, int Number)
        {
            if (Splitline.Count() > Number)
                throw ParseException.ParamCountWrong(Splitline);
        }

        public static void ErrorIfNumParamsNotBetween(string[] Splitline, int Min, int Max)
        {
            ErrorIfNumParamsSmaller(Splitline, Min);
            ErrorIfNumParamsBigger(Splitline, Max);
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

            if (IsHex(Splitstring[Index]))
                Value = (UInt32?)Convert.ToInt32(Splitstring[Index], 16);
            else
                Value = (UInt32?)Convert.ToInt32(Splitstring[Index]);

            if (Value == null)
                throw ParseException.ParamConversionError(Splitstring);

            if (Value < Min || Value > Max)
                throw ParseException.ParamOutOfRange(Splitstring);

            return Value;
        }

        public static object GetValueAndCheckRange(string[] Splitstring, int Index, float Min, float Max)
        {
            float? Value = null;

            try
            {
                if (IsHex(Splitstring[Index]))
                    Value = (float?)Convert.ToDecimal(Convert.ToInt32(Splitstring[Index], 16));
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
            switch (SplitLine[Index].ToUpper())
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

        public static void GetXYZRot(string[] SplitLine, int XIndex, int YIndex, int ZIndex, ref byte XVarT, ref byte YVarT, ref byte ZVarT,
                                    ref object XRot, ref object YRot, ref object ZRot)
        {
            XRot = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, XIndex, XVarT = ScriptHelpers.GetVarType(SplitLine, XIndex), Int16.MinValue, Int16.MaxValue));
            YRot = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, YIndex, YVarT = ScriptHelpers.GetVarType(SplitLine, YIndex), Int16.MinValue, Int16.MaxValue));
            ZRot = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, ZIndex, ZVarT = ScriptHelpers.GetVarType(SplitLine, ZIndex), Int16.MinValue, Int16.MaxValue));
        }

        public static void GetXYZPos(string[] SplitLine, int XIndex, int YIndex, int ZIndex, ref byte XVarT, ref byte YVarT, ref byte ZVarT,
                            ref object XPos, ref object YPos, ref object ZPos)
        {
            XPos = ScriptHelpers.GetValueByType(SplitLine, XIndex, XVarT = ScriptHelpers.GetVarType(SplitLine, XIndex), Int32.MinValue, Int32.MaxValue);
            YPos = ScriptHelpers.GetValueByType(SplitLine, YIndex, YVarT = ScriptHelpers.GetVarType(SplitLine, YIndex), Int32.MinValue, Int32.MaxValue);
            ZPos = ScriptHelpers.GetValueByType(SplitLine, ZIndex, ZVarT = ScriptHelpers.GetVarType(SplitLine, ZIndex), Int32.MinValue, Int32.MaxValue);
        }

        public static void GetRGBA(string[] SplitLine, int StartIndex, ref UInt32[] Output, ref byte[] TypeOutPut)
        {
            Output = new UInt32[] { 0, 0, 0, 0 };
            TypeOutPut = new byte[] { 0, 0, 0, 0 };

            TypeOutPut[0] = GetVarType(SplitLine, StartIndex);
            TypeOutPut[1] = GetVarType(SplitLine, StartIndex + 1);
            TypeOutPut[2] = GetVarType(SplitLine, StartIndex + 2);
            TypeOutPut[3] = GetVarType(SplitLine, StartIndex + 3);

            Output[0] = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, StartIndex, TypeOutPut[0], 0, 255));
            Output[1] = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, StartIndex + 1, TypeOutPut[1], 0, 255));
            Output[2] = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, StartIndex + 2, TypeOutPut[2], 0, 255));
            Output[3] = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, StartIndex + 3, TypeOutPut[3], 0, 255));
        }

        public static void GetScale(string[] SplitLine, int Index, ref byte ScaleVarT, ref object Scale)
        {
            Scale = ScriptHelpers.GetValueByType(SplitLine, Index, ScaleVarT = ScriptHelpers.GetVarType(SplitLine, Index), float.MinValue, float.MaxValue);
        }

        public static object GetValueByType(string[] SplitLine, int Index, int VarType, float Min, float Max)
        {
            switch (VarType)
            {
                case (int)Lists.VarTypes.Random:
                    {
                        string[] Values = SplitLine[Index].Split('>');

                        if (!Values[0].EndsWith("-"))
                            throw ParseException.UnrecognizedParameter(SplitLine);

                        return Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRange(Values, 1, (float)Min < Int16.MinValue ? Int16.MinValue : Min,
                                                                                   (float)Max > Int16.MaxValue ? Int16.MaxValue : Max));
                    }

                case (int)Lists.VarTypes.Player8:
                case (int)Lists.VarTypes.Player16:
                case (int)Lists.VarTypes.Player32:
                case (int)Lists.VarTypes.Playerf:
                case (int)Lists.VarTypes.Global8:
                case (int)Lists.VarTypes.Global16:
                case (int)Lists.VarTypes.Global32:
                case (int)Lists.VarTypes.Globalf:
                case (int)Lists.VarTypes.Self8:
                case (int)Lists.VarTypes.Self16:
                case (int)Lists.VarTypes.Self32:
                case (int)Lists.VarTypes.Selff:
                    {
                        string[] Values = SplitLine[Index].Split('.');
                        return Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRangeInt(Values, 1, 0, 0x88000000));
                    }
                case (int)Lists.VarTypes.Var:
                    {
                        string[] Values = SplitLine[Index].Split('.');
                        return Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRangeInt(Values, 1, 1, Lists.Num_User_Vars));
                    }
                default:
                    {
                        if (SplitLine[Index].StartsWith(Lists.Keyword_Degree))
                        {
                            string[] s = SplitLine[Index].Split('_');
                            float value = (float)ScriptHelpers.GetValueAndCheckRange(s, 1, Min, Max);

                            return (float)Rot2Deg(Convert.ToInt32(value));

                        }
                        else
                            return (float)ScriptHelpers.GetValueAndCheckRange(SplitLine, Index, Min, Max);

                    }

            }
        }

        public static void GetVarTypeAndValue(string[] SplitLine, int Index, float Min, float Max, out byte Vartype, out object Value)
        {
            Vartype = GetVarType(SplitLine, Index);
            Value = GetValueByType(SplitLine, Index, Vartype, Min, Max);
        }

        public static byte? GetSubIDForRamType(string RamType)
        {
            RamType = RamType.ToUpper();

            if (RamType.StartsWith(Lists.Keyword_RNG))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Random;
            else if (RamType.StartsWith(Lists.Keyword_Global8))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Global8;
            else if (RamType.StartsWith(Lists.Keyword_Global16))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Global16;
            else if (RamType.StartsWith(Lists.Keyword_Global32))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Global32;
            else if (RamType.StartsWith(Lists.Keyword_GlobalF))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.GlobalF;
            else if (RamType.StartsWith(Lists.Keyword_Player8))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Player8;
            else if (RamType.StartsWith(Lists.Keyword_Player16))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Player16;
            else if (RamType.StartsWith(Lists.Keyword_Player32))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Player32;
            else if (RamType.StartsWith(Lists.Keyword_PlayerF))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.PlayerF;
            else if (RamType.StartsWith(Lists.Keyword_Self8))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Self8;
            else if (RamType.StartsWith(Lists.Keyword_Self16))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Self16;
            else if (RamType.StartsWith(Lists.Keyword_Self32))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Self32;
            else if (RamType.StartsWith(Lists.Keyword_SelfF))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.SelfF;
            else if (RamType.StartsWith(Lists.Keyword_ScriptVar))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Var;
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
            if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_RNG))
                return (int)Lists.VarTypes.Random;

            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Global8))
                return (int)Lists.VarTypes.Global8;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Global16))
                return (int)Lists.VarTypes.Global16;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Global32))
                return (int)Lists.VarTypes.Global32;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_GlobalF))
                return (int)Lists.VarTypes.Globalf;

            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Player8))
                return (int)Lists.VarTypes.Player8;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Player16))
                return (int)Lists.VarTypes.Player16;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Player32))
                return (int)Lists.VarTypes.Player32;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_PlayerF))
                return (int)Lists.VarTypes.Playerf;

            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Self8))
                return (int)Lists.VarTypes.Self8;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Self16))
                return (int)Lists.VarTypes.Self16;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Self32))
                return (int)Lists.VarTypes.Self32;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_SelfF))
                return (int)Lists.VarTypes.Selff;

            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_ScriptVar))
                return (int)Lists.VarTypes.Var;
            else
                return (int)Lists.VarTypes.Normal;
        }

        public static bool IsHex(string Number)
        {
            return (Number.Length >= 3 && Number.StartsWith("0x"));
        }

        public static UInt32 Helper_GetEnumByName(string[] SplitLine, int Index, Type EnumType, ParseException ErrorToThrow = null)
        {
            try
            {
                if (!Enum.IsDefined(EnumType, SplitLine[Index].ToUpper()))
                {
                    if (ErrorToThrow == null)
                        throw ParseException.GeneralError(SplitLine);
                    else
                        throw ErrorToThrow;
                }
                else
                    return Convert.ToUInt32(System.Enum.Parse(EnumType, SplitLine[Index].ToUpper()));

            }
            catch (Exception)
            {
                throw ErrorToThrow;
            }
        }

        public static UInt32 Helper_GetEnumByNameOrVarType(string[] SplitLine, int Index, byte VarType, Type EnumType, ParseException Throw)
        {
            try
            {
                if (!Enum.IsDefined(EnumType, SplitLine[Index].ToUpper()))
                {
                    try
                    {
                        return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, Enum.GetValues(EnumType).Cast<int>().Min(), Enum.GetValues(EnumType).Cast<int>().Max()));
                    }
                    catch (Exception)
                    {
                        throw Throw;
                    }
                }
                else
                    return Convert.ToUInt32(System.Enum.Parse(EnumType, SplitLine[Index].ToUpper()));
            }
            catch (Exception)
            {
                throw Throw;
            }
        }

        public static void Helper_GetAdultChildTextIds(string[] SplitLine, ref UInt32 TextID_Adult, ref UInt32 TextID_Child, ref byte VarTAdult, ref byte VarTChild)
        {
            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);

            VarTAdult = ScriptHelpers.GetVarType(SplitLine, 1);
            TextID_Adult = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 1, VarTAdult, 0, UInt16.MaxValue));

            if (SplitLine.Count() == 3)
            {
                VarTChild = ScriptHelpers.GetVarType(SplitLine, 2);
                TextID_Child = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarTChild, 0, UInt16.MaxValue));
            }
            else
            {
                VarTChild = VarTAdult;
                TextID_Child = TextID_Adult;
            }
        }

        private static Int32 Helper_GetValFromDict(string[] SplitLine, int Index, int VarType, int RangeMin, int RangeMax,
                                                   Dictionary<string, int> Dict, ParseException ToThrow)
        {
            try
            {
                return Convert.ToInt32(Dict[SplitLine[Index].ToUpper()]);
            }
            catch (Exception)
            {
                try
                {
                    return Convert.ToInt32(GetValueByType(SplitLine, Index, VarType, RangeMin, RangeMax));
                }
                catch (Exception)
                {
                    throw ToThrow;
                }
            }
        }

        public static UInt32 Helper_GetActorId(string[] SplitLine, int Index, int VarType)
        {
            return (UInt32)Helper_GetValFromDict(SplitLine, Index, VarType, 0, UInt16.MaxValue, Dicts.Actors, ParseException.UnrecognizedActor(SplitLine));
        }

        public static Int32 Helper_GetActorCategory(string[] SplitLine, int Index, int VarType)
        {
            return Helper_GetValFromDict(SplitLine, Index, VarType, 0, 11, Dicts.ActorCategories, ParseException.UnrecognizedActorCategory(SplitLine));
        }

        public static Int32 Helper_GetSFXId(string[] SplitLine, int Index, int VarType)
        {
            return Helper_GetValFromDict(SplitLine, Index, VarType, -1, Int16.MaxValue, Dicts.SFXes, ParseException.UnrecognizedSFX(SplitLine));
        }

        public static Int32 Helper_GetMusicId(string[] SplitLine, int Index, int VarType)
        {
            return Helper_GetValFromDict(SplitLine, Index, VarType, -1, Int16.MaxValue, Dicts.Music, ParseException.UnrecognizedBGM(SplitLine));
        }

        private static UInt32 Helper_GetFromStringList(string[] SplitLine, int Index, byte VarType, List<string> SList, int RangeMin, int RangeMax, ParseException ToThrow)
        {
            UInt32? Ret = null;

            for (int i = 0; i < SList.Count; i++)
            {
                if (SplitLine[Index].ToLower() == SList[i].Replace(" ", "").ToLower())
                {
                    Ret = (UInt32)i;
                    break;
                }
            }

            if (Ret != null)
                return (UInt32)Ret;
            else
            {
                try
                {
                    return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, RangeMin, RangeMax));
                }
                catch (Exception)
                {
                    throw ToThrow;
                }
            }
        }

        public static UInt32 Helper_GetAnimationID(string[] SplitLine, int Index, byte VarType, List<AnimationEntry> Animations)
        {
            return Helper_GetFromStringList(SplitLine, Index, VarType, Animations.Select(x => x.Name).ToList(), 0, Animations.Count, ParseException.UnrecognizedAnimation(SplitLine));
        }

        public static UInt32 Helper_GetSegmentDataEntryID(string[] SplitLine, int Index, int Segment, byte VarType, List<List<SegmentEntry>> Textures)
        {
            return Helper_GetFromStringList(SplitLine, Index, VarType, Textures[Segment].Select(x => x.Name).ToList(), 0, Textures.Count, ParseException.UnrecognizedSegmentDataEntry(SplitLine));
        }

        public static UInt32 Helper_GetDListID(string[] SplitLine, int Index, byte VarType, List<DListEntry> DLists)
        {
            return Helper_GetFromStringList(SplitLine, Index, VarType, DLists.Select(x => x.Name).ToList(), 0, DLists.Count, ParseException.UnrecognizedDList(SplitLine));
        }
    }
}