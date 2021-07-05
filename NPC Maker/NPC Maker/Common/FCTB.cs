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
        public static Style MPurpleStyle = new TextStyle(Brushes.MediumPurple, null, FontStyle.Regular);
        public static Style GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        public static Style DarkGrayStyle = new TextStyle(Brushes.DarkGray, null, FontStyle.Regular);
        public static Style CyanStyle = new TextStyle(Brushes.DarkCyan, null, FontStyle.Bold);
        public static Style BoldRedStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);
        public static Style RedStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);
        public static Style BlackStyle = new TextStyle(Brushes.Black, null, FontStyle.Regular);

        public static Dictionary<List<string>, Style> StyleDict = new Dictionary<List<string>, Style>()
        {
            {Enum.GetNames(typeof(Lists.TradeItems)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(Lists.DungeonItems)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(Lists.Items)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(Lists.AwardItems)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(Lists.Instructions)).ToList(),  FCTB.PurpleStyle},
            {Lists.KeywordsBlue,  FCTB.BlueStyle},
            {Lists.KeywordsRed,  FCTB.RedStyle},
            {Lists.KeywordsGray,  FCTB.GrayStyle},
            {Lists.KeywordsPurple,  FCTB.PurpleStyle},
            {Lists.KeywordsMPurple,  FCTB.MPurpleStyle},
            {Dicts.SFXes.Keys.ToList(),  FCTB.CyanStyle},
            {Dicts.Music.Keys.ToList(),  FCTB.CyanStyle},
            {Dicts.Actors.Keys.ToList(),  FCTB.CyanStyle},
            {Dicts.ObjectIDs.Keys.ToList(),  FCTB.CyanStyle},
            {Dicts.ActorCategories.Keys.ToList(),  FCTB.CyanStyle},
        };

        public static Dictionary<string, Style> RegexDict = new Dictionary<string, Style>()
        {
            { @"/\*(.|[\r\n])*?\*/", FCTB.GreenStyle},      // Comments like /* comment */
            { @"//.+", FCTB.GreenStyle},                    // Comments like // comment
            { @".+:[\n\r ]+", FCTB.BoldRedStyle},           // Labels
            { @"::([\S]+)", FCTB.RedStyle},                 // Procedure calls
            { @"#define.+", FCTB.GreenStyle},               // Defines
        };

        public static void ApplySyntaxHighlight(FastColoredTextBox txb, bool SyntaxHighlightingOn)
        {
            if (String.IsNullOrEmpty(txb.Text))
                return;

            string[] Lines = txb.Text.Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range(txb, 0, 0, Lines[Lines.Length - 1].Length - 1, Lines.Length - 1);

            txb.ClearStyle(StyleIndex.All);

            if (!SyntaxHighlightingOn)
                return;

            // Color in regexed values
            foreach (KeyValuePair<string, Style> regex in RegexDict)
                r.SetStyle(regex.Value, regex.Key, RegexOptions.Multiline);

            // Color in labels
            List<string> Labels = NewScriptParser.ScriptParser.GetLabels(txb.Text);
            H_SetStyle(Labels, FCTB.RedStyle, r);

            // Color in instruction subtypes
            foreach (string Item in Enum.GetNames(typeof(Lists.Instructions)))
            {
                if (Dicts.FunctionSubtypes.ContainsKey(Item))
                    H_SetStyle(Dicts.FunctionSubtypes[Item].ToList(), FCTB.GrayStyle, r);
            }

            // Color in keywords
            foreach (KeyValuePair<List<string>, Style> entry in StyleDict)
                H_SetStyle(entry.Key, entry.Value, r);
        }

        private static void H_SetStyle(List<string> List, Style s, Range r)
        {
            foreach (string KWord in List)
                r.SetStyle(s, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

    }
}
