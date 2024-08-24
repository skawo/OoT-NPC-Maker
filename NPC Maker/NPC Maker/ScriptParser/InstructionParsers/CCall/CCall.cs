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
                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 2, 11);

                var Func = CodeEntry.Functions.Find(x => x.Key.ToUpper() == SplitLine[1].ToUpper());

                if (Func.Key == null)
                    throw ParseException.CFunctionNotFound(SplitLine);

                var Destination = new ScriptVarVal(0, 0);

                if (SplitLine.Length > 2 && SplitLine[2] != "_")
                {
                    Destination = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt32.MaxValue);

                    if (Destination.Vartype <= (byte)Lists.VarTypes.RANDOM)
                        throw ParseException.DestValWrong(SplitLine);
                }

                List<ScriptVarVal> Args = new List<ScriptVarVal>();

                if (SplitLine.Length > 3)
                {
                    for (int i = 3; i < SplitLine.Length; i++)
                    {
                        var Arg = ScriptHelpers.GetScriptVarVal(SplitLine, i, float.MinValue, float.MaxValue);
                        Args.Add(Arg);
                    }
                }

                return new InstructionCCall(Func.Value, Destination, Args);
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
