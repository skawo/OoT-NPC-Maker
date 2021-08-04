using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private List<Instruction> ParseTalkInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                List<Instruction> Instructions = new List<Instruction>();
                string LabelR = ScriptDataHelpers.RandomString(this);

                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                object TextID_Adult = 0;
                object TextID_Child = 0;
                byte TextIDAdultT = 0;
                byte TextIDChildT = 0;

                ScriptHelpers.Helper_GetAdultChildTextIds(SplitLine, ref TextID_Adult, ref TextID_Child, ref TextIDAdultT, ref TextIDChildT, Entry.Messages);

                int End = GetCorrespondingEndTalking(Lines, LineNo);

                if (End == -1)
                    throw ParseException.TalkNotClosed(SplitLine);

                Instructions.Add(new InstructionTextbox((byte)Lists.Instructions.ENABLE_TALKING, TextID_Adult, TextID_Child, TextIDAdultT, TextIDChildT));
                Instructions.Add(new InstructionIfWhile((byte)Lists.Instructions.IF, (byte)Lists.IfSubTypes.IS_TALKING, 0, 0, Lists.ConditionTypes.TRUE, -1, -1, LabelR));
                Instructions.Add(new InstructionLabel("__IFTRUE__" + LabelR));
                Instructions.AddRange(GetInstructions(Lines.Skip(LineNo + 1).Take(End - LineNo - 1).ToList()));
                Instructions.Add(new InstructionGoto("__ENDIF__" + LabelR));
                Instructions.Add(new InstructionLabel("__IFFALSE__" + LabelR));
                Instructions.Add(new InstructionLabel("__ENDIF__" + LabelR));

                LineNo = End;

                return Instructions;
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new List<Instruction>();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new List<Instruction>();
            }
        }

        private int GetCorrespondingEndTalking(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].Split(' ')[0].ToUpper() == Lists.Instructions.TALK.ToString())
                {
                    int j = i;

                    i = GetCorrespondingEndTalking(Lines, i);

                    if (i < 0)
                        throw ParseException.IfNotClosed(Lines[j]);

                    continue;
                }

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndTalk)
                    return i;
            }

            return -1;
        }
    }
}
