using System.Linq;

namespace Cafemoca.CommandEditor.Utils
{
    public static class Escaping
    {
        public static string Escape(this string unescaped, char quote)
        {
            return unescaped.Replace("\\", "\\\\").Replace(quote.ToString(), "\\" + quote);
        }

        public static string Unescape(this string escaped, char quote)
        {
            return escaped.Replace("\\" + quote, quote.ToString()).Replace("\\\\", "\\");
        }

        public static string Quote(this string escaped, char quote)
        {
            return quote + escaped + quote;
        }

        public static string Quote(this string escaped, string quote)
        {
            return quote + escaped + quote;
        }

        public static string Enclose(this string unenclosed, char start, char end)
        {
            return start + unenclosed + end;
        }

        public static string Enclose(this string unenclosed, string start, string end)
        {
            return start + unenclosed + end;
        }

        public static string Unenclose(this string enclosed, char start, char end)
        {
            enclosed = enclosed.FirstOrDefault() == start
                ? enclosed.Remove(0, 1)
                : enclosed;

            enclosed = enclosed.LastOrDefault() == end
                ? enclosed.Remove(enclosed.Length - 1, 1)
                : enclosed;

            return enclosed;
        }
    }
}
