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
                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                    var Type = ScriptHelpers.GetScriptVarVal(SplitLine, 1, typeof(Lists.QuakeTypes), ParseException.UnrecognizedQuake(SplitLine));
                    var Speed = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, Int16.MaxValue);
                    var Duration = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, Int16.MaxValue);

                    return new InstructionQuake(Speed, Type, Duration);

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
