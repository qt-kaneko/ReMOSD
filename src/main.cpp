#include <string_view>
#include <vector>

#include "Program.h"

int main(int argc, char* argv[])
{
  Program(std::vector<std::string_view>(argv, argv + argc));
}