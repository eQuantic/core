using System.Linq;
using eQuantic.Core.Linq.Filter;
using eQuantic.Core.Linq.Sorter;

namespace eQuantic.Core.Linq.Extensions
{
    /// <summary>
    /// Queryable Extensions
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Order by criteria using Sorting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="sortings">The sortings.</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, params ISorting[] sortings)
        {
            return EntitySorter<T>.OrderBy(sortings).Sort(source);
        }

        /// <summary>
        /// Query by criteria using Filtering.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="filterings">The filterings.</param>
        /// <returns></returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> source, params IFiltering[] filterings)
        {
            return EntityFilter<T>.Where(filterings).Filter(source);
        }
    }
}