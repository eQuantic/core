using System.Linq;

namespace eQuantic.Core.Linq
{
    public interface IEntitySorter<TEntity>
    {
        IOrderedQueryable<TEntity> Sort(IQueryable<TEntity> collection);
    }
}
