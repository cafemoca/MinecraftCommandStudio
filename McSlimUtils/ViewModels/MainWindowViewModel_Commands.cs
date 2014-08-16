using Cafemoca.McSlimUtils.Internals.Utils.Commands;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Bases;
using Livet;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cafemoca.McSlimUtils.ViewModels
{
    public partial class MainWindowViewModel : ViewModel
    {
        public void InitializeCommandBinding(Window window)
        {
            #region Exit
            window.CommandBindings.Add(new CommandBinding(AppCommand.Exit,
                (s, e) => this.ExitCommand.Execute()));
            #endregion

            #region Open
            window.CommandBindings.Add(new CommandBinding(AppCommand.Open,
                (s, e) => this.OpenCommand.Execute()));
            #endregion

            #region Save
            window.CommandBindings.Add(new CommandBinding(AppCommand.Save,
                (s, e) => this.SaveCommand.Execute(),
                (s, e) => this.SaveCommand.CanExecute()));
            #endregion

            #region SaveAs
            window.CommandBindings.Add(new CommandBinding(AppCommand.SaveAs,
                (s, e) => this.SaveAsCommand.Execute(),
                (s, e) => this.SaveAsCommand.CanExecute()));
            #endregion

            #region SaveAll
            window.CommandBindings.Add(new CommandBinding(AppCommand.SaveAll,
                (s, e) => this.SaveAll()));
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
            window.CommandBindings.Add(new CommandBinding(AppCommand.CloseFile, (s, e) =>
            {
                try
                {
                    var vm = null as FileViewModel;
                    if (e != null)
                    {
                        e.Handled = true;
                        vm = e.Parameter as FileViewModel;
                    }
                    if (vm != null)
                    {
                        this.Close(vm);
                    }
                    else if (this.ActiveDocument.Value != null)
                    {
                        this.Close(this.ActiveDocument.Value);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }));
            #endregion
        }
    }
}
