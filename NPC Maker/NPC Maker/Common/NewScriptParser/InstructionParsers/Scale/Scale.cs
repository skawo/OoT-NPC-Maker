using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {

        private Instruction ParseScaleInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.RotationSubTypes));

                UInt32 ActorNum = 0;
                byte ActorNumT = 0;
                Int32 ActorCat = 0;
                byte ActorCatT = 0;
                object Scale = 0;
                byte ScaleT = 0;
 
                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.RotationSubTypes.SET:
                        case (int)Lists.RotationSubTypes.CHANGE:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (SetSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorCatT = ScriptHelpers.GetVarType(SplitLine, 4);

                                            ActorNum = (UInt32)ScriptHelpers.Helper_GetActorId(SplitLine, 3, ActorNumT);
                                            ActorCat = (Int32)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4, ActorCatT);

                                            ScriptHelpers.GetScale(SplitLine, 5, ref ScaleT, ref Scale);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorNum = (UInt32)ScriptHelpers.GetValueByType(SplitLine, 3, ActorNumT, 0, UInt16.MaxValue);

                                            ScriptHelpers.GetScale(SplitLine, 4, ref ScaleT, ref Scale);
                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);
                                            ScriptHelpers.GetScale(SplitLine, 3, ref ScaleT, ref Scale);
                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionScale((byte)SubID, ActorNum, ActorNumT, ActorCat, ActorCatT, Scale, ScaleT);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionScale((byte)SubID, 0, 0, 0, 0, 0, 0);
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
