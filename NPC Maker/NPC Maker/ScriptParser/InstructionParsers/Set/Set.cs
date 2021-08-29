using System;

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
                        case (int)Lists.SetSubTypes.MAXIMUM_ROAM:
                        case (int)Lists.SetSubTypes.MOVEMENT_LOOP_DELAY:
                        case (int)Lists.SetSubTypes.COLLISION_RADIUS:
                        case (int)Lists.SetSubTypes.COLLISION_HEIGHT:
                        case (int)Lists.SetSubTypes.CUTSCENE_FRAME:
                            return H_SimpleSet(SubID, SplitLine, 0, UInt16.MaxValue);
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
                        case (int)Lists.SetSubTypes.SHADOW_RADIUS:
                            return H_SimpleSet(SubID, SplitLine, Int16.MinValue, Int16.MaxValue);
                        // case u32:
                        //return H_SimpleSet(SubID, SplitLine, 0, 0, typeof(UInt32));
                        // case s32:
                        //H_SimpleSet(SubID, SplitLine, Int32.MinValue, Int32.MaxValue, typeof(Int32));
                        case (int)Lists.SetSubTypes.GRAVITY_FORCE:
                            return H_SimpleSet(SubID, SplitLine, Int32.MinValue, Int32.MaxValue);
                        case (int)Lists.SetSubTypes.MOVEMENT_SPEED:
                        case (int)Lists.SetSubTypes.TALK_RADIUS:
                            return H_SimpleSet(SubID, SplitLine, 0, Int32.MaxValue);
                        case (int)Lists.SetSubTypes.SMOOTHING_CONSTANT:
                            return H_SimpleSet(SubID, SplitLine, -2, 65535);
                        case (int)Lists.SetSubTypes.LOOP_MOVEMENT:
                        case (int)Lists.SetSubTypes.HAS_COLLISION:
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
                        case (int)Lists.SetSubTypes.AFFECTED_BY_LENS:
                        case (int)Lists.SetSubTypes.INVISIBLE:
                        case (int)Lists.SetSubTypes.CASTS_SHADOW:
                        case (int)Lists.SetSubTypes.PLAYER_ANIMATE_MODE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);

                                var Val = new ScriptVarVal();
                                Val.Vartype = ScriptHelpers.GetVarType(SplitLine, 2);

                                if (Val.Vartype == (int)Lists.VarTypes.NORMAL)
                                    Val.Value = (float)ScriptHelpers.GetBoolConditionID(SplitLine, 2);
                                else
                                    Val.Value = (float)ScriptHelpers.GetValueByType(SplitLine, 2, Val.Vartype, 0, 1);

                                return new InstructionSet((byte)SubID, Val, 0);
                            }
                        case (int)Lists.SetSubTypes.TARGET_LIMB:
                        case (int)Lists.SetSubTypes.TARGET_DISTANCE:
                        case (int)Lists.SetSubTypes.HEAD_LIMB:
                        case (int)Lists.SetSubTypes.WAIST_LIMB:
                        case (int)Lists.SetSubTypes.MASS:
                        case (int)Lists.SetSubTypes.ALPHA:
                        case (int)Lists.SetSubTypes.MOVEMENT_PATH_ID:
                            return H_SimpleSet(SubID, SplitLine, byte.MinValue, byte.MaxValue);
                        case (int)Lists.SetSubTypes.PLAYER_BOMBS:
                        case (int)Lists.SetSubTypes.PLAYER_BOMBCHUS:
                        case (int)Lists.SetSubTypes.PLAYER_ARROWS:
                        case (int)Lists.SetSubTypes.PLAYER_DEKUNUTS:
                        case (int)Lists.SetSubTypes.PLAYER_DEKUSTICKS:
                        case (int)Lists.SetSubTypes.PLAYER_SEEDS:
                        case (int)Lists.SetSubTypes.PLAYER_BEANS:
                            return H_SimpleSet(SubID, SplitLine, Int16.MinValue, Int16.MaxValue);
                        case (int)Lists.SetSubTypes.PLAYER_HEALTH:
                            return H_SimpleSet(SubID, SplitLine, -20, 20);
                        case (int)Lists.SetSubTypes.RESPONSE_ACTIONS:
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

                                var FlagID = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, UInt16.MaxValue);
                                var Val = new ScriptVarVal();

                                Val.Vartype = ScriptHelpers.GetVarType(SplitLine, 3);

                                if (Val.Vartype == (int)Lists.VarTypes.NORMAL)
                                    Val.Value = (float)ScriptHelpers.GetBoolConditionID(SplitLine, 3);
                                else
                                    Val.Value = (float)ScriptHelpers.GetValueByType(SplitLine, 3, Val.Vartype, 0, 1);

                                return new InstructionSetWTwoValues((byte)SubID, FlagID, Val, 0);
                            }
                        case (int)Lists.SetSubTypes.ATTACKED_EFFECT:
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
                        case (int)Lists.SetSubTypes.ANIMATION:
                        case (int)Lists.SetSubTypes.ANIMATION_INSTANTLY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 4);

                                var AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);
                                var Once = new ScriptVarVal();

                                if (SplitLine.Length == 4)
                                {
                                    if (SplitLine[3].ToUpper() == Lists.Keyword_Once)
                                        Once.Value = (float)1;
                                    else
                                        throw ParseException.UnrecognizedParameter(SplitLine);
                                }

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, Once, 0);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_OBJECT:
                        case (int)Lists.SetSubTypes.ANIMATION_OFFSET:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                var AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);
                                var Value2 = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt32.MaxValue);

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, Value2, 0);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_SPEED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                var AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);
                                var Speed = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, float.MaxValue);

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, Speed, 0);
                            }
                        case (int)Lists.SetSubTypes.BLINK_PATTERN:
                        case (int)Lists.SetSubTypes.TALK_PATTERN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 3, 6);

                                byte[] Data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };

                                for (int i = 2; i < SplitLine.Length; i++)
                                {
                                    int Segment = (SubID == (int)Lists.SetSubTypes.BLINK_PATTERN ? Entry.BlinkSegment : Entry.TalkSegment) - 8;
                                    var TexID = ScriptHelpers.Helper_GetSegmentDataEntryID(SplitLine, i, Segment, Entry.Segments);

                                    Data[i - 2] = (byte)TexID.Value;
                                }

                                return new InstructionSetPattern((byte)SubID, Data);
                            }
                        case (int)Lists.SetSubTypes.SEGMENT_ENTRY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                var SegmentID = new ScriptVarVal();
                                SegmentID.Value = (float)ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.Segments), ParseException.UnrecognizedSegment(SplitLine));

                                var TexID = ScriptHelpers.Helper_GetSegmentDataEntryID(SplitLine, 3, (int)SegmentID.Value, Entry.Segments);

                                return new InstructionSetWTwoValues((byte)SubID, SegmentID, TexID, 0);
                            }
                        case (int)Lists.SetSubTypes.ENV_COLOR:
                        case (int)Lists.SetSubTypes.LIGHT_COLOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);

                                ScriptVarVal R = new ScriptVarVal();
                                ScriptVarVal G = new ScriptVarVal();
                                ScriptVarVal B = new ScriptVarVal();
                                ScriptVarVal A = null;

                                ScriptHelpers.GetRGBorRGBA(SplitLine, 2, ref R, ref G, ref B, ref A);

                                return new InstructionSetEnvColor((byte)SubID, R, G, B);
                            }
                        case (int)Lists.SetSubTypes.DLIST_VISIBILITY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                var DListID = ScriptHelpers.Helper_GetDListID(SplitLine, 2, Entry.ExtraDisplayLists);

                                var DlistOption = new ScriptVarVal();
                                DListID.Value = (float)ScriptHelpers.Helper_GetEnumByName(SplitLine, 3, typeof(Lists.DListVisibilityOptions), ParseException.UnregonizedDlistVisibility(SplitLine));

                                return new InstructionSetWTwoValues((byte)SubID, DListID, DlistOption, 0);
                            }
                        case (int)Lists.SetSubTypes.ANIMATION_STARTFRAME:
                        case (int)Lists.SetSubTypes.ANIMATION_ENDFRAME:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);

                                var AnimID = ScriptHelpers.Helper_GetAnimationID(SplitLine, 2, Entry.Animations);
                                var Frame = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, byte.MaxValue);

                                return new InstructionSetWTwoValues((byte)SubID, AnimID, Frame, 0);
                            }
                        case (int)Lists.SetSubTypes.CAMERA_TRACKING_ON:
                        case (int)Lists.SetSubTypes.REF_ACTOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                                object TrackSubType = ScriptHelpers.Helper_GetEnumByName(SplitLine, 2, typeof(Lists.TargetActorSubtypes), ParseException.UnrecognizedParameter(SplitLine));

                                switch (Convert.ToInt32(TrackSubType))
                                {
                                    case (int)Lists.TargetActorSubtypes.SELF: return new InstructionSetActor((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.SELF, new ScriptVarVal());
                                    case (int)Lists.TargetActorSubtypes.PLAYER: return new InstructionSetActor((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.PLAYER, new ScriptVarVal());
                                    case (int)Lists.TargetActorSubtypes.REF_ACTOR: return new InstructionSetActor((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.REF_ACTOR, new ScriptVarVal());
                                    case (int)Lists.TargetActorSubtypes.NPCMAKER:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);
                                            var ActorNum = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, UInt16.MaxValue);

                                            return new InstructionSetActor((byte)SubID, (byte)Lists.TargetActorSubtypes.NPCMAKER, ActorNum);
                                        }
                                    case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                                        {
                                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);
                                            var ActorNum = ScriptHelpers.Helper_GetActorId(SplitLine, 3);

                                            return new InstructionSetActor((byte)SubID, (byte)(int)Lists.TargetActorSubtypes.ACTOR_ID, ActorNum);
                                        }
                                    default: throw new Exception();
                                }
                            }
                        case (int)Lists.SetSubTypes.CUTSCENE_SLOT:
                            return H_SimpleSet(SubID, SplitLine, -1, 10);
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

                                var Time = new ScriptVarVal();
                                Time.Vartype = ScriptHelpers.GetVarType(SplitLine, 3);

                                if (Time.Vartype == (int)Lists.VarTypes.NORMAL)
                                    Time.Value = (float)Convert.ToDecimal(ScriptHelpers.GetOcarinaTime(SplitLine, 3));
                                else
                                    Time.Value = (float)ScriptHelpers.GetValueByType(SplitLine, 3, Time.Vartype, 0, UInt16.MaxValue);

                                return new InstructionSet((byte)SubID, Time, Operator);
                            }
                        case (int)Lists.SetSubTypes.ATTACKED_SFX:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);
                                var SFXID = ScriptHelpers.Helper_GetSFXId(SplitLine, 2);

                                return new InstructionSet((byte)SubID, SFXID, 0);

                            }
                        case (int)Lists.SetSubTypes.EXT_VAR:
                            {
                                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 6);

                                byte Operator = ScriptHelpers.GetOperator(SplitLine, 4);

                                var Value = ScriptHelpers.GetScriptVarVal(SplitLine, 5, float.MinValue, float.MaxValue);
                                var ActorID = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, Int16.MaxValue);

                                byte ExtVarNum = Convert.ToByte(ScriptHelpers.GetValueByType(SplitLine, 3, (int)Lists.VarTypes.NORMAL, 1, Lists.Num_User_Vars));

                                return new InstructionSetExtVar((byte)SubID, ExtVarNum, Value, ActorID, Operator);
                            }
                        case (int)Lists.SetSubTypes.PLAYER_ANIMATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotBetween(SplitLine, 4, 7);

                                var Offset = ScriptHelpers.Helper_GetLinkAnimation(SplitLine, 2);
                                var Speed = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, float.MaxValue);
                                var StFr = new ScriptVarVal();
                                var EFr = new ScriptVarVal(255);

                                if (SplitLine.Length > 4)
                                    StFr = ScriptHelpers.GetScriptVarVal(SplitLine, 4, 0, 255);

                                if (SplitLine.Length > 5)
                                    EFr = ScriptHelpers.GetScriptVarVal(SplitLine, 5, (float)StFr.Value, 255);

                                byte Once = 0;

                                if (SplitLine.Length == 7)
                                {
                                    if (SplitLine[6].ToUpper() == Lists.Keyword_Once)
                                        Once = 1;
                                    else
                                        throw ParseException.UnrecognizedParameter(SplitLine);
                                }

                                return new InstructionSetPlayerAnim((byte)SubID, Offset, Speed, StFr, EFr, Once);

                            }
                        default: throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionNop();
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

                var Value1 = ScriptHelpers.GetScriptVarVal(SplitLine, 1, UInt32.MinValue, UInt32.MaxValue);
                var Value2 = ScriptHelpers.GetScriptVarVal(SplitLine, 3, float.MinValue, float.MaxValue);

                return new InstructionSetWTwoValues((byte)SubID, Value1, Value2, Operator);
            }
            else
                return null;
        }

        private Instruction H_SetByEnum(int SubID, string[] SplitLine, Type Enum, ParseException Throw)
        {
            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 3);
            var Data = ScriptHelpers.GetScriptVarVal(SplitLine, 2, Enum, Throw);

            return new InstructionSet((byte)SubID, Data, 0);
        }

        private Instruction H_SimpleSet(int SubID, string[] SplitLine, int Min, int Max)
        {
            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

            byte Operator = ScriptHelpers.GetOperator(SplitLine, 2);
            var Data = ScriptHelpers.GetScriptVarVal(SplitLine, 3, Min, Max);

            if (Data == null)
                throw ParseException.ParamConversionError(SplitLine);

            return new InstructionSet((byte)SubID, Data, Operator);
        }
    }
}
