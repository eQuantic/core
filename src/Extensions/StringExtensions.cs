using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using eQuantic.Core.Constants;

namespace eQuantic.Core.Extensions;

public static class StringExtensions
{
    private static readonly Regex Tags = new Regex(@"<[^>]+?>", RegexOptions.Multiline);

    //add characters that are should not be removed to this regex
    private static readonly Regex NotOkCharacter = new Regex(@"[^\w;&#@.:/\\?=|%!() -]");

    public static string LeftOf(this string src, char c)
    {
        string ret = src;
        int idx = src.IndexOf(c);
        if (idx != -1)
        {
            ret = src.Substring(0, idx);
        }

        return ret;
    }

    public static string LeftOf(this string src, char c, int n)
    {
        string ret = src;
        int idx = -1;
        while (n > 0)
        {
            idx = src.IndexOf(c, idx + 1);
            if (idx == -1)
            {
                break;
            }
        }

        if (idx != -1)
        {
            ret = src.Substring(0, idx);
        }

        return ret;
    }

    public static string RightOf(this string src, char c)
    {
        string ret = String.Empty;
        int idx = src.IndexOf(c);
        if (idx != -1)
        {
            ret = src.Substring(idx + 1);
        }

        return ret;
    }

    public static string RightOf(this string src, char c, int n)
    {
        string ret = String.Empty;
        int idx = -1;
        while (n > 0)
        {
            idx = src.IndexOf(c, idx + 1);
            if (idx == -1)
            {
                break;
            }

            --n;
        }

        if (idx != -1)
        {
            ret = src.Substring(idx + 1);
        }

        return ret;
    }

    public static string LeftOfRightmostOf(this string src, char c)
    {
        string ret = src;
        int idx = src.LastIndexOf(c);
        if (idx != -1)
        {
            ret = src.Substring(0, idx);
        }

        return ret;
    }

    public static string RightOfRightmostOf(this string src, char c)
    {
        string ret = String.Empty;
        int idx = src.LastIndexOf(c);
        if (idx != -1)
        {
            ret = src.Substring(idx + 1);
        }

        return ret;
    }

    public static string Between(this string src, char start, char end)
    {
        string ret = String.Empty;
        int idxStart = src.IndexOf(start);
        if (idxStart != -1)
        {
            ++idxStart;
            int idxEnd = src.IndexOf(end, idxStart);
            if (idxEnd != -1)
            {
                ret = src.Substring(idxStart, idxEnd - idxStart);
            }
        }

        return ret;
    }

    public static int Count(this string src, char find)
    {
        int ret = 0;
        foreach (char s in src)
        {
            if (s == find)
            {
                ++ret;
            }
        }

        return ret;
    }

    public static char Rightmost(this string src)
    {
        char c = '\0';
        if (src.Length > 0)
        {
            c = src[src.Length - 1];
        }

        return c;
    }

    public static string Multiply(this string src, int multiplier)
    {
        return String.Concat(Enumerable.Repeat(src, multiplier));
    }
#if !NETSTANDARD1_6
    public static string RemoveDiacritics(this string text)
    {
        return string.Concat(
            text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                             UnicodeCategory.NonSpacingMark)
        ).Normalize(NormalizationForm.FormC);
    }
#endif
    public static string RemoveSpecialCharacters(this string src, char replacement = '_',
        params char[] charsToRemove)
    {
        string chars = "¹²³ÄÅÁÂÀÃäáâàãªÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõºÜÚÛüúûùÇç !@#$%¨´&*()?{}][/" +
                       (charsToRemove != null ? new string(charsToRemove) : "");
        string replacements = "123AAAAAAaaaaaaEEEEeeeeIIIIiiiiOOOOOooooooUUUuuuuCc";
        string r = replacement.ToString().Multiply(chars.Length - replacements.Length);

        replacements += r;

        for (int i = 0; i < chars.Length; i++)
            src = src.Replace(chars[i].ToString(), replacements[i].ToString()).Trim();
        return src;
    }
#if !NETSTANDARD1_6
    public static string CapitalizeName(this string name)
    {
        return CapitalizeName(name, Thread.CurrentThread.CurrentCulture);
    }
#endif
#if NETSTANDARD1_6
        public static string CapitalizeName(this string name)
        {
            return CapitalizeName(name, null);
        }
