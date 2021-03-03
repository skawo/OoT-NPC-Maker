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
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.RotationSubTypes));

                UInt16 ActorNum = 0;
                UInt16 ActorType = 0;
                Int16 XRot = 0;
                Int16 YRot = 0;
                Int16 ZRot = 0;
                byte XRotT = 0;
                byte YRotT = 0;
                byte ZRotT = 0;

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.RotationSubTypes.SET:
                        case (int)Lists.RotationSubTypes.CHANGE:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (SetSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 8);
                                            ActorNum = (UInt16)ScriptHelpers.Helper_GetActorId(SplitLine, 3);
                                            ActorType = (UInt16)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4);


                                            ScriptHelpers.GetXYZRot(SplitLine, 5, 6, 7, ref XRotT, ref YRotT, ref ZRotT, ref XRot, ref YRot, ref ZRot);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 7);
                                            ActorNum = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));

                                            ScriptHelpers.GetXYZRot(SplitLine, 4, 5, 6, ref XRotT, ref YRotT, ref ZRotT, ref XRot, ref YRot, ref ZRot);
                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);
                                            ScriptHelpers.GetXYZRot(SplitLine, 3, 4, 5, ref XRotT, ref YRotT, ref ZRotT, ref XRot, ref YRot, ref ZRot);
                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionRotation((byte)SubID, XRot, YRot, ZRot, XRotT, YRotT, ZRotT);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionRotation((byte)SubID, 0, 0, 0, 0, 0, 0);
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
