using Cafemoca.McSlimUtils.Models;
using Codeplex.Reactive;
using Codeplex.Reactive.Extensions;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Media;

namespace Cafemoca.McSlimUtils.ViewModels.Layouts.Bases
{
    public class FileViewModel : PaneViewModel
    {
        private static ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
        private static readonly string documentIcon = @"pack://application:,,/Resources/document.png";

        public new ReactiveProperty<string> Title { get; private set; }

        public virtual ReactiveProperty<string> FilePath { get; private set; }
        public virtual ReactiveProperty<string> FileName { get; private set; }
        public virtual ReactiveProperty<string> Text { get; private set; }
        public virtual ReactiveProperty<bool> IsModified { get; private set; }

        public virtual ReactiveCommand SaveCommand { get; private set; }
        public virtual ReactiveCommand SaveAsCommand { get; private set; }
        public virtual ReactiveCommand CloseCommand { get; private set; }

        public FileViewModel()
            : this(null)
        {
        }

        public FileViewModel(string filePath)
            : base()
        {
            this.Title = new ReactiveProperty<string>();

            this.FilePath = new ReactiveProperty<string>(filePath);
            this.Text = new ReactiveProperty<string>((filePath != null)
                ? FileManager.LoadTextFile(filePath)
                : string.Empty);

            this.IsModified = new ReactiveProperty<bool>(false);
            this.FileName = this.FilePath
                .Select(p => (p == null) ? "untitled" : Path.GetFileName(p))
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
