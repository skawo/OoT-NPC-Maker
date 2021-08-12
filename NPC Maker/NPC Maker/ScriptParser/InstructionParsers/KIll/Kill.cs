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

                ScriptVarVal ActorNum = new ScriptVarVal();

                switch (SetSubType)
                {
                    case (int)Lists.TargetActorSubtypes.NPCMAKER:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                            ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt16.MaxValue, ref ActorNum);

                            break;
                        }
                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                            ActorNum = ScriptHelpers.Helper_GetActorId(SplitLine, 2);

                            break;
                        }
                    case (int)Lists.TargetActorSubtypes.PLAYER: break;
                    case (int)Lists.TargetActorSubtypes.SELF: break;
                    default:
                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                }

                return new InstructionKill((byte)SetSubType, ActorNum);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionNop();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
