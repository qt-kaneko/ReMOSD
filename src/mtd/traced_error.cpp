#include "traced_error.h"

namespace mtd
{
  traced_error::traced_error(const std::exception_ptr& error,
                             const std::source_location& location)
    : error(error)
    , location(location)
  {
  }
}