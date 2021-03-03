using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseShowTextboxInstruction(string[] SplitLine)
        {
            try
            {
                Instruction Ins = ParseEnableTalkingInstruction(SplitLine);
                Ins.ID = (int)Lists.Instructions.SHOW_TEXTBOX;
                return Ins;
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionTextbox((int)Lists.Instructions.SHOW_TEXTBOX, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
