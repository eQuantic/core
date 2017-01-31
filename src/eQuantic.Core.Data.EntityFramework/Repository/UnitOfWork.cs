using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using eQuantic.Core.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace eQuantic.Core.Data.EntityFramework.Repository
{
    public abstract class UnitOfWork : IQueryableUnitOfWork
    {
        private readonly DbContext _context;
        private IDbContextTransaction _transaction;
        public static int IsMigrating = 0;


        protected UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }

        public abstract TRepository GetRepository<TRepository>() where TRepository : IRepository;

        public abstract TRepository GetRepository<TRepository>(string name) where TRepository : IRepository;

        public void BeginTransaction()
        {
            _transaction?.Dispose();
            _transaction = _context.Database.BeginTransaction();
        }

        public int Commit()
        {
            try
            {
                var i = _context.SaveChanges();

                _transaction?.Commit();

                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CommitAndRefreshChanges()
        {
            int changes = 0;
            bool saveFailed = false;

            do
            {
                try
                {
                    changes = _context.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));

                }
            } while (saveFailed);

            return changes;
        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            _context?.ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);

            _transaction?.Rollback();
        }

        private string GetQueryParameters(params object[] parameters)
        {
            var cmd = "";
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i > 0) cmd += ",";

                    if (parameters[i] == null) cmd += " NULL";
                    else
                    {
                        var fmt = " {0}";
                        if (parameters[i] is Guid || parameters[i] is String || parameters[i] is DateTime)
                            fmt = " '{0}'";

                        cmd += string.Format(fmt, parameters[i]);
                    }
                }
            }
            return cmd;
        }
        private string GetQueryProcedure(string name, params object[] parameters)
        {
            return $"EXEC {name}{GetQueryParameters(parameters)}";
        }

        private string GetQueryFunction(string name, params object[] parameters)
        {
            return $"SELECT {name}({GetQueryParameters(parameters)} )";
        }

        public int ExecuteProcedure(string name, params object[] parameters)
        {
            return ExecuteCommand(GetQueryProcedure(name, parameters) + ";");
        }

        public async Task<int> ExecuteProcedureAsync(string name, params object[] parameters)
        {
            return await ExecuteCommandAsync(GetQueryProcedure(name, parameters) + ";");
        }

        public TResult ExecuteFunction<TResult>(string name, params object[] parameters) where TResult : class
        {
            return _context.Set<TResult>().FromSql(GetQueryFunction(name, parameters)).FirstOrDefault();
        }

        public async Task<TResult> ExecuteFunctionAsync<TResult>(string name, params object[] parameters) where TResult : class
        {
            return await _context.Set<TResult>().FromSql(GetQueryFunction(name, parameters)).FirstOrDefaultAsync();
        }


        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class
        {
            return _context.Set<TEntity>().FromSql(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public async Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sqlCommand, default(CancellationToken), parameters);
        }

        public IQueryable<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            //attach and set as unchanged
            _context.Entry<TEntity>(item).State = EntityState.Unchanged;
        }

        public void Reload<TEntity>(TEntity item) where TEntity : class
        {
            var entry = _context.Entry(item);
            entry.CurrentValues.SetValues(entry.OriginalValues);
            entry.Reload();
            //var oc = ((IObjectContextAdapter)_context).ObjectContext;
            //var k = oc.ObjectStateManager.GetObjectStateEntry(item).EntityKey;
            //oc.Detach(item);
            //item = base.Set<TEntity>().Find(k.EntityKeyValues.Select(kv => kv.Value).ToArray());
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            //this operation also attach item in object state manager
            _context.Entry<TEntity>(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            //if it is not attached, attach original and set current values
            _context.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        public void LoadCollection<TEntity, TElement>(TEntity item, Expression<Func<TEntity, IEnumerable<TElement>>> navigationProperty, Expression<Func<TElement, bool>> filter = null) where TEntity : class where TElement : class
        {
            if (filter != null)
                _context.Entry<TEntity>(item).Collection(navigationProperty).Query().Where(filter).Load();
            else
                _context.Entry<TEntity>(item).Collection(navigationProperty).Load();
        }

        public void LoadProperty<TEntity, TComplexProperty>(TEntity item, Expression<Func<TEntity, TComplexProperty>> selector) where TEntity : class where TComplexProperty : class
        {
            //TODO Waiting for EntityFramework Core team update
            //var entity = _context.Entry<TEntity>(item);
            //_context.Entry(entity.ComplexProperty(selector).CurrentValue).Reload();
        }

        public void UpdateDatabase()
        {
            if (0 == System.Threading.Interlocked.Exchange(ref IsMigrating, 1))
            {
                try
                {
                    //TODO Run migrations
                }
                finally
                {
                    System.Threading.Interlocked.Exchange(ref IsMigrating, 0);
                }
            }
        }

        public IEnumerable<string> GetPendingMigrations()
        {
            //TODO List pending migrations
            //var dbMigrator = GetMigrator();
            //return dbMigrator.GetPendingMigrations();
            return null;
        }
    }
}
