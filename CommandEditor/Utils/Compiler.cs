using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cafemoca.CommandEditor.Utils
{
    public static class Compiler
    {
        #region 特定のトークンが連続した場合に半角スペースを挿入する

        private static readonly TokenType[] spaceBefore = new[]
        {
            TokenType.Literal,
            TokenType.OperatorLocation,
            TokenType.OperatorTarget,
            TokenType.CloseSquareBracket,
            TokenType.String,
            TokenType.ScoreAdd,
            TokenType.ScoreDivide,
            TokenType.ScoreMax,
            TokenType.ScoreMin,
            TokenType.ScoreModulo,
            TokenType.ScoreMultiple,
            TokenType.ScoreSubtract,
            TokenType.ScoreSwaps,
            TokenType.Asterisk,
        };

        private static readonly TokenType[] spaceAfter = new[]
        {
            TokenType.Literal,
            TokenType.OperatorLocation,
            TokenType.OperatorTarget,
            TokenType.OpenCurlyBrace,
            TokenType.String,
            TokenType.ScoreAdd,
            TokenType.ScoreDivide,
            TokenType.ScoreMax,
            TokenType.ScoreMin,
            TokenType.ScoreModulo,
            TokenType.ScoreMultiple,
            TokenType.ScoreSubtract,
            TokenType.ScoreSwaps,
            TokenType.Slash,
            TokenType.Asterisk,
        };

        #endregion

        public static string Compile(this IEnumerable<Token> tokens,
            EscapeModeValue escapeMode = EscapeModeValue.New)
        {
            var result = string.Empty;

            tokens = tokens.Where(t => t.Type != TokenType.Blank && t.Type != TokenType.Comment);

            var lastBeforeToken = new Token();
            var lastToken = new Token();

            var escapeLevel = 0;
            var stackCurlyBracket = new Stack<bool>();
            var stackParenthesis = new Stack<bool>();

            foreach (var token in tokens)
            {
                var value = token.Value;
                if (spaceAfter.Contains(token.Type) && spaceBefore.Contains(lastToken.Type))
                {
                    result += " ";
                }

                switch (token.Type)
                {
                    case TokenType.OpenCurlyBrace:
                        if ((lastBeforeToken.Type == TokenType.Literal) &&
                            (lastToken.Type == TokenType.Collon))
                        {
                            switch (lastBeforeToken.Value)
                            {
                                case "Text1":
                                case "Text2":
                                case "Text3":
                                case "Text4":
                                case "value":
                                    stackCurlyBracket.Push(true);
                                    value = GetRepeatedEscape(escapeLevel, escapeMode) + "\"" + value;
                                    escapeLevel++;
                                    break;
                                default:
                                    stackCurlyBracket.Push(false);
                                    break;
                            }
                        }
                        else
                        {
                            stackCurlyBracket.Push(false);
                        }
                        break;
                    case TokenType.String:
                        if (escapeMode == EscapeModeValue.New)
                        {
                            for (int i = 0; i < escapeLevel; i++)
                            {
                                value = value.Escape('"');
                            }
                        }
                        else if (escapeMode == EscapeModeValue.Old)
                        {
                            value = value
                                .Replace("\\", GetRepeatedEscape(escapeLevel, escapeMode))
                                .Replace("\"", GetRepeatedEscape(escapeLevel, escapeMode) + "\"");
                        }
                        break;
                    case TokenType.CloseCurlyBrace:
                        if (lastToken.Type == TokenType.Comma)
                        {
                            result = result.TrimEnd(',');
                        }
                        if (stackCurlyBracket.Count > 0)
                        {
                            if (stackCurlyBracket.Pop())
                            {
                                escapeLevel--;
                                value = value + GetRepeatedEscape(escapeLevel, escapeMode) + "\"";
                            }
                        }
                        break;
                    case TokenType.OpenParenthesis:
                        if (lastBeforeToken.Type == TokenType.Literal && lastBeforeToken.Value == "tellraw")
                        {
                            stackParenthesis.Push(false);
                        }
                        else
                        {
                            stackParenthesis.Push(true);
                            value = GetRepeatedEscape(escapeLevel, escapeMode) + "\"";
                            escapeLevel++;
                        }
                        break;
                    case TokenType.CloseParenthesis:
                        if (stackParenthesis.Count > 0)
                        {
                            if (stackParenthesis.Pop())
                            {
                                escapeLevel--;
                                value = GetRepeatedEscape(escapeLevel, escapeMode) + "\"";
                            }
                        }
                        break;
                    case TokenType.ExternalCommand:
                        continue;
                    default:
                        break;
                }

                lastBeforeToken = lastToken;
                lastToken = token;

                result += value;
            }

            return result;
        }

        private static string GetRepeatedEscape(int escapeLevel, EscapeModeValue escapeMode)
        {
            if (escapeLevel <= 0)
            {
                return string.Empty;
            }
            if (escapeLevel == 1)
            {
                return "\\";
            }
            var count = escapeMode == EscapeModeValue.New
                ? (int) Math.Pow(2, escapeLevel) - 1
                : (int) Math.Pow(2, escapeLevel - 1);

            var b = new StringBuilder(count);
            Enumerable.Range(1, count).ForEach(_ => b.Append("\\"));
            return b.ToString();
        }
    }

    public enum EscapeModeValue
    {
        New,
        Old,
    }
}
