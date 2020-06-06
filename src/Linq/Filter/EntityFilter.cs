using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Filter
{
    /// <summary>Enables filtering of entities.</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public static class EntityFilter<TEntity>
    {
        /// <summary>
        /// Returns a <see cref="IEntityFilter{TEntity}"/> instance that allows construction of
        /// <see cref="IEntityFilter{TEntity}"/> objects though the use of LINQ syntax.
        /// </summary>
        /// <returns>A <see cref="IEntityFilter{TEntity}"/> instance.</returns>
        public static IEntityFilter<TEntity> AsQueryable()
        {
            return new EmptyEntityFilter();
        }

        /// <summary>
        /// Returns a <see cref="IEntityFilter{TEntity}"/> that filters a sequence based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A new <see cref="IEntityFilter{TEntity}"/>.</returns>
        public static IEntityFilter<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return new WhereEntityFilter<TEntity>(predicate);
        }

        /// <summary>
        /// Returns a <see cref="IEntityFilter{TEntity}"/> that filters a sequence based on a filtering.
        /// </summary>
        /// <param name="filterings">The filterings.</param>
        /// <returns></returns>
        public static IEntityFilter<TEntity> Where(params IFiltering[] filterings)
        {
            IEntityFilter<TEntity> entityFilter = null;
            foreach (var filtering in filterings)
            {
                var builder = new EntityFilterBuilder<TEntity>(filtering.ColumnName, filtering.StringValue, filtering.Operator);
                entityFilter = builder.BuildWhereEntityFilter(entityFilter);
            }
            return entityFilter;
        }

        /// <summary>An empty entity filter.</summary>
        [DebuggerDisplay("EntityFilter ( Unfiltered )")]
        private sealed class EmptyEntityFilter : IEntityFilter<TEntity>
        {
            /// <summary>Filters the specified collection.</summary>
            /// <param name="collection">The collection.</param>
            /// <returns>A filtered collection.</returns>
            public IQueryable<TEntity> Filter(IQueryable<TEntity> collection)
            {
                // We don't filter, but simply return the collection.
                return collection;
            }

            /// <summary>
            /// Gets the expression.
            /// </summary>
            /// <returns></returns>
            public Expression<Func<TEntity, bool>> GetExpression()
            {
                return null;
            }

            /// <summary>Returns an empty string.</summary>
            /// <returns>An empty string.</returns>
            public override string ToString()
            {
                return string.Empty;
            }
        }
    }
}