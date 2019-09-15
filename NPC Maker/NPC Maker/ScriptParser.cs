using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NPC_Maker
{
    public enum Segments
    {
        SEGMENT_8 = 0,
        SEGMENT_9 = 1,
        SEGMENT_A = 2,
        SEGMENT_B = 3,
        SEGMENT_C = 4,
        SEGMENT_D = 5,
        SEGMENT_E = 6,
        SEGMENT_F = 7
    }

    public enum InstructionIDs
    {
        NOP = 0,
        IF = 1,
        SET = 2,
        WAITFOR = 3,
        ENABLETEXTBOX = 4,
        SHOWTEXTBOX = 5,
        GIVEITEM = 6,
        GOTO = 7,
        TURNTOPLAYER = 8,
        PLAY = 9,
        KILL = 10,
        RETURN = 255,
    }

    public enum IfSubTypes
    {
        inf_table = 0,
        event_chk_inf = 1,
        switch_table = 2,
        uscene = 3,
        treasure = 4,
        room_clear = 5,
        scene_collect = 6,
        temporary = 7,

        age = 10,
        day = 11,
        talking = 12,
        has_empty_bottle = 13,

        rupees = 30,
        time_of_day = 31,
        scene_id = 32,
        worn_mask = 33,
        skulltulas = 34,
    }

    public enum SetSubTypes
    {
        /* u16 Subtypes */
        movement_distance = 0,
        loop_delay = 1,
        collision_radius = 2,
        collision_height = 3,
        target_limb = 4,
        path_id = 6,
        time_of_day = 7,

        /* s16 Subtypes */
        loop_start = 35,
        loop_end = 36,
        collision_offset_x = 37,
        collision_offset_y = 38,
        collision_offset_z = 39,
        target_offset_x = 40,
        target_offset_y = 41,
        target_offset_z = 42,
        model_offset_x = 43,
        model_offset_y = 44,
        model_offset_z = 45,
        rupees = 46,

        /* u32 Subtypes */

        /* s32 Subtypes */

        /* Float Subtypes */
        model_scale = 140,
        movement_speed = 141,

        /* u8 and bool Subtypes */
        do_loop = 175,
        collision = 176,
        shadow = 177, 
        switches = 178, 
        pushable = 179,
        targettable = 180,
        player_movement = 181,
        movement = 182,
        do_blinking_anim = 183,
        do_talking_anim = 184,

        /* s8 Subtypes */


        /* Special handling */
        responses = 230,
        flag = 231,
        movement_type = 234,
        look_type = 235,
        head_axis = 236,
        animation = 237,
        animation_object = 238,
        animation_offset = 239,
        animation_speed = 240,
        script_start = 241,
        blink_pattern = 242,
        talk_pattern = 243,
        segment_tex = 244,
    }

    public enum WaitForSubTypes
    {
        path_end = 0,
        response = 1,
        text_end = 2,
        endless = 3,

        path_node = 35,
        frames = 36,
        animation_frame = 37,
    }

    public enum MovementStyles
    {
        none = 0,
        random = 1,
        follow = 2,
        path_collisionwise = 3,
        path_direct
    }

    public class ScriptParser
    {
        public List<string> ParseErrors = new List<string>();
        private static Dictionary<string, int> Labels = new Dictionary<string, int>();

        public ScriptParser()
        {
        }

        public byte[] Parse(string Script, List<AnimationEntry> Animations, List<List<TextureEntry>> Textures)
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

            foreach (string Line in Lines)                                                                      // Convert every instruction into an 8 byte array and add it to the output
            {
                byte[] ParsedBytes = GetInstructionBytes(Line, Animations, Textures, ref ParseErrors);

                if (ParsedBytes.Length != 8)
                {
                    System.Windows.Forms.MessageBox.Show("Fatal error. Instruction not 8 bytes:" + Line);
                    return Parsed.ToArray();
                }

                Parsed.AddRange(ParsedBytes);
            }

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

        private static Int32 Helper_GetAnimationID(string AnimName, List<AnimationEntry> Animations)
        {
            for (int i = 0; i < Animations.Count; i++ )
            {
                if (AnimName.ToLower() == Animations[i].Name.Replace(" ", "").ToLower())
                    return i;
            }

            return -1;
        }

        private static Int32 Helper_GetTextureID(string TextureName, int Segment, List<List<TextureEntry>> Textures)
        {
            for (int i = 0; i < Textures[Segment].Count; i++)
            {
                if (TextureName.ToLower() == Textures[Segment][i].Name.Replace(" ", "").ToLower())
                    return i;
            }

            return -1;
        }

        private static Int32 Helper_GetSFXId(string SFXName)
        {
            try
            {
                return (int)System.Enum.Parse(typeof(OotSFX.SFXes), SFXName.ToUpper());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static byte[] GetInstructionBytes(string Line, List<AnimationEntry> Animations, List<List<TextureEntry>> Textures, ref List<string> ParseErrors)
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

                            int IfSubType = (int)System.Enum.Parse(typeof(IfSubTypes), Instr[1].ToLower());

                            if (IfSubType < 10)
                            {
                                if (Instr.Length != 8)
                                    throw new WrongParamCountException(Line);

                                UInt32 FlagID = Helper_ConvertToUInt32(Instr[2]);
                                int Label_True = GetLabelOffset(Line, Instr[5]);
                                int Label_False = GetLabelOffset(Line, Instr[7]);

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

                                int Label_True = GetLabelOffset(Line, Instr[4]);
                                int Label_False = GetLabelOffset(Line, Instr[6]);

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
                            else if (IfSubType < 64)
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
                            else
                            {
                                throw new Exception();
                            }
                        }
                    case "set":
                        {
                            if (Instr.Length < 2)
                                throw new WrongParamCountException(Line);

                            int SetSubType = (int)System.Enum.Parse(typeof(SetSubTypes), Instr[1].ToLower());

                            if (SetSubType < 35)        // u16 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                UInt32 Data = Helper_ConvertToUInt32(Instr[2]);

                                if (Data > UInt16.MaxValue || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU16Instruction SetU16 = new GenericU16Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToUInt16(Data));
                                return SetU16.GetByteData();
                            }
                            else if (SetSubType < 70)   // s16 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = Helper_ConvertToInt32(Instr[2]);

                                if (Data > Int16.MaxValue || Data < Int16.MinValue)
                                    throw new ParamOutOfRangeException(Line);

                                GenericS16Instruction SetS16 = new GenericS16Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToInt16(Data));
                                return SetS16.GetByteData();
                            }
                            else if (SetSubType < 105) // u32 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                UInt32 Data = Helper_ConvertToUInt32(Instr[2]);

                                GenericU32Instruction SetU32 = new GenericU32Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToUInt32(Data));
                                return SetU32.GetByteData();
                            }
                            else if (SetSubType < 140) // s32 Subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = Helper_ConvertToInt32(Instr[2]);

                                if (Data > Int32.MaxValue || Data < Int32.MinValue)
                                    throw new ParamOutOfRangeException(Line);

                                GenericS32Instruction SetS32 = new GenericS32Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToInt32(Data));
                                return SetS32.GetByteData();
                            }
                            else if (SetSubType < 175) // Float subtypes
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                GenericFloatInstruction SetFloat = new GenericFloatInstruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToDecimal(Instr[2]));
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

                                GenericU8Instruction SetU8 = new GenericU8Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Data));
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

                                GenericS8Instruction SetS8 = new GenericS8Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToSByte(Data));
                                return SetS8.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.responses)
                            {
                                if (Instr.Length < 3 || Instr.Length > 5)
                                    throw new WrongParamCountException(Line);

                                int Label_1 = GetLabelOffset(Line, Instr[2]);
                                int Label_2 = Instr.Length > 3 ? GetLabelOffset(Line, Instr[3]) : GetLabelOffset(Line, Instr[2]);
                                int Label_3 = Instr.Length > 4 ? GetLabelOffset(Line, Instr[4]) :
                                                                 Instr.Length > 3 ? GetLabelOffset(Line, Instr[3]) : GetLabelOffset(Line, Instr[2]);

                                if (Label_1 > UInt16.MaxValue || Label_2 > UInt16.MaxValue || Label_3 > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                SetResponseInstruction Respond = new SetResponseInstruction(Convert.ToUInt16(Label_1), Convert.ToUInt16(Label_2), Convert.ToUInt16(Label_3));
                                return Respond.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.flag) 
                            {
                                if (Instr.Length != 5)
                                    throw new WrongParamCountException(Line);

                                UInt32 FlagID = Helper_ConvertToUInt32(Instr[3]);

                                if (FlagID > UInt16.MaxValue || FlagID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                if (Instr[4].ToLower() != "true" && Instr[4].ToLower() != "false")
                                    throw new Exception(Line);

                                SetFlagInstruction SetFlag = new SetFlagInstruction(Instr[2], Convert.ToUInt16(FlagID), Instr[4].ToLower() == "true" ? true : false);
                                return SetFlag.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.movement_type) 
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = 0;

                                switch (Instr[2].ToLower())
                                {
                                    case "none": Data = 0; break;
                                    case "random": Data = 1; break;
                                    case "follow": Data = 2; break;
                                    case "path_collisionwise": Data = 3; break;
                                    case "path_direct": Data = 4; break;
                                    default: Data = Helper_ConvertToInt32(Instr[2]); break;
                                }

                                if (Data > 4 || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU8Instruction SetMovT = new GenericU8Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Data));
                                return SetMovT.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.look_type) 
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = 0;

                                switch (Instr[2].ToLower())
                                {
                                    case "none": Data = 0; break;
                                    case "body": Data = 1; break;
                                    case "head": Data = 2; break;
                                    default: Data = Helper_ConvertToInt32(Instr[2]); break;
                                }

                                if (Data > 2 || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU8Instruction SetLookT = new GenericU8Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Data));
                                return SetLookT.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.head_axis) 
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                Int32 Data = 0;

                                switch (Instr[2].ToLower())
                                {
                                    case "xz": Data = 0; break;
                                    case "xy": Data = 1; break;
                                    case "yz": Data = 2; break;
                                    default: Data = Helper_ConvertToInt32(Instr[2]); break;
                                }

                                if (Data > 2 || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU8Instruction SetHeadA = new GenericU8Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToByte(Data));
                                return SetHeadA.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.animation)
                            {
                                if (Instr.Length < 3 && Instr.Length > 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                UInt32 Loops = 0;

                                if (Instr.Length == 4)
                                {
                                    Loops = Helper_ConvertToUInt32(Instr[3]);

                                    if (Loops > Byte.MaxValue || Loops < 0)
                                        throw new ParamOutOfRangeException(Line);
                                }

                                SetAnimInstruction SetAnim = new SetAnimInstruction(Convert.ToByte(Loops), Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.animation_object) 
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                Int32 Object = Helper_ConvertToInt32(Instr[3]);

                                if (Object > UInt16.MaxValue || Object < 0)
                                    throw new ParamOutOfRangeException(Line);

                                SetAnimObjectInstruction SetAnim = new SetAnimObjectInstruction(Convert.ToUInt16(Object), Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.animation_offset) 
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                UInt32 Offset = Helper_ConvertToUInt32(Instr[3]);

                                SetAnimOffsetInstruction SetAnim = new SetAnimOffsetInstruction(Offset, Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.animation_speed)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                Int32 AnimID = Helper_GetAnimationID(Instr[2], Animations);

                                if (AnimID == -1)
                                    AnimID = Helper_ConvertToInt32(Instr[2]);

                                if (AnimID > (Animations.Count() - 1) || AnimID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                SetAnimSpeedInstruction SetAnim = new SetAnimSpeedInstruction(Convert.ToDecimal(Instr[3]), Convert.ToUInt16(AnimID));
                                return SetAnim.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.script_start)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                int GotoLabel = GetLabelOffset(Line, Instr[2]);

                                if (GotoLabel > UInt16.MaxValue || GotoLabel > UInt16.MaxValue)
                                    throw new LabelOutOfRangeException(Line);

                                GenericU16Instruction SetScriptStart = new GenericU16Instruction((byte)InstructionIDs.SET, (byte)SetSubType, Convert.ToUInt16(GotoLabel));
                                return SetScriptStart.GetByteData();
                            }
                            else if (SetSubType == (int)SetSubTypes.segment_tex)
                            {
                                if (Instr.Length != 4)
                                    throw new WrongParamCountException(Line);

                                int SegmentID = (int)System.Enum.Parse(typeof(Segments), Instr[2].ToUpper());
                                Int32 TexID = Helper_GetTextureID(Instr[3], SegmentID, Textures);

                                if (TexID == -1)
                                    TexID = Helper_ConvertToInt32(Instr[3]);

                                if (TexID > 31 || TexID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                SetSegmentTextureIDInstruction SetSegmentTex = new SetSegmentTextureIDInstruction(Convert.ToByte(SegmentID), Convert.ToUInt16(TexID));
                                return SetSegmentTex.GetByteData();
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    case "waitfor":
                        {
                            if (Instr.Length < 2)
                                throw new WrongParamCountException(Line);

                            int WaitForSubType = (int)System.Enum.Parse(typeof(WaitForSubTypes), Instr[1].ToLower());

                            if (WaitForSubType < 35)
                            {
                                if (Instr.Length != 2)
                                    throw new WrongParamCountException(Line);

                                GenericInstructionWithSubType WaitResp = new GenericInstructionWithSubType((byte)InstructionIDs.WAITFOR, (byte)WaitForSubType);
                                return WaitResp.GetByteData();
                            }
                            else if (WaitForSubType < 70)
                            {
                                if (Instr.Length != 3)
                                    throw new WrongParamCountException(Line);

                                UInt32 Data = Helper_ConvertToUInt32(Instr[2]);

                                if (Data > UInt16.MaxValue || Data < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU16Instruction WaitForU16 = new GenericU16Instruction((byte)InstructionIDs.WAITFOR, (byte)WaitForSubType, Convert.ToUInt16(Data));
                                return WaitForU16.GetByteData();
                            }
                            else
                            {
                                throw new Exception();
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

                            GenericU16Instruction Give = new GenericU16Instruction((byte)InstructionIDs.GIVEITEM, 0, Convert.ToUInt16(ItemID));
                            return Give.GetByteData();
                        }
                    case "goto":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            int GotoLabel = GetLabelOffset(Line, Instr[1]);

                            if (GotoLabel > UInt16.MaxValue || GotoLabel > UInt16.MaxValue)
                                throw new LabelOutOfRangeException(Line);

                            GenericU16Instruction Goto = new GenericU16Instruction((byte)InstructionIDs.GOTO, 0, Convert.ToUInt16(GotoLabel));
                            return Goto.GetByteData();
                        }
                    case "turn_towards_player":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Turn = new GenericInstruction((byte)InstructionIDs.TURNTOPLAYER);
                            return Turn.GetByteData();
                        }
                    case "return":
                        {
                            if (Instr.Length != 1)
                                throw new WrongParamCountException(Line);

                            GenericInstruction Stop = new GenericInstruction((byte)InstructionIDs.RETURN);
                            return Stop.GetByteData();
                        }
                    case "play":
                        {
                            if (Instr.Length != 3)
                                throw new WrongParamCountException(Line);

                            if (Instr[1].ToLower() == "sfx")
                            {
                                int SNDID = Helper_GetSFXId(Instr[2]);

                                if (SNDID == -1)
                                    SNDID = Helper_ConvertToInt32(Instr[2]);

                                if (SNDID > UInt16.MaxValue || SNDID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU16Instruction Sound = new GenericU16Instruction((byte)InstructionIDs.PLAY, 0, Convert.ToUInt16(SNDID));
                                return Sound.GetByteData();
                            }
                            else if (Instr[1].ToLower() == "music")
                            {
                                UInt32 SNDID = Helper_ConvertToUInt32(Instr[2]);

                                if (SNDID > byte.MaxValue || SNDID < 0)
                                    throw new ParamOutOfRangeException(Line);

                                GenericU8Instruction Sound = new GenericU8Instruction((byte)InstructionIDs.PLAY, 1, Convert.ToByte(SNDID));
                                return Sound.GetByteData();
                            }
                            else
                                throw new Exception();
                        }
                    case "kill":
                        {
                            if (Instr.Length != 2)
                                throw new WrongParamCountException(Line);

                            byte Type = 0;
                            Int32 ActorNum = 0;
                            Int32 ActorType = 0;

                            if (Instr[1].ToLower() == "self")
                                Type = 0;
                            else
                            {
                                if (Instr[1].ToLower() == "configid")
                                {
                                    if (Instr.Length != 3)
                                        throw new WrongParamCountException(Line);

                                    Type = 1;
                                    ActorNum = Helper_ConvertToInt32(Instr[2]);

                                    if (ActorNum > UInt16.MaxValue || ActorNum < 0)
                                        throw new ParamOutOfRangeException(Line);
                                }
                                else if (Instr[1].ToLower() == "actorid")
                                {
                                    if (Instr.Length != 4)
                                        throw new WrongParamCountException(Line);

                                    Type = 2;
                                    ActorNum = Helper_ConvertToInt32(Instr[2]);
                                    ActorType = Helper_ConvertToInt32(Instr[3]);

                                    if (ActorNum > UInt16.MaxValue)
                                        throw new ParamOutOfRangeException(Line);

                                    if (ActorType > 12 || ActorType < 0)
                                        throw new ParamOutOfRangeException(Line);
                                }
                                else
                                    throw new Exception();
                            }

                            KillInstruction Kill = new KillInstruction((byte)Type, Convert.ToInt16(ActorNum), Convert.ToUInt16(ActorType));
                            return Kill.GetByteData();
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

    public class GenericInstructionWithSubType
    {
        Byte ID { get; set; }
        Byte SubType { get; set; }

        public GenericInstructionWithSubType(Byte _ID, Byte _SubType)
        {
            ID = _ID;
            SubType = _SubType;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubType);
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

        public GenericU8Instruction(Byte _ID, Byte _SubID, byte _Data)
        {
            ID = _ID;
            SubID = _SubID;
            U8 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.Add(U8);
            Data.Add(0);
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
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.Add((byte)S8);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes((UInt32)0));

            return Data.ToArray();
        }
    }

    public class GenericU16Instruction
    {
        Byte ID { get; set; }
        Byte SubID { get; set; }
        UInt16 U16 { get; set; }

        public GenericU16Instruction(Byte _ID, Byte _SubID, UInt16 _Data)
        {
            ID = _ID;
            SubID = _SubID;
            U16 = _Data;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.AddRange(Program.BEConverter.GetBytes(U16));
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
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
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
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
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
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
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
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            Data.AddRange(Program.BEConverter.GetBytes(Fl));

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

    public class SetResponseInstruction
    {
        Byte ID = (byte)InstructionIDs.SET;
        Byte SubID = (byte)SetSubTypes.responses;
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
            Data.Add(SubID);
            Data.AddRange(Program.BEConverter.GetBytes(Offs_1));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_2));
            Data.AddRange(Program.BEConverter.GetBytes(Offs_3));

            return Data.ToArray();
        }
    }

    public class SetFlagInstruction
    {
        Byte ID = (byte)InstructionIDs.SET;
        Byte SubId = (byte)SetSubTypes.flag;
        Byte FlagType { get; set; }
        bool OnOff { get; set; }
        UInt16 FlagID { get; set; }

        public SetFlagInstruction(string Type, UInt16 ID, bool _OnOff)
        {
            FlagType = Convert.ToByte(System.Enum.Parse(typeof(IfSubTypes), Type.ToLower()));

            if (FlagType > 7)
                throw new Exception();

            FlagID = ID;

            OnOff = _OnOff;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubId);
            Data.Add(FlagType);
            Data.AddRange(Program.BEConverter.GetBytes(OnOff));
            Data.AddRange(Program.BEConverter.GetBytes(FlagID));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class SetAnimInstruction
    {
        Byte ID = (byte)InstructionIDs.SET;
        Byte SubID = (byte)SetSubTypes.animation;
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
            Data.Add(SubID);
            Data.Add(Loops);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes(U16));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

            return Data.ToArray();
        }
    }

    public class SetAnimOffsetInstruction
    {
        Byte ID = (byte)InstructionIDs.SET;
        Byte SubID = (byte)SetSubTypes.animation_offset;
        UInt16 AnimID { get; set; }
        UInt32 Offset { get; set; }

        public SetAnimOffsetInstruction(UInt32 _Offset, UInt16 _AnimID)
        {
            AnimID = _AnimID;
            Offset = _Offset;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.AddRange(Program.BEConverter.GetBytes(AnimID));
            Data.AddRange(Program.BEConverter.GetBytes(Offset));

            return Data.ToArray();
        }
    }

    public class SetAnimObjectInstruction
    {
        Byte ID = (byte)InstructionIDs.SET;
        Byte SubID = (byte)SetSubTypes.animation_object;
        UInt16 AnimID { get; set; }
        UInt16 Object { get; set; }

        public SetAnimObjectInstruction(UInt16 _Object, UInt16 _AnimID)
        {
            AnimID = _AnimID;
            Object = _Object;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.AddRange(Program.BEConverter.GetBytes(AnimID));
            Data.AddRange(Program.BEConverter.GetBytes(Object));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));
            return Data.ToArray();
        }
    }

    public class SetAnimSpeedInstruction
    {
        Byte ID = (byte)InstructionIDs.SET;
        Byte SubID = (byte)SetSubTypes.animation_speed;
        UInt16 AnimID { get; set; }
        float Speed { get; set; }

        public SetAnimSpeedInstruction(decimal _Speed, UInt16 _AnimID)
        {
            AnimID = _AnimID;
            Speed = (float)_Speed;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.AddRange(Program.BEConverter.GetBytes(AnimID));
            Data.AddRange(Program.BEConverter.GetBytes(Speed));
            return Data.ToArray();
        }
    }

    public class SetSegmentTextureIDInstruction
    {
        Byte ID = (byte)InstructionIDs.SET;
        Byte SubID = (byte)SetSubTypes.segment_tex;
        Byte SegmentID { get; set; }
        UInt16 TextureID { get; set; }

        public SetSegmentTextureIDInstruction(byte _Segment, UInt16 _TextureID)
        {
            SegmentID = _Segment;
            TextureID = _TextureID;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubID);
            Data.Add(SegmentID);
            Data.Add(0);
            Data.AddRange(Program.BEConverter.GetBytes(TextureID));
            Data.AddRange(Program.BEConverter.GetBytes((Int16)0));

            return Data.ToArray();
        }
    }

    public class KillInstruction
    {
        Byte ID = (byte)InstructionIDs.KILL;
        Byte SubId { get; set; }
        Int16 ActorNum { get; set; }
        UInt16 ActorType { get; set; }

        public KillInstruction(Byte _SubID, Int16 _ActorNum, UInt16 _ActorType)
        {
            SubId = _SubID;
            ActorNum = _ActorNum;
            ActorType = _ActorType;
        }

        public byte[] GetByteData()
        {
            List<byte> Data = new List<byte>();

            Data.Add(ID);
            Data.Add(SubId);
            Data.AddRange(Program.BEConverter.GetBytes(ActorNum));
            Data.AddRange(Program.BEConverter.GetBytes(ActorType));
            Data.AddRange(Program.BEConverter.GetBytes((UInt16)0));

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
