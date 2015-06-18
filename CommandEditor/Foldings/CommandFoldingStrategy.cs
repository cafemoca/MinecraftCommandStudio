using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace Cafemoca.CommandEditor.Foldings
{
    public class CommandFoldingStrategy
    {
        public void UpdateFoldings(FoldingManager manager, TextDocument document)
        {
            var firstErrorOffset = -1;
            var newFoldings = CreateNewFoldings(document, out firstErrorOffset);
            manager.UpdateFoldings(newFoldings, firstErrorOffset);
        }

        public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            firstErrorOffset = -1;
            return this.CreateNewFoldings(document);
        }

        public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
        {
            var newFoldings = new List<NewFolding>();

            var startParenthesis = new Stack<int>();
            var startCurlyBracket = new Stack<int>();
            var startSquareBracket = new Stack<int>();

            var startComment = -1;

            var inLineComment = false;
            var inBlockComment = false;

            var lastNewLineOffset = 0;

            for (int i = 0; i < document.TextLength; i++)
            {
                var character = document.GetCharAt(i);

                if (inLineComment)
                {
                    if (character != '\n')
                    {
                        continue;
                    }
                    else
                    {
                        inLineComment = false;
                    }
                }
                else if (i + 1 < document.TextLength)
                {
                    var next = document.GetCharAt(i + 1);
                    if (character == '/')
                    {
                        if (next == '/')
                        {
                            inLineComment = true;
                            i++;
                            continue;
                        }
                        if (next == '*')
                        {
                            startComment = startComment == -1 ? i : -1;
                            inBlockComment = true;
                            i++;
                            continue;
                        }
                    }
                    if (inBlockComment)
                    {
                        if (character != '*')
                        {
                            continue;
                        }
                        if (next == '/')
                        {
                            i++;
                            if (startComment != -1 && document.Text.Substring(startComment, i - startComment).Contains("\n"))
                            {
                                newFoldings.Add(new NewFolding(startComment, i + 1));
                                startComment = -1;
                            }
                            inBlockComment = false;
                        }
                    }
                }

                var startOffset = lastNewLineOffset;
                switch (character)
                {
                    case '(':
                        startParenthesis.Push(i);
                        break;
                    case '{':
                        startCurlyBracket.Push(i);
                        break;
                    case '[':
                        startSquareBracket.Push(i);
                        break;
                    case ')':
                        if (startParenthesis.Count > 0)
                        {
                            startOffset = startParenthesis.Pop();
                        }
                        if (startOffset < lastNewLineOffset)
                        {
                            newFoldings.Add(new NewFolding(startOffset, i + 1));
                        }
                        break;
                    case '}':
                        if (startCurlyBracket.Count > 0)
                        {
                            startOffset = startCurlyBracket.Pop();
                        }
                        if (startOffset < lastNewLineOffset)
                        {
                            newFoldings.Add(new NewFolding(startOffset, i + 1));
                        }
                        break;
                    case ']':
                        if (startSquareBracket.Count > 0)
                        {
                            startOffset = startSquareBracket.Pop();
                        }
                        if (startOffset < lastNewLineOffset)
                        {
                            newFoldings.Add(new NewFolding(startOffset, i + 1));
                        }
                        break;
                    case '\r':
                    case '\n':
                        lastNewLineOffset = i + 1;
                        break;
                    default:
                        break;
                }
            }

            newFoldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
            return newFoldings;
        }
    }
}
