using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private List<Instruction> ParseOcarinaInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                List<Instruction> Instructions = new List<Instruction>();
                string LabelR = ScriptDataHelpers.RandomString(this);

                string True = "__OCARINATRUE__" + LabelR;
                string End = "__OCARINAEND__" + LabelR;

                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                int LineNoEnd = GetCorrespondingEndOcarina(Lines, LineNo);

                if (LineNoEnd < 0)
                    throw ParseException.OcarinaNotClosed(SplitLine);

                var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 1, typeof(Lists.OcarinaSongs), ParseException.UnrecognizedOcarinaSong(SplitLine));

                Instructions.Add(new InstructionOcarina((int)Lists.Instructions.OCARINA, Value.Value, Value.Vartype, True, End));
                Instructions.Add(new InstructionLabel(True));
                Instructions.AddRange(GetInstructions(Lines.Skip(LineNo + 1).Take(LineNoEnd - LineNo - 1).ToList()));
                Instructions.Add(new InstructionSet((byte)Lists.SetSubTypes.PLAYER_CAN_MOVE, new ScriptVarVal(1, 0), 0));
                Instructions.Add(new InstructionLabel(End));

                LineNo = LineNoEnd;

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


        private int GetCorrespondingEndOcarina(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndOcarina)
                    return i;
            }

            return -1;
        }
    }
}
