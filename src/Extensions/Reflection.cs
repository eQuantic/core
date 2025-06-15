using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using eQuantic.Core.Utils;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides utility methods for reflection operations using lambda expressions.
/// </summary>
public static class Reflection
{
    private static List<string> GetBreadcrumbMemberList(Expression expression)
    {
        if (expression == null || expression is ParameterExpression || expression is ConstantExpression)
            return new List<string>();

        var memberExpression = expression as MemberExpression;
        if (memberExpression == null)
            throw new ArgumentException("Expression must have property accessors only");

        var list = GetBreadcrumbMemberList(memberExpression.Expression);
        list.Add(memberExpression.Member.Name);
        return list;
    }

    /// <summary>
    /// Gets a list of member names from a breadcrumb expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the property path.</param>
    /// <returns>An enumerable of member names in the breadcrumb path.</returns>
    public static IEnumerable<string> GetBreadcrumbMemberList<TResult>(Expression<Func<TResult>> action)
    {
        return GetBreadcrumbMemberList(action as LambdaExpression);
    }

    internal static IEnumerable<string> GetBreadcrumbMemberList(LambdaExpression action)
    {
        #region Input Validation

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        #endregion

        var member = action.Body as MemberExpression;
        if (member == null)
            throw new ArgumentException("Expression must be a property accessor");

        return GetBreadcrumbMemberList(member);
    }

    internal static string GetBreadcrumbMemberName(LambdaExpression action, string separator = ".", int skip = 0)
    {
        return string.Join(separator, GetBreadcrumbMemberList(action).Skip(skip));
    }

    /// <summary>
    /// Gets a breadcrumb member name from a lambda expression with custom separator and skip options.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the property path.</param>
    /// <param name="separator">The separator to use between member names. Defaults to ".".</param>
    /// <param name="skip">The number of members to skip from the beginning. Defaults to 0.</param>
    /// <returns>A string representing the breadcrumb path of member names.</returns>
    public static string GetBreadcrumbMemberName<TResult>(Expression<Func<TResult>> action, string separator = ".",
        int skip = 0)
    {
        return GetBreadcrumbMemberName(action as LambdaExpression, separator, skip);
    }

    /// <summary>
    /// Gets a breadcrumb member name from a lambda expression with skip option.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the property path.</param>
    /// <param name="skip">The number of members to skip from the beginning.</param>
    /// <returns>A string representing the breadcrumb path of member names.</returns>
    public static string GetBreadcrumbMemberName<TResult>(Expression<Func<TResult>> action, int skip)
    {
        return GetBreadcrumbMemberName(action as LambdaExpression, skip: skip);
    }

    /// <summary>
    /// Gets the member name from a lambda expression for an enumerable object.
    /// </summary>
    /// <typeparam name="T">The type of the enumerable items.</typeparam>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="obj">The enumerable object (not used in the implementation).</param>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The name of the member accessed in the expression.</returns>
    public static string GetMemberName<T, TResult>(IEnumerable<T> obj, Expression<Func<T, TResult>> action)
    {
        return GetMemberName(action);
    }

    /// <summary>
    /// Gets the member name from a lambda expression for a specific object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="obj">The object (not used in the implementation).</param>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The name of the member accessed in the expression.</returns>
    public static string GetObjectMemberName<T, TResult>(T obj, Expression<Func<T, TResult>> action)
    {
        return GetMemberName(action);
    }

    /// <summary>
    /// Gets the member name from a lambda expression with parameter and result types.
    /// </summary>
    /// <typeparam name="TParam">The parameter type of the expression.</typeparam>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The name of the member accessed in the expression.</returns>
    public static string GetMemberName<TParam, TResult>(Expression<Func<TParam, TResult>> action)
    {
        return GetMemberName(action as LambdaExpression);
    }

    /// <summary>
    /// Gets the member name from a lambda expression with result type.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The name of the member accessed in the expression.</returns>
    public static string GetMemberName<TResult>(Expression<Func<TResult>> action)
    {
        return GetMemberName(action as LambdaExpression);
    }

    /// <summary>
    /// Gets the member name from an action lambda expression.
    /// </summary>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The name of the member accessed in the expression.</returns>
    public static string GetMemberName(Expression<Action> action)
    {
        return GetMemberName(action as LambdaExpression);
    }

