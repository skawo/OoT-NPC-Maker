using System;
using System.Collections.Generic;

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
        public const string Keyword_Elif = "ELIF";
        public const string Keyword_CallProcedure = "::";
        public const string Keyword_EndProcedure = "ENDPROC";
        public const string Keyword_End = "END";
        public const string Keyword_EndIf = "ENDIF";
        public const string Keyword_EndWhile = "ENDWHILE";
        public const string Keyword_EndSpawn = "ENDSPAWN";
        public const string Keyword_EndParticle = "ENDPARTICLE";
        public const string Keyword_EndTalk = "ENDTALK";
        public const string Keyword_EndOcarina = "ENDOCARINA";
        public const string Keyword_EndTrade = "ENDTRADE";
        public const string Keyword_TradeDefault = "DEFAULT";
        public const string Keyword_TradeSucccess = "SUCCESS";
        public const string Keyword_TradeFailure = "FAILURE";
        public const string Keyword_EndTradeFailure = "ENDFAILURE";
        public const string Keyword_TradeNone = "TALKED_TO";
        public const string Keyword_Else = "ELSE";
        public const string Keyword_Or = "OR";
        public const string Keyword_And = "AND";

        /*
        public const string Keyword_ScriptVar = "VAR";
        public const string Keyword_ScriptVarF = "VARF";
        public const string Keyword_RNG = "RANDOM";
        public const string Keyword_GlobalF = "GLOBALF";
        public const string Keyword_ActorF = "ACTORF";
        public const string Keyword_Global8 = "GLOBAL8";
        public const string Keyword_Global16 = "GLOBAL16";
        public const string Keyword_Global32 = "GLOBAL32";
        public const string Keyword_Actor8 = "ACTOR8";
        public const string Keyword_Actor16 = "ACTOR16";
        public const string Keyword_Actor32 = "ACTOR32";
        public const string Keyword_Save8 = "SAVEF8";
        public const string Keyword_Save16 = "SAVE16";
        public const string Keyword_Save32 = "SAVE32";
        public const string Keyword_SaveF = "SAVEF";
        */

        public const string Keyword_Degree = "DEG_";
        public const string Keyword_Once = "ONCE";
        public const string Keyword_Ignore_Y = "IGNORE_Y";
        public const string Keyword_Debug_Skip_Label_Check = "__SKIPCHECK__";
        public const string Keyword_Label_Return = "__RETURN__";
        public const string Keyword_Label_Null = "__NULL__";

        public const int Num_User_Vars = 255;

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
            Keyword_EndParticle,
            Keyword_EndTalk,
            Keyword_EndTrade,
            Keyword_TradeDefault,
            Keyword_TradeSucccess,
            Keyword_TradeNone,
            Keyword_Else,
            Keyword_And,
            Keyword_Or,
            Keyword_CallProcedure,
            Keyword_EndOcarina,
            Keyword_Once,

            VarTypes.NORMAL.ToString(),
            VarTypes.RANDOM.ToString(),
            VarTypes.GLOBAL8.ToString(),
            VarTypes.GLOBAL16.ToString(),
            VarTypes.GLOBAL32.ToString(),
            VarTypes.GLOBALF.ToString(),
            VarTypes.ACTOR8.ToString(),
            VarTypes.ACTOR16.ToString(),
            VarTypes.ACTOR32.ToString(),
            VarTypes.ACTORF.ToString(),
            VarTypes.SAVE8.ToString(),
            VarTypes.SAVE16.ToString(),
            VarTypes.SAVE32.ToString(),
            VarTypes.SAVEF.ToString(),
            VarTypes.VAR.ToString(),
            VarTypes.VARF.ToString(),
        };

        public static List<string> KeywordsBlue = new List<string>()
        {
            Keyword_True,
            Keyword_False,
        };

        public static List<string> KeywordsPurple = new List<string>()
        {
            Keyword_Else,
            Keyword_Elif,
        };

        public static List<string> KeywordsMPurple = new List<string>()
        {
            Keyword_EndIf,
            Keyword_EndWhile,
            Keyword_EndSpawn,
            Keyword_EndTalk,
            Keyword_EndTrade,
            Keyword_EndParticle,
        };

        public static List<string> KeywordsRed = new List<string>()
        {
            Keyword_Procedure,
            Keyword_EndProcedure,
            Keyword_Return,
        };

        public static List<string> KeywordsGray = new List<string>()
        {
            
            Keyword_And,
            Keyword_Or,
            Keyword_TradeSucccess,
            Keyword_TradeFailure,
            Keyword_TradeNone,
            Keyword_EndTradeFailure,

            ParticleSubOptions.POSITION.ToString(),
            ParticleSubOptions.ACCELERATION.ToString(),
            ParticleSubOptions.VELOCITY.ToString(),
            ParticleSubOptions.COLOR1.ToString(),
            ParticleSubOptions.COLOR2.ToString(),
            ParticleSubOptions.SCALE.ToString(),
            ParticleSubOptions.SCALE_UPDATE.ToString(),
            ParticleSubOptions.SCALE_UPDATE_DOWN.ToString(),
            ParticleSubOptions.DURATION.ToString(),
            ParticleSubOptions.COUNT.ToString(),
            ParticleSubOptions.YAW.ToString(),
            ParticleSubOptions.DLIST.ToString(),
            ParticleSubOptions.LIGHTPOINT_COLOR.ToString(),
            ParticleSubOptions.SPOTTED.ToString(),
            ParticleSubOptions.VARIABLE.ToString(),
            ParticleSubOptions.RANDOMIZE_XZ.ToString(),
            ParticleSubOptions.OPACITY.ToString(),
            ParticleSubOptions.SCORE_AMOUNT.ToString(),
            ParticleSubOptions.FADE_DELAY.ToString(),

            SpawnParams.VARIABLE.ToString(),
            SpawnParams.ROTATION.ToString(),

            VarTypes.NORMAL.ToString(),
            VarTypes.RANDOM.ToString(),
            VarTypes.GLOBAL8.ToString(),
            VarTypes.GLOBAL16.ToString(),
            VarTypes.GLOBAL32.ToString(),
            VarTypes.GLOBALF.ToString(),
            VarTypes.ACTOR8.ToString(),
            VarTypes.ACTOR16.ToString(),
            VarTypes.ACTOR32.ToString(),
            VarTypes.ACTORF.ToString(),
            VarTypes.SAVE8.ToString(),
            VarTypes.SAVE16.ToString(),
            VarTypes.SAVE32.ToString(),
            VarTypes.SAVEF.ToString(),
            VarTypes.VAR.ToString(),
            VarTypes.VARF.ToString(),
        };

        public enum VarTypes
        {
            NORMAL = 0,
            RANDOM = 1,
            GLOBAL8 = 2,
            GLOBAL16 = 3,
            GLOBAL32 = 4,
            GLOBALF = 5,
            ACTOR8 = 6,
            ACTOR16 = 7,
            ACTOR32 = 8,
            ACTORF = 9,
            SAVE8 = 10,
            SAVE16 = 11,
            SAVE32 = 12,
            SAVEF = 13,
            VAR = 14,
            VARF = 15,
        }

        public enum Instructions
        {
            IF,
            WHILE,
            AWAIT,
            SET,
            TALK,
            TRADE,
            ENABLE_TALKING,
            SHOW_TEXTBOX,
            ITEM,
            PLAY ,
            KILL,
            SPAWN,
            WARP,
            ROTATION,
            POSITION,
            SCALE,
            FACE,
            PARTICLE,
            OCARINA,
            RETURN,
            GOTO,
            LABEL,
            NOP,
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
            Values.AddRange(Enum.GetNames(typeof(Lists.EffectsIfAttacked)));

            return Values;
        }

        public enum DictType
        {
            SFX,
            Music,
            Actors,
            Objects,
            LinkAnims,
        }

        public enum IfWhileAwaitSetRamSubTypes
        {
            GLOBAL8 = 242,
            GLOBAL16 = 243,
            GLOBAL32 = 244,
            GLOBALF = 245,
            ACTOR8 = 246,
            ACTOR16 = 247,
            ACTOR32 = 248,
            ACTORF = 249,
            SAVE8 = 250,
            SAVE16 = 251,
            SAVE32 = 252,
            SAVEF = 253,
            VAR = 254,
            VARF = 255,
        }
        
        public enum IfSubTypes
        {
            FLAG_INF,
            FLAG_EVENT,
            FLAG_SWITCH,
            FLAG_SCENE,
            FLAG_TREASURE,
            FLAG_ROOM_CLEAR,
            FLAG_SCENE_COLLECT,
            FLAG_TEMPORARY,

            LINK_IS_ADULT,
            IS_DAY,
            IS_TALKING,
            PLAYER_HAS_EMPTY_BOTTLE,
            IN_CUTSCENE,
            TEXTBOX_ON_SCREEN,
            TEXTBOX_DRAWING,
            PLAYER_HAS_MAGIC,
            ATTACKED,

            PLAYER_RUPEES,
            SCENE_ID,
            PLAYER_SKULLTULAS,
            PATH_NODE,
            ANIMATION_FRAME,
            CUTSCENE_FRAME,
            PLAYER_HEALTH,
            PLAYER_BOMBS,
            PLAYER_BOMBCHUS,
            PLAYER_ARROWS,
            PLAYER_DEKUNUTS,
            PLAYER_DEKUSTICKS,
            PLAYER_BEANS,
            PLAYER_SEEDS,
            EXT_VAR,

            STICK_X,
            STICK_Y,

            ITEM_BEING_TRADED,
            TRADE_STATUS,
            PLAYER_MASK,
            TIME_OF_DAY,
            ANIMATION,
            PLAYER_HAS_INVENTORY_ITEM,
            PLAYER_HAS_QUEST_ITEM,
            PLAYER_HAS_DUNGEON_ITEM,
            BUTTON_PRESSED,
            BUTTON_HELD,
            TARGETTED,

            DISTANCE_FROM_PLAYER,
            LENS_OF_TRUTH_ON,
        }

        public enum ItemSubTypes
        {
            AWARD,
            GIVE,
            TAKE,
        }

        public enum SetSubTypes
        {
            TARGET_LIMB,
            TARGET_DISTANCE,
            HEAD_LIMB,
            WAIST_LIMB,
            LOOKAT_TYPE,
            HEAD_VERT_AXIS,
            HEAD_HORIZ_AXIS,
            WAIST_VERT_AXIS,
            WAIST_HORIZ_AXIS,
            CUTSCENE_SLOT,
            BLINK_SEGMENT,
            TALK_SEGMENT,
            ALPHA,

            MOVEMENT_DISTANCE,
            MOVEMENT_LOOP_DELAY,
            ATTACKED_SFX,
            LIGHT_RADIUS,
            CUTSCENE_FRAME,

            COLLISION_RADIUS,
            COLLISION_HEIGHT,
            MOVEMENT_LOOP_START,
            MOVEMENT_LOOP_END,
            COLLISION_YOFFSET,
            TARGET_OFFSET_X,
            TARGET_OFFSET_Y,
            TARGET_OFFSET_Z,
            MODEL_OFFSET_X,
            MODEL_OFFSET_Y,
            MODEL_OFFSET_Z,
            CAMERA_ID,
            LOOKAT_OFFSET_X,
            LOOKAT_OFFSET_Y,
            LOOKAT_OFFSET_Z,
            CURRENT_PATH_NODE,
            CURRENT_ANIMATION_FRAME,
            LIGHT_OFFSET_X,
            LIGHT_OFFSET_Y,
            LIGHT_OFFSET_Z,
            TIMED_PATH_START_TIME,
            TIMED_PATH_END_TIME,

            MOVEMENT_SPEED,
            TALK_RADIUS,
            SMOOTHING_CONSTANT,
            SHADOW_RADIUS,

            LOOP_MOVEMENT,
            HAS_COLLISION,
            DO_BLINKING_ANIMATIONS,
            DO_TALKING_ANIMATIONS,
            JUST_SCRIPT,
            OPEN_DOORS,
            MOVEMENT_IGNORE_Y,
            FADES_OUT,
            LIGHT_GLOW,
            PAUSE_CUTSCENE,
            INVISIBLE,
            CASTS_SHADOW,
            NO_AUTO_ANIM,
            TALK_MODE,


            PLAYER_BOMBS,
            PLAYER_BOMBCHUS,
            PLAYER_ARROWS,
            PLAYER_DEKUNUTS,
            PLAYER_DEKUSTICKS,
            PLAYER_BEANS,
            PLAYER_SEEDS,
            PLAYER_RUPEES,
            PLAYER_HEALTH,

            ENV_COLOR,
            LIGHT_COLOR,

            RESPONSE_ACTIONS,

            ANIMATION_OBJECT,
            ANIMATION_OFFSET,
            ANIMATION_SPEED,
            ANIMATION_STARTFRAME,
            ANIMATION_ENDFRAME,
            FLAG_INF,
            FLAG_EVENT,
            FLAG_SWITCH,
            FLAG_SCENE,
            FLAG_TREASURE,
            FLAG_ROOM_CLEAR,
            FLAG_SCENE_COLLECT,
            FLAG_TEMPORARY,

            MASS,

            PRESS_SWITCHES,
            IS_TARGETTABLE,
            VISIBLE_ONLY_UNDER_LENS,
            IS_ALWAYS_ACTIVE,
            IS_ALWAYS_DRAWN,
            REACTS_IF_ATTACKED,

            GRAVITY_FORCE,
            MOVEMENT_PATH_ID,
            PLAYER_CAN_MOVE,
            ACTOR_CAN_MOVE,
            ANIMATION,
            ANIMATION_INSTANTLY,
            SCRIPT_START,
            BLINK_PATTERN,
            TALK_PATTERN,
            SEGMENT_ENTRY,
            DLIST_VISIBILITY,
            CAMERA_TRACKING_ON,
            EXT_VAR,
            TIME_OF_DAY,
            ATTACKED_EFFECT,
            MOVEMENT_TYPE,
            GENERATES_LIGHT,
            REF_ACTOR,
            PLAYER_ANIMATION,
            PLAYER_ANIMATE_MODE,
        }

        public enum AwaitSubTypes
        {
            MOVEMENT_PATH_END,
            RESPONSE,
            TALKING_END,
            TEXTBOX_ON_SCREEN,
            FOREVER,

            PATH_NODE,
            FRAMES,
            ANIMATION_FRAME,
            CUTSCENE_FRAME,
            TIME_OF_DAY,

            STICK_X,
            STICK_Y,

            BUTTON_PRESSED,
            BUTTON_HELD,
            TEXTBOX_NUM,
            TEXTBOX_DISMISSED,
            TEXTBOX_DRAWING,

            ANIMATION_END,
            PLAYER_ANIMATION_END,
            EXT_VAR,
        }

        public enum EffectsIfAttacked
        {
            BLUE_WHITE = 0,
            NONE_DUST = 1,
            GREEN_DUST = 2,
            NONE_WHITE = 3,
            WATER_NONE = 4,
            NONE_RED = 5,
            GREEN_WHITE = 6,
            RED_WHITE = 7,
            BLUE_RED = 8,
            METAL = 9,
            NONE = 10,
            WOOD = 11,
            HARD_SURFACE = 12,
            TREE = 13,
        }

        public enum SpawnParams
        {
            POSITION,
            ROTATION,
            VARIABLE,
        }

        public enum SpawnPosParams
        {
            ABSOLUTE = 0,
            RELATIVE = 1,
            DIRECTION = 2,
        }

        public enum ConditionTypes
        {
            EQUALTO = 0,
            TRUE = 1,
            LESSTHAN = 1,
            FALSE = 0,
            MORETHAN = 2,
            LESSOREQ = 3,
            MOREOREQ = 4,
            NOTEQUAL = 5,
            NONE = 6,
        }

        public enum TradeItems
        {
            /* 0x01 */
            EXCH_ITEM_LETTER_ZELDA = 1,
            /* 0x02 */
            EXCH_ITEM_WEIRD_EGG = 2,
            /* 0x03 */
            EXCH_ITEM_CHICKEN = 3,
            /* 0x04 */
            EXCH_ITEM_BEAN = 4,
            /* 0x05 */
            EXCH_ITEM_POCKET_EGG = 5,
            /* 0x06 */
            EXCH_ITEM_POCKET_CUCCO = 6,
            /* 0x07 */
            EXCH_ITEM_COJIRO = 7,
            /* 0x08 */
            EXCH_ITEM_ODD_MUSHROOM = 8,
            /* 0x09 */
            EXCH_ITEM_ODD_POTION = 9,
            /* 0x0A */
            EXCH_ITEM_SAW = 10,
            /* 0x0B */
            EXCH_ITEM_SWORD_BROKEN = 11,
            /* 0x0C */
            EXCH_ITEM_PRESCRIPTION = 12,
            /* 0x0D */
            EXCH_ITEM_FROG = 13,
            /* 0x0E */
            EXCH_ITEM_EYEDROPS = 14,
            /* 0x0F */
            EXCH_ITEM_CLAIM_CHECK = 15,
            /* 0x10 */
            EXCH_ITEM_MASK_SKULL = 16,
            /* 0x11 */
            EXCH_ITEM_MASK_SPOOKY = 17,
            /* 0x12 */
            EXCH_ITEM_MASK_KEATON = 18,
            /* 0x13 */
            EXCH_ITEM_MASK_BUNNY = 19,
            /* 0x14 */
            EXCH_ITEM_MASK_TRUTH = 20,
            /* 0x15 */
            EXCH_ITEM_MASK_GORON = 21,
            /* 0x16 */
            EXCH_ITEM_MASK_ZORA = 22,
            /* 0x17 */
            EXCH_ITEM_MASK_GERUDO = 23,
            /* 0x18 */
            EXCH_ITEM_FISH = 24,
            /* 0x19 */
            EXCH_ITEM_BLUE_FIRE = 25,
            /* 0x1A */
            EXCH_ITEM_BUG = 26,
            /* 0x1B */
            EXCH_ITEM_POE = 27,
            /* 0x1C */
            EXCH_ITEM_BIG_POE = 28,
            /* 0x1D */
            EXCH_ITEM_LETTER_RUTO = 29,

            EXCH_ITEM_NONE = 30
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

            ITEM_TRADE_ADULT = ITEM_POCKET_EGG,
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

        public enum ParticleTypes
        {
            DUST,
            EXPLOSION,
            RING,
            SPARK,
            LIGHTNING,
            BUBBLE,
            DISPLAY_LIST,
            WATER_SPLASH,
            SMOKE,
            HIT_MARK_FLASH,
            HIT_MARK_DUST,
            HIT_MARK_BURST,
            HIT_MARK_SPARK,
            LIGHT_POINT,
            RED_FLAME,
            BLUE_FLAME,
            SEARCH_EFFECT,
            ICE_CHUNK,
            ICE_BURST,
            SCORE,
            FLAME,
            BURN_MARK,
            DODONGO_FIRE,
            FREEZARD_SMOKE,
            ELECTRICITY,
            FOCUSED_STAR,
            DISPERSED_STAR,
            FIRE_TAIL,
        }

        public enum ParticleSubOptions
        {
            POSITION,
            VELOCITY,
            ACCELERATION,
            COLOR1,
            COLOR2,
            SCALE,
            SCALE_UPDATE,
            SCALE_UPDATE_DOWN,
            DURATION,
            COUNT,
            YAW,
            DLIST,
            LIGHTPOINT_COLOR,
            SPOTTED,
            OPACITY,
            VARIABLE,
            RANDOMIZE_XZ,
            SCORE_AMOUNT,
            FADE_DELAY,
        }

        public enum LightPointColors
        {
            WHITE = 0,
            BLUE = 1,
            RED = 2,
            YELLOW = 3,
            PURPLE = 4,
            PINK = 5,
            ORANGE = 6,
            GRAY = 7
        }

        public enum OcarinaSongs
        {
            MINUET_OF_FOREST = 0,
            BOLERO_OF_FIRE = 1,
            SERENADE_OF_WATER = 2,
            REQUIEM_OF_SPIRIT = 3,
            NOCTURNE_OF_SHADOW = 4,
            PRELUDE_OF_LIGHT = 5,

            SARIAS_SONG = 0x22,
            EPONAS_SONG = 0x23,
            ZELDAS_LULLABY = 0x24,
            SUNS_SONG = 0x25,
            SONG_OF_TIME = 0x26,
            SONG_OF_STORMS = 0x27,
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
            SUCCESS,
            TALKED_TO,
            FAILURE,
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
            ROAM = 1,
            FOLLOW = 2,
            RUN_AWAY = 3,
            PATH = 4,
            TIMED_PATH = 5,
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
            SFX,
            BGM,
            CUTSCENE,
            CUTSCENE_ID
        }

        public enum RotationSubTypes
        {
            SET,
            ROTATE_TO,
            ROTATE_BY,
        }

        public enum PositionSubTypes
        {
            SET,
            MOVE_TO,
            MOVE_BY,
            DIRECTION_MOVE_BY,
        }

        public enum ScaleSubTypes
        {
            SET,
            SCALE_TO,
            SCALE_BY,
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
            TOWARDS,
            AND,
            AWAY_FROM,
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
            NPCMAKER = 2,
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

        public enum MsgIcon
        {
            DEKU_STICK,
            DEKU_NUT,
            BOMBS,
            BOW,
            FIRE_ARROWS,
            DINS_FIRE,
            SLINGSHOT,
            FAIRY_OCARINA,
            OCARINA_OF_TIME,
            BOMBCHUS,
            HOOKSHOT,
            LONGSHOT,
            ICE_ARROWS,
            FARORES_WIND,
            BOOMERANG,
            LENS_OF_TRUTH,
            BEANS,
            MEGATON_HAMMER,
            LIGHT_ARROWS,
            NAYRUS_LOVE,
            EMPTY_BOTTLE,
            RED_POTION,
            GREEN_POTION,
            BLUE_POTION,
            FAIRY,
            FISH,
            MILK,
            RUTOS_LETTER,
            BLUE_FIRE,
            BOTTLE_BUG,
            BOTTLE_POE,
            HALF_MILK,
            BOTTLE_BIGPOE,
            WEIRD_EGG,
            CHICKEN,
            ZELDAS_LETTER,
            KEATON_MASK,
            SKULL_MASK,
            REDEAD_MASK,
            BUNNY_HOOD,
            GORON_MASK,
            ZORA_MASK,
            GERUDO_MASK,
            MASK_OF_TRUTH,
            SOLD_OUT,
            POCKET_EGG,
            POCKET_CUCCO,
            COJIRO,
            ODD_MUSHROOM,
            MEDICINE,
            POACHERS_SAW,
            BROKEN_SWORD,
            PRESCRIPTION,
            EYEBALL_FROG,
            EYEDROPS,
            CLAIM_CHECK,
            BOW_FIRE,
            BOW_ICE,
            BOW_LIGHT,
            KOKIRI_SWORD,
            MASTER_SWORD,
            BIGGORON_SWORD,
            DEKU_SHIELD,
            HYLIAN_SHIELD,
            MIRROR_SHIELD,
            KOKIRI_TUNIC,
            GORON_TUNIC,
            ZORA_TUNIC,
            BOOTS,
            IRON_BOOTS,
            PEGASUS_BOOTS,
            SEED_SATCHEL,
            BIGGER_SEED_SATCHEL,
            BIGGEST_SEED_SATCHEL,
            QUIVER,
            BIG_QUIVER,
            BIGGEST_QUIVER,
            BOMB_BAG,
            BIGGER_BOMB_BAG,
            BIGGEST_BOMB_BAG,
            GORON_BRACELET,
            SILVER_GAUNTLETS,
            GOLDEN_GAUNTLETS,
            ZORA_SCALE,
            GOLDEN_SCALE,
            BROKEN_KNIFE,
            WALLET,
            ADULTS_WALLET,
            GIANTS_WALLET,
            DEKU_SEEDS,
            FISHING_ROD,
            NOTHING_1,
            NOTHING_2,
            NOTHING_3,
            NOTHING_4,
            NOTHING_5,
            NOTHING_6,
            NOTHING_7,
            NOTHING_9,
            NOTHING_10,
            NOTHING_11,
            NOTHING_12,
            FOREST_MEDALLION,
            FIRE_MEDALLION,
            WATER_MEDALLION,
            SPIRIT_MEDALLION,
            SHADOW_MEDALLION,
            LIGHT_MEDALLION,
            KOKIRI_EMERALD,
            GORON_RUBY,
            ZORA_SAPPHIRE,
            STONE_OF_AGONY,
            GERUDO_PASS,
            GOLDEN_SKULLTULA,
            HEART_CONTAINER,
            HEART_PIECE,
            BOSS_KEY,
            COMPASS,
            DUNGEON_MAP,
            SMALL_KEY,
            MAGIC_JAR,
            BIG_MAGIC_JAR,
        }

        public enum MsgControlCode
        {
            LINE_BREAK = 0x01,
            END = 0x02,
            NEW_BOX = 0x04,
            COLOR = 0x05,
            SHIFT = 0x06,
            JUMP = 0x07,
            DI = 0x08,
            DC = 0x09,
            SHOP_DESCRIPTION = 0x0A,
            EVENT = 0x0B,
            DELAY = 0x0C,
            AWAIT_BUTTON = 0x0D,
            FADE = 0x0E,
            PLAYER = 0x0F,
            OCARINA = 0x10,
            FADE2 = 0x11,
            SOUND = 0x12,
            ICON = 0x13,
            SPEED = 0x14,
            BACKGROUND = 0x15,
            MARATHON_TIME = 0x16,
            RACE_TIME = 0x17,
            POINTS = 0x18,
            GOLD_SKULLTULAS = 0x19,
            NS = 0x1A,
            TWO_CHOICES = 0x1B,
            THREE_CHOICES = 0x1C,
            FISH_WEIGHT = 0x1D,
            HIGH_SCORE = 0x1E,
            TIME = 0x1F,

            DASH = 0x7F,
            À = 0x80,
            Î = 0x81,
            Â = 0x82,
            Ä = 0x83,
            Ç = 0x84,
            È = 0x85,
            É = 0x86,
            Ê = 0x87,
            Ë = 0x88,
            Ï = 0x89,
            Ô = 0x8A,
            Ö = 0x8B,
            Ù = 0x8C,
            Û = 0x8D,
            Ü = 0x8E,
            ß = 0x8F,
            à = 0x90,
            á = 0x91,
            â = 0x92,
            ä = 0x93,
            ç = 0x94,
            è = 0x95,
            é = 0x96,
            ê = 0x97,
            ë = 0x98,
            ï = 0x99,
            ô = 0x9A,
            ö = 0x9B,
            ù = 0x9C,
            û = 0x9D,
            ü = 0x9E,

            A_BUTTON = 0x9F,
            B_BUTTON = 0xA0,
            C_BUTTON = 0xA1,
            L_BUTTON = 0xA2,
            R_BUTTON = 0xA3,
            Z_BUTTON = 0xA4,
            C_UP = 0xA5,
            C_DOWN = 0xA6,
            C_LEFT = 0xA7,
            C_RIGHT = 0xA8,
            TRIANGLE = 0xA9,
            CONTROL_STICK = 0xAA,
            D_PAD = 0xAB
        }

        public enum MsgColor
        {
            W = 0x40,
            R = 0x41,
            G = 0x42,
            B = 0x43,
            C = 0x44,
            M = 0x45,
            Y = 0x46,
            BLK = 0x47
        }

        public enum MsgHighScore
        {
            ARCHERY = 0x00,
            POE_POINTS = 0x01,
            FISHING = 0x02,
            HORSE_RACE = 0x03,
            MARATHON = 0x04,
            DAMPE_RACE = 0x06
        }
    }
}
