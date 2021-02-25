using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public static class Lists
    {
        public static Dictionary<string, int> SFXes = FileOps.GetDictionary($"{Program.ExecPath}/SFX.csv");
        public static Dictionary<string, int> Music = FileOps.GetDictionary($"{Program.ExecPath}/Music.csv");
        public static Dictionary<string, int> Actors = FileOps.GetDictionary($"{Program.ExecPath}/Actors.csv");

        public const string Keyword_True = "TRUE";
        public const string Keyword_False = "FALSE";
        public const string Keyword_Return = "RETURN";
        public const string Keyword_SharpDefine = "#DEFINE";
        public const string Keyword_Define = "DEFINE";
        public const string Keyword_Procedure = "PROC";
        public const string Keyword_CallProcedure = ":";
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
        public const string Keyword_ScriptVar1 = "VAR_1";
        public const string Keyword_ScriptVar2 = "VAR_2";
        public const string Keyword_ScriptVar3 = "VAR_3";
        public const string Keyword_ScriptVar4 = "VAR_4";
        public const string Keyword_ScriptVar5 = "VAR_5";
        public const string Keyword_RNG = "RNG";

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
            Keyword_ScriptVar1,
            Keyword_ScriptVar2,
            Keyword_ScriptVar3,
            Keyword_ScriptVar4,
            Keyword_ScriptVar5,
            Keyword_RNG,
            Keyword_CallProcedure
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
            Var1 = 10,
            Var2 = 11,
            Var3 = 12,
            Var4 = 13,
            Var5 = 14,
        }

        public enum Instructions
        {
            IF = 1,
            SET = 2,
            AWAIT = 3,
            ENABLE_TALKING = 4,
            TRADE = 5,
            SHOW_TEXTBOX = 6,
            INVENTORY = 7,  // !
            PLAY = 8,
            KILL = 9,
            SPAWN = 10,  // !
            WARP = 11,
            CHANGE_SCRIPT = 12,
            WHILE = 13,
            TALK = 14,
            RETURN = 252,
            GOTO = 253,
            LABEL = 254,
            NOP = 255,
        }

        public static Dictionary<string, string[]> FunctionSubtypes = new Dictionary<string, string[]>()
        {
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.IF), Enum.GetNames(typeof(Lists.IfSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.SET), Enum.GetNames(typeof(Lists.SetSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.AWAIT), Enum.GetNames(typeof(Lists.AwaitSubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.PLAY), Enum.GetNames(typeof(Lists.PlaySubTypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.KILL), Enum.GetNames(typeof(Lists.TargetActorSubtypes)) },
            {Enum.GetName(typeof(Lists.Instructions), (int)Lists.Instructions.CHANGE_SCRIPT), Enum.GetNames(typeof(Lists.ScriptChangeSubtypes)) },
        };

        public static List<string> KeyValues = GetKeyValues();
        private static List<string> GetKeyValues()
        {
            List<string> Values = new List<string>();

            Values.AddRange(Enum.GetNames(typeof(Lists.VarTypes)));
            Values.AddRange(Enum.GetNames(typeof(Lists.SpawnParams)));
            Values.AddRange(Enum.GetNames(typeof(Lists.SpawnPosParams)));
            Values.AddRange(Enum.GetNames(typeof(Lists.TradeStatuses)));
            Values.AddRange(Enum.GetNames(typeof(Lists.PlayerMasks)));
            Values.AddRange(Enum.GetNames(typeof(Lists.LookAtStyles)));
            Values.AddRange(Enum.GetNames(typeof(Lists.DListVisibilityOptions)));
            Values.AddRange(Enum.GetNames(typeof(Lists.BombBags)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Axis)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Quivers)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Wallets)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Scales)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Gauntlets)));
            Values.AddRange(Enum.GetNames(typeof(Lists.StickCap)));
            Values.AddRange(Enum.GetNames(typeof(Lists.NutCap)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Segments)));
            Values.AddRange(Enum.GetNames(typeof(Lists.StickCap)));
            Values.AddRange(Enum.GetNames(typeof(Lists.StickCap)));

            return Values;
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
            CURRENTLY_DAY = 11,
            CURRENTLY_TALKING = 12,
            PLAYER_HAS_EMPTY_BOTTLE = 13,
            CUTSCENE_BEING_PLAYED = 14,
            TEXTBOX_ON_SCREEN = 15,
            CURRENT_ANIM_WALKING = 16,
            CURRENT_ANIM_IDLE = 17,
            PLAYER_HAS_MAGIC = 18,

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
            PLAYER_HEALTH = 41,
            PLAYER_MAGIC = 42,
            PLAYER_BOMBS = 43,
            PLAYER_BOMBCHUS = 44,
            PLAYER_ARROWS = 45,
            PLAYER_DEKUSTICKS = 46,
            PLAYER_DEKUNUTS = 47,

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
            PLAYER_HAS_DEFENSE_UPGRADE = 187,

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
            TIME_OF_DAY = 252,
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

        public enum SpawnParams
        {
            ACTOR_ID,
            POSITION,
            ROTATION,
            VARIABLE,
        }

        public enum SpawnPosParams
        {
            RELATIVE = 1,
            ABSOLUTE = 0,
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
            NONE = 255,
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

        public enum GiveItems
        {
            /* 0x00 */
            ITEM_INVALID, // Link picks up chest and it sends him flying upwards
            /* 0x01 */
            ITEM_BOMBS_5,
            /* 0x02 */
            ITEM_NUTS_5,
            /* 0x03 */
            ITEM_BOMBCHUS_10,
            /* 0x04 */
            ITEM_BOW,
            /* 0x05 */
            ITEM_SLINGSHOT,
            /* 0x06 */
            ITEM_BOOMERANG,
            /* 0x07 */
            ITEM_STICKS_1,
            /* 0x08 */
            ITEM_HOOKSHOT,
            /* 0x09 */
            ITEM_LONGSHOT,
            /* 0x0A */
            ITEM_LENS,
            /* 0x0B */
            ITEM_LETTER_ZELDA,
            /* 0x0C */
            ITEM_OCARINA_OOT,
            /* 0x0D */
            ITEM_HAMMER,
            /* 0x0E */
            ITEM_COJIRO,
            /* 0x0F */
            ITEM_BOTTLE,
            /* 0x10 */
            ITEM_POTION_RED,
            /* 0x11 */
            ITEM_POTION_GREEN,
            /* 0x12 */
            ITEM_POTION_BLUE,
            /* 0x13 */
            ITEM_FAIRY,
            /* 0x14 */
            ITEM_MILK_BOTTLE,
            /* 0x15 */
            ITEM_LETTER_RUTO,
            /* 0x16 */
            ITEM_BEAN,
            /* 0x17 */
            ITEM_MASK_SKULL,
            /* 0x18 */
            ITEM_MASK_SPOOKY,
            /* 0x19 */
            ITEM_CHICKEN, // uses bean message ID
            /* 0x1A */
            ITEM_MASK_KEATON,
            /* 0x1B */
            ITEM_MASK_BUNNY,
            /* 0x1C */
            ITEM_MASK_TRUTH,
            /* 0x1D */
            ITEM_POCKET_EGG,
            /* 0x1E */
            ITEM_POCKET_CUCCO, // uses bean message ID
            /* 0x1F */
            ITEM_ODD_MUSHROOM,
            /* 0x20 */
            ITEM_ODD_POTION,
            /* 0x21 */
            ITEM_SAW,
            /* 0x22 */
            ITEM_SWORD_BROKEN,
            /* 0x23 */
            ITEM_PERSCRIPTION,
            /* 0x24 */
            ITEM_FROG,
            /* 0x25 */
            ITEM_EYEDROPS,
            /* 0x26 */
            ITEM_CLAIM_CHECK,
            /* 0x27 */
            ITEM_SWORD_KOKIRI,
            /* 0x28 */
            ITEM_SWORD_KNIFE,
            /* 0x29 */
            ITEM_SHIELD_DEKU,   // or blue rupee if you have the shield
            /* 0x2A */
            ITEM_SHIELD_HYLIAN, // or blue rupee if you have the shield
            /* 0x2B */
            ITEM_SHIELD_MIRROR,
            /* 0x2C */
            ITEM_TUNIC_GORON, // or blue rupee if you have the tunic
            /* 0x2D */
            ITEM_TUNIC_ZORA,  // or blue rupee if you have the tunic
            /* 0x2E */
            ITEM_BOOTS_IRON,
            /* 0x2F */
            ITEM_BOOTS_HOVER,
            /* 0x30 */
            ITEM_QUIVER_40,
            /* 0x31 */
            ITEM_QUIVER_50,
            /* 0x32 */
            ITEM_BOMB_BAG_20,
            /* 0x33 */
            ITEM_BOMB_BAG_30,
            /* 0x34 */
            ITEM_BOMB_BAG_40,
            /* 0x35 */
            ITEM_GAUNTLETS_SILVER,
            /* 0x36 */
            ITEM_GAUNTLETS_GOLD,
            /* 0x37 */
            ITEM_SCALE_SILVER,
            /* 0x38 */
            ITEM_SCALE_GOLD,
            /* 0x39 */
            ITEM_STONE_OF_AGONY,
            /* 0x3A */
            ITEM_GERUDO_CARD,
            /* 0x3B */
            ITEM_OCARINA_FAIRY, // uses Ocarina of Time message ID
            /* 0x3C */
            ITEM_SEEDS_5,
            /* 0x3D */
            ITEM_HEART_CONTAINER,
            /* 0x3E */
            ITEM_HEART_PIECE,
            /* 0x3F */
            ITEM_KEY_BOSS,
            /* 0x40 */
            ITEM_COMPASS,
            /* 0x41 */
            ITEM_MAP,
            /* 0x42 */
            ITEM_KEY_SMALL,
            /* 0x43 */
            ITEM_MAGIC_SMALL, // or blue rupee if not from a drop
            /* 0x44 */
            ITEM_MAGIC_LARGE, // or blue rupee if not from a drop
            /* 0x45 */
            ITEM_WALLET_ADULT,
            /* 0x46 */
            ITEM_WALLET_GIANT,
            /* 0x47 */
            ITEM_WEIRD_EGG,
            /* 0x48 */
            ITEM_HEART,
            /* 0x49 */
            ITEM_ARROWS_SMALL,  // amount changes depending on context
            /* 0x4A */
            ITEM_ARROWS_MEDIUM, // amount changes depending on context
            /* 0x4B */
            ITEM_ARROWS_LARGE,  // amount changes depending on context
            /* 0x4C */
            ITEM_RUPEE_GREEN,
            /* 0x4D */
            ITEM_RUPEE_BLUE,
            /* 0x4E */
            ITEM_RUPEE_RED,
            /* 0x4F */
            ITEM_HEART_CONTAINER_2,
            /* 0x50 */
            ITEM_MILK,
            /* 0x51 */
            ITEM_MASK_GORON,
            /* 0x52 */
            ITEM_MASK_ZORA,
            /* 0x53 */
            ITEM_MASK_GERUDO,
            /* 0x54 */
            ITEM_BRACELET,
            /* 0x55 */
            ITEM_RUPEE_PURPLE,
            /* 0x56 */
            ITEM_RUPEE_GOLD,
            /* 0x57 */
            ITEM_SWORD_BGS,
            /* 0x58 */
            ITEM_ARROW_FIRE,
            /* 0x59 */
            ITEM_ARROW_ICE,
            /* 0x5A */
            ITEM_ARROW_LIGHT,
            /* 0x5B */
            ITEM_SKULL_TOKEN,
            /* 0x5C */
            ITEM_DINS_FIRE,
            /* 0x5D */
            ITEM_FARORES_WIND,
            /* 0x5E */
            ITEM_NAYRUS_LOVE,
            /* 0x5F */
            ITEM_BULLET_BAG_30,
            /* 0x60 */
            ITEM_BULLET_BAG_40,
            /* 0x61 */
            ITEM_STICKS_5,
            /* 0x62 */
            ITEM_STICKS_10,
            /* 0x63 */
            ITEM_NUTS_5_2,
            /* 0x64 */
            ITEM_NUTS_10,
            /* 0x65 */
            ITEM_BOMBS_1,
            /* 0x66 */
            ITEM_BOMBS_10,
            /* 0x67 */
            ITEM_BOMBS_20,
            /* 0x68 */
            ITEM_BOMBS_30,
            /* 0x69 */
            ITEM_SEEDS_30,
            /* 0x6A */
            ITEM_BOMBCHUS_5,
            /* 0x6B */
            ITEM_BOMBCHUS_20,
            /* 0x6C */
            ITEM_FISH,
            /* 0x6D */
            ITEM_BUGS,
            /* 0x6E */
            ITEM_BLUE_FIRE,
            /* 0x6F */
            ITEM_POE,
            /* 0x70 */
            ITEM_BIG_POE,
            /* 0x71 */
            ITEM_DOOR_KEY,          // specific to chest minigame
            /* 0x72 */
            ITEM_RUPEE_GREEN_LOSE,  // specific to chest minigame
            /* 0x73 */
            ITEM_RUPEE_BLUE_LOSE,   // specific to chest minigame
            /* 0x74 */
            ITEM_RUPEE_RED_LOSE,    // specific to chest minigame
            /* 0x75 */
            ITEM_RUPEE_PURPLE_LOSE, // specific to chest minigame
            /* 0x76 */
            ITEM_HEART_PIECE_WIN,   // specific to chest minigame
            /* 0x77 */
            ITEM_STICK_UPGRADE_20,
            /* 0x78 */
            ITEM_STICK_UPGRADE_30,
            /* 0x79 */
            ITEM_NUT_UPGRADE_30,
            /* 0x7A */
            ITEM_NUT_UPGRADE_40,
            /* 0x7B */
            ITEM_BULLET_BAG_50,
            /* 0x7C */
            ITEM_ICE_TRAP, // freezes link when opened from a chest
            /* 0x7D */
            ITEM_TEXT_0 // no model appears over Link, shows text id 0 (pocket egg)
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
    }
}
