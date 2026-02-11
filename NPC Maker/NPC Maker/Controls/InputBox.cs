using System.Drawing;
using System.Windows.Forms;

namespace NPC_Maker
{
    public static class InputBox
    {
        public static DialogResult ShowInputDialog(string Prompt, ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(300, 70);

            Form inputBox = new Form
            {
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ClientSize = size,
                Text = Prompt,
                Font = new System.Drawing.Font(SystemFonts.DefaultFont.FontFamily, Helpers.GetScaleFontSize())
            };

            TextBox textBox = new TextBox
            {
                Size = new System.Drawing.Size(size.Width - 10, (int)(23 * Program.Settings.GUIScale)),
                Location = new System.Drawing.Point(5, 5),
                Text = input
            };
            inputBox.Controls.Add(textBox);

            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                AutoSize = true,
                Text = "&OK",
                Location = new System.Drawing.Point(size.Width - 80 - 80, textBox.Location.Y + textBox.Height + 8)
            };
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Name = "cancelButton",
                AutoSize = true,
                Text = "&Cancel",
                Location = new System.Drawing.Point(size.Width - 80, textBox.Location.Y + textBox.Height + 8)
            };
            inputBox.Controls.Add(cancelButton);

            inputBox.ClientSize = new Size(inputBox.ClientSize.Width, cancelButton.Location.Y + cancelButton.Height + 5);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
    }
}
