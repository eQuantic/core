using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    /// <summary>
    /// A Logic OrOlse Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class OrElseSpecification<T> : DirectCompositeSpecification<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrElseSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">Left side specification</param>
        /// <param name="right">Right side specification</param>
        public OrElseSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrElseSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">Left side expression.</param>
        /// <param name="right">Right side expression.</param>
        public OrElseSpecification(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) : base(left, right)
        {
        }

        /// <summary>
        /// <see cref="ISpecification{T}"/>
        /// </summary>
        /// <returns><see cref="ISpecification{T}"/></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = LeftSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = RightSpecification.SatisfiedBy();

            return left.OrElse(right);
        }
    }
}