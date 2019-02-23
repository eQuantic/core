using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eQuantic.Core.Linq
{
    public static class EntitySorter<T>
    {
        public static IEntitySorter<T> AsQueryable()
        {
            return new EmptyEntitySorter();
        }

        public static IEntitySorter<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return new OrderBySorter<T, TKey>(keySelector,
                SortDirection.Ascending);
        }

        public static IEntitySorter<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return new OrderBySorter<T, TKey>(keySelector,
                SortDirection.Descending);
        }

        public static IEntitySorter<T> OrderBy(string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName);

            builder.Direction = SortDirection.Ascending;

            return builder.BuildOrderByEntitySorter();
        }

        public static IEntitySorter<T> OrderByDescending(string propertyName)
        {
            var builder = new EntitySorterBuilder<T>(propertyName);

            builder.Direction = SortDirection.Descending;

            return builder.BuildOrderByEntitySorter();
        }

        private sealed class EmptyEntitySorter : IEntitySorter<T>
        {
            public IOrderedQueryable<T> Sort(
                IQueryable<T> collection)
            {
                string exceptionMessage = "OrderBy should be called.";

                throw new InvalidOperationException(exceptionMessage);
            }
        }
    }
}
