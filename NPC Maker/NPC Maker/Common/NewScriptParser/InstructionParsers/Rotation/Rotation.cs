using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {

        private Instruction ParseRotationInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.PositionSubTypes));

                byte ActorNumT = 0;
                byte ActorCatT = 0;
                UInt32 ActorNum = 0;
                UInt32 ActorCat = 0;
                Int32 XRot = 0;
                Int32 YRot = 0;
                Int32 ZRot = 0;
                byte XRotT = 0;
                byte ZRotT = 0;
                byte YRotT = 0;

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.PositionSubTypes.SET:
                        case (int)Lists.PositionSubTypes.CHANGE:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (SetSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 8);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorCatT = ScriptHelpers.GetVarType(SplitLine, 4);

                                            ActorNum = (UInt32)ScriptHelpers.Helper_GetActorId(SplitLine, 3, ActorNumT);
                                            ActorCat = (UInt32)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4, ActorCatT);


                                            ScriptHelpers.GetXYZRot(SplitLine, 5, 6, 7, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 7);

                                            ActorNumT = ScriptHelpers.GetVarType(SplitLine, 3);
                                            ActorNum = (UInt32)ScriptHelpers.GetValueByType(SplitLine, 3, ActorNumT, 0, UInt16.MaxValue);

                                            ScriptHelpers.GetXYZRot(SplitLine, 4, 5, 6, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot);
                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);
                                            ScriptHelpers.GetXYZRot(SplitLine, 3, 4, 5, ref XRotT, ref ZRotT, ref YRotT, ref XRot, ref YRot, ref ZRot);
                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionRotation((byte)SubID, ActorNum, ActorNumT, ActorCat, ActorCatT, XRot, YRot, ZRot, XRotT, ZRotT, YRotT);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionRotation((byte)SubID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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
