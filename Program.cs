// This file is part of ReMOSD.
//
// ReMOSD is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// ReMOSD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with ReMOSD. If not, see <https://www.gnu.org/licenses/>.
// Copyright 2022 Kaneko Qt

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

using Windows.Win32.Graphics.Gdi;

using static Windows.Win32.PInvoke;

static class Program {
  static readonly Size _miniOsdSize = new(65, 140);

  static void Main(string[] args) {
    AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

    WindowsMediaOsd osd = WindowsMediaOsd.Find();

    HRGN newOsdRegion;
    if (args.Contains("--restore") || args.Contains("-r")) {
      newOsdRegion = default;
    }
    else {
      float osdScalingCoefficient = osd.Dpi / 96.0f;

      newOsdRegion = CreateRectRgn(0, 0,
                                   (int)Math.Round(_miniOsdSize.Width * osdScalingCoefficient),
                                   (int)Math.Round(_miniOsdSize.Height * osdScalingCoefficient));
      if (newOsdRegion == default)
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    osd.Region = newOsdRegion;
  }

  static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
    Exception exception = e.ExceptionObject as Exception;

    AllocConsole();

    Console.Title = "ReMOSD";

    Console.WriteLine(exception);

    Console.WriteLine();

    Console.WriteLine("""
                      +----------------------------------------------+
                      | PLEASE CONSIDER TO SEND A COPY OF TEXT ABOVE |
                      | OR SCREENSHOT WITH THIS WINDOW TO DEVELOPER. |
                      |                                              |
                      | Telegram: @qt_kaneko                         |
                      | GitHub:   github.com/qt-kaneko/remosd        |
                      +----------------------------------------------+
                      """);

    Console.WriteLine();

    Console.WriteLine("Press any key to close this window . . .");
    Console.ReadKey(true);

    Environment.Exit(Marshal.GetHRForException(exception));
  }
}