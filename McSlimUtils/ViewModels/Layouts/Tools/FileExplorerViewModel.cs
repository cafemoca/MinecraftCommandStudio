using Cafemoca.McSlimUtils.ViewModels.Layouts.Bases;
using Codeplex.Reactive;
using Codeplex.Reactive.Extensions;
using FileListView.ViewModels;
using FileSystemModels.Events;
using Livet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafemoca.McSlimUtils.ViewModels.Layouts.Tools
{
    public class FileExplorerViewModel : ToolViewModel
    {
        public const string ToolContentId = "FileExplorer";

        public FolderListViewModel FolderListViewModel { get; private set; }
        public ReactiveCommand SyncPathWithCurrentDocumentCommand { get; private set; }

        public FileExplorerViewModel()
            : base("エクスプローラー")
        {
            this.ContentId.Value = ToolContentId;
            this.FolderListViewModel = new FolderListViewModel(this.FolderList_OnFileOpen);
        }

        private void FolderList_OnFileOpen(object sender, FileOpenEventArgs e)
        {
            var fileName = e.FileName;
            try
            {
                App.MainViewModel.Open(fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
