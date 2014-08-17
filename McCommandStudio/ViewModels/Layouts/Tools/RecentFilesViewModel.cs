using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;

namespace Cafemoca.McCommandStudio.ViewModels.Layouts.Tools
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
