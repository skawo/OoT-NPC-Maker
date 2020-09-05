using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private List<Instruction> ParseIfWhileInstruction(int ID, List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                List<Instruction> Instructions = new List<Instruction>();
                string LabelR = ParserHelpers.RandomString(this, 5);

                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);
                int SubID = (int)System.Enum.Parse(typeof(Lists.IfSubTypes), SplitLine[1].ToUpper());

                int EndIf = 0;
                int Else = 0;

                if (ID == (int)Lists.Instructions.IF)
                {
                    EndIf = GetCorrespondingEndIf(Lines, LineNo);
                    Else = GetCorrespondingElse(Lines, LineNo, EndIf);

                    if (EndIf == -1)
                        throw ParseException.IfNotClosed(SplitLine);

                    if (Else == -1)
                        Else = EndIf;

                    #region true
                    Instructions.Add(new InstructionLabel("__IFTRUE__" + LabelR));
                    Instructions.AddRange(GetInstructions(Lines.Skip(LineNo + 1).Take(Else - LineNo - 1).ToList()));
                    Instructions.Add(new InstructionGoto("__IFEND__" + LabelR));
                    #endregion

                    #region false
                    Instructions.Add(new InstructionLabel("__IFFALSE__" + LabelR));

                    if (Else != EndIf)
                        Instructions.AddRange(GetInstructions(Lines.Skip(Else + 1).Take(EndIf - Else - 1).ToList()));

                    Instructions.Add(new InstructionLabel("__IFEND__" + LabelR));
                    #endregion

                }
                else if (ID == (int)Lists.Instructions.WHILE)
                {
                    EndIf = GetCorrespondingEndWhile(Lines, LineNo);

                    if (EndIf == -1)
                        throw ParseException.WhileNotClosed(SplitLine);

                    Else = EndIf;

                    Instructions.Add(new InstructionLabel("__WHILE__" + LabelR));
                    Instructions.AddRange(GetInstructions(Lines.Skip(LineNo + 1).Take(Else - LineNo - 1).ToList()));
                    Instructions.Add(new InstructionGoto("__RETURN__"));
                    Instructions.Add(new InstructionGoto("__WHILE__" + LabelR));
                }
                else
                    throw new Exception();

                LineNo = EndIf;

                try
                {
                    // Flag Types
                    if (SubID <= 7)
                    {
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                        UInt16 FlagID = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                        Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, FlagID, byte.MaxValue, EndIf, Else, LabelR));
                        return Instructions;
                    }
                    // Bool Types
                    if (SubID <= 29)
                    {
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                        byte Condition = ScriptHelpers.GetBoolConditionID(SplitLine[2]);

                        #region Exceptions

                        if (Condition == 255)
                            throw ParseException.UnrecognizedCondition(SplitLine);

                        #endregion

                        Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, 0, Condition, EndIf, Else, LabelR));
                        return Instructions;
                    }
                    // UInt16 Types
                    if (SubID <= 59)
                    {
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                        byte Condition = ScriptHelpers.GetConditionID(SplitLine[2]);

                        #region Exceptions

                        if (Condition == 255)
                            throw ParseException.UnrecognizedCondition(SplitLine);

                        #endregion

                        byte VarType = ScriptHelpers.GetVariable(SplitLine[3]);
                        UInt32 Value = 0;

                        if (VarType == (int)Lists.VarTypes.RNG)
                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                        if (VarType < (int)Lists.VarTypes.Var1)
                            Value = Convert.ToUInt32(ParserHelpers.GetValueAndCheckRange(SplitLine,
                                                                                         VarType == (int)Lists.VarTypes.RNG ? 4 : 3, 0, UInt16.MaxValue));

                        Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Value, Condition, EndIf, Else, LabelR));
                        return Instructions;
                    }
                    // Special
                    else
                    {
                        switch (SubID)
                        {
                            case (int)Lists.IfSubTypes.ITEM_BEING_TRADED:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Item = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.TradeItems), SplitLine[2]);

                                    #region Exceptions

                                    if (Item == null)
                                        throw ParseException.UnrecognizedTradeItem(SplitLine);

                                    #endregion

                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, Convert.ToUInt32(Item), 0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.TRADE_STATUS:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Status = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.TradeStatuses), SplitLine[2]);

                                    #region Exceptions

                                    if (Status == null)
                                        throw ParseException.UnrecognizedTradeStatus(SplitLine);

                                    #endregion

                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, Convert.ToUInt32(Status), 0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_MASK:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Mask = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.PlayerMasks), SplitLine[2]); ;

                                    #region Exceptions

                                    if (Mask == null)
                                        throw ParseException.UnrecognizedTradeStatus(SplitLine);

                                    #endregion

                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, Convert.ToUInt32(Mask), 0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.TIME_OF_DAY:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    byte Condition = ScriptHelpers.GetConditionID(SplitLine[2]);

                                    #region Exceptions

                                    if (Condition == 255)
                                        throw ParseException.UnrecognizedCondition(SplitLine);

                                    #endregion

                                    byte VarType = ScriptHelpers.GetVariable(SplitLine[3]);
                                    UInt32 Time = 0;

                                    if (VarType == (int)Lists.VarTypes.RNG)
                                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    if (VarType < (int)Lists.VarTypes.Var1)
                                    {
                                        UInt32 Value = Convert.ToUInt32(ParserHelpers.GetValueAndCheckRange(SplitLine,
                                                                                                            VarType == (int)Lists.VarTypes.RNG ? 4 : 3,
                                                                                                            0, UInt16.MaxValue));

                                        string[] HourMinute = SplitLine[3].Split(':');

                                        if (HourMinute.Length != 2)
                                            throw ParseException.BadTime(SplitLine);

                                        byte Hour = Convert.ToByte(ParserHelpers.GetValueAndCheckRange(HourMinute, 0, 0, 24));
                                        byte Min = Convert.ToByte(ParserHelpers.GetValueAndCheckRange(HourMinute, 1, 0, 59));

                                        Time = Convert.ToUInt16(((Hour * 60) + Min) * (Int16.MaxValue / 1440));
                                    }

                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Time, Condition, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.CURRENT_ANIMATION:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? AnimationID = ScriptHelpers.Helper_GetAnimationID(SplitLine[2], Entry.Animations);

                                    #region Exceptions

                                    if (AnimationID == null)
                                        throw ParseException.UnrecognizedAnimation(SplitLine);

                                    #endregion

                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, Convert.ToUInt32(AnimationID), 0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_BOMBBAG:
                                {
                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0,
                                                                  H_InventoryInstruction(SplitLine, typeof(Lists.BombBags), ParseException.UnrecognizedBombBag(SplitLine)),
                                                                  0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_QUIVER:
                                {
                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0,
                                                        H_InventoryInstruction(SplitLine, typeof(Lists.Quivers), ParseException.UnrecognizedQuiver(SplitLine)),
                                                        0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_WALLET:
                                {
                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0,
                                                        H_InventoryInstruction(SplitLine, typeof(Lists.Wallets), ParseException.UnrecognizedWallet(SplitLine)),
                                                        0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_DEKUNUTCAP:
                                {
                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0,
                                                        H_InventoryInstruction(SplitLine, typeof(Lists.NutCap), ParseException.UnrecognizedDekuNutCap(SplitLine)),
                                                        0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_STICKCAP:
                                {
                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0,
                                                        H_InventoryInstruction(SplitLine, typeof(Lists.StickCap), ParseException.UnrecognizedStickCap(SplitLine)),
                                                        0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_WATER_SCALE:
                                {
                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0,
                                                        H_InventoryInstruction(SplitLine, typeof(Lists.Scales), ParseException.UnrecognizedScale(SplitLine)),
                                                        0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            case (int)Lists.IfSubTypes.PLAYER_GAUNTLETS:
                                {
                                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0,
                                                        H_InventoryInstruction(SplitLine, typeof(Lists.Gauntlets), ParseException.UnrecognizedGauntlets(SplitLine)),
                                                        0, EndIf, Else, LabelR));
                                    return Instructions;
                                }
                            default:
                                throw new Exception();
                        }
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);

                    Instructions.Insert(0, new InstructionIfWhile((byte)ID, 0, 0, 0, 0, EndIf, Else, LabelR));
                    return Instructions;
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new List<Instruction>();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new List<Instruction>();
            }
        }

        private UInt32 H_InventoryInstruction(string[] SplitLine, Type Enumtype, ParseException ToThrow)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

            UInt32? Val = ScriptHelpers.Helper_GetEnumByName(Enumtype, SplitLine[2]);

            #region Exceptions

            if (Val == null)
                throw ToThrow;

            #endregion

            return (UInt32)Val;
        }

        private int GetCorrespondingElse(List<string> Lines, int LineSt, int LineEnd)
        {
            for (int i = LineSt + 1; i < LineEnd; i++)
            {
                if (Lines[i].Split(' ')[0].ToUpper() == Lists.Instructions.IF.ToString())
                {
                    i = GetCorrespondingEndIf(Lines, i);
                    continue;
                }

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_Else)
                    return i;
            }

            return -1;
        }

        private int GetCorrespondingEndIf(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].Split(' ')[0].ToUpper() == Lists.Instructions.IF.ToString())
                {
                    int j = i;

                    i = GetCorrespondingEndIf(Lines, i);

                    if (i < 0)
                        throw ParseException.IfNotClosed(Lines[j]);

                    continue;
                }

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndIf)
                    return i;
            }

            return -1;
        }

        private int GetCorrespondingEndWhile(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].Split(' ')[0].ToUpper() == Lists.Instructions.WHILE.ToString())
                {
                    int j = i;

                    i = GetCorrespondingEndWhile(Lines, i);

                    if (i < 0)
                        throw ParseException.IfNotClosed(Lines[j]);

                    continue;
                }

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndWhile)
                    return i;
            }

            return -1;
        }
    }
}
