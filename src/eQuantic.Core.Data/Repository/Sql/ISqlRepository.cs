using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eQuantic.Core.Data.Repository.Sql
{
    public interface ISqlRepository<TEntity, TKey> : IRepository<IQueryableUnitOfWork, TEntity, TKey> where TEntity : class, IEntity, new()
    {

    }
}
