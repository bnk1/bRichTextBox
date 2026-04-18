using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BRichTextBox
{
    [ToolboxItem(true)]
    public partial class BRichTextBox : RichTextBox
    {
        private Point  _ScrollPoint;
        private int    _SuspendCount  = 0;
        private IntPtr _EventMask;
        private int    _SuspendIndex  = 0;
        private int    _SuspendLength = 0;

        /// <summary>
        /// If true, the control will scroll to the bottom after appending text.
        /// </summary>
        [DefaultValue(true)]
        public bool AutoScrollToBottom { get; set; } = true;

        /// <summary>
        /// If true, each appended line will start with the current date/time.
        /// </summary>
        [DefaultValue(false)]
        public bool AddDate { get; set; } = false;

        public BRichTextBox()
        {
            InitializeComponent();
        }

        public void AppendExc(Exception exc, bool detailed = false)
        {
            if (exc == null)
            {
                return;
            }

            AppendLine(exc.Message, Color.Red);

            if (detailed)
            {
                if (!string.IsNullOrEmpty(exc.StackTrace))
                {
                    AppendLine("Trace: " + exc.StackTrace, Color.Red);
                }

                if (exc.InnerException != null)
                {
                    AppendExc(exc.InnerException, detailed);
                }
            }
        }

        public void AppendExc(Exception exc)
        {
            if (exc == null)
			 return;
			 
            AppendLine(exc.Message, Color.Red);
        }

        public void AppendErr(string text)
        {
            AppendLine(text, Color.Red);
        }

        public void AppendLine(string text, Color? color = null)
        {
            AppendTextBox(text + Environment.NewLine, color);
        }

        /// <summary>
        /// Returns true if the vertical scrollbar is already at the bottom.
        /// </summary>
        public bool ReachedBottom()
        {
            NativeMethods.SCROLLINFO scrollInfo = new NativeMethods.SCROLLINFO();
            scrollInfo.cbSize = Marshal.SizeOf(scrollInfo);
            // SIF_RANGE = 0x1, SIF_TRACKPOS = 0x10, SIF_PAGE = 0x2
            scrollInfo.fMask = 0x10 | 0x1 | 0x2;

            NativeMethods.GetScrollInfo(Handle, 1, ref scrollInfo); // nBar = 1 -> VScrollbar

            return scrollInfo.max == scrollInfo.nTrackPos + scrollInfo.nPage;
        }

        public void SuspendPainting()
        {
            if (_SuspendCount++ == 0)
            {
                _SuspendIndex  = SelectionStart;
                _SuspendLength = SelectionLength;

                NativeMethods.SendMessage(Handle, NativeMethods.EM_GETSCROLLPOS, 0, ref _ScrollPoint);
                NativeMethods.SendMessage(Handle, NativeMethods.WM_SETREDRAW, 0, IntPtr.Zero);
                _EventMask = NativeMethods.SendMessage(Handle, NativeMethods.EM_GETEVENTMASK, 0, IntPtr.Zero);
            }
        }

        public void ResumePainting()
        {
            if (_SuspendCount == 0)
				return;

            if (--_SuspendCount == 0)
            {
                Select(_SuspendIndex, _SuspendLength);
                NativeMethods.SendMessage(Handle, NativeMethods.EM_SETSCROLLPOS, 0, ref _ScrollPoint);
                NativeMethods.SendMessage(Handle, NativeMethods.EM_SETEVENTMASK, 0, _EventMask);
                NativeMethods.SendMessage(Handle, NativeMethods.WM_SETREDRAW, 1, IntPtr.Zero);
                Invalidate();
            }
        }

        /// <summary>
        /// Internal helper to append colored text (optionally with prefix, date, and auto-scroll).
        /// </summary>
        /// <param name="text">Text to append.</param>
        /// <param name="c">Foreground color (null = default).</param>
        /// <param name="printPrefix">If true, prepend '&gt;'.</param>
        /// <param name="newlinePre">If true, prepend a newline before the text.</param>
        /// <param name="scrollToBottom">If true, forces scroll to bottom even if AutoScroll is false.</param>
        /// <param name="addDate"> If true, prepend the current date/time (or uses AddDate property).
        /// </param>
        private void AppendTextBox(
            string  text,
            Color?  c              = null,
            bool    printPrefix    = false,
            bool    newlinePre     = false,
            bool    scrollToBottom = false,
            bool    addDate        = false)
        {
            Color color = c ?? ForeColor;

            if (InvokeRequired)
            {
                if (!IsDisposed)
                    BeginInvoke(
                        new Action<string, Color?, bool, bool, bool, bool>(AppendTextBox),
                        text,
                        color,
                        printPrefix,
                        newlinePre,
                        scrollToBottom,
                        addDate);
                return;
            }

            if (newlinePre)
            {
                AppendText(Environment.NewLine);
            }

            bool wantDate = addDate || AddDate;

            if (wantDate)
            {
                SelectionStart = TextLength;
                SelectionLength = 0;
                SelectionColor = Color.Blue;
                AppendText(DateTime.Now.ToString() + ": ");
            }

            if (printPrefix)
            {
                SelectionStart = TextLength;
                SelectionLength = 0;
                SelectionColor = color;
                AppendText(">");
            }

            bool autoScrollEffective = scrollToBottom || AutoScrollToBottom;

            if (!autoScrollEffective)
            {
                SuspendPainting();
            }

            SelectionStart = TextLength;
            SelectionLength = 0;
            SelectionColor = color;
            AppendText(text);
            SelectionColor = ForeColor;

            if (!autoScrollEffective)
            {
                ResumePainting();
            }
            else
            {
                SelectionStart = TextLength;
                ScrollToCaret();
            }
        }
    }
}
