using Cafemoca.CommandEditor;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Documents;
using ICSharpCode.AvalonEdit;
using Livet;
using System;
using System.Diagnostics;
using Cafemoca.McSlimUtils.Settings.Xml;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Cafemoca.McSlimUtils.Settings
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
                QuoteMode = QuoteModeValue.DoubleQuoteOnly,
            };
        }

        #region EscapeMode 変更通知プロパティ

        private EscapeModeValue _escapeMode;
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

        #region QuoteMode 変更通知プロパティ

        private QuoteModeValue _quoteMode;
        public QuoteModeValue QuoteMode
        {
            get { return this._quoteMode; }
            set
            {
                this._quoteMode = value;
                this.RaisePropertyChanged();
                this.ApplySettingsForDocuments();
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
