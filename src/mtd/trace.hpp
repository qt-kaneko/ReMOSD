#include "trace.h"

mtd::traced_error mtd::__make_traced(const auto& error, const std::source_location& location)
{
  return mtd::traced_error(std::make_exception_ptr(error), location);
}

auto mtd::__trace(const auto& expression, const std::source_location& location)
{
  try { return expression(); }
  catch (std::exception&)
  {
    throw mtd::traced_error(std::current_exception(), location);
  }
}