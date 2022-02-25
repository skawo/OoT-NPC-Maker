using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseWarpInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 4);
                var WarpID = ScriptHelpers.GetScriptVarVal(SplitLine, 1, 0, UInt16.MaxValue);
                var NextCutsceneIndex = new ScriptVarVal(0, 0);

                var SceneLoadFlag = new ScriptVarVal(1, 0);

                if (SplitLine.Length == 4)
                    NextCutsceneIndex = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, 16);

                if (SplitLine.Length == 3)
                    SceneLoadFlag = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, Int32.MaxValue);

                return new InstructionWarp(WarpID, NextCutsceneIndex, SceneLoadFlag);
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
