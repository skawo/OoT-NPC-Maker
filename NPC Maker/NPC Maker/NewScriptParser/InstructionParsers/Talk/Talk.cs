using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private List<Instruction> ParseTalkInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                List<Instruction> Instructions = new List<Instruction>();
                string LabelR = ParserHelpers.RandomString(this, 5);

                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                UInt16 TextID_Adult = 0;
                UInt16 TextID_Child = 0;

                TextID_Adult = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 1, 0, UInt16.MaxValue));
                TextID_Child = (SplitLine.Count() == 2) ? TextID_Adult : Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                int End = GetCorrespondingEndTalking(Lines, LineNo);

                if (End == -1)
                    throw ParseException.TalkNotClosed(SplitLine);

                Instructions.Add(new InstructionTextbox((byte)Lists.Instructions.ENABLE_TALKING, TextID_Adult, TextID_Child));
                Instructions.Add(new InstructionIfWhile((byte)Lists.Instructions.IF, (byte)Lists.IfSubTypes.CURRENTLY_TALKING, 0, 0, 1, -1, -1, LabelR));
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

                    i = GetCorrespondingEndWhile(Lines, i);

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
