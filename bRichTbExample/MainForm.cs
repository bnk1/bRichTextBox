using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

		private void button1_Click(Object sender, EventArgs e)
		{
			BRichTextBox1.AddDate = true;
			BRichTextBox1.AppendLine(textBox1.Text + " " + count++, colorDialog1.Color);
		}

		private void checkBox1_CheckedChanged(Object sender, EventArgs e)
		{
			BRichTextBox1.AutoScroll = ((CheckBox)sender).Checked;
		}

		private void ColorB_Click(Object sender, EventArgs e)
		{
			colorDialog1.ShowDialog();
			SetColor(colorDialog1.Color);
		}

		private void SetColor(Color color)
		{
			ColorB.ForeColor = color;

			if (color == Color.White)
                ColorB.BackColor = Color.Black;
			else if (color == Color.Black)
                ColorB.BackColor = Color.White;
			else
				ColorB.BackColor = BRichTextBox1.BackColor;

        }
	}
}
