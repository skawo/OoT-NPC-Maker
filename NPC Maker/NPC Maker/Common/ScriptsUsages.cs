using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class ScriptsUsages
    {
        private static string TargetActorUsage = $"{{{Lists.TargetActorSubtypes.SELF}|" +
                                                 $"{Lists.TargetActorSubtypes.PLAYER}|" +
                                                 $"{Lists.TargetActorSubtypes.REF_ACTOR}|" +
                                                 $"{Lists.TargetActorSubtypes.ACTOR_ID} actor_id|" +
                                                 $"{Lists.TargetActorSubtypes.NPCMAKER} npc_id}}";

        private static string BooleanUsage = $"{{{Lists.Keyword_True}|{Lists.Keyword_False}}}";

        private static Dictionary<Lists.Instructions, Dictionary<object, string>> Usages = new Dictionary<Lists.Instructions, Dictionary<object, string>>()
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

                    { Lists.IfSubTypes.PLAYER_RUPEES,                       $" operator value" },
                    { Lists.IfSubTypes.SCENE_ID,                            $" operator value" },
                    { Lists.IfSubTypes.PLAYER_SKULLTULAS,                   $" operator value" },
                    { Lists.IfSubTypes.PATH_NODE,                           $" operator value" },
                    { Lists.IfSubTypes.ANIMATION_FRAME,                     $" operator value" },
                    { Lists.IfSubTypes.CUTSCENE_FRAME,                      $" operator value" },
                    { Lists.IfSubTypes.PLAYER_HEALTH,                       $" operator value" },
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
                    { Lists.IfSubTypes.TRADE_STATUS,                        $" {{{Lists.Keyword_TradeSucccess}|{Lists.Keyword_TradeFailure}|{Lists.Keyword_TradeNone}}}" },
                    { Lists.IfSubTypes.PLAYER_MASK,                         $" *mask_name*" },
                    { Lists.IfSubTypes.TIME_OF_DAY,                         $" operator HH:mm" },
                    { Lists.IfSubTypes.ANIMATION,                           $" *animation_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_INVENTORY_ITEM,           $" *inventory_item_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_QUEST_ITEM,               $" *quest_item_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_DUNGEON_ITEM,             $" *dungeon_item_name*" },
                    { Lists.IfSubTypes.BUTTON_PRESSED,                      $" *button_name*" },
                    { Lists.IfSubTypes.BUTTON_HELD,                         $" *button_name*" },
                    { Lists.IfSubTypes.TARGETTED,                           $" {BooleanUsage}" },

                    { Lists.IfSubTypes.DISTANCE_FROM_PLAYER,                $" operator value" },
                    { Lists.IfSubTypes.DISTANCE_FROM_REF_ACTOR,             $" operator value" },
                    { Lists.IfSubTypes.LENS_OF_TRUTH_ON,                    $" {BooleanUsage}" },
                    { Lists.IfSubTypes.DAMAGED_BY,                          $" *damage_type_name*" },
                    { Lists.IfSubTypes.ROOM_ID,                             $" operator value" },

                    { Lists.IfSubTypes.CURRENT_STATE,                       $" *state_name*" },
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
                    { Lists.AwaitSubTypes.BUTTON_PRESSED,                   $" *button_name*" },
                    { Lists.AwaitSubTypes.BUTTON_HELD,                      $" *button_name*" },
                    { Lists.AwaitSubTypes.TEXTBOX_NUM,                      $" textbox_num" },
                    { Lists.AwaitSubTypes.TEXTBOX_DISMISSED,                $"" },
                    { Lists.AwaitSubTypes.TEXTBOX_DRAWING,                  $" {BooleanUsage}" },
                    { Lists.AwaitSubTypes.ANIMATION_END,                    $"" },
                    { Lists.AwaitSubTypes.PLAYER_ANIMATION_END,             $"" },
                    { Lists.AwaitSubTypes.EXT_VAR,                          $" npc_id variable_num operator value" },
                    { Lists.AwaitSubTypes.EXT_VARF,                         $" npc_id variable_num operator value" },
                    { Lists.AwaitSubTypes.CURRENT_STATE,                    $" *state_name*" },
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
        };

        private static Dictionary<Lists.ParticleSubOptions, string> ParticleSubOptionUsages = new Dictionary<Lists.ParticleSubOptions, string>()
        {
            {Lists.ParticleSubOptions.POSITION,                     $"[{Lists.ParticleSubOptions.POSITION} {{{Lists.SpawnPosParams.ABSOLUTE}|{Lists.SpawnPosParams.RELATIVE}|{Lists.SpawnPosParams.DIRECTION}|{Lists.SpawnPosParams.DIRECTION_REF_ACTOR}|{Lists.SpawnPosParams.RELATIVE_REF_ACTOR}}} x y z]" },
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
        };

        public static string GetUsage(Lists.Instructions Instruction, string SubType)
        {
            switch (Instruction)
            {
                case Lists.Instructions.IF:
                case Lists.Instructions.WHILE:
                    {
                        bool res = Enum.TryParse<Lists.IfSubTypes>(SubType, out Lists.IfSubTypes oSubType);

                        if (res)
                        {
                            return Environment.NewLine + $"{Instruction} {oSubType}{Usages[Instruction][oSubType]}" + Environment.NewLine +
                                                         $"   ~instructions~ " + Environment.NewLine +
                                                         $"END{Instruction}";
                        }
                        else
                        {
                            if (SubType.Contains('.'))
                                SubType = SubType.Substring(0, SubType.IndexOf("."));


                            bool res2 = Enum.TryParse<Lists.IfWhileAwaitSetRamSubTypes>(SubType, out Lists.IfWhileAwaitSetRamSubTypes oSubType2);

                            if (res2)
                            {
                                return Environment.NewLine + $"{Instruction} {oSubType2}{Usages[Instruction][oSubType2]}" + Environment.NewLine +
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
                                                           $"{{{Lists.FaceSubtypes.TOWARDS}|{Lists.FaceSubtypes.AWAY_FROM}|{Lists.FaceSubtypes.TOGETHER_WITH}}} " +
                                                           $"{TargetActorUsage}";
                    }
                case Lists.Instructions.CCALL:
                    return $"{Lists.Instructions.CCALL} c_function_name";
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
                               $"   [POSITION {{{Lists.SpawnPosParams.ABSOLUTE}|{Lists.SpawnPosParams.RELATIVE}|{Lists.SpawnPosParams.DIRECTION}|{Lists.SpawnPosParams.DIRECTION_REF_ACTOR}|{Lists.SpawnPosParams.RELATIVE_REF_ACTOR}}} x y z]" + Environment.NewLine +
                               $"   [ROTATION x y z]" + Environment.NewLine +
                               $"   [SET_AS_REF {{{Lists.Keyword_True}|{Lists.Keyword_False}}}]" + Environment.NewLine +
                               $"END{Lists.Instructions.SPAWN}";
                    }
                case Lists.Instructions.ITEM:
                    {
                        bool res = Enum.TryParse<Lists.ItemSubTypes>(SubType, out Lists.ItemSubTypes oSubType);

                        if (res)
                            return $"{Lists.Instructions.ITEM} {oSubType}{Usages[Instruction][oSubType]}";
                        else
                            return $"{Lists.Instructions.ITEM} *item_subtype* ..";
                    }
                case Lists.Instructions.PLAY:
                    {
                        bool res = Enum.TryParse<Lists.PlaySubTypes>(SubType, out Lists.PlaySubTypes oSubType);

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
                        return Environment.NewLine + $"{Lists.Instructions.TALK} textbox_name [textbox_name_child]" + Environment.NewLine +
                               $"   ~instructions~ " + Environment.NewLine +
                               $"END{Lists.Instructions.TALK}";
                    }
                case Lists.Instructions.KILL:
                    {
                        return $"{Lists.Instructions.KILL} {TargetActorUsage}";
                    }
                case Lists.Instructions.PICKUP:
                    return $"{Lists.Instructions.PICKUP}";
                case Lists.Instructions.PARTICLE:
                    {
                        bool res = Enum.TryParse<Lists.ParticleTypes>(SubType, out Lists.ParticleTypes oSubType);

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
                        bool res = Enum.TryParse<Lists.PositionSubTypes>(SubType, out Lists.PositionSubTypes oSubType);

                        if (res)
                            return $"{Lists.Instructions.POSITION} {oSubType}{Usages[Instruction][oSubType]}";
                        else
                            return $"{Lists.Instructions.POSITION} *position_subtype* ..";
                    }
                case Lists.Instructions.ROTATION:
                    {
                        bool res = Enum.TryParse<Lists.RotationSubTypes>(SubType, out Lists.RotationSubTypes oSubType);

                        if (res)
                            return $"{Lists.Instructions.ROTATION} {oSubType}{Usages[Instruction][oSubType]}";
                        else
                            return $"{Lists.Instructions.ROTATION} *rotation_subtype* ..";
                    }
                case Lists.Instructions.SCALE:
                    {
                        bool res = Enum.TryParse<Lists.ScaleSubTypes>(SubType, out Lists.ScaleSubTypes oSubType);

                        if (res)
                            return $"{Lists.Instructions.SCALE} {oSubType}{Usages[Instruction][oSubType]}";
                        else
                            return $"{Lists.Instructions.SCALE} *scale_subtype* ..";
                    }
                case Lists.Instructions.SCRIPT:
                    {
                        bool res = Enum.TryParse<Lists.ScriptSubtypes>(SubType, out Lists.ScriptSubtypes oSubType);

                        if (res)
                            return $"{Lists.Instructions.SCRIPT} {oSubType}{Usages[Instruction][oSubType]}";
                        else
                            return $"{Lists.Instructions.SCRIPT} *script_subtype* ..";
                    }
                case Lists.Instructions.TRADE:
                    {
                        return $"{Lists.Instructions.TRADE} *trade_item_name* " + Environment.NewLine +
                                                           $"   {Lists.Keyword_TradeSucccess} textbox_name [textbox_child]" + Environment.NewLine + 
                                                           $"   {Lists.Keyword_TradeFailure}" + Environment.NewLine +
                                                           $"       [*trade_item_name* textbox_name [textbox_child]]" + Environment.NewLine +
                                                           $"       {Lists.Keyword_TradeDefault} textbox_name [textbox_child]" + Environment.NewLine +
                                                           $"   {Lists.Keyword_EndTradeFailure}" + Environment.NewLine +
                                                           $"   {Lists.Keyword_TradeNone} textbox_name [textbox_child]" + Environment.NewLine +
                                                           $"END{Lists.Instructions.TRADE}";

                    }
                case Lists.Instructions.SAVE:
                    return $"{Lists.Instructions.SAVE}";
                case Lists.Instructions.RETURN:
                    return $"{Lists.Instructions.RETURN}";
                case Lists.Instructions.AWAIT:
                    {
                        bool res = Enum.TryParse<Lists.AwaitSubTypes>(SubType, out Lists.AwaitSubTypes oSubType);

                        if (res)
                            return $"{Lists.Instructions.AWAIT} {oSubType}{Usages[Instruction][oSubType]}";
                        else
                        {
                            if (SubType.Contains('.'))
                                SubType = SubType.Substring(0, SubType.IndexOf("."));


                            bool res2 = Enum.TryParse<Lists.IfWhileAwaitSetRamSubTypes>(SubType, out Lists.IfWhileAwaitSetRamSubTypes oSubType2);

                            if (res2)
                                return $"{Lists.Instructions.AWAIT} {oSubType2}{Usages[Instruction][oSubType2]}";
                            else
                                return $"{Lists.Instructions.AWAIT} *subtype*";
                        }
                    }
                default:
                    return "";
            }
        }
    }
}
