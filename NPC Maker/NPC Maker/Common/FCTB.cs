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
            {Enum.GetNames(typeof(NewScriptParser.Lists.TradeItems)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(NewScriptParser.Lists.DungeonItems)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(NewScriptParser.Lists.Items)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(NewScriptParser.Lists.GiveItems)).ToList(),  FCTB.CyanStyle},
            {Enum.GetNames(typeof(NewScriptParser.Lists.Instructions)).ToList(),  FCTB.PurpleStyle},
            {NewScriptParser.Lists.KeywordsBlue,  FCTB.BlueStyle},
            {NewScriptParser.Lists.KeywordsRed,  FCTB.RedStyle},
            {NewScriptParser.Lists.KeywordsGray,  FCTB.GrayStyle},
            {NewScriptParser.Lists.KeywordsPurple,  FCTB.PurpleStyle},
            {NewScriptParser.Lists.KeywordsMPurple,  FCTB.MPurpleStyle},
            {NewScriptParser.Lists.SFXes.Keys.ToList(),  FCTB.CyanStyle},
            {NewScriptParser.Lists.Music.Keys.ToList(),  FCTB.CyanStyle},
            {NewScriptParser.Lists.Actors.Keys.ToList(),  FCTB.CyanStyle},
        };

        public static Dictionary<string, Style> RegexDict = new Dictionary<string, Style>()
        {
            { @"/\*(.|[\r\n])*?\*/", FCTB.GreenStyle},      // Comments like /* comment */
            { @"//.+", FCTB.GreenStyle},                    // Comments like // comment
            { @".+:", FCTB.BoldRedStyle},                   // Labels
            { @":.+", FCTB.RedStyle},                       // Procedure calls
            { @"#define.+", FCTB.GreenStyle},               // Defines
        };

        public static void ApplySyntaxHighlight(FastColoredTextBox txb, TextChangedEventArgs e, bool SyntaxHighlightingOn)
        {
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
            foreach (string Item in Enum.GetNames(typeof(NewScriptParser.Lists.Instructions)))
            {
                if (NewScriptParser.Lists.FunctionSubtypes.ContainsKey(Item))
                    H_SetStyle(NewScriptParser.Lists.FunctionSubtypes[Item].ToList(), FCTB.GrayStyle, r);
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
