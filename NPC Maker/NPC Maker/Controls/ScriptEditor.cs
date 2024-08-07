﻿using FastColoredTextBoxNS;
using System;
using System.Linq;
using System.Windows.Forms;

namespace NPC_Maker
{
    public partial class ScriptEditor : UserControl
    {
        private NPCEntry Entry;
        private bool SyntaxHighlighting;
        private bool AutoParse;
        public ScriptEntry Script;
        private NPCFile File;

        private readonly System.Windows.Forms.Timer AutoParseTimer;
        private readonly System.Windows.Forms.Timer ColorizeTimer;

        public ScriptEditor(ref NPCEntry _Entry, ref NPCFile _File, ScriptEntry _Script, bool _SyntaxHighlighting, bool _AutoParse)
        {
            InitializeComponent();

            AutoParseTimer = new System.Windows.Forms.Timer
            {
                Interval = (int)Program.Settings.ParseTime,
            };
            AutoParseTimer.Tick += AutoParseTimer_Tick;

            ColorizeTimer = new System.Windows.Forms.Timer
            {
                Interval= (int)Program.Settings.ParseTime / 2,
            };
            ColorizeTimer.Tick += ColorizeTimer_Tick;

            Init(ref _Entry, ref _File, _Script, _SyntaxHighlighting, _AutoParse);
        }

        public void Init(ref NPCEntry _Entry, ref NPCFile _File, ScriptEntry _Script, bool _SyntaxHighlighting, bool _AutoParse)
        {
            Entry = _Entry;
            SyntaxHighlighting = _SyntaxHighlighting;
            AutoParse = _AutoParse;
            Script = _Script;
            File = _File;

            Textbox_Script.Text = Script.Text;

            Textbox_ParseErrors.Clear();
            Textbox_Script.ClearUndo();

            if (Script.ParseErrors.Count == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
                Textbox_ParseErrors.Text = String.Join(Environment.NewLine, Script.ParseErrors);
        }

        private void AutoParseTimer_Tick(object sender, EventArgs e)
        {
            AutoParseTimer.Stop();

            if (AutoParse)
                DoParse(true);

            SyntaxHighlighter.ApplySyntaxHighlight(Textbox_Script, SyntaxHighlighting);
        }

        private void ColorizeTimer_Tick(object sender, EventArgs e)
        {
            ColorizeTimer.Stop();
            SyntaxHighlighter.ApplySyntaxHighlight(Textbox_Script, SyntaxHighlighting);
        }

        public void SetSyntaxHighlighting(bool Value)
        {
            SyntaxHighlighting = Value;
            Textbox_Script_TextChanged(Textbox_Script, new TextChangedEventArgs(new Range(Textbox_Script)));
        }

        public void SetAutoParsing(bool Value)
        {
            AutoParse = Value;
        }

        private void Textbox_Script_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (Entry == null)
                return;

            Script.Text = Textbox_Script.Text;

            AutoParseTimer.Stop();
            AutoParseTimer.Interval = (int)Program.Settings.ParseTime;
            AutoParseTimer.Start();
            ColorizeTimer.Stop();
            ColorizeTimer.Interval = (int)Program.Settings.ParseTime;
            ColorizeTimer.Start();
        }

        private Scripts.BScript DoParse(bool GetBytes)
        {
            string[] Lines = Textbox_Script.Text.Replace(";", Environment.NewLine).Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range(Textbox_Script, 0, 0, Textbox_Script.Text.Length, Lines.Length);
            r.ClearStyle(SyntaxHighlighter.ErrorStyle);

            Scripts.ScriptParser Parser = new Scripts.ScriptParser(File, Entry, Script.Text, File.GlobalHeaders);
            Textbox_ParseErrors.Clear();

            Script.ParseErrors.Clear();
            Scripts.BScript Output = Parser.ParseScript(Script.Name, GetBytes);

            if (Output.ParseErrors.Count() == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
                Textbox_ParseErrors.Text = String.Join(Environment.NewLine, Output.ParseErrors);

            return Output;
        }

        private void Button_TryParse_Click(object sender, EventArgs e)
        {
            Scripts.BScript Output = DoParse(true);

#if DEBUG

            Debug Dbg = new Debug(String.Join(Environment.NewLine, Output.ScriptDebug.ToArray()));
            Dbg.Show();

#endif

        }


        private void Textbox_Script_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.WaitCursor;

                ScriptContextMenu.MakeContextMenu(File, Entry);

                Cursor.Current = Cursors.Default;

                ScriptContextMenu.SetTextBox(sender as FastColoredTextBox);
                ScriptContextMenu.ContextMenuStrip.Show(sender as Control, e.Location);
            }
        }

    }
}
