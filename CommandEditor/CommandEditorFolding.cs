using Cafemoca.CommandEditor.Foldings;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor
    {
        private FoldingManager _foldingManager;
        private CommandFoldingStrategy _foldingStrategy;

        private void SetDefaultFoldings()
        {
            this._foldingStrategy = new CommandFoldingStrategy();
            this._foldingManager = FoldingManager.Install(this.TextArea);
            this._foldingStrategy.UpdateFoldings(this._foldingManager, this.Document);
        }

        private void UpdateFoldings()
        {
            if (this._foldingStrategy != null)
            {
                this._foldingStrategy.UpdateFoldings(this._foldingManager, this.TextArea.Document);
            }
        }
    }
}
