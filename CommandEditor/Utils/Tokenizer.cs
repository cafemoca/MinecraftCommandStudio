using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafemoca.CommandEditor.Utils
{
    public static class Tokenizer
    {
        public static IEnumerable<Token> Tokenize(this string text)
        {
            if (text.IsEmpty())
            {
                yield break;
            }

            var cursor = 0;
            do
            {
                switch (text[cursor])
                {
                    case '<':
                        yield return new Token(TokenType.ScoreMin, "<", cursor);
                        break;
                    case '>':
                        yield return (text.CheckNext(cursor, '<'))
                            ? new Token(TokenType.ScoreSwaps, "><", cursor++)
                            : new Token(TokenType.ScoreMax, ">", cursor);
                        break;
                    case '+':
                        yield return (text.CheckNext(cursor, '='))
                            ? new Token(TokenType.ScoreAdd, "+=", cursor++)
                            : new Token(TokenType.Plus, "+", cursor);
                        break;
                    case '-':
                        if (text.CheckNext(cursor, '='))
                        {
                            yield return new Token(TokenType.ScoreSubtract, "-=", cursor++);
                        }
                        else
                        {
                            var begin = cursor;
                            yield return new Token(TokenType.Literal, text.GetLiteral(ref begin, ref cursor), cursor);
                        }
                        break;
                    case '*':
                         yield return (text.CheckNext(cursor, '='))
                            ? new Token(TokenType.ScoreMultiple, "*=", cursor++)
                            : new Token(TokenType.Asterisk, "*", cursor);
                        break;
                    case '/':
                        if (text.CheckNext(cursor, '='))
                        {
                            yield return new Token(TokenType.ScoreDivide, "/=", cursor++);
                        }
                        else if (text.CheckNext(cursor, "/*"))
                        {
                            yield return new Token(TokenType.Comment, text.GetComments(ref cursor), cursor);
                        }
                        else
                        {
                            yield return new Token(TokenType.Slash, "/", cursor);
                        }
                        break;
                    case '%':
                        yield return (text.CheckNext(cursor, '='))
                            ? new Token(TokenType.ScoreModulo, "%=", cursor++)
                            : new Token(TokenType.Percent, "%", cursor);
                        break;
                    case '=':
                        yield return new Token(TokenType.Equal, "=", cursor);
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
                    case ',':
                        yield return new Token(TokenType.Comma, ",", cursor);
                        break;
                    case ':':
                        yield return new Token(TokenType.Colon, ":", cursor);
                        break;
                    case '_':
                        if (cursor + 1 < text.Length)
                        {
                            var begin = cursor;
                            var literal = text.GetLiteral(ref begin, ref cursor);
                            yield return literal != "_" //literal.Any(x => x != '_')
                                ? new Token(TokenType.Literal, literal, cursor)
                                : new Token(TokenType._Space, " ", cursor);
                        }
                        break;
                    case ';':
                        yield return new Token(TokenType._DeleteSpace, "", cursor);
                        break;
                    case '!':
                        yield return new Token(TokenType.Exclamation, "!", cursor);
                        break;
                    case '@':
                        if (text.CheckNext(cursor, "pear"))
                        {
                            var next = text.GetNext(cursor);
                            yield return new Token(TokenType.OperatorTarget, "@" + next, cursor++);
                            break;
                        }
                        yield return new Token(TokenType.AtMark, "@", cursor);
                        break;
                    case '~':
                        if (cursor + 1 < text.Length)
                        {
                            var begin = ++cursor;
                            var location = text.GetLiteral(ref begin, ref cursor);
                            yield return new Token(TokenType.OperatorLocation, "~" + location, begin - 1);
                            break;
                        }
                        yield return new Token(TokenType.OperatorLocation, "~", cursor);
                        break;
                    case '#':
                        if (cursor + 1 < text.Length)
                        {
                            var begin = ++cursor;
                            var command = text.GetLiteral(ref begin, ref cursor);
                            if (!command.IsEmpty())
                            {
                                yield return new Token(TokenType.Extend, command, begin - 1);
                                break;
                            }
                        }
                        yield return new Token(TokenType.Sharp, "#", cursor);
                        break;
                    case '"':
                        yield return new Token(TokenType.String, GetString(text, ref cursor), cursor);
                        break;
                    case '\t':
                    case ' ':
                    case '　':
                    case '\r':
                    case '\n':
                        yield return new Token(TokenType.Blank, text.GetBlanks(ref cursor), cursor);
                        break;
                    default:
                        {
                            var begin = cursor;
                            var literal = text.GetLiteral(ref begin, ref cursor);
                            yield return new Token(TokenType.Literal, literal, begin);
                        }
                        break;
                }
                cursor++;
            } while (cursor < text.Length);
        }

        public static string GetBlanks(this string text, ref int cursor)
        {
            var begin = cursor;
            do
            {
                if ("\t\r\n 　".Contains(text[cursor]))
                {
                    cursor++;
                }
                else
                {
                    cursor--;
                    return text.Substring(begin, cursor - begin);
                }
            }
            while (cursor < text.Length);
            return "";
        }

        public static string GetComments(this string text, ref int cursor)
        {
            var begin = cursor;

            var inLineComment = false;
            var inBlockComment = false;

            var next = text[++cursor];
            switch (next)
            {
                case '*':
                    inBlockComment = true;
                    break;
                case '/':
                    inLineComment = true;
                    break;
                default:
                    return "";
            }

            do
            {
                switch (text[cursor])
                {
                    case '*':
                        if (inBlockComment)
                        {
                            if (text.Length <= cursor || text.GetNext(cursor) == '/')
                            {
                                cursor++;
                                return text.Substring(begin, cursor - begin);
                            }
                        }
                        break;
                    case '\n':
                        if (inLineComment)
                        {
                            return text.Substring(begin, cursor - begin);
                        }
                        break;
                }
                cursor++;
            }
            while (cursor < text.Length);

            return "";
        }

        public static string GetLiteral(this string text, ref int begin, ref int cursor)
        {
            const string tokens = "<>+*/%(){}[],:;=!#@~\"\t\r\n 　";
            var literal = "";

            do
            {
                if (tokens.Contains(text[cursor]))
                {
                    literal = text.Substring(begin, cursor - begin);
                    cursor--;
                    break;
                }
                cursor++;
            }
            while (cursor < text.Length);
            if (cursor == text.Length)
            {
                literal = text.Substring(begin, cursor - begin);
            }
            return literal;
        }

        public static string GetInQuoteString(this string text, ref int cursor)
        {
            var begin = cursor++;
            var result = string.Empty;

            while (cursor < text.Length)
            {
                if (text[cursor] == '\\')
                {
                    if (cursor + 1 == text.Length)
                    {
                        result = text.Substring(begin + 1).Unescape('"');
                        break;
                    }
                    if (text[cursor + 1] == '"' || text[cursor + 1] == '\\')
                    {
                        cursor++;
                    }
                }
                else if (text[cursor] == '"')
                {
                    result = text.Substring(begin + 1, cursor - begin - 1).Unescape('"');
                    break;
                }
                cursor++;
            }
            return (cursor == text.Length)
                ? text.Substring(begin + 1).Unescape('"')
                : result;
        }

        public static string GetString(this string text, ref int cursor)
        {
            var begin = cursor++;
            var result = string.Empty;

            while (cursor < text.Length)
            {
                if (text[cursor] == '\\')
                {
                    if (cursor + 1 == text.Length)
                    {
                        result = text.Substring(begin);
                        break;
                    }
                    if (text[cursor + 1] == '"' || text[cursor + 1] == '\\')
                    {
                        cursor++;
                    }
                }
                else if (text[cursor] == '"')
                {
                    result = text.Substring(begin, cursor - begin + 1);
                    break;
                }
                cursor++;
            }
            return (cursor == text.Length)
                ? text.Substring(begin)
                : result;
        }

        /*
        [Obsolete]
        public static string GetInBracketString(string text, char beginChar, char endChar, ref int cursor)
        {
            var begin = cursor++;
            var nest = 0;
            while (cursor < text.Length)
            {
                if (text[cursor] == beginChar)
                {
                    nest++;
                    if (cursor + 1 == text.Length)
                    {
                        return text.Substring(begin + 1).Unescape();
                    }
                }
                else if (text[cursor] == endChar)
                {
                    if (nest > 0)
                    {
                        nest--;
                    }
                    else
                    {
                        return text.Substring(begin + 1, cursor - begin - 1).Unescape();
                    }
                }
                cursor++;
            }
            return text.Substring(begin + 1).Escape();
        }
        */

        public static bool CheckNext(this string text, int index, char character)
        {
            index++;
            return index >= 0 && text.Length > index && text[index] == character;
        }

        public static bool CheckNext(this string text, int index, IEnumerable<char> characters)
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
            return
                index >= 0 &&
                text.Length > index + 1 &&
                previous == text[index] &&
                next == text[index + 1];
        }
    }
}
