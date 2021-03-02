using MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseSetInstruction(string[] SplitLine)
        {
            try
            {
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.SetSubTypes));

                try
                {
                    switch (SubID)
                    {
                        case (int)Lists.SetSubTypes.MOVEMENT_DISTANCE:
                        case (int)Lists.SetSubTypes.MOVEMENT_LOOP_DELAY:
                        case (int)Lists.SetSubTypes.COLLISION_RADIUS:
                        case (int)Lists.SetSubTypes.COLLISION_HEIGHT:
                        case (int)Lists.SetSubTypes.MOVEMENT_PATH_ID:
                        case (int)Lists.SetSubTypes.UNSUCCESSFUL_TRADE_TEXT_ID:
                        case (int)Lists.SetSubTypes.CUTSCENE_FRAME:
                            return H_SimpleSet(SubID, SplitLine, 0, UInt16.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.MOVEMENT_LOOP_START:
                        case (int)Lists.SetSubTypes.MOVEMENT_LOOP_END:
                        case (int)Lists.SetSubTypes.COLLISION_OFFSET_X:
                        case (int)Lists.SetSubTypes.COLLISION_OFFSET_Y:
                        case (int)Lists.SetSubTypes.COLLISION_OFFSET_Z:
                        case (int)Lists.SetSubTypes.TARGET_OFFSET_X:
                        case (int)Lists.SetSubTypes.TARGET_OFFSET_Y:
                        case (int)Lists.SetSubTypes.TARGET_OFFSET_Z:
                        case (int)Lists.SetSubTypes.MODEL_OFFSET_X:
                        case (int)Lists.SetSubTypes.MODEL_OFFSET_Y:
                        case (int)Lists.SetSubTypes.MODEL_OFFSET_Z:
                        case (int)Lists.SetSubTypes.PLAYER_RUPEES:
                        case (int)Lists.SetSubTypes.CAMERA_ID:
                        case (int)Lists.SetSubTypes.LOOKAT_OFFSET_X:
                        case (int)Lists.SetSubTypes.LOOKAT_OFFSET_Y:
                        case (int)Lists.SetSubTypes.LOOKAT_OFFSET_Z:
                        case (int)Lists.SetSubTypes.CURRENT_PATH_NODE:
                        case (int)Lists.SetSubTypes.CURRENT_ANIMATION_FRAME:
                        case (int)Lists.SetSubTypes.CURRENT_CUTSCENE_FRAME:
                            return H_SimpleSet(SubID, SplitLine, Int16.MinValue, Int16.MaxValue, typeof(Int32));
                        // case u32:
                            //return H_SimpleSet(SubID, SplitLine, 0, 0, typeof(UInt32));
                        // case s32:
                            //H_SimpleSet(SubID, SplitLine, Int32.MinValue, Int32.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.MODEL_SCALE:
                        case (int)Lists.SetSubTypes.MOVEMENT_SPEED:
                        case (int)Lists.SetSubTypes.MODEL_SCALE_SMOOTHLY:
                        case (int)Lists.SetSubTypes.GRAVITY_FORCE:
                        case (int)Lists.SetSubTypes.TALK_RADIUS:
                            return H_SimpleSet(SubID, SplitLine, 0, 0, typeof(float));
                        case (int)Lists.SetSubTypes.LOOP_MOVEMENT:
                        case (int)Lists.SetSubTypes.HAVE_COLLISION:
                        case (int)Lists.SetSubTypes.PRESS_SWITCHES:
                        case (int)Lists.SetSubTypes.IS_PUSHABLE:
                        case (int)Lists.SetSubTypes.IS_TARGETTABLE:
                        case (int)Lists.SetSubTypes.PLAYER_CAN_MOVE:
                        case (int)Lists.SetSubTypes.ACTOR_CAN_MOVE:
                        case (int)Lists.SetSubTypes.DO_BLINKING_ANIMATIONS:
                        case (int)Lists.SetSubTypes.DO_TALKING_ANIMATIONS:
                        case (int)Lists.SetSubTypes.IS_ALWAYS_ACTIVE:
                        case (int)Lists.SetSubTypes.PAUSE_CUTSCENE:
                        case (int)Lists.SetSubTypes.IS_ALWAYS_DRAWN:
                        case (int)Lists.SetSubTypes.JUST_SCRIPT:
                        case (int)Lists.SetSubTypes.TIMED_PATH:
                        case (int)Lists.SetSubTypes.REACTS_IF_ATTACKED:
                        case (int)Lists.SetSubTypes.OPEN_DOORS:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVariable(SplitLine, 2);
                                Lists.ConditionTypes Condition = Lists.ConditionTypes.EQUALTO;

                                if (VarType < (int)Lists.VarTypes.Var1)
                                {
                                    if (VarType != (int)Lists.VarTypes.RNG)
                                        Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 2);
                                }

                                return new InstructionSet((byte)SubID, Condition, VarType);
                            }
                        case (int)Lists.SetSubTypes.TARGET_LIMB:
                        case (int)Lists.SetSubTypes.TARGET_DISTANCE:
                        case (int)Lists.SetSubTypes.HEAD_LIMB:
                        case (int)Lists.SetSubTypes.WAIST_LIMB:
                            return H_SimpleSet(SubID, SplitLine, byte.MinValue, byte.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.PLAYER_BOMBS:
                        case (int)Lists.SetSubTypes.PLAYER_BOMBCHUS:
                        case (int)Lists.SetSubTypes.PLAYER_ARROWS:
                        case (int)Lists.SetSubTypes.PLAYER_HEATLH:
                        case (int)Lists.SetSubTypes.PLAYER_DEKUNUTS:
                        case (int)Lists.SetSubTypes.PLAYER_DEKUSTICKS:
                        case (int)Lists.SetSubTypes.PLAYER_MAGIC:
                            return H_SimpleSet(SubID, SplitLine, sbyte.MinValue, sbyte.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.TEXTBOX_RESPONSE_ACTIONS:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                                ScriptHelpers.ErrorIfNumParamsBigger(SplitLine, 5);

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

                                UInt16 FlagID = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, UInt16.MaxValue));
                                Lists.ConditionTypes Condition = ScriptHelpers.GetBoolConditionID(SplitLine, 3);

                                return new InstructionSetWObject((byte)SubID, Convert.ToUInt16(FlagID), Condition);
                            }
                        case (int)Lists.SetSubTypes.MOVEMENT_TYPE:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.MovementStyles), ParseException.UnrecognizedMovementStyle(SplitLine));
                        case (int)Lists.SetSubTypes.LOOKAT_TYPE:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.LookAtStyles), ParseException.UnrecognizedMovementStyle(SplitLine));
                        case (int)Lists.SetSubTypes.HEAD_AXIS:
                        case (int)Lists.SetSubTypes.WAIST_AXIS:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.Axis), ParseException.UnrecognizedMovementStyle(SplitLine));
                        case (int)Lists.SetSubTypes.CURRENT_ANIMATION:
                        case (int)Lists.SetSubTypes.CURRENT_ANIMATION_INSTANTLY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 4);

                                UInt32? AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);

                                byte Loops = 0;

                                if (SplitLine.Length == 4)
                                    Loops = Convert.ToByte(ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, byte.MaxValue));

                                return new InstructionSet((byte)SubID, Convert.ToUInt16(AnimID), Loops);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_OBJECT:
                        case (int)Lists.SetSubTypes.ANIMATION_OFFSET:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                UInt32? AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);
                                UInt16 Object = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));

                                return new InstructionSetWObject((byte)SubID, Convert.ToUInt16(AnimID), Object);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_SPEED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                UInt32? AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);

                                return new InstructionSetWObject((byte)SubID, Convert.ToUInt16(AnimID), Convert.ToDecimal(SplitLine[3]));
                            }
                        case (int)Lists.SetSubTypes.BLINK_PATTERN:
                        case (int)Lists.SetSubTypes.TALK_PATTERN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 8);

                                byte[] Data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                                for (int i = 2; i < SplitLine.Length; i++)
                                {
                                    int Segment = (SubID == (int)Lists.SetSubTypes.BLINK_PATTERN ? Entry.BlinkSegment : Entry.TalkSegment) - 8;

                                    Int32? TexID = ScriptHelpers.Helper_GetTextureID(SplitLine, i,
                                                                                     Segment,
                                                                                     Entry.Segments);

                                    Data[i - 2] = (byte)TexID;
                                }

                                return new InstructionSetPattern((byte)SubID, Data);
                            }
                        case (int)Lists.SetSubTypes.TEXTURE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                UInt32? SegmentID = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Segments), ParseException.UnrecognizedSegment(SplitLine));

                                Int32? TexID = ScriptHelpers.Helper_GetTextureID(SplitLine, 3, (int)SegmentID, Entry.Segments);

                                return new InstructionSet((byte)SubID, Convert.ToUInt16(TexID), (byte)SegmentID);
                            }
                        case (int)Lists.SetSubTypes.ENV_COLOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                byte[] ScVars = new byte[3]
                                                            {
                                                                    ScriptHelpers.GetVariable(SplitLine, 2),
                                                                    ScriptHelpers.GetVariable(SplitLine, 3),
                                                                    ScriptHelpers.GetVariable(SplitLine, 4)
                                                            };

                                int[] RGB = new int[3] { 0, 0, 0 };

                                for (int i = 0; i < 3; i++)
                                {
                                    if (ScVars[i] < (int)Lists.VarTypes.Var1)
                                        RGB[i] = Convert.ToInt32(ScriptHelpers.GetValueAndCheckRange(SplitLine,
                                                                                                     (ScVars[i] == (int)Lists.VarTypes.RNG ? 3 : 2) + i,
                                                                                                     byte.MinValue, byte.MaxValue));
                                }

                                return new InstructionSetEnvColor((byte)SubID, Convert.ToByte(RGB[0]), Convert.ToByte(RGB[1]), Convert.ToByte(RGB[2]), ScVars[0], ScVars[1], ScVars[2]);
                            }
                        case (int)Lists.SetSubTypes.DLIST_VISIBILITY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                Int32? DListID = ScriptHelpers.Helper_GetDListID(SplitLine, 2, Entry.DLists);

                                UInt32? DlistOption = ScriptHelpers.Helper_GetEnumByName(SplitLine, 3, typeof(Lists.DListVisibilityOptions), ParseException.UnregonizedDlistVisibility(SplitLine));

                                return new InstructionSet((byte)SubID, Convert.ToInt16(DListID), Convert.ToByte(DlistOption));
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_KEYFRAMES:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 4, 7);

                                Int32? AnimID = (Int32?)ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);

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

                                UInt32? TrackSubType = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (TrackSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.SELF: return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.SELF, 0, 0, 0, 0);
                                    case (int)Lists.TargetActorSubtypes.PLAYER: return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.PLAYER, 0, 0, 0, 0);
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                            byte ValueType = ScriptHelpers.GetVariable(SplitLine, 3);

                                            if (ValueType != 0)
                                                return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.CONFIG_ID, 0, 0, ValueType, 0);
                                            else
                                            {
                                                UInt16 ActorNum = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, UInt16.MaxValue));
                                                return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.CONFIG_ID, ActorNum, 0, 0, 0);
                                            }
                                        }
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                            byte ValueType = ScriptHelpers.GetVariable(SplitLine, 3);
                                            byte ValueType2 = ScriptHelpers.GetVariable(SplitLine, 4);

                                            UInt16 ActorNum = 0;
                                            byte ActorType = 0;

                                            if (ValueType == 0)
                                                ActorNum = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 3, 0, Int16.MaxValue));

                                            if (ValueType2 == 0)
                                                ActorType = Convert.ToByte(ScriptHelpers.GetValueAndCheckRange(SplitLine, 4, 0, 12));

                                            return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.CONFIG_ID, ActorNum, ActorType, ValueType, ValueType2);
                                        }
                                    default: throw new Exception();
                                }
                            }
                        case (int)Lists.SetSubTypes.CUTSCENE_SLOT:
                                return H_SimpleSet(SubID, SplitLine, -1, 10, typeof(Int32));
                        case (int)Lists.SetSubTypes.SCRIPT_START:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
                                return new InstructionSetScriptStart(new InstructionLabel(SplitLine[2]));
                            }
                        case (int)Lists.SetSubTypes.TIME_OF_DAY:
                        case (int)Lists.SetSubTypes.TIMED_PATH_START_TIME:
                        case (int)Lists.SetSubTypes.TIMED_PATH_END_TIME:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVariable(SplitLine, 2);
                                UInt16 Time = 0;

                                if (VarType < (int)Lists.VarTypes.Var1)
                                {
                                    if (VarType != (int)Lists.VarTypes.RNG)
                                        Time = ScriptHelpers.GetOcarinaTime(SplitLine, 2);
                                }

                                return new InstructionSet((byte)SubID, Time, VarType);
                            }
                        case (int)Lists.SetSubTypes.VAR_1:
                        case (int)Lists.SetSubTypes.VAR_2:
                        case (int)Lists.SetSubTypes.VAR_3:
                        case (int)Lists.SetSubTypes.VAR_4:
                        case (int)Lists.SetSubTypes.VAR_5:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                byte Operator = ScriptHelpers.GetOperator(SplitLine, 2);
                                byte ValueType = ScriptHelpers.GetVariable(SplitLine, 3);
                                float Value = ScriptHelpers.GetValueByType(SplitLine, 3, ValueType);

                                return new InstructionSetScriptVar((byte)SubID, Operator, Value, ValueType, -1);
                            }
                        case (int)Lists.SetSubTypes.EXT_VAR_1:
                        case (int)Lists.SetSubTypes.EXT_VAR_2:
                        case (int)Lists.SetSubTypes.EXT_VAR_3:
                        case (int)Lists.SetSubTypes.EXT_VAR_4:
                        case (int)Lists.SetSubTypes.EXT_VAR_5:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 5);

                                byte Operator = ScriptHelpers.GetOperator(SplitLine, 3);
                                byte ValueType = ScriptHelpers.GetVariable(SplitLine, 4);
                                float Value = ScriptHelpers.GetValueByType(SplitLine, 4, ValueType);

                                Int16 ActorID = Convert.ToInt16(ScriptHelpers.GetValueAndCheckRange(SplitLine, 2, 0, Int16.MaxValue));

                                return new InstructionSetScriptVar((byte)SubID, Operator, Value, ValueType, ActorID);
                            }
                        default: throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
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

        private Instruction H_SetByEnum(int SubID, string[] SplitLine, Type Enum, ParseException ThrowOnError)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

            UInt32? Data = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, Enum, ThrowOnError);
            return new InstructionSet((byte)SubID, Convert.ToByte(Data), 0);
        }

        private Instruction H_SimpleSet(int SubID, string[] SplitLine, int Min, int Max, Type ConvertType)
        {
            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

            byte VarType = ScriptHelpers.GetVariable(SplitLine, 2);
            object Data = 0;

            if (VarType == (int)Lists.VarTypes.RNG)
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

            if (VarType < (int)Lists.VarTypes.Var1)
            {
                if (ConvertType == typeof(UInt32))
                    Data = ScriptHelpers.Helper_ConvertToUInt32(SplitLine[VarType == (int)Lists.VarTypes.RNG ? 3 : 2]);
                else if (ConvertType == typeof(float))
                    Data = Convert.ToDecimal(SplitLine[VarType == (int)Lists.VarTypes.RNG ? 3 : 2]);
                else
                    Data = Convert.ToInt32(ScriptHelpers.GetValueAndCheckRange(SplitLine,
                                                                                VarType == (int)Lists.VarTypes.RNG ? 3 : 2,
                                                                                Min, Max));

                if (Data == null)
                    throw ParseException.ParamConversionError(SplitLine);
            }


            return new InstructionSet((byte)SubID, Data, VarType);
        }
    }
}
