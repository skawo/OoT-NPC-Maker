using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private readonly NPCEntry Entry;
        private readonly NPCFile EditedFile;
        private string ScriptText = "";
        public List<string> RandomLabels { get; set; }
        private BScript outScript;

        public ScriptParser(NPCFile _File, NPCEntry _Entry, string _ScriptText, string baseDefines)
        {
            Entry = _Entry;
            EditedFile = _File;
            RandomLabels = new List<string>();
            ScriptText = string.Join(Environment.NewLine, 
                                     EditedFile.GlobalHeaders.Select(x => x.Text).Concat(new[] { baseDefines, _ScriptText }));
        }

        public void GetDefines(ref List<string> defines)
        {
            int id = 0x8000;

            defines.AddRange(Entry.Messages.Select(x => $"#{Lists.Keyword_Define} {Lists.Keyword_Msg}{x.Name} {id++}").ToList());

            id = 0;

            defines.AddRange(Entry.ExtraDisplayLists.Select(x => $"#{Lists.Keyword_Define} {Lists.Keyword_Dlist}{x.Name} {id++}").ToList());

            id = 0;

            defines.AddRange(Entry.Animations.Select(x => $"#{Lists.Keyword_Define} {Lists.Keyword_Anim}{x.Name} {id++}").ToList());

            int i = 8;
            id = 0;

            foreach (List<SegmentEntry> m in Entry.Segments)
            {
                defines.AddRange(m.Select(x => $"#{Lists.Keyword_Define} SEG{i}_DATAID_{x.Name} {id++}").ToList());
                i++;
            }

            id = 0;
            defines.AddRange(Entry.Scripts.Select(x => $"#{Lists.Keyword_Define} {Lists.Keyword_Script}{x.Name.Replace(' ', '_')} {id++}").ToList());
        }

        public BScript ParseScript(string scriptName, bool getBytes)
        {
            outScript = new BScript { Name = scriptName };

            // Load external header
            string extHeader = string.Empty;
            try
            {
                extHeader = EditedFile.GetExtHeader();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(new ParseException("Could not load external header:",
                    Helpers.ReplaceTokenWithPath(Program.Settings.ProjectPath, EditedFile.ExtScriptHeaderPath, "{PROJECTPATH}")));
            }

            ScriptText += Environment.NewLine + extHeader;
            RegexText(ref ScriptText);

            if (string.IsNullOrWhiteSpace(ScriptText))
                return outScript;

            RandomLabels = new List<string>();

            // Split text into lines and separate defines
            List<string> lines = SplitLines(ScriptText);
            var defineLines = new List<string>();
            var codeLines = new List<string>();

            foreach (string line in lines)
            {
                if (line.ToUpper().StartsWith($"#{Lists.Keyword_Define}"))
                    defineLines.Add(line);
                else
                    codeLines.Add(line);
            }

            lines = codeLines;
            GetDefines(ref defineLines);

            // Preprocessing pipeline
            lines = ReplaceTernary(lines, ref outScript);
            lines = GetAndReplaceProcedures(lines, defineLines, ref outScript);
            lines = ReplaceDefines(defineLines, lines, ref outScript);
            lines = ReplaceSwitches(lines, ref outScript);
            lines = ReplaceElifs(lines, ref outScript);
            lines = RefactorComplexIfs(lines, ref outScript);
            lines = ReplaceOrs(lines, ref outScript);
            lines = ReplaceAnds(lines, ref outScript);
            lines = ReplaceScriptStartHeres(lines, ref outScript);

            CheckLabels(lines);

            // Parse instructions
            List<Instruction> instructions = GetInstructions(lines);

            // Ensure script ends with return instruction
            if (instructions.Count == 0)
            {
                instructions.Add(new InstructionGoto(Lists.Keyword_Label_Return));
            }
            else
            {
                var lastInstruction = instructions[instructions.Count - 1] as InstructionGoto;
                if (lastInstruction == null || lastInstruction.GotoInstr.Name != Lists.Keyword_Label_Return)
                {
                    instructions.Add(new InstructionGoto(Lists.Keyword_Label_Return));
                }
            }

            #if DEBUG
                outScript.ScriptDebug = GetOutString(instructions);
            #endif

            List<InstructionLabel> labels = GetLabelsAndRemove(ref outScript, ref instructions);

            // Convert to bytes if no errors and requested
            #if DEBUG
                        bool shouldConvertToBytes = outScript.ParseErrors.Count == 0;
            #else
            bool shouldConvertToBytes = outScript.ParseErrors.Count == 0 && getBytes;
            #endif

            if (shouldConvertToBytes)
            {
                outScript.Script = ConvertScriptToBytes(labels, ref outScript, ref instructions);

                #if DEBUG
                        //outScript.ScriptDebug.Insert(0, $"-----SCRIPT SIZE: {outScript.Script.Length} bytes-----" + Environment.NewLine);
                        //System.IO.File.WriteAllBytes("DEBUGOUT", outScript.Script);
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

            foreach (string line in Lines)
            {
                string lineUpper = line.ToUpper();

                if (line.EndsWith(":") && lineUpper != $"{Lists.Keyword_DefaultCase}" && !lineUpper.StartsWith($"{Lists.Keyword_Case} "))
                    Labels.Add(line.Substring(0, line.Length - 1));
            }

            return Labels;
        }

        private static void RegexText(ref string ScriptText)
        {
            ScriptText = Regex.Replace(ScriptText, @"(\+=|-=|/=|\*=|!=|==|=\+|=-|=/|=!|>=|<=)", m => $" {m.Groups[1].Value} ", RegexOptions.Compiled);                                  // Separate operators
            ScriptText = Regex.Replace(ScriptText, @"([^><=\-+/*!])(=)([^=\-+/*!><])", m => $"{m.Groups[1].Value} {m.Groups[2].Value} {m.Groups[3].Value}", RegexOptions.Compiled);     // Separate single =s
            ScriptText = Regex.Replace(ScriptText, @"([^=])(<)([^=])", m => $"{m.Groups[1].Value} {m.Groups[2].Value} {m.Groups[3].Value}", RegexOptions.Compiled);                     // Separate single <s
            ScriptText = Regex.Replace(ScriptText, @"([^=-])(>)([^=])", m => $"{m.Groups[1].Value} {m.Groups[2].Value} {m.Groups[3].Value}", RegexOptions.Compiled);                    // Separate singe >s
            ScriptText = Regex.Replace(ScriptText, @"\\\s*\r?\n", "", RegexOptions.Compiled);                                                                                           // Override line carriage return if preceded by \
            ScriptText = Regex.Replace(ScriptText, @"[(){},\t](?=(?:[^""]*""[^""]*"")*[^""]*$)", " ", RegexOptions.Compiled);                                                           // Remove ignored characters
            ScriptText = ScriptText.Replace(";", Environment.NewLine);                                                                                                                  // Change ;s into linebreaks
            ScriptText = Regex.Replace(ScriptText, @"\/\*([\s\S]*?)\*\/", string.Empty, RegexOptions.Compiled);                                                                         // Remove comment blocks
            ScriptText = Regex.Replace(ScriptText, "//.+", string.Empty, RegexOptions.Compiled);                                                                                        // Remove inline comments
            ScriptText = Regex.Replace(ScriptText, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline | RegexOptions.Compiled).TrimEnd();                                              // Remove empty lines
            ScriptText = Regex.Replace(ScriptText, @"[ ]{2,}", " ", RegexOptions.Compiled);                                                                                             // Remove double spaces
            ScriptText = Regex.Replace(ScriptText, $@"{Lists.Keyword_Else} {Lists.Instructions.IF}", $"{Lists.Keyword_Elif}", RegexOptions.Compiled | RegexOptions.IgnoreCase);         // Remove double spaces

        }

        public static List<string> SplitLines(string ScriptText)
        {
            return Regex.Split(ScriptText, "\r?\n")
                        .Select(x => x.Trim())
                        .ToList();
        }

        private void CheckLabels(List<string> Lines)
        {
            foreach (string Line in Lines)
            {
                if (Line.EndsWith(":"))
                {
                    string labelN = Line.Substring(0, Line.Length - 1);

                    if (Lists.AllKeywords.Contains(labelN) || Line.StartsWith("__") || labelN.IsNumeric() ||
                        ScriptHelpers.IsHex(labelN) || ScriptHelpers.OnlyHexInString(labelN) ||
                        labelN.Equals(Lists.Keyword_Label_HERE, StringComparison.OrdinalIgnoreCase))
                    {
                        outScript.ParseErrors.Add(ParseException.LabelNameCannotBe(labelN));
                    }
                }
            }
        }

        private string[] FullyResolveDefine(List<string[]> Defines, string[] Define)
        {
            int index = Defines.FindIndex(x => String.Equals(x[1], Define[2]));

            if (index == -1)
                return Define;
            else
            {
                if (Define[1] == Defines[index][2])
                    return Define;
                else
                    return FullyResolveDefine(Defines, new string[] { Define[0], Define[1], Defines[index][2] });
            }
        }

        private List<string> ReplaceDefines(List<string> DefineLines, List<string> Lines, ref BScript outScript)
        {
            try
            {
                var defines = new string[DefineLines.Count][];
                var validDefines = new List<string[]>(DefineLines.Count);
                var defineNames = new HashSet<string>();

                // Single pass: parse, validate, and dedupe
                for (int i = 0; i < DefineLines.Count; i++)
                {
                    defines[i] = SplitWithQuotes(DefineLines[i]);

                    if (defines[i].Length != 3)
                    {
                        outScript.ParseErrors.Add(ParseException.DefineIncorrect(defines[i]));
                        continue;
                    }

                    if (!defineNames.Add(defines[i][1]))
                    {
                        outScript.ParseErrors.Add(ParseException.RepeatDefine(defines[i][1]));
                        continue;
                    }

                    validDefines.Add(defines[i]);
                }

                if (validDefines.Count == 0)
                    return Lines;

                string s = String.Join(Environment.NewLine, Lines);
                DataTable dt = new DataTable();

                // Only process defines that exist in the content
                foreach (var def in validDefines)
                {
                    if (!s.Contains(def[1])) continue;

                    string[] f = def[2].Trim().Split(' ');
                    string result;

                    if (f.Length == 1)
                    {
                        result = FullyResolveDefine(validDefines, def)[2];
                    }
                    else
                    {
                        for (int i = 0; i < f.Length; i++)
                            f[i] = FullyResolveDefine(validDefines, new[] { def[0], def[1], f[i] })[2];

                        try
                        {
                            result = dt.Compute(string.Join(" ", f), null)?.ToString() ?? string.Join(" ", f);
                        }
                        catch
                        {
                            result = string.Join(" ", f);
                        }
                    }

                    s = ScriptHelpers.ReplaceExpr(s, def[1], result);
                }

                return s.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
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


        private List<string> GetAndReplaceProcedures(List<string> Lines, List<string> DefineLines, ref BScript outScript)
        {
            try
            {
                HashSet<string> uniqueProcedures = new HashSet<string>();
                int ProcLineIndex = Lines.FindIndex(x => x.StartsWith(Lists.Keyword_Procedure, StringComparison.OrdinalIgnoreCase));

                Dictionary<string, List<string>[]> Procedures = new Dictionary<string, List<string>[]>();

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

                    if (!uniqueProcedures.Contains(ProcName))
                        uniqueProcedures.Add(ProcName);
                    else
                        outScript.ParseErrors.Add(ParseException.ProcDoubleError(SplitDefinition));

                    int RecurCheck = ProcLines.FindIndex(x => x.StartsWith($"{ProcedureString} ", StringComparison.OrdinalIgnoreCase));

                    // Error if procedure recursion is detected
                    if (RecurCheck != -1)
                        throw ParseException.ProcRecursion(ProcLines[RecurCheck]);

                    Procedures.Add(ProcedureString, new List<string>[2] { ProcLines, ProcArgs });
                    ProcLineIndex = Lines.FindIndex(ProcLineIndex, x => x.StartsWith(Lists.Keyword_Procedure, StringComparison.OrdinalIgnoreCase));
                }

                int ProcCallIndex = Lines.FindIndex(x => x.StartsWith(Lists.Keyword_CallProcedure, StringComparison.OrdinalIgnoreCase));

                while (ProcCallIndex != -1)
                {
                    string ProcedureString = Lines[ProcCallIndex].Split(' ')[0].ToUpper();

                    if (!Procedures.ContainsKey(ProcedureString))
                        throw ParseException.ProcedureNotFoundError(ProcedureString);

                    var proc = Procedures[ProcedureString];

                    List<string> ProcLines = proc[0];
                    List<string> ProcArgs = proc[1];

                    string RepLine = Lines[ProcCallIndex];
                    string[] SplitRepLine = RepLine.Split(' ');

                    List<string> Args = new List<string>();
                    int argIndex = 1; // Start with the first argument index

                    while (argIndex < SplitRepLine.Length)
                    {
                        string arg = SplitRepLine[argIndex];

                        if (arg.StartsWith("\""))
                        {
                            Match match = Regex.Match(RepLine.Substring(RepLine.IndexOf(arg)), "\"(?<arg>(?:\\\\\"|[^\"])*)\"", RegexOptions.Compiled);
                            if (match.Success)
                            {
                                // Unescape the quotes in the captured value
                                string unescapedArg = match.Groups["arg"].Value.Replace("\\\"", "\"");
                                Args.Add(unescapedArg);
                                argIndex += Regex.Split(match.Value, @"\s+").Length; 
                            }
                            else
                            {
                                throw ParseException.ArgsMalformedError(SplitRepLine);
                            }
                        }
                        else
                        {
                            Args.Add(arg);
                            argIndex++;
                        }
                    }

                    if (argIndex != SplitRepLine.Length)
                        throw ParseException.ArgsMalformedError(SplitRepLine);

                    Lines.RemoveAt(ProcCallIndex);

                    if (Args.Count != ProcArgs.Count)
                        outScript.ParseErrors.Add(ParseException.ProcNumArgsError(SplitRepLine, ProcArgs.ToArray()));
                    else
                    {
                        List<string> Instructions = new List<string>();

                        string s = String.Join(Environment.NewLine, ProcLines);

                        for (int f = 0; f < Args.Count; f++)
                            s = ScriptHelpers.ReplaceExprAndEscaped(s, ProcArgs[f], Args[f]);

                        Instructions = s.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                        Lines.InsertRange(ProcCallIndex, Instructions);
                    }


                    ProcCallIndex = Lines.FindIndex(ProcCallIndex, x => x.StartsWith(Lists.Keyword_CallProcedure, StringComparison.OrdinalIgnoreCase));
                }

                return Lines;
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new List<string>();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.ProcedureError());
                return new List<string>();
            }
        }

        private List<string> ReplaceOrs(List<string> Lines, ref BScript outScript)
        {
            try
            {
                string OrKeyword = $" {Lists.Keyword_Or} ";
                string AndKeyword = $" {Lists.Keyword_And} ";

                int OrLineIndex = Lines.FindIndex(x => x.ToUpper().Contains(OrKeyword));

                while (OrLineIndex != -1)
                {
                    string Label = ScriptDataHelpers.GetRandomLabelString(this);
                    string Line = Lines[OrLineIndex].ToUpper();

                    if (Line.Contains(AndKeyword))
                        outScript.ParseErrors.Add(ParseException.MixedAndOr(Line));


                    bool If = Line.StartsWith(Lists.Instructions.IF.ToString());

                    if (!If)
                    {
                        Lines.RemoveAt(OrLineIndex);
                        outScript.ParseErrors.Add(ParseException.AndOrCanOnlyBeInIf(Line));
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

                        Second = Second.ReplaceFirstExpr($"{Lists.Keyword_Or} ", Repl, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                        Lines.RemoveAt(OrLineIndex);
                        Lines.Insert(OrLineIndex, First);
                        Lines.Insert(OrLineIndex + 1, $"{Lists.Instructions.GOTO} {Label}");
                        Lines.Insert(OrLineIndex + 2, If ? Lists.Keyword_EndIf : Lists.Keyword_EndWhile);
                        Lines.Insert(OrLineIndex + 3, Second);
                        Lines.Insert(OrLineIndex + 4, $"{Label}:");
                    }

                    OrLineIndex = Lines.FindIndex(OrLineIndex, x => x.ToUpper().Contains(OrKeyword));

                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Error during parsing OR"));
            }


            return Lines;
        }

        private List<string> GenerateScriptFromLogicNode(NPC_Maker.ScriptParser.LogicNode lNode, string TrueLabel, string FalseLabel, ref BScript outScript)
        {
            List<string> res = new List<string>();

            switch (lNode.Type)
            {
                case NPC_Maker.ScriptParser.NodeType.OR:
                    {
                        string False1 = ScriptDataHelpers.GetRandomLabelString(this, 8);
                        res.AddRange(GenerateScriptFromLogicNode(lNode.Children[0], TrueLabel, False1, ref outScript));
                        res.Add($"{False1}:");
                        res.AddRange(GenerateScriptFromLogicNode(lNode.Children[1], TrueLabel, FalseLabel, ref outScript));
                        break;
                    }
                case NPC_Maker.ScriptParser.NodeType.AND:
                    {
                        string True1 = ScriptDataHelpers.GetRandomLabelString(this, 8);

                        res.AddRange(GenerateScriptFromLogicNode(lNode.Children[0], True1, FalseLabel, ref outScript));
                        res.Add($"{True1}:");
                        res.AddRange(GenerateScriptFromLogicNode(lNode.Children[1], TrueLabel, FalseLabel, ref outScript));
                        break;
                    }
                case NPC_Maker.ScriptParser.NodeType.STATEMENT:
                    {
                        res.Add($"{Lists.Instructions.IF} {lNode.Statement}");
                        res.Add($"{Lists.Instructions.GOTO} {TrueLabel}");
                        res.Add($"{Lists.Keyword_Else}");
                        res.Add($"{Lists.Instructions.GOTO} {FalseLabel}");
                        res.Add($"{Lists.Keyword_EndIf}");
                        break;
                    }
            }

            return res;
        }

        private int FindNextComplexAndOr(List<string> Lines)
        {
            string AndKeyword = $" {Lists.Keyword_And} ";
            string OrKeyword = $" {Lists.Keyword_Or} ";

            int lIndex = Lines.FindIndex(x =>
                x.ToUpper().StartsWith(Lists.Instructions.IF.ToString()) &&
                (x.ToUpper().Contains(AndKeyword) || x.ToUpper().Contains(OrKeyword)) &&
                (x.Contains("(") && x.Contains(")"))
            );

            return lIndex;
        }

        private List<string> RefactorComplexIfs(List<string> Lines, ref BScript outScript)
        {
            try
            {
                int lIndex = FindNextComplexAndOr(Lines);

                while (lIndex != -1)
                {
                    string Line = Lines[lIndex].ToUpper();
                    
                    int End = GetCorrespondingEndIf(Lines, lIndex);
                    int Else = GetCorrespondingElse(Lines, lIndex, End);

                    List<string> LinesTrue = Lines.Skip(lIndex + 1).Take(Else != -1 ? Else - lIndex - 1 : End - lIndex - 1).ToList();
                    List<string> LinesFalse = Else == -1 ? null : Lines.Skip(Else + 1).Take(End - Else - 1).ToList();

                    Lines.RemoveRange(lIndex, End - lIndex + 1);

                    List<string> NewLines = new List<string>();

                    string TrueLabel = ScriptDataHelpers.GetRandomLabelString(this, 8);

                    NewLines.Add($"{TrueLabel}:");
                    NewLines.AddRange(LinesTrue);

                    string FalseLabel = ScriptDataHelpers.GetRandomLabelString(this, 8);

                    if (LinesFalse != null)
                    {
                        NewLines.Add($"{FalseLabel}:");
                        NewLines.AddRange(LinesFalse);
                    }
                    else
                        NewLines.Add($"{FalseLabel}:");

                    var lNode = NPC_Maker.ScriptParser.IfParser.ParseIf(Line);

                    List<string> NodeLines = GenerateScriptFromLogicNode(lNode, TrueLabel, FalseLabel, ref outScript);

                    NewLines.InsertRange(0, NodeLines);
                    Lines.InsertRange(lIndex, NewLines);

                    lIndex = FindNextComplexAndOr(Lines);
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Error during parsing AND"));
            }


            return Lines;
        }


        private List<string> ReplaceAnds(List<string> Lines, ref BScript outScript)
        {
            try
            {
                string AndKeyword = $" {Lists.Keyword_And} ";
                string OrKeyword = $" {Lists.Keyword_Or} ";

                int AndLineIndex = Lines.FindIndex(x => x.ToUpper().Contains(AndKeyword));

                while (AndLineIndex != -1)
                {
                    string Line = Lines[AndLineIndex].ToUpper();

                    if (Line.Contains(OrKeyword))
                        outScript.ParseErrors.Add(ParseException.MixedAndOr(Line));

                    bool If = Line.StartsWith(Lists.Instructions.IF.ToString());

                    if (!If)
                    {
                        Lines.RemoveAt(AndLineIndex);
                        outScript.ParseErrors.Add(ParseException.AndOrCanOnlyBeInIf(Line));
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
                            outScript.ParseErrors.Add(If ? ParseException.IfNotClosed(Lines[AndLineIndex]) : ParseException.WhileNotClosed(Lines[AndLineIndex]));
                            Lines.RemoveAt(AndLineIndex);
                        }
                        else
                        {
                            Second = Second.ReplaceFirstExpr($"{Lists.Keyword_And} ", Repl, RegexOptions.IgnoreCase);

                            int Else = GetCorrespondingElse(Lines, AndLineIndex, End);

                            Lines.RemoveAt(AndLineIndex);
                            Lines.Insert(AndLineIndex, First);
                            Lines.Insert(End, If ? Lists.Keyword_EndIf : Lists.Keyword_EndWhile);
                            Lines.Insert(AndLineIndex + 1, Second);

                            if (Else != -1)
                            {
                                string JumpElseLabel = ScriptDataHelpers.GetRandomLabelString(this, 5);

                                Lines.Insert(Else + 2, $"ANDELSE_{JumpElseLabel}:");
                                Lines.Insert(End + 3, Lists.Keyword_Else);
                                Lines.Insert(End + 4, $"{Lists.Instructions.GOTO} ANDELSE_{JumpElseLabel}");
                            }
                        }
                    }

                    AndLineIndex = Lines.FindIndex(AndLineIndex, x => x.ToUpper().Contains(AndKeyword));
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Error during parsing AND"));
            }

            return Lines;
        }

        private List<string> ReplaceScriptStartHeres(List<string> Lines, ref BScript outScript)
        {
            if (Lines == null || Lines.Count == 0)
                return new List<string>();

            var outputLines = new List<string>(Lines.Count + Lines.Count / 4); // Pre-allocate with estimated capacity

            // Pre-compile patterns for better performance
            var scriptStartPattern1 = $"{Lists.Instructions.SET} {Lists.SetSubTypes.SCRIPT_START} {Lists.Keyword_Label_HERE}";
            var scriptStartPattern2 = $"{Lists.SetSubTypes.SCRIPT_START} {Lists.Keyword_Label_HERE}";
            var labelToVarPattern = $"{Lists.Instructions.SET} {Lists.SetSubTypes.LABEL_TO_VAR} {Lists.Keyword_Label_HERE} ";
            var labelToVarfPattern = $"{Lists.Instructions.SET} {Lists.SetSubTypes.LABEL_TO_VARF} {Lists.Keyword_Label_HERE} ";

            foreach (string line in Lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    outputLines.Add(line);
                    continue;
                }

                string lineUpperTrimmed = line.ToUpperInvariant().Trim();

                // Handle script start patterns
                if (lineUpperTrimmed == scriptStartPattern1 || lineUpperTrimmed == scriptStartPattern2)
                {
                    string newLabel = ScriptDataHelpers.GetRandomLabelString(this, 7);
                    outputLines.Add($"{newLabel}:");
                    outputLines.Add($"{Lists.Instructions.SET} {Lists.SetSubTypes.SCRIPT_START} {newLabel}");
                }
                // Handle label to variable patterns
                else if (lineUpperTrimmed.StartsWith(labelToVarPattern, StringComparison.Ordinal) ||
                         lineUpperTrimmed.StartsWith(labelToVarfPattern, StringComparison.Ordinal))
                {
                    var splitLine = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (splitLine.Length >= 4)
                    {
                        string newLabel = ScriptDataHelpers.GetRandomLabelString(this, 7);
                        outputLines.Add($"{newLabel}:");
                        outputLines.Add($"{Lists.Instructions.SET} {splitLine[1]} {newLabel} {splitLine[3]}");
                    }
                    else
                    {
                        // Malformed line, keep original
                        outputLines.Add(line);
                    }
                }
                else
                {
                    outputLines.Add(line);
                }
            }

            return outputLines;
        }


        // Change Switch Cases into If...Elif...Endif
        private List<string> ReplaceSwitches(List<string> Lines, ref BScript outScript)
        {
            try
            {
                int SwitchLineIndex = Lines.FindIndex(x => x.ToUpper().StartsWith($"{Lists.Keyword_Switch} "));

                while (SwitchLineIndex != -1)
                {
                    string[] splitL = Lines[SwitchLineIndex].ToUpper().Trim().Split(' ');

                    if (splitL.Length != 2)
                        outScript.ParseErrors.Add(ParseException.ParamCountWrong(splitL));

                    string swVar = splitL[1];

                    int end = ScriptHelpers.GetCorresponding(Lines, SwitchLineIndex, Lists.Keyword_Switch, Lists.Keyword_EndSwitch);

                    if (end < 0)
                        throw ParseException.SwitchNotClosed(Lines[SwitchLineIndex]);

                    bool InCase = false;
                    bool firstCase = true;
                    bool hasDefault = false;
                    int switchInner = 0;

                    Lines[SwitchLineIndex] = "";
                    Lines[end] = $"{Lists.Keyword_EndIf}";

                    for (int i = SwitchLineIndex + 1; i < end; i++)
                    {
                        string[] splitLl = Lines[i].ToUpper().Trim().Split(' ');

                        string st = splitLl[0];

                        if (st == Lists.Keyword_Switch)
                        {
                            switchInner++;
                            continue;
                        }

                        if (st == Lists.Keyword_EndSwitch)
                        {
                            switchInner--;
                            continue;
                        }

                        if (switchInner > 0)
                            continue;

                        if (!InCase)
                        {
                            if (st == Lists.Keyword_Case)
                            {
                                if (hasDefault)
                                    throw ParseException.DefaultStatementMustBeLast(splitL);

                                if (splitLl.Length != 2 || !splitLl[1].EndsWith(":"))
                                    throw ParseException.CaseFormatError(splitLl);

                                string swVarL = splitLl[1].TrimEnd(':');

                                if (firstCase)
                                    Lines[i] = $"{Lists.Instructions.IF} {swVar} == {swVarL}";
                                else
                                    Lines[i] = $"{Lists.Keyword_Elif} {swVar} == {swVarL}";

                                InCase = true;
                                firstCase = false;
                            }
                            else if (st == Lists.Keyword_DefaultCase)
                            {
                                if (hasDefault)
                                    throw ParseException.MultipleDefaultsError(splitL);

                                hasDefault = true;
                                InCase = true;

                                if (firstCase)
                                {
                                    Lines[i] = "";
                                    Lines[end] = "";
                                    firstCase = false;
                                }
                                else
                                    Lines[i] = $"{Lists.Keyword_Else}";
                            }
                            else
                                throw ParseException.StatementOutsideCaseError(splitLl);
                        }
                        else
                        {
                            if (st == Lists.Keyword_EndCase)
                            {
                                Lines[i] = "";
                                InCase = false;
                                continue;
                            }
                            else if (st == Lists.Keyword_Case)
                            {
                                if (hasDefault)
                                    throw ParseException.DefaultStatementMustBeLast(splitL);

                                string LabelR = ScriptDataHelpers.GetRandomLabelString(this);
                                Lines.Insert(i, $"{Lists.Instructions.GOTO} {LabelR}");
                                i++;
                                end++;

                                if (splitLl.Length != 2)
                                    throw ParseException.CaseFormatError(splitLl);

                                string swVarL = splitLl[1].TrimEnd(':');

                                Lines[i] = $"{Lists.Keyword_Elif} {swVar} == {swVarL}";
                                Lines.Insert(i + 1, $"{LabelR}:");
                                i++;
                                end++;
                            }
                            else if (st == Lists.Keyword_DefaultCase)
                            {
                                if (hasDefault)
                                    throw ParseException.MultipleDefaultsError(splitL);

                                hasDefault = true;

                                string LabelR = ScriptDataHelpers.GetRandomLabelString(this);
                                Lines.Insert(i, $"{Lists.Instructions.GOTO} {LabelR}");
                                i++;
                                end++;

                                Lines[i] = $"{Lists.Keyword_Else}";
                                Lines.Insert(i + 1, $"{LabelR}:");
                                i++;
                                end++;
                            }
                            else
                                continue;
                        }
                    }

                    SwitchLineIndex = Lines.FindIndex(SwitchLineIndex, x => x.ToUpper().StartsWith($"{Lists.Keyword_Switch} "));
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception ex)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Error during parsing SWITCH " + ex.Message));
            }

            return Lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        private List<string> ReplaceTernary(List<string> Lines, ref BScript outScript)
        {
            try
            {
                int TernaryIndex = Lines.FindIndex(x => x.ToUpper().Contains($" {Lists.Keyword_Ternary} "));

                while (TernaryIndex != -1)
                {
                    string[] SplitLine = Lines[TernaryIndex].Trim().Split(' ');
                    Lines.RemoveAt(TernaryIndex);

                    string IfSt = String.Join(" ", SplitLine.TakeWhile(x => x != Lists.Keyword_Ternary));
                    string Condition = String.Join(" ", SplitLine.SkipWhile(x => x != Lists.Keyword_Ternary).Skip(1).TakeWhile(x => x != Lists.Keyword_TernarySplit));
                    string Condition2 = String.Join(" ", SplitLine.SkipWhile(x => x != Lists.Keyword_TernarySplit).Skip(1));

                    Lines.Insert(TernaryIndex, $"{Lists.Instructions.IF} {IfSt}");
                    Lines.Insert(TernaryIndex + 1, Condition);
                    Lines.Insert(TernaryIndex + 2, Lists.Keyword_Else);
                    Lines.Insert(TernaryIndex + 3, Condition2);
                    Lines.Insert(TernaryIndex + 4, Lists.Keyword_EndIf);

                    TernaryIndex = Lines.FindIndex(TernaryIndex, x => x.ToUpper().Contains($" {Lists.Keyword_Ternary} "));
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception ex)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Error during parsing ELIF " + ex.Message));
            }

            return Lines;
        }

        // Change Elifs into proper sets of Else EndIf
        private List<string> ReplaceElifs(List<string> Lines, ref BScript outScript)
        {
            try
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
                    ElifLineIndex = Lines.FindIndex(ElifLineIndex, x => x.ToUpper().StartsWith(Lists.Keyword_Elif));
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception ex)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Error during parsing ELIF " + ex.Message));
            }

            return Lines;
        }

        private int GetCorrespondingEndProcedure(List<string> Lines, int LineNo)
        {
            int outIndex = Lines.FindIndex(x => String.Equals(x.Trim(), Lists.Keyword_EndProcedure, StringComparison.OrdinalIgnoreCase));

            if (outIndex == -1)
                throw ParseException.ProcedureNotClosed(Lines[LineNo]);
            else
                return outIndex;
        }
        static string[] SplitWithQuotes(string input)
        {
            return input.Split('"')
                        .Select((s, i) => i % 2 == 0 ? s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new[] { s })
                        .SelectMany(s => s)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToArray();
        }

        private List<Instruction> GetInstructions(List<string> lines)
        {
            var instructions = new List<Instruction>();

            for (int i = 0; i < lines.Count; i++)
            {
                try
                {
                    string[] splitLine = SplitWithQuotes(lines[i].Trim());

                    // Handle labels
                    if (splitLine.Length == 1 && splitLine[0].EndsWith(":"))
                    {
                        instructions.Add(new InstructionLabel(splitLine[0].TrimEnd(':')));
                        continue;
                    }

                    // Get instruction type
                    string instructionUpperCase = splitLine[0].ToUpper();
                    Lists.Instructions? instructionType = null;

                    if (Enum.IsDefined(typeof(Lists.Instructions), instructionUpperCase))
                        instructionType = (Lists.Instructions)Enum.Parse(typeof(Lists.Instructions), instructionUpperCase);

                    // Parse instruction based on type
                    if (instructionType.HasValue)
                    {
                        switch (instructionType.Value)
                        {
                            case Lists.Instructions.IF:
                            case Lists.Instructions.WHILE:
                                instructions.AddRange(ParseIfWhileInstruction((int)instructionType.Value, Entry.EmbeddedOverlayCode, lines, splitLine, ref i));
                                break;

                            case Lists.Instructions.OCARINA:
                                instructions.AddRange(ParseOcarinaInstruction(lines, splitLine, ref i));
                                break;

                            case Lists.Instructions.TALK:
                                instructions.AddRange(ParseTalkInstruction(lines, splitLine, ref i));
                                break;

                            case Lists.Instructions.FORCE_TALK:
                                instructions.AddRange(ParseForceTalkInstruction(splitLine));
                                break;

                            case Lists.Instructions.TRADE:
                                instructions.Add(ParseTradeInstruction(lines, splitLine, ref i));
                                break;

                            case Lists.Instructions.NOP:
                                instructions.Add(ParseNopInstruction(splitLine));
                                break;

                            case Lists.Instructions.SET:
                                instructions.Add(ParseSetInstruction(splitLine));
                                break;

                            case Lists.Instructions.AWAIT:
                                instructions.Add(ParseAwaitInstruction(Entry.EmbeddedOverlayCode, splitLine));
                                break;

                            case Lists.Instructions.SHOW_TEXTBOX:
                                instructions.Add(ParseShowTextboxInstruction(splitLine));
                                break;

                            case Lists.Instructions.SHOW_TEXTBOX_SP:
                                instructions.Add(ParseShowTextboxSPInstruction(splitLine));
                                break;

                            case Lists.Instructions.ENABLE_TALKING:
                                instructions.Add(ParseEnableTalkingInstruction(splitLine));
                                break;

                            case Lists.Instructions.CLOSE_TEXTBOX:
                                instructions.Add(ParseCloseTextboxInstruction(splitLine));
                                break;

                            case Lists.Instructions.PLAY:
                                instructions.Add(ParsePlayInstruction(splitLine));
                                break;

                            case Lists.Instructions.SCRIPT:
                                instructions.Add(ParseScriptInstruction(splitLine));
                                break;

                            case Lists.Instructions.GOTO:
                                instructions.Add(ParseGotoInstruction(splitLine));
                                break;

                            case Lists.Instructions.WARP:
                                instructions.Add(ParseWarpInstruction(splitLine));
                                break;

                            case Lists.Instructions.FACE:
                                instructions.Add(ParseFaceInstruction(splitLine));
                                break;

                            case Lists.Instructions.KILL:
                                instructions.Add(ParseKillInstruction(splitLine));
                                break;

                            case Lists.Instructions.SPAWN:
                                instructions.Add(ParseSpawnInstruction(lines, splitLine, ref i));
                                break;

                            case Lists.Instructions.PARTICLE:
                                instructions.Add(ParseParticleInstruction(lines, splitLine, ref i));
                                break;

                            case Lists.Instructions.ROTATION:
                                instructions.Add(ParseRotationInstruction(splitLine));
                                break;

                            case Lists.Instructions.POSITION:
                                instructions.Add(ParsePositionInstruction(splitLine));
                                break;

                            case Lists.Instructions.SCALE:
                                instructions.Add(ParseScaleInstruction(splitLine));
                                break;

                            case Lists.Instructions.ITEM:
                                instructions.Add(ParseItemInstruction(splitLine));
                                break;

                            case Lists.Instructions.PICKUP:
                                instructions.Add(ParsePickupInstruction(splitLine));
                                break;

                            case Lists.Instructions.RETURN:
                                instructions.Add(ParseReturnInstruction(splitLine));
                                break;

                            case Lists.Instructions.SAVE:
                                instructions.Add(ParseSaveInstruction(splitLine));
                                break;

                            case Lists.Instructions.FADEIN:
                            case Lists.Instructions.FADEOUT:
                                instructions.Add(ParseFadeInstruction(splitLine));
                                break;

                            case Lists.Instructions.QUAKE:
                                instructions.Add(ParseQuakeInstruction(splitLine));
                                break;

                            case Lists.Instructions.CCALL:
                                instructions.Add(ParseCCallInstruction(Entry.EmbeddedOverlayCode, splitLine));
                                break;

                            case Lists.Instructions.GET:
                                instructions.Add(ParseGetInstruction(splitLine));
                                break;

                            case Lists.Instructions.GOTO_VAR:
                                instructions.Add(ParseGotoVarInstruction(splitLine));
                                break;

                            case Lists.Instructions.STOP:
                                instructions.Add(ParseStopInstruction(splitLine));
                                break;
                        }
                    }
                    else
                    {
                        // Handle unknown instructions - check if it's a SET variant
                        byte? matchesSetRAM = ScriptHelpers.GetSubIDForRamType(splitLine[0]);

                        if (matchesSetRAM.HasValue || Enum.IsDefined(typeof(Lists.SetSubTypes), instructionUpperCase))
                        {
                            var modifiedSplitLine = new List<string> { Lists.Instructions.SET.ToString() };
                            modifiedSplitLine.AddRange(splitLine);
                            instructions.Add(ParseSetInstruction(modifiedSplitLine.ToArray()));
                        }
                        else
                        {
                            outScript.ParseErrors.Add(ParseException.UnrecognizedInstruction(splitLine));
                        }
                    }
                }
                catch (Exception)
                {
                    outScript.ParseErrors.Add(ParseException.GeneralError(lines[i]));
                }
            }

            return instructions;
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
            catch (Exception ex)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Error parsing labels: " + ex.Message));
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

                    if (HeaderOffs % 4 != 0)
                        throw ParseException.AlignmentError();

                    Offsets.Add((UInt16)(HeaderOffs / 4));
                    InstructionBytes.AddRange(Data);
                    HeaderOffs += (UInt16)Data.Length;
                }

                // Add the size of the header to the offsets
                UInt16 OffsetsOffset = (UInt16)((Offsets.Count * 2 + ((Offsets.Count % 2 != 0) ? 2 : 0)) / 4);

                // Add the size of the header to the offsets
                for (int i = 0; i < Offsets.Count; i++)
                    Offsets[i] += OffsetsOffset;

                // Ensure there's an even amount of offsets so everything is 4-aligned
                if (Offsets.Count % 2 != 0)
                    Offsets.Add(0);

                foreach (UInt16 Offset in Offsets)
                    Out.AddRange(Program.BEConverter.GetBytes(Offset));

                Out.AddRange(InstructionBytes);
                Helpers.Ensure4ByteAlign(Out);

                // If we exceed 16 bit range of addresses, the script is deemed too big (So, max script size is about 240KB. Plenty enough.)
                if ((Out.Count / 4) > UInt16.MaxValue)
                    throw ParseException.ScriptTooBigError();

                return Out.ToArray();
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Clear();
                outScript.ParseErrors.Add(pEx);
            }
            catch (Exception ex)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError("Unknown error converting to bytes" + ex.Message));
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
