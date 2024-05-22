using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class ScriptsUsages
    {
        private static readonly string TargetActorUsage = $"{{{Lists.TargetActorSubtypes.SELF}|" +
                                                 $"{Lists.TargetActorSubtypes.PLAYER}|" +
                                                 $"{Lists.TargetActorSubtypes.REF_ACTOR}|" +
                                                 $"{Lists.TargetActorSubtypes.ACTOR_ID} actor_id|" +
                                                 $"{Lists.TargetActorSubtypes.NPCMAKER} npc_id}}";

        private static readonly string BooleanUsage = $"{{{Lists.Keyword_True}|{Lists.Keyword_False}}}";


        private static readonly Dictionary<Lists.Instructions, Dictionary<object, string>> Usages = new Dictionary<Lists.Instructions, Dictionary<object, string>>()
        {
            { Lists.Instructions.IF,  new Dictionary<object, string>()
                {
                    { Lists.IfSubTypes.FLAG_INF,                            $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_EVENT,                          $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_SWITCH,                         $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_SCENE,                          $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_TREASURE,                       $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_ROOM_CLEAR,                     $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_SCENE_COLLECT,                  $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_TEMPORARY,                      $" flag_number {BooleanUsage}" },
                    { Lists.IfSubTypes.FLAG_INTERNAL,                       $" flag_number {BooleanUsage}" },

                    { Lists.IfSubTypes.LINK_IS_ADULT,                       $" {BooleanUsage}" },
                    { Lists.IfSubTypes.IS_DAY,                              $" {BooleanUsage}" },
                    { Lists.IfSubTypes.IS_TALKING,                          $" {BooleanUsage}" },
                    { Lists.IfSubTypes.PLAYER_HAS_EMPTY_BOTTLE,             $" {BooleanUsage}" },
                    { Lists.IfSubTypes.IN_CUTSCENE,                         $" {BooleanUsage}" },
                    { Lists.IfSubTypes.TEXTBOX_ON_SCREEN,                   $" {BooleanUsage}" },
                    { Lists.IfSubTypes.TEXTBOX_DRAWING,                     $" {BooleanUsage}" },
                    { Lists.IfSubTypes.PLAYER_HAS_MAGIC,                    $" {BooleanUsage}" },
                    { Lists.IfSubTypes.ATTACKED,                            $" {BooleanUsage}" },
                    { Lists.IfSubTypes.REF_ACTOR_EXISTS,                    $" {BooleanUsage}" },
                    { Lists.IfSubTypes.PICKUP_IDLE,                         $" {BooleanUsage}" },
                    { Lists.IfSubTypes.PICKUP_PICKED_UP,                    $" {BooleanUsage}" },
                    { Lists.IfSubTypes.PICKUP_THROWN,                       $" {BooleanUsage}" },
                    { Lists.IfSubTypes.PICKUP_LANDED,                       $" {BooleanUsage}" },
                    { Lists.IfSubTypes.IS_SPEAKING,                         $" {BooleanUsage}" },

                    { Lists.IfSubTypes.PLAYER_RUPEES,                       $" operator value" },
                    { Lists.IfSubTypes.SCENE_ID,                            $" operator value" },
                    { Lists.IfSubTypes.PLAYER_SKULLTULAS,                   $" operator value" },
                    { Lists.IfSubTypes.PATH_NODE,                           $" operator value" },
                    { Lists.IfSubTypes.ANIMATION_FRAME,                     $" operator value" },
                    { Lists.IfSubTypes.CUTSCENE_FRAME,                      $" operator value" },
                    { Lists.IfSubTypes.PLAYER_HEALTH,                       $" operator value" },
                    { Lists.IfSubTypes.PLAYER_MAGIC,                        $" operator value" },
                    { Lists.IfSubTypes.PLAYER_BOMBS,                        $" operator value" },
                    { Lists.IfSubTypes.PLAYER_BOMBCHUS,                     $" operator value" },
                    { Lists.IfSubTypes.PLAYER_ARROWS,                       $" operator value" },
                    { Lists.IfSubTypes.PLAYER_DEKUNUTS,                     $" operator value" },
                    { Lists.IfSubTypes.PLAYER_DEKUSTICKS,                   $" operator value" },
                    { Lists.IfSubTypes.PLAYER_BEANS,                        $" operator value" },
                    { Lists.IfSubTypes.PLAYER_SEEDS,                        $" operator value" },
                    { Lists.IfSubTypes.EXT_VAR,                             $" npc_id variable_num operator value" },
                    { Lists.IfSubTypes.EXT_VARF,                            $" npc_id variable_num operator value" },

                    { Lists.IfSubTypes.STICK_X,                             $" operator value" },
                    { Lists.IfSubTypes.STICK_Y,                             $" operator value" },

                    { Lists.IfSubTypes.ITEM_BEING_TRADED,                   $" *trade_item_name*" },
                    { Lists.IfSubTypes.TRADE_STATUS,                        $" {GetListFromEnum(typeof(Lists.TradeStatuses))}" },
                    { Lists.IfSubTypes.PLAYER_MASK,                         $" {GetListFromEnum(typeof(Lists.PlayerMasks))}" },
                    { Lists.IfSubTypes.TIME_OF_DAY,                         $" operator HH:mm" },
                    { Lists.IfSubTypes.ANIMATION,                           $" *animation_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_INVENTORY_ITEM,           $" *inventory_item_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_QUEST_ITEM,               $" *quest_item_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_DUNGEON_ITEM,             $" *dungeon_item_name*" },
                    { Lists.IfSubTypes.BUTTON_PRESSED,                      $" {GetListFromEnum(typeof(Lists.Buttons))}" },
                    { Lists.IfSubTypes.BUTTON_HELD,                         $" {GetListFromEnum(typeof(Lists.Buttons))}" },
                    { Lists.IfSubTypes.TARGETTED,                           $" {BooleanUsage}" },

                    { Lists.IfSubTypes.DISTANCE_FROM_PLAYER,                $" operator value" },
                    { Lists.IfSubTypes.DISTANCE_FROM_REF_ACTOR,             $" operator value" },
                    { Lists.IfSubTypes.LENS_OF_TRUTH_ON,                    $" {BooleanUsage}" },
                    { Lists.IfSubTypes.DAMAGED_BY,                          $" {GetListFromEnum(typeof(Lists.DamageTypes))}" },
                    { Lists.IfSubTypes.ROOM_ID,                             $" operator value" },

                    { Lists.IfSubTypes.CURRENT_STATE,                       $" {GetListFromEnum(typeof(Lists.StateTypes))}" },
                    { Lists.IfSubTypes.CCALL,                               $" c_function_name [operator] [value]" },
                    { Lists.IfWhileAwaitSetRamSubTypes.RANDOM,              $".min_range->max_range operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL8,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL16,            $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL32,            $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBALF,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR8,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR16,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR32,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTORF,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE8,               $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE16,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE32,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVEF,               $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VAR,                 $".varnum operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VARF,                $".varnum operator value" },
                }
            },
            { Lists.Instructions.ITEM,  new Dictionary<object, string>()
                {
                    { Lists.ItemSubTypes.AWARD,                             $" *award_item_name*" },
                    { Lists.ItemSubTypes.GIVE,                              $" *inventory_item_name*" },
                    { Lists.ItemSubTypes.TAKE,                              $" *inventory_item_name*" },
                }
            },
            { Lists.Instructions.PLAY,  new Dictionary<object, string>()
                {
                    { Lists.PlaySubTypes.BGM,                               $" *bgm_name*" },
                    { Lists.PlaySubTypes.SFX,                               $" *sfx_name*" },
                    { Lists.PlaySubTypes.SFX_GLOBAL,                        $" *sfx_name*" },
                    { Lists.PlaySubTypes.CUTSCENE,                          $"" },
                    { Lists.PlaySubTypes.CUTSCENE_ID,                       $" header_id" },
                }
            },
            { Lists.Instructions.POSITION,  new Dictionary<object, string>()
                {
                    { Lists.PositionSubTypes.SET,                           $" {TargetActorUsage} x y z" },
                    { Lists.PositionSubTypes.MOVE_BY,                       $" {TargetActorUsage} x y z speed [ignore_y:{BooleanUsage}]" },
                    { Lists.PositionSubTypes.MOVE_TO,                       $" {TargetActorUsage} x y z speed [ignore_y:{BooleanUsage}]" },
                    { Lists.PositionSubTypes.DIRECTION_MOVE_BY,             $" {TargetActorUsage} x y z speed [ignore_y:{BooleanUsage}]" },
                    { Lists.PositionSubTypes.MOVE_BY_REF_ACTOR,             $" {TargetActorUsage} x y z speed [ignore_y:{BooleanUsage}]" },
                    { Lists.PositionSubTypes.DIRECTION_MOVE_BY_REF_ACTOR,   $" {TargetActorUsage} x y z speed [ignore_y:{BooleanUsage}]" },
                }
            },
            { Lists.Instructions.ROTATION,  new Dictionary<object, string>()
                {
                    { Lists.RotationSubTypes.SET,                           $" {TargetActorUsage} x y z" },
                    { Lists.RotationSubTypes.ROTATE_BY,                     $" {TargetActorUsage} x y z speed" },
                    { Lists.RotationSubTypes.ROTATE_TO,                     $" {TargetActorUsage} x y z speed" },
                }
            },
            { Lists.Instructions.SCALE,  new Dictionary<object, string>()
                {
                    { Lists.ScaleSubTypes.SET,                              $" {TargetActorUsage} scale" },
                    { Lists.ScaleSubTypes.SCALE_BY,                         $" {TargetActorUsage} scale speed" },
                    { Lists.ScaleSubTypes.SCALE_TO,                         $" {TargetActorUsage} scale speed" },
                }
            },
            { Lists.Instructions.SCRIPT,  new Dictionary<object, string>()
                {
                    { Lists.ScriptSubtypes.START,                           $" script_id" },
                    { Lists.ScriptSubtypes.STOP,                            $" script_id" },
                }
            },
            { Lists.Instructions.AWAIT,  new Dictionary<object, string>()
                {
                    { Lists.AwaitSubTypes.FLAG_INF,                         $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_EVENT,                       $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_SWITCH,                      $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_SCENE,                       $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_TREASURE,                    $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_ROOM_CLEAR,                  $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_SCENE_COLLECT,               $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_TEMPORARY,                   $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FLAG_INTERNAL,                    $" flag_number {BooleanUsage}" },
                    { Lists.AwaitSubTypes.MOVEMENT_PATH_END,                $"" },
                    { Lists.AwaitSubTypes.RESPONSE,                         $"" },
                    { Lists.AwaitSubTypes.TALKING_END,                      $"" },
                    { Lists.AwaitSubTypes.TEXTBOX_ON_SCREEN,                $" {BooleanUsage}" },
                    { Lists.AwaitSubTypes.FOREVER,                          $"" },
                    { Lists.AwaitSubTypes.PATH_NODE,                        $" node_id" },
                    { Lists.AwaitSubTypes.FRAMES,                           $" frames_num" },
                    { Lists.AwaitSubTypes.ANIMATION_FRAME,                  $" operator frame_num" },
                    { Lists.AwaitSubTypes.CUTSCENE_FRAME,                   $" operator frame_num" },
                    { Lists.AwaitSubTypes.TIME_OF_DAY,                      $" operator HH:mm" },
                    { Lists.AwaitSubTypes.STICK_X,                          $" operator value" },
                    { Lists.AwaitSubTypes.STICK_Y,                          $" operator value" },
                    { Lists.AwaitSubTypes.BUTTON_PRESSED,                   $" {GetListFromEnum(typeof(Lists.Buttons))}" },
                    { Lists.AwaitSubTypes.BUTTON_HELD,                      $" {GetListFromEnum(typeof(Lists.Buttons))}" },
                    { Lists.AwaitSubTypes.TEXTBOX_NUM,                      $" textbox_num" },
                    { Lists.AwaitSubTypes.TEXTBOX_DISMISSED,                $"" },
                    { Lists.AwaitSubTypes.TEXTBOX_DRAWING,                  $" {BooleanUsage}" },
                    { Lists.AwaitSubTypes.ANIMATION_END,                    $"" },
                    { Lists.AwaitSubTypes.PLAYER_ANIMATION_END,             $"" },
                    { Lists.AwaitSubTypes.EXT_VAR,                          $" npc_id variable_num operator value" },
                    { Lists.AwaitSubTypes.EXT_VARF,                         $" npc_id variable_num operator value" },
                    { Lists.AwaitSubTypes.CURRENT_STATE,                    $" {GetListFromEnum(typeof(Lists.StateTypes))}" },
                    { Lists.AwaitSubTypes.CCALL,                            $" c_function_name [operator] [value]" },
                    { Lists.IfWhileAwaitSetRamSubTypes.RANDOM,              $".min_range->max_range operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL8,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL16,            $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL32,            $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBALF,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR8,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR16,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR32,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTORF,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE8,               $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE16,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE32,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVEF,               $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VAR,                 $".varnum operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VARF,                $".varnum operator value" },
                }
            },
            { Lists.Instructions.SET,  new Dictionary<object, string>()
                {
                    { Lists.SetSubTypes.TARGET_LIMB,                        $" operator value" },
                    { Lists.SetSubTypes.TARGET_DISTANCE,                    $" operator value" },
                    { Lists.SetSubTypes.HEAD_LIMB,                          $" operator value" },
                    { Lists.SetSubTypes.WAIST_LIMB,                         $" operator value" },
                    { Lists.SetSubTypes.LOOKAT_TYPE,                        $" {GetListFromEnum(typeof(Lists.LookAtStyles))}" },
                    { Lists.SetSubTypes.HEAD_VERT_AXIS,                     $" operator value" },
                    { Lists.SetSubTypes.HEAD_HORIZ_AXIS,                    $" operator value" },
                    { Lists.SetSubTypes.WAIST_VERT_AXIS,                    $" operator value" },
                    { Lists.SetSubTypes.WAIST_HORIZ_AXIS,                   $" operator value" },
                    { Lists.SetSubTypes.CUTSCENE_SLOT,                      $" operator value" },
                    { Lists.SetSubTypes.BLINK_SEGMENT,                      $" {{SEG_8-F}}" },
                    { Lists.SetSubTypes.TALK_SEGMENT,                       $" {{SEG_8-F}}" },
                    { Lists.SetSubTypes.ALPHA,                              $" operator value" },

                    { Lists.SetSubTypes.MOVEMENT_DISTANCE,                  $" operator value" },
                    { Lists.SetSubTypes.MAXIMUM_ROAM,                       $" operator value" },
                    { Lists.SetSubTypes.MOVEMENT_LOOP_DELAY,                $" operator value" },
                    { Lists.SetSubTypes.ATTACKED_SFX,                       $" *sfx_name*" },
                    { Lists.SetSubTypes.LIGHT_RADIUS,                       $" operator value" },
                    { Lists.SetSubTypes.CUTSCENE_FRAME,                     $" operator value" },

                    { Lists.SetSubTypes.COLLISION_RADIUS,                   $" operator value" },
                    { Lists.SetSubTypes.COLLISION_HEIGHT,                   $" operator value" },
                    { Lists.SetSubTypes.MOVEMENT_LOOP_START,                $" operator value" },
                    { Lists.SetSubTypes.MOVEMENT_LOOP_END,                  $" operator value" },
                    { Lists.SetSubTypes.COLLISION_YOFFSET,                  $" operator value" },
                    { Lists.SetSubTypes.TARGET_OFFSET_X,                    $" operator value" },
                    { Lists.SetSubTypes.TARGET_OFFSET_Y,                    $" operator value" },
                    { Lists.SetSubTypes.TARGET_OFFSET_Z,                    $" operator value" },
                    { Lists.SetSubTypes.MODEL_OFFSET_X,                     $" operator value" },
                    { Lists.SetSubTypes.MODEL_OFFSET_Y,                     $" operator value" },
                    { Lists.SetSubTypes.MODEL_OFFSET_Z,                     $" operator value" },
                    { Lists.SetSubTypes.CAMERA_ID,                          $" operator value" },
                    { Lists.SetSubTypes.LOOKAT_OFFSET_X,                    $" operator value" },
                    { Lists.SetSubTypes.LOOKAT_OFFSET_Y,                    $" operator value" },
                    { Lists.SetSubTypes.LOOKAT_OFFSET_Z,                    $" operator value" },
                    { Lists.SetSubTypes.CURRENT_PATH_NODE,                  $" operator value" },
                    { Lists.SetSubTypes.CURRENT_ANIMATION_FRAME,            $" operator value" },
                    { Lists.SetSubTypes.LIGHT_OFFSET_X,                     $" operator value" },
                    { Lists.SetSubTypes.LIGHT_OFFSET_Y,                     $" operator value" },
                    { Lists.SetSubTypes.LIGHT_OFFSET_Z,                     $" operator value" },
                    { Lists.SetSubTypes.TIMED_PATH_START_TIME,              $" operator HH:mm" },
                    { Lists.SetSubTypes.TIMED_PATH_END_TIME,                $" operator HH:mm" },

                    { Lists.SetSubTypes.MOVEMENT_SPEED,                     $" operator value" },
                    { Lists.SetSubTypes.TALK_RADIUS,                        $" operator value" },
                    { Lists.SetSubTypes.SMOOTHING_CONSTANT,                 $" operator value" },
                    { Lists.SetSubTypes.SHADOW_RADIUS,                      $" operator value" },

                    { Lists.SetSubTypes.LOOP_MOVEMENT,                      $" {BooleanUsage}" },
                    { Lists.SetSubTypes.HAS_COLLISION,                      $" {BooleanUsage}" },
                    { Lists.SetSubTypes.DO_BLINKING_ANIMATIONS,             $" {BooleanUsage}" },
                    { Lists.SetSubTypes.DO_TALKING_ANIMATIONS,              $" {BooleanUsage}" },
                    { Lists.SetSubTypes.JUST_SCRIPT,                        $" {BooleanUsage}" },
                    { Lists.SetSubTypes.OPEN_DOORS,                         $" {BooleanUsage}" },
                    { Lists.SetSubTypes.MOVEMENT_IGNORE_Y,                  $" {BooleanUsage}" },
                    { Lists.SetSubTypes.FADES_OUT,                          $" {BooleanUsage}" },
                    { Lists.SetSubTypes.LIGHT_GLOW,                         $" {BooleanUsage}" },
                    { Lists.SetSubTypes.PAUSE_CUTSCENE,                     $" {BooleanUsage}" },
                    { Lists.SetSubTypes.INVISIBLE,                          $" {BooleanUsage}" },
                    { Lists.SetSubTypes.TALK_PERSIST,                       $" {BooleanUsage}" },
                    { Lists.SetSubTypes.CASTS_SHADOW,                       $" {BooleanUsage}" },
                    { Lists.SetSubTypes.NO_AUTO_ANIM,                       $" {BooleanUsage}" },
                    { Lists.SetSubTypes.TALK_MODE,                          $" {BooleanUsage}" },


                    { Lists.SetSubTypes.PLAYER_BOMBS,                       $" operator value" },
                    { Lists.SetSubTypes.PLAYER_BOMBCHUS,                    $" operator value" },
                    { Lists.SetSubTypes.PLAYER_ARROWS,                      $" operator value" },
                    { Lists.SetSubTypes.PLAYER_DEKUNUTS,                    $" operator value" },
                    { Lists.SetSubTypes.PLAYER_DEKUSTICKS,                  $" operator value" },
                    { Lists.SetSubTypes.PLAYER_BEANS,                       $" operator value" },
                    { Lists.SetSubTypes.PLAYER_SEEDS,                       $" operator value" },
                    { Lists.SetSubTypes.PLAYER_RUPEES,                      $" operator value" },
                    { Lists.SetSubTypes.PLAYER_HEALTH,                      $" operator value" },
                    { Lists.SetSubTypes.PLAYER_MAGIC,                       $" operator value" },

                    { Lists.SetSubTypes.ENV_COLOR,                          $" r g b" },
                    { Lists.SetSubTypes.LIGHT_COLOR,                        $" r g b" },

                    { Lists.SetSubTypes.RESPONSE_ACTIONS,                   $" label_option1 label_option2 [label_option3]" },

                    { Lists.SetSubTypes.ANIMATION_OBJECT,                   $" animation_name operator value" },
                    { Lists.SetSubTypes.ANIMATION_OFFSET,                   $" animation_name operator value" },
                    { Lists.SetSubTypes.ANIMATION_STARTFRAME,               $" animation_name operator value" },
                    { Lists.SetSubTypes.ANIMATION_ENDFRAME,                 $" animation_name operator value" },
                    { Lists.SetSubTypes.ANIMATION_SPEED,                    $" animation_name operator value" },
                    { Lists.SetSubTypes.FLAG_INF,                           $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_EVENT,                         $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_SWITCH,                        $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_SCENE,                         $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_TREASURE,                      $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_ROOM_CLEAR,                    $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_SCENE_COLLECT,                 $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_TEMPORARY,                     $" flag_id {BooleanUsage}" },
                    { Lists.SetSubTypes.FLAG_INTERNAL,                      $" flag_id {BooleanUsage}" },

                    { Lists.SetSubTypes.MASS,                               $" operator value" },

                    { Lists.SetSubTypes.PRESS_SWITCHES,                     $" {BooleanUsage}" },
                    { Lists.SetSubTypes.IS_TARGETTABLE,                     $" {BooleanUsage}" },
                    { Lists.SetSubTypes.AFFECTED_BY_LENS,                   $" {BooleanUsage}" },
                    { Lists.SetSubTypes.IS_ALWAYS_ACTIVE,                   $" {BooleanUsage}" },
                    { Lists.SetSubTypes.IS_ALWAYS_DRAWN,                    $" {BooleanUsage}" },
                    { Lists.SetSubTypes.REACTS_IF_ATTACKED,                 $" {BooleanUsage}" },
                    { Lists.SetSubTypes.EXISTS_IN_ALL_ROOMS,                $" {BooleanUsage}" },
                    { Lists.SetSubTypes.IS_SPEAKING,                        $" {BooleanUsage}" },

                    { Lists.SetSubTypes.GRAVITY_FORCE,                      $" operator value" },
                    { Lists.SetSubTypes.MOVEMENT_PATH_ID,                   $" operator value" },
                    { Lists.SetSubTypes.PLAYER_CAN_MOVE,                    $" {BooleanUsage}" },
                    { Lists.SetSubTypes.ACTOR_CAN_MOVE,                     $" {BooleanUsage}" },
                    { Lists.SetSubTypes.ANIMATION,                          $" *animation_name* [\"once\"]" },
                    { Lists.SetSubTypes.ANIMATION_INSTANTLY,                $" *animation_name* [\"once\"]" },
                    { Lists.SetSubTypes.SCRIPT_START,                       $" label" },
                    { Lists.SetSubTypes.BLINK_PATTERN,                      $" *texture_1* [*texture_2*] [*texture_3*] [*texture_4*]" },
                    { Lists.SetSubTypes.TALK_PATTERN,                       $" *texture_1* [*texture_2*] [*texture_3*] [*texture_4*]" },
                    { Lists.SetSubTypes.SEGMENT_ENTRY,                      $" {{SEG_8-F}} *texture_name*" },
                    { Lists.SetSubTypes.DLIST_VISIBILITY,                   $" {GetListFromEnum(typeof(Lists.DListVisibilityOptions))}" },
                    { Lists.SetSubTypes.CAMERA_TRACKING_ON,                 $" {TargetActorUsage}" },
                    { Lists.SetSubTypes.EXT_VAR,                            $" actor_id var_num operator value" },
                    { Lists.SetSubTypes.EXT_VARF,                           $" actor_id var_num operator value" },
                    { Lists.SetSubTypes.TIME_OF_DAY,                        $" operator HH:mm" },
                    { Lists.SetSubTypes.ATTACKED_EFFECT,                    $" {GetListFromEnum(typeof(Lists.EffectsIfAttacked))}" },
                    { Lists.SetSubTypes.MOVEMENT_TYPE,                      $" {GetListFromEnum(typeof(Lists.MovementStyles))}" },
                    { Lists.SetSubTypes.GENERATES_LIGHT,                    $" {BooleanUsage}" },
                    { Lists.SetSubTypes.REF_ACTOR,                          $" {TargetActorUsage}" },
                    { Lists.SetSubTypes.PLAYER_ANIMATION,                   $" *link_animation_name* speed [start_frame] [end_frame] [\"once\"] " },
                    { Lists.SetSubTypes.PLAYER_ANIMATE_MODE,                $" {BooleanUsage}" },

                    { Lists.SetSubTypes.DLIST_COLOR,                        $" *ex_dlist_name* r g b" },
                    { Lists.SetSubTypes.DLIST_OFFSET,                       $" *ex_dlist_name* 0xOffset" },

                    { Lists.SetSubTypes.DLIST_TRANS_X,                      $" *ex_dlist_name* operator value" },
                    { Lists.SetSubTypes.DLIST_TRANS_Y,                      $" *ex_dlist_name* operator value" },
                    { Lists.SetSubTypes.DLIST_TRANS_Z,                      $" *ex_dlist_name* operator value" },
                    { Lists.SetSubTypes.DLIST_SCALE,                        $" *ex_dlist_name* operator value" },

                    { Lists.SetSubTypes.DLIST_ROT_X,                        $" *ex_dlist_name* operator value" },
                    { Lists.SetSubTypes.DLIST_ROT_Y,                        $" *ex_dlist_name* operator value" },
                    { Lists.SetSubTypes.DLIST_ROT_Z,                        $" *ex_dlist_name* operator value" },

                    { Lists.SetSubTypes.DLIST_LIMB,                         $" *ex_dlist_name* operator value" },
                    { Lists.SetSubTypes.DLIST_OBJECT,                       $" *ex_dlist_name* operator value" },

                    { Lists.SetSubTypes.RAM,                                $" {{Data size:8|16|32}} 0xOffset value" },

                    { Lists.SetSubTypes.LABEL_TO_VAR,                       $" label out_variable" },
                    { Lists.SetSubTypes.LABEL_TO_VARF,                      $" label out_variable" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL8,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL16,            $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL32,            $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBALF,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR8,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR16,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR32,             $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTORF,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE8,               $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE16,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE32,              $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVEF,               $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VAR,                 $".varnum operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VARF,                $".varnum operator value" },
                }
            },
            { Lists.Instructions.GET, new Dictionary<object, string>()
                {
                    {Lists.GetSubTypes.EXT_VAR, $" out_variable actor_id var_num" },
                    {Lists.GetSubTypes.EXT_VARF, $" out_variable actor_id var_num" },
                }
            },
        };



        private static readonly Dictionary<Lists.ParticleSubOptions, string> ParticleSubOptionUsages = new Dictionary<Lists.ParticleSubOptions, string>()
        {
            {Lists.ParticleSubOptions.POSITION,                     $"[{Lists.ParticleSubOptions.POSITION} {GetListFromEnum(typeof(Lists.SpawnPosParams))} x y z]" },
            {Lists.ParticleSubOptions.VELOCITY,                     $"[{Lists.ParticleSubOptions.VELOCITY} x y z]" },
            {Lists.ParticleSubOptions.ACCELERATION,                 $"[{Lists.ParticleSubOptions.ACCELERATION} x y z]" },
            {Lists.ParticleSubOptions.COLOR1,                       $"[{Lists.ParticleSubOptions.COLOR1} r g b a]" },
            {Lists.ParticleSubOptions.COLOR2,                       $"[{Lists.ParticleSubOptions.COLOR2} r g b a]" },
            {Lists.ParticleSubOptions.SCALE,                        $"[{Lists.ParticleSubOptions.SCALE} value]" },
            {Lists.ParticleSubOptions.SCALE_UPDATE,                 $"[{Lists.ParticleSubOptions.SCALE_UPDATE} value]" },
            {Lists.ParticleSubOptions.SCALE_UPDATE_DOWN,            $"[{Lists.ParticleSubOptions.SCALE_UPDATE_DOWN} value]" },
            {Lists.ParticleSubOptions.OPACITY,                      $"[{Lists.ParticleSubOptions.OPACITY} value]" },
            {Lists.ParticleSubOptions.RANDOMIZE_XZ,                 $"[{Lists.ParticleSubOptions.RANDOMIZE_XZ} {BooleanUsage}]" },
            {Lists.ParticleSubOptions.SCORE_AMOUNT,                 $"[{Lists.ParticleSubOptions.SCORE_AMOUNT} {{0|1|2}}]" },
            {Lists.ParticleSubOptions.COUNT,                        $"[{Lists.ParticleSubOptions.COUNT} value]" },
            {Lists.ParticleSubOptions.LIGHTPOINT_COLOR,             $"[{Lists.ParticleSubOptions.LIGHTPOINT_COLOR} {{WHITE|BLUE|RED|YELLOW|PURPLE|PINK|ORANGE|GRAY}}]" },
            {Lists.ParticleSubOptions.FADE_DELAY,                   $"[{Lists.ParticleSubOptions.FADE_DELAY} value]" },
            {Lists.ParticleSubOptions.DURATION,                     $"[{Lists.ParticleSubOptions.DURATION} value]" },
            {Lists.ParticleSubOptions.YAW,                          $"[{Lists.ParticleSubOptions.YAW} value]" },
            {Lists.ParticleSubOptions.DLIST,                        $"[{Lists.ParticleSubOptions.DLIST} *extra_dlist_name*]" },
            {Lists.ParticleSubOptions.SPOTTED,                      $"[{Lists.ParticleSubOptions.SPOTTED} label]" },
            {Lists.ParticleSubOptions.VARIABLE,                     $"[{Lists.ParticleSubOptions.VARIABLE} value]" },
        };

        public static string GetUsage(Lists.Instructions Instruction, string SubType)
        {
            try
            {
                switch (Instruction)
                {
                    case Lists.Instructions.IF:
                    case Lists.Instructions.WHILE:
                        {
                            bool res = Enum.TryParse<Lists.IfSubTypes>(SubType, out Lists.IfSubTypes oSubType);

                            if (res)
                            {
                                return Environment.NewLine + $"{Instruction} {oSubType}{Usages[Lists.Instructions.IF][oSubType]}" + Environment.NewLine +
                                                             $"   ~instructions~ " + Environment.NewLine +
                                                             $"END{Instruction}";
                            }
                            else
                            {
                                if (SubType.Contains('.'))
                                    SubType = SubType.Substring(0, SubType.IndexOf("."));


                                bool res2 = Enum.TryParse(SubType, out Lists.IfWhileAwaitSetRamSubTypes oSubType2);

                                if (res2)
                                {
                                    return Environment.NewLine + $"{Instruction} {oSubType2}{Usages[Lists.Instructions.IF][oSubType2]}" + Environment.NewLine +
                                                                 $"   ~instructions~ " + Environment.NewLine +
                                                                 $"END{Instruction}";
                                }
                                else
                                {
                                    return Environment.NewLine + $"{Instruction} *subtype*" + Environment.NewLine +
                                            $"   ~instructions~ " + Environment.NewLine +
                                            $"END{Instruction}";
                                }
                            }
                        }
                    case Lists.Instructions.FACE:
                        {
                            return $"{Lists.Instructions.FACE} {TargetActorUsage} " +
                                                               $"{GetListFromEnum(typeof(Lists.FaceSubtypes))} " +
                                                               $"{TargetActorUsage}";
                        }
                    case Lists.Instructions.CCALL:
                        return $"{Lists.Instructions.CCALL} c_function_name [out_variable]";
                    case Lists.Instructions.CLOSE_TEXTBOX:
                        return $"{Lists.Instructions.CLOSE_TEXTBOX}";
                    case Lists.Instructions.FADEIN:
                        return $"{Lists.Instructions.FADEIN} fadein_rate";
                    case Lists.Instructions.FADEOUT:
                        return $"{Lists.Instructions.FADEOUT} r g b fadeout_rate";
                    case Lists.Instructions.FORCE_TALK:
                        return $"{Lists.Instructions.FORCE_TALK} textbox [textbox_child]";
                    case Lists.Instructions.GOTO:
                        return $"{Lists.Instructions.GOTO} label";
                    case Lists.Instructions.GOTO_VAR:
                        return $"{Lists.Instructions.GOTO} variable";
                    case Lists.Instructions.SHOW_TEXTBOX:
                        return $"{Lists.Instructions.SHOW_TEXTBOX} textbox [textbox_child]";
                    case Lists.Instructions.SHOW_TEXTBOX_SP:
                        return $"{Lists.Instructions.SHOW_TEXTBOX} textbox [textbox_child]";
                    case Lists.Instructions.WARP:
                        return $"{Lists.Instructions.WARP} route_id [scene_load_flag] [next_cutscene_index]";
                    case Lists.Instructions.SPAWN:
                        {
                            return Environment.NewLine + $"{Lists.Instructions.SPAWN} actor_id" + Environment.NewLine +
                                   $"   [VARIABLE value]" + Environment.NewLine +
                                   $"   [POSITION {GetListFromEnum(typeof(Lists.EffectsIfAttacked))} x y z]" + Environment.NewLine +
                                   $"   [ROTATION x y z]" + Environment.NewLine +
                                   $"   SET_AS_REF" + Environment.NewLine +
                                   $"END{Lists.Instructions.SPAWN}";
                        }
                    case Lists.Instructions.ITEM:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.ItemSubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.ITEM} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                                return $"{Lists.Instructions.ITEM} *item_subtype* ..";
                        }
                    case Lists.Instructions.PLAY:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.PlaySubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.PLAY} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                                return $"{Lists.Instructions.PLAY} *play_subtype* ..";
                        }
                    case Lists.Instructions.QUAKE:
                        return $"{Lists.Instructions.QUAKE} *quake_type_name* speed duration";
                    case Lists.Instructions.OCARINA:
                        {
                            return Environment.NewLine + $"{Lists.Instructions.OCARINA} song_name" + Environment.NewLine +
                                   $"   ~instructions~ " + Environment.NewLine +
                                   $"END{Lists.Instructions.OCARINA}";
                        }
                    case Lists.Instructions.TALK:
                        {
                            return Environment.NewLine + $"{Lists.Instructions.TALK} message_name [message_name_child]" + Environment.NewLine +
                                   $"   ~instructions~ " + Environment.NewLine +
                                   $"END{Lists.Instructions.TALK}";
                        }
                    case Lists.Instructions.KILL:
                        {
                            if (SubType != "")
                                return $"{Lists.Instructions.KILL} {SubType}";
                            else
                                return $"{Lists.Instructions.KILL} {TargetActorUsage}";
                        }
                    case Lists.Instructions.PICKUP:
                        return $"{Lists.Instructions.PICKUP}";
                    case Lists.Instructions.PARTICLE:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.ParticleTypes oSubType);

                            if (!res)
                            {
                                string Out = $"{Lists.Instructions.PARTICLE} {SubType}" + Environment.NewLine;

                                foreach (Lists.ParticleSubOptions sub in Enum.GetValues(typeof(Lists.ParticleSubOptions)))
                                    Out += $"   {ParticleSubOptionUsages[sub]}" + Environment.NewLine;

                                Out += $"END{Lists.Instructions.PARTICLE}";
                                return Out;
                            }
                            else
                            {
                                string Out = $"{Lists.Instructions.PARTICLE} {SubType}" + Environment.NewLine;

                                foreach (Lists.ParticleSubOptions sub in Enum.GetValues(typeof(Lists.ParticleSubOptions)))
                                {
                                    if (Dicts.UsableParticleSubOptions[oSubType].Contains(sub))
                                        Out += $"   {ParticleSubOptionUsages[sub]}" + Environment.NewLine;
                                }

                                Out += $"END{Lists.Instructions.PARTICLE}";
                                return Out;
                            }
                        }
                    case Lists.Instructions.POSITION:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.PositionSubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.POSITION} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                                return $"{Lists.Instructions.POSITION} *position_subtype* ..";
                        }
                    case Lists.Instructions.ROTATION:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.RotationSubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.ROTATION} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                                return $"{Lists.Instructions.ROTATION} *rotation_subtype* ..";
                        }
                    case Lists.Instructions.SCALE:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.ScaleSubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.SCALE} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                                return $"{Lists.Instructions.SCALE} *scale_subtype* ..";
                        }
                    case Lists.Instructions.SCRIPT:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.ScriptSubtypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.SCRIPT} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                                return $"{Lists.Instructions.SCRIPT} *script_subtype* ..";
                        }
                    case Lists.Instructions.TRADE:
                        {
                            return $"{Lists.Instructions.TRADE} *trade_item_name* " + Environment.NewLine +
                                                               $"   {Lists.Keyword_TradeSucccess} message_name [textbox_child]" + Environment.NewLine +
                                                               $"   {Lists.Keyword_TradeFailure}" + Environment.NewLine +
                                                               $"       [*trade_item_name* message_name [textbox_child]]" + Environment.NewLine +
                                                               $"       {Lists.Keyword_TradeDefault} message_name [textbox_child]" + Environment.NewLine +
                                                               $"   {Lists.Keyword_EndTradeFailure}" + Environment.NewLine +
                                                               $"   {Lists.Keyword_TradeNone} message_name [textbox_child]" + Environment.NewLine +
                                                               $"END{Lists.Instructions.TRADE}";

                        }
                    case Lists.Instructions.SAVE:
                        return $"{Lists.Instructions.SAVE}";
                    case Lists.Instructions.RETURN:
                        return $"{Lists.Instructions.RETURN}";
                    case Lists.Instructions.AWAIT:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.AwaitSubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.AWAIT} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                            {
                                if (SubType.Contains('.'))
                                    SubType = SubType.Substring(0, SubType.IndexOf("."));


                                bool res2 = Enum.TryParse(SubType, out Lists.IfWhileAwaitSetRamSubTypes oSubType2);

                                if (res2)
                                    return $"{Lists.Instructions.AWAIT} {oSubType2}{Usages[Instruction][oSubType2]}";
                                else
                                    return $"{Lists.Instructions.AWAIT} *subtype*";
                            }
                        }
                    case Lists.Instructions.SET:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.SetSubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.SET} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                            {
                                if (SubType.Contains('.'))
                                    SubType = SubType.Substring(0, SubType.IndexOf("."));


                                bool res2 = Enum.TryParse(SubType, out Lists.IfWhileAwaitSetRamSubTypes oSubType2);

                                if (res2)
                                    return $"{Lists.Instructions.SET} {oSubType2}{Usages[Instruction][oSubType2]}";
                                else
                                    return $"{Lists.Instructions.SET} *subtype*";
                            }
                        }
                    case Lists.Instructions.GET:
                        {
                            bool res = Enum.TryParse(SubType, out Lists.GetSubTypes oSubType);

                            if (res)
                                return $"{Lists.Instructions.GET} {oSubType}{Usages[Instruction][oSubType]}";
                            else
                                return $"{Lists.Instructions.GET} *subtype*";

                        }
                    default:
                        return "";
                }
            }
            catch (Exception)
            {
                return "Error retrieving function usage.";
            }
        }

        private static string GetListFromEnum(Type t)
        {
            string outs = "{";

            foreach (var s in Enum.GetNames(t))
                outs += s + "|";

            outs = outs.Trim('|');
            outs += "}";
            return outs;
        }
    }
}
