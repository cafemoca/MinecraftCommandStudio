using Cafemoca.CommandEditor.Extensions;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafemoca.CommandEditor.Utils
{
    public class BracketSearcher
    {
        private IEnumerable<char> openBrackets = "({[";
        private IEnumerable<char> closeBrackets = ")}]";

        public BracketSearchResult SearchBrackets(TextDocument document, int offset)
        {
            if (offset <= 0 || offset > document.TextLength)
            {
                return null;
            }

            var cha = document.GetCharAt(offset - 1);

            var index = openBrackets.IndexOf(cha);
            var other = -1;
            if (index >= 0)
            {
                other = SearchBracketForward(document, offset, openBrackets.ElementAt(index), closeBrackets.ElementAt(index));
            }

            index = closeBrackets.IndexOf(cha);
            if (index >= 0)
            {
                other = SearchBracketBackward(document, offset - 2, openBrackets.ElementAt(index), closeBrackets.ElementAt(index));
            }

            return (other >= 0)
                ? new BracketSearchResult(Math.Min(offset - 1, other), 1, Math.Max(offset - 1, other), 1)
                : null;
        }

        private static int ScanLineStart(TextDocument document, int offset)
        {
            for (int i = offset - 1; i > 0; --i)
            {
                if (document.GetCharAt(i) == '\n')
                {
                    return i + 1;
                }
            }
            return 0;
        }

        private static int GetStartType(TextDocument document, int lineStart, int offset)
        {
            var inString = false;
            var inChar = false;
            var verbatim = false;
            var result = 0;

            for (int i = lineStart; i < offset; i++)
            {
                switch (document.GetCharAt(i))
                {
                    case '/':
                        if (!inString && !inChar && i + 1 < document.TextLength)
                        {
                            if (document.GetCharAt(i + 1) == '/')
                            {
                                result = 1;
                            }
                        }
                        break;
                    case '"':
                        if (!inChar)
                        {
                            if (inString && verbatim)
                            {
                                if (i + 1 < document.TextLength && document.GetCharAt(i + 1) == '"')
                                {
                                    ++i;
                                    inString = false;
                                }
                                else
                                {
                                    verbatim = false;
                                }
                            }
                            else if (!inString && i > 0 && document.GetCharAt(i - 1) == '@')
                            {
                                verbatim = true;
                            }
                            inString = !inString;
                        }
                        break;
                    case '\'':
                        if (!inString) inChar = !inChar;
                        break;
                    case '\\':
                        if ((inString && !verbatim) || inChar)
                            ++i;
                        break;
                }
            }

            return (inString || inChar) ? 2 : result;
        }

        private int SearchBracketBackward(TextDocument document, int offset, char openBracket, char closeBracket)
        {
            if (offset + 1 >= document.TextLength)
            {
                return -1;
            }

            var quickResult = QuickSearchBracketBackward(document, offset, openBracket, closeBracket);
            if (quickResult >= 0)
            {
                return quickResult;
            }

            var lineStart = ScanLineStart(document, offset + 1);
            var starttype = GetStartType(document, lineStart, offset + 1);

            if (starttype == 1)
            {
                return -1;
            }

            var bracketStack = new Stack<int>();
            var blockComment = false;
            var lineComment = false;
            var inChar = false;
            var inString = false;
            var verbatim = false;

            for (int i = 0; i <= offset; ++i)
            {
                var cha = document.GetCharAt(i);
                switch (cha)
                {
                    case '\r':
                    case '\n':
                        lineComment = false;
                        inChar = false;
                        if (!verbatim)
                        {
                            inString = false;
                        }
                        break;
                    case '/':
                        if (blockComment)
                        {
                            if (document.GetCharAt(i - 1) == '*')
                            {
                                blockComment = false;
                            }
                        }
                        if (!inString && !inChar && i + 1 < document.TextLength)
                        {
                            if (!blockComment && document.GetCharAt(i + 1) == '/')
                            {
                                lineComment = true;
                            }
                            if (!lineComment && document.GetCharAt(i + 1) == '*')
                            {
                                blockComment = true;
                            }
                        }
                        break;
                    case '"':
                        if (!(inChar || lineComment || blockComment))
                        {
                            if (inString && verbatim)
                            {
                                if (i + 1 < document.TextLength && document.GetCharAt(i + 1) == '"')
                                {
                                    ++i;
                                    inString = false;
                                }
                                else
                                {
                                    verbatim = false;
                                }
                            }
                            else if (!inString && offset > 0 && document.GetCharAt(i - 1) == '@')
                            {
                                verbatim = true;
                            }
                            inString = !inString;
                        }
                        break;
                    case '\'':
                        if (!(inString || lineComment || blockComment))
                        {
                            inChar = !inChar;
                        }
                        break;
                    case '\\':
                        if ((inString && !verbatim) || inChar)
                        {
                            ++i;
                        }
                        break;
                    default:
                        if (cha == openBracket)
                        {
                            if (!(inString || inChar || lineComment || blockComment))
                            {
                                bracketStack.Push(i);
                            }
                        }
                        else if (cha == closeBracket)
                        {
                            if (!(inString || inChar || lineComment || blockComment))
                            {
                                if (bracketStack.Count > 0)
                                {
                                    bracketStack.Pop();
                                }
                            }
                        }
                        break;
                }
            }

            return (bracketStack.Count > 0) ? (int)bracketStack.Pop() : -1;
        }

        private int SearchBracketForward(TextDocument document, int offset, char openBracket, char closeBracket)
        {
            var inString = false;
            var inChar = false;
            var verbatim = false;

            var lineComment = false;
            var blockComment = false;

            if (offset < 0)
            {
                return -1;
            }

            var quickResult = QuickSearchBracketForward(document, offset, openBracket, closeBracket);
            if (quickResult >= 0)
            {
                return quickResult;
            }

            var lineStart = ScanLineStart(document, offset);
            var starttype = GetStartType(document, lineStart, offset);
            if (starttype != 0)
            {
                return -1;
            }

            var brackets = 1;

            while (offset < document.TextLength)
            {
                var cha = document.GetCharAt(offset);
                switch (cha)
                {
                    case '\r':
                    case '\n':
                        lineComment = false;
                        inChar = false;
                        if (!verbatim)
                        {
                            inString = false;
                        }
                        break;
                    case '/':
                        if (blockComment)
                        {
                            if (document.GetCharAt(offset - 1) == '*')
                            {
                                blockComment = false;
                            }
                        }
                        if (!inString && !inChar && offset + 1 < document.TextLength)
                        {
                            if (!blockComment && document.GetCharAt(offset + 1) == '/')
                            {
                                lineComment = true;
                            }
                            if (!lineComment && document.GetCharAt(offset + 1) == '*')
                            {
                                blockComment = true;
                            }
                        }
                        break;
                    case '"':
                        if (!(inChar || lineComment || blockComment))
                        {
                            if (inString && verbatim)
                            {
                                if (offset + 1 < document.TextLength && document.GetCharAt(offset + 1) == '"')
                                {
                                    ++offset;
                                    inString = false;
                                }
                                else
                                {
                                    verbatim = false;
                                }
                            }
                            else if (!inString && offset > 0 && document.GetCharAt(offset - 1) == '@')
                            {
                                verbatim = true;
                            }
                            inString = !inString;
                        }
                        break;
                    case '\'':
                        if (!(inString || lineComment || blockComment))
                        {
                            inChar = !inChar;
                        }
                        break;
                    case '\\':
                        if ((inString && !verbatim) || inChar)
                        {
                            ++offset;
                        }
                        break;
                    default:
                        if (cha == openBracket)
                        {
                            if (!(inString || inChar || lineComment || blockComment))
                            {
                                ++brackets;
                            }
                        }
                        else if (cha == closeBracket)
                        {
                            if (!(inString || inChar || lineComment || blockComment))
                            {
                                --brackets;
                                if (brackets == 0)
                                {
                                    return offset;
                                }
                            }
                        }
                        break;
                }
                ++offset;
            }
            return -1;
        }

        private int QuickSearchBracketBackward(TextDocument document, int offset, char openBracket, char closeBracket)
        {
            var brackets = -1;

            for (int i = offset; i >= 0; --i)
            {
                var cha = document.GetCharAt(i);
                if (cha == openBracket)
                {
                    ++brackets;
                    if (brackets == 0)
                    {
                        return i;
                    }
                }
                else if (cha == closeBracket)
                {
                    --brackets;
                }
                else if (cha == '"')
                {
                    break;
                }
                else if (cha == '\'')
                {
                    break;
                }
                else if (cha == '/' && i > 0)
                {
                    if ((document.GetCharAt(i - 1) == '/') ||
                        (document.GetCharAt(i - 1) == '*'))
                    {
                        break;
                    }
                }
            }

            return -1;
        }

        private int QuickSearchBracketForward(TextDocument document, int offset, char openBracket, char closeBracket)
        {
            var brackets = 1;

            for (int i = offset; i < document.TextLength; ++i)
            {
                var cha = document.GetCharAt(i);
                if (cha == openBracket)
                {
                    ++brackets;
                }
                else if (cha == closeBracket)
                {
                    --brackets;
                    if (brackets == 0)
                    {
                        return i;
                    }
                }
                else if (cha == '"')
                {
                    break;
                }
                else if (cha == '\'')
                {
                    break;
                }
                else if (cha == '/' && i > 0)
                {
                    if (document.GetCharAt(i - 1) == '/')
                    {
                        break;
                    }
                }
                else if (cha == '*' && i > 0)
                {
                    if (document.GetCharAt(i - 1) == '/')
                    {
                        break;
                    }
                }
            }
            return -1;
        }
    }
}
