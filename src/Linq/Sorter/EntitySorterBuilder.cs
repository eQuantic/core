using System;
using System.Linq;
using System.Linq.Expressions;

namespace eQuantic.Core.Linq.Sorter
{
    /// <summary>
    /// Entity Sorter Builder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class EntitySorterBuilder<T>
    {
        private readonly LambdaExpression keySelector;
        private readonly Type keyType;

        public EntitySorterBuilder(string propertyName)
        {
            var properties = EntityBuilder.GetProperties<T>(propertyName);
            keyType = properties.Last().PropertyType;
            var builder = CreateLambdaBuilder(keyType);
            keySelector = builder.BuildLambda(properties);
        }

        public EntitySorterBuilder(ISorting sorting) : this(sorting.ColumnName, sorting.SortDirection)
        {
        }

        public EntitySorterBuilder(string propertyName, SortDirection sortDirection) : this(propertyName)
        {
            Direction = sortDirection;
        }

        public SortDirection Direction { get; set; }

        public IEntitySorter<T> BuildOrderByEntitySorter()
        {
            var typeArgs = new[] { typeof(T), keyType };

            var sortType = typeof(OrderBySorter<,>).MakeGenericType(typeArgs);

            return (IEntitySorter<T>)Activator.CreateInstance(sortType, keySelector, Direction);
        }

        public IEntitySorter<T> BuildThenByEntitySorter(
            IEntitySorter<T> baseSorter)
        {
            var typeArgs = new[] { typeof(T), keyType };

            var sortType = typeof(ThenBySorter<,>).MakeGenericType(typeArgs);

            return (IEntitySorter<T>)Activator.CreateInstance(sortType, baseSorter, keySelector, Direction);
        }

        private static ILambdaBuilder CreateLambdaBuilder(Type keyType)
        {
            var typeArgs = new[] { typeof(T), keyType };

            var builderType = typeof(LambdaBuilder<,>).MakeGenericType(typeArgs);

            return (ILambdaBuilder)Activator.CreateInstance(builderType);
        }
    }
}