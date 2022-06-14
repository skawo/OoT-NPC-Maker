using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
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
            ScriptVarVal Talk_TextID_Adult = null;
            ScriptVarVal Talk_TextID_Child = null;

            int LineNoEnd = GetCorrespondingEndTrade(Lines, LineNo);

            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

                if (LineNoEnd < 0)
                    throw ParseException.TradeNotClosed(SplitLine);

                var Item = ScriptHelpers.GetScriptVarVal(SplitLine, 1, typeof(Lists.TradeItems), ParseException.UnrecognizedTradeItem(SplitLine));

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

                                ScriptVarVal TextID_Adult = new ScriptVarVal();
                                ScriptVarVal TextID_Child = new ScriptVarVal();

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref TextID_Adult, ref TextID_Child, Entry.Messages);

                                Correct = new TradeSetting(Item, TextID_Adult, TextID_Child);

                                LineNo++;

                                break;
                            }
                        case Lists.Keyword_TradeFailure:
                            {
                                if (Failure != null)
                                    throw ParseException.DuplicateTradeInstruction(Lines[LineNo]);

                                if (SplitLTrade.Length != 1)
                                {
                                    ScriptVarVal TextID_Adult = new ScriptVarVal();
                                    ScriptVarVal TextID_Child = new ScriptVarVal();

                                    ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref TextID_Adult, ref TextID_Child, Entry.Messages);

                                    Failure = new List<TradeSetting>() { new TradeSetting(new ScriptVarVal(-1), TextID_Adult, TextID_Child) };
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

                                        ScriptVarVal TextID_Adult_Fail = new ScriptVarVal();
                                        ScriptVarVal TextID_Child_Fail = new ScriptVarVal();

                                        ScriptVarVal FailItem = new ScriptVarVal(-1);

                                        bool PutAtEnd = false;

                                        if (SplitTrFailItem[0].ToUpper().Trim() != Lists.Keyword_TradeDefault)
                                            FailItem = ScriptHelpers.GetScriptVarVal(SplitTrFailItem, 0, typeof(Lists.TradeItems), ParseException.UnrecognizedTradeItem(SplitTrFailItem));
                                        else
                                            PutAtEnd = true;

                                        ScriptHelpers.Helper_GetAdultChildTextIds(SplitTrFailItem, ref TextID_Adult_Fail, ref TextID_Child_Fail, Entry.Messages);

                                        Failure.Insert(PutAtEnd ? Failure.Count : 0, new TradeSetting(FailItem, TextID_Adult_Fail, TextID_Child_Fail));

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

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref Talk_TextID_Adult, ref Talk_TextID_Child, Entry.Messages);

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

                return new InstructionTrade((byte)Lists.Instructions.TRADE, Correct, Failure, Talk_TextID_Adult, Talk_TextID_Child);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionNop();
            }
            catch (Exception exx)
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
