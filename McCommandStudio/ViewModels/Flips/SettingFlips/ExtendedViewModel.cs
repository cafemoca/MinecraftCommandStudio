using Cafemoca.McCommandStudio.Settings;
using Livet;

namespace Cafemoca.McCommandStudio.ViewModels.Flips.SettingFlips
{
    public class ExtendedViewModel : ViewModel
    {
        public bool AutoReformat
        {
            get { return Setting.Current.ExtendedOptions.AutoReformat; }
            set { Setting.Current.ExtendedOptions.AutoReformat = value; }
        }

        public bool EncloseSelection
        {
            get { return Setting.Current.ExtendedOptions.EncloseSelection; }
            set { Setting.Current.ExtendedOptions.EncloseSelection = value; }
        }

        public bool EncloseMultiLine
        {
            get { return Setting.Current.ExtendedOptions.EncloseMultiLine; }
            set { Setting.Current.ExtendedOptions.EncloseMultiLine = value; }
        }

        public bool BracketCompletion
        {
            get { return Setting.Current.ExtendedOptions.BracketCompletion; }
            set { Setting.Current.ExtendedOptions.BracketCompletion = value; }
        }

        public bool EnableCompletion
        {
            get { return Setting.Current.ExtendedOptions.EnableCompletion; }
            set { Setting.Current.ExtendedOptions.EnableCompletion = value; }
        }
    }
}
