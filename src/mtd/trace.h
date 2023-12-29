#pragma once

#include "traced_error.h"

namespace mtd
{
  mtd::traced_error __make_traced(const auto& error, const std::source_location& location);

  auto __trace(const auto& expression, const std::source_location& location);

  #define make_traced(error) __make_traced(error, std::source_location::current())

  #define trace(expression) __trace([&]() { return expression; }, std::source_location::current())
}

#include "trace.hpp"