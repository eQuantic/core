using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using eQuantic.Core.Linq.Filter;

namespace eQuantic.Core.Linq
{
    internal interface ILambdaBuilder
    {
        LambdaExpression BuildLambda(IEnumerable<MethodInfo> propertyAccessors);

        LambdaExpression BuildLambda(IEnumerable<PropertyInfo> properties);

        LambdaExpression BuildLambda(IEnumerable<PropertyInfo> properties, object value, FilterOperator @operator = FilterOperator.Equal);
    }

    internal sealed class LambdaBuilder<T, TKey> : ILambdaBuilder
    {
        public LambdaExpression BuildLambda(IEnumerable<MethodInfo> propertyAccessors)
        {
            var parameterExpression = GetParameterExpression();
            var propertyExpression = BuildPropertyExpression(propertyAccessors, parameterExpression);
            return Expression.Lambda<Func<T, TKey>>(propertyExpression, parameterExpression);
        }

        public LambdaExpression BuildLambda(IEnumerable<PropertyInfo> properties)
        {
            var parameterExpression = GetParameterExpression();
            var propertyExpression = BuildPropertyExpression(properties, parameterExpression);
            return Expression.Lambda<Func<T, TKey>>(propertyExpression, parameterExpression);
        }

        public LambdaExpression BuildLambda(IEnumerable<PropertyInfo> properties, object value, FilterOperator @operator = FilterOperator.Equal)
        {
            var parameterExpression = GetParameterExpression();
            var binaryExpression = BuildBinaryExpression(properties, parameterExpression, value, @operator);
            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
        }

        private static Expression BuildBinaryExpression(IEnumerable<PropertyInfo> properties, ParameterExpression parameterExpression, object value, FilterOperator @operator)
        {
            var propertyExpression = BuildPropertyExpression(properties, parameterExpression);
            var constant = Expression.Constant(value, typeof(TKey));

            switch (@operator)
            {
                case FilterOperator.GreaterThan:
                    return Expression.GreaterThan(propertyExpression, constant);

                case FilterOperator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(propertyExpression, constant);

                case FilterOperator.LessThan:
                    return Expression.LessThan(propertyExpression, constant);

                case FilterOperator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(propertyExpression, constant);

                case FilterOperator.Contains:
                    return Expression.Call(propertyExpression, GetMethod(nameof(string.Contains), typeof(string)), constant);

                case FilterOperator.StartsWith:
                    return Expression.Call(propertyExpression, GetMethod(nameof(string.StartsWith), typeof(string)), constant);

                case FilterOperator.EndsWith:
                    return Expression.Call(propertyExpression, GetMethod(nameof(string.EndsWith), typeof(string)), constant);

                case FilterOperator.NotEqual:
                    return Expression.NotEqual(propertyExpression, constant);

                case FilterOperator.Equal:
                default:
                    return Expression.Equal(propertyExpression, constant);
            }
        }

        private static Expression BuildPropertyExpression(IEnumerable<MethodInfo> propertyAccessors, Expression parameterExpression)
        {
            Expression propertyExpression = null;

            foreach (var propertyAccessor in propertyAccessors)
            {
                var innerExpression = propertyExpression ?? parameterExpression;
                propertyExpression = Expression.Property(innerExpression, propertyAccessor);
            }

            return propertyExpression;
        }

        private static Expression BuildPropertyExpression(IEnumerable<PropertyInfo> properties, Expression parameterExpression)
        {
            Expression propertyExpression = null;

            foreach (var property in properties)
            {
                var innerExpression = propertyExpression ?? parameterExpression;

                propertyExpression = Expression.Property(innerExpression, property);
            }

            return propertyExpression;
        }

        private static MethodInfo GetMethod(string name, Type type)
        {
            return type.GetMethod(name, new[] { type });
        }

        private ParameterExpression GetParameterExpression()
        {
            return Expression.Parameter(typeof(T), "entity");
        }
    }
}