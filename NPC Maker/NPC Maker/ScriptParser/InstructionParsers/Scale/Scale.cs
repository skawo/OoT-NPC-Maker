using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {

        private Instruction ParseScaleInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.ScaleSubTypes), 1);

                ScriptVarVal ActorNum = new ScriptVarVal();
                ScriptVarVal Scale = new ScriptVarVal();
                ScriptVarVal Speed = new ScriptVarVal();

                int NoSpeed = 0;

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.ScaleSubTypes.SET:
                            {
                                NoSpeed = 1;
                                break;
                            }
                        case (int)Lists.ScaleSubTypes.SCALE_TO:
                        case (int)Lists.ScaleSubTypes.SCALE_BY:
                            break;
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }


                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                    int SetSubType = Convert.ToInt32(ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine)));

                    switch (SetSubType)
                    {
                        case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6 - NoSpeed);

                                ActorNum = ScriptHelpers.Helper_GetActorId(SplitLine, 3);
                                ScriptHelpers.GetScriptVarVal(SplitLine, 4, 0, float.MaxValue, ref Scale);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 5, 0, float.MaxValue, ref Speed);

                                break;
                            }
                        case (int)Lists.TargetActorSubtypes.NPCMAKER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6 - NoSpeed);

                                ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue, ref ActorNum);
                                ScriptHelpers.GetScriptVarVal(SplitLine, 4, 0, float.MaxValue, ref Scale);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 5, 0, float.MaxValue, ref Speed);

                                break;
                            }

                        case (int)Lists.TargetActorSubtypes.PLAYER:
                        case (int)Lists.TargetActorSubtypes.SELF:
                        case (int)Lists.TargetActorSubtypes.REF_ACTOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5 - NoSpeed);
                                ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, float.MaxValue, ref Scale);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 4, 0, float.MaxValue, ref Speed);

                                break;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                    return new InstructionScale((byte)SubID, (byte)SetSubType, ActorNum, Scale, Speed);

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionNop();
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionNop();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
