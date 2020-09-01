using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public static class Lists
    {
        public static Dictionary<string, int> SFXes = FileOps.GetSoundDictionary("SFX.csv");
        public static Dictionary<string, int> Music = FileOps.GetSoundDictionary("Music.csv");

        public const string Keyword_True = "TRUE";
        public const string Keyword_False = "FALSE";
        public const string Keyword_Return = "RETURN";
        public const string Keyword_SharpDefine = "#DEFINE";
        public const string Keyword_Define = "DEFINE";
        public const string Keyword_End = "END";
        public const string Keyword_EndIf = "ENDIF";
        public const string Keyword_EndWhile = "ENDWHILE";
        public const string Keyword_Else = "ELSE";
        public const string Keyword_ScriptVar1 = "VAR_1";
        public const string Keyword_ScriptVar2 = "VAR_2";
        public const string Keyword_ScriptVar3 = "VAR_3";
        public const string Keyword_ScriptVar4 = "VAR_4";
        public const string Keyword_ScriptVar5 = "VAR_5";
        public const string Keyword_RNG = "RNG";

        public static List<string> Keywords = new List<string>()
        {
            Keyword_True,
            Keyword_False,
            Keyword_Return,
            Keyword_SharpDefine,
            Keyword_Define,
            Keyword_End,
            Keyword_EndIf,
            Keyword_EndWhile,
            Keyword_ScriptVar1,
            Keyword_ScriptVar2,
            Keyword_ScriptVar3,
            Keyword_ScriptVar4,
            Keyword_ScriptVar5,
            Keyword_RNG
        };

        public enum VarTypes
        {
            RNG = 1,
            Var1 = 5,
            Var2 = 6,
            Var3 = 7,
            Var4 = 8,
            Var5 = 9,
        }

        public enum Instructions
        {
            IF = 1,
            SET = 2,
            AWAIT = 3,
            ENABLE_TALKING = 4,
            ENABLE_TRADING = 5,
            SHOW_TEXTBOX = 6,
            INVENTORY = 7,
            PLAY = 8,
            KILL = 9,
            SPAWN = 10,
            WARP = 11,
            CHANGE_SCRIPT = 12,
            RNG = 13,
            WHILE = 14,
            RETURN = 252,
            GOTO = 253,
            LABEL = 254,
            NOP = 255,
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
            LINK_IS_ADULT = 10,
            LINK_IS_CHILD = 11,
            IS_CURRENTLY_DAY = 12,
            IS_CURRENTLY_NIGHT = 13,
            IS_CURRENTLY_TALKING = 14,
            PLAYER_HAS_EMPTY_BOTTLE = 15,
            CUTSCENE_IS_BEING_PLAYED = 16,
            TEXTBOX_IS_ON_SCREEN = 17,
            NO_TEXTBOX_ON_SCREEN = 18,
            CURRENT_ANIM_IS_WALKING = 19,
            CURRENT_ANIM_IS_IDLE = 20,
            PLAYER_HAS_MAGIC = 21,

            /* s16s */
            PLAYER_RUPEES = 30,
            SCENE_ID = 31,
            PLAYER_SKULLTULAS = 32,
            CURRENT_PATH_NODE = 33,
            CURRENT_ANIMATION_FRAME = 34,
            CURRENT_CUTSCENE_FRAME = 35,
            VAR_1 = 36,
            VAR_2 = 37,
            VAR_3 = 38,
            VAR_4 = 39,
            VAR_5 = 40,
            CURRENT_HEALTH = 41,
            CURRENT_MAGIC = 42,
            PLAYER_BOMBS = 31,
            PLAYER_BOMBCHUS = 32,
            PLAYER_ARROWS = 33,
            PLAYER_HEALTH = 34,
            PLAYER_DEKUSTICKS = 35,
            PLAYER_DEKUNUTS = 36,

            /* special */
            ITEM_BEING_TRADED = 61,
            TRADE_STATUS = 62,
            PLAYER_MASK = 64,
            TIME_OF_DAY = 65,
            CURRENT_ANIMATION = 66,
            PLAYER_BOMBBAG = 67,
            PLAYER_WALLET = 68,
            PLAYER_QUIVER = 69,
            PLAYER_WATER_SCALE = 70,
            PLAYER_GAUNTLETS = 71,
            PLAYER_STICKCAP = 72,
            PLAYER_DEKUNUTCAP = 73,
        }

        public enum SetSubTypes
        {
            /* u16s */
            MOVEMENT_DISTANCE = 0,
            MOVEMENT_LOOP_DELAY = 1,
            COLLISION_RADIUS = 2,
            COLLISION_HEIGHT = 3,
            MOVEMENT_PATH_ID = 6,
            TIME_OF_DAY = 7,
            UNSUCCESSFUL_TRADE_TEXT_ID = 8,
            CUTSCENE_FRAME = 9,

            /* s16s */
            MOVEMENT_LOOP_START = 35,
            MOVEMENT_LOOP_END = 36,
            COLLISION_OFFSET_X = 37,
            COLLISION_OFFSET_Y = 38,
            COLLISION_OFFSET_Z = 39,
            TARGET_OFFSET_X = 40,
            TARGET_OFFSET_Y = 41,
            TARGET_OFFSET_Z = 42,
            MODEL_OFFSET_X = 43,
            MODEL_OFFSET_Y = 44,
            MODEL_OFFSET_Z = 45,
            PLAYER_RUPEES = 46,
            CAMERA_ID = 47,
            LOOKAT_OFFSET_X = 48,
            LOOKAT_OFFSET_Y = 49,
            LOOKAT_OFFSET_Z = 50,
            CURRENT_PATH_NODE = 51,
            CURRENT_ANIMATION_FRAME = 52,
            CURRENT_CUTSCENE_FRAME = 53,

            /* u32s */
            /* s32s */

            /* floats */
            MODEL_SCALE = 140,
            MOVEMENT_SPEED = 141,
            MODEL_SCALE_SMOOTHLY = 142,

            /* bools */
            LOOP_MOVEMENT = 175,
            HAVE_COLLISION = 176,
            PRESS_SWITCHES = 177,
            IS_PUSHABLE = 178,
            IS_TARGETTABLE = 179,
            PLAYER_CAN_MOVE = 180,
            ACTOR_CAN_MOVE = 181,
            DO_BLINKING_ANIMATIONS = 182,
            DO_TALKING_ANIMATIONS = 183,
            IS_ALWAYS_ACTIVE = 184,
            PAUSE_CUTSCENE = 185,
            IS_ALWAYS_DRAWN = 186,
            NO_TEXTBOX_ON_SCREEN = 187,
            PLAYER_HAS_DEFENSE_UPGRADE = 188,

            /* u8s */
            TARGET_LIMB = 195,
            TARGET_DISTANCE = 196,
            HEAD_LIMB = 197,
            WAIST_LIMB = 198,

            /* s8s */
            PLAYER_BOMBS = 210,
            PLAYER_BOMBCHUS = 211,
            PLAYER_ARROWS = 212,
            PLAYER_HEATLH = 213,
            PLAYER_DEKUNUTS = 214,
            PLAYER_DEKUSTICKS = 215,
            PLAYER_MAGIC = 216,

            /* specials */
            TEXTBOX_RESPONSE_ACTIONS = 220,
            FLAG_INF = 221,
            FLAG_EVENT = 222,
            FLAG_SWITCH = 223,
            FLAG_SCENE = 224,
            FLAG_TREASURE = 225,
            FLAG_ROOM_CLEAR = 226,
            FLAG_SCENE_COLLECT = 227,
            FLAG_TEMPORARY = 228,
            MOVEMENT_TYPE = 229,
            LOOKAT_TYPE = 230,
            HEAD_AXIS = 231,
            CURRENT_ANIMATION = 232,
            ANIMATION_OBJECT = 233,
            ANIMATION_OFFSET = 234,
            ANIMATION_SPEED = 235,
            SCRIPT_START = 236,
            BLINK_PATTERN = 237,
            TALK_PATTERN = 238,
            TEXTURE = 239,
            ENV_COLOR = 240,
            DLIST_VISIBILITY = 241,
            ANIMATION_KEYFRAMES = 242,
            CAMERA_TRACKING_ON = 243,
            CURRENT_ANIMATION_INSTANTLY = 244,
            WAIST_AXIS = 245,
            CUTSCENE_SLOT = 246,
            VAR_1 = 247,
            VAR_2 = 248,
            VAR_3 = 249,
            VAR_4 = 250,
            VAR_5 = 251,
        }

        public enum AwaitSubTypes
        {
            MOVEMENT_PATH_END = 0,
            TEXTBOX_RESPONSE = 1,
            TALKING_END = 2,
            NO_TEXTBOX_ON_SCREEN = 3,
            FOREVER = 4,

            CURRENT_PATH_NODE = 35,
            FRAMES = 36,
            CURRENT_ANIMATION_FRAME = 37,
            CURRENT_CUTSCENE_FRAME = 38,

            VAR_1 = 75,
            VAR_2 = 76,
            VAR_3 = 77,
            VAR_4 = 78,
            VAR_5 = 79,
        }

        public enum TradeItems
        {
            ITEM_LETTER_ZELDA = 1,
            ITEM_WEIRD_EGG = 2,
            ITEM_CHICKEN = 3,
            ITEM_BEAN = 4,
            ITEM_POCKET_EGG = 5,
            ITEM_POCKET_CUCCO = 6,
            ITEM_COJIRO = 7,
            ITEM_ODD_MUSHROOM = 8,
            ITEM_ODD_POTION = 9,
            ITEM_SAW = 10,
            ITEM_SWORD_BROKEN = 11,
            ITEM_PRESCRIPTION = 12,
            ITEM_FROG = 13,
            ITEM_EYEDROPS = 14,
            ITEM_CLAIM_CHECK = 15,
            ITEM_FISH = 24,
            ITEM_BLUE_FIRE = 25,
            ITEM_BUGS = 26,
            ITEM_POE = 27,
            ITEM_BIG_POE = 28,
            ITEM_LETTER_RUTO = 29,
        }
        
        public enum TradeStatuses
        {
            TRADE_SUCCESSFUL = 0,
            TRADE_UNSUCCESSFUL = 1,
            NOT_TRADING = 2,
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

        public enum BombBags
        {
            SMALL = 0,
            BIG = 1,
            BIGGEST = 2
        }

        public enum Quivers
        {
            SMALL = 0,
            BIG = 1,
            BIGGEST = 2
        }

        public enum Wallets
        {
            SMALL = 0,
            ADULT = 1,
            GIANT = 2
        }

        public enum Scales
        {
            NONE = 0,
            SILVER = 1,
            GOLDEN = 2
        }

        public enum Gauntlets
        {
            NONE = 0,
            GORON = 1,
            SILVER = 2,
            GOLDEN = 3
        }

        public enum StickCap
        {
            NONE = 0,
            NORMAL = 1,
            UPGRADED = 2
        }

        public enum NutCap
        {
            NONE = 0,
            NORMAL = 1,
            UPGRADED = 2
        }

        public enum PlaySubTypes
        {
            SFX = 0,
            BGM = 1,
            CUTSCENE = 2,
            CUTSCENE_ADDR = 3,
            CUTSCENE_ID = 4
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

        public enum TurnTowardsSubtypes
        {
            SELF = 0,
            PLAYER = 1,
            CONFIG_ID = 2,
            ACTOR_ID = 3,
        }
    }
}
