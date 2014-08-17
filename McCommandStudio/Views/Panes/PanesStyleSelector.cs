using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using System.Windows;
using System.Windows.Controls;

namespace Cafemoca.McCommandStudio.Views.Panes
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }
        public Style FileStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is FileViewModel)
            {
                return this.FileStyle;
            }
            if (item is ToolViewModel)
            {
                return this.ToolStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
