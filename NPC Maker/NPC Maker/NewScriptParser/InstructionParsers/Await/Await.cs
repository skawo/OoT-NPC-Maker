using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseAwaitInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = (int)System.Enum.Parse(typeof(Lists.AwaitSubTypes), SplitLine[1].ToUpper());

                try
                {
                    if (SubID < 35)
                    {
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);
                        return new InstructionAwait((byte)SubID, Convert.ToByte(0), 0);
                    }
                    else if (SubID < 75)
                    {
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                        byte VarType = ScriptHelpers.GetVariable(SplitLine[2]);
                        UInt16 Data = 0;

                        if (VarType == (int)Lists.VarTypes.RNG)
                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                        if (VarType < (int)Lists.VarTypes.Var1)
                            Data = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 
                                                                                        VarType == (int)Lists.VarTypes.RNG ? 3 : 2, 
                                                                                        0, UInt16.MaxValue));

                        return new InstructionAwait((byte)SubID, Data, VarType);
                    }
                    else
                    {
                        switch (SubID)
                        {
                            case (int)Lists.AwaitSubTypes.VAR_1:
                            case (int)Lists.AwaitSubTypes.VAR_2:
                            case (int)Lists.AwaitSubTypes.VAR_3:
                            case (int)Lists.AwaitSubTypes.VAR_4:
                            case (int)Lists.AwaitSubTypes.VAR_5:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    byte Condition = ScriptHelpers.GetConditionID(SplitLine[2]);

                                    if (Condition == byte.MaxValue)
                                        throw ParseException.UnrecognizedCondition(SplitLine);

                                    byte VarType = ScriptHelpers.GetVariable(SplitLine[3]);
                                    sbyte Data = 0;

                                    if (VarType == (int)Lists.VarTypes.RNG)
                                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                    if (VarType < (int)Lists.VarTypes.Var1)
                                        Data = Convert.ToSByte(ParserHelpers.GetValueAndCheckRange(SplitLine, 
                                                                                                   VarType == (int)Lists.VarTypes.RNG ? 4 : 3, 
                                                                                                   sbyte.MinValue, sbyte.MaxValue));

                                    return new InstructionAwaitScriptVar((byte)SubID, Data, Condition, VarType);
                                }
                            default:
                                throw new Exception();
                        }
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionSet((byte)SubID, 0, 0);
                }

            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
