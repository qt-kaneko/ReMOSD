// This file is part of ReMOSD.
// 
// ReMOSD is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// ReMOSD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with ReMOSD. If not, see <https://www.gnu.org/licenses/>. 

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace Windows.Win32
{
  static unsafe partial class PInvoke
  {
    /// <inheritdoc cref="SetWindowRgn(HWND, HRGN, BOOL)"/>
    public static bool SetWindowRgn(HWND hWnd, HRGN hRgn, bool bRedraw) =>
      Convert.ToBoolean(SetWindowRgn(hWnd, hRgn, (BOOL)bRedraw));

    /// <inheritdoc cref="GetWindowThreadProcessId(HWND, uint*)"/>
    public static uint GetWindowThreadProcessId(HWND hWnd, out uint lpdwProcessId)
    {
      fixed (uint* lpdwProcessIdPtr = &lpdwProcessId)
        return GetWindowThreadProcessId(hWnd, lpdwProcessIdPtr);
    }

    /// <inheritdoc cref="GetClassName(HWND, PWSTR, int)"/>
    public static int GetClassName(HWND hWnd, out string lpClassName, int nMaxCount)
    {
      lpClassName = new string('\0', nMaxCount);
      return GetClassName(hWnd, lpClassName, nMaxCount);
    }
  }
}
