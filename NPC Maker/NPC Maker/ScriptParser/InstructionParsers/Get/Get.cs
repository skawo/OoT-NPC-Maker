using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseGetInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                try
                {
                    int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.GetSubTypes));

                    switch (SubID)
                    {
                        case (int)Lists.GetSubTypes.EXT_VAR:
                        case (int)Lists.GetSubTypes.EXT_VARF:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 5);

                                var Destination = new ScriptVarVal(0, 0);

                                Destination = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt32.MaxValue);

                                if (Destination.Vartype <= (byte)Lists.VarTypes.RANDOM)
                                    throw ParseException.DestValWrong(SplitLine);

                                var ActorID = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, Int16.MaxValue);

                                byte ExtVarNum = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, 4, (int)Lists.VarTypes.NORMAL, 1, Lists.Num_User_Vars));

                                return new InstructionGetExtVar((byte)SubID, ExtVarNum, ActorID, Destination);
                            }
                       
                        default: throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionNop();
                }

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
