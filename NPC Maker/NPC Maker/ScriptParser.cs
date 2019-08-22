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
        WAIT = 10,
        TURNTOPLAYER = 11,
        SETMOVEMENT = 12,
        PLAYSND = 13,
        RUPEESOP = 14,
        WAITFORRESPONSE = 15,
        STOP = 255
    }

    public enum FlagTables
    {
        inf_table = 0,
        event_chk_inf = 1,
        switch_table = 2,
        uscene = 3,
        treasure = 4,
        room_clear = 5,
        scene_collect = 6,
        temporary = 7
    }

    public enum Flags
    {
        age = 10,
        day = 11,
        talking = 12,
        has_empty_bottle = 13
    }

    public enum IfValues
    {
        rupees = 30,
        time = 31,
        scene_id = 32,
        worn_mask = 33,
        skulltulas = 34
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
            Script = Script.Replace("\t", " ");                                                                 // Remove tabs

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

        private static Int32 Helper_ConvertToInt32(string Number)
        {
            Int32 Result = 0;

            if (Number.Length >= 3 && Number.Substring(0, 2) == "0x")
                Result = Convert.ToInt32(Number, 16);
            else
                Result = Convert.ToInt32(Number);

            return Result;
        }

        private static byte[] GetInstructionBytes(string Line, ref List<string> ParseErrors)
        {
            string[] Instr = Line.Trim().Split(' ');

            try
            {
                switch (Instr[0].ToLower())
                {
                    case "if":
                        {
                            if (Instr.Length < 2)
                                throw new WrongParamCountException(Line);

                            switch (Instr[1])
                            {
                                case "flag_table":
                                    {
                                        if (Instr.Length != 9)
                                            throw new WrongParamCountException(Line);

                                        UInt32 FlagID = Helper_ConvertToUInt32(Instr[3]);

                                        if (FlagID > UInt16.MaxValue || FlagID < 0)
                                            throw new ParamOutOfRangeException(Line);

                                        int Label_True = GetLabelOffset(Line, Instr[6]);
                                        int Label_False = GetLabelOffset(Line, Instr[8]);

                                        if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                            throw new LabelOutOfRangeException(Line);

                                        if (Instr[5].ToLower() != "then" || Instr[7].ToLower() != "else")
                                            throw new Exception();

                                        if (Instr[4].ToLower() != "true" && Instr[4].ToLower() != "false")
                                            throw new Exception();

                       
                                        IfInstruction If = new IfInstruction(Convert.ToByte(System.Enum.Parse(typeof(FlagTables), Instr[2].ToLower())),
                                                                             (byte)(Instr[4].ToLower() == "true" ? 1 : 0),
                                                                             Convert.ToUInt16(FlagID), 
                                                                             Convert.ToUInt16(Label_True), 
                                                                             Convert.ToUInt16(Label_False));
                                        return If.GetByteData();
                                    }
                                case "flag":
                                    {
                                        if (Instr.Length != 8)
                                            throw new WrongParamCountException(Line);

                                        int Label_True = GetLabelOffset(Line, Instr[5]);
                                        int Label_False = GetLabelOffset(Line, Instr[7]);

                                        if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                            throw new LabelOutOfRangeException(Line);

                                        if (Instr[4].ToLower() != "then" || Instr[6].ToLower() != "else")
                                            throw new Exception();

                                        if (Instr[3].ToLower() != "true" && Instr[3].ToLower() != "false")
                                            throw new Exception();

                                        IfInstruction If = new IfInstruction(Convert.ToByte(System.Enum.Parse(typeof(Flags), Instr[2].ToLower())),
                                                                             (byte)(Instr[3].ToLower() == "true" ? 1 : 0),
                                                                             0,
                                                                             Convert.ToUInt16(Label_True),
                                                                             Convert.ToUInt16(Label_False));
                                        return If.GetByteData();
                                    }
                                default:
                                    {
                                        if (Instr.Length != 8)
                                            throw new WrongParamCountException(Line);

                                        UInt32 Value = Helper_ConvertToUInt32(Instr[3]);

                                        if (Value > UInt16.MaxValue || Value > UInt16.MaxValue)
                                            throw new ParamOutOfRangeException(Line);

                                        int Label_True = GetLabelOffset(Line, Instr[5]);
                                        int Label_False = GetLabelOffset(Line, Instr[7]);

                                        if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                            throw new LabelOutOfRangeException(Line);

                                        if (Instr[4].ToLower() != "then" || Instr[6].ToLower() != "else")
                                            throw new Exception();

                                        byte Condition = 0;

                                        switch (Instr[2])
                                        {
                                            case "==" : Condition = 0; break;
                                            case "<" : Condition = 1; break;
                                            case ">": Condition = 2; break;
                                            default: throw new Exception();
                                        }

                                        IfInstruction If = new IfInstruction(Convert.ToByte(System.Enum.Parse(typeof(IfValues), Instr[1].ToLower())),
                                                                             Condition,
                                                                             Convert.ToUInt16(Value),
                                                                             Convert.ToUInt16(Label_True),
                                                                             Convert.ToUInt16(Label_False));
                                        return If.GetByteData();
                                    }
                            }
                        }
                    case "enable_textbox":
                        {
                            if (Instr.Length != 3)
                                throw new WrongParamCountException(Line);

                            UInt32 TextID_Adult = Helper_ConvertToUInt32(Instr[1]);
                            UInt32 TextID_Child = Helper_ConvertToUInt32(Instr[2]);

                            if (TextID_Adult > UInt16.MaxValue || TextID_Adult < 0)
                                throw new ParamOutOfRangeException(Line);

                            if (TextID_Child > UInt16.MaxValue || TextID_Child < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericDoubleU16Instruction EnableTextbox = new GenericDoubleU16Instruction((byte)InstructionIDs.ENABLETEXTBOX, Convert.ToUInt16(TextID_Adult), Convert.ToUInt16(TextID_Child));
                            return EnableTextbox.GetByteData();
                        }
                    case "show_textbox":
                        {
                            if (Instr.Length != 3)
                                throw new WrongParamCountException(Line);

                            UInt32 TextID_Adult = Helper_ConvertToUInt32(Instr[1]);
                            UInt32 TextID_Child = Helper_ConvertToUInt32(Instr[2]);

                            if (TextID_Adult > UInt16.MaxValue || TextID_Adult < 0)
                                throw new ParamOutOfRangeException(Line);

                            if (TextID_Child > UInt16.MaxValue || TextID_Child < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericDoubleU16Instruction ShowTextbox = new GenericDoubleU16Instruction((byte)InstructionIDs.SHOWTEXTBOX, Convert.ToUInt16(TextID_Adult), Convert.ToUInt16(TextID_Child));
                            return ShowTextbox.GetByteData();
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
                    case "wait_for_text_end":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Wait = new GenericInstruction((byte)InstructionIDs.WAITFORTEXTEND);
                            return Wait.GetByteData();
                        }
                    case "wait_for_response":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction WaitResp = new GenericInstruction((byte)InstructionIDs.WAITFORRESPONSE);
                            return WaitResp.GetByteData();
                        }
                    case "turn_towards_player":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Turn = new GenericInstruction((byte)InstructionIDs.TURNTOPLAYER);
                            return Turn.GetByteData();
                        }
                    case "return":
                    case "stop":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Stop = new GenericInstruction((byte)InstructionIDs.STOP);
                            return Stop.GetByteData();
                        }
                    case "set_movement":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            if (Instr[1].ToLower() != "true" && Instr[1].ToLower() != "false")
                                throw new Exception();

                            GenericU16Instruction Stop = new GenericU16Instruction((byte)InstructionIDs.SETMOVEMENT, (UInt16)(Instr[1].ToLower() == "true" ? 1 : 0));
                            return Stop.GetByteData();
                        }
                    case "wait":
                        {
                            if (Instr.Length > 2)
                                throw new WrongParamCountException(Line);

                            UInt32 Frames = 0;

                            if (Instr.Length == 2)
                            {
                                Frames = Helper_ConvertToUInt32(Instr[1]);

                                if (Frames > UInt16.MaxValue)
                                    throw new ParamOutOfRangeException(Line);

                                if (Frames == 0)
                                    throw new ParamOutOfRangeException(Line);
                            }

                            GenericU16Instruction Wait = new GenericU16Instruction((byte)InstructionIDs.WAIT, Convert.ToUInt16(Frames));
                            return Wait.GetByteData();
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
                    case "change_rupees":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            Int32 RupeeCount = Helper_ConvertToInt32(Instr[1]);

                            if (RupeeCount > Int16.MaxValue || RupeeCount < Int16.MinValue)
                                throw new ParamOutOfRangeException(Line);

                            GenericS16Instruction RupeeSub = new GenericS16Instruction((byte)InstructionIDs.RUPEESOP, Convert.ToInt16(RupeeCount));
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
                ParseErrors.Add("Problem parsing. Line: \"" + Line.Trim() + "\"");
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
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericS16Instruction
    {
        Byte ID { get; set; }
        Int16 Int16 { get; set; }

        public GenericS16Instruction(Byte _ID, Int16 _Data)
        {
            ID = _ID;
            Int16 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes(Int16));
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericDoubleU16Instruction
    {
        Byte ID { get; set; }
        UInt16 U16_1 { get; set; }
        UInt16 U16_2 { get; set; }

        public GenericDoubleU16Instruction(Byte _ID, UInt16 _Data1, UInt16 _Data2)
        {
            ID = _ID;
            U16_1 = _Data1;
            U16_2 = _Data2;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes(U16_1));
            Data.AddRange(Program.BEConverter.GetBytes(U16_2));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class IfInstruction
    {
        Byte ID = (byte)InstructionIDs.IF;
        Byte Check { get; set; }
        UInt16 Value { get; set; }
        UInt16 Offs_True { get; set; }
        UInt16 Offs_False { get; set; }

        public IfInstruction(Byte Checked, Byte Condition, UInt16 Val, UInt16 True, UInt16 False)
        {
            Check = 0;
            Check |= (byte)(Checked << 2);
            Check |= Condition;
            Value = Val;
            Offs_True = True;
            Offs_False = False;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(Check);
            Data.AddRange(Program.BEConverter.GetBytes(Value));
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
            FlagType = Convert.ToByte(System.Enum.Parse(typeof(FlagTables), Type.ToLower()));
            FlagID = ID;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(FlagType);
            Data.AddRange(Program.BEConverter.GetBytes(FlagID));
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

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
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class WrongParamCountException : Exception
    {
        public WrongParamCountException(string Line) : base(String.Format("Incorrect number of parameters. Line: \"{0}\"", Line.Trim()))
        {
        }
    }

    public class ParamOutOfRangeException : Exception
    {
        public ParamOutOfRangeException(string Line) : base(String.Format("One of the parameters is out of range. Line: \"{0}\"", Line.Trim()))
        {
        }
    }

    public class LabelOutOfRangeException : Exception
    {
        public LabelOutOfRangeException(string Line) : base(String.Format("Script is too long as one of the labels falls out of range. Line: \"{0}\"", Line.Trim()))
        {
        }
    }

    public class LabelNotFoundException : Exception
    {
        public LabelNotFoundException(string Line) : base(String.Format("Could not find some of the labels. Line: \"{0}\"", Line.Trim()))
        {
        }
    }
}
