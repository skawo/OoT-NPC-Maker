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
            List<TradeSetting> Failure = null;
            UInt16? Talk_TextID_Adult = null;
            UInt16? Talk_TextID_Child = null;
            int LineNoEnd = GetCorrespondingEndTrade(Lines, LineNo);

            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

                if (LineNoEnd < 0)
                    throw ParseException.TradeNotClosed(SplitLine);

                uint? Item = ScriptHelpers.Helper_GetEnumByName(SplitLine, 1, typeof(Lists.TradeItems), ParseException.UnrecognizedTradeItem(SplitLine));

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

                                UInt16? TextID_Adult = 0;
                                UInt16? TextID_Child = 0;

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref TextID_Adult, ref TextID_Child);

                                Correct = new TradeSetting(Convert.ToInt16(Item), (UInt16)TextID_Adult, (UInt16)TextID_Child);

                                LineNo++;

                                break;
                            }
                        case Lists.Keyword_TradeFailure:
                            {
                                if (Failure != null)
                                    throw ParseException.DuplicateTradeInstruction(Lines[LineNo]);

                                if (SplitLTrade.Length != 1)
                                {
                                    UInt16? TextID_Adult = 0;
                                    UInt16? TextID_Child = 0;

                                    ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref TextID_Adult, ref TextID_Child);

                                    Failure = new List<TradeSetting>() { new TradeSetting(-1, (UInt16)TextID_Adult, (UInt16)TextID_Child) } ;
                                    LineNo++;
                                }
                                else
                                {
                                    LineNo++;
                                    string[] SplitTrFailItem = Lines[LineNo].Split(' ');

                                    while (SplitTrFailItem[0].ToUpper().Trim() != Lists.Keyword_EndTradeFailure)
                                    {
                                        if (Failure == null)
                                            Failure = new List<TradeSetting>();

                                        UInt16? TextID_Adult_Fail = 0;
                                        UInt16? TextID_Child_Fail = 0;

                                        int? FailItem = -1;

                                        if (SplitTrFailItem[0].ToUpper().Trim() != Lists.Keyword_TradeDefault)
                                            FailItem = (int?)ScriptHelpers.Helper_GetEnumByName(SplitLine, 1, typeof(Lists.TradeItems), ParseException.UnrecognizedTradeItem(SplitLine));

                                        ScriptHelpers.Helper_GetAdultChildTextIds(SplitTrFailItem, ref TextID_Adult_Fail, ref TextID_Child_Fail);

                                        Failure.Add(new TradeSetting(Convert.ToInt16(FailItem), (UInt16)TextID_Adult_Fail, (UInt16)TextID_Child_Fail));

                                        LineNo++;
                                        SplitTrFailItem = Lines[LineNo].Split(' ');
                                    }

                                    LineNo++;
                                }
                                break;
                            }
                        case Lists.Keyword_TradeNone:
                            {
                                if (Talk_TextID_Adult != null)
                                    throw ParseException.DuplicateTradeInstruction(Lines[LineNo]);

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref Talk_TextID_Adult, ref Talk_TextID_Child);
                                LineNo++;
                                break;
                            }
                        default: 
                            throw ParseException.UnexpectedTradeInstruction(Lines[LineNo]);
                    }

                    SplitLTrade = Lines[LineNo].Split(' ');
                }

                if (Correct == null ||
                    Failure == null ||
                    Talk_TextID_Adult == null ||
                    Talk_TextID_Child == null)
                    throw ParseException.TradeMissingComponents(SplitLine);

                LineNo = LineNoEnd;

                return new InstructionTrade((byte)Lists.Instructions.TRADE, Correct, Failure, (UInt16)Talk_TextID_Adult, (UInt16)Talk_TextID_Child);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionTrade((int)Lists.Instructions.TRADE, new TradeSetting(-1, 0, 0), new List<TradeSetting>(), 0, 0);
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
