using Cafemoca.McSlimUtils.ViewModels.Layouts.Documents;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Tools;
using System.Windows;
using System.Windows.Controls;

namespace Cafemoca.McSlimUtils.Views.Panes
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public PanesTemplateSelector()
        {
        }

        public DataTemplate StartPageTemplate { get; set; }
        public DataTemplate DocumentTemplate { get; set; }
        public DataTemplate RecentFilesTemplate { get; set; }
        public DataTemplate FileExplorerTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is StartPageViewModel)
            {
                return this.StartPageTemplate;
            }
            if (item is DocumentViewModel)
            {
                return this.DocumentTemplate;
            }
            if (item is RecentFilesViewModel)
            {
                return this.RecentFilesTemplate;
            }
            if (item is FileExplorerViewModel)
            {
                return this.FileExplorerTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
