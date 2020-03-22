using System;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Extensions
{
    public static class ExpressionExtensions
    {
        public static void ToExpressionMapper<T>(this Expression<Func<T, bool>> expression)
        {

        }
    }
}
