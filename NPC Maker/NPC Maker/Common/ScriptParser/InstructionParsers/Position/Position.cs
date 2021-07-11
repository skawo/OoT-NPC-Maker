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
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.PositionSubTypes), 2);

                byte ActorNumT = 0;
                byte ActorCatT = 0;
                object ActorNum = 0;
                object ActorCat = 0;
                object XPos = 0;
                object YPos = 0;
                object ZPos = 0;
                byte XPosT = 0;
                byte YPosT = 0;
                byte ZPosT = 0;
                object Speed = 0;
                byte SpeedT = 0;
                byte IgnoreYPos = 0;

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.PositionSubTypes.SET:
                        case (int)Lists.PositionSubTypes.CHANGE_TO:
                        case (int)Lists.PositionSubTypes.CHANGE_BY:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                if (SplitLine.Last().ToUpper() == Lists.Keyword_Ignore_Y)
                                    IgnoreYPos = 1;

                                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(SplitLine, 1, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (SetSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 9, 10);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorCatT = ScriptHelpers.GetVarType(SplitLine, 4);

                                            ActorNum = (UInt32)ScriptHelpers.Helper_GetActorId(SplitLine, 3, ActorNumT);
                                            ActorCat = (Int32)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4, ActorCatT);


                                            ScriptHelpers.GetXYZPos(SplitLine, 5, 6, 7, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);

                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 8);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 8, SpeedT, 0, float.MaxValue);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 8, 9);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorNum = (UInt32)ScriptHelpers.GetValueByType(SplitLine, 3, ActorNumT, 0, UInt16.MaxValue);

                                            ScriptHelpers.GetXYZPos(SplitLine, 4, 5, 6, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);

                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 7);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 7, SpeedT, 0, float.MaxValue);

                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 7, 8);
                                            ScriptHelpers.GetXYZPos(SplitLine, 3, 4, 5, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);

                                            SpeedT = ScriptHelpers.GetVarType(SplitLine, 6);
                                            Speed = ScriptHelpers.GetValueByType(SplitLine, 6, SpeedT, 0, float.MaxValue);

                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionPosition((byte)SubID, ActorNum, ActorNumT, ActorCat, ActorCatT, XPos, YPos, ZPos, XPosT, YPosT, ZPosT, (byte)SetSubType, Speed, SpeedT, IgnoreYPos);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionPosition((byte)SubID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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
