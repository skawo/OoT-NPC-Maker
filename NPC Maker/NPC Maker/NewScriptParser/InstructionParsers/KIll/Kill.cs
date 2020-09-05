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

                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(typeof(Lists.KillSubtypes), SplitLine[1].ToUpper());

                UInt16 ActorNum = 0;
                UInt16 ActorType = 0;

                switch (SetSubType)
                {
                    case (int)Lists.KillSubtypes.CONFIG_ID:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                        ActorNum = (UInt16)ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue);
                        break;
                    }
                    case (int)Lists.KillSubtypes.ACTOR_ID:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);
                        ActorNum = (UInt16)ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue);
                        ActorType = (UInt16)ParserHelpers.GetValueAndCheckRange(SplitLine, 3, 0, 12);
                        break;
                    }

                    default: break;
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
