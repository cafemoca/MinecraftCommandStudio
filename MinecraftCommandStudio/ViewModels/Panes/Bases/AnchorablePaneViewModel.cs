using System;
using Reactive.Bindings;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Bases
{
    public class AnchorablePaneViewModel : PaneViewModel
    {
        public ReactiveProperty<string> Name { get; private set; }
        public ReactiveProperty<bool> IsVisible { get; set; }

        public ReactiveCommand CloseCommand { get; private set; }

        public AnchorablePaneViewModel()
            : this(string.Empty)
        {
        }

        public AnchorablePaneViewModel(string name)
            : base()
        {
            this.Title.Value = name;
            this.Name = new ReactiveProperty<string>(name);
            this.IsVisible = new ReactiveProperty<bool>(true);

            this.CloseCommand = new ReactiveCommand();
            this.CloseCommand.Subscribe(_ => App.MainViewModel.Close(this));
        }
    }
}
