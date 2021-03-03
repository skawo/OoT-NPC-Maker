using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {

        private Instruction ParseScaleInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.RotationSubTypes));

                UInt16 ActorNum = 0;
                UInt16 ActorType = 0;
                float Scale = 0;
                byte ScaleT = 0;
 
                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.RotationSubTypes.SET:
                        case (int)Lists.RotationSubTypes.CHANGE:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (SetSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);
                                            ActorNum = (UInt16)ScriptHelpers.Helper_GetActorId(SplitLine, 3);
                                            ActorType = (UInt16)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4);


                                            ScriptHelpers.GetScale(SplitLine, 5, ref ScaleT, ref Scale);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);
                                            ActorNum = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));

                                            ScriptHelpers.GetScale(SplitLine, 4, ref ScaleT, ref Scale);
                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);
                                            ScriptHelpers.GetScale(SplitLine, 3, ref ScaleT, ref Scale);
                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionScale((byte)SubID, Scale, ScaleT);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionScale((byte)SubID, 0, 0);
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
