using System;
using System.Threading.Tasks;
using Cafemoca.MinecraftCommandStudio.Services;
using Livet;
using Reactive.Bindings;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Flips.SettingFlips
{
    public class VersionInfoViewModel : ViewModel
    {
        public ReactiveProperty<bool> IsChecking { get; private set; }
        public ReactiveProperty<bool> IsUpdateAvailable { get; private set; }

        public ReactiveCommand CheckUpdateCommand { get; private set; }
        public ReactiveCommand StartUpdateCommand { get; private set; }

        public string Version
        {
            get { return "Version " + App.Version.ToString(3); }
        }

        public VersionInfoViewModel()
        {
            this.IsChecking = new ReactiveProperty<bool>(false);
            this.IsUpdateAvailable = new ReactiveProperty<bool>(false);

            this.CheckUpdateCommand = new ReactiveCommand();
            this.CheckUpdateCommand.Subscribe(async _ => await this.CheckUpdateAsync());

            this.StartUpdateCommand = this.IsUpdateAvailable.ToReactiveCommand();
            this.StartUpdateCommand.Subscribe(async _ => await this.StartUpdateAsync());

            Task.Run(async () =>
            {
                await this.CheckUpdateAsync();
                if (this.IsUpdateAvailable.Value)
                {
                    StatusService.Current.Notify("アップデートが利用できます。最新版に更新するには、設定画面からアップデートを実行してください。");
                }
            });
        }

        public async Task CheckUpdateAsync()
        {
            this.IsChecking.Value = true;
            this.IsUpdateAvailable.Value = await AutoUpdateService.CheckUpdateAsync(App.Version);
            this.IsChecking.Value = false;
        }

        public async Task StartUpdateAsync()
        {
            await AutoUpdateService.StartUpdateAsync(App.Version);
        }
    }
}
