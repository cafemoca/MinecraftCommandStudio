using Cafemoca.McSlimUtils.Models;
using Cafemoca.McSlimUtils.ViewModels.Flips;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Bases;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Documents;
using Codeplex.Reactive;
using Livet;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cafemoca.McSlimUtils.ViewModels
{
    public class MainWindowViewModel : ViewModel
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
        public ReactiveCommand CloseCommand { get; private set; }
        public ReactiveCommand ExitCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.ActiveDocument = new ReactiveProperty<FileViewModel>();

            this.Tools = new ReactiveCollection<ToolViewModel>();
            //this.Tools.Add(new RecentFilesViewModel());

            this.Files = new ReactiveCollection<FileViewModel>();
            //this.Files.Add(new DocumentViewModel());
            this.Files.Add(new StartPageViewModel());

            this.NewCommand = new ReactiveCommand();
            this.NewCommand.Subscribe(_ =>
            {
                var newFile = new DocumentViewModel();
                this.Files.Add(newFile);
                this.ActiveDocument.Value = newFile;
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
                    this.ActiveDocument.Value = fileViewModel;
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

            this.CloseCommand = this.ActiveDocument
                .Select(f => f != null)
                .ToReactiveCommand(false);
            this.CloseCommand.Subscribe(async _ =>
                await this.CloseAsync(this.ActiveDocument.Value));

            this.ExitCommand = new ReactiveCommand();
            this.ExitCommand.Subscribe(_ =>
                this.Exit());

            this.SettingFlipViewModel = new SettingFlipViewModel();
            this.IsSettingFlipOpen = new ReactiveProperty<bool>(false);
            this.SettingCommand = new ReactiveCommand();
            this.SettingCommand.Subscribe(_ =>
                this.IsSettingFlipOpen.Value = !this.IsSettingFlipOpen.Value);
        }

        public FileViewModel Open(string filePath)
        {
            var fileViewModel = this.Files.FirstOrDefault(fm => fm.FilePath.Value == filePath);
            if (fileViewModel != null)
            {
                return fileViewModel;
            }

            fileViewModel = new DocumentViewModel(filePath);
            this.Files.Add(fileViewModel);
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

        public async Task CloseAsync(FileViewModel fileToClose)
        {
            if (fileToClose.IsModified.Value)
            {
                var result = MessageBox.Show("ドキュメントを閉じる前に " + fileToClose.FileName.Value + " を保存しますか？",
                                             "保存", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                if (result == MessageBoxResult.Yes)
                {
                    this.Save(fileToClose);
                }
                /*
                var result = await DialogService.ShowMessageAsync(
                    "保存",
                    "ドキュメントを閉じる前に " + fileToClose.FileName.Value + " を保存しますか？",
                    MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary);

                if (result == MessageDialogResult.Affirmative)
                {
                    this.Save(fileToClose);
                }
                if (result == MessageDialogResult.Negative)
                {
                    return;
                }
                */
            }

            this.Files.Remove(fileToClose);
            if (this.ActiveDocument.Value == fileToClose)
            {
                this.ActiveDocument.Value = this.Files.FirstOrDefault();
            }
        }

        public void Exit()
        {
        }
    }
}
