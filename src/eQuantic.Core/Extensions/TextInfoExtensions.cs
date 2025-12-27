using System;
using System.Globalization;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for TextInfo class.
/// </summary>
public static class TextInfoExtensions
{
#if NETSTANDARD1_6
        /// <summary>
        /// Converts the specified string to title case (except for words that are entirely in uppercase, which are considered to be acronyms).
        /// </summary>
        /// <param name="textInfo">The TextInfo instance.</param>
        /// <param name="str">The string to convert to title case.</param>
        /// <returns>The specified string converted to title case.</returns>
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