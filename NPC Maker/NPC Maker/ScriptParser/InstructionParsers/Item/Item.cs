using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseItemInstruction(string[] SplitLine)
        {
            try
            {
                try
                {
                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);
                    int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.ItemSubTypes));

                    switch (SubID)
                    {
                        case (int)Lists.ItemSubTypes.AWARD:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 2, typeof(Lists.AwardItems), ParseException.UnrecognizedAwardItem(SplitLine));

                                return new InstructionItem((byte)Lists.Instructions.ITEM, Convert.ToByte(SubID), Value);
                            }
                        case (int)Lists.ItemSubTypes.GIVE:
                        case (int)Lists.ItemSubTypes.TAKE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 2, typeof(Lists.Items), ParseException.UnrecognizedInventoryItem(SplitLine));

                                return new InstructionItem((byte)Lists.Instructions.ITEM, Convert.ToByte(SubID), Value);
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionNop();
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
