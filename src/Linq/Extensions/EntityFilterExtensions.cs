using eQuantic.Core.Linq.Filter;

namespace eQuantic.Core.Linq.Extensions
{
    public static class EntityFilterExtensions
    {
        public static IEntityFilter<T> Filter<T>(this IEntityFilter<T> filter, IFiltering filtering)
        {
            var builder = new EntityFilterBuilder<T>(filtering.ColumnName, filtering.StringValue, filtering.Operator);

            return builder.BuildWhereEntityFilter(filter);
        }
    }
}