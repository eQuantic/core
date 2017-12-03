using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using eQuantic.Core.Data.Repository;
using eQuantic.Core.Data.Repository.Sql;
using eQuantic.Core.Linq;
using eQuantic.Core.Linq.Extensions;
using eQuantic.Core.Linq.Specification;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace eQuantic.Core.Data.EntityFramework.Repository
{
    public class AsyncRepository<TUnitOfWork, TEntity, TKey> : Repository<TUnitOfWork, TEntity, TKey>, IAsyncRepository<TUnitOfWork, TEntity, TKey>
        where TUnitOfWork : IQueryableUnitOfWork
        where TEntity : class, IEntity, new()
    {
        public AsyncRepository(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<TEntity> GetAsync(TKey id, bool force)
        {
            return await GetAsync(id, force, new string[0]);
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await GetAsync(id, false, new string[0]);
        }

        public async Task<TEntity> GetAsync(TKey id, params string[] loadProperties)
        {
            return await GetAsync(id, false, loadProperties);
        }

        public async Task<TEntity> GetAsync(TKey id, bool force, params string[] loadProperties)
        {
            if (id != null)
            {
                var item = await GetSet().FindAsync(id);
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
                                    await UnitOfWork.LoadPropertyAsync(item, property);
                                }
                                else
                                {
                                    await LoadCascadeAsync(props, item);
                                }

                            }
                        }
                    }
                    if (force) UnitOfWork.Reload(item);
                }
                return item;
            }
            else
                return null;
        }

        protected async Task LoadCascadeAsync(string[] props, object obj, int index = 0)
        {
#if NETSTANDARD1_6 || NETSTANDARD2_0
            var prop = obj.GetType().GetTypeInfo().GetDeclaredProperty(props[index]);
#else
            var prop = obj.GetType().GetProperty(props[index]);
#endif
            var nextObj = prop?.GetValue(obj);
            if (nextObj == null)
            {
                await UnitOfWork.LoadPropertyAsync(obj, props[index]);
                nextObj = prop?.GetValue(obj);
            }

            if (props.Length > index + 1) LoadCascade(props, nextObj, index + 1);
        }

        public async Task<TEntity> GetAsync(TKey id, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetAsync(id, false, GetPropertyNames(loadProperties));
        }

        public async Task<TEntity> GetAsync(TKey id, bool force, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetAsync(id, force, GetPropertyNames(loadProperties));
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetSingleAsync(filter, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            return await GetQueryable(loadProperties).SingleOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetQueryable(loadProperties).SingleOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetFirstAsync(filter, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            return await GetQueryable(loadProperties).FirstOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetQueryable(loadProperties).FirstOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter, ISorting[] sortingColumns, params string[] loadProperties)
        {
            return await GetQueryable(loadProperties).OrderBy(sortingColumns).FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await GetQueryable(new Expression<Func<TEntity, object>>[0]).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params string[] loadProperties)
        {
            return await GetQueryable(loadProperties).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetQueryable(loadProperties).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISorting[] sortingColumns)
        {
            return await GetAllAsync(sortingColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISorting[] sortingColumns, params string[] loadProperties)
        {
            var query = GetQueryable(loadProperties);

            if (sortingColumns != null && sortingColumns.Length > 0)
            {
                query = query.OrderBy(sortingColumns);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISorting[] sortingColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetAllAsync(sortingColumns, GetPropertyNames(loadProperties));
        }

        public async Task<IEnumerable<TEntity>> AllMatchingAsync(ISpecification<TEntity> specification)
        {
            return await AllMatchingAsync(specification, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> AllMatchingAsync(ISpecification<TEntity> specification, params string[] loadProperties)
        {
            if (specification == null)
                throw new ArgumentException("Specification cannot be null", nameof(specification));

            return await GetQueryable(loadProperties).Where(specification.SatisfiedBy()).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> AllMatchingAsync(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            if (specification == null)
                throw new ArgumentException("Specification cannot be null", nameof(specification));

            return await GetQueryable(loadProperties).Where(specification.SatisfiedBy()).ToListAsync();
        }

        public async Task<long> CountAsync()
        {
            return await GetSet().LongCountAsync();
        }

        public async Task<long> CountAsync(ISpecification<TEntity> specification)
        {
            return await GetSet().LongCountAsync(specification.SatisfiedBy());
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetSet().LongCountAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int limit, ISorting[] sortColumns)
        {
            return await GetPagedAsync(1, limit, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            return await GetPagedAsync((ISpecification<TEntity>)null, 1, limit, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetPagedAsync((ISpecification<TEntity>)null, 1, limit, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns)
        {
            return await GetPagedAsync(specification, 1, limit, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            return await GetPagedAsync(specification, 1, limit, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(ISpecification<TEntity> specification, int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetPagedAsync(specification, 1, limit, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns)
        {
            return await GetPagedAsync(filter, 1, limit, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns, params string[] loadProperties)
        {
            return await GetPagedAsync(filter, 1, limit, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, int limit, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetPagedAsync(filter, 1, limit, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            return await GetPagedAsync(pageIndex, pageCount, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageCount, ISorting[] sortColumns, params string[] loadProperties)
        {
            return await GetPagedAsync((ISpecification<TEntity>)null, pageIndex, pageCount, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageCount, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetPagedAsync((ISpecification<TEntity>)null, pageIndex, pageCount, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            return await GetPagedAsync(specification, pageIndex, pageCount, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns,
            params string[] loadProperties)
        {
            return await GetPagedAsync(specification?.SatisfiedBy(), pageIndex, pageCount, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(ISpecification<TEntity> specification, int pageIndex, int pageCount, ISorting[] sortColumns,
            params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetPagedAsync(specification?.SatisfiedBy(), pageIndex, pageCount, sortColumns, loadProperties);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns)
        {
            return await GetPagedAsync(filter, pageIndex, pageCount, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns,
            params string[] loadProperties)
        {
            var query = filter != null ? GetQueryable(loadProperties).Where(filter) : GetQueryable(loadProperties);

            if (sortColumns != null && sortColumns.Length > 0)
            {
                query = query.OrderBy(sortColumns);
            }
            if (pageCount > 0)
                return query.Skip((pageIndex - 1) * pageCount).Take(pageCount);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, ISorting[] sortColumns,
            params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetPagedAsync(filter, pageIndex, pageCount, sortColumns, GetPropertyNames(loadProperties));
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetFilteredAsync(filter, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, params string[] loadProperties)
        {
            if (filter == null)
                throw new ArgumentException("Filter expression cannot be null", nameof(filter));

            return await GetQueryable(loadProperties).Where(filter).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            if (filter == null)
                throw new ArgumentException("Filter expression cannot be null", nameof(filter));

            return await GetQueryable(loadProperties).Where(filter).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns)
        {
            return await GetFilteredAsync(filter, sortColumns, new Expression<Func<TEntity, object>>[0]);
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns, params string[] loadProperties)
        {
            if (filter == null)
                throw new ArgumentException("Filter expression cannot be null", nameof(filter));

            var query = GetQueryable(loadProperties).Where(filter);
            if (sortColumns != null && sortColumns.Length > 0)
            {
                query = query.OrderBy(sortColumns);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, ISorting[] sortColumns, params Expression<Func<TEntity, object>>[] loadProperties)
        {
            return await GetFilteredAsync(filter, sortColumns, GetPropertyNames(loadProperties));
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public async Task<int> DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetSet().Where(filter).DeleteAsync();
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public async Task<int> DeleteManyAsync(ISpecification<TEntity> specification)
        {
            return await DeleteManyAsync(specification.SatisfiedBy());
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="filter"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="updateFactory"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public async Task<int> UpdateManyAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> updateFactory)
        {
            return await GetSet().Where(filter).UpdateAsync(updateFactory);
        }

        /// <summary>
        /// <see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/>
        /// </summary>
        /// <param name="specification"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <param name="updateFactory"><see cref="eQuantic.Core.Data.Repository.IRepository{TUnitOfWork, TEntity, TKey}"/></param>
        /// <returns></returns>
        public async Task<int> UpdateManyAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, TEntity>> updateFactory)
        {
            return await UpdateManyAsync(specification.SatisfiedBy(), updateFactory);
        }
    }
}
