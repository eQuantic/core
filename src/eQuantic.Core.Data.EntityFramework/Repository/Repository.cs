using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using eQuantic.Core.Data.Repository;
using eQuantic.Core.Data.Repository.Sql;
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
    /// <typeparam name="TKey"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></typeparam>
    public class Repository<TUnitOfWork, TEntity, TKey> : IRepository<TUnitOfWork, TEntity, TKey>
        where TUnitOfWork : IQueryableUnitOfWork
        where TEntity : class, IEntity, new()
    {

        #region Members

        readonly TUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public Repository(TUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        public TUnitOfWork UnitOfWork => _unitOfWork;

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="item"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="id"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="force"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></returns>
        public virtual TEntity Get(TKey id, bool force)
        {
            return Get(id, force, new string[0]);
        }

        public TEntity Get(TKey id)
        {
            return Get(id, false, new string[0]);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="id"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public TEntity Get(TKey id, params string[] loadProperties)
        {
            return Get(id, false, loadProperties);
        }

        public TEntity Get(TKey id, bool force, params string[] loadProperties)
        {
            if (id != null)
            {
                var item = GetSet().Find(id);
                if (item != null)
                {
                    if (loadProperties != null && loadProperties.Length > 0)
                    {
                        foreach (var property in loadProperties)
                        {
                            if (!string.IsNullOrEmpty(property))
                            {
                                var props = property.Split('.');

                                if (props.Length == 1)
                                {
                                    _unitOfWork.LoadProperty(item, property);
                                }
                                else
                                {
                                    LoadCascade(props, item);
                                }
                            }
                        }
                    }
                    if (force) _unitOfWork.Reload(item);
                }
                return item;
            }
            else
                return null;
        }

        protected void LoadCascade(string[]props, object obj, int index = 0)
        {
            if (obj == null) return;

#if NETSTANDARD1_6 || NETSTANDARD2_0
            var prop = obj.GetType().GetTypeInfo().GetDeclaredProperty(props[index]);
#else
            var prop = obj.GetType().GetProperty(props[index]);
#endif
            var nextObj = prop?.GetValue(obj);
            if (nextObj == null)
            {
                _unitOfWork.LoadProperty(obj, props[index]);
                nextObj = prop?.GetValue(obj);
            }

            if(props.Length > index+1) LoadCascade(props, nextObj, index+1);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public TEntity Get(TKey id, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return Get(id, false, GetPropertyNames(loadProperties));
        }

        public TEntity Get(TKey id, bool force, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return Get(id, force, GetPropertyNames(loadProperties));
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            return GetSingle(filter, new Expression<Func<TEntity, object>>[0]);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            return GetQueryable(loadProperties).SingleOrDefault(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetQueryable(loadProperties).SingleOrDefault(filter);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter)
        {
            return GetFirst(filter, new Expression<Func<TEntity, object>>[0]);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            return GetQueryable(loadProperties).FirstOrDefault(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetQueryable(loadProperties).FirstOrDefault(filter);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, ISorting[] sortingColumns, params string[] loadProperties)
        {
            return GetQueryable(loadProperties).OrderBy(sortingColumns).FirstOrDefault(filter);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return GetQueryable(new Expression<Func<TEntity, object>>[0]);
        }

        public IEnumerable<TEntity> GetAll(ISorting[] sortingColumns)
        {
            return GetAll(sortingColumns, new Expression<Func<TEntity, object>>[0]);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></returns>
        public IEnumerable<TEntity> GetAll(params string[] loadProperties)
        {
            return GetQueryable(loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetQueryable(loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="sortingColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="sortingColumns"></param>
        /// <param name="loadProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(ISorting[] sortingColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return GetAll(sortingColumns, GetPropertyNames(loadProperties));
        }

        public IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification)
        {
            return AllMatching(specification, new Expression<Func<TEntity, object>>[0]);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></returns>
        public IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification, params string[] loadProperties)
        {
            if(specification == null)
                throw new ArgumentException("Specification cannot be null", nameof(specification));

            return GetQueryable(loadProperties).Where(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return GetSet().LongCount();
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public long Count(ISpecification<TEntity> specification)
        {
            return GetSet().LongCount(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().LongCount(filter);
        }

        public IEnumerable<TEntity> GetPaged(int limit, ISorting[] sortColumns)
        {
            return GetPaged(limit, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns)
        {
            return GetPaged(specification, limit, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns)
        {
            return GetPaged(filter, limit, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            return GetPaged(pageIndex, pageCount, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            return GetPaged(specification, pageIndex, pageCount, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            return GetPaged(filter, pageIndex, pageCount, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="pageIndex"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="pageCount"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="sortColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns,
            params string[] loadProperties)
        {
            return GetPaged(specification?.SatisfiedBy(), pageIndex, pageCount, sortColumns, loadProperties);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="pageIndex"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="pageCount"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="sortColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return GetFiltered(filter, new Expression<Func<TEntity, object>>[0]);
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns)
        {
            return GetFiltered(filter, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></returns>
        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            if(filter == null)
                throw new ArgumentException("Filter expression cannot be null", nameof(filter));

            return GetQueryable(loadProperties).Where(filter);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="sortColumns"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="loadProperties"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
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
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public int DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter).Delete();
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public int DeleteMany(ISpecification<TEntity> specification)
        {
            return DeleteMany(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="updateFactory"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public int UpdateMany(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> updateFactory)
        {
            return GetSet().Where(filter).Update(updateFactory);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="updateFactory"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public int UpdateMany(ISpecification<TEntity> specification, Expression<Func<TEntity, TEntity>> updateFactory)
        {
            return UpdateMany(specification.SatisfiedBy(), updateFactory);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="persisted"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="current"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
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
