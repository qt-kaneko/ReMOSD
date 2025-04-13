using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

using static Windows.Win32.PInvoke;
using static Windows.Win32.System.SystemServices.APPCOMMAND_ID;
using static Windows.Win32.Graphics.Gdi.GDI_REGION_TYPE;

AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

var osdHWnd = FindMediaOsd();
var osdRgnType = GetWindowRgnBox(osdHWnd, out _);

HRGN newOsdRgn;
// The specified window does not have a region...
// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrgn
if (osdRgnType == RGN_ERROR)
{
  #if REMOSD
    var osdDpi = GetDpiForWindow(osdHWnd);
    if (osdDpi == 0) throw new Win32Exception();

    var osdScalingCoefficient = osdDpi / 96.0f;

    newOsdRgn = CreateRectRgn(0, 0,
                              (int)Math.Round(65 * osdScalingCoefficient),
                              (int)Math.Round(140 * osdScalingCoefficient));
  #elif REVOSD
    newOsdRgn = CreateRectRgn(0, 0,
                              0, 0);
  #endif
  if (newOsdRgn == HRGN.Null) throw new Win32Exception();
}
else
{
  newOsdRgn = HRGN.Null;
}

var result = SetWindowRgn(osdHWnd, newOsdRgn, true);
if (result == 0) throw new Win32Exception();

static HWND FindMediaOsd()
{
  var shellHWnd = GetShellWindow();

  var hWnd = HWND.Null;
  for (var attempt = 1; attempt <= 5; ++attempt)
  {
    // Trigger show volume control window so it is created and can be found later
    // to pack APPCOMMAND lParam https://stackoverflow.com/a/29301152/18449435
    // https://forums.codeguru.com/showthread.php?147192-How-to-construct-WM_APPCOMMAND-message
    SendMessage(shellHWnd, WM_APPCOMMAND, 0, (int)APPCOMMAND_VOLUME_MUTE << 16);
    SendMessage(shellHWnd, WM_APPCOMMAND, 0, (int)APPCOMMAND_VOLUME_MUTE << 16);

    Thread.Sleep(250);

    hWnd = FindWindow("NativeHWNDHost\0", null);
    if (hWnd != HWND.Null) break;

    Thread.Sleep(750);
  }
  if (hWnd == HWND.Null) throw new InvalidOperationException("Media OSD was not found :(");

  return hWnd;
}

static void OnUnhandledException(object s, UnhandledExceptionEventArgs e)
{
  var exception = e.ExceptionObject as Exception;

  AllocConsole();
  #if REMOSD
    Console.Title = "ReMOSD";
  #elif REVOSD
    Console.Title = "ReVOSD";
  #endif

  Console.WriteLine(exception);
  Console.WriteLine();

  Console.WriteLine("""
                    +--------------------------------------------------+
                    | PLEASE CONSIDER SENDING A COPY OF THE TEXT ABOVE |
                    | OR SCREENSHOT WITH THIS WINDOW TO A DEVELOPER.   |
                    |                                                  |
                    | Telegram: @qt-kaneko                             |
                    | GitHub:   github.com/qt-kaneko/remosd            |
                    +--------------------------------------------------+
                    """);
  Console.WriteLine();

  Console.WriteLine("Press any key to close this window . . .");
  Console.ReadKey(true);

  Environment.Exit(Marshal.GetHRForException(exception));
}