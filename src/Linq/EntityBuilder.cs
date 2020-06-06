using System;
using System.Collections.Generic;
using System.Reflection;

namespace eQuantic.Core.Linq
{
    internal static class EntityBuilder
    {
        public static List<PropertyInfo> GetProperties<T>(string propertyName)
        {
            var properties = new List<PropertyInfo>();

            var declaringType = typeof(T);

            foreach (var name in propertyName.Split('.'))
            {
                var property = GetPropertyByName(propertyName, declaringType, name);

                properties.Add(property);

                declaringType = property.PropertyType;
            }

            return properties;
        }

        private static PropertyInfo GetPropertyByName(string propertyName, IReflect declaringType, string subPropertyName)
        {
            const BindingFlags flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;
            var prop = declaringType.GetProperty(subPropertyName, flags);

            if (prop == null)
            {
                throw new ArgumentException($@"{propertyName} could not be parsed. {declaringType} does not contain a property named '{subPropertyName}'.", nameof(propertyName));
            }

            return prop;
        }
    }
}