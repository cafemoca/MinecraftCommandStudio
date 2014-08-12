using Cafemoca.CommandEditor;
using Cafemoca.CommandEditor.Utils;
using Cafemoca.McSlimUtils.Settings;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Bases;
using Codeplex.Reactive;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

namespace Cafemoca.McSlimUtils.ViewModels.Layouts.Documents
{
    public class DocumentViewModel : FileViewModel
    {
        public ReactiveProperty<string> CompiledText { get; private set; }
        public ReactiveProperty<int> Line { get; private set; }
        public ReactiveProperty<int> Column { get; private set; }

        public EscapeModeValue EscapeMode
        {
            get { return Setting.Current.EscapeMode; }
        }

        public QuoteModeValue QuoteMode
        {
            get { return Setting.Current.QuoteMode; }
        }

        public ReactiveCommand CopyCommand { get; private set; }

        public DocumentViewModel()
            : this(null)
        {
        }

        public DocumentViewModel(string filePath)
            : base(filePath)
        {
            this.CompiledText = this.Text
                .Throttle(TimeSpan.FromSeconds(1))
                .Select(s => s.IsEmpty() ? "" : s.Tokenize().Compile(this.EscapeMode, this.QuoteMode))
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
            this.CompiledText.Value = this.Text.Value.Tokenize().Compile(this.EscapeMode, this.QuoteMode);
        }
    }
}
