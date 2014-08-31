using Cafemoca.CommandEditor.Utils;
using Cafemoca.McCommandStudio.Settings;
using Livet;
using System.Collections.Generic;

namespace Cafemoca.McCommandStudio.ViewModels.Flips.SettingFlips
{
    public class CompileViewModel : ViewModel
    {
        public int CompileInterval
        {
            get { return Setting.Current.CompileInterval; }
            set { Setting.Current.CompileInterval = value; }
        }
        public EscapeModeValue EscapeMode
        {
            get { return Setting.Current.ExtendedOptions.EscapeMode; }
            set { Setting.Current.ExtendedOptions.EscapeMode = value; }
        }

        public Dictionary<EscapeModeValue, string> EscapeModeList { get; private set; }

        public CompileViewModel()
        {
            this.EscapeModeList = new Dictionary<EscapeModeValue, string>()
            {
                { EscapeModeValue.New, "version 1.8 (14w31a 以降)" },
                { EscapeModeValue.Old, "version 1.7 (14w30c 以前)" },
            };
        }
    }
}
