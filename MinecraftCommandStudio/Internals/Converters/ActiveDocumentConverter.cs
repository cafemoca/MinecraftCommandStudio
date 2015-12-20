using System;
using System.Globalization;
using System.Windows.Data;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Bases;
using Cafemoca.MinecraftCommandStudio.ViewModels.Panes.Documents;

namespace Cafemoca.MinecraftCommandStudio.Internals.Converters
{
    public class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DocumentPaneViewModel ||
                value is DocumentViewModel)
            {
                return value;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DocumentPaneViewModel ||
                value is DocumentViewModel)
            {
                return value;
            }
            return Binding.DoNothing;
        }
    }
}
