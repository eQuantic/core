using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    /// <summary>
    /// A Logic OR Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class OrSpecification<T>
         : CompositeSpecification<T>
         where T : class
    {
        #region Members

        private ISpecification<T> rightSideSpecification = null;
        private ISpecification<T> leftSideSpecification = null;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Default constructor for AndSpecification
        /// </summary>
        /// <param name="left">Left side specification</param>
        /// <param name="right">Right side specification</param>
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            if (left == (ISpecification<T>)null)
                throw new ArgumentNullException("leftSide");

            if (right == (ISpecification<T>)null)
                throw new ArgumentNullException("rightSide");

            this.leftSideSpecification = left;
            this.rightSideSpecification = right;
        }

        #endregion

        #region Composite Specification overrides

        /// <summary>
        /// Left side specification
        /// </summary>
        public override ISpecification<T> LeftSpecification
        {
            get { return leftSideSpecification; }
        }

        /// <summary>
        /// Righ side specification
        /// </summary>
        public override ISpecification<T> RightSpecification
        {
            get { return rightSideSpecification; }
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.Specification.ISpecification{T}"/>
        /// </summary>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.Specification.ISpecification{T}"/></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = leftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = rightSideSpecification.SatisfiedBy();

            return (left.Or(right));

        }

        #endregion
    }
}
