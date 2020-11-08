using System;
using System.Globalization;
using System.Linq.Expressions;
using eQuantic.Core.Linq.Extensions;
using eQuantic.Core.Linq.Helpers;

namespace eQuantic.Core.Linq.Sorter
{
    public class Sorting<T> : Sorting
    {
        public Sorting() { }

        public Sorting(Expression<Func<T, object>> expression, SortDirection sortDirection)
        {
            SetColumn(expression);
            SortDirection = sortDirection;
        }

        public void SetColumn(Expression<Func<T, object>> expression)
        {
            if (!(expression.Body is MemberExpression member))
            {
                var op = ((UnaryExpression) expression.Body).Operand;
                member = (MemberExpression) op;
            }
            ColumnName = PropertiesHelper.BuildColumnNameFromMemberExpression(member);
        }
    }

    public class Sorting : ISorting
    {
        public const string DefaultFormat = "{0}:{1}";

        public Sorting() { }

        public Sorting(string columnName, SortDirection sortDirection)
        {
            this.ColumnName = columnName;
            this.SortDirection = sortDirection;
        }

        public string ColumnName { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        public override string ToString()
        {
            return this.ToString(DefaultFormat, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format ??= DefaultFormat;
            formatProvider ??= CultureInfo.InvariantCulture;
            var directionString = SortDirection == SortDirection.Ascending ? "asc" : "desc";
            return string.Format(formatProvider, format, ColumnName, directionString);
        }
    }
}
