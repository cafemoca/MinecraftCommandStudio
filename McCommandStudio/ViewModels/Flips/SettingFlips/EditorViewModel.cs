using Cafemoca.McCommandStudio.Settings;
using Livet;

namespace Cafemoca.McCommandStudio.ViewModels.Flips.SettingFlips
{
    public class EditorViewModel : ViewModel
    {
        public bool AllowScrollBelowDocument
        {
            get { return Setting.Current.EditorOptions.AllowScrollBelowDocument; }
            set { Setting.Current.EditorOptions.AllowScrollBelowDocument = value; }
        }

        public bool CutCopyWholeLine
        {
            get { return Setting.Current.EditorOptions.CutCopyWholeLine; }
            set { Setting.Current.EditorOptions.CutCopyWholeLine = value; }
        }

        public bool EnableTextDragDrop
        {
            get { return Setting.Current.EditorOptions.EnableTextDragDrop; }
            set { Setting.Current.EditorOptions.EnableTextDragDrop = value; }
        }

        public bool HideCursorWhileTyping
        {
            get { return Setting.Current.EditorOptions.HideCursorWhileTyping; }
            set { Setting.Current.EditorOptions.HideCursorWhileTyping = value; }
        }

        public bool ConvertTabsToSpaces
        {
            get { return Setting.Current.EditorOptions.ConvertTabsToSpaces; }
            set { Setting.Current.EditorOptions.ConvertTabsToSpaces = value; }
        }

        public int IndentationSize
        {
            get { return Setting.Current.EditorOptions.IndentationSize; }
            set { Setting.Current.EditorOptions.IndentationSize = value; }
        }

        public bool ShowColumnRuler
        {
            get { return Setting.Current.EditorOptions.ShowColumnRuler; }
            set { Setting.Current.EditorOptions.ShowColumnRuler = value; }
        }

        public int ColumnRulerPosition
        {
            get { return Setting.Current.EditorOptions.ColumnRulerPosition; }
            set { Setting.Current.EditorOptions.ColumnRulerPosition = value; }
        }

        public bool ShowSpaces
        {
            get { return Setting.Current.EditorOptions.ShowSpaces; }
            set { Setting.Current.EditorOptions.ShowSpaces = value; }
        }

        public bool ShowTabs
        {
            get { return Setting.Current.EditorOptions.ShowTabs; }
            set { Setting.Current.EditorOptions.ShowTabs = value; }
        }

        public bool ShowEndOfLine
        {
            get { return Setting.Current.EditorOptions.ShowEndOfLine; }
            set { Setting.Current.EditorOptions.ShowEndOfLine = value; }
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
    }
}
