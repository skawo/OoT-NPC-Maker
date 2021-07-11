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
                        case (int)Lists.AwaitSubTypes.MOVEMENT_PATH_END:
                        case (int)Lists.AwaitSubTypes.TEXTBOX_RESPONSE:
                        case (int)Lists.AwaitSubTypes.TALKING_END:
                        case (int)Lists.AwaitSubTypes.NO_TEXTBOX_ON_SCREEN:
                        case (int)Lists.AwaitSubTypes.FOREVER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                                return new InstructionAwait((byte)SubID, 0, Lists.ConditionTypes.EQUALTO, 0);
                            }
                        case (int)Lists.AwaitSubTypes.FRAMES:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 Data = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, UInt16.MaxValue));

                                return new InstructionAwait((byte)SubID, Data, Lists.ConditionTypes.EQUALTO, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.CURRENT_PATH_NODE:
                        case (int)Lists.AwaitSubTypes.CURRENT_ANIMATION_FRAME:
                        case (int)Lists.AwaitSubTypes.CURRENT_CUTSCENE_FRAME:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                UInt32 Data = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue));

                                return new InstructionAwait((byte)SubID, Data, Lists.ConditionTypes.EQUALTO, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.STICK_X:
                        case (int)Lists.AwaitSubTypes.STICK_Y:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                Int32 Data = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, sbyte.MinValue, sbyte.MaxValue));

                                return new InstructionAwait((byte)SubID, Data, Condition, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.BUTTON_HELD:
                        case (int)Lists.AwaitSubTypes.BUTTON_PRESSED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                UInt32 Value = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Buttons), ParseException.UnrecognizedButton(SplitLine));

                                return new InstructionAwait((byte)SubID, Value, Lists.ConditionTypes.EQUALTO, 0);

                            }
                        case (int)Lists.AwaitSubTypes.TIME_OF_DAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                UInt32 Time = 0;

                                if (VarType == (int)Lists.VarTypes.Normal)
                                    Time = ScriptHelpers.GetOcarinaTime(SplitLine, 3);
                                else
                                    Time = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue));

                                return new InstructionAwait((byte)SubID, Time, Condition, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.EXT_VAR:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 4);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 5);
                                object Data = ScriptHelpers.GetValueByType(SplitLine, 5, VarType, float.MinValue, float.MaxValue);

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 NPCID = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, UInt16.MaxValue));

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
                UInt32 Value1 = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 1, VarType1, UInt32.MinValue, UInt32.MaxValue));

                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                UInt32 Value2 = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, Int32.MinValue, Int32.MaxValue));

                return new InstructionAwaitWithSecondValue((byte)SubID, Value1, Value2, Condition, VarType1, VarType2);
            }
            else
                return null;
        }
    }
}
