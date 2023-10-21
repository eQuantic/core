using System.IO;

namespace eQuantic.Core.Extensions;

public static class StreamExtensions
{
    public static byte[] ToArray(this Stream input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}