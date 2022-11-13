using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseQuakeInstruction(string[] SplitLine)
        {
            try
            {
                try
                {
                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                    var Type = ScriptHelpers.GetScriptVarVal(SplitLine, 1, typeof(Lists.QuakeTypes), ParseException.UnrecognizedQuake(SplitLine));
                    var Speed = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, Int16.MaxValue);

                    return new InstructionQuake(Speed, Type);

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionNop();
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
