using Cafemoca.CommandEditor.Completions;
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
        private Key _lastInputted;

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
            var inputted = e.Text;

            var completionData = this.GetCompletionData(index, inputted);
            if (completionData != null)
            {
                this.ShowCompletionWindow(completionData);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            var key = e.Key;

            this._lastInputted = key;
        }

        private IEnumerable<CompletionData> GetCompletionData(int index, string inputted)
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

            switch (inputted.ToLower())
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
    }
}
