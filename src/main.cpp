#include "Program.h"

#include <string_view>
#include <vector>

int main(int argc, char* argv[])
{
  Program::main(std::vector<std::string_view>(argv, argv + argc));
}