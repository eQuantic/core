using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    public sealed class DirectSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        private readonly Expression<Func<TEntity, bool>> matchingCriteria;

        public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            this.matchingCriteria = matchingCriteria ?? throw new ArgumentNullException(nameof(matchingCriteria), "No criteria were informed.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return matchingCriteria;
        }
    }
}