using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseWarpInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);
                var WarpID = ScriptHelpers.GetScriptVarVal(SplitLine, 1, 0, UInt16.MaxValue);
                var NextCutsceneIndex = new ScriptVarVal(0, 0);

                if (SplitLine.Length == 3)
                    NextCutsceneIndex = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 4, 16);

                return new InstructionWarp(WarpID, NextCutsceneIndex);
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
