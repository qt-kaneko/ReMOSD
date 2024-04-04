using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using ReMOSD;
using static ReMOSD.PInvoke;

AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

var osd = MediaOsd.Find();

var osdRegion = osd.GetRegionBox(out _);

nint newOsdRegion;
// The specified window does not have a region...
// (see https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrgn)
if (osdRegion == ERROR) // Unchanged
{
  var osdScalingCoefficient = osd.GetDpi() / 96.0f;

  newOsdRegion = CreateRectRgn(0, 0,
                               (int)Math.Round(65 * osdScalingCoefficient),
                               (int)Math.Round(140 * osdScalingCoefficient));
  if (newOsdRegion == default) throw new Win32Exception(Marshal.GetLastWin32Error());
}
else // Reset
{
  newOsdRegion = 0;
}

osd.SetRegion(newOsdRegion);

static void OnUnhandledException(object s, UnhandledExceptionEventArgs e)
{
  var exception = e.ExceptionObject as Exception;

  AllocConsole();
  Console.Title = "ReMOSD";

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