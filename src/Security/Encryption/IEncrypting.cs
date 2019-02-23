namespace eQuantic.Core.Security.Encryption
{
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

        string Random(int length);
    }
}
