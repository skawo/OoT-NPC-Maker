using MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseSetInstruction(string[] SplitLine)
        {
            try
            {
                try
                {
                    Instruction Ram = H_SetRam(SplitLine);

                    if (Ram != null)
                        return Ram;

                    int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.SetSubTypes));

                    switch (SubID)
                    {
                        case (int)Lists.SetSubTypes.MOVEMENT_DISTANCE:
                        case (int)Lists.SetSubTypes.MOVEMENT_LOOP_DELAY:
                        case (int)Lists.SetSubTypes.COLLISION_RADIUS:
                        case (int)Lists.SetSubTypes.COLLISION_HEIGHT:
                        case (int)Lists.SetSubTypes.UNSUCCESSFUL_TRADE_TEXT_ID:
                        case (int)Lists.SetSubTypes.CURRENT_CUTSCENE_FRAME:
                            return H_SimpleSet(SubID, SplitLine, 0, UInt16.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.MOVEMENT_LOOP_START:
                        case (int)Lists.SetSubTypes.MOVEMENT_LOOP_END:
                        case (int)Lists.SetSubTypes.COLLISION_YOFFSET:
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
                        case (int)Lists.SetSubTypes.LIGHT_OFFSET_X:
                        case (int)Lists.SetSubTypes.LIGHT_OFFSET_Y:
                        case (int)Lists.SetSubTypes.LIGHT_OFFSET_Z:
                        case (int)Lists.SetSubTypes.LIGHT_RADIUS:
                            return H_SimpleSet(SubID, SplitLine, Int16.MinValue, Int16.MaxValue, typeof(Int32));
                        // case u32:
                            //return H_SimpleSet(SubID, SplitLine, 0, 0, typeof(UInt32));
                        // case s32:
                            //H_SimpleSet(SubID, SplitLine, Int32.MinValue, Int32.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.MOVEMENT_SPEED:
                        case (int)Lists.SetSubTypes.GRAVITY_FORCE:
                        case (int)Lists.SetSubTypes.TALK_RADIUS:
                        case (int)Lists.SetSubTypes.SMOOTHING_CONSTANT:
                            return H_SimpleSet(SubID, SplitLine, 0, 0, typeof(float));
                        case (int)Lists.SetSubTypes.LOOP_MOVEMENT:
                        case (int)Lists.SetSubTypes.HAVE_COLLISION:
                        case (int)Lists.SetSubTypes.PRESS_SWITCHES:
                        case (int)Lists.SetSubTypes.IS_TARGETTABLE:
                        case (int)Lists.SetSubTypes.PLAYER_CAN_MOVE:
                        case (int)Lists.SetSubTypes.ACTOR_CAN_MOVE:
                        case (int)Lists.SetSubTypes.DO_BLINKING_ANIMATIONS:
                        case (int)Lists.SetSubTypes.DO_TALKING_ANIMATIONS:
                        case (int)Lists.SetSubTypes.IS_ALWAYS_ACTIVE:
                        case (int)Lists.SetSubTypes.PAUSE_CUTSCENE:
                        case (int)Lists.SetSubTypes.IS_ALWAYS_DRAWN:
                        case (int)Lists.SetSubTypes.JUST_SCRIPT:
                        case (int)Lists.SetSubTypes.REACTS_IF_ATTACKED:
                        case (int)Lists.SetSubTypes.OPEN_DOORS:
                        case (int)Lists.SetSubTypes.MOVEMENT_IGNORE_Y:
                        case (int)Lists.SetSubTypes.FADES_OUT:
                        case (int)Lists.SetSubTypes.LIGHT_GLOW:
                        case (int)Lists.SetSubTypes.GENERATES_LIGHT:
                        case (int)Lists.SetSubTypes.NO_AUTO_ANIM:
                        case (int)Lists.SetSubTypes.TALK_MODE:
                        case (int)Lists.SetSubTypes.VISIBLE_ONLY_UNDER_LENS:
                        case (int)Lists.SetSubTypes.INVISIBLE:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 Val = 0;

                                if (VarType == (int)Lists.VarTypes.Normal)
                                    Val = (UInt32)ScriptHelpers.GetBoolConditionID(SplitLine, 2);
                                else
                                    Val = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, 1));

                                return new InstructionSet((byte)SubID, Val, VarType, 0);
                            }
                        case (int)Lists.SetSubTypes.TARGET_LIMB:
                        case (int)Lists.SetSubTypes.TARGET_DISTANCE:
                        case (int)Lists.SetSubTypes.HEAD_LIMB:
                        case (int)Lists.SetSubTypes.WAIST_LIMB:
                        case (int)Lists.SetSubTypes.MASS:
                        case (int)Lists.SetSubTypes.ALPHA:
                        case (int)Lists.SetSubTypes.MOVEMENT_PATH_ID:
                            return H_SimpleSet(SubID, SplitLine, byte.MinValue, byte.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.PLAYER_BOMBS:
                        case (int)Lists.SetSubTypes.PLAYER_BOMBCHUS:
                        case (int)Lists.SetSubTypes.PLAYER_ARROWS:
                        case (int)Lists.SetSubTypes.PLAYER_DEKUNUTS:
                        case (int)Lists.SetSubTypes.PLAYER_DEKUSTICKS:
                        case (int)Lists.SetSubTypes.PLAYER_SEEDS:
                        case (int)Lists.SetSubTypes.PLAYER_BEANS:
                            return H_SimpleSet(SubID, SplitLine, Int16.MinValue, Int16.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.PLAYER_HEALTH:
                            return H_SimpleSet(SubID, SplitLine, -20, 20, typeof(float));
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

                                byte VarType1 = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 FlagID = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType1, 0, UInt16.MaxValue));

                                UInt32 Val = 0;

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);

                                if (VarType2 == (int)Lists.VarTypes.Normal)
                                    Val = (UInt32)ScriptHelpers.GetBoolConditionID(SplitLine, 3);
                                else
                                    Val = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, 0, 1));

                                return new InstructionSetWTwoValues((byte)SubID, FlagID, VarType1, Val, VarType2, 0);
                            }
                        case (int)Lists.SetSubTypes.EFFECT_IF_ATTACKED:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.EffectsIfAttacked), ParseException.UnrecognizedEffectIfAttacked(SplitLine));
                        case (int)Lists.SetSubTypes.MOVEMENT_TYPE:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.MovementStyles), ParseException.UnrecognizedMovementStyle(SplitLine));
                        case (int)Lists.SetSubTypes.LOOKAT_TYPE:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.LookAtStyles), ParseException.UnrecognizedLookAtStyle(SplitLine));
                        case (int)Lists.SetSubTypes.HEAD_HORIZ_AXIS:
                        case (int)Lists.SetSubTypes.HEAD_VERT_AXIS:
                        case (int)Lists.SetSubTypes.WAIST_HORIZ_AXIS:
                        case (int)Lists.SetSubTypes.WAIST_VERT_AXIS:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.Axis), ParseException.UnrecognizedAxis(SplitLine));
                        case (int)Lists.SetSubTypes.BLINK_SEGMENT:
                        case (int)Lists.SetSubTypes.TALK_SEGMENT:
                            return H_SetByEnum(SubID, SplitLine, typeof(Lists.Segments), ParseException.UnrecognizedSegment(SplitLine));
                        case (int)Lists.SetSubTypes.CURRENT_ANIMATION:
                        case (int)Lists.SetSubTypes.CURRENT_ANIMATION_INSTANTLY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 4);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, VarType, Entry.Animations);

                                UInt32 Loops = 0;
                                byte VarType2 = 0;

                                if (SplitLine.Length == 4)
                                {
                                    VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                                    Loops = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, 0, Int32.MaxValue));
                                }

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, VarType, Loops, VarType2, 0);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_OBJECT:
                        case (int)Lists.SetSubTypes.ANIMATION_OFFSET:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, VarType, Entry.Animations);

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                                UInt32 Value2 = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, 0, UInt32.MaxValue));

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, VarType, Value2, VarType2, 0);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_SPEED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, VarType, Entry.Animations);

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                                object Speed = ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, 0, float.MaxValue);

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, VarType, Speed, VarType2, 0);
                            }
                        case (int)Lists.SetSubTypes.BLINK_PATTERN:
                        case (int)Lists.SetSubTypes.TALK_PATTERN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 6);

                                byte[] Data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };

                                for (int i = 2; i < SplitLine.Length; i++)
                                {
                                    int Segment = (SubID == (int)Lists.SetSubTypes.BLINK_PATTERN ? Entry.BlinkSegment : Entry.TalkSegment) - 8;

                                    UInt32? TexID = ScriptHelpers.Helper_GetSegmentDataEntryID(SplitLine, i,
                                                                                               Segment,
                                                                                               0,
                                                                                               Entry.Segments);

                                    Data[i - 2] = (byte)TexID;
                                }

                                return new InstructionSetPattern((byte)SubID, Data);
                            }
                        case (int)Lists.SetSubTypes.SEGMENT_ENTRY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                UInt32 SegmentID = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Segments), ParseException.UnrecognizedSegment(SplitLine));

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                UInt32? TexID = ScriptHelpers.Helper_GetSegmentDataEntryID(SplitLine, 3, (int)SegmentID, VarType, Entry.Segments);

                                return new InstructionSetWTwoValues((byte)SubID, SegmentID, 0, TexID, VarType, 0); 
                            }
                        case (int)Lists.SetSubTypes.ENV_COLOR:
                        case (int)Lists.SetSubTypes.LIGHT_COLOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                byte[] ScVars = new byte[3]
                                                            {
                                                                    ScriptHelpers.GetVarType(SplitLine, 2),
                                                                    ScriptHelpers.GetVarType(SplitLine, 3),
                                                                    ScriptHelpers.GetVarType(SplitLine, 4)
                                                            };

                                UInt32[] RGB = new UInt32[3] { 0, 0, 0 };

                                for (int i = 0; i < 3; i++)
                                {
                                    RGB[i] = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2 + i, ScVars[i], byte.MinValue, byte.MaxValue));
                                }

                                return new InstructionSetEnvColor((byte)SubID, RGB[0], RGB[1], RGB[2], ScVars[0], ScVars[1], ScVars[2]);
                            }
                        case (int)Lists.SetSubTypes.DLIST_VISIBILITY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 DListID = ScriptHelpers.Helper_GetDListID(SplitLine, 2, VarType, Entry.ExtraDisplayLists);

                                UInt32 DlistOption = ScriptHelpers.Helper_GetEnumByName(SplitLine, 3, typeof(Lists.DListVisibilityOptions), ParseException.UnregonizedDlistVisibility(SplitLine));

                                return new InstructionSetWTwoValues((byte)SubID, DListID, VarType, DlistOption, (byte)Lists.VarTypes.Normal, 0);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_STARTFRAME:
                        case (int)Lists.SetSubTypes.ANIMATION_ENDFRAME:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, VarType, Entry.Animations);

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                                UInt32? Frame = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, 255));

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, VarType, Frame, VarType2, 0);
                            }
                        case (int)Lists.SetSubTypes.CAMERA_TRACKING_ON:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                UInt32 TrackSubType = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (TrackSubType)
                                {
                                    case (int)Lists.TargetActorSubtypes.SELF: return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.SELF, 0, 0, 0, 0);
                                    case (int)Lists.TargetActorSubtypes.PLAYER: return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.PLAYER, 0, 0, 0, 0);
                                    case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                            byte ValueType = ScriptHelpers.GetVarType(SplitLine, 3);
                                            UInt32 ActorNum = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, ValueType, 0, UInt16.MaxValue));

                                            return new InstructionSetCameraTracking((byte)SubID, (byte)Lists.TargetActorSubtypes.CONFIG_ID, ActorNum, ValueType, 0, 0);
                                        }
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                            byte ValueType = ScriptHelpers.GetVarType(SplitLine, 3);
                                            UInt32 ActorNum = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, ValueType, 0, UInt16.MaxValue));

                                            byte ValueType2 = ScriptHelpers.GetVarType(SplitLine, 4);
                                            UInt32 ActorCat = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 4, ValueType, 0, UInt16.MaxValue));

                                            return new InstructionSetCameraTracking((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.CONFIG_ID, ActorNum, ValueType, ActorCat, ValueType2);
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
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                                byte Operator = ScriptHelpers.GetOperator(SplitLine, 2);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
                                UInt32 Time = 0;

                                if (VarType == (int)Lists.VarTypes.Normal)
                                    Time = ScriptHelpers.GetOcarinaTime(SplitLine, 3);
                                else
                                    Time = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, UInt16.MaxValue));

                                return new InstructionSet((byte)SubID, Time, VarType, Operator);
                            }
                        case (int)Lists.SetSubTypes.SFX_IF_ATTACKED:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                                Int32 SFXID = ScriptHelpers.Helper_GetSFXId(SplitLine, 2, VarType);

                                return new InstructionSet((byte)SubID, SFXID, VarType, 0);

                            }
                        case (int)Lists.SetSubTypes.EXT_VAR:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                byte Operator = ScriptHelpers.GetOperator(SplitLine, 4);
                                byte VarType = ScriptHelpers.GetVarType(SplitLine, 5);
                                Int32 Value = Convert.ToInt32(Convert.ToDecimal(ScriptHelpers.GetValueByType(SplitLine, 5, VarType, float.MinValue, float.MaxValue)));

                                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 2);
                                UInt32 ActorID = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, Int16.MaxValue));

                                byte ExtVarNum = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, 3, (int)Lists.VarTypes.Normal, 1, Lists.Num_User_Vars));

                                return new InstructionSetExtVar((byte)SubID, ExtVarNum, Value, VarType, ActorID, VarType2, Operator);
                            }
                        default: throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionSet(0, 0, 0, 0);
                }

            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }

        private Instruction H_SetRam(string[] SplitLine)
        {
            byte? SubID = ScriptHelpers.GetSubIDForRamType(SplitLine[1]);

            if (SubID != null)
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                byte Operator = ScriptHelpers.GetOperator(SplitLine, 2);

                byte VarType1 = ScriptHelpers.GetVarType(SplitLine, 1);
                UInt32 Value1 = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 1, VarType1, UInt32.MinValue, UInt32.MaxValue));

                byte VarType2 = ScriptHelpers.GetVarType(SplitLine, 3);
                Int32 Value2 = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType2, Int32.MinValue, Int32.MaxValue));

                return new InstructionSetWTwoValues((byte)SubID, Value1, VarType1, Value2, VarType2, Operator);
            }
            else
                return null;
        }

        private Instruction H_SetByEnum(int SubID, string[] SplitLine, Type Enum, ParseException Throw)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

            byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
            UInt32? Data = ScriptHelpers.Helper_GetEnumByNameOrVarType(SplitLine, 2, VarType, Enum, Throw);

            return new InstructionSet((byte)SubID, Data, VarType, 0);
        }

        private Instruction H_SimpleSet(int SubID, string[] SplitLine, int Min, int Max, Type ConvertType)
        {
            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

            byte Operator = ScriptHelpers.GetOperator(SplitLine, 2);

            byte VarType = ScriptHelpers.GetVarType(SplitLine, 3);
            object Data;

            if (ConvertType == typeof(UInt32) && VarType == (byte)Lists.VarTypes.Normal)
                Data = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, Min, Max));
            else if (ConvertType == typeof(float) && VarType == (byte)Lists.VarTypes.Normal)
                Data = (float)(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, Min, Max));
            else
                Data = Convert.ToInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, Min, Max));


            if (Data == null)
                throw ParseException.ParamConversionError(SplitLine);


            return new InstructionSet((byte)SubID, Data, VarType, Operator);
        }
    }
}
