#pragma once

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
};