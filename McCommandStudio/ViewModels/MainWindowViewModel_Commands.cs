using Cafemoca.McCommandStudio.Internals.Utils.Commands;
using Cafemoca.McCommandStudio.Models;
using Cafemoca.McCommandStudio.Services;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Livet;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using TaskDialog = TaskDialogInterop;

namespace Cafemoca.McCommandStudio.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {
        public ReactiveCommand NewCommand { get; private set; }
        public ReactiveCommand OpenCommand { get; private set; }
        public ReactiveCommand SaveCommand { get; private set; }
        public ReactiveCommand SaveAsCommand { get; private set; }
        public ReactiveCommand SaveAllCommand { get; private set; }
        public ReactiveCommand CloseCommand { get; private set; }

        public void InitializeCommandBinding(Window window)
        {
            #region Exit
            window.CommandBindings.Add(new CommandBinding(AppCommand.Exit,
                (s, e) => this.ExitCommand.Execute(),
                (s, e) => e.CanExecute = this.ExitCommand.CanExecute()));
            #endregion

            #region New
            window.CommandBindings.Add(new CommandBinding(AppCommand.New,
                (s, e) => this.NewCommand.Execute(),
                (s, e) => e.CanExecute = this.NewCommand.CanExecute()));
            #endregion

            #region Open
            window.CommandBindings.Add(new CommandBinding(AppCommand.Open,
                (s, e) => this.OpenCommand.Execute(),
                (s, e) => e.CanExecute = this.OpenCommand.CanExecute()));
            #endregion

            #region Save
            window.CommandBindings.Add(new CommandBinding(AppCommand.Save,
                (s, e) => this.SaveCommand.Execute(),
                (s, e) => e.CanExecute = this.SaveCommand.CanExecute()));
            #endregion

            #region SaveAs
            window.CommandBindings.Add(new CommandBinding(AppCommand.SaveAs,
                (s, e) => this.SaveAsCommand.Execute(),
                (s, e) => e.CanExecute = this.SaveAsCommand.CanExecute()));
            #endregion

            #region SaveAll
            window.CommandBindings.Add(new CommandBinding(AppCommand.SaveAll,
                (s, e) => this.SaveAllCommand.Execute(),
                (s, e) => e.CanExecute = this.SaveAllCommand.CanExecute()));
            #endregion

            #region LoadFile
            window.CommandBindings.Add(new CommandBinding(AppCommand.LoadFile, (s, e) =>
            {
                try
                {
                    if (e == null) return;
                    var fileName = e.Parameter as string;
                    if (fileName.IsEmpty()) return;
                    this.Open(fileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }));
            #endregion

            #region CloseFile
            window.CommandBindings.Add(new CommandBinding(AppCommand.CloseFile,
                (s, e) => this.CloseCommand.Execute(),
                (s, e) => e.CanExecute = this.CloseCommand.CanExecute()));
            #endregion

            #region OpenSettings
            window.CommandBindings.Add(new CommandBinding(AppCommand.OpenSettings, (s, e) =>
                this.IsSettingFlipOpen.Value = !this.IsSettingFlipOpen.Value));
            #endregion
        }

        public void InitializeDockCommands()
        {
            this.NewCommand = new ReactiveCommand();
            this.NewCommand.Subscribe(_ =>
            {
                var newFile = new DocumentViewModel(_documentCount++);
                this.Files.Add(newFile);
                this.ActiveDocument.Value = newFile;
                this.ToggleStartPage();
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
            this.ToggleStartPage();

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
            this.ToggleStartPage();
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
