using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MapsSample.WindowsPhone8.Converters
{
    /// <summary>
    ///     The push pin content converter.
    /// </summary>
    public class ClusterPushPinFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as string;

            switch (param)
            {
                case "FontSize":
                    return this.GetFontSize(value);
                case "Height":
                    return this.GetPushPinDiameter(value);
            }

            return DependencyProperty.UnsetValue;
        }

        private int GetFontSize(object value)
        {
            int clusterSize;
            if (value != null && int.TryParse(value.ToString(), out clusterSize))
            {
                // For 4-digit numbers we use font size 13
                if (clusterSize >= 1000)
                {
                    return 13;
                }

                // For 3-digit numbers we use font size 16
                if (clusterSize >= 100)
                {
                    return 16;
                }
            }

            // For 1- and 2-digit numbers we use font size 18
            return 18;
        }

        private int GetPushPinDiameter(object value)
        {
            int clusterSize;
            if (value != null && int.TryParse(value.ToString(), out clusterSize))
            {
                if (clusterSize > 1000)
                {
                    return 304;
                }

                if (clusterSize > 100)
                {
                    return 152;
                }

                if (clusterSize > 10)
                {
                    return 76;
                }
            }

            return 38;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}