using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using eQuantic.Core.MediaFormatter;

namespace eQuantic.Core.Extensions;

internal static class TypeExtensions
{
    private static IEnumerable<KeyValuePair<Type, IEnumerable<T>>> AllTypesWithAttribute<T>(Assembly[] assemblies,
        bool inherit = true)
    {
        foreach (Assembly assembly in assemblies)
        {
            Type[] types = null;

            try
            {
#if NETSTANDARD1_6
                    types = assembly.ExportedTypes.ToArray();
#else
                types = assembly.GetTypes();
#endif

            }
            catch (ReflectionTypeLoadException)
            {
            }

            //This if cannot be inside the try clause because of the "yield return"

            if (types != null)
            {
                foreach (Type type in types)
                {
                    IEnumerable<T> attquery = type.GetTypeInfo().GetCustomAttributes(inherit).OfType<T>();

                    if (attquery.Any())
                    {
                        yield return new KeyValuePair<Type, IEnumerable<T>>(type, attquery);
                    }
                }
            }
        }
    }

    private static IEnumerable<KeyValuePair<Type, T>> AllTypesWithAttributeFirstOnly<T>(Assembly[] assemblies,
        bool inherit = true)
    {
        foreach (Assembly assembly in assemblies)
        {
            Type[] types = null;

            try
            {
#if NETSTANDARD1_6
                    types = assembly.ExportedTypes.ToArray();
#else
                types = assembly.GetTypes();
#endif
            }
            catch (ReflectionTypeLoadException)
            {
            }

            //This if cannot be inside the try clause because of the "yield return"

            if (types != null)
            {
                foreach (Type type in types)
                {
                    IEnumerable<T> attquery = type.GetTypeInfo().GetCustomAttributes(inherit).OfType<T>();

                    if (attquery.Any())
                    {
                        yield return new KeyValuePair<Type, T>(type, attquery.FirstOrDefault());
                    }
                }
            }
        }
    }

#if !NETSTANDARD1_6
    public static IEnumerable<KeyValuePair<Type, IEnumerable<T>>> AllTypesWithAttribute<T>(this AppDomain domain, bool inherit = true)
    {
        return AllTypesWithAttribute<T>(domain.GetAssemblies(), inherit);
    }

    public static IEnumerable<KeyValuePair<Type, T>> AllTypesWithAttributeFirstOnly<T>(this AppDomain domain, bool inherit = true)
    {
        return AllTypesWithAttributeFirstOnly<T>(domain.GetAssemblies(), inherit);
    }

#endif
    internal static TypeConverter GetFromStringConverter(this Type type)
    {
        TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
        if (typeConverter != null && !typeConverter.CanConvertFrom(typeof(String)))
        {
            typeConverter = null;
        }
        return typeConverter;
    }

    internal static TypeConverter GetToStringConverter(this Type type)
    {
        TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
        if (typeConverter is DateTimeConverter)
        {
            //replace default datetime converter for serializing datetime in ISO 8601 format
            typeConverter = new DateTimeConverterISO8601();
        }
        if (typeConverter != null && !typeConverter.CanConvertTo(typeof(String)))
        {
            typeConverter = null;
        }
        return typeConverter;
    }

    internal static IEnumerable<PropertyInfo> GetPublicAccessibleProperties(this Type type)
    {
            
#if NETSTANDARD1_6
            var properties = type.GetTypeInfo().DeclaredProperties;
#else
        var properties = type.GetProperties();
#endif
        foreach (PropertyInfo propertyInfo in properties)
        {
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite || propertyInfo.SetMethod == null || propertyInfo.SetMethod.IsPrivate)
                continue;
            yield return propertyInfo;
        }
    }
#if !NETSTANDARD1_6
    internal static bool IsCustomNonEnumerableType(this Type type)
    {
        var nullType = Nullable.GetUnderlyingType(type);
        if (nullType != null)
        {
            type = nullType;
        }
        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
        }
        return type != typeof(object)
               && Type.GetTypeCode(type) == TypeCode.Object
               && type != typeof(HttpFile)
               && type != typeof(Guid)
               && type.GetInterface(typeof(IEnumerable).Name) == null;
    }
#endif
}