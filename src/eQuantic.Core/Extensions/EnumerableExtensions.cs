using System;
using System.Collections.Generic;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for IEnumerable collections.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Executes the specified action on each element of the enumerable collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="enumeration">The enumerable collection to iterate over.</param>
    /// <param name="action">The action to execute on each element.</param>
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        if (enumeration == null || action == null)
            return;
        foreach (T item in enumeration)
            action(item);
    }

    /// <summary>
    /// Returns distinct elements from a sequence by using a specified key selector function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>An IEnumerable&lt;TSource&gt; that contains distinct elements from the source sequence.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> knownKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (knownKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}