using System;
using System.Linq;
using System.Text;

namespace Cafemoca.CommandEditor.Utils
{
    public struct Token
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }

        public int Index { get; set; }

        public Token(TokenType type, int index)
            : this()
        {
            this.Type = type;
            this.Value = null;
            this.Index = index;
        }

        public Token(TokenType type, string value, int index)
            : this()
        {
            this.Type = type;
            this.Value = value;
            this.Index = index;
        }
    }

    public static class TokenExtensions
    {
        public static bool IsMatchLiteral(this Token token, string value, bool ignoreCase = true)
        {
            return (token.Type == TokenType.Literal)
                ? ignoreCase
                    ? token.Value.Equals(value, StringComparison.OrdinalIgnoreCase)
                    : token.Value == value
                : false;
        }

        public static bool ContainsLiteral(this Token token, string[] value, bool ignoreCase = true)
        {
            var comparer = ignoreCase
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal;
            return (token.Type == TokenType.Literal)
                ? value.Contains(token.Value, comparer)
                : false;
        }

        public static bool IsMatchType(this Token token, TokenType type)
        {
            return token.Type == type;
        }

        public static bool ContainsType(this Token token, params TokenType[] types)
        {
            return types.Any(x => token.IsMatchType(x));
        }

        public static bool IsEmpty(this Token token)
        {
            var value = token as Token?;
            return value == null;
        }
    }

    public enum TokenType
    {
        // common
        Literal,
        String,
        Equal,

        // command
        Command,
        LocationSelector,
        TargetSelector,
        TagBlock,
        ArrayBlock,

        // scoreboard
        ScoreMin,
        ScoreMax,
        ScoreSwaps,
        ScoreAdd,
        ScoreSubtract,
        ScoreMultiple,
        ScoreDivide,
        ScoreModulo,
        Asterisk,

        // tag
        Comma,
        Colon,
        OpenParenthesis,
        CloseParenthesis,
        OpenSquareBracket,
        CloseSquareBracket,
        OpenCurlyBrace,
        CloseCurlyBrace,
        Exclamation,
        StringBlock,

        // skip
        Blank,
        Comment,
    }
}
