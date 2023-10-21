using System.Linq.Expressions;
using System.Reflection;

namespace eQuantic.Core.Utils;

public class MethodExtractorVisitor : ExpressionVisitor
{
    public MethodInfo Method { get; private set; }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (Method == null)
        {
            Method = node.Method;
        }

        return base.VisitMethodCall(node);
    }
}