#include "Program.h"

#include <span>
#include <string_view>
#include <ranges>
#include <vector>

int main(int argc, char* argv[])
{
  Program::main(std::vector<std::string_view>(argv, argv + argc));
}