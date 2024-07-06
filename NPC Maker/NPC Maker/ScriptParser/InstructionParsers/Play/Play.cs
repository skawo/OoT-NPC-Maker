using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParsePlayInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.PlaySubTypes));

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.PlaySubTypes.SFX:
                        case (int)Lists.PlaySubTypes.SFX_GLOBAL:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 6);

                                ScriptVarVal SND = ScriptHelpers.Helper_GetSFXId(SplitLine, 2);

                                if (Convert.ToInt32(SND.Value) < 0)
                                    throw ParseException.ParamOutOfRange(SplitLine);

                                if (SplitLine.Length == 3)
                                    return new InstructionPlay((byte)SubID, SND);
                                else
                                {
                                    ScriptVarVal Volume = new ScriptVarVal() { Value = 1.0f, Vartype = (byte)Lists.VarTypes.NORMAL };
                                    ScriptVarVal Pitch = new ScriptVarVal() { Value = 1.0f, Vartype = (byte)Lists.VarTypes.NORMAL };
                                    ScriptVarVal Reverb = new ScriptVarVal() { Value = 0, Vartype = (byte)Lists.VarTypes.NORMAL };

                                    if (SplitLine.Length >= 4)
                                        Volume = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0.0f, 2.0f);
                                    if (SplitLine.Length >= 5)
                                        Pitch = ScriptHelpers.GetScriptVarVal(SplitLine, 4, 0.0f, 2.0f);
                                    if (SplitLine.Length == 6)
                                        Reverb = ScriptHelpers.GetScriptVarVal(SplitLine, 5, -127, 127);

                                    return new InstructionPlayWithParams((byte)Lists.PlaySubTypes.SFX_WITH_PARAMS, SND, Volume, Pitch, Reverb);
                                }
                            }
                        case (int)Lists.PlaySubTypes.BGM:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                ScriptVarVal SND = ScriptHelpers.Helper_GetMusicId(SplitLine, 2);

                                if (Convert.ToInt32(SND.Value) < 0)
                                    throw ParseException.ParamOutOfRange(SplitLine);

                                return new InstructionPlay((byte)SubID, SND);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                                return new InstructionPlay((byte)SubID, new ScriptVarVal());
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                var ID = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, Int32.MaxValue);

                                return new InstructionPlay((byte)SubID, ID);
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
    }
}
