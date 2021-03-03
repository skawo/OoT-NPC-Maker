using MiscUtil.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {

        private Instruction ParsePositionInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.PositionSubTypes));

                UInt16 ActorNum = 0;
                UInt16 ActorType = 0;
                float XPos = 0;
                float YPos = 0;
                float ZPos = 0;
                byte XPosT = 0;
                byte YPosT = 0;
                byte ZPosT = 0;

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
                                            ActorNum = (UInt16)ScriptHelpers.Helper_GetActorId(SplitLine, 3);
                                            ActorType = (UInt16)ScriptHelpers.Helper_GetActorCategory(SplitLine, 4);


                                            ScriptHelpers.GetXYZPos(SplitLine, 5, 6, 7, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);

                                            break;
                                        }
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 7);
                                            ActorNum = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));

                                            ScriptHelpers.GetXYZPos(SplitLine, 4, 5, 6, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);
                                            break;
                                        }

                                    case (int)Lists.TargetActorSubtypes.PLAYER:
                                    case (int)Lists.TargetActorSubtypes.SELF:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 6);
                                            ScriptHelpers.GetXYZPos(SplitLine, 3, 4, 5, ref XPosT, ref YPosT, ref ZPosT, ref XPos, ref YPos, ref ZPos);
                                            break;
                                        }
                                    default:
                                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                                }

                                return new InstructionPosition((byte)SubID, XPos, YPos, ZPos, XPosT, YPosT, ZPosT);

                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionPosition((byte)SubID, 0, 0, 0, 0, 0, 0);
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
