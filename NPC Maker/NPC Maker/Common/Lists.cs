using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class Lists
    {
        public const string Keyword_True = "TRUE";
        public const string Keyword_False = "FALSE";
        public const string Keyword_Return = "RETURN";
        public const string Keyword_SharpDefine = "#DEFINE";
        public const string Keyword_Define = "DEFINE";
        public const string Keyword_Procedure = "PROC";
        public const string Keyword_CallProcedure = "::";
        public const string Keyword_EndProcedure = "ENDPROC";
        public const string Keyword_End = "END";
        public const string Keyword_EndIf = "ENDIF";
        public const string Keyword_EndWhile = "ENDWHILE";
        public const string Keyword_EndSpawn = "ENDSPAWN";
        public const string Keyword_EndTalk = "ENDTALK";
        public const string Keyword_EndTrade = "ENDTRADE";
        public const string Keyword_EndTradeFailure = "ENDFAILURE";
        public const string Keyword_TradeDefault = "DEFAULT";
        public const string Keyword_TradeSucccess = "SUCCESS";
        public const string Keyword_TradeFailure = "FAILURE";
        public const string Keyword_TradeNone = "NO_TRADE";
        public const string Keyword_Else = "ELSE";
        public const string Keyword_ScriptVar = "VAR";
        public const string Keyword_RNG = "RANDOM";
        public const string Keyword_RAM8 = "RAM8";
        public const string Keyword_RAM16 = "RAM16";
        public const string Keyword_RAM32 = "RAM32";
        public const string Keyword_Global8 = "GLOBAL8";
        public const string Keyword_Global16 = "GLOBAL16";
        public const string Keyword_Global32 = "GLOBAL32";
        public const string Keyword_Player8 = "PLAYER8";
        public const string Keyword_Player16 = "PLAYER16";
        public const string Keyword_Player32 = "PLAYER32";
        public const string Keyword_Self8 = "SELF8";
        public const string Keyword_Self16 = "SELF16";
        public const string Keyword_Self32 = "SELF32";

        public static List<string> AllKeywords = new List<string>()
        {
            Keyword_True,
            Keyword_False,
            Keyword_Return,
            Keyword_SharpDefine,
            Keyword_Define,
            Keyword_Procedure,
            Keyword_EndProcedure,
            Keyword_End,
            Keyword_EndIf,
            Keyword_EndWhile,
            Keyword_EndSpawn,
            Keyword_EndTalk,
            Keyword_EndTrade,
            Keyword_EndTradeFailure,
            Keyword_TradeDefault,
            Keyword_TradeSucccess,
            Keyword_TradeFailure,
            Keyword_TradeNone,
            Keyword_Else,
            Keyword_ScriptVar,
            Keyword_RNG,
            Keyword_CallProcedure,
            Keyword_RAM8,
            Keyword_RAM16,
            Keyword_RAM32,
            Keyword_Global8,
            Keyword_Global16,
            Keyword_Global32,
            Keyword_Player8,
            Keyword_Player16,
            Keyword_Player32,
            Keyword_Self8,
            Keyword_Self16,
            Keyword_Self32
        };

        public static List<string> KeywordsBlue = new List<string>()
        {
            Keyword_True,
            Keyword_False,
        };

        public static List<string> KeywordsPurple = new List<string>()
        {
            Keyword_Else,
        };

        public static List<string> KeywordsMPurple = new List<string>()
        {
            Keyword_EndIf,
            Keyword_EndWhile,
            Keyword_EndSpawn,
            Keyword_EndTalk,
            Keyword_EndTrade,
        };

        public static List<string> KeywordsRed = new List<string>()
        {
            Keyword_Procedure,
            Keyword_EndProcedure,
            Keyword_Return,
        };

        public static List<string> KeywordsGray = new List<string>()
        {
            Keyword_ScriptVar,
            Keyword_RNG,
            Keyword_RAM8,
            Keyword_RAM16,
            Keyword_RAM32,
            Keyword_Global8,
            Keyword_Global16,
            Keyword_Global32,
            Keyword_Player8,
            Keyword_Player16,
            Keyword_Player32,
            Keyword_Self8,
            Keyword_Self16,
            Keyword_Self32
        };

        public enum VarTypes
        {
            Normal = 0,
            Random = 1,
            Ram8 = 2,
            Ram16 = 3,
            Ram32 = 4,
            Global8 = 5,
            Global16 = 6,
            Global32 = 7,
            Player8 = 8,
            Player16 = 9,
            Player32 = 10,
            Self8 = 11,
            Self16 = 12,
            Self32 = 13,
            Var = 14,
        }

        public enum Instructions
        {
            IF = 1,
            WHILE = 2,
            AWAIT = 3,
            SET = 4,
            TALK = 5,
            TRADE = 6,
            ENABLE_TALKING = 7,
            SHOW_TEXTBOX = 8,
            ITEM = 9,  
            PLAY = 10,
            KILL = 11,
            SPAWN = 12,  
            WARP = 13,
            CHANGE_SCRIPT = 14,
            ROTATION = 15,
            POSITION = 16,
            SCALE = 17,
            FACE = 18,
            PARTICLE = 19,
            RETURN = 252,
            GOTO = 253,
            LABEL = 254,
            NOP = 255,
        }

        public static List<string> KeyValues = GetKeyValues();
        private static List<string> GetKeyValues()
        {
            List<string> Values = new List<string>();

            Values.AddRange(Enum.GetNames(typeof(Lists.VarTypes)));
            Values.AddRange(Enum.GetNames(typeof(Lists.SpawnParams)));
            Values.AddRange(Enum.GetNames(typeof(Lists.SpawnPosParams)));
            Values.AddRange(Enum.GetNames(typeof(Lists.TradeStatuses)));
            Values.AddRange(Enum.GetNames(typeof(Lists.LookAtStyles)));
            Values.AddRange(Enum.GetNames(typeof(Lists.DListVisibilityOptions)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Axis)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Segments)));
            Values.AddRange(Enum.GetNames(typeof(Lists.TargetActorSubtypes)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Buttons)));

            return Values;
        }

        public enum DictType
        {
            SFX,
            Music,
            Actors,
            Objects,
            ActorCategories,
        }

        public enum IfWhileAwaitSetRamSubTypes
        {
            Random = 241,
            Ram8 = 242,
            Ram16 = 243,
            Ram32 = 244,
            Global8 = 245,
            Global16 = 246,
            Global32 = 247,
            Player8 = 248,
            Player16 = 249,
            Player32 = 250,
            Self8 = 251,
            Self16 = 252,
            Self32 = 253,
            Var = 254,
        }

        public enum IfSubTypes
        {
            /* flags */
            FLAG_INF = 0,
            FLAG_EVENT = 1,
            FLAG_SWITCH = 2,
            FLAG_SCENE = 3,
            FLAG_TREASURE = 4,
            FLAG_ROOM_CLEAR = 5,
            FLAG_SCENE_COLLECT = 6,
            FLAG_TEMPORARY = 7,

            /* bools */
            LINK_IS_ADULT = 8,
            CURRENTLY_DAY = 9,
            CURRENTLY_TALKING = 10,
            PLAYER_HAS_EMPTY_BOTTLE = 11,
            CUTSCENE_BEING_PLAYED = 12,
            TEXTBOX_ON_SCREEN = 13,
            CURRENT_ANIM_WALKING = 14,
            CURRENT_ANIM_IDLE = 15,
            PLAYER_HAS_MAGIC = 16,
            ATTACKED = 17,

            /* s16s */
            PLAYER_RUPEES = 18,
            SCENE_ID = 19,
            PLAYER_SKULLTULAS = 20,
            CURRENT_PATH_NODE = 21,
            CURRENT_ANIMATION_FRAME = 22,
            CURRENT_CUTSCENE_FRAME = 23,
            PLAYER_HEALTH = 24,
            PLAYER_MAGIC = 25,
            PLAYER_BOMBS = 26,
            PLAYER_BOMBCHUS = 27,
            PLAYER_ARROWS = 28,
            PLAYER_DEKUSTICKS = 29,
            PLAYER_DEKUNUTS = 30,
            EXT_VAR_1 = 31,
            EXT_VAR_2 = 32,
            EXT_VAR_3 = 33,
            EXT_VAR_4 = 34,
            EXT_VAR_5 = 35,

            /* s8s */
            STICK_X = 36,
            STICK_Y = 37,

            /* special */
            ITEM_BEING_TRADED = 38,
            TRADE_STATUS = 39,
            PLAYER_MASK = 40,
            TIME_OF_DAY = 41,
            CURRENT_ANIMATION = 42,
            PLAYER_HAS_INVENTORY_ITEM = 43,
            PLAYER_HAS_QUEST_ITEM = 44,
            PLAYER_HAS_DUNGEON_ITEM = 45,
            LAST_ITEM_USED = 46,
            BUTTON_PRESSED = 47,
            BUTTON_HELD = 48,
        }

        public enum ItemSubTypes
        {
            AWARD = 0,
            GIVE = 1,
            TAKE = 2,
        }

        public enum SetSubTypes
        {
            /* u16s */
            MOVEMENT_DISTANCE = 0,
            MOVEMENT_LOOP_DELAY = 1,
            COLLISION_RADIUS = 2,
            COLLISION_HEIGHT = 3,
            MOVEMENT_PATH_ID = 6,
            UNSUCCESSFUL_TRADE_TEXT_ID = 8,
            CUTSCENE_FRAME = 9,

            /* s16s */
            MOVEMENT_LOOP_START = 10,
            MOVEMENT_LOOP_END = 11,
            COLLISION_OFFSET_X = 12,
            COLLISION_OFFSET_Y = 13,
            COLLISION_OFFSET_Z = 14,
            TARGET_OFFSET_X = 15,
            TARGET_OFFSET_Y = 16,
            TARGET_OFFSET_Z = 17,
            MODEL_OFFSET_X = 18,
            MODEL_OFFSET_Y = 19,
            MODEL_OFFSET_Z = 20,
            PLAYER_RUPEES = 21,
            CAMERA_ID = 22,
            LOOKAT_OFFSET_X = 23,
            LOOKAT_OFFSET_Y = 24,
            LOOKAT_OFFSET_Z = 25,
            CURRENT_PATH_NODE = 26,
            CURRENT_ANIMATION_FRAME = 27,
            CURRENT_CUTSCENE_FRAME = 28,

            /* u32s */
            /* s32s */

            /* floats */
            MOVEMENT_SPEED = 29,
            GRAVITY_FORCE = 30,
            TALK_RADIUS = 31,

            /* bools */
            LOOP_MOVEMENT = 32,
            HAVE_COLLISION = 33,
            PRESS_SWITCHES = 34,
            IS_PUSHABLE = 35,
            IS_TARGETTABLE = 36,
            PLAYER_CAN_MOVE = 37,
            ACTOR_CAN_MOVE = 38,
            DO_BLINKING_ANIMATIONS = 39,
            DO_TALKING_ANIMATIONS = 40,
            IS_ALWAYS_ACTIVE = 41,
            PAUSE_CUTSCENE = 42,
            IS_ALWAYS_DRAWN = 43,
            REACTS_IF_ATTACKED = 44,
            TIMED_PATH = 45,
            JUST_SCRIPT = 46,
            OPEN_DOORS = 47,

            /* u8s */
            TARGET_LIMB = 48,
            TARGET_DISTANCE = 49,
            HEAD_LIMB = 50,
            WAIST_LIMB = 51,

            /* s8s */
            PLAYER_BOMBS = 52,
            PLAYER_BOMBCHUS = 53,
            PLAYER_ARROWS = 54,
            PLAYER_HEATLH = 55,
            PLAYER_DEKUNUTS = 56,
            PLAYER_DEKUSTICKS = 57,
            PLAYER_MAGIC = 58,

            /* specials */
            TEXTBOX_RESPONSE_ACTIONS = 59,
            FLAG_INF = 60,
            FLAG_EVENT = 61,
            FLAG_SWITCH = 62,
            FLAG_SCENE = 63,
            FLAG_TREASURE = 64,
            FLAG_ROOM_CLEAR = 65,
            FLAG_SCENE_COLLECT = 66,
            FLAG_TEMPORARY = 67,
            MOVEMENT_TYPE = 68,
            LOOKAT_TYPE = 69,
            HEAD_AXIS = 70,
            CURRENT_ANIMATION = 71,
            ANIMATION_OBJECT = 72,
            ANIMATION_OFFSET = 73,
            ANIMATION_SPEED = 74,
            SCRIPT_START = 75,
            BLINK_PATTERN = 76,
            TALK_PATTERN = 77,
            SEGMENT_ENTRY = 78,
            ENV_COLOR = 79,
            DLIST_VISIBILITY = 80,
            ANIMATION_KEYFRAMES = 81,
            CAMERA_TRACKING_ON = 82,
            CURRENT_ANIMATION_INSTANTLY = 83,
            WAIST_AXIS = 84,
            CUTSCENE_SLOT = 85,
            VAR_1 = 86,
            VAR_2 = 87,
            VAR_3 = 88,
            VAR_4 = 89,
            VAR_5 = 90,
            EXT_VAR_1 = 91,
            EXT_VAR_2 = 92,
            EXT_VAR_3 = 93,
            EXT_VAR_4 = 94,
            EXT_VAR_5 = 95,
            TIME_OF_DAY = 96,
            TIMED_PATH_START_TIME = 97,
            TIMED_PATH_END_TIME = 98,
            BLINK_SEGMENT = 99,
            TALK_SEGMENT = 100,
        }

        public enum AwaitSubTypes
        {
            MOVEMENT_PATH_END = 0,
            TEXTBOX_RESPONSE = 1,
            TALKING_END = 2,
            NO_TEXTBOX_ON_SCREEN = 3,
            FOREVER = 4,

            CURRENT_PATH_NODE = 5,
            FRAMES = 6,
            CURRENT_ANIMATION_FRAME = 7,
            CURRENT_CUTSCENE_FRAME = 8,
            TIME_OF_DAY = 9,

            STICK_X = 10,
            STICK_Y = 11,

            BUTTON_PRESSED = 12,
            BUTTON_HELD = 13,

            VAR_1 = 14,
            VAR_2 = 15,
            VAR_3 = 16,
            VAR_4 = 17,
            VAR_5 = 18,

            EXT_VAR_1 = 19,
            EXT_VAR_2 = 20,
            EXT_VAR_3 = 21,
            EXT_VAR_4 = 22,
            EXT_VAR_5 = 23,
        }

        public enum SpawnParams
        {
            ACTOR_ID,
            POSITION,
            ROTATION,
            VARIABLE,
        }

        public enum SpawnPosParams
        {
            ABSOLUTE = 0,
            RELATIVE = 1,
        }

        public enum ConditionTypes
        {
            EQUALTO = 0,
            TRUE = 0,
            LESSTHAN = 1,
            FALSE = 1,
            MORETHAN = 2,
            LESSOREQ = 3,
            MOREOREQ = 4,
            NOTEQUAL = 5,
            NONE = 6,
        }

        public enum TradeItems
        {
            /* 0x00 */
            EXCH_ITEM_NONE,
            /* 0x01 */
            EXCH_ITEM_LETTER_ZELDA,
            /* 0x02 */
            EXCH_ITEM_WEIRD_EGG,
            /* 0x03 */
            EXCH_ITEM_CHICKEN,
            /* 0x04 */
            EXCH_ITEM_BEAN,
            /* 0x05 */
            EXCH_ITEM_POCKET_EGG,
            /* 0x06 */
            EXCH_ITEM_POCKET_CUCCO,
            /* 0x07 */
            EXCH_ITEM_COJIRO,
            /* 0x08 */
            EXCH_ITEM_ODD_MUSHROOM,
            /* 0x09 */
            EXCH_ITEM_ODD_POTION,
            /* 0x0A */
            EXCH_ITEM_SAW,
            /* 0x0B */
            EXCH_ITEM_SWORD_BROKEN,
            /* 0x0C */
            EXCH_ITEM_PRESCRIPTION,
            /* 0x0D */
            EXCH_ITEM_FROG,
            /* 0x0E */
            EXCH_ITEM_EYEDROPS,
            /* 0x0F */
            EXCH_ITEM_CLAIM_CHECK,
            /* 0x10 */
            EXCH_ITEM_MASK_SKULL,
            /* 0x11 */
            EXCH_ITEM_MASK_SPOOKY,
            /* 0x12 */
            EXCH_ITEM_MASK_KEATON,
            /* 0x13 */
            EXCH_ITEM_MASK_BUNNY,
            /* 0x14 */
            EXCH_ITEM_MASK_TRUTH,
            /* 0x15 */
            EXCH_ITEM_MASK_GORON,
            /* 0x16 */
            EXCH_ITEM_MASK_ZORA,
            /* 0x17 */
            EXCH_ITEM_MASK_GERUDO,
            /* 0x18 */
            EXCH_ITEM_FISH,
            /* 0x19 */
            EXCH_ITEM_BLUE_FIRE,
            /* 0x1A */
            EXCH_ITEM_BUG,
            /* 0x1B */
            EXCH_ITEM_POE,
            /* 0x1C */
            EXCH_ITEM_BIG_POE,
            /* 0x1D */
            EXCH_ITEM_LETTER_RUTO,
        }

        public enum AwardItems
        {
            /* 0x00 */
            GI_NONE,
            /* 0x01 */
            GI_BOMBS_5,
            /* 0x02 */
            GI_NUTS_5,
            /* 0x03 */
            GI_BOMBCHUS_10,
            /* 0x04 */
            GI_BOW,
            /* 0x05 */
            GI_SLINGSHOT,
            /* 0x06 */
            GI_BOOMERANG,
            /* 0x07 */
            GI_STICKS_1,
            /* 0x08 */
            GI_HOOKSHOT,
            /* 0x09 */
            GI_LONGSHOT,
            /* 0x0A */
            GI_LENS,
            /* 0x0B */
            GI_LETTER_ZELDA,
            /* 0x0C */
            GI_OCARINA_OOT,
            /* 0x0D */
            GI_HAMMER,
            /* 0x0E */
            GI_COJIRO,
            /* 0x0F */
            GI_BOTTLE,
            /* 0x10 */
            GI_POTION_RED,
            /* 0x11 */
            GI_POTION_GREEN,
            /* 0x12 */
            GI_POTION_BLUE,
            /* 0x13 */
            GI_FAIRY,
            /* 0x14 */
            GI_MILK_BOTTLE,
            /* 0x15 */
            GI_LETTER_RUTO,
            /* 0x16 */
            GI_BEAN,
            /* 0x17 */
            GI_MASK_SKULL,
            /* 0x18 */
            GI_MASK_SPOOKY,
            /* 0x19 */
            GI_CHICKEN, // uses bean message ID
            /* 0x1A */
            GI_MASK_KEATON,
            /* 0x1B */
            GI_MASK_BUNNY,
            /* 0x1C */
            GI_MASK_TRUTH,
            /* 0x1D */
            GI_POCKET_EGG,
            /* 0x1E */
            GI_POCKET_CUCCO, // uses bean message ID
            /* 0x1F */
            GI_ODD_MUSHROOM,
            /* 0x20 */
            GI_ODD_POTION,
            /* 0x21 */
            GI_SAW,
            /* 0x22 */
            GI_SWORD_BROKEN,
            /* 0x23 */
            GI_PRESCRIPTION,
            /* 0x24 */
            GI_FROG,
            /* 0x25 */
            GI_EYEDROPS,
            /* 0x26 */
            GI_CLAIM_CHECK,
            /* 0x27 */
            GI_SWORD_KOKIRI,
            /* 0x28 */
            GI_SWORD_KNIFE,
            /* 0x29 */
            GI_SHIELD_DEKU,   // or blue rupee if you have the shield
            /* 0x2A */
            GI_SHIELD_HYLIAN, // or blue rupee if you have the shield
            /* 0x2B */
            GI_SHIELD_MIRROR,
            /* 0x2C */
            GI_TUNIC_GORON, // or blue rupee if you have the tunic
            /* 0x2D */
            GI_TUNIC_ZORA,  // or blue rupee if you have the tunic
            /* 0x2E */
            GI_BOOTS_IRON,
            /* 0x2F */
            GI_BOOTS_HOVER,
            /* 0x30 */
            GI_QUIVER_40,
            /* 0x31 */
            GI_QUIVER_50,
            /* 0x32 */
            GI_BOMB_BAG_20,
            /* 0x33 */
            GI_BOMB_BAG_30,
            /* 0x34 */
            GI_BOMB_BAG_40,
            /* 0x35 */
            GI_GAUNTLETS_SILVER,
            /* 0x36 */
            GI_GAUNTLETS_GOLD,
            /* 0x37 */
            GI_SCALE_SILVER,
            /* 0x38 */
            GI_SCALE_GOLD,
            /* 0x39 */
            GI_STONE_OF_AGONY,
            /* 0x3A */
            GI_GERUDO_CARD,
            /* 0x3B */
            GI_OCARINA_FAIRY, // uses Ocarina of Time message ID
            /* 0x3C */
            GI_SEEDS_5,
            /* 0x3D */
            GI_HEART_CONTAINER,
            /* 0x3E */
            GI_HEART_PIECE,
            /* 0x3F */
            GI_KEY_BOSS,
            /* 0x40 */
            GI_COMPASS,
            /* 0x41 */
            GI_MAP,
            /* 0x42 */
            GI_KEY_SMALL,
            /* 0x43 */
            GI_MAGIC_SMALL, // or blue rupee if not from a drop
            /* 0x44 */
            GI_MAGIC_LARGE, // or blue rupee if not from a drop
            /* 0x45 */
            GI_WALLET_ADULT,
            /* 0x46 */
            GI_WALLET_GIANT,
            /* 0x47 */
            GI_WEIRD_EGG,
            /* 0x48 */
            GI_HEART,
            /* 0x49 */
            GI_ARROWS_SMALL,  // amount changes depending on context
            /* 0x4A */
            GI_ARROWS_MEDIUM, // amount changes depending on context
            /* 0x4B */
            GI_ARROWS_LARGE,  // amount changes depending on context
            /* 0x4C */
            GI_RUPEE_GREEN,
            /* 0x4D */
            GI_RUPEE_BLUE,
            /* 0x4E */
            GI_RUPEE_RED,
            /* 0x4F */
            GI_HEART_CONTAINER_2,
            /* 0x50 */
            GI_MILK,
            /* 0x51 */
            GI_MASK_GORON,
            /* 0x52 */
            GI_MASK_ZORA,
            /* 0x53 */
            GI_MASK_GERUDO,
            /* 0x54 */
            GI_BRACELET,
            /* 0x55 */
            GI_RUPEE_PURPLE,
            /* 0x56 */
            GI_RUPEE_GOLD,
            /* 0x57 */
            GI_SWORD_BGS,
            /* 0x58 */
            GI_ARROW_FIRE,
            /* 0x59 */
            GI_ARROW_ICE,
            /* 0x5A */
            GI_ARROW_LIGHT,
            /* 0x5B */
            GI_SKULL_TOKEN,
            /* 0x5C */
            GI_DINS_FIRE,
            /* 0x5D */
            GI_FARORES_WIND,
            /* 0x5E */
            GI_NAYRUS_LOVE,
            /* 0x5F */
            GI_BULLET_BAG_30,
            /* 0x60 */
            GI_BULLET_BAG_40,
            /* 0x61 */
            GI_STICKS_5,
            /* 0x62 */
            GI_STICKS_10,
            /* 0x63 */
            GI_NUTS_5_2,
            /* 0x64 */
            GI_NUTS_10,
            /* 0x65 */
            GI_BOMBS_1,
            /* 0x66 */
            GI_BOMBS_10,
            /* 0x67 */
            GI_BOMBS_20,
            /* 0x68 */
            GI_BOMBS_30,
            /* 0x69 */
            GI_SEEDS_30,
            /* 0x6A */
            GI_BOMBCHUS_5,
            /* 0x6B */
            GI_BOMBCHUS_20,
            /* 0x6C */
            GI_FISH,
            /* 0x6D */
            GI_BUGS,
            /* 0x6E */
            GI_BLUE_FIRE,
            /* 0x6F */
            GI_POE,
            /* 0x70 */
            GI_BIG_POE,
            /* 0x71 */
            GI_DOOR_KEY,          // specific to chest minigame
            /* 0x72 */
            GI_RUPEE_GREEN_LOSE,  // specific to chest minigame
            /* 0x73 */
            GI_RUPEE_BLUE_LOSE,   // specific to chest minigame
            /* 0x74 */
            GI_RUPEE_RED_LOSE,    // specific to chest minigame
            /* 0x75 */
            GI_RUPEE_PURPLE_LOSE, // specific to chest minigame
            /* 0x76 */
            GI_HEART_PIECE_WIN,   // specific to chest minigame
            /* 0x77 */
            GI_STICK_UPGRADE_20,
            /* 0x78 */
            GI_STICK_UPGRADE_30,
            /* 0x79 */
            GI_NUT_UPGRADE_30,
            /* 0x7A */
            GI_NUT_UPGRADE_40,
            /* 0x7B */
            GI_BULLET_BAG_50,
            /* 0x7C */
            GI_ICE_TRAP, // freezes link when opened from a chest
            /* 0x7D */
            GI_TEXT_0, // no model appears over Link, shows text id 0 (pocket egg)
        }

        public enum Items
        {
            /* 0x00 */
            ITEM_STICK,
            /* 0x01 */
            ITEM_NUT,
            /* 0x02 */
            ITEM_BOMB,
            /* 0x03 */
            ITEM_BOW,
            /* 0x04 */
            ITEM_ARROW_FIRE,
            /* 0x05 */
            ITEM_DINS_FIRE,
            /* 0x06 */
            ITEM_SLINGSHOT,
            /* 0x07 */
            ITEM_OCARINA_FAIRY,
            /* 0x08 */
            ITEM_OCARINA_TIME,
            /* 0x09 */
            ITEM_BOMBCHU,
            /* 0x0A */
            ITEM_HOOKSHOT,
            /* 0x0B */
            ITEM_LONGSHOT,
            /* 0x0C */
            ITEM_ARROW_ICE,
            /* 0x0D */
            ITEM_FARORES_WIND,
            /* 0x0E */
            ITEM_BOOMERANG,
            /* 0x0F */
            ITEM_LENS,
            /* 0x10 */
            ITEM_BEAN,
            /* 0x11 */
            ITEM_HAMMER,
            /* 0x12 */
            ITEM_ARROW_LIGHT,
            /* 0x13 */
            ITEM_NAYRUS_LOVE,
            /* 0x14 */
            ITEM_BOTTLE,
            /* 0x15 */
            ITEM_POTION_RED,
            /* 0x16 */
            ITEM_POTION_GREEN,
            /* 0x17 */
            ITEM_POTION_BLUE,
            /* 0x18 */
            ITEM_FAIRY,
            /* 0x19 */
            ITEM_FISH,
            /* 0x1A */
            ITEM_MILK_BOTTLE,
            /* 0x1B */
            ITEM_LETTER_RUTO,
            /* 0x1C */
            ITEM_BLUE_FIRE,
            /* 0x1D */
            ITEM_BUG,
            /* 0x1E */
            ITEM_BIG_POE,
            /* 0x1F */
            ITEM_MILK_HALF,
            /* 0x20 */
            ITEM_POE,
            /* 0x21 */
            ITEM_WEIRD_EGG,
            /* 0x22 */
            ITEM_CHICKEN,
            /* 0x23 */
            ITEM_LETTER_ZELDA,
            /* 0x24 */
            ITEM_MASK_KEATON,
            /* 0x25 */
            ITEM_MASK_SKULL,
            /* 0x26 */
            ITEM_MASK_SPOOKY,
            /* 0x27 */
            ITEM_MASK_BUNNY,
            /* 0x28 */
            ITEM_MASK_GORON,
            /* 0x29 */
            ITEM_MASK_ZORA,
            /* 0x2A */
            ITEM_MASK_GERUDO,
            /* 0x2B */
            ITEM_MASK_TRUTH,
            /* 0x2C */
            ITEM_SOLD_OUT,
            /* 0x2D */
            ITEM_POCKET_EGG,
            /* 0x2E */
            ITEM_POCKET_CUCCO,
            /* 0x2F */
            ITEM_COJIRO,
            /* 0x30 */
            ITEM_ODD_MUSHROOM,
            /* 0x31 */
            ITEM_ODD_POTION,
            /* 0x32 */
            ITEM_SAW,
            /* 0x33 */
            ITEM_SWORD_BROKEN,
            /* 0x34 */
            ITEM_PRESCRIPTION,
            /* 0x35 */
            ITEM_FROG,
            /* 0x36 */
            ITEM_EYEDROPS,
            /* 0x37 */
            ITEM_CLAIM_CHECK,
            /* 0x38 */
            ITEM_BOW_ARROW_FIRE,
            /* 0x39 */
            ITEM_BOW_ARROW_ICE,
            /* 0x3A */
            ITEM_BOW_ARROW_LIGHT,
            /* 0x3B */
            ITEM_SWORD_KOKIRI,
            /* 0x3C */
            ITEM_SWORD_MASTER,
            /* 0x3D */
            ITEM_SWORD_BGS,
            /* 0x3E */
            ITEM_SHIELD_DEKU,
            /* 0x3F */
            ITEM_SHIELD_HYLIAN,
            /* 0x40 */
            ITEM_SHIELD_MIRROR,
            /* 0x41 */
            ITEM_TUNIC_KOKIRI,
            /* 0x42 */
            ITEM_TUNIC_GORON,
            /* 0x43 */
            ITEM_TUNIC_ZORA,
            /* 0x44 */
            ITEM_BOOTS_KOKIRI,
            /* 0x45 */
            ITEM_BOOTS_IRON,
            /* 0x46 */
            ITEM_BOOTS_HOVER,
            /* 0x47 */
            ITEM_BULLET_BAG_30,
            /* 0x48 */
            ITEM_BULLET_BAG_40,
            /* 0x49 */
            ITEM_BULLET_BAG_50,
            /* 0x4A */
            ITEM_QUIVER_30,
            /* 0x4B */
            ITEM_QUIVER_40,
            /* 0x4C */
            ITEM_QUIVER_50,
            /* 0x4D */
            ITEM_BOMB_BAG_20,
            /* 0x4E */
            ITEM_BOMB_BAG_30,
            /* 0x4F */
            ITEM_BOMB_BAG_40,
            /* 0x50 */
            ITEM_BRACELET,
            /* 0x51 */
            ITEM_GAUNTLETS_SILVER,
            /* 0x52 */
            ITEM_GAUNTLETS_GOLD,
            /* 0x53 */
            ITEM_SCALE_SILVER,
            /* 0x54 */
            ITEM_SCALE_GOLDEN,
            /* 0x55 */
            ITEM_SWORD_KNIFE,
            /* 0x56 */
            ITEM_WALLET_ADULT,
            /* 0x57 */
            ITEM_WALLET_GIANT,
            /* 0x58 */
            ITEM_SEEDS,
            /* 0x59 */
            ITEM_FISHING_POLE,
            /* 0x5A */
            ITEM_SONG_MINUET,
            /* 0x5B */
            ITEM_SONG_BOLERO,
            /* 0x5C */
            ITEM_SONG_SERENADE,
            /* 0x5D */
            ITEM_SONG_REQUIEM,
            /* 0x5E */
            ITEM_SONG_NOCTURNE,
            /* 0x5F */
            ITEM_SONG_PRELUDE,
            /* 0x60 */
            ITEM_SONG_LULLABY,
            /* 0x61 */
            ITEM_SONG_EPONA,
            /* 0x62 */
            ITEM_SONG_SARIA,
            /* 0x63 */
            ITEM_SONG_SUN,
            /* 0x64 */
            ITEM_SONG_TIME,
            /* 0x65 */
            ITEM_SONG_STORMS,
            /* 0x66 */
            ITEM_MEDALLION_FOREST,
            /* 0x67 */
            ITEM_MEDALLION_FIRE,
            /* 0x68 */
            ITEM_MEDALLION_WATER,
            /* 0x69 */
            ITEM_MEDALLION_SPIRIT,
            /* 0x6A */
            ITEM_MEDALLION_SHADOW,
            /* 0x6B */
            ITEM_MEDALLION_LIGHT,
            /* 0x6C */
            ITEM_KOKIRI_EMERALD,
            /* 0x6D */
            ITEM_GORON_RUBY,
            /* 0x6E */
            ITEM_ZORA_SAPPHIRE,
            /* 0x6F */
            ITEM_STONE_OF_AGONY,
            /* 0x70 */
            ITEM_GERUDO_CARD,
            /* 0x71 */
            ITEM_SKULL_TOKEN,
            /* 0x72 */
            ITEM_HEART_CONTAINER,
            /* 0x73 */
            ITEM_HEART_PIECE,
            /* 0x74 */
            ITEM_KEY_BOSS,
            /* 0x75 */
            ITEM_COMPASS,
            /* 0x76 */
            ITEM_DUNGEON_MAP,
            /* 0x77 */
            ITEM_KEY_SMALL,
            /* 0x78 */
            ITEM_MAGIC_SMALL,
            /* 0x79 */
            ITEM_MAGIC_LARGE,
            /* 0x7A */
            ITEM_HEART_PIECE_2,
            /* 0x7B */
            ITEM_INVALID_1,
            /* 0x7C */
            ITEM_INVALID_2,
            /* 0x7D */
            ITEM_INVALID_3,
            /* 0x7E */
            ITEM_INVALID_4,
            /* 0x7F */
            ITEM_INVALID_5,
            /* 0x80 */
            ITEM_INVALID_6,
            /* 0x81 */
            ITEM_INVALID_7,
            /* 0x82 */
            ITEM_MILK,
            /* 0x83 */
            ITEM_HEART,
            /* 0x84 */
            ITEM_RUPEE_GREEN,
            /* 0x85 */
            ITEM_RUPEE_BLUE,
            /* 0x86 */
            ITEM_RUPEE_RED,
            /* 0x87 */
            ITEM_RUPEE_PURPLE,
            /* 0x88 */
            ITEM_RUPEE_GOLD,
            /* 0x89 */
            ITEM_INVALID_8,
            /* 0x8A */
            ITEM_STICKS_5,
            /* 0x8B */
            ITEM_STICKS_10,
            /* 0x8C */
            ITEM_NUTS_5,
            /* 0x8D */
            ITEM_NUTS_10,
            /* 0x8E */
            ITEM_BOMBS_5,
            /* 0x8F */
            ITEM_BOMBS_10,
            /* 0x90 */
            ITEM_BOMBS_20,
            /* 0x91 */
            ITEM_BOMBS_30,
            /* 0x92 */
            ITEM_ARROWS_SMALL,
            /* 0x93 */
            ITEM_ARROWS_MEDIUM,
            /* 0x94 */
            ITEM_ARROWS_LARGE,
            /* 0x95 */
            ITEM_SEEDS_30,
            /* 0x96 */
            ITEM_BOMBCHUS_5,
            /* 0x97 */
            ITEM_BOMBCHUS_20,
            /* 0x98 */
            ITEM_STICK_UPGRADE_20,
            /* 0x99 */
            ITEM_STICK_UPGRADE_30,
            /* 0x9A */
            ITEM_NUT_UPGRADE_30,
            /* 0x9B */
            ITEM_NUT_UPGRADE_40,
            /* 0xFC */
            ITEM_LAST_USED = 0xFC,
            /* 0xFE */
            ITEM_NONE_FE = 0xFE,
            /* 0xFF */
            ITEM_NONE = 0xFF,

            ITEM_TRADE_CHILD = ITEM_WEIRD_EGG,

            ITEMITEM_TRADE_ADULT = ITEM_POCKET_EGG,
        }

        public enum QuestItems
        {
            /* 0x00 */
            QUEST_MEDALLION_FOREST,
            /* 0x01 */
            QUEST_MEDALLION_FIRE,
            /* 0x02 */
            QUEST_MEDALLION_WATER,
            /* 0x03 */
            QUEST_MEDALLION_SPIRIT,
            /* 0x04 */
            QUEST_MEDALLION_SHADOW,
            /* 0x05 */
            QUEST_MEDALLION_LIGHT,
            /* 0x06 */
            QUEST_SONG_MINUET,
            /* 0x07 */
            QUEST_SONG_BOLERO,
            /* 0x08 */
            QUEST_SONG_SERENADE,
            /* 0x09 */
            QUEST_SONG_REQUIEM,
            /* 0x0A */
            QUEST_SONG_NOCTURNE,
            /* 0x0B */
            QUEST_SONG_PRELUDE,
            /* 0x0C */
            QUEST_SONG_LULLABY,
            /* 0x0D */
            QUEST_SONG_EPONA,
            /* 0x0E */
            QUEST_SONG_SARIA,
            /* 0x0F */
            QUEST_SONG_SUN,
            /* 0x10 */
            QUEST_SONG_TIME,
            /* 0x11 */
            QUEST_SONG_STORMS,
            /* 0x12 */
            QUEST_KOKIRI_EMERALD,
            /* 0x13 */
            QUEST_GORON_RUBY,
            /* 0x14 */
            QUEST_ZORA_SAPPHIRE,
            /* 0x15 */
            QUEST_STONE_OF_AGONY,
            /* 0x16 */
            QUEST_GERUDO_CARD,
            /* 0x17 */
            QUEST_SKULL_TOKEN,
            /* 0x18 */
            QUEST_HEART_PIECE
        }

        public enum DungeonItems
        {
            /* 0x00 */
            DUNGEON_KEY_BOSS,
            /* 0x01 */
            DUNGEON_COMPASS,
            /* 0x02 */
            DUNGEON_MAP
        }

        public enum TradeStatuses
        {
            TRADE_SUCCESSFUL = 0,
            SUCCESS = 0,
            TRADE_UNSUCCESSFUL = 1,
            WRONG = 1,
            FAILURE = 1,
            NONE = 2,
            TALK = 2,
            NO_TRADE = 2,
        }

        public enum PlayerMasks
        {
            ITEM_MASK_KEATON, 
            ITEM_MASK_SPOOKY, 
            ITEM_MASK_SKULL, 
            ITEM_MASK_BUNNY,
            ITEM_MASK_TRUTH, 
            ITEM_MASK_ZORA, 
            ITEM_MASK_GORON, 
            ITEM_MASK_GERUDO
        }

        public enum MovementStyles
        {
            NONE = 0,
            RANDOM = 1,
            FOLLOW_PLAYER = 2,
            PATH_COLLISION = 3,
            PATH_DIRECT = 4
        }

        public enum LookAtStyles
        {
            NONE = 0,
            BODY = 1,
            HEAD = 2,
            WAIST = 3,
            HEAD_WAIST = 4,
        }

        public enum Axis
        {
            X = 0,
            PLUS_X = 0,
            NEG_X = 1,
            MINUS_X = 1,
            Y = 2,
            PLUS_Y = 2,
            NEG_Y = 3,
            MINUS_Y = 3,
            Z = 4,
            PLUS_Z = 4,
            NEG_Z = 5,
            MINUS_Z = 5,
        }

        public enum DListVisibilityOptions
        {
            NOT_VISIBLE = 0,
            WITH_LIMB = 1,
            INSTEAD_OF_LIMB = 2,
        }
   
        public enum PlaySubTypes
        {
            SFX = 0,
            BGM = 1,
            CUTSCENE = 2,
            CUTSCENE_ADDR = 3,
            CUTSCENE_ID = 4
        }

        public enum RotationSubTypes
        {
            SET = 0,
            CHANGE = 1,
        }

        public enum PositionSubTypes
        {
            SET = 0,
            CHANGE = 1,
        }

        public enum ScaleSubTypes
        {
            SET = 0,
            CHANGE = 1,
        }

        public enum Segments
        {
            SEGMENT_8 = 0,
            SEGMENT_9 = 1,
            SEGMENT_A = 2,
            SEGMENT_B = 3,
            SEGMENT_C = 4,
            SEGMENT_D = 5,
            SEGMENT_E = 6,
            SEGMENT_F = 7,
            SEG_8 = 0,
            SEG_9 = 1,
            SEG_A = 2,
            SEG_B = 3,
            SEG_C = 4,
            SEG_D = 5,
            SEG_E = 6,
            SEG_F = 7,

        }

        public enum FaceSubtypes
        {
            TOWARDS = 0,
            AND = 1,
            AWAY_FROM = 2,
        }

        public static List<string> FaceSubTypesForCtxMenu = GetFaceSubTypesFoxCtxMenu();

        private static List<string> GetFaceSubTypesFoxCtxMenu()
        {
            List<string> FList = new List<string>();

            FList.AddRange(Enum.GetNames(typeof(Lists.TargetActorSubtypes)));
            FList.AddRange(Enum.GetNames(typeof(Lists.FaceSubtypes)));

            return FList;
        }

        public enum TargetActorSubtypes
        {
            SELF = 0,
            PLAYER = 1,
            CONFIG_ID = 2,
            ACTOR_ID = 3,
        }

        public enum ScriptChangeSubtypes
        {
            OVERWRITE = 0,
            RESTORE = 1,
        }

        public enum Buttons
        {
            BTN_CRIGHT = 0x0001,
            BTN_CLEFT = 0x0002,
            BTN_CDOWN = 0x0004,
            BTN_CUP = 0x0008,
            BTN_R = 0x0010,
            BTN_L = 0x0020,
            BTN_DRIGHT = 0x0100,
            BTN_DLEFT = 0x0200,
            BTN_DDOWN = 0x0400,
            BTN_DUP = 0x0800,
            BTN_START = 0x1000,
            BTN_Z = 0x2000,
            BTN_B = 0x4000,
            BTN_A = 0x8000,
        }
    }
}
