using System;
using System.Linq.Expressions;
using eQuantic.Core.Linq.Sorter;

namespace eQuantic.Core.Linq.Extensions
{
    /// <summary>
    /// Extensions for entity sorter
    /// </summary>
    public static class EntitySorterExtensions
    {
        public static IEntitySorter<T> OrderBy<T, TKey>(this IEntitySorter<T> sorter, Expression<Func<T, TKey>> keySelector)
        {
            return EntitySorter<T>.OrderBy(keySelector);
        }

        public static IEntitySorter<T> OrderByDescending<T, TKey>(this IEntitySorter<T> sorter, Expression<Func<T, TKey>> keySelector)
        {
            return EntitySorter<T>.OrderByDescending(keySelector);
        }

        public static IEntitySorter<T> ThenBy<T, TKey>(this IEntitySorter<T> sorter, Expression<Func<T, TKey>> keySelector)
        {
            return new ThenBySorter<T, TKey>(sorter, keySelector, SortDirection.Ascending);
        }

        public static IEntitySorter<T> ThenBy<T>(this IEntitySorter<T> sorter, string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName, SortDirection.Ascending);

            return builder.BuildThenByEntitySorter(sorter);
        }

        public static IEntitySorter<T> ThenBy<T>(this IEntitySorter<T> sorter, ISorting sorting)
        {
            var builder = new EntitySorterBuilder<T>(sorting.ColumnName, sorting.SortDirection);

            return builder.BuildThenByEntitySorter(sorter);
        }

        public static IEntitySorter<T> ThenByDescending<T, TKey>(this IEntitySorter<T> sorter, Expression<Func<T, TKey>> keySelector)
        {
            return new ThenBySorter<T, TKey>(sorter, keySelector, SortDirection.Descending);
        }

        public static IEntitySorter<T> ThenByDescending<T>(this IEntitySorter<T> sorter, string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName, SortDirection.Descending);

            return builder.BuildThenByEntitySorter(sorter);
        }
    }
}
