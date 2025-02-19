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
                int i = LineNo + 1;
                LineNo = End;

                if (End < 0)
                    throw ParseException.IfNotClosed(SplitLine);

                string LabelR = ScriptDataHelpers.GetRandomLabelString(this);
                Lines.Insert(End + 1, $"__SWITCHRETURN__{LabelR}:");

                if (Lines[Lines.Count - 1].ToUpper().Trim() != (Lists.Instructions.RETURN.ToString()))
                    Lines.Add(Lists.Instructions.RETURN.ToString());

                List<SwitchEntry> entries = new List<SwitchEntry>();

                var switchedVar = ScriptHelpers.GetScriptVarVal(SplitLine, 1, float.MinValue, float.MaxValue);
                bool inCase = false;
                bool lastWasCase = false;
                string lastGotoLabel = "";

                while (Lines[i].ToUpper().Trim() != Lists.Keyword_EndSwitch)
                {
                    if (Lines[i].ToUpper().Trim().StartsWith(Lists.Keyword_Case))
                    {
                        string[] SplitL = Lines[i].Trim().Split(' ');
                        ScriptHelpers.ErrorIfNumParamsNotEq(SplitL, 2);

                        if (!SplitL[1].EndsWith(":"))
                            throw ParseException.CaseFormatError(SplitLine);
                        else
                        {
                            SplitL[1] = SplitL[1].TrimEnd(':');
                            var CaseVar = ScriptHelpers.GetScriptVarVal(SplitL, 1, float.MinValue, float.MaxValue);

                            if (CaseVar.Vartype == (int)Lists.VarTypes.RANDOM)
                                throw ParseException.CaseNotConstantError(SplitLine);

                            if (!lastWasCase)
                            {
                                lastGotoLabel = $"__GOTOSWITCH_{ScriptDataHelpers.GetRandomLabelString(this)}";

                                SwitchEntry currentEntry = new SwitchEntry(lastGotoLabel, CaseVar);
                                entries.Add(currentEntry);

                                Lines.Add($"{lastGotoLabel}:");
                            }
                            else
                            {
                                SwitchEntry currentEntry = new SwitchEntry(lastGotoLabel, CaseVar);
                                entries.Add(currentEntry);
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
                        Lines.Add($"{Lists.Instructions.GOTO} __SWITCHRETURN__{LabelR}");
                    }
                    else
                    {
                        Lines.Add(Lines[i]);
                        lastWasCase = false;
                    }

                    i++;
                }

                Lines.Add($"{Lists.Instructions.GOTO} __SWITCHRETURN__{LabelR}");

                return new InstructionSwitch(switchedVar, entries);
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
