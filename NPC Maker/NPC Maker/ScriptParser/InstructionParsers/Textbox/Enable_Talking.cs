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

                ScriptVarVal TextID_Adult = new ScriptVarVal();
                ScriptVarVal TextID_Child = new ScriptVarVal();

                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLine, ref TextID_Adult, ref TextID_Child, Entry.Messages);

                return new InstructionTextbox((int)Lists.Instructions.ENABLE_TALKING, TextID_Adult, TextID_Child);
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
