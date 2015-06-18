using System.Linq;
using System.Windows.Input;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;
using Cafemoca.McCommandStudio.Views.Behaviors;
using Cafemoca.McCommandStudio.Views.Behaviors.Actions;
using Livet;
using Reactive.Bindings;

namespace Cafemoca.McCommandStudio.ViewModels
{
    public partial class MainWindowViewModel : ViewModel, IStateQueryableViewModel
    {
        #region Cursor 変更通知プロパティ

        private Cursor _cursor = Cursors.Arrow;
        public Cursor Cursor
        {
            get { return _cursor; }
            set
            {
                this._cursor = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region WindowClose 変更通知プロパティ

        private bool _windowClose = false;
        public bool WindowClose
        {
            get { return this._windowClose; }
            set
            {
                this._windowClose = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        public ReactiveCommand ExitCommand { get; private set; }

        public int GetModifiedDocumentCount()
        {
            if (this.Files != null)
            {
                return this.Files.Where(x => x.IsModified.Value).Count();
            }
            return 0;
        }

        public CloseCondition GetCondition()
        {
            if (this.Files != null)
            {
                var files = this.Files.Where(x => !(x is StartPageViewModel));
                var count = this.GetModifiedDocumentCount();
                if (count > 0)
                {
                    return count > 1
                        ? CloseCondition.AskExit
                        : this.ActiveDocument.Value.IsModified.Value
                            ? CloseCondition.AskSave
                            : CloseCondition.AskExit;
                }
                return files.Count() > 1
                    ? CloseCondition.AskCloseTab
                    : CloseCondition.Exit;
            }
            return CloseCondition.Exit;
        }
    }
}