#endif
    public static string CapitalizeName(this string name, CultureInfo cultureInfo)
    {
        var textInfo = cultureInfo?.TextInfo;
        var sout = new StringBuilder();
        var words = name.ToLower().Trim().Split(' ');
        foreach (var s in words)
        {
            if (!(s.Equals("e") ||
                  s.Equals("de") ||
                  s.Equals("da") ||
                  s.Equals("do") ||
                  s.Equals("dos") ||
                  s.Equals("das")))
            {
                sout.Append(textInfo?.ToTitleCase(s) ?? s.ToTitleCase());
            }
            else
            {
                sout.Append(s);
            }

            sout.Append(" ");
        }

        return sout.ToString().Trim();
    }

    public static string UnHtml(this string html)
    {
        html = WebUtility.UrlDecode(html);
        html = WebUtility.HtmlDecode(html);

        html = RemoveTag(html, "<!--", "-->");
        html = RemoveTag(html, "<script", "</script>");
        html = RemoveTag(html, "<style", "</style>");

        //replace matches of these regexes with space
        html = Tags.Replace(html, " ");
        html = NotOkCharacter.Replace(html, " ");
        html = SingleSpacedTrim(html);

        return html;
    }

    private static string RemoveTag(this string html, string startTag, string endTag)
    {
        Boolean bAgain;
        do
        {
            bAgain = false;
            Int32 startTagPos = html.IndexOf(startTag, 0, StringComparison.CurrentCultureIgnoreCase);
            if (startTagPos < 0)
                continue;
            Int32 endTagPos = html.IndexOf(endTag, startTagPos + 1, StringComparison.CurrentCultureIgnoreCase);
            if (endTagPos <= startTagPos)
                continue;
            html = html.Remove(startTagPos, endTagPos - startTagPos + endTag.Length);
            bAgain = true;
        } while (bAgain);

        return html;
    }

    public static string SingleSpacedTrim(this string inString)
    {
        StringBuilder sb = new StringBuilder();
        Boolean inBlanks = false;
        foreach (Char c in inString)
        {
            switch (c)
            {
                case '\r':
                case '\n':
                case '\t':
                case ' ':
                    if (!inBlanks)
                    {
                        inBlanks = true;
                        sb.Append(' ');
                    }

                    continue;
                default:
                    inBlanks = false;
                    sb.Append(c);
                    break;
            }
        }

        return sb.ToString().Trim();
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source.IndexOf(toCheck, comp) >= 0;
    }

    public static string ToTitleCase(this string str)
    {
        var tokens = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
        }

        return string.Join(" ", tokens);
    }

    public static string SplitOnCapitals(this string str, char separator = ' ')
    {
        var newstring = "";
        foreach (char t in str)
        {
            if (char.IsUpper(t))
                newstring += separator;
            newstring += t.ToString();
        }

        return newstring;
    }

    /// <summary>
    /// Trims the end using the specified input
    /// </summary>
    /// <param name="input">The input</param>
    /// <param name="suffixToRemove">The suffix to remove</param>
    /// <param name="comparisonType">The comparison type</param>
    /// <returns>The input</returns>
    public static string TrimEnd(this string input, string suffixToRemove,
        StringComparison comparisonType = StringComparison.CurrentCulture)
    {
        if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
        {
            return input.Substring(0, input.Length - suffixToRemove.Length);
        }

        return input;
    }

    /// <summary>
    /// Breaks the lines to array using the specified text
    /// </summary>
    /// <param name="text">The text</param>
    /// <returns>The string array</returns>
    public static string[] BreakLinesToArray(this string text)
    {
        return text.Split(StringConstants.BreakLines, StringSplitOptions.RemoveEmptyEntries);
    }


    /// <summary>  
    /// Removes all accents from the input string.  
    /// </summary>  
    /// <param name="text">The input string.</param>  
    /// <returns></returns>  
    public static string RemoveAccents(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        text = text.Normalize(NormalizationForm.FormD);
        var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC);
    }

    /// <summary>  
    /// Turn a string into a slug by removing all accents,   
    /// special characters, additional spaces, substituting   
    /// spaces with hyphens & making it lower-case.  
    /// </summary>  
    /// <param name="phrase">The string to turn into a slug.</param>  
    /// <returns></returns>  
    public static string Slugify(this string? phrase)
    {
        if (string.IsNullOrEmpty(phrase)) return string.Empty;

        // Remove all accents and make the string lower case.  
        var output = phrase.RemoveAccents().ToLower();

        // Remove all special characters from the string.  
        output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

        // Remove all additional spaces in favour of just one.  
        output = Regex.Replace(output, @"\s+", " ").Trim();

        // Replace all spaces with the hyphen.  
        output = Regex.Replace(output, @"\s", "-");

        // Return the slug.  
        return output;
    }

    /// <summary>
    /// Validate IPv4
    /// </summary>
    /// <param name="ipString"></param>
    /// <returns></returns>
    public static bool IsValidIPv4(this string ipString)
    {
        if (string.IsNullOrWhiteSpace(ipString))
        {
            return false;
        }

        var splitValues = ipString.Split('.');
        return splitValues.Length == 4 && splitValues.All(r => byte.TryParse(r, out _));
    }
}