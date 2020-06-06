using System;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Sorter
{
    /// <summary>
    /// The Entity Sorter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EntitySorter<T>
    {
        /// <summary>
        /// Cast entity sorter to queryable.
        /// </summary>
        /// <returns></returns>
        public static IEntitySorter<T> AsQueryable()
        {
            return new EmptyEntitySorter();
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEntitySorter<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return new OrderBySorter<T, TKey>(keySelector, SortDirection.Ascending);
        }

        /// <summary>
        /// Order by property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static IEntitySorter<T> OrderBy(string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName, SortDirection.Ascending);
            return builder.BuildOrderByEntitySorter();
        }

        /// <summary>
        /// Order by sortings.
        /// </summary>
        /// <param name="sortings">The sortings.</param>
        /// <returns></returns>
        public static IEntitySorter<T> OrderBy(params ISorting[] sortings)
        {
            IEntitySorter<T> entitySorter = null;
            foreach (var sorting in sortings)
            {
                var builder = new EntitySorterBuilder<T>(sorting.ColumnName, sorting.SortDirection);
                entitySorter = entitySorter == null ? builder.BuildOrderByEntitySorter() : builder.BuildThenByEntitySorter(entitySorter);
            }
            return entitySorter;
        }

        /// <summary>
        /// Orders the by descending.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEntitySorter<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return new OrderBySorter<T, TKey>(keySelector, SortDirection.Descending);
        }

        public static IEntitySorter<T> OrderByDescending(string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName, SortDirection.Descending);
            return builder.BuildOrderByEntitySorter();
        }

        private sealed class EmptyEntitySorter : IEntitySorter<T>
        {
            /// <exception cref="InvalidOperationException">OrderBy should be called.</exception>
            [Obsolete("User OrderBy instead")]
            public IOrderedQueryable<T> Sort(IQueryable<T> collection)
            {
                throw new InvalidOperationException("OrderBy should be called.");
            }
        }
    }
}