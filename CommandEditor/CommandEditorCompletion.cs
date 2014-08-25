using Cafemoca.CommandEditor.Completions;
using Cafemoca.CommandEditor.Extensions;
using Cafemoca.CommandEditor.Indentations;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Collections.Generic;
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

            switch (key)
            {
                case Key.Back:
                case Key.Delete:
                    if (this.CheckBothSide("(", ")") ||
                        this.CheckBothSide("{", "}") ||
                        this.CheckBothSide("[", "]") ||
                        this.CheckBothSide("'", "'") ||
                        this.CheckBothSide("\"", "\""))
                    {
                        e.Handled = true;
                        this.Document.Remove(index - 1, 2);
                    }
                    break;
                case Key.Tab:
                    if (this._latestKey == Key.Tab)
                    {
                        //snippets
                    }
                    this._latestKey = Key.Tab;
                    break;
                default:
                    break;
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
                    if (next == input &&
                        prev == input)
                    {
                        e.Handled = true;
                        this.CaretOffset++;
                    }
                    else if (spaces.Contains(next))
                    {
                        e.Handled = true;
                        this.Document.Insert(index, input + input);
                        this.CaretOffset--;
                    }
                    break;
                case "(":
                case "{":
                case "[":
                    if (spaces.Contains(next))
                    {
                        e.Handled = true;
                        this.Document.Insert(index, input + bracketPair[input]);
                        this.CaretOffset--;
                    }
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
                case ":":
                    if (lastToken.IsMatchLiteral("minecraft"))
                    {
                    }
                    break;
                case " ":
                    break;
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
                    this.BeginChange();
                    this.TextArea.IndentationStrategy.AsCommandIndentationStrategy().Indent(this.Document, true);
                    this.EndChange();
                    break;
                case "\n":
                    if ("]})".Contains(next))
                    {
                        this.BeginChange();
                        this.TextArea.IndentationStrategy.AsCommandIndentationStrategy().Indent(this.Document, true);
                        this.EndChange();
                    }
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
