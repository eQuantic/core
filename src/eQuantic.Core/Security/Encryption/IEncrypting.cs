namespace eQuantic.Core.Security.Encryption;

/// <summary>
/// Defines methods for encryption and random string generation.
/// </summary>
public interface IEncrypting
{
    /// <summary>
    /// Encodes a password using a hashing algorithm.
    /// </summary>
    /// <param name="password">The password to encode.</param>
    /// <returns>The encoded password as a string.</returns>
    string Encode(string password);

    /// <summary>
    /// Generates a random string of the specified length.
    /// </summary>
    /// <param name="length">The length of the random string to generate.</param>
    /// <returns>A random string.</returns>
    string Random(int length);
}