using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseIfInstruction(List<string> Lines, string[] SplitLine, int LineNo)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                int SubID = (int)System.Enum.Parse(typeof(Lists.IfSubTypes), SplitLine[1].ToUpper());

                int EndIf = GetCorrespondingEndIf(Lines, LineNo);
                int Else = GetCorrespondingElse(Lines, LineNo, EndIf);

                if (EndIf == -1)
                    throw ParseException.IfNotClosed(SplitLine);

                if (Else == -1)
                    Else = EndIf;

                List<Instruction> True = GetInstructions(Lines.Skip(LineNo + 1).Take(Else - LineNo - 1).ToList());
                List<Instruction> False = Else == EndIf ? new List<Instruction>() : GetInstructions(Lines.Skip(Else + 1).Take(EndIf - Else - 1).ToList());

                try
                {
                    // Flag Types
                    if (SubID <= 7)
                    {
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                        UInt16 FlagID = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                        return new InstructionIf(Convert.ToByte(SubID), 0, FlagID, byte.MaxValue, EndIf, Else, True, False);
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

                        return new InstructionIf(Convert.ToByte(SubID), 0, 0, Condition, EndIf, Else, True, False);
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

                       byte ValueType = ScriptHelpers.GetVariable(SplitLine[3]);

                        if (ValueType != 0)
                            return new InstructionIf(Convert.ToByte(SubID), ValueType, (byte)0, Condition, EndIf, Else, True, False);
                        else
                        {
                            UInt32 Value = Convert.ToUInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));
                            return new InstructionIf(Convert.ToByte(SubID), 0, Value, Condition, EndIf, Else, True, False);
                        }
                    }
                    // Special
                    else
                    {
                        switch (SubID)
                        {
                            case (int)Lists.IfSubTypes.ITEM_BEING_TRADED:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Item = ScriptHelpers.Helper_GetTradeItemId(SplitLine[2]);

                                    #region Exceptions

                                    if (Item == null)
                                        throw ParseException.UnrecognizedTradeItem(SplitLine);

                                    #endregion

                                    return new InstructionIf(Convert.ToByte(SubID), 0, Convert.ToUInt32(Item), 0, EndIf, Else, True, False);
                                }
                            case (int)Lists.IfSubTypes.TRADE_STATUS:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Status = ScriptHelpers.Helper_GetTradeStatusID(SplitLine[2]);

                                    #region Exceptions

                                    if (Status == null)
                                        throw ParseException.UnrecognizedTradeStatus(SplitLine);

                                    #endregion

                                    return new InstructionIf(Convert.ToByte(SubID), 0, Convert.ToUInt32(Status), 0, EndIf, Else, True, False);
                                }
                            case (int)Lists.IfSubTypes.PLAYER_MASK:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Mask = ScriptHelpers.Helper_GetMaskID(SplitLine[2]);

                                    #region Exceptions

                                    if (Mask == null)
                                        throw ParseException.UnrecognizedTradeStatus(SplitLine);

                                    #endregion

                                    return new InstructionIf(Convert.ToByte(SubID), 0, Convert.ToUInt32(Mask), 0, EndIf, Else, True, False);
                                }
                            case (int)Lists.IfSubTypes.TIME_OF_DAY:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                    Int32 Time = Convert.ToInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, Int16.MinValue, Int16.MaxValue));
                                    return new InstructionIf(Convert.ToByte(SubID), 0, Time, 0, EndIf, Else, True, False);
                                }
                            case (int)Lists.IfSubTypes.CURRENT_ANIMATION:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? AnimationID = ScriptHelpers.Helper_GetAnimationID(SplitLine[2], Entry.Animations);

                                    #region Exceptions

                                    if (AnimationID == null)
                                        throw ParseException.UnrecognizedAnimation(SplitLine);

                                    #endregion

                                    return new InstructionIf(Convert.ToByte(SubID), 0, Convert.ToUInt32(AnimationID), 0, EndIf, Else, True, False);
                                }
                            default:
                                throw new Exception();
                        }
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionIf(0, 0, 0, 0, EndIf, Else, new List<Instruction>(), new List<Instruction>());
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionNop();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
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

                if (Lines[i].ToUpper().Trim() == "ELSE")
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
                    i = GetCorrespondingEndIf(Lines, i);
                    continue;
                }

                if (Lines[i].ToUpper().Trim() == "ENDIF")
                    return i;
            }

            return -1;
        }
    }
}
