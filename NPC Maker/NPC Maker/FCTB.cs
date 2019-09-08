using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;

namespace NPC_Maker
{
    public static class FCTB
    {
        public static Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        public static Style BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        public static Style BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
        public static Style ErrorStyle = new TextStyle(Brushes.Black, Brushes.Red, FontStyle.Bold);
        public static Style PurpleStyle = new TextStyle(Brushes.Purple, null, FontStyle.Regular);
        public static Style GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        public static Style DarkGrayStyle = new TextStyle(Brushes.DarkGray, null, FontStyle.Regular);

        public static List<string> Functions = new List<string>()
        {
            "if",
            "set",
            "waitfor",
            "enable_textbox",
            "show_textbox",
            "give_item",
            "goto",
            "turn_towards_player",
            "play",
            "kill"
        };

        public static List<string> Keywords = new List<string>()
        {
            "if",
            "true",
            "false",
            "return",
            "stop"
        };

        public static List<string> KeywordsDarkGray = new List<string>()
        {
            "then",
            "else"
        };

        public static List<string> KeyValues = new List<string>()
        {
            "inf_table",
            "event_chk_inf",
            "switch_table",
            "uscene",
            "treasure",
            "room_clear",
            "scene_collect",
            "temporary",
            "age",
            "day",
            "talking",
            "has_empty_bottle",
            "rupees",
            "time",
            "scene_id",
            "worn_mask",
            "skulltulas",
            "none",
            "roam",
            "follow",
            "path_follow",
            "movement_distance",
            "loop_delay",
            "collision_radius",
            "collision_height",
            "target_limb",
            "movement_speed",
            "path_id",
            "time_of_day",
            "loop_start",
            "loop_end",
            "collision_offset_x",
            "collision_offset_y",
            "collision_offset_z",
            "target_offset_x",
            "target_offset_y",
            "target_offset_z",
            "model_offset_x",
            "model_offset_y",
            "model_offset_z",
            "rupees",
            "model_scale",
            "do_loop",
            "collision",
            "shadow",
            "switches",
            "pushable",
            "player_movement",
            "movement",
            "responses",
            "flag",
            "movement_type",
            "look_type",
            "head_axis",
            "animation",
            "animation_object",
            "animation_offset",
            "animation_speed",
            "script_start",
            "path_end",
            "response",
            "text_end",
            "endless",
            "path_node",
            "frames",
            "animation_frame",
            "none",
            "body",
            "head",
            "random",
            "follow",
            "path_collisionwise",
            "path_direct",
            "music",
            "sfx",
            "self",
            "configid",
            "actorid"
        };

        public static void ApplySyntaxHighlight(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(FCTB.ErrorStyle);
            e.ChangedRange.ClearStyle(FCTB.GreenStyle);
            e.ChangedRange.ClearStyle(FCTB.BrownStyle);
            e.ChangedRange.ClearStyle(FCTB.BlueStyle);
            e.ChangedRange.ClearStyle(FCTB.PurpleStyle);
            e.ChangedRange.ClearStyle(FCTB.GrayStyle);
            e.ChangedRange.ClearStyle(FCTB.DarkGrayStyle);

            e.ChangedRange.SetStyle(FCTB.GreenStyle, @"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(FCTB.GreenStyle, @"//.+", RegexOptions.Multiline);

            foreach (string KWord in Keywords)
                e.ChangedRange.SetStyle(FCTB.BlueStyle, KWord, RegexOptions.Multiline);

            foreach (string KWord in KeyValues)
                e.ChangedRange.SetStyle(FCTB.GrayStyle, KWord, RegexOptions.Multiline);

            foreach (string KWord in KeywordsDarkGray)
                e.ChangedRange.SetStyle(FCTB.DarkGrayStyle, KWord, RegexOptions.Multiline);

            foreach (string Func in Functions)
                e.ChangedRange.SetStyle(FCTB.PurpleStyle, Func, RegexOptions.Multiline);
        }

        public static void ApplyError(FastColoredTextBox tb, string Line)
        {
            string[] Lines = tb.Text.Split(new[] { "\n" }, StringSplitOptions.None);

            for (int i = 0; i < Lines.Count(); i++)
            {
                if (Lines[i].Trim() == Line.Trim())
                {
                    Range r = new Range(tb, 0, i, Lines[i].Length - 1, i);
                    r.ClearStyle(FCTB.ErrorStyle);
                    r.ClearStyle(FCTB.GreenStyle);
                    r.ClearStyle(FCTB.BrownStyle);
                    r.ClearStyle(FCTB.BlueStyle);
                    r.ClearStyle(FCTB.PurpleStyle);
                    r.ClearStyle(FCTB.GrayStyle);
                    r.ClearStyle(FCTB.DarkGrayStyle);
                    r.SetStyle(FCTB.ErrorStyle, ".*", RegexOptions.Multiline);
                }
            }
        }

    }
}
