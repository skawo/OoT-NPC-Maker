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
            object Talk_TextID_Adult = null;
            object Talk_TextID_Child = null;
            byte Talk_TextIDAdultT = 0;
            byte Talk_TextIDChildT = 0;
            int LineNoEnd = GetCorrespondingEndTrade(Lines, LineNo);

            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

                if (LineNoEnd < 0)
                    throw ParseException.TradeNotClosed(SplitLine);

                byte ItemT = ScriptHelpers.GetVarType(SplitLine, 1);
                object Item = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 1, ItemT, typeof(Lists.TradeItems), ParseException.UnrecognizedTradeItem(SplitLine));

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

                                object TextID_Adult = 0;
                                object TextID_Child = 0;
                                byte TextIDAdultT = 0;
                                byte TextIDChildT = 0;

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref TextID_Adult, ref TextID_Child, ref TextIDAdultT, ref TextIDChildT, Entry.Messages);

                                Correct = new TradeSetting(Item, TextID_Adult, TextID_Child, ItemT, TextIDAdultT, TextIDChildT);

                                LineNo++;

                                break;
                            }
                        case Lists.Keyword_TradeFailure:
                            {
                                if (Failure != null)
                                    throw ParseException.DuplicateTradeInstruction(Lines[LineNo]);

                                if (SplitLTrade.Length != 1)
                                {
                                    object TextID_Adult = 0;
                                    object TextID_Child = 0;
                                    byte TextIDAdultT = 0;
                                    byte TextIDChildT = 0;

                                    ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref TextID_Adult, ref TextID_Child, ref TextIDAdultT, ref TextIDChildT, Entry.Messages);

                                    Failure = new List<TradeSetting>() { new TradeSetting(-1, TextID_Adult, TextID_Child, 0, TextIDAdultT, TextIDChildT) };
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

                                        object TextID_Adult_Fail = 0;
                                        object TextID_Child_Fail = 0;

                                        byte TextID_Adult_FailT = 0;
                                        byte TextID_Child_FailT = 0;

                                        object FailItem = -1;
                                        byte FailItemT = (byte)Lists.VarTypes.Normal;

                                        if (SplitTrFailItem[0].ToUpper().Trim() != Lists.Keyword_TradeDefault)
                                        {
                                            FailItemT = ScriptHelpers.GetVarType(SplitTrFailItem, 0);
                                            FailItem = Convert.ToInt32(ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitTrFailItem, 0, FailItemT, typeof(Lists.TradeItems), ParseException.UnrecognizedTradeItem(SplitLine)));
                                        }

                                        ScriptHelpers.Helper_GetAdultChildTextIds(SplitTrFailItem, ref TextID_Adult_Fail, ref TextID_Child_Fail, ref TextID_Adult_FailT, ref TextID_Child_FailT, Entry.Messages);

                                        Failure.Add(new TradeSetting(FailItem, TextID_Adult_Fail, TextID_Child_Fail, FailItemT, TextID_Adult_FailT, TextID_Child_FailT));

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

                                object tTextID_Adult_Fail = 0;
                                object tTextID_Child_Fail = 0;

                                byte tTextID_Adult_FailT = 0;
                                byte tTextID_Child_FailT = 0;

                                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLTrade, ref tTextID_Adult_Fail, ref tTextID_Child_Fail, ref tTextID_Adult_FailT, ref tTextID_Child_FailT, Entry.Messages);

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
                    Failure == null ||
                    Talk_TextID_Adult == null ||
                    Talk_TextID_Child == null)
                    throw ParseException.TradeMissingComponents(SplitLine);

                LineNo = LineNoEnd;

                return new InstructionTrade((byte)Lists.Instructions.TRADE, Correct, Failure.OrderByDescending(x => x.Item).ToList(), Talk_TextID_Adult, Talk_TextID_Child, Talk_TextIDAdultT, Talk_TextIDChildT);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionTrade((int)Lists.Instructions.TRADE, new TradeSetting(-1, 0, 0, 0, 0, 0), new List<TradeSetting>(), 0, 0, 0, 0);
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
