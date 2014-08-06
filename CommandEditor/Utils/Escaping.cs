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
    }
}
