using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseEnableTalkingInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                UInt16 TextID_Adult = 0;
                UInt16 TextID_Child = 0;

                TextID_Adult = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 1, 0, UInt16.MaxValue));
                TextID_Child = (SplitLine.Count() == 2) ? TextID_Adult : Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, TextID_Adult, TextID_Child);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, 0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
