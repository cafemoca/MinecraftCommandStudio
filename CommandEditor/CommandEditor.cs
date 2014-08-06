using Cafemoca.CommandEditor.Indentations;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Collections.Generic;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor, IDisposable
    {
        public CommandEditor()
            : base()
        {
            this.TextArea.IndentationStrategy = new CommandIndentationStrategy();

            this.LoadSyntaxHighlight();
            this.SetDefaultFoldings();
            this.BracketHighlightInitialize();

            this.TextArea.Caret.PositionChanged += this.Caret_PositionChanged;
            this.TextArea.TextEntering += this.TextArea_TextEntering;
            this.TextArea.TextEntered += this.TextArea_TextEntered;

            this._searchPanel = SearchPanel.Install(this.TextArea);
        }

        private IEnumerable<Token> _tokens;
        private SearchPanel _searchPanel;

        protected override void OnTextChanged(EventArgs e)
        {
            this.BindableText = this.Document.Text;
            base.OnTextChanged(e);
            this.UpdateFoldings();

            this._tokens = this.BindableText.Tokenize();
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            this.TextArea.TextView.InvalidateLayer(KnownLayer.Background);

            if (this.TextArea != null)
            {
                this.Column = this.TextArea.Caret.Column;
                this.Line = this.TextArea.Caret.Line;
            }
            else
            {
                this.Column = 0;
                this.Line = 0;
            }

            this.HighlightBrackets();
        }

        public void Dispose()
        {
            this._searchPanel.Uninstall();

            this.TextArea.Caret.PositionChanged -= Caret_PositionChanged;
            this.TextArea.TextEntering -= this.TextArea_TextEntering;
            this.TextArea.TextEntered -= this.TextArea_TextEntered;
        }
    }
}
