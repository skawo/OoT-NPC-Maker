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

        public static void ApplySyntaxHighlight(object sender, TextChangedEventArgs e, bool SyntaxHighlightingOn)
        {
            string[] Lines = (sender as FastColoredTextBox).Text.Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range((sender as FastColoredTextBox), 0, 0, Lines[Lines.Length - 1].Length - 1, Lines.Length - 1);

            r.ClearStyle(FCTB.ErrorStyle);
            r.ClearStyle(FCTB.BrownStyle);
            r.ClearStyle(FCTB.BlueStyle);
            r.ClearStyle(FCTB.PurpleStyle);
            r.ClearStyle(FCTB.MPurpleStyle);
            r.ClearStyle(FCTB.GrayStyle);
            r.ClearStyle(FCTB.DarkGrayStyle);
            r.ClearStyle(FCTB.CyanStyle);
            r.ClearStyle(FCTB.BoldRedStyle);
            r.ClearStyle(FCTB.RedStyle);
            r.ClearStyle(FCTB.GreenStyle);

            if (!SyntaxHighlightingOn)
                return;

            r.SetStyle(FCTB.GreenStyle, @"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline);
            r.SetStyle(FCTB.GreenStyle, @"//.+", RegexOptions.Multiline);
            r.SetStyle(FCTB.BoldRedStyle, @".+:", RegexOptions.Multiline);
            r.SetStyle(FCTB.RedStyle, @":.+", RegexOptions.Multiline);
            r.SetStyle(FCTB.GreenStyle, @"#define.+", RegexOptions.Multiline);

            // Color in labels
            List<string> Labels = NewScriptParser.ScriptParser.GetLabels((sender as FastColoredTextBox).Text);

            foreach (string KWord in Labels)
                r.SetStyle(FCTB.RedStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in instruction subtypes
            foreach (string Item in Enum.GetNames(typeof(NewScriptParser.Lists.Instructions)))
            {
                if (NewScriptParser.Lists.FunctionSubtypes.ContainsKey(Item))
                {
                    foreach (string KWord in NewScriptParser.Lists.FunctionSubtypes[Item])
                        r.SetStyle(FCTB.GrayStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                }
            }

            // Color in functions
            foreach (string Func in Enum.GetNames(typeof(NewScriptParser.Lists.Instructions)))
                r.SetStyle(FCTB.PurpleStyle, @"\b" + Func + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in blue keywords
            foreach (string KWord in NewScriptParser.Lists.KeywordsBlue)
                r.SetStyle(FCTB.BlueStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in red keywords
            foreach (string KWord in NewScriptParser.Lists.KeywordsRed)
                r.SetStyle(FCTB.RedStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in gray keywords
            foreach (string KWord in NewScriptParser.Lists.KeywordsGray)
                r.SetStyle(FCTB.GrayStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in purple keywords
            foreach (string KWord in NewScriptParser.Lists.KeywordsPurple)
                r.SetStyle(FCTB.PurpleStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in purple keywords
            foreach (string KWord in NewScriptParser.Lists.KeywordsMPurple)
                r.SetStyle(FCTB.MPurpleStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in trade items
            foreach (string KWord in Enum.GetNames(typeof(NewScriptParser.Lists.TradeItems)))
                r.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in give items
            foreach (string KWord in Enum.GetNames(typeof(NewScriptParser.Lists.GiveItems)))
                r.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in sounds
            foreach (string KWord in NewScriptParser.Lists.SFXes.Keys)
                r.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // Color in music
            foreach (string KWord in NewScriptParser.Lists.Music.Keys)
                r.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            /*
            // Color in key values
            foreach (string KWord in NewScriptParser.Lists.KeyValues)
                r.SetStyle(FCTB.GrayStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            */
        }

    }
}
