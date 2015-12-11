using Cafemoca.MinecraftCommandStudio.ViewModels.Layouts.Tools;
using Livet;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Parts
{
    public class MainMenuViewModel : ViewModel
    {
        public CompletionEditorViewModel CompletionEditor
        {
            get { return App.MainViewModel.CompletionEditorViewModel; }
            set { App.MainViewModel.CompletionEditorViewModel = value; }
        }
    }
}
