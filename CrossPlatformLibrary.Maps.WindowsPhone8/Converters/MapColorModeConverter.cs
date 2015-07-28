using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CrossPlatformLibrary.Maps.Converters
{
    public class MapColorModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType().IsEnum == false)
            {
                return DependencyProperty.UnsetValue;
            }

            switch ((MapColorMode)value)
            {
                case MapColorMode.Dark:
                    return Microsoft.Phone.Maps.Controls.MapColorMode.Dark;
                case MapColorMode.Light:
                    return Microsoft.Phone.Maps.Controls.MapColorMode.Light;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType().IsEnum == false)
            {
                return DependencyProperty.UnsetValue;
            }

            switch ((Microsoft.Phone.Maps.Controls.MapColorMode)value)
            {
                case Microsoft.Phone.Maps.Controls.MapColorMode.Dark:
                    return MapColorMode.Dark;
                case Microsoft.Phone.Maps.Controls.MapColorMode.Light:
                    return MapColorMode.Light;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }
    }
}