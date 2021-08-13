using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseShowTextboxSPInstruction(string[] SplitLine)
        {
            try
            {
                Instruction Ins = ParseEnableTalkingInstruction(SplitLine);
                Ins.ID = (int)Lists.Instructions.SHOW_TEXTBOX_SP;
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
