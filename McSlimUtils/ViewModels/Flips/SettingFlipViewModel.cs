using Cafemoca.CommandEditor;
using Cafemoca.McSlimUtils.Settings;
using Livet;
using System.Collections.Generic;

namespace Cafemoca.McSlimUtils.ViewModels.Flips
{
    public class SettingFlipViewModel : ViewModel
    {
        public EscapeModeValue EscapeMode
        {
            get { return Setting.Current.EscapeMode; }
            set { Setting.Current.EscapeMode = value; }
        }

        public QuoteModeValue QuoteMode
        {
            get { return Setting.Current.QuoteMode; }
            set { Setting.Current.QuoteMode = value; }
        }

        public Dictionary<EscapeModeValue, string> EscapeModeList { get; private set; }
        public Dictionary<QuoteModeValue, string> QuoteModeList { get; private set; }

        public SettingFlipViewModel()
        {
            this.EscapeModeList = new Dictionary<EscapeModeValue, string>()
            {
                { EscapeModeValue.New, "1.8.x (14w31a 以降)" },
                { EscapeModeValue.Old, "1.7.x (14w30c 以前)" },
            };
            this.QuoteModeList = new Dictionary<QuoteModeValue, string>()
            {
                { QuoteModeValue.DoubleQuoteOnly, "ダブルクォーテーションのみ" },
                { QuoteModeValue.UseSingleQuote, "文字列にシングルクォーテーションを使う" },
            };
        }
    }
}
