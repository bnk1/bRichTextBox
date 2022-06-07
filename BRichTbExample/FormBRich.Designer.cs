namespace BRichTbExample
{
	partial class FormBRich
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.AddB = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.bRichTextBox1 = new BRichTextBox.BRichTextBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.ColorB = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// AddB
			// 
			this.AddB.Location = new System.Drawing.Point(1256, 37);
			this.AddB.Name = "AddB";
			this.AddB.Size = new System.Drawing.Size(120, 56);
			this.AddB.TabIndex = 1;
			this.AddB.Text = "Add";
			this.AddB.UseVisualStyleBackColor = true;
			this.AddB.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(1256, 99);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(233, 26);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "xxxxxxxxxxxxxxx";
			// 
			// bRichTextBox1
			// 
			this.bRichTextBox1.AddDate = false;
			this.bRichTextBox1.AutoScroll = false;
			this.bRichTextBox1.Location = new System.Drawing.Point(48, 37);
			this.bRichTextBox1.Name = "bRichTextBox1";
			this.bRichTextBox1.Size = new System.Drawing.Size(1174, 189);
			this.bRichTextBox1.TabIndex = 3;
			this.bRichTextBox1.Text = "";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(1256, 131);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(112, 24);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "Auto Scroll";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// ColorB
			// 
			this.ColorB.Location = new System.Drawing.Point(1382, 37);
			this.ColorB.Name = "ColorB";
			this.ColorB.Size = new System.Drawing.Size(120, 56);
			this.ColorB.TabIndex = 5;
			this.ColorB.Text = "Color";
			this.ColorB.UseVisualStyleBackColor = true;
			this.ColorB.Click += new System.EventHandler(this.ColorB_Click);
			// 
			// FormBRich
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1631, 451);
			this.Controls.Add(this.ColorB);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.bRichTextBox1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.AddB);
			this.Name = "FormBRich";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button AddB;
		private System.Windows.Forms.TextBox textBox1;
		private BRichTextBox.BRichTextBox bRichTextBox1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Button ColorB;
	}
}

