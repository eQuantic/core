using System;
using System.Collections.Generic;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for IList collections.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Shuffles the elements of the list in place using the Fisher-Yates shuffle algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}