using System.Windows;
using System.Windows.Controls;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Anchorables;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Documents;

namespace Cafemoca.MinecraftCommandStudio.Views.Panes.Layouts
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StartPageTemplate { get; set; }
        public DataTemplate DocumentTemplate { get; set; }
        public DataTemplate CompletionEditorTemplate { get; set; }

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
            if (item is KeywordSettingViewModel)
            {
                return this.CompletionEditorTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
