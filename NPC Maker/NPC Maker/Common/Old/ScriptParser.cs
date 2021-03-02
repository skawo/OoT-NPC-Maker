using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace NPC_Maker
{

    public class ScriptParser
    {
        public List<string> ParseErrors = new List<string>();
        private static Dictionary<string, int> Labels = new Dictionary<string, int>();

        public ScriptParser()
        {
        }

        public byte[] Parse(NPCEntry Entry, string Script)
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

            int LineCount = 0;

            foreach (string Line in Lines)                                                                      // Convert every instruction into an 8 byte array and add it to the output
            {
                byte[] ParsedBytes = GetInstructionBytes(Line, Entry, LineCount, ref ParseErrors);

                if (ParsedBytes.Length != 8)
                {
                    System.Windows.Forms.MessageBox.Show($"Fatal error. Instruction not 8 bytes: {Line}");
                    return Parsed.ToArray();
                }

                Parsed.AddRange(ParsedBytes);

                LineCount++;
            }

            // Add a return at the very end if one isn't there.
            if (Parsed.Count != 0)
            {
                byte[] ParsedBytes = GetInstructionBytes("return", Entry, LineCount, ref ParseErrors);

                if (Parsed[Parsed.Count - 8] != ParsedBytes[0])
                    Parsed.AddRange(ParsedBytes);
            }

            return Parsed.ToArray();
        }

        public static Dictionary<string, int> GetLabels(List<string> Lines, ref List<string> ParseErrors)
        {
            Dictionary<string, int> Labels = new Dictionary<string, int>();

            for (int i = 0; i < Lines.Count(); i++)
            {
                if (Lines[i].EndsWith(":"))
                {
                    if (Labels.ContainsKey(Lines[i]))
                        ParseErrors.Add($"Label \"{Lines[i].Substring(0, Lines[i].Length - 1)}\" is defined more than once.");
                    else if (Lines[i].ToUpper() == "NEXT:")
                        ParseErrors.Add("A label cannot be named 'next'.");
                    else if (Lines[i].ToUpper() == "RETURN:")
                        ParseErrors.Add("A label cannot be named 'return'.");
                    else
                        Labels.Add(Lines[i], i - Labels.Count);                                                 // Decrementing the index by label count, since we'll be removing them
                }
            }

            return Labels;
        }

        private static int GetLabelOffset(string Line, int LineNo, string Label)
        {
            if (Label.ToUpper() == "RETURN")
                return 65535;

            else
            { 
                int Return;

                if (Label.ToUpper() == "NEXT")
                    Return = LineNo + 1;
                else if (!Labels.ContainsKey(Label + ":"))
                    throw new LabelNotFoundException(Line);
                else
                    Return = Labels[Label + ":"];

                if (Return == 65535)
                    throw new LabelOutOfRangeException(Line);

                return Return;
            }
        }

        private static UInt32 Helper_ConvertToUInt32(string Number)
        {
            UInt32 Result;

            if (NewScriptParser.ScriptHelpers.IsHex(Number))
                Result = Convert.ToUInt32(Number, 16);
            else
                Result = Convert.ToUInt32(Number);

            return Result;
        }

        private static Int32 Helper_ConvertToInt32(string Number)
        {
            Int32 Result;

            if (NewScriptParser.ScriptHelpers.IsHex(Number))
                Result = Convert.ToInt32(Number, 16);
            else
                Result = Convert.ToInt32(Number);

            return Result;
        }

        private static Int32 Helper_GetAnimationID(string AnimName, List<AnimationEntry> Animations)
        {
            for (int i = 0; i < Animations.Count; i++)
            {
                if (AnimName.ToLower() == Animations[i].Name.Replace(" ", "").ToLower())
                    return i;
            }

            return -1;
        }

        private static Int32 Helper_GetTextureID(string TextureName, int Segment, List<List<SegmentEntry>> Textures)
        {
            for (int i = 0; i < Textures[Segment].Count; i++)
            {
                if (TextureName.ToLower() == Textures[Segment][i].Name.Replace(" ", "").ToLower())
                    return i;
            }

            return -1;
        }

        private static Int32 Helper_GetDListID(string DlistName, List<DListEntry> DLists)
        {
            for (int i = 0; i < DLists.Count; i++)
            {
                if (DlistName.ToLower() == DLists[i].Name.Replace(" ", "").ToLower())
                    return i;
            }

            return -1;
        }

        private static Int32 Helper_GetTradeItemId(string Name)
        {
            try
            {
                return (int)System.Enum.Parse(typeof(OldLists.TradeItems), Name.ToUpper());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static Int32 Helper_GetGiveItemId(string Name)
        {
            try
            {
                return (int)System.Enum.Parse(typeof(OldLists.GiveItems), Name.ToUpper());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static Int32 Helper_GetSFXId(string SFXName)
        {
            try
            {
                return (int)OldLists.SFXes[SFXName.ToUpper()];
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static Int32 Helper_GetMusicId(string MusicName)
        {
            try
            {
                return (int)OldLists.Music[MusicName.ToUpper()];
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static byte[] GetInstructionBytes(string Line, NPCEntry Entry, int LineNo, ref List<string> ParseErrors)
        {
            string[] Instr = Line.Trim().Split(' ');

            try
            {
                int FunctionType = (int)System.Enum.Parse(typeof(OldLists.InstructionIDs), Instr[0].ToUpper());

                switch (FunctionType)
                {
                    case (int)OldLists.InstructionIDs.RNG:
                        {
                            if (Instr.Length != 8)
                                throw new WrongParamCountException(Line);

                            if (Instr[4].ToLower() != "then" || Instr[6].ToLower() != "else")
                                throw new Exception();

                            int Label_True = GetLabelOffset(Line, LineNo, Instr[5]);
                            int Label_False = GetLabelOffset(Line, LineNo, Instr[7]);

                            #region Exceptions
                            if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                throw new LabelOutOfRangeException(Line);
                            #endregion

                            int Range_En = Convert.ToInt32(Instr[1]);
                            int Value = Convert.ToInt32(Instr[3]);

                            #region Exceptions
                            if (Range_En > byte.MaxValue || Range_En < 0 || Value > byte.MaxValue || Value < 0)
                                throw new ParamOutOfRangeException(Line);
                            #endregion

                            byte Condition = 0;

                            switch (Instr[2])
                            {
                                case "==": Condition = 0; break;
                                case "<": Condition = 1; break;
                                case ">": Condition = 2; break;
                                default: throw new Exception();
                            }

                            RNGInstruction RNG = new RNGInstruction(Condition,
                                                                    Convert.ToByte(Range_En),
                                                                    Convert.ToByte(Value),
                                                                    Convert.ToUInt16(Label_True),
                                                                    Convert.ToUInt16(Label_False));

                            return RNG.GetByteData();
                        }

                    case (int)OldLists.InstructionIDs.IF:
                        {
                            if (Instr.Length < 2)
                                throw new WrongParamCountException(Line);

                            int IfSubType = (int)System.Enum.Parse(typeof(OldLists.IfSubTypes), Instr[1].ToLower());


                            if (IfSubType < 10)
                            {
                                if (Instr.Length != 8)
                                    throw new WrongParamCountException(Line);

                                UInt32 FlagID = Helper_ConvertToUInt32(Instr[2]);
                                int Label_True = GetLabelOffset(Line, LineNo, Instr[5]);
                                int Label_False = GetLabelOffset(Line, LineNo, Instr[7]);

                                #region Exceptions

                                if (FlagID > UInt16.MaxValue || FlagID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                if (Instr[4].ToLower() != "then" || Instr[6].ToLower() != "else")
                                    throw new Exception();

                                if (Instr[3].ToLower() != "true" && Instr[3].ToLower() != "false")
                                    throw new Exception();

                                #endregion

                                IfInstruction If = new IfInstruction(Convert.ToByte(IfSubType),
                                                                     (byte)(Instr[3].ToLower() == "true" ? 1 : 0),
                                                                     Convert.ToUInt16(FlagID),
                                                                     Convert.ToUInt16(Label_True),
                                                                     Convert.ToUInt16(Label_False));
                                return If.GetByteData();
                            }
                            else if (IfSubType < 30)
                            {
                                if (Instr.Length != 7)
                                    throw new WrongParamCountException(Line);

                                int Label_True = GetLabelOffset(Line, LineNo, Instr[4]);
                                int Label_False = GetLabelOffset(Line, LineNo, Instr[6]);

                                #region Exceptions

                                if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                if (Instr[3].ToLower() != "then" || Instr[5].ToLower() != "else")
                                    throw new Exception();

                                if (Instr[2].ToLower() != "true" && Instr[2].ToLower() != "false")
                                    throw new Exception();

                                #endregion

                                IfInstruction If = new IfInstruction(Convert.ToByte(IfSubType),
                                                                     (byte)(Instr[2].ToLower() == "true" ? 1 : 0),
                                                                     0,
                                                                     Convert.ToUInt16(Label_True),
                                                                     Convert.ToUInt16(Label_False));
                                return If.GetByteData();
                            }
                            else if (IfSubType < 61)
                            {
                                if (Instr.Length != 8)
                                    throw new WrongParamCountException(Line);

                                UInt32 Value = Helper_ConvertToUInt32(Instr[3]);

                                if (Value > UInt16.MaxValue || Value > UInt16.MaxValue)
                                    throw new ParamOutOfRangeException(Line);

                                int Label_True = GetLabelOffset(Line, LineNo, Instr[5]);
                                int Label_False = GetLabelOffset(Line, LineNo, Instr[7]);

                                if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                if (Instr[4].ToLower() != "then" || Instr[6].ToLower() != "else")
                                    throw new Exception();

                                byte Condition = 0;

                                switch (Instr[2])
                                {
                                    case "==": Condition = 0; break;
                                    case "<": Condition = 1; break;
                                    case ">": Condition = 2; break;
                                    default: throw new Exception();
                                }

                                IfInstruction If = new IfInstruction(Convert.ToByte(IfSubType),
                                                                     Condition,
                                                                     Convert.ToUInt16(Value),
                                                                     Convert.ToUInt16(Label_True),
                                                                     Convert.ToUInt16(Label_False));
                                return If.GetByteData();
                            }
                            else if (IfSubType == (int)OldLists.IfSubTypes.item_being_traded)
                            {
                                if (Instr.Length != 8)
                                    throw new WrongParamCountException(Line);

                                int Item = Helper_GetTradeItemId(Instr[3]);

                                if (Item == -1)
                                    Item = (int)Helper_ConvertToUInt32(Instr[3]);

                                if (Item > UInt16.MaxValue || Item > UInt16.MaxValue)
                                    throw new ParamOutOfRangeException(Line);

                                int Label_True = GetLabelOffset(Line, LineNo, Instr[5]);
                                int Label_False = GetLabelOffset(Line, LineNo, Instr[7]);

                                if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                if (Instr[4].ToLower() != "then" || Instr[6].ToLower() != "else")
                                    throw new Exception();

                                byte Condition = 0;

                                switch (Instr[2])
                                {
                                    case "==": Condition = 0; break;
                                    default: throw new Exception();
                                }

                                IfInstruction If = new IfInstruction(Convert.ToByte(IfSubType),
                                                                     Condition,
                                                                     Convert.ToUInt16(Item),
                                                                     Convert.ToUInt16(Label_True),
                                                                     Convert.ToUInt16(Label_False));
                                return If.GetByteData();
                            }
                            else if (IfSubType == (int)OldLists.IfSubTypes.trade_status)
                            {
                                if (Instr.Length != 5)
                                    throw new WrongParamCountException(Line);


                                int Label_Successful = GetLabelOffset(Line, LineNo, Instr[2]);
                                int LabelUnsuccesful = GetLabelOffset(Line, LineNo, Instr[3]);
                                int LabelNone = GetLabelOffset(Line, LineNo, Instr[4]);

                                IfInstruction If = new IfInstruction(Convert.ToByte(IfSubType),
                                                                     0,
                                                                     Convert.ToUInt16(Label_Successful),
                                                                     Convert.ToUInt16(LabelUnsuccesful),
                                                                     Convert.ToUInt16(LabelNone));
                                return If.GetByteData();

                            }
                            else if (IfSubType == (int)OldLists.IfSubTypes.script_var)
                            {
                                if (Instr.Length != 9)
                                    throw new WrongParamCountException(Line);

                                int Label_True = GetLabelOffset(Line, LineNo, Instr[6]);
                                int Label_False = GetLabelOffset(Line, LineNo, Instr[8]);

                                if (Label_False > UInt16.MaxValue || Label_True > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                if (Instr[5].ToLower() != "then" || Instr[7].ToLower() != "else")
                                    throw new Exception();

                                int ScriptVarNum = Helper_ConvertToInt32(Instr[2]);

                                if (ScriptVarNum > 3 || ScriptVarNum < 0)
                                    throw new ParamOutOfRangeException(Line);

                                byte Condition = 0;

                                switch (Instr[3])
                                {
                                    case "==": Condition = 0; break;
                                    case "<": Condition = 1; break;
                                    case ">": Condition = 2; break;
                                    default: throw new Exception();
                                }

                                int Value = Helper_ConvertToInt32(Instr[4]);

                                if (Value > 127 || Value < -127)
                                    throw new ParamOutOfRangeException(Line);


                                IfScriptVarInstruction If = new IfScriptVarInstruction(Convert.ToByte(IfSubType),
                                                                                       Condition,
                                                                                       (byte)Value,
                                                                                       (byte)ScriptVarNum,
                                                                                       Convert.ToUInt16(Label_True),
                                                                                       Convert.ToUInt16(Label_False));
                                return If.GetByteData();
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    case (int)OldLists.InstructionIDs.SET:
                        {
                            if (Instr.Length < 2)
                                throw new WrongParamCountException(Line);

                            int SetSubType = (int)System.Enum.Parse(typeof(OldLists.SetSubTypes), Instr[1].ToLower());

                            if (SetSubType < 35)        // u16 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                UInt32 Data = Helper_ConvertToUInt32(Instr[2]);

                                if (Data > UInt16.MaxValue || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU16Instruction SetU16 = new GenericU16Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToUInt16(Data));
                                return SetU16.GetByteData();
                            }
                            else if (SetSubType < 70)   // s16 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = Helper_ConvertToInt32(Instr[2]);

                                if (Data > Int16.MaxValue || Data < Int16.MinValue)
                                    throw new ParamOutOfRangeException(Line);

                                GenericS16Instruction SetS16 = new GenericS16Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToInt16(Data));
                                return SetS16.GetByteData();
                            }
                            else if (SetSubType < 105) // u32 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                UInt32 Data = Helper_ConvertToUInt32(Instr[2]);

                                GenericU32Instruction SetU32 = new GenericU32Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToUInt32(Data));
                                return SetU32.GetByteData();
                            }
                            else if (SetSubType < 140) // s32 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = Helper_ConvertToInt32(Instr[2]);

                                if (Data > Int32.MaxValue || Data < Int32.MinValue)
                                    throw new ParamOutOfRangeException(Line);

                                GenericS32Instruction SetS32 = new GenericS32Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToInt32(Data));
                                return SetS32.GetByteData();
                            }
                            else if (SetSubType < 175) // Float subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                GenericFloatInstruction SetFloat = new GenericFloatInstruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToDecimal(Instr[2]));
                                return SetFloat.GetByteData();
                            }
                            else if (SetSubType < 210) // u8 and bool Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                UInt32 Data = 0;

                                if (Instr[2].ToLower() == "true")
                                    Data = 1;
                                else if (Instr[2].ToLower() == "false")
                                    Data = 0;
                                else
                                {
                                    Data = Helper_ConvertToUInt32(Instr[2]);

                                    if (Data > byte.MaxValue || Data < 0)
                                        throw new ParamOutOfRangeException(Line);
                                }

                                GenericU8Instruction SetU8 = new GenericU8Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Data));
                                return SetU8.GetByteData();
                            }
                            else if (SetSubType < 230) // s8 Subtype
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = 0;

                                Data = Helper_ConvertToInt32(Instr[2]);

                                if (Data > sbyte.MaxValue || Data < sbyte.MinValue)
                                    throw new ParamOutOfRangeException(Line);

                                GenericS8Instruction SetS8 = new GenericS8Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToSByte(Data));
                                return SetS8.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.responses)
                            {
                                if (Instr.Length < 3 || Instr.Length > 5)
                                    throw new WrongParamCountException(Line);

                                int Label_1 = GetLabelOffset(Line, LineNo, Instr[2]);
                                int Label_2 = Instr.Length > 3 ? GetLabelOffset(Line, LineNo, Instr[3]) : GetLabelOffset(Line, LineNo, Instr[2]);
                                int Label_3 = Instr.Length > 4 ? GetLabelOffset(Line, LineNo, Instr[4]) :
                                                                 Instr.Length > 3 ? GetLabelOffset(Line, LineNo, Instr[3]) : GetLabelOffset(Line, LineNo, Instr[2]);

                                if (Label_1 > UInt16.MaxValue || Label_2 > UInt16.MaxValue || Label_3 > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                SetResponseInstruction Respond = new SetResponseInstruction(Convert.ToUInt16(Label_1), Convert.ToUInt16(Label_2), Convert.ToUInt16(Label_3));
                                return Respond.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.flag)
                            {
                                if (Instr.Length != 5)
                                    throw new WrongParamCountException(Line);

                                UInt32 FlagID = Helper_ConvertToUInt32(Instr[3]);

                                if (FlagID > UInt16.MaxValue || FlagID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                if (Instr[4].ToLower() != "true" && Instr[4].ToLower() != "false")
                                    throw new Exception(Line);

                                SetFlagInstruction SetFlag = new SetFlagInstruction(Instr[2], Convert.ToUInt16(FlagID), Instr[4].ToLower() == "true");
                                return SetFlag.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.movement_type)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                int Data = (int)System.Enum.Parse(typeof(OldLists.MovementStyles), Instr[2].ToLower());

                                if (Data > 4 || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU8Instruction SetMovT = new GenericU8Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Data));
                                return SetMovT.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.look_type)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                int Data = (int)System.Enum.Parse(typeof(OldLists.LookTypes), Instr[2].ToLower());

                                if (Data > 2 || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU8Instruction SetLookT = new GenericU8Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Data));
                                return SetLookT.GetByteData();
                            }
                            else if ((SetSubType == (int)OldLists.SetSubTypes.head_axis) || (SetSubType == (int)OldLists.SetSubTypes.waist_axis))
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                bool Horiz = false;

                                switch (Instr[2].ToLower())
                                {
                                    case "horizontal": Horiz = true; break;
                                    case "vertical": Horiz = false; break;
                                    default: throw new ParamOutOfRangeException(Line);
                                }

                                int Axis = (int)Enum.Parse(typeof(OldLists.Axis), Instr[3], true);

                                GenericU8Instruction SetHeadA = new GenericU8Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Axis), Convert.ToByte(Horiz));
                                return SetHeadA.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.animation || SetSubType == (int)OldLists.SetSubTypes.animation_instantly)
                            {
                                if (Instr.Length < 3 && Instr.Length > 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Entry.Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Entry.Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                UInt32 Loops = 0;

                                if (Instr.Length == 4)
                                {
                                    Loops = Helper_ConvertToUInt32(Instr[3]);

                                    if (Loops > Byte.MaxValue || Loops < 0)
                                        throw new ParamOutOfRangeException(Line);
                                }

                                SetAnimInstruction SetAnim = new SetAnimInstruction((byte)SetSubType, Convert.ToByte(Loops), Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.animation_object)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Entry.Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Entry.Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                Int32 Object = Helper_ConvertToInt32(Instr[3]);

                                if (Object > UInt16.MaxValue || Object < 0)
                                    throw new ParamOutOfRangeException(Line);

                                SetAnimObjectInstruction SetAnim = new SetAnimObjectInstruction(Convert.ToUInt16(Object), Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.animation_offset)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Entry.Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Entry.Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                UInt32 Offset = Helper_ConvertToUInt32(Instr[3]);

                                SetAnimOffsetInstruction SetAnim = new SetAnimOffsetInstruction(Offset, Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.animation_speed)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Entry.Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Entry.Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                SetAnimSpeedInstruction SetAnim = new SetAnimSpeedInstruction(Convert.ToDecimal(Instr[3]), Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.animation_keyframes)
                            {
                                if (Instr.Length < 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Entry.Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Entry.Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                int[] Frames = new int[4] { 255, 255, 255, 255 };

                                for (int i = 3; i < Instr.Length; i++)
                                {
                                    Frames[i - 3] = Helper_ConvertToInt32(Instr[i]);

                                    if (Frames[i - 3] < 0)
                                        Frames[i - 3] = 255;

                                    if (Frames[i - 3] > 255)
                                        throw new ParamOutOfRangeException(Line);
                                }

                                SetAnimKeyFramesInstruction SetAnim = new SetAnimKeyFramesInstruction(Convert.ToUInt16(AnimID), Convert.ToByte(Frames[0]), Convert.ToByte(Frames[1]), Convert.ToByte(Frames[2]), Convert.ToByte(Frames[3]));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.cutscene_slot)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                int CtsSlot = Helper_ConvertToInt32(Instr[2]);

                                if (CtsSlot > 10 || CtsSlot < -1)
                                    throw new LabelOutOfRangeException(Line);

                                GenericS8Instruction SetScriptStart = new GenericS8Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToSByte(CtsSlot));
                                return SetScriptStart.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.script_start)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                int GotoLabel = GetLabelOffset(Line, LineNo, Instr[2]);

                                if (GotoLabel > UInt16.MaxValue || GotoLabel > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                GenericU16Instruction SetScriptStart = new GenericU16Instruction((byte)OldLists.InstructionIDs.SET, (byte)SetSubType, Convert.ToUInt16(GotoLabel));
                                return SetScriptStart.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.segment_tex)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                int SegmentID = (int)System.Enum.Parse(typeof(OldLists.Segments), Instr[2].ToUpper());
                                Int32 TexID = Helper_GetTextureID(Instr[3], SegmentID, Entry.Segments);

                                if (TexID == -1)
                                    TexID = Helper_ConvertToInt32(Instr[3]);

                                if (TexID > 31 || TexID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                SetSegmentTextureIDInstruction SetSegmentTex = new SetSegmentTextureIDInstruction(Convert.ToByte(SegmentID), Convert.ToUInt16(TexID));
                                return SetSegmentTex.GetByteData();
                            }
                            else if ((SetSubType == (int)OldLists.SetSubTypes.blink_pattern) || (SetSubType == (int)OldLists.SetSubTypes.talk_pattern))
                            {
                                if (Instr.Length < 3)
                                    throw new WrongParamCountException(Line);

                                byte[] Data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                                for (int i = 2; i < Instr.Length; i++)
                                {
                                    Int32 TexID = Helper_GetTextureID(Instr[i], SetSubType == (int)OldLists.SetSubTypes.blink_pattern ? Entry.BlinkSegment - 8 : Entry.TalkSegment - 8, Entry.Segments);

                                    if (TexID == -1)
                                        TexID = Helper_ConvertToInt32(Instr[i]);

                                    if (TexID > 31 || TexID < 0)
                                        throw new ParamOutOfRangeException(Line);

                                    Data[i - 2] = (byte)TexID;
                                }

                                SetPatternInstruction SetBlink = new SetPatternInstruction((Byte)SetSubType, Data);
                                return SetBlink.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.env_color)
                            {
                                if (Instr.Length != 6)
                                    throw new WrongParamCountException(Line);

                                int R = Helper_ConvertToInt32(Instr[2]);
                                int G = Helper_ConvertToInt32(Instr[3]);
                                int B = Helper_ConvertToInt32(Instr[4]);

                                if (R > 255 || R < 0)
                                    throw new ParamOutOfRangeException(Line);
                                if (G > 255 || G < 0)
                                    throw new ParamOutOfRangeException(Line);
                                if (B > 255 || B < 0)
                                    throw new ParamOutOfRangeException(Line);

                                string Use = Instr[5].ToLower();

                                if (Use != "true" && Use != "false")
                                    throw new Exception();

                                SetRGBAInstruction SetRGBA = new SetRGBAInstruction((byte)R, (byte)G, (byte)B, Use == "true" ? (byte)255 : (byte)0);
                                return SetRGBA.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.dlist_show)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Int32 DListID = Helper_GetDListID(Instr[2], Entry.DLists);

                                if (DListID == -1)
                                    DListID = Helper_ConvertToInt32(Instr[2]);

                                if (DListID > (Entry.DLists.Count() - 1) || DListID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                int VisibleType = (int)Enum.Parse(typeof(OldLists.DListVisibilityTypes), Instr[3].ToLower());

                                SetDListVisibilityInstruction SetDListV = new SetDListVisibilityInstruction(Convert.ToByte(VisibleType), Convert.ToUInt16(DListID));
                                return SetDListV.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.camera_tracking_on)
                            {
                                if (Instr.Length < 3)
                                    throw new WrongParamCountException(Line);

                                int TrackSubType = (int)System.Enum.Parse(typeof(OldLists.TurnTowardsSubtypes), Instr[2].ToLower());

                                switch (TrackSubType)
                                {
                                    case 0: return new GenericU16Instruction((byte)OldLists.InstructionIDs.SET, (byte)OldLists.SetSubTypes.camera_tracking_on, 0, 0, 0).GetByteData();
                                    case 1: return new GenericU16Instruction((byte)OldLists.InstructionIDs.SET, (byte)OldLists.SetSubTypes.camera_tracking_on, 1, 0, 0).GetByteData();
                                    case 2:
                                        {
                                            if (Instr.Length != 4)
                                                throw new WrongParamCountException(Line);

                                            int ActorNum = Helper_ConvertToInt32(Instr[3]);

                                            if (ActorNum > UInt16.MaxValue || ActorNum < 0)
                                                throw new ParamOutOfRangeException(Line);

                                            return new GenericU16Instruction((byte)OldLists.InstructionIDs.SET, (byte)OldLists.SetSubTypes.camera_tracking_on, 2, Convert.ToUInt16(ActorNum), 0).GetByteData();
                                        }
                                    case 3:
                                        {
                                            if (Instr.Length != 5)
                                                throw new WrongParamCountException(Line);

                                            int ActorNum = Helper_ConvertToInt32(Instr[2]);
                                            int ActorType = Helper_ConvertToInt32(Instr[3]);

                                            if (ActorNum > UInt16.MaxValue)
                                                throw new ParamOutOfRangeException(Line);

                                            if (ActorType > 12 || ActorType < 0)
                                                throw new ParamOutOfRangeException(Line);

                                            return new GenericU16Instruction((byte)OldLists.InstructionIDs.SET, (byte)OldLists.SetSubTypes.camera_tracking_on, 3, Convert.ToUInt16(ActorNum), Convert.ToUInt16(ActorType)).GetByteData();
                                        }
                                    default: throw new Exception();
                                }
                            }
                            else if (SetSubType == (int)OldLists.SetSubTypes.script_var)
                            {
                                if (Instr.Length != 5)
                                    throw new WrongParamCountException(Line);

                                int ScriptVarNum = Helper_ConvertToInt32(Instr[2]);

                                if (ScriptVarNum > 3 || ScriptVarNum < 0)
                                    throw new ParamOutOfRangeException(Line);

                                byte Type = 0;

                                switch (Instr[3])
                                {
                                    case "=": Type = 0; break;
                                    case "-": Type = 1; break;
                                    case "+": Type = 2; break;
                                    default: throw new Exception();
                                }

                                int Value = Helper_ConvertToInt32(Instr[4]);

                                if (Value > 127 || Value < -127)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU16Instruction SetScriptVar = new GenericU16Instruction((int)OldLists.InstructionIDs.SET, (byte)OldLists.SetSubTypes.script_var, Type, (UInt16)Value, (UInt16)ScriptVarNum); 
                                return SetScriptVar.GetByteData();
                            }

                            else
                            {
                                throw new Exception();
                            }
                        }
                    case (int)OldLists.InstructionIDs.WAITFOR:
                        {
                            if (Instr.Length < 2)
                                throw new WrongParamCountException(Line);

                            int WaitForSubType = (int)System.Enum.Parse(typeof(OldLists.WaitForSubTypes), Instr[1].ToLower());

                            if (WaitForSubType < 35)
                            {
                                if (Instr.Length != 2)
                                    throw new WrongParamCountException(Line);

                                GenericInstruction WaitResp = new GenericInstruction((byte)OldLists.InstructionIDs.WAITFOR, (byte)WaitForSubType);
                                return WaitResp.GetByteData();
                            }
                            else if (WaitForSubType < 70)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                UInt32 Data = Helper_ConvertToUInt32(Instr[2]);

                                if (Data > UInt16.MaxValue || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU16Instruction WaitForU16 = new GenericU16Instruction((byte)OldLists.InstructionIDs.WAITFOR, (byte)WaitForSubType, Convert.ToUInt16(Data));
                                return WaitForU16.GetByteData();
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    case (int)OldLists.InstructionIDs.ENABLE_TEXTBOX:
                        {
                            if (Instr.Length != 3)
                                throw new WrongParamCountException(Line);

                            UInt32 TextID_Adult = Helper_ConvertToUInt32(Instr[1]);
                            UInt32 TextID_Child = Helper_ConvertToUInt32(Instr[2]);

                            if (TextID_Adult > UInt16.MaxValue || TextID_Adult < 0)
                                throw new ParamOutOfRangeException(Line);

                            if (TextID_Child > UInt16.MaxValue || TextID_Child < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction EnableTextbox = new GenericU16Instruction((byte)OldLists.InstructionIDs.ENABLE_TEXTBOX, 0, Convert.ToUInt16(TextID_Adult), Convert.ToUInt16(TextID_Child));
                            return EnableTextbox.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.ENABLE_TRADE:
                        {
                            if (Instr.Length != 5)
                                throw new WrongParamCountException(Line);

                            UInt32 TextID_Trade = Helper_ConvertToUInt32(Instr[1]);
                            UInt32 TextID_Incorrect = Helper_ConvertToUInt32(Instr[2]);
                            UInt32 TextID_Talking = Helper_ConvertToUInt32(Instr[3]);

                            int Item = Helper_GetTradeItemId(Instr[4]);

                            if (Item == -1)
                                Item = (int)Helper_ConvertToUInt32(Instr[4]);

                            if (TextID_Trade > UInt16.MaxValue || TextID_Trade < 0)
                                throw new ParamOutOfRangeException(Line);

                            if (TextID_Talking > UInt16.MaxValue || TextID_Talking < 0)
                                throw new ParamOutOfRangeException(Line);

                            if (TextID_Incorrect > UInt16.MaxValue || TextID_Incorrect < 0)
                                throw new ParamOutOfRangeException(Line);

                     
                            if (Item > byte.MaxValue || Item < 0)
                                throw new ParamOutOfRangeException(Line);

                            EnableTradeInstruction EnableTrade = new EnableTradeInstruction((byte)OldLists.InstructionIDs.ENABLE_TRADE, Convert.ToUInt16(TextID_Trade), Convert.ToUInt16(TextID_Incorrect), Convert.ToUInt16(TextID_Talking), Convert.ToByte(Item));
                            return EnableTrade.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.SHOW_TEXTBOX:
                        {
                            if (Instr.Length != 3)
                                throw new WrongParamCountException(Line);

                            UInt32 TextID_Adult = Helper_ConvertToUInt32(Instr[1]);
                            UInt32 TextID_Child = Helper_ConvertToUInt32(Instr[2]);

                            if (TextID_Adult > UInt16.MaxValue || TextID_Adult < 0)
                                throw new ParamOutOfRangeException(Line);

                            if (TextID_Child > UInt16.MaxValue || TextID_Child < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction ShowTextbox = new GenericU16Instruction((byte)OldLists.InstructionIDs.SHOW_TEXTBOX, 0, Convert.ToUInt16(TextID_Adult), Convert.ToUInt16(TextID_Child));
                            return ShowTextbox.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.GIVE_ITEM:
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);


                            int ItemID = Helper_GetGiveItemId(Instr[1]);

                            if (ItemID == -1)
                                ItemID = (int)Helper_ConvertToUInt32(Instr[1]);

                            if (ItemID > UInt16.MaxValue || ItemID < 0)
                                throw new ParamOutOfRangeException(Line);

                            GenericU16Instruction Give = new GenericU16Instruction((byte)OldLists.InstructionIDs.GIVE_ITEM, 0, Convert.ToUInt16(ItemID));
                            return Give.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.GOTO:
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            int GotoLabel = GetLabelOffset(Line, LineNo, Instr[1]);

                            if (GotoLabel > UInt16.MaxValue || GotoLabel > UInt16.MaxValue)
                                throw new LabelOutOfRangeException(Line);

                            GenericU16Instruction Goto = new GenericU16Instruction((byte)OldLists.InstructionIDs.GOTO, 0, Convert.ToUInt16(GotoLabel));
                            return Goto.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.TURN:
                        {
                            int Acting = (int)System.Enum.Parse(typeof(OldLists.TurnTowardsSubtypes), Instr[1].ToLower());
                            int Type = (int)System.Enum.Parse(typeof(OldLists.TurnTypeSubtypes), Instr[2].ToLower());
                            int Value = 0;
                            int Value2 = 0;
                            int ActorType = 0;

                            if (Acting > 1)
                                throw new ParamOutOfRangeException(Line);


                            if (Type == (int)OldLists.TurnTypeSubtypes.towards)
                            {
                                Value = (int)System.Enum.Parse(typeof(OldLists.TurnTowardsSubtypes), Instr[3].ToLower());

                                if (Value == Acting)
                                    throw new Exception();

                                if (Value == (int)OldLists.TurnTowardsSubtypes.actorid)
                                {
                                    if (Instr.Length != 6)
                                        throw new WrongParamCountException(Line);

                                    Value2 = Helper_ConvertToInt32(Instr[4]);
                                    ActorType = Helper_ConvertToInt32(Instr[5]);

                                    if (Value2 > UInt16.MaxValue)
                                        throw new ParamOutOfRangeException(Line);

                                    if (ActorType > 12 || ActorType < 0)
                                        throw new ParamOutOfRangeException(Line);
                                }
                                else if (Value == (int)OldLists.TurnTowardsSubtypes.configid)
                                {
                                    if (Instr.Length != 5)
                                        throw new WrongParamCountException(Line);

                                    Value2 = Helper_ConvertToInt32(Instr[4]);

                                    if (Value2 > UInt16.MaxValue)
                                        throw new ParamOutOfRangeException(Line);
                                }
                                else
                                {
                                    if (Instr.Length != 4)
                                        throw new WrongParamCountException(Line);
                                }
                            }
                            else
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Value = Helper_ConvertToInt32(Instr[3].ToLower());

                                if (Value > UInt16.MaxValue || Value > UInt16.MaxValue)
                                    throw new ParamOutOfRangeException(Line);
                            }

                            TurnInstruction Turn = new TurnInstruction((byte)OldLists.InstructionIDs.TURN, (Byte)Acting, (Byte)Type, (UInt16)Value, (UInt16)Value2, (Byte)ActorType);
                            return Turn.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.RETURN:
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Stop = new GenericInstruction((byte)OldLists.InstructionIDs.RETURN);
                            return Stop.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.PLAY:
                        {
                            int SetSubType = (int)System.Enum.Parse(typeof(OldLists.PlaySubtypes), Instr[1].ToLower());

                            if (SetSubType == (int)OldLists.PlaySubtypes.sfx)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                int SNDID = Helper_GetSFXId(Instr[2]);

                                if (SNDID == -1)
                                    SNDID = Helper_ConvertToInt32(Instr[2]);

                                if (SNDID > UInt16.MaxValue || SNDID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU16Instruction Sound = new GenericU16Instruction((byte)OldLists.InstructionIDs.PLAY, Convert.ToByte(SetSubType), Convert.ToUInt16(SNDID));
                                return Sound.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.PlaySubtypes.music)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                int SNDID = Helper_GetMusicId(Instr[2]);

                                if (SNDID == -1)
                                    SNDID = Helper_ConvertToInt32(Instr[2]);

                                if (SNDID > byte.MaxValue || SNDID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU8Instruction Sound = new GenericU8Instruction((byte)OldLists.InstructionIDs.PLAY, Convert.ToByte(SetSubType), Convert.ToByte(SNDID));
                                return Sound.GetByteData();
                            }
                            else if (SetSubType == (int)OldLists.PlaySubtypes.cutscene)
                            {
                                if (Instr.Length < 2 || Instr.Length > 3)
                                    throw new WrongParamCountException(Line);

                                int CutsceneOffs = -1;

                                if (Instr.Length == 3)
                                    CutsceneOffs = Helper_ConvertToInt32(Instr[2]);

                                if (CutsceneOffs > Int32.MaxValue || CutsceneOffs < -1)
                                    throw new ParamOutOfRangeException(Line);

                                GenericS32Instruction Sound = new GenericS32Instruction((byte)OldLists.InstructionIDs.PLAY, Convert.ToByte(SetSubType), Convert.ToInt32(CutsceneOffs));
                                return Sound.GetByteData();
                            }
                            else
                                throw new Exception();
                        }
                    case (int)OldLists.InstructionIDs.KILL:
                        {
                            Int32 ActorNum = 0;
                            Int32 ActorType = 0;

                            int SetSubType = (int)System.Enum.Parse(typeof(OldLists.KillSubtypes), Instr[1].ToLower());

                            if (SetSubType == (int)OldLists.KillSubtypes.configid)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                ActorNum = Helper_ConvertToInt32(Instr[2]);

                                if (ActorNum > UInt16.MaxValue || ActorNum < 0)
                                    throw new ParamOutOfRangeException(Line);
                            }
                            else if (SetSubType == (int)OldLists.KillSubtypes.actorid)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                ActorNum = Helper_ConvertToInt32(Instr[2]);
                                ActorType = Helper_ConvertToInt32(Instr[3]);

                                if (ActorNum > UInt16.MaxValue)
                                    throw new ParamOutOfRangeException(Line);

                                if (ActorType > 12 || ActorType < 0)
                                    throw new ParamOutOfRangeException(Line);
                            }

                            ExternalActorDependantInstruction Kill = new ExternalActorDependantInstruction((byte)FunctionType, (byte)SetSubType, Convert.ToInt16(ActorNum), Convert.ToUInt16(ActorType), 0);
                            return Kill.GetByteData();
                        }
                    case (int)OldLists.InstructionIDs.SCRIPT_CHANGE:
                        {
                            int ActorNum = 0;
                            int Label = 0;

                            int SetSubType = (int)System.Enum.Parse(typeof(OldLists.ScriptOverwriteTypes), Instr[1].ToLower());

                            if (SetSubType == (int)OldLists.ScriptOverwriteTypes.overwrite)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                ActorNum = Helper_ConvertToInt32(Instr[2]);

                                if (ActorNum > UInt16.MaxValue || ActorNum < 0)
                                    throw new ParamOutOfRangeException(Line);

                                Label = GetLabelOffset(Line, LineNo, Instr[3]);

                                if (Label > UInt16.MaxValue || Label > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);
                            }
                            else if (SetSubType == (int)OldLists.ScriptOverwriteTypes.restore)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                ActorNum = Helper_ConvertToInt32(Instr[2]);

                                if (ActorNum > UInt16.MaxValue || ActorNum < 0)
                                    throw new ParamOutOfRangeException(Line);
                            }
                            else
                                throw new Exception();

                            ExternalActorDependantInstruction Change = new ExternalActorDependantInstruction((byte)FunctionType, (byte)SetSubType, Convert.ToInt16(ActorNum), 0, Convert.ToUInt16(Label));
                            return Change.GetByteData();
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
                ParseErrors.Add($"Problem parsing. Line: \"{Line.Trim()}\"");
                return new byte[8];
            }
        }
    }

    public class GenericInstruction
    {
        Byte ID { get; set; }
        Byte SubType { get; set; }

        public GenericInstruction(Byte _ID, Byte _SubType = 0)
        {
            ID = _ID;
            SubType = _SubType;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubType
            };
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericU8Instruction
    {
        Byte ID { get; set; }
        Byte SubID { get; set; }
        byte U8 { get; set; }
        byte U8_2 { get; set; }

        public GenericU8Instruction(Byte _ID, Byte _SubID, byte _Data, byte _Data2 = 0)
        {
            ID = _ID;
            SubID = _SubID;
            U8 = _Data;
            U8_2 = _Data2;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID,
                U8,
                U8_2
            };
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericS8Instruction
    {
        Byte ID { get; set; }
        Byte SubID { get; set; }
        sbyte S8 { get; set; }

        public GenericS8Instruction(Byte _ID, Byte _SubID, sbyte _Data)
        {
            ID = _ID;
            SubID = _SubID;
            S8 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID,
                (byte)S8,
                0
            };
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericS16Instruction
    {
        Byte ID { get; set; }
        Byte SubID { get; set; }
        Int16 S16 { get; set; }

        public GenericS16Instruction(Byte _ID, Byte _SubID, Int16 _Data)
        {
            ID = _ID;
            SubID = _SubID;
            S16 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes(S16));
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericU32Instruction
    {
        Byte ID { get; set; }
        Byte SubID { get; set; }
        UInt32 U32 { get; set; }

        public GenericU32Instruction(Byte _ID, Byte _SubID, UInt32 _Data)
        {
            ID = _ID;
            SubID = _SubID;
            U32 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes(U32));

            return Data.ToArray();
        }
    }

    public class GenericS32Instruction
    {
        Byte ID { get; set; }
        Byte SubID { get; set; }
        Int32 S32 { get; set; }

        public GenericS32Instruction(Byte _ID, Byte _SubID, Int32 _Data)
        {
            ID = _ID;
            SubID = _SubID;
            S32 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes(S32));

            return Data.ToArray();
        }
    }

    public class GenericFloatInstruction
    {
        Byte ID { get; set; }
        Byte SubID { get; set; }
        float Fl { get; set; }

        public GenericFloatInstruction(Byte _ID, Byte _SubID, decimal _Data)
        {
            ID = _ID;
            SubID = _SubID;
            Fl = (float)_Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes(Fl));

            return Data.ToArray();
        }
    }

    public class GenericU16Instruction
    {
        Byte ID { get; set; }
        Byte SubId { get; set; }
        UInt16 U16_1 { get; set; }
        UInt16 U16_2 { get; set; }
        UInt16 U16_3 { get; set; }

        public GenericU16Instruction(Byte _ID, Byte _SubId, UInt16 _Data1, UInt16 _Data2 = 0, UInt16 _Data3 = 0)
        {
            ID = _ID;
            SubId = _SubId;
            U16_1 = _Data1;
            U16_2 = _Data2;
            U16_3 = _Data3;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubId
            };
            Data.AddRange(Program.BEConverter.GetBytes(U16_1));
            Data.AddRange(Program.BEConverter.GetBytes(U16_2));
            Data.AddRange(Program.BEConverter.GetBytes(U16_3));

            return Data.ToArray();
        }
    }

    public class TurnInstruction
    {
        Byte ID { get; set; }
        Byte Acting { get; set; }
        Byte Type { get; set; }
        Byte ActorType { get; set; }
        UInt16 Value { get; set; }
        UInt16 Value2 { get; set; }

        public TurnInstruction(Byte _ID, Byte _Acting, Byte _Type, UInt16 _Value, UInt16 _Value2, Byte _ActorType)
        {
            ID = _ID;
            Acting = _Acting;
            Type = _Type;
            Value = _Value;
            Value2 = _Value2;
            ActorType = _ActorType;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                ActorType,
                Acting,
                Type
            };
            Data.AddRange(Program.BEConverter.GetBytes(Value));
            Data.AddRange(Program.BEConverter.GetBytes(Value2));

            return Data.ToArray();
        }
    }

    public class EnableTradeInstruction
    {
        Byte ID { get; set; }
        Byte Item { get; set; }
        UInt16 Correct { get; set; }
        UInt16 False { get; set; }
        UInt16 None { get; set; }

        public EnableTradeInstruction(Byte _ID, UInt16 _TextIDCorrect, UInt16 _TextIDFalse, UInt16 _TextIDNone, byte _Item)
        {
            ID = _ID;
            Item = _Item;
            Correct = _TextIDCorrect;
            False = _TextIDFalse;
            None = _TextIDNone;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                Item
            };
            Data.AddRange(Program.BEConverter.GetBytes(Correct));
            Data.AddRange(Program.BEConverter.GetBytes(False));
            Data.AddRange(Program.BEConverter.GetBytes(None));

            return Data.ToArray();
        }
    }

    public class IfInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.IF;
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
            List<byte> Data = new List<byte>
            {
                ID,
                Check
            };
            Data.AddRange(Program.BEConverter.GetBytes(Value));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_True));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_False));

            return Data.ToArray();
        }
    }

    public class RNGInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.RNG;
        Byte Condition { get; set; }
        byte Value { get; set; }
        Byte RangeEn { get; set; }
        UInt16 Offs_True { get; set; }
        UInt16 Offs_False { get; set; }

        public RNGInstruction(Byte _Condition, Byte _RangeEn, Byte Val, UInt16 True, UInt16 False)
        {
            Condition = _Condition;
            Value = Val;
            RangeEn = _RangeEn;
            Offs_True = True;
            Offs_False = False;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                Condition,
                RangeEn,
                Value
            };
            Data.AddRange(Program.BEConverter.GetBytes(Offs_True));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_False));

            return Data.ToArray();
        }
    }

    public class IfScriptVarInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.IF;
        Byte Check { get; set; }
        Byte Value { get; set; }
        Byte ScriptVarID { get; set; }
        UInt16 Offs_True { get; set; }
        UInt16 Offs_False { get; set; }

        public IfScriptVarInstruction(Byte Checked, Byte Condition, Byte Val, Byte _ScriptVarId, UInt16 True, UInt16 False)
        {
            Check = 0;
            Check |= (byte)(Checked << 2);
            Check |= Condition;
            Value = Val;
            ScriptVarID = _ScriptVarId;
            Offs_True = True;
            Offs_False = False;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                Check,
                Value,
                ScriptVarID
            };
            Data.AddRange(Program.BEConverter.GetBytes(Offs_True));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_False));

            return Data.ToArray();
        }
    }

    public class SetResponseInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.responses;
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
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes(Offs_1));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_2));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_3));

            return Data.ToArray();
        }
    }

    public class SetFlagInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubId = (byte)OldLists.SetSubTypes.flag;
        Byte FlagType { get; set; }
        bool OnOff { get; set; }
        UInt16 FlagID { get; set; }

        public SetFlagInstruction(string Type, UInt16 ID, bool _OnOff)
        {
            FlagType = Convert.ToByte(System.Enum.Parse(typeof(OldLists.IfSubTypes), Type.ToLower()));

            if (FlagType > 7)
                throw new Exception();

            FlagID = ID;

            OnOff = _OnOff;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubId,
                FlagType
            };
            Data.AddRange(Program.BEConverter.GetBytes(OnOff));
            Data.AddRange(Program.BEConverter.GetBytes(FlagID));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class SetAnimInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.animation;
        Byte Loops { get; set; }
        UInt16 U16 { get; set; }

        public SetAnimInstruction(Byte _SubId, Byte _Loops, UInt16 _Data)
        {
            SubID = _SubId;
            Loops = _Loops;
            U16 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID,
                Loops,
                0
            };
            Data.AddRange(Program.BEConverter.GetBytes(U16));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class SetAnimOffsetInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.animation_offset;
        UInt16 AnimID { get; set; }
        UInt32 Offset { get; set; }

        public SetAnimOffsetInstruction(UInt32 _Offset, UInt16 _AnimID)
        {
            AnimID = _AnimID;
            Offset = _Offset;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes(AnimID));
            Data.AddRange(Program.BEConverter.GetBytes(Offset));

            return Data.ToArray();
        }
    }

    public class SetAnimObjectInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.animation_object;
        UInt16 AnimID { get; set; }
        UInt16 Object { get; set; }

        public SetAnimObjectInstruction(UInt16 _Object, UInt16 _AnimID)
        {
            AnimID = _AnimID;
            Object = _Object;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes(AnimID));
            Data.AddRange(Program.BEConverter.GetBytes(Object));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            return Data.ToArray();
        }
    }

    public class SetAnimSpeedInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.animation_speed;
        UInt16 AnimID { get; set; }
        float Speed { get; set; }

        public SetAnimSpeedInstruction(decimal _Speed, UInt16 _AnimID)
        {
            AnimID = _AnimID;
            Speed = (float)_Speed;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes(AnimID));
            Data.AddRange(Program.BEConverter.GetBytes(Speed));
            return Data.ToArray();
        }
    }

    public class SetAnimKeyFramesInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.animation_keyframes;
        UInt16 AnimID { get; set; }
        byte Frame1 { get; set; }
        byte Frame2 { get; set; }
        byte Frame3 { get; set; }
        byte Frame4 { get; set; }

        public SetAnimKeyFramesInstruction(UInt16 _AnimID, byte Fr1, byte Fr2, byte Fr3, byte Fr4)
        {
            AnimID = _AnimID;
            Frame1 = Fr1;
            Frame2 = Fr2;
            Frame3 = Fr3;
            Frame4 = Fr4;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes(AnimID));
            Data.Add(Frame1);
            Data.Add(Frame2);
            Data.Add(Frame3);
            Data.Add(Frame4);
            return Data.ToArray();
        }
    }

    public class SetDListVisibilityInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.dlist_show;
        UInt16 DlistID { get; set; }
        byte VisibilityType { get; set; }

        public SetDListVisibilityInstruction(byte _VisibilityType, UInt16 _DlistID)
        {
            DlistID = _DlistID;
            VisibilityType = _VisibilityType;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Program.BEConverter.GetBytes(DlistID));
            Data.Add(VisibilityType);
            Data.Add(0);
            Data.Add(0);
            Data.Add(0);
            return Data.ToArray();
        }
    }

    public class SetSegmentTextureIDInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.segment_tex;
        Byte SegmentID { get; set; }
        UInt16 TextureID { get; set; }

        public SetSegmentTextureIDInstruction(byte _Segment, UInt16 _TextureID)
        {
            SegmentID = _Segment;
            TextureID = _TextureID;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID,
                SegmentID,
                0
            };
            Data.AddRange(Program.BEConverter.GetBytes(TextureID));
            Data.AddRange(Program.BEConverter.GetBytes((Int16)0));

            return Data.ToArray();
        }
    }

    public class SetPatternInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID;
        Byte[] Bytes { get; set; }

        public SetPatternInstruction(byte _SubID, Byte[] _Bytes)
        {
            SubID = _SubID;
            Bytes = _Bytes;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID
            };
            Data.AddRange(Bytes);

            return Data.ToArray();
        }
    }

    public class SetRGBAInstruction
    {
        readonly Byte ID = (byte)OldLists.InstructionIDs.SET;
        readonly Byte SubID = (byte)OldLists.SetSubTypes.env_color;
        Byte R { get; set; }
        Byte G { get; set; }
        Byte B { get; set; }
        Byte A { get; set; }

        public SetRGBAInstruction(Byte _R, Byte _G, Byte _B, Byte _A)
        {
            R = _R;
            G = _G;
            B = _B;
            A = _A;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubID,
                R,
                G,
                B,
                A
            };
            Data.AddRange(Program.BEConverter.GetBytes((Int16)0));

            return Data.ToArray();
        }
    }

    public class ExternalActorDependantInstruction
    {
        Byte ID { get; set; }
        Byte SubId { get; set; }
        Int16 ActorNum { get; set; }
        UInt16 Value { get; set; }
        UInt16 ActorType { get; set; }

        public ExternalActorDependantInstruction(Byte _ID, Byte _SubID, Int16 _ActorNum, UInt16 _ActorType, UInt16 _Data)
        {
            ID = _ID;
            SubId = _SubID;
            ActorNum = _ActorNum;
            ActorType = _ActorType;
            Value = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>
            {
                ID,
                SubId
            };
            Data.AddRange(Program.BEConverter.GetBytes(ActorNum));
            Data.AddRange(Program.BEConverter.GetBytes(ActorType));
            Data.AddRange(Program.BEConverter.GetBytes(Value));

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
