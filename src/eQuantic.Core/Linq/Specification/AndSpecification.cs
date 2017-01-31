using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    public class AndSpecification<T> : CompositeSpecification<T> where T:class
    {

        private readonly ISpecification<T> _leftSpecification = null;
        private readonly ISpecification<T> _rightSpecification = null;
        public override ISpecification<T> LeftSpecification => _leftSpecification;
        public override ISpecification<T> RightSpecification => _rightSpecification;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            if (left == (ISpecification<T>)null)
                throw new ArgumentNullException(nameof(left));

            if (right == (ISpecification<T>)null)
                throw new ArgumentNullException(nameof(right));

            this._leftSpecification = left;
            this._rightSpecification = right;
        }

        public AndSpecification(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            _leftSpecification = new DirectSpecification<T>(left);
            _rightSpecification = new DirectSpecification<T>(right);
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = _leftSpecification.SatisfiedBy();
            var right = _rightSpecification.SatisfiedBy();
            
            return left.And(right);
        }
    }
}
