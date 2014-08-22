using Cafemoca.CommandEditor.Utils;
using Cafemoca.McCommandStudio.Settings;
using Livet;
using System.Collections.Generic;

namespace Cafemoca.McCommandStudio.ViewModels.Flips.SettingFlips
{
    public class CompileViewModel : ViewModel
    {
        public EscapeModeValue EscapeMode
        {
            get { return Setting.Current.EscapeMode; }
            set { Setting.Current.EscapeMode = value; }
        }

        public Dictionary<EscapeModeValue, string> EscapeModeList { get; private set; }

        public CompileViewModel()
        {
            this.EscapeModeList = new Dictionary<EscapeModeValue, string>()
            {
                { EscapeModeValue.New, "1.8.x (14w31a 以降)" },
                { EscapeModeValue.Old, "1.7.x (14w30c 以前)" },
            };
        }
    }
}
