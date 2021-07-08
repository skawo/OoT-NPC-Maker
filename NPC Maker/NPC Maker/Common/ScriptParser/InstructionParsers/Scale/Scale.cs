using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {

        private Instruction ParseScaleInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.ScaleSubTypes), 2);

                UInt32 ActorNum = 0;
                byte ActorNumT = 0;
                Int32 ActorCat = 0;
                byte ActorCatT = 0;
                object Scale = 0;
                byte ScaleT = 0;
                object Speed = 0;
                byte SpeedT = 0;

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.ScaleSubTypes.SET:
                        case (int)Lists.ScaleSubTypes.CHANGE_TO:
                        case (int)Lists.ScaleSubTypes.CHANGE_BY:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(SplitLine, 1, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (SetSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 7);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorCatT = ScriptHelpers.GetVarType(SplitLine, 4);

                                            ActorNum = (UInt32)ScriptHelpers.Helper_GetActorId(SplitLine, 3, ActorNumT);
                                            ActorCat = (Int32)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4, ActorCatT);

                                            ScriptHelpers.GetScale(SplitLine, 5, ref ScaleT, ref Scale);


                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 6);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 6, SpeedT, 0, float.MaxValue);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorNum = (UInt32)ScriptHelpers.GetValueByType(SplitLine, 3, ActorNumT, 0, UInt16.MaxValue);

                                            ScriptHelpers.GetScale(SplitLine, 4, ref ScaleT, ref Scale);


                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 5);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 5, SpeedT, 0, float.MaxValue);
                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);
                                            ScriptHelpers.GetScale(SplitLine, 3, ref ScaleT, ref Scale);


                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 4);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 4, SpeedT, 0, float.MaxValue);
                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionScale((byte)SubID, ActorNum, ActorNumT, ActorCat, ActorCatT, Scale, ScaleT, (byte)SetSubType, Speed, SpeedT);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionScale((byte)SubID, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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
