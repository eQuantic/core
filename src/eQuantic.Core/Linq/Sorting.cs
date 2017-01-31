using eQuantic.Core.Linq.Helpers;
using System;
using System.Linq.Expressions;
namespace eQuantic.Core.Linq
{
    internal enum SortDirection
    {
        Ascending,
        Descending
    }
    public class Sorting<T> : Sorting
    {
        public Expression<Func<T, object>> Column
        {
            set
            {
                MemberExpression member;
                if (value.Body is MemberExpression)
                {
                    member = (MemberExpression)value.Body;
                }
                else
                {
                    var op = ((UnaryExpression)value.Body).Operand;
                    member = (MemberExpression)op;
                }
                ColumnName = PropertiesHelper.BuildColumnNameFromMemberExpression(member);
            }
        }
    }

    public class Sorting : ISorting
    {
        public string ColumnName { get; set; }
        public bool Ascending { get; set; }

        public Sorting()
        {
            Ascending = true;
        }
    }
}
