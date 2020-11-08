using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace eQuantic.Core.Linq.Filter
{
    public enum FilterOperator
    {
        Equal = 0,
        NotEqual = 1,
        Contains = 2,
        StartsWith = 3,
        EndsWith = 4,
        GreaterThan = 5,
        GreaterThanOrEqual = 6,
        LessThan = 7,
        LessThanOrEqual = 8
    }

    public static class FilterOperatorValues
    {
        public static ReadOnlyDictionary<FilterOperator, string> Values => new ReadOnlyDictionary<FilterOperator, string>(new Dictionary<FilterOperator, string>
        { { FilterOperator.Equal, "eq" },
            { FilterOperator.NotEqual, "neq" },
            { FilterOperator.Contains, "contains" },
            { FilterOperator.StartsWith, "sw" },
            { FilterOperator.EndsWith, "ew" },
            { FilterOperator.GreaterThan, "gt" },
            { FilterOperator.GreaterThanOrEqual, "gte" },
            { FilterOperator.LessThan, "lt" },
            { FilterOperator.LessThanOrEqual, "lte" }
        });

        public static string GetOperator(FilterOperator filterOperator)
        {
            return Values[filterOperator];
        }

        public static FilterOperator GetOperator(string filterOperator)
        {
            return Values.FirstOrDefault(kv => kv.Value.Equals(filterOperator, StringComparison.InvariantCultureIgnoreCase)).Key;
        }
    }
}