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

                UInt32 TextID_Adult = 0;
                UInt32 TextID_Child = 0;
                byte TextIDAdultT = 0;
                byte TextIDChildT = 0;

                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLine, ref TextID_Adult, ref TextID_Child, ref TextIDAdultT, ref TextIDChildT);

                return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, TextID_Adult, TextID_Child, TextIDAdultT, TextIDChildT);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
