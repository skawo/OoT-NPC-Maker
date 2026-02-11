using NPC_Maker.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private List<Instruction> ParseAsyncInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                var instructions = new List<Instruction>();
                string label = ScriptDataHelpers.GetRandomLabelString(this);

                int subId = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.AsyncTypes));

                if (subId == (int)Lists.AsyncTypes.EXIT)
                {
                    instructions.Add(new InstructionAsyncExit((byte)Lists.Instructions.ASYNC, (byte)Lists.AsyncTypes.EXIT));
                    return instructions;
                }

                List<Instruction> body;

                if (SplitLine.Length > 2)
                {
                    string Line = string.Join(" ", SplitLine.Skip(2));
                    body = GetInstructions(new List<string>() { Line });
                }
                else
                {
                    int start = LineNo;
                    int end = GetCorrespondingEndAsync(Lines, LineNo);

                    if (end < 0)
                        throw ParseException.AsyncNotClosed(Lines[LineNo]);

                    LineNo = end;

                    body = GetInstructions(Lines.Skip(start + 1).Take(end - start - 1).ToList());
                }

                instructions.Add(new InstructionAsync((byte)Lists.Instructions.ASYNC, (byte)subId, label));
                instructions.Add(new InstructionLabel("__ASYNC_START__" + label));
                instructions.AddRange(body);

                if (subId == (int)Lists.AsyncTypes.LOOP)
                    instructions.Add(new InstructionGoto(Lists.Keyword_Label_Return));
                else
                    instructions.Add(new InstructionAsyncExit((byte)Lists.Instructions.ASYNC, (byte)Lists.AsyncTypes.EXIT));

                instructions.Add(new InstructionLabel("__ASYNC_END__" + label));

                return instructions;
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new List<Instruction>();
            }
            catch (Exception ex)
            {
                BigMessageBox.Show(ex.Message);
                outScript.ParseErrors.Add(
                    ParseException.GeneralError(SplitLine));
                return new List<Instruction>();
            }
        }

        public int GetCorrespondingEndAsync(List<string> Lines, int LineNo)
        {
            return ScriptHelpers.GetCorresponding(Lines, LineNo, Lists.Instructions.ASYNC.ToString(), Lists.Keyword_EndAsync);
        }
    }
}
