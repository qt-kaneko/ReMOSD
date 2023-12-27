#pragma once

#include <exception>

namespace utils
{
  std::exception retrieve_exception(std::exception_ptr exception);
}