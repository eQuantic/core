using System;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Filter
{
    /// <summary>
    /// Specifies a method that filters a collection by returning a filtered collection.
    /// </summary>
    /// <typeparam name="TEntity">The element type of the collection to filter.</typeparam>
    public interface IEntityFilter<TEntity>
    {
        /// <summary>Filters the specified collection.</summary>
        /// <param name="collection">The collection.</param>
        /// <returns>A filtered collection.</returns>
        IQueryable<TEntity> Filter(IQueryable<TEntity> collection);

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> GetExpression();
    }
}