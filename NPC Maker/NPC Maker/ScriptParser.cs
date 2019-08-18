using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NPC_Maker
{
    public enum InstructionIDs
    {
        NOP = 0,
        IF = 1,
        SETFLAG = 2,
        SETRESPONSE = 3,
        ENABLETEXTBOX = 4,
        SHOWTEXTBOX = 5,
        GIVEITEM = 6,
        GOTO = 7,
        SETANIM = 8,
        WAITFORTEXTEND = 9,
        STOP = 10,
        TURNTOPLAYER = 11,
        RETURNIFNOTTALKING = 12,
        STOPMOVEMENT = 13,
        ENABLEMOVEMENT = 14,
        PLAYSND = 15,
        SUBTRACTRUPEES = 16
    }

    public class ScriptParser
    {
        public List<string> ParseErrors = new List<string>();
        private static Dictionary<string, int> Labels = new Dictionary<string, int>();

        public ScriptParser()
        {
        }

        public byte[] Parse(string Script)
        {
            ParseErrors.Clear();

            if (Script.Trim() == "")
                return new byte[0];

            Script = Regex.Replace(Script, @"/\*(.|[\r\n])*?\*/", string.Empty);                                // Remove comment blocks
            Script = Regex.Replace(Script, "//.+", string.Empty);                                               // Remove inline comments
            Script = Regex.Replace(Script, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();      // Remove empty lines

            List<string> Lines = Script.Split(new[] { "\n" }, StringSplitOptions.None).ToList();                // Split text into lines
            Labels = GetLabels(Lines, ref ParseErrors);                                                         // Get all the labels with their indexes...

            foreach (string Line in Labels.Keys)
            {
                if (Labels.ContainsKey(Line))                                                                   // Then remove them from the text
                    Lines.RemoveAll(x => x == Line);
            }

            List<byte> Parsed = new List<byte>();

            foreach (string Line in Lines)
                Parsed.AddRange(GetInstructionBytes(Line, ref ParseErrors));                                    // Convert every instruction into an 8 byte array and add it to the output

            return Parsed.ToArray();
        }

        private static Dictionary<string, int> GetLabels(List<string> Lines, ref List<string> ParseErrors)
        {
            Dictionary<string, int> Labels = new Dictionary<string, int>();

            for (int i = 0; i < Lines.Count(); i++)
            {
                if (Lines[i].EndsWith(":"))
                {
                    if (Labels.ContainsKey(Lines[i]))
                        ParseErrors.Add("Label \"" + Lines[i].Substring(0, Lines[i].Length - 1) + "\" is defined more than once.");
                    else
                        Labels.Add(Lines[i], i - Labels.Count);                                                 // Decrementing the index by label count, since we'll be removing them
                }
            }

            return Labels;
        }

        private static int GetLabelOffset(string Line, string Label)
        {
            if (!Labels.ContainsKey(Label + ":"))
                throw new LabelNotFoundException(Line);
            else
                return Labels[Label + ":"];
        }

        private static UInt32 Helper_ConvertToUInt32(string Number)
        {
            UInt32 Result = 0;

            if (Number.Length >= 3 && Number.Substring(0, 2) == "0x")
                Result = Convert.ToUInt32(Number, 16);
            else
                Result = Convert.ToUInt32(Number);

            return Result;
        }

        private static byte[] GetInstructionBytes(string Line, ref List<string> ParseErrors)
        {
            string[] Instr = Line.TrimEnd().Split(' ');

            try
            {
                switch (Instr[0].ToLower())
                {
                    case "if":
                        {
                            if (Instr.Length != 7)
                                throw new WrongParamCountException(Line);

                            UInt32 FlagID = Helper_ConvertToUInt32(Instr[2]);

                            if (FlagID > UInt16.MaxValue || FlagID < 0)
                                throw new ParamOutOfRangeException(Line);

                            int Label_True = GetLabelOffset(Line, Instr[4]);
                            int Label_False = GetLabelOffset(Line, Instr[6]);

                            if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                throw new LabelOutOfRangeException(Line);

                            if (Instr[3].ToLower() != "then" || Instr[5].ToLower() != "else")
                                throw new Exception();

                            IfInstruction If = new IfInstruction(Instr[1], Convert.ToUInt16(FlagID), Convert.ToUInt16(Label_True), Convert.ToUInt16(Label_False));
                            return If.GetByteData();
                        }
                    case "enable_textbox":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            UInt32 TextID = Helper_ConvertToUInt32(Instr[1]);

                            if (TextID > UInt16.MaxValue || TextID < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction EnableSay = new GenericU16Instruction((byte)InstructionIDs.ENABLETEXTBOX, Convert.ToUInt16(TextID));
                            return EnableSay.GetByteData();
                        }
                    case "show_textbox":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            UInt32 TextID = Helper_ConvertToUInt32(Instr[1]);

                            if (TextID > UInt16.MaxValue || TextID < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction Say = new GenericU16Instruction((byte)InstructionIDs.SHOWTEXTBOX, Convert.ToUInt16(TextID));
                            return Say.GetByteData();
                        }
                    case "give_item":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            UInt32 ItemID = Helper_ConvertToUInt32(Instr[1]);

                            if (ItemID > UInt16.MaxValue || ItemID < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction Give = new GenericU16Instruction((byte)InstructionIDs.GIVEITEM, Convert.ToUInt16(ItemID));
                            return Give.GetByteData();
                        }
                    case "set_responses":
                        {
                            if (Instr.Length < 2 || Instr.Length > 4)
                                throw new WrongParamCountException(Line);

                            int Label_1 = GetLabelOffset(Line, Instr[1]);
                            int Label_2 = Instr.Length > 2 ? GetLabelOffset(Line, Instr[2]) : GetLabelOffset(Line, Instr[1]);
                            int Label_3 = Instr.Length > 3 ? GetLabelOffset(Line, Instr[3]) :
                                                             Instr.Length > 2 ? GetLabelOffset(Line, Instr[2]) : GetLabelOffset(Line, Instr[1]);

                            if (Label_1 > UInt16.MaxValue || Label_2 > UInt16.MaxValue || Label_3 > UInt16.MaxValue)
                                throw new LabelOutOfRangeException(Line);

                            SetResponseInstruction Respond = new SetResponseInstruction(Convert.ToUInt16(Label_1), Convert.ToUInt16(Label_2), Convert.ToUInt16(Label_3));
                            return Respond.GetByteData();
                        }
                    case "goto":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            int GotoLabel = GetLabelOffset(Line, Instr[1]);

                            if (GotoLabel > UInt16.MaxValue || GotoLabel > UInt16.MaxValue)
                                throw new LabelOutOfRangeException(Line);

                            GenericU16Instruction Goto = new GenericU16Instruction((byte)InstructionIDs.GOTO, Convert.ToUInt16(GotoLabel));
                            return Goto.GetByteData();
                        }
                    case "set_anim":
                        {
                            if (Instr.Length < 2 && Instr.Length > 3)
                                throw new WrongParamCountException(Line);

                            UInt32 AnimID = Helper_ConvertToUInt32(Instr[1]);
                            UInt32 Loops = 0;

                            if (AnimID > UInt16.MaxValue || AnimID < 0)
                                throw new ParamOutOfRangeException(Line);

                            if (Instr.Length == 3)
                            {
                                Loops = Helper_ConvertToUInt32(Instr[2]);

                                if (Loops > Byte.MaxValue || Loops < 0)
                                    throw new ParamOutOfRangeException(Line);
                            }

                            SetAnimInstruction SetAnim = new SetAnimInstruction(Convert.ToByte(Loops), Convert.ToUInt16(AnimID));
                            return SetAnim.GetByteData();
                        }
                    case "set_flag":
                        {
                            if (Instr.Length != 3)
                                throw new WrongParamCountException(Line);

                            UInt32 FlagID = Helper_ConvertToUInt32(Instr[2]);

                            if (FlagID > UInt16.MaxValue || FlagID < 0)
                                throw new ParamOutOfRangeException(Line);

                            SetFlagInstruction SetFlag = new SetFlagInstruction(Instr[1], Convert.ToUInt16(FlagID));
                            return SetFlag.GetByteData();
                        }
                    case "return":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericU16Instruction Return = new GenericU16Instruction((byte)InstructionIDs.GOTO, 0);
                            return Return.GetByteData();
                        }
                    case "wait_for_text_end":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Wait = new GenericInstruction((byte)InstructionIDs.WAITFORTEXTEND);
                            return Wait.GetByteData();
                        }
                    case "turn_towards_player":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Turn = new GenericInstruction((byte)InstructionIDs.TURNTOPLAYER);
                            return Turn.GetByteData();
                        }
                    case "return_if_not_talking":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Turn = new GenericInstruction((byte)InstructionIDs.RETURNIFNOTTALKING);
                            return Turn.GetByteData();
                        }
                    case "wait_for_response":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction WaitForResp = new GenericInstruction((byte)InstructionIDs.STOP);
                            return WaitForResp.GetByteData();
                        }
                    case "stop":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Stop = new GenericInstruction((byte)InstructionIDs.STOP);
                            return Stop.GetByteData();
                        }
                    case "player_lock_movement":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Stop = new GenericInstruction((byte)InstructionIDs.STOPMOVEMENT);
                            return Stop.GetByteData();
                        }
                    case "player_unlock_movement":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Stop = new GenericInstruction((byte)InstructionIDs.ENABLEMOVEMENT);
                            return Stop.GetByteData();
                        }
                    case "nop":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Stop = new GenericInstruction((byte)InstructionIDs.NOP);
                            return Stop.GetByteData();
                        }
                    case "play_snd":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            UInt32 SNDID = Helper_ConvertToUInt32(Instr[1]);

                            if (SNDID > UInt16.MaxValue || SNDID > UInt16.MaxValue)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction Sound = new GenericU16Instruction((byte)InstructionIDs.PLAYSND, Convert.ToUInt16(SNDID));
                            return Sound.GetByteData();
                        }
                    case "subtract_rupees":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            UInt32 RupeeCount = Helper_ConvertToUInt32(Instr[1]);

                            if (RupeeCount > UInt16.MaxValue || RupeeCount > UInt16.MaxValue)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction RupeeSub = new GenericU16Instruction((byte)InstructionIDs.SUBTRACTRUPEES, Convert.ToUInt16(RupeeCount));
                            return RupeeSub.GetByteData();
                        }
                    default: throw new Exception();
                }
            }
            catch (WrongParamCountException ex)
            {
                ParseErrors.Add(ex.Message);
                return new byte[8];
            }
            catch (ParamOutOfRangeException ex)
            {
                ParseErrors.Add(ex.Message);
                return new byte[8];
            }
            catch (LabelOutOfRangeException ex)
            {
                ParseErrors.Add(ex.Message);
                return new byte[8];
            }
            catch (LabelNotFoundException ex)
            {
                ParseErrors.Add(ex.Message);
                return new byte[8];
            }
            catch (Exception)
            {
                ParseErrors.Add("Problem parsing instruction: " + Line);
                return new byte[8];
            }
        }
    }

    public class GenericInstruction
    {
        Byte ID { get; set; }

        public GenericInstruction(Byte _ID)
        {
            ID = _ID;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericU16Instruction
    {
        Byte ID { get; set; }
        UInt16 U16 { get; set; }

        public GenericU16Instruction(Byte _ID, UInt16 _Data)
        {
            ID = _ID;
            U16 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes(U16));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class IfInstruction
    {
        Byte ID = (byte)InstructionIDs.IF;
        Byte FlagType { get; set; }
        UInt16 FlagID { get; set; }
        UInt16 Offs_True { get; set; }
        UInt16 Offs_False { get; set; }

        public IfInstruction(string Type, UInt16 ID, UInt16 True, UInt16 False)
        {
            switch (Type)
            {
                case "inf_table": FlagType = 0; break;
                case "event_chk_inf": FlagType = 1; break;
                case "switch": FlagType = 2; break;
                case "uscene": FlagType = 3; break;
                case "treasure": FlagType = 4; break;
                case "roomclear": FlagType = 5; break;
                case "scenecollect": FlagType = 6; break;
                case "temp": FlagType = 7; break;
                default: FlagType = 0; break;
            }

            FlagID = ID;
            Offs_True = True;
            Offs_False = False;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(FlagType);
            Data.AddRange(Program.BEConverter.GetBytes(FlagID));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_True));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_False));

            return Data.ToArray();
        }
    }

    public class SetFlagInstruction
    {
        Byte ID = (byte)InstructionIDs.SETFLAG;
        Byte FlagType { get; set; }
        UInt16 FlagID { get; set; }

        public SetFlagInstruction(string Type, UInt16 ID)
        {
            switch (Type)
            {
                case "inf_table": FlagType = 0; break;
                case "event_chk_inf": FlagType = 1; break;
                case "switch": FlagType = 2; break;
                case "uscene": FlagType = 3; break;
                case "treasure": FlagType = 4; break;
                case "roomclear": FlagType = 5; break;
                case "scenecollect": FlagType = 6; break;
                case "temp": FlagType = 7; break;
                default: FlagType = 0; break;
            }

            FlagID = ID;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(FlagType);
            Data.AddRange(Program.BEConverter.GetBytes(FlagID));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class SetResponseInstruction
    {
        Byte ID = (byte)InstructionIDs.SETRESPONSE;
        UInt16 Offs_1 { get; set; }
        UInt16 Offs_2 { get; set; }
        UInt16 Offs_3 { get; set; }

        public SetResponseInstruction(UInt16 Option1Offs, UInt16 Option2Offs, UInt16 Option3Offs)
        {
            Offs_1 = Option1Offs;
            Offs_2 = Option2Offs;
            Offs_3 = Option3Offs;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes(Offs_1));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_2));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_3));

            return Data.ToArray();
        }
    }

    public class SetAnimInstruction
    {
        Byte ID = (byte)InstructionIDs.SETANIM;
        Byte Loops { get; set; }
        UInt16 U16 { get; set; }

        public SetAnimInstruction(Byte _Loops, UInt16 _Data)
        {
            Loops = _Loops;
            U16 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(Loops);
            Data.AddRange(Program.BEConverter.GetBytes(U16));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class WrongParamCountException : Exception
    {
        public WrongParamCountException(string Line) : base(String.Format("Incorrect number of parameters in line: \"{0}\"", Line))
        {
        }
    }

    public class ParamOutOfRangeException : Exception
    {
        public ParamOutOfRangeException(string Line) : base(String.Format("One of the parameters is out of range: \"{0}\"", Line))
        {
        }
    }

    public class LabelOutOfRangeException : Exception
    {
        public LabelOutOfRangeException(string Line) : base(String.Format("Script is too long as one of the labels falls out of range: \"{0}\"", Line))
        {
        }
    }

    public class LabelNotFoundException : Exception
    {
        public LabelNotFoundException(string Line) : base(String.Format("Could not find some of the labels in line: \"{0}\"", Line))
        {
        }
    }
}
