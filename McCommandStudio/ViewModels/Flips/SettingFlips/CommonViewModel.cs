using Cafemoca.McCommandStudio.Settings;
using Livet;

namespace Cafemoca.McCommandStudio.ViewModels.Flips.SettingFlips
{
    public class CommonViewModel : ViewModel
    {
        public string DefaultFileName
        {
            get { return Setting.Current.DefaultFileName; }
            set { Setting.Current.DefaultFileName = value; }
        }
    }
}
