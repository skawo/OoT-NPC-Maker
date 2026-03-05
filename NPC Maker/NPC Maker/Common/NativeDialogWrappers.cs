using NPC_Maker;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

// ─────────────────────────────────────────────────────────────────────────────
// Shared base class
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Base class
/// On Linux, native desktop pickers (zenity, kdialog, qarma, yad) are tried
/// first.Falls back to the standard WinForms dialog on Windows/macOS or when no native tool is found.
/// </summary>
public abstract class NativeFileDialogBase
{
    // ── Three-state result for Linux process dialogs ──────────────────────

    protected enum PickerResult { NotAvailable, Cancelled, OK }

    // ── Public configuration ──────────────────────────────────────────────

    public string Title { get; set; } = "File";
    public string Filter { get; set; } = "All Files (*.*)|*.*";
    public string InitialDirectory { get; set; } = "";
    public bool RestoreDirectory { get; set; } = false;

    // ── Results ───────────────────────────────────────────────────────────

    /// <summary>The selected file path.</summary>
    public string FileName { get; protected set; } = "";

    // ── Main entry point ──────────────────────────────────────────────────

    public DialogResult ShowDialog(IWin32Window owner = null)
    {
        if (Program.IsRunningUnderMono)
            return ShowLinuxDialog();

        return ShowWinFormsDialog(owner);
    }

    // ── Abstract: subclasses supply mode-specific behaviour ───────────────

    protected abstract string BuildZenityArgs(string startDir, string filterArgs);
    protected abstract string BuildKdialogArgs(string startDir, string glob);
    protected abstract string BuildYadArgs(string startDir, string filterArgs);
    protected abstract DialogResult ShowWinFormsDialog(IWin32Window owner);

    // ── Linux dispatcher ──────────────────────────────────────────────────

    private DialogResult ShowLinuxDialog()
    {
        string prevDir = RestoreDirectory ? Directory.GetCurrentDirectory() : null;
        try
        {
            ExitMenuMode();
            Application.DoEvents();

            PickerResult result;
            string[] paths;

            result = TryZenityOrQarma(out paths);
            if (result == PickerResult.OK) return Commit(paths);
            if (result == PickerResult.Cancelled) return DialogResult.Cancel;

            result = TryKdialog(out paths);
            if (result == PickerResult.OK) return Commit(paths);
            if (result == PickerResult.Cancelled) return DialogResult.Cancel;

            result = TryYad(out paths);
            if (result == PickerResult.OK) return Commit(paths);
            if (result == PickerResult.Cancelled) return DialogResult.Cancel;

            return ShowWinFormsDialog(null);
        }
        finally
        {
            if (prevDir != null)
                Directory.SetCurrentDirectory(prevDir);
        }
    }

    // ── Native tool launchers ─────────────────────────────────────────────

    private PickerResult TryZenityOrQarma(out string[] paths)
    {
        paths = null;
        string cmd = CommandExists("zenity") ? "zenity"
                   : CommandExists("qarma") ? "qarma"
                   : null;
        if (cmd == null) return PickerResult.NotAvailable;

        string startDir = StartDir();
        string filterArgs = ConvertFilterToZenity(Filter);
        return RunProcess(cmd, BuildZenityArgs(startDir, filterArgs), out paths);
    }

    private PickerResult TryKdialog(out string[] paths)
    {
        paths = null;
        if (!CommandExists("kdialog")) return PickerResult.NotAvailable;

        string startDir = StartDir();
        string glob = ExtractFirstGlob(Filter);
        return RunProcess("kdialog", BuildKdialogArgs(startDir, glob), out paths);
    }

