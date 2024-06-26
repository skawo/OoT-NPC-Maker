﻿using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseGotoVarInstruction(string[] SplitLine)
        {
            try
            {
                return ParseGotoVarInstruction_Internal(SplitLine);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionGoto(Lists.Keyword_Label_Null);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }

        public Instruction ParseGotoVarInstruction_Internal(string[] SplitLine)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

            ScriptVarVal s = ScriptHelpers.GetScriptVarVal(SplitLine, 1, 0, UInt32.MaxValue);
            return new InstructionGotoVar(s);
        }
    }
}
