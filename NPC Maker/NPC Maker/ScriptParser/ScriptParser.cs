using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private readonly NPCEntry Entry;
        private string ScriptText;
        public List<string> RandomLabels { get; set; }
        private BScript outScript;

        public ScriptParser(NPCEntry _Entry, string _ScriptText, List<ScriptEntry> _GlobalHeader)
        {
            ScriptText = String.Join(Environment.NewLine, _GlobalHeader.Select(x => x.Text).ToArray()) + Environment.NewLine + _ScriptText;
            Entry = _Entry;
        }

        public BScript ParseScript()
        {
            RegexText(ref ScriptText);

            outScript = new BScript();
            RandomLabels = new List<string>();

            if (ScriptText.Trim() == "")
                return outScript;

            if (outScript.ParseErrors.Count != 0)
                return outScript;

            // Split text into lines
            List<string> Lines = SplitLines(ScriptText);

            // "Preprocessor"
            Lines = GetAndReplaceProcedures(Lines, ref outScript);
            Lines = ReplaceDefines(Lines, ref outScript);
            Lines = ReplaceElifs(Lines, ref outScript);
            Lines = ReplaceOrs(Lines, ref outScript);
            Lines = ReplaceAnds(Lines, ref outScript);
            CheckLabels(Lines);

            List<Instruction> Instructions = GetInstructions(Lines);

            // Add a return instruction at the end if one doesn't exist.
            if (Instructions.Count == 0 ||
                !(Instructions[Instructions.Count - 1] is InstructionGoto) ||
                ((Instructions[Instructions.Count - 1] as InstructionGoto).GotoInstr.Name != Lists.Keyword_Label_Return))
            {
                Instructions.Add(new InstructionGoto(Lists.Keyword_Label_Return));
            }

#if DEBUG
            outScript.ScriptDebug = GetOutString(Instructions);
#endif

            List<InstructionLabel> Labels = GetLabelsAndRemove(ref outScript, ref Instructions);

            // If everything was successful up 'till now, try parsing.
            if (outScript.ParseErrors.Count == 0)
            {
                outScript.Script = ConvertScriptToBytes(Labels, ref outScript, ref Instructions);

#if DEBUG
                outScript.ScriptDebug.Insert(0, $"-----SCRIPT SIZE: {outScript.Script.Length} bytes-----" + Environment.NewLine);
                System.IO.File.WriteAllBytes("DEBUGOUT", outScript.Script);
#endif
            }

            return outScript;
        }

        public static List<string> GetLabels(string ScriptText)
        {
            ScriptParser.RegexText(ref ScriptText);
            List<string> Lines = SplitLines(ScriptText);

            return GetLabels(Lines);
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

        private static void RegexText(ref string ScriptText)
        {
            string[] Operators = { "+=", "-=", "/=", "*=", "!=", "==", "=+", "=-", "=/", "=!", ">=", "<=" };

            // Separate oparators
            foreach (string op in Operators)
                ScriptText = ScriptText.Replace(op, " " + op + " ");

            ScriptText = Regex.Replace(ScriptText, @"([^>^<^=^\-^+^/^*^!])(=)([^=^\-^+^/^*^!^>^<])", m => m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value);
            ScriptText = Regex.Replace(ScriptText, @"([^=])(<)([^=])", m => m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value);
            ScriptText = Regex.Replace(ScriptText, @"([^=])(>)([^=])", m => m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value);

            ScriptText = Regex.Replace(ScriptText, @"\\\s+\n", "");                                                             // Override line carriage return if preceded by \
            ScriptText = ScriptText.Replace(",", " ").Replace("{", " ").Replace("}", " ").Replace("(", " ").Replace(")", " ");  // Remove ignored characters
            ScriptText = ScriptText.Replace(";", Environment.NewLine);                                                          // Change ;s into linebreaks
            ScriptText = Regex.Replace(ScriptText, @"\/\*([\s\S]*?)\*\/", string.Empty);                                        // Remove comment blocks
            ScriptText = Regex.Replace(ScriptText, "//.+", string.Empty);                                                       // Remove inline comments
            ScriptText = Regex.Replace(ScriptText, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();              // Remove empty lines
            ScriptText = ScriptText.Replace("\t", "");                                                                          // Remove tabs
            ScriptText = Regex.Replace(ScriptText, @"[ ]{2,}", " ");                                                            // Remove double spaces
        }

        private static List<string> SplitLines(string ScriptText)
        {
            List<string> Lines = ScriptText.Split(new[] { "\n" }, StringSplitOptions.None).ToList();

            for (int i = 0; i < Lines.Count(); i++)
                Lines[i] = Lines[i].Trim();

            return Lines;
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

        private List<string> ReplaceDefines(List<string> Lines, ref BScript outScript)
        {
            try
            {
                List<string[]> Defines = new List<string[]>();

                // Get all define lines...
                foreach (string Line in Lines)
                {
                    if (Line.ToUpper().StartsWith(Lists.Keyword_SharpDefine))
                    {
                        string[] Split = Line.Split(' ');

                        ScriptHelpers.ErrorIfNumParamsNotEq(Split, 3);

                        if (Defines.Find(x => x[0] == Split[1]) == null)
                            Defines.Add(new string[] { Split[1], Split[2] });
                        else
                            outScript.ParseErrors.Add(ParseException.RepeatDefine(Line));
                    }
                }

                // If none found, there's nothing to do.
                if (Defines.Count == 0)
                    return Lines;

                // Otherwise, loop through all lines and replace the defines.
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
                List<string> Procedures = new List<string>();

                List<string> NewLines = Lines.Select(item => (string)item.Clone()).ToList();
                int ProcLineIndex = Lines.FindIndex(x => x.ToUpper().StartsWith(Lists.Keyword_Procedure));

                // Looping through all lines until we can't find a line containing the keyword...
                while (ProcLineIndex != -1)
                {
                    string[] SplitDefinition = Lines[ProcLineIndex].Split(' ');
                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitDefinition, 2);

                    string ProcName = SplitDefinition[1];
                    string ProcedureString = $"{Lists.Keyword_CallProcedure}{ProcName}".ToUpper();

                    int EndMacro = GetCorrespondingEndProcedure(Lines, ProcLineIndex);
                    List<string> ProcLines = Lines.Skip(ProcLineIndex + 1).Take(EndMacro - ProcLineIndex - 1).ToList();
                    List<string> ProcArgs = new List<string>();

                    if (SplitDefinition.Length > 2)
                        ProcArgs = SplitDefinition.Skip(2).Take(SplitDefinition.Length - 2).ToList();

                    Lines.RemoveRange(ProcLineIndex, EndMacro - ProcLineIndex + 1);

                    if (!Procedures.Contains(ProcName))
                        Procedures.Add(ProcName);
                    else
                        outScript.ParseErrors.Add(ParseException.ProcDoubleError(SplitDefinition));

                    // Error if procedure recursion is detected
                    if (ProcLines.Select(x => x.ToUpper()).Contains(ProcedureString))
                        throw ParseException.ProcRecursion(Lines[ProcLineIndex]);

                    int ProcCallIndex = Lines.FindIndex(x => x.Split(' ')[0].ToUpper() == ProcedureString);

                    while (ProcCallIndex != -1)
                    {
                        string RepLine = Lines[ProcCallIndex];
                        string[] SplitRepLine = RepLine.Split(' ');

                        List<string> Args = new List<string>();

                        if (SplitRepLine.Length > 1)
                            Args = SplitRepLine.Skip(1).ToList();

                        Lines.RemoveAt(ProcCallIndex);

                        if (Args.Count != ProcArgs.Count)
                            outScript.ParseErrors.Add(ParseException.ProcNumArgsError(SplitRepLine, ProcArgs.ToArray()));
                        else
                        {
                            List<string> Instructions = new List<string>();

                            foreach (string Instruction in ProcLines)
                            {
                                string NewInstruction = Instruction;

                                for (int f = 0; f < Args.Count; f++)
                                    NewInstruction = ScriptHelpers.ReplaceExpr(NewInstruction, ProcArgs[f], Args[f]);

                                Instructions.Add(NewInstruction);
                            }

                            Lines.InsertRange(ProcCallIndex, Instructions);
                        }

                        ProcCallIndex = Lines.FindIndex(x => x.Split()[0].ToUpper() == ProcedureString);
                    }

                    ProcLineIndex = Lines.FindIndex(x => x.ToUpper().StartsWith(Lists.Keyword_Procedure));
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

        private List<string> ReplaceOrs(List<string> Lines, ref BScript outScript)
        {
            string OrKeyword = $" {Lists.Keyword_Or} ";

            int OrLineIndex = Lines.FindIndex(x => x.ToUpper().Contains(OrKeyword));

            while (OrLineIndex != -1)
            {
                string Label = ScriptDataHelpers.RandomString(this);
                string Line = Lines[OrLineIndex].ToUpper();

                bool If = Line.StartsWith(Lists.Instructions.IF.ToString());
                bool While = Line.StartsWith(Lists.Instructions.WHILE.ToString());

                if (!If && !While)
                {
                    Lines.RemoveAt(OrLineIndex);
                    outScript.ParseErrors.Add(ParseException.AndOrCanOnlyBeInIfWhile(Line));
                }
                else
                {
                    string First = Line.Substring(0, Line.IndexOf(OrKeyword)).Trim();
                    string Second = Line.Substring(Line.IndexOf(OrKeyword)).Trim();

                    string Repl = "";
                    int EndOfInstr = First.IndexOf(' ');

                    if (EndOfInstr == -1)
                        Repl = First;
                    else
                        Repl = First.Substring(0, EndOfInstr + 1);

                    Second = Second.ReplaceFirstExpr($"{Lists.Keyword_Or} ", Repl, RegexOptions.IgnoreCase);

                    Lines.RemoveAt(OrLineIndex);
                    Lines.Insert(OrLineIndex, First);
                    Lines.Insert(OrLineIndex + 1, $"{Lists.Instructions.GOTO} {Label}");
                    Lines.Insert(OrLineIndex + 2, If ? Lists.Keyword_EndIf : Lists.Keyword_EndWhile);
                    Lines.Insert(OrLineIndex + 3, Second);
                    Lines.Insert(OrLineIndex + 4, $"{Label}:");
                }

                OrLineIndex = Lines.FindIndex(x => x.ToUpper().Contains(OrKeyword));
            }

            return Lines;
        }

        private List<string> ReplaceAnds(List<string> Lines, ref BScript outScript)
        {
            string AndKeyword = $" {Lists.Keyword_And} ";

            int AndLineIndex = Lines.FindIndex(x => x.ToUpper().Contains(AndKeyword));

            while (AndLineIndex != -1)
            {
                string Line = Lines[AndLineIndex].ToUpper();

                bool If = Line.StartsWith(Lists.Instructions.IF.ToString());
                bool While = Line.StartsWith(Lists.Instructions.WHILE.ToString());

                if (!If && !While)
                {
                    Lines.RemoveAt(AndLineIndex);
                    outScript.ParseErrors.Add(ParseException.AndOrCanOnlyBeInIfWhile(Line));
                }
                else
                {
                    string First = Line.Substring(0, Line.IndexOf(AndKeyword)).Trim();
                    string Second = Line.Substring(Line.IndexOf(AndKeyword)).Trim();

                    string Repl = "";
                    int EndOfInstr = First.IndexOf(' ');

                    if (EndOfInstr == -1)
                        Repl = First;
                    else
                        Repl = First.Substring(0, EndOfInstr + 1);

                    int End = If ? GetCorrespondingEndIf(Lines, AndLineIndex) : GetCorrespondingEndWhile(Lines, AndLineIndex);

                    if (End < 0)
                    {
                        Lines.RemoveAt(AndLineIndex);
                        outScript.ParseErrors.Add(If ? ParseException.IfNotClosed(Lines[AndLineIndex]) : ParseException.WhileNotClosed(Lines[AndLineIndex]));
                    }
                    else
                    {
                        Second = Second.ReplaceFirstExpr($"{Lists.Keyword_And} ", Repl, RegexOptions.IgnoreCase);

                        Lines.RemoveAt(AndLineIndex);
                        Lines.Insert(AndLineIndex, First);
                        Lines.Insert(End, If ? Lists.Keyword_EndIf : Lists.Keyword_EndWhile);
                        Lines.Insert(AndLineIndex + 1, Second);
                    }
                }

                AndLineIndex = Lines.FindIndex(x => x.ToUpper().Contains(AndKeyword));
            }

            return Lines;
        }

        // Change Elifs into proper sets of Else EndIf
        private List<string> ReplaceElifs(List<string> Lines, ref BScript outScript)
        {
            int ElifLineIndex = Lines.FindIndex(x => x.ToUpper().StartsWith(Lists.Keyword_Elif));

            while (ElifLineIndex != -1)
            {
                bool InIfScope = false;
                int ScopeCounter = 0;

                // Checking if the elif is in scope.
                for (int i = ElifLineIndex; i >= 0; i--)
                {
                    if (Lines[i].ToUpper().StartsWith(Lists.Keyword_EndIf))
                        ScopeCounter++;

                    if (Lines[i].ToUpper().StartsWith(Lists.Keyword_Else) && ScopeCounter == 0)
                        break;


                    if (Lines[i].ToUpper().StartsWith(Lists.Instructions.IF.ToString()))
                    {
                        if (ScopeCounter == 0)
                        {
                            InIfScope = true;
                            break;
                        }
                        else
                            ScopeCounter--;
                    }
                }

                // If not in scope, remove the line, add an error, and continue.
                if (!InIfScope)
                {
                    outScript.ParseErrors.Add(ParseException.ElifNotInIfScope(Lines[ElifLineIndex]));
                    Lines.RemoveAt(ElifLineIndex);
                    ElifLineIndex = Lines.FindIndex(x => x.ToUpper().StartsWith(Lists.Keyword_Elif));
                    continue;
                }

                Lines[ElifLineIndex] = ScriptHelpers.ReplaceExpr(Lines[ElifLineIndex], Lists.Keyword_Elif, Lists.Instructions.IF.ToString(), RegexOptions.IgnoreCase);
                int EndIf = GetCorrespondingEndIf(Lines, ElifLineIndex);

                if (EndIf != -1)
                    Lines.Insert(EndIf, Lists.Keyword_EndIf);
                else
                    outScript.ParseErrors.Add(ParseException.IfNotClosed(Lines[ElifLineIndex]));

                Lines.Insert(ElifLineIndex, Lists.Keyword_Else);
                ElifLineIndex = Lines.FindIndex(x => x.ToUpper().StartsWith(Lists.Keyword_Elif));
            }

            return Lines;
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

                    int InstructionID = -1;

                    if (Enum.IsDefined(typeof(Lists.Instructions), SplitLine[0].ToUpper()))
                        InstructionID = (int)System.Enum.Parse(typeof(Lists.Instructions), SplitLine[0].ToUpper());

                    switch (InstructionID)
                    {
                        case (int)Lists.Instructions.IF:
                        case (int)Lists.Instructions.WHILE:
                            Instructions.AddRange(ParseIfWhileInstruction(InstructionID, Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.OCARINA:
                            Instructions.AddRange(ParseOcarinaInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.TALK:
                            Instructions.AddRange(ParseTalkInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.FORCE_TALK:
                            Instructions.AddRange(ParseForceTalkInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.TRADE: Instructions.Add(ParseTradeInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.NOP: Instructions.Add(ParseNopInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SET: Instructions.Add(ParseSetInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.AWAIT: Instructions.Add(ParseAwaitInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SHOW_TEXTBOX: Instructions.Add(ParseShowTextboxInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SHOW_TEXTBOX_SP: Instructions.Add(ParseShowTextboxSPInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.ENABLE_TALKING: Instructions.Add(ParseEnableTalkingInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.CLOSE_TEXTBOX: Instructions.Add(ParseCloseTextboxInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.PLAY: Instructions.Add(ParsePlayInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SCRIPT: Instructions.Add(ParseScriptInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.GOTO: Instructions.Add(ParseGotoInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.WARP: Instructions.Add(ParseWarpInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.FACE: Instructions.Add(ParseFaceInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.KILL: Instructions.Add(ParseKillInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SPAWN: Instructions.Add(ParseSpawnInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.PARTICLE: Instructions.Add(ParseParticleInstruction(Lines, SplitLine, ref i)); break;
                        case (int)Lists.Instructions.ROTATION: Instructions.Add(ParseRotationInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.POSITION: Instructions.Add(ParsePositionInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SCALE: Instructions.Add(ParseScaleInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.ITEM: Instructions.Add(ParseItemInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.PICKUP: Instructions.Add(ParsePickupInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.RETURN: Instructions.Add(ParseReturnInstruction(SplitLine)); break;
                        default:
                            {
                                byte? matchesSetRAM = ScriptHelpers.GetSubIDForRamType(SplitLine[0]);

                                if (matchesSetRAM != null || Enum.IsDefined(typeof(Lists.SetSubTypes), SplitLine[0].ToUpper()))
                                {
                                    List<string> sp = SplitLine.ToList();
                                    sp.Insert(0, Lists.Instructions.SET.ToString());
                                    Instructions.Add(ParseSetInstruction(sp.ToArray())); break;
                                }
                                else
                                    outScript.ParseErrors.Add(ParseException.UnrecognizedInstruction(SplitLine)); break;
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

        private static List<InstructionLabel> GetLabelsAndRemove(ref BScript outScript, ref List<Instruction> Instructions)
        {
            List<InstructionLabel> OutList = new List<InstructionLabel>();

            try
            {
                for (int i = 0; i < Instructions.Count; i++)
                {
                    if (Instructions[i] is InstructionLabel lbl)
                    {
                        Instructions.Remove(lbl);
                        lbl.InstructionNumber = (UInt16)(i);

                        if (OutList.Find(x => x.Name == lbl.Name) != null)
                            throw ParseException.LabelAlreadyExists(lbl.Name);

                        OutList.Add(lbl);
                        i--;
                    }
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Unknown error parsing labels."));
            }

            return OutList;
        }

        private static byte[] ConvertScriptToBytes(List<InstructionLabel> Labels, ref BScript outScript, ref List<Instruction> Instructions)
        {
            try
            {
                List<byte> Out = new List<byte>();
                List<UInt16> Offsets = new List<UInt16>();
                List<byte> InstructionBytes = new List<byte>();

                UInt32 HeaderOffs = 0;

                foreach (Instruction Inst in Instructions)
                {
                    byte[] Data = Inst.ToBytes(Labels);

                    Offsets.Add((UInt16)HeaderOffs);
                    InstructionBytes.AddRange(Data);
                    HeaderOffs += (UInt16)Data.Length;
                }

                for (int i = 0; i < Offsets.Count; i++)
                    Offsets[i] += (UInt16)(Offsets.Count * 2 + ((Offsets.Count % 2 != 0) ? 2 : 0));

                if (Offsets.Count % 2 != 0)
                    Offsets.Add(0);

                foreach (UInt16 Offset in Offsets)
                    Out.AddRange(Program.BEConverter.GetBytes(Offset));

                Out.AddRange(InstructionBytes);
                Helpers.Ensure4ByteAlign(Out);

                // If we exceed 16 bit range of addresses, the script is deemed too big (So, max script size is about 63KB. Plenty enough.)
                if (Out.Count > UInt16.MaxValue)
                    throw ParseException.ScriptTooBigError();

                return Out.ToArray();
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Clear();
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Unknown error converting to bytes."));
            }

            return new byte[0];
        }

#if DEBUG
        private List<string> GetOutString(List<Instruction> Instructions)
        {
            List<string> Out = new List<string>();

            foreach (Instruction Int in Instructions)
            {
                if (Int is InstructionLabel p)
                    Out.Add(p.ToMarkingString());
                else
                    Out.Add(Int.ToString());
            }

            return Out;
        }
#endif

    }
}
