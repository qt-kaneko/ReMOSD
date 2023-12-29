#pragma once

#include <exception>
#include <source_location>

namespace mtd
{
  class traced_error : public std::exception
  {
    public:
      const std::exception_ptr error;
      const std::source_location location;

      explicit traced_error(const std::exception_ptr& error,
                            const std::source_location& location);
  };
}

#define make_traced(error) traced_error(std::make_exception_ptr(error), \
                                        std::source_location::current())

#define trace(expression)                                          \
  [&](const std::source_location& location) {                      \
    try { return expression; }                                     \
    catch (std::exception&)                                        \
    {                                                              \
      throw mtd::traced_error(std::current_exception(), location); \
    }                                                              \
  }(std::source_location::current())
