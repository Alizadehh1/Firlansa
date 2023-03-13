using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Firlansa.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        static public string HtmlToPlainText(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;

            html = Regex.Replace(html, @"(<[^>]*>|\r\n|\r|\n)", "");
            html = Regex.Replace(html, @"\s+", " ");
            return html;
        }

        static public string ToEllipse(this string text,int length = 50)
        {
            if (string.IsNullOrEmpty(text) || length>=text.Length)
                return text;

            return $"{text.Substring(0, length)}...";
        }

        static public string ToSlug(this string context)
        {
            if (string.IsNullOrWhiteSpace(context))
                return null;

            //c# &&&&& sql => csharp-and-sql

            var replaceSet = new Dictionary<string, string>() {
                {"Ü|ü", "u"},
                {"İ|I|ı", "i"},
                {"Ş|ş", "s"},
                {"Ö|ö", "o"},
                {"Ç|ç", "c"},
                {"Ğ|ğ", "g"},
                {"Ə|ə", "e"},
                {"#", "sharp"},
                {@"(\?|/|\|\.|'|`|%|\*|!|@|\+)+", ""},
                {@"\&+", "and"},
                {@"[^a-z0-9]+", "-"},
                };

            return replaceSet.Aggregate(context, (i, m) => Regex.Replace(i, m.Key, m.Value, RegexOptions.IgnoreCase))
                .ToLower();
        }
    }
}
