using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using static Windows.Win32.System.SystemServices.APPCOMMAND_ID;
using static Windows.Win32.PInvoke;
using System;
using System.Threading;

class MediaOsd
{
  const string _className = "NativeHWNDHost\0";

  public HWND HWnd { get; private set; }

  public HRGN Region
  {
    set {
      var result = SetWindowRgn(HWnd, value, true);
      if (result == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
    }
  }

  public uint Dpi
  {
    get {
      var dpi = GetDpiForWindow(HWnd);
      if (dpi == 0) throw new Win32Exception(Marshal.GetLastWin32Error());

      return dpi;
    }
  }

  MediaOsd() {}

  public static MediaOsd Find()
  {
    var osd = new MediaOsd();

    // Trigger show volume control window so it is created and can be found later
    // (to pack APPCOMMAND lParam https://stackoverflow.com/a/29301152/18449435)
    // (https://forums.codeguru.com/showthread.php?147192-How-to-construct-WM_APPCOMMAND-message)
    SendMessage(GetShellWindow(), WM_APPCOMMAND, 0, (int)APPCOMMAND_VOLUME_MUTE << 16);
    SendMessage(GetShellWindow(), WM_APPCOMMAND, 0, (int)APPCOMMAND_VOLUME_MUTE << 16);

    for (var attempt = 1; attempt <= 5; ++attempt)
    {
      osd.HWnd = FindWindowEx(default, default, _className, default);
      if (osd.HWnd != default) break;

      Thread.Sleep(250);
    }
    if (osd.HWnd == default) throw new Exception("Media OSD was not found :(");

    return osd;
  }
}