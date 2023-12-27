#include "contains_any_of.h"

template<std::input_iterator TIterator,
         typename TValue>
bool utils::contains_any_of(TIterator begin, TIterator end,
                            const std::same_as<TValue> auto&... items)
{
  return std::any_of(begin, end, [&](const TValue& item) {
    return ((item == items) || ...);
  });
}