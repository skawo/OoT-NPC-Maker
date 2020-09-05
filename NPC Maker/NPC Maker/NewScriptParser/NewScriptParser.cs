using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private readonly string ScriptText;
        private readonly NPCEntry Entry;
        public List<string> RandomLabels { get; set; }
        private BScript outScript;

        public ScriptParser(NPCEntry _Entry, string _ScriptText)
        {
            ScriptText = _ScriptText;
            Entry = _Entry;

            ScriptText = ScriptText.Replace(",", " ").Replace("{", " ").Replace("}", " ").Replace("(", " ").Replace(")", " ");  // Remove ignored characters
            ScriptText = ScriptText.Replace(";", Environment.NewLine);                                                          // Change ;s into linebreaks
            ScriptText = Regex.Replace(ScriptText, @"/\*(.|[\r\n])*?\*/", string.Empty);                                        // Remove comment blocks
            ScriptText = Regex.Replace(ScriptText, "//.+", string.Empty);                                                       // Remove inline comments
            ScriptText = Regex.Replace(ScriptText, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();              // Remove empty lines
            ScriptText = ScriptText.Replace("\t", "");                                                                          // Remove tabs
            ScriptText = Regex.Replace(ScriptText, @"[ ]{2,}", " ");                                                            // Remove double spaces
        }


        public BScript ParseScript()
        {
            outScript = new BScript();
            RandomLabels = new List<string>();

            if (ScriptText.Trim() == "")
                return outScript;

            if (outScript.ParseErrors.Count != 0)
                return outScript;

            List<string> Lines = ScriptText.Split(new[] { "\n" }, StringSplitOptions.None).ToList();                                 // Split text into lines

            for (int i = 0; i < Lines.Count(); i++)
                Lines[i] = Lines[i].Trim();

            Lines = ReplaceDefines(Lines);
            CheckLabels(Lines);
            List<Instruction> Instructions = GetInstructions(Lines);

            outScript.ScriptDebug = GetOutString(Instructions);

            return outScript;
        }

        private List<string> ReplaceDefines(List<string> Lines)
        {
            List<string[]> Defines = new List<string[]>();

            foreach (string Line in Lines)
            {
                if (Line.ToUpper().StartsWith("#DEFINE"))
                {
                    string[] Split = Line.Split(' ');

                    ScriptHelpers.ErrorIfNumParamsNotEq(Split, 3);
                    Defines.Add(new string[] { Split[1], Split[2] });
                }
            }

            if (Defines.Count == 0)
                return Lines;

            List<string> NewLines = new List<string>();

            for (int i = 0; i < Lines.Count(); i++)
            {
                if (Lines[i].ToUpper().StartsWith("#DEFINE"))
                    continue;

                foreach (string[] Def in Defines)
                    Lines[i] = ScriptHelpers.ReplaceExpr(Lines[i], Def[0], Def[1]);

                NewLines.Add(Lines[i]);
            }

            return NewLines;
        }

        private void CheckLabels(List<string> Lines)
        {
            foreach (string Line in Lines)
            {
                if (Line.EndsWith(":"))
                {
                    if (Lists.Keywords.Contains(Line.Remove(Line.Length - 1))
                        || Line.StartsWith("__"))
                    {
                        outScript.ParseErrors.Add(ParseException.LabelNameCannotBe(Line));
                        continue;
                    }
                }
            }
        }


        private List<string> GetOutString(List<Instruction> Instructions)
        {
            List<string> Out = new List<string>();

            foreach (Instruction Int in Instructions)
                Out.Add(Int.ToString());

            return Out;
        }
        

        private List<Instruction> GetInstructions(List<string> Lines)
        {
            List<Instruction> Instructions = new List<Instruction>();

            for (int i = 0; i < Lines.Count(); i++)
            {
                try
                {
                    string[] SplitLine = Lines[i].Trim().Split(' ');

                    if (SplitLine.Count() == 1 && SplitLine[0].EndsWith(":"))
                    {
                        Instructions.Add(new InstructionLabel(SplitLine[0].Remove(SplitLine[0].Length - 1)));
                        continue;
                    }

                    int InstructionID = (int)System.Enum.Parse(typeof(Lists.Instructions), SplitLine[0].ToUpper());

                    switch (InstructionID)
                    {
                        case (int)Lists.Instructions.IF:
                        case (int)Lists.Instructions.WHILE:
                            {
                                List<Instruction> Instr = ParseIfWhileInstruction(InstructionID, Lines, SplitLine, ref i);
                                Instructions.AddRange(Instr);
                                break;
                            }
                        case (int)Lists.Instructions.TALK:
                            {
                                List<Instruction> Instr = ParseTalkInstruction(Lines, SplitLine, ref i);
                                Instructions.AddRange(Instr);
                                break;
                            }
                        case (int)Lists.Instructions.NOP: Instructions.Add(ParseNopInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SET: Instructions.Add(ParseSetInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.AWAIT: Instructions.Add(ParseAwaitInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SHOW_TEXTBOX: Instructions.Add(ParseShowTextboxInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.ENABLE_TALKING: Instructions.Add(ParseEnableTalkingInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.PLAY: Instructions.Add(ParsePlayInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.GOTO: Instructions.Add(ParseGotoInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.WARP: Instructions.Add(ParseWarpInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.KILL: Instructions.Add(ParseKillInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.CHANGE_SCRIPT: Instructions.Add(ParseChangeScriptInstruction(SplitLine)); break;

                        case (int)Lists.Instructions.RETURN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 1);
                                Instructions.Add(new InstructionGoto("__RETURN__"));
                                break;
                            }

                        default:
                            {
                                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                                break;
                            }
                    }
                }
                catch (Exception)
                {
                    outScript.ParseErrors.Add(ParseException.GeneralError(Lines[i]));
                    continue;
                }
            }

            return Instructions;
        }
    }
}
