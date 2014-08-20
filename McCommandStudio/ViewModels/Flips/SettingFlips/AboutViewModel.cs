using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafemoca.McCommandStudio.ViewModels.Flips.SettingFlips
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
