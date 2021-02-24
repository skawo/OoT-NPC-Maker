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

                #region Setup

                switch (ID)
                {
                    case (int)Lists.Instructions.IF:
                        {
                            EndIf = GetCorrespondingEndIf(Lines, LineNo);
                            Else = GetCorrespondingElse(Lines, LineNo, EndIf);

                            if (EndIf < 0)
                                throw ParseException.IfNotClosed(SplitLine);

                            if (Else < 0)
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

                            break;
                        }
                    case (int)Lists.Instructions.WHILE:
                        {
                            EndIf = GetCorrespondingEndWhile(Lines, LineNo);

                            if (EndIf == -1)
                                throw ParseException.WhileNotClosed(SplitLine);

                            Else = EndIf;

                            Instructions.Add(new InstructionLabel("__WHILE__" + LabelR));
                            Instructions.AddRange(GetInstructions(Lines.Skip(LineNo + 1).Take(Else - LineNo - 1).ToList()));
                            Instructions.Add(new InstructionGoto("__RETURN__"));
                            Instructions.Add(new InstructionGoto("__WHILE__" + LabelR));
                            break;
                        }
                    default:
                        throw ParseException.GeneralError(SplitLine);
                }

                LineNo = EndIf;

                #endregion

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.IfSubTypes.FLAG_INF:
                        case (int)Lists.IfSubTypes.FLAG_EVENT:
                        case (int)Lists.IfSubTypes.FLAG_SWITCH:
                        case (int)Lists.IfSubTypes.FLAG_SCENE:
                        case (int)Lists.IfSubTypes.FLAG_TREASURE:
                        case (int)Lists.IfSubTypes.FLAG_ROOM_CLEAR:
                        case (int)Lists.IfSubTypes.FLAG_SCENE_COLLECT:
                        case (int)Lists.IfSubTypes.FLAG_TEMPORARY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                UInt16 FlagID = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, FlagID, Lists.ConditionTypes.NONE, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.LINK_IS_ADULT:
                        case (int)Lists.IfSubTypes.CURRENTLY_DAY:
                        case (int)Lists.IfSubTypes.CURRENTLY_TALKING:
                        case (int)Lists.IfSubTypes.PLAYER_HAS_EMPTY_BOTTLE:
                        case (int)Lists.IfSubTypes.CUTSCENE_BEING_PLAYED:
                        case (int)Lists.IfSubTypes.TEXTBOX_ON_SCREEN:
                        case (int)Lists.IfSubTypes.CURRENT_ANIM_WALKING:
                        case (int)Lists.IfSubTypes.PLAYER_HAS_MAGIC:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                Lists.ConditionTypes Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 2);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, 0, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_RUPEES:
                        case (int)Lists.IfSubTypes.SCENE_ID:
                        case (int)Lists.IfSubTypes.PLAYER_SKULLTULAS:
                        case (int)Lists.IfSubTypes.CURRENT_PATH_NODE:
                        case (int)Lists.IfSubTypes.CURRENT_ANIMATION_FRAME:
                        case (int)Lists.IfSubTypes.CURRENT_CUTSCENE_FRAME:
                        case (int)Lists.IfSubTypes.VAR_1:
                        case (int)Lists.IfSubTypes.VAR_2:
                        case (int)Lists.IfSubTypes.VAR_3:
                        case (int)Lists.IfSubTypes.VAR_4:
                        case (int)Lists.IfSubTypes.VAR_5:
                        case (int)Lists.IfSubTypes.PLAYER_HEALTH:
                        case (int)Lists.IfSubTypes.PLAYER_MAGIC:
                        case (int)Lists.IfSubTypes.PLAYER_BOMBS:
                        case (int)Lists.IfSubTypes.PLAYER_BOMBCHUS:
                        case (int)Lists.IfSubTypes.PLAYER_ARROWS:
                        case (int)Lists.IfSubTypes.PLAYER_DEKUSTICKS:
                        case (int)Lists.IfSubTypes.PLAYER_DEKUNUTS:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVariable(SplitLine, 3);
                                UInt32 Value = 0;

                                if (VarType == (int)Lists.VarTypes.RNG)
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                if (VarType < (int)Lists.VarTypes.Var1)
                                    Value = Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRange(SplitLine,
                                                                                                 VarType == (int)Lists.VarTypes.RNG ? 4 : 3, 0, UInt16.MaxValue));

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Value, Condition, EndIf, Else, LabelR));
                                return Instructions;

                            }
                        case (int)Lists.IfSubTypes.ITEM_BEING_TRADED:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.TradeItems), EndIf, Else, LabelR, ParseException.UnrecognizedTradeItem(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.TRADE_STATUS:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.TradeStatuses), EndIf, Else, LabelR, ParseException.UnrecognizedTradeStatus(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_MASK:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.PlayerMasks), EndIf, Else, LabelR, ParseException.UnrecognizedMask(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.TIME_OF_DAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVariable(SplitLine, 3);
                                UInt32 Time = 0;

                                if (VarType == 0)
                                    Time = ScriptHelpers.GetOcarinaTime(SplitLine, 3);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Time, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.CURRENT_ANIMATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                UInt32? AnimationID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, Convert.ToUInt32(AnimationID), 0, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_BOMBBAG:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.BombBags), EndIf, Else, LabelR, ParseException.UnrecognizedBombBag(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_QUIVER:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.Quivers), EndIf, Else, LabelR, ParseException.UnrecognizedQuiver(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_WALLET:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.Wallets), EndIf, Else, LabelR, ParseException.UnrecognizedWallet(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_DEKUNUTCAP:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.NutCap), EndIf, Else, LabelR, ParseException.UnrecognizedDekuNutCap(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_STICKCAP:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.StickCap), EndIf, Else, LabelR, ParseException.UnrecognizedStickCap(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_WATER_SCALE:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.Scales), EndIf, Else, LabelR, ParseException.UnrecognizedScale(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_GAUNTLETS:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.Gauntlets), EndIf, Else, LabelR, ParseException.UnrecognizedGauntlets(SplitLine)));
                                return Instructions;
                            }
                        default:
                            throw new Exception();
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

        private Instruction H_IfWhileEnum(int ID, int SubID, string[] SplitLine, Type Enumtype, int EndIf, int Else, string LabelR, ParseException ToThrow)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

            UInt32? Value = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, Enumtype, ToThrow);

            return new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, Convert.ToByte(Value), 0, EndIf, Else, LabelR);
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
