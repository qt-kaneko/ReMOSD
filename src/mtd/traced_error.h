#pragma once

#include <exception>
#include <source_location>
#include <memory>
#include <string>

namespace mtd
{
  class traced_error : public std::exception
  {
    public:
      const std::shared_ptr<std::exception> error;
      const std::source_location location;

      explicit traced_error(const std::shared_ptr<std::exception> error,
                            const std::source_location& location);
  };
}

#define make_traced(error) traced_error(std::shared_ptr<std::exception>(error), \
                                        std::source_location::current())

#define trace(expression)                                         \
  [&]() {                                                         \
    try { return expression; }                                    \
    catch (mtd::traced_error& tracedError)                        \
    {                                                             \
      throw mtd::make_traced(new mtd::traced_error(tracedError)); \
    }                                                             \
  }()
