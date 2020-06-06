using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    public abstract class DirectCompositeSpecification<T> : CompositeSpecification<T> where T : class
    {
        protected DirectCompositeSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            LeftSpecification = left ?? throw new ArgumentNullException(nameof(left));
            RightSpecification = right ?? throw new ArgumentNullException(nameof(right));
        }

        protected DirectCompositeSpecification(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            LeftSpecification = left != null ? new DirectSpecification<T>(left) : throw new ArgumentNullException(nameof(left));
            RightSpecification = right != null ? new DirectSpecification<T>(right) : throw new ArgumentNullException(nameof(right));
        }

        public override ISpecification<T> LeftSpecification { get; }
        public override ISpecification<T> RightSpecification { get; }
    }
}