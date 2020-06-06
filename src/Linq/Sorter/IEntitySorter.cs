using System.Linq;

namespace eQuantic.Core.Linq.Sorter
{
    public interface IEntitySorter<TEntity>
    {
        IOrderedQueryable<TEntity> Sort(IQueryable<TEntity> collection);
    }
}