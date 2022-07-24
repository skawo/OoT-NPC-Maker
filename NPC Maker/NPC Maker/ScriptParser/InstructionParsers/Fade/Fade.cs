using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseFadeInstruction(string[] SplitLine)
        {
            try
            {
                if (SplitLine[0].ToUpper() == Lists.Instructions.FADEIN.ToString())
                {
                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                    var Rate = ScriptHelpers.GetScriptVarVal(SplitLine, 1, 1, byte.MaxValue);

                    return new InstructionFade((byte)Lists.Instructions.FADEIN, new ScriptVarVal(), new ScriptVarVal(), new ScriptVarVal(), Rate);

                }
                else
                {
                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                    ScriptVarVal R = new ScriptVarVal();
                    ScriptVarVal G = new ScriptVarVal();
                    ScriptVarVal B = new ScriptVarVal();
                    ScriptVarVal A = null;

                    ScriptHelpers.GetRGBorRGBA(SplitLine, 1, ref R, ref G, ref B, ref A);
                    var Rate = ScriptHelpers.GetScriptVarVal(SplitLine, 4, 1, byte.MaxValue);

                    return new InstructionFade((byte)Lists.Instructions.FADEOUT, R, G, B, Rate);
                }
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
