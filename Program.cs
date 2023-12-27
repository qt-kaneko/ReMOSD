using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

using Windows.Win32.Graphics.Gdi;

using static Windows.Win32.PInvoke;


Size _miniOsdSize = new(65, 140);

AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

var osd = MediaOsd.Find();

HRGN newOsdRegion;
if (args.Contains("--restore") || args.Contains("-r")) newOsdRegion = new HRGN();
else
{
  var osdScalingCoefficient = osd.Dpi / 96.0f;

  newOsdRegion = CreateRectRgn(0, 0,
                               (int)Math.Round(_miniOsdSize.Width * osdScalingCoefficient),
                               (int)Math.Round(_miniOsdSize.Height * osdScalingCoefficient));
  if (newOsdRegion == default) throw new Win32Exception(Marshal.GetLastWin32Error());
}

osd.Region = newOsdRegion;


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