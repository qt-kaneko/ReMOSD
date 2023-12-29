#include "Program.h"

#include <cmath>
#include <cstdio>
#include <windows.h>

#include "mtd/contains_any_of.h"
#include "mtd/trace.h"
#include "MediaOsd.h"

using namespace std::literals;

static constexpr int _miniOsdWidth = 65;
static constexpr int _miniOsdHeight = 140;

Program::Program(const std::vector<std::string_view>& args)
{
  std::set_terminate(onException);

  auto osd = mtd::trace(
    MediaOsd::find()
  );

  HRGN newRegion;
  if (mtd::contains_any_of(args.begin(), args.end(), "--restore"sv, "-r"sv))
  {
    newRegion = HRGN();
  }
  else
  {
    auto osdDpi = mtd::trace(
      osd.getDpi()
    );

    auto osdScalingCoefficient = static_cast<float>(osdDpi) / 96.0f;

    auto newRegionWidth = static_cast<int>(std::round(_miniOsdWidth * osdScalingCoefficient));
    auto newRegionHeight = static_cast<int>(std::round(_miniOsdHeight * osdScalingCoefficient));

    newRegion = ::CreateRectRgn(0, 0, newRegionWidth, newRegionHeight);
    if (newRegion == nullptr) throw mtd::make_traced(std::system_error(::GetLastError(), std::system_category()));
  }

  mtd::trace(
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
    auto errorPtr = std::make_exception_ptr(tracedError);
    while (true)
    {
      try { std::rethrow_exception(errorPtr); }
      catch (const mtd::traced_error& innerTracedError)
      {
        message.insert(0, "'")
               .insert(0, innerTracedError.location.function_name())
               .insert(0, "'")
               .insert(0, " ")
               .insert(0, std::to_string(innerTracedError.location.line()))
               .insert(0, ":")
               .insert(0, innerTracedError.location.file_name())
               .insert(0, "\n  at ");

        errorPtr = innerTracedError.error;
      }
      catch (std::exception&) { break; }
    }

    try { std::rethrow_exception(errorPtr); }
    catch(const std::exception& error)
    {
      message.insert(0, "\"");
      message.insert(0, error.what());
      message.insert(0, "\"");
    }

    message.insert(0, " ");
    message.insert(0, mapErrorType(errorPtr));

    message.insert(0, "Unhandled exception: ");
  }
  catch (const std::exception& error)
  {
    message += "Unhandled exception: std::exception \"";
    message += error.what();
    message += "\"";
  }
  catch (...) { message += "Not an exception: why it even exists? o_O"; }

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

const char* Program::mapErrorType(const std::exception_ptr& errorPtr)
{
  try { std::rethrow_exception(errorPtr); }
  catch (std::invalid_argument&)     { return "std::invalid_argument"; }
  catch (std::domain_error&)         { return "std::domain_error"; }
  catch (std::length_error&)         { return "std::length_error"; }
  catch (std::out_of_range&)         { return "std::out_of_range"; }
  catch (std::logic_error&)          { return "std::logic_error"; }
  catch (std::range_error&)          { return "std::range_error"; }
  catch (std::overflow_error&)       { return "std::overflow_error"; }
  catch (std::underflow_error&)      { return "std::underflow_error"; }
  catch (std::system_error&)         { return "std::system_error"; }
  catch (std::runtime_error&)        { return "std::runtime_error"; }
  catch (std::bad_typeid&)           { return "std::bad_typeid"; }
  catch (std::bad_cast&)             { return "std::bad_cast"; }
  catch (std::bad_array_new_length&) { return "std::bad_array_new_length"; }
  catch (std::bad_alloc&)            { return "std::bad_alloc"; }
  catch (std::bad_exception&)        { return "std::bad_exception"; }
  catch (std::exception&)            { return "std::exception"; }
  catch (...)                        { return "unknown"; }
}