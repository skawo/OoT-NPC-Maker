using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseCCallInstruction(CCodeEntry CodeEntry, string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 3);

                var Func = CodeEntry.Functions.Find(x => x.Key.ToUpper() == SplitLine[1].ToUpper());

                if (Func.Key == null)
                    throw ParseException.CFunctionNotFound(SplitLine);

                var Destination = new ScriptVarVal(0, 0);

                if (SplitLine.Length > 2)
                {
                    Destination = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt32.MaxValue);

                    if (Destination.Vartype <= (byte)Lists.VarTypes.RANDOM)
                        throw ParseException.DestValWrong(SplitLine);
                }

                return new InstructionCCall(Func.Value, Destination);
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
