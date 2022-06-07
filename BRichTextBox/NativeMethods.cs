using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;


namespace BRichTextBox
{
	public static class NativeMethods
	{
		public const int WM_USER = 0x400;
		public const int WM_SETREDRAW = 0x000B;
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

		/// <summary>
		/// Windows messages enum
		/// </summary>
		public enum WindowsMessage
		{
			WM_NULL = 0x0000,
			WM_CREATE = 0x0001,
			WM_DESTROY = 0x0002,
			WM_MOVE = 0x0003,
			WM_SIZE = 0x0005,
			WM_ACTIVATE = 0x0006,
			WM_SETFOCUS = 0x0007,
			WM_KILLFOCUS = 0x0008,
			WM_ENABLE = 0x000A,
			WM_SETREDRAW = 0x000B,
			WM_SETTEXT = 0x000C,
			WM_GETTEXT = 0x000D,
			WM_GETTEXTLENGTH = 0x000E,
			WM_PAINT = 0x000F,
			WM_CLOSE = 0x0010,
			WM_QUERYENDSESSION = 0x0011,
			WM_QUERYOPEN = 0x0013,
			WM_ENDSESSION = 0x0016,
			WM_QUIT = 0x0012,
			WM_ERASEBKGND = 0x0014,
			WM_SYSCOLORCHANGE = 0x0015,
			WM_SHOWWINDOW = 0x0018,
			WM_WININICHANGE = 0x001A,
			WM_SETTINGCHANGE = WM_WININICHANGE,
			WM_DEVMODECHANGE = 0x001B,
			WM_ACTIVATEAPP = 0x001C,
			WM_FONTCHANGE = 0x001D,
			WM_TIMECHANGE = 0x001E,
			WM_CANCELMODE = 0x001F,
			WM_SETCURSOR = 0x0020,
			WM_MOUSEACTIVATE = 0x0021,
			WM_CHILDACTIVATE = 0x0022,
			WM_QUEUESYNC = 0x0023,
			WM_GETMINMAXINFO = 0x0024,
			WM_PAINTICON = 0x0026,
			WM_ICONERASEBKGND = 0x0027,
			WM_NEXTDLGCTL = 0x0028,
			WM_SPOOLERSTATUS = 0x002A,
			WM_DRAWITEM = 0x002B,
			WM_MEASUREITEM = 0x002C,
			WM_DELETEITEM = 0x002D,
			WM_VKEYTOITEM = 0x002E,
			WM_CHARTOITEM = 0x002F,
			WM_SETFONT = 0x0030,
			WM_GETFONT = 0x0031,
			WM_SETHOTKEY = 0x0032,
			WM_GETHOTKEY = 0x0033,
			WM_QUERYDRAGICON = 0x0037,
			WM_COMPAREITEM = 0x0039,
			WM_GETOBJECT = 0x003D,
			WM_COMPACTING = 0x0041,
			WM_COMMNOTIFY = 0x0044,
			WM_WINDOWPOSCHANGING = 0x0046,
			WM_WINDOWPOSCHANGED = 0x0047,
			WM_POWER = 0x0048,
			WM_COPYDATA = 0x004A,
			WM_CANCELJOURNAL = 0x004B,
			WM_NOTIFY = 0x004E,
			WM_INPUTLANGCHANGEREQUEST = 0x0050,
			WM_INPUTLANGCHANGE = 0x0051,
			WM_TCARD = 0x0052,
			WM_HELP = 0x0053,
			WM_USERCHANGED = 0x0054,
			WM_NOTIFYFORMAT = 0x0055,
			WM_CONTEXTMENU = 0x007B,
			WM_STYLECHANGING = 0x007C,
			WM_STYLECHANGED = 0x007D,
			WM_DISPLAYCHANGE = 0x007E,
			WM_GETICON = 0x007F,
			WM_SETICON = 0x0080,
			WM_NCCREATE = 0x0081,
			WM_NCDESTROY = 0x0082,
			WM_NCCALCSIZE = 0x0083,
			WM_NCHITTEST = 0x0084,
			WM_NCPAINT = 0x0085,
			WM_NCACTIVATE = 0x0086,
			WM_GETDLGCODE = 0x0087,
			WM_SYNCPAINT = 0x0088,
			WM_NCMOUSEMOVE = 0x00A0,
			WM_NCLBUTTONDOWN = 0x00A1,
			WM_NCLBUTTONUP = 0x00A2,
			WM_NCLBUTTONDBLCLK = 0x00A3,
			WM_NCRBUTTONDOWN = 0x00A4,
			WM_NCRBUTTONUP = 0x00A5,
			WM_NCRBUTTONDBLCLK = 0x00A6,
			WM_NCMBUTTONDOWN = 0x00A7,
			WM_NCMBUTTONUP = 0x00A8,
			WM_NCMBUTTONDBLCLK = 0x00A9,
			WM_NCXBUTTONDOWN = 0x00AB,
			WM_NCXBUTTONUP = 0x00AC,
			WM_NCXBUTTONDBLCLK = 0x00AD,
			WM_INPUT_DEVICE_CHANGE = 0x00FE,
			WM_INPUT = 0x00FF,
			WM_KEYFIRST = 0x0100,
			WM_KEYDOWN = 0x0100,
			WM_KEYUP = 0x0101,
			WM_CHAR = 0x0102,
			WM_DEADCHAR = 0x0103,
			WM_SYSKEYDOWN = 0x0104,
			WM_SYSKEYUP = 0x0105,
			WM_SYSCHAR = 0x0106,
			WM_SYSDEADCHAR = 0x0107,
			WM_UNICHAR = 0x0109,
			WM_KEYLAST = 0x0109,
			WM_IME_STARTCOMPOSITION = 0x010D,
			WM_IME_ENDCOMPOSITION = 0x010E,
			WM_IME_COMPOSITION = 0x010F,
			WM_IME_KEYLAST = 0x010F,
			WM_INITDIALOG = 0x0110,
			WM_COMMAND = 0x0111,
			WM_SYSCOMMAND = 0x0112,
			WM_TIMER = 0x0113,
			WM_HSCROLL = 0x0114,
			WM_VSCROLL = 0x0115,
			WM_INITMENU = 0x0116,
			WM_INITMENUPOPUP = 0x0117,
			WM_MENUSELECT = 0x011F,
			WM_MENUCHAR = 0x0120,
			WM_ENTERIDLE = 0x0121,
			WM_MENURBUTTONUP = 0x0122,
			WM_MENUDRAG = 0x0123,
			WM_MENUGETOBJECT = 0x0124,
			WM_UNINITMENUPOPUP = 0x0125,
			WM_MENUCOMMAND = 0x0126,
			WM_CHANGEUISTATE = 0x0127,
			WM_UPDATEUISTATE = 0x0128,
			WM_QUERYUISTATE = 0x0129,
			WM_CTLCOLORMSGBOX = 0x0132,
			WM_CTLCOLOREDIT = 0x0133,
			WM_CTLCOLORLISTBOX = 0x0134,
			WM_CTLCOLORBTN = 0x0135,
			WM_CTLCOLORDLG = 0x0136,
			WM_CTLCOLORSCROLLBAR = 0x0137,
			WM_CTLCOLORSTATIC = 0x0138,
			MN_GETHMENU = 0x01E1,
			WM_MOUSEFIRST = 0x0200,
			WM_MOUSEMOVE = 0x0200,
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_LBUTTONDBLCLK = 0x0203,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205,
			WM_RBUTTONDBLCLK = 0x0206,
			WM_MBUTTONDOWN = 0x0207,
			WM_MBUTTONUP = 0x0208,
			WM_MBUTTONDBLCLK = 0x0209,
			WM_MOUSEWHEEL = 0x020A,
			WM_XBUTTONDOWN = 0x020B,
			WM_XBUTTONUP = 0x020C,
			WM_XBUTTONDBLCLK = 0x020D,
			WM_MOUSEHWHEEL = 0x020E,
			WM_PARENTNOTIFY = 0x0210,
			WM_ENTERMENULOOP = 0x0211,
			WM_EXITMENULOOP = 0x0212,
			WM_NEXTMENU = 0x0213,
			WM_SIZING = 0x0214,
			WM_CAPTURECHANGED = 0x0215,
			WM_MOVING = 0x0216,
			WM_POWERBROADCAST = 0x0218,
			WM_DEVICECHANGE = 0x0219,
			WM_MDICREATE = 0x0220,
			WM_MDIDESTROY = 0x0221,
			WM_MDIACTIVATE = 0x0222,
			WM_MDIRESTORE = 0x0223,
			WM_MDINEXT = 0x0224,
			WM_MDIMAXIMIZE = 0x0225,
			WM_MDITILE = 0x0226,
			WM_MDICASCADE = 0x0227,
			WM_MDIICONARRANGE = 0x0228,
			WM_MDIGETACTIVE = 0x0229,
			WM_MDISETMENU = 0x0230,
			WM_ENTERSIZEMOVE = 0x0231,
			WM_EXITSIZEMOVE = 0x0232,
			WM_DROPFILES = 0x0233,
			WM_MDIREFRESHMENU = 0x0234,
			WM_IME_SETCONTEXT = 0x0281,
			WM_IME_NOTIFY = 0x0282,
			WM_IME_CONTROL = 0x0283,
			WM_IME_COMPOSITIONFULL = 0x0284,
			WM_IME_SELECT = 0x0285,
			WM_IME_CHAR = 0x0286,
			WM_IME_REQUEST = 0x0288,
			WM_IME_KEYDOWN = 0x0290,
			WM_IME_KEYUP = 0x0291,
			WM_MOUSEHOVER = 0x02A1,
			WM_MOUSELEAVE = 0x02A3,
			WM_NCMOUSEHOVER = 0x02A0,
			WM_NCMOUSELEAVE = 0x02A2,
			WM_WTSSESSION_CHANGE = 0x02B1,
			WM_TABLET_FIRST = 0x02c0,
			WM_TABLET_LAST = 0x02df,
			WM_CUT = 0x0300,
			WM_COPY = 0x0301,
			WM_PASTE = 0x0302,
			WM_CLEAR = 0x0303,
			WM_UNDO = 0x0304,
			WM_RENDERFORMAT = 0x0305,
			WM_RENDERALLFORMATS = 0x0306,
			WM_DESTROYCLIPBOARD = 0x0307,
			WM_DRAWCLIPBOARD = 0x0308,
			WM_PAINTCLIPBOARD = 0x0309,
			WM_VSCROLLCLIPBOARD = 0x030A,
			WM_SIZECLIPBOARD = 0x030B,
			WM_ASKCBFORMATNAME = 0x030C,
			WM_CHANGECBCHAIN = 0x030D,
			WM_HSCROLLCLIPBOARD = 0x030E,
			WM_QUERYNEWPALETTE = 0x030F,
			WM_PALETTEISCHANGING = 0x0310,
			WM_PALETTECHANGED = 0x0311,
			WM_HOTKEY = 0x0312,
			WM_PRINT = 0x0317,
			WM_PRINTCLIENT = 0x0318,
			WM_APPCOMMAND = 0x0319,
			WM_THEMECHANGED = 0x031A,
			WM_CLIPBOARDUPDATE = 0x031D,
			WM_DWMCOMPOSITIONCHANGED = 0x031E,
			WM_DWMNCRENDERINGCHANGED = 0x031F,
			WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,
			WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
			WM_GETTITLEBARINFOEX = 0x033F,
			WM_HANDHELDFIRST = 0x0358,
			WM_HANDHELDLAST = 0x035F,
			WM_AFXFIRST = 0x0360,
			WM_AFXLAST = 0x037F,
			WM_PENWINFIRST = 0x0380,
			WM_PENWINLAST = 0x038F,
			WM_APP = 0x8000,
			WM_USER = 0x0400,
			WM_REFLECT = WM_USER + 0x1C00,
		}

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

