using System;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq
{
    internal sealed class ThenBySorter<T, TKey> : IEntitySorter<T>
    {
        private readonly IEntitySorter<T> baseSorter;
        private readonly Expression<Func<T, TKey>> keySelector;
        private readonly SortDirection direction;

        public ThenBySorter(IEntitySorter<T> baseSorter,
            Expression<Func<T, TKey>> selector, SortDirection direction)
        {
            this.baseSorter = baseSorter;
            this.keySelector = selector;
            this.direction = direction;
        }

        public IOrderedQueryable<T> Sort(IQueryable<T> col)
        {
            var sorted = this.baseSorter.Sort(col);

            if (this.direction == SortDirection.Ascending)
            {
                return Queryable.ThenBy(sorted, this.keySelector);
            }
            else
            {
                return Queryable.ThenByDescending(sorted,
                    this.keySelector);
            }
        }
    }
}