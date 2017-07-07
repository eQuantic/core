using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using eQuantic.Core.Data.Repository;
using eQuantic.Core.Linq;
using eQuantic.Core.Linq.Specification;

namespace eQuantic.Core.Data.MongoDb.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWork UnitOfWork { get; }
        public void Add(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void Modify(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void TrackItem(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void Merge(TEntity persisted, TEntity current)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey id, bool force)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey id)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey id, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(TKey id, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll(params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll(ISorting[] sortingColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll(ISorting[] sortingColumns, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll(ISorting[] sortingColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public long LongCount()
        {
            throw new NotImplementedException();
        }

        public long LongCount(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        public long LongCount(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(int limit, ISorting[] sortColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISorting[] sortColumns, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns,
            params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns,
            params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns,
            params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns,
            params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns, params string[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            throw new NotImplementedException();
        }

        public int DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public int DeleteMany(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        public int UpdateMany(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            throw new NotImplementedException();
        }

        public int UpdateMany(ISpecification<TEntity> specification, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            throw new NotImplementedException();
        }
    }
}
