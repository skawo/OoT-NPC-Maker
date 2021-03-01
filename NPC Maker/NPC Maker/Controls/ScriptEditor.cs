using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace NPC_Maker
{
    public partial class ScriptEditor : UserControl
    {
        private NPCEntry Entry;
        private bool SyntaxHighlighting;
        public ScriptEntry Script;

        public ScriptEditor(ref NPCEntry _Entry, ScriptEntry _Script, bool _SyntaxHighlighting)
        {
            InitializeComponent();

            Entry = _Entry;
            SyntaxHighlighting = _SyntaxHighlighting;
            Script = _Script;

            Textbox_Script.Text = Script.Text;

            if (Script.ParseErrors.Count == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
            {
                foreach (string Error in Script.ParseErrors)
                    Textbox_ParseErrors.Text += Error + Environment.NewLine;
            }

        }

        public void SetSyntaxHighlighting(bool Value)
        {
            SyntaxHighlighting = Value;
            Textbox_Script_TextChanged(Textbox_Script, new TextChangedEventArgs(new Range(Textbox_Script)));
        }

        private void Textbox_Script_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (Entry == null)
                return;

            Script.Text = (sender as FastColoredTextBox).Text;
            FCTB.ApplySyntaxHighlight(sender as FastColoredTextBox, e, SyntaxHighlighting);
        }

        private void Button_TryParse_Click(object sender, EventArgs e)
        {
            string[] Lines = Textbox_Script.Text.Replace(";", Environment.NewLine).Split(new[] { "\n" }, StringSplitOptions.None);
            Range r = new Range(Textbox_Script, 0, 0, Textbox_Script.Text.Length, Lines.Length);
            r.ClearStyle(FCTB.ErrorStyle);

            NewScriptParser.ScriptParser Parser = new NewScriptParser.ScriptParser(Entry, Script.Text);
            Textbox_ParseErrors.Clear();

            NewScriptParser.BScript Output = Parser.ParseScript();

#if DEBUG

            Debug Dbg = new Debug(String.Join(Environment.NewLine, Output.ScriptDebug.ToArray()));
            Dbg.Show();

#endif
            if (Output.ParseErrors.Count() == 0)
                Textbox_ParseErrors.Text = "Parsed successfully!";
            else
            {
                foreach (NewScriptParser.ParseException Error in Output.ParseErrors)
                {
                    Textbox_ParseErrors.Text += Error.ToString() + Environment.NewLine;
                }
            }
        }


        private void Textbox_Script_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.WaitCursor;

                if (ScriptContextMenu.ContextMenuStrip == null)
                    ScriptContextMenu.MakeContextMenu();

                Cursor.Current = Cursors.Default;

                ScriptContextMenu.SetTextBox(sender as FastColoredTextBox);
                ScriptContextMenu.ContextMenuStrip.Show(sender as Control, e.Location);
            }
        }

    }
}
