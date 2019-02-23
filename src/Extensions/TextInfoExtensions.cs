using System;
using System.Globalization;

namespace eQuantic.Core.Extensions
{
    public static class TextInfoExtensions
    {
#if NETSTANDARD1_6
        public static string ToTitleCase(this TextInfo textInfo, string str)
        {
            var tokens = str.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }
#endif
    }
}