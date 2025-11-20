using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ButtonBackColorExtensions;

namespace BRichExample
{
    public partial class MainForm : Form
    {
        int count  = 0;

        public MainForm()
        {
            InitializeComponent();
            SetColor(colorDialog1.Color);
            BRichTextBox1.AutoScroll = AutoScrollTb.Checked;
        }

        private void Enter_Click(Object sender, EventArgs e)
        {
            BRichTextBox1.AddDate = true;
            BRichTextBox1.AppendLine(textBox1.Text + " " + count++, colorDialog1.Color);
        }

        private void checkBox1_CheckedChanged(Object sender, EventArgs e)
        {
            BRichTextBox1.AutoScroll = ((CheckBox)sender).Checked;
        }

        private void TextColorLabel_Click(Object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();

            SetColor(colorDialog1.Color);
        }

        private void SetColor(Color color)
        {
            //bSetColor.BackColor = bSetColor.BackColor;

            TextColorLabel.SetForeColorWithAutoBack(color);

            //bSetColor.BackColor = SystemColors.ButtonFace;
            //bSetColor.UseVisualStyleBackColor = true;
            //bSetColor.BackColor = default(Color);
            //bSetColor.UseVisualStyleBackColor = true;

        }

        internal class OriginalButtonStyle
        {
            public bool HadCustomBackColor { get; set; }
            public Color BackColor { get; set; }
            public bool UseVisualStyleBackColor { get; set; }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Enter_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
