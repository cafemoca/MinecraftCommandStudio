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
        }
    }
}
