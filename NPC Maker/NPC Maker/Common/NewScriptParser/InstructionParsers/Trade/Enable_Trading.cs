using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    /*
   * TRADE [item]
        CORRECT (Correct_message_id)
        WRONG
            MAGICBEAN (Incorrect_message_id_for_magicbean)
            DEFAULT (Default_incorrect_message_id)
        NONE (Talking_message_id)
    ENDTRADE
    
    IF TRADE_STATUS SUCCESS
        TALK

        ENDTALK
    ENDIF

    IF TRADE_STATUS WRONG
        TALK

        ENDTALK
    ENDIF

    IF TRADE_STATUS NONE
        TALK

        ENDTALK
    ENDIF*/

    public partial class ScriptParser
    {
        private Instruction ParseTradeInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            TradeSetting Correct = null;
            UInt32? Talk_TextID_Adult = null;
            UInt32? Talk_TextID_Child = null;
            byte Talk_TextIDAdultT = 0;
            byte Talk_TextIDChildT = 0;
            int LineNoEnd = GetCorrespondingEndTrade(Lines, LineNo);

            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

                if (LineNoEnd < 0)
                    throw ParseException.TradeNotClosed(SplitLine);

                byte ItemT = ScriptHelpers.GetVarType(SplitLine, 1);
                UInt32 Item = Convert.ToUInt32(ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 1, ItemT, typeof(Lists.TradeItems), ParseException.UnrecognizedTradeItem(SplitLine)));

                LineNo++;
                string[] SplitLTrade = Lines[LineNo].Split(' ');

                while (SplitLTrade[0].ToUpper().Trim() != Lists.Keyword_EndTrade)
                {
                    switch (SplitLTrade[0].ToUpper().Trim())
                    {
                        case Lists.Keyword_TradeSucccess:
                            {
                                if (Correct != null)
                                    throw ParseException.DuplicateTradeInstruction(Lines[LineNo]);

                                UInt32 TextID_Adult = 0;
                                UInt32 TextID_Child = 0;
                                byte TextIDAdultT = 0;
                                byte TextIDChildT = 0;

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref TextID_Adult, ref TextID_Child, ref TextIDAdultT, ref TextIDChildT);

                                Correct = new TradeSetting((Int32)Item, TextID_Adult, TextID_Child, ItemT, TextIDAdultT, TextIDAdultT);

                                LineNo++;

                                break;
                            }
                        case Lists.Keyword_TradeNone:
                            {
                                if (Talk_TextID_Adult != null)
                                    throw ParseException.DuplicateTradeInstruction(Lines[LineNo]);

                                UInt32 tTextID_Adult_Fail = 0;
                                UInt32 tTextID_Child_Fail = 0;

                                byte tTextID_Adult_FailT = 0;
                                byte tTextID_Child_FailT = 0;

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref tTextID_Adult_Fail, ref tTextID_Child_Fail, ref tTextID_Adult_FailT, ref tTextID_Child_FailT);

                                Talk_TextID_Adult = tTextID_Adult_Fail;
                                Talk_TextID_Child = tTextID_Child_Fail;
                                Talk_TextIDAdultT = tTextID_Adult_FailT;
                                Talk_TextIDChildT = tTextID_Child_FailT;

                                LineNo++;
                                break;
                            }
                        default: 
                            throw ParseException.UnexpectedTradeInstruction(Lines[LineNo]);
                    }

                    SplitLTrade = Lines[LineNo].Split(' ');
                }

                if (Correct == null ||
                    Talk_TextID_Adult == null ||
                    Talk_TextID_Child == null)
                    throw ParseException.TradeMissingComponents(SplitLine);

                LineNo = LineNoEnd;

                return new InstructionTrade((byte)Lists.Instructions.TRADE, Correct, (UInt32)Talk_TextID_Adult, (UInt32)Talk_TextID_Child, Talk_TextIDAdultT, Talk_TextIDChildT);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionTrade((int)Lists.Instructions.TRADE, new TradeSetting(-1, 0, 0, 0, 0, 0), 0, 0, 0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
            finally
            {
                if (LineNoEnd >= 0)
                    LineNo = LineNoEnd;
            }
        }

        private int GetCorrespondingEndTrade(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndTrade)
                    return i;
            }

            return -1;
        }
    }
}
