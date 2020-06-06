using System;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Sorter
{
    internal class OrderBySorter<T, TKey> : IEntitySorter<T>
    {
        private readonly SortDirection direction;
        private readonly Expression<Func<T, TKey>> keySelector;

        public OrderBySorter(Expression<Func<T, TKey>> selector,
            SortDirection direction)
        {
            this.keySelector = selector;
            this.direction = direction;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OrderBySorter<T, TKey> sorter))
            {
                return false;
            }

            return this.keySelector.ToString() == sorter.keySelector.ToString() && this.direction == sorter.direction;
        }

        public override int GetHashCode()
        {
            return (keySelector, direction).GetHashCode();
        }

        public IOrderedQueryable<T> Sort(IQueryable<T> collection)
        {
            if (this.direction == SortDirection.Ascending)
            {
                return collection.OrderBy(this.keySelector);
            }
            else
            {
                return collection.OrderByDescending(this.keySelector);
            }
        }
    }
}