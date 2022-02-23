using System;

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
                        case (int)Lists.AwaitSubTypes.FLAG_INF:
                        case (int)Lists.AwaitSubTypes.FLAG_EVENT:
                        case (int)Lists.AwaitSubTypes.FLAG_SWITCH:
                        case (int)Lists.AwaitSubTypes.FLAG_SCENE:
                        case (int)Lists.AwaitSubTypes.FLAG_TREASURE:
                        case (int)Lists.AwaitSubTypes.FLAG_ROOM_CLEAR:
                        case (int)Lists.AwaitSubTypes.FLAG_SCENE_COLLECT:
                        case (int)Lists.AwaitSubTypes.FLAG_TEMPORARY:
                        case (int)Lists.AwaitSubTypes.FLAG_INTERNAL:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);
                                int MaxFlagId = (SubID == (int)Lists.AwaitSubTypes.FLAG_INTERNAL ? 31 : UInt16.MaxValue);
                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, MaxFlagId);

                                return new InstructionAwait((byte)SubID, Val, Condition);
                            }
                        case (int)Lists.AwaitSubTypes.MOVEMENT_PATH_END:
                        case (int)Lists.AwaitSubTypes.RESPONSE:
                        case (int)Lists.AwaitSubTypes.TALKING_END:
                        case (int)Lists.AwaitSubTypes.FOREVER:
                        case (int)Lists.AwaitSubTypes.TEXTBOX_DISMISSED:
                        case (int)Lists.AwaitSubTypes.ANIMATION_END:
                        case (int)Lists.AwaitSubTypes.PLAYER_ANIMATION_END:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                                return new InstructionAwait((byte)SubID, new ScriptVarVal(), Lists.ConditionTypes.EQUALTO);
                            }
                        case (int)Lists.AwaitSubTypes.TEXTBOX_ON_SCREEN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);

                                Lists.ConditionTypes Condition = Lists.ConditionTypes.TRUE;

                                if (SplitLine.Length == 3)
                                    Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 2);

                                return new InstructionAwait((byte)SubID, new ScriptVarVal(), Condition);
                            }
                        case (int)Lists.AwaitSubTypes.FRAMES:
                        case (int)Lists.AwaitSubTypes.TEXTBOX_NUM:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt16.MaxValue);

                                return new InstructionAwait((byte)SubID, Val, Lists.ConditionTypes.EQUALTO);
                            }
                        case (int)Lists.AwaitSubTypes.PATH_NODE:
                        case (int)Lists.AwaitSubTypes.ANIMATION_FRAME:
                        case (int)Lists.AwaitSubTypes.CUTSCENE_FRAME:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);
                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue);

                                return new InstructionAwait((byte)SubID, Val, Lists.ConditionTypes.EQUALTO);
                            }
                        case (int)Lists.AwaitSubTypes.STICK_X:
                        case (int)Lists.AwaitSubTypes.STICK_Y:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                var Val = ScriptHelpers.GetScriptVarVal(SplitLine, 3, sbyte.MinValue, sbyte.MaxValue);

                                return new InstructionAwait((byte)SubID, Val, Condition);
                            }
                        case (int)Lists.AwaitSubTypes.BUTTON_HELD:
                        case (int)Lists.AwaitSubTypes.BUTTON_PRESSED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                var Val = new ScriptVarVal();
                                Val.Value = (float)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Buttons), ParseException.UnrecognizedButton(SplitLine));

                                return new InstructionAwait((byte)SubID, Val, Lists.ConditionTypes.EQUALTO);

                            }
                        case (int)Lists.AwaitSubTypes.TIME_OF_DAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);

                                var Val = new ScriptVarVal();
                                Val.Vartype = ScriptHelpers.GetVarType(SplitLine, 3);

                                if (Val.Vartype == (int)Lists.VarTypes.NORMAL)
                                    Val.Value = (float)Convert.ToDecimal(ScriptHelpers.GetOcarinaTime(SplitLine, 3));
                                else
                                    Val.Value = (float)ScriptHelpers.GetValueByType(SplitLine, 3, Val.Vartype, 0, UInt16.MaxValue);

                                return new InstructionAwait((byte)SubID, Val, Condition);
                            }
                        case (int)Lists.AwaitSubTypes.EXT_VAR:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 4);

                                var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 5, float.MinValue, float.MaxValue);
                                var NPCID = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt16.MaxValue);

                                byte ExtVarNum = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, 3, (int)Lists.VarTypes.NORMAL, 0, 5));

                                return new InstructionAwaitExtVar((byte)SubID, ExtVarNum, Value, NPCID, Condition);
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionNop();
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

            if (SubID != null && SubID != (byte)Lists.IfWhileAwaitSetRamSubTypes.RANDOM)
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);

                var Value1 = ScriptHelpers.GetScriptVarVal(SplitLine, 1, UInt32.MinValue, UInt32.MaxValue);
                var Value2 = ScriptHelpers.GetScriptVarVal(SplitLine, 3, float.MinValue, float.MaxValue);

                return new InstructionAwaitWithSecondValue((byte)SubID, Value1, Value2, Condition);
            }
            else
                return null;
        }
    }
}
