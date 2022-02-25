using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseScriptInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                var ScriptID = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt16.MaxValue);
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.ScriptSubtypes));

                return new InstructionScript((byte)SubID, ScriptID);
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
