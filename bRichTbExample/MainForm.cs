using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bRichExample
{
	public partial class MainForm : Form
	{
		int count  = 0;

		public MainForm()
		{
			InitializeComponent();
			SetColor(colorDialog1.Color);
            bRichTextBox1.AutoScroll = AutoScrollTb.Checked;
        }

		private void button1_Click(Object sender, EventArgs e)
		{
			bRichTextBox1.AddDate = true;
			bRichTextBox1.AppendLine(textBox1.Text + " " + count++, colorDialog1.Color);
		}

		private void checkBox1_CheckedChanged(Object sender, EventArgs e)
		{
			bRichTextBox1.AutoScroll = ((CheckBox)sender).Checked;
		}

		private void ColorB_Click(Object sender, EventArgs e)
		{
			colorDialog1.ShowDialog();
			SetColor(colorDialog1.Color);
		}

		private void SetColor(Color color)
		{
			ColorB.BackColor = color;

			if (color == Color.Black)
				ColorB.ForeColor = Color.White;
			else
				ColorB.ForeColor = Color.Black;
		}
	}
}
