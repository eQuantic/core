using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eQuantic.Core.Linq.Helpers;

namespace eQuantic.Core.Data.Repository
{
    public class UpdateField<TEntity> where TEntity : class, new()
    {
        public Expression<Func<TEntity, object>> Column { get; set; }
        public object Value { get; set; }
    }

    public class UpdateField<TEntity, TProperty> : UpdateField<TEntity> where TEntity : class , new()
    {
        public new Expression<Func<TEntity, TProperty>> Column { get; set; }

        public new TProperty Value { get; set; }
    }


}
