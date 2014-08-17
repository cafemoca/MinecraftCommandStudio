using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Codeplex.Reactive;
using System;

namespace Cafemoca.McCommandStudio.ViewModels.Layouts.Documents
{
    public class StartPageViewModel : FileViewModel
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
