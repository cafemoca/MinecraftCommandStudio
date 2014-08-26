using Cafemoca.CommandEditor.Completions;
using Cafemoca.CommandEditor.Extensions;
using Cafemoca.CommandEditor.Indentations;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor
    {
        private CompletionWindow _completionWindow;

        private void ShowCompletionWindow(IEnumerable<CompletionData> completions)
        {
            if (this._completionWindow != null)
            {
                return;
            }
            var window = new CompletionWindow(this.TextArea);
            window.Closed += (o, e) =>
            {
                if (this._completionWindow.Equals(window))
                {
                    this._completionWindow = null;
                }
            };
            completions.ForEach(window.CompletionList.CompletionData.Add);
            this._completionWindow = window;
            window.Show();
        }

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if ((e.Text.Length > 0) &&
                (this._completionWindow != null) &&
                !char.IsLetterOrDigit(e.Text[0]))
            {
                this._completionWindow.CompletionList.RequestInsertion(e);
            }
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (this._completionWindow != null)
            {
                return;
            }

            var index = this.CaretOffset;
            var input = e.Text;

            var completionData = this.GetCompletionData(index, input);
            if (completionData != null)
            {
                this.ShowCompletionWindow(completionData);
            }
        }

        private Key _latestKey;

        private void FixOnPreviewKeyDown(KeyEventArgs e)
        {
            var index = this.CaretOffset;
            var key = e.Key;

            var prev = this.PreviousChar.ToString();
            var next = this.NextChar.ToString();

            var line = this.Document.GetLineByOffset(index);

            switch (key)
            {
                case Key.Back:
                    this.DeleteBothBracket(e);
                    if (this.TextArea.Caret.Column == 1 &&
                        line.LineNumber > 1)
                    {
                        var indentLength = this.Text
                            .Substring(line.PreviousLine.EndOffset)
                            .TakeWhile(x => "\t\r\n 　".Contains(x))
                            .Count();
                        e.Handled = true;
                        this.Document.Remove(line.PreviousLine.EndOffset, indentLength);
                    }
                    break;
                case Key.Delete:
                    this.DeleteBothBracket(e);
                    if (index == line.EndOffset)
                    {
                        var indentLength = this.Text
                            .Substring(index)
                            .TakeWhile(x => "\t\r\n 　".Contains(x))
                            .Count();
                        e.Handled = true;
                        this.Document.Remove(index, indentLength);
                    }
                    break;
                case Key.Tab:
                    if (this._latestKey == Key.Tab)
                    {
                        //snippets
                    }
                    this._latestKey = Key.Tab;
                    break;
                case Key.Enter:
                    if (!this.IsSelection && ")}]".Contains(next))
                    {
                        e.Handled = true;
                        this.BeginChange();
                        this.Document.Insert(index, Environment.NewLine + "\t" + Environment.NewLine);
                        this.TextArea.IndentationStrategy.IndentLines(this.Document, line.LineNumber, line.NextLine.NextLine.LineNumber);
                        this.CaretOffset--;
                        this.EndChange();
                    }
                    break;
                default:
                    break;
            }
        }

        private void TrimStartLine(TextDocumentAccessor accessor)
        {
            accessor.Text = accessor.Text.TrimStart();
        }

        private void DeleteBothBracket(KeyEventArgs e)
        {
            if (this.CheckBothSide("(", ")") ||
                this.CheckBothSide("{", "}") ||
                this.CheckBothSide("[", "]") ||
                this.CheckBothSide("'", "'") ||
                this.CheckBothSide("\"", "\""))
            {
                e.Handled = true;
                this.Document.Remove(this.CaretOffset - 1, 2);
            }
        }

        private void FixOnPreviewTextInput(TextCompositionEventArgs e)
        {
            var index = this.CaretOffset;
            var input = e.Text;

            var prev = this.PreviousChar.ToString();
            var next = this.NextChar.ToString();

            var spaces = "\t\r\n\0　 ";
            var bracketPair = new Dictionary<string, string>()
            {
                { "(", ")" },
                { "{", "}" },
                { "[", "]" },
            };

            switch (input)
            {
                case "'":
                case "\"":
                    if (this.IsSingleLineSelection ||
                       (this.IsMultiLineSelection && this.EncloseMultiLine))
                    {
                        if (this.EncloseSelection)
                        {
                            e.Handled = true;
                            this.BeginChange();
                            this.Document.Insert(this.SelectionStart, input);
                            this.Document.Insert(this.SelectionStart + this.SelectionLength, input);
                            this.EndChange();
                        }
                        break;
                    }
                    if (this.IsMultiLineSelection && !this.EncloseMultiLine)
                    {
                        break;
                    }
                    if (next == input &&
                        prev == input)
                    {
                        e.Handled = true;
                        this.CaretOffset++;
                        break;
                    }
                    if (spaces.Contains(next))
                    {
                        e.Handled = true;
                        this.Document.Insert(index, input + input);
                        if (this.IsMultiLineSelection)
                        {
                            this.CaretOffset++;
                        }
                        else
                        {
                            this.CaretOffset--;
                        }
                        break;
                    }
                    break;
                case "(":
                case "{":
                case "[":
                    if (this.IsSingleLineSelection ||
                       ( this.IsMultiLineSelection && this.EncloseMultiLine))
                    {
                        if (this.EncloseSelection)
                        {
                            e.Handled = true;
                            this.BeginChange();
                            this.Document.Insert(this.SelectionStart, input);
                            this.Document.Insert(this.SelectionStart + this.SelectionLength, bracketPair[input]);
                            this.EndChange();
                        }
                        break;
                    }
                    if (this.IsMultiLineSelection && !this.EncloseMultiLine)
                    {
                        break;
                    }
                    if (spaces.Contains(next))
                    {
                        e.Handled = true;
                        this.Document.Insert(index, input + bracketPair[input]);
                        if (this.IsMultiLineSelection)
                        {
                            this.CaretOffset++;
                        }
                        else
                        {
                            this.CaretOffset--;
                        }
                        break;
                    }
                    break;
                case ")":
                case "}":
                case "]":
                    if (next == input &&
                        prev == bracketPair.First(x => x.Value == input).Key)
                    {
                        e.Handled = true;
                        this.CaretOffset++;
                        break;
                    }
                    this.BeginChange();
                    this.TextArea.IndentationStrategy.AsCommandIndentationStrategy().Indent(this.Document, true);
                    this.EndChange();
                    break;
                default:
                    break;
            }
        }
        
        private IEnumerable<CompletionData> GetCompletionData(int index, string input)
        {
            var tokens = this.Text
                .Tokenize()
                .Where(x => !x.IsMatchType(TokenType.Blank) || !x.IsMatchType(TokenType.Comment));

            var previous = index > 1
                ? this.Document.GetCharAt(index - 2)
                : '\0';
            var next = this.Document.TextLength > index + 1
                ? this.Document.GetCharAt(index)
                : '\0';

            var beforeTokens = tokens.TakeWhile(x => x.Index < index);
            var afterTokens = tokens.SkipWhile(x => x.Index < index);

            var lastToken = beforeTokens.SkipLast(1).LastOrDefault();

            switch (input.ToLower())
            {
                case "/":
                    if ("*/".Contains(previous))
                    {
                        break;
                    }
                    return MinecraftCompletions.GetCommandCompletion();
                case "*":
                    if (previous == '/')
                    {
                        return null;
                    }
                    break;
                case ")":
                case "}":
                case "]":
                    break;
                case "\n":
                    if ("]})".Contains(next))
                    {
                        this.BeginChange();
                        this.TextArea.IndentationStrategy.AsCommandIndentationStrategy().Indent(this.Document, true);
                        this.EndChange();
                    }
                    break;
                case "{":
                    break;
                case ":":
                    if (lastToken.IsMatchLiteral("minecraft"))
                    {
                    }
                    break;
                case " ":
                    break;
                default:
                    break;
            }

            return null;
        }

        private IEnumerable<CompletionData> GetName(Token command)
        {
            switch (command.Value)
            {
                default:
                    break;
            }
            return null;
        }

        private bool CheckBothSide(char prev, char next)
        {
            return this.CheckBothSide(prev, next);
        }

        private bool CheckBothSide(string prev, string next)
        {
            return this.PreviousChar.ToString() == prev &&
                   this.NextChar.ToString() == next;
        }
    }
}
