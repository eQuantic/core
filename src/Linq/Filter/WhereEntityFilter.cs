using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using eQuantic.Core.Linq.Specification;

namespace eQuantic.Core.Linq.Filter
{
    /// <summary>
    /// Filters the collection using a predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [DebuggerDisplay("EntityFilter ( where {ToString()} )")]
    internal sealed class WhereEntityFilter<TEntity> : IEntityFilter<TEntity>
    {
        private readonly IEntityFilter<TEntity> baseFilter;
        private readonly Expression<Func<TEntity, bool>> predicate;

        /// <summary>Initializes a new instance of the <see cref="WhereEntityFilter{TEntity}"/> class.</summary>
        /// <param name="predicate">The predicate.</param>
        public WhereEntityFilter(Expression<Func<TEntity, bool>> predicate)
        {
            this.predicate = predicate;
        }

        /// <summary>Initializes a new instance of the <see cref="WhereEntityFilter{TEntity}"/> class.</summary>
        /// <param name="baseFilter">The base filter.</param>
        /// <param name="predicate">The predicate.</param>
        public WhereEntityFilter(IEntityFilter<TEntity> baseFilter, Expression<Func<TEntity, bool>> predicate)
        {
            this.baseFilter = baseFilter;
            this.predicate = predicate;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WhereEntityFilter<TEntity> filter))
            {
                return false;
            }

            return this.baseFilter == filter.baseFilter && this.predicate.ToString() == filter.predicate.ToString();
        }

        /// <summary>Filters the specified collection.</summary>
        /// <param name="collection">The collection.</param>
        /// <returns>A filtered collection.</returns>
        public IQueryable<TEntity> Filter(IQueryable<TEntity> collection)
        {
            if (this.baseFilter == null)
            {
                return collection.Where(this.predicate);
            }
            else
            {
                return this.baseFilter.Filter(collection).Where(this.predicate);
            }
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> GetExpression()
        {
            if (this.baseFilter == null)
            {
                return this.predicate;
            }
            else
            {
                return ExpressionBuilder.AndAlso(this.baseFilter.GetExpression(), this.predicate);
            }
        }

        public override int GetHashCode()
        {
            return (this.baseFilter, this.predicate).GetHashCode();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string baseFilterPresentation =
                this.baseFilter != null ? this.baseFilter.ToString() : string.Empty;

            // The returned string is used in de DebuggerDisplay.
            if (!string.IsNullOrEmpty(baseFilterPresentation))
            {
                return baseFilterPresentation + ", " + this.predicate.ToString();
            }
            else
            {
                return this.predicate.ToString();
            }
        }
    }
}