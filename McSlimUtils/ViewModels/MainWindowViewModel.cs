using Cafemoca.McSlimUtils.Models;
using Cafemoca.McSlimUtils.ViewModels.Flips;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Bases;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Documents;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Tools;
using Codeplex.Reactive;
using Codeplex.Reactive.Extensions;
using Livet;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskDialog = TaskDialogInterop;

namespace Cafemoca.McSlimUtils.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {
        public ReactiveProperty<FileViewModel> ActiveDocument { get; private set; }

        public ReactiveCollection<ToolViewModel> Tools { get; private set; }
        public ReactiveCollection<FileViewModel> Files { get; private set; }

        public SettingFlipViewModel SettingFlipViewModel { get; private set; }
        public ReactiveProperty<bool> IsSettingFlipOpen { get; private set; }
        public ReactiveCommand SettingCommand { get; private set; }

        public ReactiveCommand NewCommand { get; private set; }
        public ReactiveCommand OpenCommand { get; private set; }
        public ReactiveCommand SaveCommand { get; private set; }
        public ReactiveCommand SaveAsCommand { get; private set; }
        public ReactiveCommand SaveAllCommand { get; private set; }
        public ReactiveCommand CloseCommand { get; private set; }

        private readonly StartPageViewModel startPageViewModel = new StartPageViewModel();

        public MainWindowViewModel()
        {
            this.ActiveDocument = new ReactiveProperty<FileViewModel>();

            this.Tools = new ReactiveCollection<ToolViewModel>();
            //this.Tools.Add(new RecentFilesViewModel());
            this.Tools.Add(new FileExplorerViewModel());

            this.Files = new ReactiveCollection<FileViewModel>();
            this.Files.Add(startPageViewModel);

            this.NewCommand = new ReactiveCommand();
            this.NewCommand.Subscribe(_ =>
            {
                var newFile = new DocumentViewModel();
                this.Files.Add(newFile);
                this.ActiveDocument.Value = newFile;
                this.CloseStartPage();
            });

            this.OpenCommand = new ReactiveCommand();
            this.OpenCommand.Subscribe(_ =>
            {
                var dialog = new CommonOpenFileDialog();
                dialog.Filters.Add(new CommonFileDialogFilter("テキスト ファイル (*.txt)", "*.txt"));
                dialog.Filters.Add(new CommonFileDialogFilter("すべてのファイル (*.*)", "*.*"));
                dialog.EnsurePathExists = true;
                dialog.IsFolderPicker = false;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var fileViewModel = this.Open(dialog.FileName);
                }
            });

            this.SaveCommand = this.ActiveDocument
                .Select(f => f != null ? f.IsModified.Value : false)
                .ToReactiveCommand(false);
            this.SaveCommand.Subscribe(_ =>
                this.Save(this.ActiveDocument.Value, false));

            this.SaveAsCommand = this.ActiveDocument
                .Select(f => f != null && !(f is StartPageViewModel))
                .ToReactiveCommand(false);
            this.SaveAsCommand.Subscribe(_ =>
                this.Save(this.ActiveDocument.Value, true));

            this.SaveAllCommand = this.ObserveProperty(x => x.Files)
                .Any()
                .ToReactiveCommand();
            this.SaveAllCommand.Subscribe(_ =>
            {
                this.SaveAll();
            });

            this.CloseCommand = this.ActiveDocument
                .Select(f => f != null)
                .ToReactiveCommand(false);
            this.CloseCommand.Subscribe(_ =>
                this.Close(this.ActiveDocument.Value));

            this.SettingFlipViewModel = new SettingFlipViewModel();
            this.IsSettingFlipOpen = new ReactiveProperty<bool>(false);
            this.SettingCommand = new ReactiveCommand();
            this.SettingCommand.Subscribe(_ =>
                this.IsSettingFlipOpen.Value = !this.IsSettingFlipOpen.Value);


            this.ExitCommand = new ReactiveCommand();
            this.ExitCommand.Subscribe(_ =>
                this.WindowClose = true);
        }

        private void CloseStartPage()
        {
            if (this.Files.Contains(this.startPageViewModel))
            {
                this.Files.Remove(this.startPageViewModel);
            }
        }

        public FileViewModel Open(string filePath)
        {
            var fileViewModel = this.Files.FirstOrDefault(fm => fm.FilePath.Value == filePath);
            if (fileViewModel != null)
            {
                return fileViewModel;
            }
            var supportExt = new[] { ".txt" };
            if (!supportExt.Contains(Path.GetExtension(filePath)))
            {
                return null;
            }

            fileViewModel = new DocumentViewModel(filePath);
            this.Files.Add(fileViewModel);

            this.ActiveDocument.Value = fileViewModel;
            this.CloseStartPage();

            return fileViewModel;
        }

        public void Save(FileViewModel fileToSave, bool saveAsFlag = false)
        {
            if (fileToSave.FilePath.Value == null || saveAsFlag)
            {
                var dialog = new CommonSaveFileDialog();
                dialog.Filters.Add(new CommonFileDialogFilter("テキスト ファイル (*.txt)", "*.txt"));
                dialog.Filters.Add(new CommonFileDialogFilter("すべてのファイル (*.*)", "*.*"));
                dialog.DefaultExtension = "txt";

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    fileToSave.FilePath.Value = dialog.FileName;
                }
                else
                {
                    return;
                }
            }

            FileManager.SaveTextFile(fileToSave.FilePath.Value, fileToSave.Text.Value);
            this.ActiveDocument.Value.IsModified.Value = false;
        }

        public void SaveAll()
        {
            if (this.Files != null)
            {
                this.Files.Where(x => x.IsModified.Value).ForEach(x => x.SaveCommand.Execute());
            }
        }

        public void Close(FileViewModel fileToClose)
        {
            if (fileToClose.IsModified.Value)
            {
                var dialog = new TaskDialog.TaskDialogOptions();
                dialog.Owner = App.MainView;
                dialog.Title = "保存の確認";
                dialog.MainInstruction = "ドキュメントを保存しますか？";
                dialog.Content = "保存しない場合、現在の変更は失われます";
                dialog.CustomButtons = new[] { "保存 (&S)", "保存しない (&N)", "キャンセル (&C)" };

                var result = TaskDialog.TaskDialog.Show(dialog);
                switch (result.CustomButtonResult)
                {
                    case 0:
                        this.Save(fileToClose);
                        if (fileToClose.IsModified.Value)
                        {
                            return;
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        return;
                }
            }

            this.Files.Remove(fileToClose);
            if (this.ActiveDocument.Value == fileToClose)
            {
                this.ActiveDocument.Value = this.Files.FirstOrDefault();
            }
        }
    }
}
