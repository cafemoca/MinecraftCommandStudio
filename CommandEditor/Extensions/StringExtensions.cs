using System.Linq;
using System.Text;

namespace Cafemoca.CommandEditor.Extensions
{
    public static class StringExtensions
    {
        public static char ToChar(this string value)
        {
            return value.ToCharArray()[0];
        }

        public static string Repeat(this string value, int count)
        {
            if (count <= 0)
            {
                return string.Empty;
            }
            if (count == 1)
            {
                return value;
            }
            var b = new StringBuilder(value.Length * count);
            Enumerable.Range(1, count).ForEach(_ => b.Append(value));

            return b.ToString();
        }
    }
}
