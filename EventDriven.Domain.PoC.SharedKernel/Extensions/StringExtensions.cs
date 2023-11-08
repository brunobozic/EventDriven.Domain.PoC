using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EventDriven.Domain.PoC.SharedKernel.Extensions
{
    public static class StringExtensions
    {
        public static int CalcLevenshteinDistance(this string s, string t)
        {
            if (s == null) s = string.Empty;

            if (t == null) t = string.Empty;

            var strip = new Func<string, string>(x => x.ToLower().Replace(" ", "").Replace("-", ""));

            s = strip(s);
            t = strip(t);

            var n = s.Length;
            var m = t.Length;

            var d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0) return m;

            if (m == 0) return n;

            // Step 2
            for (var i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (var i = 1; i <= n; i++)
                //Step 4
                for (var j = 1; j <= m; j++)
                {
                    // Step 5
                    var cost = t[j - 1] == s[i - 1] ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }

            // Step 7
            return d[n, m];
        }

        public static string CleanUpSerilogProperties(this string serilogProperty)
        {
            var returnValue = string.Empty;

            serilogProperty = serilogProperty.Replace("[", "");
            serilogProperty = serilogProperty.Replace("]", "");
            serilogProperty = serilogProperty.Replace(" ", "");
            serilogProperty = serilogProperty.Replace(",", "");
            serilogProperty = serilogProperty.Replace("\"", "");

            returnValue = serilogProperty;

            return returnValue;
        }

        public static bool ContainsDigits(this string str)
        {
            return str.Any(c => char.IsDigit(c));
        }

        public static Stream GetStreamFromString(this string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(str);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public static bool IsCamelCased(this string str)
        {
            var rx = new Regex(@"/^[a-z][a-z0-9]+(?:[A-Z][a-z0-9]+)*$/");
            return rx.IsMatch(str);
        }

        public static int WordCount(this string str)
        {
            return str.Split(new[] { ' ', '.', '?' },
                StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}