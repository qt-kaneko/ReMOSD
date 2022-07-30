// This file is part of ReMOSD.
//
// ReMOSD is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// ReMOSD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with ReMOSD. If not, see <https://www.gnu.org/licenses/>.
// Copyright 2022 Kaneko Qt

using System.ComponentModel;
using System.Runtime.InteropServices;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Accessibility;
using Windows.Win32.UI.WindowsAndMessaging;

using static Windows.Win32.PInvoke;
using static Windows.Win32.UI.WindowsAndMessaging.GET_ANCESTOR_FLAGS;

unsafe class WindowsMediaOsd {
  const string _className = "NativeHWNDHost\0";

  public HWND HWnd { get; private set; }

  public HRGN Region {
    set {
      if (!SetWindowRgn(HWnd, value, true))
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }
  }

  public uint Dpi {
    get {
      uint dpi = GetDpiForWindow(HWnd);
      if (dpi == default)
        throw new Win32Exception(Marshal.GetLastWin32Error());

      return dpi;
    }
  }

  WindowsMediaOsd() { }

  public static WindowsMediaOsd Find() {
    var osd = new WindowsMediaOsd();

    osd.HWnd = FindWindowEx(default, default, _className, default);
    if (osd.HWnd == default) {
      GetWindowThreadProcessId(GetShellWindow(), out uint shellPid);

      HWINEVENTHOOK winEventHook = SetWinEventHook(EVENT_OBJECT_CREATE, EVENT_OBJECT_CREATE,
                                                   default(HINSTANCE), FindHWnd,
                                                   shellPid, 0,
                                                   WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
      if (winEventHook == default)
        throw new Win32Exception(Marshal.GetLastWin32Error());

      GetMessage(out MSG msg, default, WM_QUIT, WM_QUIT); // Wait for quit message
    }

    return osd;


    void FindHWnd(HWINEVENTHOOK hWinEventHook,
                  uint @event,
                  HWND hWnd,
                  int idObject,
                  int idChild,
                  uint idEventThread,
                  uint dwmsEventTime) {
      GetClassName(hWnd, out string lpClassName, _className.Length);

      if (lpClassName == _className &&
          hWnd == GetAncestor(hWnd, GA_ROOT)) { // Filter languages window
        osd.HWnd = hWnd;
        UnhookWinEvent(hWinEventHook);
        PostQuitMessage(0); // Send quit message
      }
    }
  }
}