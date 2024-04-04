using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

using static ReMOSD.PInvoke;

namespace ReMOSD;

class MediaOsd
{
  public nint HWnd { get; private set; }

  MediaOsd() {}

  public nint GetRegion(out int region)
  {
    var hRgn = CreateRectRgn(0, 0, 0, 0);

    region = GetWindowRgn(HWnd, hRgn);
    if (region == ERROR) throw new Win32Exception(Marshal.GetLastWin32Error());

    return hRgn;
  }
  public void SetRegion(nint value)
  {
    var result = SetWindowRgn(HWnd, value, true);
    if (result == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
  }

  public uint GetDpi()
  {
    var dpi = GetDpiForWindow(HWnd);
    if (dpi == 0) throw new Win32Exception(Marshal.GetLastWin32Error());

    return dpi;
  }

  public static MediaOsd Find()
  {
    var shellHWnd = GetShellWindow();

    // Trigger show volume control window so it is created and can be found later
    // (to pack APPCOMMAND lParam https://stackoverflow.com/a/29301152/18449435)
    // (https://forums.codeguru.com/showthread.php?147192-How-to-construct-WM_APPCOMMAND-message)
    SendMessage(shellHWnd, WM_APPCOMMAND, 0, (int)APPCOMMAND_VOLUME_MUTE << 16);
    SendMessage(shellHWnd, WM_APPCOMMAND, 0, (int)APPCOMMAND_VOLUME_MUTE << 16);

    var hWnd = default(nint);
    for (var attempt = 1; attempt <= 5; ++attempt)
    {
      hWnd = FindWindowExA(default, default, "NativeHWNDHost\0", default);
      if (hWnd != default) break;

      Thread.Sleep(250);
    }
    if (hWnd == default) throw new InvalidOperationException("Media OSD was not found :(");

    var osd = new MediaOsd() {
      HWnd = hWnd
    };
    return osd;
  }
}