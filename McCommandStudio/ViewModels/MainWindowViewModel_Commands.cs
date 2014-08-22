using Cafemoca.McCommandStudio.Internals.Utils.Commands;
using Livet;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cafemoca.McCommandStudio.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {
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
    }
}
