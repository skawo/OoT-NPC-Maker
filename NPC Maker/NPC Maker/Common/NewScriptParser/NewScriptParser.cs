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

            RegexText(ref ScriptText);
        }

        public BScript ParseScript()
        {
            outScript = new BScript();
            RandomLabels = new List<string>();

            if (ScriptText.Trim() == "")
                return outScript;

            if (outScript.ParseErrors.Count != 0)
                return outScript;

            // Split text into lines
            List<string> Lines = SplitLines(ScriptText);

            Lines = GetAndReplaceProcedures(Lines, ref outScript);
            Lines = ReplaceDefines(Lines, ref outScript);
            CheckLabels(Lines);

            List<Instruction> Instructions = GetInstructions(Lines);
            outScript.ScriptDebug = GetOutString(Instructions);

            return outScript;
        }

        public static void RegexText(ref string ScriptText)
        {
            ScriptText = ScriptText.Replace(",", " ").Replace("{", " ").Replace("}", " ").Replace("(", " ").Replace(")", " ");  // Remove ignored characters
            ScriptText = ScriptText.Replace(";", Environment.NewLine);                                                          // Change ;s into linebreaks
            ScriptText = Regex.Replace(ScriptText, @"/\*(.|[\r\n])*?\*/", string.Empty);                                        // Remove comment blocks
            ScriptText = Regex.Replace(ScriptText, "//.+", string.Empty);                                                       // Remove inline comments
            ScriptText = Regex.Replace(ScriptText, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();              // Remove empty lines
            ScriptText = ScriptText.Replace("\t", "");                                                                          // Remove tabs
            ScriptText = Regex.Replace(ScriptText, @"[ ]{2,}", " ");                                                            // Remove double spaces
        }

        public static List<string> SplitLines(string ScriptText)
        {
            List<string> Lines = ScriptText.Split(new[] { "\n" }, StringSplitOptions.None).ToList();

            for (int i = 0; i < Lines.Count(); i++)
                Lines[i] = Lines[i].Trim();

            return Lines;
        }

        public static List<string> GetLabels(string ScriptText)
        {
            NewScriptParser.ScriptParser.RegexText(ref ScriptText);
            List<string> Lines = SplitLines(ScriptText);

            return GetLabels(Lines);
        }

        private void CheckLabels(List<string> Lines)
        {
            foreach (string Line in Lines)
            {
                if (Line.EndsWith(":"))
                {
                    if (Lists.AllKeywords.Contains(Line.Remove(Line.Length - 1))
                        || Line.StartsWith("__"))
                    {
                        outScript.ParseErrors.Add(ParseException.LabelNameCannotBe(Line));
                        continue;
                    }
                }
            }
        }

        public static List<string> GetLabels(List<string> Lines)
        {
            List<string> Labels = new List<string>();

            for (int i = 0; i < Lines.Count(); i++)
            {
                if (Lines[i].EndsWith(":"))
                    Labels.Add(Lines[i].Substring(0, Lines[i].Length - 1));
            }

            return Labels;
        }

        private List<string> ReplaceDefines(List<string> Lines, ref BScript outScript)
        {
            try
            {
                List<string[]> Defines = new List<string[]>();

                foreach (string Line in Lines)
                {
                    if (Line.ToUpper().StartsWith(Lists.Keyword_SharpDefine))
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
                    if (Lines[i].ToUpper().StartsWith(Lists.Keyword_SharpDefine))
                        continue;

                    foreach (string[] Def in Defines)
                        Lines[i] = ScriptHelpers.ReplaceExpr(Lines[i], Def[0], Def[1]);

                    NewLines.Add(Lines[i]);
                }

                return NewLines;
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return Lines;
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.DefineError());
                return Lines;
            }
        }

        private List<string> GetAndReplaceProcedures(List<string> Lines, ref BScript outScript)
        {
            try
            {
                List<Procedure> Procedures = new List<Procedure>();

                while (Lines.Find(x => x.ToUpper().StartsWith(Lists.Keyword_Procedure)) != null)
                {
                    for (int i = 0; i < Lines.Count; i++)
                    {
                        if (Lines[i].ToUpper().StartsWith(Lists.Keyword_Procedure))
                        {
                            string[] Split = Lines[i].Split(' ');
                            ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                            int EndMacro = GetCorrespondingEndProcedure(Lines, i);

                            string ProcName = Split[1];
                            List<string> ProcLines = Lines.Skip(i + 1).Take(EndMacro - i - 1).ToList();
                            Lines.RemoveRange(i, EndMacro - i + 1);

                            string ProcedureString = $"{Lists.Keyword_CallProcedure}{ProcName}".ToUpper();

                            // Replace procedure calls with procedure
                            Procedure prc = new Procedure(ProcName, ProcLines);

                            // Error if procedure recursion is detected
                            if (ProcLines.Select(x => x.ToUpper()).Contains(ProcedureString))
                                throw ParseException.ProcRecursion(Lines[i]);

                            int Index = Lines.FindIndex(x => x.ToUpper() == ProcedureString);

                            while (Index != -1)
                            {
                                Lines.RemoveAt(Index);
                                Lines.InsertRange(Index, prc.Instructions);
                                Index = Lines.FindIndex(x => x.ToUpper() == ProcedureString);
                            }
                        }
                    }
                }

                return Lines;
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return Lines;
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.ProcedureError());
                return Lines;
            }
        }

        private int GetCorrespondingEndProcedure(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].ToUpper().StartsWith(Lists.Keyword_Procedure))
                    throw ParseException.ProcRecursion(Lines[i]);

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndProcedure)
                    return i;
            }

            throw ParseException.ProcedureNotClosed(Lines[LineNo]);
        }

        private List<string> GetOutString(List<Instruction> Instructions)
        {
            List<string> Out = new List<string>();

            int Size = 0;

            foreach (Instruction Int in Instructions)
            {
                Size += Int.ToBytes().Length;
                Out.Add(Int.ToString());
            }

            Out.Insert(0, $"-----SCRIPT SIZE: {Size} bytes-----" + Environment.NewLine);

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
                            Instructions.AddRange(ParseIfWhileInstruction(InstructionID, Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.TALK:
                            Instructions.AddRange(ParseTalkInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.TRADE: Instructions.Add(ParseTradeInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.NOP: Instructions.Add(ParseNopInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SET: Instructions.Add(ParseSetInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.AWAIT: Instructions.Add(ParseAwaitInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SHOW_TEXTBOX: Instructions.Add(ParseShowTextboxInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.ENABLE_TALKING: Instructions.Add(ParseEnableTalkingInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.PLAY: Instructions.Add(ParsePlayInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.GOTO: Instructions.Add(ParseGotoInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.WARP: Instructions.Add(ParseWarpInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.FACE: Instructions.Add(ParseFaceInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.KILL: Instructions.Add(ParseKillInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.CHANGE_SCRIPT: Instructions.Add(ParseChangeScriptInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SPAWN: Instructions.Add(ParseSpawnInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.ROTATION: Instructions.Add(ParseRotationInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.POSITION: Instructions.Add(ParsePositionInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SCALE: Instructions.Add(ParseScaleInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.ITEM: Instructions.Add(ParseItemInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.RETURN: Instructions.Add(ParseReturnInstruction(SplitLine)); break;
                        default:
                            outScript.ParseErrors.Add(ParseException.UnrecognizedInstruction(SplitLine)); break;
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
