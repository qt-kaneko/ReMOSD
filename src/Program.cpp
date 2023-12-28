#include "Program.h"

#include <cmath>
#include <cstdio>
#include <exception>
#include <windows.h>

#include "utils/contains_any_of.h"
#include "MediaOsd.h"

using namespace std::literals;

static constexpr int _miniOsdWidth = 65;
static constexpr int _miniOsdHeight = 140;

Program::Program(const std::vector<std::string_view>& args)
{
  std::set_terminate(onException);

  auto osd = MediaOsd::find();

  HRGN newRegion;
  if (utils::contains_any_of(args.begin(), args.end(), "--restore"sv, "-r"sv))
  {
    newRegion = HRGN();
  }
  else
  {
    auto osdScalingCoefficient = static_cast<float>(osd.getDpi()) / 96.0f;

    auto newRegionWidth = static_cast<int>(::round(_miniOsdWidth * osdScalingCoefficient));
    auto newRegionHeight = static_cast<int>(::round(_miniOsdHeight * osdScalingCoefficient));
    newRegion = ::CreateRectRgn(0, 0, newRegionWidth, newRegionHeight);
    if (newRegion == nullptr) throw std::system_error(::GetLastError(), std::system_category());
  }

  osd.setRegion(newRegion);
}

void Program::onException()
{
  try { std::rethrow_exception(std::current_exception()); }
  catch (const std::exception& exception)
  {
    ::AllocConsole();

    ::SetConsoleTitleA("ReMOSD");

    std::printf("%s\n", exception.what());
    std::printf("\n");

    std::printf("+--------------------------------------------------+\n"
                "| PLEASE CONSIDER SENDING A COPY OF THE TEXT ABOVE |\n"
                "| OR SCREENSHOT WITH THIS WINDOW TO A DEVELOPER.   |\n"
                "|                                                  |\n"
                "| Telegram: @qt-kaneko                             |\n"
                "| GitHub:   github.com/qt-kaneko/remosd            |\n"
                "+--------------------------------------------------+\n");
    std::printf("\n");

    std::printf("Press any key to close this window . . .\n");
    std::getchar();

    std::abort();
  }
}