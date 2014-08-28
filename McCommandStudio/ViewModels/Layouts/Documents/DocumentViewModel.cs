﻿using Cafemoca.CommandEditor;
using Cafemoca.CommandEditor.Utils;
using Cafemoca.McCommandStudio.Services;
using Cafemoca.McCommandStudio.Settings;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Codeplex.Reactive;
using Codeplex.Reactive.Extensions;
using ICSharpCode.AvalonEdit;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Cafemoca.McCommandStudio.ViewModels.Layouts.Documents
{
    public class DocumentViewModel : FileViewModel
    {
        private ReactiveProperty<string> _compiledText;
        public ReactiveProperty<string> CompiledText
        {
            get { return this._compiledText; }
            set
            {
                this._compiledText = value;
                this.RaisePropertyChanged();
            }
        }

        public ReactiveProperty<int> Line { get; private set; }
        public ReactiveProperty<int> Column { get; private set; }

        public ReactiveProperty<TextEditorOptions> Options { get; private set; }
        public ReactiveProperty<string> FontFamily { get; private set; }
        public ReactiveProperty<int> FontSize { get; private set; }
        public ReactiveProperty<bool> ShowLineNumbers { get; private set; }
        public ReactiveProperty<bool> TextWrapping { get; private set; }
        public ReactiveProperty<bool> EncloseSelection { get; private set; }
        public ReactiveProperty<bool> EncloseMultiLine { get; private set; }
        public ReactiveProperty<bool> AutoReformat { get; private set; }
        public ReactiveProperty<bool> BracketCompletion { get; private set; }
        public ReactiveProperty<int> CompileInterval { get; private set; }
        public ReactiveProperty<EscapeModeValue> EscapeMode { get; private set; }

        public ReactiveCommand CopyCommand { get; private set; }

        public DocumentViewModel()
            : this(null, -1)
        {
        }

        public DocumentViewModel(int count)
            : this(null, count)
        {
        }

        public DocumentViewModel(string filePath)
            : this(filePath, -1)
        {
        }

        public DocumentViewModel(string filePath, int count)
            : base(filePath, count)
        {
            this.Line = new ReactiveProperty<int>(0);
            this.Column = new ReactiveProperty<int>(0);

            this.Options = Setting.Current.ObserveProperty(x => x.Options).ToReactiveProperty();
            this.FontFamily = Setting.Current.ObserveProperty(x => x.FontFamily).ToReactiveProperty();
            this.FontSize = Setting.Current.ObserveProperty(x => x.FontSize).ToReactiveProperty();
            this.ShowLineNumbers = Setting.Current.ObserveProperty(x => x.ShowLineNumbers).ToReactiveProperty();
            this.TextWrapping = Setting.Current.ObserveProperty(x => x.TextWrapping).ToReactiveProperty();
            this.EncloseSelection = Setting.Current.ObserveProperty(x => x.EncloseSelection).ToReactiveProperty();
            this.EncloseMultiLine = Setting.Current.ObserveProperty(x => x.EncloseMultiLine).ToReactiveProperty();
            this.AutoReformat = Setting.Current.ObserveProperty(x => x.AutoReformat).ToReactiveProperty();
            this.BracketCompletion = Setting.Current.ObserveProperty(x => x.BracketCompletion).ToReactiveProperty();
            this.CompileInterval = Setting.Current.ObserveProperty(x => x.CompileInterval).ToReactiveProperty();

            this.CompileInterval.Subscribe(v =>
            {
                var initialValue = string.Empty;
                if (this.CompiledText != null)
                {
                    initialValue = this.CompiledText.Value;
                    this.CompiledText.Dispose();
                }
                this.CompiledText = this.Text
                    .Throttle(TimeSpan.FromMilliseconds(v))
                    .ToReactiveProperty(initialValue);
            });

            this.CopyCommand = new ReactiveCommand();
            this.CopyCommand.Subscribe(_ =>
                Clipboard.SetText(this.CompiledText.Value));
        }

        public void JumpToLine(int line)
        {
            this.Line.Value = line;
        }

        public void JumpToColumn(int column)
        {
            this.Column.Value = column;
        }

        public void UpdateCommand()
        {
            this.CompiledText.Value = this.Text.Value.Compile();
        }

        private void SetDocumentStatus()
        {
            var message = new[]
            {
                this.Encoding.Value.ToString(),
                this.Line.Value.ToString(),
                this.Column.Value.ToString()
            }
            .JoinString(" / ")
            .ToString();

            StatusService.Current.SetMain(message);
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.RaisePropertyChanged(propertyName);

            return true;
        }
    }
}
