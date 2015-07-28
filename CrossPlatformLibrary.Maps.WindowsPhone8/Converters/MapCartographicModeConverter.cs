using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CrossPlatformLibrary.Maps.Converters
{
    public class MapCartographicModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType().IsEnum == false)
            {
                return DependencyProperty.UnsetValue;
            }

            switch ((MapCartographicMode)value)
            {
                case MapCartographicMode.Aerial:
                    return Microsoft.Phone.Maps.Controls.MapCartographicMode.Aerial;
                case MapCartographicMode.Hybrid:
                    return Microsoft.Phone.Maps.Controls.MapCartographicMode.Hybrid;
                case MapCartographicMode.Road:
                    return Microsoft.Phone.Maps.Controls.MapCartographicMode.Road;
                case MapCartographicMode.Terrain:
                    return Microsoft.Phone.Maps.Controls.MapCartographicMode.Terrain;
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

            switch ((Microsoft.Phone.Maps.Controls.MapCartographicMode)value)
            {
                case Microsoft.Phone.Maps.Controls.MapCartographicMode.Aerial:
                    return MapCartographicMode.Aerial;
                case Microsoft.Phone.Maps.Controls.MapCartographicMode.Hybrid:
                    return MapCartographicMode.Hybrid;
                case Microsoft.Phone.Maps.Controls.MapCartographicMode.Road:
                    return MapCartographicMode.Road;
                case Microsoft.Phone.Maps.Controls.MapCartographicMode.Terrain:
                    return MapCartographicMode.Terrain;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }
    }
}