using System;
using System.Security.Cryptography;
using System.Text;

namespace eQuantic.Core.Security.Encryption;

/// <summary>
/// The encryptor.
/// </summary>
public class Encryptor : IEncrypting
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