using System;
using System.Security.Cryptography;
using System.Text;

namespace eQuantic.Core.Security.Encryption;

/// <summary>
/// Provides encryption functionality using MD5 hashing and random string generation.
/// </summary>
public class Encryptor : IEncrypting
{
    /// <summary>
    /// Encodes a password using MD5 hashing algorithm.
    /// </summary>
    /// <param name="password">The password to encode.</param>
    /// <returns>The MD5 hash of the password as a hexadecimal string.</returns>
    public string Encode(string password)
    {
        var md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(password);
        byte[] hash = md5.ComputeHash(inputBytes);
        var sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Generates a random string of the specified length using alphanumeric characters.
    /// </summary>
    /// <param name="length">The length of the random string to generate.</param>
    /// <returns>A random string containing letters and digits.</returns>
    public string Random(int length)
    {
        string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        string res = "";
        var rnd = new Random();
        while (0 < length--)
            res += valid[rnd.Next(valid.Length)];
        return res;
    }
}