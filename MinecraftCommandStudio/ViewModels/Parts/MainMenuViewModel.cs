using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Anchorables;
using Livet;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Parts
{
    public class MainMenuViewModel : ViewModel
    {
        public KeywordSettingViewModel CompletionEditor
        {
            get { return App.MainViewModel.CompletionEditorViewModel; }
            set { App.MainViewModel.CompletionEditorViewModel = value; }
        }
    }
}
