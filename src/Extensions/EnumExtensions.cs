using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Enum"/> types.
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// Gets the description of an enum value from the <see cref="DescriptionAttribute"/> if available, otherwise returns the enum name.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The description of the enum value, or the enum name if no description is available.</returns>
    public static string GetDescription(this Enum value)
    {
#if NETSTANDARD1_6
            FieldInfo field = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());
#else
        FieldInfo field = value.GetType().GetField(value.ToString());
#endif
        var attribute = field.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }

    /// <summary>
    /// Gets a list of all values for the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="en">The enum instance (used for type inference).</param>
    /// <returns>A list containing all values of the specified enum type.</returns>
    public static List<T> GetList<T>(this Enum en)
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    public static IEnumerable<Enum> GetFlags(this Enum value)
    {
        return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
    }

    public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
    {
        return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
    }

    private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
    {
        ulong bits = Convert.ToUInt64(value);
        List<Enum> results = new List<Enum>();
        for (int i = values.Length - 1; i >= 0; i--)
        {
            ulong mask = Convert.ToUInt64(values[i]);
            if (i == 0 && mask == 0L)
                break;
            if ((bits & mask) == mask)
            {
                results.Add(values[i]);
                bits -= mask;
            }
        }
        if (bits != 0L)
            return Enumerable.Empty<Enum>();
        if (Convert.ToUInt64(value) != 0L)
            return results.Reverse<Enum>();
        if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
            return values.Take(1);
        return Enumerable.Empty<Enum>();
    }

    private static IEnumerable<Enum> GetFlagValues(Type enumType)
    {
        ulong flag = 0x1;
        foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
        {
            ulong bits = Convert.ToUInt64(value);
            if (bits == 0L)
                //yield return value;
                continue; // skip the zero value
            while (flag < bits) flag <<= 1;
            if (flag == bits)
                yield return value;
        }
    }
}