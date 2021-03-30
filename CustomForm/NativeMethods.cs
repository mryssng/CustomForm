using System;
using System.Runtime.InteropServices;

namespace NativeAPI
{
    public enum GetWindow_Cmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6
    }

    public enum HitTestValues
    {
        HT_ERROR = -2,
        HT_TRANSPARENT = -1,
        HT_NOWHERE = 0,
        HT_CLIENT = 1,
        HT_CAPTION = 2,
        HT_SYSMENU = 3,
        HT_GROWBOX = 4,
        HT_MENU = 5,
        HT_HSCROLL = 6,
        HT_VSCROLL = 7,
        HT_MINBUTTON = 8,
        HT_MAXBUTTON = 9,
        HT_LEFT = 10,
        HT_RIGHT = 11,
        HT_TOP = 12,
        HT_TOPLEFT = 13,
        HT_TOPRIGHT = 14,
        HT_BOTTOM = 15,
        HT_BOTTOMLEFT = 16,
        HT_BOTTOMRIGHT = 17,
        HT_BORDER = 18,
        HT_OBJECT = 19,
        HT_CLOSE = 20,
        HT_HELP = 21
    }

    public enum WindowMessages
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

        WM_QUIT = 0x0012,
        WM_ERASEBKGND = 0x0014,
        WM_SYSCOLORCHANGE = 0x0015,
        WM_SHOWWINDOW = 0x0018,

        WM_ACTIVATEAPP = 0x001C,

        WM_SETCURSOR = 0x0020,
        WM_MOUSEACTIVATE = 0x0021,
        WM_DRAWITEM = 0x002B,
        WM_GETMINMAXINFO = 0x0024,
        WM_MEASUREITEM = 0x002C,

        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_NOTIFY = 0x004E,

        WM_REFLECT = 0x2000,
        WM_REFLECT_NOTIFY = 0x204E,

        WM_CONTEXTMENU = 0x007B,
        WM_STYLECHANGING = 0x007C,
        WM_STYLECHANGED = 0x007D,
        WM_DISPLAYCHANGE = 0x007E,
        WM_GETICON = 0x007F,
        WM_SETICON = 0x0080,

        // non client area
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCCALCSIZE = 0x0083,
        WM_NCHITTEST = 0x84,
        WM_NCPAINT = 0x0085,
        WM_NCACTIVATE = 0x0086,

        WM_GETDLGCODE = 0x0087,

        WM_SYNCPAINT = 0x0088,

        // non client mouse
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

        // keyboard
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,

        WM_SYSCOMMAND = 0x0112,

        // menu
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

        // mouse
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
        WM_MOUSELAST = 0x020D,

        WM_PARENTNOTIFY = 0x0210,
        WM_ENTERMENULOOP = 0x0211,
        WM_EXITMENULOOP = 0x0212,

        WM_NEXTMENU = 0x0213,
        WM_SIZING = 0x0214,
        WM_CAPTURECHANGED = 0x0215,
        WM_MOVING = 0x0216,

        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,

        WM_MOUSELEAVE = 0x02A3,
        WM_MOUSEHOVER = 0x02A1,
        WM_NCMOUSEHOVER = 0x02A0,
        WM_NCMOUSELEAVE = 0x02A2,

        WM_MDIACTIVATE = 0x0222,
        WM_HSCROLL = 0x0114,
        WM_VSCROLL = 0x0115,

        WM_SYSMENU = 0x313,

        WM_PRINT = 0x0317,
        WM_PRINTCLIENT = 0x0318,
    }

    public enum SystemCommands
    {
        SC_SIZE = 0xF000,
        SC_MOVE = 0xF010,
        SC_MINIMIZE = 0xF020,
        SC_MAXIMIZE = 0xF030,
        SC_MAXIMIZE2 = 0xF032,	// fired from double-click on caption
        SC_NEXTWINDOW = 0xF040,
        SC_PREVWINDOW = 0xF050,
        SC_CLOSE = 0xF060,
        SC_VSCROLL = 0xF070,
        SC_HSCROLL = 0xF080,
        SC_MOUSEMENU = 0xF090,
        SC_KEYMENU = 0xF100,
        SC_ARRANGE = 0xF110,
        SC_RESTORE = 0xF120,
        SC_RESTORE2 = 0xF122,	// fired from double-click on caption
        SC_TASKLIST = 0xF130,
        SC_SCREENSAVE = 0xF140,
        SC_HOTKEY = 0xF150,

        SC_DEFAULT = 0xF160,
        SC_MONITORPOWER = 0xF170,
        SC_CONTEXTHELP = 0xF180,
        SC_SEPARATOR = 0xF00F
    }

    public enum ResizingEdge
    {
        WMSZ_TOP = 3,
        WMSZ_TOPLEFT = 4,
        WMSZ_TOPRIGHT = 5,
        WMSZ_LEFT = 1,
        WMSZ_RIGHT = 2,
        WMSZ_BOTTOM = 6,
        WMSZ_BOTTOMLEFT = 7,
        WMSZ_BOTTOMRIGHT = 8
    }

    public enum ABNotify : int
    {
        ABN_STATECHANGE = 0,
        ABN_POSCHANGED,
        ABN_FULLSCREENAPP,
        ABN_WINDOWARRANGE
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width()
        {
            return Right - Left;
        }

        public int Height()
        {
            return Bottom - Top;
        }


        public RECT(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x,
                            y,
                            x + width,
                            y + height);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WINDOWPOS
    {
        internal IntPtr hwnd;
        internal IntPtr hWndInsertAfter;
        internal int x;
        internal int y;
        internal int cx;
        internal int cy;
        internal uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINTS
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HDLAYOUT
    {
        public IntPtr prc;//RECT*
        public IntPtr pwpos;//WINDOWPOS*
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct NMCUSTOMDRAW
    {
        public NMHDR hdr;
        public int dwDrawStage;
        public IntPtr hdc;
        public RECT rc;
        public IntPtr dwItemSpec;
        public int uItemState;
        public IntPtr lItemlParam;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct NMLVCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public uint clrText;
        public uint clrTextBk;
        public int iSubItem;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public IntPtr idFrom;
        public int code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHEADER
    {
        public NMHDR nhdr;
        public int iItem;
        public int iButton;
        public IntPtr pHDITEM;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DRAWITEMSTRUCT
    {
        public int ctlType;
        public int ctlID;
        public int itemID;
        public int itemAction;
        public int itemState;
        public IntPtr hWndItem;
        public IntPtr hDC;
        public int rcLeft;
        public int rcTop;
        public int rcRight;
        public int rcBottom;
        public IntPtr itemData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeMessage
    {
        public IntPtr handle;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public int cbSize; // initialize this field using: Marshal.SizeOf(typeof(APPBARDATA));
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;
    }

    [Flags]
    public enum WindowStyle
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = -2147483648, //0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_CAPTION = 0x00C00000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
        WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU |
                                WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
        WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),
        WS_CHILDWINDOW = (WS_CHILD)
    }

    [Flags]
    public enum SetWindowPos
    {
        SWP_NOSIZE = 1,
        SWP_NOMOVE = 2,
        SWP_NOZORDER = 4,
        SWP_NOREDRAW = 8,
        SWP_NOACTIVATE = 16,
        SWP_FRAMECHANGED = 32,

        SWP_ZORDERONLY = SWP_NOSIZE | SWP_NOMOVE | SWP_NOREDRAW | SWP_NOACTIVATE,
        SWP_SIZEONLY = SWP_NOMOVE | SWP_NOREDRAW | SWP_NOZORDER | SWP_NOACTIVATE,
        SWP_UPDATE_FRAME = SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE | SWP_NOZORDER | SWP_FRAMECHANGED
    }

    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetCapture();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hwnd, int msg, int wparam, int lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hwnd, int msg, int wparam, POINTS pos);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageRECT(IntPtr hWnd, int msg, int wParam, ref RECT r);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int msg, int wparam, int lparam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int msg, int wparam, POINTS pos);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32.dll")]
        public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("gdi32.dll")]
        public static extern int GetRgnBox(IntPtr hrgn, out RECT lprc);

        [DllImport("user32.dll")]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 Offset);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("shell32.dll")]
        public static extern int SHAppBarMessage(uint dwMessage, [In] ref APPBARDATA pData);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObj);

        [DllImport("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int index);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
         );
    }

    public static class NativeConstants
    {
        public const int SM_CXSIZEFRAME = 32;
        public const int SM_CYSIZEFRAME = 33;
        public const int SM_CXPADDEDBORDER = 92;

        public const int GWL_ID = (-12);
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);

        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCRBUTTONUP = 0x00A5;

        public const uint TPM_LEFTBUTTON = 0x0000;
        public const uint TPM_RIGHTBUTTON = 0x0002;
        public const uint TPM_LEFTALIGN = 0x0000; // ポップアップメニューの左端を、指定するX座標に合わせる(TrackPopupMenu)
        public const uint TPM_RETURNCMD = 0x0100; // 関数の戻り値として、ユーザーが選択したメニュー項目の識別子を返する(TrackPopupMenu)

        public static readonly IntPtr TRUE = new IntPtr(1);
        public static readonly IntPtr FALSE = new IntPtr(0);

        public const uint ABM_GETSTATE = 0x4;
        public const int ABS_AUTOHIDE = 0x1;

        public const int CS_DROPSHADOW = 0x00020000;

        // 論理インチ当たりの画面の水平方向のピクセル数
        public const int LOGPIXELSX = 88;
        // 論理インチ当たりの画面の垂直方向のピクセル数
        public const int LOGPIXELSY = 90;

    }       
}
