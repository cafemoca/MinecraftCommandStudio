using Cafemoca.CommandEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Cafemoca.CommandEditor
{
    [Serializable]
    public class ExtendedOptions : INotifyPropertyChanged
    {
        public ExtendedOptions()
        {
        }

        public ExtendedOptions(ExtendedOptions options)
        {
            var fields = typeof(ExtendedOptions).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (!field.IsNotSerialized)
                {
                    field.SetValue(this, field.GetValue(options));
                }
            }
        }

        #region PlayerNames 変更通知プロパティ

        private ObservableCollection<string> _playerNames = new ObservableCollection<string>();
        public ObservableCollection<string> PlayerNames
        {
            get { return this._playerNames; }
            set
            {
                if (this._playerNames == value) return;
                this._playerNames = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region ScoreNames 変更通知プロパティ

        private ObservableCollection<string> _scoreNames = new ObservableCollection<string>();
        public ObservableCollection<string> ScoreNames
        {
            get { return this._scoreNames; }
            set
            {
                if (this._scoreNames == value) return;
                this._scoreNames = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region TeamNames 変更通知プロパティ

        private ObservableCollection<string> _teamNames = new ObservableCollection<string>();
        public ObservableCollection<string> TeamNames
        {
            get { return this._teamNames; }
            set
            {
                if (this._teamNames == value) return;
                this._teamNames = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region EncloseSelection 変更通知プロパティ

        private bool _encloseSelection = false;
        public bool EncloseSelection
        {
            get { return this._encloseSelection; }
            set
            {
                if (this._encloseSelection == value) return;
                this._encloseSelection = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region EncloseMultiLine 変更通知プロパティ

        private bool _encloseMultiLine = false;
        public bool EncloseMultiLine
        {
            get { return this._encloseMultiLine; }
            set
            {
                if (this._encloseMultiLine == value) return;
                this._encloseMultiLine = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region AutoReformat 変更通知プロパティ

        private bool _autoReformat = true;
        public bool AutoReformat
        {
            get { return this._autoReformat; }
            set
            {
                if (this._autoReformat == value) return;
                this._autoReformat = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region BracketCompletion 変更通知プロパティ

        private bool _bracketCompletion = true;
        public bool BracketCompletion
        {
            get { return this._bracketCompletion; }
            set
            {
                if (this._bracketCompletion == value) return;
                this._bracketCompletion = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region EnableCompletion 変更通知プロパティ

        private bool _enableCompletion = true;
        public bool EnableCompletion
        {
            get { return this._enableCompletion; }
            set
            {
                if (this._enableCompletion == value) return;
                this._enableCompletion = value;
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
                if (this._escapeMode == value) return;
                this._escapeMode = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region PropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaisePropertyChanged(string[] propertyNames)
        {
            var handler = this.PropertyChanged;
            if (handler == null) return;
            propertyNames.ForEach(p => handler(this, new PropertyChangedEventArgs(p)));
        }

        #endregion
    }
}
