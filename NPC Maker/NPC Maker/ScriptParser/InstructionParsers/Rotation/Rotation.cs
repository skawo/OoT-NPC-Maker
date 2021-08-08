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

                byte ActorNumT = 0;
                object ActorNum = (float)0;
                object XRot = (float)0;
                object YRot = (float)0;
                object ZRot = (float)0;
                byte XRotT = 0;
                byte ZRotT = 0;
                byte YRotT = 0;
                object Speed = (float)0;
                byte SpeedT = 0;

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

                                ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                ActorNum = ScriptHelpers.Helper_GetActorId(SplitLine, 3, ActorNumT);

                                ScriptHelpers.GetXYZRot(SplitLine, 4, 5, 6, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot, Min, Max);

                                if (NoSpeed == 0)
                                {
                                    SpeedT = ScriptHelpers.GetVarType(SplitLine, 7);
                                    Speed = ScriptHelpers.GetValueByType(SplitLine, 7, SpeedT, 0, float.MaxValue);
                                }

                                break;

                            }
                        case (int)Lists.TargetActorSubtypes.NPCMAKER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 8 - NoSpeed);

                                ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                ActorNum = ScriptHelpers.GetValueByType(SplitLine, 3, ActorNumT, 0, UInt16.MaxValue);

                                ScriptHelpers.GetXYZRot(SplitLine, 4, 5, 6, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot, Min, Max);

                                if (NoSpeed == 0)
                                {
                                    SpeedT = ScriptHelpers.GetVarType(SplitLine, 7);
                                    Speed = ScriptHelpers.GetValueByType(SplitLine, 7, SpeedT, 0, float.MaxValue);
                                }

                                break;
                            }

                        case (int)Lists.TargetActorSubtypes.PLAYER:
                        case (int)Lists.TargetActorSubtypes.SELF:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 7 - NoSpeed);
                                ScriptHelpers.GetXYZRot(SplitLine, 3, 4, 5, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot, Min, Max);

                                if (NoSpeed == 0)
                                {
                                    SpeedT = ScriptHelpers.GetVarType(SplitLine, 6);
                                    Speed = ScriptHelpers.GetValueByType(SplitLine, 6, SpeedT, 0, float.MaxValue);
                                }

                                break;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                    return new InstructionRotation((byte)SubID, ActorNum, ActorNumT, XRot, YRot, ZRot, XRotT, ZRotT, YRotT, (byte)SetSubType, Speed, SpeedT);
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionRotation((byte)SubID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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
