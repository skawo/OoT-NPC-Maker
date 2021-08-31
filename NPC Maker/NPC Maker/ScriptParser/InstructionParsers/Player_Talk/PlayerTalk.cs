using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private List<Instruction> ParseForceTalkInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsBigger(SplitLine, 3);

                List<Instruction> outl = new List<Instruction>();

                if (SplitLine.Length != 1)
                {
                    Instruction Ins = ParseEnableTalkingInstruction(SplitLine);
                    Ins.ID = (int)Lists.Instructions.SHOW_TEXTBOX;
                    outl.Add(Ins);
                }

                outl.Add(new InstructionForceTalk());
                return outl;
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new List<Instruction>();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new List<Instruction>();
            }
        }
    }
}
