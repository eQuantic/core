using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Specification
{
    public interface ISpecification<T> where T :class
    {
        ISpecification<T> And(ISpecification<T> specification);
        ISpecification<T> Or(ISpecification<T> specification);
        ISpecification<T> Not();
        ISpecification<T> OrNot(ISpecification<T> specification);
        ISpecification<T> AndNot(ISpecification<T> specification);

        Expression<Func<T, bool>> SatisfiedBy();
    }
}
