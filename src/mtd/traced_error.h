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