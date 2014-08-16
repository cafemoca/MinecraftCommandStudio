using Cafemoca.McSlimUtils.ViewModels.Layouts.Documents;
using Cafemoca.McSlimUtils.Views.Behaviors;
using Cafemoca.McSlimUtils.Views.Behaviors.Actions;
using Codeplex.Reactive;
using Livet;
using System.Linq;
using System.Windows.Input;

namespace Cafemoca.McSlimUtils.ViewModels
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

        public ReactiveCommand ExitCommand { get; private set; }

        public CloseCondition GetCondition()
        {
            if (this.Files != null)
            {
                var files = this.Files.Where(x => !(x is StartPageViewModel));
                if (files.Where(x => x.IsModified.Value).Any())
                {
                    return files.Count() > 1
                        ? CloseCondition.AskExit
                        : CloseCondition.AskSave;
                }
                return files.Count() > 1
                    ? CloseCondition.AskCloseTab
                    : CloseCondition.Exit;
            }
            return CloseCondition.Exit;
        }
    }
}
