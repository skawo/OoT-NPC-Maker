using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseSaveInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 1);
                return new InstructionSave();
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionSave();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionSave();
            }
        }
    }
}
