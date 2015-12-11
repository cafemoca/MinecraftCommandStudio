using Cafemoca.MinecraftCommandStudio.ViewModels.Layouts.Bases;

namespace Cafemoca.MinecraftCommandStudio.ViewModels.Layouts.Tools
{
    public class RecentFilesViewModel : ToolViewModel
    {
        public const string ToolContentId = "RecentFiles";

        public RecentFilesViewModel()
            : base("最近使用したファイル")
        {
            this.ContentId.Value = ToolContentId;
        }
    }
}
