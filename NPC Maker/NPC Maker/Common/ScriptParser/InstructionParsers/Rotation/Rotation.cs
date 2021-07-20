using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {

        private Instruction ParseRotationInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.RotationSubTypes), 2);

                byte ActorNumT = 0;
                byte ActorCatT = 0;
                object ActorNum = 0;
                object ActorCat = 0;
                object XRot = 0;
                object YRot = 0;
                object ZRot = 0;
                byte XRotT = 0;
                byte ZRotT = 0;
                byte YRotT = 0;
                object Speed = 0;
                byte SpeedT = 0;


                int Min = Int16.MinValue;
                int Max = Int16.MaxValue;

                if (SubID == (int)Lists.RotationSubTypes.CHANGE_BY)
                {
                    Min = Int32.MinValue;
                    Max = Int32.MaxValue;
                }

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.RotationSubTypes.SET:
                        case (int)Lists.RotationSubTypes.CHANGE_TO:
                        case (int)Lists.RotationSubTypes.CHANGE_BY:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(SplitLine, 1, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (SetSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 9);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorCatT = ScriptHelpers.GetVarType(SplitLine, 4);

                                            ActorNum = (UInt32)ScriptHelpers.Helper_GetActorId(SplitLine, 3, ActorNumT);
                                            ActorCat = (Int32)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4, ActorCatT);

                                            ScriptHelpers.GetXYZRot(SplitLine, 5, 6, 7, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot, Min, Max);

                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 8);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 8, SpeedT, 0, float.MaxValue);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 8);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorNum = (UInt32)ScriptHelpers.GetValueByType(SplitLine, 3, ActorNumT, 0, UInt16.MaxValue);

                                            ScriptHelpers.GetXYZRot(SplitLine, 4, 5, 6, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot, Min, Max);

                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 7);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 7, SpeedT, 0, float.MaxValue);
                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 7);
                                            ScriptHelpers.GetXYZRot(SplitLine, 3, 4, 5, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot, Min, Max);

                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 6);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 6, SpeedT, 0, float.MaxValue);

                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionRotation((byte)SubID, ActorNum, ActorNumT, ActorCat, ActorCatT, XRot, YRot, ZRot, XRotT, ZRotT, YRotT, (byte)SetSubType, Speed, SpeedT);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionRotation((byte)SubID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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
