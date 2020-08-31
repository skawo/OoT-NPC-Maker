using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private string ScriptText;
        private List<Label> Labels;
        private NPCEntry Entry;
        private bScript outScript;

        public ScriptParser(NPCEntry _Entry, string _ScriptText)
        {
            ScriptText = _ScriptText;
            Entry = _Entry;

            ScriptText = Regex.Replace(ScriptText, @"/\*(.|[\r\n])*?\*/", string.Empty);                                // Remove comment blocks
            ScriptText = Regex.Replace(ScriptText, "//.+", string.Empty);                                               // Remove inline comments
            ScriptText = Regex.Replace(ScriptText, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();      // Remove empty lines
            ScriptText = ScriptText.Replace("\t", "");                                                                  // Remove tabs
        }


        public bScript ParseScript()
        {
            outScript = new bScript();

            if (ScriptText.Trim() == "")
                return outScript;

            if (outScript.ParseErrors.Count != 0)
                return outScript;

            List<string> Lines = ScriptText.Split(new[] { "\n" }, StringSplitOptions.None).ToList();                                 // Split text into lines

            for (int i = 0; i < Lines.Count(); i++)
                Lines[i] = Lines[i].Trim();

            Labels = GetLabels(Lines);
            List<Instruction> Instructions = GetInstructions(Lines);




            return outScript;
        }

        private List<Label> GetLabels(List<string> Lines)
        {
            List<Label> Labels = new List<Label>();

            foreach (string Line in Lines)
            {
                if (Line.EndsWith(":"))
                {
                    if (Lists.Keywords.Contains(Line.Remove(Line.Length - 1)))
                    {
                        outScript.ParseErrors.Add(ParseException.LabelNameCannotBe(Line));
                        continue;
                    }

                    if (Labels.Find(x => x.Name == Line) != null)
                    {
                        outScript.ParseErrors.Add(ParseException.LabelAlreadyExists(Line));
                        continue;
                    }

                    Labels.Add(new Label(Line, -1));
                }    
            }

            return Labels;
        }

        private List<Instruction> GetInstructions(List<string> Lines)
        {
            List<Instruction> Instructions = new List<Instruction>();

            for (int i = 0; i < Lines.Count(); i++)
            {
                try
                {
                    string[] SplitLine = Lines[i].Trim().Split(' ');

                    int InstructionID = (int)System.Enum.Parse(typeof(Lists.Instructions), SplitLine[0].ToUpper());

                    switch (InstructionID)
                    {
                        case (int)Lists.Instructions.IF:
                            {
                                Instruction IfInstruction = ParseIfInstruction(Lines, SplitLine, i);

                                if (IfInstruction.ID == (int)Lists.Instructions.IF)
                                    i = (IfInstruction as InstructionIf).EndIfLineNo + 1;

                                Instructions.Add(IfInstruction);
                                break;
                            }
                        case (int)Lists.Instructions.NOP: Instructions.Add(ParseNopInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SET: Instructions.Add(ParseSetInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.AWAIT: Instructions.Add(ParseAwaitInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.SHOW_TEXTBOX: Instructions.Add(ParseShowTextboxInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.ENABLE_TALKING: Instructions.Add(ParseEnableTalkingInstruction(SplitLine)); break;
                        case (int)Lists.Instructions.PLAY: Instructions.Add(ParsePlayInstruction(SplitLine)); break;


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
