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
        public bool AutoScrollToBottom { get; set; }

        /// <summary>
        /// If true, each appended line will start with the current date/time.
        /// </summary>
        [DefaultValue(false)]
        public bool AddDate { get; set; }

        /// <summary>
        /// Date/time format used when AddDate is enabled.
        /// </summary>
        [DefaultValue("yyyy-MM-dd HH:mm:ss.fff")]
        public string DateFormat { get; set; }

        /// <summary>
        /// Color used for the date/time prefix.
        /// </summary>
        public Color DateColor { get; set; }

        /// <summary>
        /// Optional maximum text length. 0 = unlimited.
        /// When exceeded, older text is trimmed from the beginning.
        /// </summary>
        [DefaultValue(0)]
        public int MaxTextLengthEx { get; set; }

        /// <summary>
        /// When trimming is required, keep approximately this many characters.
        /// Must be lower than MaxTextLengthEx to avoid trimming on every append.
        /// </summary>
        [DefaultValue(0)]
        public int TrimToTextLength { get; set; }

        /// <summary>
        /// If true and the user is not currently at the bottom, appends preserve the current view.
        /// </summary>
        [DefaultValue(true)]
        public bool PreserveViewWhenNotAtBottom { get; set; }

        public BRichTextBox()
        {
            InitializeComponent();

            AutoScrollToBottom          = true;
            AddDate                     = false;
            DateFormat                  = "yyyy-MM-dd HH:mm:ss.fff";
            DateColor                   = Color.Blue;
            MaxTextLengthEx             = 0;
            TrimToTextLength            = 0;
            PreserveViewWhenNotAtBottom = true;
            HideSelection               = false;
            DetectUrls                  = false;
        }

        public void AppendExc(Exception exc)
        {
            AppendExc(exc, false);
        }

        public void AppendExc(Exception exc, bool detailed)
        {
            if (exc == null)
            {
                return;
            }

            AppendLine(exc.Message, Color.Red);

            if (!detailed)
            {
                return;
            }

            if (!string.IsNullOrEmpty(exc.StackTrace))
            {
                AppendLine("Trace: " + exc.StackTrace, Color.Red);
            }

            if (exc.InnerException != null)
            {
                AppendLine("Inner:", Color.Red);
                AppendExc(exc.InnerException, true);
            }
        }

        public void AppendErr(string text)
        {
            AppendLine(text, Color.Red);
        }

        public void AppendWarn(string text)
        {
            AppendLine(text, Color.DarkOrange);
        }

        public void AppendInfo(string text)
        {
            AppendLine(text, ForeColor);
        }

        public void AppendSuccess(string text)
        {
            AppendLine(text, Color.ForestGreen);
        }

        public void AppendLine(string text)
        {
            AppendLine(text, null);
        }

        public void AppendLine(string text, Color? color)
        {
            AppendTextBox((text ?? string.Empty) + Environment.NewLine, color, false, false, false, false);
        }

        public void Append(string text, Color? color)
        {
            AppendTextBox(text ?? string.Empty, color, false, false, false, false);
        }

        public void AppendWithDate(string text, Color? color)
        {
            AppendTextBox((text ?? string.Empty) + Environment.NewLine, color, false, false, false, true);
        }

        public void ClearSafe()
        {
            if (InvokeRequired)
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke(new MethodInvoker(ClearSafe));
                }

                return;
            }

            Clear();
        }

        /// <summary>
        /// Returns true if the vertical scrollbar is already at or very near the bottom.
        /// </summary>
        public bool ReachedBottom()
        {
            if (!IsHandleCreated)
            {
                return true;
            }

            NativeMethods.SCROLLINFO scrollInfo = new NativeMethods.SCROLLINFO();
            scrollInfo.cbSize = Marshal.SizeOf(scrollInfo);
            scrollInfo.fMask  = 0x1 | 0x2 | 0x4;

            NativeMethods.GetScrollInfo(Handle, 1, ref scrollInfo);

            int bottom = scrollInfo.nPos + scrollInfo.nPage;
            return bottom >= scrollInfo.max - 1;
        }

        public void SuspendPainting()
        {
            if (!IsHandleCreated)
            {
                return;
            }

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
            if (_SuspendCount == 0 || !IsHandleCreated)
            {
                return;
            }

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
        private void AppendTextBox(
            string text,
            Color? c,
            bool printPrefix,
            bool newlinePre,
            bool scrollToBottom,
            bool addDate)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (InvokeRequired)
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke(
                        new Action<string, Color?, bool, bool, bool, bool>(AppendTextBox),
                        text,
                        c,
                        printPrefix,
                        newlinePre,
                        scrollToBottom,
                        addDate);
                }

                return;
            }

            if (IsDisposed)
            {
                return;
            }

            Color color                  = c ?? ForeColor;
            bool userWasAtBottom         = ReachedBottom();
            bool autoScrollEffective     = scrollToBottom || (AutoScrollToBottom && userWasAtBottom);
            bool preserveExistingView    = PreserveViewWhenNotAtBottom && !autoScrollEffective;
            int previousSelectionStart   = SelectionStart;
            int previousSelectionLength  = SelectionLength;

            if (preserveExistingView)
            {
                SuspendPainting();
            }

            try
            {
                Select(TextLength, 0);

                if (newlinePre)
                {
                    SelectionColor = ForeColor;
                    base.AppendText(Environment.NewLine);
                }

                if (addDate || AddDate)
                {
                    SelectionColor = DateColor;
                    base.AppendText(DateTime.Now.ToString(DateFormat) + ": ");
                }

                if (printPrefix)
                {
                    SelectionColor = color;
                    base.AppendText("> ");
                }

                SelectionColor = color;
                base.AppendText(text);
                SelectionColor = ForeColor;

                TrimIfNeeded();

                if (autoScrollEffective)
                {
                    Select(TextLength, 0);
                    ScrollToCaret();
                }
            }
            finally
            {
                if (preserveExistingView)
                {
                    ResumePainting();
                }
                else if (!IsDisposed)
                {
                    Select(previousSelectionStart, previousSelectionLength);
                }
            }
        }

        private void TrimIfNeeded()
        {
            if (MaxTextLengthEx <= 0)
            {
                return;
            }

            if (TextLength <= MaxTextLengthEx)
            {
                return;
            }

            int trimToLength = TrimToTextLength;

            if (trimToLength <= 0 || trimToLength >= MaxTextLengthEx)
            {
                trimToLength = MaxTextLengthEx - (MaxTextLengthEx / 4);
            }

            if (trimToLength < 1)
            {
                trimToLength = 1;
            }

            int removeCount = TextLength - trimToLength;
            int firstKeep   = FindSafeTrimStart(removeCount);

            if (firstKeep <= 0)
            {
                return;
            }

            Select(0, firstKeep);
            SelectedText = string.Empty;
            Select(TextLength, 0);
        }

        private int FindSafeTrimStart(int preferredIndex)
        {
            if (preferredIndex <= 0)
            {
                return 0;
            }

            if (preferredIndex >= TextLength)
            {
                return TextLength;
            }

            int newlineIndex = Text.IndexOf('\n', preferredIndex);
            if (newlineIndex >= 0 && newlineIndex + 1 < TextLength)
            {
                return newlineIndex + 1;
            }

            return preferredIndex;
        }
    }
}
