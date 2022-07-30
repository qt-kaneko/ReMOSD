// This file is part of ReMOSD.
//
// ReMOSD is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// ReMOSD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with ReMOSD. If not, see <https://www.gnu.org/licenses/>.
// Copyright 2022 Kaneko Qt

namespace Windows.Win32.Foundation;

unsafe readonly partial struct PWSTR {
  public static implicit operator string(PWSTR value) => value.ToString();
  public static implicit operator PWSTR(string value) {
    fixed (char* valuePtr = value)
      return new PWSTR(valuePtr);
  }
}