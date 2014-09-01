namespace Cafemoca.CommandEditor.Utils
{
    internal static class StringChecker
    {
        public static char GetOpenBracket(char openBracket)
        {
            return GetOpenBracket(openBracket.ToString());
        }

        public static char GetCloseBracket(char closeBracket)
        {
            return GetCloseBracket(closeBracket.ToString());
        }

        public static char GetOpenBracket(string closeBracket)
        {
            if (closeBracket == ")") return '(';
            if (closeBracket == "}") return '{';
            if (closeBracket == "]") return '[';
            return '\0';
        }

        public static char GetCloseBracket(string closeBracket)
        {
            if (closeBracket == "(") return ')';
            if (closeBracket == "{") return '}';
            if (closeBracket == "[") return ']';
            return '\0';
        }
    }
}
