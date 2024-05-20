using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseGotoInstruction(string[] SplitLine)
        {
            try
            {
                try
                {
                    return ParseGotoVarInstruction_Internal(SplitLine);
                }
                catch (ParseException)
                {
                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                    return new InstructionGoto(SplitLine[1].TrimEnd(':'));
                }
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
    }
}
