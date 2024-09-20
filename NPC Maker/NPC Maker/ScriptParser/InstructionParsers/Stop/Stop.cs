using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseStopInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.StopSubtypes));

                if (SubID == -1)
                    throw ParseException.UnrecognizedFunctionSubtype(SplitLine);

                ScriptVarVal Val = (SubID == (int)Lists.StopSubtypes.BGM) ?
                                       ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt16.MaxValue)
                                                               :
                                       ScriptHelpers.Helper_GetSFXId(SplitLine, 2);


                return new InstructionStop((byte)SubID, Val);
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
