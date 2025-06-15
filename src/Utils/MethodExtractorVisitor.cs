using System.Linq.Expressions;
using System.Reflection;

namespace eQuantic.Core.Utils;

/// <summary>
/// An expression visitor that extracts method information from method call expressions.
/// </summary>
public class MethodExtractorVisitor : ExpressionVisitor
{
    /// <summary>
    /// Gets the extracted method information from the visited expression.
    /// </summary>
    public MethodInfo Method { get; private set; }

    /// <summary>
    /// Visits a method call expression and extracts the method information.
    /// </summary>
    /// <param name="node">The method call expression to visit.</param>
    /// <returns>The visited expression.</returns>
    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (Method == null)
        {
            Method = node.Method;
        }

        return base.VisitMethodCall(node);
    }
}