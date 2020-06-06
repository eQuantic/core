using System;
using System.Linq.Expressions;
using eQuantic.Core.Linq.Extensions;
using eQuantic.Core.Linq.Helpers;

namespace eQuantic.Core.Linq.Filter
{
    /// <summary>
    /// Filtering
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="eQuantic.Core.Linq.Filter.Filtering" />
    public class Filtering<T> : Filtering
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filtering{T}"/> class.
        /// </summary>
        public Filtering()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filtering{T}"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="stringValue">The string value.</param>
        /// <param name="operator">The operator.</param>
        public Filtering(Expression<Func<T, object>> expression, string stringValue, FilterOperator @operator = FilterOperator.Equal)
            : base(GetColumnName(expression), stringValue, @operator)
        {
        }

        /// <summary>
        /// Sets the column.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public void SetColumn(Expression<Func<T, object>> expression)
        {
            ColumnName = GetColumnName(expression);
        }

        private static string GetColumnName(Expression<Func<T, object>> expression)
        {
            if (!(expression.Body is MemberExpression member))
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                member = (MemberExpression)op;
            }
            return PropertiesHelper.BuildColumnNameFromMemberExpression(member);
        }
    }

    public class Filtering : IFiltering
    {
        public Filtering()
        {
        }

        public Filtering(string columnName, string stringValue, FilterOperator @operator = FilterOperator.Equal)
        {
            this.ColumnName = columnName;
            this.StringValue = stringValue;
            this.Operator = @operator;
        }

        public string ColumnName { get; set; }
        public FilterOperator Operator { get; set; } = FilterOperator.Equal;
        public string StringValue { get; set; }
    }
}