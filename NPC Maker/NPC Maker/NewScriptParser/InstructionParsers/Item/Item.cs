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
                                UInt32? Value = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.GiveItems), ParseException.UnrecognizedGiveItem(SplitLine));

                                return new InstructionItem((byte)Lists.Instructions.ITEM, Convert.ToByte(SubID), Convert.ToUInt16(Value));
                            }
                        case (int)Lists.ItemSubTypes.GIVE:
                        case (int)Lists.ItemSubTypes.TAKE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                UInt32? Value = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Items), ParseException.UnrecognizedInventoryItem(SplitLine));

                                return new InstructionItem((byte)Lists.Instructions.ITEM, Convert.ToByte(SubID), Convert.ToUInt16(Value));
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionItem((byte)SubID, 0, 0);
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
