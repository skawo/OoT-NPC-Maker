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
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.PlaySubTypes));

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.PlaySubTypes.SFX:
                        case (int)Lists.PlaySubTypes.BGM:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);

                                UInt32? SNDID = 0;

                                if (VarType < (int)Lists.VarTypes.Var1)
                                {
                                    SNDID = (SubID == (int)Lists.PlaySubTypes.SFX) ?
                                                           ScriptHelpers.Helper_GetSFXId(SplitLine, 2)
                                                                                   :
                                                           ScriptHelpers.Helper_GetMusicId(SplitLine, 2);

                                    if (SNDID == null)
                                        SNDID = Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0,
                                                                                                     (SubID == (int)Lists.PlaySubTypes.SFX) ? 
                                                                                                            Dicts.SFXes.Max(x => x).Value
                                                                                                                    :
                                                                                                            Dicts.Music.Max(x => x).Value));
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
                                UInt32 Addr = Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, Int32.MaxValue));

                                return new InstructionPlay((byte)SubID, Addr, 0);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                byte ID = Convert.ToByte(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, byte.MaxValue));

                                return new InstructionPlay((byte)SubID, ID, 0);
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
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
