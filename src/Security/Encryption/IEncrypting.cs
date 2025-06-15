namespace eQuantic.Core.Security.Encryption;

/// <summary>
/// The Encrypting interface.
/// </summary>
public interface IEncrypting
{
    /// <summary>
    /// The encode.
    /// </summary>
    /// <param name="password">
    /// The source.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    string Encode(string password);

    /// <summary>
    /// Generates a random string of the specified length.
    /// </summary>
    /// <param name="length">The length of the random string to generate.</param>
    /// <returns>A random string.</returns>
    string Random(int length);
}