using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Cafemoca.CommandEditor;
using Cafemoca.CommandEditor.Utils;
using Cafemoca.McCommandStudio.Settings.Xml;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;
using ICSharpCode.AvalonEdit;
using Livet;

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
            catch
            {
                Initialize();
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
                EditorOptions = new TextEditorOptions()
                {
                    AllowScrollBelowDocument = true,
                    //AllowToggleOverstrikeMode = true,
                    ColumnRulerPosition = 80,
                    ConvertTabsToSpaces = false,
                    CutCopyWholeLine = true,
                    //EnableEmailHyperlinks = false,
                    //EnableHyperlinks = false,
                    //EnableImeSupport = true,
                    EnableRectangularSelection = true,
                    EnableTextDragDrop = true,
                    EnableVirtualSpace = false,
                    HideCursorWhileTyping = false,
                    //HighlightCurrentLine = false,
                    IndentationSize = 4,
                    //InheritWordWrapIndentation = false,
                    //RequireControlModifierForHyperlinkClick = false,
                    //ShowBoxForControlCharacters = false,
                    ShowColumnRuler = false,
                    ShowEndOfLine = false,
                    ShowSpaces = false,
                    ShowTabs = false,
                    //WordWrapIndentation = 0,
                },
                ExtendedOptions = new ExtendedOptions()
                {
                    AutoReformat = true,
                    BracketCompletion = true,
                    EnableCompletion = true,
                    EncloseMultiLine = false,
                    EncloseSelection = false,
                    EscapeMode = EscapeModeValue.New,
                    PlayerNames = new ObservableCollection<string>(),
                    ScoreNames = new ObservableCollection<string>(),
                    TeamNames = new ObservableCollection<string>(),
                },
                FontFamily = "Consolas",
                FontSize = 12,
                ShowLineNumbers = true,
                TextWrapping = false,
                DefaultFileName = "untitled",
                CompileInterval = 1000,
                ShowStartPage = true,
            };
        }

        #region EditorOptions 変更通知プロパティ

        private TextEditorOptions _editorOptions = new TextEditorOptions();
        [XmlElement("EditorOptions")]
        public TextEditorOptions EditorOptions
        {
            get { return this._editorOptions; }
            set
            {
                this._editorOptions = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region ExtendedOptions 変更通知プロパティ

        private ExtendedOptions _extendedOptions = new ExtendedOptions();
        [XmlElement("ExtendedOptions")]
        public ExtendedOptions ExtendedOptions
        {
            get { return this._extendedOptions; }
            set
            {
                this._extendedOptions = value;
                this.RaisePropertyChanged();
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

        #region CompileInterval 変更通知プロパティ

        private int _compileInterval = 1000;
        public int CompileInterval
        {
            get { return this._compileInterval; }
            set
            {
                this._compileInterval = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region ShowStartPage 変更通知プロパティ

        private bool _showStartPage = true;
        public bool ShowStartPage
        {
            get { return this._showStartPage; }
            set
            {
                this._showStartPage = value;
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
