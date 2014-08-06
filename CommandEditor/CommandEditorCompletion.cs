using Cafemoca.CommandEditor.Completions;
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
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            var key = e.Key;


            this._lastInputted = key;
        }
    }
}
