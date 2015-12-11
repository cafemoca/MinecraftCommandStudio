using Cafemoca.MinecraftCommandStudio.Services;
using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Parts
{
    public class StatusBarViewModel : ViewModel
    {
        public ReactiveProperty<string> MainStatus { get; private set; }
        public ReactiveProperty<string> SubStatus { get; private set; }

        public StatusBarViewModel()
        {
            this.MainStatus = StatusService.Current.ObserveProperty(x => x.MainMessage).ToReactiveProperty();
            this.SubStatus = StatusService.Current.ObserveProperty(x => x.SubMessage).ToReactiveProperty();
        }
    }
}
