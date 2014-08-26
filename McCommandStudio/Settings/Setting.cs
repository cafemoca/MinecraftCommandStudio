using Cafemoca.CommandEditor.Utils;
using Cafemoca.McCommandStudio.Settings.Xml;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;
using ICSharpCode.AvalonEdit;
using Livet;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Cafemoca.McCommandStudio.Settings
{
    public class Setting : NotificationObject
    {
        private const string filePath = "Settings.xml";

        public static Setting Current { get; private set; }

        public static void Load()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Current = filePath.ReadXml<Setting>();
                }
                else
                {
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                Initialize();
                Debug.WriteLine(ex);
            }
        }

        public static void Save()
        {
            try
            {
                Current.WriteXml(filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static void Initialize()
        {
            Current = new Setting()
            {
                EscapeMode = EscapeModeValue.New,
                ShowSpaces = true,
                ShowTabs = true,
                ShowEndOfLine = false,
                AllowScrollBelowDocument = true,
                CutCopyWholeLine = true,
                ConvertTabsToSpaces = false,
                IndentationSize = 4,
                ColumnRulerPosition = 80,
                HideCursorWhileTyping = false,
                ShowColumnRuler = false,
                EnableTextDragDrop = true,
                FontFamily = "Consolas",
                FontSize = 12,
                ShowLineNumbers = true,
                TextWrapping = false,
                DefaultFileName = "untitled",
                EncloseSelection = true,
            };
        }

        #region Options 変更通知プロパティ

        private TextEditorOptions _options = new TextEditorOptions();
        [XmlIgnore]
        public TextEditorOptions Options
        {
            get
            {
                this._options.ShowSpaces = this.ShowSpaces;
                this._options.ShowTabs = this.ShowTabs;
                this._options.ShowEndOfLine = this.ShowEndOfLine;
                this._options.AllowScrollBelowDocument = this.AllowScrollBelowDocument;
                this._options.CutCopyWholeLine = this.CutCopyWholeLine;
                this._options.ConvertTabsToSpaces = this.ConvertTabsToSpaces;
                this._options.IndentationSize = this.IndentationSize;
                this._options.ColumnRulerPosition = this.ColumnRulerPosition;
                this._options.HideCursorWhileTyping = this.HideCursorWhileTyping;
                this._options.ShowColumnRuler = this.ShowColumnRuler;
                this._options.EnableTextDragDrop = this.EnableTextDragDrop;
                return this._options;
            }
            set
            {
                this._options = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region EscapeMode 変更通知プロパティ

        private EscapeModeValue _escapeMode = EscapeModeValue.New;
        public EscapeModeValue EscapeMode
        {
            get { return this._escapeMode; }
            set
            {
                this._escapeMode = value;
                this.RaisePropertyChanged();
                this.ApplySettingsForDocuments();
            }
        }

        #endregion

        #region ShowSpaces 変更通知プロパティ

        private bool _showSpaces = true;
        public bool ShowSpaces
        {
            get { return this._showSpaces; }
            set
            {
                this._showSpaces = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region ShowTabs 変更通知プロパティ

        private bool _showTabs = true;
        public bool ShowTabs
        {
            get { return this._showTabs; }
            set
            {
                this._showTabs = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region ShowEndOfLine 変更通知プロパティ

        private bool _showEndOfLine = false;
        public bool ShowEndOfLine
        {
            get { return this._showEndOfLine; }
            set
            {
                this._showEndOfLine = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region AllowScrollBelowDocument 変更通知プロパティ

        private bool _allowScrollBelowDocument = true;
        public bool AllowScrollBelowDocument
        {
            get { return this._allowScrollBelowDocument; }
            set
            {
                this._allowScrollBelowDocument = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region CutCopyWholeLine 変更通知プロパティ

        private bool _cutCopyWholeLine = true;
        public bool CutCopyWholeLine
        {
            get { return this._cutCopyWholeLine; }
            set
            {
                this._cutCopyWholeLine = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region ConvertTabsToSpaces 変更通知プロパティ

        private bool _convertTabsToSpaces = false;
        public bool ConvertTabsToSpaces
        {
            get { return this._convertTabsToSpaces; }
            set
            {
                this._convertTabsToSpaces = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region IndentationSize 変更通知プロパティ

        private int _indentationSize = 4;
        public int IndentationSize
        {
            get { return this._indentationSize; }
            set
            {
                this._indentationSize = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region ColumnRulerPosition 変更通知プロパティ

        private int _columnRulerPosition = 80;
        public int ColumnRulerPosition
        {
            get { return this._columnRulerPosition; }
            set
            {
                this._columnRulerPosition = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region HideCursorWhileTyping 変更通知プロパティ

        private bool _hideCursorWhileTyping = false;
        public bool HideCursorWhileTyping
        {
            get { return this._hideCursorWhileTyping; }
            set
            {
                this._hideCursorWhileTyping = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region ShowColumnRuler 変更通知プロパティ

        private bool _showColumnRuler = false;
        public bool ShowColumnRuler
        {
            get { return this._showColumnRuler; }
            set
            {
                this._showColumnRuler = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region EnableTextDragDrop 変更通知プロパティ

        private bool _enableTextDragDrop = true;
        public bool EnableTextDragDrop
        {
            get { return this._enableTextDragDrop; }
            set
            {
                this._enableTextDragDrop = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Options");
            }
        }

        #endregion

        #region FontFamily 変更通知プロパティ

        private string _fontFamily = "Consolas";
        public string FontFamily
        {
            get { return this._fontFamily; }
            set
            {
                this._fontFamily = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region FontSize 変更通知プロパティ

        private int _fontSize = 12;
        public int FontSize
        {
            get { return this._fontSize; }
            set
            {
                this._fontSize = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region ShowLineNumbers 変更通知プロパティ

        private bool _showLineNumbers = true;
        public bool ShowLineNumbers
        {
            get { return this._showLineNumbers; }
            set
            {
                this._showLineNumbers = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region TextWrapping 変更通知プロパティ

        private bool _textWrapping = false;
        public bool TextWrapping
        {
            get { return this._textWrapping; }
            set
            {
                this._textWrapping = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region DefaultFileName 変更通知プロパティ

        private string _defaultFileName = "untitled";
        public string DefaultFileName
        {
            get { return this._defaultFileName; }
            set
            {
                this._defaultFileName = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region EncloseSelection 変更通知プロパティ

        private bool _encloseSelection = true;
        public bool EncloseSelection
        {
            get { return this._encloseSelection; }
            set
            {
                this._encloseSelection = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        public void ApplySettingsForDocuments()
        {
            if (App.MainViewModel != null && App.MainViewModel.Files.Any())
            {
                App.MainViewModel.Files
                    .Where(x => x is DocumentViewModel)
                    .ForEach(x => (x as DocumentViewModel).UpdateCommand());
            }
        }
    }
}
