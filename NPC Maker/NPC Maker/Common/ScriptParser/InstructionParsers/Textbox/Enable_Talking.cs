using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseEnableTalkingInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                Int32 TextID_Adult = 0;
                Int32 TextID_Child = 0;
                byte TextIDAdultT = 0;
                byte TextIDChildT = 0;

                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLine, ref TextID_Adult, ref TextID_Child, ref TextIDAdultT, ref TextIDChildT, Entry.Messages);

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
