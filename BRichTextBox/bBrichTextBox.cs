using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BRichTextBox
{
	public partial class BRichTextBox : RichTextBox
	{
		Point  _ScrollPoint;
		bool   _Painting      = true;
		IntPtr _EventMask;
		int    _SuspendIndex  = 0;
		int    _SuspendLength = 0;
		public bool AutoScroll { get ; set; } = false;
		public bool AddDate { get; set; } = false;

		public BRichTextBox()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		public void AppendExc(Exception exc, bool detailed = false)
		{
			if (exc == null)
				return;

			AppendLine(exc.Message, Color.Red);

			if (detailed)
			{
				AppendLine("Trace: " + exc.StackTrace);

				if (exc.InnerException != null)
					AppendExc(exc.InnerException, detailed);
			}
		}

		public void AppendExc(Exception exc)
		{
			AppendLine(exc.Message, Color.Red);
		}

		public void AppendErr(string text)
		{
			AppendLine(text, Color.Red);
		}

		public void AppendLine(String text, Color? color = null)
		{
			AppendTextBox(text + "\n", color);
		}

		public bool ReachedBottom()
		{
			NativeMethods.SCROLLINFO scrollInfo = new NativeMethods.SCROLLINFO();
			scrollInfo.cbSize = Marshal.SizeOf(scrollInfo);

			scrollInfo.fMask = 0x10 | 0x1 | 0x2;                                // SIF_RANGE = 0x1, SIF_TRACKPOS = 0x10,  SIF_PAGE= 0x2
			NativeMethods.GetScrollInfo(Handle, 1, ref scrollInfo);                       // nBar = 1 -> VScrollbar
			return scrollInfo.max == scrollInfo.nTrackPos + scrollInfo.nPage;
		}


		public void SuspendPainting()
		{
			if (_Painting)
			{
				_SuspendIndex = SelectionStart;
				_SuspendLength = SelectionLength;
				NativeMethods.SendMessage(Handle, NativeMethods.EM_GETSCROLLPOS, 0, ref _ScrollPoint);
				NativeMethods.SendMessage(Handle, NativeMethods.WM_SETREDRAW, 0, IntPtr.Zero);
				_EventMask = NativeMethods.SendMessage(Handle, NativeMethods.EM_GETEVENTMASK, 0, IntPtr.Zero);
				_Painting = false;
			}
		}

		public void ResumePainting()
		{
			if (!_Painting)
			{
				Select(_SuspendIndex, _SuspendLength);
				NativeMethods.SendMessage(Handle, NativeMethods.EM_SETSCROLLPOS, 0, ref _ScrollPoint);
				NativeMethods.SendMessage(Handle, NativeMethods.EM_SETEVENTMASK, 0, _EventMask);
				NativeMethods.SendMessage(Handle, NativeMethods.WM_SETREDRAW, 1, IntPtr.Zero);
				_Painting = true;
				Invalidate();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="text"></param>
		/// <param name="c"></param>
		/// <param name="newlinePre"> Append in a new line </param>
		void AppendTextBox(string text, Color? c = null, bool printPrefix = false, bool newlinePre = false, bool forceAutoScroll = false, bool addDate = false)
		{
			Color color = c ?? Color.Black;

			if (InvokeRequired)
			{
				BeginInvoke(new Action<string, Color?, bool, bool, bool, bool>(AppendTextBox), [text, color, printPrefix, newlinePre, forceAutoScroll, addDate]);
				return;
			}

			if (newlinePre)
				AppendText("\n");

			if (addDate || AddDate)                                                                        // Date if wanted
			{
				SelectionStart = TextLength;
				SelectionLength = text.Length;
				SelectionColor = Color.Blue;
				AppendText(System.DateTime.Now.ToString() + ": ");
			}

			if (printPrefix)
				AppendText(">");

			bool autoscroll = forceAutoScroll || AutoScroll;

			if (!autoscroll)
				SuspendPainting();

			SelectionStart = TextLength;              // Set fore color
			SelectionLength = 0;
			SelectionColor = color;
			AppendText(text);
			SelectionColor = ForeColor;               // Return default color

			if (!autoscroll)
				ResumePainting();
			else
			{
				SelectionStart = Text.Length;
				ScrollToCaret();
			}
		}
	}
}
