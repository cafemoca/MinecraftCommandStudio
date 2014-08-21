using Codeplex.Reactive;
using System;

namespace Cafemoca.McCommandStudio.ViewModels.Layouts.Bases
{
    public class ToolViewModel : PaneViewModel
    {
        public ReactiveProperty<string> Name { get; private set; }
        public ReactiveProperty<bool> IsVisible { get; private set; }

        public ReactiveCommand CloseCommand { get; private set; }

        public ToolViewModel()
            : this(string.Empty)
        {
        }

        public ToolViewModel(string name)
            : base()
        {
            this.Title.Value = name;
            this.Name = new ReactiveProperty<string>(name);

            this.CloseCommand = new ReactiveCommand();
            this.CloseCommand.Subscribe(_ => App.MainViewModel.Close(this));
        }
    }
}
