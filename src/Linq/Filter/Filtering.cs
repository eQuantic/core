using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filtering{T}"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="stringValue">The string value.</param>
        /// <param name="operator">The operator.</param>
        public Filtering(Expression<Func<T, object>> expression, string stringValue, FilterOperator @operator = FilterOperator.Equal) : base(GetColumnName(expression), stringValue, @operator)
        { }

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
                var op = ((UnaryExpression) expression.Body).Operand;
                member = (MemberExpression) op;
            }
            return PropertiesHelper.BuildColumnNameFromMemberExpression(member);
        }
    }

    public class Filtering : IFiltering
    {
        public const string DefaultFormat = "{0}:{1}({2})";
        private const string FuncRegex = @"(\b[^()]+)\((.*)\)$";

        public Filtering()
        { }

        public Filtering(string columnName, string stringValue, FilterOperator? @operator = FilterOperator.Equal)
        {
            this.ColumnName = columnName;

            if (@operator == null)
            {
                var parsedValue = ParseValue(stringValue);
                this.StringValue = parsedValue.StringValue;
                this.Operator = parsedValue.Operator;
            }
            else
            {
                this.StringValue = stringValue;
                this.Operator = @operator.Value;
            }
        }

        public string ColumnName { get; set; }
        public FilterOperator Operator { get; set; } = FilterOperator.Equal;
        public string StringValue { get; set; }

        public static(FilterOperator Operator, string StringValue) ParseValue(string value)
        {
            var match = Regex.Match(value, FuncRegex);
            if (match.Success && match.Groups.Count == 3)
            {
                var @operator = match.Groups[1].Value;
                var operatorFilter = FilterOperatorValues.GetOperator(@operator);
                value = match.Groups[2].Value;

                return (operatorFilter, value);
            }

            return (FilterOperator.Equal, value);
        }

        public override string ToString()
        {
            return this.ToString(DefaultFormat, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format ??= DefaultFormat;
            formatProvider ??= CultureInfo.InvariantCulture;
            var @operator = FilterOperatorValues.GetOperator(Operator);
            return string.Format(formatProvider, format, ColumnName, @operator, StringValue);
        }
    }
}
