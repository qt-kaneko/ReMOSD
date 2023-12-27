#include "retrieve_exception.h"

std::exception utils::retrieve_exception(std::exception_ptr exception)
{
  try
  {
    std::rethrow_exception(exception);
  }
  catch(const std::exception& e)
  {
    return e;
  }
}