		[DllImport("MSPorts.dll", SetLastError = true)]
		public static extern int ComDBOpen(out IntPtr hComDB);

		[DllImport("MSPorts.dll")]
		public static extern long ComDBClose(IntPtr PHCOMDB);

		[DllImport("msports.dll", SetLastError = true)]
		public static extern int ComDBReleasePort(IntPtr hComDB, int ComNumber);

		//[DllImport("msports.dll", SetLastError = true)]
		//public static extern long ComDBGetCurrentPortUsage(IntPtr HComDB, byte Buffer, UInt32 BufferSize, UInt32 ReportType, UInt32 MaxPortsReported);

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

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool SetProcessDPIAware();

		/// <summary>
		/// Set the application to DPI awareness
		/// </summary>
		static public void SetDpiAwareness()
		{
			if (Environment.OSVersion.Version.Major >= 6)
				SetProcessDPIAware();
		}
		/// <summary>
		/// Send text to a window
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="keys"></param>
		/// <param name="asIs"> If false, special chars such as '+','^','()','~', are used as intended on SendKeys (see SendKeys for more info)</param>
		static public void MySendKeys(IntPtr hwnd, string keys, bool asIs = true)
		{
			if (hwnd != IntPtr.Zero)
			{
				if (SetForegroundWindow(hwnd))
				{
					if (asIs)
					{
						keys = keys.Replace("+", "{+}");
						keys = keys.Replace("^", "{^}");
						keys = keys.Replace("~", "{~}");
						keys = keys.Replace("%", "{%}");
						keys = keys.Replace("(", "{(}");
						keys = keys.Replace(")", "{)}");
					}

					System.Windows.Forms.SendKeys.Send(keys);
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="text"></param>
		static public void SendText(IntPtr hwnd, string text)
		{
			WindowsInput.InputSimulator x = new WindowsInput.InputSimulator();

			if (SetForegroundWindow(hwnd))
			{
				ShowWindow(hwnd, SW_RESTORE);

				SendMessage(hwnd, (uint)WindowsMessage.WM_PAINT, (IntPtr)0, (IntPtr)0);

				x.Keyboard.TextEntry(text);
			}
		}

		/// <summary>
		/// Bring the process to the front
		/// </summary>
		/// <param name="process"></param>
		public static void BringProcessToFront(Process process)
		{
			IntPtr handle = process.MainWindowHandle;

			if (IsIconic(handle))
			{
				ShowWindow(handle, SW_RESTORE);
			}

			SetForegroundWindow(handle);
		}

		const int SW_RESTORE = 9;

		/// <summary>
		/// Bring a process to the front (by title)
		/// </summary>
		/// <param name="title"></param>
		public static void BringToFront(string title)
		{
			IntPtr handle = FindWindow(null, title);

			if (handle == IntPtr.Zero)
				return;

			SetForegroundWindow(handle);
		}

		/// <summary>
		/// Release a com port
		/// </summary>
		/// <param name="portNo"></param>
		public static void ReleasePort(int portNo)
		{
			_ = ComDBOpen(out IntPtr HComDB);
			_ = ComDBReleasePort(HComDB, portNo);
			_ = ComDBClose(HComDB);
		}
	}
}