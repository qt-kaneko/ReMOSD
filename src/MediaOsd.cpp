#include "MediaOsd.h"

#include <string_view>
#include <stdexcept>
#include <thread>
#include <chrono>
#include <windows.h>

using namespace std::literals;

static constexpr std::string_view _className = "NativeHWNDHost\0";

void MediaOsd::setRegion(HRGN value)
{
  auto result = ::SetWindowRgn(_hWnd, value, true);
  if (result == 0) throw std::system_error(::GetLastError(), std::system_category());
}

unsigned int MediaOsd::getDpi()
{
  auto dpi = ::GetDpiForWindow(_hWnd);
  if (dpi == 0) throw std::system_error(::GetLastError(), std::system_category());

  return dpi;
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

  for (auto attempt = 1; attempt <= 5; ++attempt)
  {
    osd._hWnd = ::FindWindowExA(nullptr, nullptr, _className.cbegin(), nullptr);
    if (osd._hWnd != nullptr) break;

    std::this_thread::sleep_for(250ms);
  }
  if (osd._hWnd == nullptr) throw std::runtime_error("Media OSD window was not found :(");

  return osd;
}