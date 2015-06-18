using Cafemoca.McCommandStudio.Services;
using Cafemoca.McCommandStudio.Settings;
using Cafemoca.McCommandStudio.ViewModels.Flips;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Tools;
using Cafemoca.McCommandStudio.ViewModels.Parts;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Livet;
using System;
using System.Linq;

namespace Cafemoca.McCommandStudio.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {
        public MainMenuViewModel MainMenuViewModel { get; private set; }

        public ReactiveProperty<FileViewModel> ActiveDocument { get; private set; }

        public ReactiveCollection<ToolViewModel> Tools { get; private set; }
        public ReactiveCollection<FileViewModel> Files { get; private set; }

        public StatusBarViewModel StatusBarViewModel { get; private set; }

        public SettingFlipViewModel SettingFlipViewModel { get; private set; }
        public ReactiveProperty<bool> IsSettingFlipOpen { get; private set; }
        public ReactiveCommand SettingCommand { get; private set; }

        public CompletionEditorViewModel CompletionEditorViewModel { get; set; }

        public bool ShowStartPage
        {
            get { return Setting.Current.ShowStartPage; }
        }

        private static readonly StartPageViewModel StartPageViewModel = new StartPageViewModel();
        private static int _documentCount = 0;

        public MainWindowViewModel()
        {
            this.MainMenuViewModel = new MainMenuViewModel();
            this.StatusBarViewModel = new StatusBarViewModel();

            this.ActiveDocument = new ReactiveProperty<FileViewModel>();
            this.CompletionEditorViewModel = new CompletionEditorViewModel();

            this.Tools = new ReactiveCollection<ToolViewModel> { this.CompletionEditorViewModel };

            this.Files = new ReactiveCollection<FileViewModel>();
            this.Files.CollectionChangedAsObservable().Subscribe(_ =>
                StatusService.Current.SetMain(this.Files.Count + " 個のドキュメント"));

            if (this.ShowStartPage)
            {
                this.Files.Add(StartPageViewModel);
            }
            else
            {
                this.Files.Add(new DocumentViewModel(_documentCount++));
            }

            this.InitializeDockCommands();

            this.SettingFlipViewModel = new SettingFlipViewModel();
            this.IsSettingFlipOpen = new ReactiveProperty<bool>(false);
            this.SettingCommand = new ReactiveCommand();
            this.SettingCommand.Subscribe(_ =>
                this.IsSettingFlipOpen.Value = !this.IsSettingFlipOpen.Value);

            this.ExitCommand = new ReactiveCommand();
            this.ExitCommand.Subscribe(_ =>
                this.WindowClose = true);
        }

        private void ToggleStartPage()
        {
            if (this.Files.Contains(StartPageViewModel))
            {
                this.Files.Remove(StartPageViewModel);
            }
            else if (this.ShowStartPage && !this.Files.Any())
            {
                this.Files.Add(StartPageViewModel);
            }
        }
    }
}
