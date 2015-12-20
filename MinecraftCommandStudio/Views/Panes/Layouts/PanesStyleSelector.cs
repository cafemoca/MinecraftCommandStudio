using System.Windows;
using System.Windows.Controls;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Bases;

namespace Cafemoca.MinecraftCommandStudio.Views.Panes.Layouts
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style AnchorableStyle { get; set; }
        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is DocumentPaneViewModel)
            {
                return this.DocumentStyle;
            }
            if (item is AnchorablePaneViewModel)
            {
                return this.AnchorableStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
