using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class ScriptsUsages
    {
        public static Dictionary<Lists.Instructions, Dictionary<object, string>> Usages = new Dictionary<Lists.Instructions, Dictionary<object, string>>()
        {
            { Lists.Instructions.IF,  new Dictionary<object, string>()
                {
                    { Lists.IfSubTypes.FLAG_INF,                        $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_EVENT,                      $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_SWITCH,                     $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_SCENE,                      $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_TREASURE,                   $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_ROOM_CLEAR,                 $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_SCENE_COLLECT,              $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_TEMPORARY,                  $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.FLAG_INTERNAL,                   $" flag_number {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },

                    { Lists.IfSubTypes.LINK_IS_ADULT,                   $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.IS_DAY,                          $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.IS_TALKING,                      $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.PLAYER_HAS_EMPTY_BOTTLE,         $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.IN_CUTSCENE,                     $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.TEXTBOX_ON_SCREEN,               $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.TEXTBOX_DRAWING,                 $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.PLAYER_HAS_MAGIC,                $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.ATTACKED,                        $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.REF_ACTOR_EXISTS,                $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.PICKUP_IDLE,                     $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.PICKUP_PICKED_UP,                $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.PICKUP_THROWN,                   $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.PICKUP_LANDED,                   $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },

                    { Lists.IfSubTypes.PLAYER_RUPEES,                   $" operator value" },
                    { Lists.IfSubTypes.SCENE_ID,                        $" operator value" },
                    { Lists.IfSubTypes.PLAYER_SKULLTULAS,               $" operator value" },
                    { Lists.IfSubTypes.PATH_NODE,                       $" operator value" },
                    { Lists.IfSubTypes.ANIMATION_FRAME,                 $" operator value" },
                    { Lists.IfSubTypes.CUTSCENE_FRAME,                  $" operator value" },
                    { Lists.IfSubTypes.PLAYER_HEALTH,                   $" operator value" },
                    { Lists.IfSubTypes.PLAYER_BOMBS,                    $" operator value" },
                    { Lists.IfSubTypes.PLAYER_BOMBCHUS,                 $" operator value" },
                    { Lists.IfSubTypes.PLAYER_ARROWS,                   $" operator value" },
                    { Lists.IfSubTypes.PLAYER_DEKUNUTS,                 $" operator value" },
                    { Lists.IfSubTypes.PLAYER_DEKUSTICKS,               $" operator value" },
                    { Lists.IfSubTypes.PLAYER_BEANS,                    $" operator value" },
                    { Lists.IfSubTypes.PLAYER_SEEDS,                    $" operator value" },
                    { Lists.IfSubTypes.EXT_VAR,                         $" npc_id variable_num operator value" },
                    { Lists.IfSubTypes.EXT_VARF,                        $" npc_id variable_num operator value" },

                    { Lists.IfSubTypes.STICK_X,                         $" operator value" },
                    { Lists.IfSubTypes.STICK_Y,                         $" operator value" },

                    { Lists.IfSubTypes.ITEM_BEING_TRADED,               $" *trade_item_name*" },
                    { Lists.IfSubTypes.TRADE_STATUS,                    $" {{{Lists.Keyword_TradeSucccess}|{Lists.Keyword_TradeFailure}|{Lists.Keyword_TradeNone}}}" },
                    { Lists.IfSubTypes.PLAYER_MASK,                     $" *mask_name*" },
                    { Lists.IfSubTypes.TIME_OF_DAY,                     $" operator HH:mm" },
                    { Lists.IfSubTypes.ANIMATION,                       $" *animation_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_INVENTORY_ITEM,       $" *inventory_item_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_QUEST_ITEM,           $" *quest_item_name*" },
                    { Lists.IfSubTypes.PLAYER_HAS_DUNGEON_ITEM,         $" *dungeon_item_name*" },
                    { Lists.IfSubTypes.BUTTON_PRESSED,                  $" *button_name*" },
                    { Lists.IfSubTypes.BUTTON_HELD,                     $" *button_name*" },
                    { Lists.IfSubTypes.TARGETTED,                       $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },

                    { Lists.IfSubTypes.DISTANCE_FROM_PLAYER,            $" operator value" },
                    { Lists.IfSubTypes.DISTANCE_FROM_REF_ACTOR,         $" operator value" },
                    { Lists.IfSubTypes.LENS_OF_TRUTH_ON,                $" {{{Lists.Keyword_True}|{Lists.Keyword_False}}}" },
                    { Lists.IfSubTypes.DAMAGED_BY,                      $" *damage_type_name*" },
                    { Lists.IfSubTypes.ROOM_ID,                         $" operator value" },

                    { Lists.IfSubTypes.CURRENT_STATE,                   $" state_name" },
                    { Lists.IfSubTypes.CCALL,                           $" c_function_name [operator] [value]" },
                    { Lists.IfWhileAwaitSetRamSubTypes.RANDOM,          $".min_range->max_range operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL8,         $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL16,        $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBAL32,        $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.GLOBALF,         $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR8,          $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR16,         $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTOR32,         $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.ACTORF,          $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE8,           $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE16,          $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVE32,          $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.SAVEF,           $".0xoffset operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VAR,             $".varnum operator value" },
                    { Lists.IfWhileAwaitSetRamSubTypes.VARF,            $".varnum operator value" },
                }
            },
            { Lists.Instructions.ITEM,  new Dictionary<object, string>()
                {
                    { Lists.ItemSubTypes.AWARD,                         $" *award_item_name*" },
                    { Lists.ItemSubTypes.GIVE,                          $" *inventory_item_name*" },
                    { Lists.ItemSubTypes.TAKE,                          $" *inventory_item_name*" },
                }
            },
            { Lists.Instructions.PLAY,  new Dictionary<object, string>()
                {
                    { Lists.PlaySubTypes.BGM,                           $" *bgm_name*" },
                    { Lists.PlaySubTypes.SFX,                           $" *sfx_name*" },
                    { Lists.PlaySubTypes.SFX_GLOBAL,                    $" *sfx_name*" },
                    { Lists.PlaySubTypes.CUTSCENE,                      $"" },
                    { Lists.PlaySubTypes.CUTSCENE_ID,                   $" header_id" },
                }
            },
        };

        public static string GetUsage(Lists.Instructions Instruction, string SubType)
        {
            switch (Instruction)
            {
                case Lists.Instructions.IF:
                case Lists.Instructions.WHILE:
                    {
                        bool res = Enum.TryParse<Lists.IfSubTypes>(SubType, out Lists.IfSubTypes oSubType);
                        string EndKeyword = Instruction == Lists.Instructions.IF ? Lists.Keyword_EndIf : Lists.Keyword_EndWhile;

                        if (res)
                        {
                            return Environment.NewLine + $"{Instruction} {oSubType}{Usages[Instruction][oSubType]}" + Environment.NewLine +
                                                         $"   ~instructions~ " + Environment.NewLine +
                                                         $"{EndKeyword}";
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
                                                             $"{EndKeyword}";
                            }
                            else
                            {
                                return Environment.NewLine + $"{Instruction} *subtype*" + Environment.NewLine +
                                        $"   ~instructions~ " + Environment.NewLine +
                                        $"{EndKeyword}";
                            }
                        }
                    }
                case Lists.Instructions.FACE:
                    {
                        return $"{Lists.Instructions.FACE} {{{Lists.TargetActorSubtypes.SELF}|" +
                                                           $"{Lists.TargetActorSubtypes.PLAYER}|" +
                                                           $"{Lists.TargetActorSubtypes.REF_ACTOR}|" +
                                                           $"{Lists.TargetActorSubtypes.ACTOR_ID} actor_id|" +
                                                           $"{Lists.TargetActorSubtypes.NPCMAKER} npc_id}} " +
                                                           $"{{{Lists.FaceSubtypes.TOWARDS}|{Lists.FaceSubtypes.AWAY_FROM}|{Lists.FaceSubtypes.TOGETHER_WITH}}} " +
                                                           $"{{{Lists.TargetActorSubtypes.SELF}|" +
                                                           $"{Lists.TargetActorSubtypes.PLAYER}|" +
                                                           $"{Lists.TargetActorSubtypes.REF_ACTOR}|" +
                                                           $"{Lists.TargetActorSubtypes.ACTOR_ID} actor_id|" +
                                                           $"{Lists.TargetActorSubtypes.NPCMAKER} npc_id}}";
                    }
                case Lists.Instructions.CCALL:
                    return $"{Lists.Instructions.CCALL} c_function_name";
                case Lists.Instructions.CLOSE_TEXTBOX:
                    return $"{Lists.Instructions.CLOSE_TEXTBOX}";
                case Lists.Instructions.FADEIN:
                    return $"{Lists.Instructions.FADEIN} fadein_rate";
                case Lists.Instructions.FADEOUT:
                    return $"{Lists.Instructions.FADEOUT} r_color g_color b_color fadeout_rate";
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
                               $"ENDSPAWN";
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
                               $"ENDOCARINA";
                    }
                case Lists.Instructions.TALK:
                    {
                        return Environment.NewLine + $"{Lists.Instructions.TALK} textbox_name [textbox_name_child]" + Environment.NewLine +
                               $"   ~instructions~ " + Environment.NewLine +
                               $"ENDTALK";
                    }
                default:
                    return "";
            }
        }
    }
}