    private PickerResult TryYad(out string[] paths)
    {
        paths = null;
        if (!CommandExists("yad")) return PickerResult.NotAvailable;

        string startDir = StartDir();
        string filterArgs = ConvertFilterToZenity(Filter);
        return RunProcess("yad", BuildYadArgs(startDir, filterArgs), out paths);
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    protected string StartDir()
    {
        return string.IsNullOrEmpty(InitialDirectory)
            ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            : InitialDirectory;
    }

    protected DialogResult Commit(string[] paths)
    {
        if (paths == null || paths.Length == 0)
            return DialogResult.Cancel;

        FileName = paths[0];
        return DialogResult.OK;
    }

    /// <summary>
    /// Tells WinForms to exit menu/ToolStrip modal mode via reflection,
    /// mirroring what a real modal dialog does internally.
    /// </summary>
    private static void ExitMenuMode()
    {
        try
        {
            var t = typeof(ToolStripManager);
            var f = t.GetField("modalMenuFilter", BindingFlags.Static | BindingFlags.NonPublic);
            if (f != null)
            {
                var filter = f.GetValue(null);
                if (filter != null)
                {
                    var m = filter.GetType().GetMethod("ExitMenuMode",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (m != null) m.Invoke(filter, null);
                }
            }
        }
        catch { /* best effort */ }
    }

    protected static PickerResult RunProcess(string cmd, string args, out string[] paths)
    {
        paths = null;
        try
        {
            var psi = new ProcessStartInfo(cmd, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            using (var proc = Process.Start(psi))
            {
                string stdout = proc.StandardOutput.ReadToEnd().Trim();
                proc.WaitForExit();

                if (proc.ExitCode != 0 || string.IsNullOrEmpty(stdout))
                    return PickerResult.Cancelled;

                paths = stdout.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                return paths.Length > 0 ? PickerResult.OK : PickerResult.Cancelled;
            }
        }
        catch
        {
            return PickerResult.NotAvailable;
        }
    }

    protected static bool CommandExists(string cmd)
    {
        try
        {
            var psi = new ProcessStartInfo("which", cmd)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
        }
        catch { return false; }
    }

    /// <summary>
    /// Converts a WinForms filter string into --file-filter args for zenity/yad.
    /// "Images (*.png)|*.png;*.jpg|All (*.*)|*.*"
    ///   -> "--file-filter=\"Images | *.png *.jpg\" --file-filter=\"All | *.*\""
    /// </summary>
    protected static string ConvertFilterToZenity(string filter)
    {
        if (string.IsNullOrEmpty(filter)) return "";

        var parts = filter.Split('|');
        var sb = new System.Text.StringBuilder();

        for (int i = 0; i + 1 < parts.Length; i += 2)
        {
            string label = parts[i];
            string patterns = parts[i + 1].Replace(";", " ");
            sb.Append($"--file-filter=\"{Escape(label)} | {patterns}\" ");
        }

        return sb.ToString().TrimEnd();
    }

    protected static string ExtractFirstGlob(string filter)
    {
        if (string.IsNullOrEmpty(filter)) return "*";
        var parts = filter.Split('|');
        return parts.Length >= 2 ? parts[1].Split(';')[0] : "*";
    }

    protected static string Escape(string s) => s.Replace("\"", "\\\"");
}

// ─────────────────────────────────────────────────────────────────────────────
// Open dialog
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Cross-platform OpenFileDialog.
/// </summary>
public class NativeOpenFileDialog : NativeFileDialogBase
{
    public bool MultiSelect { get; set; } = false;
    public string[] FileNames { get; private set; } = new string[0];

    public NativeOpenFileDialog() { Title = "Open File"; }

    protected override string BuildZenityArgs(string startDir, string filterArgs)
    {
        string multi = MultiSelect ? "--multiple --separator=|" : "";
        return $"--file-selection --title=\"{Escape(Title)}\" --filename=\"{startDir}/\" {multi} {filterArgs}";
    }

    protected override string BuildKdialogArgs(string startDir, string glob)
    {
        string multi = MultiSelect ? "--multiple-files" : "";
        return $"--getopenfilename \"{startDir}\" \"{glob}\" --title \"{Escape(Title)}\" {multi}";
    }

    protected override string BuildYadArgs(string startDir, string filterArgs)
    {
        string multi = MultiSelect ? "--multiple --separator=|" : "";
        return $"--file-selection --title=\"{Escape(Title)}\" --filename=\"{startDir}/\" {multi} {filterArgs}";
    }

    protected override DialogResult ShowWinFormsDialog(IWin32Window owner)
    {
        using (var dlg = new OpenFileDialog())
        {
            dlg.Title = Title;
            dlg.Filter = Filter;
            dlg.Multiselect = MultiSelect;
            dlg.RestoreDirectory = RestoreDirectory;

            if (!string.IsNullOrEmpty(InitialDirectory))
                dlg.InitialDirectory = InitialDirectory;

            var result = owner != null ? dlg.ShowDialog(owner) : dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileNames = dlg.FileNames;
                return Commit(dlg.FileNames);
            }

            return DialogResult.Cancel;
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Save dialog
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Cross-platform SaveFileDialog.
/// </summary>
public class NativeSaveFileDialog : NativeFileDialogBase
{
    public string DefaultExt { get; set; } = "";
    public bool OverwritePrompt { get; set; } = true;

    /// <summary>
    /// Pre-filled filename shown in the dialog's filename field.
    /// Also readable after the dialog closes to get the chosen path.
    /// </summary>
    public new string FileName
    {
        get { return base.FileName; }
        set { base.FileName = value; }
    }

    public NativeSaveFileDialog() { Title = "Save File"; }

    // Builds the full suggested path to pre-fill the filename field.
    // e.g. /home/user/documents/myfile.png
    private string SuggestedPath(string startDir)
    {
        if (string.IsNullOrEmpty(FileName))
            return startDir + "/";

        // If FileName is already an absolute path, use it as-is
        if (Path.IsPathRooted(FileName))
            return FileName;

        return Path.Combine(startDir, FileName);
    }

    protected override string BuildZenityArgs(string startDir, string filterArgs)
    {
        string path = SuggestedPath(startDir);
        return $"--file-selection --save --confirm-overwrite --title=\"{Escape(Title)}\" --filename=\"{Escape(path)}\" {filterArgs}";
    }

    protected override string BuildKdialogArgs(string startDir, string glob)
    {
        string path = SuggestedPath(startDir);
        return $"--getsavefilename \"{Escape(path)}\" \"{glob}\" --title \"{Escape(Title)}\"";
    }

    protected override string BuildYadArgs(string startDir, string filterArgs)
    {
        string path = SuggestedPath(startDir);
        return $"--file-selection --save --confirm-overwrite --title=\"{Escape(Title)}\" --filename=\"{Escape(path)}\" {filterArgs}";
    }

    protected override DialogResult ShowWinFormsDialog(IWin32Window owner)
    {
        using (var dlg = new SaveFileDialog())
        {
            dlg.Title = Title;
            dlg.Filter = Filter;
            dlg.RestoreDirectory = RestoreDirectory;
            dlg.OverwritePrompt = OverwritePrompt;

            if (!string.IsNullOrEmpty(DefaultExt))
                dlg.DefaultExt = DefaultExt;

            if (!string.IsNullOrEmpty(InitialDirectory))
                dlg.InitialDirectory = InitialDirectory;

            if (!string.IsNullOrEmpty(FileName))
                dlg.FileName = Path.GetFileName(FileName);

            var result = owner != null ? dlg.ShowDialog(owner) : dlg.ShowDialog();

            if (result == DialogResult.OK)
                return Commit(new[] { dlg.FileName });

            return DialogResult.Cancel;
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Folder dialog
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Cross-platform FolderBrowserDialog.
/// </summary>
public class NativeFolderBrowserDialog
{
    // ── Three-state result for Linux process dialogs ──────────────────────

    private enum PickerResult { NotAvailable, Cancelled, OK }

    // ── Public configuration ──────────────────────────────────────────────

    public string Title { get; set; } = "Select Folder";
    public string InitialDir { get; set; } = "";

    // ── Results ───────────────────────────────────────────────────────────

    /// <summary>The selected folder path.</summary>
    public string SelectedPath { get; private set; } = "";

    // ── Main entry point ──────────────────────────────────────────────────

    public DialogResult ShowDialog(IWin32Window owner = null)
    {
        if (Program.IsRunningUnderMono)
            return ShowLinuxDialog();

        return ShowWinFormsDialog(owner);
    }

    // ── Linux dispatcher ──────────────────────────────────────────────────

    private DialogResult ShowLinuxDialog()
    {
        ExitMenuMode();
        Application.DoEvents();

        string[] paths;
        PickerResult result;

        result = TryZenityOrQarma(out paths);
        if (result == PickerResult.OK) return Commit(paths[0]);
        if (result == PickerResult.Cancelled) return DialogResult.Cancel;

        result = TryKdialog(out paths);
        if (result == PickerResult.OK) return Commit(paths[0]);
        if (result == PickerResult.Cancelled) return DialogResult.Cancel;

        result = TryYad(out paths);
        if (result == PickerResult.OK) return Commit(paths[0]);
        if (result == PickerResult.Cancelled) return DialogResult.Cancel;

        return ShowWinFormsDialog(null);
    }

    // ── Native tool launchers ─────────────────────────────────────────────

    private PickerResult TryZenityOrQarma(out string[] paths)
    {
        paths = null;
        string cmd = CommandExists("zenity") ? "zenity"
                   : CommandExists("qarma") ? "qarma"
                   : null;
        if (cmd == null) return PickerResult.NotAvailable;

        string args = $"--file-selection --directory --title=\"{Escape(Title)}\" --filename=\"{StartDir()}/\"";
        return RunProcess(cmd, args, out paths);
    }

    private PickerResult TryKdialog(out string[] paths)
    {
        paths = null;
        if (!CommandExists("kdialog")) return PickerResult.NotAvailable;

        string args = $"--getexistingdirectory \"{StartDir()}\" --title \"{Escape(Title)}\"";
        return RunProcess("kdialog", args, out paths);
    }

    private PickerResult TryYad(out string[] paths)
    {
        paths = null;
        if (!CommandExists("yad")) return PickerResult.NotAvailable;

        string args = $"--file-selection --directory --title=\"{Escape(Title)}\" --filename=\"{StartDir()}/\"";
        return RunProcess("yad", args, out paths);
    }

    // ── WinForms fallback ─────────────────────────────────────────────────

    private DialogResult ShowWinFormsDialog(IWin32Window owner)
    {
        using (var dlg = new FolderBrowserDialog())
        {
            dlg.Description = Title;

            if (!string.IsNullOrEmpty(InitialDir))
                dlg.SelectedPath = InitialDir;

            var result = owner != null ? dlg.ShowDialog(owner) : dlg.ShowDialog();

            if (result == DialogResult.OK)
                return Commit(dlg.SelectedPath);

            return DialogResult.Cancel;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private DialogResult Commit(string path)
    {
        if (string.IsNullOrEmpty(path))
            return DialogResult.Cancel;

        SelectedPath = path;
        return DialogResult.OK;
    }

    private string StartDir()
    {
        return string.IsNullOrEmpty(InitialDir)
            ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            : InitialDir;
    }

    private static void ExitMenuMode()
    {
        try
        {
            var t = typeof(ToolStripManager);
            var f = t.GetField("modalMenuFilter", BindingFlags.Static | BindingFlags.NonPublic);
            if (f != null)
            {
                var filter = f.GetValue(null);
                if (filter != null)
                {
                    var m = filter.GetType().GetMethod("ExitMenuMode",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (m != null) m.Invoke(filter, null);
                }
            }
        }
        catch { /* best effort */ }
    }

    private static PickerResult RunProcess(string cmd, string args, out string[] paths)
    {
        paths = null;
        try
        {
            var psi = new ProcessStartInfo(cmd, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            using (var proc = Process.Start(psi))
            {
                string stdout = proc.StandardOutput.ReadToEnd().Trim();
                proc.WaitForExit();

                if (proc.ExitCode != 0 || string.IsNullOrEmpty(stdout))
                    return PickerResult.Cancelled;

                paths = new[] { stdout };
                return PickerResult.OK;
            }
        }
        catch
        {
            return PickerResult.NotAvailable;
        }
    }

    private static bool CommandExists(string cmd)
    {
        try
        {
            var psi = new ProcessStartInfo("which", cmd)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
        }
        catch { return false; }
    }

    private static string Escape(string s) => s.Replace("\"", "\\\"");
}

public class NativeColorDialog
{
    // ── Three-state result for Linux process dialogs ──────────────────────

    private enum PickerResult { NotAvailable, Cancelled, OK }

    // ── Public configuration ──────────────────────────────────────────────

    public string Title { get; set; } = "Select Color";

    /// <summary>The initial color shown when the dialog opens.</summary>
    public Color Color { get; set; } = Color.White;

    public bool AllowFullOpen { get; set; } = true;
    public bool AnyColor { get; set; } = false;
    public bool SolidColorOnly { get; set; } = false;

    // ── Main entry point ──────────────────────────────────────────────────

    public DialogResult ShowDialog(IWin32Window owner = null)
    {
        if (Program.IsRunningUnderMono)
            return ShowLinuxDialog();

        return ShowWinFormsDialog(owner);
    }

    // ── Linux dispatcher ──────────────────────────────────────────────────

    private DialogResult ShowLinuxDialog()
    {
        ExitMenuMode();

        string[] output;
        PickerResult result;

        result = TryZenityOrQarma(out output);
        if (result == PickerResult.OK) return Commit(output[0]);
        if (result == PickerResult.Cancelled) return DialogResult.Cancel;

        result = TryKdialog(out output);
        if (result == PickerResult.OK) return Commit(output[0]);
        if (result == PickerResult.Cancelled) return DialogResult.Cancel;

        result = TryYad(out output);
        if (result == PickerResult.OK) return Commit(output[0]);
        if (result == PickerResult.Cancelled) return DialogResult.Cancel;

        return ShowWinFormsDialog(null);
    }

    // ── Native tool launchers ─────────────────────────────────────────────

    private PickerResult TryZenityOrQarma(out string[] output)
    {
        output = null;
        string cmd = CommandExists("zenity") ? "zenity"
                   : CommandExists("qarma") ? "qarma"
                   : null;
        if (cmd == null) return PickerResult.NotAvailable;

        // zenity expects initial color as hex: --color="#rrggbb"
        string hex = ColorToHex(Color);
        string args = $"--color-selection --title=\"{Escape(Title)}\" --color=\"{hex}\"";

        return RunProcess(cmd, args, out output);
    }

    private PickerResult TryKdialog(out string[] output)
    {
        output = null;
        if (!CommandExists("kdialog")) return PickerResult.NotAvailable;

        // kdialog expects initial color as hex: --getcolor --default "#rrggbb"
        string hex = ColorToHex(Color);
        string args = $"--getcolor --default \"{hex}\" --title \"{Escape(Title)}\"";

        return RunProcess("kdialog", args, out output);
    }

    private PickerResult TryYad(out string[] output)
    {
        output = null;
        if (!CommandExists("yad")) return PickerResult.NotAvailable;

        string hex = ColorToHex(Color);
        string args = $"--color --title=\"{Escape(Title)}\" --init-color=\"{hex}\"";

        return RunProcess("yad", args, out output);
    }

    // ── WinForms fallback ─────────────────────────────────────────────────

    private DialogResult ShowWinFormsDialog(IWin32Window owner)
    {
        using (var dlg = new ColorDialog())
        {
            dlg.Color = Color;
            dlg.AllowFullOpen = AllowFullOpen;
            dlg.AnyColor = AnyColor;
            dlg.SolidColorOnly = SolidColorOnly;

            var result = owner != null ? dlg.ShowDialog(owner) : dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                Color = dlg.Color;
                return DialogResult.OK;
            }

            return DialogResult.Cancel;
        }
    }

    // ── Commit parsed color output ────────────────────────────────────────

    /// <summary>
    /// Parses the raw stdout from a native picker and stores the result.
    /// Handles both hex (#rrggbb / #rrggbbaa) and rgb(r,g,b) / rgba(r,g,b,a)
    /// formats, which different tools may emit.
    /// </summary>
    private DialogResult Commit(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return DialogResult.Cancel;

        raw = raw.Trim();

        try
        {
            Color parsed;

            if (raw.StartsWith("#"))
            {
                parsed = ParseHexColor(raw);
            }
            else if (raw.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
            {
                parsed = ParseRgbColor(raw);
            }
            else
            {
                // Last-ditch: try the framework's own parser (e.g. "Red", "255,0,0")
                parsed = ColorTranslator.FromHtml(raw);
            }

            Color = parsed;
            return DialogResult.OK;
        }
        catch
        {
            return DialogResult.Cancel;
        }
    }

    // ── Color parsing helpers ─────────────────────────────────────────────

    /// <summary>Parses #rgb, #rrggbb, and #rrggbbaa.</summary>
    private static Color ParseHexColor(string hex)
    {
        hex = hex.TrimStart('#');

        switch (hex.Length)
        {
            case 3: // #rgb → #rrggbb
                hex = "" + hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
                break;
            case 6: // #rrggbb — nothing to do
                break;
            case 8: // #rrggbbaa
                {
                    int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                    int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                    int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                    int a = Convert.ToInt32(hex.Substring(6, 2), 16);
                    return Color.FromArgb(a, r, g, b);
                }
            default:
                throw new FormatException("Unrecognised hex color: " + hex);
        }

        return ColorTranslator.FromHtml("#" + hex);
    }

    /// <summary>Parses rgb(r,g,b) and rgba(r,g,b,a) strings.</summary>
    private static Color ParseRgbColor(string raw)
    {
        // Strip function name and parentheses: "rgba(255, 128, 0, 0.5)" → "255, 128, 0, 0.5"
        int start = raw.IndexOf('(');
        int end = raw.LastIndexOf(')');

        if (start < 0 || end < 0)
            throw new FormatException("Unrecognised rgb() color: " + raw);

        string inner = raw.Substring(start + 1, end - start - 1);
        string[] parts = inner.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        int r = int.Parse(parts[0]);
        int g = int.Parse(parts[1]);
        int b = int.Parse(parts[2]);

        if (parts.Length >= 4)
        {
            // Alpha may be 0..1 (CSS) or 0..255
            double alpha = double.Parse(parts[3],
                System.Globalization.CultureInfo.InvariantCulture);
            int a = alpha <= 1.0 ? (int)Math.Round(alpha * 255) : (int)alpha;
            return Color.FromArgb(a, r, g, b);
        }

        return Color.FromArgb(r, g, b);
    }

    /// <summary>Formats a Color as a lowercase #rrggbb hex string.</summary>
    private static string ColorToHex(Color c)
        => $"#{c.R:x2}{c.G:x2}{c.B:x2}";

    // ── Shared Linux helpers ──────────────────────────────────────────────

    private static void ExitMenuMode()
    {
        try
        {
            var t = typeof(ToolStripManager);
            var f = t.GetField("modalMenuFilter", BindingFlags.Static | BindingFlags.NonPublic);
            if (f != null)
            {
                var filter = f.GetValue(null);
                if (filter != null)
                {
                    var m = filter.GetType().GetMethod("ExitMenuMode",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (m != null) m.Invoke(filter, null);
                }
            }
        }
        catch { /* best effort */ }
    }

    private static PickerResult RunProcess(string cmd, string args, out string[] output)
    {
        output = null;
        try
        {
            var psi = new ProcessStartInfo(cmd, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            using (var proc = Process.Start(psi))
            {
                string stdout = proc.StandardOutput.ReadToEnd().Trim();
                proc.WaitForExit();

                if (proc.ExitCode != 0 || string.IsNullOrEmpty(stdout))
                    return PickerResult.Cancelled;

                output = new[] { stdout };
                return PickerResult.OK;
            }
        }
        catch
        {
            return PickerResult.NotAvailable;
        }
    }

    private static bool CommandExists(string cmd)
    {
        try
        {
            var psi = new ProcessStartInfo("which", cmd)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
        }
        catch { return false; }
    }

    private static string Escape(string s) => s.Replace("\"", "\\\"");
}