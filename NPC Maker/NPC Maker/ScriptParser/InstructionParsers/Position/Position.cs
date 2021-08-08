using MiscUtil.Linq.Extensions;
using System;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {

        private Instruction ParsePositionInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.PositionSubTypes), 1);

                byte ActorNumT = 0;
                object ActorNum = (float)0;
                object XPos = (float)0;
                object YPos = (float)0;
                object ZPos = (float)0;
                byte XPosT = 0;
                byte YPosT = 0;
                byte ZPosT = 0;
                object Speed = (float)0;
                byte SpeedT = 0;
                byte IgnoreYPos = 0;

                int NoSpeed = 0;

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.PositionSubTypes.SET:
                            {
                                NoSpeed = 1;
                                break;
                            }
                        case (int)Lists.PositionSubTypes.MOVE_TO:
                        case (int)Lists.PositionSubTypes.MOVE_BY:
                        case (int)Lists.PositionSubTypes.DIRECTION_MOVE_BY:
                            break;
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }


                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                    if (SplitLine.Last().ToUpper() == Lists.Keyword_Ignore_Y)
                        IgnoreYPos = 1;

                    int SetSubType = Convert.ToInt32(ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine)));

                    switch (SetSubType)
                    {
                        case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 8 - NoSpeed, 9 - NoSpeed);

                                ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                ActorNum = ScriptHelpers.Helper_GetActorId(SplitLine, 3, ActorNumT);

                                ScriptHelpers.GetXYZPos(SplitLine, 4, 5, 6, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);

                                if (NoSpeed == 0)
                                {
                                    SpeedT = ScriptHelpers.GetVarType(SplitLine, 7);
                                    Speed = ScriptHelpers.GetValueByType(SplitLine, 7, SpeedT, 0, float.MaxValue);
                                }

                                break;
                            }
                        case (int)Lists.TargetActorSubtypes.NPCMAKER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 8 - NoSpeed, 9 - NoSpeed);

                                ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                ActorNum = ScriptHelpers.GetValueByType(SplitLine, 3, ActorNumT, 0, UInt16.MaxValue);

                                ScriptHelpers.GetXYZPos(SplitLine, 4, 5, 6, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);

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
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 7 - NoSpeed, 8 - NoSpeed);
                                ScriptHelpers.GetXYZPos(SplitLine, 3, 4, 5, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);

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

                    return new InstructionPosition((byte)SubID, ActorNum, ActorNumT, XPos, YPos, ZPos, XPosT, YPosT, ZPosT, (byte)SetSubType, Speed, SpeedT, IgnoreYPos);
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionPosition((byte)SubID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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
