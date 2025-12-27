using System;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for Guid operations.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Converts a Guid to its short string representation using ShortGuid encoding.
    /// </summary>
    /// <param name="guid">The Guid to convert.</param>
    /// <returns>A ShortGuid representation of the Guid.</returns>
    public static ShortGuid ToShort(this Guid guid)
    {
        return new ShortGuid(guid);
    }
}