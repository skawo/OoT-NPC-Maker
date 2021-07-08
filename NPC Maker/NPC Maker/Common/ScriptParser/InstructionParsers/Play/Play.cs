using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                        case (int)Lists.PlaySubTypes.BGM:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);

                                Int32? SNDID = 0;

                                SNDID = (SubID == (int)Lists.PlaySubTypes.SFX) ?
                                                       ScriptHelpers.Helper_GetSFXId(SplitLine, 2, VarType)
                                                                               :
                                                       ScriptHelpers.Helper_GetMusicId(SplitLine, 2, VarType);

                                if (SNDID < 0)
                                    throw ParseException.ParamOutOfRange(SplitLine);


                                return new InstructionPlay((byte)SubID, (UInt32)SNDID, VarType);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                                return new InstructionPlay((byte)SubID, 0, 0);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE_ADDR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 Addr = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, Int32.MaxValue));

                                return new InstructionPlay((byte)SubID, Addr, VarType);
                            }
                        case (int)Lists.PlaySubTypes.CUTSCENE_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 ID = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, Int32.MaxValue));

                                return new InstructionPlay((byte)SubID, ID, VarType);
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionPlay((int)Lists.Instructions.PLAY, 0, 0);
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
