using Cafemoca.CommandEditor;
using Cafemoca.CommandEditor.Utils;
using Cafemoca.McCommandStudio.Settings;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Codeplex.Reactive;
using Codeplex.Reactive.Extensions;
using ICSharpCode.AvalonEdit;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

namespace Cafemoca.McCommandStudio.ViewModels.Layouts.Documents
{
    public class DocumentViewModel : FileViewModel
    {
        public ReactiveProperty<string> CompiledText { get; private set; }
        public ReactiveProperty<int> Line { get; private set; }
        public ReactiveProperty<int> Column { get; private set; }

        public TextEditorOptions Options
        {
            get { return Setting.Current.Options; }
        }

        public string FontFamily
        {
            get { return Setting.Current.FontFamily; }
        }

        public int FontSize
        {
            get { return Setting.Current.FontSize; }
        }

        public bool ShowLineNumbers
        {
            get { return Setting.Current.ShowLineNumbers; }
        }

        public bool TextWrapping
        {
            get { return Setting.Current.TextWrapping; }
        }

        public EscapeModeValue EscapeMode
        {
            get { return Setting.Current.EscapeMode; }
        }

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
            Setting.Current.ObserveProperty(x => x.Options)
                .Subscribe(_ => this.RaisePropertyChanged("Options"));
            Setting.Current.ObserveProperty(x => x.ShowLineNumbers)
                .Subscribe(_ => this.RaisePropertyChanged("ShowLineNumbers"));
            Setting.Current.ObserveProperty(x => x.TextWrapping)
                .Subscribe(_ => this.RaisePropertyChanged("TextWrapping"));

            this.CompiledText = this.Text
                .Throttle(TimeSpan.FromSeconds(1))
                .Select(s => s.IsEmpty() ? "" : s.Tokenize().Compile(this.EscapeMode))
                .ToReactiveProperty<string>("");

            this.Line = new ReactiveProperty<int>(0);
            this.Column = new ReactiveProperty<int>(0);

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
            this.CompiledText.Value = this.Text.Value.Tokenize().Compile(this.EscapeMode);
        }
    }
}
