using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseKillInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                int SetSubType = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.TargetActorSubtypes));

                UInt32 ActorNum = 0;
                byte ANumVarT = 0;

                switch (SetSubType)
                {
                    case (int)Lists.TargetActorSubtypes.NPCMAKER:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                            ANumVarT = ScriptHelpers.GetVarType(SplitLine, 2);
                            ActorNum = (UInt32)ScriptHelpers.GetValueByType(SplitLine, 3, ANumVarT, 0, UInt16.MaxValue);

                            break;
                        }
                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                            ANumVarT = ScriptHelpers.GetVarType(SplitLine, 2);
                            ActorNum = (UInt16)ScriptHelpers.Helper_GetActorId(SplitLine, 2, ANumVarT);

                            break;
                        }
                    case (int)Lists.TargetActorSubtypes.PLAYER: break;
                    case (int)Lists.TargetActorSubtypes.SELF: break;
                    default:
                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                }

                return new InstructionKill((byte)SetSubType, ActorNum, ANumVarT);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionKill(0, 0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
