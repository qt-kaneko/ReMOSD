using System.Runtime.InteropServices;

namespace ReMOSD;

static class PInvoke
{
  /// <summary> https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-appcommand </summary>
  public const uint WM_APPCOMMAND = 793;

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-appcommand </summary>
  public const uint APPCOMMAND_VOLUME_MUTE = 8;

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrgn </summary>
  public const int NULLREGION = 0;
  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrgn </summary>
  public const int ERROR = 3;

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrgn </summary>
  [DllImport("user32.dll")]
  public static extern int GetWindowRgn(nint hWnd, nint hRgn);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowrgn </summary>
  [DllImport("user32.dll")]
  public static extern int SetWindowRgn(nint hWnd, nint hRgn, bool bRedraw);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow </summary>
  [DllImport("user32.dll")]
  public static extern uint GetDpiForWindow(nint hWnd);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getshellwindow </summary>
  [DllImport("user32.dll")]
  public static extern nint GetShellWindow();

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage </summary>
  [DllImport("user32.dll")]
  public static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-findwindowexa </summary>
  [DllImport("user32.dll")]
  public static extern nint FindWindowExA(nint hWndParent, nint hWndChildAfter, string lpszClass, string? lpszWindow);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-createrectrgn </summary>
  [DllImport("gdi32.dll")]
  public static extern nint CreateRectRgn(int x1, int y1, int x2, int y2);

  /// <summary> https://learn.microsoft.com/en-us/windows/console/allocconsole </summary>
  [DllImport("kernel32.dll")]
  public static extern bool AllocConsole();
}