using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
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

        public static bool CheckNext(this string text, int index, char character)
        {
            index++;
            return index >= 0 && text.Length > index && text[index] == character;
        }

        public static bool CheckNext(this string text, int index, params char[] characters)
        {
            index++;
            return index >= 0 && text.Length > index && characters.Contains(text[index]);
        }

        public static char GetNext(this string text, int index)
        {
            index++;
            return index >= 0 && text.Length > index ? text[index] : '\0';
        }

        public static bool CheckPrevious(this string text, int index, char character)
        {
            return index >= 0 && text.Length > index + 1 && text[index] == character;
        }

        public static bool CheckPrevious(this string text, int index, IEnumerable<char> characters)
        {
            return index >= 0 && text.Length > index + 1 && characters.Contains(text[index]);
        }

        public static char GetPrevious(this string text, int index)
        {
            return index >= 0 && text.Length > index + 1 ? text[index] : '\0';
        }

        public static bool CheckBothSide(this string text, int index, char previous, char next)
        {
            return index >= 0 &&
                   text.Length > index + 1 &&
                   previous == text[index] &&
                   next == text[index + 1];
        }
    }
}
