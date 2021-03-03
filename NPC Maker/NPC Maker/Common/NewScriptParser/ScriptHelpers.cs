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

        public static object GetValueAndCheckRange(string[] Splitstring, int Index, float Min, float Max)
        {
            float? Value;

            if (IsHex(Splitstring[Index]))
                Value = (float?)Convert.ToDecimal(Convert.ToInt32(Splitstring[Index], 16));
            else
                Value = (float?)Convert.ToDecimal(Splitstring[Index]);

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

        public static void GetXYZRot(string[] SplitLine, int XIndex, int YIndex, int ZIndex, ref byte XVarT, ref byte YVarT, ref byte ZVarT,
                                    ref Int32 XRot, ref Int32 YRot, ref Int32 ZRot)
        {
            XRot = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, XIndex, XVarT = ScriptHelpers.GetVarType(SplitLine, XIndex), Int16.MinValue, Int16.MaxValue));
            YRot = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, YIndex, YVarT = ScriptHelpers.GetVarType(SplitLine, YIndex), Int16.MinValue, Int16.MaxValue));
            ZRot = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, ZIndex, ZVarT = ScriptHelpers.GetVarType(SplitLine, ZIndex), Int16.MinValue, Int16.MaxValue));
        }

        public static void GetXYZPos(string[] SplitLine, int XIndex, int YIndex, int ZIndex, ref byte XVarT, ref byte YVarT, ref byte ZVarT,
                            ref float XPos, ref float YPos, ref float ZPos)
        {
            XPos = (float)(ScriptHelpers.GetValueByType(SplitLine, XIndex, XVarT = ScriptHelpers.GetVarType(SplitLine, XIndex), Int32.MinValue, Int32.MaxValue));
            YPos = (float)(ScriptHelpers.GetValueByType(SplitLine, YIndex, YVarT = ScriptHelpers.GetVarType(SplitLine, YIndex), Int32.MinValue, Int32.MaxValue));
            ZPos = (float)(ScriptHelpers.GetValueByType(SplitLine, ZIndex, ZVarT = ScriptHelpers.GetVarType(SplitLine, ZIndex), Int32.MinValue, Int32.MaxValue));
        }

        public static void GetScale(string[] SplitLine, int Index, ref byte ScaleVarT, ref float Scale)
        {
            Scale = (float)ScriptHelpers.GetValueByType(SplitLine, Index, ScaleVarT = ScriptHelpers.GetVarType(SplitLine, Index), float.MinValue, float.MaxValue);
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

                        Int32 Val = Convert.ToInt32(ScriptHelpers.GetValueAndCheckRange(Values, 1,
                                                                                        (float)Min < Int16.MinValue ? Int16.MinValue : Min,
                                                                                        (float)Max > Int16.MaxValue ? Int16.MaxValue : Max));

                        return Val;
                    }
                case (int)Lists.VarTypes.Ram8:
                case (int)Lists.VarTypes.Ram16:
                case (int)Lists.VarTypes.Ram32:
                    {
                        string[] Values = SplitLine[Index].Split('.');
                        return Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRange(Values, 1, 0x8000000, 0x8800000));
                    }
                case (int)Lists.VarTypes.Player8:
                case (int)Lists.VarTypes.Player16:
                case (int)Lists.VarTypes.Player32:
                case (int)Lists.VarTypes.Global8:
                case (int)Lists.VarTypes.Global16:
                case (int)Lists.VarTypes.Global32:
                    {
                        string[] Values = SplitLine[Index].Split('.');
                        return Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRange(Values, 1, 0, 0x8800000));
                    }
                case (int)Lists.VarTypes.Var1:
                case (int)Lists.VarTypes.Var2:
                case (int)Lists.VarTypes.Var3:
                case (int)Lists.VarTypes.Var4:
                case (int)Lists.VarTypes.Var5:
                    return 0;
                default:
                    return (float)ScriptHelpers.GetValueAndCheckRange(SplitLine, Index, Min, Max);

            }
        }

        public static byte? GetSubIDForRamType(string RamType)
        {
            RamType = RamType.ToUpper();

            if (RamType.StartsWith(Lists.Keyword_RNG))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Random;
            else if (RamType.StartsWith(Lists.Keyword_RAM8))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Ram8;
            else if (RamType.StartsWith(Lists.Keyword_RAM16))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Ram16;
            else if (RamType.StartsWith(Lists.Keyword_RAM32))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Ram32;
            else if (RamType.StartsWith(Lists.Keyword_Global8))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Global8;
            else if (RamType.StartsWith(Lists.Keyword_Global16))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Global16;
            else if (RamType.StartsWith(Lists.Keyword_Global32))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Global32;
            else if (RamType.StartsWith(Lists.Keyword_Player8))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Player8;
            else if (RamType.StartsWith(Lists.Keyword_Player16))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Player16;
            else if (RamType.StartsWith(Lists.Keyword_Player32))
                return (byte)Lists.IfWhileAwaitSetRamSubTypes.Player32;
            else
            {
                switch (RamType)
                {
                    case Lists.Keyword_ScriptVar1: return (byte)Lists.IfWhileAwaitSetRamSubTypes.Var1;
                    case Lists.Keyword_ScriptVar2: return (byte)Lists.IfWhileAwaitSetRamSubTypes.Var2;
                    case Lists.Keyword_ScriptVar3: return (byte)Lists.IfWhileAwaitSetRamSubTypes.Var3;
                    case Lists.Keyword_ScriptVar4: return (byte)Lists.IfWhileAwaitSetRamSubTypes.Var4;
                    case Lists.Keyword_ScriptVar5: return (byte)Lists.IfWhileAwaitSetRamSubTypes.Var5;
                    default: return null;
                }
            }
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

            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_RAM8))
                return (int)Lists.VarTypes.Ram8;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_RAM16))
                return (int)Lists.VarTypes.Ram16;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_RAM32))
                return (int)Lists.VarTypes.Ram16;


            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Global8))
                return (int)Lists.VarTypes.Global8;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Global16))
                return (int)Lists.VarTypes.Global16;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Global32))
                return (int)Lists.VarTypes.Global32;


            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Player8))
                return (int)Lists.VarTypes.Player8;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Player16))
                return (int)Lists.VarTypes.Player16;
            else if (SplitLine[Index].ToUpper().StartsWith(Lists.Keyword_Player32))
                return (int)Lists.VarTypes.Player32;


            else
            {
                switch (SplitLine[Index].ToUpper())
                {
                    case Lists.Keyword_ScriptVar1: return (int)Lists.VarTypes.Var1;
                    case Lists.Keyword_ScriptVar2: return (int)Lists.VarTypes.Var2;
                    case Lists.Keyword_ScriptVar3: return (int)Lists.VarTypes.Var3;
                    case Lists.Keyword_ScriptVar4: return (int)Lists.VarTypes.Var4;
                    case Lists.Keyword_ScriptVar5: return (int)Lists.VarTypes.Var5;
                    default: return 0;
                }
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

        public static Int64? Helper_ConvertNumeric(string Number)
        {
            try
            {
                if (IsHex(Number))
                    return Convert.ToInt64(Number, 16);
                else
                    return Convert.ToInt64(Number);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UInt64? Helper_ConvertSignedNumeric(string Number)
        {
            try
            {
                if (IsHex(Number))
                    return Convert.ToUInt64(Number, 16);
                else
                    return Convert.ToUInt64(Number);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static UInt16? Helper_ConvertToUInt16(string Number)
        {
            try
            {
                if (IsHex(Number))
                    return Convert.ToUInt16(Number, 16);
                else
                    return Convert.ToUInt16(Number);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Int16? Helper_ConvertToInt16(string Number)
        {
            try
            {
                if (IsHex(Number))
                    return Convert.ToInt16(Number, 16);
                else
                    return Convert.ToInt16(Number);
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

        public static UInt32? Helper_GetEnumByNameOrVarType(string[] SplitLine, int Index, byte VarType, Type EnumType)
        {
            try
            {
                return Convert.ToUInt32(System.Enum.Parse(EnumType, SplitLine[Index].ToUpper()));
            }
            catch (Exception)
            {
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, Enum.GetValues(EnumType).Cast<int>().Min(), Enum.GetValues(EnumType).Cast<int>().Max()));
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

        public static UInt32? Helper_GetAnimationID(string[] SplitLine, int Index, byte VarType, List<AnimationEntry> Animations)
        {
            UInt32? AnimID = null;

            for (int i = 0; i < Animations.Count; i++)
            {
                if (SplitLine[Index].ToLower() == Animations[i].Name.Replace(" ", "").ToLower())
                {
                    AnimID = (UInt32)i;
                    break;
                }
            }

            if (AnimID != null)
                return AnimID;
            else
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, 0, Animations.Count));
        }

        public static UInt32 Helper_GetActorId(string[] SplitLine, int Index, int VarType)
        {
            try
            {
                return Convert.ToUInt32(Dicts.Actors[SplitLine[Index].ToUpper()]);
            }
            catch (Exception)
            {
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, 0, UInt16.MaxValue));
            }
        }

        public static UInt32 Helper_GetActorCategory(string[] SplitLine, int Index, int VarType)
        {
            try
            {
                return Convert.ToUInt32(Dicts.ActorCategories[SplitLine[Index].ToUpper()]);
            }
            catch (Exception)
            {
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, 0, 11));
            }
        }

        public static UInt32 Helper_GetSFXId(string[] SplitLine, int Index, int VarType)
        {
            try
            {
                return Convert.ToUInt32(Dicts.SFXes[SplitLine[Index].ToUpper()]);
            }
            catch (Exception)
            {
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, 0, UInt16.MaxValue));
            }
        }

        public static UInt32 Helper_GetMusicId(string[] SplitLine, int Index, int VarType)
        {
            try
            {
                return Convert.ToUInt32(Dicts.Music[SplitLine[Index].ToUpper()]);
            }
            catch (Exception)
            {
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, 0, UInt16.MaxValue));
            }
        }

        public static UInt32 Helper_GetTextureID(string[] SplitLine, int Index, int Segment, byte VarType, List<List<SegmentEntry>> Textures)
        {
            UInt32? TexID = null;

            for (int i = 0; i < Textures[Segment].Count; i++)
            {
                if (SplitLine[Index].ToLower() == Textures[Segment][i].Name.Replace(" ", "").ToLower())
                {
                    TexID = (UInt32)i;
                    break;
                }
            }

            if (TexID != null)
                return (UInt32)TexID;
            else
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, 0, Textures[Segment].Count));
        }

        public static UInt32 Helper_GetDListID(string[] SplitLine, int Index, byte VarType, List<DListEntry> DLists)
        {
            UInt32? DListID = null;

            for (int i = 0; i < DLists.Count; i++)
            {
                if (SplitLine[Index].ToLower() == DLists[i].Name.Replace(" ", "").ToLower())
                {
                    DListID = (UInt32)i;
                    break;
                }
            }

            if (DListID != null)
                return (UInt32)DListID;
            else
                return Convert.ToUInt32(GetValueByType(SplitLine, Index, VarType, 0, DLists.Count));
        }
    }
}
