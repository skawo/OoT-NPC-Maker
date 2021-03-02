using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseKillInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                int SetSubType = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.TargetActorSubtypes));

                UInt16 ActorNum = 0;
                UInt16 ActorType = 0;

                switch (SetSubType)
                {
                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                        ActorNum = (UInt16)ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue);
                        break;
                    }
                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);
                        ActorNum = (UInt16)ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue);
                        ActorType = (UInt16)ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, 12);
                        break;
                    }
                    case (int)Lists.TargetActorSubtypes.PLAYER: break;
                    case (int)Lists.TargetActorSubtypes.SELF: break;
                    default:
                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                }

                return new InstructionKill((byte)SetSubType, ActorNum, ActorType);
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
