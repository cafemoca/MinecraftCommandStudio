using Livet;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Flips.SettingFlips
{
    public class AboutViewModel : ViewModel
    {
        public VersionInfoViewModel VersionInfoViewModel { get; private set; }

        public AboutViewModel()
        {
            this.VersionInfoViewModel = new VersionInfoViewModel();
        }
    }
}
