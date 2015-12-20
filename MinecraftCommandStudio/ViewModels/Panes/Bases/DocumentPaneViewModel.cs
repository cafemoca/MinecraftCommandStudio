using System;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Media;
using Cafemoca.MinecraftCommandStudio.Models;
using Cafemoca.MinecraftCommandStudio.Settings;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Bases
{
    public class DocumentPaneViewModel : PaneViewModel
    {
        private static ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
        private static readonly string documentIcon = @"pack://application:,,/Resources/document.png";

        public new ReactiveProperty<string> Title { get; private set; }

        public virtual ReactiveProperty<string> FilePath { get; private set; }
        public virtual ReactiveProperty<string> FileName { get; private set; }
        public virtual ReactiveProperty<string> Text { get; private set; }
        public virtual ReactiveProperty<bool> IsModified { get; private set; }
        public virtual ReactiveProperty<Encoding> Encoding { get; private set; }

        public virtual ReactiveCommand SaveCommand { get; private set; }
        public virtual ReactiveCommand SaveAsCommand { get; private set; }
        public virtual ReactiveCommand CloseCommand { get; private set; }

        public virtual string DefaultFileName
        {
            get { return Setting.Current.DefaultFileName; }
        }

        public DocumentPaneViewModel()
            : this(null, -1)
        {
        }

        public DocumentPaneViewModel(int count)
            : this(null, count)
        {
        }

        public DocumentPaneViewModel(string filePath)
            : this(filePath, -1)
        {
        }

        public DocumentPaneViewModel(string filePath, int count)
            : base()
        {
            this.Title = new ReactiveProperty<string>();

            var textFile = FileManager.LoadTextFile(filePath);

            this.FilePath = new ReactiveProperty<string>(textFile.FilePath);
            this.Text = new ReactiveProperty<string>(textFile.Text ?? string.Empty);
            this.Encoding = new ReactiveProperty<Encoding>(textFile.Encoding);

            this.IsModified = new ReactiveProperty<bool>(false);
            this.FileName = this.FilePath
                .Select(p => (p == null)
                    ? this.DefaultFileName + ((count >= 0) ? " " + count : string.Empty) + ".txt"
                    : Path.GetFileName(p))
                .ToReactiveProperty();

            this.Text.Pairwise()
                .Subscribe(x => this.IsModified.Value = true);

            this.IsModified.Subscribe(m => this.Title.Value = this.FileName.Value + (m ? " *" : ""));

            this.IconSource.Value = imageSourceConverter.ConvertFromInvariantString(documentIcon) as ImageSource;

            this.SaveCommand = this.IsModified.ToReactiveCommand();
            this.SaveCommand.Subscribe(_ => App.MainViewModel.Save(this, false));

            this.SaveAsCommand = new ReactiveCommand();
            this.SaveAsCommand.Subscribe(_ => App.MainViewModel.Save(this, true));

            this.CloseCommand = new ReactiveCommand();
            this.CloseCommand.Subscribe(_ => App.MainViewModel.Close(this));
        }
    }
}
