using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BRichTextBox
{
	internal static class NativeMethods
    {
        public const int WM_USER        = 0x400;
        public const int WM_SETREDRAW   = 0x000B;
        public const int EM_GETEVENTMASK = WM_USER + 59;
        public const int EM_SETEVENTMASK = WM_USER + 69;
        public const int EM_GETSCROLLPOS = WM_USER + 221;
        public const int EM_SETSCROLLPOS = WM_USER + 222;

        public const short SWP_NOSIZE = 1;
        public const short SWP_NOZORDER = 0X4;
        public const int SWP_SHOWWINDOW = 0x0040;

		public struct SCROLLINFO
		{
			public int cbSize;
			public int fMask;
			public int min;
			public int max;
			public int nPage;
			public int nPos;
			public int nTrackPos;
		}

		public const int STD_OUTPUT_HANDLE = -11;

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PostThreadMessage(uint threadId, uint msg, IntPtr wParam, IntPtr lParam);

		public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
		[DllImport("user32.dll")]
		public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		/// <summary>
		/// Set windows position
		/// </summary>
		/// <param name="hWnd">				</param>
		/// <param name="hWndInsertAfter">	</param>
		/// <param name="x">	</param>
		/// <param name="Y">	</param>
		/// <param name="cx">	</param>
		/// <param name="cy">	</param>
		/// <param name="wFlags"> SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW</param>
		/// <returns></returns>
		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

		/// <summary>
		/// Move window
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <param name="nWidth"></param>
		/// <param name="nHeight"></param>
		/// <param name="bRepaint"></param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr MonitorFromRect(IntPtr lprc, int dwFlags);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern bool AllocConsole();

		[System.Runtime.InteropServices.DllImport("User32.dll")]
		public static extern bool ShowWindow(IntPtr handle, int nCmdShow);

		[System.Runtime.InteropServices.DllImport("User32.dll")]
		public static extern bool IsIconic(IntPtr handle);

		// defines for commandline output
		[DllImport("kernel32.dll")]
		public static extern bool AttachConsole(int dwProcessId);

		public const int ATTACH_PARENT_PROCESS = -1;

		[DllImport("User32.dll", CharSet = CharSet.Unicode)]
		public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] String lParam);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, [MarshalAs(UnmanagedType.LPWStr)] String lpszClass, [MarshalAs(UnmanagedType.LPWStr)] String lpszWindow);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, Int32 wParam, ref Point lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, Int32 wParam, IntPtr lParam);

		[DllImport("user32")]
		public static extern int GetScrollInfo(IntPtr hwnd, int nBar, ref SCROLLINFO scrollInfo);

			}
		}
