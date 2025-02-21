using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace NPC_Maker
{
    public static class SyntaxHighlighter
    {
        public static Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        public static Style BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        public static Style BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
        public static Style ErrorStyle = new TextStyle(Brushes.Black, Brushes.Red, FontStyle.Bold);
        public static Style PurpleStyle = new TextStyle(Brushes.Purple, null, FontStyle.Regular);
        public static Style MPurpleStyle = new TextStyle(Brushes.MediumPurple, null, FontStyle.Regular);
        public static Style MPurpleBoldStyle = new TextStyle(Brushes.MediumPurple, null, FontStyle.Bold);
        public static Style GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        public static Style DarkGrayStyle = new TextStyle(Brushes.DarkGray, null, FontStyle.Regular);
        public static Style CyanStyle = new TextStyle(Brushes.DarkCyan, null, FontStyle.Bold);
        public static Style BoldRedStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);
        public static Style RedStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);
        public static Style BlackStyle = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        public static Style DefineStyle = new TextStyle(Brushes.Black, null, FontStyle.Bold);

        public static Dictionary<List<string>, Style> StyleDict = new Dictionary<List<string>, Style>()
        {
            {Enum.GetNames(typeof(Lists.TradeItems)).ToList(),  SyntaxHighlighter.CyanStyle},
            {Enum.GetNames(typeof(Lists.DungeonItems)).ToList(),  SyntaxHighlighter.CyanStyle},
            {Enum.GetNames(typeof(Lists.Items)).ToList(),  SyntaxHighlighter.CyanStyle},
            {Enum.GetNames(typeof(Lists.AwardItems)).ToList(),  SyntaxHighlighter.CyanStyle},
            {Enum.GetNames(typeof(Lists.QuestItems)).ToList(),  SyntaxHighlighter.CyanStyle},
            {Enum.GetNames(typeof(Lists.Instructions)).ToList(),  SyntaxHighlighter.PurpleStyle},
            {Lists.KeywordsCyan,  SyntaxHighlighter.CyanStyle},
            {Lists.KeywordsBlue,  SyntaxHighlighter.BlueStyle},
            {Lists.KeywordsRed,  SyntaxHighlighter.RedStyle},
            {Lists.KeywordsGray,  SyntaxHighlighter.GrayStyle},
            {Lists.KeywordsPurple,  SyntaxHighlighter.PurpleStyle},
            {Lists.KeywordsMPurple,  SyntaxHighlighter.MPurpleStyle},
            {Lists.KeywordsMPurpleBold,  SyntaxHighlighter.MPurpleBoldStyle},
            {Dicts.SFXes.Keys.ToList(),  SyntaxHighlighter.CyanStyle},
            {Dicts.Music.Keys.ToList(),  SyntaxHighlighter.CyanStyle},
            {Dicts.Actors.Keys.ToList(),  SyntaxHighlighter.CyanStyle},
            {Dicts.ObjectIDs.Keys.ToList(),  SyntaxHighlighter.CyanStyle},
            {Dicts.LinkAnims.Keys.ToList(),  SyntaxHighlighter.CyanStyle},
        };

        public static Dictionary<string, Style> RegexDict = new Dictionary<string, Style>()
        {
            { @"\/\*([\s\S]*?)\*\/", SyntaxHighlighter.GreenStyle},      // Comments like /* comment */
            { @"//.+", SyntaxHighlighter.GreenStyle},                    // Comments like // comment
            { @"case .+:", SyntaxHighlighter.MPurpleBoldStyle},          // Cases
            { @"default:", SyntaxHighlighter.MPurpleBoldStyle},          // Defaults
            { @".+:[\n\r ]+", SyntaxHighlighter.BoldRedStyle},           // Labels
            { @"::([\S]+)", SyntaxHighlighter.RedStyle},                 // Procedure calls
            { @"#define.+", SyntaxHighlighter.GreenStyle},               // Defines
        };

        public static void ApplySyntaxHighlight(FastColoredTextBox txb, bool SyntaxHighlightingOn, NPCFile File, NPCEntry Entry)
        {
            if (String.IsNullOrEmpty(txb.Text))
                return;

            Range r = new Range(txb, 0, 0, txb.Text.Length - 1, txb.LinesCount - 1);

            txb.ClearStyle(StyleIndex.All);

            if (!SyntaxHighlightingOn)
                return;

            //List<string[]> Defines = Scripts.ScriptParser.GetDefines(txb.Text, File, Entry);

            //foreach (string[] def in Defines)
            //    r.SetStyle(SyntaxHighlighter.DefineStyle, @"\b" + def[1] + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in regexed values
            foreach (KeyValuePair<string, Style> regex in RegexDict)
                r.SetStyle(regex.Value, regex.Key, RegexOptions.Multiline);

            // Color in labels
            List<string> Labels = Scripts.ScriptParser.GetLabels(txb.Text);
            H_SetStyle(Labels, SyntaxHighlighter.RedStyle, r);

            // Color in instruction subtypes
            foreach (string Item in Enum.GetNames(typeof(Lists.Instructions)))
            {
                if (Dicts.FunctionSubtypes.ContainsKey(Item))
                    H_SetStyle(Dicts.FunctionSubtypes[Item].ToList(), SyntaxHighlighter.GrayStyle, r);
            }

            // Color in keywords
            foreach (KeyValuePair<List<string>, Style> entry in StyleDict)
                H_SetStyle(entry.Key, entry.Value, r);

            r.SetStyle(SyntaxHighlighter.RedStyle, @"\b" + Lists.Keyword_Label_HERE + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        private static void H_SetStyle(List<string> List, Style s, Range r)
        {
            foreach (string KWord in List)
            {
                if (KWord == "CCALL")
                    r.SetStyle(SyntaxHighlighter.RedStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                else if (KWord == "SET")
                    r.SetStyle(SyntaxHighlighter.PurpleStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                else
                    r.SetStyle(s, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }
        }

    }
}
