#pragma once

#include <algorithm>
#include <iterator>

namespace mtd
{
  template<std::input_iterator TIterator,
           typename TValue = std::iterator_traits<TIterator>::value_type>
  bool contains_any_of(TIterator begin, TIterator end,
                       const std::same_as<TValue> auto&... items);
}

#include "contains_any_of.hpp"