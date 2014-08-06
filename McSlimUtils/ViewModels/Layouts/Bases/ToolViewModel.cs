using Codeplex.Reactive;

namespace Cafemoca.McSlimUtils.ViewModels.Layouts.Bases
{
    public class ToolViewModel : PaneViewModel
    {
        public ReactiveProperty<string> Name { get; private set; }
        public ReactiveProperty<bool> IsVisible { get; private set; }

        public ToolViewModel()
            : this(string.Empty)
        {
        }

        public ToolViewModel(string name)
            : base()
        {
            this.Title.Value = name;
            this.Name = new ReactiveProperty<string>(name);
        }
    }
}
