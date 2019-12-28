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
        public static Style CyanStyle = new TextStyle(Brushes.DarkCyan, null, FontStyle.Regular);

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
            "kill",
            "enable_trade",
        };

        public static Dictionary<string, string[]> FunctionSubtypes = new Dictionary<string, string[]>()
        {
            {"if", Enum.GetNames(typeof(Enums.IfSubTypes)) },
            {"set", Enum.GetNames(typeof(Enums.SetSubTypes)) },
            {"waitfor", Enum.GetNames(typeof(Enums.WaitForSubTypes)) },
            {"play", Enum.GetNames(typeof(Enums.PlaySubtypes)) },
            {"kill", Enum.GetNames(typeof(Enums.KillSubtypes)) },
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
            "none",
            "roam",
            "follow",
            "path_follow",
            "body",
            "head",
            "random",
            "path_collisionwise",
            "path_direct",
            "invisible",
            "at_limb",
            "instead_of_limb"
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
            e.ChangedRange.ClearStyle(FCTB.CyanStyle);

            e.ChangedRange.SetStyle(FCTB.GreenStyle, @"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(FCTB.GreenStyle, @"//.+", RegexOptions.Multiline);

            foreach (string Item in FCTB.Functions.ToArray())
            {
                if (FCTB.FunctionSubtypes.ContainsKey(Item))
                {
                    foreach (string KWord in FCTB.FunctionSubtypes[Item])
                        e.ChangedRange.SetStyle(FCTB.GrayStyle, KWord, RegexOptions.Multiline);
                }
            }

            foreach (string KWord in Keywords)
                e.ChangedRange.SetStyle(FCTB.BlueStyle, KWord, RegexOptions.Multiline);

            foreach (string KWord in KeyValues)
                e.ChangedRange.SetStyle(FCTB.GrayStyle, KWord, RegexOptions.Multiline);

            foreach (string KWord in KeywordsDarkGray)
                e.ChangedRange.SetStyle(FCTB.DarkGrayStyle, KWord, RegexOptions.Multiline);

            foreach (string Func in Functions)
                e.ChangedRange.SetStyle(FCTB.PurpleStyle, Func, RegexOptions.Multiline);


            foreach (string KWord in Enum.GetNames(typeof(Enums.TradeItems)))
                e.ChangedRange.SetStyle(FCTB.CyanStyle, KWord, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string KWord in Enum.GetNames(typeof(Enums.GiveItems)))
                e.ChangedRange.SetStyle(FCTB.CyanStyle, KWord, RegexOptions.IgnoreCase | RegexOptions.Multiline);
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
