using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseSwitchInstruction(ref List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

                int End = GetCorrespondingEndSwitch(Lines, LineNo);

                if (End < 0)
                    throw ParseException.SwitchNotClosed(SplitLine);

                int i = LineNo + 1;
                LineNo = End;

                string LabelR = ScriptDataHelpers.GetRandomLabelString(this);
                string ReturnLabel = $"__SWITCHRETURN__{LabelR}";
                string GotoReturnLabel = $"{Lists.Instructions.GOTO} __SWITCHRETURN__{LabelR}";
                Lines.Insert(End + 1, $"{ReturnLabel}:");

                List<SwitchEntry> entries = new List<SwitchEntry>();

                var switchedVar = ScriptHelpers.GetScriptVarVal(SplitLine, 1, float.MinValue, float.MaxValue);
                bool inCase = false;
                bool lastWasCase = false;
                int switchNest = 0;
                string lastGotoLabel = "";

                string defaultEntry = ReturnLabel;

                while (Lines[i].ToUpper().Trim() != Lists.Keyword_EndSwitch || switchNest != 0)
                {
                    string CurLine = Lines[i].ToUpper().Trim();

                    if (switchNest > 0 && CurLine == Lists.Keyword_EndSwitch)
                    {
                        Lines.Add(Lines[i]);
                        i++;
                        switchNest--;
                        lastWasCase = false;
                        continue;
                    }

                    if (CurLine.StartsWith(Lists.Instructions.SWITCH.ToString()))
                    {
                        lastWasCase = false;
                        switchNest++;
                    }

                    if (switchNest > 0)
                    {
                        Lines.Add(Lines[i]);
                        i++;
                        continue;
                    }

                    if (CurLine.TrimEnd(':') == Lists.Keyword_DefaultCase)
                    {
                        Lines[i] = $"{Lists.Keyword_Case} {Lists.Keyword_DefaultCase}:";
                        CurLine = Lines[i];
                    }

                    if (CurLine.StartsWith(Lists.Keyword_Case))
                    {
                        string[] SplitL = CurLine.Split(' ');
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitL, 2);

                        if (!SplitL[1].EndsWith(":"))
                            throw ParseException.CaseFormatError(SplitLine);
                        else
                        {
                            SplitL[1] = SplitL[1].TrimEnd(':');

                            ScriptVarVal CaseVar = null;

                            if (SplitL[1].ToUpper().Trim() != Lists.Keyword_DefaultCase)
                            {
                                CaseVar = ScriptHelpers.GetScriptVarVal(SplitL, 1, float.MinValue, float.MaxValue);

                                if (CaseVar.Vartype == (int)Lists.VarTypes.RANDOM)
                                    throw ParseException.CaseNotConstantError(SplitLine);
                            }
                            else if (defaultEntry != ReturnLabel)
                                throw ParseException.MultipleDefaultsError(SplitLine);

                            if (!lastWasCase)
                            {
                                lastGotoLabel = $"__GOTOSWITCH_{ScriptDataHelpers.GetRandomLabelString(this)}";

                                if (CaseVar == null)
                                    defaultEntry = lastGotoLabel;
                                else
                                {
                                    SwitchEntry currentEntry = new SwitchEntry(lastGotoLabel, CaseVar);
                                    entries.Add(currentEntry);
                                }

                                Lines.Add($"{lastGotoLabel}:");
                            }
                            else
                            {
                                if (CaseVar == null)
                                    defaultEntry = lastGotoLabel;
                                else
                                {
                                    SwitchEntry currentEntry = new SwitchEntry(lastGotoLabel, CaseVar);
                                    entries.Add(currentEntry);
                                }
                            }
                        }

                        inCase = true;
                        lastWasCase = true;
                    }
                    else if (!inCase)
                    {
                        throw ParseException.CaseFormatError(SplitLine);
                    }
                    else if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndCase)
                    {
                        Lines.Add(GotoReturnLabel);
                    }
                    else
                    {
                        Lines.Add(Lines[i]);
                        lastWasCase = false;
                    }

                    i++;
                }

                if (Lines[Lines.Count - 1] != GotoReturnLabel)
                    Lines.Add(GotoReturnLabel);

                return new InstructionSwitch(switchedVar, entries, new InstructionLabel(defaultEntry));
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

        private int GetCorrespondingEndSwitch(List<string> Lines, int LineNo)
        {
            return ScriptHelpers.GetCorresponding(Lines, LineNo, Lists.Instructions.SWITCH.ToString(), Lists.Keyword_EndSwitch);
        }
    }
}
