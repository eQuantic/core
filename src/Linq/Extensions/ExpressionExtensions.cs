using System;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Extensions
{
    public static class ExpressionExtensions
    {
        public static ExpressionMapper ToExpressionMapper<T>(this Expression<Func<T, bool>> expression)
        {
            return new ExpressionMapper(expression);
        }
    }
}
