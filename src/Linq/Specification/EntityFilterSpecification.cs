using System;
using System.Linq.Expressions;
using eQuantic.Core.Linq.Filter;

namespace eQuantic.Core.Linq.Specification
{
    public class EntityFilterSpecification<T> : Specification<T> where T : class
    {
        private readonly IFiltering[] filterings;

        public EntityFilterSpecification(IFiltering[] filterings)
        {
            this.filterings = filterings;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return EntityFilter<T>.Where(filterings).GetExpression();
        }
    }
}