using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseCloseTextboxInstruction(string[] SplitLine)
        {
            try
            {
                Instruction Ins = new Instruction((int)Lists.Instructions.CLOSE_TEXTBOX);
                return Ins;
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
