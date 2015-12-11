using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Cafemoca.MinecraftCommandStudio.Internals.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = value == null;

            return string.Equals((parameter as string), "inverse", StringComparison.OrdinalIgnoreCase)
                ? (flag ? Visibility.Collapsed : Visibility.Visible)
                : (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
