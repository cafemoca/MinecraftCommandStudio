using Cafemoca.McSlimUtils.ViewModels.Layouts.Bases;
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

        public DataTemplate FileViewTemplate { get; set; }
        public DataTemplate ToolViewTemplate { get; set; }
        public DataTemplate DocumentTemplate { get; set; }
        public DataTemplate RecentFilesTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            /*
            if (item is FileViewModel)
            {
                return this.FileViewTemplate;
            }
            if (item is ToolViewModel)
            {
                return this.ToolViewTemplate;
            }
            */
            if (item is DocumentViewModel)
            {
                return this.DocumentTemplate;
            }
            if (item is RecentFilesViewModel)
            {
                return this.RecentFilesTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
