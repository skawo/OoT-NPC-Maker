using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseWarpInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                byte VarType = ScriptHelpers.GetVarType(SplitLine, 1);
                UInt32 WarpID = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 1, VarType, 0, UInt16.MaxValue));

                return new InstructionWarp(WarpID, VarType);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionWarp(0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
