using System.Runtime.InteropServices;

namespace ReMOSD;

static class PInvoke
{
  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct RECT
  {
    public int left;
    public int top;
    public int right;
    public int bottom;
  }

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrgnbox </summary>
  public const int ERROR = 0;

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-appcommand </summary>
  public const int WM_APPCOMMAND = 793;

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-appcommand </summary>
  public const int APPCOMMAND_VOLUME_MUTE = 8;

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrgnbox </summary>
  [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern int GetWindowRgnBox(nint hWnd, out RECT lprc);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowrgn </summary>
  [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern int SetWindowRgn(nint hWnd, nint hRgn, bool bRedraw);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow </summary>
  [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern uint GetDpiForWindow(nint hWnd);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getshellwindow </summary>
  [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern nint GetShellWindow();

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessagea </summary>
  [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern nint SendMessageA(nint hWnd, uint Msg, nint wParam, nint lParam);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-findwindowa </summary>
  [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern nint FindWindowA(string? lpClassName, string? lpWindowName);

  /// <summary> https://learn.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-createrectrgn </summary>
  [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern nint CreateRectRgn(int x1, int y1, int x2, int y2);

  /// <summary> https://learn.microsoft.com/en-us/windows/console/allocconsole </summary>
  [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
  public static extern bool AllocConsole();
}