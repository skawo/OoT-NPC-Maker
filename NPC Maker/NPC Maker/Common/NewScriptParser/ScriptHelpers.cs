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

        public static void ErrorIfNumParamsNotBetween(string[] Splitline, int Min, int Max)
        {
            ErrorIfNumParamsSmaller(Splitline, Min);
            ErrorIfNumParamsBigger(Splitline, Max);
        }

        public static int GetSubIDValue(string[] SplitLine, Type SubTypeEnum)
        {
            return (int)Enum.Parse(SubTypeEnum, SplitLine[1].ToUpper());
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

        public static object GetValueAndCheckRange(string[] Splitstring, int Index, int Min, int Max)
        {
            Int32? Value = ScriptHelpers.Helper_ConvertToInt32(Splitstring[Index]);

            if (Value == null)
                throw ParseException.ParamConversionError(Splitstring);

            if (Value < Min || Value > Max)
                throw ParseException.ParamOutOfRange(Splitstring);

            return Value;
        }

        public static object GetValueAndCheckRange(string[] Splitstring, int Index, float Min, float Max)
        {
            float? Value = (float?)Convert.ToDecimal(Splitstring[Index]);

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

        public static float GetValueByType(string[] SplitLine, int Index, int ValueType, float Min = float.MinValue, float Max = float.MaxValue)
        {
            float Value = 0;

            if (ValueType == (int)Lists.VarTypes.RNG)
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, Index + 2);
                Int32 Val = (Int32)ScriptHelpers.GetValueAndCheckRange(SplitLine, 
                                                                       Index + 1, 
                                                                       (int)Math.Min(Min < Int16.MinValue ? Int16.MinValue : Min, Int16.MinValue), 
                                                                       (int)Math.Max(Max > Int16.MaxValue ? Int16.MaxValue : Max, Int16.MaxValue));

                Value = (float)Convert.ToDecimal(Val);
            }
            else if (ValueType < (int)Lists.VarTypes.Var1)
                Value = (float)ScriptHelpers.GetValueAndCheckRange(SplitLine, Index, Min, Max);

            return Value;
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

        public static byte GetVariable(string[] SplitLine, int Index)
        {
            switch (SplitLine[Index].ToUpper())
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

        public static bool IsHex(string Number)
        {
            return (Number.Length >= 3 && Number.StartsWith("0x"));
        }

        public static UInt32? Helper_ConvertToUInt32(string Number)
        {
            try
            {
                if (IsHex(Number))
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
                if (IsHex(Number))
                    return Convert.ToInt32(Number, 16);
                else
                    return Convert.ToInt32(Number);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UInt32? Helper_GetEnumByName(string[] SplitLine, int Index, Type EnumType, ParseException ErrorToThrow = null)
        {
            try
            {
                return Convert.ToUInt32(System.Enum.Parse(EnumType, SplitLine[Index].ToUpper()));
            }
            catch (Exception)
            {
                if (ErrorToThrow == null)
                    throw ParseException.GeneralError(SplitLine);
                else
                    throw ErrorToThrow;
            }
        }

        public static void Helper_GetAdultChildTextIds(string[] SplitLine, ref UInt16? TextID_Adult, ref UInt16? TextID_Child)
        {
            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);

            TextID_Adult = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 1, 0, UInt16.MaxValue));
            TextID_Child = (SplitLine.Count() == 2) ? TextID_Adult : Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));
        }
        
        public static UInt32? Helper_GetAnimationID(string[] SplitLine, int Index, List<AnimationEntry> Animations)
        {
            UInt32? AnimID = null;

            if (SplitLine[Index].IsNumeric())
                AnimID = ScriptHelpers.Helper_ConvertToUInt32(SplitLine[Index]);
            else
            {
                for (int i = 0; i < Animations.Count; i++)
                {
                    if (SplitLine[Index].ToLower() == Animations[i].Name.Replace(" ", "").ToLower())
                    {
                        AnimID = (UInt32)i;
                        break;
                    }
                }
            }

            if (AnimID == null || (AnimID > UInt16.MaxValue || AnimID < 0))
                throw ParseException.UnrecognizedAnimation(SplitLine);

            return AnimID;
        }

        public static UInt32? Helper_GetSFXId(string SFXName)
        {
            try
            {
                return (UInt32?)Dicts.SFXes[SFXName.ToUpper()];
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
                return (UInt32?)Dicts.Music[MusicName.ToUpper()];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UInt32? Helper_GetActorId(string ActorName)
        {
            try
            {
                return (UInt32?)Dicts.Actors[ActorName.ToUpper()];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Int32? Helper_GetTextureID(string[] SplitLine, int Index, int Segment, List<List<SegmentEntry>> Textures)
        {
            Int32? TexID = null;

            if (SplitLine[Index].IsNumeric())
                TexID = Convert.ToInt32(ScriptHelpers.GetValueAndCheckRange(SplitLine, Index, 0, 31));
            else
            {
                for (int i = 0; i < Textures[Segment].Count; i++)
                {
                    if (SplitLine[Index].ToLower() == Textures[Segment][i].Name.Replace(" ", "").ToLower())
                    {
                        TexID = i;
                        break;
                    }
                }
            }

            if (TexID == null)
                throw ParseException.UnrecognizedTexture(SplitLine);

            return TexID;
        }

        public static Int32? Helper_GetDListID(string[] SplitLine, int Index, List<DListEntry> DLists)
        {
            Int32? DListID = null;

            if (SplitLine[Index].IsNumeric())
                DListID = Convert.ToInt32(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));
            else
            {
                for (int i = 0; i < DLists.Count; i++)
                {
                    if (SplitLine[Index].ToLower() == DLists[i].Name.Replace(" ", "").ToLower())
                    {
                        DListID = i;
                        break;
                    }
                }
            }

            if (DListID == null)
                throw ParseException.UnrecognizedDList(SplitLine);


            return null;
        }
    }
}
