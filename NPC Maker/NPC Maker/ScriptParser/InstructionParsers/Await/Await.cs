﻿using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseAwaitInstruction(string[] SplitLine)
        {
            try
            {
                try
                {
                    Instruction AwaitRam = H_AwaitRam(SplitLine);

                    if (AwaitRam != null)
                        return AwaitRam;

                    int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.AwaitSubTypes));

                    switch (SubID)
                    {
                        case (int)Lists.AwaitSubTypes.MOVEMENT_PATH_END:
                        case (int)Lists.AwaitSubTypes.TEXTBOX_RESPONSE:
                        case (int)Lists.AwaitSubTypes.TALKING_END:
                        case (int)Lists.AwaitSubTypes.FOREVER:
                        case (int)Lists.AwaitSubTypes.TEXTBOX_DISMISSED:
                        case (int)Lists.AwaitSubTypes.ANIMATION_END:
                        case (int)Lists.AwaitSubTypes.PLAYER_ANIMATION_END:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                                return new InstructionAwait((byte)SubID, 0, Lists.ConditionTypes.EQUALTO, 0);
                            }
                        case (int)Lists.AwaitSubTypes.TEXTBOX_ON_SCREEN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);

                                Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

                                if (SplitLine.Length == 3)
                                    Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 2);

                                return new InstructionAwait((byte)SubID, 0, Condition, 0);
                            }
                        case (int)Lists.AwaitSubTypes.FRAMES:
                        case (int)Lists.AwaitSubTypes.TEXTBOX_NUM:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                object Data = ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, UInt16.MaxValue);

                                return new InstructionAwait((byte)SubID, Data, Lists.ConditionTypes.EQUALTO, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.CURRENT_PATH_NODE:
                        case (int)Lists.AwaitSubTypes.CURRENT_ANIMATION_FRAME:
                        case (int)Lists.AwaitSubTypes.CURRENT_CUTSCENE_FRAME:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Data = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue);

                                return new InstructionAwait((byte)SubID, Data, Lists.ConditionTypes.EQUALTO, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.STICK_X:
                        case (int)Lists.AwaitSubTypes.STICK_Y:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Data = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, sbyte.MinValue, sbyte.MaxValue);

                                return new InstructionAwait((byte)SubID, Data, Condition, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.BUTTON_HELD:
                        case (int)Lists.AwaitSubTypes.BUTTON_PRESSED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                object Value = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Buttons), ParseException.UnrecognizedButton(SplitLine));

                                return new InstructionAwait((byte)SubID, Value, Lists.ConditionTypes.EQUALTO, 0);

                            }
                        case (int)Lists.AwaitSubTypes.TIME_OF_DAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Time = 0;

                                if (VarType == (int)Lists.VarTypes.Normal)
                                    Time = (float)Convert.ToDecimal(ScriptHelpers.GetOcarinaTime(SplitLine, 3));
                                else
                                    Time = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue);

                                return new InstructionAwait((byte)SubID, Time, Condition, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.EXT_VAR:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 4);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 5);
                                object Data = ScriptHelpers.GetValueByType(SplitLine, 5, VarType, float.MinValue, float.MaxValue);

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 2);
                                object NPCID = ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, UInt16.MaxValue);

                                byte ExtVarNum = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, 3, (int)Lists.VarTypes.Normal, 0, 5));

                                return new InstructionAwaitExtVar((byte)SubID, ExtVarNum, Data, NPCID, Condition, VarType, VarType2);
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionAwait(0, 0, 0, 0);
                }
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }

        private Instruction H_AwaitRam(string[] SplitLine)
        {
            byte? SubID = ScriptHelpers.GetSubIDForRamType(SplitLine[1]);

            if (SubID != null)
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);

                byte VarType1 = ScriptHelpers.GetVarType(SplitLine, 1);
                object Value1 = ScriptHelpers.GetValueByType(SplitLine, 1, VarType1, UInt32.MinValue, UInt32.MaxValue);

                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                object Value2 = ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, float.MinValue, float.MaxValue);

                return new InstructionAwaitWithSecondValue((byte)SubID, Value1, Value2, Condition, VarType1, VarType2);
            }
            else
                return null;
        }
    }
}