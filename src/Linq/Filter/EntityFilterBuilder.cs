﻿using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Filter
{
    internal class EntityFilterBuilder<T>
    {
        private readonly LambdaExpression keySelector;

        public EntityFilterBuilder(string propertyName, object value, FilterOperator @operator)
        {
            var properties = EntityBuilder.GetProperties<T>(propertyName);
            var keyType = properties.Last().PropertyType;
            var builder = CreateLambdaBuilder(keyType);
            var convertedValue = ConvertValue(value, keyType);
            keySelector = builder.BuildLambda(properties, convertedValue, @operator);
        }

        public IEntityFilter<T> BuildWhereEntityFilter()
        {
            var typeArgs = new[] { typeof(T) };

            var filterType = typeof(WhereEntityFilter<>).MakeGenericType(typeArgs);

            return (IEntityFilter<T>)Activator.CreateInstance(filterType, keySelector);
        }

        public IEntityFilter<T> BuildWhereEntityFilter(IEntityFilter<T> filter)
        {
            var typeArgs = new[] { typeof(T) };

            var filterType = typeof(WhereEntityFilter<>).MakeGenericType(typeArgs);

            return (IEntityFilter<T>)Activator.CreateInstance(filterType, filter, keySelector);
        }

        private static ILambdaBuilder CreateLambdaBuilder(Type keyType)
        {
            var typeArgs = new[] { typeof(T), keyType };

            var builderType = typeof(LambdaBuilder<,>).MakeGenericType(typeArgs);

            return (ILambdaBuilder)Activator.CreateInstance(builderType);
        }
        
        private static object ConvertValue(object value, Type keyType)
        {
            if (value.GetType() == keyType) return value;
            if (string.IsNullOrEmpty(value?.ToString()) && Nullable.GetUnderlyingType(keyType) != null)
            {
                return null;
            }

            if (keyType == typeof(Guid))
            {
                return Guid.Parse(value.ToString());
            }

            return Convert.ChangeType(value, keyType, CultureInfo.InvariantCulture);
        }
    }
}