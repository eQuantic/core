using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    public class TrueSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            Expression<Func<TEntity, bool>> trueExpression = t => true;
            return trueExpression;
        }
    }
}