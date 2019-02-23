using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eQuantic.Core.Linq
{
    internal class OrderBySorter<T, TKey> : IEntitySorter<T>
    {
        private readonly Expression<Func<T, TKey>> keySelector;
        private readonly SortDirection direction;

        public OrderBySorter(Expression<Func<T, TKey>> selector,
            SortDirection direction)
        {
            this.keySelector = selector;
            this.direction = direction;
        }

        public IOrderedQueryable<T> Sort(IQueryable<T> col)
        {
            if (this.direction == SortDirection.Ascending)
            {
                return Queryable.OrderBy(col, this.keySelector);
            }
            else
            {
                return Queryable.OrderByDescending(col,
                    this.keySelector);
            }
        }
    }
}
