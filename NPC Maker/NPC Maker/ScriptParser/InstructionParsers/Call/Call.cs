using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseCallInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 1, 9);

                string Goto = $"{Lists.Keyword_CallFunc}{SplitLine[0].TrimStart(':')}";

                List<ScriptVarVal> l = new List<ScriptVarVal>();

                for (int i = 1; i < SplitLine.Length; i++)
                    l.Add(ScriptHelpers.GetScriptVarVal(SplitLine, i, float.MinValue, float.MaxValue));

                byte numArgs = (byte)(SplitLine.Length - 1);

                return new InstructionCall(numArgs, Goto, l);
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
