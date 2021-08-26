using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {

        private Instruction ParseRotationInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.RotationSubTypes), 1);

                ScriptVarVal ActorNum = new ScriptVarVal();
                ScriptVarVal XRot = new ScriptVarVal();
                ScriptVarVal YRot = new ScriptVarVal();
                ScriptVarVal ZRot = new ScriptVarVal();
                ScriptVarVal Speed = new ScriptVarVal();

                int Min = Int16.MinValue;
                int Max = Int16.MaxValue;
                int NoSpeed = 0;

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.RotationSubTypes.SET:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);
                                Min = Int16.MinValue;
                                Max = Int16.MaxValue;
                                NoSpeed = 1;
                                break;
                            }
                        case (int)Lists.RotationSubTypes.ROTATE_TO:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 7);
                                Min = Int16.MinValue;
                                Max = Int16.MaxValue;
                                break;
                            }
                        case (int)Lists.RotationSubTypes.ROTATE_BY:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 7);
                                Min = Int32.MinValue;
                                Max = Int32.MaxValue;
                                break;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                    int SetSubType = Convert.ToInt32(ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine)));

                    switch (SetSubType)
                    {
                        case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 8 - NoSpeed);

                                ActorNum = ScriptHelpers.Helper_GetActorId(SplitLine, 3);
                                ScriptHelpers.GetXYZ(SplitLine, 4, 5, 6, ref XRot, ref YRot, ref ZRot, Min, Max);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 7, 0, float.MaxValue, ref Speed);

                                break;

                            }
                        case (int)Lists.TargetActorSubtypes.NPCMAKER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 8 - NoSpeed);
                                ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue, ref ActorNum);
                                ScriptHelpers.GetXYZ(SplitLine, 4, 5, 6, ref XRot, ref YRot, ref ZRot, Min, Max);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 7, 0, float.MaxValue, ref Speed);

                                break;
                            }

                        case (int)Lists.TargetActorSubtypes.PLAYER:
                        case (int)Lists.TargetActorSubtypes.SELF:
                        case (int)Lists.TargetActorSubtypes.REF_ACTOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 7 - NoSpeed);
                                ScriptHelpers.GetXYZ(SplitLine, 3, 4, 5, ref XRot, ref YRot, ref ZRot, Min, Max);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 6, 0, float.MaxValue, ref Speed);

                                break;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                    return new InstructionRotation((byte)SubID, (byte)SetSubType, ActorNum, XRot, YRot, ZRot, Speed);
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
