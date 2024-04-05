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

                                var ExtVarNum = ScriptHelpers.GetScriptExtVarVal(SplitLine, 4, 0, Int16.MaxValue);

                                int SubIdN = (ExtVarNum.Vartype == (byte)Lists.VarTypes.VAR ?
                                                (int)Lists.GetSubTypes.EXT_VAR : ExtVarNum.Vartype == (byte)Lists.VarTypes.VARF ?
                                                (int)Lists.GetSubTypes.EXT_VARF : SubID);


                                return new InstructionGetExtVar((byte)SubIdN, (byte)ExtVarNum.Value, ActorID, Destination);
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
