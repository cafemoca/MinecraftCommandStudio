using Cafemoca.McSlimUtils.ViewModels.Layouts.Bases;
using Cafemoca.McSlimUtils.ViewModels.Layouts.Documents;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Cafemoca.McSlimUtils.Internals.Converters
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
