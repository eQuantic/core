using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    public class AndAlsoSpecification<T> : DirectCompositeSpecification<T> where T : class
    {
        public AndAlsoSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right)
        {
        }

        public AndAlsoSpecification(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) : base(left, right)
        {
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = LeftSpecification.SatisfiedBy();
            var right = RightSpecification.SatisfiedBy();

            return left.AndAlso(right);
        }
    }
}