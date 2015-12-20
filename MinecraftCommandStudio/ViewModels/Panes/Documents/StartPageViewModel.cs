using System;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Bases;
using Reactive.Bindings;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Documents
{
    public class StartPageViewModel : DocumentPaneViewModel
    {
        public ReactiveCommand NewCommand { get; private set; }
        public ReactiveCommand OpenCommand { get; private set; }

        public StartPageViewModel()
            : base()
        {
            this.Title.Value = "スタート ページ";

            this.NewCommand = new ReactiveCommand();
            this.NewCommand.Subscribe(_ => App.MainViewModel.NewCommand.Execute());

            this.OpenCommand = new ReactiveCommand();
            this.OpenCommand.Subscribe(_ => App.MainViewModel.OpenCommand.Execute());
        }
    }
}
