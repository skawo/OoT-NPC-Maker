using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseAwaitInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.AwaitSubTypes));

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.AwaitSubTypes.MOVEMENT_PATH_END:
                        case (int)Lists.AwaitSubTypes.TEXTBOX_RESPONSE:
                        case (int)Lists.AwaitSubTypes.TALKING_END:
                        case (int)Lists.AwaitSubTypes.NO_TEXTBOX_ON_SCREEN:
                        case (int)Lists.AwaitSubTypes.FOREVER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                                return new InstructionAwait((byte)SubID, Convert.ToByte(0), 0);
                            }
                        case (int)Lists.AwaitSubTypes.CURRENT_PATH_NODE:
                        case (int)Lists.AwaitSubTypes.FRAMES:
                        case (int)Lists.AwaitSubTypes.CURRENT_ANIMATION_FRAME:
                        case (int)Lists.AwaitSubTypes.CURRENT_CUTSCENE_FRAME:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                byte VarType = ScriptHelpers.GetVariable(SplitLine, 3);
                                float Data = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue);

                                return new InstructionAwait((byte)SubID, Data, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.STICK_X:
                        case (int)Lists.AwaitSubTypes.STICK_Y:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVariable(SplitLine, 3);
                                float Data = ScriptHelpers.GetValueByType(SplitLine, 3, VarType, sbyte.MinValue, sbyte.MaxValue);

                                return new InstructionAwaitValue((byte)SubID, Data, Condition, VarType);
                            }
                        case (int)Lists.AwaitSubTypes.BUTTON_HELD:
                        case (int)Lists.AwaitSubTypes.BUTTON_PRESSED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                UInt32? Value = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Buttons), ParseException.UnrecognizedButton(SplitLine));

                                return new InstructionAwait((byte)SubID, Value, 0);

                            }
                        case (int)Lists.AwaitSubTypes.VAR_1:
                        case (int)Lists.AwaitSubTypes.VAR_2:
                        case (int)Lists.AwaitSubTypes.VAR_3:
                        case (int)Lists.AwaitSubTypes.VAR_4:
                        case (int)Lists.AwaitSubTypes.VAR_5:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                Lists.ConditionTypes Condition = ScriptHelpers.GetConditionID(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVariable(SplitLine, 3);
                                float Data = ScriptHelpers.GetValueByType(SplitLine, 3, VarType);

                                return new InstructionAwaitValue((byte)SubID, Data, Condition, VarType);
                            }
                        default: 
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionSet((byte)SubID, 0, 0);
                }
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
