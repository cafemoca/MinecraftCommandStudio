using System.Windows.Media;
using Livet;
using Reactive.Bindings;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Bases
{
    public class PaneViewModel : ViewModel
    {
        public virtual ReactiveProperty<string> Title { get; private set; }
        public virtual ReactiveProperty<ImageSource> IconSource { get; private set; }
        public virtual ReactiveProperty<string> ContentId { get; private set; }
        public virtual ReactiveProperty<bool> IsSelected { get; private set; }
        public virtual ReactiveProperty<bool> IsActive { get; private set; }

        public PaneViewModel()
        {
            this.Title = new ReactiveProperty<string>();
            this.IconSource = new ReactiveProperty<ImageSource>();
            this.ContentId = new ReactiveProperty<string>();
            this.IsSelected = new ReactiveProperty<bool>();
            this.IsActive = new ReactiveProperty<bool>();
        }
    }
}
