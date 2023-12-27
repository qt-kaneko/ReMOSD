#include "retrieve_exception.h"

std::exception utils::retrieve_exception(std::exception_ptr exceptionPtr)
{
  try
  {
    std::rethrow_exception(exceptionPtr);
  }
  catch(const std::exception& e)
  {
    return e;
  }
}