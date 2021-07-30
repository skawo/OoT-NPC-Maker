using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private List<Instruction> ParseIfWhileInstruction(int ID, List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                List<Instruction> Instructions = new List<Instruction>();
                string LabelR = ScriptDataHelpers.RandomString(this);

                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

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

                            Instructions.Add(new InstructionLabel("__IFTRUE__" + LabelR));
                            Instructions.AddRange(GetInstructions(Lines.Skip(LineNo + 1).Take(Else - LineNo - 1).ToList()));
                            Instructions.Add(new InstructionAwait((byte)Lists.AwaitSubTypes.FRAMES, 1, Lists.ConditionTypes.EQUALTO, (byte)Lists.VarTypes.Normal));
                            Instructions.Add(new InstructionGoto("__IFTRUE__" + LabelR));
                            Instructions.Add(new InstructionLabel("__IFFALSE__" + LabelR));
                            break;
                        }
                    default:
                        throw ParseException.GeneralError(SplitLine);
                }

                LineNo = EndIf;

                #endregion

                try
                {
                    Instruction Ram = H_IfRam(ID, SplitLine, EndIf, Else, LabelR);

                    if (Ram != null)
                    {
                        Instructions.Insert(0, Ram);
                        return Instructions;
                    }

                    int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.IfSubTypes));

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
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                object FlagID = ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, UInt16.MaxValue);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, FlagID, Condition,
                                                                              EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.LINK_IS_ADULT:
                        case (int)Lists.IfSubTypes.CURRENTLY_DAY:
                        case (int)Lists.IfSubTypes.TALKED_TO:
                        case (int)Lists.IfSubTypes.PLAYER_HAS_EMPTY_BOTTLE:
                        case (int)Lists.IfSubTypes.CUTSCENE_BEING_PLAYED:
                        case (int)Lists.IfSubTypes.TEXTBOX_ON_SCREEN:
                        case (int)Lists.IfSubTypes.CURRENT_ANIM_WALKING:
                        case (int)Lists.IfSubTypes.PLAYER_HAS_MAGIC:
                        case (int)Lists.IfSubTypes.ATTACKED:
                        case (int)Lists.IfSubTypes.TARGETTED:
                        case (int)Lists.IfSubTypes.LENS_OF_TRUTH_ON:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                                Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

                                if (SplitLine.Length == 3)
                                    Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 2);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, 0, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_RUPEES:
                        case (int)Lists.IfSubTypes.SCENE_ID:
                        case (int)Lists.IfSubTypes.PLAYER_SKULLTULAS:
                        case (int)Lists.IfSubTypes.CURRENT_PATH_NODE:
                        case (int)Lists.IfSubTypes.CURRENT_ANIMATION_FRAME:
                        case (int)Lists.IfSubTypes.CURRENT_CUTSCENE_FRAME:
                        case (int)Lists.IfSubTypes.PLAYER_HEALTH:
                        case (int)Lists.IfSubTypes.PLAYER_BEANS:
                        case (int)Lists.IfSubTypes.PLAYER_BOMBS:
                        case (int)Lists.IfSubTypes.PLAYER_BOMBCHUS:
                        case (int)Lists.IfSubTypes.PLAYER_ARROWS:
                        case (int)Lists.IfSubTypes.PLAYER_DEKUSTICKS:
                        case (int)Lists.IfSubTypes.PLAYER_DEKUNUTS:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Value = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Value, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.DISTANCE_FROM_PLAYER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Value = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, float.MinValue, float.MaxValue);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Value, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.EXT_VAR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 4);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 5);
                                object Value = ScriptHelpers.GetValueByType(SplitLine, 5, VarType, float.MinValue, float.MaxValue);

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 2);
                                object ActorID = ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, Int16.MaxValue);

                                byte ExtVarNum = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, 3, (int)Lists.VarTypes.Normal, 1, Lists.Num_User_Vars));

                                Instructions.Insert(0, new InstructionIfWhileExtVar((byte)ID, Convert.ToByte(SubID), ExtVarNum, VarType, Value, VarType2, ActorID, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.STICK_X:
                        case (int)Lists.IfSubTypes.STICK_Y:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Value = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, sbyte.MinValue, sbyte.MaxValue);

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
                                Instructions.Insert(0, H_IfWhileBoolEnum(ID, SubID, SplitLine, typeof(Lists.TradeStatuses), EndIf, Else, LabelR, ParseException.UnrecognizedTradeStatus(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_MASK:
                            {
                                Instructions.Insert(0, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.PlayerMasks), EndIf, Else, LabelR, ParseException.UnrecognizedMask(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.TIME_OF_DAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Time = 0;

                                if (VarType == (int)Lists.VarTypes.Normal)
                                    Time = (float)Convert.ToDecimal(ScriptHelpers.GetOcarinaTime(SplitLine, 3));
                                else
                                    Time = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Time, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.CURRENT_ANIMATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte Vartype = ScriptHelpers.GetVarType(SplitLine, 2);
                                object AnimationID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Vartype, Entry.Animations);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Vartype, AnimationID, Lists.ConditionTypes.EQUALTO, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_HAS_DUNGEON_ITEM:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 2);
                                object Value2 = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 2, VarType2, typeof(Lists.DungeonItems), ParseException.UnrecognizedDungeonItem(SplitLine));

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Dungeon = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 4);

                                Instructions.Insert(0, new InstructionIfWhileWithSecondValue((byte)ID, Convert.ToByte(SubID), VarType, Dungeon, VarType2, Value2, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_HAS_INVENTORY_ITEM:
                            {
                                Instructions.Insert(0, H_IfWhileBoolEnum(ID, SubID, SplitLine, typeof(Lists.Items), EndIf, Else, LabelR, ParseException.UnrecognizedInventoryItem(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_HAS_QUEST_ITEM:
                            {
                                Instructions.Insert(0, H_IfWhileBoolEnum(ID, SubID, SplitLine, typeof(Lists.QuestItems), EndIf, Else, LabelR, ParseException.UnrecognizedQuestItem(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.BUTTON_HELD:
                        case (int)Lists.IfSubTypes.BUTTON_PRESSED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);
                                object Value = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Buttons));

                                Lists.ConditionTypes Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);

                                Instructions.Insert(0, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), 0, (UInt32)Value, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
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
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new List<Instruction>();
            }
        }

        private Instruction H_IfRam(int ID, string[] SplitLine, int EndIf, int Else, string LabelR)
        {
            byte? SubID = ScriptHelpers.GetSubIDForRamType(SplitLine[1]);

            if (SubID != null)
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);

                byte VarType1 = ScriptHelpers.GetVarType(SplitLine, 1);
                object Value1 = ScriptHelpers.GetValueByType(SplitLine, 1, VarType1, UInt32.MinValue, UInt32.MaxValue);

                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                object Value2 = ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, Int32.MinValue, Int32.MaxValue);

                return new InstructionIfWhileWithSecondValue((byte)ID, (byte)SubID, VarType1, Value1, VarType2, Value2, Condition, EndIf, Else, LabelR);
            }
            else
                return null;
        }

        private Instruction H_IfWhileBoolEnum(int ID, int SubID, string[] SplitLine, Type Enumtype, int EndIf, int Else, string LabelR, ParseException Throw)
        {
            Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

            if (SplitLine.Length == 4)
                Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);

            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

            byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
            object Value = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 2, VarType, Enumtype, Throw);

            return new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Value, Condition, EndIf, Else, LabelR);
        }


        private Instruction H_IfWhileEnum(int ID, int SubID, string[] SplitLine, Type Enumtype, int EndIf, int Else, string LabelR, ParseException Throw)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

            byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
            object Value = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 2, VarType, Enumtype, Throw);

            return new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), VarType, Value, Lists.ConditionTypes.EQUALTO, EndIf, Else, LabelR);
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

        public int GetCorrespondingEndIf(List<string> Lines, int LineNo)
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
