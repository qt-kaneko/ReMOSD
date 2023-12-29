#pragma once

#include <vector>
#include <string_view>
#include <exception>

class Program
{
  public:
    explicit Program(const std::vector<std::string_view>& args);

  private:
    [[noreturn]]
    static void onException();

    static const char* mapErrorType(const std::exception_ptr& errorPtr);
};