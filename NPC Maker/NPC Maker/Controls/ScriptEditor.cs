using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace NPC_Maker
{
    public partial class ScriptEditor : UserControl
    {
        private NPCEntry Entry;
        private bool SyntaxHighlighting;
        private bool AutoParse;
        public ScriptEntry Script;
        private NPCFile File;
        private Scripts.BScript Output = null;

        private readonly System.Windows.Forms.Timer AutoParseTimer;
        private readonly System.Windows.Forms.Timer ColorizeTimer;
        private readonly System.Windows.Forms.Timer ResultsTimer;

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

            ResultsTimer = new System.Windows.Forms.Timer
            {
                Interval = 100,
            };
            ResultsTimer.Tick += ResultsTimer_Tick;

            ResultsTimer.Start();

            Init(ref _Entry, ref _File, _Script, _SyntaxHighlighting, _AutoParse);
        }

        public void Init(ref NPCEntry _Entry, ref NPCFile _File, ScriptEntry _Script, bool _SyntaxHighlighting, bool _AutoParse)
        {
            Entry = _Entry;
            SyntaxHighlighting = _SyntaxHighlighting;
            AutoParse = _AutoParse;
            Script = _Script;
            File = _File;

            Textbox_Script.TextChanged -= Textbox_Script_TextChanged;

            Textbox_Script.Text = Script.Text;

            if (SyntaxHighlighting)
                SyntaxHighlighter.ApplySyntaxHighlight(Textbox_Script, SyntaxHighlighting, _File, _Entry);

            Textbox_Script.TextChanged += Textbox_Script_TextChanged;

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

            SyntaxHighlighter.ApplySyntaxHighlight(Textbox_Script, SyntaxHighlighting, File, Entry);
        }

        private void ColorizeTimer_Tick(object sender, EventArgs e)
        {
            ColorizeTimer.Stop();
            SyntaxHighlighter.ApplySyntaxHighlight(Textbox_Script, SyntaxHighlighting, File, Entry);
        }

        private void ResultsTimer_Tick(object sender, EventArgs e)
        {
            ResultsTimer.Stop();

            try
            {
                if (Output != null)
                {
                    Textbox_ParseErrors.Clear();
                    Script.ParseErrors.Clear();

                    if (Output.ParseErrors.Count() == 0)
                        Textbox_ParseErrors.Text = "Parsed successfully!";
                    else
                        Textbox_ParseErrors.Text = String.Join(Environment.NewLine, Output.ParseErrors);

                    Output = null;
                }
            }
            catch (Exception)
            { }

            ResultsTimer.Start();
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



        private void DoParse(bool GetBytes)
        {
            Thread th = new Thread(() => 
            {
                try
                {
                    Scripts.ScriptParser Parser = new Scripts.ScriptParser(File, Entry, Script.Text, File.GlobalHeaders);
                    Output = Parser.ParseScript(Script.Name, GetBytes);
                }
                catch (Exception)
                { }
            });

            th.Start();
            return;
        }

        private void Button_TryParse_Click(object sender, EventArgs e)
        {
            DoParse(true);

#if DEBUG

            //Debug Dbg = new Debug(String.Join(Environment.NewLine, Output.ScriptDebug.ToArray()));
            //Dbg.Show();



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

        private void Textbox_Script_KeyPressed(object sender, KeyPressEventArgs e)
        {

        }

        private void Textbox_Script_KeyUp(object sender, KeyEventArgs e)
        {
            // CONTROL + /
            if (e.Control && e.KeyCode == Keys.OemQuestion)
            {
                int Caret = Textbox_Script.SelectionStart;
                int iLineStart = Textbox_Script.Selection.Start.iLine;

                int ScrollPos = Textbox_Script.VerticalScroll.Value;

                List<string> i = Regex.Split(Textbox_Script.Text, "\r?\n").ToList();

                int Start = Math.Min(Textbox_Script.Selection.Start.iLine, Textbox_Script.Selection.End.iLine);
                int End = Math.Max(Textbox_Script.Selection.Start.iLine, Textbox_Script.Selection.End.iLine);

                List<string> l = i.Skip(Start).Take(End - Start + 1).ToList();

                List<string> n = new List<string>();

                bool Comment = true;

                if (l.All(x => String.IsNullOrWhiteSpace(x) || x.TrimStart().StartsWith("//")))
                    Comment = false;

                foreach (string s in l)
                {
                    if (String.IsNullOrWhiteSpace(s))
                        n.Add(s);
                    else
                        n.Add(!Comment ? s.Substring(2) : "//" + s);
                }

                string RString = String.Join(Environment.NewLine, i.Take(Start));
                string NString = String.Join(Environment.NewLine, n);
                string PString = String.Join(Environment.NewLine, i.Skip(End + 1));

                Textbox_Script.Text = (Start != 0 ? (RString + Environment.NewLine) : "") + NString + (End + 1 != i.Count ? (Environment.NewLine + PString) : "");
                Textbox_Script.VerticalScroll.Value = ScrollPos;
                Textbox_Script.SelectionStart = Caret + (Comment ? 2 : -2);

                if (Textbox_Script.Selection.Start.iLine < iLineStart)
                    Textbox_Script.SelectionStart += 2;
                else if (Textbox_Script.Selection.Start.iLine > iLineStart)
                    Textbox_Script.SelectionStart -= 2;

                Textbox_Script.UpdateScrollbars();
   
            }
        }
    }
}
