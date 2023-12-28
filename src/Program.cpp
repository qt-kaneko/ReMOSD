#include "Program.h"

#include <cmath>
#include <cstdio>
#include <exception>
#include <windows.h>

#include "utils/contains_any_of.h"
#include "utils/traced_error.h"
#include "MediaOsd.h"

using namespace std::literals;

static constexpr int _miniOsdWidth = 65;
static constexpr int _miniOsdHeight = 140;

Program::Program(const std::vector<std::string_view>& args)
{
  std::set_terminate(onException);

  MediaOsd osd;
  try { osd = MediaOsd::find(); }
  catch (const utils::traced_error& e) { throw utils::TRACED_ERROR(new utils::traced_error(e)); }

  HRGN newRegion;
  if (utils::contains_any_of(args.begin(), args.end(), "--restore"sv, "-r"sv))
  {
    newRegion = HRGN();
  }
  else
  {
    unsigned int osdDpi;
    try { osdDpi = osd.getDpi(); }
    catch (const utils::traced_error& e) { throw utils::TRACED_ERROR(new utils::traced_error(e)); }

    auto osdScalingCoefficient = static_cast<float>(osdDpi) / 96.0f;

    auto newRegionWidth = static_cast<int>(::round(_miniOsdWidth * osdScalingCoefficient));
    auto newRegionHeight = static_cast<int>(::round(_miniOsdHeight * osdScalingCoefficient));

    newRegion = ::CreateRectRgn(0, 0, newRegionWidth, newRegionHeight);
    if (newRegion == nullptr) throw utils::TRACED_ERROR(new std::system_error(::GetLastError(), std::system_category()));
  }

  try { osd.setRegion(newRegion); }
  catch (const utils::traced_error& e) { throw utils::TRACED_ERROR(new utils::traced_error(e)); }
}

void Program::onException()
{
  try { std::rethrow_exception(std::current_exception()); }
  catch (const std::exception& e)
  {
    ::AllocConsole();

    ::SetConsoleTitleA("ReMOSD");

    std::printf("%s\n", e.what());
    std::printf("\n");

    std::printf("+--------------------------------------------------+\n"
                "| PLEASE CONSIDER SENDING A COPY OF THE TEXT ABOVE |\n"
                "| OR SCREENSHOT WITH THIS WINDOW TO A DEVELOPER.   |\n"
                "|                                                  |\n"
                "| Telegram: @qt-kaneko                             |\n"
                "| GitHub:   github.com/qt-kaneko/remosd            |\n"
                "+--------------------------------------------------+\n");
    std::printf("\n");

    std::printf("Press Enter to close this window . . .\n");
    std::getchar();

    std::abort();
  }
}