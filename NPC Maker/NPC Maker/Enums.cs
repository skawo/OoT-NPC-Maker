using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace NPC_Maker
{
    public static class Enums
    {
        public enum Segments
        {
            SEGMENT_8 = 0,
            SEGMENT_9 = 1,
            SEGMENT_A = 2,
            SEGMENT_B = 3,
            SEGMENT_C = 4,
            SEGMENT_D = 5,
            SEGMENT_E = 6,
            SEGMENT_F = 7
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

        public static Dictionary<string, int> SFXes = GetDictionary("SFX.csv");
        public static Dictionary<string, int> Music = GetDictionary("Music.csv");

        private static Dictionary<string, int> GetDictionary(string Filename)
        {
            Dictionary<string, int> Dict = new Dictionary<string, int>();

            try
            {
                string[] RawData = File.ReadAllLines(Filename);

                foreach (string Row in RawData)
                {
                    string[] NameAndID = Row.Split(',');
                    Dict.Add(NameAndID[1], Convert.ToInt32(NameAndID[0]));
                }

                return Dict;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(Filename + " is missing or incorrect.");
                return Dict;
            }

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
            TURN_TOWARDS_PLAYER = 8,
            PLAY = 9,
            KILL = 10,
            ENABLE_TRADE = 11,
            RETURN = 255,
        }

        public enum TradeItems
        {
            ZELDALETTER = 1,
            WEIRDEGG = 2,
            CHICKEN = 3,
            MAGICBEAN = 4,
            POCKETEGG = 5,
            POCKETCUCCO = 6,
            COJIRO = 7,
            ODDMUSHROOM = 8,
            ODDPOTION = 9,
            POACHERSAW = 10,
            BROKENGORONSWORD = 11,
            PRESCRIPTION = 12,
            EYEBALLFROG = 13,
            EYEDROPS = 14,
            CLAIMCHECK = 15,
            FISH = 24,
            BLUEFIRE = 25,
            BUG = 26,
            POE = 27,
            BIGPOE = 28,
            RUTOLETTER = 29,
        }

        public enum GiveItems
        {
            BOMBSX5 = 1,
            DEKUNUTSX5 = 2,
            BOMBCHUX10 = 3,
            FAIRYBOW = 4,
            FAIRYSLINGSHOT = 5,
            BOOMERANG = 6,
            DEKUSTICK = 7,
            HOOKSHOT = 8,
            LONGSHOT = 9,
            LENSOFTRUTH = 10,
            ZELDALETTER = 11,
            OCARINAOFTIME = 12,
            MEGATONHAMMER = 13,
            COJIRO = 14,
            EMPTYBOTTLE = 15,
            REDPOTION = 16,
            GREENPOTION = 17,
            BLUEPOTION = 18,
            BOTTLEDFAIRY = 19,
            BOTTLEDMILK = 20,
            BOTTLEDRUTOLETTER = 21,
            MAGICBEAN = 22,
            SKULLMASK = 23,
            SPOOKYMASK = 24,
            CHICKEN = 25,
            KEATONMASK = 26,
            BUNNYHOOD = 27,
            MASKOFTRUTH = 28,
            POCKETEGG = 29,
            POCKETCUCCO = 30,
            ODDMUSHROOM = 31,
            ODDPOTION = 32,
            POACHERSAW = 33,
            BROKENGORONSWORD = 34,
            PRESCRIPTION = 35,
            EYEBALLFROG = 36,
            EYEDROPS = 37,
            CLAIMCHECK = 38,
            KOKIRISWORD = 39,
            GIANTKNIFE = 40,
            DEKUSHIELD = 41,
            HYLIANSHIELD = 42,
            MIRRORSHIELD = 43,
            GORONTUNIC = 44,
            ZORATUNIC = 45,
            IRONBOOTS = 46,
            HOVERBOOTS = 47,
            BIGQUIVER = 48,
            BIGGESTQUIVER = 49,
            BOMBBAG = 50,
            BIGBOMBBAG = 51,
            BIGGESTBOMBBAG = 52,
            SILVERGAUNTLETS = 53,
            GOLDENGAUNTLETS = 54,
            SILVERSCALE = 55,
            GOLDENSCALE = 56,
            STONEOFAGONY = 57,
            GERUDOCARD = 58,
            INCORRECTFAIRYOCARINA = 59,
            DEKUSEEDSX5 = 60,
            HEARTCONTAINER = 61,
            PIECEOFHEART = 62,
            BOSSKEY = 63,
            COMPASS = 64,
            DUNGEONMAP = 65,
            SMALLKEY = 66,
            SMALLMAGICJAR = 67,
            LARGEMAGICJAR = 68,
            ADULTWALLET = 69,
            GIANTWALLET = 70,
            WEIRDEGG = 71,
            RECOVERYHEART = 72,
            ARROWSX5 = 73,
            ARROWSX10 = 74,
            ARROWSX30 = 75,
            GREENRUPEE = 76,
            BLUERUPEE = 77,
            REDRUPEE = 78,
            HEARTCONTAINER_ID79 = 79,
            PURCHASEDMILK = 80,
            GORONMASK = 81,
            ZORAMASK = 82,
            GERUDOMASK = 83,
            GORONBRACELET = 84,
            PURPLERUPEE = 85,
            HUGERUPEE = 86,
            BIGGORONSWORD = 87,
            FIREARROW = 88,
            ICEARROW = 89,
            LIGHTARROW = 90,
            GOLDSKULLTULATOKEN = 91,
            DINFIRE = 92,
            FAROREWIND = 93,
            NAYRULOVE = 94,
            BULLETBAG = 95,
            BIGBULLETBAG = 96,
            DEKUSTICKSX5 = 97,
            DEKUSTICKSX10 = 98,
            DEKUNUTSX5_ID99 = 99,
            DEKUNUTSX10 = 100,
            BOMB = 101,
            BOMBSX10 = 102,
            BOMBSX20 = 103,
            BOMBXX30 = 104,
            DEKUSEEDSX30 = 105,
            BOMBCHUX5 = 106,
            BOMBCHUX20 = 107,
            PURCHASEDFISH = 108,
            PURCHASEDBUG = 109,
            PURCHASEDBLUEFIRE = 110,
            PURCHASEDPOE = 111,
            PURCHASEDBIGPOE = 112,
            WONSMALLKEY = 113,
            WONGREENRUPEE = 114,
            WONBLUERUPEE = 115,
            WONREDRUPEE = 116,
            WONPURPLERUPEEE = 117,
            WONPIECEOFHEART = 118,
            DEKUSTICKUPGRADE = 119,
            DEKUSTICKUPGRADE2 = 120,
            DEKUNUTUPGRADE = 121,
            DEKUNUTUPGRADE2 = 122,
            BIGGESTBULLETBAG = 123,
            ICETRAP = 124
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

            rupees = 30,
            time_of_day = 31,
            scene_id = 32,
            worn_mask = 33,
            skulltulas = 34,
            item_being_traded = 62,

            trade_status = 63,
        }

        public enum SetSubTypes
        {
            /* u16 Subtypes */
            movement_distance = 0,
            loop_delay = 1,
            collision_radius = 2,
            collision_height = 3,
            target_limb = 4,
            path_id = 6,
            time_of_day = 7,
            incorrect_trade_text_id = 8,

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

            /* u32 Subtypes */

            /* s32 Subtypes */

            /* Float Subtypes */
            model_scale = 140,
            movement_speed = 141,

            /* u8 and bool Subtypes */
            do_loop = 175,
            collision = 176,
            shadow = 177,
            switches = 178,
            pushable = 179,
            targettable = 180,
            player_movement = 181,
            movement = 182,
            do_blinking_anim = 183,
            do_talking_anim = 184,

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
        }

        public enum WaitForSubTypes
        {
            path_end = 0,
            response = 1,
            text_end = 2,
            endless = 3,

            path_node = 35,
            frames = 36,
            animation_frame = 37,
        }

        public enum PlaySubtypes
        {
            sfx = 0,
            music = 1,
        }

        public enum KillSubtypes
        {
            self = 0,
            configid = 1,
            actorid = 2,
        }
    }
}
