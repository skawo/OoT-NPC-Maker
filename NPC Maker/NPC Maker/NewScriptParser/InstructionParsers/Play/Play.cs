using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParsePlayInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = (int)System.Enum.Parse(typeof(Lists.PlaySubTypes), SplitLine[1].ToUpper());

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.PlaySubTypes.SFX:
                        case (int)Lists.PlaySubTypes.BGM:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVariable(SplitLine[2]);

                                if (VarType == (int)Lists.VarTypes.Keyword_RNG)
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                UInt32? SNDID = 0;

                                if (VarType < (int)Lists.VarTypes.Keyword_ScriptVar1)
                                {
                                    SNDID = (SubID == (int)Lists.PlaySubTypes.SFX) ?
                                                           ScriptHelpers.Helper_GetSFXId(SplitLine[2])
                                                                                   :
                                                           ScriptHelpers.Helper_GetMusicId(SplitLine[2]);

                                    if (SNDID == null)
                                        SNDID = Convert.ToUInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0,
                                                                                                     (SubID == (int)Lists.PlaySubTypes.SFX) ? 
                                                                                                            Lists.SFXes.Max(x => x).Value
                                                                                                                    :
                                                                                                            Lists.Music.Max(x => x).Value));
                                }


                                return new InstructionPlay((byte)SubID, Convert.ToUInt16(SNDID), VarType);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                                return new InstructionPlay((byte)SubID, 0, 0);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE_ADDR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                UInt32 Addr = Convert.ToUInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, Int32.MaxValue));

                                return new InstructionPlay((byte)SubID, Addr, 0);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                byte ID = Convert.ToByte(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, byte.MaxValue));

                                return new InstructionPlay((byte)SubID, ID, 0);
                            }
                        default: throw new Exception();
                    }


                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, 0, 0);
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
