using System;
using System.Globalization;
using System.Windows.Data;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;

namespace Cafemoca.McCommandStudio.Internals.Converters
{
    public class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FileViewModel ||
                value is DocumentViewModel)
            {
                return value;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FileViewModel ||
                value is DocumentViewModel)
            {
                return value;
            }
            return Binding.DoNothing;
        }
    }
}
