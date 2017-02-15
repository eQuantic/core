using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using eQuantic.Core.Data.Repository;
using eQuantic.Core.Extensions;
using eQuantic.Core.Linq;
using eQuantic.Core.Linq.Extensions;
using eQuantic.Core.Linq.Helpers;
using eQuantic.Core.Linq.Specification;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace eQuantic.Core.Data.EntityFramework.Repository
{
    /// <summary>
    /// Repository base class
    /// </summary>
    /// <typeparam name="TEntity">The type of underlying entity in this repository</typeparam>
    /// <typeparam name="TKey"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity
    {

        #region Members

        readonly IQueryableUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == (IUnitOfWork)null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        public IUnitOfWork UnitOfWork => _unitOfWork;

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        public virtual void Add(TEntity item)
        {

            if (item != (TEntity)null)
                GetSet().Add(item); // add new item in this set
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_NaoPodeAdicionarEntidadeNula, typeof(TEntity).ToString());

            }

        }
        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        public virtual void Remove(TEntity item)
        {
            if (item != (TEntity)null)
            {
                //attach item if not exist
                _unitOfWork.Attach(item);

                //set as "removed"
                GetSet().Remove(item);
            }
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_NaoPodeRemoverEntidadeNula, typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        public virtual void TrackItem(TEntity item)
        {
            if (item != (TEntity)null)
                _unitOfWork.Attach<TEntity>(item);
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_NaoPodeRemoverEntidadeNula, typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        public virtual void Modify(TEntity item)
        {
            if (item != (TEntity)null)
                _unitOfWork.SetModified(item);
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_NaoPodeRemoverEntidadeNula, typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="id"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="force"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></returns>
        public virtual TEntity Get(TKey id, bool force = false)
        {
            var item = Get(id, new string[0]);

            if (force) _unitOfWork.Reload(item);

            return item;
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="id"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public TEntity Get(TKey id, params string[] loadProperties)
        {
            if (id != null)
            {
                var item = GetSet().Find(id);
                if (loadProperties != null && loadProperties.Length > 0)
                {
                    foreach (var property in loadProperties)
                    {
                        if (!string.IsNullOrEmpty(property))
                        {
#if NETSTANDARD1_3
                            var prop = item.GetType().GetTypeInfo().GetDeclaredProperty(property);
#else
                            var prop = item.GetType().GetProperty(property);
#endif
                            prop?.GetValue(item);
                        }
                    }
                }
                return item;
            }
            else
                return null;
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public TEntity Get(TKey id, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return Get(id, GetPropertyNames(loadProperties));
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            return GetQueryable(loadProperties).SingleOrDefault(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetQueryable(loadProperties).SingleOrDefault(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            return GetQueryable(loadProperties).FirstOrDefault(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetQueryable(loadProperties).FirstOrDefault(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></returns>
        public IEnumerable<TEntity> GetAll(params string[] loadProperties)
        {
            return GetQueryable(loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetQueryable(loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="sortingColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(ISorting[] sortingColumns, params string[] loadProperties)
        {
            var query = GetQueryable(loadProperties);

            if (sortingColumns != null && sortingColumns.Length > 0)
            {
                query = query.OrderBy(sortingColumns);
            }
            return query;
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="sortingColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(ISorting[] sortingColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetAll(sortingColumns, GetPropertyNames(loadProperties));
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></returns>
        public IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification, params string[] loadProperties)
        {
            if(specification == null)
                throw new ArgumentException("Specification cannot be null", nameof(specification));

            return GetQueryable(loadProperties).Where(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            if(specification == null)
                throw new ArgumentException("Specification cannot be null", nameof(specification));

            return GetQueryable(loadProperties).Where(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return GetSet().Count();
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public int Count(ISpecification<TEntity> specification)
        {
            return GetSet().Count(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Count(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <returns></returns>
        public long LongCount()
        {
            return GetSet().LongCount();
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public long LongCount(ISpecification<TEntity> specification)
        {
            return GetSet().LongCount(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public long LongCount(Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().LongCount(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            return GetPaged((ISpecification<TEntity>)null, 1, limit, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetPaged((ISpecification<TEntity>)null, 1, limit, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="limit"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            return GetPaged(specification, 1, limit, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="limit"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns,
            params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetPaged(specification, 1, limit, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="limit"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            return GetPaged(filter, 1, limit, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="limit"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetPaged(filter, 1, limit, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISorting[] sortColumns, params string[] loadProperties)
        {
            return GetPaged((ISpecification<TEntity>)null, pageIndex, pageCount, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetPaged((ISpecification<TEntity>)null, pageIndex, pageCount, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="pageIndex"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="pageCount"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="sortColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns,
            params string[] loadProperties)
        {
            return GetPaged(specification?.SatisfiedBy(), pageIndex, pageCount, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns,
            params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetPaged(specification?.SatisfiedBy(), pageIndex, pageCount, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="pageIndex"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="pageCount"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="sortColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns,
            params string[] loadProperties)
        {
            var query = filter != null ? GetQueryable(loadProperties).Where(filter) : GetQueryable(loadProperties);

            if (sortColumns != null && sortColumns.Length > 0)
            {
                query = query.OrderBy(sortColumns);
            }
            if (pageCount > 0)
                return query.Skip((pageIndex - 1) * pageCount).Take(pageCount);

            return query;
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns,
            params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetPaged(filter, pageIndex, pageCount, sortColumns, GetPropertyNames(loadProperties));
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></returns>
        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            if(filter == null)
                throw new ArgumentException("Filter expression cannot be null", nameof(filter));

            return GetQueryable(loadProperties).Where(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            if(filter == null)
                throw new ArgumentException("Filter expression cannot be null", nameof(filter));

            return GetQueryable(loadProperties).Where(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="sortColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns, params string[] loadProperties)
        {
            if(filter == null)
                throw new ArgumentException("Filter expression cannot be null", nameof(filter));

            var query = GetQueryable(loadProperties).Where(filter);
            if (sortColumns != null && sortColumns.Length > 0)
            {
                query = query.OrderBy(sortColumns);
            }
            return query;
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetFiltered(filter, sortColumns, GetPropertyNames(loadProperties));
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public int DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter).Delete();
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public int DeleteMany(ISpecification<TEntity> specification)
        {
            return DeleteMany(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="updateExpression"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public int UpdateMany(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            if(updateExpression == null)
                throw new ArgumentException("Expression cannot be null", nameof(updateExpression));

            return GetSet().Where(filter).Update(updateExpression);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="updateExpression"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <returns></returns>
        public int UpdateMany(ISpecification<TEntity> specification, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return UpdateMany(specification.SatisfiedBy(), updateExpression);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/>
        /// </summary>
        /// <param name="persisted"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        /// <param name="current"><see cref="eQuantic.Core.Data.Repository.IRepository{TEntity, TKey}"/></param>
        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _unitOfWork.ApplyCurrentValues(persisted, current);
        }

        
#endregion

#region IDisposable Members

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

#endregion

#region Private Methods

        private DbSet<TEntity> _dbset = null;
        
        protected DbSet<TEntity> GetSet()
        {
            return _dbset ?? (_dbset = (DbSet<TEntity>)_unitOfWork.CreateSet<TEntity>());
        }

        protected IQueryable<TEntity> GetQueryable(params string[] loadProperties)
        {
            IQueryable<TEntity> query = GetSet();
            if (loadProperties != null && loadProperties.Length > 0)
                query = loadProperties.Where(property => !string.IsNullOrEmpty(property))
                    .Aggregate(query, (current, property) => current.Include(property));

            return query;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        protected string[] GetPropertyNames(Expression<Func<TEntity, object>>[] loadProperties)
        {
            var columnNames = new List<string>();
            foreach (var loadProperty in loadProperties)
            {
                MemberExpression member;
                if (loadProperty.Body is MemberExpression)
                {
                    member = (MemberExpression)loadProperty.Body;
                }
                else
                {
                    var op = ((UnaryExpression)loadProperty.Body).Operand;
                    member = (MemberExpression)op;
                }
                columnNames.Add(PropertiesHelper.BuildColumnNameFromMemberExpression(member));
            }
            return columnNames.ToArray();
        }

        protected IQueryable<TEntity> GetQueryable(params Expression<Func<TEntity, object>>[] loadProperties)
        {

            return GetQueryable(GetPropertyNames(loadProperties));
        }

        #endregion
    }
}
