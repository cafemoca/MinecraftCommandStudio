using Cafemoca.CommandEditor.Completions;
using Cafemoca.CommandEditor.Indentations;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor, IDisposable
    {
        public CommandEditor()
            : base()
        {
            this.LoadSyntaxHighlight();
            this.SetDefaultFoldings();
            this.BracketHighlightInitialize();

            this.ExtendedOptions = new ExtendedOptions();
            this.TextArea.IndentationStrategy = new CommandIndentationStrategy();

            this.TextArea.Caret.PositionChanged += this.Caret_PositionChanged;
            this.TextArea.TextEntering += this.TextArea_TextEntering;
            this.TextArea.TextEntered += this.TextArea_TextEntered;

            this._searchPanel = SearchPanel.Install(this.TextArea);

            Minecraft.LoadMinecraftDefinition();
        }

        private SearchPanel _searchPanel;

        protected override void OnTextChanged(EventArgs e)
        {
            this.BindableText = this.Text;
            base.OnTextChanged(e);
            this.UpdateFoldings();
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            this.HighlightBrackets();

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
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (this.TextArea.IsFocused)
            {
                this.FixOnPreviewKeyDown(e);
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            if (this.TextArea.IsFocused)
            {
                this.FixOnPreviewTextInput(e);
            }
        }

        public void Dispose()
        {
            this._searchPanel.Uninstall();
            this._foldingStrategy = null;
            this._foldingManager = null;

            this.TextArea.Caret.PositionChanged -= Caret_PositionChanged;
            this.TextArea.TextEntering -= this.TextArea_TextEntering;
            this.TextArea.TextEntered -= this.TextArea_TextEntered;
        }
    }
}
