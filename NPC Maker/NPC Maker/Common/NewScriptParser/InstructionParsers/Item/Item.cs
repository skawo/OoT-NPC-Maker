using MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseItemInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.ItemSubTypes));

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.ItemSubTypes.AWARD:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte ValueType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32? Value = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 2, ValueType, typeof(Lists.AwardItems), ParseException.UnrecognizedAwardItem(SplitLine));

                                return new InstructionItem((byte)Lists.Instructions.ITEM, Convert.ToByte(SubID), (UInt32)Value, ValueType);
                            }
                        case (int)Lists.ItemSubTypes.GIVE:
                        case (int)Lists.ItemSubTypes.TAKE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                byte ValueType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32? Value = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 2, ValueType, typeof(Lists.Items), ParseException.UnrecognizedInventoryItem(SplitLine));

                                return new InstructionItem((byte)Lists.Instructions.ITEM, Convert.ToByte(SubID), (UInt32)Value, ValueType);
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionItem((byte)Lists.Instructions.ITEM, (byte)SubID, 0, 0);
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