    internal static string GetMemberName(LambdaExpression action)
    {
        #region Input Validation

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        #endregion

        var member = action.Body as MemberExpression;
        if (member != null)
        {
            return member.Member.Name;
        }

        var method = action.Body as MethodCallExpression;
        if (method == null)
        {
            throw new ArgumentException("Expression must be a property accessor or method call");
        }

        return method.Method.Name;
    }

    /// <summary>
    /// Gets the type of the member accessed in the lambda expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The type of the result.</returns>
    public static Type GetMemberType<TResult>(Expression<Func<TResult>> action)
    {
        return typeof(TResult);
    }

    /// <summary>
    /// Gets the type of the member accessed in the lambda expression for an enumerable object.
    /// </summary>
    /// <typeparam name="T">The type of the enumerable items.</typeparam>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="obj">The enumerable object (not used in the implementation).</param>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The type of the result.</returns>
    public static Type GetMemberType<T, TResult>(IEnumerable<T> obj, Expression<Func<T, TResult>> action)
    {
        return typeof(TResult);
    }

    /// <summary>
    /// Gets the PropertyInfo for a property accessed in a lambda expression.
    /// </summary>
    /// <typeparam name="T">The type containing the property.</typeparam>
    /// <param name="obj">The object instance (not used in the implementation).</param>
    /// <param name="func">The lambda expression representing the property access.</param>
    /// <returns>The PropertyInfo for the accessed property.</returns>
    public static PropertyInfo GetPropertyInfo<T>(T obj, Expression<Func<T, object>> func)
    {
        return GetPropertyInfo<T>(func);
    }

    /// <summary>
    /// Gets the PropertyInfo for a property accessed in a lambda expression.
    /// </summary>
    /// <typeparam name="T">The type containing the property.</typeparam>
    /// <param name="func">The lambda expression representing the property access.</param>
    /// <returns>The PropertyInfo for the accessed property.</returns>
    /// <exception cref="ArgumentException">Thrown when the lambda expression is not a property accessor.</exception>
    public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> func)
    {
        var unary = (func.Body as UnaryExpression);
        MemberExpression member;
        if (unary == null)
        {
            member = func.Body as MemberExpression;
        }
        else
        {
            member = unary.Operand as MemberExpression;
        }

        if ((member.Member is PropertyInfo) == false)
        {
            throw new ArgumentException("Lambda is not a Property");
        }
        var type = (member.Expression as ParameterExpression)?.Type;
#if NETSTANDARD1_6
            return type.GetTypeInfo().GetDeclaredProperty(member.Member.Name);
#else
        return
            type.GetProperty(member.Member.Name)
                as PropertyInfo;
#endif
    }

    /// <summary>
    /// Gets the MethodInfo for a method accessed in a complex lambda expression.
    /// </summary>
    /// <typeparam name="T1">The first type parameter.</typeparam>
    /// <typeparam name="T2">The second type parameter.</typeparam>
    /// <param name="a">The lambda expression representing the method access.</param>
    /// <returns>The MethodInfo for the accessed method.</returns>
    public static MethodInfo GetMethodInfo<T1, T2>(Expression<Func<T1, Action<T2>>> a)
    {
        UnaryExpression exp = a.Body as UnaryExpression;
        MethodCallExpression call = exp.Operand as MethodCallExpression;
        ConstantExpression arg = call.Arguments[2] as ConstantExpression;
        return arg.Value as MethodInfo;
    }

    /// <summary>
    /// Gets the MethodInfo for a method called in a lambda expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the method call.</param>
    /// <returns>The MethodInfo for the called method.</returns>
    /// <exception cref="ArgumentException">Thrown when the expression is not a method call.</exception>
    public static MethodInfo GetMethodInfo<TResult>(Expression<Func<TResult>> action)
    {
        var method = action.Body as MethodCallExpression;
        if (method == null)
        {
            throw new ArgumentException("Expression must be a method call");
        }

        return method.Method;
    }
}

/// <summary>
/// Provides type-specific utility methods for reflection operations using lambda expressions.
/// </summary>
/// <typeparam name="T">The type to perform reflection operations on.</typeparam>
public static class Reflection<T>
{
    /// <summary>
    /// Gets the MethodInfo for a method called in a lambda expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the method call.</param>
    /// <returns>The MethodInfo for the called method.</returns>
    public static MethodInfo GetMethod<TResult>(Expression<Func<T, TResult>> action)
    {
        var methodExtractor = new MethodExtractorVisitor();
        methodExtractor.Visit(action);

        return methodExtractor.Method;
    }

