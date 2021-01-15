using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace NPC_Maker
{
    public static class Lists
    {

        public static Dictionary<string, int> SFXes = FileOps.GetDictionary($"{Program.ExecPath}/SFX.csv");
        public static Dictionary<string, int> Music = FileOps.GetDictionary($"{Program.ExecPath}/Music.csv");
        public static Dictionary<string, int> Actors = FileOps.GetDictionary($"{Program.ExecPath}/Actors.csv");

        public static Dictionary<string, string[]> FunctionSubtypes = new Dictionary<string, string[]>()
        {
            {Enum.GetName(typeof(Lists.InstructionIDs), (int)Lists.InstructionIDs.IF), Enum.GetNames(typeof(Lists.IfSubTypes)) },
            {Enum.GetName(typeof(Lists.InstructionIDs), (int)Lists.InstructionIDs.SET), Enum.GetNames(typeof(Lists.SetSubTypes)) },
            {Enum.GetName(typeof(Lists.InstructionIDs), (int)Lists.InstructionIDs.WAITFOR), Enum.GetNames(typeof(Lists.WaitForSubTypes)) },
            {Enum.GetName(typeof(Lists.InstructionIDs), (int)Lists.InstructionIDs.PLAY), Enum.GetNames(typeof(Lists.PlaySubtypes)) },
            {Enum.GetName(typeof(Lists.InstructionIDs), (int)Lists.InstructionIDs.KILL), Enum.GetNames(typeof(Lists.KillSubtypes)) },
            {Enum.GetName(typeof(Lists.InstructionIDs), (int)Lists.InstructionIDs.TURN), Enum.GetNames(typeof(Lists.TurnTypeSubtypes)) },
            {Enum.GetName(typeof(Lists.InstructionIDs), (int)Lists.InstructionIDs.SCRIPT_CHANGE), Enum.GetNames(typeof(Lists.ScriptOverwriteTypes)) },
        };

        public static List<string> Keywords = new List<string>()
        {
            "true",
            "false",
            "return",
            "stop",
            "next"
        };
        public static List<string> KeywordsDarkGray = new List<string>()
        {
            "then",
            "else"
        };
        public static List<string> KeyValues = GetKeyValues();
        private static List<string> GetKeyValues()
        {
            List<string> Values = new List<string>();

            Values.AddRange(Enum.GetNames(typeof(Lists.MovementStyles)));
            Values.AddRange(Enum.GetNames(typeof(Lists.DListVisibilityTypes)));
            Values.AddRange(Enum.GetNames(typeof(Lists.LookTypes)));
            Values.AddRange(Enum.GetNames(typeof(Lists.Segments)));

            return Values;
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
            SEG_F = 7

        }
        public enum MovementStyles
        {
            none = 0,
            random = 1,
            follow = 2,
            path_collisionwise = 3,
            path_direct = 4
        }
        public enum DListVisibilityTypes
        {
            invisible = 0,
            at_limb = 1,
            instead_of_limb = 2,
        }
        public enum LookTypes
        {
            none = 0,
            body = 1,
            head = 2,
        }
        public enum InstructionIDs
        {
            NOP = 0,
            IF = 1,
            SET = 2,
            WAITFOR = 3,
            ENABLE_TEXTBOX = 4,
            SHOW_TEXTBOX = 5,
            GIVE_ITEM = 6,
            GOTO = 7,
            TURN = 8,
            PLAY = 9,
            KILL = 10,
            ENABLE_TRADE = 11,
            SCRIPT_CHANGE = 12,
            RNG = 13,
            RETURN = 255,
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
        public enum IfSubTypes
        {
            inf_table = 0,
            event_chk_inf = 1,
            switch_table = 2,
            uscene = 3,
            treasure = 4,
            room_clear = 5,
            scene_collect = 6,
            temporary = 7,

            age = 10,
            day = 11,
            talking = 12,
            has_empty_bottle = 13,
            cutscene_being_played = 14,
            textbox_on_screen = 15,
            walking = 16,
            idle = 17,

            rupees = 30,
            time_of_day = 31,
            scene_id = 32,
            worn_mask = 33,
            skulltulas = 34,
            current_path_node = 35,
            current_animation_frame = 36,
            current_cutscene_frame = 37,
            current_animation_id = 38,

            item_being_traded = 61,
            trade_status = 62,
            script_var = 63,
        }
        public enum SetSubTypes
        {
            /* u16 Subtypes */
            movement_distance = 0,
            loop_delay = 1,
            collision_radius = 2,
            collision_height = 3,
            path_id = 6,
            time_of_day = 7,
            incorrect_trade_text_id = 8,
            cutscene_frame = 9,

            /* s16 Subtypes */
            loop_start = 35,
            loop_end = 36,
            collision_offset_x = 37,
            collision_offset_y = 38,
            collision_offset_z = 39,
            target_offset_x = 40,
            target_offset_y = 41,
            target_offset_z = 42,
            model_offset_x = 43,
            model_offset_y = 44,
            model_offset_z = 45,
            rupees = 46,
            camera_id = 47,
            lookat_offset_x = 48,
            lookat_offset_y = 49,
            lookat_offset_z = 50,

            /* u32 Subtypes */

            /* s32 Subtypes */

            /* Float Subtypes */
            model_scale = 140,
            movement_speed = 141,
            model_scale_smoothly = 142,

            /* u8 and bool Subtypes */
            do_loop = 175,
            collision = 176,
            switches = 178,
            pushable = 179,
            targettable = 180,
            player_movement = 181,
            movement = 182,
            do_blinking_anim = 183,
            do_talking_anim = 184,
            always_active = 185,
            target_limb = 188,
            target_dist = 189,
            head_limb = 190,
            waist_limb = 191,
            pause_cutscene = 192,
            always_drawn = 193,

            /* s8 Subtypes */


            /* Special handling */
            responses = 230,
            flag = 231,
            movement_type = 234,
            look_type = 235,
            head_axis = 236,
            animation = 237,
            animation_object = 238,
            animation_offset = 239,
            animation_speed = 240,
            script_start = 241,
            blink_pattern = 242,
            talk_pattern = 243,
            segment_tex = 244,
            env_color = 245,
            dlist_show = 246,
            animation_keyframes = 247,
            camera_tracking_on = 248,
            script_var = 249,
            animation_instantly = 250,
            waist_axis = 251,
            cutscene_slot = 252
        }
        public enum WaitForSubTypes
        {
            path_end = 0,
            response = 1,
            text_end = 2,
            no_text_on_screen = 3,
            endless = 4,

            path_node = 35,
            frames = 36,
            animation_frame = 37,
            cutscene_frame = 38,
        }
        public enum Axis
        {
            X = 0,
            PLUSX = 0,
            NEGX = 1,
            MINUSX = 1,
            Y = 2,
            PLUSY = 2,
            NEGY = 3,
            MINUSY = 3,
            Z = 4,
            PLUSZ = 4,
            NEGZ = 5,
            MINUSZ = 5,
        }

        public enum ScriptOverwriteTypes
        {
            overwrite = 0,
            restore = 1,
        }
        public enum TurnTowardsSubtypes
        {
            self = 0,
            player = 1,
            configid = 2,
            actorid = 3,
        }
        public enum TurnTypeSubtypes
        {
            towards = 0,
            degrees_left = 1,
            degrees_right = 2
        }
        public enum PlaySubtypes
        {
            sfx = 0,
            music = 1,
            cutscene = 2,
        }
        public enum KillSubtypes
        {
            self = 0,
            configid = 1,
            actorid = 2,
        }
    }
}
