#include "Program.h"

#include <cmath>
#include <cstdio>
#include <exception>
#include <windows.h>

#include "mtd/contains_any_of.h"
#include "mtd/traced_error.h"
#include "MediaOsd.h"

using namespace std::literals;

static constexpr int _miniOsdWidth = 65;
static constexpr int _miniOsdHeight = 140;

Program::Program(const std::vector<std::string_view>& args)
{
  std::set_terminate(onException);

  auto osd = trace(MediaOsd::find());

  HRGN newRegion;
  if (mtd::contains_any_of(args.begin(), args.end(), "--restore"sv, "-r"sv))
  {
    newRegion = HRGN();
  }
  else
  {
    auto osdDpi = trace(osd.getDpi());

    auto osdScalingCoefficient = static_cast<float>(osdDpi) / 96.0f;

    auto newRegionWidth = static_cast<int>(::round(_miniOsdWidth * osdScalingCoefficient));
    auto newRegionHeight = static_cast<int>(::round(_miniOsdHeight * osdScalingCoefficient));

    newRegion = ::CreateRectRgn(0, 0, newRegionWidth, newRegionHeight);
    if (newRegion == nullptr) throw mtd::make_traced(new std::system_error(::GetLastError(), std::system_category()));
  }

  trace(
    osd.setRegion(newRegion)
  );
}

void Program::onException()
{
  ::AllocConsole();
  ::SetConsoleTitleA("ReMOSD");

  auto message = std::string();

  try { std::rethrow_exception(std::current_exception()); }
  catch (const mtd::traced_error& tracedError)
  {
    std::shared_ptr<std::exception> error;
    for (auto tracedErrorPtr = &tracedError;
              tracedErrorPtr != nullptr;
              tracedErrorPtr = dynamic_cast<mtd::traced_error*>(tracedErrorPtr->error.get()))
    {
      error = tracedErrorPtr->error;

      message.insert(0, "'")
             .insert(0, tracedErrorPtr->location.function_name())
             .insert(0, "'")
             .insert(0, " ")
             .insert(0, std::to_string(tracedErrorPtr->location.line()))
             .insert(0, ":")
             .insert(0, tracedErrorPtr->location.file_name())
             .insert(0, "\n  at ");
    }

    message.insert(0, "\"");
    message.insert(0, error->what());
    message.insert(0, "\"");

    message.insert(0, " ");
    message.insert(0,
      dynamic_cast<std::invalid_argument*>(error.get())     ? "std::invalid_argument" :
      dynamic_cast<std::domain_error*>(error.get())         ? "std::domain_error" :
      dynamic_cast<std::length_error*>(error.get())         ? "std::length_error" :
      dynamic_cast<std::out_of_range*>(error.get())         ? "std::out_of_range" :
#ifdef future_error
      dynamic_cast<std::future_error*>(error.get())         ? "std::future_error" :
#endif
      dynamic_cast<std::logic_error*>(error.get())          ? "std::logic_error" :
      dynamic_cast<std::range_error*>(error.get())          ? "std::range_error" :
      dynamic_cast<std::overflow_error*>(error.get())       ? "std::overflow_error" :
      dynamic_cast<std::underflow_error*>(error.get())      ? "std::underflow_error" :
#ifdef regex_error
      dynamic_cast<std::regex_error*>(error.get())          ? "std::regex_error" :
#endif
      dynamic_cast<std::system_error*>(error.get())         ? "std::system_error" :
      dynamic_cast<std::runtime_error*>(error.get())        ? "std::runtime_error" :
      dynamic_cast<std::bad_typeid*>(error.get())           ? "std::bad_typeid" :
      dynamic_cast<std::bad_cast*>(error.get())             ? "std::bad_cast" :
      dynamic_cast<std::bad_weak_ptr*>(error.get())         ? "std::bad_weak_ptr" :
      dynamic_cast<std::bad_array_new_length*>(error.get()) ? "std::bad_array_new_length" :
      dynamic_cast<std::bad_alloc*>(error.get())            ? "std::bad_alloc" :
      dynamic_cast<std::bad_exception*>(error.get())        ? "std::bad_exception" :
                                                              "std::exception"
    );

    message.insert(0, "Unhandled exception: ");
  }
  catch (const std::exception& error)
  {
    message += "Unhandled exception: std::exception \"";
    message += error.what();
    message += "\"";
  }
  catch (...) { message += "Not an exception: Why it even exists? o_O"; }

  std::printf("%s\n", message.c_str());
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