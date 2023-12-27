#include "MediaOsd.h"

#include <iostream>
#include <string>
#include <windows.h>

static constexpr std::string_view _className = "NativeHWNDHost\0";

void MediaOsd::setRegion(HRGN value)
{
  ::SetWindowRgn(_hWnd, value, true);
}

unsigned int MediaOsd::getDpi()
{
  return ::GetDpiForWindow(_hWnd);
}

HWND MediaOsd::getHWnd()
{
  return _hWnd;
}

MediaOsd MediaOsd::find()
{
  auto osd = MediaOsd();

  // Trigger show volume control window so it is created and can be found later
  // (to pack APPCOMMAND lParam https://stackoverflow.com/a/29301152/18449435)
  // (https://forums.codeguru.com/showthread.php?147192-How-to-construct-WM_APPCOMMAND-message)
  ::SendMessageA(::GetShellWindow(), WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_MUTE << 16);
  ::SendMessageA(::GetShellWindow(), WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_MUTE << 16);

  osd._hWnd = ::FindWindowExA(NULL, NULL, _className.cbegin(), NULL);

  if (osd._hWnd == NULL) throw std::runtime_error("Media OSD window was not found :(");

  return osd;
}