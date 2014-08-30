using Cafemoca.McCommandStudio.Models;
using Cafemoca.McCommandStudio.Services;
using Cafemoca.McCommandStudio.ViewModels.Flips;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Tools;
using Cafemoca.McCommandStudio.ViewModels.Parts;
using Codeplex.Reactive;
using Codeplex.Reactive.Extensions;
using Livet;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using TaskDialog = TaskDialogInterop;

namespace Cafemoca.McCommandStudio.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {
        public MainMenuViewModel MainMenuViewModel { get; private set; }

        public ReactiveProperty<FileViewModel> ActiveDocument { get; private set; }

        public ReactiveCollection<ToolViewModel> Tools { get; private set; }
        public ReactiveCollection<FileViewModel> Files { get; private set; }

        public StatusBarViewModel StatusBarViewModel { get; private set; }

        public SettingFlipViewModel SettingFlipViewModel { get; private set; }
        public ReactiveProperty<bool> IsSettingFlipOpen { get; private set; }
        public ReactiveCommand SettingCommand { get; private set; }

        public CompletionEditorViewModel CompletionEditorViewModel { get; set; }

        public ReactiveCommand NewCommand { get; private set; }
        public ReactiveCommand OpenCommand { get; private set; }
        public ReactiveCommand SaveCommand { get; private set; }
        public ReactiveCommand SaveAsCommand { get; private set; }
        public ReactiveCommand SaveAllCommand { get; private set; }
        public ReactiveCommand CloseCommand { get; private set; }

        private readonly StartPageViewModel startPageViewModel = new StartPageViewModel();
        private int documentCount = 0;

        public MainWindowViewModel()
        {
            this.MainMenuViewModel = new MainMenuViewModel();

            this.ActiveDocument = new ReactiveProperty<FileViewModel>();

            this.CompletionEditorViewModel = new CompletionEditorViewModel();

            this.Tools = new ReactiveCollection<ToolViewModel>();
            this.Tools.Add(this.CompletionEditorViewModel);
            //this.Tools.Add(new RecentFilesViewModel());
            //this.Tools.Add(new FileExplorerViewModel());

            this.Files = new ReactiveCollection<FileViewModel>();
            this.Files.Add(this.startPageViewModel);
            this.Files.CollectionChangedAsObservable().Subscribe(_ =>
                StatusService.Current.SetMain(this.Files.Count + " 個のドキュメント"));

            this.NewCommand = new ReactiveCommand();
            this.NewCommand.Subscribe(_ =>
            {
                var newFile = new DocumentViewModel(this.documentCount++);
                this.Files.Add(newFile);
                this.ActiveDocument.Value = newFile;
                this.CloseStartPage();
                StatusService.Current.Notify("新規ドキュメントを作成しました。");
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
                    var vm = this.Open(dialog.FileName);
                }
            });

            this.SaveCommand = (ReactiveCommand)this.ActiveDocument
                .Where(f => f != null)
                .Select(f => !(f is StartPageViewModel))
                .ToReactiveCommand(false);
            this.SaveCommand.Subscribe(_ =>
                this.Save(this.ActiveDocument.Value, false));

            this.SaveAsCommand = this.ActiveDocument
                .Where(f => f != null)
                .Select(f => !(f is StartPageViewModel))
                .ToReactiveCommand(false);
            this.SaveAsCommand.Subscribe(_ =>
                this.Save(this.ActiveDocument.Value, true));

            this.SaveAllCommand = this.ObserveProperty(x => x.Files)
                .Any()
                .ToReactiveCommand();
            this.SaveAllCommand.Subscribe(_ =>
                this.SaveAll());

            this.CloseCommand = this.ActiveDocument
                .Where(f => f != null)
                .Select(f => !(f is StartPageViewModel))
                .ToReactiveCommand(false);
            this.CloseCommand.Subscribe(_ =>
                this.Close(this.ActiveDocument.Value));

            this.StatusBarViewModel = new StatusBarViewModel();

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
            else if (!this.Files.Any())
            {
                this.Files.Add(this.startPageViewModel);
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
            this.CloseStartPage();

            this.ActiveDocument.Value = fileViewModel;
            StatusService.Current.Notify(fileViewModel.FilePath.Value + " を開きました。");

            return fileViewModel;
        }

        public void Save(FileViewModel fileToSave, bool saveAsFlag = false)
        {
            if (fileToSave is StartPageViewModel)
            {
                return;
            }
            if (fileToSave.FilePath.Value != null && !saveAsFlag && !fileToSave.IsModified.Value)
            {
                return;
            }
            if (fileToSave.FilePath.Value == null || saveAsFlag)
            {
                var dialog = new CommonSaveFileDialog();
                dialog.Filters.Add(new CommonFileDialogFilter("テキスト ファイル (*.txt)", "*.txt"));
                dialog.Filters.Add(new CommonFileDialogFilter("すべてのファイル (*.*)", "*.*"));
                dialog.DefaultFileName = fileToSave.FileName.Value;
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

            FileManager.SaveTextFile(fileToSave.FilePath.Value, fileToSave.Text.Value, fileToSave.Encoding.Value);
            StatusService.Current.Notify(fileToSave.FilePath.Value + "に保存しました。");
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
            this.CloseStartPage();
            if (this.ActiveDocument.Value == fileToClose)
            {
                this.ActiveDocument.Value = this.Files.FirstOrDefault();
            }
        }

        public void Close(ToolViewModel toolViewModel)
        {
            if (this.Tools.Any(x => x == toolViewModel))
            {
                this.Tools.Remove(toolViewModel);
            }
        }
    }
}
