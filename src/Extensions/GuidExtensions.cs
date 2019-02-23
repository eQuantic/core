using System;

namespace eQuantic.Core.Extensions
{
    public static class GuidExtensions
    {
        public static ShortGuid ToShort(this Guid guid)
        {
            return new ShortGuid(guid);
        }
    }
}
