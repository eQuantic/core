using System;
using System.Linq.Expressions;
using eQuantic.Core.Linq.Extensions;
using eQuantic.Core.Linq.Helpers;

namespace eQuantic.Core.Linq.Sorter
{
    public class Sorting<T> : Sorting
    {
        public Sorting()
        {
        }

        public Sorting(Expression<Func<T, object>> expression, SortDirection sortDirection)
        {
            SetColumn(expression);
            SortDirection = sortDirection;
        }

        public void SetColumn(Expression<Func<T, object>> expression)
        {
            if (!(expression.Body is MemberExpression member))
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                member = (MemberExpression)op;
            }
            ColumnName = PropertiesHelper.BuildColumnNameFromMemberExpression(member);
        }
    }

    public class Sorting : ISorting
    {
        public Sorting()
        {
        }

        public Sorting(string columnName, SortDirection sortDirection)
        {
            this.ColumnName = columnName;
            this.SortDirection = sortDirection;
        }

        public string ColumnName { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}