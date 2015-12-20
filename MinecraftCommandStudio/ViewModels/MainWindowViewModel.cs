using System;
using System.Linq;
using Cafemoca.MinecraftCommandStudio.Services;
using Cafemoca.MinecraftCommandStudio.Settings;
using Cafemoca.MinecraftCommandStudio.ViewModels.Flips;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Anchorables;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Bases;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Documents;
using Cafemoca.MinecraftCommandStudio.ViewModels.Parts;
using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Cafemoca.MinecraftCommandStudio.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {
        public MainMenuViewModel MainMenuViewModel { get; private set; }

        public ReactiveProperty<DocumentPaneViewModel> ActiveDocument { get; private set; }

        public ReactiveCollection<AnchorablePaneViewModel> Tools { get; private set; }
        public ReactiveCollection<DocumentPaneViewModel> Files { get; private set; }

        public StatusBarViewModel StatusBarViewModel { get; private set; }

        public SettingFlipViewModel SettingFlipViewModel { get; private set; }
        public ReactiveProperty<bool> IsSettingFlipOpen { get; private set; }
        public ReactiveCommand SettingCommand { get; private set; }

        public KeywordSettingViewModel CompletionEditorViewModel { get; set; }

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

            this.ActiveDocument = new ReactiveProperty<DocumentPaneViewModel>();
            this.CompletionEditorViewModel = new KeywordSettingViewModel();

            this.Tools = new ReactiveCollection<AnchorablePaneViewModel> { this.CompletionEditorViewModel };

            this.Files = new ReactiveCollection<DocumentPaneViewModel>();
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
