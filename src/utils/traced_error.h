#pragma once

#include <exception>
#include <source_location>
#include <memory>
#include <string>

namespace utils
{
  class traced_error : public std::exception
  {
    public:
      const std::shared_ptr<std::exception> error;
      const std::source_location location;

      explicit traced_error(const std::shared_ptr<std::exception> error,
                            const std::source_location& location);

      const char* what() const noexcept override;

    private:
      std::string _message;
  };
}

#define TRACED_ERROR(ERROR) traced_error(std::shared_ptr<std::exception>(ERROR), \
                                         std::source_location::current())
