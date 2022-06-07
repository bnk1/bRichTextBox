using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRichTbExample
{
	public partial class FormBRich : Form
	{
		public FormBRich()
		{
			InitializeComponent();
		}

		private void button1_Click(Object sender, EventArgs e)
		{
			bRichTextBox1.AppendLine(textBox1.Text);
		}
	}
}