    /// <summary>
    /// Gets the member name from a lambda expression with result type.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The name of the member accessed in the expression.</returns>
    public static string GetMemberName<TResult>(Expression<Func<T, TResult>> action)
    {
        return Reflection.GetMemberName(action);
    }

    /// <summary>
    /// Gets the member name from an action lambda expression.
    /// </summary>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The name of the member accessed in the expression.</returns>
    public static string GetMemberName(Expression<Action<T>> action)
    {
        return Reflection.GetMemberName(action);
    }

    /// <summary>
    /// Gets the type of the member accessed in the lambda expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the member access.</param>
    /// <returns>The type of the result.</returns>
    public static Type GetMemberType<TResult>(Expression<Func<T, TResult>> action)
    {
        return typeof(TResult);
    }

    /// <summary>
    /// Gets a dot-separated path of property names from a property expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="propertyExpression">The lambda expression representing the property path.</param>
    /// <returns>A string representing the property path with dots as separators.</returns>
    public static string GetPropertiesPath<TResult>(Expression<Func<T, TResult>> propertyExpression)
    {
        return string.Join(".", GetProperties(propertyExpression).Select(p => p.Name).ToArray());
    }

    /// <summary>
    /// Gets an enumerable of PropertyInfo objects from a property expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="propertyExpression">The lambda expression representing the property path.</param>
    /// <returns>An enumerable of PropertyInfo objects in the property path.</returns>
    public static IEnumerable<PropertyInfo> GetProperties<TResult>(Expression<Func<T, TResult>> propertyExpression)
    {
        var unary = (propertyExpression.Body as UnaryExpression);
        if (unary != null)
        {
            return GetProperties(unary.Operand);
        }

        return GetProperties(propertyExpression.Body);
    }

    private static IEnumerable<PropertyInfo> GetProperties(Expression expression)
    {
        var memberExpression = expression as MemberExpression;
        if (memberExpression == null) yield break;

        var property = memberExpression.Member as PropertyInfo;
        if (property == null)
        {
            throw new ArgumentException("Expression is not a property accessor");
        }
        foreach (var propertyInfo in GetProperties(memberExpression.Expression))
        {
            yield return propertyInfo;
        }
        yield return property;
    }

    /// <summary>
    /// Gets custom attributes of the specified type from a property accessed in a lambda expression.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
    /// <param name="propertyExpression">The lambda expression representing the property access.</param>
    /// <param name="inherit">Whether to search the inheritance chain for attributes. Defaults to true.</param>
    /// <returns>An enumerable of attributes of the specified type.</returns>
    public static IEnumerable<TAttribute> GetPropertyAttributes<TAttribute>(
        Expression<Func<T, object>> propertyExpression, bool inherit = true) where TAttribute : Attribute
    {
        var memberExpression = propertyExpression.Body as MemberExpression;
        if (memberExpression == null)
            return new TAttribute[0];

        return memberExpression.Member.GetCustomAttributes<TAttribute>(inherit);
    }

    /// <summary>
    /// Gets a list of member names from a breadcrumb expression.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the property path.</param>
    /// <returns>An enumerable of member names in the breadcrumb path.</returns>
    public static IEnumerable<string> GetBreadcrumbMemberList<TResult>(Expression<Func<T, TResult>> action)
    {
        return Reflection.GetBreadcrumbMemberList(action as LambdaExpression);
    }

    /// <summary>
    /// Gets a breadcrumb member name from a lambda expression with custom separator.
    /// </summary>
    /// <typeparam name="TResult">The result type of the expression.</typeparam>
    /// <param name="action">The lambda expression representing the property path.</param>
    /// <param name="separator">The separator to use between member names. Defaults to ".".</param>
    /// <returns>A string representing the breadcrumb path of member names.</returns>
    public static string GetBreadcrumbMemberName<TResult>(Expression<Func<T, TResult>> action,
        string separator = ".")
    {
        return Reflection.GetBreadcrumbMemberName(action as LambdaExpression, separator);
    }
}