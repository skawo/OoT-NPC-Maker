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

                ScriptVarVal ActorNum = new ScriptVarVal();
                ScriptVarVal XPos = new ScriptVarVal();
                ScriptVarVal YPos = new ScriptVarVal();
                ScriptVarVal ZPos = new ScriptVarVal();
                ScriptVarVal Speed = new ScriptVarVal();

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
                                ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue, ref ActorNum);
                                ScriptHelpers.GetXYZPos(SplitLine, 4, 5, 6, ref XPos, ref YPos, ref ZPos);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 7, 0, float.MaxValue, ref Speed);

                                break;
                            }
                        case (int)Lists.TargetActorSubtypes.NPCMAKER:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 8 - NoSpeed, 9 - NoSpeed);
                                ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue, ref ActorNum);
                                ScriptHelpers.GetXYZPos(SplitLine, 4, 5, 6, ref XPos, ref YPos, ref ZPos);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 7, 0, float.MaxValue, ref Speed);

                                break;
                            }

                        case (int)Lists.TargetActorSubtypes.PLAYER:
                        case (int)Lists.TargetActorSubtypes.SELF:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 7 - NoSpeed, 8 - NoSpeed);
                                ScriptHelpers.GetXYZPos(SplitLine, 3, 4, 5, ref XPos, ref YPos, ref ZPos);

                                if (NoSpeed == 0)
                                    ScriptHelpers.GetScriptVarVal(SplitLine, 6, 0, float.MaxValue, ref Speed);

                                break;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                    return new InstructionPosition((byte)SubID, (byte)SetSubType, IgnoreYPos, ActorNum, XPos, YPos, ZPos, Speed);
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
