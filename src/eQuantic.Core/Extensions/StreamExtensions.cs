using System.IO;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for Stream operations.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Converts the stream content to a byte array.
    /// </summary>
    /// <param name="input">The stream to convert.</param>
    /// <returns>A byte array containing the stream content.</returns>
    public static byte[] ToArray(this Stream input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}