using MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction h_SimpleSet(int SubID, string[] SplitLine, int Min, int Max, Type ConvertType)
        {
            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

            byte VarType = ScriptHelpers.GetVariable(SplitLine[2]);
            object Data = 0;

            if (VarType == (int)Lists.VarTypes.Keyword_RNG)
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

            if (VarType < (int)Lists.VarTypes.Keyword_ScriptVar1)
            {
                if (ConvertType == typeof(UInt32))
                    Data = ScriptHelpers.Helper_ConvertToUInt32(SplitLine[VarType == (int)Lists.VarTypes.Keyword_RNG ? 3 : 2]);
                else if (ConvertType == typeof(float))
                    Data = Convert.ToDecimal(SplitLine[VarType == (int)Lists.VarTypes.Keyword_RNG ? 3 : 2]);
                else
                    Data = Convert.ToUInt32(ParserHelpers.GetValueAndCheckRange(SplitLine,
                                                                                VarType == (int)Lists.VarTypes.Keyword_RNG ? 3 : 2,
                                                                                Min, Max));

                if (Data == null)
                    throw ParseException.ParamConversionError(SplitLine);
            }


            return new InstructionSet((byte)SubID, Data, VarType);
        }


        private Instruction ParseSetInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = (int)System.Enum.Parse(typeof(Lists.SetSubTypes), SplitLine[1].ToUpper());

                try
                {
                    if (SubID < 35)        // u16 Subtypes
                    {
                        return h_SimpleSet(SubID, SplitLine, 0, UInt16.MaxValue, typeof(Int32));
                    }
                    else if (SubID < 70)        // s16 Subtypes
                    {
                        return h_SimpleSet(SubID, SplitLine, Int16.MinValue, Int16.MaxValue, typeof(Int32));
                    }
                    else if (SubID < 105)        // u32 Subtypes
                    {
                        return h_SimpleSet(SubID, SplitLine, 0, 0, typeof(UInt32));
                    }
                    else if (SubID < 140)        // s32 Subtypes
                    {
                        return h_SimpleSet(SubID, SplitLine, Int32.MinValue, Int32.MaxValue, typeof(Int32));
                    }
                    else if (SubID < 175)        // Float Subtypes
                    {
                        return h_SimpleSet(SubID, SplitLine, 0, 0, typeof(float));
                    }
                    else if (SubID < 195)        // bool Subtypes
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                        byte VarType = ScriptHelpers.GetVariable(SplitLine[2]);
                        byte Condition = 0;

                        if (VarType == (int)Lists.VarTypes.Keyword_RNG)
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                        if (VarType < (int)Lists.VarTypes.Keyword_ScriptVar1)
                        {
                            Condition = ScriptHelpers.GetBoolConditionID(SplitLine[VarType == (int)Lists.VarTypes.Keyword_RNG ? 3 : 2]);
                            
                            if (Condition == byte.MaxValue)
                                throw ParseException.UnrecognizedCondition(SplitLine);
                        }

                        return new InstructionSet((byte)SubID, Condition, VarType);
                    }
                    else if (SubID < 210)        // u8 Subtypes
                    {
                        return h_SimpleSet(SubID, SplitLine, byte.MinValue, byte.MaxValue, typeof(Int32));
                    }
                    else if (SubID < 220)        // s8 Subtypes
                    {
                        return h_SimpleSet(SubID, SplitLine, sbyte.MinValue, sbyte.MaxValue, typeof(Int32));
                    }
                    else
                    {
                        switch (SubID)
                        {
                            case (int)Lists.SetSubTypes.TEXTBOX_RESPONSE_ACTIONS:
                                {
                                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 5);

                                    return new InstructionSetResponses((byte)SubID,
                                                                        new InstructionLabel(SplitLine[2]),
                                                                        SplitLine.Length > 3 ? new InstructionLabel(SplitLine[3]) : new InstructionLabel(SplitLine[2]),
                                                                        SplitLine.Length > 4 ? new InstructionLabel(SplitLine[4]) : new InstructionLabel(SplitLine[3]));
                                }
                            case (int)Lists.SetSubTypes.FLAG_INF:
                            case (int)Lists.SetSubTypes.FLAG_EVENT:
                            case (int)Lists.SetSubTypes.FLAG_SWITCH:
                            case (int)Lists.SetSubTypes.FLAG_SCENE:
                            case (int)Lists.SetSubTypes.FLAG_TREASURE:
                            case (int)Lists.SetSubTypes.FLAG_ROOM_CLEAR:
                            case (int)Lists.SetSubTypes.FLAG_SCENE_COLLECT:
                            case (int)Lists.SetSubTypes.FLAG_TEMPORARY:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    UInt16 FlagID = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                                    byte WrittenCondition = ScriptHelpers.GetBoolConditionID(SplitLine[3]);

                                    if (WrittenCondition != byte.MaxValue)
                                        return new InstructionSetWObject((byte)SubID, Convert.ToUInt16(FlagID), WrittenCondition);
                                    else
                                        throw ParseException.UnrecognizedCondition(SplitLine);

                                }
                            case (int)Lists.SetSubTypes.MOVEMENT_TYPE:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Data = ScriptHelpers.Helper_GetMovementStyleID(SplitLine[2]);

                                    if (Data == null)
                                        throw ParseException.UnrecognizedMovementStyle(SplitLine);

                                    return new InstructionSet((byte)SubID, Convert.ToByte(Data), 0);
                                }
                            case (int)Lists.SetSubTypes.LOOKAT_TYPE:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Data = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.LookAtStyles), SplitLine[2]);

                                    if (Data == null)
                                        throw ParseException.UnrecognizedLookAtStyle(SplitLine);

                                    return new InstructionSet((byte)SubID, Convert.ToByte(Data), 0);
                                }
                            case (int)Lists.SetSubTypes.HEAD_AXIS:
                            case (int)Lists.SetSubTypes.WAIST_AXIS:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                    UInt32? Data = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.Axis), SplitLine[2]);

                                    if (Data == null)
                                        throw ParseException.UnrecognizedAxis(SplitLine);

                                    return new InstructionSet((byte)SubID, Convert.ToByte(Data), 0);
                                }
                            case (int)Lists.SetSubTypes.CURRENT_ANIMATION:
                            case (int)Lists.SetSubTypes.CURRENT_ANIMATION_INSTANTLY:
                                {
                                    ScriptHelpers.ErrorIfNumParamsBigger(SplitLine, 4);
                                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                    UInt32? AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine[2], Entry.Animations);

                                    if (AnimID == null)
                                        AnimID = ScriptHelpers.Helper_ConvertToUInt32(SplitLine[2]);

                                    if (AnimID == null || (AnimID > UInt16.MaxValue || AnimID < 0))
                                        throw ParseException.UnrecognizedAnimation(SplitLine);

                                    byte Loops = 0;

                                    if (SplitLine.Length == 4)
                                        Loops = Convert.ToByte(ParserHelpers.GetValueAndCheckRange(SplitLine, 3, 0, byte.MaxValue));

                                    return new InstructionSet((byte)SubID, Convert.ToUInt16(AnimID), Loops);
                                }
                            case (int)Lists.SetSubTypes.ANIMATION_OBJECT:
                            case (int)Lists.SetSubTypes.ANIMATION_OFFSET:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    UInt32? AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine[2], Entry.Animations);

                                    if (AnimID == null)
                                        AnimID = ScriptHelpers.Helper_ConvertToUInt32(SplitLine[2]);

                                    if (AnimID == null || (AnimID > UInt16.MaxValue || AnimID < 0))
                                        throw ParseException.UnrecognizedAnimation(SplitLine);

                                    UInt16 Object = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));

                                    return new InstructionSetWObject((byte)SubID, Convert.ToUInt16(AnimID), Object);
                                }
                            case (int)Lists.SetSubTypes.ANIMATION_SPEED:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    UInt32? AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine[2], Entry.Animations);

                                    if (AnimID == null)
                                        AnimID = ScriptHelpers.Helper_ConvertToUInt32(SplitLine[2]);

                                    if (AnimID > UInt16.MaxValue || AnimID < 0)
                                        throw ParseException.UnrecognizedAnimation(SplitLine);

                                    return new InstructionSetWObject((byte)SubID, Convert.ToUInt16(AnimID), Convert.ToDecimal(SplitLine[3]));
                                }
                            case (int)Lists.SetSubTypes.BLINK_PATTERN:
                            case (int)Lists.SetSubTypes.TALK_PATTERN:
                                {
                                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                                    ScriptHelpers.ErrorIfNumParamsBigger(SplitLine, 8);

                                    byte[] Data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                                    for (int i = 2; i < SplitLine.Length; i++)
                                    {
                                        Int32? TexID = ScriptHelpers.Helper_GetTextureID(SplitLine[i],
                                                                                        SubID == (int)Lists.SetSubTypes.BLINK_PATTERN ? Entry.BlinkSegment - 8 : Entry.TalkSegment - 8, Entry.Textures);

                                        if (TexID == null)
                                            TexID = Convert.ToInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, i, 0, 31));

                                        Data[i - 2] = (byte)TexID;
                                    }

                                    return new InstructionSetPattern((byte)SubID, Data);
                                }
                            case (int)Lists.SetSubTypes.TEXTURE:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    UInt32? SegmentID = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.Segments), SplitLine[2].ToUpper());

                                    if (SegmentID == null)
                                        throw ParseException.UnrecognizedSegment(SplitLine);

                                    Int32? TexID = ScriptHelpers.Helper_GetTextureID(SplitLine[3], (int)SegmentID, Entry.Textures);

                                    if (TexID == null)
                                        TexID = Convert.ToInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, 3, 0, 31));

                                    return new InstructionSet((byte)SubID, Convert.ToUInt16(TexID), (byte)SegmentID);
                                }
                            case (int)Lists.SetSubTypes.ENV_COLOR:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                    byte[] ScVars = new byte[3]
                                                                {
                                                                    ScriptHelpers.GetVariable(SplitLine[2]),
                                                                    ScriptHelpers.GetVariable(SplitLine[3]),
                                                                    ScriptHelpers.GetVariable(SplitLine[4])
                                                                };

                                    int[] RGB = new int[3] { 0, 0, 0 };

                                    for (int i = 0; i < 3; i++)
                                    {
                                        if (ScVars[i] < (int)Lists.VarTypes.Keyword_ScriptVar1)
                                            RGB[i] = Convert.ToInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, 
                                                                                                         (ScVars[i] == (int)Lists.VarTypes.Keyword_RNG ? 3 : 2) + i, 
                                                                                                         byte.MinValue, byte.MaxValue));
                                    }

                                    return new InstructionSetEnvColor((byte)SubID, Convert.ToByte(RGB[0]), Convert.ToByte(RGB[1]), Convert.ToByte(RGB[2]), ScVars[0], ScVars[1], ScVars[2]);
                                }
                            case (int)Lists.SetSubTypes.DLIST_VISIBILITY:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    Int32? DListID = ScriptHelpers.Helper_GetDListID(SplitLine[2], Entry.DLists);

                                    if (DListID == null)
                                        DListID = Convert.ToInt32(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                                    UInt32? DlistOption = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.DListVisibilityOptions), SplitLine[3].ToUpper());

                                    if (DlistOption == null)
                                        throw ParseException.UnregonizedDlistVisibility(SplitLine);

                                    return new InstructionSet((byte)SubID, Convert.ToInt16(DListID), Convert.ToByte(DlistOption));
                                }
                            case (int)Lists.SetSubTypes.ANIMATION_KEYFRAMES:
                                {
                                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);
                                    ScriptHelpers.ErrorIfNumParamsBigger(SplitLine, 7);

                                    Int32? AnimID = (Int32?)ScriptHelpers.Helper_GetAnimationID(SplitLine[2], Entry.Animations);

                                    if (AnimID == null)
                                        AnimID = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));

                                    int?[] Frames = new int?[4] { 255, 255, 255, 255 };

                                    for (int i = 3; i < SplitLine.Length; i++)
                                    {
                                        Frames[i - 3] = ScriptHelpers.Helper_ConvertToInt32(SplitLine[i]);

                                        if (Frames[i - 3] == null)
                                            throw ParseException.ParamConversionError(SplitLine);

                                        if (Frames[i - 3] < 0)
                                            Frames[i - 3] = 255;

                                        if (Frames[i - 3] > 255)
                                            throw ParseException.ParamOutOfRange(SplitLine);
                                    }

                                    byte[] FrameB = new byte[4] {
                                                                    Convert.ToByte(Frames[0]),
                                                                    Convert.ToByte(Frames[1]),
                                                                    Convert.ToByte(Frames[2]),
                                                                    Convert.ToByte(Frames[3]),
                                                                };

                                    return new InstructionSetKeyframes((byte)SubID, Convert.ToUInt16(AnimID), FrameB);
                                }
                            case (int)Lists.SetSubTypes.CAMERA_TRACKING_ON:
                                {
                                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                    UInt32? TrackSubType = ScriptHelpers.Helper_GetEnumByName(typeof(Lists.TurnTowardsSubtypes), SplitLine[2].ToUpper());

                                    switch (TrackSubType)
                                    {
                                        case (int)Lists.TurnTowardsSubtypes.SELF: return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TurnTowardsSubtypes.SELF, 0, 0, 0, 0);
                                        case (int)Lists.TurnTowardsSubtypes.PLAYER: return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TurnTowardsSubtypes.PLAYER, 0, 0, 0, 0);
                                        case (int)Lists.TurnTowardsSubtypes.CONFIG_ID:
                                            {
                                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                                byte ValueType = ScriptHelpers.GetVariable(SplitLine[3]);

                                                if (ValueType != 0)
                                                    return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TurnTowardsSubtypes.CONFIG_ID, 0, 0, ValueType, 0);
                                                else
                                                {
                                                    UInt16 ActorNum = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));
                                                    return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TurnTowardsSubtypes.CONFIG_ID, ActorNum, 0, 0, 0);
                                                }
                                            }
                                        case (int)Lists.TurnTowardsSubtypes.ACTOR_ID:
                                            {
                                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                                byte ValueType = ScriptHelpers.GetVariable(SplitLine[3]);
                                                byte ValueType2 = ScriptHelpers.GetVariable(SplitLine[4]);

                                                UInt16 ActorNum = 0;
                                                byte ActorType = 0;

                                                if (ValueType == 0)
                                                    ActorNum = Convert.ToUInt16(ParserHelpers.GetValueAndCheckRange(SplitLine, 3, 0, Int16.MaxValue));

                                                if (ValueType2 == 0)
                                                    ActorType = Convert.ToByte(ParserHelpers.GetValueAndCheckRange(SplitLine, 4, 0, 12));

                                                return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TurnTowardsSubtypes.CONFIG_ID, ActorNum, ActorType, ValueType, ValueType2);
                                            }
                                        default: throw new Exception();
                                    }
                                }
                            case (int)Lists.SetSubTypes.CUTSCENE_SLOT:
                                {
                                    return h_SimpleSet(SubID, SplitLine, -1, 10, typeof(Int32));
                                }
                            case (int)Lists.SetSubTypes.SCRIPT_START:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                    return new InstructionSetScriptStart(new InstructionLabel(SplitLine[2]));
                                }
                            case (int)Lists.SetSubTypes.VAR_1:
                            case (int)Lists.SetSubTypes.VAR_2:
                            case (int)Lists.SetSubTypes.VAR_3:
                            case (int)Lists.SetSubTypes.VAR_4:
                            case (int)Lists.SetSubTypes.VAR_5:
                                {
                                    ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                    byte Operator = 0;

                                    switch (SplitLine[2])
                                    {
                                        case "=": Operator = 0; break;
                                        case "-": Operator = 1; break;
                                        case "+": Operator = 2; break;
                                        default: throw ParseException.UnrecognizedOperator(SplitLine);
                                    }

                                    byte ValueType = ScriptHelpers.GetVariable(SplitLine[3]);
                                    sbyte Value = 0;

                                    if (ValueType < (int)Lists.VarTypes.Keyword_ScriptVar1)
                                        Value = Convert.ToSByte(ParserHelpers.GetValueAndCheckRange(SplitLine, 
                                                                                                    ValueType == (int)Lists.VarTypes.Keyword_RNG ? 4 : 3,
                                                                                                    sbyte.MinValue, sbyte.MaxValue));

                                    return new InstructionSetScriptVar((byte)SubID, Operator, (sbyte)Value, ValueType);
                                }
                            default: throw new Exception();
                        }
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionSet((byte)SubID, 0, 0);
                }

            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
