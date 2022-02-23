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
                int InsertIdx = 0;

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

                            Instructions.Add(new InstructionLabel("__WHILESTART__" + LabelR));
                            Instructions.Add(new InstructionLabel("__IFTRUE__" + LabelR));
                            Instructions.AddRange(GetInstructions(Lines.Skip(LineNo + 1).Take(Else - LineNo - 1).ToList()));
                            Instructions.Add(new InstructionGoto("__WHILESTART__" + LabelR));
                            Instructions.Add(new InstructionLabel("__IFFALSE__" + LabelR));
                            InsertIdx = 1;
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
                        Instructions.Insert(InsertIdx, Ram);
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
                        case (int)Lists.IfSubTypes.FLAG_INTERNAL:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);
                                int MaxFlagId = (SubID == (int)Lists.IfSubTypes.FLAG_INTERNAL ? 31 : UInt16.MaxValue);
                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, MaxFlagId);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Val, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.LINK_IS_ADULT:
                        case (int)Lists.IfSubTypes.IS_DAY:
                        case (int)Lists.IfSubTypes.IS_TALKING:
                        case (int)Lists.IfSubTypes.PLAYER_HAS_EMPTY_BOTTLE:
                        case (int)Lists.IfSubTypes.IN_CUTSCENE:
                        case (int)Lists.IfSubTypes.TEXTBOX_ON_SCREEN:
                        case (int)Lists.IfSubTypes.PLAYER_HAS_MAGIC:
                        case (int)Lists.IfSubTypes.ATTACKED:
                        case (int)Lists.IfSubTypes.TARGETTED:
                        case (int)Lists.IfSubTypes.LENS_OF_TRUTH_ON:
                        case (int)Lists.IfSubTypes.TEXTBOX_DRAWING:
                        case (int)Lists.IfSubTypes.REF_ACTOR_EXISTS:
                        case (int)Lists.IfSubTypes.PICKUP_IDLE:
                        case (int)Lists.IfSubTypes.PICKUP_THROWN:
                        case (int)Lists.IfSubTypes.PICKUP_LANDED:
                        case (int)Lists.IfSubTypes.PICKUP_PICKED_UP:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                                Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

                                if (SplitLine.Length == 3)
                                    Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 2);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), new ScriptVarVal(), Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_RUPEES:
                        case (int)Lists.IfSubTypes.SCENE_ID:
                        case (int)Lists.IfSubTypes.ROOM_ID:
                        case (int)Lists.IfSubTypes.PLAYER_SKULLTULAS:
                        case (int)Lists.IfSubTypes.PATH_NODE:
                        case (int)Lists.IfSubTypes.ANIMATION_FRAME:
                        case (int)Lists.IfSubTypes.CUTSCENE_FRAME:
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

                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Val, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.DISTANCE_FROM_PLAYER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);

                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 3, float.MinValue, float.MaxValue);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Val, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.EXT_VAR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 4);

                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 5, float.MinValue, float.MaxValue);
                                var ActorID = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, Int16.MaxValue);

                                byte ExtVarNum = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, 3, (int)Lists.VarTypes.NORMAL, 1, Lists.Num_User_Vars));

                                Instructions.Insert(InsertIdx, new InstructionIfWhileExtVar((byte)ID, Convert.ToByte(SubID), ExtVarNum, Val, ActorID, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.STICK_X:
                        case (int)Lists.IfSubTypes.STICK_Y:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 3, sbyte.MinValue, sbyte.MaxValue);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Value, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.ITEM_BEING_TRADED:
                            {
                                Instructions.Insert(InsertIdx, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.TradeItems), EndIf, Else, LabelR, ParseException.UnrecognizedTradeItem(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.TRADE_STATUS:
                            {
                                Instructions.Insert(InsertIdx, H_IfWhileBoolEnum(ID, SubID, SplitLine, typeof(Lists.TradeStatuses), EndIf, Else, LabelR, ParseException.UnrecognizedTradeStatus(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_MASK:
                            {
                                Instructions.Insert(InsertIdx, H_IfWhileEnum(ID, SubID, SplitLine, typeof(Lists.PlayerMasks), EndIf, Else, LabelR, ParseException.UnrecognizedMask(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.TIME_OF_DAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);

                                var Time = new ScriptVarVal();
                                Time.Vartype = ScriptHelpers.GetVarType(SplitLine, 3);

                                if (Time.Vartype == (int)Lists.VarTypes.NORMAL)
                                    Time.Value = (float)Convert.ToDecimal(ScriptHelpers.GetOcarinaTime(SplitLine, 3));
                                else
                                    Time.Value = (float)ScriptHelpers.GetValueByType(SplitLine, 3, Time.Vartype, 0, UInt16.MaxValue);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Time, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.ANIMATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                var AnimationID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), AnimationID, Lists.ConditionTypes.EQUALTO, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_HAS_DUNGEON_ITEM:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 4, 5);

                                var Value2 = ScriptHelpers.GetScriptVarVal(SplitLine, 2, typeof(Lists.DungeonItems), ParseException.UnrecognizedDungeonItem(SplitLine));
                                var Dungeon = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue);

                                Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

                                if (SplitLine.Length == 5)
                                    Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 4);

                                Instructions.Insert(InsertIdx, new InstructionIfWhileWithSecondValue((byte)ID, Convert.ToByte(SubID), Dungeon, Value2, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_HAS_INVENTORY_ITEM:
                            {
                                Instructions.Insert(InsertIdx, H_IfWhileBoolEnum(ID, SubID, SplitLine, typeof(Lists.Items), EndIf, Else, LabelR, ParseException.UnrecognizedInventoryItem(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.PLAYER_HAS_QUEST_ITEM:
                            {
                                Instructions.Insert(InsertIdx, H_IfWhileBoolEnum(ID, SubID, SplitLine, typeof(Lists.QuestItems), EndIf, Else, LabelR, ParseException.UnrecognizedQuestItem(SplitLine)));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.BUTTON_HELD:
                        case (int)Lists.IfSubTypes.BUTTON_PRESSED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 4);
                                var Value = new ScriptVarVal();
                                Value.Value = (float)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Buttons), ParseException.UnrecognizedButton(SplitLine));

                                Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

                                if (SplitLine.Length == 4)
                                    Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Value, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        case (int)Lists.IfSubTypes.DAMAGED_BY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                var Value = new ScriptVarVal();
                                Value.Value = (float)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.DamageTypes), ParseException.UnrecognizedDamage(SplitLine));

                                Lists.ConditionTypes Condition = Lists.ConditionTypes.EQUALTO;

                                Instructions.Insert(InsertIdx, new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Value, Condition, EndIf, Else, LabelR));
                                return Instructions;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new List<Instruction>();
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

                var Value1 = ScriptHelpers.GetScriptVarVal(SplitLine, 1, float.MinValue, float.MaxValue);
                var Value2 = ScriptHelpers.GetScriptVarVal(SplitLine, 3, float.MinValue, float.MaxValue);

                return new InstructionIfWhileWithSecondValue((byte)ID, (byte)SubID, Value1, Value2, Condition, EndIf, Else, LabelR);
            }
            else
                return null;
        }

        private Instruction H_IfWhileBoolEnum(int ID, int SubID, string[] SplitLine, Type Enumtype, int EndIf, int Else, string LabelR, ParseException Throw)
        {
            Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 4);

            if (SplitLine.Length == 4)
                Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);

            var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 2, Enumtype, Throw);

            return new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Value, Condition, EndIf, Else, LabelR);
        }

        private Instruction H_IfWhileEnum(int ID, int SubID, string[] SplitLine, Type Enumtype, int EndIf, int Else, string LabelR, ParseException Throw)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
            var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 2, Enumtype, Throw);

            return new InstructionIfWhile((byte)ID, Convert.ToByte(SubID), Value, Lists.ConditionTypes.EQUALTO, EndIf, Else, LabelR);
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
