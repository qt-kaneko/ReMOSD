#pragma once

#include <vector>
#include <string_view>

class Program
{
  public:
    explicit Program(const std::vector<std::string_view>& args);

  private:
    [[noreturn]]
    static void onException();
};