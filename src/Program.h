#pragma once

#include <vector>
#include <string_view>

class Program
{
  public:
    static void main(const std::vector<std::string_view> args);

  private:
    static void onException();
};