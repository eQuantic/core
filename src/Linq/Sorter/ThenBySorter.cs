using System;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Sorter
{
    internal sealed class ThenBySorter<T, TKey> : IEntitySorter<T>
    {
        private readonly IEntitySorter<T> baseSorter;
        private readonly SortDirection direction;
        private readonly Expression<Func<T, TKey>> keySelector;

        public ThenBySorter(IEntitySorter<T> baseSorter,
            Expression<Func<T, TKey>> selector, SortDirection direction)
        {
            this.baseSorter = baseSorter;
            this.keySelector = selector;
            this.direction = direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ThenBySorter<T, TKey> sorter))
            {
                return false;
            }

            return this.baseSorter == sorter.baseSorter &&
                this.keySelector.ToString() == sorter.keySelector.ToString() &&
                this.direction == sorter.direction;
        }

        public override int GetHashCode() => (baseSorter, keySelector, direction).GetHashCode();

        public IOrderedQueryable<T> Sort(IQueryable<T> collection)
        {
            var sorted = this.baseSorter.Sort(collection);

            if (this.direction == SortDirection.Ascending)
            {
                return sorted.ThenBy(this.keySelector);
            }
            else
            {
                return sorted.ThenByDescending(this.keySelector);
            }
        }
    }
}