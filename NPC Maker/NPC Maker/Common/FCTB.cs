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
        public static Style CyanStyle = new TextStyle(Brushes.DarkCyan, null, FontStyle.Bold);
        public static Style BoldRedStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);
        public static Style RedStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);

        public static void ApplySyntaxHighlight(object sender, TextChangedEventArgs e, bool SyntaxHighlightingOn)
        {
            e.ChangedRange.ClearStyle(FCTB.ErrorStyle);
            e.ChangedRange.ClearStyle(FCTB.GreenStyle);
            e.ChangedRange.ClearStyle(FCTB.BrownStyle);
            e.ChangedRange.ClearStyle(FCTB.BlueStyle);
            e.ChangedRange.ClearStyle(FCTB.PurpleStyle);
            e.ChangedRange.ClearStyle(FCTB.GrayStyle);
            e.ChangedRange.ClearStyle(FCTB.DarkGrayStyle);
            e.ChangedRange.ClearStyle(FCTB.CyanStyle);
            e.ChangedRange.ClearStyle(FCTB.BoldRedStyle);

            if (!SyntaxHighlightingOn)
                return;

            e.ChangedRange.SetStyle(FCTB.GreenStyle, @"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(FCTB.GreenStyle, @"//.+", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(FCTB.BoldRedStyle, @".+:", RegexOptions.Multiline);

            string[] Lines = (sender as FastColoredTextBox).Text.Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range((sender as FastColoredTextBox), 0, 0, Lines[Lines.Length - 1].Length - 1, Lines.Length - 1);

            List<string> Labels = GetLabels((sender as FastColoredTextBox).Text);

            r.ClearStyle(FCTB.RedStyle);

            foreach (string KWord in Labels)
                r.SetStyle(FCTB.RedStyle, @"\b" + KWord + @"\b", RegexOptions.Multiline);

            foreach (string Item in Enum.GetNames(typeof(Lists.InstructionIDs)))
            {
                if (Lists.FunctionSubtypes.ContainsKey(Item))
                {
                    foreach (string KWord in Lists.FunctionSubtypes[Item])
                        e.ChangedRange.SetStyle(FCTB.GrayStyle, @"\b" + KWord + @"\b", RegexOptions.Multiline);
                }
            }

            foreach (string KWord in Lists.Keywords)
                e.ChangedRange.SetStyle(FCTB.BlueStyle, @"\b" + KWord + @"\b", RegexOptions.Multiline);

            foreach (string KWord in Lists.KeyValues)
                e.ChangedRange.SetStyle(FCTB.GrayStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string KWord in Lists.KeywordsDarkGray)
                e.ChangedRange.SetStyle(FCTB.DarkGrayStyle, @"\b" + KWord + @"\b", RegexOptions.Multiline);

            foreach (string Func in Enum.GetNames(typeof(Lists.InstructionIDs)))
                e.ChangedRange.SetStyle(FCTB.PurpleStyle, @"\b" + Func + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string KWord in Enum.GetNames(typeof(Lists.TradeItems)))
                e.ChangedRange.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string KWord in Enum.GetNames(typeof(Lists.GiveItems)))
                e.ChangedRange.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string KWord in Lists.SFXes.Keys)
                e.ChangedRange.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string KWord in Lists.Music.Keys)
                e.ChangedRange.SetStyle(FCTB.CyanStyle, @"\b" + KWord + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static List<string> GetLabels(string Text)
        {
            Text = Regex.Replace(Text, @"/\*(.|[\r\n])*?\*/", string.Empty);                                // Remove comment blocks
            Text = Regex.Replace(Text, "//.+", string.Empty);                                               // Remove inline comments
            Text = Regex.Replace(Text, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();      // Remove empty lines
            Text = Text.Replace("\t", " ");

            List<string> Lines = Text.Split(new[] { "\n" }, StringSplitOptions.None).ToList();
            List<string> Labels = new List<string>();

            for (int i = 0; i < Lines.Count(); i++)
            {
                if (Lines[i].EndsWith(":"))
                    Labels.Add(Lines[i].Substring(0, Lines[i].Length - 1));
            }

            return Labels;
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
