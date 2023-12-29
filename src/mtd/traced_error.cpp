#include "traced_error.h"

namespace mtd
{
  traced_error::traced_error(std::shared_ptr<std::exception> error,
                             const std::source_location& location)
    : error(error)
    , location(location)
  {
  }
}