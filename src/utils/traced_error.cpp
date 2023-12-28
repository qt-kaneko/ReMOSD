#include "traced_error.h"

namespace utils
{
  traced_error::traced_error(std::shared_ptr<std::exception> error,
                             const std::source_location& location)
    : error(error)
    , location(location)
  {
    _message = std::string();
    _message += error->what();
    _message += "\n  at ";
    _message += location.file_name();
    _message += ":";
    _message += std::to_string(location.line());
    _message += " ";
    _message += "'";
    _message += location.function_name();
    _message += "'";
  }

  const char* traced_error::what() const noexcept
  {
    return _message.c_str();
  }
}