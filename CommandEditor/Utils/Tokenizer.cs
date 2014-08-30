using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafemoca.CommandEditor.Utils
{
    public static class Tokenizer
    {
        public static IEnumerable<Token> Tokenize(this string text, TokenizeType type = TokenizeType.Command)
        {
            if (text.IsEmpty())
            {
                text = string.Empty;
            }
            return (type == TokenizeType.All)
                ? text.TokenizeAll()
                : (type == TokenizeType.Command)
                    ? text.TokenizeCommand()
                    : text.TokenizeDataTag();
        }

        public static IEnumerable<Token> TokenizeCommand(this string text)
        {
            var cursor = 0;

            do
            {
                switch (text[cursor])
                {
                    case '<':
                        yield return new Token(TokenType.ScoreMin, "<", cursor);
                        break;
                    case '>':
                        yield return text.CheckNext(cursor, '<')
                            ? new Token(TokenType.ScoreSwaps, "><", cursor++)
                            : new Token(TokenType.ScoreMax, ">", cursor);
                        break;
                    case '+':
                        yield return text.CheckNext(cursor, '=')
                            ? new Token(TokenType.ScoreAdd, "+=", cursor++)
                            : new Token(TokenType.Literal, text.GetLiteral(TokenizeType.Command, ref cursor), cursor);
                        break;
                    case '-':
                        yield return text.CheckNext(cursor, '=')
                            ? new Token(TokenType.ScoreSubtract, "-=", cursor++)
                            : new Token(TokenType.Literal, text.GetLiteral(TokenizeType.Command, ref cursor), cursor);
                        break;
                    case '*':
                        yield return text.CheckNext(cursor, '=')
                           ? new Token(TokenType.ScoreMultiple, "*=", cursor++)
                           : new Token(TokenType.Asterisk, "*", cursor);
                        break;
                    case '%':
                        yield return text.CheckNext(cursor, '=')
                            ? new Token(TokenType.ScoreModulo, "%=", cursor++)
                            : new Token(TokenType.Literal, text.GetLiteral(TokenizeType.Command, ref cursor), cursor);
                        break;
                    case '/':
                        if (text.CheckNext(cursor, '/', '*'))
                        {
                            yield return new Token(TokenType.Comment, text.GetComments(ref cursor), cursor);
                        }
                        else
                        {
                            yield return (text.CheckNext(cursor, '='))
                                ? new Token(TokenType.ScoreDivide, "/=", cursor++)
                                : new Token(TokenType.Command, text.GetLiteral(TokenizeType.Command, ref cursor), cursor);
                        }
                        break;
                    case '{':
                        yield return new Token(TokenType.TagBlock, text.GetBlock('{', '}', ref cursor), cursor);
                        break;
                    case '[':
                        yield return new Token(TokenType.ArrayBlock, text.GetBlock('[', ']', ref cursor), cursor);
                        break;
                    case '@':
                        yield return new Token(TokenType.TargetSelector, text.GetLiteral(TokenizeType.Command, ref cursor), cursor);
                        break;
                    case '~':
                        yield return new Token(TokenType.LocationSelector, text.GetLiteral(TokenizeType.Command, ref cursor), cursor);
                        break;
                    case ',':
                        yield return new Token(TokenType.Comma, ",", cursor);
                        break;
                    case '"':
                        yield return new Token(TokenType.String, text.GetString('"', ref cursor), cursor);
                        break;
                    case '\t':
                    case ' ':
                    case '\r':
                    case '\n':
                        yield return new Token(TokenType.Blank, text.GetBlanks(ref cursor), cursor);
                        break;
                    default:
                        yield return new Token(TokenType.Literal, text.GetLiteral(TokenizeType.Command, ref cursor), cursor);
                        break;
                }
                cursor++;
            } while (cursor < text.Length);
        }

        public static IEnumerable<Token> TokenizeDataTag(this string text)
        {
            var cursor = 0;

            do
            {
                switch (text[cursor])
                {
                    case '/':
                        yield return text.CheckNext(cursor, '/', '*')
                            ? new Token(TokenType.Comment, text.GetComments(ref cursor), cursor)
                            : new Token(TokenType.Command, text.GetLiteral(TokenizeType.Block, ref cursor), cursor);
                        break;
                    case '=':
                        yield return new Token(TokenType.Equal, "=", cursor);
                        break;
                    case '!':
                        yield return new Token(TokenType.Exclamation, "!", cursor);
                        break;
                    case '(':
                        yield return new Token(TokenType.StringBlock, text.GetBlock('(', ')', ref cursor), cursor);
                        break;
                    case '{':
                        yield return new Token(TokenType.TagBlock, text.GetBlock('{', '}', ref cursor), cursor);
                        break;
                    case '[':
                        yield return new Token(TokenType.ArrayBlock, text.GetBlock('[', ']', ref cursor), cursor);
                        break;
                    case ',':
                        yield return new Token(TokenType.Comma, ",", cursor);
                        break;
                    case ':':
                        yield return new Token(TokenType.Colon, ":", cursor);
                        break;
                    case '\'':
                        yield return new Token(TokenType.String, text.GetString('\'', ref cursor), cursor);
                        break;
                    case '"':
                        yield return new Token(TokenType.String, text.GetString('"', ref cursor), cursor);
                        break;
                    case '\t':
                    case '\r':
                    case '\n':
                    case ' ':
                        yield return new Token(TokenType.Blank, text.GetBlanks(ref cursor), cursor);
                        break;
                    default:
                        yield return new Token(TokenType.Literal, text.GetLiteral(TokenizeType.Block, ref cursor), cursor);
                        break;
                }
                cursor++;
            } while (cursor < text.Length);
        }

        public static IEnumerable<Token> TokenizeAll(this string text)
        {
            var cursor = 0;

            do
            {
                switch (text[cursor])
                {
                    case '<':
                        yield return new Token(TokenType.ScoreMin, "<", cursor);
                        break;
                    case '>':
                        yield return text.CheckNext(cursor, '<')
                            ? new Token(TokenType.ScoreSwaps, "><", cursor++)
                            : new Token(TokenType.ScoreMax, ">", cursor);
                        break;
                    case '+':
                        yield return text.CheckNext(cursor, '=')
                            ? new Token(TokenType.ScoreAdd, "+=", cursor++)
                            : new Token(TokenType.Literal, text.GetLiteral(TokenizeType.All, ref cursor), cursor);
                        break;
                    case '-':
                        yield return text.CheckNext(cursor, '=')
                            ? new Token(TokenType.ScoreSubtract, "-=", cursor++)
                            : new Token(TokenType.Literal, text.GetLiteral(TokenizeType.All, ref cursor), cursor);
                        break;
                    case '*':
                        yield return text.CheckNext(cursor, '=')
                           ? new Token(TokenType.ScoreMultiple, "*=", cursor++)
                           : new Token(TokenType.Asterisk, "*", cursor);
                        break;
                    case '%':
                        yield return text.CheckNext(cursor, '=')
                            ? new Token(TokenType.ScoreModulo, "%=", cursor++)
                            : new Token(TokenType.Literal, text.GetLiteral(TokenizeType.All, ref cursor), cursor);
                        break;
                    case '/':
                        if (text.CheckNext(cursor, '/', '*'))
                        {
                            yield return new Token(TokenType.Comment, text.GetComments(ref cursor), cursor);
                        }
                        else
                        {
                            yield return (text.CheckNext(cursor, '='))
                                ? new Token(TokenType.ScoreDivide, "/=", cursor++)
                                : new Token(TokenType.Command, text.GetLiteral(TokenizeType.All, ref cursor), cursor);
                        }
                        break;
                    case '=':
                        yield return new Token(TokenType.Equal, "=", cursor);
                        break;
                    case '!':
                        yield return new Token(TokenType.Exclamation, "!", cursor);
                        break;
                    case '(':
                        yield return new Token(TokenType.OpenParenthesis, "(", cursor);
                        break;
                    case ')':
                        yield return new Token(TokenType.CloseParenthesis, ")", cursor);
                        break;
                    case '{':
                        yield return new Token(TokenType.OpenCurlyBrace, "{", cursor);
                        break;
                    case '}':
                        yield return new Token(TokenType.CloseCurlyBrace, "}", cursor);
                        break;
                    case '[':
                        yield return new Token(TokenType.OpenSquareBracket, "[", cursor);
                        break;
                    case ']':
                        yield return new Token(TokenType.CloseSquareBracket, "]", cursor);
                        break;
                    case '@':
                        yield return new Token(TokenType.TargetSelector, text.GetLiteral(TokenizeType.All, ref cursor), cursor);
                        break;
                    case '~':
                        yield return new Token(TokenType.LocationSelector, text.GetLiteral(TokenizeType.All, ref cursor), cursor);
                        break;
                    case ',':
                        yield return new Token(TokenType.Comma, ",", cursor);
                        break;
                    case ':':
                        yield return new Token(TokenType.Colon, ":", cursor);
                        break;
                    case '"':
                        yield return new Token(TokenType.String, text.GetString('"', ref cursor), cursor);
                        break;
                    case '\t':
                    case ' ':
                    case '\r':
                    case '\n':
                        yield return new Token(TokenType.Blank, text.GetBlanks(ref cursor), cursor);
                        break;
                    default:
                        yield return new Token(TokenType.Literal, text.GetLiteral(TokenizeType.All, ref cursor), cursor);
                        break;
                }
                cursor++;
            } while (cursor < text.Length);
        }

        public static string GetBlanks(this string text, ref int cursor)
        {
            const string blanks = "\t\r\n ";
            var begin = cursor;

            while (cursor < text.Length)
            {
                if (blanks.Contains(text[cursor]))
                {
                    cursor++;
                }
                else
                {
                    cursor--;
                    return text.Substring(begin, cursor - begin);
                }
            }

            return (cursor >= text.Length)
                ? text.Substring(begin)
                : string.Empty;
        }

        public static string GetComments(this string text, ref int cursor)
        {
            var begin = cursor++;
            var inBlockComment = (text[cursor++] == '*') ? true : false;

            while (cursor < text.Length)
            {
                switch (text[cursor])
                {
                    case '*':
                        if (inBlockComment)
                        {
                            if (text.GetNext(cursor) == '/')
                            {
                                cursor++;
                                return text.Substring(begin, cursor - begin);
                            }
                        }
                        break;
                    case '\r':
                    case '\n':
                        if (!inBlockComment)
                        {
                            return text.Substring(begin, cursor - begin);
                        }
                        break;
                }
                cursor++;
            }

            return (cursor >= text.Length)
                ? text.Substring(begin)
                : string.Empty;
        }

        public static string GetLiteral(this string text, TokenizeType type, ref int cursor)
        {
            var tokens = string.Empty;
            
            switch (type)
            {
                case TokenizeType.Command:
                    tokens = "{}[]=@~/\t\r\n ";
                    break;
                case TokenizeType.Block:
                    tokens = "(){}[],:;=\"\t\r\n ";
                    break;
                case TokenizeType.All:
                default:
                    tokens = "(){}[]<>+-*/%@~,:;!=\"\t\r\n ";
                    break;
            }

            var begin = cursor++;

            while (cursor < text.Length)
            {
                if (tokens.Contains(text[cursor]))
                {
                    cursor--;
                    return text.Substring(begin, cursor - begin + 1);
                }
                cursor++;
            }

            return (cursor >= text.Length)
                ? text.Substring(begin)
                : string.Empty;
        }

        public static string GetString(this string text, char quote, ref int cursor)
        {
            var begin = cursor++;

            while (cursor < text.Length)
            {
                if (text[cursor] == '\\')
                {
                    if (cursor > text.Length)
                    {
                        return text.Substring(begin);
                    }
                    if (text.CheckNext(cursor, quote) || text.CheckNext(cursor, '\\'))
                    {
                        cursor++;
                    }
                }
                else if (text[cursor] == quote)
                {
                    return text.Substring(begin, cursor - begin + 1);
                }
                cursor++;
            }

            return (cursor >= text.Length)
                ? text.Substring(begin)
                : string.Empty;
        }

        public static string GetBlock(this string text, char start, char end, ref int cursor)
        {
            var begin = cursor++;
            var stack = new Stack<char>();

            while (cursor < text.Length)
            {
                var cha = text[cursor];
                if (cha == start)
                {
                    stack.Push(cha);
                }
                if (cha == end)
                {
                    if (stack.Any())
                    {
                        stack.Pop();
                    }
                    else
                    {
                        return text.Substring(begin, cursor - begin + 1);
                    }
                }
                cursor++;
            }

            return (cursor >= text.Length)
                ? text.Substring(begin)
                : string.Empty;
        }
    }

    public enum TokenizeType
    {
        All,
        Command,
        Block,
    }
}
