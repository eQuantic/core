using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using eQuantic.Core.Utils;

namespace eQuantic.Core.Extensions
{
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

        public static string GetBreadcrumbMemberName<TResult>(Expression<Func<TResult>> action, string separator = ".",
            int skip = 0)
        {
            return GetBreadcrumbMemberName(action as LambdaExpression, separator, skip);
        }

        public static string GetBreadcrumbMemberName<TResult>(Expression<Func<TResult>> action, int skip)
        {
            return GetBreadcrumbMemberName(action as LambdaExpression, skip: skip);
        }

        public static string GetMemberName<T, TResult>(IEnumerable<T> obj, Expression<Func<T, TResult>> action)
        {
            return GetMemberName(action);
        }

        public static string GetObjectMemberName<T, TResult>(T obj, Expression<Func<T, TResult>> action)
        {
            return GetMemberName(action);
        }

        public static string GetMemberName<TParam, TResult>(Expression<Func<TParam, TResult>> action)
        {
            return GetMemberName(action as LambdaExpression);
        }

        public static string GetMemberName<TResult>(Expression<Func<TResult>> action)
        {
            return GetMemberName(action as LambdaExpression);
        }

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

        public static Type GetMemberType<TResult>(Expression<Func<TResult>> action)
        {
            return typeof(TResult);
        }

        public static Type GetMemberType<T, TResult>(IEnumerable<T> obj, Expression<Func<T, TResult>> action)
        {
            return typeof(TResult);
        }

        public static PropertyInfo GetPropertyInfo<T>(T obj, Expression<Func<T, object>> func)
        {
            return GetPropertyInfo<T>(func);
        }

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
#if NETSTANDARD1_3
            return type.GetTypeInfo().GetDeclaredProperty(member.Member.Name);
#else
            return
                type.GetProperty(member.Member.Name)
                    as PropertyInfo;
#endif
        }

        public static MethodInfo GetMethodInfo<T1, T2>(Expression<Func<T1, Action<T2>>> a)
        {
            UnaryExpression exp = a.Body as UnaryExpression;
            MethodCallExpression call = exp.Operand as MethodCallExpression;
            ConstantExpression arg = call.Arguments[2] as ConstantExpression;
            return arg.Value as MethodInfo;
        }

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

    public static class Reflection<T>
    {
        public static MethodInfo GetMethod<TResult>(Expression<Func<T, TResult>> action)
        {
            var methodExtractor = new MethodExtractorVisitor();
            methodExtractor.Visit(action);

            return methodExtractor.Method;
        }

        public static string GetMemberName<TResult>(Expression<Func<T, TResult>> action)
        {
            return Reflection.GetMemberName(action);
        }

        public static string GetMemberName(Expression<Action<T>> action)
        {
            return Reflection.GetMemberName(action);
        }

        public static Type GetMemberType<TResult>(Expression<Func<T, TResult>> action)
        {
            return typeof(TResult);
        }

        public static string GetPropertiesPath<TResult>(Expression<Func<T, TResult>> propertyExpression)
        {
            return string.Join(".", GetProperties(propertyExpression).Select(p => p.Name).ToArray());
        }

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

        public static IEnumerable<TAttribute> GetPropertyAttributes<TAttribute>(
            Expression<Func<T, object>> propertyExpression, bool inherit = true) where TAttribute : Attribute
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                return new TAttribute[0];

            return memberExpression.Member.GetCustomAttributes<TAttribute>(inherit);
        }

        public static IEnumerable<string> GetBreadcrumbMemberList<TResult>(Expression<Func<T, TResult>> action)
        {
            return Reflection.GetBreadcrumbMemberList(action as LambdaExpression);
        }

        public static string GetBreadcrumbMemberName<TResult>(Expression<Func<T, TResult>> action,
            string separator = ".")
        {
            return Reflection.GetBreadcrumbMemberName(action as LambdaExpression, separator);
        }
    }
}