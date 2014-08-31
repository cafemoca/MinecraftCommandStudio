using Cafemoca.McCommandStudio.ViewModels.Flips.SettingFlips;
using Livet;

namespace Cafemoca.McCommandStudio.ViewModels.Flips
{
    public class SettingFlipViewModel : ViewModel
    {
        public CommonViewModel CommonViewModel { get; private set; }
        public EditorViewModel EditorViewModel { get; private set; }
        public ExtendedViewModel ExtendedViewModel { get; private set; }
        public CompileViewModel CompileViewModel { get; private set; }
        public AboutViewModel AboutViewModel { get; private set; }

        public SettingFlipViewModel()
        {
            this.CommonViewModel = new CommonViewModel();
            this.EditorViewModel = new EditorViewModel();
            this.ExtendedViewModel = new ExtendedViewModel();
            this.CompileViewModel = new CompileViewModel();
            this.AboutViewModel = new AboutViewModel();
        }
    }
}
