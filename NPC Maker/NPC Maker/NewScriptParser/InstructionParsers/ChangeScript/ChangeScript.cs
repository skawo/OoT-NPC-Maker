using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseChangeScriptInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                int SetSubType = (int)ScriptHelpers.Helper_GetEnumByName(typeof(Lists.ScriptChangeSubtypes), SplitLine[1]);
                UInt16 ActorID = (UInt16)ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue);

                switch (SetSubType)
                {
                    case (int)Lists.ScriptChangeSubtypes.OVERWRITE:
                        {
                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);
                            return new InstructionChangeScript((byte)SetSubType, ActorID, SplitLine[3]);
                        }
                    case (int)Lists.ScriptChangeSubtypes.RESTORE:
                        {
                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                            return new InstructionChangeScript((byte)SetSubType, ActorID, "__NONE__");
                        }
                    default: throw new Exception;
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionChangeScript(0, 0, "__NONE__");
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
