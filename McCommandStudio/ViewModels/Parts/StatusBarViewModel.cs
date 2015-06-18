using Cafemoca.McCommandStudio.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Livet;

namespace Cafemoca.McCommandStudio.ViewModels.Parts
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
