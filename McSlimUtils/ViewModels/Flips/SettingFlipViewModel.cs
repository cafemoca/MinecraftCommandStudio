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

        public bool ShowSpaces
        {
            get { return Setting.Current.ShowSpaces; }
            set { Setting.Current.ShowSpaces = value; }
        }

        public bool ShowTabs
        {
            get { return Setting.Current.ShowTabs; }
            set { Setting.Current.ShowTabs = value; }
        }

        public bool ShowEndOfLine
        {
            get { return Setting.Current.ShowEndOfLine; }
            set { Setting.Current.ShowEndOfLine = value; }
        }

        public bool ShowLineNumbers
        {
            get { return Setting.Current.ShowLineNumbers; }
            set { Setting.Current.ShowLineNumbers = value; }
        }

        public bool TextWrapping
        {
            get { return Setting.Current.TextWrapping; }
            set { Setting.Current.TextWrapping = value; }
        }

        public bool AllowScrollBelowDocument
        {
            get { return Setting.Current.AllowScrollBelowDocument; }
            set { Setting.Current.AllowScrollBelowDocument = value; }
        }

        public bool CutCopyWholeLine
        {
            get { return Setting.Current.CutCopyWholeLine; }
            set { Setting.Current.CutCopyWholeLine = value; }
        }

        public bool ConvertTabsToSpaces
        {
            get { return Setting.Current.ConvertTabsToSpaces; }
            set { Setting.Current.ConvertTabsToSpaces = value; }
        }

        public int ColumnRulerPosition
        {
            get { return Setting.Current.ColumnRulerPosition; }
            set { Setting.Current.ColumnRulerPosition = value; }
        }

        public bool HideCursorWhileTyping
        {
            get { return Setting.Current.HideCursorWhileTyping; }
            set { Setting.Current.HideCursorWhileTyping = value; }
        }

        public bool ShowColumnRuler
        {
            get { return Setting.Current.ShowColumnRuler; }
            set { Setting.Current.ShowColumnRuler = value; }
        }

        public bool EnableTextDragDrop
        {
            get { return Setting.Current.EnableTextDragDrop; }
            set { Setting.Current.EnableTextDragDrop = value; }
        }

        public string FontFamily
        {
            get { return Setting.Current.FontFamily; }
            set { Setting.Current.FontFamily = value; }
        }

        public int FontSize
        {
            get { return Setting.Current.FontSize; }
            set { Setting.Current.FontSize = value; }
        }

        public Dictionary<EscapeModeValue, string> EscapeModeList { get; private set; }

        public SettingFlipViewModel()
        {
            this.EscapeModeList = new Dictionary<EscapeModeValue, string>()
            {
                { EscapeModeValue.New, "1.8.x (14w31a 以降)" },
                { EscapeModeValue.Old, "1.7.x (14w30c 以前)" },
            };
        }
    }
}
