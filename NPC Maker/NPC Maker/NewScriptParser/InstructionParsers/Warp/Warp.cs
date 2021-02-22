using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseWarpInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);
                UInt16 WarpID = (UInt16)ScriptHelpers.GetValueAndCheckRange(SplitLine, 1, 0, UInt16.MaxValue);

                return new InstructionWarp(WarpID);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionWarp(0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
