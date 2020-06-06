namespace eQuantic.Core.Linq.Specification
{
    public abstract class CompositeSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        public abstract ISpecification<TEntity> LeftSpecification { get; }

        public abstract ISpecification<TEntity> RightSpecification { get; }
    }
}