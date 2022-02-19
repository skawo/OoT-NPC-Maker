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
                        case (int)Lists.PlaySubTypes.BGM:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                ScriptVarVal SND = (SubID == (int)Lists.PlaySubTypes.SFX) ?
                                                       ScriptHelpers.Helper_GetSFXId(SplitLine, 2)
                                                                               :
                                                       ScriptHelpers.Helper_GetMusicId(SplitLine, 2);

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
