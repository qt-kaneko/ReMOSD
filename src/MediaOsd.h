#pragma once

#include <string_view>
#include <windows.h>

class MediaOsd
{
  public:
    void setRegion(HRGN value);

    unsigned int getDpi();

    HWND getHWnd();

    static MediaOsd find();

  private:
    HWND _hWnd;

    MediaOsd() = default;
};