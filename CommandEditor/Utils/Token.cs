using System;

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
        internal static bool IsMatchLiteral(this Token token, string value, bool ignoreCase = true)
        {
            return (token.Type != TokenType.Literal)
                ? false
                : ignoreCase
                    ? token.Value.Equals(value, StringComparison.OrdinalIgnoreCase)
                    : token.Value == value;
        }
    }

    public enum TokenType
    {
        Blank,
        Comment,
        AtMark,
        Period,
        Comma,
        Collon,
        SemiCollon,
        Exclamation,
        Literal,
        Sharp,
        And,
        Or,
        Plus,
        Minus,
        Asterisk,
        Slash,
        Percent,
        Equal,
        OperatorLocation,
        OperatorTarget,
        ScoreMin,
        ScoreMax,
        ScoreSwaps,
        ScoreAdd,
        ScoreSubtract,
        ScoreMultiple,
        ScoreDivide,
        ScoreModulo,
        OpenParenthesis,
        CloseParenthesis,
        OpenSquareBracket,
        CloseSquareBracket,
        OpenCurlyBrace,
        CloseCurlyBrace,
        String,
        ExternalCommand,
    }
}
