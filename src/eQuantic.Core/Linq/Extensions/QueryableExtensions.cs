using System.Linq;

namespace eQuantic.Core.Linq.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, params ISorting[] sortings)
        {
            IEntitySorter<T> sorter = null;
            if (sortings != null)
            {
                foreach (var sorting in sortings)
                {
                    if (sorter == null)
                        sorter = sorting.Ascending
                            ? EntitySorter<T>.OrderBy(sorting.ColumnName)
                            : EntitySorter<T>.OrderByDescending(sorting.ColumnName);
                    else
                        sorter = sorting.Ascending
                            ? sorter.ThenBy(sorting.ColumnName)
                            : sorter.ThenByDescending(sorting.ColumnName);
                }
                return sorter != null ? sorter.Sort(source) : source;
            }
            return source;
        }
    }
}